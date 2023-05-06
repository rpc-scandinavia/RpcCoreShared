namespace RpcScandinavia.Core.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json.Serialization;
using RpcScandinavia.Core;

#region RpcKeyValueSerializerEnumExtensions
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerEnumExtensions.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Extension methods for the <see cref="System.Enum" /> class used by the RPC Key/Value serializer.
/// </summary>
public static class RpcKeyValueSerializerEnumExtensions {

	/// <summary>
	/// Gets the value of the enum.
	/// The value is either the numeric value or the string value, accordingly to the options.
	/// </summary>
	/// <param name="enumValue">The enum value.</param>
	/// <param name="options">The serializer options.</param>
	/// <returns>The enum value as a number or a string.</returns>
	public static Object GetEnumUnderlayingValue(this Enum enumValue, RpcKeyValueSerializerOptions options) {
		// Validate.
		if (enumValue == null) {
			throw new NullReferenceException(nameof(enumValue));
		}
		if (options == null) {
			throw new NullReferenceException(nameof(options));
		}

		switch (options.SerializeEnums) {
			case RpcKeyValueSerializerEnumOption.AsString:
				return Enum.GetName(enumValue.GetType(), enumValue).NotNull();
			case RpcKeyValueSerializerEnumOption.AsInteger:
			default:
				// Int64 can hold all possible values, except those which UInt64 can hold.
				if (Enum.GetUnderlyingType(enumValue.GetType()) == typeof(UInt64)) {
					return Convert.ToUInt64(enumValue);
				} else {
					return Convert.ToInt64(enumValue);
				}
		}
	} // GetEnumUnderlayingValue

	/// <summary>
	/// Parses the enum value to a enum type.
	/// If the value is a number, either number type or string with number, then it is used to create the enum.
	/// Otherwise the "ToString" on the enum value is parsed.
	/// </summary>
	/// <param name="enumType">The enum type.</param>
	/// <param name="enumValue">The enum value.</param>
	/// <returns>The enum either with the enum value or zero.</returns>
	public static Enum GetEnum(this Type enumType, Object enumValue) {
		// Validate.
		if (enumType == null) {
			throw new NullReferenceException(nameof(enumType));
		}
		if (enumType.IsEnum() == false) {
			throw new ArgumentException($"The '{nameof(enumType)}' type is not a enum.");
		}
		if (enumValue == null) {
			throw new NullReferenceException(nameof(enumValue));
		}

		try {
			// Int64 can hold all possible values, except those which UInt64 can hold.
			if (Enum.GetUnderlyingType(enumType) == typeof(UInt64)) {
				UInt64 enumNumber = 0;
				if (UInt64.TryParse(enumValue.ToString(), out enumNumber) == true) {
					return (Enum)Enum.ToObject(enumType, enumNumber);
				} else {
					return (Enum)Enum.Parse(enumType, enumValue.ToString());
				}


			} else {
				Int64 enumNumber = -1;
				if (Int64.TryParse(enumValue.ToString(), out enumNumber) == true) {
					return (Enum)Enum.ToObject(enumType, enumNumber);
				} else {
					return (Enum)Enum.Parse(enumType, enumValue.ToString());
				}
			}
		} catch {
			return (Enum)Enum.ToObject(enumType, 0);
		}
	} // GetEnum

	/// <summary>
	/// Enumerates through all set values, of a enum type with the flags attribute.
	/// </summary>
	/// <param name="enumValue">The enum value.</param>
	public static IEnumerable<Object> GetEnumFlagValues(this Enum enumValue) {
		// Validate.
		if (enumValue == null) {
			throw new NullReferenceException(nameof(enumValue));
		}
		if (enumValue.GetType().IsEnumWithFlags() == false) {
			throw new ArgumentException($"The '{nameof(enumValue)}' type is not a enum with the flags attribute.");
		}

		// Int64 can hold all possible values, except those which UInt64 can hold.
		if (Enum.GetUnderlyingType(enumValue.GetType()) == typeof(UInt64)) {
			foreach (Object obj in Enum.GetValues(enumValue.GetType())) {
				if (((UInt64)obj & (UInt64)(Object)enumValue) != 0) {
					yield return obj;
				}
			}
		} else {
			foreach (Object obj in Enum.GetValues(enumValue.GetType())) {
				if (((Int64)obj & (Int64)(Object)enumValue) != 0) {
					yield return obj;
				}
			}
		}
	} // GetEnumFlagValues

	/// <summary>
	/// Set the enum flag on a enum type with the flags attribute.
	/// </summary>
	/// <param name="enumValue">The enum value.</param>
	/// <param name="enumFlag">The value that should be set.</param>
	public static Enum SetEnumFlag(this Enum enumValue, Object enumFlag) {
		// Validate.
		if (enumValue == null) {
			throw new NullReferenceException(nameof(enumValue));
		}
		if (enumValue.GetType().IsEnumWithFlags() == false) {
			throw new ArgumentException($"The '{nameof(enumValue)}' type is not a enum with the flags attribute.");
		}

		// Int64 can hold all possible values, except those which UInt64 can hold.
		if (Enum.GetUnderlyingType(enumValue.GetType()) == typeof(UInt64)) {
			UInt64 numericValue = Convert.ToUInt64(enumValue);// (UInt64)enumValue);
			numericValue |= Convert.ToUInt64(enumFlag);
			return (Enum)Enum.ToObject(enumValue.GetType(), numericValue);
		} else {
			Int64 numericValue = Convert.ToInt64(enumValue);
			numericValue |= Convert.ToInt64(enumFlag);
			return (Enum)Enum.ToObject(enumValue.GetType(), numericValue);
		}
	} // SetEnumFlag

} // RpcKeyValueSerializerEnumExtensions
#endregion
