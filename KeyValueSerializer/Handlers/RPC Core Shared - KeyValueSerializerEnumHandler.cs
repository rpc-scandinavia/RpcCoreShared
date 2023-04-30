namespace RpcScandinavia.Core.Shared;
using System;
using System.Collections.Generic;

#region RpcKeyValueSerializerEnumHandler
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerEnumHandler.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// This key/value serializer handler, serializes and deserializes a enum without the flags attribute, to and
/// from one or more key/value items.
/// </summary>
public class RpcKeyValueSerializerEnumHandler : RpcKeyValueSerializerHandler {

	#region Overridden methods
	//------------------------------------------------------------------------------------------------------------------
	// Overridden methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <inheritdoc />
	public override Boolean CanHandle(RpcMemberInfo memberInfo, RpcKeyValueSerializerOptions options) {
		return ((memberInfo.IsEnum == true) && (memberInfo.IsEnumWithFlags == false));
	} // CanHandle

	/// <inheritdoc />
	public override void Serialize<KeyValueType>(RpcMemberInfo memberInfo, Object obj, String keyPath, RpcKeyValueBuilder<KeyValueType> keyValueBuilder, Int32 level, RpcKeyValueSerializerOptions options) {
		// Validate.
		RpcKeyValueException.ValidateLevel(level, options);
		RpcKeyValueException.ValidateIsAssignableFrom(memberInfo, obj);
		RpcKeyValueException.ValidateIsEnum(memberInfo);

		// Get the type.
		Type objType = obj.GetType();

		// Convert the item to either a integer or a string.
		switch (options.SerializeEnums) {
			case RpcKeyValueSerializerEnumOption.AsInteger:
				obj = Convert.ToInt64(obj).ToString();	//((Int32)obj).ToString();
				break;
			case RpcKeyValueSerializerEnumOption.AsString:
				obj = Enum.GetName(memberInfo.Type, obj).NotNull();
				break;
		}

		// Get the member information for the current item.
		RpcMemberInfo member = new RpcMemberInfoType(
			obj.GetType(),
			String.Empty,
			((obj, tag) => obj),
			null
		);

		// Serialize the item.
		base.SerializeMember<KeyValueType>(
			member,
			obj,
			false,
			keyPath,
			keyValueBuilder,
			level,
			options
		);
	} // Serialize

	/// <inheritdoc />
	public override Object Deserialize<KeyValueType>(RpcMemberInfo memberInfo, Object obj, String keyPath, RpcKeyValueProvider<KeyValueType> keyValueProvider, Int32 level, RpcKeyValueSerializerOptions options) {
		// Validate.
		RpcKeyValueException.ValidateLevel(level, options);
		RpcKeyValueException.ValidateIsAssignableFrom(memberInfo, obj);
		RpcKeyValueException.ValidateIsEnum(memberInfo);

		// Get the type.
		Type objType = obj.GetType();

		// Get the value.
		Object itemValue = keyValueProvider.GetValue(keyPath, String.Empty);

		// Convert the item from a string.
		Int32 itemValueNumeric = this.ToInt32OrDefault((String)itemValue);
		if (itemValueNumeric > -1) {
			itemValue = Enum.ToObject(memberInfo.Type, itemValueNumeric);
		} else {
			itemValue = Enum.Parse(memberInfo.Type, (String)itemValue);
		}

		obj = itemValue;

		// Return the object.
		return obj;
	} // Deserialize
	#endregion

	#region Helper methods
	//------------------------------------------------------------------------------------------------------------------
	// Helper methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Converts the string into a Int32, or a Int32 with a value of "-1" when the string is not a valid number.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The number or -1.</returns>
	private Int32 ToInt32OrDefault(String value) {
		try {
			return Int32.Parse(value);
		} catch {
			return -1;
		}
	} // ToInt32OrDefault

	/// <inheritdoc />
	public override List<RpcMemberInfo> GetMemberInfos(Object obj, RpcKeyValueSerializerOptions options) {
		// Validate.
		if (obj == null) {
			throw new NullReferenceException(nameof(obj));
		}

		// Not implemented.
		throw new NotImplementedException($"Getting member informations from the enum handler is not supported.");
	} // GetMemberInfos
	#endregion

} // RpcKeyValueSerializerEnumHandler
#endregion
