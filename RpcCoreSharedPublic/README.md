[![.NET](https://github.com/rpc-scandinavia/RpcCoreShared/actions/workflows/dotnet.yml/badge.svg)](https://github.com/rpc-scandinavia/RpcCoreShared/actions/workflows/dotnet.yml)
[![GitHub](https://img.shields.io/github/license/rpc-scandinavia/RpcCoreShared?logo=github)](https://github.com/rpc-scandinavia/RpcCoreShared/blob/master/LICENSE)

# RpcCoreShared
RPC Core Shared contains interfaces and common classes used in most applications from RPC Scandinavia.

## Extensions
RPC Core Extensions is my static methods, for extending all sort of types.
Currently there are extension methods for:

* Memory<Char>
* Span<Char>
* String
* StringBuilder
* Type

Note that I have a lot of old extension methods, and I will add those not available in Linq.

## SimpleLocalCache [🔗](https://github.com/rpc-scandinavia/RpcCoreShared/blob/master/RpcCoreSharedPublic/Miscelenious/SimpleLocalCache.cs) and SimpleStaticCache [🔗](https://github.com/rpc-scandinavia/RpcCoreShared/blob/master/RpcCoreSharedPublic/Miscelenious/SimpleStaticCache.cs)
Simple in-memory cache, that store cached values in a dictionary. When the cache do not contain the requested value, 
the provider delegate specified in the constructor in called to get the requested value, which is added to the cache 
and returned.

A `SemaphoreSlim` is used for thread safety. The difference between the two, are that one is a static class, and the 
other is a normal class. 

## Triple [🔗](https://github.com/rpc-scandinavia/RpcCoreShared/blob/master/RpcCoreSharedPublic/Structs/Triple.cs)

RPC Triple is a struct with three states, `True`, `False` and `Unknown`.
