namespace RpcScandinavia.Core.Shared;
using System;
using System.Globalization;
using System.Text;

#region RpcCoreExtensions
//----------------------------------------------------------------------------------------------------------------------
// RpcCoreExtensions.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// The RpcCoreExtensions class contains usefull extension methods for a lot of the basic classes.
///
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
	/// The default chars used to separate multiple strings, for instance when converting
	/// between String and String collections.
	/// </summary>
	public static String[] StringSeparators = new String[] {
		RpcCoreSharedConstants.STRING_RETURN,
		RpcCoreSharedConstants.STRING_NEWLINE,
		RpcCoreSharedConstants.STRING_RETURN_NEWLINE,
		Environment.NewLine
	};

	/// <summary>
	/// The string values that is accepted as "true".
	/// </summary>
	public static String[] TrueValues = new String[] {
		RpcCoreSharedConstants.STRING_1,
		RpcCoreSharedConstants.STRING_TRUE,
		RpcCoreSharedConstants.STRING_ON,
		RpcCoreSharedConstants.STRING_YES,
		RpcCoreSharedConstants.STRING_ENABLE
	};

	/// <summary>
	/// The string values that is accepted as "false".
	/// </summary>
	public static String[] FalseValues = new String[] {
		RpcCoreSharedConstants.STRING_0,
		RpcCoreSharedConstants.STRING_FALSE,
		RpcCoreSharedConstants.STRING_OFF,
		RpcCoreSharedConstants.STRING_NO,
		RpcCoreSharedConstants.STRING_DISABLE
	};

} // RpcCoreExtensions
#endregion
