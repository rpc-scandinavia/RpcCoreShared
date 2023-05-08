namespace RpcScandinavia.Core.Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

#region RpcAssemblyQualifiedName
//----------------------------------------------------------------------------------------------------------------------
// RpcAssemblyQualifiedName.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// This is a Assembly Qualified Name parser.
/// It uses a <see cref="System.Memory{System.Char}" /> to parse the Assembly Qualified Name.
/// </summary>
public class RpcAssemblyQualifiedName {
	private const Char CharSeparator = ',';
	private const Char CharBeginArray = '[';
	private const Char CharEndArray = ']';
	private const Char CharBeginGenericCount = '`';
	private const Char CharBeginGeneric = '[';
	private const Char CharEndGeneric = ']';
	private const String StrVersion = "Version=";
	private const String StrCulture = "Culture=";
	private const String StrPublicKeyToken = "PublicKeyToken=";
	private const String StrPublicKeyTokenEmpty = "null";

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
		// Strange examples:
		//		System.Array+EmptyArray`1, System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
		//
		// The Assembly Qualified Name contains five parts of information:
		//		System.Guid, System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
		//		0            1                       2                3                4
		//
		//		0 = Type name with the namespace and additional type arguments.
		//		1 = Assembly name.
		//		2 = Assembly version.
		//		3 = Assembly culture.
		//		4 = Assembly public key token.
		//

		// Parse the Assembly Qualified Name:
		// 1)	Find the end of the type name. This is one of these characters (",", "[", "`").
		// 2a)	Recursively get the generic type arguments. They are wrapped in "[[" and "]]"-
		// 2b)	Get the dimensions of the array, by counting "," characters untill the "]" character.
		// 3)	Find the end of the assembly name (optional).
		// 4)	Find the end of the assembly version (optional).
		// 5)	Find the end of the assembly culture (optional).
		// 6)	Find the end of the assembly public key token (optional).

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
			Boolean beginGenericFound = false;
			Int32 numberIndex = index;
			Int32 numberLength = 0;
			while (index < this.assemblyQualifiedName.Length) {
				if ((this.assemblyQualifiedName.Span[index] == RpcCoreSharedConstants.CHAR_0) ||
					(this.assemblyQualifiedName.Span[index] == RpcCoreSharedConstants.CHAR_1) ||
					(this.assemblyQualifiedName.Span[index] == RpcCoreSharedConstants.CHAR_2) ||
					(this.assemblyQualifiedName.Span[index] == RpcCoreSharedConstants.CHAR_3) ||
					(this.assemblyQualifiedName.Span[index] == RpcCoreSharedConstants.CHAR_4) ||
					(this.assemblyQualifiedName.Span[index] == RpcCoreSharedConstants.CHAR_5) ||
					(this.assemblyQualifiedName.Span[index] == RpcCoreSharedConstants.CHAR_6) ||
					(this.assemblyQualifiedName.Span[index] == RpcCoreSharedConstants.CHAR_7) ||
					(this.assemblyQualifiedName.Span[index] == RpcCoreSharedConstants.CHAR_8) ||
					(this.assemblyQualifiedName.Span[index] == RpcCoreSharedConstants.CHAR_9)) {
					// Found number.
					numberLength++;

					// Iterate.
					index++;
				} else if (this.assemblyQualifiedName.Span[index] == CharBeginGeneric) {
					// Found generic type.
					beginGenericFound = true;

					// Iterate.
					index++;

					break;
				} else if (this.assemblyQualifiedName.Span[index] == CharSeparator) {
					// Missing generic arguments.
					beginGenericFound = false;

					// Iterate.
					index++;

					break;
				} else {
					// Ignore all other characters.
					// Iterate.
					index++;
					//throw new Exception($"Expected generic type parameter count at index {index} in '{this.assemblyQualifiedName.Span}'.");
				}
			}

			if ((beginGenericFound == true) && (numberLength > 0)) {
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
						this.ParseTrim(ref index, RpcCoreSharedConstants.CHAR_SPACE);
					}
				}

				// Iterate second end generic characters.
				this.ParseTrim(ref index, CharEndGeneric);
			}
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
					// Ignore all other characters.
					// Iterate.
					index++;
					//throw new Exception($"Expected array dimension '{CharSeparator}' character or end of array '{CharEndArray}' character at index {index} in '{this.assemblyQualifiedName.Span}'.");
				}
			}
		}

		// Iterate separator and whitespace characters.
		this.ParseTrim(ref index, CharSeparator);
		this.ParseTrim(ref index, RpcCoreSharedConstants.CHAR_SPACE);

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
			this.ParseTrim(ref index, RpcCoreSharedConstants.CHAR_SPACE);

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
	/// </summary>
	/// <param name="obj">The object to compare with.</param>
	/// <returns>True when this Assembly Qualified Name equals the specified Assembly Qualified Name.</returns>
	public override Boolean Equals(Object obj) {
		return this.Equals((obj as RpcAssemblyQualifiedName), false, false, false);
	} // Equals

	/// <summary>
	/// Compares this Assembly Qualified Name with the specified name.
	/// </summary>
	/// <param name="assemblyQualifiedName">The Assembly Qualified Name to compare with.</param>
	/// <returns>True when this Assembly Qualified Name equals the specified Assembly Qualified Name.</returns>
	public Boolean Equals(RpcAssemblyQualifiedName assemblyQualifiedName) {
		return this.Equals(assemblyQualifiedName, false, false, false);
	} // Equals

	/// <summary>
	/// Compares this Assembly Qualified Name with the specified name.
	/// </summary>
	/// <param name="assemblyQualifiedName">The Assembly Qualified Name to compare with.</param>
	/// <param name="ignoreVersion">Specify if the version should be ignored when comparing.</param>
	/// <param name="ignoreCulture">Specify if the culture should be ignored when comparing.</param>
	/// <param name="ignorePublicKey">Specify if the public key token should be ignored when comparing.</param>
	/// <returns>True when this Assembly Qualified Name equals the specified Assembly Qualified Name.</returns>
	public Boolean Equals(RpcAssemblyQualifiedName assemblyQualifiedName, Boolean ignoreVersion, Boolean ignoreCulture, Boolean ignorePublicKey) {
		if (assemblyQualifiedName == null) {
			return false;
		}

		return (
			(this.typePart.Span.SequenceEqual(assemblyQualifiedName.typePart.Span) == true) &&
			(this.isArray == assemblyQualifiedName.isArray) &&
			(this.isGeneric == assemblyQualifiedName.isGeneric) &&
			(this.genericTypeArguments.SequenceEqual(assemblyQualifiedName.genericTypeArguments, new RpcAssemblyQualifiedNameEqualityComparer(ignoreVersion, ignoreCulture, ignorePublicKey)) == true) &&
			(this.assemblyNamePart.Span.SequenceEqual(assemblyQualifiedName.assemblyNamePart.Span) == true) &&
			((ignoreVersion == true) || (this.assemblyVersionPart.Span.SequenceEqual(assemblyQualifiedName.assemblyVersionPart.Span) == true)) &&
			((ignoreCulture == true) || (this.assemblyCulturePart.Span.SequenceEqual(assemblyQualifiedName.assemblyCulturePart.Span) == true)) &&
			((ignorePublicKey == true) || (this.assemblyPublicKeyPart.Span.SequenceEqual(assemblyQualifiedName.assemblyPublicKeyPart.Span) == true))
		);
	} // Equals
	#endregion

	#region HashCode methods
	//------------------------------------------------------------------------------------------------------------------
	// HashCode methods.
	//------------------------------------------------------------------------------------------------------------------
	public override Int32 GetHashCode() {
		return HashCode.Combine(
			this.typePart,
			this.assemblyNamePart,
			this.assemblyVersionPart,
			this.assemblyCulturePart,
			this.assemblyPublicKeyPart
		);
	} // GetHashCode

	public Int32 GetHashCode(Boolean ignoreVersion, Boolean ignoreCulture, Boolean ignorePublicKey) {
		switch (ignoreVersion, ignoreCulture, ignorePublicKey) {
			case (true, true, true): return HashCode.Combine(
				this.typePart,
				this.assemblyNamePart,
				this.assemblyVersionPart,
				this.assemblyCulturePart,
				this.assemblyPublicKeyPart
			);

			case (true, true, false): return HashCode.Combine(
				this.typePart,
				this.assemblyNamePart,
				this.assemblyVersionPart,
				this.assemblyCulturePart
			);

			case (true, false, false): return HashCode.Combine(
				this.typePart,
				this.assemblyNamePart,
				this.assemblyVersionPart
			);

			case (false, true, true): return HashCode.Combine(
				this.typePart,
				this.assemblyNamePart,
				this.assemblyCulturePart,
				this.assemblyPublicKeyPart
			);

			case (false, true, false): return HashCode.Combine(
				this.typePart,
				this.assemblyNamePart,
				this.assemblyCulturePart
			);

			case (true, false, true): return HashCode.Combine(
				this.typePart,
				this.assemblyNamePart,
				this.assemblyVersionPart,
				this.assemblyPublicKeyPart
			);

			case (false, false, true): return HashCode.Combine(
				this.typePart,
				this.assemblyNamePart,
				this.assemblyPublicKeyPart
			);

			case (false, false, false): return HashCode.Combine(
				this.typePart,
				this.assemblyNamePart
			);
		}
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
		String typeName = this.typePart.ToString();
		Type type = Type.GetType(typeName, false);

		// Try to get the type using the assembly.
		if (type == null) {
			Assembly assembly = this.Assembly;
			if (assembly != null) {
				type = assembly.GetType(typeName, false);
			}
		}

		// Try to get the type using the RPC Activator.

		return type;
	} // getBaseType

	private Type GetGenericBaseType() {
		String typeName = $"{this.typePart}{CharBeginGenericCount}{this.genericTypeArguments.Length}";
		Type type = Type.GetType(typeName, false);

		// Try to get the type using the assembly.
		if (type == null) {
			Assembly assembly = this.Assembly;
			if (assembly != null) {
				type = assembly.GetType(typeName, false);
			}
		}

		// Try to get the type using the RPC Activator.

		return type;
	} // GetGenericBaseType
	#endregion

	#region Properties
	//------------------------------------------------------------------------------------------------------------------
	// Properties.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets a value indicating whether or not the assembly qualified name is empty.
	/// </summary>
	public Boolean IsEmpty {
		get {
			return (
				(this.typePart.Length == 0) &&
				(this.assemblyNamePart.Length == 0)
				/*
					assemblyQualifiedName;
					typePart;
					assemblyNamePart;
					assemblyVersionPart;
					assemblyCulturePart;
					assemblyPublicKeyPart;
				*/
			);
		}
	} // IsEmpty

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
	#endregion

	#region Natural properties
	//------------------------------------------------------------------------------------------------------------------
	// Natural properties.
	//------------------------------------------------------------------------------------------------------------------
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
	} // Version

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

	/// <summary>
	/// Gets the parsed public key token, which is the last 8 bytes of the SHA-1 hash of the public key under which the
	/// assembly is signed.
	/// </summary>
	public Byte[] PublicKeyToken {
		get {
			if (this.assemblyPublicKeyPart.Span.Length == 16) {
				Byte[] token = new Byte[8];
				token[0] = Convert.ToByte(this.assemblyPublicKeyPart.Span.Slice(00, 2).ToString(), 16);
				token[1] = Convert.ToByte(this.assemblyPublicKeyPart.Span.Slice(02, 2).ToString(), 16);
				token[2] = Convert.ToByte(this.assemblyPublicKeyPart.Span.Slice(04, 2).ToString(), 16);
				token[3] = Convert.ToByte(this.assemblyPublicKeyPart.Span.Slice(06, 2).ToString(), 16);
				token[4] = Convert.ToByte(this.assemblyPublicKeyPart.Span.Slice(08, 2).ToString(), 16);
				token[5] = Convert.ToByte(this.assemblyPublicKeyPart.Span.Slice(10, 2).ToString(), 16);
				token[6] = Convert.ToByte(this.assemblyPublicKeyPart.Span.Slice(12, 2).ToString(), 16);
				token[7] = Convert.ToByte(this.assemblyPublicKeyPart.Span.Slice(14, 2).ToString(), 16);
				return token;
			} else {
				return Array.Empty<Byte>();
			}
		}
	} // PublicKeyToken
	#endregion

	#region Memory properties
	//------------------------------------------------------------------------------------------------------------------
	// Memory properties.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the parsed type information.
	/// </summary>
	internal ReadOnlyMemory<Char> TypePart {
		get {
			return this.typePart;
		}
	} // TypePart

	/// <summary>
	/// Gets the parsed assembly information.
	/// </summary>
	internal ReadOnlyMemory<Char> AssemblyPart {
		get {
			return this.assemblyNamePart;
		}
	} // AssemblyPart

	/// <summary>
	/// Gets the parsed version information.
	/// </summary>
	internal ReadOnlyMemory<Char> VersionPart {
		get {
			return this.assemblyVersionPart;
		}
	} // VersionPart

	/// <summary>
	/// Gets the parsed culture information.
	/// </summary>
	internal ReadOnlyMemory<Char> CulturePart {
		get {
			return this.assemblyCulturePart;
		}
	} // CulturePart

	/// <summary>
	/// Gets the parsed public key token information.
	/// </summary>
	internal ReadOnlyMemory<Char> PublicKeyTokenPart {
		get {
			if (this.assemblyPublicKeyPart.Span.Length == 16) {
				return this.assemblyPublicKeyPart;
			} else {
				return ReadOnlyMemory<Char>.Empty;
			}
		}
	} // PublicKeyTokenPart
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
	/// Gets the parsed public key token information.
	/// </summary>
	public ReadOnlySpan<Char> PublicKeyTokenSpan {
		get {
			if (this.assemblyPublicKeyPart.Span.Length == 16) {
				return this.assemblyPublicKeyPart.Span;
			} else {
				return ReadOnlySpan<Char>.Empty;
			}
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
	/// Gets the parsed public key token information.
	/// </summary>
	public String PublicKeyTokenString {
		get {
			if (this.assemblyPublicKeyPart.Span.Length == 16) {
				return this.assemblyPublicKeyPart.ToString();
			} else {
				return String.Empty;
			}
		}
	} // PublicKeyTokenString
	#endregion

	#region Compare methods
	//------------------------------------------------------------------------------------------------------------------
	// Compare methods.
	//------------------------------------------------------------------------------------------------------------------
	public Boolean Equals(Type type, Boolean ignoreVersion, Boolean ignoreCulture, Boolean ignorePublicKey) {
		if (type == null) {
			return false;
		}

		return this.Equals(new RpcAssemblyQualifiedName(type.AssemblyQualifiedName), ignoreVersion, ignoreCulture, ignorePublicKey);
	} // Equals

	public Boolean Equals(TypeInfo typeInfo, Boolean ignoreVersion, Boolean ignoreCulture, Boolean ignorePublicKey) {
		if (typeInfo == null) {
			return false;
		}

//Console.WriteLine($"EQUALS: '{typeInfo.AssemblyQualifiedName}'  AND  '{typeInfo.AsType().AssemblyQualifiedName}'");
		return this.Equals(new RpcAssemblyQualifiedName(typeInfo.AssemblyQualifiedName), ignoreVersion, ignoreCulture, ignorePublicKey);
/*
		// Compare Qualified Name (namespace and type name).
		if (typeInfo.FullName != this.TypeString) {
			return false;
		}

		// Compate array type.
		if (typeInfo.IsArray != this.IsArray) {
			return false;
		}
		// TODO: typeInfo.GetElementType()

		// Compate generic type.
		if (typeInfo.IsGenericType != this.IsGeneric) {
			return false;
		}
		// TODO: Generic type params match.

		// Compare Assembly Name.
		if (typeInfo.Assembly.GetName().Name != this.AssemblyString) {
			return false;
		}

		// Ignore version.
		if (ignoreVersion == true) {
			return true;
		}

		// Compare version.
		if (typeInfo.Assembly.GetName().Version != this.Version) {
			return false;
		}

		// Compare culture.
		if (typeInfo.Assembly.GetName().CultureInfo != this.Culture) {
			return false;
		}

		// Compare public key.
		if (typeInfo.Assembly.GetName().GetPublicKeyToken().SequenceEqual(this.PublicKeyToken) == false) {
			return false;
		}

		// Total match.
		return true;
*/
	} // Equals
	#endregion

} // RpcAssemblyQualifiedName
#endregion

#region RpcAssemblyQualifiedNameEqualityComparer
//----------------------------------------------------------------------------------------------------------------------
// RpcAssemblyQualifiedNameEqualityComparer.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// This is a Assembly Qualified Name equality comparer.
/// It can compare all name parts, or ignore some name parts like version, culture and public key token.
/// </summary>
public class RpcAssemblyQualifiedNameEqualityComparer : IEqualityComparer<RpcAssemblyQualifiedName> {
	private Boolean ignoreVersion;
	private Boolean ignoreCulture;
	private Boolean ignorePublicKey;

	#region Constructors
	//------------------------------------------------------------------------------------------------------------------
	// Constructors.
	//------------------------------------------------------------------------------------------------------------------
	public RpcAssemblyQualifiedNameEqualityComparer() {
		this.ignoreVersion = true;
		this.ignoreCulture = true;
		this.ignorePublicKey = true;
	} // RpcAssemblyQualifiedNameEqualityComparer

	public RpcAssemblyQualifiedNameEqualityComparer(Boolean ignoreVersion, Boolean ignoreCulture, Boolean ignorePublicKey) {
		this.ignoreVersion = ignoreVersion;
		this.ignoreCulture = ignoreCulture;
		this.ignorePublicKey = ignorePublicKey;
	} // RpcAssemblyQualifiedNameEqualityComparer
	#endregion

	#region Equals methods
	//------------------------------------------------------------------------------------------------------------------
	// Equals methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Compares two Assembly Qualified Name with each other.
	/// </summary>
	/// <param name="assemblyQualifiedNameA">The Assembly Qualified Name to compare.</param>
	/// <param name="assemblyQualifiedNameB">The Assembly Qualified Name to compare.</param>
	/// <returns>True when both Assembly Qualified Names are null, true when both Assembly Qualified Names are equal, otherwise false.</returns>
	public Boolean Equals(RpcAssemblyQualifiedName assemblyQualifiedNameA, RpcAssemblyQualifiedName assemblyQualifiedNameB) {
		// Both are null.
		if ((assemblyQualifiedNameA == null) && (assemblyQualifiedNameB == null)) {
			return true;
		}

		// One are null.
		if ((assemblyQualifiedNameA == null) || (assemblyQualifiedNameB == null)) {
			return false;
		}

		// Compare.
		return assemblyQualifiedNameA.Equals(assemblyQualifiedNameB, ignoreVersion, ignoreCulture, ignorePublicKey);
	} // Equals
	#endregion

	#region HashCode methods
	//------------------------------------------------------------------------------------------------------------------
	// HashCode methods.
	//------------------------------------------------------------------------------------------------------------------
	public Int32 GetHashCode(RpcAssemblyQualifiedName assemblyQualifiedName) {
		if (assemblyQualifiedName != null) {
			return assemblyQualifiedName.GetHashCode(ignoreVersion, ignoreCulture, ignorePublicKey);
		} else {
			return 0;
		}
	} // GetHashCode
	#endregion

} // RpcAssemblyQualifiedNameEqualityComparer
#endregion
