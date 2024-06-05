using System;
namespace RpcScandinavia.Core.Shared;

#region RpcHashAlgorithm
//----------------------------------------------------------------------------------------------------------------------
// RpcHashAlgorithm.
//----------------------------------------------------------------------------------------------------------------------
/// <summary>
/// The hash algorithm.
/// </summary>
public enum RpcHashAlgorithm {

	//------------------------------------------------------------------------------------------------------------------
	// Normal hashed strings.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Do not hash the string.
	/// </summary>
	None,

	/// <summary>
	/// Hash password using the MD5 algorithm.
	/// The hash size is 128 bits, 16 bytes long.
	/// This algorithms have been found to be insecure.
	/// </summary>
	MD5,

	/// <summary>
	/// Hash password using the SHA1 algorithm.
	/// The hash size is 160 bits, 20 bytes long.
	/// This algorithms have been found to be insecure.
	/// </summary>
	SHA1,

	/// <summary>
	/// Hash password using the SHA2 algorithm.
	/// The hash size is 256 bits, 32 bytes long.
	/// </summary>
	SHA256,

	/// <summary>
	/// Hash password using the SHA2 algorithm.
	/// The hash size is 384 bits, 48 bytes long.
	/// </summary>
	SHA384,

	/// <summary>
	/// Hash password using the SHA2 algorithm.
	/// The hash size is 512 bits, 64 bytes long.
	/// </summary>
	SHA512,

	//------------------------------------------------------------------------------------------------------------------
	// Salted hashed strings.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Hash password using the MD5 algorithm and a salt.
	/// The hash size is 128 bits, 16 bytes long.
	/// This algorithms have been found to be insecure.
	/// </summary>
	SMD5,

	/// <summary>
	/// Hash password using the SHA1 algorithm and a salt.
	/// The hash size is 160 bits, 20 bytes long.
	/// This algorithms have been found to be insecure.
	/// </summary>
	SSHA1,

	/// <summary>
	/// Hash password using the SHA2 algorithm and a salt.
	/// The hash size is 256 bits, 32 bytes long.
	/// </summary>
	SSHA256,

	/// <summary>
	/// Hash password using the SHA2 algorithm and a salt.
	/// The hash size is 384 bits, 48 bytes long.
	/// </summary>
	SSHA384,

	/// <summary>
	/// Hash password using the SHA2 algorithm and a salt.
	/// The hash size is 512 bits, 64 bytes long.
	/// </summary>
	SSHA512,

	//------------------------------------------------------------------------------------------------------------------
	// Special hashed strings.
	//------------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// Hash password using the MD5 algorithm for Kopano mail database.
	/// The hash size is 128 bits, 16 bytes long.
	/// This algorithms have been found to be insecure.
	/// </summary>
	KopanoMD5,

} // RpcHashAlgorithm
#endregion
