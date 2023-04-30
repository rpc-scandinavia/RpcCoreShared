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
		if (type.IsEnum == true) {
			FlagsAttribute typeEnumFlagsAttribute = type.GetCustomAttribute<FlagsAttribute>(true);
			return (typeEnumFlagsAttribute == null);
		} else {
			return false;
		}
	} // CanConvert

	/// <inheritdoc />
	public override String Serialize(Enum obj, RpcKeyValueSerializerOptions options) {
		switch (options.SerializeEnums) {
			case RpcKeyValueSerializerEnumOption.AsString:
				return Enum.GetName(obj.GetType(), obj).NotNull();
			case RpcKeyValueSerializerEnumOption.AsInteger:
			default:
				return Convert.ToInt64(obj).ToString();
		}
	} // Serialize

	/// <inheritdoc />
	public override Enum Deserialize(String value, Type type, RpcKeyValueSerializerOptions options) {
		Int64 valueNumeric = value.ToInt64(-1);
		if (valueNumeric > -1) {
			return (Enum)Enum.ToObject(type, valueNumeric);
		} else {
			return (Enum)Enum.Parse(type, value);
		}
	} // Deserialize

} // RpcKeyValueSerializerConverterEnum
#endregion
