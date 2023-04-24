namespace RpcScandinavia.Core.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#region RpcKeyValueSerializerListHandler
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerListHandler.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// This key/value serializer handler, serializes and deserializes a generic list, to and from one or more key/value
/// items.
/// </summary>
public class RpcKeyValueSerializerListHandler : RpcKeyValueSerializerHandler {

	#region Overridden methods
	//------------------------------------------------------------------------------------------------------------------
	// Overridden methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <inheritdoc />
	public override Boolean CanHandle(RpcMemberInfo memberInfo, RpcKeyValueSerializerOptions options) {
		return (memberInfo.IsGenericList == true);
	} // CanHandle

	/// <inheritdoc />
	public override void Serialize<KeyValueType>(RpcMemberInfo memberInfo, Object obj, String keyPath, RpcKeyValueBuilder<KeyValueType> keyValueBuilder, Int32 level, RpcKeyValueSerializerOptions options) {
		// Validate.
		RpcKeyValueException.ValidateLevel(level, options);
		RpcKeyValueException.ValidateIsAssignableFrom(memberInfo, obj);
		RpcKeyValueException.ValidateIsGenericList(memberInfo);

		// Get the type.
		Type objType = obj.GetType();

		// Get the items, which is the elements in the list.
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
		PropertyInfo objCountPropertyInfo = objType.GetProperty("Count");
		MethodInfo objGetItemMethodInfo = objType.GetMethod("get_Item");

		// Get the item type.
		Type itemType = objType.GetGenericArguments().First();

		// Serialize the items, which is the elements in the list.
		Int32 itemCount = (Int32)objCountPropertyInfo.GetValue(obj, null);
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
//Console.WriteLine($"DESERIALIZE LIST   Name: '{memberInfo.Name}'   Type: '{memberInfo.Type.Name}'   Key: '{keyPath}'   Object: '{obj?.GetType().Name}'   Level: {level}");
		// Validate.
		RpcKeyValueException.ValidateLevel(level, options);
		RpcKeyValueException.ValidateIsAssignableFrom(memberInfo, obj);
		RpcKeyValueException.ValidateIsGenericList(memberInfo);

		// Get the type.
		Type objType = obj.GetType();
		//MethodInfo objSetValueMethodInfo = objType.GetMethod("set_Item");
		MethodInfo objClearMethodInfo = objType.GetMethod("Clear");
		MethodInfo objAddMethodInfo = objType.GetMethod("Add");

		// Get the item type.
		Type itemType = objType.GetGenericArguments().First();

		// Clear the items collection.
		objClearMethodInfo.Invoke(obj, Array.Empty<Object>());

		// Deserialize each item.
		foreach (Int32 itemKey in keyValueProvider.GetKeysAsInt32(keyPath)) {
			try {
				// Get the member information for the current item.
				RpcMemberInfo member = new RpcMemberInfoType(
					itemType,
					$"{itemKey}",
					null,
					((obj, value, tag) => objAddMethodInfo.Invoke(obj, new Object[] { value }))
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
		PropertyInfo objCountPropertyInfo = objType.GetProperty("Count");
		MethodInfo objGetItemMethodInfo = objType.GetMethod("get_Item");
		MethodInfo objSetItemMethodInfo = objType.GetMethod("set_Item");

		// Get the item type.
		Type itemType = objType.GetGenericArguments().First();

		// Get the items, which is the elements in the list.
		List<RpcMemberInfo> items = new List<RpcMemberInfo>();
		Int32 itemCount = (Int32)objCountPropertyInfo.GetValue(obj, null);
		for (Int32 itemIndex = 0; itemIndex < itemCount; itemIndex++) {
			items.Add(
				// Get the member information for the current item.
				new RpcMemberInfoType(
					itemType,
					$"{itemIndex}",
					((obj, tag) => objGetItemMethodInfo.Invoke(obj, new Object[] { tag })),					// Get indexth value.
					((obj, value, tag) => objSetItemMethodInfo.Invoke(obj, new Object[] { value, tag })),		// Set indexth value.
					itemIndex																					// Tag.
				)
			);
		}

		return items;
	} // GetMemberInfos
	#endregion

} // RpcKeyValueSerializerListHandler
#endregion
