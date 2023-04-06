namespace RpcScandinavia.Core.Shared.KeyValueSerializer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

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
	public override void Serialize<KeyValueType>(RpcMemberInfo memberInfo, Object obj, String keyPrefix, RpcKeyValueBuilder<KeyValueType> keyValueBuilder, Int32 level, RpcKeyValueSerializerOptions options) {
		// Validate.
		RpcKeyValueException.ValidateLevel(level, options);
		RpcKeyValueException.ValidateIsAssignableFrom(memberInfo, obj);
		// Validate is array.

		// Get the type.
		Type objType = obj.GetType();
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
//Console.WriteLine($"ARRAY   '{memberInfo.Name}'   '{memberInfo.Type.Name}'   '{obj?.GetType().Name}'");
		RpcKeyValueException.ValidateLevel(level, options);
		RpcKeyValueException.ValidateIsAssignableFrom(memberInfo, obj);
		// Validate is array.

		// Get the type.
		Type objType = obj.GetType();

		// Get the item type.
		Type itemType = objType.GetElementType();

		// Clear the items collection.
		// Because this is an array, we need to create a list where the deserialized items can be added, and then
		// replace the array with a new array from the list.
		ArrayList objItemsList = new ArrayList();

		// Deserialize each item.
		foreach (Int32 itemKey in keyValueProvider.GetKeysAsInt32(keyPrefix)) {
			try {
				// Get the member information for the current item.
				RpcMemberInfo member = new RpcMemberInfoType(
					itemType,
					$"{itemKey}",
					null,
					((obj, value) => objItemsList.Add(value))
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

		// Because this is an array, we need to create a new array with the precise number of items, and then copy
		// the items from the temporary array list to the new array.
		// Create a new instance of the array.
		obj = objItemsList.ToArray(itemType);

		// Return the object.
		return obj;
	} // Deserialize
	#endregion

} // RpcKeyValueSerializerArrayHandler
#endregion
