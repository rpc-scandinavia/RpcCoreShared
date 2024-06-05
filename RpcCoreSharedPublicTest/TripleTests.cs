using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RpcScandinavia.Core.Shared;
namespace RpcScandinavia.Core.Shared.Tests;

[TestClass]
public class RpcTripleTests {

	[TestMethod]
	public void Test() {
		// Prepare.
		String strEmpty = String.Empty;
		String strNull = null;
		String strText = "qwerty is the keyboard.";
		String strWhiteSpaces = "   ";

		// Assert.
		Assert.IsFalse(strNull == Triple.True);
		Assert.IsFalse(strNull == Triple.False);
		Assert.IsTrue(strNull == Triple.Unknown);

		Assert.IsFalse(strEmpty == Triple.True);
		Assert.IsFalse(strEmpty == Triple.False);
		Assert.IsTrue(strEmpty == Triple.Unknown);

		Assert.IsFalse(strWhiteSpaces == Triple.True);
		Assert.IsFalse(strWhiteSpaces == Triple.False);
		Assert.IsTrue(strWhiteSpaces == Triple.Unknown);

		Assert.IsFalse(strText == Triple.True);
		Assert.IsFalse(strText == Triple.False);
		Assert.IsTrue(strText == Triple.Unknown);

		Assert.IsTrue("true" == Triple.True);
		Assert.IsTrue("trUe" == Triple.True);
		Assert.IsFalse("trUe" == Triple.False);
		Assert.IsFalse("false" == Triple.Unknown);

		Assert.IsFalse("true" != Triple.True);
		Assert.IsFalse("trUe" != Triple.True);
		Assert.IsTrue("trUe" != Triple.False);
		Assert.IsTrue("false" != Triple.Unknown);

		Assert.IsFalse(0 == Triple.True);
		Assert.IsTrue(0 == Triple.False);
		Assert.IsFalse(0 == Triple.Unknown);

		Assert.IsTrue(1 == Triple.True);
		Assert.IsFalse(1 == Triple.False);
		Assert.IsFalse(1 == Triple.Unknown);

		Assert.IsFalse(2 == Triple.True);
		Assert.IsFalse(2 == Triple.False);
		Assert.IsTrue(2 == Triple.Unknown);

		Assert.IsFalse(Triple.True.Equals(0) == true);
		Assert.IsFalse(Triple.False.Equals(1) == true);
		Assert.IsTrue(Triple.Unknown.Equals(2) == true);

		Assert.IsFalse(Triple.True.Equals("true ") == true);
		Assert.IsTrue(Triple.False.Equals("fALSe") == true);
		Assert.IsFalse(Triple.Unknown.Equals("true") == true);

	} // Test

} // RpcStringExtensionsTests
