using System;
using System.Collections;
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

	extension (Type type) {
		/// <summary>
		/// Gets true if the type is an enumerator.
		/// </summary>
		public Boolean IsEnum() {
			return (type.IsEnum == true);
		} // IsEnum

		/// <summary>
		/// Gets true if the type is an enumerator with the flags attribute.
		/// </summary>
		public Boolean IsEnumWithFlags() {
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
		public Boolean IsGenericDictionary() {
			return ((typeof(IDictionary).IsAssignableFrom(type) == true) && (type.IsGenericType == true));
		} // IsGenericDictionary

		/// <summary>
		/// Gets true if the type is a generic list.
		/// </summary>
		public Boolean IsGenericList() {
			return ((typeof(IList).IsAssignableFrom(type) == true) && (type.IsGenericType == true));
		} // IsGenericList

		/// <summary>
		/// Gets true if the type is an array.
		/// </summary>
		public Boolean IsArray() {
			return ((typeof(IList).IsAssignableFrom(type) == true) && (type.IsGenericType == false));
		} // IsGenericList

		/// <summary>
		/// Gets true if the type is a generic <see cref="RpcScandinavia.Core.Shared.RpcDictionaryList{TKey,TValue}"/>.
		/// </summary>
		public Boolean IsRpcDictionaryList() {
			return (
				(type.IsGenericType == true) &&
				(type.GetGenericTypeDefinition().FullName == "RpcScandinavia.Core.Shared.RpcDictionaryList`2")
			);
		} // IsRpcDictionaryList

	}

} // RpcCoreExtensions
#endregion
