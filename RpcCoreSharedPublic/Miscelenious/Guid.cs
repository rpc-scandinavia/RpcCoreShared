using System;
namespace RpcScandinavia.Core.Shared;

#region RpcGuid
//----------------------------------------------------------------------------------------------------------------------
// RpcGuid.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// Generates custom GUIDs, where the different parts are used to store integer values in human-readable form.
/// There are 5 different parts "AAAAAAAA-BBBB-CCCC-DDDD-EEEEEEEEEEEE".
///
/// The numbers are not converted into hex, but intentionally kept in human-readable form.
/// The number "3784" is thus not converted into "0xEC8".
///
/// Example with 20 as group identifier and number identifier: "00000020-0000-abba-0000-000000000020".
/// </summary>
public static class RpcGuid {

	#region Fields
	//------------------------------------------------------------------------------------------------------------------
	// Fields.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// A read-only instance of the Guid structure whose value is all zeros.
	/// </summary>
	public static readonly Guid Empty = Guid.Empty;

	/// <summary>
	/// A read-only instance of the Guid structure whose value is all F's.
	/// </summary>
	public static readonly Guid Full = new Guid("ffffffff-ffff-ffff-ffff-ffffffffffff");

	/// <summary>
	/// A read-only instance of the Guid structure whose value is all E's.
	/// </summary>
	public static readonly Guid Echo = new Guid("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee");

	/// <summary>
	/// A read-only instance of the Guid structure whose value is all D's.
	/// </summary>
	public static readonly Guid Delta = new Guid("dddddddd-dddd-dddd-dddd-dddddddddddd");

	/// <summary>
	/// A read-only instance of the Guid structure whose value is all C's.
	/// </summary>
	public static readonly Guid Charlie = new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc");

	/// <summary>
	/// A read-only instance of the Guid structure whose value is all B's.
	/// </summary>
	public static readonly Guid Bravo = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");

	/// <summary>
	/// A read-only instance of the Guid structure whose value is all A's.
	/// </summary>
	public static readonly Guid Alpha = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

	/// <summary>
	/// A read-only instance of the Guid structure whose value is all 9's.
	/// </summary>
	public static readonly Guid Nine = new Guid("99999999-9999-9999-9999-999999999999");

	/// <summary>
	/// A read-only instance of the Guid structure whose value is all 8's.
	/// </summary>
	public static readonly Guid Eight = new Guid("88888888-8888-8888-8888-888888888888");

	/// <summary>
	/// A read-only instance of the Guid structure whose value is all 7's.
	/// </summary>
	public static readonly Guid Seven = new Guid("77777777-7777-7777-7777-777777777777");

	/// <summary>
	/// A read-only instance of the Guid structure whose value is all 6's.
	/// </summary>
	public static readonly Guid Six = new Guid("66666666-6666-6666-6666-666666666666");

	/// <summary>
	/// A read-only instance of the Guid structure whose value is all 5's.
	/// </summary>
	public static readonly Guid Five = new Guid("55555555-5555-5555-5555-555555555555");

	/// <summary>
	/// A read-only instance of the Guid structure whose value is all 4's.
	/// </summary>
	public static readonly Guid Four = new Guid("44444444-4444-4444-4444-444444444444");

	/// <summary>
	/// A read-only instance of the Guid structure whose value is all 3's.
	/// </summary>
	public static readonly Guid Three = new Guid("33333333-3333-3333-3333-333333333333");

	/// <summary>
	/// A read-only instance of the Guid structure whose value is all 2's.
	/// </summary>
	public static readonly Guid Two = new Guid("22222222-2222-2222-2222-222222222222");

	/// <summary>
	/// A read-only instance of the Guid structure whose value is all 1's.
	/// </summary>
	public static readonly Guid One = new Guid("11111111-1111-1111-1111-111111111111");
	#endregion

	#region NewGuid methods
	//------------------------------------------------------------------------------------------------------------------
	// NewGuid methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Generates a custom GUID, with the group identifier in part A and the number identifier in part E.
	/// The minimum group identifier is 0 and the maximum group identifier is 99.999.999.
	/// The minimum number identifier is 0 and the maximum number identifier is 999.999.999.999.
	/// </summary>
	/// <param name="groupId">The group identifier, part A.</param>
	/// <param name="numberId">The number identifier, part E.</param>
	/// <returns>The new GUID.</returns>
	public static Guid NewGuid(Int32 groupId, Int32 numberId) {
		// Validate.
		if ((groupId < 0) || (groupId > 99999999)) {
			throw new ArgumentOutOfRangeException($"The group identifier '{groupId}' must be from 0 to 99.999.999.");
		}
		if ((numberId < 0) || (numberId > 999999999)) {
			throw new ArgumentOutOfRangeException($"The number identifier '{numberId}' must be from 0 to 999.999.999.");
		}

		// Return the constructed GUID.
		return new Guid($"{groupId:00000000}-0000-abba-0000-000{numberId:000000000}");
	} // NewGuid

	/// <summary>
	/// Generates a custom GUID, with the group identifier in part A and the number identifier in part E.
	/// The minimum group identifier is 0 and the maximum group identifier is 99.999.999.
	/// The minimum number identifier is 0 and the maximum number identifier is 999.999.999.999.
	/// </summary>
	/// <param name="groupId">The group identifier, part A.</param>
	/// <param name="numberId">The number identifier, part E.</param>
	/// <returns>The new GUID.</returns>
	public static Guid NewGuid(Int32 groupId, Int64 numberId) {
		// Validate.
		if ((groupId < 0) || (groupId > 99999999)) {
			throw new ArgumentOutOfRangeException($"The group identifier '{groupId}' must be from 0 to 99.999.999.");
		}
		if ((numberId < 0) || (numberId > 999999999999)) {
			throw new ArgumentOutOfRangeException($"The number identifier '{numberId}' must be from 0 to 999.999.999.999.");
		}

		// Return the constructed GUID.
		return new Guid($"{groupId:00000000}-0000-abba-0000-{numberId:000000000000}");
	} // NewGuid
	#endregion

	#region Extension methods
	//------------------------------------------------------------------------------------------------------------------
	// Extension methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Returns true if the GUID parts B, C and D matches the custom GUID.
	/// </summary>
	/// <param name="guid">The guid.</param>
	/// <returns>True if the guid is a custom guid, or you are extreamly (un)lucky.</returns>
	public static Boolean IsCustomGuid(this Guid guid) {
		return guid.ToString().Substring(8, 16).Equals("-0000-abba-0000-");
	} // IsCustomGuid

	/// <summary>
	/// Returns the group identifier from part A.
	/// </summary>
	/// <param name="guid">The guid.</param>
	/// <param name="defaultValue">Default value if it is not a custom guid.</param>
	/// <returns>Group identifier or default value.</returns>
	public static Int32 GetGroupId(this Guid guid, Int32 defaultValue = 0) {
		if (guid.ToString().Substring(8, 16).Equals("-0000-abba-0000-") == true) {
			return guid.ToString().Substring(0, 8).ToInt32(defaultValue);
		} else {
			return defaultValue;
		}
	} // GetGroupId

	/// <summary>
	/// Returns the number identifier from part E.
	/// </summary>
	/// <param name="guid">The guid.</param>
	/// <param name="defaultValue">Default value if it is not a custom guid.</param>
	/// <returns>Number identifier or default value.</returns>
	public static Int32 GetNumberId(this Guid guid, Int32 defaultValue = 0) {
		if (guid.ToString().Substring(8, 16).Equals("-0000-abba-0000-") == true) {
			return guid.ToString().Substring(24).ToInt32(defaultValue);
		} else {
			return defaultValue;
		}
	} // GetNumberId

	/// <summary>
	/// Returns the number identifier from part E.
	/// </summary>
	/// <param name="guid">The guid.</param>
	/// <param name="defaultValue">Default value if it is not a custom guid.</param>
	/// <returns>Number identifier or default value.</returns>
	public static Int64 GetNumberId64(this Guid guid, Int64 defaultValue = 0) {
		if (guid.ToString().Substring(8, 16).Equals("-0000-abba-0000-") == true) {
			return guid.ToString().Substring(24).ToInt64(defaultValue);
		} else {
			return defaultValue;
		}
	} // GetNumberId
	#endregion

} // RpcGuid
#endregion
