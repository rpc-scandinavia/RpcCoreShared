namespace RpcScandinavia.Core.Shared.KeyValueSerializer;
using System;

#region RpcKeyValueSerializerExceptionOption
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerExceptionOption.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Key/Value exception option.
/// </summary>
[Flags]
public enum RpcKeyValueSerializerExceptionOption {

	/// <summary>
	/// Catch all exceptions.
	/// </summary>
	CatchAll = 0x0,

	/// <summary>
	/// Throw individual item exceptions.
	/// </summary>
	ThrowItemExceptions = 0x1,

	/// <summary>
	/// Throw critical exceptions.
	/// </summary>
	ThrowCriticalExceptions = 0x2,

	/// <summary>
	/// Throw all exceptions.
	/// </summary>
	ThrowAll = ThrowItemExceptions | ThrowCriticalExceptions

} // RpcKeyValueSerializerExceptionOption
#endregion
