using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RpcScandinavia.Core.Shared;
namespace RpcScandinavia.Core.Shared.Tests;

[TestClass]
public class RpcMemoryCharExtensionsTests {

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
	public void TestMemoryCharSplits() {
		// Prepare.
		Memory<Char> strEmpty = Memory<Char>.Empty;
		Memory<Char> strTextA = new Memory<Char>("|qwerty| is||the| keyboard|.".ToArray());
		Memory<Char> strTextB = new Memory<Char>("qwerty is the keyboard.".ToArray());
		Memory<Char> strWhiteSpaces = new Memory<Char>("   ".ToArray());
		Memory<Char> strSeparator = new Memory<Char>("|".ToArray());

		// Assert.
		// The SliceToArray methods use the Slice methods, which returns the IEnumerable.
		Assert.ThrowsException<ArgumentException>(() => strTextA.SliceToArray(strEmpty, StringSplitOptions.None));
		Assert.AreEqual(0, strEmpty.SliceToArray(strSeparator, StringSplitOptions.None).Length);

		Assert.AreEqual(7, strTextA.SliceToArray(strSeparator, StringSplitOptions.None).Length);
		Assert.AreEqual("", strTextA.SliceToArray(strSeparator, StringSplitOptions.None)[0].ToString());
		Assert.AreEqual("qwerty", strTextA.SliceToArray(strSeparator, StringSplitOptions.None)[1].ToString());
		Assert.AreEqual(" is", strTextA.SliceToArray(strSeparator, StringSplitOptions.None)[2].ToString());
		Assert.AreEqual("", strTextA.SliceToArray(strSeparator, StringSplitOptions.None)[3].ToString());
		Assert.AreEqual("the", strTextA.SliceToArray(strSeparator, StringSplitOptions.None)[4].ToString());
		Assert.AreEqual(" keyboard", strTextA.SliceToArray(strSeparator, StringSplitOptions.None)[5].ToString());
		Assert.AreEqual(".", strTextA.SliceToArray(strSeparator, StringSplitOptions.None)[6].ToString());

		Assert.AreEqual(5, strTextA.SliceToArray(strSeparator, StringSplitOptions.RemoveEmptyEntries).Length);
		Assert.AreEqual("qwerty", strTextA.SliceToArray(strSeparator, StringSplitOptions.RemoveEmptyEntries)[0].ToString());
		Assert.AreEqual(" is", strTextA.SliceToArray(strSeparator, StringSplitOptions.RemoveEmptyEntries)[1].ToString());
		Assert.AreEqual("the", strTextA.SliceToArray(strSeparator, StringSplitOptions.RemoveEmptyEntries)[2].ToString());
		Assert.AreEqual(" keyboard", strTextA.SliceToArray(strSeparator, StringSplitOptions.RemoveEmptyEntries)[3].ToString());
		Assert.AreEqual(".", strTextA.SliceToArray(strSeparator, StringSplitOptions.RemoveEmptyEntries)[4].ToString());

		Assert.AreEqual(5, strTextA.SliceToArray(strSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Length);
		Assert.AreEqual("qwerty", strTextA.SliceToArray(strSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[0].ToString());
		Assert.AreEqual("is", strTextA.SliceToArray(strSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[1].ToString());
		Assert.AreEqual("the", strTextA.SliceToArray(strSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[2].ToString());
		Assert.AreEqual("keyboard", strTextA.SliceToArray(strSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[3].ToString());
		Assert.AreEqual(".", strTextA.SliceToArray(strSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[4].ToString());

		Assert.AreEqual(1, strTextB.SliceToArray(strSeparator, StringSplitOptions.None).Length);

		Assert.AreEqual(1, strWhiteSpaces.SliceToArray(strSeparator, StringSplitOptions.None).Length);
		Assert.AreEqual(1, strWhiteSpaces.SliceToArray(strSeparator, StringSplitOptions.RemoveEmptyEntries).Length);
		Assert.AreEqual(0, strWhiteSpaces.SliceToArray(strSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Length);

		Assert.AreEqual(2, strSeparator.SliceToArray(strSeparator, StringSplitOptions.None).Length);
		Assert.AreEqual(0, strSeparator.SliceToArray(strSeparator, StringSplitOptions.RemoveEmptyEntries).Length);

	} // TestMemoryCharSplits

	[TestMethod]
	public void TestMemoryCharReadOnlySplits() {
		// Prepare.
		ReadOnlyMemory<Char> strEmpty = ReadOnlyMemory<Char>.Empty;
		ReadOnlyMemory<Char> strTextA = "|qwerty| is||the| keyboard|.".AsMemory();
		ReadOnlyMemory<Char> strTextB = "qwerty is the keyboard.".AsMemory();
		ReadOnlyMemory<Char> strWhiteSpaces = "   ".AsMemory();
		ReadOnlyMemory<Char> strSeparator = "|".AsMemory();

		// Assert.
		// The SliceToArray methods use the Slice methods, which returns the IEnumerable.
		Assert.ThrowsException<ArgumentException>(() => strTextA.SliceToArray(strEmpty, StringSplitOptions.None));
		Assert.AreEqual(0, strEmpty.SliceToArray(strSeparator, StringSplitOptions.None).Length);

		Assert.AreEqual(7, strTextA.SliceToArray(strSeparator, StringSplitOptions.None).Length);
		Assert.AreEqual("", strTextA.SliceToArray(strSeparator, StringSplitOptions.None)[0].ToString());
		Assert.AreEqual("qwerty", strTextA.SliceToArray(strSeparator, StringSplitOptions.None)[1].ToString());
		Assert.AreEqual(" is", strTextA.SliceToArray(strSeparator, StringSplitOptions.None)[2].ToString());
		Assert.AreEqual("", strTextA.SliceToArray(strSeparator, StringSplitOptions.None)[3].ToString());
		Assert.AreEqual("the", strTextA.SliceToArray(strSeparator, StringSplitOptions.None)[4].ToString());
		Assert.AreEqual(" keyboard", strTextA.SliceToArray(strSeparator, StringSplitOptions.None)[5].ToString());
		Assert.AreEqual(".", strTextA.SliceToArray(strSeparator, StringSplitOptions.None)[6].ToString());

		Assert.AreEqual(5, strTextA.SliceToArray(strSeparator, StringSplitOptions.RemoveEmptyEntries).Length);
		Assert.AreEqual("qwerty", strTextA.SliceToArray(strSeparator, StringSplitOptions.RemoveEmptyEntries)[0].ToString());
		Assert.AreEqual(" is", strTextA.SliceToArray(strSeparator, StringSplitOptions.RemoveEmptyEntries)[1].ToString());
		Assert.AreEqual("the", strTextA.SliceToArray(strSeparator, StringSplitOptions.RemoveEmptyEntries)[2].ToString());
		Assert.AreEqual(" keyboard", strTextA.SliceToArray(strSeparator, StringSplitOptions.RemoveEmptyEntries)[3].ToString());
		Assert.AreEqual(".", strTextA.SliceToArray(strSeparator, StringSplitOptions.RemoveEmptyEntries)[4].ToString());

		Assert.AreEqual(5, strTextA.SliceToArray(strSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Length);
		Assert.AreEqual("qwerty", strTextA.SliceToArray(strSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[0].ToString());
		Assert.AreEqual("is", strTextA.SliceToArray(strSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[1].ToString());
		Assert.AreEqual("the", strTextA.SliceToArray(strSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[2].ToString());
		Assert.AreEqual("keyboard", strTextA.SliceToArray(strSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[3].ToString());
		Assert.AreEqual(".", strTextA.SliceToArray(strSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[4].ToString());

		Assert.AreEqual(1, strTextB.SliceToArray(strSeparator, StringSplitOptions.None).Length);

		Assert.AreEqual(1, strWhiteSpaces.SliceToArray(strSeparator, StringSplitOptions.None).Length);
		Assert.AreEqual(1, strWhiteSpaces.SliceToArray(strSeparator, StringSplitOptions.RemoveEmptyEntries).Length);
		Assert.AreEqual(0, strWhiteSpaces.SliceToArray(strSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Length);

		Assert.AreEqual(2, strSeparator.SliceToArray(strSeparator, StringSplitOptions.None).Length);
		Assert.AreEqual(0, strSeparator.SliceToArray(strSeparator, StringSplitOptions.RemoveEmptyEntries).Length);

	} // TestMemoryCharReadOnlySplits

} // RpcMemoryCharExtensionsTests
