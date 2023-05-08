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
	public override ReadOnlyMemory<Char> Serialize(UInt16 obj, RpcKeyValueSerializerOptions options) {
		return obj.ToString(CultureInfo.InvariantCulture.NumberFormat).AsMemory();
	} // Serialize

	/// <inheritdoc />
	public override UInt16 Deserialize(ReadOnlyMemory<Char> value, Type type, RpcKeyValueSerializerOptions options) {
		return UInt16.Parse(value.Span, CultureInfo.InvariantCulture.NumberFormat);
	} // Deserialize

} // RpcKeyValueSerializerConverterUInt16
#endregion
