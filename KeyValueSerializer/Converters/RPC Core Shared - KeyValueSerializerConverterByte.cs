namespace RpcScandinavia.Core.Shared.KeyValueSerializer;
using System;
using System.Globalization;

#region RpcKeyValueSerializerConverterByte
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerConverterByte.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// RPC Key/Value serializer converter, that serializes and deserialized to/from a <see cref="System.Byte" />.
/// </summary>
public class RpcKeyValueSerializerConverterByte : RpcKeyValueSerializerConverter<Byte> {

	/// <inheritdoc />
	public override String Serialize(Byte obj, RpcKeyValueSerializerOptions options) {
		return obj.ToString(CultureInfo.InvariantCulture.NumberFormat);
	} // Serialize

	/// <inheritdoc />
	public override Byte Deserialize(String value, RpcKeyValueSerializerOptions options) {
		return Byte.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
	} // Deserialize

} // RpcKeyValueSerializerConverterByte
#endregion
