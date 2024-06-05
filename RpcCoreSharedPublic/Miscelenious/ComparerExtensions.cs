using System;
using System.Collections.Generic;
namespace RpcScandinavia.Core.Shared;

#region RpcComparerExtensions
//----------------------------------------------------------------------------------------------------------------------
// RpcComparerExtensions.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// This class contains useful extension methods for <see cref="System.Collections.Generic.IComparer{T}" />.
/// </summary>
/// <typeparam name="T">The type to compare.</typeparam>
public static class RpcComparerExtensions {

	/// <summary>
	/// Chain a <see cref="System.Collections.Generic.IComparer{T}" /> with another, using the <see cref="RpcScandinavia.Core.Shared.RpcComparerChainNode{T}" />.
	/// </summary>
	/// <param name="first">The first comparer.</param>
	/// <param name="next">The next comparer in the chain, used when the first comparer return zero.</param>
	/// <typeparam name="T">The type to compare.</typeparam>
	/// <returns>The comparer.</returns>
	public static IComparer<T> Then<T>(this IComparer<T> first, IComparer<T> next) {
		return new RpcComparerChainNode<T>(first, next);
	} // Then

	/// <summary>
	/// Reverse a <see cref="System.Collections.Generic.IComparer{T}" />, using the <see cref="RpcScandinavia.Core.Shared.RpcComparerReverse{T}" />.
	/// </summary>
	/// <param name="comparer">The comparer.</param>
	/// <typeparam name="T">The type to compare.</typeparam>
	/// <returns>The comparer.</returns>
	public static IComparer<T> Reverse<T>(this IComparer<T> comparer) {
		if (comparer is RpcComparerReverse<T> reverse) {
			return reverse.comparer;
		} else {
			return new RpcComparerReverse<T>(comparer);
		}
	} // Reverse

} // RpcComparerExtensions
#endregion
