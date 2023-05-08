namespace RpcScandinavia.Core.Shared;
using System;
using System.Collections.Generic;

#region IRpcKeyValueBuilder
//----------------------------------------------------------------------------------------------------------------------
// IRpcKeyValueBuilder.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// The key/value builder collects keys and values during serialization.
/// </summary>
public interface IRpcKeyValueBuilder {

	public RpcKeyValueSerializerOptions Options { get; }

	public IRpcKeyValueBuilder AddLevel(ReadOnlyMemory<Char> key);

	public void Add(ReadOnlyMemory<Char> key, ReadOnlyMemory<Char> value);

	public void AddTypeMetadata(ReadOnlyMemory<Char> key, Type type);

} // IRpcKeyValueBuilder
#endregion
