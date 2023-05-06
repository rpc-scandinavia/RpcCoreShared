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

	public Int32 Level { get; }

	public RpcKeyValueSerializerOptions Options { get; }

	public IRpcKeyValueBuilder AddLevel(String key);

	public void Add(String key, String value);

	public void AddTypeMetadata(String key, Type type);

} // IRpcKeyValueBuilder
#endregion
