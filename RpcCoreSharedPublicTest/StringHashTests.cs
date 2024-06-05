using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RpcScandinavia.Core.Shared;
namespace RpcScandinavia.Core.Shared.Tests;

[TestClass]
public class RpcStringHashExtensionsTests {

	[TestMethod]
	public void Test() {
		// Prepare.
		// Hashed values taken from "https://www.pelock.com/products/hash-calculator".
		String clear = "This is a very long passphrase, that can be hashed, oh, we need some numb3r5 t00.";
		String md5 = Convert.ToBase64String(Convert.FromHexString("ED9BC132993E84FDCD4F3BB3BB434F8B"));
		String sha1 = Convert.ToBase64String(Convert.FromHexString("AB58E4E39A6A673D2ADD014F0EF17887AC23468C"));
		String sha256 = Convert.ToBase64String(Convert.FromHexString("B3F6CE15A9B77D87FE68F38D5E59296452E5E0C624E3D715D0068341F161226B"));
		String sha384 = Convert.ToBase64String(Convert.FromHexString("E84E2DB0C1E8565DACDB2C7090FB9952BB80A5A882B244EA559DF963C65CEA2A4EE1F8B720FB8B71818EEE310924F1DE"));
		String sha512 = Convert.ToBase64String(Convert.FromHexString("5DD9B867CDD3BE031952F31D5BAE9E4600615B2382FD2823B373DF222A897BA831795813EB7C0688B7F8D4332DB4AF1CA85BF4327BDB2343990FF82513420B47"));

		// Hashed values taken from Apache Directory Studio.
		Byte[] salt1 = Convert.FromHexString("0b9404733b334dd4");
		String ssha1 = Convert.ToBase64String(Convert.FromHexString("d42715a6e61d0fbdb1144bb3cd7731964486716a" + "0b9404733b334dd4"));
		Byte[] salt256 = Convert.FromHexString("db03d4fe1b781e32");
		String ssha256 = Convert.ToBase64String(Convert.FromHexString("f8621e9a436379c07b671064325c8f3f1a8af7dce4341e7f0e3c78ecfee40350" + "db03d4fe1b781e32"));
		Byte[] salt384 = Convert.FromHexString("2c08c6a171e6790c");
		String ssha384 = Convert.ToBase64String(Convert.FromHexString("3f9317766ba5438fce67d7a562964ec7415f98b6e234682bc4d699780c5e8ba8f47f619e3cb8b03de741bdc7d647a027" + "2c08c6a171e6790c"));
		Byte[] salt512 = Convert.FromHexString("3e97d4a4c6643e65");
		String ssha512 = Convert.ToBase64String(Convert.FromHexString("9ce00022d871d5aee480738800a4e58debf1469b33a4188a37eb6c57a6e60ef58290e8d2aca2e268510b43489885a28473fa4889aab4535d96818136281527f5" + "3e97d4a4c6643e65"));

		// Assert.
		Assert.AreEqual(md5, clear.ToStringHash(RpcHashAlgorithm.MD5, false));
		Assert.AreEqual(sha1, clear.ToStringHash(RpcHashAlgorithm.SHA1, false));
		Assert.AreEqual(sha256, clear.ToStringHash(RpcHashAlgorithm.SHA256, false));
		Assert.AreEqual(sha384, clear.ToStringHash(RpcHashAlgorithm.SHA384, false));
		Assert.AreEqual(sha512, clear.ToStringHash(RpcHashAlgorithm.SHA512, false));

		Assert.AreEqual(ssha1, clear.ToStringHash(RpcHashAlgorithm.SSHA1, false, salt1));
		Assert.AreEqual(ssha256, clear.ToStringHash(RpcHashAlgorithm.SSHA256, false, salt256));
		Assert.AreEqual(ssha384, clear.ToStringHash(RpcHashAlgorithm.SSHA384, false, salt384));
		Assert.AreEqual(ssha512, clear.ToStringHash(RpcHashAlgorithm.SSHA512, false, salt512));

	} // Test

} // RpcStringHashExtensionsTests
