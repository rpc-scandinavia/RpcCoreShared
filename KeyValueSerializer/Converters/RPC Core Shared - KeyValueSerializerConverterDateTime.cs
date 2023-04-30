namespace RpcScandinavia.Core.Shared;
using System;
using System.Globalization;

#region RpcKeyValueSerializerConverterDateTime
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerConverterDateTime.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// RPC Key/Value serializer converter, that serializes and deserialized to/from a <see cref="System.DateTime" />.
/// </summary>
public class RpcKeyValueSerializerConverterDateTime : RpcKeyValueSerializerConverter<DateTime> {

	/// <inheritdoc />
	public override String Serialize(DateTime obj, RpcKeyValueSerializerOptions options) {
		// Sortable pattern, "yyyy-MM-dd T HH:mm:ss".
		return obj.ToString("o");	// "o" or "r".
	} // Serialize

	/// <inheritdoc />
	public override DateTime Deserialize(String value, Type type, RpcKeyValueSerializerOptions options) {
		return DateTime.Parse(value, CultureInfo.InvariantCulture.DateTimeFormat, options.DeserializeDateTimeStyles);
	} // Deserialize

} // RpcKeyValueSerializerConverterDateTime
#endregion
