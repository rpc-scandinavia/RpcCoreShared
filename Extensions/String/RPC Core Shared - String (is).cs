namespace RpcScandinavia.Core.Shared;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

#region RpcCoreExtensions
//----------------------------------------------------------------------------------------------------------------------
// RpcCoreExtensions.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// This part of the class contains usefull extension methods for the System.String class.
/// </summary>
static public partial class RpcCoreExtensions {

	#region IsNullOrEmpty (from String)
	//------------------------------------------------------------------------------------------------------------------
	// IsNullOrEmpty (from String).
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Indicates whether a specified string value is null or empty.
	/// A string containing one or more white-space characters are not considered empty.
	///
	/// White-space characters are defined by the Unicode standard. Any character that returns a value of
	/// true when it is passed to the Char.IsWhiteSpace method, is considered a white-space character.
	/// </summary>
	/// <param name="value">The value to verify.</param>
	/// <returns>True if the value is null or empty.</returns>
	static public Boolean IsNullOrEmpty(this String value) {
		return String.IsNullOrEmpty(value);
	} // IsNullOrEmpty
	#endregion

	#region IsNullOrWhiteSpace (from String)
	//------------------------------------------------------------------------------------------------------------------
	// IsNullOrWhiteSpace (from String).
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Indicates whether a specified string value is null, empty or consists only of white-space characters.
	///
	/// White-space characters are defined by the Unicode standard. The IsNullOrWhiteSpace method interprets
	/// any character that returns a value of true when it is passed to the Char.IsWhiteSpace method as a
	/// white-space character.
	/// </summary>
	/// <param name="value">The value to verify.</param>
	/// <returns>True if the value is null, empty or consists only of white-space characters.</returns>
	static public Boolean IsNullOrWhiteSpace(this String value) {
		return String.IsNullOrWhiteSpace(value);
	} // IsNullOrWhiteSpace
	#endregion

} // RpcCoreExtensions
#endregion
