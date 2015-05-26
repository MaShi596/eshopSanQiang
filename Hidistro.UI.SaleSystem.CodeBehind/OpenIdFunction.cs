using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web;
namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public static class OpenIdFunction
	{
		private const string FormFormat = "<form id=\"openidform\" name=\"openidform\" action=\"{0}\" method=\"POST\">{1}</form>";
		private const string InputFormat = "<input type=\"hidden\" id=\"{0}\" name=\"{0}\" value=\"{1}\">";
		public static string CreateField(string name, string strValue)
		{
			return string.Format(System.Globalization.CultureInfo.InvariantCulture, "<input type=\"hidden\" id=\"{0}\" name=\"{0}\" value=\"{1}\">", new object[]
			{
				name,
				strValue
			});
		}
		public static string CreateForm(string content, string action)
		{
			content += "<input type=\"submit\" value=\"信任登录\" style=\"display:none;\">";
			return string.Format(System.Globalization.CultureInfo.InvariantCulture, "<form id=\"openidform\" name=\"openidform\" action=\"{0}\" method=\"POST\">{1}</form>", new object[]
			{
				action,
				content
			});
		}
		public static void Submit(string formContent)
		{
			string s = formContent + "<script>document.forms['openidform'].submit();</script>";
			System.Web.HttpContext.Current.Response.Write(s);
			System.Web.HttpContext.Current.Response.End();
		}
		public static string BuildMysign(System.Collections.Generic.Dictionary<string, string> dicArray, string string_0, string sign_type, string _input_charset)
		{
			string text = OpenIdFunction.CreateLinkString(dicArray);
			text += string_0;
			return OpenIdFunction.Sign(text, sign_type, _input_charset);
		}
		public static System.Collections.Generic.Dictionary<string, string> FilterPara(System.Collections.Generic.SortedDictionary<string, string> dicArrayPre)
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
		public static string CreateLinkString(System.Collections.Generic.Dictionary<string, string> dicArray)
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
	}
}
