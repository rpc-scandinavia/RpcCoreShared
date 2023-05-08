namespace RpcScandinavia.Core.Shared;
using System;
using System.Globalization;

#region RpcKeyValueSerializerConverterDouble
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerConverterDouble.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// RPC Key/Value serializer converter, that serializes and deserialized to/from a <see cref="System.Double" />.
/// </summary>
public class RpcKeyValueSerializerConverterDouble : RpcKeyValueSerializerConverter<Double> {

	/// <inheritdoc />
	public override ReadOnlyMemory<Char> Serialize(Double obj, RpcKeyValueSerializerOptions options) {
		return obj.ToString(CultureInfo.InvariantCulture.NumberFormat).AsMemory();
	} // Serialize

	/// <inheritdoc />
	public override Double Deserialize(ReadOnlyMemory<Char> value, Type type, RpcKeyValueSerializerOptions options) {
		return Double.Parse(value.Span, CultureInfo.InvariantCulture.NumberFormat);
	} // Deserialize

} // RpcKeyValueSerializerConverterDouble
#endregion
