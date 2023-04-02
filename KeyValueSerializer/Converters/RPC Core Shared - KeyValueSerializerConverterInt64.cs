namespace RpcScandinavia.Core.Shared;
using System;
using System.Globalization;

#region RpcKeyValueSerializerConverterInt64
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerConverterInt64.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// RPC Key/Value serializer converter, that serializes and deserialized to/from a <see cref="System.Int64" />.
/// </summary>
public class RpcKeyValueSerializerConverterInt64 : RpcKeyValueSerializerConverter<Int64> {

	/// <inheritdoc />
	public override String Serialize(Int64 obj, RpcKeyValueSerializerOptions options) {
		return obj.ToString(CultureInfo.InvariantCulture.NumberFormat);
	} // Serialize

	/// <inheritdoc />
	public override Int64 Deserialize(String value, RpcKeyValueSerializerOptions options) {
		return Int64.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
	} // Deserialize

} // RpcKeyValueSerializerConverterInt64
#endregion
