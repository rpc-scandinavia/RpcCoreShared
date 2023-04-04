namespace RpcScandinavia.Core.Shared.KeyValueSerializer;
using System;
using System.Reflection;
using System.Text.Json.Serialization;

#region RpcMemberInfoField
//----------------------------------------------------------------------------------------------------------------------
// RpcMemberInfoField.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// RPC Key/Value serializator uses reflection to gather information about all members in the types.
/// This member information holds information about a field. See <see cref="System.Reflection.FieldInfo" />.
/// </summary>
public class RpcMemberInfoField : RpcMemberInfo {
	private FieldInfo fieldInfo;

	private RpcKeyValueSerializerGroupAttribute fieldGroupAttribute;

	private JsonIgnoreAttribute fieldJsonIgnoreAttribute;
	private JsonIncludeAttribute fieldJsonIncludeAttribute;
	private JsonPropertyNameAttribute fieldJsonNameAttribute;
	private JsonPropertyOrderAttribute fieldJsonOrderAttribute;
	private JsonPropertyGroupAttribute fieldJsonGroupAttribute;

	#region Constructors
	//------------------------------------------------------------------------------------------------------------------
	// Constructors.
	//------------------------------------------------------------------------------------------------------------------
	public RpcMemberInfoField(FieldInfo fieldInfo) : base() {
		this.fieldInfo = fieldInfo;

		this.fieldGroupAttribute = this.fieldInfo.GetCustomAttribute<RpcKeyValueSerializerGroupAttribute>(true);

		this.fieldJsonIgnoreAttribute = this.fieldInfo.GetCustomAttribute<JsonIgnoreAttribute>(true);
		this.fieldJsonIncludeAttribute = this.fieldInfo.GetCustomAttribute<JsonIncludeAttribute>(true);
		this.fieldJsonNameAttribute = this.fieldInfo.GetCustomAttribute<JsonPropertyNameAttribute>(true);
		this.fieldJsonOrderAttribute = this.fieldInfo.GetCustomAttribute<JsonPropertyOrderAttribute>(true);
		this.fieldJsonGroupAttribute = this.fieldInfo.GetCustomAttribute<JsonPropertyGroupAttribute>(true);
	} // RpcTypeInfoField
	#endregion

	#region Overridden properties
	//------------------------------------------------------------------------------------------------------------------
	// Overridden properties.
	//------------------------------------------------------------------------------------------------------------------
	/// <inheritdoc />
	public override Type Type {
		get {
			return this.fieldInfo.FieldType;
		}
	} // Type

	/// <inheritdoc />
	public override Type DeclaringType {
		get {
			return this.fieldInfo.DeclaringType;
		}
	} // Type

	/// <inheritdoc />
	public override String Name {
		get {
			if (this.fieldJsonNameAttribute != null) {
				return this.fieldJsonNameAttribute.Name;
			} else {
				return this.fieldInfo.Name;
			}
		}
	} // Name

	/// <inheritdoc />
	public override Int32 Order {
		get {
			if (this.fieldJsonOrderAttribute != null) {
				return this.fieldJsonOrderAttribute.Order;
			} else {
				return 0;
			}
		}
	} // Order

	/// <inheritdoc />
	public override String Group {
		get {
			if (this.fieldGroupAttribute != null) {
				return this.fieldGroupAttribute.GroupName;
			} else if (this.fieldJsonGroupAttribute != null) {
				return this.fieldJsonGroupAttribute.GroupName;
			} else {
				return null;
			}
		}
	} // Group

	/// <inheritdoc />
	public override Boolean IsIncluded {
		get {
			return (this.fieldJsonIncludeAttribute != null);
		}
	} // IsIncluded

	/// <inheritdoc />
	public override Boolean IsIgnored {
		get {
			return (
				(this.fieldJsonIgnoreAttribute != null) &&
				(this.fieldJsonIgnoreAttribute.Condition != JsonIgnoreCondition.Never)
			);
		}
	} // IsIgnored

	/// <inheritdoc />
	public override Boolean IsAbstract {
		get {
			return this.fieldInfo.FieldType.IsAbstract;
		}
	} // IsAbstract

	/// <inheritdoc />
	public override Boolean IsPublicGet {
		get {
			return this.fieldInfo.IsPublic;
		}
	} // IsPublicGet

	/// <inheritdoc />
	public override Boolean IsPublicSet {
		get {
			return this.fieldInfo.IsPublic;
		}
	} // IsPublicSet

	/// <inheritdoc />
	public override Boolean IsPrivateGet {
		get {
			return this.fieldInfo.IsPrivate;
		}
	} // IsPrivateGet

	/// <inheritdoc />
	public override Boolean IsPrivateSet {
		get {
			return this.fieldInfo.IsPrivate;
		}
	} // IsPrivateSet

	/// <inheritdoc />
	public override Boolean IsReadOnly {
		get {
			return (this.fieldInfo.IsInitOnly == false);
		}
	} // IsReadOnly
	#endregion

	#region Overridden methods
	//------------------------------------------------------------------------------------------------------------------
	// Overridden methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <inheritdoc />
	public override AttributeType GetCustomAttribute<AttributeType>() {
		return this.fieldInfo.GetCustomAttribute<AttributeType>();
	} // GetCustomAttribute

	/// <inheritdoc />
	public override Object GetValue(Object obj) {
		return this.fieldInfo.GetValue(obj);
	} // GetValue

	/// <inheritdoc />
	public override void SetValue(Object obj, Object value) {
		this.fieldInfo.SetValue(obj, value);
	} // SetValue
	#endregion

} // RpcMemberInfoField
#endregion
