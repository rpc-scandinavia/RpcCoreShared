namespace RpcScandinavia.Core.Shared;
using System;
using System.Globalization;
using System.Linq;

#region RpcCoreExtensions
//----------------------------------------------------------------------------------------------------------------------
// RpcCoreExtensions.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// This part of the class contains usefull extension methods for the System.String class.
/// </summary>
static public partial class RpcCoreExtensions {

	#region ToBoolean (from String)
	//------------------------------------------------------------------------------------------------------------------
	// ToBoolean (from String).
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the System.String converted to a System.Boolean.
	/// All strings concidered true, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered false, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or false on exceptions.</returns>
	static public Boolean ToBoolean(this String value) {
		if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
			return true;
		} else {
			return false;
		}
	} // ToBoolean

	/// <summary>
	/// Gets the System.String converted to a System.Boolean.
	/// All strings concidered true, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered false, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	static public Boolean ToBoolean(this String value, Boolean defaultValue) {
		if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
			return true;
		} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
			return false;
		} else {
			return defaultValue;
		}
	} // ToBoolean

	/// <summary>
	/// Gets the System.String converted to a System.Boolean.
	/// All strings concidered true, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered false, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or an exception if thrown.</returns>
	static public Boolean ToBooleanThrow(this String value) {
		if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
			return true;
		} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
			return false;
		} else {
			throw new InvalidCastException (String.Format ("Unable to convert the string '{0}' to a boolean", value));
		}
	} // ToBooleanThrow
	#endregion

	#region ToTriple (from String)
	//------------------------------------------------------------------------------------------------------------------
	// ToTriple (from String).
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the System.String converted to a RpcScandinavia.Core.Shared.Triple.
	/// All strings concidered true, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered false, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or Triple.Unknown on exceptions.</returns>
	static public Triple ToTriple(this String value) {
		return new Triple(value);
	} // ToTriple
	#endregion

	#region ToSByte (from String)
	//------------------------------------------------------------------------------------------------------------------
	// ToSByte.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the System.String converted to a System.SByte.
	/// All strings concidered one, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered zero, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or zero on exceptions.</returns>
	static public SByte ToSByte(this String value) {
		try {
			if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 1;
			} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 0;
			} else {
				return SByte.Parse(value);
			}
		} catch {
			return 0;
		}
	} // ToSByte

	/// <summary>
	/// Gets the System.String converted to a System.SByte.
	/// All strings concidered one, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered zero, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	static public SByte ToSByte(this String value, SByte defaultValue) {
		try {
			if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 1;
			} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 0;
			} else {
				return SByte.Parse(value);
			}
		} catch {
			return defaultValue;
		}
	} // ToSByte

	/// <summary>
	/// Gets the System.String converted to a System.SByte.
	/// All strings concidered one, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered zero, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or an exception if thrown.</returns>
	static public SByte ToSByteThrow(this String value) {
		if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
			return 1;
		} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
			return 0;
		} else {
			return SByte.Parse(value);
		}
	} // ToSByteThrow
	#endregion

	#region ToInt16 (from String)
	//------------------------------------------------------------------------------------------------------------------
	// ToInt16 (from String).
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the System.String converted to a System.Int16.
	/// All strings concidered one, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered zero, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or zero on exceptions.</returns>
	static public Int16 ToInt16(this String value) {
		try {
			if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 1;
			} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 0;
			} else {
				return Int16.Parse(value);
			}
		} catch {
			return 0;
		}
	} // ToInt16

	/// <summary>
	/// Gets the System.String converted to a System.Int16.
	/// All strings concidered one, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered zero, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	static public Int16 ToInt16(this String value, Int16 defaultValue) {
		try {
			if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 1;
			} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 0;
			} else {
				return Int16.Parse(value);
			}
		} catch {
			return defaultValue;
		}
	} // ToInt16

	/// <summary>
	/// Gets the System.String converted to a System.Int16.
	/// All strings concidered one, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered zero, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or an exception if thrown.</returns>
	static public Int16 ToInt16Throw(this String value) {
		if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
			return 1;
		} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
			return 0;
		} else {
			return Int16.Parse(value);
		}
	} // ToInt16Throw
	#endregion

	#region ToInt32 (from String)
	//------------------------------------------------------------------------------------------------------------------
	// ToInt32 (from String).
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the System.String converted to a System.Int32.
	/// All strings concidered one, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered zero, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or zero on exceptions.</returns>
	static public Int32 ToInt32(this String value) {
		try {
			if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 1;
			} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 0;
			} else {
				return Int32.Parse(value);
			}
		} catch {
			return 0;
		}
	} // ToInt32

	/// <summary>
	/// Gets the System.String converted to a System.Int32.
	/// All strings concidered one, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered zero, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	static public Int32 ToInt32(this String value, Int32 defaultValue) {
		try {
			if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 1;
			} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 0;
			} else {
				return Int32.Parse(value);
			}
		} catch {
			return defaultValue;
		}
	} // ToInt32

	/// <summary>
	/// Gets the System.String converted to a System.Int32.
	/// All strings concidered one, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered zero, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or an exception if thrown.</returns>
	static public Int32 ToInt32Throw(this String value) {
		if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
			return 1;
		} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
			return 0;
		} else {
			return Int32.Parse(value);
		}
	} // ToInt32Throw
	#endregion

	#region ToInt64 (from String)
	//------------------------------------------------------------------------------------------------------------------
	// ToInt64 (from String).
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the System.String converted to a System.Int64.
	/// All strings concidered one, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered zero, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or zero on exceptions.</returns>
	static public Int64 ToInt64(this String value) {
		try {
			if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 1;
			} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 0;
			} else {
				return Int64.Parse(value);
			}
		} catch {
			return 0;
		}
	} // ToInt64

	/// <summary>
	/// Gets the System.String converted to a System.Int64.
	/// All strings concidered one, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered zero, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	static public Int64 ToInt64(this String value, Int64 defaultValue) {
		try {
			if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 1;
			} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 0;
			} else {
				return Int64.Parse(value);
			}
		} catch {
			return defaultValue;
		}
	} // ToInt64

	/// <summary>
	/// Gets the System.String converted to a System.Int64.
	/// All strings concidered one, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered zero, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or an exception if thrown.</returns>
	static public Int64 ToInt64Throw(this String value) {
		if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
			return 1;
		} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
			return 0;
		} else {
			return Int64.Parse(value);
		}
	} // ToInt64Throw
	#endregion

	#region ToByte (from String)
	//------------------------------------------------------------------------------------------------------------------
	// ToByte (from String).
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the System.String converted to a System.Byte.
	/// All strings concidered one, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered zero, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or zero on exceptions.</returns>
	static public Byte ToByte(this String value) {
		try {
			if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 1;
			} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 0;
			} else {
				return Byte.Parse(value);
			}
		} catch {
			return 0;
		}
	} // ToByte

	/// <summary>
	/// Gets the System.String converted to a System.Byte.
	/// All strings concidered one, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered zero, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	static public Byte ToByte(this String value, Byte defaultValue) {
		try {
			if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 1;
			} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 0;
			} else {
				return Byte.Parse(value);
			}
		} catch {
			return defaultValue;
		}
	} // ToByte

	/// <summary>
	/// Gets the System.String converted to a System.Byte.
	/// All strings concidered one, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered zero, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or an exception if thrown.</returns>
	static public Byte ToByteThrow(this String value) {
		if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
			return 1;
		} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
			return 0;
		} else {
			return Byte.Parse(value);
		}
	} // ToByteThrow
	#endregion

	#region ToUInt16 (from String)
	//------------------------------------------------------------------------------------------------------------------
	// ToUInt16 (from String).
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the System.String converted to a System.UInt16.
	/// All strings concidered one, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered zero, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or zero on exceptions.</returns>
	static public UInt16 ToUInt16(this String value) {
		try {
			if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 1;
			} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 0;
			} else {
				return UInt16.Parse(value);
			}
		} catch {
			return 0;
		}
	} // ToUInt16

	/// <summary>
	/// Gets the System.String converted to a System.UInt16.
	/// All strings concidered one, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered zero, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	static public UInt16 ToUInt16(this String value, UInt16 defaultValue) {
		try {
			if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 1;
			} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 0;
			} else {
				return UInt16.Parse(value);
			}
		} catch {
			return defaultValue;
		}
	} // ToUInt16

	/// <summary>
	/// Gets the System.String converted to a System.UInt16.
	/// All strings concidered one, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered zero, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or an exception if thrown.</returns>
	static public UInt16 ToUInt16Throw(this String value) {
		if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
			return 1;
		} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
			return 0;
		} else {
			return UInt16.Parse(value);
		}
	} // ToUInt16Throw
	#endregion

	#region ToUInt32 (from String)
	//------------------------------------------------------------------------------------------------------------------
	// ToUInt32 (from String).
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the System.String converted to a System.UInt32.
	/// All strings concidered one, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered zero, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or zero on exceptions.</returns>
	static public UInt32 ToUInt32(this String value) {
		try {
			if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 1;
			} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 0;
			} else {
				return UInt32.Parse(value);
			}
		} catch {
			return 0;
		}
	} // ToUInt32

	/// <summary>
	/// Gets the System.String converted to a System.UInt32.
	/// All strings concidered one, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered zero, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	static public UInt32 ToUInt32(this String value, UInt32 defaultValue) {
		try {
			if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 1;
			} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 0;
			} else {
				return UInt32.Parse(value);
			}
		} catch {
			return defaultValue;
		}
	} // ToUInt32

	/// <summary>
	/// Gets the System.String converted to a System.UInt32.
	/// All strings concidered one, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered zero, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or an exception if thrown.</returns>
	static public UInt32 ToUInt32Throw(this String value) {
		if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
			return 1;
		} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
			return 0;
		} else {
			return UInt32.Parse(value);
		}
	} // ToUInt32Throw
	#endregion

	#region ToUInt64 (from String)
	//------------------------------------------------------------------------------------------------------------------
	// ToUInt64 (from String).
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the System.String converted to a System.UInt64.
	/// All strings concidered one, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered zero, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or zero on exceptions.</returns>
	static public UInt64 ToUInt64(this String value) {
		try {
			if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 1;
			} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 0;
			} else {
				return UInt64.Parse(value);
			}
		} catch {
			return 0;
		}
	} // ToUInt64

	/// <summary>
	/// Gets the System.String converted to a System.UInt64.
	/// All strings concidered one, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered zero, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	static public UInt64 ToUInt64(this String value, UInt64 defaultValue) {
		try {
			if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 1;
			} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 0;
			} else {
				return UInt64.Parse(value);
			}
		} catch {
			return defaultValue;
		}
	} // ToUInt64

	/// <summary>
	/// Gets the System.String converted to a System.UInt64.
	/// All strings concidered one, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered zero, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or an exception if thrown.</returns>
	static public UInt64 ToUInt64Throw(this String value) {
		if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
			return 1;
		} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
			return 0;
		} else {
			return UInt64.Parse(value);
		}
	} // ToUInt64Throw
	#endregion

	#region ToSingle (from String)
	//------------------------------------------------------------------------------------------------------------------
	// ToSingle (from String).
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the System.String converted to a System.Single.
	/// All strings concidered 1.0F, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered 0.0F, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or 0.0F on exceptions.</returns>
	static public Single ToSingle(this String value) {
		try {
			if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 1.0F;
			} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 0.0F;
			} else {
				return Single.Parse(value);
			}
		} catch {
			return 0.0F;
		}
	} // ToSingle

	/// <summary>
	/// Gets the System.String converted to a System.Single.
	/// All strings concidered 1.0F, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered 0.0F, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	static public Single ToSingle(this String value, Single defaultValue) {
		try {
			if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 1.0F;
			} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 0.0F;
			} else {
				return Single.Parse(value);
			}
		} catch {
			return defaultValue;
		}
	} // ToSingle

	/// <summary>
	/// Gets the System.String converted to a System.Single.
	/// All strings concidered 1.0F, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered 0.0F, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or an exception if thrown.</returns>
	static public Single ToSingleThrow(this String value) {
		if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
			return 1.0F;
		} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
			return 0.0F;
		} else {
			return Single.Parse(value);
		}
	} // ToSingleThrow
	#endregion

	#region ToSingleInvariant (from String)
	//------------------------------------------------------------------------------------------------------------------
	// ToSingleInvariant (from String).
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the System.String converted to a System.Single using the invariant culture.
	/// All strings concidered 1.0F, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered 0.0F, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or 0.0F on exceptions.</returns>
	static public Single ToSingleInvariant(this String value) {
		try {
			if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 1.0F;
			} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 0.0F;
			} else {
				return Single.Parse(value, CultureInfo.InvariantCulture);
			}
		} catch {
			return 0.0F;
		}
	} // ToSingleInvariant

	/// <summary>
	/// Gets the System.String converted to a System.Single using the invariant culture.
	/// All strings concidered 1.0F, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered 0.0F, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	static public Single ToSingleInvariant(this String value, Single defaultValue) {
		try {
			if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 1.0F;
			} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 0.0F;
			} else {
				return Single.Parse(value, CultureInfo.InvariantCulture);
			}
		} catch {
			return defaultValue;
		}
	} // ToSingleInvariant

	/// <summary>
	/// Gets the System.String converted to a System.Single using the invariant culture.
	/// All strings concidered 1.0F, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered 0.0F, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or an exception if thrown.</returns>
	static public Single ToSingleInvariantThrow(this String value) {
		if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
			return 1.0F;
		} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
			return 0.0F;
		} else {
			return Single.Parse(value, CultureInfo.InvariantCulture);
		}
	} // ToSingleInvariantThrow
	#endregion

	#region ToDouble (from String)
	//------------------------------------------------------------------------------------------------------------------
	// ToDouble (from String).
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the System.String converted to a System.Double.
	/// All strings concidered 1.0F, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered 0.0F, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or 0.0F on exceptions.</returns>
	static public Double ToDouble(this String value) {
		try {
			if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 1.0F;
			} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 0.0F;
			} else {
				return Double.Parse(value);
			}
		} catch {
			return 0.0F;
		}
	} // ToDouble

	/// <summary>
	/// Gets the System.String converted to a System.Double.
	/// All strings concidered 1.0F, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered 0.0F, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	static public Double ToDouble(this String value, Double defaultValue) {
		try {
			if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 1.0F;
			} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 0.0F;
			} else {
				return Double.Parse(value);
			}
		} catch {
			return defaultValue;
		}
	} // ToDouble

	/// <summary>
	/// Gets the System.String converted to a System.Double.
	/// All strings concidered 1.0F, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered 0.0F, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or an exception if thrown.</returns>
	static public Double ToDoubleThrow(this String value) {
		if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
			return 1.0F;
		} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
			return 0.0F;
		} else {
			return Double.Parse(value);
		}
	} // ToDoubleThrow
	#endregion

	#region ToDoubleInvariant (from String)
	//------------------------------------------------------------------------------------------------------------------
	// ToDoubleInvariant (from String).
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the System.String converted to a System.Double using the invariant culture.
	/// All strings concidered 1.0F, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered 0.0F, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or 0.0F on exceptions.</returns>
	static public Double ToDoubleInvariant(this String value) {
		try {
			if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 1.0F;
			} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 0.0F;
			} else {
				return Double.Parse(value, CultureInfo.InvariantCulture);
			}
		} catch {
			return 0.0F;
		}
	} // ToDoubleInvariant

	/// <summary>
	/// Gets the System.String converted to a System.Double using the invariant culture.
	/// All strings concidered 1.0F, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered 0.0F, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	static public Double ToDoubleInvariant(this String value, Double defaultValue) {
		try {
			if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 1.0F;
			} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 0.0F;
			} else {
				return Double.Parse(value, CultureInfo.InvariantCulture);
			}
		} catch {
			return defaultValue;
		}
	} // ToDoubleInvariant

	/// <summary>
	/// Gets the System.String converted to a System.Double using the invariant culture.
	/// All strings concidered 1.0F, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered 0.0F, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or an exception if thrown.</returns>
	static public Double ToDoubleInvariantThrow(this String value) {
		if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
			return 1.0F;
		} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
			return 0.0F;
		} else {
			return Double.Parse(value, CultureInfo.InvariantCulture);
		}
	} // ToDoubleInvariantThrow
	#endregion

	#region ToDecimal (from String)
	//------------------------------------------------------------------------------------------------------------------
	// ToDecimal (from String).
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the System.String converted to a System.Decimal.
	/// All strings concidered 1.0M, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered 0.0M, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or 0.0M on exceptions.</returns>
	static public Decimal ToDecimal(this String value) {
		try {
			if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 1.0M;
			} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 0.0M;
			} else {
				return Decimal.Parse(value);
			}
		} catch {
			return 0.0M;
		}
	} // ToDecimal

	/// <summary>
	/// Gets the System.String converted to a System.Decimal.
	/// All strings concidered 1.0M, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered 0.0M, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	static public Decimal ToDecimal(this String value, Decimal defaultValue) {
		try {
			if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 1.0M;
			} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 0.0M;
			} else {
				return Decimal.Parse(value);
			}
		} catch {
			return defaultValue;
		}
	} // ToDecimal

	/// <summary>
	/// Gets the System.String converted to a System.Decimal.
	/// All strings concidered 1.0M, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered 0.0M, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or an exception if thrown.</returns>
	static public Decimal ToDecimalThrow(this String value) {
		if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
			return 1.0M;
		} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
			return 0.0M;
		} else {
			return Decimal.Parse(value);
		}
	} // ToDecimalThrow
	#endregion

	#region ToDecimalInvariant (from String)
	//------------------------------------------------------------------------------------------------------------------
	// ToDecimalInvariant (from String).
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the System.String converted to a System.Decimal using the invariant culture.
	/// All strings concidered 1.0M, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered 0.0M, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or 0.0M on exceptions.</returns>
	static public Decimal ToDecimalInvariant(this String value) {
		try {
			if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 1.0M;
			} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 0.0M;
			} else {
				return Decimal.Parse(value, CultureInfo.InvariantCulture);
			}
		} catch {
			return 0.0M;
		}
	} // ToDecimalInvariant

	/// <summary>
	/// Gets the System.String converted to a System.Decimal using the invariant culture.
	/// All strings concidered 1.0M, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered 0.0M, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	static public Decimal ToDecimalInvariant(this String value, Decimal defaultValue) {
		try {
			if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 1.0M;
			} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
				return 0.0M;
			} else {
				return Decimal.Parse(value, CultureInfo.InvariantCulture);
			}
		} catch {
			return defaultValue;
		}
	} // ToDecimalInvariant

	/// <summary>
	/// Gets the System.String converted to a System.Decimal using the invariant culture.
	/// All strings concidered 1.0M, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered 0.0M, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or an exception if thrown.</returns>
	static public Decimal ToDecimalInvariantThrow(this String value) {
		if (RpcCoreExtensions.TrueValues.Contains(value.NotNull().Trim().ToLower()) == true) {
			return 1.0M;
		} else if (RpcCoreExtensions.FalseValues.Contains(value.NotNull().Trim().ToLower()) == true) {
			return 0.0M;
		} else {
			return Decimal.Parse(value, CultureInfo.InvariantCulture);
		}
	} // ToDecimalInvariantThrow
	#endregion

	#region ToDateTime (from String)
	//------------------------------------------------------------------------------------------------------------------
	// ToDateTime (from String).
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the System.String converted to a System.DateTime.
	/// All strings concidered 1.0M, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered 0.0M, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or DateTime.Now on exceptions.</returns>
	static public DateTime ToDateTime(this String value) {
		try {
			return DateTime.Parse(value);
		} catch {
			return DateTime.Now;
		}
	} // ToDateTime

	/// <summary>
	/// Gets the System.String converted to a System.DateTime.
	/// All strings concidered 1.0M, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered 0.0M, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	static public DateTime ToDateTime(this String value, DateTime defaultValue) {
		try {
			return DateTime.Parse(value);
		} catch {
			return defaultValue;
		}
	} // ToDateTime

	/// <summary>
	/// Gets the System.String converted to a System.DateTime.
	/// All strings concidered 1.0M, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered 0.0M, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or an exception if thrown.</returns>
	static public DateTime ToDateTimeThrow(this String value) {
		return DateTime.Parse(value);
	} // ToDateTimeThrow
	#endregion

	#region ToDateTimeInvariant (from String)
	//------------------------------------------------------------------------------------------------------------------
	// ToDateTimeInvariant (from String).
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the System.String converted to a System.DateTime using the invariant culture.
	/// All strings concidered 1.0M, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered 0.0M, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or DateTime.Now on exceptions.</returns>
	static public DateTime ToDateTimeInvariant(this String value) {
		try {
			return DateTime.Parse(value, CultureInfo.InvariantCulture);
		} catch {
			return DateTime.Now;
		}
	} // ToDateTimeInvariant

	/// <summary>
	/// Gets the System.String converted to a System.DateTime using the invariant culture.
	/// All strings concidered 1.0M, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered 0.0M, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	static public DateTime ToDateTimeInvariant(this String value, DateTime defaultValue) {
		try {
			return DateTime.Parse(value, CultureInfo.InvariantCulture);
		} catch {
			return defaultValue;
		}
	} // ToDateTimeInvariant

	/// <summary>
	/// Gets the System.String converted to a System.DateTime using the invariant culture.
	/// All strings concidered 1.0M, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// All strings concidered 0.0M, is specified in RpcScandinavia.Core.Shared.RpcCoreExtensions.TrueValues.
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or an exception if thrown.</returns>
	static public DateTime ToDateTimeInvariantThrow(this String value) {
		return DateTime.Parse(value, CultureInfo.InvariantCulture);
	} // ToDateTimeInvariantThrow
	#endregion

	#region ToGuid (from String)
	//------------------------------------------------------------------------------------------------------------------
	// ToGuid (from String).
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the System.String converted to a System.Guid.
	/// The value may be a BASE64 encoded string representing a Guid.
	/// Such a string should be either 24 characters long, or 22 characters long (without the ending '==').
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or Guid.Empty on exceptions.</returns>
	static public Guid ToGuid(this String value) {
		try {
			if (value.Trim().Length == 24) {
				value					= value.Replace("_", "/");
				value					= value.Replace("-", "+");
				Byte[]	buffer	= Convert.FromBase64String(value);
				return new Guid(buffer);
			} else if (value.Trim().Length == 22) {
				value					= value.Replace("_", "/");
				value					= value.Replace("-", "+");
				value					= value + "==";
				Byte[]	buffer	= Convert.FromBase64String(value);
				return new Guid(buffer);
			} else {
				return new Guid(value);
			}
		} catch {
			return Guid.Empty;
		}
	} // ToGuid

	/// <summary>
	/// Gets the System.String converted to a System.Guid.
	/// The value may be a BASE64 encoded string representing a Guid.
	/// Such a string should be either 24 characters long, or 22 characters long (without the ending '==').
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <param name="defaultValue">The value to return on exceptions.</param>
	/// <returns>The converted value, or the default value on exceptions.</returns>
	static public Guid ToGuid(this String value, Guid defaultValue) {
		try {
			if (value.Trim().Length == 24) {
				value					= value.Replace("_", "/");
				value					= value.Replace("-", "+");
				Byte[]	buffer	= Convert.FromBase64String(value);
				return new Guid(buffer);
			} else if (value.Trim().Length == 22) {
				value					= value.Replace("_", "/");
				value					= value.Replace("-", "+");
				value					= value + "==";
				Byte[]	buffer	= Convert.FromBase64String(value);
				return new Guid(buffer);
			} else {
				return new Guid(value);
			}
		} catch {
			return defaultValue;
		}
	} // ToGuid

	/// <summary>
	/// Gets the System.String converted to a System.Guid.
	/// The value may be a BASE64 encoded string representing a Guid.
	/// Such a string should be either 24 characters long, or 22 characters long (without the ending '==').
	/// </summary>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value, or an exception if thrown.</returns>
	static public Guid ToGuidThrow(this String value) {
		if (value.Trim().Length == 24) {
			value					= value.Replace("_", "/");
			value					= value.Replace("-", "+");
			Byte[]	buffer	= Convert.FromBase64String(value);
			return new Guid(buffer);
		} else if (value.Trim().Length == 22) {
			value					= value.Replace("_", "/");
			value					= value.Replace("-", "+");
			value					= value + "==";
			Byte[]	buffer	= Convert.FromBase64String(value);
			return new Guid(buffer);
		} else {
			return new Guid(value);
		}
	} // ToGuidThrow
	#endregion

} // RpcCoreExtensions
#endregion
