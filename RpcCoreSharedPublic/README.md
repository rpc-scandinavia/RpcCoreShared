[![.NET](https://github.com/rpc-scandinavia/RpcCoreShared/actions/workflows/dotnet.yml/badge.svg)](https://github.com/rpc-scandinavia/RpcCoreShared/actions/workflows/dotnet.yml)
[![GitHub](https://img.shields.io/github/license/rpc-scandinavia/RpcCoreShared?logo=github)](https://github.com/rpc-scandinavia/RpcCoreShared/blob/master/LICENSE)

# RpcCoreShared
RPC Core Shared contains interfaces and common classes used in most applications from RPC Scandinavia.

## Extensions
RPC Core Extensions is my static methods, for extending all sort of types.
Currently, there are extension methods for:

* Comparer [🔗](https://github.com/rpc-scandinavia/RpcCoreShared/blob/master/RpcCoreSharedPublic/Miscelenious/ComparerExtensions.cs) (chain comparer with `then` [🔗](https://github.com/rpc-scandinavia/RpcCoreShared/blob/master/RpcCoreSharedPublic/Miscelenious/ComparerChainNode.cs), and reverse with `reverse` [🔗](https://github.com/rpc-scandinavia/RpcCoreShared/blob/master/RpcCoreSharedPublic/Miscelenious/ComparerReverse.cs)) 
* Memory<Char> [🔗](https://github.com/rpc-scandinavia/RpcCoreShared/blob/master/RpcCoreSharedPublic/Extensions/Char%20(Contiguous%20region%20of%20memory)/)
* Span<Char> [🔗](https://github.com/rpc-scandinavia/RpcCoreShared/blob/master/RpcCoreSharedPublic/Extensions/Char%20(Contiguous%20region%20of%20memory)/)
* String [🔗](https://github.com/rpc-scandinavia/RpcCoreShared/blob/master/RpcCoreSharedPublic/Extensions/String/)
* String (hash and validate) [🔗](https://github.com/rpc-scandinavia/RpcCoreShared/blob/master/RpcCoreSharedPublic/Miscelenious/Hash.cs)
* StringBuilder [🔗](https://github.com/rpc-scandinavia/RpcCoreShared/blob/master/RpcCoreSharedPublic/Extensions/Miscelenious/StringBuilder.cs)
* Type [🔗](https://github.com/rpc-scandinavia/RpcCoreShared/blob/master/RpcCoreSharedPublic/Extensions/Type/Type.cs)

Note that I have a lot of old extension methods, and I will add those not available in Linq.

## RpcAssemblyQualifiedName [🔗](https://github.com/rpc-scandinavia/RpcCoreShared/blob/master/RpcCoreSharedPublic/Miscelenious/AssemblyQualifiedName.cs)
This is a Assembly Qualified Name parser. It uses `ReadOnlyMemory<Char>` and can compare names ignoring the assembly version, assembly culture and/or the assembly public key token.

## RpcDictionaryList<TKey, TValue> [🔗](https://github.com/rpc-scandinavia/RpcCoreShared/blob/master/RpcCoreSharedPublic/Miscelenious/DictionaryList.cs)
This is a dictionary, where the value is a list of values, basically a `IDictionary<TKey, List<TValue>>`.

## RpcGridList<T> [🔗](https://github.com/rpc-scandinavia/RpcCoreShared/blob/master/RpcCoreSharedPublic/Miscelenious/GridList.cs)
This is a two-dimensional list, with rows and columns.
The items are stored in a `List<T>`, and the position in the grid is calculated.

## RpcGuid [🔗](https://github.com/rpc-scandinavia/RpcCoreShared/blob/master/RpcCoreSharedPublic/Miscelenious/Guid.cs)
This can generate custom GUIDs, where the different parts are used to store integer values in human-readable form (not hexadecimal).
There are 5 different parts in a GUID `AAAAAAAA-BBBB-CCCC-DDDD-EEEEEEEEEEEE`, and this can store values in **A** and **E**.

Value stored in **A** is called group identifier, and can store a number between 0 and 99.999.999.
Value stored in **E** is called number identifier, and can store a number between 0 and 999.999.999.999.

## RpcSimpleLocalCache<TKey, TValue> [🔗](https://github.com/rpc-scandinavia/RpcCoreShared/blob/master/RpcCoreSharedPublic/Miscelenious/SimpleLocalCache.cs) and RpcSimpleStaticCache<TIsolation, TKey, TValue> [🔗](https://github.com/rpc-scandinavia/RpcCoreShared/blob/master/RpcCoreSharedPublic/Miscelenious/SimpleStaticCache.cs)
Simple in-memory cache, that store cached values in a dictionary. When the cache do not contain the requested value, 
the provider delegate specified in the constructor is called to get the requested value, which is added to the cache 
and returned.

A `SemaphoreSlim` is used for thread safety. The difference between the two, are that one is a normal class and the
other is a static class, which allow access to the cache from everywhere within the application. 

## Triple [🔗](https://github.com/rpc-scandinavia/RpcCoreShared/blob/master/RpcCoreSharedPublic/Structs/Triple.cs)
RPC Triple is a struct with three states, `True`, `False` and `Unknown`.
