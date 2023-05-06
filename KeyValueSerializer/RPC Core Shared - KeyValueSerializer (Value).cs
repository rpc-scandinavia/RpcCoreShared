namespace RpcScandinavia.Core.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using RpcScandinavia.Core;

#region RpcKeyValueSerializer (Value)
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializer (Value).
//----------------------------------------------------------------------------------------------------------------------
public static partial class RpcKeyValueSerializer {

	private static IRpcKeyValueInfo GotoValue(Object obj, ReadOnlyMemory<Char> path, RpcKeyValueSerializerOptions options) {
		return RpcKeyValueSerializer.GotoValue(obj, path, path, options);
	} // GotoValue

	private static IRpcKeyValueInfo GotoValue(Object obj, ReadOnlyMemory<Char> remainingPath, ReadOnlyMemory<Char> fullPath, RpcKeyValueSerializerOptions options) {
		// Can't goto item in null.
		if (obj == null) {
			return null;
		}

		// Get the type.
		Type objType = obj.GetType();

		// Get the key and the remaining path.
		ReadOnlyMemory<Char> key;
		Int32 pathSeparatorIndex = remainingPath.Span.IndexOf(options.HierarchySeparatorChar);
		if (pathSeparatorIndex > -1) {
			key = remainingPath.Slice(0, pathSeparatorIndex);
			remainingPath = remainingPath.Slice(pathSeparatorIndex + 1);
		} else {
			key = remainingPath;
			remainingPath = ReadOnlyMemory<Char>.Empty;
		}

		//--------------------------------------------------------------------------------------------------------------
		// Converter.
		//--------------------------------------------------------------------------------------------------------------

		//--------------------------------------------------------------------------------------------------------------
		// Array.
		//--------------------------------------------------------------------------------------------------------------
		if (objType.IsArray() == true) {
			PropertyInfo objLengthPropertyInfo = objType.GetProperty("Length");
			MethodInfo objGetItemMethodInfo = objType.GetMethod("Get");
			MethodInfo objSetItemMethodInfo = objType.GetMethod("Set");

			// Goto.
			Int32 itemCount = (Int32)objLengthPropertyInfo.GetValue(obj, null);
			Int32 itemIndex = key.ToString().ToInt32(-1);
			if ((itemIndex > -1) && (itemIndex < itemCount)) {
				if (remainingPath.Length == 0) {
					// Return.
					Type itemType = objType.GetElementType();
					return new RpcKeyValueInfo(
						fullPath,
						key,
						itemType,
						() => objGetItemMethodInfo.Invoke(obj, new Object[] { itemIndex }),
						(itemValue) => objSetItemMethodInfo.Invoke(obj, new Object[] { itemIndex, itemValue })
					);
				} else {
					// Goto next key in the path.
					Object valueObject = objGetItemMethodInfo.Invoke(obj, new Object[] { itemIndex });
					return RpcKeyValueSerializer.GotoValue(valueObject, remainingPath, fullPath, options);
				}
			}
		}

		//--------------------------------------------------------------------------------------------------------------
		// Generic List.
		//--------------------------------------------------------------------------------------------------------------
		if (objType.IsGenericList() == true) {
			PropertyInfo objCountPropertyInfo = objType.GetProperty("Count");
			MethodInfo objGetItemMethodInfo = objType.GetMethod("get_Item");
			MethodInfo objSetItemMethodInfo = objType.GetMethod("set_Item");

			// Goto.
			Int32 itemCount = (Int32)objCountPropertyInfo.GetValue(obj, null);
			Int32 itemIndex = key.ToString().ToInt32(-1);
			if ((itemIndex > -1) && (itemIndex < itemCount)) {
				if (remainingPath.Length == 0) {
					// Return.
					Type itemType = objType.GetGenericArguments().First();
					return new RpcKeyValueInfo(
						fullPath,
						key,
						itemType,
						() => objGetItemMethodInfo.Invoke(obj, new Object[] { itemIndex }),
						(itemValue) => objSetItemMethodInfo.Invoke(obj, new Object[] { itemIndex, itemValue })
					);
				} else {
					// Goto next key in the path.
					Object valueObject = objGetItemMethodInfo.Invoke(obj, new Object[] { itemIndex });
					return RpcKeyValueSerializer.GotoValue(valueObject, remainingPath, fullPath, options);
				}
			}
		}

		//--------------------------------------------------------------------------------------------------------------
		// Generic Dictionary.
		//--------------------------------------------------------------------------------------------------------------
		if (objType.IsGenericDictionary() == true) {
			PropertyInfo objCountPropertyInfo = objType.GetProperty("Count");
			PropertyInfo objKeysPropertyInfo = objType.GetProperty("Keys");
			PropertyInfo objItemPropertyInfo = objType.GetProperty("Item");
			MethodInfo objContainsKeyMethodInfo = objType.GetMethod("ContainsKey");
			Type keyType = objType.GetGenericArguments()[0];
			Type itemType = objType.GetGenericArguments()[1];

			// Get the key converter, that can serialize the key.
			RpcKeyValueSerializerConverter keyConverter = RpcKeyValueSerializerConverter.GetConverter(keyType, options);
			if (keyConverter == null) {
				throw new RpcKeyValueException(
					$"The dictionary '{keyType.Name}' key type, can not be converted.",
					RpcKeyValueExceptionType.Item
				);
			}

			// Goto.
			Object itemIndex = keyConverter.InternalDeserialize(key.ToString(), keyType, options);
			Boolean itemCount = (Boolean)objContainsKeyMethodInfo.Invoke(obj, new Object[] { itemIndex });
			if (itemCount == true) {
				if (remainingPath.Length == 0) {
					// Return.
					return new RpcKeyValueInfo(
						fullPath,
						key,
						itemType,
						() => objItemPropertyInfo.GetValue(obj, new Object[] { itemIndex }),
						(itemValue) => objItemPropertyInfo.SetValue(obj, itemValue, new Object[] { itemIndex })
					);
				} else {
					// Goto next key in the path.
					Object valueObject = objItemPropertyInfo.GetValue(obj, new Object[] { itemIndex });
					return RpcKeyValueSerializer.GotoValue(valueObject, remainingPath, fullPath, options);
				}
			}
		}

		//--------------------------------------------------------------------------------------------------------------
		// Enum without Flags attribute.
		//--------------------------------------------------------------------------------------------------------------
		if ((objType.IsEnum() == true) && (objType.IsEnumWithFlags() == false)) {
			if (remainingPath.Length == 0) {
				// Return.
				return new RpcKeyValueInfo(
					fullPath,
					key,
					objType,
					() => RpcKeyValueSerializerEnumExtensions.GetEnumUnderlayingValue((Enum)obj, options),
					(itemValue) => RpcKeyValueSerializerEnumExtensions.GetEnum(objType, itemValue)
				);
			} else {
				return null;
			}
		}

		//--------------------------------------------------------------------------------------------------------------
		// Enum with Flags attribute.
		//--------------------------------------------------------------------------------------------------------------
		if (objType.IsEnumWithFlags() == true) {
			if (remainingPath.Length == 0) {
				Object itemIndex = RpcKeyValueSerializerEnumExtensions.GetEnum(objType, key.ToString());

				// Return.
				return new RpcKeyValueInfo(
					fullPath,
					key,
					objType,
					() => {
						Enum objEnum = (Enum)obj;
						Enum keyEnum = (Enum)itemIndex;
						return objEnum.HasFlag(keyEnum);
					},
					(itemValue) => {
						if ((Boolean)itemValue == true) {
							obj = RpcKeyValueSerializerEnumExtensions.SetEnumFlag((Enum)obj, itemIndex);
						}
					}
				);
			} else {
				return null;
			}
		}

		//--------------------------------------------------------------------------------------------------------------
		// Object.
		//--------------------------------------------------------------------------------------------------------------
		FieldInfo fieldInfo = objType.GetField(key.ToString(), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
		if (fieldInfo != null) {
			if (remainingPath.Length == 0) {
				// Return.
				return new RpcKeyValueInfo(
					fullPath,
					key,
					fieldInfo.FieldType,
					() => fieldInfo.GetValue(obj),
					(itemValue) => fieldInfo.SetValue(obj, itemValue)
				);
			} else {
				// Goto next key in the path.
				Object valueObject = fieldInfo.GetValue(obj);
				return RpcKeyValueSerializer.GotoValue(valueObject, remainingPath, fullPath, options);
			}
		}

		PropertyInfo propertyInfo = objType.GetProperty(key.ToString(), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
		if (propertyInfo != null) {
			if (remainingPath.Length == 0) {
				// Return.
				return new RpcKeyValueInfo(
					fullPath,
					key,
					propertyInfo.PropertyType,
					() => propertyInfo.GetValue(obj),
					(itemValue) => propertyInfo.SetValue(obj, itemValue)
				);
			} else {
				// Goto next key in the path.
				Object valueObject = propertyInfo.GetValue(obj);
				return RpcKeyValueSerializer.GotoValue(valueObject, remainingPath, fullPath, options);
			}
		}

		// Can't goto missing item.
		return null;
	} // GotoValue

} // RpcKeyValueDeserializer
#endregion
