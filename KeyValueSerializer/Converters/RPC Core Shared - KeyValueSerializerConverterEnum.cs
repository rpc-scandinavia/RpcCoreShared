namespace RpcScandinavia.Core.Shared;
using System;
using System.Reflection;

#region RpcKeyValueSerializerConverterEnum
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerConverterEnum.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// RPC Key/Value serializer converter, that serializes and deserialized to/from a <see cref="System.Enum" />.
/// </summary>
public class RpcKeyValueSerializerConverterEnum : RpcKeyValueSerializerConverter<Enum> {

	/// <inheritdoc />
	public override Boolean CanConvert(Type type) {
		return ((type.IsEnum() == true) && (type.IsEnumWithFlags() == false));
	} // CanConvert

	/// <inheritdoc />
	public override ReadOnlyMemory<Char> Serialize(Enum obj, RpcKeyValueSerializerOptions options) {
		return RpcKeyValueSerializerEnumExtensions.GetEnumUnderlayingValue(obj, options).ToString().AsMemory();
	} // Serialize

	/// <inheritdoc />
	public override Enum Deserialize(ReadOnlyMemory<Char> value, Type type, RpcKeyValueSerializerOptions options) {
		return (Enum)RpcKeyValueSerializerEnumExtensions.GetEnum(type, value.Span.ToString());
	} // Deserialize

} // RpcKeyValueSerializerConverterEnum
#endregion
