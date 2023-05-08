namespace RpcScandinavia.Core.Shared.Tests.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RpcScandinavia.Core.Shared;

[TestClass]
public class RpcMemoryCharExtensionsTests {

	public RpcMemoryCharExtensionsTests() {
	} // RpcMemoryCharExtensionsTests

	[TestMethod]
	public void TestMemoryCharIs() {
		// Prepare.
		Memory<Char> strEmpty = Memory<Char>.Empty;
		Memory<Char> strText = "qwerty is the keyboard.".AsMemory().ToArray();
		Memory<Char> strWhiteSpaces = "   ".AsMemory().ToArray();

		// Assert.
		Assert.IsTrue(strEmpty.IsEmptyOrWhiteSpace());
		Assert.IsFalse(strText.IsEmptyOrWhiteSpace());
		Assert.IsTrue(strWhiteSpaces.IsEmptyOrWhiteSpace());

		Assert.IsTrue(strEmpty.Span.IsEmptyOrWhiteSpace());
		Assert.IsFalse(strText.Span.IsEmptyOrWhiteSpace());
		Assert.IsTrue(strWhiteSpaces.Span.IsEmptyOrWhiteSpace());

	} // TestMemoryCharIs

	[TestMethod]
	public void TestMemoryCharReadOnlyIs() {
		// Prepare.
		ReadOnlyMemory<Char> strEmpty = ReadOnlyMemory<Char>.Empty;
		ReadOnlyMemory<Char> strText = "qwerty is the keyboard.".AsMemory();
		ReadOnlyMemory<Char> strWhiteSpaces = "   ".AsMemory();

		// Assert.
		Assert.IsTrue(strEmpty.IsEmptyOrWhiteSpace());
		Assert.IsFalse(strText.IsEmptyOrWhiteSpace());
		Assert.IsTrue(strWhiteSpaces.IsEmptyOrWhiteSpace());

		Assert.IsTrue(strEmpty.Span.IsEmptyOrWhiteSpace());
		Assert.IsFalse(strText.Span.IsEmptyOrWhiteSpace());
		Assert.IsTrue(strWhiteSpaces.Span.IsEmptyOrWhiteSpace());

	} // TestMemoryCharReadOnlyIs

} // RpcMemoryCharExtensionsTests
