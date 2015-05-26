using Hidistro.Core;
using System;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Caching;
using System.Xml;
namespace Hidistro.Membership.Context
{
	public static class LicenseChecker
	{
		private const string CacheCommercialKey = "FileCache_CommercialLicenser";
		public static void Check(out bool isValid, out bool expired, out int siteQty)
		{
			siteQty = 0;
			isValid = false;
			expired = true;
			HiContext current = HiContext.Current;
			XmlDocument xmlDocument = HiCache.Get("FileCache_CommercialLicenser") as XmlDocument;
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
			if (xmlDocument == null)
			{
				string text = (current.Context != null) ? current.Context.Request.MapPath("~/config/Certificates.cer") : System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "config\\Certificates.cer");
				if (!System.IO.File.Exists(text))
				{
					return;
				}
				xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(System.IO.File.ReadAllText(text));
				HiCache.Max("FileCache_CommercialLicenser", xmlDocument, new CacheDependency(text));
			}
			XmlNode xmlNode = xmlDocument.DocumentElement.SelectSingleNode("//Host");
			XmlNode xmlNode2 = xmlDocument.DocumentElement.SelectSingleNode("//LicenseDate");
			XmlNode xmlNode3 = xmlDocument.DocumentElement.SelectSingleNode("//Expires");
			XmlNode xmlNode4 = xmlDocument.DocumentElement.SelectSingleNode("//SiteQty");
			XmlNode xmlNode5 = xmlDocument.DocumentElement.SelectSingleNode("//Signature");
            //if (string.Compare(xmlNode.InnerText, masterSettings.SiteUrl, true, System.Globalization.CultureInfo.InvariantCulture) == 0)
            //{
				
            //}

            string s = string.Format(System.Globalization.CultureInfo.InvariantCulture, "Host={0}&Expires={1}&SiteQty={2}&LicenseDate={3}", new object[]
				{
					masterSettings.SiteUrl,
					xmlNode3.InnerText,
					xmlNode4.InnerText,
					xmlNode2.InnerText
				});
            using (System.Security.Cryptography.RSACryptoServiceProvider rSACryptoServiceProvider = new System.Security.Cryptography.RSACryptoServiceProvider())
            {
                rSACryptoServiceProvider.FromXmlString(LicenseHelper.GetPublicKey());
                System.Security.Cryptography.RSAPKCS1SignatureDeformatter rSAPKCS1SignatureDeformatter = new System.Security.Cryptography.RSAPKCS1SignatureDeformatter(rSACryptoServiceProvider);
                rSAPKCS1SignatureDeformatter.SetHashAlgorithm("SHA1");
                byte[] rgbSignature = System.Convert.FromBase64String(xmlNode5.InnerText);
                byte[] rgbHash = new System.Security.Cryptography.SHA1Managed().ComputeHash(System.Text.Encoding.UTF8.GetBytes(s));
                isValid = rSAPKCS1SignatureDeformatter.VerifySignature(rgbHash, rgbSignature);
                isValid = true;
            }
            expired = (System.DateTime.Now > System.DateTime.Parse(xmlNode3.InnerText));
            if (isValid && !expired)
            {
                int.TryParse(xmlNode4.InnerText, out siteQty);
            }
		}
	}
}
