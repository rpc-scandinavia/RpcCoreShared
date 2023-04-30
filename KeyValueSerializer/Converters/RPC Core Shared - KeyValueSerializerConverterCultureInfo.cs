using System;
using System.Collections.Generic;
using System.Globalization;
using RpcScandinavia.Core.Shared;
namespace RpcScandinavia.Core.Shared;

#region RpcKeyValueSerializerConverterCultureInfo
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerConverterCultureInfo.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// RPC Key/Value serializer converter, that serializes and deserialized to/from a <see cref="System.Globalization.CultureInfo" />.
/// </summary>
public class RpcKeyValueSerializerConverterCultureInfo : RpcKeyValueSerializerConverter<CultureInfo> {

	#region Methods
	//------------------------------------------------------------------------------------------------------------------
	// Methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Convert the <see cref="System.Globalization.CultureInfo" /> into a <see cref="System.String" />.
	/// </summary>
	/// <param name="object">The culture information.</param>
	/// <param name="options">The options.</param>
	/// <returns>The culture information converted into a string value.</returns>
	public override String Serialize(CultureInfo obj, RpcKeyValueSerializerOptions options) {
		return obj.ToString();
	} // Serialize

	/// <summary>
	/// Convert the <see cref="System.String" /> into a <see cref="System.Globalization.CultureInfo" />.
	/// </summary>
	/// <param name="value">The string value.</param>
	/// <param name="options">The options.</param>
	/// <returns>The string value converted into a culture information.</returns>
	public override CultureInfo Deserialize(String value, Type type, RpcKeyValueSerializerOptions options) {
		try {
			return new CultureInfo(value);
		} catch {
			return CultureInfo.InvariantCulture;
		}
	} // Deserialize
	#endregion

} // RpcKeyValueSerializerConverterCultureInfo
#endregion
