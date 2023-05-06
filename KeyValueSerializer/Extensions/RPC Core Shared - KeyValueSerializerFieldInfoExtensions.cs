namespace RpcScandinavia.Core.Shared;
using System;
using System.Reflection;
using System.Text.Json.Serialization;
using RpcScandinavia.Core;

#region RpcKeyValueSerializerFieldInfoExtensions
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerFieldInfoExtensions.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Extension methods for the <see cref="System.Reflection.FieldInfo" /> class used by the RPC Key/Value serializer.
/// </summary>
public static class RpcKeyValueSerializerFieldInfoExtensions {

	/// <summary>
	/// Gets whether or not the field information is for an abstract type.
	/// </summary>
	/// <param name="fieldInfo">The fieid member information.</param>
	/// <returns>True if the field type is abstract; False when not.</returns>
	public static Boolean GetMemberIsAbstract(this FieldInfo fieldInfo) {
		return fieldInfo.FieldType.IsAbstract;
	} // GetMemberIsAbstract

	/// <summary>
	/// Gets whether or not the field information is for a public field.
	/// </summary>
	/// <param name="fieldInfo">The fieid member information.</param>
	/// <returns>True if the field is public; False when not.</returns>
	public static Boolean GetMemberIsPublic(this FieldInfo fieldInfo) {
		return fieldInfo.IsPublic;
	} // GetMemberIsPublic

	/// <summary>
	/// Gets whether or not the field information is for a private field.
	/// </summary>
	/// <param name="fieldInfo">The fieid member information.</param>
	/// <returns>True if the field is private; False when not.</returns>
	public static Boolean GetMemberIsPrivate(this FieldInfo fieldInfo) {
		return fieldInfo.IsPrivate;
	} // GetMemberIsPrivate

	/// <summary>
	/// Gets whether or not the field information is for a read-only field.
	/// </summary>
	/// <param name="fieldInfo">The fieid member information.</param>
	/// <returns>True if the field is read-only; False when not.</returns>
	public static Boolean GetMemberIsReadOnly(this FieldInfo fieldInfo) {
		return (fieldInfo.IsInitOnly == false);
	} // GetMemberIsReadOnly

} // RpcKeyValueSerializerFieldInfoExtensions
#endregion
