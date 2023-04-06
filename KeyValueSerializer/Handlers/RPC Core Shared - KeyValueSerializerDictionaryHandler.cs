namespace RpcScandinavia.Core.Shared.KeyValueSerializer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

#region RpcKeyValueSerializerDictionaryHandler
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerDictionaryHandler.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// This key/value serializer handler, serializes and deserializes a generic dictionary, to and from one or more
/// key/value items.
/// </summary>
public class RpcKeyValueSerializerDictionaryHandler : RpcKeyValueSerializerHandler {

	#region Overridden methods
	//------------------------------------------------------------------------------------------------------------------
	// Overridden methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <inheritdoc />
	public override Boolean CanHandle(RpcMemberInfo memberInfo, RpcKeyValueSerializerOptions options) {
		return (memberInfo.IsGenericDictionary == true);
	} // CanHandle

	/// <inheritdoc />
	public override void Serialize<KeyValueType>(RpcMemberInfo memberInfo, Object obj, String keyPrefix, RpcKeyValueBuilder<KeyValueType> keyValueBuilder, Int32 level, RpcKeyValueSerializerOptions options) {
		// Validate.
		RpcKeyValueException.ValidateLevel(level, options);
		RpcKeyValueException.ValidateIsAssignableFrom(memberInfo, obj);
		RpcKeyValueException.ValidateIsGenericDictionary(memberInfo);

		// Get the type.
		Type objType = obj.GetType();
		//PropertyInfo objCountPropertyInfo = objType.GetProperty("Count");
		PropertyInfo objKeysPropertyInfo = objType.GetProperty("Keys");
		PropertyInfo objItemPropertyInfo = objType.GetProperty("Item");

		// Get the key type, and the item type.
		Type keyType = objType.GetGenericArguments()[0];
		Type itemType = objType.GetGenericArguments()[1];

		// Get the key converter, that can serialize and deserialize the key.
		RpcKeyValueSerializerConverter keyConverter = RpcKeyValueSerializerConverter.GetConverter(keyType, options);
		if (keyConverter == null) {
			throw new RpcKeyValueException(
				$"The dictionary '{keyType.Name}' key type, can not be serialized.",
				RpcKeyValueExceptionType.Item
			);
		}

		// Serialize the items, which is the elements in the dictionary.
		ICollection keys = (ICollection)objKeysPropertyInfo.GetValue(obj, null);
		foreach (Object key in keys) {
			try {
				// Get the current key and item.
				String keyString = keyConverter.InternalSerialize(key, options);
				Object itemObject = objItemPropertyInfo.GetValue(obj, new Object[] { key });

				// Get the member information for the current item.
				RpcMemberInfo member = new RpcMemberInfoType(
					itemObject.GetType(),
					$"{keyString}",
					((obj) => itemObject),
					null
				);

				// Serialize the item.
				base.SerializeMember<KeyValueType>(
					member,
					obj,
					itemType.IsAbstract,
					keyPrefix,
					keyValueBuilder,
					level,
					options
				);
			} catch (RpcKeyValueException exception) {
				// Throw the exception.
				exception.Throw(options);
			}
		}
	} // Serialize

	/// <inheritdoc />
	public override Object Deserialize<KeyValueType>(RpcMemberInfo memberInfo, Object obj, String keyPrefix, RpcKeyValueProvider<KeyValueType> keyValueProvider, Int32 level, RpcKeyValueSerializerOptions options) {
		// Validate.
		RpcKeyValueException.ValidateLevel(level, options);
		RpcKeyValueException.ValidateIsAssignableFrom(memberInfo, obj);
		RpcKeyValueException.ValidateIsGenericDictionary(memberInfo);

		// Get the type.
		Type objType = obj.GetType();
		MethodInfo objClearMethodInfo = objType.GetMethod("Clear");
		MethodInfo objAddMethodInfo = objType.GetMethod("Add");

		// Get the key type and the item type.
		Type keyType = objType.GetGenericArguments()[0];
		Type itemType = objType.GetGenericArguments()[1];

		// Get the key converter, that can serialize and deserialize the key.
		RpcKeyValueSerializerConverter keyConverter = RpcKeyValueSerializerConverter.GetConverter(keyType, options);
		if (keyConverter == null) {
			throw new RpcKeyValueException(
				$"The dictionary '{keyType.Name}' key type, can not be serialized.",
				RpcKeyValueExceptionType.Item
			);
		}

		// Clear the items collection.
		objClearMethodInfo.Invoke(obj, Array.Empty<Object>());

		// Deserialize each item.
		foreach (String itemKey in keyValueProvider.GetKeys(keyPrefix)) {
			try {
				// Get the key.
				Object key = keyConverter.InternalDeserialize(itemKey, options);

				// Get the member information for the current item.
				RpcMemberInfo member = new RpcMemberInfoType(
					itemType,
					$"{itemKey}",
					null,
					((obj, value) => objAddMethodInfo.Invoke(obj, new Object[] { key, value }))
				);

				// Deserialize the item.
				base.DeserializeMember<KeyValueType>(
					member,
					obj,
					keyPrefix,
					keyValueProvider,
					level,
					options
				);
			} catch (RpcKeyValueException exception) {
				// Throw the exception.
				exception.Throw(options);
			}
		}

		// Return the object.
		return obj;
	} // Deserialize
	#endregion

} // RpcKeyValueSerializerDictionaryHandler
#endregion
