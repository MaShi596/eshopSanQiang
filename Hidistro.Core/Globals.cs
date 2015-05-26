using Hidistro.Core.Configuration;
using Hidistro.Core.Enums;
using Hidistro.Core.Urls;
using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
namespace Hidistro.Core
{
	public static class Globals
	{
		public static string IPAddress
		{
			get
			{
				HttpRequest request = HttpContext.Current.Request;
				string text;
				if (string.IsNullOrEmpty(request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
				{
					text = request.ServerVariables["REMOTE_ADDR"];
				}
				else
				{
					text = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
				}
				if (string.IsNullOrEmpty(text))
				{
					text = request.UserHostAddress;
				}
				return text;
			}
		}
		public static string ApplicationPath
		{
			get
			{
				string text = "/";
				if (HttpContext.Current != null)
				{
					text = HttpContext.Current.Request.ApplicationPath;
				}
				string result;
				if (text == "/")
				{
					result = string.Empty;
				}
				else
				{
					result = text.ToLower(CultureInfo.InvariantCulture);
				}
				return result;
			}
		}
		public static string DomainName
		{
			get
			{
				string result;
				if (HttpContext.Current == null)
				{
					result = string.Empty;
				}
				else
				{
					result = HttpContext.Current.Request.Url.Host;
				}
				return result;
			}
		}
		public static void ValidateSecureConnection(SSLSettings sslsettings_0, HttpContext context)
		{
			if (HiConfiguration.GetConfig().SSL == sslsettings_0)
			{
				Globals.RedirectToSSL(context);
			}
		}
		public static SiteUrls GetSiteUrls()
		{
			return SiteUrls.Instance();
		}
		public static string PhysicalPath(string path)
		{
			string result;
			if (null == path)
			{
				result = string.Empty;
			}
			else
			{
				result = Globals.RootPath().TrimEnd(new char[]
				{
					Path.DirectorySeparatorChar
				}) + Path.DirectorySeparatorChar.ToString() + path.TrimStart(new char[]
				{
					Path.DirectorySeparatorChar
				});
			}
			return result;
		}
		private static string RootPath()
		{
			string text = AppDomain.CurrentDomain.BaseDirectory;
			string text2 = Path.DirectorySeparatorChar.ToString();
			text = text.Replace("/", text2);
			string text3 = HiConfiguration.GetConfig().FilesPath;
			if (text3 != null)
			{
				text3 = text3.Replace("/", text2);
				if (text3.Length > 0 && text3.StartsWith(text2) && text.EndsWith(text2))
				{
					text += text3.Substring(1);
				}
				else
				{
					text += text3;
				}
			}
			return text;
		}
		public static string MapPath(string path)
		{
			string result;
			if (string.IsNullOrEmpty(path))
			{
				result = string.Empty;
			}
			else
			{
				HttpContext current = HttpContext.Current;
				if (current != null)
				{
					result = current.Request.MapPath(path);
				}
				else
				{
					result = Globals.PhysicalPath(path.Replace("/", Path.DirectorySeparatorChar.ToString()).Replace("~", ""));
				}
			}
			return result;
		}
		public static void RedirectToSSL(HttpContext context)
		{
			if (null != context && !context.Request.IsSecureConnection)
			{
				Uri url = context.Request.Url;
				context.Response.Redirect("https://" + url.ToString().Substring(7));
			}
		}
		public static string AppendQuerystring(string string_0, string querystring)
		{
			return Globals.AppendQuerystring(string_0, querystring, false);
		}
		public static string AppendQuerystring(string string_0, string querystring, bool urlEncoded)
		{
			if (string.IsNullOrEmpty(string_0))
			{
				throw new ArgumentNullException("url");
			}
			string str = "?";
			if (string_0.IndexOf('?') > -1)
			{
				if (!urlEncoded)
				{
					str = "&";
				}
				else
				{
					str = "&amp;";
				}
			}
			return string_0 + str + querystring;
		}
		public static void EntityCoding(object entity, bool encode)
		{
			if (entity != null)
			{
				Type type = entity.GetType();
				PropertyInfo[] properties = type.GetProperties();
				PropertyInfo[] array = properties;
				for (int i = 0; i < array.Length; i++)
				{
					PropertyInfo propertyInfo = array[i];
					if (propertyInfo.GetCustomAttributes(typeof(HtmlCodingAttribute), true).Length != 0)
					{
						if (!propertyInfo.CanWrite || !propertyInfo.CanRead)
						{
							throw new Exception("使用HtmlEncodeAttribute修饰的属性必须是可读可写的");
						}
						if (!propertyInfo.PropertyType.Equals(typeof(string)))
						{
							throw new Exception("非字符串类型的属性不能使用HtmlEncodeAttribute修饰");
						}
						string text = propertyInfo.GetValue(entity, null) as string;
						if (!string.IsNullOrEmpty(text))
						{
							if (encode)
							{
								propertyInfo.SetValue(entity, Globals.HtmlEncode(text), null);
							}
							else
							{
								propertyInfo.SetValue(entity, Globals.HtmlDecode(text), null);
							}
						}
					}
				}
			}
		}
		public static string HtmlDecode(string textToFormat)
		{
			string result;
			if (string.IsNullOrEmpty(textToFormat))
			{
				result = textToFormat;
			}
			else
			{
				result = HttpUtility.HtmlDecode(textToFormat);
			}
			return result;
		}
		public static string HtmlEncode(string textToFormat)
		{
			string result;
			if (string.IsNullOrEmpty(textToFormat))
			{
				result = textToFormat;
			}
			else
			{
				result = HttpUtility.HtmlEncode(textToFormat);
			}
			return result;
		}
		public static string UrlEncode(string urlToEncode)
		{
			string result;
			if (string.IsNullOrEmpty(urlToEncode))
			{
				result = urlToEncode;
			}
			else
			{
				result = HttpUtility.UrlEncode(urlToEncode, Encoding.UTF8);
			}
			return result;
		}
		public static string UrlDecode(string urlToDecode)
		{
			string result;
			if (string.IsNullOrEmpty(urlToDecode))
			{
				result = urlToDecode;
			}
			else
			{
				result = HttpUtility.UrlDecode(urlToDecode, Encoding.UTF8);
			}
			return result;
		}
		public static string StripScriptTags(string content)
		{
			content = Regex.Replace(content, "<script((.|\n)*?)</script>", "", RegexOptions.IgnoreCase | RegexOptions.Multiline);
			content = Regex.Replace(content, "'javascript:", "", RegexOptions.IgnoreCase | RegexOptions.Multiline);
			return Regex.Replace(content, "\"javascript:", "", RegexOptions.IgnoreCase | RegexOptions.Multiline);
		}
		public static string StripAllTags(string strToStrip)
		{
			strToStrip = Regex.Replace(strToStrip, "</p(?:\\s*)>(?:\\s*)<p(?:\\s*)>", "\n\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			strToStrip = Regex.Replace(strToStrip, "<br(?:\\s*)/>", "\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			strToStrip = Regex.Replace(strToStrip, "\"", "''", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			strToStrip = Globals.StripHtmlXmlTags(strToStrip);
			return strToStrip;
		}
		public static string StripHtmlXmlTags(string content)
		{
			return Regex.Replace(content, "<[^>]+>", "", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		}
		public static string HostPath(Uri uri_0)
		{
			string result;
			if (uri_0 == null)
			{
				result = string.Empty;
			}
			else
			{
				string text = (uri_0.Port == 80) ? string.Empty : (":" + uri_0.Port.ToString(CultureInfo.InvariantCulture));
				result = string.Format(CultureInfo.InvariantCulture, "{0}://{1}{2}", new object[]
				{
					uri_0.Scheme,
					uri_0.Host,
					text
				});
			}
			return result;
		}
		public static string FullPath(string local)
		{
			string result;
			if (string.IsNullOrEmpty(local))
			{
				result = local;
			}
			else
			{
				if (local.ToLower(CultureInfo.InvariantCulture).StartsWith("http://"))
				{
					result = local;
				}
				else
				{
					if (HttpContext.Current == null)
					{
						result = local;
					}
					else
					{
						result = Globals.FullPath(Globals.HostPath(HttpContext.Current.Request.Url), local);
					}
				}
			}
			return result;
		}
		public static string FullPath(string hostPath, string local)
		{
			return hostPath + local;
		}
		public static string UnHtmlEncode(string formattedPost)
		{
			formattedPost = Regex.Replace(formattedPost, "&quot;", "\"", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			formattedPost = Regex.Replace(formattedPost, "&lt;", "<", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			formattedPost = Regex.Replace(formattedPost, "&gt;", ">", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			return formattedPost;
		}
		public static string StripForPreview(string content)
		{
			content = Regex.Replace(content, "<br>", "\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			content = Regex.Replace(content, "<br/>", "\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			content = Regex.Replace(content, "<br />", "\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			content = Regex.Replace(content, "<p>", "\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			content = content.Replace("'", "&#39;");
			return Globals.StripHtmlXmlTags(content);
		}
		public static string ToDelimitedString(ICollection collection, string delimiter)
		{
			string result;
			if (collection == null)
			{
				result = string.Empty;
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (collection is Hashtable)
				{
					foreach (object current in ((Hashtable)collection).Keys)
					{
						stringBuilder.Append(current.ToString() + delimiter);
					}
				}
				if (collection is ArrayList)
				{
					foreach (object current in (ArrayList)collection)
					{
						stringBuilder.Append(current.ToString() + delimiter);
					}
				}
				if (collection is string[])
				{
					string[] array = (string[])collection;
					for (int i = 0; i < array.Length; i++)
					{
						string str = array[i];
						stringBuilder.Append(str + delimiter);
					}
				}
				if (collection is MailAddressCollection)
				{
					foreach (MailAddress current2 in (MailAddressCollection)collection)
					{
						stringBuilder.Append(current2.Address + delimiter);
					}
				}
				result = stringBuilder.ToString().TrimEnd(new char[]
				{
					Convert.ToChar(delimiter, CultureInfo.InvariantCulture)
				});
			}
			return result;
		}
		public static string GetAdminAbsolutePath(string path)
		{
			string result;
			if (path.StartsWith("/"))
			{
				result = Globals.ApplicationPath + "/" + HiConfiguration.GetConfig().AdminFolder + path;
			}
			else
			{
				result = string.Concat(new string[]
				{
					Globals.ApplicationPath,
					"/",
					HiConfiguration.GetConfig().AdminFolder,
					"/",
					path
				});
			}
			return result;
		}
		public static string FormatMoney(decimal money)
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}", new object[]
			{
				money.ToString("F", CultureInfo.InvariantCulture)
			});
		}
		public static int[] BubbleSort(int[] int_0)
		{
			for (int i = 0; i < int_0.Length; i++)
			{
				bool flag = false;
				for (int j = int_0.Length - 2; j >= i; j--)
				{
					if (int_0[j + 1] > int_0[j])
					{
						int num = int_0[j + 1];
						int_0[j + 1] = int_0[j];
						int_0[j] = num;
						flag = true;
					}
				}
				if (!flag)
				{
					break;
				}
			}
			return int_0;
		}
		public static string GetGenerateId()
		{
			string text = string.Empty;
			Random random = new Random();
			for (int i = 0; i < 7; i++)
			{
				int num = random.Next();
				text += ((char)(48 + (ushort)(num % 10))).ToString();
			}
			return DateTime.Now.ToString("yyyyMMdd") + text;
		}
	}
}
