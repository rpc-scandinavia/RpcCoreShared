namespace RpcScandinavia.Core.Shared;
using System;
using System.Collections.Generic;
using System.Text;

#region RpcKeyValueSerializerConverter
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerConverter.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// A RPC Key/Value serializer converter, serializes and deserializes a object of a specifig type, to and from a string.
/// </summary>
public abstract class RpcKeyValueSerializerConverter {

	#region Static methods
	//------------------------------------------------------------------------------------------------------------------
	// Static methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets a converter, that can can serialize and deserialize the specified type.
	/// </summary>
	/// <param name="type">The type.</param>
	/// <param name="options">The serialization options.</param>
	/// <returns>Returns the handler or null none exist.</returns>
	public static RpcKeyValueSerializerConverter GetConverter(Type type, RpcKeyValueSerializerOptions options) {
		foreach (RpcKeyValueSerializerConverter converter in options.Converters) {
			if (converter.CanConvert(type) == true) {
				return converter;
			}
		}

		// No converter.
		return null;
	} // GetConverter
	#endregion

	#region Abstract methods
	//------------------------------------------------------------------------------------------------------------------
	// Abstract methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets a value indicating whether or not this converter can serialize and deserialize the specified type.
	/// </summary>
	/// <param name="type">The type.</param>
	/// <returns>Returns true if the type can be serialized and deserialized by this converter, false if not.</returns>
	public abstract Boolean CanConvert(Type type);

	/// <summary>
	/// Serializes a object of the type this converter can serialize to a string.
	/// </summary>
	/// <param name="obj">The value object.</param>
	/// <param name="options">The serializer options.</param>
	/// <returns>The object serialized to a string.</returns>
	internal abstract String InternalSerialize(Object obj, RpcKeyValueSerializerOptions options);

	/// <summary>
	/// Deserializes a string to a object of the type this converter can deserialize to.
	/// </summary>
	/// <param name="value">The string.</param>
	/// <param name="type">The preferred type.</param>
	/// <param name="options">The serializer options.</param>
	/// <returns>The string deserialized to a value object.</returns>
	internal abstract Object InternalDeserialize(String value, Type type, RpcKeyValueSerializerOptions options);
	#endregion

} // RpcKeyValueSerializerConverter
#endregion

#region RpcKeyValueSerializerConverter<T>
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerConverter<T>.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// A RPC Key/Value serializer converter, serializes and deserializes a object of a specifig type, to and from a string.
///
/// Inherit this class and override the "Serialize" and "Deserialize" methods, to serialize and deserialize the type
/// to and from a string value.
///
/// This class has build in tool for serializing and deserializing multiple strings.
/// To serialize:
/// 	base.ClearStrings();
/// 	...
/// 	base.AppendString("string one");
/// 	base.AppendString("string two");
/// 	base.AppendString("string n");
/// 	...
/// 	return base.GetStringForSerialize();
///
/// To deserialize:
/// 	base.ClearStrings(value);
/// 	String str = base.GetStringNextDeserialize();
/// 	...
/// Or (get five strings or null in each loop):
/// 	base.ClearStrings(value);
/// 	String[] strings = base.GetStringNextDeserialize(5);
/// 	...
/// </summary>
/// <typeparam name="T">The object type.</typeparam>
public abstract class RpcKeyValueSerializerConverter<T> : RpcKeyValueSerializerConverter {
	private StringBuilder serializeStringBuilder;
	private String deserializeString;
	private Int32 indexStart;
	private Int32 indexSpace;

	#region Overridden methods
	//------------------------------------------------------------------------------------------------------------------
	// Overridden methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <inheritdoc />
	public override Boolean CanConvert(Type type) {
		return (typeof(T).Equals(type) == true);
	} // CanConvert

	/// <inheritdoc />
	internal override String InternalSerialize(Object obj, RpcKeyValueSerializerOptions options) {
		return this.Serialize((T)obj, options);
	} // InternalSerialize

	/// <inheritdoc />
	internal override Object InternalDeserialize(String value, Type type, RpcKeyValueSerializerOptions options) {
		return this.Deserialize(value, type, options);
	} // InternalDeserialize
	#endregion

	#region Abstract methods
	//------------------------------------------------------------------------------------------------------------------
	// Abstract methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Serialize (convert) the T object into a string value.
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <param name="options">The serializer options.</param>
	/// <returns>The object as a string value.</returns>
	public abstract String Serialize(T obj, RpcKeyValueSerializerOptions options);

	/// <summary>
	/// Deserialize (parse) the string value into a T object.
	/// </summary>
	/// <param name="value">The string value.</param>
	/// <param name="type">The preferred type.</param>
	/// <param name="options">The serializer options.</param>
	/// <returns>The object.</returns>
	public abstract T Deserialize(String value, Type type, RpcKeyValueSerializerOptions options);
	#endregion

	#region Multiple strings helper methods
	//------------------------------------------------------------------------------------------------------------------
	// Multiple strings helper methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Clear the multiple strings builder.
	/// Clear the multiple strings iterator.
	/// </summary>
	protected void ClearStrings(String deserializeString = null) {
		this.serializeStringBuilder = new StringBuilder();
		this.deserializeString = deserializeString ?? "";
		this.indexStart = 0;
		this.indexSpace = this.deserializeString.IndexOf(' ', this.indexStart);
	} // ClearStrings

	/// <summary>
	/// Append the string to the multiple strings builder.
	/// </summary>
	/// <param name="str">The string.</param>
	protected void AppendString(String str) {
		// Append the string like this: <length><space><string><newline>
		if (str != null) {
			this.serializeStringBuilder.AppendDelim($"{str.Length} {str}", "\n");
		} else {
			this.serializeStringBuilder.AppendDelim($"0 ", "\n");
		}
	} // AppendString

	/// <summary>
	/// Get all the appended strings, from the multiple strings builder, as one string.
	/// Ready and safe to serialize.
	/// </summary>
	/// <returns></returns>
	protected String GetStringForSerialize() {
		return this.serializeStringBuilder.ToString();
	} // GetStringForSerialize

	/// <summary>
	/// Gets the next string, from the multiple strings iterator.
	/// </summary>
	/// <returns>The next deserialized string, or null.</returns>
	protected String GetStringNextDeserialize() {
		if ((this.indexStart > -1) && (this.indexSpace > this.indexStart)) {
			// Get the string.
			Int32 length = this.deserializeString.Substring(this.indexStart, this.indexSpace - this.indexStart).ToInt32(0);
			String str = this.deserializeString.Substring(this.indexSpace + 1, length);									// +1 is the " " (space).

			// Iterate.
			this.indexStart = this.indexSpace + length + 1;																// +1 is the "\n" (newline).
			this.indexSpace = this.deserializeString.IndexOf(' ', this.indexStart);

			return str;
		} else {
			return null;
		}
	} // GetStringNextDeserialize

	/// <summary>
	/// Gets the specified number of next strings, from the multiple strings iterator.
	/// </summary>
	/// <returns>The specified number of next deserialized string, or null.</returns>
	protected String[] GetStringNextDeserialize(Int32 count) {
		String[] result = new String[count];
		for (Int32 index = 0; index < count; index++) {
			result[index] = this.GetStringNextDeserialize();

			// Return null, when the specified number of strings are not available.
			if (result[index] == null) {
				return null;
			}
		}
		return result;
	} // GetStringNextDeserialize
	#endregion

} // RpcKeyValueSerializerConverter
#endregion
