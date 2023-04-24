using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
namespace RpcScandinavia.Core.Shared;

#region RpcCoreExtensions
//----------------------------------------------------------------------------------------------------------------------
// RpcCoreExtensions.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// The RpcCoreExtensions class contains usefull extension methods for a lot of the basic classes.
/// Some of this code is old, from before Linq and .NET framework version 4.
///
/// Insted of String.Split you can use the ToStringArray, ToStringList extension methods.
/// Insted of String.Merge you can use the ToString on the IEnumerable&lt;String&gt; extension method.
/// Insted of String.IsNullOrWhiteSpace you can use the IsNullOrWhiteSpace extension method.
///
/// Almost everything lives within one large partial class, except for the Double extensions, which are in their
/// own RpcDoubleExtensions class.
///
/// This part of the class contains static variables.
/// </summary>
static public partial class RpcCoreExtensions {
	private static UnicodeEncoding encoderBigEndian = new UnicodeEncoding(true, false);
	private static UnicodeEncoding encoderLittleEndian = new UnicodeEncoding(false, false);
	private static GregorianCalendar gregorianCalendar = new GregorianCalendar();

	/// <summary>
	/// The default char used to separate multiple strings, for instance when converting
	/// between String collections and String.
	/// The default is newline.
	/// </summary>
	public static String StringSeparator = Environment.NewLine;

	/// <summary>
	/// The default chars used to separate multiple strings, for instance when converting
	/// between String and String collections.
	/// The default is newline.
	/// </summary>
	public static String[] StringSeparators = new String[] { "\r", "\n", "\r\n", Environment.NewLine };

	/// <summary>
	/// The string values that is accepted as "true".
	/// </summary>
	public static String[] TrueValues = new String[] { "true", "1", "on", "yes", "enable", "enabled", "success", "ok", "okay" };

	/// <summary>
	/// The string values that is accepted as "false".
	/// </summary>
	public static String[] FalseValues = new String[] { "false", "0", "off", "no", "disable", "disabled", "failure", "err", "error" };
} // RpcCoreExtensions
#endregion
