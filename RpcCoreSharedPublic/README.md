[![.NET](https://github.com/rpc-scandinavia/RpcCoreShared/actions/workflows/dotnet.yml/badge.svg)](https://github.com/rpc-scandinavia/RpcCoreShared/actions/workflows/dotnet.yml)
[![GitHub](https://img.shields.io/github/license/rpc-scandinavia/RpcCoreShared?logo=github)](https://github.com/rpc-scandinavia/RpcCoreShared/blob/master/LICENSE)

# RpcCoreShared
RPC Core Shared contains interfaces and common classes used in most applications from RPC Scandinavia.

## RpcCoreExtensions
RPC Core Extensions is my static methods, for extending all sort of types.
Currently there are extension methods for:

* Memory<Char>
* Span<Char>
* String

Note that I have a lot of old extension methods, and I will add those not available in Linq.

## RpcTriple [🔗](https://github.com/rpc-scandinavia/RpcCoreShared/blob/master/RpcCoreSharedPublic/Structs/Triple.cs)

RPC Triple is a struct with three states, `True`, `False` and `Unknown`.
