namespace RpcScandinavia.Core.Shared;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

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
	public static ReadOnlyMemory<Char>[] Slices(this ReadOnlyMemory<Char> value, ReadOnlyMemory<Char> separator, StringSplitOptions options) {
		// Validate
		if (separator.Length == 0) {
			throw new ArgumentException(nameof(separator));
		}

		// Return.
		if (value.Length == 0) {
			return Array.Empty<ReadOnlyMemory<Char>>();
		}
/* do not work, 30+ milliseconds
		// Split.
		List<ReadOnlyMemory<Char>> result = new List<ReadOnlyMemory<Char>>();
		Int32 index;
		ReadOnlyMemory<Char> part;
		do {
			// Get the part, and remove the part and separator from the value.
			index = value.Span.IndexOf(separator.Span);
			if (index > -1) {
				part = value.Slice(0, index);
				value = value.Slice(index + separator.Length);
			} else {
				// Last part.
				part = value;
				value = ReadOnlyMemory<Char>.Empty;
			}

			// Trim the part.
			if ((part.Length > 0) && (options.HasFlag(StringSplitOptions.TrimEntries) == true)) {
				part = part.Trim();
			}

			// Add the part.
			if ((part.Length > 0) || ((part.Length == 0) && (options.HasFlag(StringSplitOptions.RemoveEmptyEntries) == false))) {
				result.Add(part);
			}
		} while (value.Length > 0);

		return result.ToArray();
*/

		// Split.
		List<ReadOnlyMemory<Char>> result = new List<ReadOnlyMemory<Char>>();
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
				result.Add(part);
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
			result.Add(value);
		}

		return result.ToArray();
	} // Slices
	#endregion

} // RpcCoreExtensions
#endregion
