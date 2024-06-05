using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
namespace RpcScandinavia.Core.Shared;

#region RpcCoreExtensions
//----------------------------------------------------------------------------------------------------------------------
// RpcCoreExtensions.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// This part of the class contains useful extension methods for the <see cref="System.Type" /> class.
/// </summary>
public static partial class RpcCoreExtensions {

	/// <summary>
	/// Gets true if the type is an enumerator.
	/// </summary>
	public static Boolean IsEnum(this Type type) {
		return (type.IsEnum == true);
	} // IsEnum

	/// <summary>
	/// Gets true if the type is an enumerator with the flags attribute.
	/// </summary>
	public static Boolean IsEnumWithFlags(this Type type) {
		if (type.IsEnum == true) {
			FlagsAttribute typeEnumFlagsAttribute = type.GetCustomAttribute<FlagsAttribute>(true);
			return (typeEnumFlagsAttribute != null);
		} else {
			return false;
		}
	} // IsEnumWithFlags

	/// <summary>
	/// Gets true if the type is a generic dictionary.
	/// </summary>
	public static Boolean IsGenericDictionary(this Type type) {
		return ((typeof(IDictionary).IsAssignableFrom(type) == true) && (type.IsGenericType == true));
	} // IsGenericDictionary

	/// <summary>
	/// Gets true if the type is a generic list.
	/// </summary>
	public static Boolean IsGenericList(this Type type) {
		return ((typeof(IList).IsAssignableFrom(type) == true) && (type.IsGenericType == true));
	} // IsGenericList

	/// <summary>
	/// Gets true if the type is an array.
	/// </summary>
	public static Boolean IsArray(this Type type) {
		return ((typeof(IList).IsAssignableFrom(type) == true) && (type.IsGenericType == false));
	} // IsGenericList

} // RpcCoreExtensions
#endregion
