namespace RpcScandinavia.Core.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

#region RpcDictionaryList
//----------------------------------------------------------------------------------------------------------------------
// RpcDictionaryList.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// This class is a dictionary where the value is a list.
/// </summary>
/// <typeparam name="TKey">The key type.</typeparam>
/// <typeparam name="TValue">The value type.</typeparam>
public class RpcDictionaryList<TKey, TValue> {
	private readonly IDictionary<TKey, List<TValue>> items;

	#region Constructors
	//------------------------------------------------------------------------------------------------------------------
	// Constructors.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Initializes a new instance of the dictionary list, that is empty, has the default initial capacity, and uses the
	/// default equality comparer for the key type.
	/// </summary>
	public RpcDictionaryList() {
		this.items = new Dictionary<TKey, List<TValue>>();
	} // RpcDictionaryList

	/// <summary>
	/// Initializes a new instance of the dictionary list, that contains elements copied from the specified dictionary
	/// list and uses the default equality comparer for the key type.
	/// </summary>
	/// <param name="items">The collection whose elements are copied to the new dictionary list.</param>
	public RpcDictionaryList(IDictionary<TKey, List<TValue>> items) {
		this.items = new Dictionary<TKey, List<TValue>>(items);
	} // RpcDictionaryList

	/// <summary>
	/// Initializes a new instance of the dictionary list, that contains elements copied from the specified dictionary
	/// list and uses the specified equality comparer for the key type.
	/// </summary>
	/// <param name="items">The collection whose elements are copied to the new dictionary list.</param>
	/// <param name="comparer">The equality comparer for the key type.</param>
	public RpcDictionaryList(IDictionary<TKey, List<TValue>> items, IEqualityComparer<TKey> comparer) {
		this.items = new Dictionary<TKey, List<TValue>>(items, comparer);
	} // RpcDictionaryList

	/// <summary>
	/// Initializes a new instance of the dictionary list, that contains elements copied from the specified enumerable.
	/// </summary>
	/// <param name="items">The collection whose elements are copied to the new dictionary list.</param>
	public RpcDictionaryList(IEnumerable<KeyValuePair<TKey, List<TValue>>> items) {
		this.items = new Dictionary<TKey, List<TValue>>(items);
	} // RpcDictionaryList

	/// <summary>
	/// Initializes a new instance of the dictionary list, that contains elements copied from the specified enumerable
	/// and uses the specified equality comparer for the key type.
	/// </summary>
	/// <param name="items">The collection whose elements are copied to the new dictionary list.</param>
	/// <param name="comparer">The equality comparer for the key type.</param>
	public RpcDictionaryList(IEnumerable<KeyValuePair<TKey, List<TValue>>> items, IEqualityComparer<TKey> comparer) {
		this.items = new Dictionary<TKey, List<TValue>>(items, comparer);
	} // RpcDictionaryList

	/// <summary>
	/// Initializes a new instance of the dictionary list, that is empty, has the default initial capacity, and uses the
	/// specified equality comparer for the key type.
	/// </summary>
	/// <param name="comparer">The equality comparer for the key type.</param>
	public RpcDictionaryList(IEqualityComparer<TKey> comparer) {
		this.items = new Dictionary<TKey, List<TValue>>(comparer);
	} // RpcDictionaryList

	/// <summary>
	/// Initializes a new instance of the dictionary list, that is empty, has the specified initial capacity, and uses
	/// the default equality comparer for the key type.
	/// </summary>
	/// <param name="capacity">The initial number of elements that the dictionary list can contain.</param>
	public RpcDictionaryList(Int32 capacity) {
		this.items = new Dictionary<TKey, List<TValue>>(capacity);
	} // RpcDictionaryList

	/// <summary>
	/// Initializes a new instance of the dictionary list, that is empty, has the specified initial capacity, and uses
	/// the specified equality comparer for the key type.
	/// </summary>
	/// <param name="capacity">The initial number of elements that the dictionary list can contain.</param>
	/// <param name="comparer">The equality comparer for the key type.</param>
	public RpcDictionaryList(Int32 capacity, IEqualityComparer<TKey> comparer) {
		this.items = new Dictionary<TKey, List<TValue>>(capacity, comparer);
	} // RpcDictionaryList

	///// <summary>
	///// Initializes a new instance of the dictionary list, with serialized data.
	///// </summary>
	//public RpcDictionaryList(SerializationInfo, StreamingContext) {
	//	this.items = new Dictionary<TKey, List<TValue>>();
	//} // RpcDictionaryList
	#endregion

	#region Properties
	//------------------------------------------------------------------------------------------------------------------
	// Properties.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the number of elements contained in the dictionary list.
	/// </summary>
	public Int32 Count {
		get {
			return this.items.Count;
		}
	} // Count

	/// <summary>
	/// Gets a value indicating whether the dictionary list is read-only.
	/// </summary>
	public Boolean IsReadOnly {
		get {
			return this.items.IsReadOnly;
		}
	} // IsReadOnly

	/// <summary>
	/// Gets a collection containing the keys of the dictionary list.
	/// </summary>
	public ICollection<TKey> Keys {
		get {
			return this.items.Keys;
		}
	} // Keys

	/// <summary>
	/// Gets or sets the list element with the specified key.
	///
	/// Note: This is the actual list, so use with caution!
	/// </summary>
	/// <param name="key">The key.</param>
	/// <value>The list element with the specified key.</value>
	public List<TValue> this[TKey key] {
		get {
			return this.items[key];
		}
		set {
			if (value != null) {
				this.Clear(key);
				this.Add(key, value);
			} else {
				this.Remove(key);
			}
		}
	} // this
	#endregion

	#region Methods
	//------------------------------------------------------------------------------------------------------------------
	// Methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Removes all items from the dictionary list.
	/// </summary>
	public void Clear() {
		this.items.Clear();
	} // Clear

	/// <summary>
	/// Removes all values associated with teh key in the dictionary list.
	/// </summary>
	/// <param name="key">The key.</param>
	public void Clear(TKey key) {
		if (this.items.ContainsKey(key) == true) {
			this.items.Clear();
		}
	} // Clear

	/// <summary>
	/// Determines whether the dictionary list contains an list element with the specified key.
	/// </summary>
	/// <param name="key">The key.</param>
	/// <returns>Returns true if the dictionary list contains an list element with the key; otherwise, false.</returns>
	public Boolean ContainsKey(TKey key) {
		return this.items.ContainsKey(key);
	} // ContainsKey

	/// <summary>
	/// Adds the values associated with teh key in the dictionary list.
	/// </summary>
	/// <param name="key">The key.</param>
	/// <param name="values">The values.</param>
	public void Add(TKey key, params TValue[] values) {
		// Add the key and list.
		if (this.items.ContainsKey(key) == false) {
			this.items.Add(key, new List<TValue>());
		}

		// Add the values to the list.
		this.items[key].AddRange(values);
	} // Add

	/// <summary>
	/// Adds the values associated with teh key in the dictionary list.
	/// </summary>
	/// <param name="key">The key.</param>
	/// <param name="values">The values.</param>
	public void Add(TKey key, IEnumerable<TValue> values) {
		// Add the key and list.
		if (this.items.ContainsKey(key) == false) {
			this.items.Add(key, new List<TValue>());
		}

		// Add the values to the list.
		this.items[key].AddRange(values);
	} // Add

	/// <summary>
	/// Removes the key and all associated values from the dictionary list.
	/// </summary>
	/// <param name="key">The key.</param>
	public void Remove(TKey key) {
		// Remove the key and list.
		if (this.items.ContainsKey(key) == true) {
			this.items.Remove(key);
		}
	} // Remove
	#endregion

} // RpcDictionaryList
#endregion
