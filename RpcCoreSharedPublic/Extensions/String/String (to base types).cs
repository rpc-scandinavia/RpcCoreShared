using System;
using System.Globalization;
using System.Linq;
namespace RpcScandinavia.Core.Shared;

#region RpcCoreExtensions
//----------------------------------------------------------------------------------------------------------------------
// RpcCoreExtensions.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// This part of the class contains useful extension methods for the <see cref="System.String" /> class.
/// </summary>
public static partial class RpcCoreExtensions {

	#region ToBoolean
	//------------------------------------------------------------------------------------------------------------------
	// ToBoolean.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a <see cref="System.Boolean" />.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or false on exceptions.</returns>
	public static Boolean ToBoolean(this String value) {
		return value.ToParsable<Boolean>(false);
	} // ToBoolean

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a <see cref="System.Boolean" />.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	public static Boolean ToBoolean(this String value, Boolean defaultValue) {
		return value.ToParsable<Boolean>(defaultValue);
	} // ToBoolean

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a <see cref="System.Boolean" />.
	/// All strings concidered true, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered false, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or an exception if thrown.</returns>
	public static Boolean ToBooleanOrThrow(this String value) {
		return value.ToParsableOrThrow<Boolean>();
	} // ToBooleanOrThrow
	#endregion

	#region ToTriple
	//------------------------------------------------------------------------------------------------------------------
	// ToTriple.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a <see cref="RpcScandinavia.Core.Shared.Triple" />.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or Triple.Unknown on exceptions.</returns>
	public static Triple ToTriple(this String value) {
		return value.ToParsable<Triple>();
	} // ToTriple
	#endregion

	#region ToSByte
	//------------------------------------------------------------------------------------------------------------------
	// ToSByte.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.SByte.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or zero on exceptions.</returns>
	public static SByte ToSByte(this String value) {
		return value.ToParsable<SByte>(0);
	} // ToSByte

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.SByte.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	public static SByte ToSByte(this String value, SByte defaultValue) {
		return value.ToParsable<SByte>(defaultValue);
	} // ToSByte

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.SByte.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or an exception if thrown.</returns>
	public static SByte ToSByteOrThrow(this String value) {
		return value.ToParsableOrThrow<SByte>();
	} // ToSByteOrThrow
	#endregion

	#region ToInt16
	//------------------------------------------------------------------------------------------------------------------
	// ToInt16.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.Int16.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or zero on exceptions.</returns>
	public static Int16 ToInt16(this String value) {
		return value.ToParsable<Int16>(0);
	} // ToInt16

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.Int16.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	public static Int16 ToInt16(this String value, Int16 defaultValue) {
		return value.ToParsable<Int16>(defaultValue);
	} // ToInt16

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.Int16.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or an exception if thrown.</returns>
	public static Int16 ToInt16OrThrow(this String value) {
		return value.ToParsableOrThrow<Int16>();
	} // ToInt16OrThrow
	#endregion

	#region ToInt32
	//------------------------------------------------------------------------------------------------------------------
	// ToInt32.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.Int32.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or zero on exceptions.</returns>
	public static Int32 ToInt32(this String value) {
		return value.ToParsable<Int32>(0);
	} // ToInt32

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.Int32.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	public static Int32 ToInt32(this String value, Int32 defaultValue) {
		return value.ToParsable<Int32>(defaultValue);
	} // ToInt32

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.Int32.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or an exception if thrown.</returns>
	public static Int32 ToInt32OrThrow(this String value) {
		return value.ToParsableOrThrow<Int32>();
	} // ToInt32OrThrow
	#endregion

	#region ToInt64
	//------------------------------------------------------------------------------------------------------------------
	// ToInt64.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.Int64.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or zero on exceptions.</returns>
	public static Int64 ToInt64(this String value) {
		return value.ToParsable<Int64>(0);
	} // ToInt64

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.Int64.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	public static Int64 ToInt64(this String value, Int64 defaultValue) {
		return value.ToParsable<Int64>(0);
	} // ToInt64

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.Int64.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or an exception if thrown.</returns>
	public static Int64 ToInt64OrThrow(this String value) {
		return value.ToParsableOrThrow<Int64>();
	} // ToInt64OrThrow
	#endregion

	#region ToByte
	//------------------------------------------------------------------------------------------------------------------
	// ToByte.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.Byte.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or zero on exceptions.</returns>
	public static Byte ToByte(this String value) {
		return value.ToParsable<Byte>(0);
	} // ToByte

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.Byte.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	public static Byte ToByte(this String value, Byte defaultValue) {
		return value.ToParsable<Byte>(defaultValue);
	} // ToByte

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.Byte.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or an exception if thrown.</returns>
	public static Byte ToByteOrThrow(this String value) {
		return value.ToParsableOrThrow<Byte>();
	} // ToByteOrThrow
	#endregion

	#region ToUInt16
	//------------------------------------------------------------------------------------------------------------------
	// ToUInt16.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.UInt16.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or zero on exceptions.</returns>
	public static UInt16 ToUInt16(this String value) {
		return value.ToParsable<UInt16>(0);
	} // ToUInt16

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.UInt16.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	public static UInt16 ToUInt16(this String value, UInt16 defaultValue) {
		return value.ToParsable<UInt16>(defaultValue);
	} // ToUInt16

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.UInt16.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or an exception if thrown.</returns>
	public static UInt16 ToUInt16OrThrow(this String value) {
		return value.ToParsableOrThrow<UInt16>();
	} // ToUInt16OrThrow
	#endregion

	#region ToUInt32
	//------------------------------------------------------------------------------------------------------------------
	// ToUInt32.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.UInt32.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or zero on exceptions.</returns>
	public static UInt32 ToUInt32(this String value) {
		return value.ToParsable<UInt32>(0);
	} // ToUInt32

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.UInt32.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	public static UInt32 ToUInt32(this String value, UInt32 defaultValue) {
		return value.ToParsable<UInt32>(defaultValue);
	} // ToUInt32

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.UInt32.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or an exception if thrown.</returns>
	public static UInt32 ToUInt32OrThrow(this String value) {
		return value.ToParsableOrThrow<UInt32>();
	} // ToUInt32OrThrow
	#endregion

	#region ToUInt64
	//------------------------------------------------------------------------------------------------------------------
	// ToUInt64.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.UInt64.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or zero on exceptions.</returns>
	public static UInt64 ToUInt64(this String value) {
		return value.ToParsable<UInt64>(0);
	} // ToUInt64

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.UInt64.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	public static UInt64 ToUInt64(this String value, UInt64 defaultValue) {
		return value.ToParsable<UInt64>(defaultValue);
	} // ToUInt64

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.UInt64.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or an exception if thrown.</returns>
	public static UInt64 ToUInt64OrThrow(this String value) {
		return value.ToParsableOrThrow<UInt64>();
	} // ToUInt64OrThrow
	#endregion

	#region ToSingle
	//------------------------------------------------------------------------------------------------------------------
	// ToSingle.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.Single.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or 0.0F on exceptions.</returns>
	public static Single ToSingle(this String value) {
		return value.ToParsable<Single>(0.0F);
	} // ToSingle

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.Single.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	public static Single ToSingle(this String value, Single defaultValue) {
		return value.ToParsable<Single>(defaultValue);
	} // ToSingle

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.Single.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or an exception if thrown.</returns>
	public static Single ToSingleOrThrow(this String value) {
		return value.ToParsableOrThrow<Single>();
	} // ToSingleOrThrow
	#endregion

	#region ToSingleInvariant
	//------------------------------------------------------------------------------------------------------------------
	// ToSingleInvariant.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.Single using the invariant culture.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or 0.0F on exceptions.</returns>
	public static Single ToSingleInvariant(this String value) {
		return value.ToParsable<Single>(0.0F, NumberFormatInfo.InvariantInfo);
	} // ToSingleInvariant

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.Single using the invariant culture.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	public static Single ToSingleInvariant(this String value, Single defaultValue) {
		return value.ToParsable<Single>(defaultValue, NumberFormatInfo.InvariantInfo);
	} // ToSingleInvariant

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.Single using the invariant culture.
	/// All strings concidered 1.0F, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered 0.0F, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or an exception if thrown.</returns>
	public static Single ToSingleInvariantOrThrow(this String value) {
		return value.ToParsableOrThrow<Single>(NumberFormatInfo.InvariantInfo);
	} // ToSingleInvariantOrThrow
	#endregion

	#region ToDouble
	//------------------------------------------------------------------------------------------------------------------
	// ToDouble.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.Double.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or 0.0F on exceptions.</returns>
	public static Double ToDouble(this String value) {
		return value.ToParsable<Double>(0.0F);
	} // ToDouble

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.Double.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	public static Double ToDouble(this String value, Double defaultValue) {
		return value.ToParsable<Double>(defaultValue);
	} // ToDouble

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.Double.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or an exception if thrown.</returns>
	public static Double ToDoubleOrThrow(this String value) {
		return value.ToParsableOrThrow<Double>();
	} // ToDoubleOrThrow
	#endregion

	#region ToDoubleInvariant
	//------------------------------------------------------------------------------------------------------------------
	// ToDoubleInvariant.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.Double using the invariant culture.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or 0.0F on exceptions.</returns>
	public static Double ToDoubleInvariant(this String value) {
		return value.ToParsable<Double>(0.0F, NumberFormatInfo.InvariantInfo);
	} // ToDoubleInvariant

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.Double using the invariant culture.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	public static Double ToDoubleInvariant(this String value, Double defaultValue) {
		return value.ToParsable<Double>(defaultValue, NumberFormatInfo.InvariantInfo);
	} // ToDoubleInvariant

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.Double using the invariant culture.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or an exception if thrown.</returns>
	public static Double ToDoubleInvariantOrThrow(this String value) {
		return value.ToParsableOrThrow<Double>(NumberFormatInfo.InvariantInfo);
	} // ToDoubleInvariantOrThrow
	#endregion

	#region ToDecimal
	//------------------------------------------------------------------------------------------------------------------
	// ToDecimal.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.Decimal.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or 0.0M on exceptions.</returns>
	public static Decimal ToDecimal(this String value) {
		return value.ToParsable<Decimal>(0.0M);
	} // ToDecimal

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.Decimal.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	public static Decimal ToDecimal(this String value, Decimal defaultValue) {
		return value.ToParsable<Decimal>(defaultValue);
	} // ToDecimal

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.Decimal.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or an exception if thrown.</returns>
	public static Decimal ToDecimalOrThrow(this String value) {
		return value.ToParsableOrThrow<Decimal>();
	} // ToDecimalOrThrow
	#endregion

	#region ToDecimalInvariant
	//------------------------------------------------------------------------------------------------------------------
	// ToDecimalInvariant.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.Decimal using the invariant culture.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or 0.0M on exceptions.</returns>
	public static Decimal ToDecimalInvariant(this String value) {
		return value.ToParsable<Decimal>(0.0M, NumberFormatInfo.InvariantInfo);
	} // ToDecimalInvariant

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.Decimal using the invariant culture.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	public static Decimal ToDecimalInvariant(this String value, Decimal defaultValue) {
		return value.ToParsable<Decimal>(defaultValue, NumberFormatInfo.InvariantInfo);
	} // ToDecimalInvariant

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.Decimal using the invariant culture.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or an exception if thrown.</returns>
	public static Decimal ToDecimalInvariantOrThrow(this String value) {
		return value.ToParsableOrThrow<Decimal>(NumberFormatInfo.InvariantInfo);
	} // ToDecimalInvariantOrThrow
	#endregion

	#region ToDateTime
	//------------------------------------------------------------------------------------------------------------------
	// ToDateTime.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.DateTime.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or DateTime.Now on exceptions.</returns>
	public static DateTime ToDateTime(this String value) {
		return value.ToParsable<DateTime>(DateTime.Now);
	} // ToDateTime

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.DateTime.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	public static DateTime ToDateTime(this String value, DateTime defaultValue) {
		return value.ToParsable<DateTime>(defaultValue);
	} // ToDateTime

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.DateTime.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or an exception if thrown.</returns>
	public static DateTime ToDateTimeOrThrow(this String value) {
		return value.ToParsableOrThrow<DateTime>();
	} // ToDateTimeOrThrow
	#endregion

	#region ToDateTimeInvariant
	//------------------------------------------------------------------------------------------------------------------
	// ToDateTimeInvariant.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.DateTime using the invariant culture.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or DateTime.Now on exceptions.</returns>
	public static DateTime ToDateTimeInvariant(this String value) {
		return value.ToParsable<DateTime>(DateTime.Now, DateTimeFormatInfo.InvariantInfo);
	} // ToDateTimeInvariant

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.DateTime using the invariant culture.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	public static DateTime ToDateTimeInvariant(this String value, DateTime defaultValue) {
		return value.ToParsable<DateTime>(defaultValue, DateTimeFormatInfo.InvariantInfo);
	} // ToDateTimeInvariant

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.DateTime using the invariant culture.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or an exception if thrown.</returns>
	public static DateTime ToDateTimeInvariantOrThrow(this String value) {
		return value.ToParsableOrThrow<DateTime>(DateTimeFormatInfo.InvariantInfo);
	} // ToDateTimeInvariantOrThrow
	#endregion

	#region ToDateOnly
	//------------------------------------------------------------------------------------------------------------------
	// ToDateOnly.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.DateOnly.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or the current local date on exceptions.</returns>
	public static DateOnly ToDateOnly(this String value) {
		DateTime today = DateTime.Today;
		return value.ToParsable<DateOnly>(new DateOnly(today.Year, today.Month, today.Day));
	} // ToDateOnly

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.DateOnly.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	public static DateOnly ToDateOnly(this String value, DateOnly defaultValue) {
		return value.ToParsable<DateOnly>(defaultValue);
	} // ToDateOnly

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.DateOnly.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or an exception if thrown.</returns>
	public static DateOnly ToDateOnlyOrThrow(this String value) {
		return value.ToParsableOrThrow<DateOnly>();
	} // ToDateOnlyOrThrow
	#endregion

	#region ToDateOnlyInvariant
	//------------------------------------------------------------------------------------------------------------------
	// ToDateOnlyInvariant.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.DateOnly using the invariant culture.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or the current local date on exceptions.</returns>
	public static DateOnly ToDateOnlyInvariant(this String value) {
		DateTime today = DateTime.Today;
		return value.ToParsable<DateOnly>(new DateOnly(today.Year, today.Month, today.Day), DateTimeFormatInfo.InvariantInfo);
	} // ToDateOnlyInvariant

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.DateOnly using the invariant culture.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	public static DateOnly ToDateOnlyInvariant(this String value, DateOnly defaultValue) {
		return value.ToParsable<DateOnly>(defaultValue, DateTimeFormatInfo.InvariantInfo);
	} // ToDateOnlyInvariant

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.DateOnly using the invariant culture.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or an exception if thrown.</returns>
	public static DateOnly ToDateOnlyInvariantOrThrow(this String value) {
		return value.ToParsableOrThrow<DateOnly>(DateTimeFormatInfo.InvariantInfo);
	} // ToDateOnlyInvariantOrThrow
	#endregion

	#region ToTimeOnly
	//------------------------------------------------------------------------------------------------------------------
	// ToTimeOnly.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.TimeOnly.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or the current local time on exceptions.</returns>
	public static TimeOnly ToTimeOnly(this String value) {
		DateTime now = DateTime.Now;
		return value.ToParsable<TimeOnly>(new TimeOnly(now.Hour, now.Minute, now.Second));
	} // ToTimeOnly

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.TimeOnly.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	public static TimeOnly ToTimeOnly(this String value, TimeOnly defaultValue) {
		return value.ToParsable<TimeOnly>(defaultValue);
	} // ToTimeOnly

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.TimeOnly.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or an exception if thrown.</returns>
	public static TimeOnly ToTimeOnlyOrThrow(this String value) {
		return value.ToParsableOrThrow<TimeOnly>();
	} // ToTimeOnlyOrThrow
	#endregion

	#region ToTimeOnlyInvariant
	//------------------------------------------------------------------------------------------------------------------
	// ToTimeOnlyInvariant.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.TimeOnly using the invariant culture.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or the current local time on exceptions.</returns>
	public static TimeOnly ToTimeOnlyInvariant(this String value) {
		DateTime now = DateTime.Now;
		return value.ToParsable<TimeOnly>(new TimeOnly(now.Hour, now.Minute, now.Second), DateTimeFormatInfo.InvariantInfo);
	} // ToTimeOnlyInvariant

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.TimeOnly using the invariant culture.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	public static TimeOnly ToTimeOnlyInvariant(this String value, TimeOnly defaultValue) {
		return value.ToParsable<TimeOnly>(defaultValue, DateTimeFormatInfo.InvariantInfo);
	} // ToTimeOnlyInvariant

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.TimeOnly using the invariant culture.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or an exception if thrown.</returns>
	public static TimeOnly ToTimeOnlyInvariantOrThrow(this String value) {
		return value.ToParsableOrThrow<TimeOnly>(DateTimeFormatInfo.InvariantInfo);
	} // ToTimeOnlyInvariantOrThrow
	#endregion

	#region ToGuid
	//------------------------------------------------------------------------------------------------------------------
	// ToGuid.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.Guid.
	/// The value may be a BASE64 encoded string representing a Guid.
	/// Such a string should be either 24 characters long, or 22 characters long (without the ending '==').
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or Guid.Empty on exceptions.</returns>
	static public Guid ToGuid(this String value) {
		if (value == null) {
			return Guid.Empty;
		}

		try {
			if (value.Trim().Length == 24) {
				value					= value.Replace("_", "/");
				value					= value.Replace("-", "+");
				Byte[]	buffer	= Convert.FromBase64String(value);
				return new Guid(buffer);
			} else if (value.Trim().Length == 22) {
				value					= value.Replace("_", "/");
				value					= value.Replace("-", "+");
				value					= value + "==";
				Byte[]	buffer	= Convert.FromBase64String(value);
				return new Guid(buffer);
			} else {
				return new Guid(value);
			}
		} catch {
			return Guid.Empty;
		}
	} // ToGuid

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.Guid.
	/// The value may be a BASE64 encoded string representing a Guid.
	/// Such a string should be either 24 characters long, or 22 characters long (without the ending '==').
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	static public Guid ToGuid(this String value, Guid defaultValue) {
		if (value == null) {
			return defaultValue;
		}

		try {
			if (value.Trim().Length == 24) {
				value					= value.Replace("_", "/");
				value					= value.Replace("-", "+");
				Byte[]	buffer	= Convert.FromBase64String(value);
				return new Guid(buffer);
			} else if (value.Trim().Length == 22) {
				value					= value.Replace("_", "/");
				value					= value.Replace("-", "+");
				value					= value + "==";
				Byte[]	buffer	= Convert.FromBase64String(value);
				return new Guid(buffer);
			} else {
				return new Guid(value);
			}
		} catch {
			return defaultValue;
		}
	} // ToGuid

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a System.Guid.
	/// The value may be a BASE64 encoded string representing a Guid.
	/// Such a string should be either 24 characters long, or 22 characters long (without the ending '==').
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or an exception if thrown.</returns>
	static public Guid ToGuidOrThrow(this String value) {
		if (value.Trim().Length == 24) {
			value = value.Replace("_", "/");
			value = value.Replace("-", "+");
			Byte[] buffer = Convert.FromBase64String(value);
			return new Guid(buffer);
		} else if (value.Trim().Length == 22) {
			value = value.Replace("_", "/");
			value = value.Replace("-", "+");
			value = value + "==";
			Byte[] buffer = Convert.FromBase64String(value);
			return new Guid(buffer);
		} else {
			return new Guid(value);
		}
	} // ToGuidOrThrow
	#endregion

} // RpcCoreExtensions
#endregion
