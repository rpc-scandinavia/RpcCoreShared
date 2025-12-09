using System;
namespace RpcScandinavia.Core.Shared;

#region RpcMemoryCharExtensions
//----------------------------------------------------------------------------------------------------------------------
// RpcMemoryCharExtensions.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// This part of the class contains useful extension methods for the contiguous region of arbitrary memory structs.
/// They are:
/// * <see cref="System.Memory{Char}" />
/// * <see cref="System.ReadOnlyMemory{Char}" />
/// * <see cref="System.Span{Char}" />
/// * <see cref="System.ReadOnlySpan{Char}" />
/// </summary>
public static partial class RpcCoreExtensions {

	extension (Memory<Char> value) {
		/// <summary>
		/// Indicates whether the memory region is empty or only contain whitespace characters.
		/// </summary>
		/// <returns>True if the memory region is empty or only contain whitespace characters; otherwise, false.</returns>
		public Boolean IsEmptyOrWhiteSpace() {
			return RpcCoreExtensions.IsEmptyOrWhiteSpace(value.Span);
		} // IsEmptyOrWhiteSpace

	}

	extension (Span<Char> value) {
		/// <summary>
		/// Indicates whether the memory region is empty or only contain whitespace characters.
		/// </summary>
		/// <returns>True if the memory region is empty or only contain whitespace characters; otherwise, false.</returns>
		public Boolean IsEmptyOrWhiteSpace() {
			for (Int32 index = 0; index < value.Length; index++) {
				if (Char.IsWhiteSpace(value[index]) == false) {
					return false;
				}
			}

			return true;
		} // IsEmptyOrWhiteSpace

	}

	extension (ReadOnlyMemory<Char> value) {
		/// <summary>
		/// Indicates whether the memory region is empty or only contain whitespace characters.
		/// </summary>
		/// <returns>True if the memory region is empty or only contain whitespace characters; otherwise, false.</returns>
		public Boolean IsEmptyOrWhiteSpace() {
			return RpcCoreExtensions.IsEmptyOrWhiteSpace(value.Span);
		} // IsEmptyOrWhiteSpace

	}

	extension (ReadOnlySpan<Char> value) {
		/// <summary>
		/// Indicates whether the memory region is empty or only contain whitespace characters.
		/// </summary>
		/// <returns>True if the memory region is empty or only contain whitespace characters; otherwise, false.</returns>
		public Boolean IsEmptyOrWhiteSpace() {
			for (Int32 index = 0; index < value.Length; index++) {
				if (Char.IsWhiteSpace(value[index]) == false) {
					return false;
				}
			}

			return true;
		} // IsEmptyOrWhiteSpace

	}

} // RpcCoreExtensions
#endregion
