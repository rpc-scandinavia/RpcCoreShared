namespace RpcScandinavia.Core.Shared;
using System;
using System.Globalization;

#region RpcKeyValueSerializerConverterSingle
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerConverterSingle.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// RPC Key/Value serializer converter, that serializes and deserialized to/from a <see cref="System.Single" />.
/// </summary>
public class RpcKeyValueSerializerConverterSingle : RpcKeyValueSerializerConverter<Single> {

	/// <inheritdoc />
	public override String Serialize(Single obj, RpcKeyValueSerializerOptions options) {
		return obj.ToString(CultureInfo.InvariantCulture.NumberFormat);
	} // Serialize

	/// <inheritdoc />
	public override Single Deserialize(String value, RpcKeyValueSerializerOptions options) {
		return Single.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
	} // Deserialize

} // RpcKeyValueSerializerConverterSingle
#endregion
