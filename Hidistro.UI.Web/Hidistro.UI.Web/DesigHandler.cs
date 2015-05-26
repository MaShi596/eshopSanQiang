using Hidistro.Core;
using Hidistro.Entities.Comments;
using Hidistro.Membership.Context;
using Hidistro.UI.ControlPanel.Utility;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web;
using System.Xml;
namespace Hidistro.UI.Web
{
	public class DesigHandler : AdminPage, System.Web.IHttpHandler
	{
		private string pagename = "";
		private string message = "";
		private string modeId = "";
		private string desigtype = "";
		private string elementId = "";
		private string configurl = "";
		private System.Xml.XmlNode currennode;
		public new bool IsReusable
		{
			get
			{
				return false;
			}
		}
		public new void ProcessRequest(System.Web.HttpContext context)
		{
			try
			{
				this.modeId = context.Request.Form["ModelId"];
				string format = "{{\"success\":{0},\"Result\":{1}}}";
				string a;
				if ((a = this.modeId) != null)
				{
					if (!(a == "Load"))
					{
						if (a == "editedialog")
						{
							this.desigtype = context.Request.Form["Type"];
							if (this.desigtype != "logo")
							{
								string text = context.Request.Form["Elementid"];
								if (this.desigtype != "" && text.Split(new char[]
								{
									'_'
								}).Length == 2)
								{
									this.elementId = text.Split(new char[]
									{
										'_'
									})[1];
									this.configurl = Globals.PhysicalPath(Hidistro.Membership.Context.HiContext.Current.GetSkinPath() + "/config/" + text.Split(new char[]
									{
										'_'
									})[0] + ".xml");
									this.currennode = this.FindNode(text.Split(new char[]
									{
										'_'
									})[0]);
									if (this.currennode != null)
									{
										string text2 = JavaScriptConvert.SerializeXmlNode(this.currennode);
										text2 = text2.Remove(0, text2.IndexOf(":") + 1).Remove(text2.Length - (text2.IndexOf(":") + 1) - 1).Replace("@", "");
										this.message = string.Format(format, "true", text2);
									}
								}
							}
							else
							{
								this.message = string.Format(format, "true", "{\"LogoUrl\":\"" + Hidistro.Membership.Context.HiContext.Current.SiteSettings.LogoUrl + "\",\"DialogName\":\"DialogTemplates/advert_logo.html\"}");
							}
						}
					}
					else
					{
						this.pagename = context.Request.Form["PageName"];
						DesigAttribute.PageName = this.pagename;
						string text3 = this.LoadFirstHtml();
						if (!string.IsNullOrEmpty(text3))
						{
							this.message = string.Format(format, "true", text3);
						}
					}
				}
			}
			catch (System.Exception ex)
			{
				this.message = "{\"success\":false,\"Result\":\"未知错误:" + ex.Message + "\"}";
			}
			context.Response.ContentType = "text/json";
			context.Response.Write(this.message);
		}
		public HtmlDocument GetHtmlDocument(string url)
		{
			HtmlDocument htmlDocument = null;
			if (url != "")
			{
				htmlDocument = new HtmlDocument();
				htmlDocument.Load(url);
			}
			return htmlDocument;
		}
		public HtmlDocument GetWebHtmlDocument(string weburl)
		{
			HtmlDocument result = null;
			if (weburl != "")
			{
				HtmlWeb htmlWeb = new HtmlWeb();
				result = htmlWeb.Load(weburl);
			}
			return result;
		}
		public string LoadFirstHtml()
		{
			string result = "";
			HtmlDocument htmlDocument = this.GetHtmlDocument(DesigAttribute.DesigPagePath);
			HtmlDocument webHtmlDocument = this.GetWebHtmlDocument(DesigAttribute.SourcePagePath);
			HtmlNodeCollection htmlNodeCollection = htmlDocument.DocumentNode.SelectNodes("//div[@rel=\"desig\"]");
			System.Collections.Generic.IList<DesignTempleteInfo> list = new System.Collections.Generic.List<DesignTempleteInfo>();
			foreach (HtmlNode current in htmlNodeCollection)
			{
				HtmlNode elementbyId = webHtmlDocument.GetElementbyId(current.Id);
				if (elementbyId != null)
				{
					list.Add(new DesignTempleteInfo
					{
						TempleteID = current.Id,
						TempleteContent = elementbyId.InnerHtml
					});
				}
			}
			if (list.Count > 0)
			{
				result = JavaScriptConvert.SerializeObject(list);
			}
			return result;
		}
		public System.Xml.XmlNode FindNode(string configname)
		{
			System.Xml.XmlNode xmlNode = null;
			if (this.elementId != "")
			{
				System.Xml.XmlDocument xmlNode2 = this.GetXmlNode();
				string text;
				if (configname == "products")
				{
					text = string.Format("//Subject[@SubjectId='{0}']", this.elementId);
				}
				else
				{
					if (configname == "ads")
					{
						text = string.Format("//Ad[@Id='{0}']", this.elementId);
					}
					else
					{
						if (configname == "comments")
						{
							text = string.Format("//Comment[@Id='{0}']", this.elementId);
						}
						else
						{
							text = string.Format("//Menu[@Id='{0}']", this.elementId);
						}
					}
				}
				if (text != "")
				{
					xmlNode = xmlNode2.SelectSingleNode(text);
					System.Xml.XmlAttribute xmlAttribute = xmlNode2.CreateAttribute("DialogName");
					xmlAttribute.InnerText = this.GetDialoName();
					xmlNode.Attributes.Append(xmlAttribute);
				}
			}
			return xmlNode;
		}
		public string GetDialoName()
		{
			string str = "error.html";
			if (this.desigtype != "")
			{
				string key;
				switch (key = this.desigtype)
				{
				case "floor":
					str = "product_floor_edite.html";
					goto IL_1D2;
				case "tab":
					str = "product_tab_edite.html";
					goto IL_1D2;
				case "top":
					str = "product_top_edite.html";
					goto IL_1D2;
				case "group":
					str = "product_group_edite.html";
					goto IL_1D2;
				case "simple":
					str = "simple_edite.html";
					goto IL_1D2;
				case "slide":
					str = "advert_slide_edite.html";
					goto IL_1D2;
				case "image":
					str = "advert_image_edite.html";
					goto IL_1D2;
				case "custom":
					str = "advert_custom_edite.html";
					goto IL_1D2;
				case "article":
					str = "comment_article_edite.html";
					goto IL_1D2;
				case "category":
					str = "comment_category_edite.html";
					goto IL_1D2;
				case "brand":
					str = "comment_brand_edite.html";
					goto IL_1D2;
				case "keyword":
					str = "comment_keyword_edite.html";
					goto IL_1D2;
				case "attribute":
					str = "comment_attribute_edite.html";
					goto IL_1D2;
				case "title":
					str = "comment_title_edite.html";
					goto IL_1D2;
				case "morelink":
					str = "comment_morelink_edite.html";
					goto IL_1D2;
				}
				str = "error.html";
			}
			IL_1D2:
			return "DialogTemplates/" + str;
		}
		public System.Xml.XmlDocument GetXmlNode()
		{
			System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
			if (!string.IsNullOrEmpty(this.configurl))
			{
				xmlDocument.Load(this.configurl);
			}
			return xmlDocument;
		}
	}
}
