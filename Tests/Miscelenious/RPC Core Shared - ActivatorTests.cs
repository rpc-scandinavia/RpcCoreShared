namespace RpcScandinavia.Core.Shared.Tests.Miscelenious;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RpcScandinavia.Core.Shared;

[TestClass]
public class RpcActivatorTests {

	public RpcActivatorTests() {
	} // RpcActivatorTests

	[TestMethod]
	public void TestActivator() {
		// Prepare.
		Guid guidEmpty = Guid.Empty;
		String guidEmptyAqn1 = "System.Guid, System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e";
		String guidEmptyAqn2 = guidEmpty.GetType().AssemblyQualifiedName;

		String[] stringArrayOne = new String[0];
		String stringArrayOneAqn1 = "System.String[], System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e";
		String stringArrayOneAqn2 = stringArrayOne.GetType().AssemblyQualifiedName;

		String[,] stringArrayTwo = new String[0, 0];
		String stringArrayTwoAqn1 = "System.String[,], System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e";
		String stringArrayTwoAqn2 = stringArrayTwo.GetType().AssemblyQualifiedName;

		DataGeneric<Int32>[] genericArray = new DataGeneric<Int32>[0];
		String genericArrayAqn1 = "RpcScandinavia.Core.Shared.Tests.Miscelenious.DataGeneric`1[[System.Int32, System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]][], RpcCoreShared.test, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";
		String genericArrayAqn2 = genericArray.GetType().AssemblyQualifiedName;

		DataGeneric<String> genericOne = new DataGeneric<String>("One");
		String genericOneAqn1 = "RpcScandinavia.Core.Shared.Tests.Miscelenious.DataGeneric`1[[System.String, System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]], RpcCoreShared.test, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";
		String genericOneAqn2 = genericOne.GetType().AssemblyQualifiedName;

		Dictionary<String, KeyValuePair<Int32, String>> genericTwo = new Dictionary<String, KeyValuePair<Int32, String>>();
		String genericTwoAqn1 = "System.Collections.Generic.Dictionary`2[[System.String, System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e],[System.Collections.Generic.KeyValuePair`2[[System.Int32, System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e],[System.String, System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]], System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]], System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e";
		String genericTwoAqn2 = genericTwo.GetType().AssemblyQualifiedName;

		// Assert.
		Assert.AreEqual(guidEmptyAqn1, guidEmptyAqn2);

		Assert.AreEqual(stringArrayOneAqn1, stringArrayOneAqn2);

		Assert.AreEqual(stringArrayTwoAqn1, stringArrayTwoAqn2);

		Assert.AreEqual(genericArrayAqn1, genericArrayAqn2);

		Assert.AreEqual(genericOneAqn1, genericOneAqn2);

		Assert.AreEqual(genericTwoAqn1, genericTwoAqn2);

	} // TestActivator

} // RpcActivatorTests

public class DataGeneric<T> : IEqualityComparer<DataGeneric<T>> {
	public T Value { get; set; }

	public DataGeneric() {
		this.Value = default(T);
	} // DataGeneric

	public DataGeneric(T value) {
		this.Value = value;
	} // DataGeneric

	public override Boolean Equals(Object obj) {
		return (obj != null) ? this.Equals(this, (obj as DataGeneric<T>)) : false;
	} // Equals

	public override int GetHashCode() {
		return this.GetHashCode(this);
	} // GetHashCode

	public Boolean Equals(DataGeneric<T> valueA, DataGeneric<T> valueB) {
		return (valueA.Value.Equals(valueB.Value));
	} // CompareTo

	public Int32 GetHashCode(DataGeneric<T> value) {
		return value.Value.GetHashCode();
	} // GetHashCode

} // DataGeneric
