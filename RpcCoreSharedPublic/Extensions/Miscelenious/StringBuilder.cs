using System;
using System.Text;
namespace RpcScandinavia.Core.Shared;

#region RpcCoreExtensions
//----------------------------------------------------------------------------------------------------------------------
// RpcCoreExtensions.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// This part of the class contains useful extension methods for the <see cref="System.Text.StringBuilder" /> class.
/// </summary>
public static partial class RpcCoreExtensions {

	#region Prepend
	//------------------------------------------------------------------------------------------------------------------
	// Prepend.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Prepends (insert at the beginning) a copy of the string to the string builder, using the separator (delimiter)
	/// between the existing text in the string builder and the Prepended text.
	/// </summary>
	/// <param name="stringBuilder">The string builder.</param>
	/// <param name="text">The string to Prepend to the string builder.</param>
	/// <param name="delimiter">The separator bewteen existing text in the string builder, and the Prepended text (tabulator).</param>
	/// <param name="writeEmptyString">Indicates whether or not empty strings should be Prepended to the string builder.</param>
	public static void PrependDelimiter(this StringBuilder stringBuilder, String text, String delimiter = RpcCoreSharedConstants.STRING_TAB, Boolean writeEmptyString = true) {
		if ((writeEmptyString == true) || (text.IsNullOrWhiteSpace() == false)) {
			if (stringBuilder.Length != 0) {
				stringBuilder.Insert(0, delimiter);
				stringBuilder.Insert(0, text);
			} else {
				stringBuilder.Insert(0, text);
			}
		}
	} // PrependDelimiter

	/// <summary>
	/// Prepends (insert at the beginning) a copy of the string to the string builder, using the separator (delimiter)
	/// between the existing text in the string builder and the Prepended text.
	/// </summary>
	/// <param name="stringBuilder">The string builder.</param>
	/// <param name="text">The string to Prepend to the string builder.</param>
	/// <param name="delimiter">The separator bewteen existing text in the string builder, and the Prepended text (tabulator).</param>
	/// <param name="writeEmptyString">Indicates whether empty strings should be Prepended to the string builder.</param>
	public static void PrependDelimiter(this StringBuilder stringBuilder, Memory<Char> text, String delimiter = RpcCoreSharedConstants.STRING_TAB, Boolean writeEmptyString = true) {
		if ((writeEmptyString == true) || (text.IsEmpty == false)) {
			if (stringBuilder.Length != 0) {
				stringBuilder.Insert(0, delimiter);
				stringBuilder.Insert(0, text);
			} else {
				stringBuilder.Insert(0, text);
			}
		}
	} // PrependDelimiter

	/// <summary>
	/// Prepends (insert at the beginning) a copy of the string to the string builder, using the separator (delimiter)
	/// between the existing text in the string builder and the Prepended text.
	/// </summary>
	/// <param name="stringBuilder">The string builder.</param>
	/// <param name="text">The string to Prepend to the string builder.</param>
	/// <param name="delimiter">The separator bewteen existing text in the string builder, and the Prepended text (tabulator).</param>
	/// <param name="writeEmptyString">Indicates whether empty strings should be Prepended to the string builder.</param>
	public static void PrependDelimiter(this StringBuilder stringBuilder, ReadOnlyMemory<Char> text, String delimiter = RpcCoreSharedConstants.STRING_TAB, Boolean writeEmptyString = true) {
		if ((writeEmptyString == true) || (text.IsEmpty == false)) {
			if (stringBuilder.Length != 0) {
				stringBuilder.Insert(0, delimiter);
				stringBuilder.Insert(0, text);
			} else {
				stringBuilder.Insert(0, text);
			}
		}
	} // PrependDelimiter

	/// <summary>
	/// Prepends (insert at the beginning) a copy of the string to the string builder, using the separator (delimiter)
	/// between the existing text in the string builder and the Prepended text.
	/// </summary>
	/// <param name="stringBuilder">The string builder.</param>
	/// <param name="text">The string to Prepend to the string builder.</param>
	/// <param name="delimiter">The separator bewteen existing text in the string builder, and the Prepended text (tabulator).</param>
	/// <param name="writeEmptyString">Indicates whether empty strings should be Prepended to the string builder.</param>
	public static void PrependDelimiter(this StringBuilder stringBuilder, Span<Char> text, String delimiter = RpcCoreSharedConstants.STRING_TAB, Boolean writeEmptyString = true) {
		if ((writeEmptyString == true) || (text.IsEmpty == false)) {
			if (stringBuilder.Length != 0) {
				stringBuilder.Insert(0, delimiter);
				stringBuilder.Insert(0, text);
			} else {
				stringBuilder.Insert(0, text);
			}
		}
	} // PrependDelimiter

	/// <summary>
	/// Prepends (insert at the beginning) a copy of the string to the string builder, using the separator (delimiter)
	/// between the existing text in the string builder and the Prepended text.
	/// </summary>
	/// <param name="stringBuilder">The string builder.</param>
	/// <param name="text">The string to Prepend to the string builder.</param>
	/// <param name="delimiter">The separator bewteen existing text in the string builder, and the Prepended text (tabulator).</param>
	/// <param name="writeEmptyString">Indicates whether empty strings should be Prepended to the string builder.</param>
	public static void PrependDelimiter(this StringBuilder stringBuilder, ReadOnlySpan<Char> text, String delimiter = RpcCoreSharedConstants.STRING_TAB, Boolean writeEmptyString = true) {
		if ((writeEmptyString == true) || (text.IsEmpty == false)) {
			if (stringBuilder.Length != 0) {
				stringBuilder.Insert(0, delimiter);
				stringBuilder.Insert(0, text);
			} else {
				stringBuilder.Insert(0, text);
			}
		}
	} // PrependDelimiter
	#endregion

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
	/// <param name="writeEmptyString">Indicates whether empty strings should be appended to the string builder.</param>
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
	/// <param name="writeEmptyString">Indicates whether empty strings should be appended to the string builder.</param>
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
	/// <param name="writeEmptyString">Indicates whether empty strings should be appended to the string builder.</param>
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
	/// <param name="writeEmptyString">Indicates whether empty strings should be appended to the string builder.</param>
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
	/// <param name="writeEmptyString">Indicates whether empty strings should be appended to the string builder.</param>
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
