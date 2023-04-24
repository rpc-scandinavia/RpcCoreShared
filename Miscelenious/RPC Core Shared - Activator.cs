namespace RpcScandinavia.Core.Shared;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading;

#region RpcActivator
//----------------------------------------------------------------------------------------------------------------------
// RpcActivator.
//----------------------------------------------------------------------------------------------------------------------
public static class RpcActivator {
	private static Dictionary<String, List<TypeInfo>> typeCache = new Dictionary<String, List<TypeInfo>>();
	private static SemaphoreSlim typeCacheAddSemaphore = new SemaphoreSlim(1, 1);

	#region Cache methods
	//------------------------------------------------------------------------------------------------------------------
	// Cache methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Adds or replaces the type informations associated with the type name, when the version is ignored.
	/// </summary>
	/// <param name="typeName">The type name.</param>
	/// <param name="typeInfos">The type name informations.</param>
	private static void SetCacheTypes(String typeName, IEnumerable<TypeInfo> typeInfos) {
		// Split the type name into its parts (the type name and the assembly name).
		// RpcScandinavia.Web.Page.Shared.DataRecordUser, RPC Web Page, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
		// 0                                              1             2                3                4
		String[] typeNameParts = typeName.NotNull().Split(", ");

		// Accept real type names with 5 parts, and short type names with the first 2 parts.
		if ((typeNameParts.Length == 5) || (typeNameParts.Length == 2)) {
			String typeNameCacheId = $"{typeNameParts[1]}:{typeNameParts[0]}";

			// Only one thread may modify the cache at one time.
			RpcActivator.typeCacheAddSemaphore.Wait();

			if (RpcActivator.typeCache.ContainsKey(typeNameCacheId) == false) {
				//Add the type informations to the cache.
				RpcActivator.typeCache.Add(typeNameCacheId, typeInfos.ToList());
			}

			// Release the semaphore.
			RpcActivator.typeCacheAddSemaphore.Release();
		}
	} // SetCacheTypes

	/// <summary>
	/// Gets the list of type informations from the cache, that matches the type name, when the version is ignored.
	/// The returned list IS the cache, for the given type name.
	/// </summary>
	/// <param name="typeName">The System.Type.FullName of the type to locate.</param>
	/// <returns>The list of type informations or null.</returns>
	private static List<TypeInfo> GetCacheTypes(String typeName) {
		// Split the type name into its parts (the type name and the assembly name).
		// RpcScandinavia.Web.Page.Shared.DataRecordUser, RPC Web Page, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
		// 0                                              1             2                3                4
		String[] typeNameParts = typeName.NotNull().Split(", ");

		// Accept real type names with 5 parts, and short type names with the first 2 parts.
		if ((typeNameParts.Length == 5) || (typeNameParts.Length == 2)) {
			String typeNameCacheId = $"{typeNameParts[1]}:{typeNameParts[0]}";

			// Return the type informations from the cache.
			if (RpcActivator.typeCache.ContainsKey(typeNameCacheId) == true) {
				return RpcActivator.typeCache[typeNameCacheId];
			} else {
				return null;
			}
		}

		// Throw exception when the type name is invalid.
		throw new Exception($"The type name '{typeName}' is not a assembly qualified name.");
	} // GetCacheTypes
	#endregion

	#region Create instance methods
	//------------------------------------------------------------------------------------------------------------------
	// Create instance methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Locates the specified type from all available assemblies in the current application domain, and creates an
	/// instance of it using the system activator, using case-sensitive search.
	/// </summary>
	/// <param name="ignoreVersion">Indicate whether or not to ignore the version, culture and token when comparing type names (true).</param>
	/// <typeparam name="T">The type.</typeparam>
	/// <returns>An instance of the specified type created with the parameterless constructor.</returns>
	public static T CreateInstance<T>(Boolean ignoreVersion = true) {
		return RpcActivator.CreateInstance<T>(typeof(T).AssemblyQualifiedName, ignoreVersion);
	} // CreateInstance

	/// <summary>
	/// Locates the specified type from all available assemblies in the current application domain, and creates an
	/// instance of it using the system activator, using case-sensitive search.
	/// </summary>
	/// <param name="typeName">The System.Type.FullName of the type to locate.</param>
	/// <param name="ignoreVersion">Indicate whether or not to ignore the version, culture and token when comparing type names (true).</param>
	/// <typeparam name="T">The expected type.</typeparam>
	/// <returns>An instance of the specified type created with the parameterless constructor, or default(T) if typeName is not found.</returns>
	public static T CreateInstance<T>(String typeName, Boolean ignoreVersion = true) {
		// Get the type.
		Type cacheType = RpcActivator.GetType(typeName, ignoreVersion);
		if ((cacheType != null) && (cacheType.IsAssignableTo(typeof(T)) == true)) {
			// Create a new instance, cast it to the expected type and return it.
			return (T)Activator.CreateInstance(cacheType);
		}

		// Return default.
		return default(T);
	} // CreateInstance

	/// <summary>
	/// Locates the specified type from the cache.
	/// If the cache is empty, it is updated.
	/// </summary>
	/// <param name="ignoreVersion">Indicate whether or not to ignore the version, culture and token when comparing type names (true).</param>
	/// <returns>The type if found, or null.</returns>
	public static Type GetType(String typeName, Boolean ignoreVersion = true) {
		// Try to find the requested type in the cache.
		// The cache should contain the requested type in all versions, and from all loaded assemblies.
		List<TypeInfo> cacheTypes = RpcActivator.GetCacheTypes(typeName);

		if (cacheTypes == null) {
			// Add the type information, from all available assemblies in the current application domain, to the cache.
			RpcActivator.SetCacheTypes(typeName, RpcActivator.GetAllDomainTypes(typeName, true));
			cacheTypes = RpcActivator.GetCacheTypes(typeName);
		}

		if (cacheTypes != null) {
			// First try to find the type information, with the specified version.
			foreach (TypeInfo typeInfo in cacheTypes) {
				if (RpcActivator.IsMatch(typeInfo, typeName, false) == true) {
					return typeInfo.AsType();
				}
			}

			// Then try to find the type information, ignoring the version.
			if (ignoreVersion == true) {
				foreach (TypeInfo typeInfo in cacheTypes) {
					if (RpcActivator.IsMatch(typeInfo, typeName, true) == true) {
						return typeInfo.AsType();
					}
				}
			}
		}

		// Return null.
		return null;
	} // GetType
	#endregion

	#region Matching methods
	//------------------------------------------------------------------------------------------------------------------
	// Matching methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Validates if the type information is matching the type name
	/// </summary>
	/// <param name="typeInfo">The type information.</param>
	/// <param name="typeName">The assembly qualified type name.</param>
	/// <param name="ignoreVersion">Only compares the first two parts of the type name (class name and assembly name).</param>
	/// <returns>True if the type information matches the type name.</returns>
	public static Boolean IsMatch(TypeInfo typeInfo, String typeName, Boolean ignoreVersion) {
		// Split the type name into its parts (the type name and the assembly name).
		// RpcScandinavia.Web.Page.Shared.DataRecordUser, RPC Web Page, Version=6.0.0.0, Culture=neutral, PublicKeyToken=null
		// 0                                              1             2                3                4
		String[] typeNameParts = typeName.NotNull().Split(", ");

		// Accept real type names with 5 parts, and short type names with the first 2 parts.
		if ((typeNameParts.Length == 5) || (typeNameParts.Length == 2)) {
			// Also ignore the version when the type name is short.
			ignoreVersion = ((ignoreVersion == true) || (typeNameParts.Length == 2));

			// Compare.
			return (
					(ignoreVersion == false) &&
					(typeInfo.AssemblyQualifiedName == typeName)
				) || (
					(ignoreVersion == true) &&
					(typeInfo.FullName == typeNameParts[0]) &&
					(typeInfo.Assembly.GetName().Name == typeNameParts[1])
				);
		}

		// Return false.
		return false;
	} // IsMatch

	/// <summary>
	/// Gets an enumerator with all types from the assembly, that matches the type name.
	/// </summary>
	/// <param name="assembly">The assembly.</param>
	/// <param name="typeName">The assembly qualified type name.</param>
	/// <param name="ignoreVersion">Only compares the first two parts of the type name (class name and assembly name).</param>
	/// <param name="throwExceptions">Throw exceptions (true).</param>
	/// <returns>The enumerator.</returns>
	public static IEnumerable<TypeInfo> GetAllAssemblyTypes(Assembly assembly, String typeName, Boolean ignoreVersion, Boolean throwExceptions = true) {
		if (throwExceptions == true) {
			foreach (TypeInfo typeInfo in assembly.DefinedTypes) {
				if (RpcActivator.IsMatch(typeInfo, typeName, ignoreVersion) == true) {
					yield return typeInfo;
				}
			}
		} else {
			IEnumerable<TypeInfo> definedTypes = Enumerable.Empty<TypeInfo>();
			try {
				definedTypes = assembly.DefinedTypes;
			} catch {}

			foreach (TypeInfo typeInfo in definedTypes) {
				if (RpcActivator.IsMatch(typeInfo, typeName, ignoreVersion) == true) {
					yield return typeInfo;
				}
			}
		}
	} // GetAllAssemblyTypes

	/// <summary>
	/// Gets an enumerator with all types from the domain, that matches the type name.
	/// </summary>
	/// <param name="typeName">The assembly qualified type name.</param>
	/// <param name="ignoreVersion">Only compares the first two parts of the type name (class name and assembly name).</param>
	/// <param name="throwExceptions">Throw exceptions (true).</param>
	/// <returns>The enumerator.</returns>
	public static IEnumerable<TypeInfo> GetAllDomainTypes(String typeName, Boolean ignoreVersion, Boolean throwExceptions = true) {
		// Iterate through all available assemblies in the current application domain.
		foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) {
			foreach (TypeInfo typeInfo in RpcActivator.GetAllAssemblyTypes(assembly, typeName, ignoreVersion, throwExceptions)) {
				yield return typeInfo;
			}
		}
	} // GetAllDomainTypes
	#endregion

	#region All methods
	//------------------------------------------------------------------------------------------------------------------
	// All methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets an enumerator with all types from the assembly.
	/// </summary>
	/// <param name="assembly">The assembly.</param>
	/// <param name="throwExceptions">Throw exceptions (true).</param>
	/// <returns>The enumerator.</returns>
	public static IEnumerable<TypeInfo> GetAllAssemblyTypes(Assembly assembly, Boolean throwExceptions = true) {
		if (throwExceptions == true) {
			return assembly.DefinedTypes;
		} else {
			try {
				return assembly.DefinedTypes;
			} catch {
				return Enumerable.Empty<TypeInfo>();
			}
		}
	} // GetAllAssemblyTypes

	/// <summary>
	/// Gets an enumerator with all types from all available assemblies in the current application domain.
	/// </summary>
	/// <param name="throwExceptions">Throw exceptions (true).</param>
	/// <returns>The enumerator.</returns>
	public static IEnumerable<TypeInfo> GetAllDomainTypes(Boolean throwExceptions = true) {
		// Iterate through all available assemblies in the current application domain.
		foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()) {
			foreach (TypeInfo typeInfo in RpcActivator.GetAllAssemblyTypes(assembly, throwExceptions)) {
				yield return typeInfo;
			}
		}
	} // GetAllDomainTypes
	#endregion

	#region Extension methods
	//------------------------------------------------------------------------------------------------------------------
	// Extension methods.
	//------------------------------------------------------------------------------------------------------------------
	public static String GetAssemblyQualifiedName(this Object obj) {
		return obj.GetType().AssemblyQualifiedName ?? null;
	} // GetAssemblyQualifiedName

	public static String GetAssemblyQualifiedName(this Type type) {
		return type.AssemblyQualifiedName ?? null;
	} // GetAssemblyQualifiedName
	#endregion

} // RpcActivator
#endregion