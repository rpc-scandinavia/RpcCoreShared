namespace RpcScandinavia.Core.Shared;
using System;
using System.Reflection;
using System.Text.Json.Serialization;
using RpcScandinavia.Core;

#region RpcKeyValueSerializerPropertyInfoExtensions
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerPropertyInfoExtensions.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Extension methods for the <see cref="System.Reflection.PropertyInfo" /> class used by the RPC Key/Value serializer.
/// </summary>
public static class RpcKeyValueSerializerPropertyInfoExtensions {

	/// <summary>
	/// Gets whether or not the property information is for an abstract type.
	/// </summary>
	/// <param name="propertyInfo">The property member information.</param>
	/// <returns>True if the property type is abstract; False when not.</returns>
	public static Boolean GetMemberIsAbstract(this PropertyInfo propertyInfo) {
		return propertyInfo.PropertyType.IsAbstract;
	} // GetMemberIsAbstract

	/// <summary>
	/// Gets whether or not the property information is for a property with a public getter.
	/// </summary>
	/// <param name="propertyInfo">The property member information.</param>
	/// <returns>True if the property has a public getter; False when not.</returns>
	public static Boolean GetMemberIsPublicGet(this PropertyInfo propertyInfo) {
		return propertyInfo.GetGetMethod(true)?.IsPublic ?? false;
	} // GetMemberIsPublicGet

	/// <summary>
	/// Gets whether or not the property information is for a property with a public setter.
	/// </summary>
	/// <param name="propertyInfo">The property member information.</param>
	/// <returns>True if the property has a public setter; False when not.</returns>
	public static Boolean GetMemberIsPublicSet(this PropertyInfo propertyInfo) {
		return propertyInfo.GetSetMethod(true)?.IsPublic ?? false;
	} // GetMemberIsPublicSet

	/// <summary>
	/// Gets whether or not the property information is for a property with a private getter.
	/// </summary>
	/// <param name="propertyInfo">The property member information.</param>
	/// <returns>True if the property has a private getter; False when not.</returns>
	public static Boolean GetMemberIsPrivateGet(this PropertyInfo propertyInfo) {
		return propertyInfo.GetGetMethod(true)?.IsPrivate ?? false;
	} // GetMemberIsPrivateGet

	/// <summary>
	/// Gets whether or not the property information is for a property with a private setter.
	/// </summary>
	/// <param name="propertyInfo">The property member information.</param>
	/// <returns>True if the property has a private setter; False when not.</returns>
	public static Boolean GetMemberIsPrivateSet(this PropertyInfo propertyInfo) {
		return propertyInfo.GetSetMethod(true)?.IsPrivate ?? false;
	} // GetMemberIsPrivateSet

	/// <summary>
	/// Gets whether or not the property information is for a read-only property.
	/// </summary>
	/// <param name="propertyInfo">The property member information.</param>
	/// <returns>True if the property is read-only; False when not.</returns>
	public static Boolean GetMemberIsReadOnly(this PropertyInfo propertyInfo) {
		return (propertyInfo.CanWrite == false);
	} // GetMemberIsReadOnly

} // RpcKeyValueSerializerPropertyInfoExtensions
#endregion
