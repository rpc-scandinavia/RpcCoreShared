namespace RpcScandinavia.Core.Shared;
using System;
using System.Reflection;
using System.Text.Json.Serialization;
using RpcScandinavia.Core;

#region RpcKeyValueSerializerMemberInfoExtensions
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerMemberInfoExtensions.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Extension methods for the <see cref="System.Reflection.MemberInfo" /> class used by the RPC Key/Value serializer.
/// </summary>
public static class RpcKeyValueSerializerMemberInfoExtensions {

	/// <summary>
	/// Gets whether or not the <see cref="System.Reflection.MemberInfo" /> is decorated with the
	/// <see cref="System.Text.Json.Serialization.JsonIgnoreAttribute" /> attribute.
	/// </summary>
	/// <param name="memberInfo">The field or property member information.</param>
	/// <returns>True if the member is decorated; False when not.</returns>
	public static Boolean GetSerializerIgnore(this MemberInfo memberInfo) {
		JsonIgnoreAttribute jsonIgnoreAttribute = memberInfo.GetCustomAttribute<JsonIgnoreAttribute>(true);
		if (jsonIgnoreAttribute != null) {
			return (jsonIgnoreAttribute.Condition != JsonIgnoreCondition.Never);
		}

		return false;
	} // GetSerializerIgnore

	/// <summary>
	/// Gets whether or not the <see cref="System.Reflection.MemberInfo" /> is decorated with the
	/// <see cref="System.Text.Json.Serialization.JsonIncludeAttribute" /> attribute.
	/// </summary>
	/// <param name="memberInfo">The field or property member information.</param>
	/// <returns>True if the member is decorated; False when not.</returns>
	public static Boolean GetSerializerInclude(this MemberInfo memberInfo) {
		JsonIncludeAttribute jsonIncludeAttribute = memberInfo.GetCustomAttribute<JsonIncludeAttribute>(true);
		if (jsonIncludeAttribute != null) {
			return true;
		}

		return false;
	} // GetSerializerInclude

	/// <summary>
	/// Gets the name of the <see cref="System.Reflection.MemberInfo" />.
	/// The name is either <see cref="System.Reflection.MemberInfo.Name" /> or <see cref="System.Text.Json.Serialization.JsonPropertyNameAttribute.Name" /> when
	/// the member information is decorated with the <see cref="System.Text.Json.Serialization.JsonPropertyNameAttribute" /> attribute.
	/// </summary>
	/// <param name="memberInfo">The field or property member information.</param>
	/// <returns>The name.</returns>
	public static String GetSerializerName(this MemberInfo memberInfo) {
		JsonPropertyNameAttribute jsonPropertyNameAttribute = memberInfo.GetCustomAttribute<JsonPropertyNameAttribute>(true);
		if (jsonPropertyNameAttribute != null) {
			return jsonPropertyNameAttribute.Name;
		}

		return memberInfo.Name;
	} // GetSerializerName

	/// <summary>
	/// Gets the order of the <see cref="System.Reflection.MemberInfo" />.
	/// The order is either 0 (default) or or <see cref="System.Text.Json.Serialization.JsonPropertyOrderAttribute.Order" /> when
	/// the member information is decorated with the <see cref="System.Text.Json.Serialization.JsonPropertyOrderAttribute" /> attribute.
	/// </summary>
	/// <param name="memberInfo">The field or property member information.</param>
	/// <returns>The order.</returns>
	public static Int32 GetSerializerOrder(this MemberInfo memberInfo) {
		JsonPropertyOrderAttribute jsonPropertyOrderAttribute = memberInfo.GetCustomAttribute<JsonPropertyOrderAttribute>(true);
		if (jsonPropertyOrderAttribute != null) {
			return jsonPropertyOrderAttribute.Order;
		}

		return 0;
	} // GetSerializerOrder

	/// <summary>
	/// Gets the group name of the <see cref="System.Reflection.MemberInfo" />.
	/// The group name is either null (default) or or <see cref="RpcScandinavia.Core.Shared.RpcKeyValueSerializerGroupAttribute.GroupName" /> when
	/// the member information is decorated with the <see cref="RpcScandinavia.Core.Shared.RpcKeyValueSerializerGroupAttribute" /> attribute,
	/// or <see cref="RpcScandinavia.Core.Shared.JsonGroupAttribute.GroupName" /> when the member information is decorated with
	/// the <see cref="RpcScandinavia.Core.Shared.JsonGroupAttribute" /> attribute.
	/// </summary>
	/// <param name="memberInfo">The field or property member information.</param>
	/// <returns>The group name or null.</returns>
	public static String GetSerializerGroupName(this MemberInfo memberInfo) {
		RpcKeyValueSerializerGroupAttribute groupAttribute = memberInfo.GetCustomAttribute<RpcKeyValueSerializerGroupAttribute>(true);
		if (groupAttribute != null) {
			return groupAttribute.GroupName;
		}

		JsonGroupAttribute jsonGroupAttribute = memberInfo.GetCustomAttribute<JsonGroupAttribute>(true);
		if (jsonGroupAttribute != null) {
			return jsonGroupAttribute.GroupName;
		}

		return null;
	} // GetSerializerGroupName

} // RpcKeyValueSerializerMemberInfoExtensions
#endregion
