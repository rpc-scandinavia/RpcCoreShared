namespace RpcScandinavia.Core.Shared.KeyValueSerializer;
using System;
using System.Globalization;

#region RpcKeyValueSerializerConverterDecimal
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerConverterDecimal.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// RPC Key/Value serializer converter, that serializes and deserialized to/from a <see cref="System.Decimal" />.
/// </summary>
public class RpcKeyValueSerializerConverterDecimal : RpcKeyValueSerializerConverter<Decimal> {

	/// <inheritdoc />
	public override String Serialize(Decimal obj, RpcKeyValueSerializerOptions options) {
		return obj.ToString(CultureInfo.InvariantCulture.NumberFormat);
	} // Serialize

	/// <inheritdoc />
	public override Decimal Deserialize(String value, RpcKeyValueSerializerOptions options) {
		return Decimal.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
	} // Deserialize

} // RpcKeyValueSerializerConverterDecimal
#endregion
