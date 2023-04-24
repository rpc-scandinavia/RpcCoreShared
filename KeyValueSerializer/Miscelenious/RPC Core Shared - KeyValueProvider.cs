namespace RpcScandinavia.Core.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using RpcScandinavia.Core;

#region RpcKeyValueProvider
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueProvider.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// The key/value provider extracts keys and values from the key/value collection, during deserialization.
/// </summary>
/// <typeparam name="KeyValueType">The type of the key/value items.</typeparam>
public class RpcKeyValueProvider<KeyValueType> {
	private readonly IEnumerable<KeyValueType> values;
	private readonly Func<KeyValueType, String> getKey;
	private readonly Func<KeyValueType, String> getValue;
	private readonly RpcKeyValueSerializerOptions options;

	/// <summary>
	/// Create a new key/value provider.
	/// </summary>
	/// <param name="values">The KeyValueType collection.</param>
	/// <param name="getKey">A function that gets the key from the KeyValueType.</param>
	/// <param name="getValue">A function that gets the value from the KeyValueType.</param>
	/// <param name="options">The serialization options.</param>
	public RpcKeyValueProvider(IEnumerable<KeyValueType> values, Func<KeyValueType, String> getKey, Func<KeyValueType, String> getValue, RpcKeyValueSerializerOptions options) {
		// Validate.
		if (values == null) {
			throw new NullReferenceException(nameof(values));
		}
		if (getKey == null) {
			throw new NullReferenceException(nameof(getKey));
		}
		if (getValue == null) {
			throw new NullReferenceException(nameof(getValue));
		}
		if (options == null) {
			throw new NullReferenceException(nameof(options));
		}

		// Initialize.
		this.values = values;
		this.getKey = getKey;
		this.getValue = getValue;
		this.options = options;
	} // RpcKeyValueProvider

	/// <summary>
	/// Gets the number of values under the key.
	/// </summary>
	/// <param name="keyPath">The path to the key.</param>
	/// <param name="key">The key.</param>
	/// <returns>The number of values.</returns>
	public Int32 GetCount(String keyPath, String key) {
		// Get a list of keys:
		// 1) Get a string list of keys from the values (key/value pair).
		// 2) Get the keys that belong to the path, and below.
		// 3) Trim the path from the keys.
		// 4) Get the keys only. Each string key is split by the hierarchy separator character, so the first item is the collection key.
		// 5) Exclude all keys that starts with "$".
		// 6) Remove doublets.
		keyPath = $"{keyPath}{options.HierarchySeparatorChar}{key}{options.HierarchySeparatorChar}".TrimStart(options.HierarchySeparatorChar);
		return values
			.Select((value) => getKey(value))
			.Where((key) => key.StartsWith(keyPath))
			.Select((key) => key.Substring(keyPath.Length))
			.Select((key) => key.Split(options.HierarchySeparatorChar)[0])
			.Where((key) => (key.StartsWith("$") == false))
			.Distinct()
			.Count();
	} // GetCount

	/// <summary>
	/// Gets the keys at the path.
	/// </summary>
	/// <param name="keyPath">The path to the key.</param>
	/// <returns>The enumerator with the matching keys.</returns>
	public IEnumerable<String> GetKeys(String keyPath) {
		// Get a list of keys:
		// 1) Get a string list of keys from the values (key/value pair).
		// 2) Get the keys that belong to the path, and below.
		// 3) Trim the path from the keys.
		// 4) Get the keys only. Each string key is split by the hierarchy separator character, so the first item is the collection key.
		// 5) Exclude all keys that starts with "$".
		// 6) Remove doublets.
		keyPath = $"{keyPath}{options.HierarchySeparatorChar}".TrimStart(options.HierarchySeparatorChar);
		return values
			.Select((value) => getKey(value))
			.Where((key) => key.StartsWith(keyPath))
			.Select((key) => key.Substring(keyPath.Length))
			.Select((key) => key.Split(options.HierarchySeparatorChar)[0])
			.Where((key) => (key.StartsWith("$") == false))
			.Distinct();
	} // GetKeys

	/// <summary>
	/// Gets the keys at the path.
	/// The keys are converted to <see cref="System.Int32" />, and non numeric keys are ignored.
	/// </summary>
	/// <param name="keyPath">The path to the key.</param>
	/// <returns>The enumerator with the matching keys.</returns>
	public IEnumerable<Int32> GetKeysAsInt32(String keyPath) {
		// Get a list of keys:
		// 1) Get a string list of keys from the values (key/value pair).
		// 2) Get the keys that belong to the path, and below.
		// 3) Trim the path from the keys.
		// 4) Get the keys only. Each string key is split by the hierarchy separator character, so the first item is the collection key.
		// 5) Exclude all keys that starts with "$".
		// 6) Remove doublets.
		// 7) Convert each key to Int32.
		keyPath = $"{keyPath}{options.HierarchySeparatorChar}".TrimStart(options.HierarchySeparatorChar);
		return values
			.Select((value) => getKey(value))
			.Where((key) => key.StartsWith(keyPath))
			.Select((key) => key.Substring(keyPath.Length))
			.Select((key) => key.Split(options.HierarchySeparatorChar)[0])
			.Where((key) => (key.StartsWith("$") == false))
			.Distinct()
			.Select((key) => {
				try {
					return Int32.Parse(key);
				} catch {
					return -1;
				}
			})
			.Where((key) => (key > -1));
	} // GetKeysAsInt32

/*
	/// <summary>
	/// Gets the key/value items under the path and all remaining levels.
	/// </summary>
	/// <param name="keyPath">The path to the key.</param>
	/// <returns>The enumerator with the matching key/value items.</returns>
	public IEnumerable<KeyValueType> GetValues(String keyPath) {
		// Validate.
		if (keyPath == null) {
			throw new NullReferenceException(nameof(keyPath));
		}

		// Return the matching key/value items.
		return this.values
			.Where((keyValue) => getKey(keyValue).StartsWith(keyPath));
	} // GetValues
*/

	/// <summary>
	/// Gets the value from the key.
	/// </summary>
	/// <param name="keyPath">The path to the key.</param>
	/// <param name="key">The key.</param>
	/// <returns>The value or null.</returns>
	public String GetValue(String keyPath, String key) {
		// Validate.
		if (keyPath == null) {
			throw new NullReferenceException(nameof(keyPath));
		}
		if (key == null) {
			throw new NullReferenceException(nameof(key));
		}

		// Return the matching value or null.
		key = $"{keyPath}{this.options.HierarchySeparatorChar}{key}".Trim(options.HierarchySeparatorChar);
		return this.values
			.Where((keyValue) => getKey(keyValue).Equals(key))
			.Select((keyValue) => getValue(keyValue))
			.FirstOrDefault();
	} // GetValue

	/// <summary>
	/// Gets the type meta data under the key.
	/// </summary>
	/// <param name="keyPath">The path to the key.</param>
	/// <param name="key">The key.</param>
	/// <returns>The type meta data or null.</returns>
	public String GetTypeMetadata(String keyPath, String key) {
		// Validate.
		if (keyPath == null) {
			throw new NullReferenceException(nameof(keyPath));
		}
		if (key == null) {
			throw new NullReferenceException(nameof(key));
		}

		// Return the matching value or null.
		key = $"{keyPath}{this.options.HierarchySeparatorChar}{key}{this.options.HierarchySeparatorChar}$Type".Trim(options.HierarchySeparatorChar);
		return this.values
			.Where((keyValue) => getKey(keyValue).Equals(key))
			.Select((keyValue) => getValue(keyValue))
			.FirstOrDefault();
	} // GetTypeMetadata

	// TODO: Perhaps not in both "RpcKeyValueBuilder" and "RpcKeyValueProvider".
	/// <summary>
	/// Gets the path for the next level.
	/// </summary>
	/// <param name="keyPath">The path to the key.</param>
	/// <param name="key">The key.</param>
	/// <returns>The path.</returns>
	public String GetNextLevelKeyPrefix(String keyPath, String key) {
		// Validate.
		if (keyPath == null) {
			throw new NullReferenceException(nameof(keyPath));
		}
		if (key == null) {
			throw new NullReferenceException(nameof(key));
		}

		// Return.
		return $"{keyPath}{this.options.HierarchySeparatorChar}{key}".Trim(options.HierarchySeparatorChar);
	} // GetNextLevelKeyPrefix

} // RpcKeyValueProvider
#endregion
