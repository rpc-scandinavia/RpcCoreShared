namespace RpcScandinavia.Core.Shared;
using System;
using System.Globalization;

#region RpcKeyValueSerializerConverterByte
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerConverterByte.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// RPC Key/Value serializer converter, that serializes and deserialized to/from a <see cref="System.Byte" />.
/// </summary>
public class RpcKeyValueSerializerConverterByte : RpcKeyValueSerializerConverter<Byte> {

	/// <inheritdoc />
	public override ReadOnlyMemory<Char> Serialize(Byte obj, RpcKeyValueSerializerOptions options) {
		return obj.ToString(CultureInfo.InvariantCulture.NumberFormat).AsMemory();
	} // Serialize

	/// <inheritdoc />
	public override Byte Deserialize(ReadOnlyMemory<Char> value, Type type, RpcKeyValueSerializerOptions options) {
		return Byte.Parse(value.Span, CultureInfo.InvariantCulture.NumberFormat);
	} // Deserialize

} // RpcKeyValueSerializerConverterByte
#endregion
