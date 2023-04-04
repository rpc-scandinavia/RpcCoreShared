namespace RpcScandinavia.Core.Shared.KeyValueSerializer;
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
	public override String Serialize(TimeOnly obj, RpcKeyValueSerializerOptions options) {
		// Sortable pattern, "HH:mm:ss".
		return obj.ToString("o");	// "o" or "r".
	} // Serialize

	/// <inheritdoc />
	public override TimeOnly Deserialize(String value, RpcKeyValueSerializerOptions options) {
		return TimeOnly.Parse(value, CultureInfo.InvariantCulture.DateTimeFormat, options.DeserializeDateTimeStyles);
	} // Deserialize

} // RpcKeyValueSerializerConverterTimeOnly
#endregion
