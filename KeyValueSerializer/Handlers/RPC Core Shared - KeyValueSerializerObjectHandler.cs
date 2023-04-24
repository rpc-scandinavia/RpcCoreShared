namespace RpcScandinavia.Core.Shared;
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
	public override void Serialize<KeyValueType>(RpcMemberInfo memberInfo, Object obj, String keyPath, RpcKeyValueBuilder<KeyValueType> keyValueBuilder, Int32 level, RpcKeyValueSerializerOptions options) {
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
			keyValueBuilder.AddTypeMetadata(keyPath, String.Empty, objType);
		}

		// Get the items, which is the members of the type.
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
	} // Serialize

	/// <inheritdoc />
	public override Object Deserialize<KeyValueType>(RpcMemberInfo memberInfo, Object obj, String keyPath, RpcKeyValueProvider<KeyValueType> keyValueProvider, Int32 level, RpcKeyValueSerializerOptions options) {
		// Validate.
		RpcKeyValueException.ValidateLevel(level, options);
		RpcKeyValueException.ValidateIsAssignableFrom(memberInfo, obj);

		// Special case, only on level zero (top level object).
		// Create a new instance of the object, using the type taken from the meta data "$Type" value, or the member
		// information.
		if (level == 0) {
			obj = base.CreateInstance<KeyValueType>(memberInfo, keyPath, keyValueProvider, obj);
		}

		// Get the type.
		Type objType = obj.GetType();

		// Get the items, which is the members of the type.
		List<RpcMemberInfo> items = GetMemberInfos(obj, options);

		// Deserialize each item.
		foreach (RpcMemberInfo item in items) {
			try {
				base.DeserializeMember<KeyValueType>(
					item,
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

		// Get the items, which is the members of the type.
		List<RpcMemberInfo> items = new List<RpcMemberInfo>();
		foreach (FieldInfo fieldInfo in obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
			items.Add(new RpcMemberInfoField(fieldInfo));
		}
		foreach (PropertyInfo propertyInfo in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)) {
			items.Add(new RpcMemberInfoProperty(propertyInfo));
		}

		return items;
	} // GetMemberInfos
	#endregion

} // RpcKeyValueSerializerObjectHandler
#endregion
