using System;
using System.Collections.Generic;
namespace RpcScandinavia.Core.Shared;

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
		return assemblyQualifiedNameA.EqualsType(assemblyQualifiedNameB, ignoreVersion, ignoreCulture, ignorePublicKey);
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
