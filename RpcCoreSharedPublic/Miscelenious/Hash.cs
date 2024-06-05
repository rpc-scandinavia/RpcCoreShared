using System;
using System.Globalization;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using RpcScandinavia.Core.Shared;
namespace RpcScandinavia.Core.Shared;

#region RpcStringHashExtensions
//----------------------------------------------------------------------------------------------------------------------
// RpcStringHashExtensions.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// This class contains useful hashing extension methods for the <see cref="System.String" />.
/// </summary>
public static class RpcStringHashExtensions {

	#region String hash methods
	//------------------------------------------------------------------------------------------------------------------
	// String hash methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Return the hashed value.
	/// The result is prefixed with the hash algorithm used, like this "{md5}+x1Y3pm+CrYzYm9OaPcSnQ==".
	/// The default salt size is 8 bytes, for most of the salted algorithms.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <param name="algorithm">The hash algorithm.</param>
	/// <returns>The hashed string.</returns>
	public static String ToStringHash(this String value, RpcHashAlgorithm algorithm) {
		// Validate.
		RpcStringHashExtensions.ValidateNotNull(value, nameof(value));

		return RpcStringHashExtensions.ToStringHash(value, algorithm, true, null);
	} // ToStringHash

	/// <summary>
	/// Return the hashed value.
	/// The result can be prefixed with the hash algorithm used, like this "{ssha}l4zjDpgQ6c1AKGiEEa+vknpm/BqGI08TPu5BZg==".
	/// The default salt size is 8 bytes, for most of the salted algorithms.
	/// </summary>
	/// <param name="value">The value.</param>
	/// <param name="algorithm">The hash algorithm.</param>
	/// <param name="prefix">True if the hash algorithm prefix should be included in the result.</param>
	/// <returns>The hashed string.</returns>
	public static String ToStringHash(this String value, RpcHashAlgorithm algorithm, Boolean prefix) {
		// Validate.
		RpcStringHashExtensions.ValidateNotNull(value, nameof(value));

		return RpcStringHashExtensions.ToStringHash(value, algorithm, prefix, null);
	} // ToStringHash

	/// <summary>
	/// Return the hashed value.
	/// The result can be prefixed with the hash algorithm used, like this "{ssha}l4zjDpgQ6c1AKGiEEa+vknpm/BqGI08TPu5BZg==".
	/// </summary>
	/// <param name="value">The value.</param>
	/// <param name="algorithm">The hash algorithm.</param>
	/// <param name="prefix">True if the hash algorithm prefix should be included in the result.</param>
	/// <param name="salt">The salt.</param>
	/// <returns>The hashed string.</returns>
	public static String ToStringHash(this String value, RpcHashAlgorithm algorithm, Boolean prefix, Byte[] salt) {
		switch (algorithm) {

			// Normal hash.
			// Example: "{algorithm}BASE64(HASH(clear))".
			case RpcHashAlgorithm.MD5:
			case RpcHashAlgorithm.SHA1:
			case RpcHashAlgorithm.SHA256:
			case RpcHashAlgorithm.SHA384:
			case RpcHashAlgorithm.SHA512:
				using (HashAlgorithm hashAlgorithm = RpcStringHashExtensions.GetHashAlgorithm(algorithm)) {
					value = System.Convert.ToBase64String(hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(value)));
					if (prefix == true) {
						value = $"{{{algorithm}}}{value}";
					}
					return value;
				}

			// Salted hash.
			// Example: "{algorithm}BASE64(HASH(clear + salt) + salt)".
			case RpcHashAlgorithm.SMD5:
			case RpcHashAlgorithm.SSHA1:
			case RpcHashAlgorithm.SSHA256:
			case RpcHashAlgorithm.SSHA384:
			case RpcHashAlgorithm.SSHA512:
				using (HashAlgorithm hashAlgorithm = RpcStringHashExtensions.GetHashAlgorithm(algorithm)) {
					Byte[] saltBytes = ((salt != null) && (salt.Length > 0)) ? salt : RpcStringHashExtensions.GetSalt(8);
					Byte[] clearBytes = Encoding.UTF8.GetBytes(value);
					value = System.Convert.ToBase64String(RpcStringHashExtensions.AppendByteArray(hashAlgorithm.ComputeHash(RpcStringHashExtensions.AppendByteArray(clearBytes, saltBytes)), saltBytes));
/*
					Byte[] saltBytes = ((salt != null) && (salt.Length > 0)) ? salt : RpcStringHashExtensions.GetSalt(8);
					Byte[] clearBytes = Encoding.UTF8.GetBytes(value);
					Byte[] clearWithSaltBytes = RpcStringHashExtensions.AppendByteArray(clearBytes, saltBytes);
					Byte[] saltedHashBytes = hashAlgorithm.ComputeHash(clearWithSaltBytes);
					Byte[] saltedHashWithSaltBytes = RpcStringHashExtensions.AppendByteArray(saltedHashBytes, saltBytes);
					value = System.Convert.ToBase64String(saltedHashWithSaltBytes);
*/
					if (prefix == true) {
						value = $"{{{algorithm}}}{value}";
					}
					return value;
				}

			// Special hash.
			// Example: "HEX(salt)HEX(MD5(salt + clear))".
			case RpcHashAlgorithm.KopanoMD5:
				using (HashAlgorithm hashAlgorithm = RpcStringHashExtensions.GetHashAlgorithm(algorithm)) {
					// Get the hashed password.
					// Kopano uses the MD5 algorithm to hash the password.
					// Kopano uses 32 bits, 4 bytes long salt, but the salt is used as a HEX string so it doubles in length.
					Byte[] saltBytes = Encoding.UTF8.GetBytes(RpcStringHashExtensions.GetHexString(RpcStringHashExtensions.GetSalt(4)));
					Byte[] clearBytes = Encoding.UTF8.GetBytes(value);
					Byte[] clearWithSaltBytes = RpcStringHashExtensions.AppendByteArray(saltBytes, clearBytes);
					Byte[] saltedHashBytes = hashAlgorithm.ComputeHash(clearWithSaltBytes);
					return
						Encoding.UTF8.GetString(saltBytes) +							// First the salt, which is already a HEX string.
						RpcStringHashExtensions.GetHexString(saltedHashBytes);			// Second the hashed password as a HEX string.
				}

			default:
				// This is never reached!
				return null;
		}
	} // ToStringHash
	#endregion

	#region Validate string hash methods
	//------------------------------------------------------------------------------------------------------------------
	// Validate string hash methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Validates the hashed value by hashing the clear value and comparing the two hashed values.
	/// </summary>
	/// <param name="hashedValue">The hashed value.</param>
	/// <param name="clearValue">The clear value.</param>
	/// <returns>Returns true if the hashed value equals the clear value after it has been  hashed; otherwise false.</returns>
	public static Boolean ValidateStringHash(this String hashedValue, String clearValue) {
		// Validate.
		RpcStringHashExtensions.ValidateNotNull(hashedValue, nameof(hashedValue));
		RpcStringHashExtensions.ValidateNotNull(clearValue, nameof(clearValue));

		if ((hashedValue.StartsWith("{") == true) && (hashedValue.Contains("}") == true)) {
			// Process the hash value.
			//	1) Split the algorithm prefix and the hash value. Example: "{md5}+x1Y3pm+CrYzYm9OaPcSnQ==".
			//	2) Get the algorithm objects, now we have the algorithm prefix (name).
			//	3) Get the salt if the length of the hash value is greator then the algorithm hash size.
			//	   The extra bytes are the salt. Example: "HASH(clear + salt) + salt".
			//	   Get the hash value, the "HASH(clear + salt)" from the example.
			//	4) Get the hash value from the clear value, using the same salt.
			//	   Remember to remove the extra salt bytes from the result.
			//	5) Convert to strings and return the equality.

			// 1)	Split the algorithm prefix and the hash value. Example: "{md5}+x1Y3pm+CrYzYm9OaPcSnQ==".
			String hashAlgorithmStr = String.Empty;
			hashAlgorithmStr = hashedValue.Substring(0, hashedValue.IndexOf("}") + 1).Trim().ToLower();
			String hashValueA1 = hashedValue.Substring(hashedValue.IndexOf("}") + 1).Trim();

			// 2) Get the algorithm objects, now we have the algorithm prefix (name).
			RpcHashAlgorithm hashAlgorithmRpc = RpcStringHashExtensions.GetRpcHashAlgorithm(hashAlgorithmStr);
			HashAlgorithm hashAlgorithm = RpcStringHashExtensions.GetHashAlgorithm(hashAlgorithmStr);

			// 3) Get the salt if the length of the hash value is greator then the algorithm hash size.
			Byte[] hashedValueBytesA1 = Convert.FromBase64String(hashValueA1);							// HASH(clear + salt) + salt
			Byte[] hashedValueBytesA2 = new Byte[hashAlgorithm.HashSize / 8];							// HASH(clear + salt)
			Byte[] saltBytesA2 = new Byte[hashedValueBytesA1.Length - hashedValueBytesA2.Length];		// salt
			for (Int32 index = 0; index < hashedValueBytesA2.Length; index++) {
				hashedValueBytesA2[index] = hashedValueBytesA1[index];
			}
			for (Int32 index = 0; index < saltBytesA2.Length; index++) {
				saltBytesA2[index] = hashedValueBytesA1[hashedValueBytesA2.Length + index];
			}

			// 4) Get the hash value from the clear value, using the same salt.
			String hashedValueB1 = RpcStringHashExtensions.ToStringHash(clearValue, hashAlgorithmRpc, false, saltBytesA2);
			Byte[] hashedValueBytesB1 = Convert.FromBase64String(hashedValueB1);						// HASH(clear + salt) + salt
			Byte[] hashedValueBytesB2 = new Byte[hashAlgorithm.HashSize / 8];							// HASH(clear + salt)
			for (Int32 index = 0; index < hashedValueBytesB2.Length; index++) {
				hashedValueBytesB2[index] = hashedValueBytesB1[index];
			}

			// 5) Convert to strings and return the equality.
			String hashedValueA2 = Convert.ToBase64String(hashedValueBytesA2);
			String hashedValueB2 = Convert.ToBase64String(hashedValueBytesB2);
			return hashedValueA2.Equals(hashedValueB2);
		} else {
			// Unable to determane the hash algorithm.
			// Return the normal string equality.
			return clearValue.Equals(hashedValue);
		}
	} // ValidateStringHash

	public static Boolean ValidateKopanoStringHash(this String hashedValue, String clearValue) {
		return false;
	} // ValidateKopanoStringHash
	#endregion

	#region Helper methods
	//------------------------------------------------------------------------------------------------------------------
	// Helper methods.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Gets the <see cref="System.Security.Cryptography.HashAlgorithm" /> from a <see cref="RpcScandinavia.Core.Shared.RpcHashAlgorithm" />.
	/// </summary>
	/// <param name="algorithm">The hash algorithm.</param>
	/// <returns>The hash algorithm.</returns>
	private static HashAlgorithm GetHashAlgorithm(RpcHashAlgorithm algorithm) {
		switch (algorithm) {
			case RpcHashAlgorithm.MD5:
			case RpcHashAlgorithm.SMD5:
			case RpcHashAlgorithm.KopanoMD5:
				return MD5.Create();
			case RpcHashAlgorithm.SHA1:
			case RpcHashAlgorithm.SSHA1:
				return SHA1.Create();
			case RpcHashAlgorithm.SHA256:
			case RpcHashAlgorithm.SSHA256:
				return SHA256.Create();
			case RpcHashAlgorithm.SHA384:
			case RpcHashAlgorithm.SSHA384:
				return SHA384.Create();
			case RpcHashAlgorithm.SHA512:
			case RpcHashAlgorithm.SSHA512:
				return SHA512.Create();
			default:
				// This is never reached!
				throw new ArgumentException($"The used hash algorithm '{algorithm}' is not supported.");
		}
	} // GetHashAlgorithm

	/// <summary>
	/// Gets the <see cref="System.Security.Cryptography.HashAlgorithm" /> from a <see cref="System.String" />.
	/// Note this throws an exception, if the string can not be parsed into a hash algorithm.
	/// </summary>
	/// <param name="algorithm">The hash algorithm.</param>
	/// <returns>The hash algorithm.</returns>
	private static HashAlgorithm GetHashAlgorithm(String algorithm) {
		algorithm = algorithm
			.Trim('{', '}', ' ')
			.Replace("-", "")
			.Replace("_", "")
			.ToLower();

		switch (algorithm) {
			case "md5":
			case "smd5":
			case "kopanomd5":
				return MD5.Create();
			case "sha":
			case "sha1":
			case "ssha":
			case "ssha1":
				return SHA1.Create();
			case "sha256":
			case "ssha256":
				return SHA256.Create();
			case "sha384":
			case "ssha384":
				return SHA384.Create();
			case "sha512":
			case "ssha512":
				return SHA512.Create();
			default:
				throw new ArgumentException($"The used hash algorithm '{algorithm}' is not supported.");
		}
	} // GetHashAlgorithm

	/// <summary>
	/// Gets the <see cref="RpcScandinavia.Core.Shared.RpcHashAlgorithm" /> from a <see cref="System.String" />.
	/// Note this throws an exception, if the string can not be parsed into a hash algorithm.
	/// </summary>
	/// <param name="algorithm">The hash algorithm.</param>
	/// <returns>The hash algorithm.</returns>
	private static RpcHashAlgorithm GetRpcHashAlgorithm(String algorithm) {
		algorithm = algorithm
			.Trim('{', '}', ' ')
			.Replace("-", "")
			.Replace("_", "")
			.ToLower();

		switch (algorithm) {
			case "md5":
				return RpcHashAlgorithm.MD5;
			case "sha":
			case "sha1":
				return RpcHashAlgorithm.SHA1;
			case "sha256":
				return RpcHashAlgorithm.SHA256;
			case "sha384":
				return RpcHashAlgorithm.SHA384;
			case "sha512":
				return RpcHashAlgorithm.SHA512;

			case "smd5":
				return RpcHashAlgorithm.SMD5;
			case "ssha":
			case "ssha1":
				return RpcHashAlgorithm.SSHA1;
			case "ssha256":
				return RpcHashAlgorithm.SSHA256;
			case "ssha384":
				return RpcHashAlgorithm.SSHA384;
			case "ssha512":
				return RpcHashAlgorithm.SSHA512;

			case "kopanomd5":
				return RpcHashAlgorithm.KopanoMD5;

			default:
				throw new ArgumentException($"The used hash algorithm '{algorithm}' is not supported.");
		}
	} // GetRpcHashAlgorithm

	/// <summary>
	/// Gets a new random salt.
	/// </summary>
	/// <param name="saltLength">The length of the salt (4 bytes).</param>
	/// <returns>The salt.</returns>
	private static Byte[] GetSalt(Int32 saltLength = 4) {
		// Validate.
		RpcStringHashExtensions.ValidateSaltLength(saltLength);

		// Generate random bytes.
		Byte[] salt = new Byte[saltLength];
		Random random = new Random();
		random.NextBytes(salt);

		// Return the salt as a byte array.
		return salt;
	} // GetSalt

	private static String GetHexString(Byte[] bytes) {
		// Return the bytes as a HEX string.
		return BitConverter.ToString(bytes)
			.Replace("-", "")
			.ToLower();
	} // GetHexString

	private static Byte[] GetBytes(String hexString) {
		// Return the HEX string as a byte array.
		Byte[] bytes = new Byte[hexString.Length / 2];
		for (Int32 index = 0; index < hexString.Length / 2; index++) {
			bytes[index] = Byte.Parse(hexString.Substring(index * 2, 2), NumberStyles.HexNumber);
		}
		return bytes;
	} // GetBytes

	/// <summary>
	/// Gets two concatenated byte arrays.
	/// </summary>
	/// <param name="byteArray1">The first byte array.</param>
	/// <param name="byteArray2">The second byte array.</param>
	/// <returns>The concatenated byte arrays.</returns>
	private static Byte[] AppendByteArray(Byte[] byteArray1, Byte[] byteArray2) {
		// Validate.
		//RpcStringHashExtensions.ValidateNotNull(byteArray1, nameof(byteArray1));
		//RpcStringHashExtensions.ValidateNotNull(byteArray2, nameof(byteArray2));

		// Concatenate the byte arrays.
		Byte[] byteArrayResult = new Byte[byteArray1.Length + byteArray2.Length];
		for (Int32 index = 0; index < byteArray1.Length; index++) {
			byteArrayResult[index] = byteArray1[index];
		}
		for (Int32 index = 0; index < byteArray2.Length; index++) {
			byteArrayResult[byteArray1.Length + index] = byteArray2[index];
		}

		// Return the result.
		return byteArrayResult;
	} // AppendByteArray



	private static void ValidateSaltLength(Int32 saltLength) {
		if ((saltLength <= 0) || (saltLength > 1024)) {
			throw new ArgumentException($"The salt length '{saltLength}' must be a number from 1 to 1024.");
		}
	} // ValidateSaltLength

	private static void ValidateNotNull(Object obj, String name) {
		if (obj == null) {
			throw new ArgumentNullException(name);
		}
	} // ValidateNotNull

	private static void ValidateNotNullOrWhiteSpace(String obj, String name) {
		if (obj == null) {
			throw new ArgumentNullException(name);
		}
		if (obj.IsNullOrEmpty() == true) {
			throw new ArgumentException($"The '{name}' argument is empty.");
		}
		if (obj.IsNullOrWhiteSpace() == true) {
			throw new ArgumentException($"The '{name}' argument only contains whitespace characters.");
		}
	} // ValidateNotNullOrWhiteSpace

	private static void ValidateMinimumLength(String obj, Int32 minimumLength, String name) {
		if (obj.Length < minimumLength) {
			throw new ArgumentException($"The '{name}' argument only contains {obj.Length} characters, but must be longer then {minimumLength} characters.");
		}
	} // ValidateMinimumLength
	#endregion

} // RpcStringHashExtensions
#endregion
