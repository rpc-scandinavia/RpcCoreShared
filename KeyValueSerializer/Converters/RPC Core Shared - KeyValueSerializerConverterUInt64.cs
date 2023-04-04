namespace RpcScandinavia.Core.Shared.KeyValueSerializer;
using System;
using System.Globalization;

#region RpcKeyValueSerializerConverterUInt64
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerConverterUInt64.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// RPC Key/Value serializer converter, that serializes and deserialized to/from a <see cref="System.UInt64" />.
/// </summary>
public class RpcKeyValueSerializerConverterUInt64 : RpcKeyValueSerializerConverter<UInt64> {

	/// <inheritdoc />
	public override String Serialize(UInt64 obj, RpcKeyValueSerializerOptions options) {
		return obj.ToString(CultureInfo.InvariantCulture.NumberFormat);
	} // Serialize

	/// <inheritdoc />
	public override UInt64 Deserialize(String value, RpcKeyValueSerializerOptions options) {
		return UInt64.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
	} // Deserialize

} // RpcKeyValueSerializerConverterUInt64
#endregion
