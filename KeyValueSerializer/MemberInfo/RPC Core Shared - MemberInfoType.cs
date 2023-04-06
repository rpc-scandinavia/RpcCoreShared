namespace RpcScandinavia.Core.Shared.KeyValueSerializer;
using System;

#region RpcMemberInfoType
//----------------------------------------------------------------------------------------------------------------------
// RpcMemberInfoType.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// RPC Key/Value serializator uses reflection to gather information about all members in the types.
/// This member information holds information about a type (not direct in a collection). See <see cref="System.Type" />.
/// </summary>
public class RpcMemberInfoType : RpcMemberInfo {
	private Type type;
	private String name;
	private Func<Object, Object> getValue;
	private Action<Object, Object> setValue;

	#region Constructors
	//------------------------------------------------------------------------------------------------------------------
	// Constructors.
	//------------------------------------------------------------------------------------------------------------------
	public RpcMemberInfoType(Type type, String name) : base() {
		this.type = type;
		this.name = name;
		this.getValue = null;
		this.setValue = null;
	} // RpcTypeInfoField

	public RpcMemberInfoType(Type type, String name, Func<Object, Object> getValue, Action<Object, Object> setValue) : base() {
		this.type = type;
		this.name = name;
		this.getValue = getValue;
		this.setValue = setValue;
	} // RpcTypeInfoField
	#endregion

	#region Overridden properties
	//------------------------------------------------------------------------------------------------------------------
	// Overridden properties.
	//------------------------------------------------------------------------------------------------------------------
	/// <inheritdoc />
	public override Type Type {
		get {
			return this.type;
		}
	} // Type

	/// <inheritdoc />
	public override Type DeclaringType {
		get {
			return this.type.DeclaringType;
		}
	} // Type

	/// <inheritdoc />
	public override String Name {
		get {
			return this.name ?? this.type.Name;
		}
	} // Name

	/// <inheritdoc />
	public override Int32 Order {
		get {
			return 0;
		}
	} // Order

	/// <inheritdoc />
	public override String Group {
		get {
			return null;
		}
	} // Group

	/// <inheritdoc />
	public override Boolean IsIncluded {
		get {
			return true;
		}
	} // IsIncluded

	/// <inheritdoc />
	public override Boolean IsIgnored {
		get {
			return false;
		}
	} // IsIgnored

	/// <inheritdoc />
	public override Boolean IsAbstract {
		get {
			return this.type.IsAbstract;
		}
	} // IsAbstract

	/// <inheritdoc />
	public override Boolean IsPublicGet {
		get {
			return true;
		}
	} // IsPublicGet

	/// <inheritdoc />
	public override Boolean IsPublicSet {
		get {
			return true;
		}
	} // IsPublicSet

	/// <inheritdoc />
	public override Boolean IsPrivateGet {
		get {
			return true;
		}
	} // IsPrivateGet

	/// <inheritdoc />
	public override Boolean IsPrivateSet {
		get {
			return true;
		}
	} // IsPrivateSet

	/// <inheritdoc />
	public override Boolean IsReadOnly {
		get {
			return false;
		}
	} // IsReadOnly
	#endregion

	#region Overridden methods
	//------------------------------------------------------------------------------------------------------------------
	// Overridden methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <inheritdoc />
	public override AttributeType GetCustomAttribute<AttributeType>() {
		return null;
	} // GetCustomAttribute

	/// <inheritdoc />
	public override Object GetValue(Object obj) {
		if (this.getValue != null) {
			return this.getValue(obj);
		} else {
			return null;
		}
	} // GetValue

	/// <inheritdoc />
	public override void SetValue(Object obj, Object value) {
		if (this.setValue != null) {
			this.setValue(obj, value);
		}
	} // SetValue
	#endregion

} // RpcMemberInfoType
#endregion
