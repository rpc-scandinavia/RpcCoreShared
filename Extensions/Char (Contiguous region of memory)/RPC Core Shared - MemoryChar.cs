namespace RpcScandinavia.Core.Shared;
using System;
using System.Text;

#region RpcMemoryCharExtensions
//----------------------------------------------------------------------------------------------------------------------
// RpcMemoryCharExtensions.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// This part of the class contains usefull extension methods for the <see cref="System.Memory{System.Char}" /> struct,
/// the <see cref="System.ReadOnlyMemory{System.Char}" /> struct, the <see cref="System.Span{System.Char}" /> and
/// the <see cref="System.ReadOnlySpan{System.Char}" /> struct.
/// </summary>
static public partial class RpcCoreExtensions {

	#region IsEmptyOrWhiteSpace
	//------------------------------------------------------------------------------------------------------------------
	// IsEmptyOrWhiteSpace.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Indicates whether the current instance is empty or only contain whitespace characters.
	/// </summary>
	/// <param name="value">The characters.</param>
	/// <returns>True if the current instance is empty or only contain whitespace characters; otherwise, false.</returns>
	public static Boolean IsEmptyOrWhiteSpace(this Memory<Char> value) {
		return RpcCoreExtensions.IsEmptyOrWhiteSpace(value.Span);
	} // IsEmptyOrWhiteSpace

	/// <summary>
	/// Indicates whether the current instance is empty or only contain whitespace characters.
	/// </summary>
	/// <param name="value">The characters.</param>
	/// <returns>True if the current instance is empty or only contain whitespace characters; otherwise, false.</returns>
	public static Boolean IsEmptyOrWhiteSpace(this Span<Char> value) {
		for (Int32 index = 0; index < value.Length; index++) {
			if (Char.IsWhiteSpace(value[index]) == false) {
				return false;
			}
		}

		return true;
	} // IsEmptyOrWhiteSpace

	/// <summary>
	/// Indicates whether the current instance is empty or only contain whitespace characters.
	/// </summary>
	/// <param name="value">The characters.</param>
	/// <returns>True if the current instance is empty or only contain whitespace characters; otherwise, false.</returns>
	public static Boolean IsEmptyOrWhiteSpace(this ReadOnlyMemory<Char> value) {
		return RpcCoreExtensions.IsEmptyOrWhiteSpace(value.Span);
	} // IsEmptyOrWhiteSpace

	/// <summary>
	/// Indicates whether the current instance is empty or only contain whitespace characters.
	/// </summary>
	/// <param name="value">The characters.</param>
	/// <returns>True if the current instance is empty or only contain whitespace characters; otherwise, false.</returns>
	public static Boolean IsEmptyOrWhiteSpace(this ReadOnlySpan<Char> value) {
		for (Int32 index = 0; index < value.Length; index++) {
			if (Char.IsWhiteSpace(value[index]) == false) {
				return false;
			}
		}

		return true;
	} // IsEmptyOrWhiteSpace
	#endregion

} // RpcCoreExtensions
#endregion
