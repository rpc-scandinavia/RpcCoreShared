namespace RpcScandinavia.Core.Shared.KeyValueSerializer;
using System;
using System.Collections.Generic;
using System.Text;

#region RpcKeyValueSerializerHandler
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerHandler.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// A RPC Key/Value serializer handler, serializes and deserializes a object of a specifig type, to and from one or
/// more key/value objects.
/// </summary>
public abstract class RpcKeyValueSerializerHandler {
	private static RpcKeyValueSerializerConverterHandler converterHandler = new RpcKeyValueSerializerConverterHandler();
	private static RpcKeyValueSerializerObjectHandler objectHandler = new RpcKeyValueSerializerObjectHandler();

	#region Static methods
	//------------------------------------------------------------------------------------------------------------------
	// Static methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets a handler, that can can serialize and deserialize the specified type.
	/// </summary>
	/// <param name="type">The type.</param>
	/// <param name="options">The serialization options.</param>
	/// <returns>Returns the handler or throws an exception if none exist.</returns>
	public static RpcKeyValueSerializerHandler GetHandler(Type type, RpcKeyValueSerializerOptions options) {
		if (type != null) {
//			if (RpcKeyValueSerializerHandler.converterHandler.CanHandle(type, options) == true) {
//				return RpcKeyValueSerializerHandler.converterHandler;
//			}
		}

		// Default handler.
		return RpcKeyValueSerializerHandler.objectHandler;
	} // GetHandler
	#endregion

	#region Abstract methods
	//------------------------------------------------------------------------------------------------------------------
	// Abstract methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets a value indicating whether or not this handler can serialize and deserialize the specified type.
	/// </summary>
	/// <param name="type">The type.</param>
	/// <param name="options">The serialization options.</param>
	/// <returns>Returns true if the type can be serialized and deserialized by this handler, false if not.</returns>
	public abstract Boolean CanHandle(Type type, RpcKeyValueSerializerOptions options);

	/// <summary>
	/// Serializes a object of the type this handler can serialize to one or more key/value objects.
	/// </summary>
	/// <param name="obj">The opject to serialize.</param>
	/// <param name="values">The list of serialized key/value objects.</param>
	/// <param name="keyPrefix">The prefix to the serialized keys.</param>
	/// <param name="createKeyValueInstance">A function that initializes a new KeyValueType with the key (first argument) and value (second argument).</param>
	/// <param name="options">The serializer options.</param>
	/// <typeparam name="KeyValueType">The type of the key/value objects.</typeparam>
	public abstract void Serialize<KeyValueType>(Object obj, List<KeyValueType> values, String keyPrefix, Func<String, String, KeyValueType> createKeyValueInstance, RpcKeyValueSerializerOptions options);

	/// <summary>
	/// Deserializes the matching key/value objects to a object of the type this handler can deserialize to.
	/// </summary>
	/// <param name="obj">The deserialized object. A 'null' indicates that the handler should create the object that is deserialized into.</param>
	/// <param name="values">The list of serialized key/value objects.</param>
	/// <param name="keyPrefix">The prefix to the serialized keys.</param>
	/// <param name="getKey">A function that gets the key from the KeyValueType.</param>
	/// <param name="getValue">A function that gets the value from the KeyValueType.</param>
	/// <param name="options">The serializer options.</param>
	/// <typeparam name="KeyValueType">The type of the key/value objects.</typeparam>
	/// <returns>Returns the object that is deserialized into.</returns>
	public abstract Object Deserialize<KeyValueType>(Object obj, IEnumerable<KeyValueType> values, String keyPrefix, Func<KeyValueType, String> getKey, Func<KeyValueType, String> getValue, RpcKeyValueSerializerOptions options);
	#endregion

} // RpcKeyValueSerializerHandler
#endregion
