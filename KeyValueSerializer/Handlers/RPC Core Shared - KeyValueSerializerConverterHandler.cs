namespace RpcScandinavia.Core.Shared.KeyValueSerializer;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

#region RpcKeyValueSerializerConverterHandler
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerConverterHandler.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// RPC Key/Value serializer handler, that serializes and deserializes any type that one of the converters can convert,
/// to and from one or more key/value objects.
/// </summary>
public class RpcKeyValueSerializerConverterHandler : RpcKeyValueSerializerHandler {

	#region Overridden methods
	//------------------------------------------------------------------------------------------------------------------
	// Overridden methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <inheritdoc />
	public override Boolean CanHandle(Type type, RpcKeyValueSerializerOptions options) {
		foreach (RpcKeyValueSerializerConverter converter in options.Converters) {
			if (converter.CanConvert(type) == true) {
				return true;
			}
		}

		// No converter for the specified type.
		return false;
	} // CanHandle

	/// <inheritdoc />
	public override void Serialize<KeyValueType>(Object obj, List<KeyValueType> values, String keyPrefix, Func<String, String, KeyValueType> createKeyValueInstance, RpcKeyValueSerializerOptions options) {

	} // Serialize

	/// <inheritdoc />
	public override Object Deserialize<KeyValueType>(Object obj, IEnumerable<KeyValueType> values, String keyPrefix, Func<KeyValueType, String> getKey, Func<KeyValueType, String> getValue, RpcKeyValueSerializerOptions options) {
		return null;
	} // Deserialize
	#endregion

} // RpcKeyValueSerializerConverterHandler
#endregion
