using System;
namespace RpcScandinavia.Core.Shared;

/// <summary>
/// How the <see cref="RpcScandinavia.Core.Shared.RpcSimpleLocalCache{TKey,TValue}"/> and the <see cref="RpcScandinavia.Core.Shared.RpcSimpleStaticCache{TKey,TValue}"/>
/// handles null values, when the 'provideValue' delegate return null.
/// </summary>
public enum RpcSimpleCacheNullValues {

	/// <summary>
	/// Do not allow null values, and throw an exception.
	/// </summary>
	DenyNullValuesAndThrow = 0,

	/// <summary>
	/// Do not allow null values, but just return null.
	/// </summary>
	DenyNullValuesAndReturnDefault = 1,

	/// <summary>
	/// Allow null values, and return null.
	/// </summary>
	AllowNullValues = 2

} // RpcSimpleCacheNullValues
