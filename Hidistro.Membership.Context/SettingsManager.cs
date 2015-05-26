using Hidistro.Core;
using System;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Xml;
namespace Hidistro.Membership.Context
{
	public static class SettingsManager
	{
		private const string string_0 = "FileCache-MasterSettings";
		private const string string_1 = "DataCache-Settings:{0}";
		public static SiteSettings GetMasterSettings(bool cacheable)
		{
			if (!cacheable)
			{
				HiCache.Remove("FileCache-MasterSettings");
			}
			XmlDocument xmlDocument = HiCache.Get("FileCache-MasterSettings") as XmlDocument;
			if (xmlDocument == null)
			{
				string text = SettingsManager.smethod_0();
				if (!System.IO.File.Exists(text))
				{
					return null;
				}
				xmlDocument = new XmlDocument();
				xmlDocument.Load(text);
				if (cacheable)
				{
					HiCache.Max("FileCache-MasterSettings", xmlDocument, new CacheDependency(text));
				}
			}
			return SiteSettings.FromXml(xmlDocument);
		}
		private static string smethod_0()
		{
			HttpContext current = HttpContext.Current;
			if (current == null)
			{
				return System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "config\\SiteSettings.config");
			}
			return current.Request.MapPath("~/config/SiteSettings.config");
		}
		public static SiteSettings GetSiteSettings()
		{
			string siteUrl = HiContext.Current.SiteUrl;
			string text = string.Format("DataCache-Settings:{0}", siteUrl);
			SiteSettings siteSettings = HiCache.Get(text) as SiteSettings;
			if (siteSettings != null)
			{
				return siteSettings;
			}
			siteSettings = SiteSettingsProvider.Instance().GetDistributorSettings(siteUrl);
			if (siteSettings != null)
			{
				HiCache.Insert(text, siteSettings, null, 180);
				return siteSettings;
			}
			return SettingsManager.GetMasterSettings(true);
		}
		public static SiteSettings GetSiteSettings(int distributorUserId)
		{
			return SiteSettingsProvider.Instance().GetDistributorSettings(distributorUserId);
		}
		public static void Save(SiteSettings settings)
		{
			if (settings.IsDistributorSettings)
			{
				SiteSettingsProvider.Instance().SaveDistributorSettings(settings);
				HiCache.Remove(string.Format("DataCache-Settings:{0}", settings.SiteUrl));
				return;
			}
			SettingsManager.smethod_1(settings);
			HiCache.Remove("FileCache-MasterSettings");
		}
		private static void smethod_1(SiteSettings siteSettings_0)
		{
			string text = SettingsManager.smethod_0();
			XmlDocument xmlDocument = new XmlDocument();
			if (System.IO.File.Exists(text))
			{
				xmlDocument.Load(text);
			}
			siteSettings_0.WriteToXml(xmlDocument);
			xmlDocument.Save(text);
		}
	}
}
