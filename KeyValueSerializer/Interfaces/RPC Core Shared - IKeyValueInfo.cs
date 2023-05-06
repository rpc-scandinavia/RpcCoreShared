namespace RpcScandinavia.Core.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using RpcScandinavia.Core;

#region IRpcKeyValueInfo
//----------------------------------------------------------------------------------------------------------------------
// IRpcKeyValueInfo.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// The key/value info contain information about a single key/value.
/// It can be used to get and set the value.
/// </summary>
public interface IRpcKeyValueInfo {

	/// <summary>
	/// The path to the value. This does not include the key.
	/// </summary>
	public ReadOnlySpan<Char> Path { get; }

	/// <summary>
	/// The key to the value.
	/// </summary>
	public ReadOnlySpan<Char> Key { get; }

	/// <summary>
	/// The value group.
	/// </summary>
	public ReadOnlySpan<Char> Group { get; }

	/// <summary>
	/// The value group.
	/// </summary>
	public Int32 Order { get; }

	/// <summary>
	/// The value type.
	/// </summary>
	public Type DeclaringType { get; }

	/// <summary>
	/// The value type.
	/// </summary>
	public Type Type { get; }

	/// <summary>
	/// The function delegate that can get a custom attribute, when the value is from a field or from a property.
	/// This is null when the value is not from a field or from a property.
	/// </summary>
	public Func<Type, Boolean, Attribute> GetCustomAttributeDelegate { get; }

	/// <summary>
	/// The function delegate that can get the value.
	/// </summary>
	public Func<Object> GetValueDelegate { get; }

	/// <summary>
	/// The action delegate that can set the value.
	/// </summary>
	public Action<Object> SetValueDelegate { get; }

	/// <summary>
	/// Gets the custom attribute, when the value is from a field or from a property.
	/// This return null when the value is not from a field or from a property.
	/// </summary>
	/// <param name="inherit">Specify whether the ancestors of the member is inspected (true).</param>
	/// <typeparam name="TAttribute">The attribute type.</typeparam>
	/// <returns>The custom attribute that matches attribute type, or null if no such attribute is found.</returns>
	public TAttribute GetCustomAttribute<TAttribute>(Boolean inherit = true) where TAttribute: Attribute;

	/// <summary>
	/// Gets the value.
	/// </summary>
	/// <param name="defaultValue">The default value (default(T)).</param>
	/// <typeparam name="T">The value type.</typeparam>
	/// <returns>The value or the default value.</returns>
	public T GetValue<T>(T defaultValue = default(T));

	/// <summary>
	/// Sets the value.
	/// </summary>
	public void SetValue(Object value);

} // IRpcKeyValueInfo
#endregion
