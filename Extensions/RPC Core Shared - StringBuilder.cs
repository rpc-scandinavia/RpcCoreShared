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
	/// <param name="delim">The separator bewteen existing text in the string builder, and the appended text.</param>
	/// <param name="writeEmptyString">Indicates whether or not empty strings should be appended to the string builder.</param>
	public static void AppendDelim(this StringBuilder stringBuilder, String text, String delim = "\t", Boolean writeEmptyString = true) {
		if ((writeEmptyString == true) || (text.IsNullOrWhiteSpace() == false)) {
			if (stringBuilder.Length != 0) {
				stringBuilder.Append(delim + text);
			} else {
				stringBuilder.Append(text);
			}
		}
	} // AppendDelim
	#endregion

} // RpcCoreExtensions
#endregion
