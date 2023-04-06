namespace RpcScandinavia.Core.Shared.KeyValueSerializer;
using System;
using System.Collections.Generic;

#region RpcKeyValueSerializerEnumHandler
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerEnumHandler.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// This key/value serializer handler, serializes and deserializes a enum with or without the flags attribute, to and
/// from one or more key/value items.
/// </summary>
public class RpcKeyValueSerializerEnumHandler : RpcKeyValueSerializerHandler {

	#region Overridden methods
	//------------------------------------------------------------------------------------------------------------------
	// Overridden methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <inheritdoc />
	public override Boolean CanHandle(RpcMemberInfo memberInfo, RpcKeyValueSerializerOptions options) {
		return (memberInfo.IsEnum == true);
	} // CanHandle

	/// <inheritdoc />
	public override void Serialize<KeyValueType>(RpcMemberInfo memberInfo, Object obj, String keyPrefix, RpcKeyValueBuilder<KeyValueType> keyValueBuilder, Int32 level, RpcKeyValueSerializerOptions options) {
		// Validate.
		RpcKeyValueException.ValidateLevel(level, options);
		RpcKeyValueException.ValidateIsAssignableFrom(memberInfo, obj);
		RpcKeyValueException.ValidateIsEnum(memberInfo);

		// Get the type.
		Type objType = obj.GetType();

		if (memberInfo.IsEnumWithFlags == false) {
			// Enum without the flags attribute.
			// Serialize the item.
			this.Serialize<KeyValueType>(
				-1,
				obj,
				memberInfo,
				obj,
				keyPrefix,
				keyValueBuilder,
				level,
				options
			);
		} else {
			// Enum with the flags attribute.
			// Serialize the items, which is the values in the enum.
			Int32 itemIndex = -1;
			foreach (Object itemEnum in RpcKeyValueSerializerEnumHandler.GetEnumFlagValues(memberInfo.Type, obj)) {
				// Get the current item.
				itemIndex++;
				Object itemObject = itemEnum;

				// Serialize the item.
				this.Serialize<KeyValueType>(
					itemIndex,
					itemObject,
					memberInfo,
					obj,
					keyPrefix,
					keyValueBuilder,
					level,
					options
				);
			}
		}
	} // Serialize

	private void Serialize<KeyValueType>(Int32 itemIndex, Object itemObject, RpcMemberInfo memberInfo, Object obj, String keyPrefix, RpcKeyValueBuilder<KeyValueType> keyValueBuilder, Int32 level, RpcKeyValueSerializerOptions options) {
		// Convert the item to either a integer or a string.
		switch (options.SerializeEnums) {
			case RpcKeyValueSerializerEnumOption.AsInteger:
				itemObject = ((Int32)itemObject).ToString();
				break;
			case RpcKeyValueSerializerEnumOption.AsString:
				itemObject = Enum.GetName(memberInfo.Type, itemObject).NotNull();
				break;
		}

		// Get the member information for the current item.
		RpcMemberInfo member = new RpcMemberInfoType(
			itemObject.GetType(),
			(itemIndex > -1) ? $"{itemIndex}" : String.Empty,
			((obj) => itemObject),
			null
		);

		// Serialize the item.
		base.SerializeMember<KeyValueType>(
			member,
			obj,
			false,
			keyPrefix,
			keyValueBuilder,
			level,
			options
		);
	} // Serialize

	/// <inheritdoc />
	public override Object Deserialize<KeyValueType>(RpcMemberInfo memberInfo, Object obj, String keyPrefix, RpcKeyValueProvider<KeyValueType> keyValueProvider, Int32 level, RpcKeyValueSerializerOptions options) {
		// Validate.
		RpcKeyValueException.ValidateLevel(level, options);
		RpcKeyValueException.ValidateIsAssignableFrom(memberInfo, obj);
		RpcKeyValueException.ValidateIsEnum(memberInfo);

		// Get the type.
		Type objType = obj.GetType();

		if (memberInfo.IsEnumWithFlags == false) {
			// Enum without the flags attribute.
			// Get the value.
			Object itemValue = keyValueProvider.GetValue(keyPrefix, String.Empty);

			// Convert the item from a string.
			Int32 itemValueNumeric = this.ToInt32OrDefault((String)itemValue);
			if (itemValueNumeric > -1) {
				itemValue = Enum.ToObject(memberInfo.Type, itemValueNumeric);
			} else {
				itemValue = Enum.Parse(memberInfo.Type, (String)itemValue);
			}

			obj = itemValue;
		} else {
			// Enum with the flags attribute.
			// Clear the items collection.
			obj = Enum.ToObject(memberInfo.Type, 0);

			// Deserialize each item.
			foreach (String itemKey in keyValueProvider.GetKeys(keyPrefix)) {
				try {
					// Get the value.
					Object itemValue = keyValueProvider.GetValue(keyPrefix, itemKey);

					// Convert the item from a string.
					Int32 itemValueNumeric = this.ToInt32OrDefault((String)itemValue);
					if (itemValueNumeric > -1) {
						itemValue = Enum.ToObject(memberInfo.Type, itemValueNumeric);
					} else {
						itemValue = Enum.Parse(memberInfo.Type, (String)itemValue);
					}

					// Set the flag.
					RpcKeyValueSerializerEnumHandler.SetEnumFlag(memberInfo.Type, ref obj, itemValue);
				} catch (RpcKeyValueException exception) {
					// Throw the exception.
					exception.Throw(options);
				}
			}
		}

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

	/// <summary>
	/// Enumerates through all set values, of a enum type with the flags attribute.
	/// </summary>
	/// <param name="enumType">The enum type.</param>
	/// <param name="enumValue">The enum value.</param>
	private static IEnumerable<Object> GetEnumFlagValues(Type enumType, Object enumValue) {
		foreach (Object obj in Enum.GetValues(enumType)) {
			if (((Int32)obj & (Int32)enumValue) != 0) {
				yield return obj;
			}
		}
	} // GetEnumFlagValues

	/// <summary>
	/// Set the specifig value on a enum type with the flags attribute.
	/// </summary>
	/// <param name="enumType">The enum type.</param>
	/// <param name="enumValue">The enum value.</param>
	/// <param name="enumFlag">The value that should be set.</param>
	private static void SetEnumFlag(Type enumType, ref Object enumValue, Object enumFlag) {
		// Int64 can hold all possible values, except those which UInt64 can hold.
		if (Enum.GetUnderlyingType(enumType) == typeof(UInt64)) {
			UInt64 numericValue = Convert.ToUInt64(enumValue);// (UInt64)enumValue);
			numericValue |= Convert.ToUInt64(enumFlag);
			enumValue = Enum.ToObject(enumType, numericValue);
		} else {
			Int64 numericValue = Convert.ToInt64(enumValue);
			numericValue |= Convert.ToInt64(enumFlag);
			enumValue = Enum.ToObject(enumType, numericValue);
		}
	} // SetEnumFlag
	#endregion

} // RpcKeyValueSerializerEnumHandler
#endregion
