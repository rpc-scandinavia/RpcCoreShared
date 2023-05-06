namespace RpcScandinavia.Core.Shared;
using System;
using System.Linq;

#region RpcKeyValueException
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueException.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// The key/value exception includes information about the exception type.
/// </summary>
public class RpcKeyValueException : Exception {
	private readonly RpcKeyValueExceptionType type;

	/// <summary>
	/// Create new exception with the specified exception type.
	/// </summary>
	/// <param name="message">The message.</param>
	/// <param name="type">The exception type.</param>
	public RpcKeyValueException(String message, RpcKeyValueExceptionType type) : base(message) {
		this.type = type;
	} // RpcKeyValueException

	/// <summary>
	/// Gets the exception type.
	/// </summary>
	public RpcKeyValueExceptionType Type {
		get {
			return this.type;
		}
	} // Type

	/// <summary>
	/// Throws this exception, if specified in the options.
	/// </summary>
	/// <param name="options">The serializer options.</param>
	public void Throw(RpcKeyValueSerializerOptions options) {
		if ((this.type == RpcKeyValueExceptionType.Critical) &&
			(options.SerializeThrowExceptions.HasFlag(RpcKeyValueSerializerExceptionOption.ThrowCriticalExceptions) == true)) {
			throw this;
		}

		if ((this.type == RpcKeyValueExceptionType.Item) &&
			(options.SerializeThrowExceptions.HasFlag(RpcKeyValueSerializerExceptionOption.ThrowItemExceptions) == true)) {
			throw this;
		}
	} // Throw

	/// <summary>
	/// Throws an exception if the maximum depth has been reached.
	/// </summary>
	/// <param name="level">The level.</param>
	/// <param name="options">The serializer options.</param>
	public static void ValidateLevel(Int32 level, RpcKeyValueSerializerOptions options) {
		// Validate level.
		if (level > options.MaxDepth) {
			throw new RpcKeyValueException(
				$"The configured maximum depth of '{options.MaxDepth}' has been reached.",
				RpcKeyValueExceptionType.Item
			);
		}
	} // ValidateLevel

	/// <summary>
	/// Throws an exception if the maximum depth has been reached.
	/// </summary>
	/// <param name="keyPath">The path to the serialized keys.</param>
	/// <param name="options">The serializer options.</param>
	public static void ValidateLevel(ReadOnlySpan<Char> keyPath, RpcKeyValueSerializerOptions options) {
		// Validate level.
		Int32 level = 0;
		foreach (Char ch in keyPath) {
			if (ch == options.HierarchySeparatorChar) {
				// Increase level.
				level++;

				// Validate level.
				if (level > options.MaxDepth) {
					throw new RpcKeyValueException(
						$"The configured maximum depth of '{options.MaxDepth}' has been reached.",
						RpcKeyValueExceptionType.Item
					);
				}
			}
		}
	} // ValidateLevel

	/// <summary>
	/// Throws an exception if the object cannot be assigned to the type.
	/// </summary>
	/// <param name="type">The type.</param>
	/// <param name="obj">The object.</param>
	/// <param name="throwOnNullObject">Specify to throw an exception when the object is null.</param>
	public static void ValidateIsAssignableFrom(Type type, Object obj, Boolean throwOnNullObject = false) {
		if ((throwOnNullObject == false) && (obj == null)) {
			return;
		}

		// Validate that object can be assigned to the member information type.
		if (type.IsAssignableFrom(obj.GetType()) == false) {
			throw new RpcKeyValueException(
				$"The '{type.Name}' type is not assignable from the '{obj.GetType().Name}' type.",
				RpcKeyValueExceptionType.Item
			);
		}
	} // ValidateIsAssignableFrom

} // RpcKeyValueException
#endregion
