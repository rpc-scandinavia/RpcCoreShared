namespace RpcScandinavia.Core.Shared;
using System;
using System.Text;

#region RpcKeyValueSerializerConverterString
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerConverterString.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// RPC Key/Value serializer converter, that serializes and deserialized to/from a <see cref="System.String" />.
/// </summary>
public class RpcKeyValueSerializerConverterString : RpcKeyValueSerializerConverter<String> {

	/// <inheritdoc />
	public override ReadOnlyMemory<Char> Serialize(String obj, RpcKeyValueSerializerOptions options) {
		return obj.NotNull().AsMemory();
	} // Serialize

	/// <inheritdoc />
	public override String Deserialize(ReadOnlyMemory<Char> value, Type type, RpcKeyValueSerializerOptions options) {
		return value.Span.ToString();
	} // Deserialize

} // RpcKeyValueSerializerConverterString
#endregion
