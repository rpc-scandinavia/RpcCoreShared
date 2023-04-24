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
	public override String Serialize(Double obj, RpcKeyValueSerializerOptions options) {
		return obj.ToString(CultureInfo.InvariantCulture.NumberFormat);
	} // Serialize

	/// <inheritdoc />
	public override Double Deserialize(String value, RpcKeyValueSerializerOptions options) {
		return Double.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
	} // Deserialize

} // RpcKeyValueSerializerConverterDouble
#endregion
