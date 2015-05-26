using Hidistro.Core;
using Hidistro.Core.Jobs;
using Hidistro.Membership.Context;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Xml;
namespace Hidistro.Jobs
{
	public class SiteMapJob : IJob
	{
		private SiteSettings siteSettings = SettingsManager.GetMasterSettings(true);
		private Database database = DatabaseFactory.CreateDatabase();
		private List<string> sitemaps = new List<string>();
		private string webroot;
		private string weburl;
		private string indexxml;
		private string prourl;
		public SiteMapJob()
		{
			string text = Globals.GetSiteUrls().Home;
			if (text == "/")
			{
				text = "";
			}
			else
			{
				text = "/" + text.Replace("/", "");
			}
			this.prourl = "http://" + this.siteSettings.SiteUrl;
			this.weburl = "http://" + this.siteSettings.SiteUrl + text;
			this.sitemaps.Add(this.weburl + "/sitemap1.xml");
			this.sitemaps.Add(this.weburl + "/sitemap2.xml");
			this.sitemaps.Add(this.weburl + "/sitemap3.xml");
			this.indexxml = this.weburl + "/sitemapindex.xml";
			this.webroot = Globals.MapPath("/" + Globals.ApplicationPath);
		}
		public void Execute(XmlNode node)
		{
			this.CreateCateXml();
			this.CreateProductXml();
			this.CreateArticleXml();
			this.CreateIndexXml();
		}
		public void CreateCateXml()
		{
			XmlDocument xmlDocument = new XmlDocument();
			XmlDeclaration newChild = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);
			xmlDocument.AppendChild(newChild);
			XmlElement xmlElement = xmlDocument.CreateElement("", "urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
			xmlDocument.AppendChild(xmlElement);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select CategoryId,RewriteName from Hishop_Categories");
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					XmlElement xmlElement2 = xmlDocument.CreateElement("url", xmlElement.NamespaceURI);
					XmlElement xmlElement3 = xmlDocument.CreateElement("loc", xmlElement2.NamespaceURI);
					XmlText newChild2 = xmlDocument.CreateTextNode(this.prourl + Globals.GetSiteUrls().SubCategory(Convert.ToInt32(dataReader["CategoryId"]), dataReader["RewriteName"]));
					xmlElement3.AppendChild(newChild2);
					XmlElement xmlElement4 = xmlDocument.CreateElement("lastmod", xmlElement2.NamespaceURI);
					XmlText newChild3 = xmlDocument.CreateTextNode(DateTime.Now.ToString("yyyy-MM-dd"));
					xmlElement4.AppendChild(newChild3);
					XmlElement xmlElement5 = xmlDocument.CreateElement("changefreq", xmlElement2.NamespaceURI);
					xmlElement5.InnerText = "daily";
					XmlElement xmlElement6 = xmlDocument.CreateElement("priority", xmlElement2.NamespaceURI);
					xmlElement6.InnerText = "1.0";
					xmlElement2.AppendChild(xmlElement3);
					xmlElement2.AppendChild(xmlElement4);
					xmlElement2.AppendChild(xmlElement5);
					xmlElement2.AppendChild(xmlElement6);
					xmlElement.AppendChild(xmlElement2);
				}
			}
			if (File.Exists(this.webroot + "/sitemap1.xml"))
			{
				File.Delete(this.webroot + "/sitemap1.xml");
			}
			xmlDocument.Save(this.webroot + "/sitemap1.xml");
		}
		public void CreateArticleXml()
		{
			XmlDocument xmlDocument = new XmlDocument();
			XmlDeclaration newChild = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);
			xmlDocument.AppendChild(newChild);
			XmlElement xmlElement = xmlDocument.CreateElement("", "urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
			xmlDocument.AppendChild(xmlElement);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select ArticleId from dbo.Hishop_Articles where IsRelease='1'");
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					XmlElement xmlElement2 = xmlDocument.CreateElement("url", xmlElement.NamespaceURI);
					XmlElement xmlElement3 = xmlDocument.CreateElement("loc", xmlElement2.NamespaceURI);
					XmlText newChild2 = xmlDocument.CreateTextNode(this.prourl + Globals.GetSiteUrls().UrlData.FormatUrl("ArticleDetails", new object[]
					{
						Convert.ToInt32(dataReader["ArticleId"])
					}));
					xmlElement3.AppendChild(newChild2);
					XmlElement xmlElement4 = xmlDocument.CreateElement("lastmod", xmlElement2.NamespaceURI);
					XmlText newChild3 = xmlDocument.CreateTextNode(DateTime.Now.ToString("yyyy-MM-dd"));
					xmlElement4.AppendChild(newChild3);
					XmlElement xmlElement5 = xmlDocument.CreateElement("changefreq", xmlElement2.NamespaceURI);
					xmlElement5.InnerText = "daily";
					XmlElement xmlElement6 = xmlDocument.CreateElement("priority", xmlElement2.NamespaceURI);
					xmlElement6.InnerText = "1.0";
					xmlElement2.AppendChild(xmlElement3);
					xmlElement2.AppendChild(xmlElement4);
					xmlElement2.AppendChild(xmlElement5);
					xmlElement2.AppendChild(xmlElement6);
					xmlElement.AppendChild(xmlElement2);
				}
			}
			if (File.Exists(this.webroot + "/sitemap3.xml"))
			{
				File.Delete(this.webroot + "/sitemap3.xml");
			}
			xmlDocument.Save(this.webroot + "/sitemap3.xml");
		}
		public void CreateProductXml()
		{
			XmlDocument xmlDocument = new XmlDocument();
			XmlDeclaration newChild = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);
			xmlDocument.AppendChild(newChild);
			XmlElement xmlElement = xmlDocument.CreateElement("", "urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
			xmlDocument.AppendChild(xmlElement);
			int num;
			System.Data.Common.DbCommand sqlStringCommand;
			if (int.TryParse(this.siteSettings.SiteMapNum, out num) && num > 0)
			{
				sqlStringCommand = this.database.GetSqlStringCommand("select top " + num + " productid from dbo.Hishop_Products where salestatus=1  order by productid desc");
			}
			else
			{
				sqlStringCommand = this.database.GetSqlStringCommand("select top  1000 productid from dbo.Hishop_Products where salestatus=1 order by productid desc");
			}
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					XmlElement xmlElement2 = xmlDocument.CreateElement("url", xmlElement.NamespaceURI);
					XmlElement xmlElement3 = xmlDocument.CreateElement("loc", xmlElement2.NamespaceURI);
					XmlText newChild2 = xmlDocument.CreateTextNode(this.prourl + Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[]
					{
						Convert.ToInt32(dataReader["productid"])
					}));
					xmlElement3.AppendChild(newChild2);
					XmlElement xmlElement4 = xmlDocument.CreateElement("lastmod", xmlElement2.NamespaceURI);
					XmlText newChild3 = xmlDocument.CreateTextNode(DateTime.Now.ToString("yyyy-MM-dd"));
					xmlElement4.AppendChild(newChild3);
					XmlElement xmlElement5 = xmlDocument.CreateElement("changefreq", xmlElement2.NamespaceURI);
					xmlElement5.InnerText = "daily";
					XmlElement xmlElement6 = xmlDocument.CreateElement("priority", xmlElement2.NamespaceURI);
					xmlElement6.InnerText = "1.0";
					xmlElement2.AppendChild(xmlElement3);
					xmlElement2.AppendChild(xmlElement4);
					xmlElement2.AppendChild(xmlElement5);
					xmlElement2.AppendChild(xmlElement6);
					xmlElement.AppendChild(xmlElement2);
				}
			}
			if (File.Exists(this.webroot + "/sitemap2.xml"))
			{
				File.Delete(this.webroot + "/sitemap2.xml");
			}
			xmlDocument.Save(this.webroot + "/sitemap2.xml");
		}
		public void CreateIndexXml()
		{
			XmlDocument xmlDocument = new XmlDocument();
			XmlDeclaration newChild = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);
			xmlDocument.AppendChild(newChild);
			XmlElement xmlElement = xmlDocument.CreateElement("", "sitemapindex", "http://www.sitemaps.org/schemas/sitemap/0.9");
			xmlDocument.AppendChild(xmlElement);
			foreach (string current in this.sitemaps)
			{
				XmlElement xmlElement2 = xmlDocument.CreateElement("sitemap", xmlElement.NamespaceURI);
				XmlElement xmlElement3 = xmlDocument.CreateElement("loc", xmlElement2.NamespaceURI);
				XmlText newChild2 = xmlDocument.CreateTextNode(current);
				xmlElement3.AppendChild(newChild2);
				XmlElement xmlElement4 = xmlDocument.CreateElement("lastmod", xmlElement2.NamespaceURI);
				XmlText newChild3 = xmlDocument.CreateTextNode(DateTime.Now.ToString("yyyy-MM-dd"));
				xmlElement4.AppendChild(newChild3);
				xmlElement2.AppendChild(xmlElement3);
				xmlElement2.AppendChild(xmlElement4);
				xmlElement.AppendChild(xmlElement2);
			}
			if (File.Exists(this.webroot + "/sitemapindex.xml"))
			{
				File.Delete(this.webroot + "/sitemapindex.xml");
			}
			xmlDocument.Save(this.webroot + "/sitemapindex.xml");
		}
	}
}
