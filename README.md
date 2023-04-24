[![build and test](https://github.com/rpc-scandinavia/RpcCoreShared/actions/workflows/build-and-test.yml/badge.svg?branch=main)](https://github.com/rpc-scandinavia/RpcCoreShared/actions/workflows/build-and-test.yml)

# RpcCoreShared
RPC Core Shared contains interfaces and common classes used in most applications from RPC Scandinavia.

## RpcKeyValueSerializer
RPC Key/Value Serializer can be used to serialize a object into a collection of key/value string pairs.

The following features are supported:

* Custom converters
* Handles generic lists
* Handles generic dictionaries
* Handles arrays
* Handles enums and enums with the flags attribute

It serializes the object into a list of key/value pairs, but it is posible to serialize into something else by providing a `Func` for creating a new key/vlue item when serializing, and two functions for getting a key and for getting a value when deserializing.

The keys uses comma (`:`) as separator character, and this was choosen becauae it is what Microsoft uses when storing configuration options.

When deserializing into fields or properties with an non instantiable type (abstract, interface), or into arrays and lists with an non instantiable element type - some type information about the original serialized type is needed. The serializer can include this meta data, and you can see this demonstrated with the tests.

The `Copy` method shows how to use the serializer:

```
// Setup the serializer options.
RpcKeyValueSerializerOptions keyValueSerializerOptions = new RpcKeyValueSerializerOptions();
keyValueSerializerOptions.SerializeTypeInfo = RpcKeyValueSerializerTypeInfoOption.Always;
keyValueSerializerOptions.SerializeEnums = RpcKeyValueSerializerEnumOption.AsInteger;
keyValueSerializerOptions.SerializeThrowExceptions = RpcKeyValueSerializerExceptionOption.CatchAll;
keyValueSerializerOptions.DeserializeThrowExceptions = RpcKeyValueSerializerExceptionOption.CatchAll;

// Serialize the object into key/values.
List<KeyValuePair<String, String>> values = RpcKeyValueSerializer.Serialize(obj, keyValueSerializerOptions);

// Deserialize the key/values into a new object instance.
return RpcKeyValueSerializer.Deserialize<T>(values, keyValueSerializerOptions);
```

## RpcAssemblyQualifiedName
RPC Assembly Qualified Name is a parser that can be used to parse type names.
The `Type` property return the type of the parsed Assembly Qualified Name, if it is available in the loaded assemblies.

It supports:

* Normal types
* Array types, including multi-dimentional array types
* Generic types

The default `Equals` methods, compare the type name and the assembly name, but ignores the version, culture and public key.

```
// Parse generic Assembly Qualified Name.
RpcAssemblyQualifiedName aqn = new RpcAssemblyQualifiedName("System.Collections.Generic.Dictionary`2[[System.String, System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e],[System.Collections.Generic.KeyValuePair`2[[System.Int32, System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e],[System.String, System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]], System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]], System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e");

// Create an instance of the generic type.
Dictionary<String, KeyValuePair<Int32, String>> obj = (Dictionary<String, KeyValuePair<Int32, String>>)Activator.CreateInstance(aqn.Type);
```
