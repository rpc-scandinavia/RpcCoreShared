using System;
using System.Linq;
namespace RpcScandinavia.Core.Shared;

#region Triple
//----------------------------------------------------------------------------------------------------------------------
// Triple.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// A Triple is a struct that can contain three states, True, False and Unknown.
/// This class have constructors and operators for System.Boolean, System.Byte, System.Int32 and System.String.
/// </summary>
public struct Triple {
	private	Byte value;

	#region Static new properties
	//------------------------------------------------------------------------------------------------------------------
	// Static new properties.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets a new Triple with the value of False.
	/// </summary>
	static public readonly Triple	False		= new Triple((Byte)0);

	/// <summary>
	/// Gets a new Triple with the value of True.
	/// </summary>
	static public readonly Triple	True		= new Triple((Byte)1);

	/// <summary>
	/// Gets a new Triple with the value of Unknown.
	/// </summary>
	static public readonly Triple	Unknown		= new Triple((Byte)2);
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
		if (value == false) {
			this.value = 0;
		} else {
			this.value = 1;
		}
	} // Triple

	/// <summary>
	/// Creates a new Triple.
	/// If the value is 0, the Triple value will be False, if the value is 1, the Tripple value will be True, and
	/// in all other cases the Triple value will be Unknown.
	/// </summary>
	/// <param name="value">The value of the Triple.</param>
	private Triple(Byte value) {
		this.value = value;
	} // Triple

	/// <summary>
	/// Creates a new Triple.
	/// If the value is 0, the Triple value will be False, if the value is 1, the Tripple value will be True, and
	/// in all other cases the Triple value will be Unknown.
	/// </summary>
	/// <param name="value">The value of the Triple.</param>
	private Triple(Int32 value) {
		switch (value) {
			case 0:
				this.value	= 0;
				break;
			case 1:
				this.value	= 1;
				break;
			default:
				this.value	= 2;
				break;
		}
	} // Triple

	/// <summary>
	/// Creates a new Triple.
	/// If the value is 0, the Triple value will be False, if the value is 1, the Tripple value will be True, and
	/// in all other cases the Triple value will be Unknown.
	///
	/// All strings concidered true, is specified in RpcScandinavia.Core.RpcCoreExtensions.TrueValues.
	/// All strings concidered false, is specified in RpcScandinavia.Core.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value of the Triple.</param>
	public Triple(String value) {
		if ((value != null) && (RpcCoreExtensions.FalseValues.Contains(value.Trim().ToLower()) == true)) {
			this.value	= 0;
		} else if ((value != null) && (RpcCoreExtensions.TrueValues.Contains(value.Trim().ToLower()) == true)) {
			this.value	= 1;
		} else {
			this.value	= 2;
		}
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
			switch (this.value) {
				case 0:
					// Flip from false to true.
					return Triple.True;
				case 1:
					// Flip from true to false.
					return Triple.False;
				default:
					// Unknown cannot be flipped.
					return Triple.Unknown;
			}
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
		switch (this.value) {
			case 0:
				return "False";
			case 1:
				return "True";
			default:
				return "Unknown";
		}
	} // ToString

	/// <summary>
	/// Gets True if the value is the same as the Triple value.
	/// The value type can be either a Triple or a Boolean. All other value types always return False.
	/// </summary>
	/// <param name="value">The value to compare.</param>
	/// <returns>True if the value equals the Triple value.</returns>
	public override Boolean Equals(Object value) {
		try {
			return this == (Triple)value;
		} catch {
			return false;
		}
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
	static public Boolean operator ==(Triple x, Boolean y) {
		if ((Object)x == null)
			return (Object)y == null;
		else if ((Object)y == null)
			return (Object)x == null;

		if (y == false)
			return x.value == 0;
		else
			return x.value == 1;
	} // ==

	/// <summary>
	/// Operator that evaluates if a Triple value and a Int32 value are equal.
	/// </summary>
	/// <param name="x">The Triple value.</param>
	/// <param name="y">The Int32 value.</param>
	/// <returns>True if the X and Y values are equal.</returns>
	static public Boolean operator ==(Triple x, Int32 y) {
		if ((Object)x == null)
			return (Object)y == null;
		else if ((Object)y == null)
			return (Object)x == null;

		return x.value == y;
	} // ==

	/// <summary>
	/// Operator that evaluates if a Triple value and a String value are equal.
	/// </summary>
	/// <param name="x">The Triple value.</param>
	/// <param name="y">The String value.</param>
	/// <returns>True if the X and Y values are equal.</returns>
	static public Boolean operator ==(Triple x, String y) {
		if ((Object)x == null)
			return (Object)y == null;
		else if ((Object)y == null)
			return (Object)x == null;

		return x.ToString() == y;
	} // ==

	/// <summary>
	/// Operator that evaluates if two Triple values are equal.
	/// </summary>
	/// <param name="x">The first Triple value.</param>
	/// <param name="y">The second Triple value.</param>
	/// <returns>True if the X and Y values are equal.</returns>
	static public Boolean operator ==(Triple x, Triple y) {
		if ((Object)x == null)
			return (Object)y == null;
		else if ((Object)y == null)
			return (Object)x == null;
		else
			return x.value == y.value;
	} // ==

	/// <summary>
	/// Operator that evaluates if a Boolean value and a Triple value are equal.
	/// </summary>
	/// <param name="x">The Boolean value.</param>
	/// <param name="y">The Triple value.</param>
	/// <returns>True if the X and Y values are equal.</returns>
	static public Boolean operator ==(Boolean x, Triple y) {
		return (y == x);
	} // ==

	/// <summary>
	/// Operator that evaluates if a Int32 value and a Triple value are equal.
	/// </summary>
	/// <param name="x">The Int32 value.</param>
	/// <param name="y">The Triple value.</param>
	/// <returns>True if the X and Y values are equal.</returns>
	static public Boolean operator ==(Int32 x, Triple y) {
		return (y == x);
	} // ==

	/// <summary>
	/// Operator that evaluates if a String value and a Triple value are equal.
	/// </summary>
	/// <param name="x">The String value.</param>
	/// <param name="y">The Triple value.</param>
	/// <returns>True if the X and Y values are equal.</returns>
	static public Boolean operator ==(String x, Triple y) {
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
	static public Boolean operator !=(Triple x, Boolean y) {
		return !(x == y);
	} // !=

	/// <summary>
	/// Operator that evaluates if a Triple value and a Int32 value are not equal.
	/// </summary>
	/// <param name="x">The Triple value.</param>
	/// <param name="y">The Int32 value.</param>
	/// <returns>True if the X and Y values are not equal.</returns>
	static public Boolean operator !=(Triple x, Int32 y) {
		return !(x == y);
	} // !=

	/// <summary>
	/// Operator that evaluates if a Triple value and a String value are not equal.
	/// </summary>
	/// <param name="x">The Triple value.</param>
	/// <param name="y">The String value.</param>
	/// <returns>True if the X and Y values are not equal.</returns>
	static public Boolean operator !=(Triple x, String y) {
		return !(x == y);
	} // !=

	/// <summary>
	/// Operator that evaluates if two Triple values are not equal.
	/// </summary>
	/// <param name="x">The first Triple value.</param>
	/// <param name="y">The second Triple value.</param>
	/// <returns>True if the X and Y values are not equal.</returns>
	static public Boolean operator !=(Triple x, Triple y) {
		return !(x == y);
	} // !=

	/// <summary>
	/// Operator that evaluates if a Boolean value and a Triple value are not equal.
	/// </summary>
	/// <param name="x">The Boolean value.</param>
	/// <param name="y">The Triple value.</param>
	/// <returns>True if the X and Y values are not equal.</returns>
	static public Boolean operator !=(Boolean x, Triple y) {
		return !(x == y);
	} // !=

	/// <summary>
	/// Operator that evaluates if a Int32 value and a Triple value are not equal.
	/// </summary>
	/// <param name="x">The Int32 value.</param>
	/// <param name="y">The Triple value.</param>
	/// <returns>True if the X and Y values are not equal.</returns>
	static public Boolean operator !=(Int32 x, Triple y) {
		return !(x == y);
	} // !=

	/// <summary>
	/// Operator that evaluates if a String value and a Triple value are not equal.
	/// </summary>
	/// <param name="x">The String value.</param>
	/// <param name="y">The Triple value.</param>
	/// <returns>True if the X and Y values are not equal.</returns>
	static public Boolean operator !=(String x, Triple y) {
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
	static public implicit operator Byte(Triple value) {
		return value.value;
	} // Byte

	/// <summary>
	/// Operator that assigns a Triple value to a Int32.
	/// </summary>
	/// <param name="value">The Triple value that is assigned to the Int32.</param>
	/// <returns>A Int32 with the value.</returns>
	static public implicit operator Int32(Triple value) {
		return value.value;
	} // Int32

	/// <summary>
	/// Operator that assigns a Triple value to a String.
	/// </summary>
	/// <param name="value">The Triple value that is assigned to the String.</param>
	/// <returns>A String with the value.</returns>
	static public implicit operator String(Triple value) {
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
	static public implicit operator Triple(Boolean value) {
		return new Triple(value);
	} // Triple

	/// <summary>
	/// Operator that assigns a Byte value to a Triple.
	/// </summary>
	/// <param name="value">The value that is assigned to the Triple.</param>
	/// <returns>A new Triple with the value.</returns>
	static public implicit operator Triple(Byte value) {
		return new Triple(value);
	} // Triple

	/// <summary>
	/// Operator that assigns a Int32 value to a Triple.
	/// </summary>
	/// <param name="value">The value that is assigned to the Triple.</param>
	/// <returns>A new Triple with the value.</returns>
	static public implicit operator Triple(Int32 value) {
		return new Triple(value);
	} // Triple

	/// <summary>
	/// Operator that assigns a String value to a Triple.
	/// </summary>
	/// <param name="value">The value that is assigned to the Triple.</param>
	/// <returns>A new Triple with the value.</returns>
	static public implicit operator Triple(String value) {
		return new Triple(value);
	} // Triple
	#endregion

} // Triple
#endregion
