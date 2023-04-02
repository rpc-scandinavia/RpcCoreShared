namespace RpcScandinavia.Core.Shared;
using System;

#region RpcCoreExtensions
//----------------------------------------------------------------------------------------------------------------------
// RpcCoreExtensions.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// This part of the class contains usefull extension methods for the <see cref="System.String" /> class.
/// </summary>
static public partial class RpcCoreExtensions {

	#region NotNull (from String)
	//------------------------------------------------------------------------------------------------------------------
	// NotNull (from String)
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the value or String.Empty if the value is null.
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

	#region NotNullOrEmpty (from String)
	//------------------------------------------------------------------------------------------------------------------
	// NotNullOrEmpty (from String)
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the value or the default value if the value is either null or empty.
	/// </summary>
	/// <param name="value">The value to verify.</param>
	/// <param name="defaultValue">The value to return when the value is either null or empty.</param>
	/// <returns>The value or the default value if the value is either null or empty.</returns>
	public static String NotNullOrEmpty(this String value, String defaultValue) {
		if (String.IsNullOrEmpty(value) == true) {
			return defaultValue;
		} else {
			return value;
		}
	} // NotNullOrEmpty
	#endregion

	#region NotNullOrWhiteSpace (from String)
	//------------------------------------------------------------------------------------------------------------------
	// NotNullOrWhiteSpace (from String)
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the value or the default value if the value is either null, empty or only contains white-space characters.
	/// </summary>
	/// <param name="value">The value to verify.</param>
	/// <param name="defaultValue">The value to return when the value is either null, empty or only contains white-space characters.</param>
	/// <returns>The value or the default value if the value is either null, empty or only contains white-space characters.</returns>
	public static String NotNullOrWhiteSpace(this String value, String defaultValue) {
		if (String.IsNullOrWhiteSpace(value) == true) {
			return defaultValue;
		} else {
			return value;
		}
	} // NotNullOrWhiteSpace
	#endregion

} // RpcCoreExtensions
#endregion
