namespace RpcScandinavia.Core.Shared.KeyValueSerializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

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
	public override void Serialize<KeyValueType>(RpcMemberInfo memberInfo, Object obj, String keyPrefix, RpcKeyValueBuilder<KeyValueType> keyValueBuilder, Int32 level, RpcKeyValueSerializerOptions options) {
		// Validate.
		RpcKeyValueException.ValidateLevel(level, options);
		RpcKeyValueException.ValidateIsAssignableFrom(memberInfo, obj);
		// Validate is generic list.

		// Get the type.
		Type objType = obj.GetType();
		PropertyInfo objCountPropertyInfo = objType.GetProperty("Count");
		MethodInfo objGetValueMethodInfo = objType.GetMethod("get_Item");

		// Get the item type.
		Type itemType = objType.GetGenericArguments().First();

		// Serialize the items, which is the elements in the list.
		Int32 itemCount = (Int32)objCountPropertyInfo.GetValue(obj, null);
		for (Int32 itemIndex = 0; itemIndex < itemCount; itemIndex++) {
			try {
				// Get the current item.
				Object itemObject = objGetValueMethodInfo.Invoke(obj, new Object[] { itemIndex });

				// Get the member information for the current item.
				RpcMemberInfo member = new RpcMemberInfoType(
					itemObject.GetType(),
					$"{itemIndex}",
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
//Console.WriteLine($"LIST   '{memberInfo.Name}'   '{memberInfo.Type.Name}'   '{obj?.GetType().Name}'");
		RpcKeyValueException.ValidateLevel(level, options);
		RpcKeyValueException.ValidateIsAssignableFrom(memberInfo, obj);
		// Validate is generic list.

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
		foreach (Int32 itemKey in keyValueProvider.GetKeysAsInt32(keyPrefix)) {
			try {
				// Get the member information for the current item.
				RpcMemberInfo member = new RpcMemberInfoType(
					itemType,
					$"{itemKey}",
					null,
					((obj, value) => objAddMethodInfo.Invoke(obj, new Object[] { value }))
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

} // RpcKeyValueSerializerListHandler
#endregion
