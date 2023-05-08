namespace RpcScandinavia.Core.Shared;
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
	public override ReadOnlyMemory<Char> Serialize(UInt64 obj, RpcKeyValueSerializerOptions options) {
		return obj.ToString(CultureInfo.InvariantCulture.NumberFormat).AsMemory();
	} // Serialize

	/// <inheritdoc />
	public override UInt64 Deserialize(ReadOnlyMemory<Char> value, Type type, RpcKeyValueSerializerOptions options) {
		return UInt64.Parse(value.Span, CultureInfo.InvariantCulture.NumberFormat);
	} // Deserialize

} // RpcKeyValueSerializerConverterUInt64
#endregion
