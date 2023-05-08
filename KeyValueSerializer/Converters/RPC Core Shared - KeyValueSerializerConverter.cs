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
	/// Serializes a object of the type this converter can serialize to a <see cref="System.Memory{System.Char}" />.
	/// </summary>
	/// <param name="obj">The value object.</param>
	/// <param name="options">The serializer options.</param>
	/// <returns>The object serialized to a memory.</returns>
	internal abstract ReadOnlyMemory<Char> InternalSerialize(Object obj, RpcKeyValueSerializerOptions options);

	/// <summary>
	/// Deserializes a <see cref="System.Memory{System.Char}" /> to a object of the type this converter can deserialize to.
	/// </summary>
	/// <param name="value">The serialized memory.</param>
	/// <param name="type">The preferred type.</param>
	/// <param name="options">The serializer options.</param>
	/// <returns>The memory deserialized to a value object.</returns>
	internal abstract Object InternalDeserialize(ReadOnlyMemory<Char> value, Type type, RpcKeyValueSerializerOptions options);
	#endregion

} // RpcKeyValueSerializerConverter
#endregion

#region RpcKeyValueSerializerConverter<T>
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerConverter<T>.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// A RPC Key/Value serializer converter, serializes and deserializes a object of a specifig type, to and from
/// a <see cref="System.Memory{System.Char}" />.
///
/// Inherit this class and override the "Serialize" and "Deserialize" methods, to serialize and deserialize the type
/// to and from a <see cref="System.Memory{System.Char}" /> value.
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
/// 	ReadOnlyMemory<Char>? str = base.GetStringNextDeserialize();
/// 	...
/// Or (get five strings or null in each loop):
/// 	base.ClearStrings(value);
/// 	ReadOnlyMemory<Char>[] strings = base.GetStringNextDeserialize(5);
/// 	...
/// </summary>
/// <typeparam name="T">The object type.</typeparam>
public abstract class RpcKeyValueSerializerConverter<T> : RpcKeyValueSerializerConverter {
	private StringBuilder serializeStringBuilder;
	private ReadOnlyMemory<Char> deserializeString;

	#region Overridden methods
	//------------------------------------------------------------------------------------------------------------------
	// Overridden methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <inheritdoc />
	public override Boolean CanConvert(Type type) {
		return (typeof(T).Equals(type) == true);
	} // CanConvert

	/// <inheritdoc />
	internal override ReadOnlyMemory<Char> InternalSerialize(Object obj, RpcKeyValueSerializerOptions options) {
		return this.Serialize((T)obj, options);
	} // InternalSerialize

	/// <inheritdoc />
	internal override Object InternalDeserialize(ReadOnlyMemory<Char> value, Type type, RpcKeyValueSerializerOptions options) {
		return this.Deserialize(value, type, options);
	} // InternalDeserialize
	#endregion

	#region Abstract methods
	//------------------------------------------------------------------------------------------------------------------
	// Abstract methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Serialize (convert) the T object into a <see cref="System.Memory{System.Char}" /> value.
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <param name="options">The serializer options.</param>
	/// <returns>The object as a <see cref="System.Memory{System.Char}" /> value.</returns>
	public abstract ReadOnlyMemory<Char> Serialize(T obj, RpcKeyValueSerializerOptions options);

	/// <summary>
	/// Deserialize (parse) the <see cref="System.Memory{System.Char}" /> value into a T object.
	/// </summary>
	/// <param name="value">The <see cref="System.Memory{System.Char}" /> value.</param>
	/// <param name="type">The preferred type.</param>
	/// <param name="options">The serializer options.</param>
	/// <returns>The object.</returns>
	public abstract T Deserialize(ReadOnlyMemory<Char> value, Type type, RpcKeyValueSerializerOptions options);
	#endregion

	#region Multiple strings helper methods
	//------------------------------------------------------------------------------------------------------------------
	// Multiple strings helper methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Clear the multiple strings builder.
	/// Clear the multiple strings iterator.
	/// </summary>
	protected void ClearStrings(ReadOnlyMemory<Char> deserializeString = default(ReadOnlyMemory<Char>)) {
		this.serializeStringBuilder = new StringBuilder();
		this.deserializeString = deserializeString;
	} // ClearStrings

	/// <summary>
	/// Append the string to the multiple strings builder.
	/// </summary>
	/// <param name="str">The string.</param>
	protected void AppendString(String str) {
		// Append the string like this: <length><space><string><newline>
		if (str != null) {
			this.serializeStringBuilder.AppendDelimiter($"{str.Length} {str}", RpcCoreSharedConstants.STRING_NEWLINE);
		} else {
			this.serializeStringBuilder.AppendDelimiter($"0 ", RpcCoreSharedConstants.STRING_NEWLINE);
		}
	} // AppendString

	/// <summary>
	/// Get all the appended strings, from the multiple strings builder, as one <see cref="System.Memory{System.Char}" />.
	/// Ready and safe to serialize.
	/// </summary>
	/// <returns></returns>
	protected ReadOnlyMemory<Char> GetStringForSerialize() {
		return this.serializeStringBuilder.ToString().AsMemory();
	} // GetStringForSerialize

	/// <summary>
	/// Gets the next string, from the multiple strings iterator.
	/// </summary>
	/// <returns>The next deserialized string, or null.</returns>
	protected ReadOnlyMemory<Char>? GetStringNextDeserialize() {
		Int32 indexSpace = this.deserializeString.Span.IndexOf(RpcCoreSharedConstants.CHAR_SPACE);
		if (indexSpace > 0) {
			// Get the string.
			Int32 length = Int32.Parse(this.deserializeString.Slice(0, indexSpace).Span);
			ReadOnlyMemory<Char> str = this.deserializeString.Slice(indexSpace + 1, length);	// +1 is the " " (space).

			// Iterate.
			this.deserializeString = this.deserializeString.Slice(indexSpace + length + 1);		// +1 is the "\n" (newline).

			// Return the next string.
			return str;
		}

		// No more strings.
		return null;
	} // GetStringNextDeserialize

	/// <summary>
	/// Gets the specified number of next strings, from the multiple strings iterator.
	/// </summary>
	/// <returns>The specified number of next deserialized string, or null.</returns>
	protected ReadOnlyMemory<Char>[] GetStringNextDeserialize(Int32 count) {
		ReadOnlyMemory<Char>[] result = new ReadOnlyMemory<Char>[count];
		for (Int32 index = 0; index < count; index++) {
			ReadOnlyMemory<Char>? resultString = this.GetStringNextDeserialize();
			if (resultString != null) {
				result[index] = (ReadOnlyMemory<Char>)resultString;
			} else {
				// Return null, when the specified number of strings are not available.
				return null;
			}
		}
		return result;
	} // GetStringNextDeserialize
	#endregion

} // RpcKeyValueSerializerConverter
#endregion
