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





	[TestMethod]
	public void TestMemoryCharReadOnlySplits() {
		// Prepare.
		ReadOnlyMemory<Char> strEmpty = ReadOnlyMemory<Char>.Empty;
		ReadOnlyMemory<Char> strTextA = "|qwerty| is||the| keyboard|.".AsMemory();
		ReadOnlyMemory<Char> strTextB = "qwerty is the keyboard.".AsMemory();
		ReadOnlyMemory<Char> strWhiteSpaces = "   ".AsMemory();
		ReadOnlyMemory<Char> strSeparator = "|".AsMemory();

		// Assert.
		Assert.AreEqual(0, strEmpty.Slices(strSeparator, StringSplitOptions.None).Length);

		Assert.AreEqual(7, strTextA.Slices(strSeparator, StringSplitOptions.None).Length);
		Assert.AreEqual("", strTextA.Slices(strSeparator, StringSplitOptions.None)[0].ToString());
		Assert.AreEqual("qwerty", strTextA.Slices(strSeparator, StringSplitOptions.None)[1].ToString());
		Assert.AreEqual(" is", strTextA.Slices(strSeparator, StringSplitOptions.None)[2].ToString());
		Assert.AreEqual("", strTextA.Slices(strSeparator, StringSplitOptions.None)[3].ToString());
		Assert.AreEqual("the", strTextA.Slices(strSeparator, StringSplitOptions.None)[4].ToString());
		Assert.AreEqual(" keyboard", strTextA.Slices(strSeparator, StringSplitOptions.None)[5].ToString());
		Assert.AreEqual(".", strTextA.Slices(strSeparator, StringSplitOptions.None)[6].ToString());

		Assert.AreEqual(5, strTextA.Slices(strSeparator, StringSplitOptions.RemoveEmptyEntries).Length);
		Assert.AreEqual("qwerty", strTextA.Slices(strSeparator, StringSplitOptions.RemoveEmptyEntries)[0].ToString());
		Assert.AreEqual(" is", strTextA.Slices(strSeparator, StringSplitOptions.RemoveEmptyEntries)[1].ToString());
		Assert.AreEqual("the", strTextA.Slices(strSeparator, StringSplitOptions.RemoveEmptyEntries)[2].ToString());
		Assert.AreEqual(" keyboard", strTextA.Slices(strSeparator, StringSplitOptions.RemoveEmptyEntries)[3].ToString());
		Assert.AreEqual(".", strTextA.Slices(strSeparator, StringSplitOptions.RemoveEmptyEntries)[4].ToString());

		Assert.AreEqual(5, strTextA.Slices(strSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Length);
		Assert.AreEqual("qwerty", strTextA.Slices(strSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[0].ToString());
		Assert.AreEqual("is", strTextA.Slices(strSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[1].ToString());
		Assert.AreEqual("the", strTextA.Slices(strSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[2].ToString());
		Assert.AreEqual("keyboard", strTextA.Slices(strSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[3].ToString());
		Assert.AreEqual(".", strTextA.Slices(strSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[4].ToString());

		Assert.AreEqual(1, strTextB.Slices(strSeparator, StringSplitOptions.None).Length);

		Assert.AreEqual(1, strWhiteSpaces.Slices(strSeparator, StringSplitOptions.None).Length);
		Assert.AreEqual(1, strWhiteSpaces.Slices(strSeparator, StringSplitOptions.RemoveEmptyEntries).Length);
		Assert.AreEqual(0, strWhiteSpaces.Slices(strSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Length);

		Assert.AreEqual(2, strSeparator.Slices(strSeparator, StringSplitOptions.None).Length);
		Assert.AreEqual(0, strSeparator.Slices(strSeparator, StringSplitOptions.RemoveEmptyEntries).Length);

	} // TestMemoryCharReadOnlySplits

} // RpcMemoryCharExtensionsTests
