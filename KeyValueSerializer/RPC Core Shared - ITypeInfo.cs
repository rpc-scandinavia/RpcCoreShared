namespace RpcScandinavia.Core.Shared;
using System;
using System.Collections.Generic;

#region IRpcTypeInfo
//----------------------------------------------------------------------------------------------------------------------
// IRpcTypeInfo.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// </summary>
public interface IRpcTypeInfo {

	/// <summary>
	/// Gets the key.
	/// Note that the keys separates each level in the type hierarchy with a colon (:).
	/// </summary>
	public String Key { get; }

	/// <summary>
	/// Gets the name.
	/// </summary>
	public String Name { get; }

	/// <summary>
	/// Gets the group.
	/// Note this may be null.
	/// </summary>
	public String Group { get; }

	/// <summary>
	/// Gets the order.
	/// </summary>
	public Int32 Order { get; }

	/// <summary>
	/// Gets the type.
	/// The declaring type is the parent and the type is the child.
	/// </summary>
	public Type Type { get; }

	/// <summary>
	/// Gets the type that declares the current member type.
	/// </summary>
	public Type DeclaringType { get; }

	/// <summary>
	/// Gets the attribute decorating the member.
	/// </summary>
	/// <typeparam name="AttributeType">The attribute type.</typeparam>
	/// <returns>The attribute or null.</returns>
	public AttributeType GetCustomAttribute<AttributeType>() where AttributeType: Attribute;

	/// <summary>
	/// Gets the value.
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <param name="defaultValue">The default value.</param>
	/// <typeparam name="ValueType">The type of the value.</typeparam>
	/// <returns>The value or the default value.</returns>
	public ValueType GetValue<ValueType>(Object obj, ValueType defaultValue = default(ValueType));

	/// <summary>
	/// Sets a value.
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <param name="value">The value.</param>
	public void SetValue(Object obj, Object value);

} // IRpcTypeInfo
#endregion
