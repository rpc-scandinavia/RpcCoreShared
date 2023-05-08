namespace RpcScandinavia.Core.Shared;
using System;
using System.Globalization;

#region RpcKeyValueSerializerConverterTimeOnly
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerConverterTimeOnly.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// RPC Key/Value serializer converter, that serializes and deserialized to/from a <see cref="System.TimeOnly" />.
/// </summary>
public class RpcKeyValueSerializerConverterTimeOnly : RpcKeyValueSerializerConverter<TimeOnly> {

	/// <inheritdoc />
	public override ReadOnlyMemory<Char> Serialize(TimeOnly obj, RpcKeyValueSerializerOptions options) {
		// Sortable pattern, "HH:mm:ss".
		return obj.ToString("o").AsMemory();	// "o" or "r".
	} // Serialize

	/// <inheritdoc />
	public override TimeOnly Deserialize(ReadOnlyMemory<Char> value, Type type, RpcKeyValueSerializerOptions options) {
		return TimeOnly.Parse(value.Span, CultureInfo.InvariantCulture.DateTimeFormat, options.DeserializeDateTimeStyles);
	} // Deserialize

} // RpcKeyValueSerializerConverterTimeOnly
#endregion
