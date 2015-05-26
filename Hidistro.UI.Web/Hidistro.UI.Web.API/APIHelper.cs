using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Security.Cryptography;
using System.Text;
namespace Hidistro.UI.Web.API
{
	public class APIHelper
	{
		public static System.Collections.Generic.Dictionary<string, string> Parameterfilter(System.Collections.Generic.SortedDictionary<string, string> dicArrayPre)
		{
			System.Collections.Generic.Dictionary<string, string> dictionary = new System.Collections.Generic.Dictionary<string, string>();
			foreach (System.Collections.Generic.KeyValuePair<string, string> current in dicArrayPre)
			{
				if (current.Key.ToLower() != "sign" && current.Key.ToLower() != "sign_type" && current.Value != "" && current.Value != null)
				{
					dictionary.Add(current.Key.ToLower(), current.Value);
				}
			}
			return dictionary;
		}
		public static string BuildSign(System.Collections.Generic.Dictionary<string, string> dicArray, string key, string sign_type, string _input_charset)
		{
			string text = APIHelper.CreateLinkstring(dicArray);
			text += key;
			return APIHelper.Sign(text, sign_type, _input_charset);
		}
		public static string CreateLinkstring(System.Collections.Generic.Dictionary<string, string> dicArray)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			foreach (System.Collections.Generic.KeyValuePair<string, string> current in dicArray)
			{
				stringBuilder.Append(current.Key + "=" + current.Value + "&");
			}
			int length = stringBuilder.Length;
			stringBuilder.Remove(length - 1, 1);
			return stringBuilder.ToString();
		}
		public static string Sign(string prestr, string sign_type, string _input_charset)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder(32);
			if (sign_type.ToUpper() == "MD5")
			{
				System.Security.Cryptography.MD5 mD = new System.Security.Cryptography.MD5CryptoServiceProvider();
				byte[] array = mD.ComputeHash(System.Text.Encoding.GetEncoding(_input_charset).GetBytes(prestr));
				for (int i = 0; i < array.Length; i++)
				{
					stringBuilder.Append(array[i].ToString("x").PadLeft(2, '0'));
				}
			}
			return stringBuilder.ToString();
		}
		public static string PostData(string url, string postData)
		{
			string result = string.Empty;
			try
			{
				System.Uri requestUri = new System.Uri(url);
				System.Net.HttpWebRequest httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(requestUri);
				System.Text.Encoding uTF = System.Text.Encoding.UTF8;
				byte[] bytes = uTF.GetBytes(postData);
				httpWebRequest.Method = "POST";
				httpWebRequest.ContentType = "application/x-www-form-urlencoded";
				httpWebRequest.ContentLength = (long)bytes.Length;
				using (System.IO.Stream requestStream = httpWebRequest.GetRequestStream())
				{
					requestStream.Write(bytes, 0, bytes.Length);
				}
				using (System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)httpWebRequest.GetResponse())
				{
					using (System.IO.Stream responseStream = httpWebResponse.GetResponseStream())
					{
						System.Text.Encoding uTF2 = System.Text.Encoding.UTF8;
						System.IO.Stream stream = responseStream;
						if (httpWebResponse.ContentEncoding.ToLower() == "gzip")
						{
							stream = new System.IO.Compression.GZipStream(responseStream, System.IO.Compression.CompressionMode.Decompress);
						}
						else
						{
							if (httpWebResponse.ContentEncoding.ToLower() == "deflate")
							{
								stream = new System.IO.Compression.DeflateStream(responseStream, System.IO.Compression.CompressionMode.Decompress);
							}
						}
						using (System.IO.StreamReader streamReader = new System.IO.StreamReader(stream, uTF2))
						{
							result = streamReader.ReadToEnd();
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				result = string.Format("获取信息错误：{0}", ex.Message);
			}
			return result;
		}
		public static bool CheckSign(System.Collections.Generic.SortedDictionary<string, string> tmpParas, string keycode, string sign)
		{
			System.Collections.Generic.Dictionary<string, string> dicArray = APIHelper.Parameterfilter(tmpParas);
			return APIHelper.BuildSign(dicArray, keycode, "MD5", "utf-8") == sign;
		}
	}
}
