namespace RpcScandinavia.Core.Shared.KeyValueSerializer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#region RpcKeyValueSerializer
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializer.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// RPC Key/Value serializator serializes and deserializes objects to and from a list of key/value objects.
/// It can serialize and deserialize to and from a list of <see cref="System.Collections.Generic.KeyValuePair" /> with
/// a string key and string value. It can serialize and deserialize to and from a list of any type you choose, when you
/// provide the appropiate functions/expressions to get and set the key and value:
///
/// * Func<String, String, KeyValueType> createKeyValueInstance
/// * Func<KeyValueType, String> getKey
/// * Func<KeyValueType, String> getValue
///
/// The following property attributes are honored, and overrides the JSON attributes:
/// 	<see cref="RpcScandinavia.Core.Shared.KeyValueSerializer.RpcKeyValueSerializerGroupAttribute" />
///
/// The following JSON property attributes are honored:
/// 	<see cref="System.Text.Json.Serialization.JsonIgnoreAttribute" />
/// 	<see cref="System.Text.Json.Serialization.JsonIncludeAttribute" />
/// 	<see cref="System.Text.Json.Serialization.JsonPropertyNameAttribute" />
/// 	<see cref="System.Text.Json.Serialization.JsonPropertyOrderAttribute" />
/// 	<see cref="RpcScandinavia.Core.Shared.KeyValueSerializer.JsonPropertyGroupAttribute" />
///
/// Also see <see cref="RpcScandinavia.Core.Shared.RpcTypeHandler" />.
/// </summary>
public static class RpcKeyValueSerializer {
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
		keyValueSerializerOptions.SerializeTypeInfo = RpcKeyValueSerializerTypeInfoOption.Always;
		keyValueSerializerOptions.SerializeEnums = RpcKeyValueSerializerEnumOption.AsInteger;
		keyValueSerializerOptions.SerializeThrowExceptions = RpcKeyValueSerializerExceptionOption.CatchAll;
		keyValueSerializerOptions.DeserializeEnums = true;
		keyValueSerializerOptions.DeserializeThrowExceptions = RpcKeyValueSerializerExceptionOption.CatchAll;

		// Serialize the object into key/values.
		List<KeyValuePair<String, String>> values = RpcKeyValueSerializer.Serialize(obj, keyValueSerializerOptions);

		// Deserialize the key/values into a new object instance.
		return RpcKeyValueSerializer.Deserialize<T>(values, keyValueSerializerOptions);
	} // Copy
	#endregion

	#region Serialize methods
	//------------------------------------------------------------------------------------------------------------------
	// Serialize methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Serializes the object into a list of <see cref="System.Collections.Generic.KeyValuePair" /> with a string key
	/// and string value.
	/// </summary>
	/// <param name="obj">The opject to serialize.</param>
	/// <returns>The list of serialized key/value items.</returns>
	public static List<KeyValuePair<String, String>> Serialize(Object obj) {
		// Validate.
		if (obj == null) {
			throw new NullReferenceException(nameof(obj));
		}

		// Serialize the object into key/values.
		// Serialize into the values.
		RpcKeyValueBuilder<KeyValuePair<String, String>> keyValueBuilder = new RpcKeyValueBuilder<KeyValuePair<String, String>>(
			(String key, String value) => new KeyValuePair<String, String>(key, value),
			RpcKeyValueSerializer.defaultOptions
		);
		RpcKeyValueSerializer.InternalSerialize<KeyValuePair<String, String>>(
			obj,
			keyValueBuilder,
			RpcKeyValueSerializer.defaultOptions
		);
		return keyValueBuilder.Values;
	} // Serialize

	/// <summary>
	/// Serializes the object into a list of <see cref="System.Collections.Generic.KeyValuePair" /> with a string key
	/// and string value.
	/// </summary>
	/// <param name="obj">The opject to serialize.</param>
	/// <param name="options">The serialization options.</param>
	/// <returns>The list of serialized key/value items.</returns>
	public static List<KeyValuePair<String, String>> Serialize(Object obj, RpcKeyValueSerializerOptions options) {
		// Validate.
		if (obj == null) {
			throw new NullReferenceException(nameof(obj));
		}

		// Get the default options.
		if (options == null) {
			options = RpcKeyValueSerializer.defaultOptions;
		}

		// Serialize into the values.
		RpcKeyValueBuilder<KeyValuePair<String, String>> keyValueBuilder = new RpcKeyValueBuilder<KeyValuePair<String, String>>(
			(String key, String value) => new KeyValuePair<String, String>(key, value),
			options
		);
		RpcKeyValueSerializer.InternalSerialize<KeyValuePair<String, String>>(
			obj,
			keyValueBuilder,
			options
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
	public static List<KeyValueType> Serialize<KeyValueType>(Object obj, Func<String, String, KeyValueType> createKeyValueInstance, RpcKeyValueSerializerOptions options) {
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

		// Serialize into the values.
		RpcKeyValueBuilder<KeyValueType> keyValueBuilder = new RpcKeyValueBuilder<KeyValueType>(
			createKeyValueInstance,
			options
		);
		RpcKeyValueSerializer.InternalSerialize<KeyValueType>(
			obj,
			keyValueBuilder,
			options
		);
		return keyValueBuilder.Values;
	} // Serialize

	/// <summary>
	/// Serializes the object into a list of KeyValueType.
	/// </summary>
	/// <param name="obj">The opject to serialize.</param>
	/// <param name="keyValueBuilder">The serialized key/value items.</param>
	/// <param name="options">The serialization options.</param>
	/// <typeparam name="KeyValueType">The type of the key/value items.</typeparam>
	private static void InternalSerialize<KeyValueType>(Object obj, RpcKeyValueBuilder<KeyValueType> keyValueBuilder, RpcKeyValueSerializerOptions options) {
		try {
			// Get the handler.
			RpcMemberInfo memberInfo = new RpcMemberInfoType(obj.GetType(), String.Empty);
			RpcKeyValueSerializerHandler handler = RpcKeyValueSerializerHandler.GetHandler(memberInfo, options);

			// Serialize the object.
			handler.Serialize(
				memberInfo,
				obj,
				String.Empty,
				keyValueBuilder,
				0,
				options
			);
		} catch (RpcKeyValueException exception) {
			// Throw the exception.
			exception.Throw(options);
		}
	} // Serialize
	#endregion

	#region Deserialize methods
	//------------------------------------------------------------------------------------------------------------------
	// Deserialize methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Deserializes the a list of <see cref="System.Collections.Generic.KeyValuePair" /> with a string key and string
	/// value, into a new instance of the the specified type.
	/// </summary>
	/// <param name="values">The previously serialized key/value items.</param>
	/// <typeparam name="T">The type of the deserialized object.</typeparam>
	/// <returns>A new instance of T with the deserialized values.</returns>
	public static T Deserialize<T>(IEnumerable<KeyValuePair<String, String>> values) {
		// Validate.
		if (values == null) {
			throw new NullReferenceException(nameof(values));
		}

		// Deserialize into the object.
		RpcKeyValueProvider<KeyValuePair<String, String>> keyValueProvider = new RpcKeyValueProvider<KeyValuePair<String, String>>(
			values,
			(KeyValuePair<String, String> keyValue) => keyValue.Key,
			(KeyValuePair<String, String> keyValue) => keyValue.Value,
			RpcKeyValueSerializer.defaultOptions
		);
		return RpcKeyValueSerializer.InternalDeserialize<T, KeyValuePair<String, String>>(
			keyValueProvider,
			default(T),
			RpcKeyValueSerializer.defaultOptions
		);
	} // Deserialize

	/// <summary>
	/// Deserializes the a list of <see cref="System.Collections.Generic.KeyValuePair" /> with a string key and string
	/// value, into a new instance of the the specified type.
	/// </summary>
	/// <param name="values">The previously serialized key/value items.</param>
	/// <param name="options">The serialization options.</param>
	/// <typeparam name="T">The type of the deserialized object.</typeparam>
	/// <returns>A new instance of T with the deserialized values.</returns>
	public static T Deserialize<T>(IEnumerable<KeyValuePair<String, String>> values, RpcKeyValueSerializerOptions options) {
		// Validate.
		if (values == null) {
			throw new NullReferenceException(nameof(values));
		}

		// Get the default options.
		if (options == null) {
			options = RpcKeyValueSerializer.defaultOptions;
		}

		// Deserialize into the object.
		RpcKeyValueProvider<KeyValuePair<String, String>> keyValueProvider = new RpcKeyValueProvider<KeyValuePair<String, String>>(
			values,
			(KeyValuePair<String, String> keyValue) => keyValue.Key,
			(KeyValuePair<String, String> keyValue) => keyValue.Value,
			options
		);
		return RpcKeyValueSerializer.InternalDeserialize<T, KeyValuePair<String, String>>(
			keyValueProvider,
			default(T),
			options
		);
	} // Deserialize

	/// <summary>
	/// Deserializes the a list of <see cref="System.Collections.Generic.KeyValuePair" /> with a string key and string
	/// value, into the specified object.
	/// </summary>
	/// <param name="values">The previously serialized key/value items.</param>
	/// <param name="obj">The object to deserialize into.</param>
	/// <typeparam name="T">The type of the deserialized object.</typeparam>
	public static void Deserialize<T>(IEnumerable<KeyValuePair<String, String>> values, T obj) {
		// Validate.
		if (values == null) {
			throw new NullReferenceException(nameof(values));
		}
		if (obj == null) {
			throw new NullReferenceException(nameof(obj));
		}

		// Deserialize into the object.
		RpcKeyValueProvider<KeyValuePair<String, String>> keyValueProvider = new RpcKeyValueProvider<KeyValuePair<String, String>>(
			values,
			(KeyValuePair<String, String> keyValue) => keyValue.Key,
			(KeyValuePair<String, String> keyValue) => keyValue.Value,
			RpcKeyValueSerializer.defaultOptions
		);
		RpcKeyValueSerializer.InternalDeserialize<T, KeyValuePair<String, String>>(
			keyValueProvider,
			obj,
			RpcKeyValueSerializer.defaultOptions
		);
	} // Deserialize

	/// <summary>
	/// Deserializes the a list of <see cref="System.Collections.Generic.KeyValuePair" /> with a string key and string
	/// value, into the specified object.
	/// </summary>
	/// <param name="values">The previously serialized key/value items.</param>
	/// <param name="obj">The object to deserialize into.</param>
	/// <param name="options">The serialization options.</param>
	/// <typeparam name="T">The type of the deserialized object.</typeparam>
	public static void Deserialize<T>(IEnumerable<KeyValuePair<String, String>> values, T obj, RpcKeyValueSerializerOptions options) {
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

		// Deserialize into the object.
		RpcKeyValueProvider<KeyValuePair<String, String>> keyValueProvider = new RpcKeyValueProvider<KeyValuePair<String, String>>(
			values,
			(KeyValuePair<String, String> keyValue) => keyValue.Key,
			(KeyValuePair<String, String> keyValue) => keyValue.Value,
			options
		);
		RpcKeyValueSerializer.InternalDeserialize<T, KeyValuePair<String, String>>(
			keyValueProvider,
			obj,
			options
		);
	} // Deserialize

	/// <summary>
	/// Deserializes the a list of <see cref="System.Collections.Generic.KeyValuePair" /> with a string key and string
	/// value, into a new instance of the the specified type.
	/// </summary>
	/// <param name="values">The previously serialized key/value items.</param>
	/// <param name="getKey">A function that gets the key from the KeyValueType.</param>
	/// <param name="getValue">A function that gets the value from the KeyValueType.</param>
	/// <typeparam name="T">The type of the deserialized object.</typeparam>
	/// <typeparam name="KeyValueType">The type of the key/value items.</typeparam>
	/// <returns>A new instance of T with the deserialized values.</returns>
	public static T Deserialize<T, KeyValueType>(IEnumerable<KeyValueType> values, Func<KeyValueType, String> getKey, Func<KeyValueType, String> getValue) {
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

		// Deserialize into the object.
		RpcKeyValueProvider<KeyValueType> keyValueProvider = new RpcKeyValueProvider<KeyValueType>(
			values,
			getKey,
			getValue,
			RpcKeyValueSerializer.defaultOptions
		);
		return RpcKeyValueSerializer.InternalDeserialize<T, KeyValueType>(
			keyValueProvider,
			default(T),
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
	public static T Deserialize<T, KeyValueType>(IEnumerable<KeyValueType> values, Func<KeyValueType, String> getKey, Func<KeyValueType, String> getValue, RpcKeyValueSerializerOptions options) {
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

		// Deserialize into the object.
		RpcKeyValueProvider<KeyValueType> keyValueProvider = new RpcKeyValueProvider<KeyValueType>(
			values,
			getKey,
			getValue,
			options
		);
		return RpcKeyValueSerializer.InternalDeserialize<T, KeyValueType>(
			keyValueProvider,
			default(T),
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
	public static void Deserialize<T, KeyValueType>(IEnumerable<KeyValueType> values, T obj, Func<KeyValueType, String> getKey, Func<KeyValueType, String> getValue) {
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

		// Deserialize into the object.
		RpcKeyValueProvider<KeyValueType> keyValueProvider = new RpcKeyValueProvider<KeyValueType>(
			values,
			getKey,
			getValue,
			RpcKeyValueSerializer.defaultOptions
		);
		RpcKeyValueSerializer.InternalDeserialize<T, KeyValueType>(
			keyValueProvider,
			obj,
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
	public static void Deserialize<T, KeyValueType>(IEnumerable<KeyValueType> values, T obj, Func<KeyValueType, String> getKey, Func<KeyValueType, String> getValue, RpcKeyValueSerializerOptions options) {
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

		// Deserialize into the object.
		RpcKeyValueProvider<KeyValueType> keyValueProvider = new RpcKeyValueProvider<KeyValueType>(
			values,
			getKey,
			getValue,
			options
		);
		RpcKeyValueSerializer.InternalDeserialize<T, KeyValueType>(
			keyValueProvider,
			obj,
			options
		);
	} // Deserialize

	/// <summary>
	/// Deserializes the a list of KeyValueType, into the specified object.
	/// </summary>
	/// <param name="keyValueProvider">The previously serialized key/value items.</param>
	/// <param name="obj">The object to deserialize into.</param>
	/// <param name="options">The serialization options.</param>
	/// <typeparam name="T">The type of the deserialized object.</typeparam>
	/// <typeparam name="KeyValueType">The type of the key/value items.</typeparam>
	private static T InternalDeserialize<T, KeyValueType>(RpcKeyValueProvider<KeyValueType> keyValueProvider, T obj, RpcKeyValueSerializerOptions options) {
		// Deserialize the key/values into the object.
		try {
			// Get the handler.
			RpcMemberInfo memberInfo = new RpcMemberInfoType(typeof(T), String.Empty);
			RpcKeyValueSerializerHandler handler = RpcKeyValueSerializerHandler.GetHandler(memberInfo, options);

			// Deserialize the key/values.
			Object result = handler.Deserialize<KeyValueType>(
				memberInfo,
				obj,
				String.Empty,
				keyValueProvider,
				0,
				options
			);

			// Return the deserialized object.
			return (T)result;
		} catch (RpcKeyValueException exception) {
//Console.WriteLine($"==================================================");
//Console.WriteLine(exception.Message);
//Console.WriteLine(exception.StackTrace);
//Console.WriteLine($"==================================================");
			// Throw the exception.
			exception.Throw(options);

			// Return the object or default.
			return obj ?? default(T);
//		} catch (Exception exception) {
//Console.WriteLine($"==================================================");
//Console.WriteLine(exception.Message);
//Console.WriteLine(exception.StackTrace);
//Console.WriteLine($"==================================================");
//			// Return the object or default.
//			return obj ?? default(T);
		}
	} // InternalDeserialize
	#endregion

} // RpcKeyValueSerializer
#endregion
