using System;
using System.Collections.Generic;
namespace RpcScandinavia.Core.Shared;

#region RpcComparerReverse
//----------------------------------------------------------------------------------------------------------------------
// RpcComparerReverse.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// This class is used to reverse the result of a <see cref="System.Collections.Generic.IComparer{T}" />.
/// </summary>
/// <typeparam name="T">The type to compare.</typeparam>
public class RpcComparerReverse<T> : IComparer<T> {
	internal readonly IComparer<T> comparer;

	/// <summary>
	/// Initialize a new reverse comparer.
	/// </summary>
	/// <param name="comparer">The comparer.</param>
	public RpcComparerReverse(IComparer<T> comparer) {
		// Validate.
		if (comparer == null) {
			throw new NullReferenceException(nameof(comparer));
		}

		// Initialize.
		this.comparer = comparer;
	} // RpcComparerReverse

	/// <summary>
	/// Compares X and Y using the comparer, but reverses the result.
	/// </summary>
	/// <param name="x">The X value to compare.</param>
	/// <param name="y">The Y value to compare.</param>
	/// <returns>The reversed comparison made by the comparer.</returns>
	public Int32 Compare(T x, T y) {
		return this.comparer.Compare(y, x);
	} // Compare

} // RpcComparerReverse
#endregion
