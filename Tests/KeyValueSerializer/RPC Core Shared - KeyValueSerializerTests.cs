namespace RpcScandinavia.Core.Shared.Tests.KeyValueSerializer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RpcScandinavia.Core.Shared;
using RpcScandinavia.Core.Shared.KeyValueSerializer;

[TestClass]
public class RpcKeyValueSerializerTests {

	public RpcKeyValueSerializerTests() {
	} // RpcKeyValueSerializerTests

	[TestMethod]
	public void TestSerialize() {
		// Get result and the test data.
		List<KeyValuePair<String, String>> result = this.GetTestResult().ToList();
		DataA data = this.GetTestData();

		// Serialize.
		//List<KeyValuePair<String, String>> serialized = this.GetTestResult();
		List<KeyValuePair<String, String>> serialized = RpcKeyValueSerializer.Serialize(data);

File.WriteAllLines(
	"/data/users/rpc@rpc-scandinavia.dk/Desktop/Linux/Serialized test.txt",
	serialized
		.ConvertAll<String>((keyValue) => $"{keyValue.Key} = {keyValue.Value}")
		.ToList()
);

		// Assert.
		Assert.AreEqual<Int32>(result.Count, serialized.Count);
		for (Int32 index = 0; index < result.Count; index++) {
			Assert.AreEqual<KeyValuePair<String, String>>(result[index], serialized[index]);
		}
	} // TestSerialize

	[TestMethod]
	public void TestDeSerialize() {
		// Get result and the test data.
		DataA result = this.GetTestData();
		List<KeyValuePair<String, String>> data = this.GetTestResult().ToList();

		// DeSerialize.
		//DataA deserialized = this.GetTestData();
		DataA deserialized = RpcKeyValueSerializer.Deserialize<DataA>(data);
		//deserialized.Comments.Last().Value = "I am changed!";

		// Assert.
		Assert.AreEqual<DataA>(result, deserialized);
	} // TestDeSerialize

	[TestMethod]
	public void TestCopy() {
		// Get the result and the test data.
		DataA result = this.GetTestData();

		// Copy.
		//DataA copy = this.GetTestData();
		DataA copy = RpcKeyValueSerializer.Copy<DataA>(result);

		// Assert.
		Assert.AreEqual<DataA>(result, copy);
	} // TestCopy

	public DataA GetTestData(Boolean repeatAsTag = true) {
		return new DataA(
			// Values.
			1234,
			new Guid("ff232da4-42e1-4673-b176-0049a55d5795"),
			new DateTime(2000, 05, 04, 12, 34, 56),
			"This is a test object."
/*
			// List of generics.
			new DataGeneric<String>[] {
				new DataGeneric<String>("I am the first comment."),
				new DataGeneric<String>("I am the second comment."),
				new DataGeneric<String>("I am the last comment.")
			},

			// Array of abstracts.
			new DataAbstract[] {
				new DataAbstractA("the first text abstract"),
				new DataAbstractB(true),
				new DataAbstractA("the last text abstract"),
				new DataAbstractB(false),
			},

			// Tag.
			(repeatAsTag == true) ? this.GetTestData(false) : null
*/
		);
	} // GetTestData

	public KeyValuePair<String, String>[] GetTestResult() {
		return new KeyValuePair<String, String>[] {
			new KeyValuePair<String, String>("$Type", "RpcScandinavia.Core.Shared.Tests.KeyValueSerializer.DataA, RpcCoreShared.test, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"),
			new KeyValuePair<String, String>("Id", "1234"),
			new KeyValuePair<String, String>("Guid", "ff232da4-42e1-4673-b176-0049a55d5795"),
			new KeyValuePair<String, String>("Time", "2000-05-04T12:34:56.0000000"),
			new KeyValuePair<String, String>("Text", "This is a test object."),
/*
			new KeyValuePair<String, String>("Comments:0:$Type", "RpcScandinavia.Core.Shared.Tests.KeyValueSerializer.DataGeneric`1[[System.String, System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]], RpcCoreShared.test, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"),
			new KeyValuePair<String, String>("Comments:0:Value", "I am the first comment."),
			new KeyValuePair<String, String>("Comments:1:$Type", ""),
			new KeyValuePair<String, String>("Comments:1:Value", "I am the second comment."),
			new KeyValuePair<String, String>("Comments:2:$Type", ""),
			new KeyValuePair<String, String>("Comments:2:Value", "I am the last comment"),
			new KeyValuePair<String, String>("Abstracts:0:$Type", ""),
			new KeyValuePair<String, String>("Abstracts:0:StringValue", "the first text abstract"),
			new KeyValuePair<String, String>("Abstracts:1:BoolValue", "true"),
			new KeyValuePair<String, String>("Abstracts:2:StringValue", "the last text abstract"),
			new KeyValuePair<String, String>("Abstracts:3:BoolValue", "false"),
			new KeyValuePair<String, String>("Tag:$Type", "RpcScandinavia.Core.Shared.Tests.KeyValueSerializer.DataA, RpcCoreShared.test, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"),
			new KeyValuePair<String, String>("Tag:Id", "1234"),
			new KeyValuePair<String, String>("Tag:Guid", "ff232da4-42e1-4673-b176-0049a55d5795"),
			new KeyValuePair<String, String>("Tag:Time", "2000-05-04 12:34:56"),
			new KeyValuePair<String, String>("Tag:Text", "This is a test object."),
			new KeyValuePair<String, String>("Tag:Comments:0:Value", "I am the first comment."),
			new KeyValuePair<String, String>("Tag:Comments:1:Value", "I am the second comment."),
			new KeyValuePair<String, String>("Tag:Comments:2:Value", "I am the last comment"),
			new KeyValuePair<String, String>("Tag:Abstracts:0:StringValue", "the first text abstract"),
			new KeyValuePair<String, String>("Tag:Abstracts:1:BoolValue", "true"),
			new KeyValuePair<String, String>("Tag:Abstracts:2:StringValue", "the last text abstract"),
			new KeyValuePair<String, String>("Tag:Abstracts:3:BoolValue", "false"),
			new KeyValuePair<String, String>("Tag:Tag", "")
*/
		};
	} // GetTestResult

} // RpcKeyValueSerializerTests

public class DataA : IEqualityComparer<DataA> {
	public Int32 Id { get; set; }
	public Guid Guid { get; set; }
	public DateTime Time { get; set; }
	public String Text { get; set; }
//	public List<DataGeneric<String>> Comments { get; set; }
//	public DataAbstract[] Abstracts { get; set; }
//	public Object Tag { get; set; }

	public DataA() {
		this.Id = 0;
		this.Guid = Guid.Empty;
		this.Time = DateTime.MinValue;
		this.Text = String.Empty;
//		this.Comments = new List<DataGeneric<String>>();
//		this.Abstracts = new DataAbstract[0];
//		this.Tag = null;
	} // DataA

	public DataA(Int32 id, Guid guid, DateTime time, String text) {
	//public DataA(Int32 id, Guid guid, DateTime time, String text, DataGeneric<String>[] comments, DataAbstract[] abstracts, Object tag) {
		this.Id = id;
		this.Guid = guid;
		this.Time = time;
		this.Text = text;
//		this.Comments = new List<DataGeneric<String>>();
//		this.Comments.AddRange(comments);
//		this.Abstracts = abstracts;
//		this.Tag = tag;
	} // DataA

	public override Boolean Equals(Object obj) {
		return (obj != null) ? this.Equals(this, (obj as DataA)) : false;
	} // Equals

	public override int GetHashCode() {
		return this.GetHashCode(this);
	} // GetHashCode

	public Boolean Equals(DataA valueA, DataA valueB) {
		Debug.WriteLine($"Comparing two DataA objects.");
		Debug.WriteLine($"       Id:  {(valueA.Id.Equals(valueB.Id))}");
		Debug.WriteLine($"     Guid:  {(valueA.Guid.Equals(valueB.Guid))}");
		Debug.WriteLine($"     Time:  {(valueA.Time.Equals(valueB.Time))}");
		Debug.WriteLine($"     Text:  {(valueA.Text.Equals(valueB.Text))}");
//		Debug.WriteLine($" Comments:  {(valueA.Comments.SequenceEqual(valueB.Comments))}");
//		Debug.WriteLine($"Abstracts:  {(valueA.Abstracts.SequenceEqual(valueB.Abstracts))}");
//		Debug.WriteLine($"      Tag:  {(valueA.Tag.EqualsForObjects(valueB.Tag))}");

		return (
			(valueA.Id.Equals(valueB.Id)) &&
			(valueA.Guid.Equals(valueB.Guid)) &&
			(valueA.Time.Equals(valueB.Time)) &&
			(valueA.Text.Equals(valueB.Text))
//			(valueA.Comments.SequenceEqual(valueB.Comments)) &&
//			(valueA.Abstracts.SequenceEqual(valueB.Abstracts)) &&
//			(valueA.Tag.EqualsForObjects(valueB.Tag))
		);
	} // CompareTo

	public Int32 GetHashCode(DataA value) {
		return HashCode.Combine(
			value.Id,
			value.Guid,
			value.Time,
			value.Text
//			value.Comments,
//			value.Comments,
//			value.Abstracts,
//			value.Tag
		);
	} // GetHashCode

} // DataA

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

public abstract class DataAbstract : IEqualityComparer<DataAbstract> {
	public abstract String IAm();

	public override Boolean Equals(Object obj) {
		return (obj != null) ? this.Equals(this, (obj as DataAbstract)) : false;
	} // Equals

	public override int GetHashCode() {
		return this.GetHashCode(this);
	} // GetHashCode

	public Boolean Equals(DataAbstract valueA, DataAbstract valueB) {
		return (valueA.EqualsForObjects(valueB));
	} // CompareTo

	public Int32 GetHashCode(DataAbstract value) {
		return value.GetHashCode();
	} // GetHashCode

} // DataAbstract

public class DataAbstractA : DataAbstract, IEqualityComparer<DataAbstractA> {
	public String StringValue { get; set; }

	public DataAbstractA() {
		this.StringValue = null;
	} // DataAbstractA

	public DataAbstractA(String value) {
		this.StringValue = value;
	} // DataAbstractA

	public override String IAm() {
		return $"I am '{this.StringValue}'.";
	} // IAm

	public override Boolean Equals(Object obj) {
		return (obj != null) ? this.Equals(this, (obj as DataAbstractA)) : false;
	} // Equals

	public override int GetHashCode() {
		return this.GetHashCode(this);
	} // GetHashCode

	public Boolean Equals(DataAbstractA valueA, DataAbstractA valueB) {
		return (valueA.StringValue.Equals(valueB.StringValue));
	} // CompareTo

	public Int32 GetHashCode(DataAbstractA value) {
		return value.StringValue.GetHashCode();
	} // GetHashCode

} // DataAbstractA

public class DataAbstractB : DataAbstract, IEqualityComparer<DataAbstractB> {
	public Boolean BoolValue { get; set; }

	public DataAbstractB() {
		this.BoolValue = false;
	} // DataAbstractB

	public DataAbstractB(Boolean value) {
		this.BoolValue = value;
	} // DataAbstractB

	public override String IAm() {
		return $"I am '{this.BoolValue}'.";
	} // IAm

	public override Boolean Equals(Object obj) {
		return (obj != null) ? this.Equals(this, (obj as DataAbstractB)) : false;
	} // Equals

	public override int GetHashCode() {
		return this.GetHashCode(this);
	} // GetHashCode

	public Boolean Equals(DataAbstractB valueA, DataAbstractB valueB) {
		return (valueA.BoolValue.Equals(valueB.BoolValue));
	} // CompareTo

	public Int32 GetHashCode(DataAbstractB value) {
		return value.BoolValue.GetHashCode();
	} // GetHashCode

} // DataAbstractB

public static class DataComparerExtensions {

	public static Boolean EqualsForObjects(this Object valueA, Object valueB) {
		if ((valueA == null) && (valueB == null)) {
			return true;
		} else if ((valueA == null) || (valueB == null)) {
			return false;
		} else {
			return (valueA.Equals(valueB));
		}
	} // EqualsForObjects

	public static Int32 GetHashCodeForObject(this Object value) {
		return (value != null) ? value.GetHashCode() : 0;
	} // GetHashCodeForObject

} // DataComparerExtensions
