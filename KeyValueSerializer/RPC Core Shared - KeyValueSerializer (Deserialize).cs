namespace RpcScandinavia.Core.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RpcScandinavia.Core;

#region RpcKeyValueSerializer (Deserialize)
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializer (Deserialize).
//----------------------------------------------------------------------------------------------------------------------
public static partial class RpcKeyValueSerializer {

	private static Object InternalDeserialize(Type type, Object obj, IRpcKeyValueProvider keyValueProvider, Int32 level, RpcKeyValueSerializerOptions options) {
		// Validate.
		RpcKeyValueException.ValidateLevel(level, options);

		// Get the type from the meta data.
		ReadOnlyMemory<Char> typeNameMeta = keyValueProvider.GetTypeMetadata();
		if (typeNameMeta.Length > 0) {
			RpcAssemblyQualifiedName typeName = new RpcAssemblyQualifiedName(typeNameMeta);
			if (typeName.IsEmpty == false) {
				type = typeName.Type ?? type;
			}
		}

		// Create the object.
		if (obj == null) {
			try {
				if ((type.IsAbstract == false) &&
					(type.IsPrimitive == false) &&
					(type.IsClass == true) &&
					(type != typeof(String)) &&
					(type != typeof(Object))) {
					obj = Activator.CreateInstance(type);
				}
			} catch {}
		}

		//--------------------------------------------------------------------------------------------------------------
		// Converter.
		//--------------------------------------------------------------------------------------------------------------
		// Get the converter that can deserialize the type.
		RpcKeyValueSerializerConverter converter = RpcKeyValueSerializerConverter.GetConverter(type, options);
		if (converter != null) {
			try {
				// Deserialize.
				return converter.InternalDeserialize(keyValueProvider.GetFirstKeyValueOrEmpty(), type, options);
			} catch (Exception exception) {
				throw new RpcKeyValueException($"Unable to deserialize and convert value of type {type.Name}: {exception.Message}", RpcKeyValueExceptionType.Item);
			}
		}

		//--------------------------------------------------------------------------------------------------------------
		// Array.
		//--------------------------------------------------------------------------------------------------------------
		if (type.IsArray() == true) {
			// Get the item type.
			Type itemType = type.GetElementType();

			// Clear the items collection.
			// Because this is an array, we need to create a list where the deserialized items can be added, and then
			// replace the array with a new array from the list.
			ArrayList items = new ArrayList();

			// Deserialize each item.
			foreach (IRpcKeyValueProvider key in keyValueProvider.GetKeys()) {
				try {
					Int32 keyNumber = -1;
					if ((Int32.TryParse(key.GetValue().Span, out keyNumber) == true) &&
						(key.Count() > 0)) {
						// Deserialize and add the item.
						items.Add(RpcKeyValueSerializer.InternalDeserialize(itemType, null, key, level + 1, options));
					}
				} catch (Exception exception) {
					throw new RpcKeyValueException($"Unable to deserialize array value of type {type.Name}: {exception.Message}", RpcKeyValueExceptionType.Item);
				}
			}

			// Because this is an array, we need to create a new array with the precise number of items, and then copy
			// the items from the temporary array list to the new array.
			// Create a new instance of the array.
			obj = items.ToArray(itemType);

			// Return the object.
			return obj;
		}

		//--------------------------------------------------------------------------------------------------------------
		// Generic List.
		//--------------------------------------------------------------------------------------------------------------
		if (type.IsGenericList() == true) {
			// Get the Clear and Add methods.
			MethodInfo objClearMethodInfo = type.GetMethod("Clear");
			MethodInfo objAddMethodInfo = type.GetMethod("Add");

			// Get the item type.
			Type itemType = type.GetGenericArguments().First();

			// Clear the items collection.
			objClearMethodInfo.Invoke(obj, Array.Empty<Object>());

			// Deserialize each item.
			foreach (IRpcKeyValueProvider key in keyValueProvider.GetKeys()) {
				try {
					Int32 keyNumber = -1;
					if ((Int32.TryParse(key.GetValue().Span, out keyNumber) == true) &&
						(key.Count() > 0)) {
						// Deserialize and add the item.
						objAddMethodInfo.Invoke(
							obj,
							new Object[] { RpcKeyValueSerializer.InternalDeserialize(itemType, null, key, level + 1, options) }
						);
					}
				} catch (Exception exception) {
					throw new RpcKeyValueException($"Unable to deserialize list value of type {type.Name}: {exception.Message}", RpcKeyValueExceptionType.Item);
				}
			}

			// Return the object.
			return obj;
		}

		//--------------------------------------------------------------------------------------------------------------
		// Generic Dictionary.
		//--------------------------------------------------------------------------------------------------------------
		if (type.IsGenericDictionary() == true) {
			// Get the Clear and Add methods.
			MethodInfo objClearMethodInfo = type.GetMethod("Clear");
			MethodInfo objAddMethodInfo = type.GetMethod("Add");

			// Get the key type and the item type.
			Type keyType = type.GetGenericArguments()[0];
			Type itemType = type.GetGenericArguments()[1];

			// Get the key converter, that can deserialize the key.
			RpcKeyValueSerializerConverter keyConverter = RpcKeyValueSerializerConverter.GetConverter(keyType, options);
			if (keyConverter == null) {
				throw new RpcKeyValueException(
					$"The dictionary '{keyType.Name}' key type, can not be deserialized.",
					RpcKeyValueExceptionType.Item
				);
			}

			// Clear the items collection.
			objClearMethodInfo.Invoke(obj, Array.Empty<Object>());

			// Deserialize each item.
			foreach (IRpcKeyValueProvider key in keyValueProvider.GetKeys()) {
				try {
					if (key.Count() > 0) {
						// Deserialize the key.
						Object keyObject = keyConverter.InternalDeserialize(key.GetValue(), keyType, options);

						// Deserialize and add the item.
						objAddMethodInfo.Invoke(
							obj,
							new Object[] { keyObject, RpcKeyValueSerializer.InternalDeserialize(itemType, null, key, level + 1, options) }
						);
					}
				} catch (Exception exception) {
					throw new RpcKeyValueException($"Unable to deserialize dictionary value of type {type.Name}: {exception.Message}", RpcKeyValueExceptionType.Item);
				}
			}

			// Return the object.
			return obj;
		}

		//--------------------------------------------------------------------------------------------------------------
		// Enum with Flags attribute.
		//--------------------------------------------------------------------------------------------------------------
		if (type.IsEnumWithFlags() == true) {
			// Clear the items collection.
			obj = Enum.ToObject(type, 0);

			// Deserialize each item.
			foreach (IRpcKeyValueProvider key in keyValueProvider.GetKeys()) {
				try {
					// Get the value.
					// Notice we have to get the value from the first key, because we do not recursively call
					// "this.Deserialize" with the key.
					String keyString = key.GetFirstKeyValueOrEmpty().Span.ToString();

					// Convert the item from a string.
					// The previous key/value serializer serialized enums with flags like this:
					//	Tag:EnumB:0 == 2
					//	Tag:EnumB:1 == 8
					// And the current key/value serializer serializes enums with flags like this:
					//	Tag:EnumB:0 == True
					//	Tag:EnumB:1 == False
					//	Tag:EnumB:2 == True
					//	Tag:EnumB:4 == False
					//	Tag:EnumB:8 == True
					//	Tag:EnumB:16 == False
					if (keyString.ToLower() == "true") {
						// New serialized data.
						Int64 keyNumber = -1;
						if (Int64.TryParse(key.GetValue().Span, out keyNumber) == false) {
							keyNumber = -1;
						}

						// Set the flag.
						if (keyNumber > -1) {
							// Set the flag.
							obj = RpcKeyValueSerializerEnumExtensions.SetEnumFlag((Enum)obj, Enum.ToObject(type, keyNumber));
						} else {
							// Set the flag.
							obj = RpcKeyValueSerializerEnumExtensions.SetEnumFlag((Enum)obj, Enum.Parse(type, keyString));
						}
					} else if (keyString.ToLower() == "false") {
						// New serialized data.
						// Do not set the flag.
					} else {
						// Old serialized data.
						Int64 keyNumber = -1;
						if (Int64.TryParse(keyString, out keyNumber) == false) {
							keyNumber = -1;
						}

						if (keyNumber > -1) {
							// Set the flag.
							obj = RpcKeyValueSerializerEnumExtensions.SetEnumFlag((Enum)obj, Enum.ToObject(type, keyNumber));
						} else {
							// Set the flag.
							obj = RpcKeyValueSerializerEnumExtensions.SetEnumFlag((Enum)obj, Enum.Parse(type, keyString));
						}
					}
				} catch (Exception exception) {
					throw new RpcKeyValueException($"Unable to deserialize enum flag value of type {type.Name}: {exception.Message}", RpcKeyValueExceptionType.Item);
				}
			}

			// Return the object.
			return obj;
		}

		//--------------------------------------------------------------------------------------------------------------
		// Object.
		//--------------------------------------------------------------------------------------------------------------
		if ((options.IncludePublicFields == true) || (options.IncludePrivateFields == true)) {
			RpcSimpleStaticCache<String, Type, IEnumerable<FieldInfo>> fieldInfoCache = new RpcSimpleStaticCache<String, Type, IEnumerable<FieldInfo>>(
				RpcKeyValueSerializer.CacheIsolation,
				(type) => type
					.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
					.Where((fieldInfo) => (fieldInfo.Name.Contains("k__BackingField") == false))
			);

			foreach (FieldInfo fieldInfo in fieldInfoCache.GetValue(obj.GetType())) {
				try {
					if ((fieldInfo.GetSerializerIgnore() == false) &&
//						(fieldInfo.GetSerializerInclude() == true) &&
						(fieldInfo.GetMemberIsReadOnly() == false) &&
						(
							((fieldInfo.GetMemberIsPublic() == true) && (options.IncludePublicFields == true)) ||
							((fieldInfo.GetMemberIsPrivate() == true) && (options.IncludePrivateFields == true))
						) &&
						(keyValueProvider.ContainsKey(fieldInfo.GetSerializerName().AsMemory()) == true)
					) {
						// Deserialize and set the field.
						fieldInfo.SetValue(
							obj,
							RpcKeyValueSerializer.InternalDeserialize(fieldInfo.FieldType, null, keyValueProvider.GetKey(fieldInfo.GetSerializerName().AsMemory()), level + 1, options)
						);
					}
				} catch (Exception exception) {
					throw new RpcKeyValueException($"Unable to deserialize field '{fieldInfo.Name}' of type {fieldInfo.FieldType.Name}: {exception.Message}", RpcKeyValueExceptionType.Item);
				}
			}
		}

		if ((options.IncludePublicProperties == true) || (options.IncludePrivateProperties == true)) {
			RpcSimpleStaticCache<String, Type, IEnumerable<PropertyInfo>> propertyInfoCache = new RpcSimpleStaticCache<String, Type, IEnumerable<PropertyInfo>>(
				RpcKeyValueSerializer.CacheIsolation,
				(type) => type
					.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
			);

			foreach (PropertyInfo propertyInfo in propertyInfoCache.GetValue(obj.GetType())) {
				try {
					if ((propertyInfo.GetSerializerIgnore() == false) &&
//						(propertyInfo.GetSerializerInclude() == true) &&
						(propertyInfo.GetMemberIsReadOnly() == false) &&
						(
							((propertyInfo.GetMemberIsPublicSet() == true) && (options.IncludePublicProperties == true)) ||
							((propertyInfo.GetMemberIsPrivateSet() == true) && (options.IncludePrivateProperties == true))
						) &&
						(keyValueProvider.ContainsKey(propertyInfo.GetSerializerName().AsMemory()) == true)
					) {
						// Deserialize and set the property.
						propertyInfo.SetValue(
							obj,
							RpcKeyValueSerializer.InternalDeserialize(propertyInfo.PropertyType, null, keyValueProvider.GetKey(propertyInfo.GetSerializerName().AsMemory()), level + 1, options)
						);
					}
				} catch (Exception exception) {
					throw new RpcKeyValueException($"Unable to deserialize property '{propertyInfo.Name}' of type {propertyInfo.PropertyType.Name}: {exception.Message}", RpcKeyValueExceptionType.Item);
				}
			}
		}

		// Return the object.
		return obj;
	} // InternalDeserialize

} // RpcKeyValueDeserializer
#endregion
