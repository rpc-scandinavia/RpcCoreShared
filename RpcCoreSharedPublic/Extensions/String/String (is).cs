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

		#region IsEmpty
		//--------------------------------------------------------------------------------------------------------------
		// IsEmpty.
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Indicates whether a <see cref="System.String" /> is empty.
		/// A string containing one or more white-space characters are not considered empty.
		/// </summary>
		/// <param name="throwException">Whether to throw a exception if the string is null (false).</param>
		/// <returns>True if the string is empty.</returns>
		public Boolean IsEmpty(Boolean throwException = false) {
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
		//--------------------------------------------------------------------------------------------------------------
		// IsNullOrEmpty.
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Indicates whether a <see cref="System.String" /> is null or empty.
		/// A string containing one or more white-space characters are not considered empty.
		/// </summary>
		/// <returns>True if the string is null or empty.</returns>
		public Boolean IsNullOrEmpty() {
			return ((value == null) || (value.Length == 0));
		} // IsNullOrEmpty
		#endregion

		#region IsNullOrWhiteSpace
		//--------------------------------------------------------------------------------------------------------------
		// IsNullOrWhiteSpace.
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Indicates whether a <see cref="System.String" /> is null, empty or consists only of white-space characters.
		///
		/// White-space characters are defined by the Unicode standard. The IsNullOrWhiteSpace method interprets any
		/// character that returns a value of true when it is passed to the <see cref="System.Char.IsWhiteSpace(Char)" /> method,
		/// as a white-space character.
		/// </summary>
		/// <returns>True if the string is null, empty or consists only of white-space characters.</returns>
		public Boolean IsNullOrWhiteSpace() {
			if (value == null) {
				return true;
			}

			for (Int32 index = 0; index < value.Length; index++) {
				if (Char.IsWhiteSpace(value[index]) == false) {
					return false;
				}
			}

			return true;
		} // IsNullOrWhiteSpace
		#endregion

	}

} // RpcCoreExtensions
#endregion
