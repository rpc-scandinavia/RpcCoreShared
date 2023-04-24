namespace RpcScandinavia.Core.Shared;
using System;
using System.Collections;
using System.Reflection;

#region RpcMemberInfo
//----------------------------------------------------------------------------------------------------------------------
// RpcMemberInfo.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// RPC Key/Value serializator uses reflection to gather information about all members in the types.
/// Each member information has information about a single member type.
/// </summary>
public abstract class RpcMemberInfo : IRpcTypeInfo {

	#region Constructors
	//------------------------------------------------------------------------------------------------------------------
	// Constructors.
	//------------------------------------------------------------------------------------------------------------------
	public RpcMemberInfo() {
	} // RpcMemberInfo
	#endregion

	#region Properties
	//------------------------------------------------------------------------------------------------------------------
	// Properties.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets true if the member type is a enumerator.
	/// </summary>
	public Boolean IsEnum {
		get {
			return (this.Type.IsEnum == true);
		}
	} // IsEnum

	/// <summary>
	/// Gets true if the member type is a enumerator with the flags attribute.
	/// </summary>
	public Boolean IsEnumWithFlags {
		get {
			if (this.Type.IsEnum == true) {
				FlagsAttribute typeEnumFlagsAttribute = this.Type.GetCustomAttribute<FlagsAttribute>(true);
				return (typeEnumFlagsAttribute != null);
			} else {
				return false;
			}
		}
	} // IsEnumWithFlags

	/// <summary>
	/// Gets true if the member type is a generic dictionary.
	/// </summary>
	public Boolean IsGenericDictionary {
		get {
			return ((typeof(IDictionary).IsAssignableFrom(this.Type) == true) && (this.Type.IsGenericType == true));
		}
	} // IsGenericDictionary

	/// <summary>
	/// Gets true if the member type is a generic list.
	/// </summary>
	public Boolean IsGenericList {
		get {
			return ((typeof(IList).IsAssignableFrom(this.Type) == true) && (this.Type.IsGenericType == true));
		}
	} // IsGenericList

	/// <summary>
	/// Gets true if the member type is an array.
	/// </summary>
	public Boolean IsArray {
		get {
			return ((typeof(IList).IsAssignableFrom(this.Type) == true) && (this.Type.IsGenericType == false));
		}
	} // IsGenericList

	/// <summary>
	/// Gets true if the member is a field.
	/// </summary>
	public Boolean IsField {
		get {
			return (this is RpcMemberInfoField);
		}
	} // IsField

	/// <summary>
	/// Gets true if the member is a property.
	/// </summary>
	public Boolean IsProperty {
		get {
			return (this is RpcMemberInfoProperty);
		}
	} // IsProperty
	#endregion

	#region Abstract properties
	//------------------------------------------------------------------------------------------------------------------
	// Abstract properties.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the member type.
	/// </summary>
	public abstract Type Type { get; }

	/// <summary>
	/// Gets the declaring type.
	/// </summary>
	public abstract Type DeclaringType { get; }

	/// <summary>
	/// Gets the member name, or the attributed name if the member is decorated with
	/// a <see cref="System.Text.Json.Serialization.JsonPropertyNameAttribute" />.
	/// </summary>
	public abstract String Name { get; }

	/// <summary>
	/// Gets the member sorting order, if the member is decorated with
	/// a <see cref="System.Text.Json.Serialization.JsonPropertyOrderAttribute" />.
	/// </summary>
	public abstract Int32 Order { get; }

	/// <summary>
	/// Gets the member group name, if the member is decorated with either
	/// a <see cref="RpcScandinavia.Core.Shared.KeyValueSerializer.RpcKeyValueSerializerGroupAttribute" /> or
	/// a <see cref="RpcScandinavia.Core.Shared.KeyValueSerializer.JsonGroupAttribute" />.
	/// </summary>
	public abstract String Group { get; }

	/// <summary>
	/// Gets a value indicating whether the member is included, if the member is decorated with
	/// a <see cref="System.Text.Json.Serialization.JsonIncludeAttribute" />.
	/// </summary>
	public abstract Boolean IsIncluded { get; }

	/// <summary>
	/// Gets a value indicating whether the member is ignored, if the member is decorated with
	/// a <see cref="System.Text.Json.Serialization.JsonIgnoreAttribute" />.
	/// </summary>
	public abstract Boolean IsIgnored { get; }

	/// <summary>
	/// Gets a value indicating whether the member is abstract.
	/// </summary>
	public abstract Boolean IsAbstract { get; }

	/// <summary>
	/// Gets a value indicating whether the member is public for getting the value.
	/// </summary>
	public abstract Boolean IsPublicGet { get; }

	/// <summary>
	/// Gets a value indicating whether the member is public for setting the value.
	/// </summary>
	public abstract Boolean IsPublicSet { get; }

	/// <summary>
	/// Gets a value indicating whether the member is private for getting the value.
	/// </summary>
	public abstract Boolean IsPrivateGet { get; }

	/// <summary>
	/// Gets a value indicating whether the member is private for setting the value.
	/// </summary>
	public abstract Boolean IsPrivateSet { get; }

	/// <summary>
	/// Gets a value indicating whether the member is readonly, and it is not possible to set the value.
	/// </summary>
	public abstract Boolean IsReadOnly { get; }
	#endregion

	#region Abstract methods
	//------------------------------------------------------------------------------------------------------------------
	// Abstract methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the attribute decorating the member.
	/// </summary>
	/// <typeparam name="AttributeType">The attribute type.</typeparam>
	/// <returns>The attribute or null.</returns>
	public abstract AttributeType GetCustomAttribute<AttributeType>() where AttributeType: Attribute;

	/// <summary>
	/// Gets the member value of a specified object.
	/// </summary>
	/// <param name="obj">The object whose member value will be returned.</param>
	/// <returns>The member value of the specified object.</returns>
	public abstract Object GetValue(Object obj);

	/// <summary>
	/// Sets the member value, for a specified object.
	/// </summary>
	/// <param name="obj">The object whose member value will be set.</param>
	/// <param name="value">The new member value.</param>
	public abstract void SetValue(Object obj, Object value);
	#endregion

	#region Helper methods
	//------------------------------------------------------------------------------------------------------------------
	// Helper methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets true if the member information should be serialized.
	/// </summary>
	/// <param name="options">The options.</param>
	public Boolean ShouldSerialize(RpcKeyValueSerializerOptions options) {
		return (
			(
				(this.IsIgnored == false)
			) && (
				(
					(this.IsIncluded == true)
				) || (
					(
						(
							(this.IsEnum == false)
						) || (
							(
								(this.IsEnum == true)
							) && (
								(options.SerializeEnums != RpcKeyValueSerializerEnumOption.NotSerialized)
							)
						)
					) && (
						(
							(this.IsField == false)
						) || (
							(
								(this.IsField == true)
							) && (
								(this.Name.Contains("k__BackingField") == false)
							) && (
								(
									((this.IsPublicGet == true) && (options.IncludePublicFields == true))
								) || (
									((this.IsPrivateGet == true) && (options.IncludePrivateFields == true))
								) || (
									((this.IsReadOnly == false) && (options.SerializeReadonlyFields == true))
								)
							)
						)
					) && (
						(
							(this.IsProperty == false)
						) || (
							(
								(this.IsProperty == true)
							) && (
								(
									((this.IsPublicGet == true) && (options.IncludePublicProperties == true))
								) || (
									((this.IsPrivateGet == true) && (options.IncludePrivateProperties == true))
								) || (
									((this.IsReadOnly == false) && (options.SerializeReadonlyProperties == true))
								)
							)
						)
					)
				)
			)
		);
	} // ShouldSerialize

	/// <summary>
	/// Gets true if the member information should be deserialized.
	/// </summary>
	/// <param name="options">The options.</param>
	public Boolean ShouldDeserialize(RpcKeyValueSerializerOptions options) {
		return (
			(
				(this.IsIgnored == false)
			) && (
				(
					(this.IsIncluded == true)
				) || (
					(
						(this.IsReadOnly == false)
					) && (
						(
							(this.IsEnum == false)
						) || (
							(
								(this.IsEnum == true)
							) && (
								(options.DeserializeEnums == true)
							)
						)
					) && (
						(
							(this.IsField == false)
						) || (
							(
								(this.IsField == true)
							) && (
								(this.Name.Contains("k__BackingField") == false)
							) && (
								(
									((this.IsPublicGet == true) && (options.IncludePublicFields == true))
								) || (
									((this.IsPrivateGet == true) && (options.IncludePrivateFields == true))
								)
							)
						)
					) && (
						(
							(this.IsProperty == false)
						) || (
							(
								(this.IsProperty == true)
							) && (
								(
									((this.IsPublicGet == true) && (options.IncludePublicProperties == true))
								) || (
									((this.IsPrivateGet == true) && (options.IncludePrivateProperties == true))
								)
							)
						)
					)
				)
			)
		);
	} // ShouldDeserialize
	#endregion



	public String Key {
		get {
			// TODO: Add path information with the constructor, and prepend here.
			return this.Name;
		}
	} // Key

	public ValueType GetValue<ValueType>(Object obj, ValueType defaultValue = default(ValueType)) {
		try {
			return (ValueType)this.GetValue(obj);
		} catch {
			return defaultValue;
		}
	} // GetValue



} // RpcMemberInfo
#endregion
