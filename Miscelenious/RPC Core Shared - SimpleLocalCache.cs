namespace RpcScandinavia.Core.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

#region RpcSimpleLocalCache
//----------------------------------------------------------------------------------------------------------------------
// RpcSimpleLocalCache.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// The simple local cache uses a local dictionary to cache the values.
///
/// Note that the value provider delegate, must throw an exception when it can not find a value associated with the key.
/// </summary>
/// <typeparam name="TKey">The key type.</typeparam>
/// <typeparam name="TValue">The value type.</typeparam>
public class RpcSimpleLocalCache<TKey, TValue> {
	private Dictionary<TKey, TValue> cache;
	private Func<TKey, TValue> provideValue;

	/// <summary>
	/// Initialize a new cache.
	///
	/// Note that the value provider delegate, must throw an exception when it can not find a value associated with
	/// the key.
	/// </summary>
	/// <param name="provideValue">Function delegate that can provide the value, then the key is absent from the cache.</param>
	public RpcSimpleLocalCache(Func<TKey, TValue> provideValue) {
		// Validate.
		if (provideValue == null) {
			throw new NullReferenceException(nameof(provideValue));
		}

		// Initialize.
		this.cache = new Dictionary<TKey, TValue>();
		this.provideValue = provideValue;
	} // RpcSimpleLocalCache

	/// <summary>
	/// Gets the cached value associated with the key.
	/// When the key is absent from the cache, the value provider delegate is used to get the value associated with
	/// the key, which is then added to the cache.
	///
	/// Note that the value provider delegate, must throw an exception when it can not find a value associated with
	/// the key.
	/// </summary>
	/// <param name="key">The key.</param>
	/// <returns>The cached value.</returns>
	public TValue GetValue(TKey key) {
		// Validate.
		if (key == null) {
			throw new NullReferenceException(nameof(key));
		}

		// Add missing value to the cache.
		// This can throw an exception, if two threads add to the cache at the same time. I do not want to place the
		// key check in a SemaphoreSlim, because that vould force all other threads to wait, whenever one thread add
		// to the cache.
		if (this.cache.ContainsKey(key) == false) {
			try {
				TValue value = this.provideValue(key);
				if (value != null) {
					this.cache.Add(key, value);
				}
			} catch {}
		}

		// Return property information from the cache.
		return this.cache[key];
	} // GetPropertyInfos

} // RpcSimpleLocalCache
#endregion
