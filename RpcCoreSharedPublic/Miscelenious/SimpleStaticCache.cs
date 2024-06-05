using System;
using System.Collections.Generic;
using System.Threading;
namespace RpcScandinavia.Core.Shared;

#region RpcSimpleStaticCache
//----------------------------------------------------------------------------------------------------------------------
// RpcSimpleStaticCache.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// The simple static cache uses a static dictionary to cache the values. When the cache do not contain the requested
/// value, the provider delegate specified in the constructor is called to get the requested value, which is added to
/// the cache and returned.
///
/// The isolation type is used to isolate the static cache, when several static caches are used at the same time with
/// the same key type and the same value type.
///
/// Note that the value provider delegate, must throw an exception when it can not find a value associated with the key.
/// </summary>
/// <typeparam name="TIsolation">The isolation type.</typeparam>
/// <typeparam name="TKey">The key type.</typeparam>
/// <typeparam name="TValue">The value type.</typeparam>
public class RpcSimpleStaticCache<TIsolation, TKey, TValue> {
	private static SemaphoreSlim semaphoreIsolation = new SemaphoreSlim(1);
	private static SemaphoreSlim semaphoreKey = new SemaphoreSlim(1);
	private static Dictionary<TIsolation, Dictionary<TKey, TValue>> staticCache = new Dictionary<TIsolation, Dictionary<TKey, TValue>>();

	private TIsolation isolation;
	private Dictionary<TKey, TValue> cache;
	private Func<TKey, TValue> provideValue;
	private RpcSimpleCacheNullValues handleNullValue;

	/// <summary>
	/// Initialize a new cache.
	///
	/// Note that the value provider delegate, must throw an exception when it can not find a value associated with
	/// the key.
	/// </summary>
	/// <param name="isolation">The isolation type is used to isolate the static cache, when several static caches are used at the same time with the same key type and the same value type.</param>
	/// <param name="handleNullValue">How the cache handles null values.</param>
	/// <param name="provideValue">Function delegate that can provide the value, then the key is absent from the cache.</param>
	public RpcSimpleStaticCache(TIsolation isolation, RpcSimpleCacheNullValues handleNullValue, Func<TKey, TValue> provideValue) {
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
		this.handleNullValue = handleNullValue;

		// Add missing isolation to the cache.
		if (RpcSimpleStaticCache<TIsolation, TKey, TValue>.staticCache.ContainsKey(isolation) == false) {
			try {
				RpcSimpleStaticCache<TIsolation, TKey, TValue>.semaphoreIsolation.Wait();
				if (RpcSimpleStaticCache<TIsolation, TKey, TValue>.staticCache.ContainsKey(isolation) == false) {
					RpcSimpleStaticCache<TIsolation, TKey, TValue>.staticCache.Add(isolation, new Dictionary<TKey, TValue>());
				}
			} finally {
				RpcSimpleStaticCache<TIsolation, TKey, TValue>.semaphoreIsolation.Release();
			}
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
		if (this.cache.ContainsKey(key) == false) {
			try {
				// The "provideValue" call must be here before the semaphore, in case it uses the cache.
				TValue value = this.provideValue(key);

				RpcSimpleStaticCache<TIsolation, TKey, TValue>.semaphoreKey.Wait();
				if (this.cache.ContainsKey(key) == false) {
					if (value != null) {
						this.cache.Add(key, value);
						return value;
					} else {
						switch (this.handleNullValue) {
							case RpcSimpleCacheNullValues.AllowNullValues:
								this.cache.Add(key, value);
								return value;
							case RpcSimpleCacheNullValues.DenyNullValuesAndReturnDefault:
								return default;
							case RpcSimpleCacheNullValues.DenyNullValuesAndThrow:
								throw new Exception($"The 'provideValue' function delegate returned a null value for the key '{key}'.");
						}
					}
				}
			} finally {
				RpcSimpleStaticCache<TIsolation, TKey, TValue>.semaphoreKey.Release();
			}
		}

		// Return the value from the cache.
		return this.cache[key];
	} // GetValue

} // RpcSimpleStaticCache
#endregion
