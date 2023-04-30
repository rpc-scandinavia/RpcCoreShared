namespace RpcScandinavia.Core.Shared;
using System;
using System.Text;

#region RpcKeyValueSerializerConverterString
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerConverterString.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// RPC Key/Value serializer converter, that serializes and deserialized to/from a <see cref="System.String" />.
/// </summary>
public class RpcKeyValueSerializerConverterString : RpcKeyValueSerializerConverter<String> {

	/// <inheritdoc />
	public override String Serialize(String obj, RpcKeyValueSerializerOptions options) {
		return obj.NotNull();
	} // Serialize

	/// <inheritdoc />
	public override String Deserialize(String value, Type type, RpcKeyValueSerializerOptions options) {
		return value.NotNull();
	} // Deserialize

} // RpcKeyValueSerializerConverterString
#endregion
