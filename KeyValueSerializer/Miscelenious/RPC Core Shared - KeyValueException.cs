namespace RpcScandinavia.Core.Shared.KeyValueSerializer;
using System;
using System.Linq;

#region RpcKeyValueException
//----------------------------------------------------------------------------------------------------------------------
// RpcKeyValueException.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// The key/value serializer exception includes information about the exception type.
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
	/// <param name="keyPrefix">The prefix to the serialized keys.</param>
	/// <param name="options">The serializer options.</param>
	public static void ValidateLevel(String keyPrefix, RpcKeyValueSerializerOptions options) {
		// Validate level.
		if (keyPrefix.Count((ch) => (ch == options.HierarchySeparatorChar)) > options.MaxDepth) {
			throw new RpcKeyValueException(
				$"The configured maximum depth of '{options.MaxDepth}' has been reached.",
				RpcKeyValueExceptionType.Item
			);
		}
	} // ValidateLevel

	public static void ValidateIsAssignableFrom(RpcMemberInfo memberInfo, Object obj) {
		if (memberInfo.Type.IsAssignableFrom(obj.GetType()) == false) {
			throw new RpcKeyValueException(
				$"The '{memberInfo.Type.Name}' type is not assignable from the '{obj.GetType().Name}' type.",
				RpcKeyValueExceptionType.Item
			);
		}
	} // ValidateIsAssignableFrom

} // RpcKeyValueException
#endregion
