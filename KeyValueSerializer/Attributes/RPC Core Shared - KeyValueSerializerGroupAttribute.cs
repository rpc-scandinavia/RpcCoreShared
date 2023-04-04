namespace RpcScandinavia.Core.Shared.KeyValueSerializer;
using System;
using RpcScandinavia.Core;

#region RpcKeyValueSerializerGroupAttribute
//----------------------------------------------------------------------------------------------------------------------
// KeyValueSerializerGroupAttribute.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Specify the member grouping present in the key/values when serializing.
/// Undecorated or null values are serialized first. If the attribute is not specified, the default value is null.
///
/// This attribute overrides the <see cref="RpcScandinavia.Core.Shared.KeyValueSerializer.JsonPropertyGroupAttribute" /> attribute.
/// </summary>
[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public class RpcKeyValueSerializerGroupAttribute : Attribute {
	private readonly String groupName;

	#region Constructors
	//------------------------------------------------------------------------------------------------------------------
	// Constructors.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Instantiates a new group attribute.
	/// </summary>
	/// <param name="groupName">The name of the group.</param>
	public RpcKeyValueSerializerGroupAttribute(String groupName) {
		this.groupName = groupName;
	} // RpcKeyValueSerializerGroupAttribute
	#endregion

	#region Properties
	//------------------------------------------------------------------------------------------------------------------
	// Properties.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the name of the group.
	/// </summary>
	public String GroupName {
		get {
			return this.groupName;
		}
	} // GroupName
	#endregion

} // RpcKeyValueSerializerGroupAttribute
#endregion
