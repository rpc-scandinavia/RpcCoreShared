using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace RpcScandinavia.Core.Shared;

#region RpcCoreExtensions
//----------------------------------------------------------------------------------------------------------------------
// RpcCoreExtensions.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// This part of the class contains useful extension methods for the <see cref="System.String" /> class.
/// </summary>
public static partial class RpcCoreExtensions {

	#region IsEmpty
	//------------------------------------------------------------------------------------------------------------------
	// IsEmpty.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Indicates whether a specified string value is empty.
	/// A string containing one or more white-space characters are not considered empty.
	/// </summary>
	/// <param name="value">The value to verify.</param>
	/// <param name="throwException">Whether to throw a exception if the value is null (false).</param>
	/// <returns>True if the value is empty.</returns>
	public static Boolean IsEmpty(this String value, Boolean throwException = false) {
		// Validate.
		if (value == null) {
			if (throwException == true) {
				throw new ArgumentNullException(nameof(value));
			} else {
				return true;
			}
		}

		return (value.Length == 0);
	} // IsEmpty
	#endregion

	#region IsNullOrEmpty
	//------------------------------------------------------------------------------------------------------------------
	// IsNullOrEmpty.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Indicates whether a specified string value is null or empty.
	/// A string containing one or more white-space characters are not considered empty.
	/// </summary>
	/// <param name="value">The value to verify.</param>
	/// <returns>True if the value is null or empty.</returns>
	public static Boolean IsNullOrEmpty(this String value) {
		return ((value == null) || (value.Length == 0));
	} // IsNullOrEmpty
	#endregion

	#region IsNullOrWhiteSpace
	//------------------------------------------------------------------------------------------------------------------
	// IsNullOrWhiteSpace.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Indicates whether a specified string value is null, empty or consists only of white-space characters.
	///
	/// White-space characters are defined by the Unicode standard. The IsNullOrWhiteSpace method interprets any
	/// character that returns a value of true when it is passed to the <see cref="System.Char.IsWhiteSpace" /> method,
	/// as a white-space character.
	/// </summary>
	/// <param name="value">The value to verify.</param>
	/// <returns>True if the value is null, empty or consists only of white-space characters.</returns>
	public static Boolean IsNullOrWhiteSpace(this String value) {
		if (value == null) {
			return true;
		}

		for(Int32 index = 0; index < value.Length; index++) {
			if (Char.IsWhiteSpace(value[index]) == false) {
				return false;
			}
		}

		return true;
	} // IsNullOrWhiteSpace
	#endregion

} // RpcCoreExtensions
#endregion
