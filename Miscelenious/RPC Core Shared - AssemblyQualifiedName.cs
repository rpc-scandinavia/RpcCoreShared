namespace RpcScandinavia.Core.Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

#region RpcAssemblyQualifiedName
//----------------------------------------------------------------------------------------------------------------------
// RpcAssemblyQualifiedName.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// This is a Assembly Qualified Name parser.
/// It uses a <see cref="System.Memory{System.Char}" /> to parse the Assembly Qualified Name.
/// </summary>
public class RpcAssemblyQualifiedName : EqualityComparer<RpcAssemblyQualifiedName>, IEqualityComparer<RpcAssemblyQualifiedName> {
	private const Char Char0 = '0';
	private const Char Char1 = '1';
	private const Char Char2 = '2';
	private const Char Char3 = '3';
	private const Char Char4 = '4';
	private const Char Char5 = '5';
	private const Char Char6 = '6';
	private const Char Char7 = '7';
	private const Char Char8 = '8';
	private const Char Char9 = '9';
	private const Char CharSpace = ' ';
	private const Char CharSeparator = ',';
	private const Char CharBeginArray = '[';
	private const Char CharEndArray = ']';
	private const Char CharBeginGenericCount = '`';
	private const Char CharBeginGeneric = '[';
	private const Char CharEndGeneric = ']';
	private const String StrVersion = "Version=";
	private const String StrCulture = "Culture=";
	private const String StrPublicKeyToken = "PublicKeyToken=";

	private ReadOnlyMemory<Char> assemblyQualifiedName;
	private ReadOnlyMemory<Char> typePart;
	private ReadOnlyMemory<Char> assemblyNamePart;
	private ReadOnlyMemory<Char> assemblyVersionPart;
	private ReadOnlyMemory<Char> assemblyCulturePart;
	private ReadOnlyMemory<Char> assemblyPublicKeyPart;
	private Int32 isArray;
	private Boolean isGeneric;
	private RpcAssemblyQualifiedName[] genericTypeArguments;

	private Int32 indexBegin;
	private Int32 indexLength;

	#region Constructors
	//------------------------------------------------------------------------------------------------------------------
	// Constructors.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Parse a Assembly Qualified Name.
	/// </summary>
	/// <param name="obj">The object, which type name is parsed.</param>
	public RpcAssemblyQualifiedName(Object obj) : this(obj.GetType().AssemblyQualifiedName.AsMemory(), 0) {
	} // RpcAssemblyQualifiedName

	/// <summary>
	/// Parse a Assembly Qualified Name.
	/// </summary>
	/// <param name="type">The type, which type name is parsed.</param>
	public RpcAssemblyQualifiedName(Type type) : this(type.AssemblyQualifiedName.AsMemory(), 0) {
	} // RpcAssemblyQualifiedName

	/// <summary>
	/// Parse a Assembly Qualified Name.
	/// </summary>
	/// <param name="assemblyQualifiedName">The type name parsed.</param>
	public RpcAssemblyQualifiedName(String assemblyQualifiedName) : this(assemblyQualifiedName.AsMemory(), 0) {
	} // RpcAssemblyQualifiedName

	/// <summary>
	/// Parse a Assembly Qualified Name.
	/// </summary>
	/// <param name="assemblyQualifiedName">The type name parsed.</param>
	public RpcAssemblyQualifiedName(ReadOnlyMemory<Char> assemblyQualifiedName) : this(assemblyQualifiedName, 0) {
	} // RpcAssemblyQualifiedName

	/// <summary>
	/// Parse the nested Assembly Qualified Name.
	/// </summary>
	/// <param name="assemblyQualifiedName">The type name parsed.</param>
	/// <param name="indexBegin">The position to start parsing from.</param>
	private RpcAssemblyQualifiedName(ReadOnlyMemory<Char> assemblyQualifiedName, Int32 indexBegin) {
		this.assemblyQualifiedName = assemblyQualifiedName;

		// Parts.
		this.typePart = new ReadOnlyMemory<Char>();
		this.assemblyNamePart = new ReadOnlyMemory<Char>();
		this.assemblyVersionPart = new ReadOnlyMemory<Char>();
		this.assemblyCulturePart = new ReadOnlyMemory<Char>();
		this.assemblyPublicKeyPart = new ReadOnlyMemory<Char>();

		// Array.
		this.isArray = 0;

		// Generic.
		this.isGeneric = false;
		this.genericTypeArguments = new RpcAssemblyQualifiedName[0];

		// Parse.
		this.indexBegin = indexBegin;
		this.indexLength = 0;
		this.Parse();
	} // RpcAssemblyQualifiedName
	#endregion

	#region Methods
	//------------------------------------------------------------------------------------------------------------------
	// Methods.
	//------------------------------------------------------------------------------------------------------------------
	private void Parse() {
		// Examples of Assembly Qualified Name strings:
		//		System.Guid, System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
		//		System.String[], System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
		//		System.String[,], System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
		//		RpcScandinavia.Core.Shared.Tests.Miscelenious.DataGeneric`1[[System.Int32, System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]][], RpcCoreShared.test, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
		//		RpcScandinavia.Core.Shared.Tests.Miscelenious.DataGeneric`1[[System.String, System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]], RpcCoreShared.test, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
		//		System.Collections.Generic.Dictionary`2[[System.String, System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e],[System.Collections.Generic.KeyValuePair`2[[System.Int32, System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e],[System.String, System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]], System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]], System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
		//
		// The Assembly Qualified Name contains five parts of information:
		//		System.Guid, System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
		//		0            1                       2                3                4
		//
		//		0 = Type name with the namespace and additional type arguments.
		//		1 = Assembly name.
		//		2 = Assembly version.
		//		3 = Assembly culture.
		//		4 = Assembly public signature key.
		//

		// Parse the Assembly Qualified Name:
		// 1)	Find the end of the type name. This is one of these characters (",", "[", "`").
		// 2a)	Recursively get the generic type arguments. They are wrapped in "[[" and "]]"-
		// 2b)	Get the dimensions of the array, by counting "," characters untill the "]" character.
		// 3)	Find the end of the assembly name (optional).
		// 4)	Find the end of the assembly version (optional).
		// 5)	Find the end of the assembly culture (optional).
		// 6)	Find the end of the assembly public signature key (optional).

		Int32 index = this.indexBegin;
		Int32 partIndex = 0;
		Int32 partLength = 0;
		Char partExitChar = Char.MinValue;

		// 1.
		(partIndex, partLength, partExitChar) = this.ParsePart(ref index, String.Empty, CharSeparator, CharBeginArray, CharBeginGenericCount);
		if (partLength > 0) {
			if (partExitChar == CharSeparator) {
				// Found normal type.
			} else if (partExitChar == CharBeginArray) {
				// Found array type.
				this.isArray = 1;
			} else if (partExitChar == CharBeginGenericCount) {
				// Found generic type.
				this.isGeneric = true;
			}
			this.typePart = this.assemblyQualifiedName.Slice(partIndex, partLength).Trim();
		}

		// 2a.
		if (this.isGeneric == true) {
			// Get the number of generic type arguments.
			Int32 numberIndex = index;
			Int32 numberLength = 0;
			while (index < this.assemblyQualifiedName.Length) {
				if ((this.assemblyQualifiedName.Span[index] == Char0) ||
					(this.assemblyQualifiedName.Span[index] == Char1) ||
					(this.assemblyQualifiedName.Span[index] == Char2) ||
					(this.assemblyQualifiedName.Span[index] == Char3) ||
					(this.assemblyQualifiedName.Span[index] == Char4) ||
					(this.assemblyQualifiedName.Span[index] == Char5) ||
					(this.assemblyQualifiedName.Span[index] == Char6) ||
					(this.assemblyQualifiedName.Span[index] == Char7) ||
					(this.assemblyQualifiedName.Span[index] == Char8) ||
					(this.assemblyQualifiedName.Span[index] == Char9)) {
					// Found number.
					numberLength++;

					// Iterate.
					index++;
				} else if (this.assemblyQualifiedName.Span[index] == CharBeginGeneric) {
					// Found generic type.
					// Iterate.
					index++;

					break;
				} else {
					throw new Exception($"Expected generic type parameter count.");
				}
			}

			Int32 aqnCount = Int32.Parse(this.assemblyQualifiedName.Span.Slice(numberIndex, numberLength));
			this.genericTypeArguments = new RpcAssemblyQualifiedName[aqnCount];

			for (Int32 aqnIndex = 0; aqnIndex < aqnCount; aqnIndex++) {
				// Iterate begen generic characters.
				this.ParseTrim(ref index, CharBeginGeneric);

				// Parse the generic type.
				RpcAssemblyQualifiedName aqn = new RpcAssemblyQualifiedName(this.assemblyQualifiedName, index);
				index += aqn.indexLength;
				this.genericTypeArguments[aqnIndex] = aqn;

				// Iterate first end generic characters.
				this.ParseTrim(ref index, CharEndGeneric);

				// Iterate generic separator.
				if (aqnIndex < aqnCount - 1) {
					this.ParseTrim(ref index, CharSeparator);
					this.ParseTrim(ref index, CharSpace);
				}
			}

			// Iterate second end generic characters.
			this.ParseTrim(ref index, CharEndGeneric);
		}

		// 2b.
		if (this.isArray > 0) {
			/* This is not good code, so the "ParsePart" method is not used here.
			partExitChar = CharSeparator;
			while (partExitChar == CharSeparator) {
				(partIndex, partLength, partExitChar) = this.ParsePart(ref index, String.Empty, CharSeparator, CharEndArray, Char.MinValue);
				if (partExitChar == CharSeparator) {
					// Found array dimmension.
					this.isArray++;
				}
			}
			*/

			// Get the array dimensions.
			while (index < this.assemblyQualifiedName.Length) {
				if (this.assemblyQualifiedName.Span[index] == CharSeparator) {
					// Found array dimmension.
					this.isArray++;

					// Iterate.
					index++;
				} else if (this.assemblyQualifiedName.Span[index] == CharEndArray) {
					// Found end of array type.
					// Iterate.
					index++;

					break;
				} else {
					throw new Exception($"Expected array dimension '{CharSeparator}' character or end of array '{CharEndArray}' character.");
				}
			}
		}

		// Iterate separator and whitespace characters.
		this.ParseTrim(ref index, CharSeparator);
		this.ParseTrim(ref index, CharSpace);

		// 3.
		(partIndex, partLength, partExitChar) = this.ParsePart(ref index, String.Empty, CharSeparator, CharEndGeneric, Char.MinValue);
		if (partLength > 0) {
			this.assemblyNamePart = this.assemblyQualifiedName.Slice(partIndex, partLength).Trim();
		}

		// 4.
		(partIndex, partLength, partExitChar) = this.ParsePart(ref index, StrVersion, CharSeparator, CharEndGeneric, Char.MinValue);
		if (partLength > 0) {
			this.assemblyVersionPart = this.assemblyQualifiedName.Slice(partIndex, partLength).Trim();
		}

		// 5.
		(partIndex, partLength, partExitChar) = this.ParsePart(ref index, StrCulture, CharSeparator, CharEndGeneric, Char.MinValue);
		if (partLength > 0) {
			this.assemblyCulturePart = this.assemblyQualifiedName.Slice(partIndex, partLength).Trim();
		}

		// 6.
		(partIndex, partLength, partExitChar) = this.ParsePart(ref index, StrPublicKeyToken, CharSeparator, CharEndGeneric, Char.MinValue);
		if (partLength > 0) {
			this.assemblyPublicKeyPart = this.assemblyQualifiedName.Slice(partIndex, partLength).Trim();
		}

		// Set the length of the parsed text.
		this.indexLength = partIndex + partLength - this.indexBegin;
	} // Parse

	private (Int32 partIndex, Int32 partLength, Char exitChar) ParsePart(ref Int32 index, String prefix, Char exitOnChar0, Char exitOnChar1, Char exitOnChar2) {
		if (index < this.assemblyQualifiedName.Length) {
			index = index + prefix.Length;
			Int32 partIndex = index;
			Int32 partLength = 0;
			Char exitChar = Char.MinValue;
			while ((index < this.assemblyQualifiedName.Length) && (exitChar == Char.MinValue)) {
				if ((this.assemblyQualifiedName.Span[index] == exitOnChar0) ||
					(this.assemblyQualifiedName.Span[index] == exitOnChar1) ||
					(this.assemblyQualifiedName.Span[index] == exitOnChar2)) {
					// Exit.
					exitChar = this.assemblyQualifiedName.Span[index];
				} else {
					// Found part data.
					partLength++;
				}

				// Iterate.
				index++;
			}

			// Iterate whitespace characters.
			this.ParseTrim(ref index, CharSpace);

			// Return the part data.
			return (partIndex, partLength, exitChar);
		}

		// Return nothing found.
		return (0, 0, Char.MinValue);
	} // ParsePart

	private void ParseTrim(ref Int32 index, Char trimChar) {
		// Iterate whitespace characters.
		while ((index < this.assemblyQualifiedName.Length) && (this.assemblyQualifiedName.Span[index] == trimChar)) {
			// Iterate.
			index++;
		}
	} // ParseTrim

	/// <summary>
	/// Gets the string representation of this Assembly Qualified Name.
	/// </summary>
	public override string ToString() {
		return this.assemblyQualifiedName.Span.Slice(this.indexBegin, this.indexLength).ToString();
	} // ToString
	#endregion

	#region Equals methods
	//------------------------------------------------------------------------------------------------------------------
	// Equals methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Compares this Assembly Qualified Name with the specified object.
	/// Note that the version, culture and public key are ignored when comparing.
	/// </summary>
	/// <param name="obj">The object to compare with.</param>
	/// <returns>True when this Assembly Qualified Name equals the specified Assembly Qualified Name.</returns>
	public override Boolean Equals(Object obj) {
		return this.Equals((obj as RpcAssemblyQualifiedName), true, true, true);
	} // Equals

	/// <summary>
	/// Compares this Assembly Qualified Name with the specified name.
	/// Note that the version, culture and public key are ignored when comparing.
	/// </summary>
	/// <param name="assemblyQualifiedName">The Assembly Qualified Name to compare with.</param>
	/// <returns>True when this Assembly Qualified Name equals the specified Assembly Qualified Name.</returns>
	public Boolean Equals(RpcAssemblyQualifiedName assemblyQualifiedName) {
		return this.Equals(assemblyQualifiedName, true, true, true);
	} // Equals

	/// <summary>
	/// Compares this Assembly Qualified Name with the specified name.
	/// Note that the culture and public key are ignored when comparing.
	/// </summary>
	/// <param name="assemblyQualifiedName">The Assembly Qualified Name to compare with.</param>
	/// <param name="ignoreVersion">Specify if the version should be ignored when comparing.</param>
	/// <returns>True when this Assembly Qualified Name equals the specified Assembly Qualified Name.</returns>
	public Boolean Equals(RpcAssemblyQualifiedName assemblyQualifiedName, Boolean ignoreVersion) {
		return this.Equals(assemblyQualifiedName, ignoreVersion, true, true);
	} // Equals

	/// <summary>
	/// Compares this Assembly Qualified Name with the specified name.
	/// </summary>
	/// <param name="assemblyQualifiedName">The Assembly Qualified Name to compare with.</param>
	/// <param name="ignoreVersion">Specify if the version should be ignored when comparing.</param>
	/// <param name="ignoreCulture">Specify if the culture should be ignored when comparing.</param>
	/// <param name="ignorePublicKey">Specify if the public key should be ignored when comparing.</param>
	/// <returns>True when this Assembly Qualified Name equals the specified Assembly Qualified Name.</returns>
	public Boolean Equals(RpcAssemblyQualifiedName assemblyQualifiedName, Boolean ignoreVersion, Boolean ignoreCulture, Boolean ignorePublicKey) {
		if (assemblyQualifiedName == null) {
			return false;
		}

		return (
			(this.typePart.Span.SequenceEqual(assemblyQualifiedName.typePart.Span) == true) &&
			(this.assemblyNamePart.Span.SequenceEqual(assemblyQualifiedName.assemblyNamePart.Span) == true) &&
			((ignoreVersion == true) || (this.assemblyVersionPart.Span.SequenceEqual(assemblyQualifiedName.assemblyVersionPart.Span) == true)) &&
			((ignoreCulture == true) || (this.assemblyCulturePart.Span.SequenceEqual(assemblyQualifiedName.assemblyCulturePart.Span) == true)) &&
			((ignorePublicKey == true) || (this.assemblyPublicKeyPart.Span.SequenceEqual(assemblyQualifiedName.assemblyPublicKeyPart.Span) == true))
		);
	} // Equals

	/// <summary>
	/// Compares two Assembly Qualified Name with each other.
	/// Note that the version, culture and public key are ignored when comparing.
	/// </summary>
	/// <param name="assemblyQualifiedNameA">The Assembly Qualified Name to compare.</param>
	/// <param name="assemblyQualifiedNameB">The Assembly Qualified Name to compare.</param>
	/// <returns>True when both Assembly Qualified Names are null, true when both Assembly Qualified Names are equal, otherwise false.</returns>
	public override Boolean Equals(RpcAssemblyQualifiedName assemblyQualifiedNameA, RpcAssemblyQualifiedName assemblyQualifiedNameB) {
		// Both are null.
		if ((assemblyQualifiedNameA == null) && (assemblyQualifiedNameB == null)) {
			return true;
		}

		// One are null.
		if ((assemblyQualifiedNameA == null) || (assemblyQualifiedNameB == null)) {
			return false;
		}

		// Compare.
		return assemblyQualifiedNameA.Equals(assemblyQualifiedNameB, true, true, true);
	} // Equals
	#endregion

	#region HashCode methods
	//------------------------------------------------------------------------------------------------------------------
	// HashCode methods.
	//------------------------------------------------------------------------------------------------------------------
	public override Int32 GetHashCode() {
		return this.GetHashCode(this);
	} // GetHashCode

	public override Int32 GetHashCode(RpcAssemblyQualifiedName assemblyQualifiedName) {
		if (assemblyQualifiedName == null) {
			return base.GetHashCode();
		}

		return HashCode.Combine(
			this.typePart,
			this.assemblyNamePart,
			this.assemblyVersionPart,
			this.assemblyCulturePart,
			this.assemblyPublicKeyPart
		);
	} // GetHashCode
	#endregion

	#region Array properties
	//------------------------------------------------------------------------------------------------------------------
	// Array properties.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets if this Assembly Qualified Name is a array type.
	/// </summary>
	public Boolean IsArray {
		get {
			return this.isArray > 0;
		}
	} // IsArray

	/// <summary>
	/// Gets the number of array dimentions.
	/// </summary>
	public Int32 ArrayDimentions {
		get {
			return this.isArray;
		}
	} // ArrayDimentions
	#endregion

	#region Generic properties
	//------------------------------------------------------------------------------------------------------------------
	// Generic properties.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets if this Assembly Qualified Name is a generic type.
	/// </summary>
	public Boolean IsGeneric {
		get {
			return this.isGeneric;
		}
	} // IsGeneric

	/// <summary>
	/// Gets the number of generic type arguments.
	/// </summary>
	public Int32 GenericTypeArgumentCount {
		get {
			return this.genericTypeArguments.Length;
		}
	} // GenericTypeArgumentCount

	/// <summary>
	/// Gets the generic type arguments.
	/// </summary>
	public RpcAssemblyQualifiedName[] GenericTypeArguments {
		get {
			return this.genericTypeArguments;
		}
	} // GenericTypeArguments
	#endregion

	#region BaseType methods
	//------------------------------------------------------------------------------------------------------------------
	// BaseType methods.
	//------------------------------------------------------------------------------------------------------------------
	private Type GetBaseType() {
		// TODO: Create and use cache.
		String typeName = this.typePart.ToString();
		Type type = Type.GetType(typeName, false);

		//
		if (type == null) {
			Assembly assembly = this.Assembly;
			if (assembly != null) {
				type = assembly.GetType(typeName, false);
			}
		}

		return type;
	} // getBaseType

	private Type GetGenericBaseType() {
		// TODO: Create and use cache.
		String typeName = $"{this.typePart}{CharBeginGenericCount}{this.genericTypeArguments.Length}";
		Type type = Type.GetType(typeName, false);

		//
		if (type == null) {
			Assembly assembly = this.Assembly;
			if (assembly != null) {
				type = assembly.GetType(typeName, false);
			}
		}

		return type;
	} // GetGenericBaseType
	#endregion

	#region Natural properties
	//------------------------------------------------------------------------------------------------------------------
	// Natural properties.
	//------------------------------------------------------------------------------------------------------------------
	/*
	public String BaseTypeName {
		get {
			return $"{this.typePart}, {this.assemblyNamePart}, Version={this.assemblyVersionPart}, Culture={this.assemblyCulturePart}, PublicKeyToken={this.assemblyPublicKeyPart}";
		}
	} //
	*/

	/// <summary>
	/// Gets the type information, or null if the type information can't be created.
	/// </summary>
	public TypeInfo TypeInfo {
		get {
			return this.Type?.GetTypeInfo() ?? null;
		}
	} // TypeInfo

	/// <summary>
	/// Gets the type, or null if the type can't be created.
	/// </summary>
	public Type Type {
		get {
			if (this.typePart.Length > 0) {
				Type type = null;

				// Create the array type.
				if (this.isArray == 1) {
					type = this.GetBaseType();
					type = type.MakeArrayType();
				} else if (this.isArray > 1) {
					type = this.GetBaseType();
					type = type.MakeArrayType(this.isArray);
				}

				// Create the generic type.
				if ((this.isGeneric == true) && (this.genericTypeArguments.Length > 0)) {
					type = this.GetGenericBaseType();
					type = type.MakeGenericType(
						this.genericTypeArguments
							.ToList()
							.ConvertAll<Type>((genericTypeArgument) => genericTypeArgument.Type)
							.ToArray()
					);
				}

				// Create the normal type.
				if ((this.isArray == 0) && (this.isGeneric == false)) {
					type = this.GetBaseType();
				}

				// Return the type.
				return type;
			} else {
				return null;
			}
		}
	} // Type

	/// <summary>
	/// Gets the assembly, or null if the assembly isn't loaded.
	/// </summary>
	public Assembly Assembly {
		get {
			if (this.assemblyNamePart.Length > 0) {
				String assemblyName = this.assemblyNamePart.Span.ToString();
				return AppDomain
					.CurrentDomain
					.GetAssemblies()
					.SingleOrDefault((assembly) => (assembly.GetName().Name == assemblyName));
			} else {
				return null;
			}
		}
	} // Assembly

	/// <summary>
	/// Gets the version, or null if the version information was missing.
	/// </summary>
	public Version Version {
		get {
			if (this.assemblyVersionPart.Length > 0) {
				return Version.Parse(this.assemblyVersionPart.Span);
			} else {
				return null;
			}
		}
	} // VersionSpan

	/// <summary>
	/// Gets the culture, or null if the culture information was missing.
	/// </summary>
	public CultureInfo Culture {
		get {
			if (this.assemblyCulturePart.Length > 0) {
				return CultureInfo.CreateSpecificCulture(this.assemblyCulturePart.Span.ToString());
			} else {
				return CultureInfo.InvariantCulture;
			}
		}
	} // Culture
	#endregion

	#region Span properties
	//------------------------------------------------------------------------------------------------------------------
	// Span properties.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the parsed type information.
	/// </summary>
	public ReadOnlySpan<Char> TypeSpan {
		get {
			return this.typePart.Span;
		}
	} // TypeSpan

	/// <summary>
	/// Gets the parsed assembly information.
	/// </summary>
	public ReadOnlySpan<Char> AssemblySpan {
		get {
			return this.assemblyNamePart.Span;
		}
	} // AssemblySpan

	/// <summary>
	/// Gets the parsed version information.
	/// </summary>
	public ReadOnlySpan<Char> VersionSpan {
		get {
			return this.assemblyVersionPart.Span;
		}
	} // VersionSpan

	/// <summary>
	/// Gets the parsed culture information.
	/// </summary>
	public ReadOnlySpan<Char> CultureSpan {
		get {
			return this.assemblyCulturePart.Span;
		}
	} // CultureSpan

	/// <summary>
	/// Gets the parsed public key information.
	/// </summary>
	public ReadOnlySpan<Char> PublicKeyTokenSpan {
		get {
			return this.assemblyPublicKeyPart.Span;
		}
	} // PublicKeyTokenSpan
	#endregion

	#region String properties
	//------------------------------------------------------------------------------------------------------------------
	// String properties.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the parsed type information.
	/// </summary>
	public String TypeString {
		get {
			return this.typePart.ToString();
		}
	} // TypeString

	/// <summary>
	/// Gets the parsed assembly information.
	/// </summary>
	public String AssemblyString {
		get {
			return this.assemblyNamePart.ToString();
		}
	} // AssemblyString

	/// <summary>
	/// Gets the parsed version information.
	/// </summary>
	public String VersionString {
		get {
			return this.assemblyVersionPart.ToString();
		}
	} // VersionString

	/// <summary>
	/// Gets the parsed culture information.
	/// </summary>
	public String CultureString {
		get {
			return this.assemblyCulturePart.ToString();
		}
	} // CultureString

	/// <summary>
	/// Gets the parsed public key information.
	/// </summary>
	public String PublicKeyTokenString {
		get {
			return this.assemblyPublicKeyPart.ToString();
		}
	} // PublicKeyTokenString
	#endregion

} // RpcAssemblyQualifiedName
#endregion
