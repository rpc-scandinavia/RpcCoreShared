using System;
namespace RpcScandinavia.Core.Shared;

#region RpcCoreExtensions
//----------------------------------------------------------------------------------------------------------------------
// RpcCoreExtensions.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// This part of the class contains useful extension methods for the <see cref="System.String" /> class.
/// </summary>
public static partial class RpcCoreExtensions {

	extension (String value) {

		#region ToIParsable
		//--------------------------------------------------------------------------------------------------------------
		// ToIParsable.
		//--------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a T that implements <see cref="System.IParsable{T}" />.
		/// </summary>
		/// <param name="formatProvider">The format provider (null).</param>
		/// <typeparam name="T">The IParsable type.</typeparam>
		/// <returns>The converted string, or the default value on exceptions.</returns>
		public T ToParsable<T>(IFormatProvider formatProvider = null) where T: IParsable<T> {
			try {
				return T.Parse(value, formatProvider);
			} catch {
				return default(T);
			}
		} // ToParsable

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a T that implements <see cref="System.IParsable{T}" />.
		/// </summary>
		/// <param name="defaultValue">The value to return on exceptions.</param>
		/// <param name="formatProvider">The format provider (null).</param>
		/// <typeparam name="T">The IParsable type.</typeparam>
		/// <returns>The converted string, or the default value on exceptions.</returns>
		public T ToParsable<T>(T defaultValue, IFormatProvider formatProvider = null) where T: IParsable<T> {
			try {
				return T.Parse(value, formatProvider);
			} catch {
				return defaultValue;
			}
		} // ToParsable

		/// <summary>
		/// Gets the <see cref="System.String" /> converted to a T that implements <see cref="System.IParsable{T}" />.
		/// </summary>
		/// <param name="formatProvider">The format provider (null).</param>
		/// <typeparam name="T">The IParsable type.</typeparam>
		/// <returns>The converted string, or a exception.</returns>
		public T ToParsableOrThrow<T>(IFormatProvider formatProvider = null) where T: IParsable<T> {
			if (T.TryParse(value, formatProvider, out T result) == true) {
				return result;
			} else {
				throw new ArgumentException($"Unable to parse the string '{value}' to a {typeof(T).Name}.");
			}
		} // ToParsableOrThrow
		#endregion

	}

} // RpcCoreExtensions
#endregion
