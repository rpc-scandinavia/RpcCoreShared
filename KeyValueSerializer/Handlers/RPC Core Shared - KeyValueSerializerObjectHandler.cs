namespace RpcScandinavia.Core.Shared.KeyValueSerializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

#region RpcKeyValueSerializerObjectHandler
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerObjectHandler.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// RPC Key/Value serializer handler, that serializes and deserializes any object to and from one or more key/value objects.
/// </summary>
public class RpcKeyValueSerializerObjectHandler : RpcKeyValueSerializerHandler {

	#region Overridden methods
	//------------------------------------------------------------------------------------------------------------------
	// Overridden methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <inheritdoc />
	public override Boolean CanHandle(Type type, RpcKeyValueSerializerOptions options) {
		return true;
	} // CanHandle

	/// <inheritdoc />
	public override void Serialize<KeyValueType>(Object obj, List<KeyValueType> values, String keyPrefix, Func<String, String, KeyValueType> createKeyValueInstance, RpcKeyValueSerializerOptions options) {
		// Get the type.
		Type type = obj.GetType();

		// Get the member information of the type.
		List<RpcMemberInfo> members = new List<RpcMemberInfo>();

		foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
			members.Add(new RpcMemberInfoField(fieldInfo));
		}

		foreach (PropertyInfo propertyInfo in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
			members.Add(new RpcMemberInfoProperty(propertyInfo));
		}

		// Serialize meta data.
		if (options.SerializeTypeInfo == true) {
			String key = $"{keyPrefix}{options.HierarchySeparatorChar}$Type".Trim(options.HierarchySeparatorChar);
			values.Add(
				createKeyValueInstance(
					key,
					type.AssemblyQualifiedName
				)
			);
		}

		// Serialize each member.
		foreach (RpcMemberInfo member in members) {
			if (member.ShouldSerialize(options) == true) {
				try {
					Boolean serialized = false;
					String key = $"{keyPrefix}{options.HierarchySeparatorChar}{member.Name}".Trim(options.HierarchySeparatorChar);

					// Serialize using a converter.
					if (serialized == false) {
						RpcKeyValueSerializerConverter converter = RpcKeyValueSerializerConverter.GetConverter(member.Type, options);
						if (converter != null) {
							values.Add(
								createKeyValueInstance(
									key,
									converter.InternalSerialize(member.GetValue(obj), options)
								)
							);
							serialized = true;
						}
					}

					// Serialize using a handler.
					if (serialized == false) {
						RpcKeyValueSerializerHandler handler = RpcKeyValueSerializerHandler.GetHandler(member.Type, options);
						if (handler is not RpcKeyValueSerializerObjectHandler) {
							handler.Serialize<KeyValueType>(member.GetValue(obj), values, key, createKeyValueInstance, options);
							serialized = true;
						}
					}

					// Undble to serialize the member.
					throw new Exception($"Unable to serialize member '{member.Name}' of type {member.Type.Name}.");
				} catch {
					// Throw the exception.
					if (options.SerializeThrowExceptions.HasFlag(RpcKeyValueSerializerExceptionOption.ThrowItemExceptions) == true) {
						throw;
					}
				}
			}
		}
	} // Serialize

	/// <inheritdoc />
	public override Object Deserialize<KeyValueType>(Object obj, IEnumerable<KeyValueType> values, String keyPrefix, Func<KeyValueType, String> getKey, Func<KeyValueType, String> getValue, RpcKeyValueSerializerOptions options) {
		// If the object is null, then create a new instance of the object, of the type taken from the
		// meta data "$Type" value, or throw an exception if that does not exist.
		if (options.DeserializeTypeInfo == true) {
			String key = $"{keyPrefix}{options.HierarchySeparatorChar}$Type".Trim(options.HierarchySeparatorChar);
			String typeName = values
				.Where((keyValue) => getKey(keyValue).Equals(key))
				.Select((keyValue) => getValue(keyValue))
				.FirstOrDefault();
			if (typeName.IsNullOrWhiteSpace() == false) {
				Type newType = Type.GetType(typeName, false);
				if (newType != null) {
					Object newObject = Activator.CreateInstance(newType);
					if (newObject != null) {
						obj = newObject;
					}
				}
			}
		}

		// Get the type.
		Type type = obj.GetType();

		// Get the member information of the type.
		List<RpcMemberInfo> members = new List<RpcMemberInfo>();

		foreach (FieldInfo fieldInfo in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
			members.Add(new RpcMemberInfoField(fieldInfo));
		}

		foreach (PropertyInfo propertyInfo in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
			members.Add(new RpcMemberInfoProperty(propertyInfo));
		}

		// Serialize each member.
		foreach (RpcMemberInfo member in members) {
			if (member.ShouldSerialize(options) == true) {
				try {
					String key = $"{keyPrefix}{options.HierarchySeparatorChar}{member.Name}".Trim(options.HierarchySeparatorChar);
					String value = values
						.Where((keyValue) => getKey(keyValue).Equals(key))
						.Select((keyValue) => getValue(keyValue))
						.FirstOrDefault();

					// Serialize using a converter.
					if (value != null) {
						RpcKeyValueSerializerConverter converter = RpcKeyValueSerializerConverter.GetConverter(member.Type, options);
						if (converter != null) {
							member.SetValue(
								obj,
								converter.InternalDeserialize(value, options)
							);
							value = null;
						}
					}

					// Serialize using a handler.
					if (value != null) {
						RpcKeyValueSerializerHandler handler = RpcKeyValueSerializerHandler.GetHandler(member.Type, options);
						if (handler is not RpcKeyValueSerializerObjectHandler) {
							Object valueObject = member.GetValue(obj);
							member.SetValue(
								obj,
								handler.Deserialize<KeyValueType>(
									valueObject,
									values,
									keyPrefix,
									getKey,
									getValue,
									options
								)
							);
							value = null;
						}
					}

					// Undble to deserialize the member.
					throw new Exception($"Unable to deserialize member '{member.Name}' of type {member.Type.Name}.");
				} catch {
					// Throw the exception.
					if (options.SerializeThrowExceptions.HasFlag(RpcKeyValueSerializerExceptionOption.ThrowItemExceptions) == true) {
						throw;
					}
				}
			}
		}

		// Return the object.
		return obj;
	} // Deserialize
	#endregion

} // RpcKeyValueSerializerObjectHandler
#endregion
