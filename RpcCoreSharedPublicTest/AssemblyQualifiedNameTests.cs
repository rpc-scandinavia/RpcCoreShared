using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RpcScandinavia.Core.Shared;
namespace RpcScandinavia.Core.Shared.Tests;

[TestClass]
public class RpcAssemblyQualifiedNameTests {

	[TestMethod]
	public void TestAssemblyQualifiedNameWithNormalType() {
		// Assert Assembly Qualified Name from "guidEmpty" object.
		Guid obj = Guid.Empty;
		RpcAssemblyQualifiedName aqn = new RpcAssemblyQualifiedName(obj);

		Assert.AreEqual("System.Guid", aqn.TypeString);
		Assert.AreEqual("System.Private.CoreLib", aqn.AssemblyString);
		Assert.AreEqual("8.0.0.0", aqn.VersionString);
		Assert.AreEqual("neutral", aqn.CultureString);
		Assert.AreEqual("7cec85d7bea7798e", aqn.PublicKeyTokenString);
		Assert.AreEqual(false, aqn.IsArray);
		Assert.AreEqual(0, aqn.ArrayDimentions);
		Assert.AreEqual(false, aqn.IsGeneric);
		Assert.AreEqual(obj.GetType(), aqn.Type);
		Assert.AreEqual(obj.GetType().Assembly, aqn.Type.Assembly);
		Assert.AreEqual(obj.GetType().Assembly.GetName().Version, aqn.Version);

		// Assert Assembly Qualified Name from string with all parts.
		aqn = new RpcAssemblyQualifiedName("System.Guid, System.Private.CoreLib, Version=1.2.3, Culture=neutral, PublicKeyToken=7cec85d7bea7798e");

		Assert.AreEqual("System.Guid", aqn.TypeString);
		Assert.AreEqual("System.Private.CoreLib", aqn.AssemblyString);
		Assert.AreEqual("1.2.3", aqn.VersionString);
		Assert.AreEqual("neutral", aqn.CultureString);
		Assert.AreEqual("7cec85d7bea7798e", aqn.PublicKeyTokenString);
//		Assert.IsNull(aqn.Type);

		// Assert Assembly Qualified Name from string with missing optional parts.
		aqn = new RpcAssemblyQualifiedName("System.Guid, System.Private.CoreLib, Version=1.2.3, Culture=neutral");

		Assert.AreEqual("System.Guid", aqn.TypeString);
		Assert.AreEqual("System.Private.CoreLib", aqn.AssemblyString);
		Assert.AreEqual("1.2.3", aqn.VersionString);
		Assert.AreEqual("neutral", aqn.CultureString);
		Assert.AreEqual(String.Empty, aqn.PublicKeyTokenString);
		Assert.IsNull(aqn.Type);

		// Assert Assembly Qualified Name from string with missing optional parts.
		aqn = new RpcAssemblyQualifiedName("System.Guid, System.Private.CoreLib, Version=1.2.3");

		Assert.AreEqual("System.Guid", aqn.TypeString);
		Assert.AreEqual("System.Private.CoreLib", aqn.AssemblyString);
		Assert.AreEqual("1.2.3", aqn.VersionString);
		Assert.AreEqual(String.Empty, aqn.CultureString);
		Assert.AreEqual(String.Empty, aqn.PublicKeyTokenString);
		Assert.IsNull(aqn.Type);

		// Assert Assembly Qualified Name from string with missing optional parts.
		aqn = new RpcAssemblyQualifiedName("System.Guid, System.Private.CoreLib");

		Assert.AreEqual("System.Guid", aqn.TypeString);
		Assert.AreEqual("System.Private.CoreLib", aqn.AssemblyString);
		Assert.AreEqual(String.Empty, aqn.VersionString);
		Assert.AreEqual(String.Empty, aqn.CultureString);
		Assert.AreEqual(String.Empty, aqn.PublicKeyTokenString);
		Assert.IsNull(aqn.Type);

		// Assert Assembly Qualified Name from string with missing assembly name.
		aqn = new RpcAssemblyQualifiedName("System.Guid");

		Assert.AreEqual("System.Guid", aqn.TypeString);
		Assert.AreEqual(String.Empty, aqn.AssemblyString);
		Assert.AreEqual(String.Empty, aqn.VersionString);
		Assert.AreEqual(String.Empty, aqn.CultureString);
		Assert.AreEqual(String.Empty, aqn.PublicKeyTokenString);
		Assert.IsNull(aqn.Type);

		// Assert Assembly Qualified Name from empty string.
		aqn = new RpcAssemblyQualifiedName("");

		Assert.AreEqual(String.Empty, aqn.TypeString);
		Assert.AreEqual(String.Empty, aqn.AssemblyString);
		Assert.AreEqual(String.Empty, aqn.VersionString);
		Assert.AreEqual(String.Empty, aqn.CultureString);
		Assert.AreEqual(String.Empty, aqn.PublicKeyTokenString);
		Assert.AreEqual(null, aqn.Type);
		Assert.AreEqual(true, aqn.IsEmpty);
	} // TestAssemblyQualifiedNameWithNormalType

	[TestMethod]
	public void TestAssemblyQualifiedNameWithArrayType() {
		// Assert Assembly Qualified Name from one dimmentionel array.
		String[] obj1 = new String[0];
		RpcAssemblyQualifiedName aqn = new RpcAssemblyQualifiedName(obj1);

		Assert.AreEqual("System.String", aqn.TypeString);
		Assert.AreEqual("System.Private.CoreLib", aqn.AssemblyString);
		Assert.AreEqual("8.0.0.0", aqn.VersionString);
		Assert.AreEqual("neutral", aqn.CultureString);
		Assert.AreEqual("7cec85d7bea7798e", aqn.PublicKeyTokenString);
		Assert.AreEqual(true, aqn.IsArray);
		Assert.AreEqual(1, aqn.ArrayDimentions);
		Assert.AreEqual(false, aqn.IsGeneric);
		Assert.AreEqual(obj1.GetType(), aqn.Type);

		// Assert Assembly Qualified Name from two dimmentionel array.
		String[,] obj2 = new String[0, 0];
		aqn = new RpcAssemblyQualifiedName(obj2);

		Assert.AreEqual("System.String", aqn.TypeString);
		Assert.AreEqual("System.Private.CoreLib", aqn.AssemblyString);
		Assert.AreEqual("8.0.0.0", aqn.VersionString);
		Assert.AreEqual("neutral", aqn.CultureString);
		Assert.AreEqual("7cec85d7bea7798e", aqn.PublicKeyTokenString);
		Assert.AreEqual(true, aqn.IsArray);
		Assert.AreEqual(2, aqn.ArrayDimentions);
		Assert.AreEqual(false, aqn.IsGeneric);
		Assert.AreEqual(obj2.GetType(), aqn.Type);

		// Assert Assembly Qualified Name from tree dimmentionel array.
		String[,,] obj3 = new String[0, 1, 2];
		aqn = new RpcAssemblyQualifiedName(obj3);

		Assert.AreEqual("System.String", aqn.TypeString);
		Assert.AreEqual("System.Private.CoreLib", aqn.AssemblyString);
		Assert.AreEqual("8.0.0.0", aqn.VersionString);
		Assert.AreEqual("neutral", aqn.CultureString);
		Assert.AreEqual("7cec85d7bea7798e", aqn.PublicKeyTokenString);
		Assert.AreEqual(true, aqn.IsArray);
		Assert.AreEqual(3, aqn.ArrayDimentions);
		Assert.AreEqual(false, aqn.IsGeneric);
		Assert.AreEqual(obj3.GetType(), aqn.Type);
	} // TestAssemblyQualifiedNameWithArrayType

	[TestMethod]
	public void TestAssemblyQualifiedNameWithGenericType() {
		// Assert Assembly Qualified Name from generic type with two type arguments.
		KeyValuePair<Int32, String> obj1 = new KeyValuePair<Int32, String>(0, String.Empty);
		RpcAssemblyQualifiedName aqn = new RpcAssemblyQualifiedName(obj1);

		Assert.AreEqual("System.Collections.Generic.KeyValuePair", aqn.TypeString);
		Assert.AreEqual("System.Private.CoreLib", aqn.AssemblyString);
		Assert.AreEqual("8.0.0.0", aqn.VersionString);
		Assert.AreEqual("neutral", aqn.CultureString);
		Assert.AreEqual("7cec85d7bea7798e", aqn.PublicKeyTokenString);
		Assert.AreEqual(false, aqn.IsArray);
		Assert.AreEqual(0, aqn.ArrayDimentions);
		Assert.AreEqual(true, aqn.IsGeneric);
		Assert.AreEqual(2, aqn.GenericTypeArgumentCount);
		Assert.AreEqual(obj1.GetType(), aqn.Type);

		Assert.AreEqual("System.Int32", aqn.GenericTypeArguments[0].TypeString);
		Assert.AreEqual("System.Private.CoreLib", aqn.GenericTypeArguments[0].AssemblyString);
		Assert.AreEqual("8.0.0.0", aqn.GenericTypeArguments[0].VersionString);
		Assert.AreEqual("neutral", aqn.GenericTypeArguments[0].CultureString);
		Assert.AreEqual("7cec85d7bea7798e", aqn.GenericTypeArguments[0].PublicKeyTokenString);
		Assert.AreEqual(false, aqn.GenericTypeArguments[0].IsArray);
		Assert.AreEqual(false, aqn.GenericTypeArguments[0].IsGeneric);

		Assert.AreEqual("System.String", aqn.GenericTypeArguments[1].TypeString);
		Assert.AreEqual("System.Private.CoreLib", aqn.GenericTypeArguments[1].AssemblyString);
		Assert.AreEqual("8.0.0.0", aqn.GenericTypeArguments[1].VersionString);
		Assert.AreEqual("neutral", aqn.GenericTypeArguments[1].CultureString);
		Assert.AreEqual("7cec85d7bea7798e", aqn.GenericTypeArguments[1].PublicKeyTokenString);
		Assert.AreEqual(false, aqn.GenericTypeArguments[1].IsArray);
		Assert.AreEqual(false, aqn.GenericTypeArguments[1].IsGeneric);

		// Assert Assembly Qualified Name from generic list of generic type with two type arguments.
		List<KeyValuePair<Guid, DateTime>> obj2 = new List<KeyValuePair<Guid, DateTime>>();
		aqn = new RpcAssemblyQualifiedName(obj2);

		Assert.AreEqual("System.Collections.Generic.List", aqn.TypeString);
		Assert.AreEqual("System.Private.CoreLib", aqn.AssemblyString);
		Assert.AreEqual("8.0.0.0", aqn.VersionString);
		Assert.AreEqual("neutral", aqn.CultureString);
		Assert.AreEqual("7cec85d7bea7798e", aqn.PublicKeyTokenString);
		Assert.AreEqual(false, aqn.IsArray);
		Assert.AreEqual(0, aqn.ArrayDimentions);
		Assert.AreEqual(true, aqn.IsGeneric);
		Assert.AreEqual(1, aqn.GenericTypeArgumentCount);
		Assert.AreEqual(obj2.GetType(), aqn.Type);

	} // TestAssemblyQualifiedNameWithGenericType

	[TestMethod]
	public void TestAssemblyQualifiedNameEquals() {
		RpcAssemblyQualifiedName aqn1 = new RpcAssemblyQualifiedName("System.Guid, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e");
		RpcAssemblyQualifiedName aqn2 = new RpcAssemblyQualifiedName("System.Guid, System.Private.CoreLib, Version=8.0.0.0, Culture=nb_NO, PublicKeyToken=7cec85d7bea7798e");
		RpcAssemblyQualifiedName aqn3 = new RpcAssemblyQualifiedName("System.Guid, System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=FALSE");
		RpcAssemblyQualifiedName aqn4 = new RpcAssemblyQualifiedName("System.Guid, System.Private.CoreLib, Version=1.2.3, Culture=neutral, PublicKeyToken=7cec85d7bea7798e");

		// Normal.
		Assert.AreNotEqual(aqn1, aqn2);
		Assert.AreNotEqual(aqn1, aqn3);
		Assert.AreNotEqual(aqn1, aqn4);

		// Ignore the version, culture and public key.
		Assert.AreEqual(aqn1, aqn2, new RpcAssemblyQualifiedNameEqualityComparer(true, true, true));
		Assert.AreEqual(aqn1, aqn3, new RpcAssemblyQualifiedNameEqualityComparer(true, true, true));
		Assert.AreEqual(aqn1, aqn4, new RpcAssemblyQualifiedNameEqualityComparer(true, true, true));

	} // TestAssemblyQualifiedNameEquals

} // RpcAssemblyQualifiedNameTests
