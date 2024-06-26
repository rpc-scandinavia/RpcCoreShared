namespace RpcScandinavia.Core.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

#region RpcActivator
//----------------------------------------------------------------------------------------------------------------------
// RpcActivator.
//----------------------------------------------------------------------------------------------------------------------
public static class RpcActivator {
	public const String CacheIsolation = "RpcScandinavia.Core.Shared.RpcActivator";

	#region Create instance methods
	//------------------------------------------------------------------------------------------------------------------
	// Create instance methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Locates the specified type from all available assemblies in the current application domain, and creates an
	/// instance of it using the system activator, using case-sensitive search.
	/// </summary>
	/// <param name="ignoreVersionCultureAndPublicKey">Indicate whether or not to ignore the version, culture and token when comparing type names (true).</param>
	/// <typeparam name="T">The type.</typeparam>
	/// <returns>An instance of the specified type created with the parameterless constructor.</returns>
	public static T CreateInstance<T>(Boolean ignoreVersionCultureAndPublicKey = true) {
		return RpcActivator.CreateInstance<T>(typeof(T).AssemblyQualifiedName, ignoreVersionCultureAndPublicKey);
	} // CreateInstance

	/// <summary>
	/// Locates the specified type from all available assemblies in the current application domain, and creates an
	/// instance of it using the system activator, using case-sensitive search.
	/// </summary>
	/// <param name="typeName">The name of the type to locate.</param>
	/// <param name="ignoreVersionCultureAndPublicKey">Indicate whether or not to ignore the version, culture and token when comparing type names (true).</param>
	/// <typeparam name="T">The expected type.</typeparam>
	/// <returns>An instance of the specified type created with the parameterless constructor, or default(T) if the type is not found.</returns>
	public static T CreateInstance<T>(String typeName, Boolean ignoreVersionCultureAndPublicKey = true) {
		// Get the type.
		Type type = RpcActivator.GetType(typeName, ignoreVersionCultureAndPublicKey);
		if ((type != null) && (type.IsAssignableTo(typeof(T)) == true)) {
			// Create a new instance, cast it to the expected type and return it.
			return (T)Activator.CreateInstance(type);
		}

		// Return default.
		return default(T);
	} // CreateInstance

	/// <summary>
	/// Locates the specified type from all available assemblies in the current application domain, and creates an
	/// instance of it using the system activator, using case-sensitive search.
	/// </summary>
	/// <param name="assemblyQualifiedName">The name of the type to locate.</param>
	/// <param name="ignoreVersionCultureAndPublicKey">Indicate whether or not to ignore the version, culture and token when comparing type names (true).</param>
	/// <typeparam name="T">The expected type.</typeparam>
	/// <returns>An instance of the specified type created with the parameterless constructor, or default(T) if the type is not found.</returns>
	public static T CreateInstance<T>(RpcAssemblyQualifiedName assemblyQualifiedName, Boolean ignoreVersionCultureAndPublicKey = true) {
		// Get the type.
		Type type = RpcActivator.GetType(assemblyQualifiedName, ignoreVersionCultureAndPublicKey);
		if ((type != null) && (type.IsAssignableTo(typeof(T)) == true)) {
			// Create a new instance, cast it to the expected type and return it.
			return (T)Activator.CreateInstance(type);
		}

		// Return default.
		return default(T);
	} // CreateInstance
	#endregion

	#region GetType methods
	//------------------------------------------------------------------------------------------------------------------
	// GetType methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the type from the assembly qualified name or null.
	/// This will search all assemblies in the current application domain, for a type that matches.
	/// </summary>
	/// <param name="typeName">The assembly qualified name.</param>
	/// <param name="ignoreVersionCultureAndPublicKey">Indicate whether or not the assembly version, assembly culture and assembly public key token should be ignored when comparing types.</param>
	/// <returns>The type or null.</returns>
	public static Type GetType(String typeName, Boolean ignoreVersionCultureAndPublicKey = true) {
		// Validate.
		if (typeName == null) {
			throw new NullReferenceException(nameof(typeName));
		}
		if (typeName.IsNullOrWhiteSpace() == true) {
			throw new ArgumentException(nameof(typeName));
		}

		// Get the type.
		return RpcActivator.GetType(new RpcAssemblyQualifiedName(typeName), ignoreVersionCultureAndPublicKey);
	} // GetType

	/// <summary>
	/// Gets the type from the assembly qualified name or null.
	/// This will search all assemblies in the current application domain, for a type that matches.
	/// </summary>
	/// <param name="assemblyQualifiedName">The assembly qualified name.</param>
	/// <param name="ignoreVersionCultureAndPublicKey">Indicate whether or not the assembly version, assembly culture and assembly public key token should be ignored when comparing types.</param>
	/// <returns>The type or null.</returns>
	public static Type GetType(RpcAssemblyQualifiedName assemblyQualifiedName, Boolean ignoreVersionCultureAndPublicKey) {
		// Validate.
		if (assemblyQualifiedName == null) {
			throw new NullReferenceException(nameof(assemblyQualifiedName));
		}

		// Get the type from the Assembly Qualified Name.
		Type type = assemblyQualifiedName.Type;
		if (type != null) {
			return type;
		}

		// Get the type.
		// Use a simple static cache.
		RpcSimpleStaticCache<String, String, Type> typeNameCache = new RpcSimpleStaticCache<String, String, Type>(
			RpcActivator.CacheIsolation,
			RpcSimpleCacheNullValues.DenyNullValuesAndThrow,
			(typeName) => {
				return GetTypeWithoutCache(assemblyQualifiedName, ignoreVersionCultureAndPublicKey);
			}
		);

		return typeNameCache.GetValue(assemblyQualifiedName.ToString());
	} // GetType

	public static Type GetTypeWithoutCache(RpcAssemblyQualifiedName assemblyQualifiedName, Boolean ignoreVersionCultureAndPublicKey) {
		// Validate.
		if (assemblyQualifiedName == null) {
			throw new NullReferenceException(nameof(assemblyQualifiedName));
		}

		// Get the type from the Assembly Qualified Name.
		Type type = assemblyQualifiedName.Type;
		if (type != null) {
			return type;
		}

		// Get the type.
		// Iterate through all available types in all the loaded assemblies in the current application domain.
		foreach (TypeInfo typeInfo in RpcActivator.GetAllDomainTypes(false)) {
			// Full match.
			// Just return when a full match is found.
			if ((ignoreVersionCultureAndPublicKey == false) &&
				(assemblyQualifiedName.EqualsType(typeInfo, false, false, false) == true)) {
				return typeInfo.AsType();
			}

			// Partial match.
			// Save the match with another version number.
			if ((ignoreVersionCultureAndPublicKey == true) &&
				(assemblyQualifiedName.EqualsType(typeInfo, true, true, true) == true)) {
				return typeInfo.AsType();
			}
		}

		// No match found.
		return null;
	} // GetTypeWithoutCache
	#endregion

	#region All methods
	//------------------------------------------------------------------------------------------------------------------
	// All methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets an enumerator with all types from the specified assembly.
	/// </summary>
	/// <param name="assembly">The assembly.</param>
	/// <param name="throwExceptions">Throw exceptions (true).</param>
	/// <returns>The enumerator.</returns>
	public static IEnumerable<TypeInfo> GetAllAssemblyTypes(Assembly assembly, Boolean throwExceptions = true) {
		if (throwExceptions == true) {
			return assembly.DefinedTypes;
		} else {
			// Catch exceptions like this:
			// System.Reflection.ReflectionTypeLoadException: Unable to load one or more of the requested types.
			// Could not load type 'SqlGuidCaster' from assembly 'Microsoft.Data.SqlClient, Version=5.0.0.0, Culture=neutral, PublicKeyToken=23ec7fc2d6eaa4a5' because it contains an object field at offset 0 that is incorrectly aligned or overlapped by a non-object field.
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
				if ((typeInfo.IsPublic == true) && ((typeInfo.IsClass == true) || (typeInfo.IsValueType == true))) {
					yield return typeInfo;
				}
			}
		}
	} // GetAllDomainTypes
	#endregion

} // RpcActivator
#endregion
