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
public partial class RpcCoreExtensions {

	/// <summary>
	/// Modify a <see cref="System.Text.StringBuilder" />.
	/// </summary>
	/// <param name="stringBuilder">The string builder to modify.</param>
	extension (StringBuilder stringBuilder) {

		#region Prepend
		//------------------------------------------------------------------------------------------------------------------
		// Prepend.
		//------------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Prepends (insert at the beginning) a copy of the string to the string builder, using the separator (delimiter)
		/// between the existing text in the string builder and the prepended text.
		/// </summary>
		/// <param name="text">The string to prepend to the string builder.</param>
		/// <param name="delimiter">The separator between existing text in the string builder, and the prepended text (tabulator).</param>
		/// <param name="writeEmptyString">Indicates whether empty strings should be prepended to the string builder.</param>
		public void PrependDelimiter(String text, String delimiter = RpcCoreSharedConstants.STRING_TAB, Boolean writeEmptyString = true) {
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
		/// between the existing text in the string builder and the prepended text.
		/// </summary>
		/// <param name="text">The string to prepend to the string builder.</param>
		/// <param name="delimiter">The separator between existing text in the string builder, and the prepended text (tabulator).</param>
		/// <param name="writeEmptyString">Indicates whether empty strings should be prepended to the string builder.</param>
		public void PrependDelimiter(Memory<Char> text, String delimiter = RpcCoreSharedConstants.STRING_TAB, Boolean writeEmptyString = true) {
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
		/// between the existing text in the string builder and the prepended text.
		/// </summary>
		/// <param name="text">The string to prepend to the string builder.</param>
		/// <param name="delimiter">The separator between existing text in the string builder, and the prepended text (tabulator).</param>
		/// <param name="writeEmptyString">Indicates whether empty strings should be prepended to the string builder.</param>
		public void PrependDelimiter(ReadOnlyMemory<Char> text, String delimiter = RpcCoreSharedConstants.STRING_TAB, Boolean writeEmptyString = true) {
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
		/// between the existing text in the string builder and the prepended text.
		/// </summary>
		/// <param name="text">The string to prepend to the string builder.</param>
		/// <param name="delimiter">The separator between existing text in the string builder, and the prepended text (tabulator).</param>
		/// <param name="writeEmptyString">Indicates whether empty strings should be prepended to the string builder.</param>
		public void PrependDelimiter(Span<Char> text, String delimiter = RpcCoreSharedConstants.STRING_TAB, Boolean writeEmptyString = true) {
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
		/// between the existing text in the string builder and the prepended text.
		/// </summary>
		/// <param name="text">The string to prepend to the string builder.</param>
		/// <param name="delimiter">The separator between existing text in the string builder, and the prepended text (tabulator).</param>
		/// <param name="writeEmptyString">Indicates whether empty strings should be prepended to the string builder.</param>
		public void PrependDelimiter(ReadOnlySpan<Char> text, String delimiter = RpcCoreSharedConstants.STRING_TAB, Boolean writeEmptyString = true) {
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
		//--------------------------------------------------------------------------------------------------------------
		// Append.
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Appends a copy of the string to the string builder, using the separator (delimiter) between the existing text
		/// in the string builder and the appended text.
		/// </summary>
		/// <param name="text">The string to append to the string builder.</param>
		/// <param name="delimiter">The separator between existing text in the string builder, and the appended text (tabulator).</param>
		/// <param name="writeEmptyString">Indicates whether empty strings should be appended to the string builder.</param>
		public void AppendDelimiter(String text, String delimiter = RpcCoreSharedConstants.STRING_TAB, Boolean writeEmptyString = true) {
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
		/// <param name="text">The characters to append to the string builder.</param>
		/// <param name="delimiter">The separator between existing text in the string builder, and the appended text (tabulator).</param>
		/// <param name="writeEmptyString">Indicates whether empty strings should be appended to the string builder.</param>
		public void AppendDelimiter(Memory<Char> text, String delimiter = RpcCoreSharedConstants.STRING_TAB, Boolean writeEmptyString = true) {
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
		/// <param name="text">The characters to append to the string builder.</param>
		/// <param name="delimiter">The separator between existing text in the string builder, and the appended text (tabulator).</param>
		/// <param name="writeEmptyString">Indicates whether empty strings should be appended to the string builder.</param>
		public void AppendDelimiter(ReadOnlyMemory<Char> text, String delimiter = RpcCoreSharedConstants.STRING_TAB, Boolean writeEmptyString = true) {
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
		/// <param name="text">The characters to append to the string builder.</param>
		/// <param name="delimiter">The separator between existing text in the string builder, and the appended text (tabulator).</param>
		/// <param name="writeEmptyString">Indicates whether empty strings should be appended to the string builder.</param>
		public void AppendDelimiter(Span<Char> text, String delimiter = RpcCoreSharedConstants.STRING_TAB, Boolean writeEmptyString = true) {
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
		/// <param name="text">The characters to append to the string builder.</param>
		/// <param name="delimiter">The separator between existing text in the string builder, and the appended text (tabulator).</param>
		/// <param name="writeEmptyString">Indicates whether empty strings should be appended to the string builder.</param>
		public void AppendDelimiter(ReadOnlySpan<Char> text, String delimiter = RpcCoreSharedConstants.STRING_TAB, Boolean writeEmptyString = true) {
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

	}

} // RpcCoreExtensions
#endregion
