using System;
using System.Security.Cryptography;
using System.Text;
namespace Hidistro.Core.DEncrypt
{
	public static class DEncrypt
	{
		public static string Encrypt(string original)
		{
			return DEncrypt.Encrypt(original, "zwl168918");
		}
		public static string Decrypt(string original)
		{
			return DEncrypt.Decrypt(original, "zwl168918", System.Text.Encoding.Default);
		}
		public static string Encrypt(string original, string string_0)
		{
			byte[] bytes = System.Text.Encoding.Default.GetBytes(original);
			byte[] bytes2 = System.Text.Encoding.Default.GetBytes(string_0);
			return System.Convert.ToBase64String(DEncrypt.Encrypt(bytes, bytes2));
		}
		public static string Decrypt(string original, string string_0)
		{
			return DEncrypt.Decrypt(original, string_0, System.Text.Encoding.Default);
		}
		public static string Decrypt(string encrypted, string string_0, System.Text.Encoding encoding)
		{
			byte[] encrypted2 = System.Convert.FromBase64String(encrypted);
			byte[] bytes = System.Text.Encoding.Default.GetBytes(string_0);
			return encoding.GetString(DEncrypt.Decrypt(encrypted2, bytes));
		}
		public static byte[] Decrypt(byte[] encrypted)
		{
			byte[] bytes = System.Text.Encoding.Default.GetBytes("kgditfkwvfp");
			return DEncrypt.Decrypt(encrypted, bytes);
		}
		public static byte[] Encrypt(byte[] original)
		{
			byte[] bytes = System.Text.Encoding.Default.GetBytes("kgditfkwvfp");
			return DEncrypt.Encrypt(original, bytes);
		}
		public static byte[] MakeMD5(byte[] original)
		{
			System.Security.Cryptography.MD5CryptoServiceProvider mD5CryptoServiceProvider = new System.Security.Cryptography.MD5CryptoServiceProvider();
			return mD5CryptoServiceProvider.ComputeHash(original);
		}
		public static byte[] Encrypt(byte[] original, byte[] byte_0)
		{
			return new System.Security.Cryptography.TripleDESCryptoServiceProvider
			{
				Key = DEncrypt.MakeMD5(byte_0),
				Mode = System.Security.Cryptography.CipherMode.ECB
			}.CreateEncryptor().TransformFinalBlock(original, 0, original.Length);
		}
		public static byte[] Decrypt(byte[] encrypted, byte[] byte_0)
		{
			return new System.Security.Cryptography.TripleDESCryptoServiceProvider
			{
				Key = DEncrypt.MakeMD5(byte_0),
				Mode = System.Security.Cryptography.CipherMode.ECB
			}.CreateDecryptor().TransformFinalBlock(encrypted, 0, encrypted.Length);
		}
	}
}
