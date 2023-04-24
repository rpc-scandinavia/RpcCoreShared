namespace RpcScandinavia.Core.Shared;
using System;

#region RpcKeyValueSerializerTypeInfoOption
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerTypeInfoOption.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Type information option.
/// </summary>
public enum RpcKeyValueSerializerTypeInfoOption {

	/// <summary>
	/// Newer use type information.
	/// </summary>
	Newer,

	/// <summary>
	/// Only use type information when required.
	/// </summary>
	Required,

	/// <summary>
	/// Only use type information when required, and on the top-level object.
	/// </summary>
	RequiredAndTop,

	/// <summary>
	/// Always use type information.
	/// </summary>
	Always

} // RpcKeyValueSerializerTypeInfoOption
#endregion
