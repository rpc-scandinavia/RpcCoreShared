namespace RpcScandinavia.Core.Shared;
using System;
using System.Collections.Generic;

#region RpcKeyValueSerializerEnumWithFlagsHandler
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerEnumWithFlagsHandler.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// This key/value serializer handler, serializes and deserializes a enum with the flags attribute, to and
/// from one or more key/value items.
/// </summary>
public class RpcKeyValueSerializerEnumWithFlagsHandler : RpcKeyValueSerializerHandler {

	#region Overridden methods
	//------------------------------------------------------------------------------------------------------------------
	// Overridden methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <inheritdoc />
	public override Boolean CanHandle(RpcMemberInfo memberInfo, RpcKeyValueSerializerOptions options) {
		return ((memberInfo.IsEnum == true) && (memberInfo.IsEnumWithFlags == true));
	} // CanHandle

	/// <inheritdoc />
	public override void Serialize<KeyValueType>(RpcMemberInfo memberInfo, Object obj, String keyPath, RpcKeyValueBuilder<KeyValueType> keyValueBuilder, Int32 level, RpcKeyValueSerializerOptions options) {
		// Validate.
		RpcKeyValueException.ValidateLevel(level, options);
		RpcKeyValueException.ValidateIsAssignableFrom(memberInfo, obj);
		RpcKeyValueException.ValidateIsEnum(memberInfo);

		// Get the type.
		Type objType = obj.GetType();

		// Get the items, which is the elements in the list.
		List<RpcMemberInfo> items = this.GetMemberInfos(obj, options);

		// Serialize each item.
		foreach (RpcMemberInfo item in items) {
			try {
				base.SerializeMember<KeyValueType>(
					item,
					obj,
					false,
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

/*
		// Serialize the items, which is the values in the enum.
		Int32 itemIndex = -1;
		foreach (Object itemEnum in RpcKeyValueSerializerEnumWithFlagsHandler.GetEnumFlagValues(memberInfo.Type, obj)) {
			// Get the current item.
			itemIndex++;
			Object itemObject = itemEnum;

			// Serialize the item.
			this.Serialize<KeyValueType>(
				itemIndex,
				itemObject,
				memberInfo,
				obj,
				keyPath,
				keyValueBuilder,
				level,
				options
			);
		}
*/
	} // Serialize

	private void Serialize<KeyValueType>(Int32 itemIndex, Object itemObject, RpcMemberInfo memberInfo, Object obj, String keyPath, RpcKeyValueBuilder<KeyValueType> keyValueBuilder, Int32 level, RpcKeyValueSerializerOptions options) {
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
			((obj, tag) => itemObject),
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

		// Clear the items collection.
		obj = Enum.ToObject(memberInfo.Type, 0);

		// Deserialize each item.
		foreach (String itemKey in keyValueProvider.GetKeys(keyPath)) {
			try {
				// Get the value.
				String itemValue = keyValueProvider.GetValue(keyPath, itemKey);

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
				if (itemValue.ToLower() == "true") {
					// New serialized data.
					// Set the flag.
					Int32 itemKeyNumeric = this.ToInt32OrDefault((String)itemKey);
					if (itemKeyNumeric > -1) {
						// Set the flag.
						RpcKeyValueSerializerEnumWithFlagsHandler.SetEnumFlag(memberInfo.Type, ref obj, Enum.ToObject(memberInfo.Type, itemKeyNumeric));
					} else {
						// Set the flag.
						RpcKeyValueSerializerEnumWithFlagsHandler.SetEnumFlag(memberInfo.Type, ref obj, Enum.Parse(memberInfo.Type, (String)itemKey));
					}
				} else if (itemValue.ToLower() == "false") {
					// New serialized data.
					// Do not set the flag.
				} else {
					// Old serialized data.
					Int32 itemValueNumeric = this.ToInt32OrDefault((String)itemValue);
					if (itemValueNumeric > -1) {
						// Set the flag.
						RpcKeyValueSerializerEnumWithFlagsHandler.SetEnumFlag(memberInfo.Type, ref obj, Enum.ToObject(memberInfo.Type, itemValueNumeric));
					} else {
						// Set the flag.
						RpcKeyValueSerializerEnumWithFlagsHandler.SetEnumFlag(memberInfo.Type, ref obj, Enum.Parse(memberInfo.Type, (String)itemValue));
					}
				}
			} catch (RpcKeyValueException exception) {
				// Throw the exception.
				exception.Throw(options);
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

	/// <inheritdoc />
	public override List<RpcMemberInfo> GetMemberInfos(Object obj, RpcKeyValueSerializerOptions options) {
		// Validate.
		if (obj == null) {
			throw new NullReferenceException(nameof(obj));
		}

		// Get the type.
		Type objType = obj.GetType();

		// Get the items, which is the values in the enum.
		List<RpcMemberInfo> items = new List<RpcMemberInfo>();
		foreach (Enum keyEnum in Enum.GetValues(objType)) {
			Object key = null;
			switch (options.SerializeEnums) {
				case RpcKeyValueSerializerEnumOption.AsInteger:
					key = Convert.ChangeType(keyEnum, keyEnum.GetTypeCode());
					break;
				case RpcKeyValueSerializerEnumOption.AsString:
					key = Enum.GetName(objType, keyEnum);
					break;
			}

			items.Add(
				// Get the member information for the current item.
				new RpcMemberInfoType(
					typeof(Boolean),
					$"{key}",
					((obj, tag) => {										// Get indexth value.
						Enum objEnum = (Enum)obj;
						Enum keyEnum = (Enum)tag;
						if (objEnum.HasFlag(keyEnum) == true) {
							return true;
						} else {
							return false;
						}
					}),
					((obj, value, tag) => {									// Set indexth value.
						if ((Boolean)value == true) {
							RpcKeyValueSerializerEnumWithFlagsHandler.SetEnumFlag(objType, ref obj, tag);
						}
					}),
					keyEnum
				)
			);
		}

		return items;
	} // GetMemberInfos
	#endregion

} // RpcKeyValueSerializerEnumWithFlagsHandler
#endregion
