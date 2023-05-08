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
	private readonly Func<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>, KeyValueType> createKeyValueInstance;
	private readonly RpcKeyValueSerializerOptions options;

	/// <summary>
	/// Create a new key/value builder.
	/// </summary>
	/// <param name="createKeyValueInstance">A function that initializes a new KeyValueType with the key (first argument) and value (second argument).</param>
	/// <param name="options">The serialization options.</param>
	public RpcKeyValueBuilder(Func<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>, KeyValueType> createKeyValueInstance, RpcKeyValueSerializerOptions options) {
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

	public IRpcKeyValueBuilder AddLevel(ReadOnlyMemory<Char> key) {
		return new RpcKeyValueBuilderLevel(this, key);
	} // AddLevel

	public void Add(ReadOnlyMemory<Char> key, ReadOnlyMemory<Char> value) {
		// Validate.
		if (key.Length == 0) {
			throw new ArgumentException(nameof(key));
		}

		// Add.
		this.values.Add(this.createKeyValueInstance(key, value));
	} // Add

	public void AddTypeMetadata(ReadOnlyMemory<Char> key, Type type) {
		// Validate.
		if (type == null) {
			throw new NullReferenceException(nameof(type));
		}

		// Add.
		if (key.Length > 0) {
			this.values.Add(this.createKeyValueInstance($"{key}{this.options.HierarchySeparatorChar}$Type".AsMemory(), type.AssemblyQualifiedName.AsMemory()));
		} else {
			this.values.Add(this.createKeyValueInstance($"$Type".AsMemory(), type.AssemblyQualifiedName.AsMemory()));
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
	private ReadOnlyMemory<Char> path;

	public RpcKeyValueBuilderLevel(IRpcKeyValueBuilder builder, ReadOnlyMemory<Char> path) {
		this.builder = builder;
		if (path.Length == 0) {
			this.path = ReadOnlyMemory<Char>.Empty;
		} else {
			this.path = $"{path}{builder.Options.HierarchySeparatorChar}".AsMemory();
		}
	} // RpcKeyValueBuilderLevel

	public IRpcKeyValueBuilder AddLevel(ReadOnlyMemory<Char> key) {
		return new RpcKeyValueBuilderLevel(this.builder, $"{this.path}{key}".AsMemory());
	} // AddLevel

	public void Add(ReadOnlyMemory<Char> key, ReadOnlyMemory<Char> value) {
		// Validate.
		if (key.Length == 0) {
			throw new ArgumentException(nameof(key));
		}

		// Add.
		this.builder.Add($"{this.path}{key}".AsMemory(), value);
	} // Add

	public void AddTypeMetadata(ReadOnlyMemory<Char> key, Type type) {
		// Validate.
		if (type == null) {
			throw new NullReferenceException(nameof(type));
		}

		// Add.
		if (key.Length > 0) {
			this.builder.Add($"{this.path}{key}{this.builder.Options.HierarchySeparatorChar}$Type".AsMemory(), type.AssemblyQualifiedName.AsMemory());
		} else {
			this.builder.Add($"{this.path}$Type".AsMemory(), type.AssemblyQualifiedName.AsMemory());
		}
	} // AddTypeMetadata

	public RpcKeyValueSerializerOptions Options {
		get {
			return this.builder.Options;
		}
	} // Options

} // RpcKeyValueBuilderLevel
#endregion
