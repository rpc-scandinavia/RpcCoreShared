namespace RpcScandinavia.Core.Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;

#region RpcTypeCache
//----------------------------------------------------------------------------------------------------------------------
// RpcTypeCache.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// This is a cache of types.
/// </summary>
public class RpcTypeCache {
	private Dictionary<String, Type> types;

	public RpcTypeCache() {
		this.types = new Dictionary<String, Type>();
	} // RpcTypeCache

	public void Add(RpcAssemblyQualifiedName assemblyQualifiedName, Type type) {
		if ((assemblyQualifiedName != null) &&
			(assemblyQualifiedName.TypeSpan.Length > 0) &&
			(this.types.ContainsKey(assemblyQualifiedName.ToString()) == false) &&
			(type != null)) {
			try {
				this.types.Add(assemblyQualifiedName.ToString(), type);
			} catch {}
		}
	} // Add

	public Type Get(RpcAssemblyQualifiedName assemblyQualifiedName) {
		if ((assemblyQualifiedName != null) &&
			(assemblyQualifiedName.TypeSpan.Length > 0)) {
			return this.Get(assemblyQualifiedName.ToString(), assemblyQualifiedName.Assembly);
		} else {
			return null;
		}
	} // Get

	public Type Get(String typeName) {
		return this.Get(typeName, null);
	} // Get

	public Type Get(String typeName, Assembly assembly) {
		if ((typeName == null) || (typeName.Length == 0)) {
			return null;
		}

		// Get the type from the cache.
		if (this.types.ContainsKey(typeName) == true) {
			return this.types[typeName];
		}

		// Get the type from the "hint" assembly.
		if (assembly != null) {
			Type type = assembly.GetType(typeName, false);
			if (type != null) {
				// Add the type to the cache.
				try {
					this.types.Add(typeName, type);
				} catch {}

				// Return the type.
				return type;
			}
		}

		// Get the type from all loaded assemblies.
		RpcAssemblyQualifiedName assemblyQualifiedName = new RpcAssemblyQualifiedName(typeName);
		TypeInfo typeInfo = RpcTypeCache.GetAllDomainTypes(assemblyQualifiedName, true, false).FirstOrDefault();
		if (typeInfo != null) {
			// Add the type to the cache.
			try {
				this.types.Add(typeName, typeInfo.AsType());
			} catch {}

			// Return the type.
			return typeInfo.AsType();
		}

		// The type was not found.
		return null;
	} // Get

	/// <summary>
	/// Gets an enumerator with all types from the domain, that matches the type name.
	/// </summary>
	/// <param name="assemblyQualifiedName">The assembly qualified type name.</param>
	/// <param name="ignoreVersion">Only compares the first two parts of the type name (class name and assembly name).</param>
	/// <param name="throwExceptions">Throw exceptions (true).</param>
	/// <returns>The enumerator.</returns>
	public static IEnumerable<TypeInfo> GetAllDomainTypes(RpcAssemblyQualifiedName assemblyQualifiedName, Boolean ignoreVersion, Boolean throwExceptions = true) {
		// Iterate through all available assemblies in the current application domain.
		foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) {
			foreach (TypeInfo typeInfo in RpcTypeCache.GetAllAssemblyTypes(assembly, assemblyQualifiedName, ignoreVersion, throwExceptions)) {
				yield return typeInfo;
			}
		}
	} // GetAllDomainTypes

	/// <summary>
	/// Gets an enumerator with all types from the assembly, that matches the type name.
	/// </summary>
	/// <param name="assembly">The assembly.</param>
	/// <param name="assemblyQualifiedName">The assembly qualified type name.</param>
	/// <param name="ignoreVersion">Only compares the first two parts of the type name (class name and assembly name).</param>
	/// <param name="throwExceptions">Throw exceptions (true).</param>
	/// <returns>The enumerator.</returns>
	public static IEnumerable<TypeInfo> GetAllAssemblyTypes(Assembly assembly, RpcAssemblyQualifiedName assemblyQualifiedName, Boolean ignoreVersion, Boolean throwExceptions = true) {
		if (throwExceptions == true) {
			foreach (TypeInfo typeInfo in assembly.DefinedTypes) {
				if (assemblyQualifiedName.Equals(typeInfo, ignoreVersion, ignoreVersion, ignoreVersion) == true) {
					yield return typeInfo;
				}
			}
		} else {
			IEnumerable<TypeInfo> definedTypes = Enumerable.Empty<TypeInfo>();
			try {
				definedTypes = assembly.DefinedTypes;
			} catch {}

			foreach (TypeInfo typeInfo in definedTypes) {
				if (assemblyQualifiedName.Equals(typeInfo, ignoreVersion, ignoreVersion, ignoreVersion) == true) {
					yield return typeInfo;
				}
			}
		}
	} // GetAllAssemblyTypes

} // RpcTypeCache
#endregion
