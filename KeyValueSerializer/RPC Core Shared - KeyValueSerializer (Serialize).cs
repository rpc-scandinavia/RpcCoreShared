namespace RpcScandinavia.Core.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using RpcScandinavia.Core;

#region RpcKeyValueSerializer (Serialize)
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializer (Serialize).
//----------------------------------------------------------------------------------------------------------------------
public static partial class RpcKeyValueSerializer {

	private static void InternalSerialize(ReadOnlyMemory<Char> key, Object obj, IRpcKeyValueBuilder keyValueBuilder, Boolean metaDataRequired, Int32 level, RpcKeyValueSerializerOptions options) {
		// Validate.
		RpcKeyValueException.ValidateLevel(level, options);

		// Do not serialize null.
		if (obj == null) {
			return;
		}

		// Get the type.
		Type objType = obj?.GetType();

		// Add metadata.
		if ((options.SerializeTypeInfo == RpcKeyValueSerializerTypeInfoOption.Always) ||
			((options.SerializeTypeInfo == RpcKeyValueSerializerTypeInfoOption.RequiredAndTop) && (level == 0)) ||
			((options.SerializeTypeInfo == RpcKeyValueSerializerTypeInfoOption.RequiredAndTop) && (metaDataRequired == true)) ||
			((options.SerializeTypeInfo == RpcKeyValueSerializerTypeInfoOption.Required) && (metaDataRequired == true))) {
			keyValueBuilder.AddTypeMetadata(key, objType);
		}

		//--------------------------------------------------------------------------------------------------------------
		// Converter.
		//--------------------------------------------------------------------------------------------------------------
		RpcKeyValueSerializerConverter converter = RpcKeyValueSerializerConverter.GetConverter(objType, options);
		if (converter != null) {
			try {
				// Serialize.
				keyValueBuilder.Add(key, converter.InternalSerialize(obj, options));
			} catch (Exception exception) {
				throw new RpcKeyValueException($"Unable to serialize and convert value of type {objType.Name}: {exception.Message}", RpcKeyValueExceptionType.Item);
			}
			return;
		}

		//--------------------------------------------------------------------------------------------------------------
		// Array.
		//--------------------------------------------------------------------------------------------------------------
		if (objType.IsArray() == true) {
			PropertyInfo objLengthPropertyInfo = objType.GetProperty("Length");
			MethodInfo objGetItemMethodInfo = objType.GetMethod("Get");

			// Serialize each item.
			Type itemType = objType.GetElementType();
			Int32 itemCount = (Int32)objLengthPropertyInfo.GetValue(obj, null);
			for (Int32 itemIndex = 0; itemIndex < itemCount; itemIndex++) {
				try {
					// Serialize.
					Object valueObject = objGetItemMethodInfo.Invoke(obj, new Object[] { itemIndex });
					if (valueObject != null) {
						RpcKeyValueSerializer.InternalSerialize(
							itemIndex.ToString().AsMemory(),
							valueObject,
							keyValueBuilder.AddLevel(key),
							(itemType != valueObject.GetType()),
							level + 1,
							options
						);
					}
				} catch (Exception exception) {
					throw new RpcKeyValueException($"Unable to serialize array value of type {itemType.Name}: {exception.Message}", RpcKeyValueExceptionType.Item);
				}
			}
			return;
		}

		//--------------------------------------------------------------------------------------------------------------
		// Generic List.
		//--------------------------------------------------------------------------------------------------------------
		if (objType.IsGenericList() == true) {
			PropertyInfo objCountPropertyInfo = objType.GetProperty("Count");
			MethodInfo objGetItemMethodInfo = objType.GetMethod("get_Item");

			// Serialize each item.
			Type itemType = objType.GetGenericArguments().First();
			Int32 itemCount = (Int32)objCountPropertyInfo.GetValue(obj, null);
			for (Int32 itemIndex = 0; itemIndex < itemCount; itemIndex++) {
				try {
					// Serialize.
					Object valueObject = objGetItemMethodInfo.Invoke(obj, new Object[] { itemIndex });
					if (valueObject != null) {
						RpcKeyValueSerializer.InternalSerialize(
							itemIndex.ToString().AsMemory(),
							valueObject,
							keyValueBuilder.AddLevel(key),
							(itemType != valueObject.GetType()),
							level + 1,
							options
						);
					}
				} catch (Exception exception) {
					throw new RpcKeyValueException($"Unable to serialize list value of type {itemType.Name}: {exception.Message}", RpcKeyValueExceptionType.Item);
				}
			}
			return;
		}

		//--------------------------------------------------------------------------------------------------------------
		// Generic Dictionary.
		//--------------------------------------------------------------------------------------------------------------
		if (objType.IsGenericDictionary() == true) {
			PropertyInfo objKeysPropertyInfo = objType.GetProperty("Keys");
			PropertyInfo objItemPropertyInfo = objType.GetProperty("Item");
			Type keyType = objType.GetGenericArguments()[0];
			Type itemType = objType.GetGenericArguments()[1];

			// Get the key converter, that can serialize the key.
			RpcKeyValueSerializerConverter keyConverter = RpcKeyValueSerializerConverter.GetConverter(keyType, options);
			if (keyConverter == null) {
				throw new RpcKeyValueException(
					$"The dictionary '{keyType.Name}' key type, can not be serialized.",
					RpcKeyValueExceptionType.Item
				);
			}

			// Serialize each item.
			ICollection keys = (ICollection)objKeysPropertyInfo.GetValue(obj, null);
			foreach (Object itemIndex in keys) {
				try {
					// Serialize.
					Object valueObject = objItemPropertyInfo.GetValue(obj, new Object[] { itemIndex });
					if (valueObject != null) {
						RpcKeyValueSerializer.InternalSerialize(
							keyConverter.InternalSerialize(itemIndex, options),
							valueObject,
							keyValueBuilder.AddLevel(key),
							(itemType != valueObject.GetType()),
							level + 1,
							options
						);
					}
				} catch (Exception exception) {
					throw new RpcKeyValueException($"Unable to serialize dictionary value of type {itemType.Name}: {exception.Message}", RpcKeyValueExceptionType.Item);
				}
			}
			return;
		}

		//--------------------------------------------------------------------------------------------------------------
		// Enum with Flags attribute.
		//--------------------------------------------------------------------------------------------------------------
		if (objType.IsEnumWithFlags() == true) {
			Type itemType = objType;

			// Serialize each item.
			foreach (Enum keyEnum in Enum.GetValues(objType)) {
				try {
					Object itemIndex = null;
					switch (options.SerializeEnums) {
						case RpcKeyValueSerializerEnumOption.AsString:
							itemIndex = Enum.GetName(objType, keyEnum);
							break;
						case RpcKeyValueSerializerEnumOption.AsInteger:
							itemIndex = Convert.ChangeType(keyEnum, keyEnum.GetTypeCode());
							break;
					}

					if (itemIndex != null) {
						// Serialize.
						Object valueObject = ((Enum)obj).HasFlag(keyEnum);
						RpcKeyValueSerializer.InternalSerialize(
							itemIndex.ToString().AsMemory(),
							valueObject,
							keyValueBuilder.AddLevel(key),
							false,
							level + 1,
							options
						);
					}
				} catch (Exception exception) {
					throw new RpcKeyValueException($"Unable to serialize enum flag value of type {itemType.Name}: {exception.Message}", RpcKeyValueExceptionType.Item);
				}
			}
			return;
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
						(
							((fieldInfo.GetMemberIsPublic() == true) && (options.IncludePublicFields == true)) ||
							((fieldInfo.GetMemberIsPrivate() == true) && (options.IncludePrivateFields == true))
						)
					) {
						// Serialize.
						Object valueObject = fieldInfo.GetValue(obj);
						if (valueObject != null) {
							RpcKeyValueSerializer.InternalSerialize(
								fieldInfo.GetSerializerName().AsMemory(),
								valueObject,
								keyValueBuilder.AddLevel(key),
								(fieldInfo.FieldType.Equals(valueObject.GetType()) == false),
								level + 1,
								options
							);
						}
					}
				} catch (Exception exception) {
					throw new RpcKeyValueException($"Unable to serialize field '{fieldInfo.Name}' of type {fieldInfo.FieldType.Name}: {exception.Message}", RpcKeyValueExceptionType.Item);
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
						(
							((propertyInfo.GetMemberIsPublicSet() == true) && (options.IncludePublicProperties == true)) ||
							((propertyInfo.GetMemberIsPrivateSet() == true) && (options.IncludePrivateProperties == true))
						)
					) {
						// Serialize.
						Object valueObject = propertyInfo.GetValue(obj);
						if (valueObject != null) {
							RpcKeyValueSerializer.InternalSerialize(
								propertyInfo.GetSerializerName().AsMemory(),
								valueObject,
								keyValueBuilder.AddLevel(key),
								(propertyInfo.PropertyType.Equals(valueObject.GetType()) == false),
								level + 1,
								options
							);
						}
					}
				} catch (Exception exception) {
					throw new RpcKeyValueException($"Unable to serialize property '{propertyInfo.Name}' of type {propertyInfo.PropertyType.Name}: {exception.Message}", RpcKeyValueExceptionType.Item);
				}
			}
		}
	} // InternalSerialize

} // RpcKeyValueDeserializer
#endregion
