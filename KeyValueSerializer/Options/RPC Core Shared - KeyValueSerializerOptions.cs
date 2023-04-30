namespace RpcScandinavia.Core.Shared;
using System;
using System.Globalization;
using System.Collections.Generic;

#region RpcKeyValueSerializerOptions
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueSerializerOptions.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// RPC Key/Value serialization options.
/// </summary>
public class RpcKeyValueSerializerOptions {
	private DateTime modified;
	private List<RpcKeyValueSerializerConverter> converters;
	private Char keyEscapeCharA;
	private Char keyEscapeCharB;
	private Char hierarchySeparatorChar;
	private Int32 maxDepth;
	private Int32 defaultOrderOfMembers;
	private Boolean includePublicFields;
	private Boolean includePrivateFields;
	private Boolean includePublicProperties;
	private Boolean includePrivateProperties;
	private Boolean serializeReadonlyFields;
	private Boolean serializeReadonlyProperties;
	private RpcKeyValueSerializerEnumOption serializeEnums;
	private RpcKeyValueSerializerTypeInfoOption serializeTypeInfo;
	private Boolean serializeTimeInfo;
	private RpcKeyValueSerializerExceptionOption serializeThrowExceptions;
//	private Boolean deserializeTypeInfo;
	private Boolean deserializeEnums;
	private DateTimeStyles deserializeDateTimeStyles;
	private RpcKeyValueSerializerExceptionOption deserializeThrowExceptions;

	#region Constructors
	//------------------------------------------------------------------------------------------------------------------
	// Constructors.
	//------------------------------------------------------------------------------------------------------------------
	public RpcKeyValueSerializerOptions() {
		// Add basic build-in converters.
		this.converters = new List<RpcKeyValueSerializerConverter>();
		this.converters.Add(new RpcKeyValueSerializerConverterBoolean());
		this.converters.Add(new RpcKeyValueSerializerConverterSByte());
		this.converters.Add(new RpcKeyValueSerializerConverterInt16());
		this.converters.Add(new RpcKeyValueSerializerConverterInt32());
		this.converters.Add(new RpcKeyValueSerializerConverterInt64());
		this.converters.Add(new RpcKeyValueSerializerConverterByte());
		this.converters.Add(new RpcKeyValueSerializerConverterUInt16());
		this.converters.Add(new RpcKeyValueSerializerConverterUInt32());
		this.converters.Add(new RpcKeyValueSerializerConverterUInt64());
		this.converters.Add(new RpcKeyValueSerializerConverterSingle());
		this.converters.Add(new RpcKeyValueSerializerConverterDouble());
		this.converters.Add(new RpcKeyValueSerializerConverterDecimal());
		this.converters.Add(new RpcKeyValueSerializerConverterDateTime());
		this.converters.Add(new RpcKeyValueSerializerConverterDateOnly());
		this.converters.Add(new RpcKeyValueSerializerConverterTimeOnly());
		this.converters.Add(new RpcKeyValueSerializerConverterGuid());
		this.converters.Add(new RpcKeyValueSerializerConverterString());
		this.converters.Add(new RpcKeyValueSerializerConverterEnum());

		// Add extra build-in converters.
		// They are not included in the GitHub code.
		#if ! (OPEN_SOURCE)
		this.converters.Add(new RpcKeyValueSerializerConverterTriple());
		this.converters.Add(new RpcKeyValueSerializerConverterClaim());
		this.converters.Add(new RpcKeyValueSerializerConverterCssBorderStyleSides());
		this.converters.Add(new RpcKeyValueSerializerConverterCssColor());
		this.converters.Add(new RpcKeyValueSerializerConverterCssColorSides());
		this.converters.Add(new RpcKeyValueSerializerConverterCssMinMaxUnit());
		this.converters.Add(new RpcKeyValueSerializerConverterCssUnit());
		this.converters.Add(new RpcKeyValueSerializerConverterCssUnitCornersSides());
		this.converters.Add(new RpcKeyValueSerializerConverterCssUnitSides());
		this.converters.Add(new RpcKeyValueSerializerConverterCssXyInt32());
		this.converters.Add(new RpcKeyValueSerializerConverterCultureInfo());
		this.converters.Add(new RpcKeyValueSerializerConverterGridListString());
		#endif

		this.modified = DateTime.Now;
		this.keyEscapeCharA = '\\';
		this.keyEscapeCharB = '/';
		this.hierarchySeparatorChar = ':';
		this.maxDepth = 64;
		this.defaultOrderOfMembers = Int32.MaxValue;
		this.includePublicFields = false;
		this.includePrivateFields = false;
		this.includePublicProperties = true;
		this.includePrivateProperties = false;
		this.serializeReadonlyFields = false;
		this.serializeReadonlyProperties = false;
		this.serializeEnums = RpcKeyValueSerializerEnumOption.AsInteger;
		this.serializeTypeInfo = RpcKeyValueSerializerTypeInfoOption.RequiredAndTop;
		this.serializeTimeInfo = false;
		this.serializeThrowExceptions = RpcKeyValueSerializerExceptionOption.ThrowCriticalExceptions;
//		this.deserializeTypeInfo = true;
		this.deserializeEnums = true;
		this.deserializeDateTimeStyles = DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal | DateTimeStyles.AllowWhiteSpaces;
		this.deserializeThrowExceptions = RpcKeyValueSerializerExceptionOption.ThrowCriticalExceptions;
	} // KeyValueSerializerOptions

	public RpcKeyValueSerializerOptions(RpcKeyValueSerializerOptions options) {
		this.modified = options.modified;
		this.converters = new List<RpcKeyValueSerializerConverter>();
		this.converters.AddRange(options.converters);
		this.keyEscapeCharA = options.keyEscapeCharA;
		this.keyEscapeCharB = options.keyEscapeCharB;
		this.hierarchySeparatorChar = options.hierarchySeparatorChar;
		this.maxDepth = options.maxDepth;
		this.defaultOrderOfMembers = options.defaultOrderOfMembers;
		this.hierarchySeparatorChar = options.hierarchySeparatorChar;
		this.includePublicFields = options.includePublicFields;
		this.includePrivateFields = options.includePrivateFields;
		this.includePublicProperties = options.includePublicProperties;
		this.includePrivateProperties = options.includePrivateProperties;
		this.serializeReadonlyFields = options.serializeReadonlyFields;
		this.serializeReadonlyProperties = options.serializeReadonlyProperties;
		this.serializeEnums = options.serializeEnums;
		this.serializeTypeInfo = options.serializeTypeInfo;
		this.serializeTimeInfo = options.serializeTimeInfo;
		this.serializeThrowExceptions = options.serializeThrowExceptions;
//		this.deserializeTypeInfo = options.deserializeTypeInfo;
		this.deserializeEnums = options.deserializeEnums;
		this.deserializeDateTimeStyles = options.deserializeDateTimeStyles;
		this.deserializeThrowExceptions = options.deserializeThrowExceptions;
	} // KeyValueSerializerOptions
	#endregion

	#region Properties
	//------------------------------------------------------------------------------------------------------------------
	// Properties.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the last time when the options was modified.
	/// </summary>
	public DateTime Modified {
		get {
			return this.modified;
		}
	} // Modified

	/// <summary>
	/// Gets the list of converters.
	/// </summary>
	public RpcKeyValueSerializerConverter[] Converters {
		get {
			return this.converters.ToArray();
		}
	} // Converters

	/// <summary>
	/// Gets the A character used to escape illegal characters in the keys.
	/// </summary>
	public Char KeyEscapeCharA {
		get {
			return this.keyEscapeCharA;
		}
	} // KeyEscapeCharA

	/// <summary>
	/// Gets the B character used to escape illegal characters in the keys.
	/// </summary>
	public Char KeyEscapeCharB {
		get {
			return this.keyEscapeCharB;
		}
	} // KeyEscapeCharB

	/// <summary>
	/// Gets the character used to separate the hierarchy in the serialized keys.
	/// </summary>
	public Char HierarchySeparatorChar {
		get {
			return this.hierarchySeparatorChar;
		}
	} // HierarchySeparatorChar

	/// <summary>
	/// Gets or sets the maximum depth allowed when serializing or deserializing, with the default value of 64.
	/// </summary>
	public Int32 MaxDepth {
		get {
			if (this.maxDepth < 1) {
				return 64;
			} else {
				return this.maxDepth;
			}
		}
		set {
			if (this.maxDepth < 1) {
				this.maxDepth = 64;
				this.modified = DateTime.Now;
			} else {
				this.maxDepth = value;
				this.modified = DateTime.Now;
			}
		}
	} // MaxDepth

	/// <summary>
	/// Gets or sets the default order of properties, without the <see cref="System.Text.Json.Serialization.JsonPropertyOrderAttribute" /> attribute.
	/// A value of "Int32.MinValue" will sort with the undecorated properties first, and a value of "Int32.MaxValue"
	/// will sort with the undecorated properties last.
	/// </summary>
	public Int32 DefaultOrderOfProperties {
		get {
			return this.defaultOrderOfMembers;
		}
		set {
			this.defaultOrderOfMembers = value;
			this.modified = DateTime.Now;
		}
	} // DefaultOrderOfProperties

	/// <summary>
	/// Gets or sets whether or not public fields are serialized and deserialized.
	/// All public fields are serialized and deserialized when this is set to true, except public fields decorated with
	/// the <see cref="System.Text.Json.Serialization.JsonIgnoreAttribute" /> attribute.
	/// </summary>
	public Boolean IncludePublicFields {
		get {
			return this.includePublicFields;
		}
		set {
			this.includePublicFields = value;
			this.modified = DateTime.Now;
		}
	} // IncludePublicFields

	/// <summary>
	/// Gets or sets whether or not private fields are serialized and deserialized.
	/// All private fields are serialized and deserialized when this is set to true, except private fields decorated with
	/// the <see cref="System.Text.Json.Serialization.JsonIgnoreAttribute" /> attribute.
	/// </summary>
	public Boolean IncludePrivateFields {
		get {
			return this.includePrivateFields;
		}
		set {
			this.includePrivateFields = value;
			this.modified = DateTime.Now;
		}
	} // IncludePrivateFields

	/// <summary>
	/// Gets or sets whether or not public properties are serialized and deserialized.
	/// All public properties are serialized and deserialized when this is set to true, except public properties
	/// decorated with the <see cref="System.Text.Json.Serialization.JsonIgnoreAttribute" /> attribute.
	/// </summary>
	public Boolean IncludePublicProperties {
		get {
			return this.includePublicProperties;
		}
		set {
			this.includePublicProperties = value;
			this.modified = DateTime.Now;
		}
	} // IncludePublicProperties

	/// <summary>
	/// Gets or sets whether or not private properties are serialized and deserialized.
	/// All private properties are serialized and deserialized when this is set to true, except private properties
	/// decorated with the <see cref="System.Text.Json.Serialization.JsonIgnoreAttribute" /> attribute.
	/// </summary>
	public Boolean IncludePrivateProperties {
		get {
			return this.includePrivateProperties;
		}
		set {
			this.includePrivateProperties = value;
			this.modified = DateTime.Now;
		}
	} // IncludePrivateProperties

	/// <summary>
	/// Gets or sets whether or not readonly fields should be serialized.
	/// </summary>
	public Boolean SerializeReadonlyFields {
		get {
			return this.serializeReadonlyFields;
		}
		set {
			this.serializeReadonlyFields = value;
			this.modified = DateTime.Now;
		}
	} // SerializeReadonlyFields

	/// <summary>
	/// Gets or sets whether or not readonly properties should be serialized.
	/// </summary>
	public Boolean SerializeReadonlyProperties {
		get {
			return this.serializeReadonlyProperties;
		}
		set {
			this.serializeReadonlyProperties = value;
			this.modified = DateTime.Now;
		}
	} // SerializeReadonlyProperties

	/// <summary>
	/// Gets or sets whether or not enums should be serialized.
	/// If this is disabled, selected enums may still be serialized by using a custom converter.
	/// </summary>
	public RpcKeyValueSerializerEnumOption SerializeEnums {
		get {
			return this.serializeEnums;
		}
		set {
			this.serializeEnums = value;
			this.modified = DateTime.Now;
		}
	} // SerializeEnums

	/// <summary>
	/// Gets or sets whether or not type information should be serialized.
	/// This is required to deserialize interfaces, abstract classes and object to their original type.
	/// </summary>
	public RpcKeyValueSerializerTypeInfoOption SerializeTypeInfo {
		get {
			return this.serializeTypeInfo;
		}
		set {
			this.serializeTypeInfo = value;
			this.modified = DateTime.Now;
		}
	} // SerializeTypeInfo

	/// <summary>
	/// Gets or sets whether or not time information should be serialized.
	/// </summary>
	public Boolean SerializeTimeInfo {
		get {
			return this.serializeTimeInfo;
		}
		set {
			this.serializeTimeInfo = value;
			this.modified = DateTime.Now;
		}
	} // SerializeTimeInfo

	/// <summary>
	/// Gets or sets whether or not exceptions caught setting individual properties when serializing are thrown.
	/// </summary>
	public RpcKeyValueSerializerExceptionOption SerializeThrowExceptions {
		get {
			return this.serializeThrowExceptions;
		}
		set {
			this.serializeThrowExceptions = value;
			this.modified = DateTime.Now;
		}
	} // SerializeThrowExceptions

//	/// <summary>
//	/// Gets or sets whether or not serialized type information should be used when deserializing.
//	/// The serialized values, must have been serialized with the <see cref="RpcScandinavia.Core.Shared.RpcKeyValueSerializerOptions.serializeTypeInfo" /> option set to true.
//	/// This is required to deserialize interfaces, abstract classes and object to their original type.
//	/// </summary>
//	public Boolean DeserializeTypeInfo {
//		get {
//			return this.deserializeTypeInfo;
//		}
//		set {
//			this.deserializeTypeInfo = value;
//			this.modified = DateTime.Now;
//		}
//	} // DeserializeTypeInfo

	/// <summary>
	/// Gets or sets whether or not enums should be deserialized.
	/// If this is disabled, selected enums may still be deserialized by using a custom converter.
	/// </summary>
	public Boolean DeserializeEnums {
		get {
			return this.deserializeEnums;
		}
		set {
			this.deserializeEnums = value;
			this.modified = DateTime.Now;
		}
	} // DeserializeEnums

	/// <summary>
	/// Gets or sets the styles used when dates and times are deserialized.
	/// </summary>
	public DateTimeStyles DeserializeDateTimeStyles {
		get {
			return this.deserializeDateTimeStyles;
		}
		set {
			this.deserializeDateTimeStyles = value;
			this.modified = DateTime.Now;
		}
	} // DeserializeDateTimeStyles

	/// <summary>
	/// Gets or sets whether or not exceptions caught setting individual properties when deserializing are thrown.
	/// </summary>
	public RpcKeyValueSerializerExceptionOption DeserializeThrowExceptions {
		get {
			return this.deserializeThrowExceptions;
		}
		set {
			this.deserializeThrowExceptions = value;
			this.modified = DateTime.Now;
		}
	} // DeserializeThrowExceptions
	#endregion

} // RpcKeyValueSerializerOptions
#endregion
