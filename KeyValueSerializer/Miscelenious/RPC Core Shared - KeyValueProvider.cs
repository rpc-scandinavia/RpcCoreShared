namespace RpcScandinavia.Core.Shared.KeyValueSerializer;
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

	public Int32 GetCount(String keyPrefix) {
		// Get a list of keys:
		// 1) Get a string list of keys from the values (key/value pair).
		// 2) Get the keys that belong to the key prefix, and below.
		// 3) Trim the key prefix from the keys.
		// 4) Get the keys only. Each string key is split by the hierarchy separator character, so the first item is the collection key.
		// 5) Exclude all keys that starts with "$".
		// 6) Remove doublets.
		keyPrefix = $"{keyPrefix}{options.HierarchySeparatorChar}".TrimStart(options.HierarchySeparatorChar);
		return values
			.Select((value) => getKey(value))
			.Where((key) => key.StartsWith(keyPrefix))
			.Select((key) => key.Substring(keyPrefix.Length))
			.Select((key) => key.Split(options.HierarchySeparatorChar)[0])
			.Where((key) => (key.StartsWith("$") == false))
			.Distinct()
			.Count();
	} // GetCount

	public Int32 GetCount(String keyPrefix, String key) {
		// Get a list of keys:
		// 1) Get a string list of keys from the values (key/value pair).
		// 2) Get the keys that belong to the key prefix, and below.
		// 3) Trim the key prefix from the keys.
		// 4) Get the keys only. Each string key is split by the hierarchy separator character, so the first item is the collection key.
		// 5) Exclude all keys that starts with "$".
		// 6) Remove doublets.
		keyPrefix = $"{keyPrefix}{options.HierarchySeparatorChar}{key}{options.HierarchySeparatorChar}".TrimStart(options.HierarchySeparatorChar);
		return values
			.Select((value) => getKey(value))
			.Where((key) => key.StartsWith(keyPrefix))
			.Select((key) => key.Substring(keyPrefix.Length))
			.Select((key) => key.Split(options.HierarchySeparatorChar)[0])
			.Where((key) => (key.StartsWith("$") == false))
			.Distinct()
			.Count();
	} // GetCount

	public IEnumerable<String> GetKeys(String keyPrefix) {
		// Get a list of keys:
		// 1) Get a string list of keys from the values (key/value pair).
		// 2) Get the keys that belong to the key prefix, and below.
		// 3) Trim the key prefix from the keys.
		// 4) Get the keys only. Each string key is split by the hierarchy separator character, so the first item is the collection key.
		// 5) Exclude all keys that starts with "$".
		// 6) Remove doublets.
		keyPrefix = $"{keyPrefix}{options.HierarchySeparatorChar}".TrimStart(options.HierarchySeparatorChar);
		return values
			.Select((value) => getKey(value))
			.Where((key) => key.StartsWith(keyPrefix))
			.Select((key) => key.Substring(keyPrefix.Length))
			.Select((key) => key.Split(options.HierarchySeparatorChar)[0])
			.Where((key) => (key.StartsWith("$") == false))
			.Distinct();
	} // GetKeys

	public IEnumerable<Int32> GetKeysAsInt32(String keyPrefix) {
		// Get a list of keys:
		// 1) Get a string list of keys from the values (key/value pair).
		// 2) Get the keys that belong to the key prefix, and below.
		// 3) Trim the key prefix from the keys.
		// 4) Get the keys only. Each string key is split by the hierarchy separator character, so the first item is the collection key.
		// 5) Exclude all keys that starts with "$".
		// 6) Remove doublets.
		// 7) Convert each key to Int32.
		keyPrefix = $"{keyPrefix}{options.HierarchySeparatorChar}".TrimStart(options.HierarchySeparatorChar);
		return values
			.Select((value) => getKey(value))
			.Where((key) => key.StartsWith(keyPrefix))
			.Select((key) => key.Substring(keyPrefix.Length))
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

	/// <summary>
	/// Gets all the key/value items where the key starts with the specified key prefix.
	/// </summary>
	/// <param name="keyPrefix">The prefix to the serialized keys.</param>
	/// <returns>The matching key/value items.</returns>
	public IEnumerable<KeyValueType> GetValues(String keyPrefix) {
		// Validate.
		if (keyPrefix == null) {
			throw new NullReferenceException(nameof(keyPrefix));
		}

		// Return the matching key/value items.
		return this.values
			.Where((keyValue) => getKey(keyValue).StartsWith(keyPrefix));
	} // GetValues

/*
	/// <summary>
	/// Gets all the key/value items where the key starts with the specified key prefix.
	/// </summary>
	/// <param name="keyPrefix">The prefix to the serialized keys.</param>
	/// <param name="key">The key.</param>
	/// <returns>The matching key/value items.</returns>
	public IEnumerable<KeyValueType> GetValues(String keyPrefix, String key) {
		// Validate.
		if (keyPrefix == null) {
			throw new NullReferenceException(nameof(keyPrefix));
		}
		if (key == null) {
			throw new NullReferenceException(nameof(key));
		}

		// Return the matching key/value items.
		key = $"{keyPrefix}{this.options.HierarchySeparatorChar}{key}".Trim(options.HierarchySeparatorChar);
		return this.values
			.Where((keyValue) => getKey(keyValue).StartsWith(key));
	} // GetValues
*/

	/// <summary>
	/// Gets the value from the key/value item where the key matches the specified key.
	/// </summary>
	/// <param name="key">The key.</param>
	/// <returns>The value, or null.</returns>
	public String GetValue(String keyPrefix, String key) {
		// Validate.
		if (keyPrefix == null) {
			throw new NullReferenceException(nameof(keyPrefix));
		}
		if (key == null) {
			throw new NullReferenceException(nameof(key));
		}

		// Return the matching value or null.
		key = $"{keyPrefix}{this.options.HierarchySeparatorChar}{key}".Trim(options.HierarchySeparatorChar);
		return this.values
			.Where((keyValue) => getKey(keyValue).Equals(key))
			.Select((keyValue) => getValue(keyValue))
			.FirstOrDefault();
	} // GetValue

	public String GetTypeMetadata(String keyPrefix, String key) {
		// Validate.
		if (keyPrefix == null) {
			throw new NullReferenceException(nameof(keyPrefix));
		}
		if (key == null) {
			throw new NullReferenceException(nameof(key));
		}

		// Return the matching value or null.
		key = $"{keyPrefix}{this.options.HierarchySeparatorChar}{key}{this.options.HierarchySeparatorChar}$Type".Trim(options.HierarchySeparatorChar);
		return this.values
			.Where((keyValue) => getKey(keyValue).Equals(key))
			.Select((keyValue) => getValue(keyValue))
			.FirstOrDefault();
	} // GetTypeMetadata

	// TODO: Perhaps not in both "RpcKeyValueBuilder" and "RpcKeyValueProvider".
	public String GetNextLevelKeyPrefix(String keyPrefix, String key) {
		// Validate.
		if (keyPrefix == null) {
			throw new NullReferenceException(nameof(keyPrefix));
		}
		if (key == null) {
			throw new NullReferenceException(nameof(key));
		}

		// Return.
		return $"{keyPrefix}{this.options.HierarchySeparatorChar}{key}".Trim(options.HierarchySeparatorChar);
	} // GetNextLevelKeyPrefix

} // RpcKeyValueProvider
#endregion
