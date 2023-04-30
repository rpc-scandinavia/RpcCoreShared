namespace RpcScandinavia.Core.Shared;
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
	public override void Serialize<KeyValueType>(RpcMemberInfo memberInfo, Object obj, String keyPath, RpcKeyValueBuilder<KeyValueType> keyValueBuilder, Int32 level, RpcKeyValueSerializerOptions options) {
		// Validate.
		RpcKeyValueException.ValidateLevel(level, options);
		RpcKeyValueException.ValidateIsAssignableFrom(memberInfo, obj);
		RpcKeyValueException.ValidateIsGenericDictionary(memberInfo);

		// Get the type.
		Type objType = obj.GetType();

		// Get the items, which is the elements in the array.
		List<RpcMemberInfo> items = this.GetMemberInfos(obj, options);

		// Serialize each item.
		foreach (RpcMemberInfo item in items) {
			try {
				base.SerializeMember<KeyValueType>(
					item,
					obj,
					item.IsAbstract,
					keyPath,
					keyValueBuilder,
					level,
					options
				);
			} catch (RpcKeyValueException exception) {
				// Throw the exception.
				exception.Throw(options);
			}
		}

/*
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
					((obj, tag) => itemObject),
					null
				);

				// Serialize the item.
				base.SerializeMember<KeyValueType>(
					member,
					obj,
					itemType.IsAbstract,
					keyPath,
					keyValueBuilder,
					level,
					options
				);
			} catch (RpcKeyValueException exception) {
				// Throw the exception.
				exception.Throw(options);
			}
		}
*/
	} // Serialize

	/// <inheritdoc />
	public override Object Deserialize<KeyValueType>(RpcMemberInfo memberInfo, Object obj, String keyPath, RpcKeyValueProvider<KeyValueType> keyValueProvider, Int32 level, RpcKeyValueSerializerOptions options) {
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
		foreach (String itemKey in keyValueProvider.GetKeys(keyPath)) {
			try {
				// Get the key.
				Object key = keyConverter.InternalDeserialize(itemKey, keyType, options);

				// Get the member information for the current item.
				RpcMemberInfo member = new RpcMemberInfoType(
					itemType,
					$"{itemKey}",
					null,
					((obj, value, tag) => objAddMethodInfo.Invoke(obj, new Object[] { key, value }))
				);

				// Deserialize the item.
				base.DeserializeMember<KeyValueType>(
					member,
					obj,
					keyPath,
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

	/// <inheritdoc />
	public override List<RpcMemberInfo> GetMemberInfos(Object obj, RpcKeyValueSerializerOptions options) {
		// Validate.
		if (obj == null) {
			throw new NullReferenceException(nameof(obj));
		}

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

		// Get the items, which is the elements in the dictionary.
		List<RpcMemberInfo> items = new List<RpcMemberInfo>();
		ICollection keys = (ICollection)objKeysPropertyInfo.GetValue(obj, null);
		foreach (Object key in keys) {
			// Get the current key and item.
			String keyString = keyConverter.InternalSerialize(key, options);
			items.Add(
				// Get the member information for the current item.
				new RpcMemberInfoType(
					itemType,
					keyString,
					((obj, tag) => objItemPropertyInfo.GetValue(obj, new Object[] { tag })),					// Get keyed value.
					((obj, value, tag) => objItemPropertyInfo.SetValue(obj, value, new Object[] { tag })),		// Set keyed value.
					key																							// Tag.
				)
			);
		}

		return items;
	} // GetMemberInfos
	#endregion

} // RpcKeyValueSerializerDictionaryHandler
#endregion
