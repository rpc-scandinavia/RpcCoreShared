namespace RpcScandinavia.Core.Shared;
using System;
using System.Globalization;

#region RpcKeyValueSerializerConverterSByte
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerConverterSByte.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// RPC Key/Value serializer converter, that serializes and deserialized to/from a <see cref="System.SByte" />.
/// </summary>
public class RpcKeyValueSerializerConverterSByte : RpcKeyValueSerializerConverter<SByte> {

	/// <inheritdoc />
	public override ReadOnlyMemory<Char> Serialize(SByte obj, RpcKeyValueSerializerOptions options) {
		return obj.ToString(CultureInfo.InvariantCulture.NumberFormat).AsMemory();
	} // Serialize

	/// <inheritdoc />
	public override SByte Deserialize(ReadOnlyMemory<Char> value, Type type, RpcKeyValueSerializerOptions options) {
		return SByte.Parse(value.Span, CultureInfo.InvariantCulture.NumberFormat);
	} // Deserialize

} // RpcKeyValueSerializerConverterSByte
#endregion
