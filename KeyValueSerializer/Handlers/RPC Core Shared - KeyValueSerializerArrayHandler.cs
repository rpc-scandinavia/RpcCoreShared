namespace RpcScandinavia.Core.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

#region RpcKeyValueSerializerArrayHandler
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerArrayHandler.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// This key/value serializer handler, serializes and deserializes an array, to and from one or more key/value items.
/// </summary>
public class RpcKeyValueSerializerArrayHandler : RpcKeyValueSerializerHandler {

	#region Overridden methods
	//------------------------------------------------------------------------------------------------------------------
	// Overridden methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <inheritdoc />
	public override Boolean CanHandle(RpcMemberInfo memberInfo, RpcKeyValueSerializerOptions options) {
		return (memberInfo.IsArray == true);
	} // CanHandle

	/// <inheritdoc />
	public override void Serialize<KeyValueType>(RpcMemberInfo memberInfo, Object obj, String keyPath, RpcKeyValueBuilder<KeyValueType> keyValueBuilder, Int32 level, RpcKeyValueSerializerOptions options) {
		// Validate.
		RpcKeyValueException.ValidateLevel(level, options);
		RpcKeyValueException.ValidateIsAssignableFrom(memberInfo, obj);
		RpcKeyValueException.ValidateIsArray(memberInfo);

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
		PropertyInfo objLengthPropertyInfo = objType.GetProperty("Length");
		MethodInfo objGetItemMethodInfo = objType.GetMethod("Get");

		// Get the item type.
		Type itemType = objType.GetElementType();

		// Serialize the items, which is the elements in the array.
		Int32 itemCount = (Int32)objLengthPropertyInfo.GetValue(obj, null);
		for (Int32 itemIndex = 0; itemIndex < itemCount; itemIndex++) {
			try {
				// Get the current item.
				Object itemObject = objGetItemMethodInfo.Invoke(obj, new Object[] { itemIndex });

				// Get the member information for the current item.
				RpcMemberInfo member = new RpcMemberInfoType(
					itemObject.GetType(),
					$"{itemIndex}",
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
		RpcKeyValueException.ValidateIsArray(memberInfo);

		// Get the type.
		Type objType = obj.GetType();

		// Get the item type.
		Type itemType = objType.GetElementType();

		// Clear the items collection.
		// Because this is an array, we need to create a list where the deserialized items can be added, and then
		// replace the array with a new array from the list.
		ArrayList objItemsList = new ArrayList();

		// Deserialize each item.
		foreach (Int32 itemKey in keyValueProvider.GetKeysAsInt32(keyPath)) {
			try {
				// Get the member information for the current item.
				RpcMemberInfo member = new RpcMemberInfoType(
					itemType,
					$"{itemKey}",
					null,
					((obj, value, tag) => ((ArrayList)obj).Add(value))
				);

				// Deserialize the item.
				base.DeserializeMember<KeyValueType>(
					member,
					objItemsList,
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

		// Because this is an array, we need to create a new array with the precise number of items, and then copy
		// the items from the temporary array list to the new array.
		// Create a new instance of the array.
		obj = objItemsList.ToArray(itemType);

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
		PropertyInfo objLengthPropertyInfo = objType.GetProperty("Length");
		MethodInfo objGetItemMethodInfo = objType.GetMethod("Get");
		MethodInfo objSetItemMethodInfo = objType.GetMethod("Set");

		// Get the item type.
		Type itemType = objType.GetElementType();

		// Get the items, which is the elements in the list.
		List<RpcMemberInfo> items = new List<RpcMemberInfo>();
		Int32 itemCount = (Int32)objLengthPropertyInfo.GetValue(obj, null);
		for (Int32 itemIndex = 0; itemIndex < itemCount; itemIndex++) {
			items.Add(
				// Get the member information for the current item.
				new RpcMemberInfoType(
					itemType,
					$"{itemIndex}",
					((obj, tag) => objGetItemMethodInfo.Invoke(obj, new Object[] { tag })),						// Get indexth value.
					((obj, value, tag) => objSetItemMethodInfo.Invoke(obj, new Object[] { value, tag })),		// Set indexth value.
					itemIndex																					// Tag.
				)
			);
		}

		return items;
	} // GetMemberInfos
	#endregion

} // RpcKeyValueSerializerArrayHandler
#endregion
