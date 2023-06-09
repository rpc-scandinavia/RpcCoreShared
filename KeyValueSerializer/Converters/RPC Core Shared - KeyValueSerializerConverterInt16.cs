namespace RpcScandinavia.Core.Shared;
using System;
using System.Globalization;

#region RpcKeyValueSerializerConverterInt16
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerConverterInt16.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// RPC Key/Value serializer converter, that serializes and deserialized to/from a <see cref="System.Int16" />.
/// </summary>
public class RpcKeyValueSerializerConverterInt16 : RpcKeyValueSerializerConverter<Int16> {

	/// <inheritdoc />
	public override ReadOnlyMemory<Char> Serialize(Int16 obj, RpcKeyValueSerializerOptions options) {
		return obj.ToString(CultureInfo.InvariantCulture.NumberFormat).AsMemory();
	} // Serialize

	/// <inheritdoc />
	public override Int16 Deserialize(ReadOnlyMemory<Char> value, Type type, RpcKeyValueSerializerOptions options) {
		return Int16.Parse(value.Span, CultureInfo.InvariantCulture.NumberFormat);
	} // Deserialize

} // RpcKeyValueSerializerConverterInt16
#endregion
