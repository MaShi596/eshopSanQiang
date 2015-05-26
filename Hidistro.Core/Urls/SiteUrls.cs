using System;
using System.Collections.Specialized;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Xml;
namespace Hidistro.Core.Urls
{
	public class SiteUrls
	{
		private SiteUrlsData urlData = null;
		public virtual string Home
		{
			get
			{
				return this.urlData.FormatUrl("home");
			}
		}
		public virtual string UserChangePassword
		{
			get
			{
				return this.urlData.FormatUrl("user_ChangePassword");
			}
		}
		public virtual string Favicon
		{
			get
			{
				return this.urlData.FormatUrl("favicon");
			}
		}
		public virtual string LoginReturnHome
		{
			get
			{
				return this.urlData.FormatUrl("login", new object[]
				{
					Globals.ApplicationPath
				});
			}
		}
		public virtual string Login
		{
			get
			{
				string pathAndQuery = HttpContext.Current.Request.Url.PathAndQuery;
				string text = SiteUrls.ExtractQueryParams(pathAndQuery)["ReturnUrl"];
				string result;
				if (string.IsNullOrEmpty(text))
				{
					result = this.urlData.FormatUrl("login", new object[]
					{
						HttpContext.Current.Request.RawUrl
					});
				}
				else
				{
					result = this.urlData.FormatUrl("login", new object[]
					{
						text
					});
				}
				return result;
			}
		}
		public virtual string Logout
		{
			get
			{
				return this.urlData.FormatUrl("logout");
			}
		}
		public virtual string LocationFilter
		{
			get
			{
				return this.urlData.LocationFilter;
			}
		}
		public SiteUrlsData UrlData
		{
			get
			{
				return this.urlData;
			}
		}
		public NameValueCollection ReversePaths
		{
			get
			{
				return this.urlData.ReversePaths;
			}
		}
		public LocationSet Locations
		{
			get
			{
				return this.urlData.LocationSet;
			}
		}
		public LocationSet LocationSet
		{
			get
			{
				return this.urlData.LocationSet;
			}
		}
		public static void EnableHtmRewrite()
		{
			SiteUrls.UpdateHtmRewrite("true");
		}
		public static void CloseHtmRewrite()
		{
			SiteUrls.UpdateHtmRewrite("false");
		}
		public static bool GetEnableHtmRewrite()
		{
			XmlDocument doc = SiteUrls.GetDoc();
			XmlNode xmlNode = doc.SelectSingleNode("SiteUrls");
			XmlAttribute xmlAttribute = xmlNode.Attributes["enableHtmRewrite"];
			return string.Compare(xmlAttribute.Value, "true", true) == 0;
		}
		private static void UpdateHtmRewrite(string status)
		{
			XmlDocument doc = SiteUrls.GetDoc();
			XmlNode xmlNode = doc.SelectSingleNode("SiteUrls");
			XmlAttribute xmlAttribute = xmlNode.Attributes["enableHtmRewrite"];
			if (string.Compare(xmlAttribute.Value, status, true) != 0)
			{
				xmlAttribute.Value = status;
				doc.Save(SiteUrls.GetSiteUrlsFilename());
			}
		}
		private static XmlDocument GetDoc()
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(SiteUrls.GetSiteUrlsFilename());
			return xmlDocument;
		}
		private static string GetSiteUrlsFilename()
		{
			HttpContext current = HttpContext.Current;
			string result;
			if (current != null)
			{
				result = current.Request.MapPath("~/config/SiteUrls.config");
			}
			else
			{
				result = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config\\SiteUrls.config");
			}
			return result;
		}
		public static SiteUrls Instance()
		{
			SiteUrls siteUrls = HiCache.Get("FileCache-SiteUrls") as SiteUrls;
			if (siteUrls == null)
			{
				string siteUrlsFilename = SiteUrls.GetSiteUrlsFilename();
				SiteUrlsData data = new SiteUrlsData(siteUrlsFilename);
				siteUrls = new SiteUrls(data);
				CacheDependency cacheDependency_ = new CacheDependency(siteUrlsFilename);
				HiCache.Max("FileCache-SiteUrls", siteUrls, cacheDependency_);
			}
			return siteUrls;
		}
		public static void ForceRefresh()
		{
			HiCache.Remove("SiteUrls");
		}
		public SiteUrls(SiteUrlsData data)
		{
			this.urlData = data;
		}
		public virtual string RawPath(string rawpath)
		{
			return this.urlData.FormatUrl(rawpath);
		}
		public virtual string Redirect(string string_0)
		{
			return this.urlData.FormatUrl("redirect", new object[]
			{
				Globals.UrlEncode(string_0)
			});
		}
		public virtual string SubCategory(int categoryId, object rewriteName)
		{
			string result;
			if (rewriteName == null || rewriteName == DBNull.Value || string.IsNullOrEmpty(rewriteName.ToString()))
			{
				result = this.urlData.FormatUrl("subCategory", new object[]
				{
					categoryId
				});
			}
			else
			{
				result = this.urlData.FormatUrl("subCategory_Rewrite", new object[]
				{
					categoryId,
					rewriteName
				});
			}
			return result;
		}
		public virtual string SubBrandDetails(int brandId, object rewriteName)
		{
			string result;
			if (rewriteName == null || rewriteName == DBNull.Value || string.IsNullOrEmpty(rewriteName.ToString()))
			{
				result = this.urlData.FormatUrl("branddetails", new object[]
				{
					brandId
				});
			}
			else
			{
				result = this.urlData.FormatUrl("branddetails_Rewrite", new object[]
				{
					brandId,
					rewriteName
				});
			}
			return result;
		}
		public static string RemoveParameters(string string_0)
		{
			string result;
			if (string_0 == null)
			{
				result = string.Empty;
			}
			else
			{
				int num = string_0.IndexOf("?");
				if (num > 0)
				{
					result = string_0.Substring(0, num);
				}
				else
				{
					result = string_0;
				}
			}
			return result;
		}
		protected static NameValueCollection ExtractQueryParams(string string_0)
		{
			NameValueCollection result;
			if (null == string_0)
			{
				result = null;
			}
			else
			{
				int num = string_0.IndexOf("?");
				NameValueCollection nameValueCollection = new NameValueCollection();
				if (num <= 0)
				{
					result = nameValueCollection;
				}
				else
				{
					string[] array = string_0.Substring(num + 1).Split(new char[]
					{
						'&'
					});
					string[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						string text = array2[i];
						string[] array3 = text.Split(new char[]
						{
							'='
						});
						string name = array3[0];
						string value = string.Empty;
						if (array3.Length > 1)
						{
							value = array3[1];
						}
						nameValueCollection.Add(name, value);
					}
					result = nameValueCollection;
				}
			}
			return result;
		}
		public static string FormatUrlWithParameters(string string_0, string parameters)
		{
			string result;
			if (string_0 == null)
			{
				result = string.Empty;
			}
			else
			{
				if (null == parameters)
				{
					result = string_0;
				}
				else
				{
					if (parameters.Length > 0)
					{
						string_0 = string_0 + "?" + parameters;
					}
					result = string_0;
				}
			}
			return result;
		}
	}
}
