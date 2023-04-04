namespace RpcScandinavia.Core.Shared.KeyValueSerializer;
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
	/// <param name="options">The serializer options.</param>
	/// <returns>The string deserialized to a value object.</returns>
	internal abstract Object InternalDeserialize(String value, RpcKeyValueSerializerOptions options);
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
/// </summary>
/// <typeparam name="T">The object type.</typeparam>
public abstract class RpcKeyValueSerializerConverter<T> : RpcKeyValueSerializerConverter {

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
	internal override Object InternalDeserialize(String value, RpcKeyValueSerializerOptions options) {
		return this.Deserialize(value, options);
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
	/// <param name="options">The serializer options.</param>
	/// <returns>The object.</returns>
	public abstract T Deserialize(String value, RpcKeyValueSerializerOptions options);
	#endregion

} // RpcKeyValueSerializerConverter
#endregion
