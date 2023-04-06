namespace RpcScandinavia.Core.Shared.KeyValueSerializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#region RpcKeyValueSerializerObjectHandler
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerObjectHandler.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// This key/value serializer handler, serializes and deserializes any object of a specifig type, to and from one or
/// more key/value items.
/// </summary>
public class RpcKeyValueSerializerObjectHandler : RpcKeyValueSerializerHandler {

	#region Overridden methods
	//------------------------------------------------------------------------------------------------------------------
	// Overridden methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <inheritdoc />
	public override Boolean CanHandle(RpcMemberInfo memberInfo, RpcKeyValueSerializerOptions options) {
		// This is the default handler, and the top level handler.
		return true;
	} // CanHandle

	/// <inheritdoc />
	public override void Serialize<KeyValueType>(RpcMemberInfo memberInfo, Object obj, String keyPrefix, RpcKeyValueBuilder<KeyValueType> keyValueBuilder, Int32 level, RpcKeyValueSerializerOptions options) {
		// Validate.
		RpcKeyValueException.ValidateLevel(level, options);
		RpcKeyValueException.ValidateIsAssignableFrom(memberInfo, obj);

		// Get the type.
		Type objType = obj.GetType();

		// Special case, only on level zero (top level object).
		// Serialize meta data.
		if (((level == 0)) && (
			(
				(options.SerializeTypeInfo == RpcKeyValueSerializerTypeInfoOption.Always)
			) || (
				(options.SerializeTypeInfo == RpcKeyValueSerializerTypeInfoOption.RequiredAndTop)
			) || (
				(memberInfo.Type != objType) &&
				(options.SerializeTypeInfo == RpcKeyValueSerializerTypeInfoOption.Required)
			))) {
			keyValueBuilder.AddTypeMetadata(keyPrefix, String.Empty, objType);
		}

		// Get the items, which is the members of the type.
		List<RpcMemberInfo> items = new List<RpcMemberInfo>();
		foreach (FieldInfo fieldInfo in objType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
			items.Add(new RpcMemberInfoField(fieldInfo));
		}
		foreach (PropertyInfo propertyInfo in objType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
			items.Add(new RpcMemberInfoProperty(propertyInfo));
		}

		// Serialize each item.
		foreach (RpcMemberInfo item in items) {
			try {
				base.SerializeMember<KeyValueType>(
					item,
					obj,
					item.IsAbstract,
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
//Console.WriteLine($"OBJECT   '{memberInfo.Name}'   '{memberInfo.Type.Name}'   '{obj?.GetType().Name}'   '{keyValueProvider.GetCount(keyPrefix, memberInfo.Name)}'");
		RpcKeyValueException.ValidateLevel(level, options);
		RpcKeyValueException.ValidateIsAssignableFrom(memberInfo, obj);

		// Special case, only on level zero (top level object).
		// Create a new instance of the object, using the type taken from the meta data "$Type" value, or the member
		// information.
		if (level == 0) {
			obj = base.CreateInstance<KeyValueType>(memberInfo, keyPrefix, keyValueProvider, obj);
		}

		// Get the type.
		Type objType = obj.GetType();

		// Get the items, which is the members of the type.
		List<RpcMemberInfo> items = new List<RpcMemberInfo>();
		foreach (FieldInfo fieldInfo in objType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
			items.Add(new RpcMemberInfoField(fieldInfo));
		}
		foreach (PropertyInfo propertyInfo in objType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
			items.Add(new RpcMemberInfoProperty(propertyInfo));
		}

		// Deserialize each item.
		foreach (RpcMemberInfo item in items) {
			try {
				base.DeserializeMember<KeyValueType>(
					item,
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

} // RpcKeyValueSerializerObjectHandler
#endregion
