using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
namespace RpcScandinavia.Core.Shared;

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

	#region Parse methods
	//------------------------------------------------------------------------------------------------------------------
	// Parse methods.
	//------------------------------------------------------------------------------------------------------------------
	private void Parse() {
		// Examples of Assembly Qualified Name strings:
		//		System.Guid, System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
		//		System.String[], System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
		//		System.String[,], System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
		//		System.Collections.Generic.List`1[[System.String, System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]][], System.Private.CoreLib, Version=8.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e
		//		RpcScandinavia.Core.Shared.Tests.DataGeneric`1[[System.Int32, System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]][], RpcCoreShared.test, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
		//		RpcScandinavia.Core.Shared.Tests.DataGeneric`1[[System.String, System.Private.CoreLib, Version=7.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]], RpcCoreShared.test, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
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
		// 2)	Recursively get the generic type arguments. They are wrapped in "[[" and "]]"-
		// 3)	Get the dimensions of the array, by counting "," characters until the "]" character.
		// 4)	Find the end of the assembly name (optional).
		// 5)	Find the end of the assembly version (optional).
		// 6)	Find the end of the assembly culture (optional).
		// 7)	Find the end of the assembly public key token (optional).

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

		// 2.
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
					// Iterate begin generic characters.
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

		// 3a.
		// This might be a array of generic type like "List<String>()".
		if ((this.isGeneric == true) && (index < this.assemblyQualifiedName.Length) && (this.assemblyQualifiedName.Span[index] == CharBeginArray)) {
			// Found the beginning of array type.
			this.isArray = 1;

			// Iterate.
			index++;
		}

		// 3b.
		if (this.isArray > 0) {
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

		// 4.
		(partIndex, partLength, partExitChar) = this.ParsePart(ref index, String.Empty, CharSeparator, CharEndGeneric, Char.MinValue);
		if (partLength > 0) {
			this.assemblyNamePart = this.assemblyQualifiedName.Slice(partIndex, partLength).Trim();
		}

		// 5.
		(partIndex, partLength, partExitChar) = this.ParsePart(ref index, StrVersion, CharSeparator, CharEndGeneric, Char.MinValue);
		if (partLength > 0) {
			this.assemblyVersionPart = this.assemblyQualifiedName.Slice(partIndex, partLength).Trim();
		}

		// 6.
		(partIndex, partLength, partExitChar) = this.ParsePart(ref index, StrCulture, CharSeparator, CharEndGeneric, Char.MinValue);
		if (partLength > 0) {
			this.assemblyCulturePart = this.assemblyQualifiedName.Slice(partIndex, partLength).Trim();
		}

		// 7.
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
		return (index, 0, Char.MinValue);
	} // ParsePart

	private void ParseTrim(ref Int32 index, Char trimChar) {
		// Iterate whitespace characters.
		while ((index < this.assemblyQualifiedName.Length) && (this.assemblyQualifiedName.Span[index] == trimChar)) {
			// Iterate.
			index++;
		}
	} // ParseTrim
	#endregion

	#region Equals and EqualsType methods
	//------------------------------------------------------------------------------------------------------------------
	// Equals and EqualsType methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Compares this Assembly Qualified Name with the specified object.
	/// </summary>
	/// <param name="obj">The object to compare with.</param>
	/// <returns>True when this Assembly Qualified Name equals the specified object.</returns>
	public override Boolean Equals(Object obj) {
		if (obj == null) {
			return false;
		}

		if (obj is RpcAssemblyQualifiedName assemblyQualifiedName) {
			return this.EqualsType(assemblyQualifiedName, false, false, false);
		}

		return false;
	} // Equals

	/// <summary>
	/// Compares this Assembly Qualified Name with the specified type.
	/// </summary>
	/// <param name="type">The type to compare with.</param>
	/// <param name="ignoreVersion">Specify if the version should be ignored when comparing (false).</param>
	/// <param name="ignoreCulture">Specify if the culture should be ignored when comparing (false).</param>
	/// <param name="ignorePublicKey">Specify if the public key token should be ignored when comparing (false).</param>
	/// <returns>True when this Assembly Qualified Name equals the specified type.</returns>
	public Boolean EqualsType(Type type, Boolean ignoreVersion = false, Boolean ignoreCulture = false, Boolean ignorePublicKey = false) {
		if (type == null) {
			return false;
		}

		return this.EqualsType(new RpcAssemblyQualifiedName(type.AssemblyQualifiedName), ignoreVersion, ignoreCulture, ignorePublicKey);
	} // EqualsType

	/// <summary>
	/// Compares this Assembly Qualified Name with the specified type info.
	/// </summary>
	/// <param name="typeInfo">The type info to compare with.</param>
	/// <param name="ignoreVersion">Specify if the version should be ignored when comparing (false).</param>
	/// <param name="ignoreCulture">Specify if the culture should be ignored when comparing (false).</param>
	/// <param name="ignorePublicKey">Specify if the public key token should be ignored when comparing (false).</param>
	/// <returns>True when this Assembly Qualified Name equals the specified type info.</returns>
	public Boolean EqualsType(TypeInfo typeInfo, Boolean ignoreVersion = false, Boolean ignoreCulture = false, Boolean ignorePublicKey = false) {
		if (typeInfo == null) {
			return false;
		}

		return this.EqualsType(new RpcAssemblyQualifiedName(typeInfo.AssemblyQualifiedName), ignoreVersion, ignoreCulture, ignorePublicKey);
	} // EqualsType

	/// <summary>
	/// Compares this Assembly Qualified Name with the specified object's type.
	/// </summary>
	/// <param name="obj">The object to compare with.</param>
	/// <param name="ignoreVersion">Specify if the version should be ignored when comparing (false).</param>
	/// <param name="ignoreCulture">Specify if the culture should be ignored when comparing (false).</param>
	/// <param name="ignorePublicKey">Specify if the public key token should be ignored when comparing (false).</param>
	/// <returns>True when this Assembly Qualified Name equals the specified object's type.</returns>
	public Boolean EqualsType(Object obj, Boolean ignoreVersion, Boolean ignoreCulture, Boolean ignorePublicKey) {
		if (obj == null) {
			return false;
		}

		return this.EqualsType(new RpcAssemblyQualifiedName(obj.GetType()), ignoreVersion, ignoreCulture, ignorePublicKey);
	} // EqualsType

	/// <summary>
	/// Compares this Assembly Qualified Name with the specified name.
	/// </summary>
	/// <param name="assemblyQualifiedName">The Assembly Qualified Name to compare with.</param>
	/// <param name="ignoreVersion">Specify if the version should be ignored when comparing (false).</param>
	/// <param name="ignoreCulture">Specify if the culture should be ignored when comparing (false).</param>
	/// <param name="ignorePublicKey">Specify if the public key token should be ignored when comparing (false).</param>
	/// <returns>True when this Assembly Qualified Name equals the specified Assembly Qualified Name.</returns>
	public Boolean EqualsType(RpcAssemblyQualifiedName assemblyQualifiedName, Boolean ignoreVersion = false, Boolean ignoreCulture = false, Boolean ignorePublicKey = false) {
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
	} // EqualsType
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
	/// Tries to get the type from either the <see cref="System.Type.GetType"/> method or by iterating all assemblies.
	/// </summary>
	private Type GetType(String typeName) {
		// Get the type from the Activator.
		Type type = Type.GetType(typeName, false);
		if (type != null) {
			return type;
		}

		// Iterate all assemblies in the AppDomain, and see if a type matches.
		foreach (TypeInfo typeInfo in RpcActivator.GetAllDomainTypes(false)) {
			if (typeInfo.AssemblyQualifiedName.StartsWith(typeName) == true) {
				RpcAssemblyQualifiedName typeA = new RpcAssemblyQualifiedName(typeName);
				RpcAssemblyQualifiedName typeB = new RpcAssemblyQualifiedName(typeInfo);
				//if (typeA.typePart.Span.SequenceEqual(typeB.typePart.Span) == true) {
				if (typeA.EqualsType(typeB, true, true, true) == true) {
					return typeInfo.AsType();
				}
			}
		}

		// The type was not found.
		return null;
	} // GetType

	/// <summary>
	/// Gets the type, or null if the type can't be created.
	/// This can only return the Type, if the parsed Assembly Qualified Name is that of an existing type.
	/// If for instance the version number is wrong, this property can not create the type, and wil return null.
	/// </summary>
	public Type Type {
		get {
			// Get the type.
			Type type = (this.isGeneric == false)
				? this.GetType(this.typePart.ToString())
				: this.GetType(this.ToString(false, false, false, false));

			if (type != null) {
				// Create the generic type.
				// This must be done first, in case the type is an array of generic type like "List<String>()".
				if ((this.isGeneric == true) && (this.genericTypeArguments.Length > 0)) {
					Type[] genericTypes = new Type[this.genericTypeArguments.Length];
					for (Int32 index = 0; index < this.genericTypeArguments.Length; index++) {
						genericTypes[index] = this.genericTypeArguments[index].Type;
					}
					type = type.MakeGenericType(genericTypes);
				}

				// Create the one dimensional array type.
				if (this.isArray == 1) {
					// Create the one dimensional array type.
					type = type.MakeArrayType();
				}

				// Create the multidimensional array type.
				if (this.isArray > 1) {
					type = type.MakeArrayType(this.isArray);
				}
			}

			// Return the type.
			return type;
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
	/// Gets the parsed assembly qualified name.
	/// </summary>
	public ReadOnlyMemory<Char> AssemblyQualifiedNamePart {
		get {
			return this.assemblyQualifiedName.Slice(this.indexBegin, this.indexLength);
		}
	} // AssemblyQualifiedNamePart

	/// <summary>
	/// Gets the parsed type information.
	/// </summary>
	public ReadOnlyMemory<Char> TypePart {
		get {
			return this.typePart;
		}
	} // TypePart

	/// <summary>
	/// Gets the parsed assembly information.
	/// </summary>
	public ReadOnlyMemory<Char> AssemblyPart {
		get {
			return this.assemblyNamePart;
		}
	} // AssemblyPart

	/// <summary>
	/// Gets the parsed version information.
	/// </summary>
	public ReadOnlyMemory<Char> VersionPart {
		get {
			return this.assemblyVersionPart;
		}
	} // VersionPart

	/// <summary>
	/// Gets the parsed culture information.
	/// </summary>
	public ReadOnlyMemory<Char> CulturePart {
		get {
			return this.assemblyCulturePart;
		}
	} // CulturePart

	/// <summary>
	/// Gets the parsed public key token information.
	/// </summary>
	public ReadOnlyMemory<Char> PublicKeyTokenPart {
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
	/// Gets the parsed assembly qualified name.
	/// </summary>
	public ReadOnlySpan<Char> AssemblyQualifiedNameSpan {
		get {
			return this.assemblyQualifiedName.Slice(this.indexBegin, this.indexLength).Span;
		}
	} // AssemblyQualifiedNameSpan

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

	#region String properties/methods
	//------------------------------------------------------------------------------------------------------------------
	// String properties/methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the string representation of this Assembly Qualified Name.
	/// </summary>
	public override string ToString() {
		return this.assemblyQualifiedName.Slice(this.indexBegin, this.indexLength).ToString();
	} // ToString

	/// <summary>
	/// Gets the string representation of this Assembly Qualified Name.
	/// </summary>
	/// <param name="ignoreAssembly">Specify if the assembly name should be ignored when building the string.</param>
	/// <param name="ignoreVersion">Specify if the version should be ignored when building the string.</param>
	/// <param name="ignoreCulture">Specify if the culture should be ignored when building the string.</param>
	/// <param name="ignorePublicKey">Specify if the public key token should be ignored when building the string.</param>
	public String ToString(Boolean ignoreAssembly, Boolean ignoreVersion, Boolean ignoreCulture, Boolean ignorePublicKey) {
		StringBuilder result = new StringBuilder();
		result.Append($"{this.typePart}{CharBeginGenericCount}{this.genericTypeArguments.Length}");

		if ((ignoreAssembly == false) && (this.assemblyNamePart.Length > 0)) {
			result.Append($", {this.assemblyNamePart}");
		}

		if ((ignoreVersion == false) && (this.assemblyVersionPart.Length > 0)) {
			result.Append($", {StrVersion}{this.assemblyVersionPart}");
		}

		if ((ignoreCulture == false) && (this.assemblyCulturePart.Length > 0)) {
			result.Append($", {StrCulture}{this.assemblyCulturePart}");
		}

		if ((ignorePublicKey == false) && (this.assemblyPublicKeyPart.Length > 0)) {
			result.Append($", {StrPublicKeyToken}{this.assemblyPublicKeyPart}");
		}

		return result.ToString();
	} // ToString

	/// <summary>
	/// Gets the parsed assembly qualified name.
	/// </summary>
	public String AssemblyQualifiedNameString {
		get {
			return this.assemblyQualifiedName.Slice(this.indexBegin, this.indexLength).ToString();
		}
	} // AssemblyQualifiedNameString

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

} // RpcAssemblyQualifiedName
#endregion
