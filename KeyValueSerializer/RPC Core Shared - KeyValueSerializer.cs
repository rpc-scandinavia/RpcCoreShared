namespace RpcScandinavia.Core.Shared.KeyValueSerializer;
using System;
using System.Collections.Generic;

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
	/// Copies the object, by serializing and deserializing it.
	/// </summary>
	/// <param name="obj">The object to copy.</param>
	/// <typeparam name="T">The type of the object.</typeparam>
	/// <returns>The copy of the object.</returns>
	public static T Copy<T>(T obj) {
		// Setup the serializer options.
		RpcKeyValueSerializerOptions keyValueSerializerOptions = new RpcKeyValueSerializerOptions();
		keyValueSerializerOptions.SerializeTypeInfo = true;
		keyValueSerializerOptions.DeserializeTypeInfo = true;
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
	/// <returns>The list of serialized object key/values.</returns>
	public static List<KeyValuePair<String, String>> Serialize(Object obj) {
		// Validate.
		if (obj == null) {
			throw new NullReferenceException(nameof(obj));
		}

		// Serialize the object into key/values.
		return RpcKeyValueSerializer.Serialize<KeyValuePair<String, String>>(
			obj,
			(String key, String value) => new KeyValuePair<String, String>(key, value),
			RpcKeyValueSerializer.defaultOptions
		);
	} // Serialize

	/// <summary>
	/// Serializes the object into a list of <see cref="System.Collections.Generic.KeyValuePair" /> with a string key
	/// and string value.
	/// </summary>
	/// <param name="obj">The opject to serialize.</param>
	/// <param name="options">The serialization options.</param>
	/// <returns>The list of serialized object key/values.</returns>
	public static List<KeyValuePair<String, String>> Serialize(Object obj, RpcKeyValueSerializerOptions options) {
		// Validate.
		if (obj == null) {
			throw new NullReferenceException(nameof(obj));
		}

		// Serialize the object into key/values.
		return RpcKeyValueSerializer.Serialize<KeyValuePair<String, String>>(
			obj,
			(String key, String value) => new KeyValuePair<String, String>(key, value),
			options
		);
	} // Serialize

	/// <summary>
	/// Serializes the object into a list of KeyValueType.
	/// </summary>
	/// <param name="obj">The opject to serialize.</param>
	/// <param name="createKeyValueInstance">A function that initializes a new KeyValueType with the key (first argument) and value (second argument).</param>
	/// <param name="options">The serialization options.</param>
	/// <typeparam name="KeyValueType">The type of the key/value objects.</typeparam>
	/// <returns>The list of serialized object key/values.</returns>
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

		// Serialize the object into key/values.
		List<KeyValueType> values = new List<KeyValueType>();

		try {
			// Get the handler.
			RpcKeyValueSerializerHandler handler = RpcKeyValueSerializerHandler.GetHandler(obj.GetType(), options);

			// Serialize the object.
			handler.Serialize(
				obj,
				values,
				String.Empty,
				createKeyValueInstance,
				options
			);
		} catch {
			// Throw the exception.
			if (options.SerializeThrowExceptions.HasFlag(RpcKeyValueSerializerExceptionOption.ThrowCriticalExceptions) == true) {
				throw;
			}
		}

		// Return the key/value pairs.
		return values;
	} // Serialize
	#endregion

	#region Deserialize methods
	//------------------------------------------------------------------------------------------------------------------
	// Deserialize methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Deserializes the a list of <see cref="System.Collections.Generic.KeyValuePair" /> with a string key and string
	/// value, into the specified object.
	/// </summary>
	/// <param name="values">The key/values to deserialize.</param>
	/// <typeparam name="T">The type of the deserialized object.</typeparam>
	/// <returns>A new instance of T with the deserialized values.</returns>
	public static T Deserialize<T>(IEnumerable<KeyValuePair<String, String>> values) {
		// Validate.
		if (values == null) {
			throw new NullReferenceException(nameof(values));
		}

		// Deserialize into the object.
		return RpcKeyValueSerializer.InternalDeserialize<T, KeyValuePair<String, String>>(
			values,
			default(T),
			(KeyValuePair<String, String> keyValue) => keyValue.Key,
			(KeyValuePair<String, String> keyValue) => keyValue.Value,
			RpcKeyValueSerializer.defaultOptions
		);
	} // Deserialize

	/// <summary>
	/// Deserializes the a list of <see cref="System.Collections.Generic.KeyValuePair" /> with a string key and string
	/// value, into the specified object.
	/// </summary>
	/// <param name="values">The key/values to deserialize.</param>
	/// <param name="options">The serialization options.</param>
	/// <typeparam name="T">The type of the deserialized object.</typeparam>
	/// <returns>A new instance of T with the deserialized values.</returns>
	public static T Deserialize<T>(IEnumerable<KeyValuePair<String, String>> values, RpcKeyValueSerializerOptions options) {
		// Validate.
		if (values == null) {
			throw new NullReferenceException(nameof(values));
		}

		// Deserialize into the object.
		return RpcKeyValueSerializer.InternalDeserialize<T, KeyValuePair<String, String>>(
			values,
			default(T),
			(KeyValuePair<String, String> keyValue) => keyValue.Key,
			(KeyValuePair<String, String> keyValue) => keyValue.Value,
			RpcKeyValueSerializer.defaultOptions
		);
	} // Deserialize

	/// <summary>
	/// Deserializes the a list of <see cref="System.Collections.Generic.KeyValuePair" /> with a string key and string
	/// value, into the specified object.
	/// </summary>
	/// <param name="values">The key/values to deserialize.</param>
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
		RpcKeyValueSerializer.InternalDeserialize<T, KeyValuePair<String, String>>(
			values,
			obj,
			(KeyValuePair<String, String> keyValue) => keyValue.Key,
			(KeyValuePair<String, String> keyValue) => keyValue.Value,
			RpcKeyValueSerializer.defaultOptions
		);
	} // Deserialize

	/// <summary>
	/// Deserializes the a list of <see cref="System.Collections.Generic.KeyValuePair" /> with a string key and string
	/// value, into the specified object.
	/// </summary>
	/// <param name="values">The key/values to deserialize.</param>
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

		// Deserialize into the object.
		RpcKeyValueSerializer.InternalDeserialize<T, KeyValuePair<String, String>>(
			values,
			obj,
			(KeyValuePair<String, String> keyValue) => keyValue.Key,
			(KeyValuePair<String, String> keyValue) => keyValue.Value,
			options
		);
	} // Deserialize

	/// <summary>
	/// Deserializes the a list of <see cref="System.Collections.Generic.KeyValuePair" /> with a string key and string
	/// value, into the specified object.
	/// </summary>
	/// <param name="values">The key/values to deserialize.</param>
	/// <param name="getKey">A function that gets the key from the KeyValueType.</param>
	/// <param name="getValue">A function that gets the value from the KeyValueType.</param>
	/// <typeparam name="T">The type of the deserialized object.</typeparam>
	/// <typeparam name="KeyValueType">The type of the key/value objects.</typeparam>
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
		return RpcKeyValueSerializer.InternalDeserialize<T, KeyValueType>(
			values,
			default(T),
			getKey,
			getValue,
			RpcKeyValueSerializer.defaultOptions
		);
	} // Deserialize

	/// <summary>
	/// Deserializes the a list of KeyValueType into the specified object.
	/// </summary>
	/// <param name="values">The key/values to deserialize.</param>
	/// <param name="getKey">A function that gets the key from the KeyValueType.</param>
	/// <param name="getValue">A function that gets the value from the KeyValueType.</param>
	/// <param name="options">The serialization options.</param>
	/// <typeparam name="T">The type of the deserialized object.</typeparam>
	/// <typeparam name="KeyValueType">The type of the key/value objects.</typeparam>
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

		// Deserialize into the object.
		return RpcKeyValueSerializer.InternalDeserialize<T, KeyValueType>(
			values,
			default(T),
			getKey,
			getValue,
			options
		);
	} // Deserialize

	/// <summary>
	/// Deserializes the a list of KeyValueType into the specified object.
	/// </summary>
	/// <param name="values">The key/values to deserialize.</param>
	/// <param name="obj">The object to deserialize into.</param>
	/// <param name="getKey">A function that gets the key from the KeyValueType.</param>
	/// <param name="getValue">A function that gets the value from the KeyValueType.</param>
	/// <typeparam name="T">The type of the deserialized object.</typeparam>
	/// <typeparam name="KeyValueType">The type of the key/value objects.</typeparam>
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
		RpcKeyValueSerializer.InternalDeserialize<T, KeyValueType>(
			values,
			obj,
			getKey,
			getValue,
			RpcKeyValueSerializer.defaultOptions
		);
	} // Deserialize

	/// <summary>
	/// Deserializes the a list of KeyValueType into the specified object.
	/// </summary>
	/// <param name="values">The key/values to deserialize.</param>
	/// <param name="obj">The object to deserialize into.</param>
	/// <param name="getKey">A function that gets the key from the KeyValueType.</param>
	/// <param name="getValue">A function that gets the value from the KeyValueType.</param>
	/// <param name="options">The serialization options.</param>
	/// <typeparam name="T">The type of the deserialized object.</typeparam>
	/// <typeparam name="KeyValueType">The type of the key/value objects.</typeparam>
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

		// Deserialize into the object.
		RpcKeyValueSerializer.InternalDeserialize<T, KeyValueType>(
			values,
			obj,
			getKey,
			getValue,
			options
		);
	} // Deserialize

	/// <summary>
	/// Deserializes the a list of KeyValueType into the specified object.
	/// </summary>
	/// <param name="values">The key/values to deserialize.</param>
	/// <param name="obj">The object to deserialize into.</param>
	/// <param name="getKey">A function that gets the key from the KeyValueType.</param>
	/// <param name="getValue">A function that gets the value from the KeyValueType.</param>
	/// <param name="options">The serialization options.</param>
	/// <typeparam name="T">The type of the deserialized object.</typeparam>
	/// <typeparam name="KeyValueType">The type of the key/value objects.</typeparam>
	private static T InternalDeserialize<T, KeyValueType>(IEnumerable<KeyValueType> values, T obj, Func<KeyValueType, String> getKey, Func<KeyValueType, String> getValue, RpcKeyValueSerializerOptions options) {
		// Get the default options.
		if (options == null) {
			options = RpcKeyValueSerializer.defaultOptions;
		}

		// Deserialize the key/values into the object.
		try {
			// Get the handler.
			RpcKeyValueSerializerHandler handler = RpcKeyValueSerializerHandler.GetHandler(obj?.GetType(), options);

			// Deserialize the key/values.
			Object result = handler.Deserialize<KeyValueType>(
				obj,
				values,
				String.Empty,
				getKey,
				getValue,
				options
			);

			// Return the deserialized object.
			return (T)result;
		} catch {
			// Throw the exception.
//			if (options.SerializeThrowExceptions.HasFlag(RpcKeyValueSerializerExceptionOption.ThrowCriticalExceptions) == true) {
				throw;
//			}

			// Return the object or default.
			if (obj != null) {
				return obj;
			} else {
				return default(T);
			}
		}
	} // InternalDeserialize
	#endregion

} // RpcKeyValueSerializer
#endregion
