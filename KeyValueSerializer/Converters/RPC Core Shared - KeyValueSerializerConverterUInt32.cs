namespace RpcScandinavia.Core.Shared;
using System;
using System.Globalization;

#region RpcKeyValueSerializerConverterUInt32
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerConverterUInt32.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// RPC Key/Value serializer converter, that serializes and deserialized to/from a <see cref="System.UInt32" />.
/// </summary>
public class RpcKeyValueSerializerConverterUInt32 : RpcKeyValueSerializerConverter<UInt32> {

	/// <inheritdoc />
	public override ReadOnlyMemory<Char> Serialize(UInt32 obj, RpcKeyValueSerializerOptions options) {
		return obj.ToString(CultureInfo.InvariantCulture.NumberFormat).AsMemory();
	} // Serialize

	/// <inheritdoc />
	public override UInt32 Deserialize(ReadOnlyMemory<Char> value, Type type, RpcKeyValueSerializerOptions options) {
		return UInt32.Parse(value.Span, CultureInfo.InvariantCulture.NumberFormat);
	} // Deserialize

} // RpcKeyValueSerializerConverterUInt32
#endregion
