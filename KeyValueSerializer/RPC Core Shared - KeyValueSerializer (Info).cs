namespace RpcScandinavia.Core.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RpcScandinavia.Core;

#region RpcKeyValueSerializer (Info)
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializer (Info).
//----------------------------------------------------------------------------------------------------------------------
public static partial class RpcKeyValueSerializer {

	private static void InternalGetValueInfos(ReadOnlyMemory<Char> path, Object obj, List<IRpcKeyValueInfo> keyValueInfos, RpcKeyValueSerializerOptions options) {
		// Can not get information from null.
		if (obj == null) {
			return;
		}

		// Get the type.
		Type objType = obj?.GetType();

		//--------------------------------------------------------------------------------------------------------------
		// Array.
		//--------------------------------------------------------------------------------------------------------------
		// TODO: Implement, and support traversing the object hierarchy.

		//--------------------------------------------------------------------------------------------------------------
		// Generic List.
		//--------------------------------------------------------------------------------------------------------------
		// TODO: Implement, and support traversing the object hierarchy.

		//--------------------------------------------------------------------------------------------------------------
		// Generic Dictionary.
		//--------------------------------------------------------------------------------------------------------------
		// TODO: Implement, and support traversing the object hierarchy.

		//--------------------------------------------------------------------------------------------------------------
		// Enum with Flags attribute.
		//--------------------------------------------------------------------------------------------------------------
		// TODO: Implement.

		//--------------------------------------------------------------------------------------------------------------
		// Object.
		//--------------------------------------------------------------------------------------------------------------
		foreach (FieldInfo fieldInfo in objType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
			try {
				if ((fieldInfo.Name.Contains("k__BackingField") == false) &&
					(fieldInfo.GetSerializerIgnore() == false) &&
//					(fieldInfo.GetSerializerInclude() == true) &&
					(
						((fieldInfo.GetMemberIsPublic() == true) && (options.IncludePublicFields == true)) ||
						((fieldInfo.GetMemberIsPrivate() == true) && (options.IncludePrivateFields == true))
					)
				) {
					// Add information.
					keyValueInfos.Add(
						new RpcKeyValueInfo(
							path,
							fieldInfo.GetSerializerName().AsMemory(),
							fieldInfo.GetSerializerGroupName().AsMemory(),
							fieldInfo.GetSerializerOrder(),
							fieldInfo.DeclaringType,
							fieldInfo.FieldType,
							(type, inherit) => fieldInfo.GetCustomAttribute(type, inherit),
							() => fieldInfo.GetValue(obj),
							(itemValue) => fieldInfo.SetValue(obj, itemValue)
						)
					);
				}
			} catch (Exception exception) {
				throw new RpcKeyValueException($"Unable to get information about field '{fieldInfo.Name}' of type {fieldInfo.FieldType.Name}: {exception.Message}", RpcKeyValueExceptionType.Item);
			}
		}

		foreach (PropertyInfo propertyInfo in objType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
			try {
				if ((propertyInfo.GetSerializerIgnore() == false) &&
//					(propertyInfo.GetSerializerInclude() == true) &&
					(
						((propertyInfo.GetMemberIsPublicSet() == true) && (options.IncludePublicProperties == true)) ||
						((propertyInfo.GetMemberIsPrivateSet() == true) && (options.IncludePrivateProperties == true))
					)
				) {
					// Add information.
					keyValueInfos.Add(
						new RpcKeyValueInfo(
							path,
							propertyInfo.GetSerializerName().AsMemory(),
							propertyInfo.GetSerializerGroupName().AsMemory(),
							propertyInfo.GetSerializerOrder(),
							propertyInfo.DeclaringType,
							propertyInfo.PropertyType,
							(type, inherit) => propertyInfo.GetCustomAttribute(type, inherit),
							() => propertyInfo.GetValue(obj),
							(itemValue) => propertyInfo.SetValue(obj, itemValue)
						)
					);
				}
			} catch (Exception exception) {
				throw new RpcKeyValueException($"Unable to get information about property '{propertyInfo.Name}' of type {propertyInfo.PropertyType.Name}: {exception.Message}", RpcKeyValueExceptionType.Item);
			}
		}
	} // InternalGetValueInfos

} // RpcKeyValueDeserializer
#endregion
