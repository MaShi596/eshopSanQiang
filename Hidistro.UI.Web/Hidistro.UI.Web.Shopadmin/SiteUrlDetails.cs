using Hidistro.Membership.Context;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class SiteUrlDetails : DistributorPage
	{
		protected System.Web.UI.WebControls.Literal litUserName;
		protected System.Web.UI.WebControls.Literal litFirstSiteUrl;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!base.IsPostBack)
			{
				this.litUserName.Text = Hidistro.Membership.Context.HiContext.Current.User.Username;
				Hidistro.Membership.Context.SiteSettings siteSettings = Hidistro.Membership.Context.SettingsManager.GetSiteSettings(Hidistro.Membership.Context.HiContext.Current.User.UserId);
				if (siteSettings == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				this.litFirstSiteUrl.Text = siteSettings.SiteUrl;
			}
		}
	}
}
