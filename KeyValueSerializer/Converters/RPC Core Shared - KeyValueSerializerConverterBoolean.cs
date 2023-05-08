namespace RpcScandinavia.Core.Shared;
using System;

#region RpcKeyValueSerializerConverterBoolean
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerConverterBoolean.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// RPC Key/Value serializer converter, that serializes and deserialized to/from a <see cref="System.Boolean" />.
/// </summary>
public class RpcKeyValueSerializerConverterBoolean : RpcKeyValueSerializerConverter<Boolean> {

	/// <inheritdoc />
	public override ReadOnlyMemory<Char> Serialize(Boolean obj, RpcKeyValueSerializerOptions options) {
		return obj.ToString().AsMemory();
	} // Serialize

	/// <inheritdoc />
	public override Boolean Deserialize(ReadOnlyMemory<Char> value, Type type, RpcKeyValueSerializerOptions options) {
		return Boolean.Parse(value.Span);
	} // Deserialize

} // RpcKeyValueSerializerConverterBoolean
#endregion
