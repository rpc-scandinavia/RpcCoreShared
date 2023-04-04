namespace RpcScandinavia.Core.Shared.KeyValueSerializer;
using System;
using System.Globalization;

#region RpcKeyValueSerializerConverterSByte
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerConverterSByte.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// RPC Key/Value serializer converter, that serializes and deserialized to/from a <see cref="System.SByte" />.
/// </summary>
public class RpcKeyValueSerializerConverterSByte : RpcKeyValueSerializerConverter<SByte> {

	/// <inheritdoc />
	public override String Serialize(SByte obj, RpcKeyValueSerializerOptions options) {
		return obj.ToString(CultureInfo.InvariantCulture.NumberFormat);
	} // Serialize

	/// <inheritdoc />
	public override SByte Deserialize(String value, RpcKeyValueSerializerOptions options) {
		return SByte.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
	} // Deserialize

} // RpcKeyValueSerializerConverterSByte
#endregion
