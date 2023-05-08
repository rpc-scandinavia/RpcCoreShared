namespace RpcScandinavia.Core.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

#region RpcKeyValueSerializer
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializer.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// RPC Key/Value serializator serializes and deserializes objects to and from a list of key/value objects.
/// It can serialize and deserialize to and from a list of <see cref="System.Collections.Generic.KeyValuePair" /> with
/// a <see cref="System.Memory{System.Char}" /> key and <see cref="System.Memory{System.Char}" /> value.
/// It can also serialize and deserialize to and from a list of any type you choose, when you provide the appropiate
/// functions/expressions to get and set the key and value:
///
/// * Func<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>, KeyValueType> createKeyValueInstance
/// * Func<KeyValueType, ReadOnlyMemory<Char>> getKey
/// * Func<KeyValueType, ReadOnlyMemory<Char>> getValue
///
/// /// The following property attributes are honored, and overrides the JSON attributes:
/// 	<see cref="RpcScandinavia.Core.Shared.KeyValueSerializer.RpcKeyValueSerializerGroupAttribute" />
///
/// The following JSON property attributes are honored:
/// 	<see cref="System.Text.Json.Serialization.JsonIgnoreAttribute" />
/// 	<see cref="System.Text.Json.Serialization.JsonIncludeAttribute" />
/// 	<see cref="System.Text.Json.Serialization.JsonPropertyNameAttribute" />
/// 	<see cref="System.Text.Json.Serialization.JsonPropertyOrderAttribute" />
/// 	<see cref="RpcScandinavia.Core.Shared.KeyValueSerializer.JsonGroupAttribute" />
/// </summary>
public static partial class RpcKeyValueSerializer {
	private const String CacheIsolation = "RpcScandinavia.Core.Shared.RpcKeyValueSerializer";
	private static RpcKeyValueSerializerOptions defaultOptions = new RpcKeyValueSerializerOptions();

	#region Copy methods
	//------------------------------------------------------------------------------------------------------------------
	// Copy methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Copies the object, by serializing the original instance and deserializing into a new instance.
	/// </summary>
	/// <param name="obj">The object to copy.</param>
	/// <typeparam name="T">The type of the object.</typeparam>
	/// <returns>The copy of the object.</returns>
	public static T Copy<T>(T obj) {
		// Validate.
		if (obj == null) {
			throw new NullReferenceException(nameof(obj));
		}

		// Setup the serializer options.
		RpcKeyValueSerializerOptions keyValueSerializerOptions = new RpcKeyValueSerializerOptions();
		keyValueSerializerOptions.SerializeTypeInfo = RpcKeyValueSerializerTypeInfoOption.RequiredAndTop;
		keyValueSerializerOptions.SerializeEnums = RpcKeyValueSerializerEnumOption.AsInteger;
		keyValueSerializerOptions.SerializeThrowExceptions = RpcKeyValueSerializerExceptionOption.ThrowAll;
		keyValueSerializerOptions.DeserializeThrowExceptions = RpcKeyValueSerializerExceptionOption.ThrowAll;

		// Serialize the object into key/values.
		List<KeyValuePair<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>>> values = RpcKeyValueSerializer.SerializeToMemory(obj, keyValueSerializerOptions);

		// Deserialize the key/values into a new object instance.
		return RpcKeyValueSerializer.DeserializeFromMemory<T>(values, keyValueSerializerOptions);
	} // Copy
	#endregion

	#region Serialize methods (KeyValuePair<String, String>)
	//------------------------------------------------------------------------------------------------------------------
	// Serialize methods (KeyValuePair<String, String>).
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Serializes the object into a list of <see cref="System.Collections.Generic.KeyValuePair" /> with a string key
	/// and string value.
	/// </summary>
	/// <param name="obj">The opject to serialize.</param>
	/// <returns>The list of serialized key/value items.</returns>
	public static List<KeyValuePair<String, String>> SerializeToStrings(Object obj) {
		// Validate.
		if (obj == null) {
			throw new NullReferenceException(nameof(obj));
		}

		// Serialize the object into key/values.
		RpcKeyValueBuilder<KeyValuePair<String, String>> keyValueBuilder = new RpcKeyValueBuilder<KeyValuePair<String, String>>(
			(ReadOnlyMemory<Char> key, ReadOnlyMemory<Char> value) => new KeyValuePair<String, String>(key.ToString(), value.ToString()),
			RpcKeyValueSerializer.defaultOptions
		);
		RpcKeyValueSerializer.InternalSerialize(
			ReadOnlyMemory<Char>.Empty,
			obj,
			keyValueBuilder,
			(RpcKeyValueSerializer.defaultOptions.SerializeTypeInfo == RpcKeyValueSerializerTypeInfoOption.RequiredAndTop),
			0,
			RpcKeyValueSerializer.defaultOptions
		);
		return keyValueBuilder.Values;
	} // SerializeToStrings

	/// <summary>
	/// Serializes the object into a list of <see cref="System.Collections.Generic.KeyValuePair" /> with a string key
	/// and string value.
	/// </summary>
	/// <param name="obj">The opject to serialize.</param>
	/// <param name="options">The serialization options.</param>
	/// <returns>The list of serialized key/value items.</returns>
	public static List<KeyValuePair<String, String>> SerializeToStrings(Object obj, RpcKeyValueSerializerOptions options) {
		// Validate.
		if (obj == null) {
			throw new NullReferenceException(nameof(obj));
		}

		// Get the default options.
		if (options == null) {
			options = RpcKeyValueSerializer.defaultOptions;
		}

		// Serialize the object into key/values.
		RpcKeyValueBuilder<KeyValuePair<String, String>> keyValueBuilder = new RpcKeyValueBuilder<KeyValuePair<String, String>>(
			(ReadOnlyMemory<Char> key, ReadOnlyMemory<Char> value) => new KeyValuePair<String, String>(key.ToString(), value.ToString()),
			options
		);
		RpcKeyValueSerializer.InternalSerialize(
			ReadOnlyMemory<Char>.Empty,
			obj,
			keyValueBuilder,
			(options.SerializeTypeInfo == RpcKeyValueSerializerTypeInfoOption.RequiredAndTop),
			0,
			options
		);
		return keyValueBuilder.Values;
	} // SerializeToStrings
	#endregion

	#region Serialize methods (KeyValuePair<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>>)
	//------------------------------------------------------------------------------------------------------------------
	// Serialize methods (KeyValuePair<String, String>).
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Serializes the object into a list of <see cref="System.Collections.Generic.KeyValuePair" /> with a <see cref="System.Memory{System.Char}" /> key
	/// and <see cref="System.Memory{System.Char}" /> value.
	/// </summary>
	/// <param name="obj">The opject to serialize.</param>
	/// <returns>The list of serialized key/value items.</returns>
	public static List<KeyValuePair<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>>> SerializeToMemory(Object obj) {
		// Validate.
		if (obj == null) {
			throw new NullReferenceException(nameof(obj));
		}

		// Serialize the object into key/values.
		RpcKeyValueBuilder<KeyValuePair<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>>> keyValueBuilder = new RpcKeyValueBuilder<KeyValuePair<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>>>(
			(ReadOnlyMemory<Char> key, ReadOnlyMemory<Char> value) => new KeyValuePair<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>>(key, value),
			RpcKeyValueSerializer.defaultOptions
		);
		RpcKeyValueSerializer.InternalSerialize(
			ReadOnlyMemory<Char>.Empty,
			obj,
			keyValueBuilder,
			(RpcKeyValueSerializer.defaultOptions.SerializeTypeInfo == RpcKeyValueSerializerTypeInfoOption.RequiredAndTop),
			0,
			RpcKeyValueSerializer.defaultOptions
		);
		return keyValueBuilder.Values;
	} // SerializeToMemory

	/// <summary>
	/// Serializes the object into a list of <see cref="System.Collections.Generic.KeyValuePair" /> with a <see cref="System.Memory{System.Char}" /> key
	/// and <see cref="System.Memory{System.Char}" /> value.
	/// </summary>
	/// <param name="obj">The opject to serialize.</param>
	/// <param name="options">The serialization options.</param>
	/// <returns>The list of serialized key/value items.</returns>
	public static List<KeyValuePair<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>>> SerializeToMemory(Object obj, RpcKeyValueSerializerOptions options) {
		// Validate.
		if (obj == null) {
			throw new NullReferenceException(nameof(obj));
		}

		// Get the default options.
		if (options == null) {
			options = RpcKeyValueSerializer.defaultOptions;
		}

		// Serialize the object into key/values.
		RpcKeyValueBuilder<KeyValuePair<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>>> keyValueBuilder = new RpcKeyValueBuilder<KeyValuePair<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>>>(
			(ReadOnlyMemory<Char> key, ReadOnlyMemory<Char> value) => new KeyValuePair<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>>(key, value),
			options
		);
		RpcKeyValueSerializer.InternalSerialize(
			ReadOnlyMemory<Char>.Empty,
			obj,
			keyValueBuilder,
			(options.SerializeTypeInfo == RpcKeyValueSerializerTypeInfoOption.RequiredAndTop),
			0,
			options
		);
		return keyValueBuilder.Values;
	} // SerializeToMemory
	#endregion

	#region Serialize methods (KeyValueType)
	//------------------------------------------------------------------------------------------------------------------
	// Serialize methods (KeyValueType).
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Serializes the object into a list of KeyValueType.
	/// </summary>
	/// <param name="obj">The opject to serialize.</param>
	/// <param name="createKeyValueInstance">A function that initializes a new KeyValueType with the key (first argument) and value (second argument).</param>
	/// <typeparam name="KeyValueType">The type of the key/value items.</typeparam>
	/// <returns>The list of serialized key/value items.</returns>
	public static List<KeyValueType> Serialize<KeyValueType>(Object obj, Func<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>, KeyValueType> createKeyValueInstance) {
		// Validate.
		if (obj == null) {
			throw new NullReferenceException(nameof(obj));
		}
		if (createKeyValueInstance == null) {
			throw new NullReferenceException(nameof(createKeyValueInstance));
		}

		// Serialize the object into key/values.
		RpcKeyValueBuilder<KeyValueType> keyValueBuilder = new RpcKeyValueBuilder<KeyValueType>(
			createKeyValueInstance,
			RpcKeyValueSerializer.defaultOptions
		);
		RpcKeyValueSerializer.InternalSerialize(
			ReadOnlyMemory<Char>.Empty,
			obj,
			keyValueBuilder,
			(RpcKeyValueSerializer.defaultOptions.SerializeTypeInfo == RpcKeyValueSerializerTypeInfoOption.RequiredAndTop),
			0,
			RpcKeyValueSerializer.defaultOptions
		);
		return keyValueBuilder.Values;
	} // Serialize

	/// <summary>
	/// Serializes the object into a list of KeyValueType.
	/// </summary>
	/// <param name="obj">The opject to serialize.</param>
	/// <param name="createKeyValueInstance">A function that initializes a new KeyValueType with the key (first argument) and value (second argument).</param>
	/// <param name="options">The serialization options.</param>
	/// <typeparam name="KeyValueType">The type of the key/value items.</typeparam>
	/// <returns>The list of serialized key/value items.</returns>
	public static List<KeyValueType> Serialize<KeyValueType>(Object obj, Func<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>, KeyValueType> createKeyValueInstance, RpcKeyValueSerializerOptions options) {
		// Validate.
		if (obj == null) {
			throw new NullReferenceException(nameof(obj));
		}
		if (createKeyValueInstance == null) {
			throw new NullReferenceException(nameof(createKeyValueInstance));
		}

		// Get the default options.
		if (options == null) {
			options = RpcKeyValueSerializer.defaultOptions;
		}

		// Serialize the object into key/values.
		RpcKeyValueBuilder<KeyValueType> keyValueBuilder = new RpcKeyValueBuilder<KeyValueType>(
			createKeyValueInstance,
			options
		);
		RpcKeyValueSerializer.InternalSerialize(
			ReadOnlyMemory<Char>.Empty,
			obj,
			keyValueBuilder,
			(options.SerializeTypeInfo == RpcKeyValueSerializerTypeInfoOption.RequiredAndTop),
			0,
			options
		);
		return keyValueBuilder.Values;
	} // Serialize
	#endregion

	#region Deserialize methods (KeyValuePair<String, String>)
	//------------------------------------------------------------------------------------------------------------------
	// Deserialize methods (KeyValuePair<String, String>).
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Deserializes the a list of <see cref="System.Collections.Generic.KeyValuePair" /> with a string key and string
	/// value, into a new instance of the the specified type.
	/// </summary>
	/// <param name="values">The previously serialized key/value items.</param>
	/// <typeparam name="T">The type of the deserialized object.</typeparam>
	/// <returns>A new instance of T with the deserialized values.</returns>
	public static T DeserializeFromStrings<T>(IEnumerable<KeyValuePair<String, String>> values) {
		// Validate.
		if (values == null) {
			throw new NullReferenceException(nameof(values));
		}

		// Parse the values.
		RpcKeyValueProvider<KeyValuePair<String, String>> keyValueProvider = new RpcKeyValueProvider<KeyValuePair<String, String>>(
			values,
			(KeyValuePair<String, String> keyValue) => keyValue.Key.AsMemory(),
			(KeyValuePair<String, String> keyValue) => keyValue.Value.AsMemory(),
			RpcKeyValueSerializer.defaultOptions
		);

		// Deserialize into the object.
		return (T)RpcKeyValueSerializer.InternalDeserialize(
			typeof(T),
			default(T),
			keyValueProvider,
			0,
			RpcKeyValueSerializer.defaultOptions
		);
	} // DeserializeFromStrings

	/// <summary>
	/// Deserializes the a list of <see cref="System.Collections.Generic.KeyValuePair" /> with a string key and string
	/// value, into a new instance of the the specified type.
	/// </summary>
	/// <param name="values">The previously serialized key/value items.</param>
	/// <param name="options">The serialization options.</param>
	/// <typeparam name="T">The type of the deserialized object.</typeparam>
	/// <returns>A new instance of T with the deserialized values.</returns>
	public static T DeserializeFromStrings<T>(IEnumerable<KeyValuePair<String, String>> values, RpcKeyValueSerializerOptions options) {
		// Validate.
		if (values == null) {
			throw new NullReferenceException(nameof(values));
		}

		// Get the default options.
		if (options == null) {
			options = RpcKeyValueSerializer.defaultOptions;
		}

		// Parse the values.
		RpcKeyValueProvider<KeyValuePair<String, String>> keyValueProvider = new RpcKeyValueProvider<KeyValuePair<String, String>>(
			values,
			(KeyValuePair<String, String> keyValue) => keyValue.Key.AsMemory(),
			(KeyValuePair<String, String> keyValue) => keyValue.Value.AsMemory(),
			options
		);

		// Deserialize into the object.
		return (T)RpcKeyValueSerializer.InternalDeserialize(
			typeof(T),
			default(T),
			keyValueProvider,
			0,
			options
		);
	} // DeserializeFromStrings

	/// <summary>
	/// Deserializes the a list of <see cref="System.Collections.Generic.KeyValuePair" /> with a string key and string
	/// value, into the specified object.
	/// </summary>
	/// <param name="values">The previously serialized key/value items.</param>
	/// <param name="obj">The object to deserialize into.</param>
	/// <typeparam name="T">The type of the deserialized object.</typeparam>
	public static void DeserializeFromStrings<T>(IEnumerable<KeyValuePair<String, String>> values, T obj) {
		// Validate.
		if (values == null) {
			throw new NullReferenceException(nameof(values));
		}
		if (obj == null) {
			throw new NullReferenceException(nameof(obj));
		}

		// Parse the values.
		RpcKeyValueProvider<KeyValuePair<String, String>> keyValueProvider = new RpcKeyValueProvider<KeyValuePair<String, String>>(
			values,
			(KeyValuePair<String, String> keyValue) => keyValue.Key.AsMemory(),
			(KeyValuePair<String, String> keyValue) => keyValue.Value.AsMemory(),
			RpcKeyValueSerializer.defaultOptions
		);

		// Deserialize into the object.
		RpcKeyValueSerializer.InternalDeserialize(
			typeof(T),
			obj,
			keyValueProvider,
			0,
			RpcKeyValueSerializer.defaultOptions
		);
	} // DeserializeFromStrings

	/// <summary>
	/// Deserializes the a list of <see cref="System.Collections.Generic.KeyValuePair" /> with a string key and string
	/// value, into the specified object.
	/// </summary>
	/// <param name="values">The previously serialized key/value items.</param>
	/// <param name="obj">The object to deserialize into.</param>
	/// <param name="options">The serialization options.</param>
	/// <typeparam name="T">The type of the deserialized object.</typeparam>
	public static void DeserializeFromStrings<T>(IEnumerable<KeyValuePair<String, String>> values, T obj, RpcKeyValueSerializerOptions options) {
		// Validate.
		if (values == null) {
			throw new NullReferenceException(nameof(values));
		}
		if (obj == null) {
			throw new NullReferenceException(nameof(obj));
		}

		// Get the default options.
		if (options == null) {
			options = RpcKeyValueSerializer.defaultOptions;
		}

		// Parse the values.
		RpcKeyValueProvider<KeyValuePair<String, String>> keyValueProvider = new RpcKeyValueProvider<KeyValuePair<String, String>>(
			values,
			(KeyValuePair<String, String> keyValue) => keyValue.Key.AsMemory(),
			(KeyValuePair<String, String> keyValue) => keyValue.Value.AsMemory(),
			options
		);

		// Deserialize into the object.
		RpcKeyValueSerializer.InternalDeserialize(
			typeof(T),
			obj,
			keyValueProvider,
			0,
			options
		);
	} // DeserializeFromStrings
	#endregion

	#region Deserialize methods (KeyValuePair<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>>)
	//------------------------------------------------------------------------------------------------------------------
	// Deserialize methods (KeyValuePair<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>>).
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Deserializes the a list of <see cref="System.Collections.Generic.KeyValuePair" /> with a <see cref="System.Memory{System.Char}" /> key
	/// and <see cref="System.Memory{System.Char}" /> value, into a new instance of the the specified type.
	/// </summary>
	/// <param name="values">The previously serialized key/value items.</param>
	/// <typeparam name="T">The type of the deserialized object.</typeparam>
	/// <returns>A new instance of T with the deserialized values.</returns>
	public static T DeserializeFromMemory<T>(IEnumerable<KeyValuePair<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>>> values) {
		// Validate.
		if (values == null) {
			throw new NullReferenceException(nameof(values));
		}

		// Parse the values.
		RpcKeyValueProvider<KeyValuePair<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>>> keyValueProvider = new RpcKeyValueProvider<KeyValuePair<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>>>(
			values,
			(KeyValuePair<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>> keyValue) => keyValue.Key,
			(KeyValuePair<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>> keyValue) => keyValue.Value,
			RpcKeyValueSerializer.defaultOptions
		);

		// Deserialize into the object.
		return (T)RpcKeyValueSerializer.InternalDeserialize(
			typeof(T),
			default(T),
			keyValueProvider,
			0,
			RpcKeyValueSerializer.defaultOptions
		);
	} // DeserializeFromMemory

	/// <summary>
	/// Deserializes the a list of <see cref="System.Collections.Generic.KeyValuePair" /> with a <see cref="System.Memory{System.Char}" /> key
	/// and <see cref="System.Memory{System.Char}" /> value, into a new instance of the the specified type.
	/// </summary>
	/// <param name="values">The previously serialized key/value items.</param>
	/// <param name="options">The serialization options.</param>
	/// <typeparam name="T">The type of the deserialized object.</typeparam>
	/// <returns>A new instance of T with the deserialized values.</returns>
	public static T DeserializeFromMemory<T>(IEnumerable<KeyValuePair<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>>> values, RpcKeyValueSerializerOptions options) {
		// Validate.
		if (values == null) {
			throw new NullReferenceException(nameof(values));
		}

		// Get the default options.
		if (options == null) {
			options = RpcKeyValueSerializer.defaultOptions;
		}

		// Parse the values.
		RpcKeyValueProvider<KeyValuePair<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>>> keyValueProvider = new RpcKeyValueProvider<KeyValuePair<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>>>(
			values,
			(KeyValuePair<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>> keyValue) => keyValue.Key,
			(KeyValuePair<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>> keyValue) => keyValue.Value,
			options
		);

		// Deserialize into the object.
		return (T)RpcKeyValueSerializer.InternalDeserialize(
			typeof(T),
			default(T),
			keyValueProvider,
			0,
			options
		);
	} // DeserializeFromMemory

	/// <summary>
	/// Deserializes the a list of <see cref="System.Collections.Generic.KeyValuePair" /> with a <see cref="System.Memory{System.Char}" /> key
	/// and <see cref="System.Memory{System.Char}" /> value, into the specified object.
	/// </summary>
	/// <param name="values">The previously serialized key/value items.</param>
	/// <param name="obj">The object to deserialize into.</param>
	/// <typeparam name="T">The type of the deserialized object.</typeparam>
	public static void DeserializeFromMemory<T>(IEnumerable<KeyValuePair<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>>> values, T obj) {
		// Validate.
		if (values == null) {
			throw new NullReferenceException(nameof(values));
		}
		if (obj == null) {
			throw new NullReferenceException(nameof(obj));
		}

		// Parse the values.
		RpcKeyValueProvider<KeyValuePair<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>>> keyValueProvider = new RpcKeyValueProvider<KeyValuePair<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>>>(
			values,
			(KeyValuePair<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>> keyValue) => keyValue.Key,
			(KeyValuePair<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>> keyValue) => keyValue.Value,
			RpcKeyValueSerializer.defaultOptions
		);

		// Deserialize into the object.
		RpcKeyValueSerializer.InternalDeserialize(
			typeof(T),
			obj,
			keyValueProvider,
			0,
			RpcKeyValueSerializer.defaultOptions
		);
	} // DeserializeFromMemory

	/// <summary>
	/// Deserializes the a list of <see cref="System.Collections.Generic.KeyValuePair" /> with a <see cref="System.Memory{System.Char}" /> key
	/// and <see cref="System.Memory{System.Char}" /> value, into the specified object.
	/// </summary>
	/// <param name="values">The previously serialized key/value items.</param>
	/// <param name="obj">The object to deserialize into.</param>
	/// <param name="options">The serialization options.</param>
	/// <typeparam name="T">The type of the deserialized object.</typeparam>
	public static void DeserializeFromMemory<T>(IEnumerable<KeyValuePair<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>>> values, T obj, RpcKeyValueSerializerOptions options) {
		// Validate.
		if (values == null) {
			throw new NullReferenceException(nameof(values));
		}
		if (obj == null) {
			throw new NullReferenceException(nameof(obj));
		}

		// Get the default options.
		if (options == null) {
			options = RpcKeyValueSerializer.defaultOptions;
		}

		// Parse the values.
		RpcKeyValueProvider<KeyValuePair<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>>> keyValueProvider = new RpcKeyValueProvider<KeyValuePair<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>>>(
			values,
			(KeyValuePair<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>> keyValue) => keyValue.Key,
			(KeyValuePair<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>> keyValue) => keyValue.Value,
			options
		);

		// Deserialize into the object.
		RpcKeyValueSerializer.InternalDeserialize(
			typeof(T),
			obj,
			keyValueProvider,
			0,
			options
		);
	} // DeserializeFromMemory
	#endregion

	#region Deserialize methods (KeyValueType)
	//------------------------------------------------------------------------------------------------------------------
	// Deserialize methods (KeyValueType).
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Deserializes the a list of KeyValueType, into a new instance of the the specified type.
	/// the specified type.
	/// </summary>
	/// <param name="values">The previously serialized key/value items.</param>
	/// <param name="getKey">A function that gets the key from the KeyValueType.</param>
	/// <param name="getValue">A function that gets the value from the KeyValueType.</param>
	/// <typeparam name="T">The type of the deserialized object.</typeparam>
	/// <typeparam name="KeyValueType">The type of the key/value items.</typeparam>
	/// <returns>A new instance of T with the deserialized values.</returns>
	public static T Deserialize<T, KeyValueType>(IEnumerable<KeyValueType> values, Func<KeyValueType, ReadOnlyMemory<Char>> getKey, Func<KeyValueType, ReadOnlyMemory<Char>> getValue) {
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

		// Parse the values.
		RpcKeyValueProvider<KeyValueType> keyValueProvider = new RpcKeyValueProvider<KeyValueType>(
			values,
			(KeyValueType keyValue) => getKey(keyValue),
			(KeyValueType keyValue) => getValue(keyValue),
			RpcKeyValueSerializer.defaultOptions
		);

		// Deserialize into the object.
		return (T)RpcKeyValueSerializer.InternalDeserialize(
			typeof(T),
			default(T),
			keyValueProvider,
			0,
			RpcKeyValueSerializer.defaultOptions
		);
	} // Deserialize

	/// <summary>
	/// Deserializes the a list of KeyValueType, into a new instance of the the specified type.
	/// </summary>
	/// <param name="values">The previously serialized key/value items.</param>
	/// <param name="getKey">A function that gets the key from the KeyValueType.</param>
	/// <param name="getValue">A function that gets the value from the KeyValueType.</param>
	/// <param name="options">The serialization options.</param>
	/// <typeparam name="T">The type of the deserialized object.</typeparam>
	/// <typeparam name="KeyValueType">The type of the key/value items.</typeparam>
	/// <returns>A new instance of T with the deserialized values.</returns>
	public static T Deserialize<T, KeyValueType>(IEnumerable<KeyValueType> values, Func<KeyValueType, ReadOnlyMemory<Char>> getKey, Func<KeyValueType, ReadOnlyMemory<Char>> getValue, RpcKeyValueSerializerOptions options) {
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

		// Get the default options.
		if (options == null) {
			options = RpcKeyValueSerializer.defaultOptions;
		}

		// Parse the values.
		RpcKeyValueProvider<KeyValueType> keyValueProvider = new RpcKeyValueProvider<KeyValueType>(
			values,
			(KeyValueType keyValue) => getKey(keyValue),
			(KeyValueType keyValue) => getValue(keyValue),
			options
		);

		// Deserialize into the object.
		return (T)RpcKeyValueSerializer.InternalDeserialize(
			typeof(T),
			default(T),
			keyValueProvider,
			0,
			options
		);
	} // Deserialize

	/// <summary>
	/// Deserializes the a list of KeyValueType, into the specified object.
	/// </summary>
	/// <param name="values">The previously serialized key/value items.</param>
	/// <param name="obj">The object to deserialize into.</param>
	/// <param name="getKey">A function that gets the key from the KeyValueType.</param>
	/// <param name="getValue">A function that gets the value from the KeyValueType.</param>
	/// <typeparam name="T">The type of the deserialized object.</typeparam>
	/// <typeparam name="KeyValueType">The type of the key/value items.</typeparam>
	public static void Deserialize<T, KeyValueType>(IEnumerable<KeyValueType> values, T obj, Func<KeyValueType, ReadOnlyMemory<Char>> getKey, Func<KeyValueType, ReadOnlyMemory<Char>> getValue) {
		// Validate.
		if (values == null) {
			throw new NullReferenceException(nameof(values));
		}
		if (obj == null) {
			throw new NullReferenceException(nameof(obj));
		}
		if (getKey == null) {
			throw new NullReferenceException(nameof(getKey));
		}
		if (getValue == null) {
			throw new NullReferenceException(nameof(getValue));
		}

		// Parse the values.
		RpcKeyValueProvider<KeyValueType> keyValueProvider = new RpcKeyValueProvider<KeyValueType>(
			values,
			(KeyValueType keyValue) => getKey(keyValue),
			(KeyValueType keyValue) => getValue(keyValue),
			RpcKeyValueSerializer.defaultOptions
		);

		// Deserialize into the object.
		RpcKeyValueSerializer.InternalDeserialize(
			typeof(T),
			obj,
			keyValueProvider,
			0,
			RpcKeyValueSerializer.defaultOptions
		);
	} // Deserialize

	/// <summary>
	/// Deserializes the a list of KeyValueType, into the specified object.
	/// </summary>
	/// <param name="values">The previously serialized key/value items.</param>
	/// <param name="obj">The object to deserialize into.</param>
	/// <param name="getKey">A function that gets the key from the KeyValueType.</param>
	/// <param name="getValue">A function that gets the value from the KeyValueType.</param>
	/// <param name="options">The serialization options.</param>
	/// <typeparam name="T">The type of the deserialized object.</typeparam>
	/// <typeparam name="KeyValueType">The type of the key/value items.</typeparam>
	public static void Deserialize<T, KeyValueType>(IEnumerable<KeyValueType> values, T obj, Func<KeyValueType, ReadOnlyMemory<Char>> getKey, Func<KeyValueType, ReadOnlyMemory<Char>> getValue, RpcKeyValueSerializerOptions options) {
		// Validate.
		if (values == null) {
			throw new NullReferenceException(nameof(values));
		}
		if (obj == null) {
			throw new NullReferenceException(nameof(obj));
		}
		if (getKey == null) {
			throw new NullReferenceException(nameof(getKey));
		}
		if (getValue == null) {
			throw new NullReferenceException(nameof(getValue));
		}

		// Get the default options.
		if (options == null) {
			options = RpcKeyValueSerializer.defaultOptions;
		}

		// Parse the values.
		RpcKeyValueProvider<KeyValueType> keyValueProvider = new RpcKeyValueProvider<KeyValueType>(
			values,
			(KeyValueType keyValue) => getKey(keyValue),
			(KeyValueType keyValue) => getValue(keyValue),
			options
		);

		// Deserialize into the object.
		RpcKeyValueSerializer.InternalDeserialize(
			typeof(T),
			obj,
			keyValueProvider,
			0,
			options
		);
	} // Deserialize
	#endregion

	#region GetMemberValue methods
	//------------------------------------------------------------------------------------------------------------------
	// GetMemberValue methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the value identified by the key from the object.
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <param name="key">The key.</param>
	/// <param name="defaultValue">The default value returned, when exceptions are caught and it was not possible to get the value.</param>
	/// <typeparam name="ValueType">The type of the value.</typeparam>
	/// <returns>The value or the default value.</returns>
	public static ValueType GetMemberValue<ValueType>(Object obj, String key, ValueType defaultValue = default(ValueType)) {
		// Validate.
		if (obj == null) {
			throw new NullReferenceException(nameof(obj));
		}
		if (key == null) {
			throw new NullReferenceException(nameof(key));
		}

		try {
			// Get the value.
			IRpcKeyValueInfo keyValueInfo = RpcKeyValueSerializer.GotoValue(
				obj,
				key.AsMemory(),
				RpcKeyValueSerializer.defaultOptions
			);
			return keyValueInfo.GetValue<ValueType>();
		} catch (RpcKeyValueException exception) {
			// Throw the exception.
			exception.Throw(RpcKeyValueSerializer.defaultOptions);
		}

		// Return the default value.
		return defaultValue;
	} // GetMemberValue

	/// <summary>
	/// Gets the value identified by the key from the object.
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <param name="key">The key.</param>
	/// <param name="options">The serialization options.</param>
	/// <param name="defaultValue">The default value returned, when exceptions are caught and it was not possible to get the value.</param>
	/// <typeparam name="ValueType">The type of the value.</typeparam>
	/// <returns>The value or the default value.</returns>
	public static ValueType GetMemberValue<ValueType>(Object obj, String key, RpcKeyValueSerializerOptions options, ValueType defaultValue = default(ValueType)) {
		// Validate.
		if (obj == null) {
			throw new NullReferenceException(nameof(obj));
		}
		if (key == null) {
			throw new NullReferenceException(nameof(key));
		}

		// Get the default options.
		if (options == null) {
			options = RpcKeyValueSerializer.defaultOptions;
		}

		try {
			// Get the value.
			IRpcKeyValueInfo keyValueInfo = RpcKeyValueSerializer.GotoValue(
				obj,
				key.AsMemory(),
				options
			);
			return keyValueInfo.GetValue<ValueType>();
		} catch (RpcKeyValueException exception) {
			// Throw the exception.
			exception.Throw(options);
		}

		// Return the default value.
		return defaultValue;
	} // GetMemberValue
	#endregion

	#region SetMemberValue methods
	//------------------------------------------------------------------------------------------------------------------
	// SetMemberValue methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Sets the value identified by the key in the object.
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <param name="key">The key.</param>
	/// <param name="value">The value.</param>
	public static void SetMemberValue(Object obj, String key, Object value) {
		// Validate.
		if (obj == null) {
			throw new NullReferenceException(nameof(obj));
		}
		if (key == null) {
			throw new NullReferenceException(nameof(key));
		}

		try {
			// Get the value.
			IRpcKeyValueInfo keyValueInfo = RpcKeyValueSerializer.GotoValue(
				obj,
				key.AsMemory(),
				RpcKeyValueSerializer.defaultOptions
			);
			keyValueInfo.SetValue(value);
		} catch (RpcKeyValueException exception) {
			// Throw the exception.
			exception.Throw(RpcKeyValueSerializer.defaultOptions);
		}
	} // SetMemberValue

	/// <summary>
	/// Sets the value identified by the key in the object.
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <param name="key">The key.</param>
	/// <param name="value">The value.</param>
	/// <param name="options">The serialization options.</param>
	public static void SetMemberValue(Object obj, String key, Object value, RpcKeyValueSerializerOptions options) {
		// Validate.
		if (obj == null) {
			throw new NullReferenceException(nameof(obj));
		}
		if (key == null) {
			throw new NullReferenceException(nameof(key));
		}

		// Get the default options.
		if (options == null) {
			options = RpcKeyValueSerializer.defaultOptions;
		}

		try {
			// Get the value.
			IRpcKeyValueInfo keyValueInfo = RpcKeyValueSerializer.GotoValue(
				obj,
				key.AsMemory(),
				options
			);
			keyValueInfo.SetValue(value);
		} catch (RpcKeyValueException exception) {
			// Throw the exception.
			exception.Throw(options);
		}
	} // SetMemberValue
	#endregion

	#region Value information methods
	//------------------------------------------------------------------------------------------------------------------
	// Value information methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets information about the values in the object.
	/// Currently only information about the first level of fields/properties are supported.
	/// </summary>
	/// <param name="obj">The object.</param>
	public static List<IRpcKeyValueInfo> GetValueInfos(Object obj) {
		// Validate.
		if (obj == null) {
			throw new NullReferenceException(nameof(obj));
		}

		try {
			List<IRpcKeyValueInfo> keyValueInfos = new List<IRpcKeyValueInfo>();
			RpcKeyValueSerializer.InternalGetValueInfos(
				ReadOnlyMemory<Char>.Empty,
				obj,
				keyValueInfos,
				RpcKeyValueSerializer.defaultOptions
			);
			return keyValueInfos;
		} catch (RpcKeyValueException exception) {
			// Throw the exception.
			exception.Throw(RpcKeyValueSerializer.defaultOptions);

			return new List<IRpcKeyValueInfo>();
		}
	} // GetValueInfos

	/// <summary>
	/// Gets information about the values in the object.
	/// Currently only information about the first level of fields/properties are supported.
	/// </summary>
	/// <param name="obj">The object.</param>
	/// <param name="options">The serialization options.</param>
	public static List<IRpcKeyValueInfo> GetValueInfos(Object obj, RpcKeyValueSerializerOptions options) {
		// Validate.
		if (obj == null) {
			throw new NullReferenceException(nameof(obj));
		}

		// Get the default options.
		if (options == null) {
			options = RpcKeyValueSerializer.defaultOptions;
		}

		try {
			List<IRpcKeyValueInfo> keyValueInfos = new List<IRpcKeyValueInfo>();
			RpcKeyValueSerializer.InternalGetValueInfos(
				ReadOnlyMemory<Char>.Empty,
				obj,
				keyValueInfos,
				options
			);
			return keyValueInfos;
		} catch (RpcKeyValueException exception) {
			// Throw the exception.
			exception.Throw(options);

			return new List<IRpcKeyValueInfo>();
		}
	} // GetValueInfos
	#endregion

} // RpcKeyValueSerializer
#endregion
