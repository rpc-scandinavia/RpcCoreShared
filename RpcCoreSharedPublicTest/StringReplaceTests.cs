using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RpcScandinavia.Core.Shared;
namespace RpcScandinavia.Core.Shared.Tests;

[TestClass]
public class RpcStringReplaceExtensionsTests {

	[TestMethod]
	public void TestStringReplaceVariables() {
		// Prepare.
		Dictionary<String, String> variables = new Dictionary<String, String>(StringComparer.OrdinalIgnoreCase) {
			{ "FIRST-NAME", "Test Håkon" },
			{ "LAST_NAME", "Testesen" },
			{ "AGE", "48" },
			{ "EMPTY", "" },
			{ "NULL", null },
		};

		// Assert.
		String source1 = "The first name is $FIRST-NAME$ and the last name is $LAST_name$.";
		String result1 = "The first name is Test Håkon and the last name is Testesen.";
		Assert.AreEqual(result1, source1.ReplaceVariables(variables));

		// Assert.
		String source2 = "This should fail, when $EMPTY$ values are not allowed.";
		String result2 = "This should fail, when  values are not allowed.";
		Assert.AreNotEqual(result2, source2.ReplaceVariables(variables));
		Assert.AreEqual(result2, source2.ReplaceVariables(true, variables));

		// Assert.
		String source3 = "This should fail, when $NULL$ (empty) values are not allowed.";
		String result3 = "This should fail, when  (empty) values are not allowed.";
		Assert.AreNotEqual(result3, source3.ReplaceVariables(variables));
		Assert.AreEqual(result3, source3.ReplaceVariables(true, variables));

	} // TestStringReplaceVariables

} // RpcStringReplaceExtensionsTests
