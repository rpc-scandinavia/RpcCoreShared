namespace RpcScandinavia.Core.Shared;
using System;

#region RpcKeyValueSerializerConverterGuid
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerConverterGuid.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// RPC Key/Value serializer converter, that serializes and deserialized to/from a <see cref="System.Guid" />.
/// </summary>
public class RpcKeyValueSerializerConverterGuid : RpcKeyValueSerializerConverter<Guid> {

	/// <inheritdoc />
	public override ReadOnlyMemory<Char> Serialize(Guid obj, RpcKeyValueSerializerOptions options) {
		return obj.ToString().AsMemory();
	} // Serialize

	/// <inheritdoc />
	public override Guid Deserialize(ReadOnlyMemory<Char> value, Type type, RpcKeyValueSerializerOptions options) {
		return Guid.Parse(value.Span);
	} // Deserialize

} // RpcKeyValueSerializerConverterGuid
#endregion
