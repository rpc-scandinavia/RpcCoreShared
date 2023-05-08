namespace RpcScandinavia.Core.Shared.Tests.KeyValueSerializer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RpcScandinavia.Core.Shared;

[TestClass]
public class RpcKeyValueSerializerTests {
	public static Boolean WriteComparing = false;

	public RpcKeyValueSerializerTests() {
	} // RpcKeyValueSerializerTests

	[TestMethod]
	public void TestSerializeString() {
		// Get result and the test data.
		List<KeyValuePair<String, String>> result = this.GetTestResultString().ToList();
		DataA data = this.GetTestData();

		// Serialize.
		//List<KeyValuePair<String, String>> serialized = this.GetTestResult();
		List<KeyValuePair<String, String>> serialized = RpcKeyValueSerializer.SerializeToStrings(data);

		// Assert.
		Assert.AreEqual<Int32>(result.Count, serialized.Count);
		for (Int32 index = 0; index < result.Count; index++) {
			Assert.AreEqual<KeyValuePair<String, String>>(result[index], serialized[index]);
		}
	} // TestSerializeString

	[TestMethod]
	public void TestSerializeMemory() {
		// Get result and the test data.
		List<KeyValuePair<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>>> result = this.GetTestResultMemory().ToList();
		DataA data = this.GetTestData();

		// Serialize.
		List<KeyValuePair<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>>> serialized = RpcKeyValueSerializer.SerializeToMemory(data);

		// Assert.
		Assert.AreEqual<Int32>(result.Count, serialized.Count);
		for (Int32 index = 0; index < result.Count; index++) {
			Assert.AreEqual<String>(result[index].ToString(), serialized[index].ToString());
			//Assert.AreEqual<KeyValuePair<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>>>(result[index], serialized[index]);
		}
	} // TestSerializeMemory

	[TestMethod]
	public void TestDeSerializeString() {
		// Get result and the test data.
		DataA result = this.GetTestData();
		List<KeyValuePair<String, String>> data = this.GetTestResultString().ToList();

		// DeSerialize.
		DataA deserialized = RpcKeyValueSerializer.DeserializeFromStrings<DataA>(data);

		// Assert.
		Assert.AreEqual<DataA>(result, deserialized);
	} // TestDeSerializeString

	[TestMethod]
	public void TestDeSerializeMemory() {
		// Get result and the test data.
		DataA result = this.GetTestData();
		List<KeyValuePair<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>>> data = this.GetTestResultMemory().ToList();

		// DeSerialize.
		DataA deserialized = RpcKeyValueSerializer.DeserializeFromMemory<DataA>(data);

		// Assert.
		Assert.AreEqual<DataA>(result, deserialized);
	} // TestDeSerializeMemory

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

	[TestMethod]
	public void TestGetValues() {
		// Get the test data.
		DataA data = this.GetTestData();
		List<KeyValuePair<String, String>> text = this.GetTestResultString().ToList();

		// Assert.
		Assert.AreEqual<String>(
			data.Text,
			RpcKeyValueSerializer.GetMemberValue<String>(data, text[4].Key)
		);
		Assert.AreEqual<String>(
			((DataA)data.Tag).Text,
			RpcKeyValueSerializer.GetMemberValue<String>(data, text[36].Key)
		);

		Assert.AreEqual<String>(
			data.Comments[2].Value,
			RpcKeyValueSerializer.GetMemberValue<String>(data, text[7].Key)
		);
		Assert.AreEqual<String>(
			((DataA)data.Tag).Comments[2].Value,
			RpcKeyValueSerializer.GetMemberValue<String>(data, text[39].Key)
		);

		Assert.AreEqual<DataEnumA>(
			DataEnumA.Two,
			RpcKeyValueSerializer.GetMemberValue<DataEnumA>(data, text[57].Key)
		);

		Assert.AreEqual<Boolean>(
			data.EnumB.HasFlag(DataEnumB.Bravo),
			RpcKeyValueSerializer.GetMemberValue<Boolean>(data, text[28].Key)
		);
		Assert.AreEqual<Boolean>(
			((DataA)data.Tag).EnumB.HasFlag(DataEnumB.Charlie),
			RpcKeyValueSerializer.GetMemberValue<Boolean>(data, text[61].Key)
		);

	} // TestGetValues

	[TestMethod]
	public void TestSetValues() {
		// Get the test data.
		DataA data = this.GetTestData();
		List<KeyValuePair<String, String>> text = this.GetTestResultString().ToList();

		// Assert.
		RpcKeyValueSerializer.SetMemberValue(data, text[4].Key, "qwerty");
		Assert.AreEqual<String>("qwerty", data.Text);

		RpcKeyValueSerializer.SetMemberValue(data, text[7].Key, "qwerty");
		Assert.AreEqual<String>("qwerty", data.Comments[2].Value);

		for (Int32 index = 1000; index < 6000; index++) {
			RpcKeyValueSerializer.SetMemberValue(data, text[6].Key, $"qwerty {index}");
			Assert.AreEqual<String>($"qwerty {index}", data.Comments[1].Value);
		}

		RpcKeyValueSerializer.SetMemberValue(data, text[57].Key, 1);
		Assert.AreEqual<DataEnumA>(DataEnumA.One, ((DataA)data.Tag).EnumA);

	} // TestSetValues

	public DataA GetTestData(Boolean repeatAsTag = true) {
		return new DataA(
			// Values.
			1234,
			new Guid("ff232da4-42e1-4673-b176-0049a55d5795"),
			new DateTime(2000, 05, 04, 12, 34, 56),
			"This is a test object.",

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
				new DataAbstractB(false)
			},

			// Dictionary.
			new Dictionary<Double, DataGeneric<Int16>>() {
				{ 100.25, new DataGeneric<Int16>(100) },
				{ 531.03, new DataGeneric<Int16>(331) },
				{ 999.99, new DataGeneric<Int16>(999) }
			},

			// Guid list.
			new List<Guid>() {
				new Guid("00000000-0000-0000-0000-000000000000"),
				new Guid("10000000-0000-0000-0000-000000000001"),
				new Guid("20000000-0000-0000-0000-000000000002")
			},

			// Guid array.
			new Guid[] {
				new Guid("10000000-0000-0000-0000-000000000010"),
				new Guid("11000000-0000-0000-0000-000000000011"),
				new Guid("12000000-0000-0000-0000-000000000012")
			},

			// Enum.
			DataEnumA.Two,

			// Enum with flags.
			(DataEnumB.Bravo | DataEnumB.Delta),

			// Tag.
			(repeatAsTag == true) ? this.GetTestData(false) : null
		);
	} // GetTestData

	public KeyValuePair<String, String>[] GetTestResultString() {
		return new KeyValuePair<String, String>[] {
			new KeyValuePair<String, String>("$Type", "RpcScandinavia.Core.Shared.Tests.KeyValueSerializer.DataA, RpcCoreShared.test, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"),
			new KeyValuePair<String, String>("Id", "1234"),
			new KeyValuePair<String, String>("Guid", "ff232da4-42e1-4673-b176-0049a55d5795"),
			new KeyValuePair<String, String>("Time", "2000-05-04T12:34:56.0000000"),
			new KeyValuePair<String, String>("Text", "This is a test object."),

			new KeyValuePair<String, String>("Comments:0:Value", "I am the first comment."),
			new KeyValuePair<String, String>("Comments:1:Value", "I am the second comment."),
			new KeyValuePair<String, String>("Comments:2:Value", "I am the last comment."),

			new KeyValuePair<String, String>("Abstracts:0:$Type", "RpcScandinavia.Core.Shared.Tests.KeyValueSerializer.DataAbstractA, RpcCoreShared.test, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"),
			new KeyValuePair<String, String>("Abstracts:0:StringValue", "the first text abstract"),
			new KeyValuePair<String, String>("Abstracts:1:$Type", "RpcScandinavia.Core.Shared.Tests.KeyValueSerializer.DataAbstractB, RpcCoreShared.test, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"),
			new KeyValuePair<String, String>("Abstracts:1:BoolValue", "True"),
			new KeyValuePair<String, String>("Abstracts:2:$Type", "RpcScandinavia.Core.Shared.Tests.KeyValueSerializer.DataAbstractA, RpcCoreShared.test, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"),
			new KeyValuePair<String, String>("Abstracts:2:StringValue", "the last text abstract"),
			new KeyValuePair<String, String>("Abstracts:3:$Type", "RpcScandinavia.Core.Shared.Tests.KeyValueSerializer.DataAbstractB, RpcCoreShared.test, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"),
			new KeyValuePair<String, String>("Abstracts:3:BoolValue", "False"),

			new KeyValuePair<String, String>("Mappings:100.25:Value", "100"),
			new KeyValuePair<String, String>("Mappings:531.03:Value", "331"),
			new KeyValuePair<String, String>("Mappings:999.99:Value", "999"),

			new KeyValuePair<String, String>("GuidList:0", "00000000-0000-0000-0000-000000000000"),
			new KeyValuePair<String, String>("GuidList:1", "10000000-0000-0000-0000-000000000001"),
			new KeyValuePair<String, String>("GuidList:2", "20000000-0000-0000-0000-000000000002"),

			new KeyValuePair<String, String>("GuidArray:0", "10000000-0000-0000-0000-000000000010"),
			new KeyValuePair<String, String>("GuidArray:1", "11000000-0000-0000-0000-000000000011"),
			new KeyValuePair<String, String>("GuidArray:2", "12000000-0000-0000-0000-000000000012"),

			new KeyValuePair<String, String>("EnumA", "2"),
//			new KeyValuePair<String, String>("EnumB:0", "2"),
//			new KeyValuePair<String, String>("EnumB:1", "8"),
			new KeyValuePair<String, String>("EnumB:0", "True"),
			new KeyValuePair<String, String>("EnumB:1", "False"),
			new KeyValuePair<String, String>("EnumB:2", "True"),
			new KeyValuePair<String, String>("EnumB:4", "False"),
			new KeyValuePair<String, String>("EnumB:8", "True"),
			new KeyValuePair<String, String>("EnumB:16", "False"),

			new KeyValuePair<String, String>("Tag:$Type", "RpcScandinavia.Core.Shared.Tests.KeyValueSerializer.DataA, RpcCoreShared.test, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"),
			new KeyValuePair<String, String>("Tag:Id", "1234"),
			new KeyValuePair<String, String>("Tag:Guid", "ff232da4-42e1-4673-b176-0049a55d5795"),
			new KeyValuePair<String, String>("Tag:Time", "2000-05-04T12:34:56.0000000"),
			new KeyValuePair<String, String>("Tag:Text", "This is a test object."),

			new KeyValuePair<String, String>("Tag:Comments:0:Value", "I am the first comment."),
			new KeyValuePair<String, String>("Tag:Comments:1:Value", "I am the second comment."),
			new KeyValuePair<String, String>("Tag:Comments:2:Value", "I am the last comment."),

			new KeyValuePair<String, String>("Tag:Abstracts:0:$Type", "RpcScandinavia.Core.Shared.Tests.KeyValueSerializer.DataAbstractA, RpcCoreShared.test, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"),
			new KeyValuePair<String, String>("Tag:Abstracts:0:StringValue", "the first text abstract"),
			new KeyValuePair<String, String>("Tag:Abstracts:1:$Type", "RpcScandinavia.Core.Shared.Tests.KeyValueSerializer.DataAbstractB, RpcCoreShared.test, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"),
			new KeyValuePair<String, String>("Tag:Abstracts:1:BoolValue", "True"),
			new KeyValuePair<String, String>("Tag:Abstracts:2:$Type", "RpcScandinavia.Core.Shared.Tests.KeyValueSerializer.DataAbstractA, RpcCoreShared.test, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"),
			new KeyValuePair<String, String>("Tag:Abstracts:2:StringValue", "the last text abstract"),
			new KeyValuePair<String, String>("Tag:Abstracts:3:$Type", "RpcScandinavia.Core.Shared.Tests.KeyValueSerializer.DataAbstractB, RpcCoreShared.test, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"),
			new KeyValuePair<String, String>("Tag:Abstracts:3:BoolValue", "False"),

			new KeyValuePair<String, String>("Tag:Mappings:100.25:Value", "100"),
			new KeyValuePair<String, String>("Tag:Mappings:531.03:Value", "331"),
			new KeyValuePair<String, String>("Tag:Mappings:999.99:Value", "999"),

			new KeyValuePair<String, String>("Tag:GuidList:0", "00000000-0000-0000-0000-000000000000"),
			new KeyValuePair<String, String>("Tag:GuidList:1", "10000000-0000-0000-0000-000000000001"),
			new KeyValuePair<String, String>("Tag:GuidList:2", "20000000-0000-0000-0000-000000000002"),

			new KeyValuePair<String, String>("Tag:GuidArray:0", "10000000-0000-0000-0000-000000000010"),
			new KeyValuePair<String, String>("Tag:GuidArray:1", "11000000-0000-0000-0000-000000000011"),
			new KeyValuePair<String, String>("Tag:GuidArray:2", "12000000-0000-0000-0000-000000000012"),

			new KeyValuePair<String, String>("Tag:EnumA", "2"),
//			new KeyValuePair<String, String>("Tag:EnumB:0", "2"),
//			new KeyValuePair<String, String>("Tag:EnumB:1", "8"),
			new KeyValuePair<String, String>("Tag:EnumB:0", "True"),
			new KeyValuePair<String, String>("Tag:EnumB:1", "False"),
			new KeyValuePair<String, String>("Tag:EnumB:2", "True"),
			new KeyValuePair<String, String>("Tag:EnumB:4", "False"),
			new KeyValuePair<String, String>("Tag:EnumB:8", "True"),
			new KeyValuePair<String, String>("Tag:EnumB:16", "False"),

		};
	} // GetTestResultString

	public KeyValuePair<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>>[] GetTestResultMemory() {
		return this.GetTestResultString()
			.ToList()
			.ConvertAll<KeyValuePair<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>>>((keyValue) => new KeyValuePair<ReadOnlyMemory<Char>, ReadOnlyMemory<Char>>(keyValue.Key.AsMemory(), keyValue.Value.AsMemory()))
			.ToArray();
	} // GetTestResultMemory

} // RpcKeyValueSerializerTests

public class DataA : IEqualityComparer<DataA> {
	public Int32 Id { get; set; }
	public Guid Guid { get; set; }
	public DateTime Time { get; set; }
	public String Text { get; set; }
	public List<DataGeneric<String>> Comments { get; set; }
	public DataAbstract[] Abstracts { get; set; }
	public Dictionary<Double, DataGeneric<Int16>> Mappings { get; set; }
	public List<Guid> GuidList { get; set; }
	public Guid[] GuidArray { get; set; }
	public DataEnumA EnumA { get; set; }
	public DataEnumB EnumB { get; set; }
	public Object Tag { get; set; }

	public DataA() {
		this.Id = 0;
		this.Guid = Guid.Empty;
		this.Time = DateTime.MinValue;
		this.Text = String.Empty;
		this.Comments = new List<DataGeneric<String>>();
		this.Abstracts = new DataAbstract[0];
		this.Mappings = new Dictionary<Double, DataGeneric<Int16>>();
		this.GuidList = new List<Guid>();
		this.GuidArray = new Guid[0];
		this.EnumA = DataEnumA.None;
		this.EnumB = DataEnumB.None;
		this.Tag = null;
	} // DataA

	//public DataA(Int32 id, Guid guid, DateTime time, String text, Object tag) {
	//public DataA(Int32 id, Guid guid, DateTime time, String text, DataGeneric<String>[] comments, Object tag) {
	public DataA(Int32 id, Guid guid, DateTime time, String text, DataGeneric<String>[] comments, DataAbstract[] abstracts, Dictionary<Double, DataGeneric<Int16>> mappings, List<Guid> guidList, Guid[] guidArray, DataEnumA enumA, DataEnumB enumB, Object tag) {
		this.Id = id;
		this.Guid = guid;
		this.Time = time;
		this.Text = text;
		this.Comments = new List<DataGeneric<String>>();
		this.Comments.AddRange(comments);
		this.Abstracts = abstracts;
		this.Mappings = mappings;
		this.GuidList = guidList;
		this.GuidArray = guidArray;
		this.EnumA = enumA;
		this.EnumB = enumB;
		this.Tag = tag;
	} // DataA

	public override Boolean Equals(Object obj) {
		return (obj != null) ? this.Equals(this, (obj as DataA)) : false;
	} // Equals

	public override int GetHashCode() {
		return this.GetHashCode(this);
	} // GetHashCode

	public Boolean Equals(DataA valueA, DataA valueB) {
		if (RpcKeyValueSerializerTests.WriteComparing == true) {
			Debug.WriteLine($"Comparing two DataA objects ({valueA != null}, {valueB != null}).");
			Debug.WriteLine($"       Id:  {(valueA.Id.Equals(valueB.Id))}");
			Debug.WriteLine($"     Guid:  {(valueA.Guid.Equals(valueB.Guid))}");
			Debug.WriteLine($"     Time:  {(valueA.Time.Equals(valueB.Time))}");
			Debug.WriteLine($"     Text:  {(valueA.Text.Equals(valueB.Text))}");
			Debug.WriteLine($" Comments:  {(valueA.Comments.SequenceEqual(valueB.Comments))}     {valueA.Comments?.Count} / {valueB.Comments?.Count}");
			Debug.WriteLine($"Abstracts:  {(valueA.Abstracts.SequenceEqual(valueB.Abstracts))}     {valueA.Abstracts?.Length} / {valueB.Abstracts?.Length}");
			Debug.WriteLine($" Mappings:  {(valueA.Mappings.SequenceEqual(valueB.Mappings))}     {valueA.Mappings?.Count} / {valueB.Mappings?.Count}");
			Debug.WriteLine($"   Guid L:  {(valueA.GuidList.SequenceEqual(valueB.GuidList))}     {valueA.GuidList?.Count} / {valueB.GuidList?.Count}");
			Debug.WriteLine($"   Guid L:  {(valueA.GuidArray.SequenceEqual(valueB.GuidArray))}     {valueA.GuidArray?.Length} / {valueB.GuidArray?.Length}");
			Debug.WriteLine($"    EnumA:  {(valueA.EnumA.Equals(valueB.EnumA))}");
			Debug.WriteLine($"    EnumB:  {(valueA.EnumB.Equals(valueB.EnumB))}");
			Debug.WriteLine($"      Tag:  {(valueA.Tag.EqualsForObjects(valueB.Tag))}");
		}

		return (
			(valueA.Id.Equals(valueB.Id)) &&
			(valueA.Guid.Equals(valueB.Guid)) &&
			(valueA.Time.Equals(valueB.Time)) &&
			(valueA.Text.Equals(valueB.Text)) &&
			(valueA.Comments.SequenceEqual(valueB.Comments)) &&
			(valueA.Abstracts.SequenceEqual(valueB.Abstracts)) &&
			(valueA.Mappings.SequenceEqual(valueB.Mappings)) &&
			(valueA.GuidList.SequenceEqual(valueB.GuidList)) &&
			(valueA.GuidArray.SequenceEqual(valueB.GuidArray)) &&
			(valueA.EnumA.Equals(valueB.EnumA)) &&
			(valueA.EnumB.Equals(valueB.EnumB)) &&
			(valueA.Tag.EqualsForObjects(valueB.Tag))
		);
	} // CompareTo

	public Int32 GetHashCode(DataA value) {
		return HashCode.Combine(
			value.Id,
			value.Guid,
			value.Time,
			value.Text,
			value.Comments,
			value.Abstracts,
			value.Mappings,
//			value.GuidList,
//			this.GuidArray,
//			value.EnumA,
//			value.EnumB,
			value.Tag
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

public enum DataEnumA {
	None = 0,
	One = 1,
	Two = 2,
	Three = 3
} // DataEnumA

[Flags]
public enum DataEnumB {
	None 		= 0b_0000_0000,  		// 0
	Alpha 		= 0b_0000_0001,  		// 1
	Bravo 		= 0b_0000_0010,  		// 2
	Charlie 	= 0b_0000_0100,  		// 4
	Delta 		= 0b_0000_1000,  		// 8
	Echo 		= 0b_0001_0000,  		// 16
} // DataEnumB

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
