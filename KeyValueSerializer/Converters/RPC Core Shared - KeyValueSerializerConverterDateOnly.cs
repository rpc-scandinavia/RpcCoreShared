namespace RpcScandinavia.Core.Shared;
using System;
using System.Globalization;

#region RpcKeyValueSerializerConverterDateOnly
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerConverterDateOnly.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// RPC Key/Value serializer converter, that serializes and deserialized to/from a <see cref="System.DateOnly" />.
/// </summary>
public class RpcKeyValueSerializerConverterDateOnly : RpcKeyValueSerializerConverter<DateOnly> {

	/// <inheritdoc />
	public override ReadOnlyMemory<Char> Serialize(DateOnly obj, RpcKeyValueSerializerOptions options) {
		// Sortable pattern, "yyyy-MM-dd T HH:mm:ss".
		return obj.ToString("o").AsMemory();	// "o" or "r".
	} // Serialize

	/// <inheritdoc />
	public override DateOnly Deserialize(ReadOnlyMemory<Char> value, Type type, RpcKeyValueSerializerOptions options) {
		return DateOnly.Parse(value.Span, CultureInfo.InvariantCulture.DateTimeFormat, options.DeserializeDateTimeStyles);
	} // Deserialize

} // RpcKeyValueSerializerConverterDateOnly
#endregion
