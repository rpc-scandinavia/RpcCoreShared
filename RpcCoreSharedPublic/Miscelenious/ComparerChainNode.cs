using System;
using System.Collections.Generic;
namespace RpcScandinavia.Core.Shared;

#region RpcComparerChainNode
//----------------------------------------------------------------------------------------------------------------------
// RpcComparerChainNode.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// This class is used to chain multiple <see cref="System.Collections.Generic.IComparer{T}" />.
/// </summary>
/// <typeparam name="T">The type to compare.</typeparam>
public class RpcComparerChainNode<T> : IComparer<T> {
	private readonly IComparer<T> first;
	private readonly IComparer<T> next;

	/// <summary>
	/// Initialize a new node in a <see cref="System.Collections.Generic.IComparer{T}" /> chain.
	/// </summary>
	/// <param name="first">The first comparer.</param>
	/// <param name="next">The next comparer in the chain, used when the first comparer return zero.</param>
	public RpcComparerChainNode(IComparer<T> first, IComparer<T> next) {
		// Validate.
		if (first == null) {
			throw new NullReferenceException(nameof(first));
		}
		if (next == null) {
			throw new NullReferenceException(nameof(next));
		}

		// Initialize.
		this.first = first;
		this.next = next;
	} // RpcComparerChainNode

	/// <summary>
	/// Compares X and Y using the first comparer. It that return zero, then the result from the next comparer is returned.
	/// </summary>
	/// <param name="x">The X value to compare.</param>
	/// <param name="y">The Y value to compare.</param>
	/// <returns>The comparison made either by the first comparer og the next comparer.</returns>
	public Int32 Compare(T x, T y) {
		Int32 comparison = this.first.Compare(x, y);
		if (comparison != 0) {
			return comparison;
		} else {
			return this.next.Compare(x, y);
		}
	} // Compare

} // RpcComparerChainNode
#endregion
