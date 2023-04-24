namespace RpcScandinavia.Core.Shared;
using System;

#region RpcKeyValueExceptionType
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueException.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// The key/value exception type.
/// </summary>
public enum RpcKeyValueExceptionType {

	/// <summary>
	/// Indicates a critical exception, that prevents serialization or deserialization.
	/// </summary>
	Critical,

	/// <summary>
	/// Indicates a item exception, that prevents one item from being serialize or deserialized.
	/// </summary>
	Item

} // RpcKeyValueExceptionType
#endregion
