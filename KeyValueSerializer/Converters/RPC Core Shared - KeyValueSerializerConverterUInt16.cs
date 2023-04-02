namespace RpcScandinavia.Core.Shared;
using System;
using System.Globalization;

#region RpcKeyValueSerializerConverterUInt16
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerConverterUInt16.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// RPC Key/Value serializer converter, that serializes and deserialized to/from a <see cref="System.UInt16" />.
/// </summary>
public class RpcKeyValueSerializerConverterUInt16 : RpcKeyValueSerializerConverter<UInt16> {

	/// <inheritdoc />
	public override String Serialize(UInt16 obj, RpcKeyValueSerializerOptions options) {
		return obj.ToString(CultureInfo.InvariantCulture.NumberFormat);
	} // Serialize

	/// <inheritdoc />
	public override UInt16 Deserialize(String value, RpcKeyValueSerializerOptions options) {
		return UInt16.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
	} // Deserialize

} // RpcKeyValueSerializerConverterUInt16
#endregion
