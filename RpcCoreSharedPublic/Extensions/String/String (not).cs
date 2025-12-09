using System;
namespace RpcScandinavia.Core.Shared;

#region RpcCoreExtensions
//----------------------------------------------------------------------------------------------------------------------
// RpcCoreExtensions.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// This part of the class contains useful extension methods for the <see cref="System.String" /> class.
/// </summary>
public partial class RpcCoreExtensions {

	extension (String value) {

		#region NotNull
		//--------------------------------------------------------------------------------------------------------------
		// NotNull.
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the <see cref="System.String" /> or <see cref="System.String.Empty" /> if the string is null.
		/// </summary>
		/// <returns>The string or String.Empty if the string is null.</returns>
		public String NotNull() {
			if (value == null) {
				return String.Empty;
			} else {
				return value;
			}
		} // NotNull

		/// <summary>
		/// Gets the <see cref="System.String" /> or the default string if the string is null.
		/// </summary>
		/// <param name="defaultValue">The string to return when the string is null.</param>
		/// <returns>The string or the default string if the string is null.</returns>
		public String NotNull(String defaultValue) {
			if (value == null) {
				return defaultValue;
			} else {
				return value;
			}
		} // NotNull
		#endregion

		#region NotNullOrEmpty
		//--------------------------------------------------------------------------------------------------------------
		// NotNullOrEmpty.
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the <see cref="System.String" /> or the default string if the string is either null or empty.
		/// </summary>
		/// <param name="defaultValue">The string to return when the string is either null or empty.</param>
		/// <returns>The string or the default string if the string is either null or empty.</returns>
		public String NotNullOrEmpty(String defaultValue) {
			if ((value == null) || (value.Length == 0)) {
				return defaultValue;
			} else {
				return value;
			}
		} // NotNullOrEmpty
		#endregion

		#region NotNullOrWhiteSpace
		//--------------------------------------------------------------------------------------------------------------
		// NotNullOrWhiteSpace.
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the <see cref="System.String" /> or the default string if the <see cref="System.String" /> is either
		/// null, empty or only contains white-space characters.
		/// </summary>
		/// <param name="defaultValue">The string to return when the string is either null, empty or only contains white-space characters.</param>
		/// <returns>The string or the default string if the string is either null, empty or only contains white-space characters.</returns>
		public String NotNullOrWhiteSpace(String defaultValue) {
			if (value == null) {
				return defaultValue;
			}

			for (Int32 index = 0; index < value.Length; index++) {
				if (Char.IsWhiteSpace(value[index]) == false) {
					return value;
				}
			}

			return defaultValue;
		} // NotNullOrWhiteSpace
		#endregion

	}

} // RpcCoreExtensions
#endregion
