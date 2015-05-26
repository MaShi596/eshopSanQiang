using Hidistro.ControlPanel.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
namespace Hidistro.UI.Web.Admin
{
	[AdministerCheck(true)]
	public class CnzzStatisticTotal : AdminPage
	{
		protected System.Web.UI.HtmlControls.HtmlGenericControl framcnz;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			Hidistro.Membership.Context.SiteSettings siteSettings = Hidistro.Membership.Context.HiContext.Current.SiteSettings;
			if (!string.IsNullOrEmpty(siteSettings.CnzzPassword) && !string.IsNullOrEmpty(siteSettings.CnzzUsername))
			{
				this.framcnz.Attributes["src"] = "http://wss.cnzz.com/user/companion/92hi_login.php?site_id=" + siteSettings.CnzzUsername + "&password=" + siteSettings.CnzzPassword;
				return;
			}
			this.Page.Response.Redirect("cnzzstatisticsset.aspx");
		}
	}
}
