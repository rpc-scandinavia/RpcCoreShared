namespace RpcScandinavia.Core.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using RpcScandinavia.Core;

#region RpcKeyValueInfo
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueInfo.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// The key/value info contain information about a single key/value.
/// It can be used to get and set the value.
/// </summary>
public class RpcKeyValueInfo : IRpcKeyValueInfo {
	private ReadOnlyMemory<Char> path;
	private ReadOnlyMemory<Char> key;
	private ReadOnlyMemory<Char> group;
	private Int32 order;
	private Type declaringType;
	private Type type;
	private Func<Type, Boolean, Attribute> getCustomAttribute;
	private Func<Object> getValue;
	private Action<Object> setValue;

	public RpcKeyValueInfo(ReadOnlyMemory<Char> path, ReadOnlyMemory<Char> key, Type type, Func<Object> getValue, Action<Object> setValue) : this(path, key, ReadOnlyMemory<Char>.Empty, 0, null, type, null, getValue, setValue) {
	} // RpcKeyValueInfo

	public RpcKeyValueInfo(ReadOnlyMemory<Char> path, ReadOnlyMemory<Char> key, ReadOnlyMemory<Char> group, Int32 order, Type declaringType, Type type, Func<Type, Boolean, Attribute> getCustomAttribute, Func<Object> getValue, Action<Object> setValue) {
		// Validate.
		if (key.Length == 0) {
			throw new ArgumentException(nameof(key));
		}
		if (type == null) {
			throw new NullReferenceException(nameof(type));
		}
		if (getValue == null) {
			throw new NullReferenceException(nameof(getValue));
		}
		if (setValue == null) {
			throw new NullReferenceException(nameof(setValue));
		}

		// Initialize.
		this.path = path;
		this.key = key;
		this.group = group;
		this.order = order;
		this.declaringType = declaringType;
		this.type = type;
		this.getCustomAttribute = getCustomAttribute;
		this.getValue = getValue;
		this.setValue = setValue;
	} // RpcKeyValueInfo

	/// <inheritdoc />
	public ReadOnlySpan<Char> Path {
		get {
			return this.path.Span;
		}
	} // Path

	/// <inheritdoc />
	public ReadOnlySpan<Char> Key {
		get {
			return this.key.Span;
		}
	} // Key

	/// <inheritdoc />
	public ReadOnlySpan<Char> Group {
		get {
			return this.group.Span;
		}
	} // Group

	/// <inheritdoc />
	public Int32 Order {
		get {
			return this.order;
		}
	} // Order

	/// <inheritdoc />
	public Type DeclaringType {
		get {
			return this.declaringType;
		}
	} // DeclaringType

	/// <inheritdoc />
	public Type Type {
		get {
			return this.type;
		}
	} // Type

	/// <inheritdoc />
	public Func<Type, Boolean, Attribute> GetCustomAttributeDelegate {
		get {
			return this.getCustomAttribute;
		}
	} // GetCustomAttributeDelegate

	/// <inheritdoc />
	public Func<Object> GetValueDelegate {
		get {
			return this.getValue;
		}
	} // GetValueDelegate

	/// <inheritdoc />
	public Action<Object> SetValueDelegate {
		get {
			return this.setValue;
		}
	} // SetValueDelegate

	/// <inheritdoc />
	public TAttribute GetCustomAttribute<TAttribute>(Boolean inherit = true) where TAttribute: Attribute {
		if (this.getCustomAttribute != null) {
			return (TAttribute)this.getCustomAttribute(typeof(TAttribute), inherit);
		} else {
			return null;
		}
	} // GetCustomAttribute

	/// <inheritdoc />
	public T GetValue<T>(T defaultValue = default(T)) {
		if (this.getValue != null) {
			return (T)this.getValue();
		} else {
			return defaultValue;
		}
	} // GetValue

	/// <inheritdoc />
	public void SetValue(Object value) {
		if (this.setValue != null) {
			this.setValue(value);
		}
	} // SetValue

} // RpcKeyValueInfo
#endregion
