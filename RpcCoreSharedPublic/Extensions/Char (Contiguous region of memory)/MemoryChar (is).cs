using System;
namespace RpcScandinavia.Core.Shared;

#region RpcMemoryCharExtensions
//----------------------------------------------------------------------------------------------------------------------
// RpcMemoryCharExtensions.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// This part of the class contains useful extension methods for the contiguous region of arbitrary memory structs.
/// They are:
/// * <see cref="System.Memory{System.Char}" />
/// * <see cref="System.ReadOnlyMemory{System.Char}" />
/// * <see cref="System.Span{System.Char}" />
/// * <see cref="System.ReadOnlySpan{System.Char}" />
/// </summary>
public static partial class RpcCoreExtensions {

	//------------------------------------------------------------------------------------------------------------------
	// IsEmpty.
	// This is implemented in Memory<T> and Span<T>.
	//------------------------------------------------------------------------------------------------------------------

	#region IsEmptyOrWhiteSpace
	//------------------------------------------------------------------------------------------------------------------
	// IsEmptyOrWhiteSpace.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Indicates whether the value is empty or only contain whitespace characters.
	/// </summary>
	/// <param name="value">The characters.</param>
	/// <returns>True if the value is empty or only contain whitespace characters; otherwise, false.</returns>
	public static Boolean IsEmptyOrWhiteSpace(this Memory<Char> value) {
		return RpcCoreExtensions.IsEmptyOrWhiteSpace(value.Span);
	} // IsEmptyOrWhiteSpace

	/// <summary>
	/// Indicates whether the value is empty or only contain whitespace characters.
	/// </summary>
	/// <param name="value">The characters.</param>
	/// <returns>True if the value is empty or only contain whitespace characters; otherwise, false.</returns>
	public static Boolean IsEmptyOrWhiteSpace(this Span<Char> value) {
		for (Int32 index = 0; index < value.Length; index++) {
			if (Char.IsWhiteSpace(value[index]) == false) {
				return false;
			}
		}

		return true;
	} // IsEmptyOrWhiteSpace

	/// <summary>
	/// Indicates whether the value is empty or only contain whitespace characters.
	/// </summary>
	/// <param name="value">The characters.</param>
	/// <returns>True if the value is empty or only contain whitespace characters; otherwise, false.</returns>
	public static Boolean IsEmptyOrWhiteSpace(this ReadOnlyMemory<Char> value) {
		return RpcCoreExtensions.IsEmptyOrWhiteSpace(value.Span);
	} // IsEmptyOrWhiteSpace

	/// <summary>
	/// Indicates whether the value is empty or only contain whitespace characters.
	/// </summary>
	/// <param name="value">The characters.</param>
	/// <returns>True if the value is empty or only contain whitespace characters; otherwise, false.</returns>
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
