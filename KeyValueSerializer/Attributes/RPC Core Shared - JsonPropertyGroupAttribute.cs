namespace RpcScandinavia.Core.Shared.KeyValueSerializer;
using System;
using RpcScandinavia.Core;

#region JsonPropertyGroupAttribute
//----------------------------------------------------------------------------------------------------------------------
// JsonPropertyGroupAttribute.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Specify the property grouping present in the key/values when serializing.
/// Undecorated or null values are serialized first. If the attribute is not specified, the default value is null.
///
/// This attribute does the same as the <see cref="RpcScandinavia.Core.Shared.KeyValueSerializer.RpcKeyValueSerializerGroupAttribute" /> attribute,
/// it only exists to provide a attribute with a similar name, as the other JSON serialization attributes.
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public class JsonPropertyGroupAttribute : RpcKeyValueSerializerGroupAttribute {

	#region Constructors
	//------------------------------------------------------------------------------------------------------------------
	// Constructors.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Instantiates a new JSON property group attribute.
	/// </summary>
	/// <param name="groupName">The name of the group.</param>
	public JsonPropertyGroupAttribute(String groupName) : base(groupName) {
	} // JsonPropertyGroupAttribute
	#endregion

} // JsonPropertyGroupAttribute
#endregion
