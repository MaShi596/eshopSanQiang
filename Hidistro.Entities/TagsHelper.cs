using Hidistro.Core;
using Hidistro.Membership.Context;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;
using System.Xml;
namespace Hidistro.Entities
{
	public static class TagsHelper
	{
		public static XmlNode FindProductNode(int subjectId, string type)
		{
			XmlDocument productDocument = TagsHelper.GetProductDocument();
			return productDocument.SelectSingleNode(string.Format("//Subject[@SubjectId='{0}' and @Type='{1}']", subjectId, type));
		}
		public static XmlNode FindAdNode(int int_0, string type)
		{
			XmlDocument adDocument = TagsHelper.GetAdDocument();
			return adDocument.SelectSingleNode(string.Format("//Ad[@Id='{0}' and @Type='{1}']", int_0, type));
		}
		public static XmlNode FindCommentNode(int int_0, string type)
		{
			XmlDocument commentDocument = TagsHelper.GetCommentDocument();
			return commentDocument.SelectSingleNode(string.Format("//Comment[@Id='{0}' and @Type='{1}']", int_0, type));
		}
		public static XmlNode FindHeadMenuNode(int int_0)
		{
			XmlDocument headMenuDocument = TagsHelper.GetHeadMenuDocument();
			return headMenuDocument.SelectSingleNode(string.Format("//Menu[@Id='{0}']", int_0));
		}
		public static bool UpdateProductNode(int subjectId, string type, System.Collections.Generic.Dictionary<string, string> simplenode)
		{
			string filename = HttpContext.Current.Request.MapPath(HiContext.Current.GetSkinPath() + "/config/Products.xml");
			bool result = false;
			XmlDocument productDocument = TagsHelper.GetProductDocument();
			XmlNode xmlNode = TagsHelper.FindProductNode(subjectId, type);
			if (xmlNode != null)
			{
				foreach (System.Collections.Generic.KeyValuePair<string, string> current in simplenode)
				{
					xmlNode.Attributes[current.Key].Value = current.Value;
				}
				productDocument.Save(filename);
				TagsHelper.RemoveProductNodeCache();
				result = true;
			}
			return result;
		}
		public static bool UpdateAdNode(int aId, string type, System.Collections.Generic.Dictionary<string, string> adnode)
		{
			bool result = false;
			XmlDocument adDocument = TagsHelper.GetAdDocument();
			XmlNode xmlNode = TagsHelper.FindAdNode(aId, type);
			if (xmlNode != null)
			{
				foreach (System.Collections.Generic.KeyValuePair<string, string> current in adnode)
				{
					xmlNode.Attributes[current.Key].Value = current.Value;
				}
				string filename = HttpContext.Current.Request.MapPath(HiContext.Current.GetSkinPath() + "/config/Ads.xml");
				adDocument.Save(filename);
				TagsHelper.RemoveAdNodeCache();
				result = true;
			}
			return result;
		}
		public static bool UpdateCommentNode(int commentId, string type, System.Collections.Generic.Dictionary<string, string> commentnode)
		{
			bool result = false;
			string filename = HttpContext.Current.Request.MapPath(HiContext.Current.GetSkinPath() + "/config/Comments.xml");
			XmlDocument commentDocument = TagsHelper.GetCommentDocument();
			XmlNode xmlNode = TagsHelper.FindCommentNode(commentId, type);
			if (xmlNode != null)
			{
				foreach (System.Collections.Generic.KeyValuePair<string, string> current in commentnode)
				{
					xmlNode.Attributes[current.Key].Value = current.Value;
				}
				commentDocument.Save(filename);
				TagsHelper.RemoveCommentNodeCache();
				result = true;
			}
			return result;
		}
		public static bool UpdateHeadMenuNode(int menuId, System.Collections.Generic.Dictionary<string, string> headmenunode)
		{
			string filename = HttpContext.Current.Request.MapPath(HiContext.Current.GetSkinPath() + "/config/HeaderMenu.xml");
			bool result = false;
			XmlDocument commentDocument = TagsHelper.GetCommentDocument();
			XmlNode xmlNode = TagsHelper.FindHeadMenuNode(menuId);
			if (xmlNode != null)
			{
				foreach (System.Collections.Generic.KeyValuePair<string, string> current in headmenunode)
				{
					xmlNode.Attributes[current.Key].Value = current.Value;
				}
				commentDocument.Save(filename);
				TagsHelper.RemoveHeadMenuCache();
				result = true;
			}
			return result;
		}
		private static void RemoveProductNodeCache()
		{
			string string_ = "ProductFileCache-Admin";
			if (HiContext.Current.SiteSettings.UserId.HasValue)
			{
				string_ = string.Format("ProductFileCache-{0}", HiContext.Current.SiteSettings.UserId.Value);
			}
			HiCache.Remove(string_);
		}
		private static void RemoveHeadMenuCache()
		{
			string string_ = "HeadMenuCache-Admin";
			if (HiContext.Current.SiteSettings.UserId.HasValue)
			{
				string_ = string.Format("HeadMenuCache-{0}", HiContext.Current.SiteSettings.UserId.Value);
			}
			HiCache.Remove(string_);
		}
		private static void RemoveAdNodeCache()
		{
			string string_ = "AdFileCache-Admin";
			if (HiContext.Current.SiteSettings.UserId.HasValue)
			{
				string_ = string.Format("AdFileCache-{0}", HiContext.Current.SiteSettings.UserId.Value);
			}
			HiCache.Remove(string_);
		}
		private static void RemoveCommentNodeCache()
		{
			string string_ = "CommentFileCache-Admin";
			if (HiContext.Current.SiteSettings.UserId.HasValue)
			{
				string_ = string.Format("CommentFileCache-{0}", HiContext.Current.SiteSettings.UserId.Value);
			}
			HiCache.Remove(string_);
		}
		private static XmlDocument GetProductDocument()
		{
			string filename = HttpContext.Current.Request.MapPath(HiContext.Current.GetSkinPath() + "/config/Products.xml");
			string string_ = "ProductFileCache-Admin";
			if (HiContext.Current.SiteSettings.UserId.HasValue)
			{
				string_ = string.Format("ProductFileCache-{0}", HiContext.Current.SiteSettings.UserId.Value);
			}
			XmlDocument xmlDocument = HiCache.Get(string_) as XmlDocument;
			if (xmlDocument == null)
			{
				HttpContext arg_8C_0 = HiContext.Current.Context;
				xmlDocument = new XmlDocument();
				xmlDocument.Load(filename);
				HiCache.Max(string_, xmlDocument, new CacheDependency(filename));
			}
			return xmlDocument;
		}
		private static XmlDocument GetAdDocument()
		{
			string filename = HttpContext.Current.Request.MapPath(HiContext.Current.GetSkinPath() + "/config/Ads.xml");
			string string_ = "AdFileCache-Admin";
			if (HiContext.Current.SiteSettings.UserId.HasValue)
			{
				string_ = string.Format("AdFileCache-{0}", HiContext.Current.SiteSettings.UserId.Value);
			}
			XmlDocument xmlDocument = HiCache.Get(string_) as XmlDocument;
			if (xmlDocument == null)
			{
				HttpContext arg_8C_0 = HiContext.Current.Context;
				xmlDocument = new XmlDocument();
				xmlDocument.Load(filename);
				HiCache.Max(string_, xmlDocument, new CacheDependency(filename));
			}
			return xmlDocument;
		}
		private static XmlDocument GetCommentDocument()
		{
			string filename = HttpContext.Current.Request.MapPath(HiContext.Current.GetSkinPath() + "/config/Comments.xml");
			string string_ = "CommentFileCache-Admin";
			if (HiContext.Current.SiteSettings.UserId.HasValue)
			{
				string_ = string.Format("CommentFileCache-{0}", HiContext.Current.SiteSettings.UserId.Value);
			}
			XmlDocument xmlDocument = HiCache.Get(string_) as XmlDocument;
			if (xmlDocument == null)
			{
				HttpContext arg_8C_0 = HiContext.Current.Context;
				xmlDocument = new XmlDocument();
				xmlDocument.Load(filename);
				HiCache.Max(string_, xmlDocument, new CacheDependency(filename));
			}
			return xmlDocument;
		}
		private static XmlDocument GetHeadMenuDocument()
		{
			string filename = HttpContext.Current.Request.MapPath(HiContext.Current.GetSkinPath() + "/config/HeaderMenu.xml");
			string string_ = "HeadMenuCache-Admin";
			if (HiContext.Current.SiteSettings.UserId.HasValue)
			{
				string_ = string.Format("HeadMenuCache-{0}", HiContext.Current.SiteSettings.UserId.Value);
			}
			XmlDocument xmlDocument = HiCache.Get(string_) as XmlDocument;
			if (xmlDocument == null)
			{
				HttpContext arg_8C_0 = HiContext.Current.Context;
				xmlDocument = new XmlDocument();
				xmlDocument.Load(filename);
				HiCache.Max(string_, xmlDocument, new CacheDependency(filename));
			}
			return xmlDocument;
		}
	}
}
