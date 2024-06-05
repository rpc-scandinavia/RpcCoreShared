using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
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

	#region SliceToArray
	//------------------------------------------------------------------------------------------------------------------
	// SliceToArray.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Slices the value by splitting it at each separator.
	/// </summary>
	/// <param name="value">The characters.</param>
	/// <param name="separator">The separator characters.</param>
	/// <param name="options">The split options.</param>
	/// <returns>The value split into slices at each separator.</returns>
	public static Memory<Char>[] SliceToArray(this Memory<Char> value, Memory<Char> separator, StringSplitOptions options) {
		return RpcCoreExtensions.Slices(value, separator, options).ToArray();
	} // SliceToArray

	/// <summary>
	/// Slices the value by splitting it at each separator.
	/// </summary>
	/// <param name="value">The characters.</param>
	/// <param name="separator">The separator characters.</param>
	/// <param name="options">The split options.</param>
	/// <returns>The value split into slices at each separator.</returns>
	public static ReadOnlyMemory<Char>[] SliceToArray(this ReadOnlyMemory<Char> value, ReadOnlyMemory<Char> separator, StringSplitOptions options) {
		return RpcCoreExtensions.Slices(value, separator, options).ToArray();
	} // SliceToArray
	#endregion

	#region Slices
	//------------------------------------------------------------------------------------------------------------------
	// Slices.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Slices the value by splitting it at each separator.
	/// </summary>
	/// <param name="value">The characters.</param>
	/// <param name="separator">The separator characters.</param>
	/// <param name="options">The split options.</param>
	/// <returns>The value split into slices at each separator.</returns>
	public static IEnumerable<Memory<Char>> Slices(this Memory<Char> value, Memory<Char> separator, StringSplitOptions options) {
		// Validate
		if (separator.Length == 0) {
			throw new ArgumentException(nameof(separator));
		}

		// Return.
		if (value.Length > 0) {
			// Split.
			Int32 index = value.Span.IndexOf(separator.Span);
			while (index > -1) {
				// Get the part.
				Memory<Char> part = value.Slice(0, index);

				// Trim the part.
				if ((part.Length > 0) && (options.HasFlag(StringSplitOptions.TrimEntries) == true)) {
					part = part.Trim();
				}

				// Add the part.
				if ((part.Length > 0) || (options.HasFlag(StringSplitOptions.RemoveEmptyEntries) == false)) {
					yield return part;
				}

				// Remove the separator.
				value = value.Slice(index + separator.Length);
				index = value.Span.IndexOf(separator.Span);
			}

			// Rest.
			// Trim the part.
			if ((value.Length > 0) && (options.HasFlag(StringSplitOptions.TrimEntries) == true)) {
				value = value.Trim();
			}

			// Add the part.
			if ((value.Length > 0) || (options.HasFlag(StringSplitOptions.RemoveEmptyEntries) == false)) {
				yield return value;
			}
		}
	} // Slices

	/// <summary>
	/// Slices the value by splitting it at each separator.
	/// </summary>
	/// <param name="value">The characters.</param>
	/// <param name="separator">The separator characters.</param>
	/// <param name="options">The split options.</param>
	/// <returns>The value split into slices at each separator.</returns>
	public static IEnumerable<ReadOnlyMemory<Char>> Slices(this ReadOnlyMemory<Char> value, ReadOnlyMemory<Char> separator, StringSplitOptions options) {
		// Validate
		if (separator.Length == 0) {
			throw new ArgumentException(nameof(separator));
		}

		// Return.
		if (value.Length > 0) {
			// Split.
			Int32 index = value.Span.IndexOf(separator.Span);
			while (index > -1) {
				// Get the part.
				ReadOnlyMemory<Char> part = value.Slice(0, index);

				// Trim the part.
				if ((part.Length > 0) && (options.HasFlag(StringSplitOptions.TrimEntries) == true)) {
					part = part.Trim();
				}

				// Add the part.
				if ((part.Length > 0) || (options.HasFlag(StringSplitOptions.RemoveEmptyEntries) == false)) {
					yield return part;
				}

				// Remove the separator.
				value = value.Slice(index + separator.Length);
				index = value.Span.IndexOf(separator.Span);
			}

			// Rest.
			// Trim the part.
			if ((value.Length > 0) && (options.HasFlag(StringSplitOptions.TrimEntries) == true)) {
				value = value.Trim();
			}

			// Add the part.
			if ((value.Length > 0) || (options.HasFlag(StringSplitOptions.RemoveEmptyEntries) == false)) {
				yield return value;
			}
		}
	} // Slices
	#endregion

} // RpcCoreExtensions
#endregion
