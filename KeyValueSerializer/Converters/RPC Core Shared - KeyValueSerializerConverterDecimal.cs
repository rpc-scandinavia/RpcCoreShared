namespace RpcScandinavia.Core.Shared;
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
	public override ReadOnlyMemory<Char> Serialize(Decimal obj, RpcKeyValueSerializerOptions options) {
		return obj.ToString(CultureInfo.InvariantCulture.NumberFormat).AsMemory();
	} // Serialize

	/// <inheritdoc />
	public override Decimal Deserialize(ReadOnlyMemory<Char> value, Type type, RpcKeyValueSerializerOptions options) {
		return Decimal.Parse(value.Span, CultureInfo.InvariantCulture.NumberFormat);
	} // Deserialize

} // RpcKeyValueSerializerConverterDecimal
#endregion
