namespace RpcScandinavia.Core.Shared;
using System;
using System.Collections.Generic;

#region RpcKeyValueBuilder
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueBuilder.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// The key/value builder collects keys and values during serialization.
/// </summary>
/// <typeparam name="KeyValueType">The type of the key/value items.</typeparam>
public class RpcKeyValueBuilder<KeyValueType> : IRpcKeyValueBuilder {
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

	public Int32 Level {
		get {
			return 0;
		}
	} // Level

	public RpcKeyValueSerializerOptions Options {
		get {
			return this.options;
		}
	} // Options

	public IRpcKeyValueBuilder AddLevel(String key) {
		return new RpcKeyValueBuilderLevel(this, key, 1);
	} // AddLevel

	public void Add(String key, String value) {
		// Validate.
		if (key == null) {
			throw new NullReferenceException(nameof(key));
		}

		// Add.
		this.values.Add(this.createKeyValueInstance(key, value));
	} // Add

	public void AddTypeMetadata(String key, Type type) {
		// Validate.
		if (key == null) {
			throw new NullReferenceException(nameof(key));
		}
		if (type == null) {
			throw new NullReferenceException(nameof(type));
		}

		// Add.
		if (key.IsNullOrWhiteSpace() == false) {
			this.values.Add(this.createKeyValueInstance($"{key}{this.options.HierarchySeparatorChar}$Type", type.AssemblyQualifiedName));
		} else {
			this.values.Add(this.createKeyValueInstance($"$Type", type.AssemblyQualifiedName));
		}
	} // AddTypeMetadata

} // RpcKeyValueBuilder
#endregion

#region RpcKeyValueBuilderLevel
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueBuilderLevel.
//----------------------------------------------------------------------------------------------------------------------
public class RpcKeyValueBuilderLevel : IRpcKeyValueBuilder {
	private IRpcKeyValueBuilder builder;
	private String path;
	private Int32 level;

	public RpcKeyValueBuilderLevel(IRpcKeyValueBuilder builder, String path, Int32 level) {
		this.builder = builder;
		if (path.IsNullOrWhiteSpace() == true) {
			this.path = String.Empty;
			this.level = level;
		} else {
			this.path = $"{path}{builder.Options.HierarchySeparatorChar}";
			this.level = level;
		}
	} // RpcKeyValueBuilderLevel

	public IRpcKeyValueBuilder AddLevel(String key) {
		return new RpcKeyValueBuilderLevel(this.builder, $"{this.path}{key}", level + 1);
	} // AddLevel

	public void Add(String key, String value) {
		// Validate.
		if (key == null) {
			throw new NullReferenceException(nameof(key));
		}

		// Add.
		this.builder.Add($"{this.path}{key}", value);
	} // Add

	public void AddTypeMetadata(String key, Type type) {
		// Validate.
		if (key == null) {
			throw new NullReferenceException(nameof(key));
		}
		if (type == null) {
			throw new NullReferenceException(nameof(type));
		}

		// Add.
		if (key.IsNullOrWhiteSpace() == false) {
			this.builder.Add($"{this.path}{key}{this.builder.Options.HierarchySeparatorChar}$Type", type.AssemblyQualifiedName);
		} else {
			this.builder.Add($"{this.path}$Type", type.AssemblyQualifiedName);
		}
	} // AddTypeMetadata

	public Int32 Level {
		get {
			return this.level;
		}
	} // Level

	public RpcKeyValueSerializerOptions Options {
		get {
			return this.builder.Options;
		}
	} // Options

} // RpcKeyValueBuilderLevel
#endregion
