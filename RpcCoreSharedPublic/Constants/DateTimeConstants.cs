using System;
namespace RpcScandinavia.Core.Shared;

/// <summary>
/// Date and time standard format specifier constants, for the <see cref="System.DateTime.ToString"> method.
/// </summary>
public static class RpcStandardDateTimeFormat {

	/// <summary>
	/// The short date format specifier ('d'):
	/// 2009-06-15T13:45:30 -> 6/15/2009 (en-US)
	/// 2009-06-15T13:45:30 -> 15/06/2009 (fr-FR)
	/// 2009-06-15T13:45:30 -> 2009/06/15 (ja-JP)
	/// </summary>
	public const String ShortDate = "d";

	/// <summary>
	/// The long date format specifier ('D'):
	/// 2009-06-15T13:45:30 -> Monday, June 15, 2009 (en-US)
	/// 2009-06-15T13:45:30 -> понедельник, 15 июня 2009 г. (ru-RU)
	/// 2009-06-15T13:45:30 -> Montag, 15. Juni 2009 (de-DE)
	/// </summary>
	public const String LongDate = "D";

	/// <summary>
	/// The full date short time format specifier ('f'):
	/// 2009-06-15T13:45:30 -> Monday, June 15, 2009 1:45 PM (en-US)
	/// 2009-06-15T13:45:30 -> den 15 juni 2009 13:45 (sv-SE)
	/// 2009-06-15T13:45:30 -> Δευτέρα, 15 Ιουνίου 2009 1:45 μμ (el-GR)
	/// </summary>
	public const String FullDateShortTime = "f";

	/// <summary>
	/// The full date long time format specifier ('F'):
	/// 2009-06-15T13:45:30 -> Monday, June 15, 2009 1:45:30 PM (en-US)
	/// 2009-06-15T13:45:30 -> den 15 juni 2009 13:45:30 (sv-SE)
	/// 2009-06-15T13:45:30 -> Δευτέρα, 15 Ιουνίου 2009 1:45:30 μμ (el-GR)
	/// </summary>
	public const String FullDateLongTime = "F";

	/// <summary>
	/// The general date short time format specifier ('g'):
	/// 2009-06-15T13:45:30 -> 6/15/2009 1:45 PM (en-US)
	/// 2009-06-15T13:45:30 -> 15/06/2009 13:45 (es-ES)
	/// 2009-06-15T13:45:30 -> 2009/6/15 13:45 (zh-CN)
	/// </summary>
	public const String GeneralDateShortTime = "g";

	/// <summary>
	/// The general date long time date format specifier ('G'):
	/// 2009-06-15T13:45:30 -> 6/15/2009 1:45:30 PM (en-US)
	/// 2009-06-15T13:45:30 -> 15/06/2009 13:45:30 (es-ES)
	/// 2009-06-15T13:45:30 -> 2009/6/15 13:45:30 (zh-CN)
	/// </summary>
	public const String GeneralDateLongTime = "G";

	/// <summary>
	/// The day and month format specifier ('M'):
	/// 2009-06-15T13:45:30 -> June 15 (en-US)
	/// 2009-06-15T13:45:30 -> 15. juni (da-DK)
	/// 2009-06-15T13:45:30 -> 15 Juni (id-ID)
	/// </summary>
	//public const String X = "m";
	public const String MonthAndDay = "M";

	/// <summary>
	/// The round-trip date format specifier ('O').
	/// DateTime values:
	/// 2009-06-15T13:45:30 (DateTimeKind.Local) --> 2009-06-15T13:45:30.0000000-07:00
	/// 2009-06-15T13:45:30 (DateTimeKind.Utc) --> 2009-06-15T13:45:30.0000000Z
	/// 2009-06-15T13:45:30 (DateTimeKind.Unspecified) --> 2009-06-15T13:45:30.0000000
	///
	/// DateTimeOffset values:
	/// 2009-06-15T13:45:30-07:00 --> 2009-06-15T13:45:30.0000000-07:00
	/// </summary>
	//public const String X = "o";
	public const String RoundTrip = "O";

	/// <summary>
	/// The RFC 1123 pattern date and time format specifier ('R'):
	/// 2009-06-15T13:45:30 -> Mon, 15 Jun 2009 20:45:30 GMT
	/// </summary>
	//public const String X = "r";
	public const String Rfc1123 = "R";

	/// <summary>
	/// The sortable date and time format specifier ('s'):
	/// 2009-06-15T13:45:30 (DateTimeKind.Local) -> 2009-06-15T13:45:30
	/// 2009-06-15T13:45:30 (DateTimeKind.Utc) -> 2009-06-15T13:45:30
	/// </summary>
	public const String SortableDateAndTime = "s";

	/// <summary>
	/// The short time format specifier ('t'):
	/// 2009-06-15T13:45:30 -> 1:45 PM (en-US)
	/// 2009-06-15T13:45:30 -> 13:45 (hr-HR)
	/// 2009-06-15T13:45:30 -> 01:45 م (ar-EG)
	/// </summary>
	public const String ShortTime = "t";

	/// <summary>
	/// The X date format specifier ('T'):
	/// 2009-06-15T13:45:30 -> 1:45:30 PM (en-US)
	/// 2009-06-15T13:45:30 -> 13:45:30 (hr-HR)
	/// 2009-06-15T13:45:30 -> 01:45:30 م (ar-EG)
	/// </summary>
	public const String LongTime = "T";

	/// <summary>
	/// The universal sortable date and time format specifier ('u').
	/// DateTime values:
	/// 2009-06-15T13:45:30 -> 2009-06-15 13:45:30Z
	///
	/// DateTimeOffset values:
	/// 2009-06-15T13:45:30 -> 2009-06-15 20:45:30Z
	/// </summary>
	public const String UniversalSortableDateAndTime = "u";

	/// <summary>
	/// The universal date and time format specifier ('U'):
	/// 2009-06-15T13:45:30 -> Monday, June 15, 2009 8:45:30 PM (en-US)
	/// 2009-06-15T13:45:30 -> den 15 juni 2009 20:45:30 (sv-SE)
	/// 2009-06-15T13:45:30 -> Δευτέρα, 15 Ιουνίου 2009 8:45:30 μμ (el-GR)
	/// </summary>
	public const String UniversalFullDateAndTime = "U";

	/// <summary>
	/// The year and month format specifier ('Y'):
	/// 2009-06-15T13:45:30 -> June 2009 (en-US)
	/// 2009-06-15T13:45:30 -> juni 2009 (da-DK)
	/// 2009-06-15T13:45:30 -> Juni 2009 (id-ID)
	/// </summary>
	//public const String X = "y";
	public const String YearAndMonth = "Y";

} // RpcStandardDateTimeFormat
