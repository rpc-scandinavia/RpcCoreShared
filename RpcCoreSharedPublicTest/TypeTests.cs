using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RpcScandinavia.Core.Shared;
namespace RpcScandinavia.Core.Shared.Tests;

[TestClass]
public class RpcTypeExtensionsTests {

	[TestMethod]
	public void Test() {
		// Prepare.
		Type enumType = (DayOfWeek.Friday).GetType();
		Type enumFlagsType = (BindingFlags.Public | BindingFlags.Default).GetType();
		Type dictionaryType = new Dictionary<Int32, String>().GetType();
		Type listType = new List<String>().GetType();
		Type arrayType = Array.Empty<String>().GetType();

		// Assert.
		Assert.IsTrue(enumType.IsEnum);
		Assert.IsFalse(enumType.IsEnumWithFlags());
		Assert.IsFalse(enumType.IsGenericDictionary());
		Assert.IsFalse(enumType.IsGenericList());
		Assert.IsFalse(enumType.IsArray());

		Assert.IsTrue(enumFlagsType.IsEnum);
		Assert.IsTrue(enumFlagsType.IsEnumWithFlags());
		Assert.IsFalse(enumFlagsType.IsGenericDictionary());
		Assert.IsFalse(enumFlagsType.IsGenericList());
		Assert.IsFalse(enumFlagsType.IsArray());

		Assert.IsFalse(dictionaryType.IsEnum);
		Assert.IsFalse(dictionaryType.IsEnumWithFlags());
		Assert.IsTrue(dictionaryType.IsGenericDictionary());
		Assert.IsFalse(dictionaryType.IsGenericList());
		Assert.IsFalse(dictionaryType.IsArray());

		Assert.IsFalse(listType.IsEnum);
		Assert.IsFalse(listType.IsEnumWithFlags());
		Assert.IsFalse(listType.IsGenericDictionary());
		Assert.IsTrue(listType.IsGenericList());
		Assert.IsFalse(listType.IsArray());

		Assert.IsFalse(arrayType.IsEnum);
		Assert.IsFalse(arrayType.IsEnumWithFlags());
		Assert.IsFalse(arrayType.IsGenericDictionary());
		Assert.IsFalse(arrayType.IsGenericList());
		Assert.IsTrue(arrayType.IsArray());


	} // Test

} // RpcTypeExtensionsTests
