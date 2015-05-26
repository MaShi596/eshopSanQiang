using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
namespace Hidistro.Core
{
	public sealed class HiCryptographer
	{
		public static string Encrypt(string text)
		{
			string result;
			using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
			{
				rijndaelManaged.Key = Convert.FromBase64String(ConfigurationManager.AppSettings["Key"]);
				rijndaelManaged.IV = Convert.FromBase64String(ConfigurationManager.AppSettings["IV"]);
				ICryptoTransform cryptoTransform = rijndaelManaged.CreateEncryptor();
				byte[] bytes = Encoding.UTF8.GetBytes(text);
				byte[] inArray = cryptoTransform.TransformFinalBlock(bytes, 0, bytes.Length);
				cryptoTransform.Dispose();
				result = Convert.ToBase64String(inArray);
			}
			return result;
		}
		public static string Decrypt(string text)
		{
			string @string;
			using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
			{
				rijndaelManaged.Key = Convert.FromBase64String(ConfigurationManager.AppSettings["Key"]);
				rijndaelManaged.IV = Convert.FromBase64String(ConfigurationManager.AppSettings["IV"]);
				ICryptoTransform cryptoTransform = rijndaelManaged.CreateDecryptor();
				byte[] array = Convert.FromBase64String(text);
				byte[] bytes = cryptoTransform.TransformFinalBlock(array, 0, array.Length);
				cryptoTransform.Dispose();
				@string = Encoding.UTF8.GetString(bytes);
			}
			return @string;
		}
		private static byte[] CreateHash(byte[] plaintext)
		{
			MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
			return mD5CryptoServiceProvider.ComputeHash(plaintext);
		}
		public static string CreateHash(string plaintext)
		{
			byte[] array = HiCryptographer.CreateHash(Encoding.ASCII.GetBytes(plaintext));
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("x2"));
			}
			return stringBuilder.ToString();
		}
	}
}
