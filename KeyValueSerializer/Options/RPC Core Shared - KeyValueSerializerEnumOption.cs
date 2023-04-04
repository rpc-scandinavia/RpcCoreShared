namespace RpcScandinavia.Core.Shared.KeyValueSerializer;
using System;

#region RpcKeyValueSerializerEnumOption
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerEnumOption.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Key/Value enum serialization option.
/// </summary>
public enum RpcKeyValueSerializerEnumOption {

	/// <summary>
	/// Do not serialize enums.
	/// </summary>
	NotSerialized,

	/// <summary>
	/// Serialize enums as their numeric value.
	/// </summary>
	AsInteger,

	/// <summary>
	/// Serialize enums as their name value.
	/// </summary>
	AsString

} // RpcKeyValueSerializerEnumOption
#endregion
