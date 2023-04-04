namespace RpcScandinavia.Core.Shared.KeyValueSerializer;
using System;

#region RpcKeyValueSerializerConverterBoolean
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerConverterBoolean.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// RPC Key/Value serializer converter, that serializes and deserialized to/from a <see cref="System.Boolean" />.
/// </summary>
public class RpcKeyValueSerializerConverterBoolean : RpcKeyValueSerializerConverter<Boolean> {

	/// <inheritdoc />
	public override String Serialize(Boolean obj, RpcKeyValueSerializerOptions options) {
		return obj.ToString();
	} // Serialize

	/// <inheritdoc />
	public override Boolean Deserialize(String value, RpcKeyValueSerializerOptions options) {
		return Boolean.Parse(value);
	} // Deserialize

} // RpcKeyValueSerializerConverterBoolean
#endregion
