namespace RpcScandinavia.Core.Shared;
using System;
using System.Globalization;

#region RpcKeyValueSerializerConverterSingle
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerConverterSingle.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// RPC Key/Value serializer converter, that serializes and deserialized to/from a <see cref="System.Single" />.
/// </summary>
public class RpcKeyValueSerializerConverterSingle : RpcKeyValueSerializerConverter<Single> {

	/// <inheritdoc />
	public override ReadOnlyMemory<Char> Serialize(Single obj, RpcKeyValueSerializerOptions options) {
		return obj.ToString(CultureInfo.InvariantCulture.NumberFormat).AsMemory();
	} // Serialize

	/// <inheritdoc />
	public override Single Deserialize(ReadOnlyMemory<Char> value, Type type, RpcKeyValueSerializerOptions options) {
		return Single.Parse(value.Span, CultureInfo.InvariantCulture.NumberFormat);
	} // Deserialize

} // RpcKeyValueSerializerConverterSingle
#endregion
