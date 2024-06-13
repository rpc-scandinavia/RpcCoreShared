using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RpcScandinavia.Core.Shared;
namespace RpcScandinavia.Core.Shared.Tests;

[TestClass]
public class RpcActivatorTests {

	[TestMethod]
	public void TestActivator() {
		// Prepare.
		Guid guidEmpty = Guid.Empty;
		String guidEmptyTypeString = "System.Guid, System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e";
		Type guidEmptyType = RpcActivator.GetType(guidEmptyTypeString);

		String[] stringArrayOne = new String[0];
		String stringArrayOneTypeString = "System.String[], System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e";
		Type stringArrayOneType = RpcActivator.GetType(stringArrayOneTypeString);

		String[,] stringArrayTwo = new String[0, 0];
		String stringArrayTwoTypeString = "System.String[,], System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e";
		Type stringArrayTwoType = RpcActivator.GetType(stringArrayTwoTypeString);

		List<Int32>[] genericArray = new List<Int32>[0];
		String genericArrayTypeString = "System.Collections.Generic.List`1[[System.Int32, System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]][], System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e";
		Type genericArrayType = RpcActivator.GetType(genericArrayTypeString, true);

		DataGeneric<String> genericOne = new DataGeneric<String>("One");
		String genericOneTypeString = "RpcScandinavia.Core.Shared.Tests.DataGeneric`1[[System.String, System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]], RpcCoreSharedPublicTest, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";
		Type genericOneType = RpcActivator.GetType(genericOneTypeString);

		Dictionary<String, KeyValuePair<Int32, String>> genericTwo = new Dictionary<String, KeyValuePair<Int32, String>>();
		String genericTwoTypeString = "System.Collections.Generic.Dictionary`2[[System.String, System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e],[System.Collections.Generic.KeyValuePair`2[[System.Int32, System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e],[System.String, System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]], System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]], System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e";
		Type genericTwoType = RpcActivator.GetType(genericTwoTypeString);

		// Assert.
		Assert.AreEqual(guidEmpty.GetType(), guidEmptyType);
		Assert.AreEqual(stringArrayOne.GetType(), stringArrayOneType);
		Assert.AreEqual(stringArrayTwo.GetType(), stringArrayTwoType);
		Assert.AreEqual(genericArray.GetType(), genericArrayType);
		Assert.AreEqual(genericOne.GetType(), genericOneType);
		Assert.AreEqual(genericTwo.GetType(), genericTwoType);

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
