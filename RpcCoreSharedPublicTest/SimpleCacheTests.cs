using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RpcScandinavia.Core.Shared;
namespace RpcScandinavia.Core.Shared.Tests;

[TestClass]
public class RpcSimpleCacheTests {

	[TestMethod]
	public void Test() {

		RpcSimpleLocalCache<Int32, String> cache = new RpcSimpleLocalCache<Int32, String>(RpcSimpleCacheNullValues.DenyNullValuesAndThrow, this.ProvideValue);
		String value = null;

		// Get value 2 from the provider.
		this.valueIsFromProvider = false;
		value = cache.GetValue(2);
		Assert.IsTrue(this.valueIsFromProvider);
		Assert.AreEqual(value, "Value Two");

		// Get value 2 from the cache.
		this.valueIsFromProvider = false;
		value = cache.GetValue(2);
		Assert.IsFalse(this.valueIsFromProvider);
		Assert.AreEqual(value, "Value Two");

		// Get value 9 throws exception, because null values are disallowed in the cache.
		this.valueIsFromProvider = false;
		Assert.ThrowsException<Exception>(() => cache.GetValue(9));

	} // Test

	private Boolean valueIsFromProvider = false;
	private String ProvideValue(Int32 key) {
		this.valueIsFromProvider = true;
		return key switch {
			1 => "Value One",
			2 => "Value Two",
			3 => "Value Three",
			_ => null,
		};
	} // ProvideValue


} // RpcSimpleCacheTests
