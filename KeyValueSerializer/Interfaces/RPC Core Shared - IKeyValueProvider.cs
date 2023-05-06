namespace RpcScandinavia.Core.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using RpcScandinavia.Core;

#region IRpcKeyValueProvider
//----------------------------------------------------------------------------------------------------------------------
// IRpcKeyValueProvider.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// The key/value provider extracts keys and values from the key/value collection, during deserialization.
/// </summary>
public interface IRpcKeyValueProvider {

	public ReadOnlyMemory<Char> GetTypeMetadata();

	public ReadOnlyMemory<Char> GetValue();

	public Int32 Count();

	public Boolean ContainsKey(ReadOnlyMemory<Char> key);

	public IRpcKeyValueProvider GetKey(ReadOnlyMemory<Char> key);

	public IRpcKeyValueProvider[] GetKeys();

	public ReadOnlyMemory<Char> GetFirstKeyValueOrEmpty();

} // IRpcKeyValueProvider
#endregion
