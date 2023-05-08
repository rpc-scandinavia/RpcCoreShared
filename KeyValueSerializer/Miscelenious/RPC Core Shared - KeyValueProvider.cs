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
public class RpcKeyValueProvider<KeyValueType> : IRpcKeyValueProvider {
	private readonly RpcKeyValueProviderItem items;
	private readonly IEnumerable<KeyValueType> values;
	private readonly Func<KeyValueType, ReadOnlyMemory<Char>> getKey;
	private readonly Func<KeyValueType, ReadOnlyMemory<Char>> getValue;
	private readonly RpcKeyValueSerializerOptions options;

	/// <summary>
	/// Create a new key/value provider.
	/// </summary>
	/// <param name="values">The KeyValueType collection.</param>
	/// <param name="getKey">A function that gets the key from the KeyValueType.</param>
	/// <param name="getValue">A function that gets the value from the KeyValueType.</param>
	/// <param name="options">The serialization options.</param>
	public RpcKeyValueProvider(IEnumerable<KeyValueType> values, Func<KeyValueType, ReadOnlyMemory<Char>> getKey, Func<KeyValueType, ReadOnlyMemory<Char>> getValue, RpcKeyValueSerializerOptions options) {
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
		this.items = new RpcKeyValueProviderItem();
		this.values = values;
		this.getKey = getKey;
		this.getValue = getValue;
		this.options = options;

		this.Parse();
	} // RpcKeyValueProvider

	private void Parse() {
		// Iterate through each value (key/value pair).
		foreach (KeyValueType keyValue in values) {
			// Get the key and the value.
			ReadOnlyMemory<Char> key = this.getKey(keyValue);
			ReadOnlyMemory<Char> value = this.getValue(keyValue);

			// Get the root item.
			RpcKeyValueProviderItem item = this.items;

			// Iterate through each item in the key, and add the path.
			Int32 index = key.Span.IndexOf(this.options.HierarchySeparatorChar);
			while (index > -1) {
				// Add the current key in the path.
				item = item.Add(key.Slice(0, index));

				// Remove the current key from the path.
				key = key.Slice(index + 1);

				// Iterate.
				index = key.Span.IndexOf(this.options.HierarchySeparatorChar);
			}

			// Add the last key in the path.
			item = item.Add(key);

			// Add the value.
			item = item.Add(value);
		}
	} // Parse

	//------------------------------------------------------------------------------------------------------------------
	// IRpcKeyValueProvider
	//------------------------------------------------------------------------------------------------------------------
	public ReadOnlyMemory<Char> GetTypeMetadata() {
		return this.items.GetTypeMetadata();
	} // GetTypeMetadata

	public ReadOnlyMemory<Char> GetValue() {
		return this.items.GetValue();
	} // GetValue

	public Int32 Count() {
		return this.items.Count();
	} // Count

	public Boolean ContainsKey(ReadOnlyMemory<Char> key) {
		return this.items.ContainsKey(key);
	} // ContainsKey

	public IRpcKeyValueProvider GetKey(ReadOnlyMemory<Char> key) {
		return this.items.GetKey(key);
	} // GetKey

	public IRpcKeyValueProvider[] GetKeys() {
		return this.items.GetKeys();
	} // GetKeysAsInt32

	public ReadOnlyMemory<Char> GetFirstKeyValueOrEmpty() {
		return this.items.GetFirstKeyValueOrEmpty();
	} // GetFirstKeyValueOrEmpty

} // RpcKeyValueProvider
#endregion

#region RpcKeyValueProviderItem
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueProviderItem.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// The keys and values are parsed into a tree structure, using this struct.
/// The value can either be part of a path or the value at the end of the path.
/// </summary>
public struct RpcKeyValueProviderItem : IRpcKeyValueProvider {
	public static ReadOnlyMemory<Char> TypeMetadataKey = "$Type".AsMemory();

	public readonly ReadOnlyMemory<Char> value;
	public readonly List<RpcKeyValueProviderItem> items;

	public RpcKeyValueProviderItem() {
		this.value = ReadOnlyMemory<Char>.Empty;
		this.items = new List<RpcKeyValueProviderItem>();
	} // RpcKeyValueProviderItem

	public RpcKeyValueProviderItem(ReadOnlyMemory<Char> value) {
		this.value = value;
		this.items = new List<RpcKeyValueProviderItem>();
	} // RpcKeyValueProviderItem

	public RpcKeyValueProviderItem Add(ReadOnlyMemory<Char> value) {
		foreach (RpcKeyValueProviderItem item in this.items) {
			if (item.value.Span.SequenceEqual(value.Span) == true) {
				return item;
			}
		}

		// Add.
		RpcKeyValueProviderItem newItem = new RpcKeyValueProviderItem(value);
		this.items.Add(newItem);
		return newItem;
	} // Add

	public ReadOnlyMemory<Char> GetTypeMetadata() {
		foreach (RpcKeyValueProviderItem item in this.items) {
			if (item.value.Span.SequenceEqual(RpcKeyValueProviderItem.TypeMetadataKey.Span) == true) {
				return item.GetFirstKeyValueOrEmpty();
			}
		}

		// Return empty.
		return ReadOnlyMemory<Char>.Empty;
	} // GetTypeMetadata

	public ReadOnlyMemory<Char> GetValue() {
		return this.value;
	} // GetValue

	public Int32 Count() {
		return this.items.Count;
	} // Count

	public Boolean ContainsKey(ReadOnlyMemory<Char> key) {
		foreach (RpcKeyValueProviderItem item in this.items) {
			if (item.value.Span.SequenceEqual(key.Span) == true) {
				return true;
			}
		}

		// Return empty.
		return false;
	} // ContainsKey

	public IRpcKeyValueProvider GetKey(ReadOnlyMemory<Char> key) {
		foreach (RpcKeyValueProviderItem item in this.items) {
			if (item.value.Span.SequenceEqual(key.Span) == true) {
				return item;
			}
		}

		// Return empty.
		return new RpcKeyValueProviderItem();
	} // GetKey

	public IRpcKeyValueProvider[] GetKeys() {
		return this.items
			.Where((item) => (item.value.Span.SequenceEqual(RpcKeyValueProviderItem.TypeMetadataKey.Span) == false))
			.ToList()
			.ConvertAll<IRpcKeyValueProvider>((item) => (IRpcKeyValueProvider)item)
			.ToArray();
	} // GetKeysAsInt32

	public ReadOnlyMemory<Char> GetFirstKeyValueOrEmpty() {
		foreach (RpcKeyValueProviderItem item in this.items) {
			if (item.value.Span.SequenceEqual(RpcKeyValueProviderItem.TypeMetadataKey.Span) == false) {
				return item.value;
			}
		}

		// Return empty.
		return ReadOnlyMemory<Char>.Empty;
	} // GetFirstKeyValueOrEmpty

} // RpcKeyValueProviderItem
#endregion
