using System;
using System.Collections.Generic;
using System.Linq;
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
public partial class RpcCoreExtensions {

	extension (Memory<Char> value) {
		/// <summary>
		/// Slices the memory region by splitting it at each separator.
		/// </summary>
		/// <param name="separator">The separator characters.</param>
		/// <param name="options">The split options.</param>
		/// <returns>The memory region split into slices at each separator.</returns>
		public Memory<Char>[] SliceToArray(Memory<Char> separator, StringSplitOptions options) {
			return RpcCoreExtensions.Slices(value, separator, options).ToArray();
		} // SliceToArray

		/// <summary>
		/// Slices the memory region by splitting it at each separator.
		/// </summary>
		/// <param name="separator">The separator characters.</param>
		/// <param name="options">The split options.</param>
		/// <returns>The memory region split into slices at each separator.</returns>
		public IEnumerable<Memory<Char>> Slices(Memory<Char> separator, StringSplitOptions options) {
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

	}

	extension (ReadOnlyMemory<Char> value) {
		/// <summary>
		/// Slices the memory region by splitting it at each separator.
		/// </summary>
		/// <param name="separator">The separator characters.</param>
		/// <param name="options">The split options.</param>
		/// <returns>The memory region split into slices at each separator.</returns>
		public ReadOnlyMemory<Char>[] SliceToArray(ReadOnlyMemory<Char> separator, StringSplitOptions options) {
			return RpcCoreExtensions.Slices(value, separator, options).ToArray();
		} // SliceToArray

		/// <summary>
		/// Slices the memory region by splitting it at each separator.
		/// </summary>
		/// <param name="separator">The separator characters.</param>
		/// <param name="options">The split options.</param>
		/// <returns>The memory region split into slices at each separator.</returns>
		public IEnumerable<ReadOnlyMemory<Char>> Slices(ReadOnlyMemory<Char> separator, StringSplitOptions options) {
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

	}

} // RpcCoreExtensions
#endregion
