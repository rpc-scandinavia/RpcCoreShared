namespace RpcScandinavia.Core.Shared;
using System;
using System.Reflection;
using System.Text.Json.Serialization;

#region RpcMemberInfoProperty
//----------------------------------------------------------------------------------------------------------------------
// RpcMemberInfoProperty.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// The RPC Type Hierarchy uses reflection to gather information about all members in the type hierarchy.
/// This member information holds information about a property. See <see cref="System.Reflection.PropertyInfo" />.
/// </summary>
public class RpcMemberInfoProperty : RpcMemberInfo {
	private PropertyInfo propertyInfo;
	private MethodInfo propertyGetMethod;
	private MethodInfo propertySetMethod;

	private RpcKeyValueSerializerGroupAttribute propertyGroupAttribute;

	private JsonIgnoreAttribute propertyJsonIgnoreAttribute;
	private JsonIncludeAttribute propertyJsonIncludeAttribute;
	private JsonPropertyNameAttribute propertyJsonNameAttribute;
	private JsonPropertyOrderAttribute propertyJsonOrderAttribute;
	private JsonGroupAttribute propertyJsonGroupAttribute;

	#region Constructors
	//------------------------------------------------------------------------------------------------------------------
	// Constructors.
	//------------------------------------------------------------------------------------------------------------------
	public RpcMemberInfoProperty(PropertyInfo propertyInfo) : base() {
		this.propertyInfo = propertyInfo;
		this.propertyGetMethod = this.propertyInfo.GetGetMethod(true);
		this.propertySetMethod = this.propertyInfo.GetSetMethod(true);

		this.propertyGroupAttribute = this.propertyInfo.GetCustomAttribute<RpcKeyValueSerializerGroupAttribute>(true);

		this.propertyJsonIgnoreAttribute = this.propertyInfo.GetCustomAttribute<JsonIgnoreAttribute>(true);
		this.propertyJsonIncludeAttribute = this.propertyInfo.GetCustomAttribute<JsonIncludeAttribute>(true);
		this.propertyJsonNameAttribute = this.propertyInfo.GetCustomAttribute<JsonPropertyNameAttribute>(true);
		this.propertyJsonOrderAttribute = this.propertyInfo.GetCustomAttribute<JsonPropertyOrderAttribute>(true);
		this.propertyJsonGroupAttribute = this.propertyInfo.GetCustomAttribute<JsonGroupAttribute>(true);
	} // RpcTypeInfoProperty
	#endregion

	#region Overridden properties
	//------------------------------------------------------------------------------------------------------------------
	// Overridden properties.
	//------------------------------------------------------------------------------------------------------------------
	/// <inheritdoc />
	public override Type Type {
		get {
			return this.propertyInfo.PropertyType;
		}
	} // Type

	/// <inheritdoc />
	public override Type DeclaringType {
		get {
			return this.propertyInfo.DeclaringType;
		}
	} // Type

	/// <inheritdoc />
	public override String Name {
		get {
			if (this.propertyJsonNameAttribute != null) {
				return this.propertyJsonNameAttribute.Name;
			} else {
				return this.propertyInfo.Name;
			}
		}
	} // Name

	/// <inheritdoc />
	public override Int32 Order {
		get {
			if (this.propertyJsonOrderAttribute != null) {
				return this.propertyJsonOrderAttribute.Order;
			} else {
				return 0;
			}
		}
	} // Order

	/// <inheritdoc />
	public override String Group {
		get {
			if (this.propertyGroupAttribute != null) {
				return this.propertyGroupAttribute.GroupName;
			} else if (this.propertyJsonGroupAttribute != null) {
				return this.propertyJsonGroupAttribute.GroupName;
			} else {
				return null;
			}
		}
	} // Group

	/// <inheritdoc />
	public override Boolean IsIncluded {
		get {
			return (this.propertyJsonIncludeAttribute != null);
		}
	} // IsIncluded

	/// <inheritdoc />
	public override Boolean IsIgnored {
		get {
			return (
				(this.propertyJsonIgnoreAttribute != null) &&
				(this.propertyJsonIgnoreAttribute.Condition != JsonIgnoreCondition.Never)
			);
		}
	} // IsIgnored

	/// <inheritdoc />
	public override Boolean IsAbstract {
		get {
			return this.propertyInfo.PropertyType.IsAbstract;
		}
	} // IsAbstract

	/// <inheritdoc />
	public override Boolean IsPublicGet {
		get {
			if (this.propertyGetMethod != null) {
				return (this.propertyGetMethod.IsPublic == true);
			} else {
				return false;
			}
		}
	} // IsPublicGet

	/// <inheritdoc />
	public override Boolean IsPublicSet {
		get {
			if (this.propertyGetMethod != null) {
				return (this.propertySetMethod.IsPublic == true);
			} else {
				return false;
			}
		}
	} // IsPublicSet

	/// <inheritdoc />
	public override Boolean IsPrivateGet {
		get {
			if (this.propertyGetMethod != null) {
				return (this.propertyGetMethod.IsPrivate == true);
			} else {
				return false;
			}
		}
	} // IsPrivateGet

	/// <inheritdoc />
	public override Boolean IsPrivateSet {
		get {
			if (this.propertyGetMethod != null) {
				return (this.propertySetMethod.IsPrivate == true);
			} else {
				return false;
			}
		}
	} // IsPrivateSet

	/// <inheritdoc />
	public override Boolean IsReadOnly {
		get {
			return (this.propertyInfo.CanWrite == false);
		}
	} // IsReadOnly
	#endregion

	#region Overridden methods
	//------------------------------------------------------------------------------------------------------------------
	// Overridden methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <inheritdoc />
	public override AttributeType GetCustomAttribute<AttributeType>() {
		return this.propertyInfo.GetCustomAttribute<AttributeType>();
	} // GetCustomAttribute

	/// <inheritdoc />
	public override Object GetValue(Object obj) {
		return this.propertyInfo.GetValue(obj);
	} // GetValue

	/// <inheritdoc />
	public override void SetValue(Object obj, Object value) {
		this.propertyInfo.SetValue(obj, value);
	} // SetValue
	#endregion

} // RpcMemberInfoProperty
#endregion
