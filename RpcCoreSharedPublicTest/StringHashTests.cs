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
		Byte[] salt1 = Convert.FromHexString("dfcc53c33ad10e9a");
		String ssha1 = Convert.ToBase64String(Convert.FromHexString("31db23410511e6be6b5f1975a2136643f9f493e5" + "dfcc53c33ad10e9a"));
		Byte[] salt256 = Convert.FromHexString("d586f54077a9ac78");
		String ssha256 = Convert.ToBase64String(Convert.FromHexString("b3b5ff79c67de9e22948628227bc54b3ac0d8fdef5fd52abd23be0b9063e496a" + "d586f54077a9ac78"));
		Byte[] salt384 = Convert.FromHexString("63f35e14b76f4d31");
		String ssha384 = Convert.ToBase64String(Convert.FromHexString("3b8462a3ffd6445ef7b585a39891891928f5b0ac76d7badcfc6a75fb53e9103ba8087ea850b8ab613806fa6e75aa45ff" + "63f35e14b76f4d31"));
		Byte[] salt512 = Convert.FromHexString("155f1bd26ca4d090");
		String ssha512 = Convert.ToBase64String(Convert.FromHexString("18a95a9cef23dd01b458b6ad26885bfc0e77bf1b49b8817ab8e2184623d392e812ba785173a13f1f0f4395078bde55e1d634246faeabda00c3f5cec994209676" + "155f1bd26ca4d090"));

		String pwdMd5 = "{MD5}7ZvBMpk+hP3NTzuzu0NPiw==";
		String pwdSha1 = "{SHA}q1jk45pqZz0q3QFPDvF4h6wjRow=";
		String pwdSha256 = "{SHA256}s/bOFam3fYf+aPONXlkpZFLl4MYk49cV0AaDQfFhIms=";
		String pwdSha384 = "{SHA384}6E4tsMHoVl2s2yxwkPuZUruApaiCskTqVZ35Y8Zc6ipO4fi3IPuLcYGO7jEJJPHe";
		String pwdSha512 = "{SHA512}Xdm4Z83TvgMZUvMdW66eRgBhWyOC/Sgjs3PfIiqJe6gxeVgT63wGiLf41DMttK8cqFv0MnvbI0OZD/glE0ILRw==";

		String pwdSsha1 = "{SSHA}MdsjQQUR5r5rXxl1ohNmQ/n0k+XfzFPDOtEOmg==";
		String pwdSsha256 = "{SSHA256}s7X/ecZ96eIpSGKCJ7xUs6wNj971/VKr0jvguQY+SWrVhvVAd6mseA==";
		String pwdSsha384 = "{SSHA384}O4Rio//WRF73tYWjmJGJGSj1sKx217rc/Gp1+1PpEDuoCH6oULirYTgG+m51qkX/Y/NeFLdvTTE=";
		String pwdSsha512 = "{SSHA512}GKlanO8j3QG0WLatJohb/A53vxtJuIF6uOIYRiPTkugSunhRc6E/Hw9DlQeL3lXh1jQkb66r2gDD9c7JlCCWdhVfG9JspNCQ";

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

		Assert.IsTrue(pwdMd5.ValidateStringHash(clear));
		Assert.IsTrue(pwdSha1.ValidateStringHash(clear));
		Assert.IsTrue(pwdSha256.ValidateStringHash(clear));
		Assert.IsTrue(pwdSha384.ValidateStringHash(clear));
		Assert.IsTrue(pwdSha512.ValidateStringHash(clear));

		Assert.IsTrue(pwdSsha1.ValidateStringHash(clear));
		Assert.IsTrue(pwdSsha256.ValidateStringHash(clear));
		Assert.IsTrue(pwdSsha384.ValidateStringHash(clear));
		Assert.IsTrue(pwdSsha512.ValidateStringHash(clear));

//ValidateStringHash(this String hashedValue, String clearValue) {

	} // Test

} // RpcStringHashExtensionsTests
