namespace RpcScandinavia.Core.Shared.KeyValueSerializer;
using System;
using System.Collections.Generic;
using System.Linq;

#region RpcKeyValueSerializerHandler
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerHandler.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// The key/value serializer handler, serializes and deserializes a object of a specifig type, to and from one or
/// more key/value items.
/// </summary>
public abstract class RpcKeyValueSerializerHandler {
	private static RpcKeyValueSerializerHandler[] handlers = {
		new RpcKeyValueSerializerListHandler(),
		new RpcKeyValueSerializerArrayHandler(),
		new RpcKeyValueSerializerDictionaryHandler(),
		new RpcKeyValueSerializerEnumHandler(),
		new RpcKeyValueSerializerObjectHandler()						// Must be the last handler.
	};

	#region Static methods
	//------------------------------------------------------------------------------------------------------------------
	// Static methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets a handler, that can can serialize and deserialize the specified type.
	/// </summary>
	/// <param name="memberInfo">The member information about the object.</param>
	/// <param name="options">The serialization options.</param>
	/// <returns>Returns the handler.</returns>
	public static RpcKeyValueSerializerHandler GetHandler(RpcMemberInfo memberInfo, RpcKeyValueSerializerOptions options) {
		foreach (RpcKeyValueSerializerHandler handler in RpcKeyValueSerializerHandler.handlers) {
			if (handler.CanHandle(memberInfo, options) == true) {
				return handler;
			}
		}

		// We newer get here, because the RpcKeyValueSerializerObjectHandler answers "true" to everything.
		return RpcKeyValueSerializerHandler.handlers.Last();
	} // GetHandler
	#endregion

	#region Abstract methods
	//------------------------------------------------------------------------------------------------------------------
	// Abstract methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets a value indicating whether or not this handler can serialize and deserialize the specified type.
	/// </summary>
	/// <param name="memberInfo">The member information about the object.</param>
	/// <param name="options">The serialization options.</param>
	/// <returns>Returns true if the type can be serialized and deserialized by this handler, false if not.</returns>
	public abstract Boolean CanHandle(RpcMemberInfo memberInfo, RpcKeyValueSerializerOptions options);

	/// <summary>
	/// Serializes a object of the type this handler can serialize, into one or more key/value items.
	///
	/// Note that the member information should be the declared type (abstract, interface, item/element), where the
	/// object can be a descendend/implementer of the member information.
	/// </summary>
	/// <param name="memberInfo">The member information about the object (declared).</param>
	/// <param name="obj">The opject to serialize.</param>
	/// <param name="keyPrefix">The prefix to the serialized keys.</param>
	/// <param name="keyValueBuilder">The serialized key/value items.</param>
	/// <param name="level">The level traveling down the object hierarchy.</param>
	/// <param name="options">The serializer options.</param>
	/// <typeparam name="KeyValueType">The type of the key/value items.</typeparam>
	public abstract void Serialize<KeyValueType>(RpcMemberInfo memberInfo, Object obj, String keyPrefix, RpcKeyValueBuilder<KeyValueType> keyValueBuilder, Int32 level, RpcKeyValueSerializerOptions options);

	/// <summary>
	/// Deserializes the matching key/value items to a object of the type this handler can deserialize to.
	///
	/// Note that hte member information should be the deflared type (abstract, interface, item/element), where the
	/// object can be a descendend/implementer of the member information.
	/// </summary>
	/// <param name="memberInfo">The member information about the object (declared).</param>
	/// <param name="obj">The deserialized object. A 'null' indicates that the handler should create the object that is deserialized into.</param>
	/// <param name="keyPrefix">The prefix to the serialized keys.</param>
	/// <param name="keyValueProvider">The serialized key/value items.</param>
	/// <param name="level">The level traveling down the object hierarchy.</param>
	/// <param name="options">The serializer options.</param>
	/// <typeparam name="KeyValueType">The type of the key/value items.</typeparam>
	/// <returns>Returns the object that is deserialized into.</returns>
	public abstract Object Deserialize<KeyValueType>(RpcMemberInfo memberInfo, Object obj, String keyPrefix, RpcKeyValueProvider<KeyValueType> keyValueProvider, Int32 level, RpcKeyValueSerializerOptions options);
	#endregion

	#region Helper methods
	//------------------------------------------------------------------------------------------------------------------
	// Helper methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Serialize one member value from the object.
	/// The member value can be the field or property of a object, or a item in a collection.
	///
	/// Note that the member information should be the deflared type (abstract, interface, item/element), where the
	/// object can be a descendend/implementer of the member information.
	/// </summary>
	/// <param name="memberInfo">The member information about the object (declared).</param>
	/// <param name="obj">The opject to serialize.</param>
	/// <param name="metaDataRequired">The meta data is required if the member should be deserialized.</param>
	/// <param name="keyPrefix">The prefix to the serialized keys.</param>
	/// <param name="keyValueBuilder">The serialized key/value items.</param>
	/// <param name="level">The level traveling down the object hierarchy.</param>
	/// <param name="options">The serializer options.</param>
	/// <typeparam name="KeyValueType">The type of the key/value items.</typeparam>
	protected void SerializeMember<KeyValueType>(RpcMemberInfo memberInfo, Object obj, Boolean metaDataRequired, String keyPrefix, RpcKeyValueBuilder<KeyValueType> keyValueBuilder, Int32 level, RpcKeyValueSerializerOptions options) {
		if (memberInfo.ShouldSerialize(options) == true) {
			Boolean serialized = false;

			// Serialize using a converter.
			if (serialized == false) {
				RpcKeyValueSerializerConverter converter = RpcKeyValueSerializerConverter.GetConverter(memberInfo.Type, options);
				if (converter != null) {
					// Get the member value.
					Object valueObject = memberInfo.GetValue(obj);
					if (valueObject != null) {
						// Add the meta data.
						Boolean memberMetaDataRequired = ((metaDataRequired == true) || (memberInfo.Type != valueObject.GetType()));
						if ((options.SerializeTypeInfo == RpcKeyValueSerializerTypeInfoOption.Always) ||
							((options.SerializeTypeInfo == RpcKeyValueSerializerTypeInfoOption.RequiredAndTop) && (memberMetaDataRequired == true)) ||
							((options.SerializeTypeInfo == RpcKeyValueSerializerTypeInfoOption.Required) && (memberMetaDataRequired == true))) {
							keyValueBuilder.AddTypeMetadata(keyPrefix, memberInfo.Name, valueObject.GetType());
						}

						// Add the member value.
						keyValueBuilder.Add(keyPrefix, memberInfo.Name, converter.InternalSerialize(valueObject, options));
					}
					serialized = true;
				}
			}

			// Serialize using a handler.
			if (serialized == false) {
				RpcKeyValueSerializerHandler handler = RpcKeyValueSerializerHandler.GetHandler(memberInfo, options);
				if (handler != null) {
					// Get the member value.
					Object valueObject = memberInfo.GetValue(obj);
					if (valueObject != null) {
						// Add the meta data.
						Boolean memberMetaDataRequired = ((metaDataRequired == true) || (memberInfo.Type != valueObject.GetType()));
						if ((options.SerializeTypeInfo == RpcKeyValueSerializerTypeInfoOption.Always) ||
							((options.SerializeTypeInfo == RpcKeyValueSerializerTypeInfoOption.RequiredAndTop) && (memberMetaDataRequired == true)) ||
							((options.SerializeTypeInfo == RpcKeyValueSerializerTypeInfoOption.Required) && (memberMetaDataRequired == true))) {
							keyValueBuilder.AddTypeMetadata(keyPrefix, memberInfo.Name, valueObject.GetType());
						}

						// Add the member value.
						handler.Serialize<KeyValueType>(
							memberInfo,
							valueObject,
							keyValueBuilder.GetNextLevelKeyPrefix(keyPrefix, memberInfo.Name),
							keyValueBuilder,
							level + 1,
							options
						);
					}
					serialized = true;
				}
			}

			// Undble to serialize the member.
			if (serialized == false) {
				throw new Exception($"Unable to serialize member '{memberInfo.Name}' of type {memberInfo.Type.Name}.");
			}
		}
	} // SerializeMember

	/// <summary>
	/// Deserialize one member value to the object.
	/// Member values can be fields or properties of a object, or items in a collection.
	///
	/// Note that the member information should be the deflared type (abstract, interface, item/element), where the
	/// object can be a descendend/implementer of the member information.
	/// </summary>
	/// <param name="memberInfo">The member information about the object (declared).</param>
	/// <param name="obj">The opject to serialize.</param>
	/// <param name="keyPrefix">The prefix to the serialized keys.</param>
	/// <param name="keyValueProvider">The serialized key/value items.</param>
	/// <param name="level">The level traveling down the object hierarchy.</param>
	/// <param name="options">The serializer options.</param>
	/// <typeparam name="KeyValueType">The type of the key/value items.</typeparam>
	protected void DeserializeMember<KeyValueType>(RpcMemberInfo memberInfo, Object obj, String keyPrefix, RpcKeyValueProvider<KeyValueType> keyValueProvider, Int32 level, RpcKeyValueSerializerOptions options) {
		if (memberInfo.ShouldDeserialize(options) == true) {
//Console.WriteLine($"MEMBER   '{memberInfo.Name}'   '{memberInfo.Type.Name}'   '{obj?.GetType().Name}'   '{keyValueProvider.GetCount(keyPrefix, memberInfo.Name)}'");
			Boolean deserialized = false;

			// Deserialize using a converter.
			if (deserialized == false) {
				// Get the type from the meta data.
				String metaTypeName = keyValueProvider.GetTypeMetadata(keyPrefix, memberInfo.Name);
				Type metaType = (metaTypeName != null) ? Type.GetType(metaTypeName, false) ?? memberInfo.Type : memberInfo.Type;

				RpcKeyValueSerializerConverter converter = RpcKeyValueSerializerConverter.GetConverter(metaType, options);
				if (converter != null) {
					// Get the member value.
					String value = keyValueProvider.GetValue(keyPrefix, memberInfo.Name);

					// Set the member value.
					memberInfo.SetValue(obj, converter.InternalDeserialize(value, options));
					deserialized = true;
				}
			}

			// Deserialize using a handler.
			if (deserialized == false) {
if ((keyValueProvider.GetCount(keyPrefix, memberInfo.Name) > 0) || (memberInfo.Type != typeof(Object))) {
					RpcKeyValueSerializerHandler handler = RpcKeyValueSerializerHandler.GetHandler(memberInfo, options);
					if (handler != null) {
						// Get the new/empty member value.
						// Create a new instance of the object, using the type taken from the meta data "$Type" value,
						// or the member information.
						Object value = this.CreateInstance<KeyValueType>(memberInfo, keyPrefix, keyValueProvider, memberInfo.GetValue(obj));

						// Set the member value.
						memberInfo.SetValue(
							obj,
							handler.Deserialize<KeyValueType>(
								memberInfo,
								value,
								keyValueProvider.GetNextLevelKeyPrefix(keyPrefix, memberInfo.Name),
								keyValueProvider,
								level + 1,
								options
							)
						);
					}
}
				deserialized = true;
			}

			// Undble to deserialize the member.
			if (deserialized == false) {
				throw new Exception($"Unable to deserialize member '{memberInfo.Name}' of type {memberInfo.Type.Name}.");
			}
		}
	} // DeserializeMember

	/// <summary>
	/// Create a new instance of the object that should be serialized into.
	/// </summary>
	/// <param name="memberInfo">The member information about the object.</param>
	/// <param name="keyPrefix">The prefix to the serialized keys.</param>
	/// <param name="keyValueProvider">The serialized key/value items.</param>
	/// <param name="defaultValue">The default value returned when exceptions are caught.</param>
	/// <typeparam name="KeyValueType">The type of the key/value items.</typeparam>
	/// <returns>Returns the object that can be deserialized into.</returns>
	protected Object CreateInstance<KeyValueType>(RpcMemberInfo memberInfo, String keyPrefix, RpcKeyValueProvider<KeyValueType> keyValueProvider, Object defaultValue) {
		try {
			// Get the type from the meta data.
			String metaTypeName = keyValueProvider.GetTypeMetadata(keyPrefix, memberInfo.Name);
			Type metaType = (metaTypeName != null) ? Type.GetType(metaTypeName, false) ?? memberInfo.Type : memberInfo.Type;
			return Activator.CreateInstance(metaType);
		} catch {
			// Return the default value.
			return defaultValue;
		}
	} // CreateInstance

/*
	protected String EncodeKeyString(String value, RpcKeyValueSerializerOptions options) {
		return value
			.Replace($"{options.KeyEscapeCharA}", $"{options.KeyEscapeCharA}{options.KeyEscapeCharA}")
			.Replace($"{options.KeyEscapeCharB}", $"{options.KeyEscapeCharA}{options.KeyEscapeCharB}")
			.Replace($"{options.HierarchySeparatorChar}", $"{options.KeyEscapeCharB}{options.KeyEscapeCharB}");
	} // EncodeKeyString

	protected String DecodeKeyString(String value, RpcKeyValueSerializerOptions options) {
		return value
			.Replace($"{options.KeyEscapeCharB}{options.KeyEscapeCharB}", $"{options.HierarchySeparatorChar}")
			.Replace($"{options.KeyEscapeCharA}{options.KeyEscapeCharB}", $"{options.KeyEscapeCharB}")
			.Replace($"{options.KeyEscapeCharA}{options.KeyEscapeCharA}", $"{options.KeyEscapeCharA}");
	} // DecodeKeyString
*/
/*
	protected Int32 ToInt32OrDefault(String value) {
		try {
			return Int32.Parse(value);
		} catch {
			return -1;
		}
	} // ToInt32OrDefault
*/
	#endregion

} // RpcKeyValueSerializerHandler
#endregion
