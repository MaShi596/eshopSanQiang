using Hidistro.Membership.Context;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class MyDeductSettings : DistributorPage
	{
		protected System.Web.UI.WebControls.TextBox txtDeduct;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtDeductTip;
		protected System.Web.UI.WebControls.Button btnOK;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			if (!this.Page.IsPostBack)
			{
				Hidistro.Membership.Context.SiteSettings siteSettings = Hidistro.Membership.Context.SettingsManager.GetSiteSettings(Hidistro.Membership.Context.HiContext.Current.User.UserId);
				this.txtDeduct.Text = siteSettings.ReferralDeduct.ToString();
			}
		}
		private void btnOK_Click(object sender, System.EventArgs e)
		{
			int referralDeduct = 0;
			if (!int.TryParse(this.txtDeduct.Text.Trim(), out referralDeduct))
			{
				this.ShowMsg("您输入的推荐人提成比例格式不对！", false);
				return;
			}
			Hidistro.Membership.Context.SiteSettings siteSettings = Hidistro.Membership.Context.SettingsManager.GetSiteSettings(Hidistro.Membership.Context.HiContext.Current.User.UserId);
			siteSettings.ReferralDeduct = referralDeduct;
			Hidistro.Membership.Context.SettingsManager.Save(siteSettings);
			this.ShowMsg("成功修改了推荐人提成比例", true);
		}
	}
}
