namespace RpcScandinavia.Core.Shared;
using System;

#region RpcKeyValueSerializerConverterGuid
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerConverterGuid.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// RPC Key/Value serializer converter, that serializes and deserialized to/from a <see cref="System.Guid" />.
/// </summary>
public class RpcKeyValueSerializerConverterGuid : RpcKeyValueSerializerConverter<Guid> {

	/// <inheritdoc />
	public override String Serialize(Guid obj, RpcKeyValueSerializerOptions options) {
		return obj.ToString();
	} // Serialize

	/// <inheritdoc />
	public override Guid Deserialize(String value, RpcKeyValueSerializerOptions options) {
		return Guid.Parse(value);
	} // Deserialize

} // RpcKeyValueSerializerConverterGuid
#endregion
