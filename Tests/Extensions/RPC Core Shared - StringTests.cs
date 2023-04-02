namespace RpcScandinavia.Core.Shared.Tests.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RpcScandinavia.Core.Shared;

[TestClass]
public class RpcStringExtensionsTests {

	public RpcStringExtensionsTests() {
	} // RpcStringExtensionsTests

	[TestMethod]
	public void TestIs() {
		// Prepare.
		String strEmpty = String.Empty;
		String strNull = null;
		String strText = "qwerty is the keyboard.";
		String strWhiteSpaces = "   ";

		// Assert.
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

} // RpcStringExtensionsTests
