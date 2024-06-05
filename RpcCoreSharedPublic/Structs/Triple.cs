using System;
namespace RpcScandinavia.Core.Shared;

#region Triple
//----------------------------------------------------------------------------------------------------------------------
// Triple.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// A Triple is a struct that can contain three states, True, False and Unknown.
/// This class have constructors and operators for <see cref="System.Boolean"/>, <see cref="System.Byte"/>,
/// <see cref="System.Int32"/> and <see cref="System.String"/>.
/// </summary>
public struct Triple : IParsable<Triple> {
	private	readonly Byte value;

	#region Static new properties
	//------------------------------------------------------------------------------------------------------------------
	// Static new properties.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets a new Triple with the value of False.
	/// </summary>
	public static readonly Triple False = new Triple((Byte)0);

	/// <summary>
	/// Gets a new Triple with the value of True.
	/// </summary>
	public static readonly Triple True = new Triple((Byte)1);

	/// <summary>
	/// Gets a new Triple with the value of Unknown.
	/// </summary>
	public static readonly Triple Unknown = new Triple((Byte)2);
	#endregion

	#region Contructors
	//------------------------------------------------------------------------------------------------------------------
	// Constructors.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Creates a new Triple with the value either as True or False.
	/// </summary>
	/// <param name="value">The value of the Triple.</param>
	public Triple(Boolean value) {
		this.value = value switch {
			false => 0,
			true => 1,
		};
	} // Triple

	/// <summary>
	/// Creates a new Triple.
	/// If the value is 0, the Triple value will be False, if the value is 1, the Triple value will be True, and
	/// in all other cases the Triple value will be Unknown.
	/// </summary>
	/// <param name="value">The value of the Triple.</param>
	private Triple(Byte value) {
		this.value = value switch {
			0 => 0,
			1 => 1,
			_ => 2,
		};
	} // Triple

	/// <summary>
	/// Creates a new Triple.
	/// If the value is 0, the Triple value will be False, if the value is 1, the Triple value will be True, and
	/// in all other cases the Triple value will be Unknown.
	/// </summary>
	/// <param name="value">The value of the Triple.</param>
	private Triple(Int32 value) {
		this.value = value switch {
			0 => 0,
			1 => 1,
			_ => 2,
		};
	} // Triple

	/// <summary>
	/// Creates a new Triple.
	/// If the value is 0, the Triple value will be False, if the value is 1, the Triple value will be True, and
	/// in all other cases the Triple value will be Unknown.
	///
	/// All strings considered true, is specified in RpcScandinavia.Core.RpcCoreExtensions.TrueValues.
	/// All strings considered false, is specified in RpcScandinavia.Core.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value of the Triple.</param>
	public Triple(String value) {
		this.value = value switch {
			null => 2,
			_ when value.Equals("false", StringComparison.InvariantCultureIgnoreCase) => 0,
			_ when value.Equals("true", StringComparison.InvariantCultureIgnoreCase) => 1,
			_ => 2,
		};
	} // Triple
	#endregion

	#region Properties
	//------------------------------------------------------------------------------------------------------------------
	// Properties.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets True if the Triple value is Unknown.
	/// </summary>
	public Boolean IsUnknown {
		get {
			return this.value >= 2;
		}
	} // IsUnknown

	/// <summary>
	/// Gets True if the Triple value is True.
	/// </summary>
	public Boolean IsTrue {
		get {
			return this.value == 1;
		}
	} // IsTrue

	/// <summary>
	/// Gets True if the Triple value is False.
	/// </summary>
	public Boolean IsFalse {
		get {
			return this.value == 0;
		}
	} // IsFalse

	/// <summary>
	/// Gets the flipped Triple.
	/// The True and False values are switched, but Unknown is still Unknown.
	/// </summary>
	public Triple Flipped {
		get {
			return this.value switch {
				// Flip from false to true.
				0 => Triple.True,

				// Flip from true to false.
				1 => Triple.False,

				// Unknown cannot be flipped.
				_ => Triple.Unknown,
			};
		}
	} // Flipped

	/// <summary>
	/// Gets the byte value of the Triple.
	/// </summary>
	public Byte ByteValue {
		get {
			return this.value;
		}
	} // ByteValue
	#endregion

	#region Overriden methods
	//------------------------------------------------------------------------------------------------------------------
	// Overriden methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets a String representation of the Triples value.
	/// </summary>
	/// <returns>Either 'False', 'True' or 'Unknown'.</returns>
	public override String ToString() {
		return this.value switch {
			0 => "False",
			1 => "True",
			_ => "Unknown",
		};
	} // ToString

	/// <summary>
	/// Gets True if the value is the same as the Triple value.
	/// The value type can be either a Triple or a Boolean. All other value types always return False.
	/// </summary>
	/// <param name="value">The value to compare.</param>
	/// <returns>True if the value equals the Triple value.</returns>
	public override Boolean Equals(Object value) {
		return value switch {
			null => false,
			Boolean boolean => this == boolean,
			Triple triple => this.value == triple.value,
			SByte val => this.value == val,
			Int16 val => this.value == val,
			Int32 val => this.value == val,
			Int64 val => this.value == val,
			Byte val => this.value == val,
			UInt16 val => this.value == val,
			UInt32 val => this.value == val,
			UInt64 val => this.value == val,
			String str => this == str,
			_ => false,
		};
	} // Equals

	/// <summary>
	/// Gets the hash code for this instance.
	/// </summary>
	/// <returns>The hash code.</returns>
	public override Int32 GetHashCode() {
		return this.value.GetHashCode();
	} // GetHashCode
	#endregion

	#region Operators (equal)
	//------------------------------------------------------------------------------------------------------------------
	// Operators (equal).
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Operator that evaluates if a Triple value and a Boolean value are equal.
	/// </summary>
	/// <param name="x">The Triple value.</param>
	/// <param name="y">The Boolean value.</param>
	/// <returns>True if the X and Y values are equal.</returns>
	public static Boolean operator ==(Triple x, Boolean y) {
		if (y == false) {
			return x.value == 0;
		} else {
			return x.value == 1;
		}
	} // ==

	/// <summary>
	/// Operator that evaluates if a Triple value and a Int32 value are equal.
	/// </summary>
	/// <param name="x">The Triple value.</param>
	/// <param name="y">The Int32 value.</param>
	/// <returns>True if the X and Y values are equal.</returns>
	public static Boolean operator ==(Triple x, Int32 y) {
		return x.value == y;
	} // ==

	/// <summary>
	/// Operator that evaluates if a Triple value and a String value are equal.
	/// </summary>
	/// <param name="x">The Triple value.</param>
	/// <param name="y">The String value.</param>
	/// <returns>True if the X and Y values are equal.</returns>
	public static Boolean operator ==(Triple x, String y) {
		return (Triple.Parse(y, null).value == x.value);
	} // ==

	/// <summary>
	/// Operator that evaluates if two Triple values are equal.
	/// </summary>
	/// <param name="x">The first Triple value.</param>
	/// <param name="y">The second Triple value.</param>
	/// <returns>True if the X and Y values are equal.</returns>
	public static Boolean operator ==(Triple x, Triple y) {
		return x.value == y.value;
	} // ==

	/// <summary>
	/// Operator that evaluates if a Boolean value and a Triple value are equal.
	/// </summary>
	/// <param name="x">The Boolean value.</param>
	/// <param name="y">The Triple value.</param>
	/// <returns>True if the X and Y values are equal.</returns>
	public static Boolean operator ==(Boolean x, Triple y) {
		return (y == x);
	} // ==

	/// <summary>
	/// Operator that evaluates if a Int32 value and a Triple value are equal.
	/// </summary>
	/// <param name="x">The Int32 value.</param>
	/// <param name="y">The Triple value.</param>
	/// <returns>True if the X and Y values are equal.</returns>
	public static Boolean operator ==(Int32 x, Triple y) {
		return (y == x);
	} // ==

	/// <summary>
	/// Operator that evaluates if a String value and a Triple value are equal.
	/// </summary>
	/// <param name="x">The String value.</param>
	/// <param name="y">The Triple value.</param>
	/// <returns>True if the X and Y values are equal.</returns>
	public static Boolean operator ==(String x, Triple y) {
		return (y == x);
	} // ==
	#endregion

	#region Operators (not equal)
	//------------------------------------------------------------------------------------------------------------------
	// Operators (not equal).
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Operator that evaluates if a Triple value and a Boolean value are not equal.
	/// </summary>
	/// <param name="x">The Triple value.</param>
	/// <param name="y">The Boolean value.</param>
	/// <returns>True if the X and Y values are not equal.</returns>
	public static Boolean operator !=(Triple x, Boolean y) {
		return !(x == y);
	} // !=

	/// <summary>
	/// Operator that evaluates if a Triple value and a Int32 value are not equal.
	/// </summary>
	/// <param name="x">The Triple value.</param>
	/// <param name="y">The Int32 value.</param>
	/// <returns>True if the X and Y values are not equal.</returns>
	public static Boolean operator !=(Triple x, Int32 y) {
		return !(x == y);
	} // !=

	/// <summary>
	/// Operator that evaluates if a Triple value and a String value are not equal.
	/// </summary>
	/// <param name="x">The Triple value.</param>
	/// <param name="y">The String value.</param>
	/// <returns>True if the X and Y values are not equal.</returns>
	public static Boolean operator !=(Triple x, String y) {
		return !(x == y);
	} // !=

	/// <summary>
	/// Operator that evaluates if two Triple values are not equal.
	/// </summary>
	/// <param name="x">The first Triple value.</param>
	/// <param name="y">The second Triple value.</param>
	/// <returns>True if the X and Y values are not equal.</returns>
	public static Boolean operator !=(Triple x, Triple y) {
		return !(x == y);
	} // !=

	/// <summary>
	/// Operator that evaluates if a Boolean value and a Triple value are not equal.
	/// </summary>
	/// <param name="x">The Boolean value.</param>
	/// <param name="y">The Triple value.</param>
	/// <returns>True if the X and Y values are not equal.</returns>
	public static Boolean operator !=(Boolean x, Triple y) {
		return !(x == y);
	} // !=

	/// <summary>
	/// Operator that evaluates if a Int32 value and a Triple value are not equal.
	/// </summary>
	/// <param name="x">The Int32 value.</param>
	/// <param name="y">The Triple value.</param>
	/// <returns>True if the X and Y values are not equal.</returns>
	public static Boolean operator !=(Int32 x, Triple y) {
		return !(x == y);
	} // !=

	/// <summary>
	/// Operator that evaluates if a String value and a Triple value are not equal.
	/// </summary>
	/// <param name="x">The String value.</param>
	/// <param name="y">The Triple value.</param>
	/// <returns>True if the X and Y values are not equal.</returns>
	public static Boolean operator !=(String x, Triple y) {
		return !(x == y);
	} // !=
	#endregion

	#region Operators (Assign from Triple)
	//------------------------------------------------------------------------------------------------------------------
	// Operators (Assign from Triple).
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Operator that assigns a Triple value to a Byte.
	/// </summary>
	/// <param name="value">The Triple value that is assigned to the Byte.</param>
	/// <returns>A Byte with the value.</returns>
	public static implicit operator Byte(Triple value) {
		return value.value;
	} // Byte

	/// <summary>
	/// Operator that assigns a Triple value to a Int32.
	/// </summary>
	/// <param name="value">The Triple value that is assigned to the Int32.</param>
	/// <returns>A Int32 with the value.</returns>
	public static implicit operator Int32(Triple value) {
		return value.value;
	} // Int32

	/// <summary>
	/// Operator that assigns a Triple value to a String.
	/// </summary>
	/// <param name="value">The Triple value that is assigned to the String.</param>
	/// <returns>A String with the value.</returns>
	public static implicit operator String(Triple value) {
		return value.ToString();
	} // String
	#endregion

	#region Operators (Assign to Triple)
	//------------------------------------------------------------------------------------------------------------------
	// Operators (Assign to Triple).
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Operator that assigns a Boolean value to a Triple.
	/// </summary>
	/// <param name="value">The value that is assigned to the Triple.</param>
	/// <returns>A new Triple with the value.</returns>
	public static implicit operator Triple(Boolean value) {
		return new Triple(value);
	} // Triple

	/// <summary>
	/// Operator that assigns a Byte value to a Triple.
	/// </summary>
	/// <param name="value">The value that is assigned to the Triple.</param>
	/// <returns>A new Triple with the value.</returns>
	public static implicit operator Triple(Byte value) {
		return new Triple(value);
	} // Triple

	/// <summary>
	/// Operator that assigns a Int32 value to a Triple.
	/// </summary>
	/// <param name="value">The value that is assigned to the Triple.</param>
	/// <returns>A new Triple with the value.</returns>
	public static implicit operator Triple(Int32 value) {
		return new Triple(value);
	} // Triple

	/// <summary>
	/// Operator that assigns a String value to a Triple.
	/// </summary>
	/// <param name="value">The value that is assigned to the Triple.</param>
	/// <returns>A new Triple with the value.</returns>
	public static implicit operator Triple(String value) {
		return new Triple(value);
	} // Triple
	#endregion

	#region IParsable
	//------------------------------------------------------------------------------------------------------------------
	// IParsable.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Parses a String value to a Triple.
	/// </summary>
	/// <param name="value">The value that is assigned to the Triple.</param>
	/// <param name="provider">The format provider (is not used).</param>
	/// <returns>A new Triple with the value.</returns>
	public static Triple Parse(String value, IFormatProvider provider) {
		return new Triple(value);
	} // Parse

	/// <summary>
	/// Parses a String value to a Triple.
	/// </summary>
	/// <param name="value">The value that is assigned to the Triple.</param>
	/// <param name="provider">The format provider (is not used).</param>
	/// <param name="result">A new Triple with the value.</param>
	/// <returns>Always true.</returns>
	public static Boolean TryParse(String value, IFormatProvider provider, out Triple result) {
		result = new Triple(value);
		return true;
	} // TryParse
	#endregion

} // Triple
#endregion
