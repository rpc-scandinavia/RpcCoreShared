namespace RpcScandinavia.Core.Shared;
using System;
using System.Globalization;

#region RpcKeyValueSerializerConverterInt32
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerConverterInt32.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// RPC Key/Value serializer converter, that serializes and deserialized to/from a <see cref="System.Int32" />.
/// </summary>
public class RpcKeyValueSerializerConverterInt32 : RpcKeyValueSerializerConverter<Int32> {

	/// <inheritdoc />
	public override ReadOnlyMemory<Char> Serialize(Int32 obj, RpcKeyValueSerializerOptions options) {
		return obj.ToString(CultureInfo.InvariantCulture.NumberFormat).AsMemory();
	} // Serialize

	/// <inheritdoc />
	public override Int32 Deserialize(ReadOnlyMemory<Char> value, Type type, RpcKeyValueSerializerOptions options) {
		return Int32.Parse(value.Span, CultureInfo.InvariantCulture.NumberFormat);
	} // Deserialize

} // RpcKeyValueSerializerConverterInt32
#endregion
