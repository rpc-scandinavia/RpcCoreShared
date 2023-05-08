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
	public override ReadOnlyMemory<Char> Serialize(Int64 obj, RpcKeyValueSerializerOptions options) {
		return obj.ToString(CultureInfo.InvariantCulture.NumberFormat).AsMemory();
	} // Serialize

	/// <inheritdoc />
	public override Int64 Deserialize(ReadOnlyMemory<Char> value, Type type, RpcKeyValueSerializerOptions options) {
		return Int64.Parse(value.Span, CultureInfo.InvariantCulture.NumberFormat);
	} // Deserialize

} // RpcKeyValueSerializerConverterInt64
#endregion
