namespace RpcScandinavia.Core.Shared.KeyValueSerializer;
using System;
using System.Collections.Generic;
using System.Linq;
using RpcScandinavia.Core;

#region RpcKeyValueBuilder
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueBuilder.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// The key/value builder collects keys and values during serialization.
/// </summary>
/// <typeparam name="KeyValueType">The type of the key/value items.</typeparam>
public class RpcKeyValueBuilder<KeyValueType> {
	private readonly List<KeyValueType> values;
	private readonly Func<String, String, KeyValueType> createKeyValueInstance;
	private readonly RpcKeyValueSerializerOptions options;

	/// <summary>
	/// Create a new key/value builder.
	/// </summary>
	/// <param name="createKeyValueInstance">A function that initializes a new KeyValueType with the key (first argument) and value (second argument).</param>
	/// <param name="options">The serialization options.</param>
	public RpcKeyValueBuilder(Func<String, String, KeyValueType> createKeyValueInstance, RpcKeyValueSerializerOptions options) {
		// Validate.
		if (createKeyValueInstance == null) {
			throw new NullReferenceException(nameof(createKeyValueInstance));
		}
		if (options == null) {
			throw new NullReferenceException(nameof(options));
		}

		// Initialize.
		this.values = new List<KeyValueType>();
		this.createKeyValueInstance = createKeyValueInstance;
		this.options = options;
	} // RpcKeyValueBuilder

	/// <summary>
	/// The collection of key/value items.
	/// </summary>
	public List<KeyValueType> Values {
		get {
			return this.values;
		}
	} // Values

	/// <summary>
	/// Add a new key/value item to the collection.
	/// </summary>
	/// <param name="key">The key.</param>
	/// <param name="value">The string.</param>
	public void Add(String keyPrefix, String key, String value) {
		// Validate.
		if (keyPrefix == null) {
			throw new NullReferenceException(nameof(keyPrefix));
		}
		if (key == null) {
			throw new NullReferenceException(nameof(key));
		}

		// Add.
		this.values.Add(
			this.createKeyValueInstance(
				$"{keyPrefix}{this.options.HierarchySeparatorChar}{key}".Trim(options.HierarchySeparatorChar),
				value
			)
		);
	} // Add

	public void AddTypeMetadata(String keyPrefix, String key, Type type) {
		// Validate.
		if (keyPrefix == null) {
			throw new NullReferenceException(nameof(keyPrefix));
		}
		if (key == null) {
			throw new NullReferenceException(nameof(key));
		}
		if (type == null) {
			throw new NullReferenceException(nameof(type));
		}

		// Add.
		this.values.Add(
			this.createKeyValueInstance(
				$"{keyPrefix}{this.options.HierarchySeparatorChar}{key}{this.options.HierarchySeparatorChar}$Type".Trim(options.HierarchySeparatorChar),
				type.AssemblyQualifiedName
			)
		);
	} // AddTypeMetadata

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

} // RpcKeyValueBuilder
#endregion
