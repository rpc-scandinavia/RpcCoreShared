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
	public override String Serialize(DateOnly obj, RpcKeyValueSerializerOptions options) {
		// Sortable pattern, "yyyy-MM-dd T HH:mm:ss".
		return obj.ToString("o");	// "o" or "r".
	} // Serialize

	/// <inheritdoc />
	public override DateOnly Deserialize(String value, RpcKeyValueSerializerOptions options) {
		return DateOnly.Parse(value, CultureInfo.InvariantCulture.DateTimeFormat, options.DeserializeDateTimeStyles);
	} // Deserialize

} // RpcKeyValueSerializerConverterDateOnly
#endregion
