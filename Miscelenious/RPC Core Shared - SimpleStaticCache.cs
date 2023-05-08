namespace RpcScandinavia.Core.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

#region RpcSimpleStaticCache
//----------------------------------------------------------------------------------------------------------------------
// RpcSimpleStaticCache.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// The simple static cache uses a static dictionary to cache the values.
/// The isolation type is used to isolate the static cache, when several static caches are used at the same time with
/// the same key type and the same value type.
///
/// Note that the value provider delegate, must throw an exception when it can not find a value associated with the key.
/// </summary>
/// <typeparam name="TIsolation">The isolation type.</typeparam>
/// <typeparam name="TKey">The key type.</typeparam>
/// <typeparam name="TValue">The value type.</typeparam>
public class RpcSimpleStaticCache<TIsolation, TKey, TValue> {
	private static Dictionary<TIsolation, Dictionary<TKey, TValue>> staticCache = new Dictionary<TIsolation, Dictionary<TKey, TValue>>();

	private TIsolation isolation;
	private Dictionary<TKey, TValue> cache;
	private Func<TKey, TValue> provideValue;

	/// <summary>
	/// Initialize a new cache.
	///
	/// Note that the value provider delegate, must throw an exception when it can not find a value associated with
	/// the key.
	/// </summary>
	/// <param name="isolation">The isolation type is used to isolate the static cache, when several static caches are used at the same time with the same key type and the same value type.</param>
	/// <param name="provideValue">Function delegate that can provide the value, then the key is absent from the cache.</param>
	public RpcSimpleStaticCache(TIsolation isolation, Func<TKey, TValue> provideValue) {
		// Validate.
		if (isolation == null) {
			throw new NullReferenceException(nameof(isolation));
		}
		if (provideValue == null) {
			throw new NullReferenceException(nameof(provideValue));
		}

		// Initialize.
		this.isolation = isolation;
		this.provideValue = provideValue;

		// Add missing isolation to the cache.
		// This can throw an exception, if two threads add to the cache at the same time. I do not want to place the
		// key check in a SemaphoreSlim, because that vould force all other threads to wait, whenever one thread add
		// to the cache.
		if (RpcSimpleStaticCache<TIsolation, TKey, TValue>.staticCache.ContainsKey(isolation) == false) {
			try {
				RpcSimpleStaticCache<TIsolation, TKey, TValue>.staticCache.Add(isolation, new Dictionary<TKey, TValue>());
			} catch {}
		}

		this.cache = RpcSimpleStaticCache<TIsolation, TKey, TValue>.staticCache[isolation];
	} // RpcSimpleStaticCache

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

} // RpcSimpleStaticCache
#endregion
