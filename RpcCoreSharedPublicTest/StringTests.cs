using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RpcScandinavia.Core.Shared;
namespace RpcScandinavia.Core.Shared.Tests;

[TestClass]
public class RpcStringExtensionsTests {

	[TestMethod]
	public void TestIs() {
		// Prepare.
		String strEmpty = String.Empty;
		String strNull = null;
		String strText = "qwerty is the keyboard.";
		String strWhiteSpaces = "   ";

		// Assert.
		Assert.IsTrue(strEmpty.IsEmpty());
		Assert.IsFalse(strWhiteSpaces.IsEmpty());
		Assert.IsTrue(strNull.IsEmpty());
		Assert.ThrowsException<ArgumentNullException>(() => strNull.IsEmpty(true));

		Assert.IsTrue(strEmpty.IsNullOrEmpty());
		Assert.IsTrue(strNull.IsNullOrEmpty());
		Assert.IsFalse(strText.IsNullOrEmpty());
		Assert.IsFalse(strWhiteSpaces.IsNullOrEmpty());

		Assert.IsTrue(strEmpty.IsNullOrWhiteSpace());
		Assert.IsTrue(strNull.IsNullOrWhiteSpace());
		Assert.IsFalse(strText.IsNullOrWhiteSpace());
		Assert.IsTrue(strWhiteSpaces.IsNullOrWhiteSpace());

	} // TestIs

	[TestMethod]
	public void TestNot() {
		// Prepare.
		String strEmpty = String.Empty;
		String strNull = null;
		String strText = "qwerty is the keyboard.";
		String strDefault = "summer is the best time of year.";
		String strWhiteSpaces = "   ";

		// Assert.
		Assert.AreEqual(strEmpty, strEmpty.NotNull());
		Assert.AreEqual(strEmpty, strNull.NotNull());
		Assert.AreEqual(strText, strText.NotNull());
		Assert.AreEqual(strWhiteSpaces, strWhiteSpaces.NotNull());

		Assert.AreEqual(strEmpty, strEmpty.NotNull(strDefault));
		Assert.AreEqual(strDefault, strNull.NotNull(strDefault));
		Assert.AreEqual(strText, strText.NotNull(strDefault));
		Assert.AreEqual(strWhiteSpaces, strWhiteSpaces.NotNull(strDefault));

		Assert.AreEqual(strDefault, strEmpty.NotNullOrEmpty(strDefault));
		Assert.AreEqual(strDefault, strNull.NotNullOrEmpty(strDefault));
		Assert.AreEqual(strText, strText.NotNullOrEmpty(strDefault));
		Assert.AreEqual(strWhiteSpaces, strWhiteSpaces.NotNullOrEmpty(strDefault));

		Assert.AreEqual(strDefault, strEmpty.NotNullOrWhiteSpace(strDefault));
		Assert.AreEqual(strDefault, strNull.NotNullOrWhiteSpace(strDefault));
		Assert.AreEqual(strText, strText.NotNullOrWhiteSpace(strDefault));
		Assert.AreEqual(strDefault, strWhiteSpaces.NotNullOrWhiteSpace(strDefault));

	} // TestNot

	[TestMethod]
	public void TestParsable() {

		Assert.AreEqual(12345, "12345".ToParsable<Int16>());
		Assert.AreEqual(0, "1234567890".ToParsable<Int16>());
		Assert.AreEqual(25, "1234567890".ToParsable<Int16>(25));
		Assert.ThrowsException<ArgumentException>(() => "1234567890".ToParsableOrThrow<Int16>());

//ArgumentException
	} // TestParsable

} // RpcStringExtensionsTests
