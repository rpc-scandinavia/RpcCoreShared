namespace RpcScandinavia.Core.Shared;
using System;
using System.Text;

#region RpcCoreExtensions
//----------------------------------------------------------------------------------------------------------------------
// RpcCoreExtensions.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// This part of the class contains usefull extension methods for the <see cref="System.Text.StringBuilder" /> class.
/// </summary>
static public partial class RpcCoreExtensions {

	#region Append
	//------------------------------------------------------------------------------------------------------------------
	// Append.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Appends a copy of the string to the string builder, using the separator (delimiter) between the existing text
	/// in the string builder and the appended text.
	/// </summary>
	/// <param name="stringBuilder">The string builder.</param>
	/// <param name="text">The string to append to the string builder.</param>
	/// <param name="delimiter">The separator bewteen existing text in the string builder, and the appended text (tabulator).</param>
	/// <param name="writeEmptyString">Indicates whether or not empty strings should be appended to the string builder.</param>
	public static void AppendDelimiter(this StringBuilder stringBuilder, String text, String delimiter = RpcCoreSharedConstants.STRING_TAB, Boolean writeEmptyString = true) {
		if ((writeEmptyString == true) || (text.IsNullOrWhiteSpace() == false)) {
			if (stringBuilder.Length != 0) {
				stringBuilder.Append(delimiter);
				stringBuilder.Append(text);
			} else {
				stringBuilder.Append(text);
			}
		}
	} // AppendDelimiter

	/// <summary>
	/// Appends the characters to the string builder, using the separator (delimiter) between the existing text
	/// in the string builder and the appended text.
	/// </summary>
	/// <param name="stringBuilder">The string builder.</param>
	/// <param name="text">The characters to append to the string builder.</param>
	/// <param name="delimiter">The separator bewteen existing text in the string builder, and the appended text (tabulator).</param>
	/// <param name="writeEmptyString">Indicates whether or not empty strings should be appended to the string builder.</param>
	public static void AppendDelimiter(this StringBuilder stringBuilder, Memory<Char> text, String delimiter = RpcCoreSharedConstants.STRING_TAB, Boolean writeEmptyString = true) {
		if ((writeEmptyString == true) || (text.IsEmpty == false)) {
			if (stringBuilder.Length != 0) {
				stringBuilder.Append(delimiter);
				stringBuilder.Append(text);
			} else {
				stringBuilder.Append(text);
			}
		}
	} // AppendDelimiter

	/// <summary>
	/// Appends the characters to the string builder, using the separator (delimiter) between the existing text
	/// in the string builder and the appended text.
	/// </summary>
	/// <param name="stringBuilder">The string builder.</param>
	/// <param name="text">The characters to append to the string builder.</param>
	/// <param name="delimiter">The separator bewteen existing text in the string builder, and the appended text (tabulator).</param>
	/// <param name="writeEmptyString">Indicates whether or not empty strings should be appended to the string builder.</param>
	public static void AppendDelimiter(this StringBuilder stringBuilder, ReadOnlyMemory<Char> text, String delimiter = RpcCoreSharedConstants.STRING_TAB, Boolean writeEmptyString = true) {
		if ((writeEmptyString == true) || (text.IsEmpty == false)) {
			if (stringBuilder.Length != 0) {
				stringBuilder.Append(delimiter);
				stringBuilder.Append(text);
			} else {
				stringBuilder.Append(text);
			}
		}
	} // AppendDelimiter

	/// <summary>
	/// Appends the characters to the string builder, using the separator (delimiter) between the existing text
	/// in the string builder and the appended text.
	/// </summary>
	/// <param name="stringBuilder">The string builder.</param>
	/// <param name="text">The characters to append to the string builder.</param>
	/// <param name="delimiter">The separator bewteen existing text in the string builder, and the appended text (tabulator).</param>
	/// <param name="writeEmptyString">Indicates whether or not empty strings should be appended to the string builder.</param>
	public static void AppendDelimiter(this StringBuilder stringBuilder, Span<Char> text, String delimiter = RpcCoreSharedConstants.STRING_TAB, Boolean writeEmptyString = true) {
		if ((writeEmptyString == true) || (text.IsEmpty == false)) {
			if (stringBuilder.Length != 0) {
				stringBuilder.Append(delimiter);
				stringBuilder.Append(text);
			} else {
				stringBuilder.Append(text);
			}
		}
	} // AppendDelimiter

	/// <summary>
	/// Appends the characters to the string builder, using the separator (delimiter) between the existing text
	/// in the string builder and the appended text.
	/// </summary>
	/// <param name="stringBuilder">The string builder.</param>
	/// <param name="text">The characters to append to the string builder.</param>
	/// <param name="delimiter">The separator bewteen existing text in the string builder, and the appended text (tabulator).</param>
	/// <param name="writeEmptyString">Indicates whether or not empty strings should be appended to the string builder.</param>
	public static void AppendDelimiter(this StringBuilder stringBuilder, ReadOnlySpan<Char> text, String delimiter = RpcCoreSharedConstants.STRING_TAB, Boolean writeEmptyString = true) {
		if ((writeEmptyString == true) || (text.IsEmpty == false)) {
			if (stringBuilder.Length != 0) {
				stringBuilder.Append(delimiter);
				stringBuilder.Append(text);
			} else {
				stringBuilder.Append(text);
			}
		}
	} // AppendDelimiter
	#endregion

} // RpcCoreExtensions
#endregion
