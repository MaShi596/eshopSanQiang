using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using kindeditor.Net;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Xml;
namespace Hidistro.UI.Web.DialogTemplates
{
	public class CustomContent : System.Web.UI.Page
	{
		protected System.Web.UI.HtmlControls.HtmlForm form1;
		protected KindeditorControl customDescription;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!base.IsPostBack && !string.IsNullOrEmpty(base.Request.QueryString["id"].ToString()) && !string.IsNullOrEmpty(base.Request.QueryString["type"].ToString()) && !string.IsNullOrEmpty(base.Request.QueryString["name"].ToString()))
			{
				string name = base.Request.QueryString["name"].ToString();
				System.Xml.XmlNode xmlNode = this.BindHtml(base.Request.QueryString["id"].ToString(), base.Request.QueryString["type"].ToString());
				if (xmlNode != null && xmlNode.Attributes[name] != null)
				{
                    this.customDescription.Text = Globals.HtmlDecode(Globals.HtmlDecode(xmlNode.Attributes[name].Value));
				}
			}
		}
		protected override void OnInit(System.EventArgs e)
		{
			Hidistro.Membership.Core.IUser contexUser = Hidistro.Membership.Context.Users.GetContexUser();
			if (!Hidistro.Membership.Context.HiContext.Current.SiteSettings.IsDistributorSettings && contexUser.UserRole != Hidistro.Membership.Core.Enums.UserRole.SiteManager)
			{
				this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("login.aspx"), true);
			}
			if (Hidistro.Membership.Context.HiContext.Current.SiteSettings.IsDistributorSettings && contexUser.UserRole != Hidistro.Membership.Core.Enums.UserRole.Distributor)
			{
				this.Page.Response.Redirect(Globals.ApplicationPath + "Shopadmin/DistributorLogin.aspx", true);
			}
		}
		private System.Xml.XmlNode BindHtml(string controId, string type)
		{
			System.Xml.XmlNode result = null;
			if (controId.Contains("_"))
			{
				string xmlname = controId.Split(new char[]
				{
					'_'
				})[0];
				int id = System.Convert.ToInt32(controId.Split(new char[]
				{
					'_'
				})[1].ToString());
				result = this.FindXmlNode(xmlname, id, type);
			}
			return result;
		}
		private System.Xml.XmlNode FindXmlNode(string xmlname, int Id, string type)
		{
			System.Xml.XmlNode result = null;
			if (xmlname != null)
			{
				if (!(xmlname == "ads"))
				{
					if (xmlname == "products")
					{
						result = TagsHelper.FindProductNode(Id, type);
					}
				}
				else
				{
					result = TagsHelper.FindAdNode(Id, type);
				}
			}
			return result;
		}
	}
}
