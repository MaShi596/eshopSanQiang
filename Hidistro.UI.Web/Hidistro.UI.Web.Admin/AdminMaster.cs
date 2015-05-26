using Hidistro.Core;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	public class AdminMaster : System.Web.UI.MasterPage
	{
		protected PageTitle PageTitle1;
		protected Script Script2;
		protected Script Script5;
		protected Script Script7;
		protected Script Script6;
		protected Script Script1;
		protected Script Script3;
		protected Script Script4;
		protected System.Web.UI.WebControls.ContentPlaceHolder headHolder;
		protected System.Web.UI.WebControls.ContentPlaceHolder validateHolder;
		protected System.Web.UI.HtmlControls.HtmlForm thisForm;
		protected System.Web.UI.WebControls.Image imgLogo;
		protected System.Web.UI.WebControls.HyperLink hlinkDefault;
		protected System.Web.UI.WebControls.HyperLink hlinkAdminDefault;
		protected System.Web.UI.WebControls.Label lblUserName;
		protected System.Web.UI.WebControls.HyperLink hlinkLogout;
		protected System.Web.UI.WebControls.HyperLink hlinkService;
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
		}
	}
}
