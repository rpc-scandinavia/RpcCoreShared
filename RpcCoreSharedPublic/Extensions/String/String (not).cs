namespace RpcScandinavia.Core.Shared;
using System;

#region RpcCoreExtensions
//----------------------------------------------------------------------------------------------------------------------
// RpcCoreExtensions.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// This part of the class contains useful extension methods for the <see cref="System.String" /> class.
/// </summary>
public static partial class RpcCoreExtensions {

	#region NotNull
	//------------------------------------------------------------------------------------------------------------------
	// NotNull.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the value or <see cref="System.String.Empty" /> if the value is null.
	/// </summary>
	/// <param name="value">The value to verify.</param>
	/// <returns>The value or String.Empty if the value is null.</returns>
	public static String NotNull(this String value) {
		if (value == null) {
			return String.Empty;
		} else {
			return value;
		}
	} // NotNull

	/// <summary>
	/// Gets the value or the default value if the value is null.
	/// </summary>
	/// <param name="value">The value to verify.</param>
	/// <param name="defaultValue">The value to return when the value is null.</param>
	/// <returns>The value or the default value if the value is null.</returns>
	public static String NotNull(this String value, String defaultValue) {
		if (value == null) {
			return defaultValue;
		} else {
			return value;
		}
	} // NotNull
	#endregion

	#region NotNullOrEmpty
	//------------------------------------------------------------------------------------------------------------------
	// NotNullOrEmpty.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the value or the default value if the value is either null ot empty.
	/// </summary>
	/// <param name="value">The value to verify.</param>
	/// <param name="defaultValue">The value to return when the value is either null or empty.</param>
	/// <returns>The value or the default value if the value is either null or empty.</returns>
	public static String NotNullOrEmpty(this String value, String defaultValue) {
		if ((value == null) || (value.Length == 0)) {
			return defaultValue;
		} else {
			return value;
		}
	} // NotNullOrEmpty
	#endregion

	#region NotNullOrWhiteSpace
	//------------------------------------------------------------------------------------------------------------------
	// NotNullOrWhiteSpace.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the value or the default value if the value is either null, empty or only contains white-space characters.
	/// </summary>
	/// <param name="value">The value to verify.</param>
	/// <param name="defaultValue">The value to return when the value is either null, empty or only contains white-space characters.</param>
	/// <returns>The value or the default value if the value is either null, empty or only contains white-space characters.</returns>
	public static String NotNullOrWhiteSpace(this String value, String defaultValue) {
		if (value == null) {
			return defaultValue;
		}

		for(Int32 index = 0; index < value.Length; index++) {
			if (Char.IsWhiteSpace(value[index]) == false) {
				return value;
			}
		}

		return defaultValue;
	} // NotNullOrWhiteSpace
	#endregion

} // RpcCoreExtensions
#endregion
