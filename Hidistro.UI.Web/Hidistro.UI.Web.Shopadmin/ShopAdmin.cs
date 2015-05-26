using Hidistro.Core;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Comments;
using Hidistro.UI.Common.Controls;
using System;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
namespace Hidistro.UI.Web.Shopadmin
{
	public class ShopAdmin : System.Web.UI.MasterPage
	{
		private const string moduleMenuFormat = "<li><a href=\"{0}\"><span><img src=\"{1}\" />{2}</span></a></li>";
		private const string selectedModuleMenuFormat = "<li class=\"menucurrent\"><a href=\"{0}\"><span><img src=\"{1}\" />{2}</span></a></li>";
		private const string subMenuFormat = "<li><a href=\"{0}\"><span>{1}</span></a></li>";
		private const string selectedSubMenuFormat = "<li class=\"visited\"><a href=\"{0}\"><span>{1}</span></a></li>";
		protected System.Web.UI.HtmlControls.HtmlHead Head1;
		protected PageTitle PageTitle1;
		protected HeadContainer HeadContainer1;
		protected Script Script4;
		protected Script Script1;
		protected Script Script5;
		protected Script Script7;
		protected Script Script6;
		protected Script Script2;
		protected Script Script3;
		protected System.Web.UI.WebControls.ContentPlaceHolder headHolder;
		protected System.Web.UI.WebControls.ContentPlaceHolder validateHolder;
		protected System.Web.UI.HtmlControls.HtmlForm thisForm;
		protected HiImage imgLogo;
		protected System.Web.UI.WebControls.HyperLink hlinkHome;
		protected System.Web.UI.WebControls.HyperLink hpkDefault;
		protected System.Web.UI.HtmlControls.HtmlAnchor hp_message;
		protected System.Web.UI.WebControls.HyperLink hlinkToTaobao;
		protected System.Web.UI.WebControls.Label lblUserName;
		protected System.Web.UI.WebControls.HyperLink hlinkLogout;
		protected System.Web.UI.WebControls.Literal mainMenuHolder;
		protected System.Web.UI.WebControls.Literal subMenuHolder;
		protected System.Web.UI.WebControls.ContentPlaceHolder contentHolder;
		protected override void OnInit(System.EventArgs e)
		{
			base.OnInit(e);
			PageTitle.AddTitle(Hidistro.Membership.Context.HiContext.Current.SiteSettings.SiteName, this.Context);
			foreach (System.Web.UI.Control control in this.Page.Header.Controls)
			{
				if (control is System.Web.UI.HtmlControls.HtmlLink)
				{
					System.Web.UI.HtmlControls.HtmlLink htmlLink = control as System.Web.UI.HtmlControls.HtmlLink;
					if (htmlLink.Href.StartsWith("/"))
					{
						htmlLink.Href = Globals.ApplicationPath + htmlLink.Href;
					}
					else
					{
						htmlLink.Href = Globals.ApplicationPath + "/" + htmlLink.Href;
					}
				}
			}
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.LoadMenu();
				Hidistro.Membership.Context.SiteSettings siteSettings = Hidistro.Membership.Context.SettingsManager.GetSiteSettings(Hidistro.Membership.Context.HiContext.Current.User.UserId);
				if (siteSettings == null)
				{
					this.hlinkHome.NavigateUrl = Globals.GetSiteUrls().Home;
					this.hpkDefault.NavigateUrl = "nositedefault.aspx";
					siteSettings = Hidistro.Membership.Context.SettingsManager.GetMasterSettings(true);
					if (!string.IsNullOrEmpty(siteSettings.LogoUrl))
					{
						this.imgLogo.ImageUrl = siteSettings.LogoUrl;
					}
				}
				else
				{
					this.hlinkHome.NavigateUrl = "http://" + siteSettings.SiteUrl + Globals.GetSiteUrls().Home;
					this.hpkDefault.NavigateUrl = "Default.aspx";
					if (!string.IsNullOrEmpty(siteSettings.LogoUrl))
					{
						this.imgLogo.ImageUrl = siteSettings.LogoUrl;
					}
				}
				this.hlinkLogout.NavigateUrl = Globals.ApplicationPath + "/Logout.aspx";
				int isReadMessageToAdmin = SubsiteCommentsHelper.GetIsReadMessageToAdmin();
				if (isReadMessageToAdmin > 0)
				{
					this.hp_message.Style.Add("color", "red");
					this.hp_message.InnerText = "站内信(" + isReadMessageToAdmin.ToString() + ")";
				}
				Hidistro.Membership.Context.Distributor distributor = Hidistro.Membership.Context.HiContext.Current.User as Hidistro.Membership.Context.Distributor;
				if (distributor != null)
				{
					this.lblUserName.Text = "欢迎您，" + distributor.Username;
					Hidistro.Membership.Context.SiteSettings masterSettings = Hidistro.Membership.Context.SettingsManager.GetMasterSettings(false);
					this.hlinkToTaobao.NavigateUrl = string.Format("http://order1.kuaidiangtong.com/TaoBaoApi.aspx?Host={0}&ApplicationPath={1}&DistributorUserId={2}&Email={3}&RealName={4}&CompanyName={5}&Address={6}&TelPhone={7}&QQ={8}&Wangwang={9}", new object[]
					{
						masterSettings.SiteUrl,
						Globals.ApplicationPath,
						distributor.UserId,
						distributor.Email,
						distributor.RealName,
						distributor.CompanyName,
						distributor.Address,
						distributor.TelPhone,
						distributor.QQ,
						distributor.Wangwang
					});
				}
			}
		}
		private void LoadMenu()
		{
			string text = (Globals.ApplicationPath + "/Shopadmin/").ToLower();
			string text2 = this.Page.Request.Url.AbsolutePath.ToLower();
			string xpath = string.Format("Menu/Module/Item/PageLink[@Link='{0}']", text2.Replace(text, "").ToLower());
			System.Xml.XmlDocument menuDocument = ShopAdmin.GetMenuDocument();
			System.Xml.XmlNode xmlNode = menuDocument.SelectSingleNode(xpath);
			System.Xml.XmlNode xmlNode2 = null;
			System.Xml.XmlNode xmlNode3 = null;
			if (xmlNode != null)
			{
				xmlNode2 = xmlNode.ParentNode;
				xmlNode3 = xmlNode2.ParentNode;
			}
			else
			{
				xpath = string.Format("Menu/Module/Item[@Link='{0}']", text2.Replace(text, "").ToLower());
				xmlNode2 = menuDocument.SelectSingleNode(xpath);
				if (xmlNode2 != null)
				{
					xmlNode3 = xmlNode2.ParentNode;
				}
			}
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			System.Xml.XmlNodeList xmlNodeList = menuDocument.SelectNodes("Menu/Module");
			foreach (System.Xml.XmlNode xmlNode4 in xmlNodeList)
			{
				if (xmlNode4.NodeType == System.Xml.XmlNodeType.Element)
				{
					stringBuilder.Append(ShopAdmin.BuildModuleMenu(xmlNode4.Attributes["Title"].Value, xmlNode4.Attributes["Image"].Value, xmlNode4.Attributes["Link"].Value.ToLower(), xmlNode3, text));
					stringBuilder.Append(System.Environment.NewLine);
				}
			}
			this.mainMenuHolder.Text = stringBuilder.ToString();
			stringBuilder.Remove(0, stringBuilder.Length);
			if (xmlNode3 != null)
			{
				foreach (System.Xml.XmlNode xmlNode5 in xmlNode3.ChildNodes)
				{
					if (xmlNode5.NodeType == System.Xml.XmlNodeType.Element)
					{
						stringBuilder.Append(ShopAdmin.BuildSubMenu(xmlNode5.Attributes["Title"].Value, xmlNode5.Attributes["Link"].Value.ToLower(), xmlNode2, text));
						stringBuilder.Append(System.Environment.NewLine);
					}
				}
			}
			this.subMenuHolder.Text = stringBuilder.ToString();
		}
		private static string BuildModuleMenu(string title, string imgUrl, string link, System.Xml.XmlNode currentModuleNode, string shopAdminPath)
		{
			if (currentModuleNode != null && string.Compare(title, currentModuleNode.Attributes["Title"].Value, true) == 0 && string.Compare(link, currentModuleNode.Attributes["Link"].Value, true) == 0)
			{
				return string.Format("<li class=\"menucurrent\"><a href=\"{0}\"><span><img src=\"{1}\" />{2}</span></a></li>", shopAdminPath + link, Globals.ApplicationPath + imgUrl, title);
			}
			return string.Format("<li><a href=\"{0}\"><span><img src=\"{1}\" />{2}</span></a></li>", shopAdminPath + link, Globals.ApplicationPath + imgUrl, title);
		}
		private static string BuildSubMenu(string title, string link, System.Xml.XmlNode currentItemNode, string shopAdminPath)
		{
			if (currentItemNode != null && string.Compare(title, currentItemNode.Attributes["Title"].Value, true) == 0 && string.Compare(link, currentItemNode.Attributes["Link"].Value, true) == 0)
			{
				return string.Format("<li class=\"visited\"><a href=\"{0}\"><span>{1}</span></a></li>", shopAdminPath + link, title);
			}
			return string.Format("<li><a href=\"{0}\"><span>{1}</span></a></li>", shopAdminPath + link, title);
		}
		private static System.Xml.XmlDocument GetMenuDocument()
		{
			System.Web.HttpContext context = Hidistro.Membership.Context.HiContext.Current.Context;
			Hidistro.Membership.Context.SiteSettings siteSettings = Hidistro.Membership.Context.SettingsManager.GetSiteSettings(Hidistro.Membership.Context.HiContext.Current.User.UserId);
			System.Xml.XmlDocument xmlDocument;
			if (siteSettings == null)
			{
				xmlDocument = (HiCache.Get("FileCache-ShopadminMenu-NoSite") as System.Xml.XmlDocument);
			}
			else
			{
				xmlDocument = (HiCache.Get("FileCache-ShopadminMenu") as System.Xml.XmlDocument);
			}
			if (xmlDocument == null)
			{
				if (siteSettings == null)
				{
					string filename = context.Request.MapPath("~/Shopadmin/Menu_NoSite.config");
					xmlDocument = new System.Xml.XmlDocument();
					xmlDocument.Load(filename);
					HiCache.Max("FileCache-ShopadminMenu-NoSite", xmlDocument, new System.Web.Caching.CacheDependency(filename));
				}
				else
				{
					string filename = context.Request.MapPath("~/Shopadmin/Menu.config");
					xmlDocument = new System.Xml.XmlDocument();
					xmlDocument.Load(filename);
					HiCache.Max("FileCache-ShopadminMenu", xmlDocument, new System.Web.Caching.CacheDependency(filename));
				}
			}
			return xmlDocument;
		}
	}
}
