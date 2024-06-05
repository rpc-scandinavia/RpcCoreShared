using System;
using System.Text;
namespace RpcScandinavia.Core.Shared;

#region RpcCoreExtensions
//----------------------------------------------------------------------------------------------------------------------
// RpcCoreExtensions.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// This part of the class contains useful extension methods for the <see cref="System.String" /> class.
/// </summary>
public static partial class RpcCoreExtensions {

	#region ToIParsable
	//------------------------------------------------------------------------------------------------------------------
	// ToIParsable.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a T that implements <see cref="System.IParsable{T}" />.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="formatProvider">The format provider (null).</param>
	/// <typeparam name="T">The IParsable type.</typeparam>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	public static T ToParsable<T>(this String value, IFormatProvider formatProvider = null) where T: IParsable<T> {
		try {
			return T.Parse(value, formatProvider);
		} catch {
			return default(T);
		}
	} // ToParsable

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a T that implements <see cref="System.IParsable{T}" />.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <param name="formatProvider">The format provider (null).</param>
	/// <typeparam name="T">The IParsable type.</typeparam>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	public static T ToParsable<T>(this String value, T defaultValue, IFormatProvider formatProvider = null) where T: IParsable<T> {
		try {
			return T.Parse(value, formatProvider);
		} catch {
			return defaultValue;
		}
	} // ToParsable

	/// <summary>
	/// Gets the <see cref="System.String" /> converted to a T that implements <see cref="System.IParsable{T}" />.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="formatProvider">The format provider (null).</param>
	/// <typeparam name="T">The IParsable type.</typeparam>
	/// <returns>The converted value, or a exception.</returns>
	public static T ToParsableOrThrow<T>(this String value, IFormatProvider formatProvider = null) where T: IParsable<T> {
		if (T.TryParse(value, formatProvider, out T result) == true) {
			return result;
		} else {
			throw new ArgumentException($"Unable to parse the string '{value}' to a {typeof(T).Name}.");
		}
	} // ToParsableOrThrow
	#endregion

} // RpcCoreExtensions
#endregion
