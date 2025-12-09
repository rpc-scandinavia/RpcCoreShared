using System;
using System.Globalization;
namespace RpcScandinavia.Core.Shared;

#region RpcCoreExtensions
//----------------------------------------------------------------------------------------------------------------------
// RpcCoreExtensions.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// This part of the class contains useful extension methods for the <see cref="System.String" /> class.
/// </summary>
public partial class RpcCoreExtensions {

	extension (String value) {

		#region ToBoolean
		//--------------------------------------------------------------------------------------------------------------
		// ToBoolean.
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.Boolean" />.
		/// </summary>
		/// <returns>The converted string, or false on exceptions.</returns>
		public Boolean ToBoolean() {
			return value.ToParsable<Boolean>(false);
		} // ToBoolean

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.Boolean" />.
		/// </summary>
		/// <param name="defaultValue">The boolean to return on exceptions.</param>
		/// <returns>The converted string, or the default value on exceptions.</returns>
		public Boolean ToBoolean(Boolean defaultValue) {
			return value.ToParsable<Boolean>(defaultValue);
		} // ToBoolean

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.Boolean" />.
			/// All strings considered true, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
			/// All strings considered false, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
		/// </summary>
		/// <returns>The converted string, or an exception if thrown.</returns>
		public Boolean ToBooleanOrThrow() {
			return value.ToParsableOrThrow<Boolean>();
		} // ToBooleanOrThrow
		#endregion

		#region ToTriple
		//--------------------------------------------------------------------------------------------------------------
		// ToTriple.
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="RpcScandinavia.Core.Shared.Triple" />.
		/// </summary>
		/// <returns>The converted string, or Triple.Unknown on exceptions.</returns>
		public Triple ToTriple() {
			return value.ToParsable<Triple>();
		} // ToTriple
		#endregion

		#region ToSByte
		//--------------------------------------------------------------------------------------------------------------
		// ToSByte.
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.SByte" />.
		/// </summary>
		/// <returns>The converted string, or zero on exceptions.</returns>
		public SByte ToSByte() {
			return value.ToParsable<SByte>(0);
		} // ToSByte

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.SByte" />.
		/// </summary>
		/// <param name="defaultValue">The signed byte to return on exceptions.</param>
		/// <returns>The converted string, or the default value on exceptions.</returns>
		public SByte ToSByte(SByte defaultValue) {
			return value.ToParsable<SByte>(defaultValue);
		} // ToSByte

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.SByte" />.
		/// </summary>
		/// <returns>The converted string, or an exception if thrown.</returns>
		public SByte ToSByteOrThrow() {
			return value.ToParsableOrThrow<SByte>();
		} // ToSByteOrThrow
		#endregion

		#region ToInt16
		//--------------------------------------------------------------------------------------------------------------
		// ToInt16.
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.Int16" />.
		/// </summary>
		/// <returns>The converted string, or zero on exceptions.</returns>
		public Int16 ToInt16() {
			return value.ToParsable<Int16>(0);
		} // ToInt16

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.Int16" />.
		/// </summary>
		/// <param name="defaultValue">The integer to return on exceptions.</param>
		/// <returns>The converted string, or the default value on exceptions.</returns>
		public Int16 ToInt16(Int16 defaultValue) {
			return value.ToParsable<Int16>(defaultValue);
		} // ToInt16

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.Int16" />.
		/// </summary>
		/// <returns>The converted string, or an exception if thrown.</returns>
		public Int16 ToInt16OrThrow() {
			return value.ToParsableOrThrow<Int16>();
		} // ToInt16OrThrow
		#endregion

		#region ToInt32
		//--------------------------------------------------------------------------------------------------------------
		// ToInt32.
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.Int32" />.
		/// </summary>
		/// <returns>The converted string, or zero on exceptions.</returns>
		public Int32 ToInt32() {
			return value.ToParsable<Int32>(0);
		} // ToInt32

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.Int32" />.
		/// </summary>
		/// <param name="defaultValue">The integer to return on exceptions.</param>
		/// <returns>The converted string, or the default value on exceptions.</returns>
		public Int32 ToInt32(Int32 defaultValue) {
			return value.ToParsable<Int32>(defaultValue);
		} // ToInt32

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.Int32" />.
		/// </summary>
		/// <returns>The converted string, or an exception if thrown.</returns>
		public Int32 ToInt32OrThrow() {
			return value.ToParsableOrThrow<Int32>();
		} // ToInt32OrThrow
		#endregion

		#region ToInt64
		//--------------------------------------------------------------------------------------------------------------
		// ToInt64.
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.Int64" />.
		/// </summary>
		/// <returns>The converted string, or zero on exceptions.</returns>
		public Int64 ToInt64() {
			return value.ToParsable<Int64>(0);
		} // ToInt64

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.Int64" />.
		/// </summary>
		/// <param name="defaultValue">The integer to return on exceptions.</param>
		/// <returns>The converted string, or the default value on exceptions.</returns>
		public Int64 ToInt64(Int64 defaultValue) {
			return value.ToParsable<Int64>(0);
		} // ToInt64

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.Int64" />.
		/// </summary>
		/// <returns>The converted string, or an exception if thrown.</returns>
		public Int64 ToInt64OrThrow() {
			return value.ToParsableOrThrow<Int64>();
		} // ToInt64OrThrow
		#endregion

		#region ToByte
		//--------------------------------------------------------------------------------------------------------------
		// ToByte.
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.Byte" />.
		/// </summary>
		/// <returns>The converted string, or zero on exceptions.</returns>
		public Byte ToByte() {
			return value.ToParsable<Byte>(0);
		} // ToByte

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.Byte" />.
		/// </summary>
		/// <param name="defaultValue">The byte to return on exceptions.</param>
		/// <returns>The converted string, or the default value on exceptions.</returns>
		public Byte ToByte(Byte defaultValue) {
			return value.ToParsable<Byte>(defaultValue);
		} // ToByte

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.Byte" />.
		/// </summary>
		/// <returns>The converted string, or an exception if thrown.</returns>
		public Byte ToByteOrThrow() {
			return value.ToParsableOrThrow<Byte>();
		} // ToByteOrThrow
		#endregion

		#region ToUInt16
		//--------------------------------------------------------------------------------------------------------------
		// ToUInt16.
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.UInt16" />.
		/// </summary>
		/// <returns>The converted string, or zero on exceptions.</returns>
		public UInt16 ToUInt16() {
			return value.ToParsable<UInt16>(0);
		} // ToUInt16

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.UInt16" />.
		/// </summary>
		/// <param name="defaultValue">The unsigned integer to return on exceptions.</param>
		/// <returns>The converted string, or the default value on exceptions.</returns>
		public UInt16 ToUInt16(UInt16 defaultValue) {
			return value.ToParsable<UInt16>(defaultValue);
		} // ToUInt16

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.UInt16" />.
		/// </summary>
		/// <returns>The converted string, or an exception if thrown.</returns>
		public UInt16 ToUInt16OrThrow() {
			return value.ToParsableOrThrow<UInt16>();
		} // ToUInt16OrThrow
		#endregion

		#region ToUInt32
		//--------------------------------------------------------------------------------------------------------------
		// ToUInt32.
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.UInt32" />.
		/// </summary>
		/// <returns>The converted string, or zero on exceptions.</returns>
		public UInt32 ToUInt32() {
			return value.ToParsable<UInt32>(0);
		} // ToUInt32

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.UInt32" />.
		/// </summary>
		/// <param name="defaultValue">The unsigned integer to return on exceptions.</param>
		/// <returns>The converted string, or the default value on exceptions.</returns>
		public UInt32 ToUInt32(UInt32 defaultValue) {
			return value.ToParsable<UInt32>(defaultValue);
		} // ToUInt32

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.UInt32" />.
		/// </summary>
		/// <returns>The converted string, or an exception if thrown.</returns>
		public UInt32 ToUInt32OrThrow() {
			return value.ToParsableOrThrow<UInt32>();
		} // ToUInt32OrThrow
		#endregion

		#region ToUInt64
		//--------------------------------------------------------------------------------------------------------------
		// ToUInt64.
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.UInt64" />.
		/// </summary>
		/// <returns>The converted string, or zero on exceptions.</returns>
		public UInt64 ToUInt64() {
			return value.ToParsable<UInt64>(0);
		} // ToUInt64

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.UInt64" />.
		/// </summary>
		/// <param name="defaultValue">The unsigned integer to return on exceptions.</param>
		/// <returns>The converted string, or the default value on exceptions.</returns>
		public UInt64 ToUInt64(UInt64 defaultValue) {
			return value.ToParsable<UInt64>(defaultValue);
		} // ToUInt64

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.UInt64" />.
		/// </summary>
		/// <returns>The converted string, or an exception if thrown.</returns>
		public UInt64 ToUInt64OrThrow() {
			return value.ToParsableOrThrow<UInt64>();
		} // ToUInt64OrThrow
		#endregion

		#region ToSingle
		//--------------------------------------------------------------------------------------------------------------
		// ToSingle.
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.Single" />.
		/// </summary>
		/// <returns>The converted string, or 0.0F on exceptions.</returns>
		public Single ToSingle() {
			return value.ToParsable<Single>(0.0F);
		} // ToSingle

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.Single" />.
		/// </summary>
		/// <param name="defaultValue">The single to return on exceptions.</param>
		/// <returns>The converted string, or the default value on exceptions.</returns>
		public Single ToSingle(Single defaultValue) {
			return value.ToParsable<Single>(defaultValue);
		} // ToSingle

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.Single" />.
		/// </summary>
		/// <returns>The converted string, or an exception if thrown.</returns>
		public Single ToSingleOrThrow() {
			return value.ToParsableOrThrow<Single>();
		} // ToSingleOrThrow
		#endregion

		#region ToSingleInvariant
		//--------------------------------------------------------------------------------------------------------------
		// ToSingleInvariant.
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.Single" /> using the invariant culture.
		/// </summary>
		/// <returns>The converted string, or 0.0F on exceptions.</returns>
		public Single ToSingleInvariant() {
			return value.ToParsable<Single>(0.0F, NumberFormatInfo.InvariantInfo);
		} // ToSingleInvariant

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.Single" /> using the invariant culture.
		/// </summary>
		/// <param name="defaultValue">The single to return on exceptions.</param>
		/// <returns>The converted string, or the default value on exceptions.</returns>
		public Single ToSingleInvariant(Single defaultValue) {
			return value.ToParsable<Single>(defaultValue, NumberFormatInfo.InvariantInfo);
		} // ToSingleInvariant

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.Single" /> using the invariant culture.
			/// All strings concidered 1.0F, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
			/// All strings concidered 0.0F, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
		/// </summary>
		/// <returns>The converted string, or an exception if thrown.</returns>
		public Single ToSingleInvariantOrThrow() {
			return value.ToParsableOrThrow<Single>(NumberFormatInfo.InvariantInfo);
		} // ToSingleInvariantOrThrow
		#endregion

		#region ToDouble
		//--------------------------------------------------------------------------------------------------------------
		// ToDouble.
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.Double" />.
		/// </summary>
		/// <returns>The converted string, or 0.0F on exceptions.</returns>
		public Double ToDouble() {
			return value.ToParsable<Double>(0.0F);
		} // ToDouble

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.Double" />.
		/// </summary>
		/// <param name="defaultValue">The double to return on exceptions.</param>
		/// <returns>The converted string, or the default value on exceptions.</returns>
		public Double ToDouble(Double defaultValue) {
			return value.ToParsable<Double>(defaultValue);
		} // ToDouble

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.Double" />.
		/// </summary>
		/// <returns>The converted string, or an exception if thrown.</returns>
		public Double ToDoubleOrThrow() {
			return value.ToParsableOrThrow<Double>();
		} // ToDoubleOrThrow
		#endregion

		#region ToDoubleInvariant
		//--------------------------------------------------------------------------------------------------------------
		// ToDoubleInvariant.
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.Double" /> using the invariant culture.
		/// </summary>
		/// <returns>The converted string, or 0.0F on exceptions.</returns>
		public Double ToDoubleInvariant() {
			return value.ToParsable<Double>(0.0F, NumberFormatInfo.InvariantInfo);
		} // ToDoubleInvariant

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.Double" /> using the invariant culture.
		/// </summary>
		/// <param name="defaultValue">The double to return on exceptions.</param>
		/// <returns>The converted string, or the default value on exceptions.</returns>
		public Double ToDoubleInvariant(Double defaultValue) {
			return value.ToParsable<Double>(defaultValue, NumberFormatInfo.InvariantInfo);
		} // ToDoubleInvariant

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.Double" /> using the invariant culture.
		/// </summary>
		/// <returns>The converted string, or an exception if thrown.</returns>
		public Double ToDoubleInvariantOrThrow() {
			return value.ToParsableOrThrow<Double>(NumberFormatInfo.InvariantInfo);
		} // ToDoubleInvariantOrThrow
		#endregion

		#region ToDecimal
		//--------------------------------------------------------------------------------------------------------------
		// ToDecimal.
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.Decimal" />.
		/// </summary>
		/// <returns>The converted string, or 0.0M on exceptions.</returns>
		public Decimal ToDecimal() {
			return value.ToParsable<Decimal>(0.0M);
		} // ToDecimal

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.Decimal" />.
		/// </summary>
		/// <param name="defaultValue">The decimal to return on exceptions.</param>
		/// <returns>The converted string, or the default value on exceptions.</returns>
		public Decimal ToDecimal(Decimal defaultValue) {
			return value.ToParsable<Decimal>(defaultValue);
		} // ToDecimal

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.Decimal" />.
		/// </summary>
		/// <returns>The converted string, or an exception if thrown.</returns>
		public Decimal ToDecimalOrThrow() {
			return value.ToParsableOrThrow<Decimal>();
		} // ToDecimalOrThrow
		#endregion

		#region ToDecimalInvariant
		//--------------------------------------------------------------------------------------------------------------
		// ToDecimalInvariant.
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.Decimal" /> using the invariant culture.
		/// </summary>
		/// <returns>The converted string, or 0.0M on exceptions.</returns>
		public Decimal ToDecimalInvariant() {
			return value.ToParsable<Decimal>(0.0M, NumberFormatInfo.InvariantInfo);
		} // ToDecimalInvariant

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.Decimal" /> using the invariant culture.
		/// </summary>
		/// <param name="defaultValue">The decimal to return on exceptions.</param>
		/// <returns>The converted string, or the default value on exceptions.</returns>
		public Decimal ToDecimalInvariant(Decimal defaultValue) {
			return value.ToParsable<Decimal>(defaultValue, NumberFormatInfo.InvariantInfo);
		} // ToDecimalInvariant

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.Decimal" /> using the invariant culture.
		/// </summary>
		/// <returns>The converted string, or an exception if thrown.</returns>
		public Decimal ToDecimalInvariantOrThrow() {
			return value.ToParsableOrThrow<Decimal>(NumberFormatInfo.InvariantInfo);
		} // ToDecimalInvariantOrThrow
		#endregion

		#region ToDateTime
		//--------------------------------------------------------------------------------------------------------------
		// ToDateTime.
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.DateTime" />.
		/// </summary>
		/// <returns>The converted string, or DateTime.Now on exceptions.</returns>
		public DateTime ToDateTime() {
			return value.ToParsable<DateTime>(DateTime.Now);
		} // ToDateTime

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.DateTime" />.
		/// </summary>
		/// <param name="defaultValue">The date-time to return on exceptions.</param>
		/// <returns>The converted string, or the default value on exceptions.</returns>
		public DateTime ToDateTime(DateTime defaultValue) {
			return value.ToParsable<DateTime>(defaultValue);
		} // ToDateTime

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.DateTime" />.
		/// </summary>
		/// <returns>The converted string, or an exception if thrown.</returns>
		public DateTime ToDateTimeOrThrow() {
			return value.ToParsableOrThrow<DateTime>();
		} // ToDateTimeOrThrow
		#endregion

		#region ToDateTimeInvariant
		//--------------------------------------------------------------------------------------------------------------
		// ToDateTimeInvariant.
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.DateTime" /> using the invariant culture.
		/// </summary>
		/// <returns>The converted string, or DateTime.Now on exceptions.</returns>
		public DateTime ToDateTimeInvariant() {
			return value.ToParsable<DateTime>(DateTime.Now, DateTimeFormatInfo.InvariantInfo);
		} // ToDateTimeInvariant

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.DateTime" /> using the invariant culture.
		/// </summary>
		/// <param name="defaultValue">The date-time to return on exceptions.</param>
		/// <returns>The converted string, or the default value on exceptions.</returns>
		public DateTime ToDateTimeInvariant(DateTime defaultValue) {
			return value.ToParsable<DateTime>(defaultValue, DateTimeFormatInfo.InvariantInfo);
		} // ToDateTimeInvariant

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.DateTime" /> using the invariant culture.
		/// </summary>
		/// <returns>The converted string, or an exception if thrown.</returns>
		public DateTime ToDateTimeInvariantOrThrow() {
			return value.ToParsableOrThrow<DateTime>(DateTimeFormatInfo.InvariantInfo);
		} // ToDateTimeInvariantOrThrow
		#endregion

		#region ToDateOnly
		//--------------------------------------------------------------------------------------------------------------
		// ToDateOnly.
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.DateOnly" />.
		/// </summary>
		/// <returns>The converted string, or the current local date on exceptions.</returns>
		public DateOnly ToDateOnly() {
			DateTime today = DateTime.Today;
			return value.ToParsable<DateOnly>(new DateOnly(today.Year, today.Month, today.Day));
		} // ToDateOnly

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.DateOnly" />.
		/// </summary>
		/// <param name="defaultValue">The date to return on exceptions.</param>
		/// <returns>The converted string, or the default value on exceptions.</returns>
		public DateOnly ToDateOnly(DateOnly defaultValue) {
			return value.ToParsable<DateOnly>(defaultValue);
		} // ToDateOnly

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.DateOnly" />.
		/// </summary>
		/// <returns>The converted string, or an exception if thrown.</returns>
		public DateOnly ToDateOnlyOrThrow() {
			return value.ToParsableOrThrow<DateOnly>();
		} // ToDateOnlyOrThrow
		#endregion

		#region ToDateOnlyInvariant
		//--------------------------------------------------------------------------------------------------------------
		// ToDateOnlyInvariant.
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.DateOnly" /> using the invariant culture.
		/// </summary>
		/// <returns>The converted string, or the current local date on exceptions.</returns>
		public DateOnly ToDateOnlyInvariant() {
			DateTime today = DateTime.Today;
			return value.ToParsable<DateOnly>(new DateOnly(today.Year, today.Month, today.Day), DateTimeFormatInfo.InvariantInfo);
		} // ToDateOnlyInvariant

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.DateOnly" /> using the invariant culture.
		/// </summary>
		/// <param name="defaultValue">The date to return on exceptions.</param>
		/// <returns>The converted string, or the default value on exceptions.</returns>
		public DateOnly ToDateOnlyInvariant(DateOnly defaultValue) {
			return value.ToParsable<DateOnly>(defaultValue, DateTimeFormatInfo.InvariantInfo);
		} // ToDateOnlyInvariant

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.DateOnly" /> using the invariant culture.
		/// </summary>
		/// <returns>The converted string, or an exception if thrown.</returns>
		public DateOnly ToDateOnlyInvariantOrThrow() {
			return value.ToParsableOrThrow<DateOnly>(DateTimeFormatInfo.InvariantInfo);
		} // ToDateOnlyInvariantOrThrow
		#endregion

		#region ToTimeOnly
		//--------------------------------------------------------------------------------------------------------------
		// ToTimeOnly.
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.TimeOnly" />.
		/// </summary>
		/// <returns>The converted string, or the current local time on exceptions.</returns>
		public TimeOnly ToTimeOnly() {
			DateTime now = DateTime.Now;
			return value.ToParsable<TimeOnly>(new TimeOnly(now.Hour, now.Minute, now.Second));
		} // ToTimeOnly

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.TimeOnly" />.
		/// </summary>
		/// <param name="defaultValue">The time to return on exceptions.</param>
		/// <returns>The converted string, or the default value on exceptions.</returns>
		public TimeOnly ToTimeOnly(TimeOnly defaultValue) {
			return value.ToParsable<TimeOnly>(defaultValue);
		} // ToTimeOnly

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.TimeOnly" />.
		/// </summary>
		/// <returns>The converted string, or an exception if thrown.</returns>
		public TimeOnly ToTimeOnlyOrThrow() {
			return value.ToParsableOrThrow<TimeOnly>();
		} // ToTimeOnlyOrThrow
		#endregion

		#region ToTimeOnlyInvariant
		//--------------------------------------------------------------------------------------------------------------
		// ToTimeOnlyInvariant.
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.TimeOnly" /> using the invariant culture.
		/// </summary>
		/// <returns>The converted string, or the current local time on exceptions.</returns>
		public TimeOnly ToTimeOnlyInvariant() {
			DateTime now = DateTime.Now;
			return value.ToParsable<TimeOnly>(new TimeOnly(now.Hour, now.Minute, now.Second), DateTimeFormatInfo.InvariantInfo);
		} // ToTimeOnlyInvariant

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.TimeOnly" /> using the invariant culture.
		/// </summary>
		/// <param name="defaultValue">The time to return on exceptions.</param>
		/// <returns>The converted string, or the default value on exceptions.</returns>
		public TimeOnly ToTimeOnlyInvariant(TimeOnly defaultValue) {
			return value.ToParsable<TimeOnly>(defaultValue, DateTimeFormatInfo.InvariantInfo);
		} // ToTimeOnlyInvariant

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.TimeOnly" /> using the invariant culture.
		/// </summary>
		/// <returns>The converted string, or an exception if thrown.</returns>
		public TimeOnly ToTimeOnlyInvariantOrThrow() {
			return value.ToParsableOrThrow<TimeOnly>(DateTimeFormatInfo.InvariantInfo);
		} // ToTimeOnlyInvariantOrThrow
		#endregion

		#region ToGuid
		//--------------------------------------------------------------------------------------------------------------
		// ToGuid.
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.Guid" />.
		/// The string may be a BASE64 encoded string representing a Guid.
		/// Such a string should be either 24 characters long, or 22 characters long (without the ending '==').
		/// </summary>
		/// <returns>The converted string, or Guid.Empty on exceptions.</returns>
		public Guid ToGuid() {
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
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.Guid" />.
		/// The string may be a BASE64 encoded string representing a Guid.
		/// Such a string should be either 24 characters long, or 22 characters long (without the ending '==').
		/// </summary>
		/// <param name="defaultValue">The guid to return on exceptions.</param>
		/// <returns>The converted string, or the default value on exceptions.</returns>
		public Guid ToGuid(Guid defaultValue) {
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
		/// Gets the <see cref="System.String" /> converted to a <see cref="System.Guid" />.
		/// The string may be a BASE64 encoded string representing a Guid.
		/// Such a string should be either 24 characters long, or 22 characters long (without the ending '==').
		/// </summary>
		/// <returns>The converted string, or an exception if thrown.</returns>
		public Guid ToGuidOrThrow() {
			if (value == null) {
				throw new NullReferenceException(nameof(value));
			}

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

	}

} // RpcCoreExtensions
#endregion
