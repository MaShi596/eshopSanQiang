using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
namespace ZWL.Common.DEncrypt
{
	public class RSACryption
	{
		public void RSAKey(out string xmlKeys, out string xmlPublicKey)
		{
			System.Security.Cryptography.RSACryptoServiceProvider rSACryptoServiceProvider = new System.Security.Cryptography.RSACryptoServiceProvider();
			xmlKeys = rSACryptoServiceProvider.ToXmlString(true);
			xmlPublicKey = rSACryptoServiceProvider.ToXmlString(false);
		}
		public string RSAEncrypt(string xmlPublicKey, string m_strEncryptString)
		{
			System.Security.Cryptography.RSACryptoServiceProvider rSACryptoServiceProvider = new System.Security.Cryptography.RSACryptoServiceProvider();
			rSACryptoServiceProvider.FromXmlString(xmlPublicKey);
			byte[] bytes = new System.Text.UnicodeEncoding().GetBytes(m_strEncryptString);
			byte[] inArray = rSACryptoServiceProvider.Encrypt(bytes, false);
			return System.Convert.ToBase64String(inArray);
		}
		public string RSAEncrypt(string xmlPublicKey, byte[] EncryptString)
		{
			System.Security.Cryptography.RSACryptoServiceProvider rSACryptoServiceProvider = new System.Security.Cryptography.RSACryptoServiceProvider();
			rSACryptoServiceProvider.FromXmlString(xmlPublicKey);
			byte[] inArray = rSACryptoServiceProvider.Encrypt(EncryptString, false);
			return System.Convert.ToBase64String(inArray);
		}
		public string RSADecrypt(string xmlPrivateKey, string m_strDecryptString)
		{
			System.Security.Cryptography.RSACryptoServiceProvider rSACryptoServiceProvider = new System.Security.Cryptography.RSACryptoServiceProvider();
			rSACryptoServiceProvider.FromXmlString(xmlPrivateKey);
			byte[] rgb = System.Convert.FromBase64String(m_strDecryptString);
			byte[] bytes = rSACryptoServiceProvider.Decrypt(rgb, false);
			return new System.Text.UnicodeEncoding().GetString(bytes);
		}
		public string RSADecrypt(string xmlPrivateKey, byte[] DecryptString)
		{
			System.Security.Cryptography.RSACryptoServiceProvider rSACryptoServiceProvider = new System.Security.Cryptography.RSACryptoServiceProvider();
			rSACryptoServiceProvider.FromXmlString(xmlPrivateKey);
			byte[] bytes = rSACryptoServiceProvider.Decrypt(DecryptString, false);
			return new System.Text.UnicodeEncoding().GetString(bytes);
		}
		public bool GetHash(string m_strSource, ref byte[] HashData)
		{
			System.Security.Cryptography.HashAlgorithm hashAlgorithm = System.Security.Cryptography.HashAlgorithm.Create("MD5");
			byte[] bytes = System.Text.Encoding.GetEncoding("GB2312").GetBytes(m_strSource);
			HashData = hashAlgorithm.ComputeHash(bytes);
			return true;
		}
		public bool GetHash(string m_strSource, ref string strHashData)
		{
			System.Security.Cryptography.HashAlgorithm hashAlgorithm = System.Security.Cryptography.HashAlgorithm.Create("MD5");
			byte[] bytes = System.Text.Encoding.GetEncoding("GB2312").GetBytes(m_strSource);
			byte[] inArray = hashAlgorithm.ComputeHash(bytes);
			strHashData = System.Convert.ToBase64String(inArray);
			return true;
		}
		public bool GetHash(System.IO.FileStream objFile, ref byte[] HashData)
		{
			System.Security.Cryptography.HashAlgorithm hashAlgorithm = System.Security.Cryptography.HashAlgorithm.Create("MD5");
			HashData = hashAlgorithm.ComputeHash(objFile);
			objFile.Close();
			return true;
		}
		public bool GetHash(System.IO.FileStream objFile, ref string strHashData)
		{
			System.Security.Cryptography.HashAlgorithm hashAlgorithm = System.Security.Cryptography.HashAlgorithm.Create("MD5");
			byte[] inArray = hashAlgorithm.ComputeHash(objFile);
			objFile.Close();
			strHashData = System.Convert.ToBase64String(inArray);
			return true;
		}
		public bool SignatureFormatter(string p_strKeyPrivate, byte[] HashbyteSignature, ref byte[] EncryptedSignatureData)
		{
			System.Security.Cryptography.RSACryptoServiceProvider rSACryptoServiceProvider = new System.Security.Cryptography.RSACryptoServiceProvider();
			rSACryptoServiceProvider.FromXmlString(p_strKeyPrivate);
			System.Security.Cryptography.RSAPKCS1SignatureFormatter rSAPKCS1SignatureFormatter = new System.Security.Cryptography.RSAPKCS1SignatureFormatter(rSACryptoServiceProvider);
			rSAPKCS1SignatureFormatter.SetHashAlgorithm("MD5");
			EncryptedSignatureData = rSAPKCS1SignatureFormatter.CreateSignature(HashbyteSignature);
			return true;
		}
		public bool SignatureFormatter(string p_strKeyPrivate, byte[] HashbyteSignature, ref string m_strEncryptedSignatureData)
		{
			System.Security.Cryptography.RSACryptoServiceProvider rSACryptoServiceProvider = new System.Security.Cryptography.RSACryptoServiceProvider();
			rSACryptoServiceProvider.FromXmlString(p_strKeyPrivate);
			System.Security.Cryptography.RSAPKCS1SignatureFormatter rSAPKCS1SignatureFormatter = new System.Security.Cryptography.RSAPKCS1SignatureFormatter(rSACryptoServiceProvider);
			rSAPKCS1SignatureFormatter.SetHashAlgorithm("MD5");
			byte[] inArray = rSAPKCS1SignatureFormatter.CreateSignature(HashbyteSignature);
			m_strEncryptedSignatureData = System.Convert.ToBase64String(inArray);
			return true;
		}
		public bool SignatureFormatter(string p_strKeyPrivate, string m_strHashbyteSignature, ref byte[] EncryptedSignatureData)
		{
			byte[] rgbHash = System.Convert.FromBase64String(m_strHashbyteSignature);
			System.Security.Cryptography.RSACryptoServiceProvider rSACryptoServiceProvider = new System.Security.Cryptography.RSACryptoServiceProvider();
			rSACryptoServiceProvider.FromXmlString(p_strKeyPrivate);
			System.Security.Cryptography.RSAPKCS1SignatureFormatter rSAPKCS1SignatureFormatter = new System.Security.Cryptography.RSAPKCS1SignatureFormatter(rSACryptoServiceProvider);
			rSAPKCS1SignatureFormatter.SetHashAlgorithm("MD5");
			EncryptedSignatureData = rSAPKCS1SignatureFormatter.CreateSignature(rgbHash);
			return true;
		}
		public bool SignatureFormatter(string p_strKeyPrivate, string m_strHashbyteSignature, ref string m_strEncryptedSignatureData)
		{
			byte[] rgbHash = System.Convert.FromBase64String(m_strHashbyteSignature);
			System.Security.Cryptography.RSACryptoServiceProvider rSACryptoServiceProvider = new System.Security.Cryptography.RSACryptoServiceProvider();
			rSACryptoServiceProvider.FromXmlString(p_strKeyPrivate);
			System.Security.Cryptography.RSAPKCS1SignatureFormatter rSAPKCS1SignatureFormatter = new System.Security.Cryptography.RSAPKCS1SignatureFormatter(rSACryptoServiceProvider);
			rSAPKCS1SignatureFormatter.SetHashAlgorithm("MD5");
			byte[] inArray = rSAPKCS1SignatureFormatter.CreateSignature(rgbHash);
			m_strEncryptedSignatureData = System.Convert.ToBase64String(inArray);
			return true;
		}
		public bool SignatureDeformatter(string p_strKeyPublic, byte[] HashbyteDeformatter, byte[] DeformatterData)
		{
			System.Security.Cryptography.RSACryptoServiceProvider rSACryptoServiceProvider = new System.Security.Cryptography.RSACryptoServiceProvider();
			rSACryptoServiceProvider.FromXmlString(p_strKeyPublic);
			System.Security.Cryptography.RSAPKCS1SignatureDeformatter rSAPKCS1SignatureDeformatter = new System.Security.Cryptography.RSAPKCS1SignatureDeformatter(rSACryptoServiceProvider);
			rSAPKCS1SignatureDeformatter.SetHashAlgorithm("MD5");
			return rSAPKCS1SignatureDeformatter.VerifySignature(HashbyteDeformatter, DeformatterData);
		}
		public bool SignatureDeformatter(string p_strKeyPublic, string p_strHashbyteDeformatter, byte[] DeformatterData)
		{
			byte[] rgbHash = System.Convert.FromBase64String(p_strHashbyteDeformatter);
			System.Security.Cryptography.RSACryptoServiceProvider rSACryptoServiceProvider = new System.Security.Cryptography.RSACryptoServiceProvider();
			rSACryptoServiceProvider.FromXmlString(p_strKeyPublic);
			System.Security.Cryptography.RSAPKCS1SignatureDeformatter rSAPKCS1SignatureDeformatter = new System.Security.Cryptography.RSAPKCS1SignatureDeformatter(rSACryptoServiceProvider);
			rSAPKCS1SignatureDeformatter.SetHashAlgorithm("MD5");
			return rSAPKCS1SignatureDeformatter.VerifySignature(rgbHash, DeformatterData);
		}
		public bool SignatureDeformatter(string p_strKeyPublic, byte[] HashbyteDeformatter, string p_strDeformatterData)
		{
			System.Security.Cryptography.RSACryptoServiceProvider rSACryptoServiceProvider = new System.Security.Cryptography.RSACryptoServiceProvider();
			rSACryptoServiceProvider.FromXmlString(p_strKeyPublic);
			System.Security.Cryptography.RSAPKCS1SignatureDeformatter rSAPKCS1SignatureDeformatter = new System.Security.Cryptography.RSAPKCS1SignatureDeformatter(rSACryptoServiceProvider);
			rSAPKCS1SignatureDeformatter.SetHashAlgorithm("MD5");
			byte[] rgbSignature = System.Convert.FromBase64String(p_strDeformatterData);
			return rSAPKCS1SignatureDeformatter.VerifySignature(HashbyteDeformatter, rgbSignature);
		}
		public bool SignatureDeformatter(string p_strKeyPublic, string p_strHashbyteDeformatter, string p_strDeformatterData)
		{
			byte[] rgbHash = System.Convert.FromBase64String(p_strHashbyteDeformatter);
			System.Security.Cryptography.RSACryptoServiceProvider rSACryptoServiceProvider = new System.Security.Cryptography.RSACryptoServiceProvider();
			rSACryptoServiceProvider.FromXmlString(p_strKeyPublic);
			System.Security.Cryptography.RSAPKCS1SignatureDeformatter rSAPKCS1SignatureDeformatter = new System.Security.Cryptography.RSAPKCS1SignatureDeformatter(rSACryptoServiceProvider);
			rSAPKCS1SignatureDeformatter.SetHashAlgorithm("MD5");
			byte[] rgbSignature = System.Convert.FromBase64String(p_strDeformatterData);
			return rSAPKCS1SignatureDeformatter.VerifySignature(rgbHash, rgbSignature);
		}
	}
}
