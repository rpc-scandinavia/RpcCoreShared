using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RpcScandinavia.Core.Shared;
namespace RpcScandinavia.Core.Shared.Tests;

[TestClass]
public class RpcStringBuilderExtensionsTests {

	[TestMethod]
	public void Test() {
		String pre = "Two|Three|Four";
		String post = "One|Two|Three|Four";
		StringBuilder sb = new StringBuilder(pre);
		sb.PrependDelimiter("One", "|");
		Assert.AreEqual(post, sb.ToString());

		pre = "Two|Three|Four";
		post = "Two|Three|Four|Five";
		sb = new StringBuilder(pre);
		sb.AppendDelimiter("Five", "|");
		Assert.AreEqual(post, sb.ToString());

	} // Test

} // RpcStringBuilderExtensionsTests
