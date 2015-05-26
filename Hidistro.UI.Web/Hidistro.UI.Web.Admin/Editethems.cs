using Hidistro.Membership.Context;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	public class Editethems : AdminPage
	{
		protected System.Web.UI.WebControls.Literal litThemeName;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.litThemeName.Text = Hidistro.Membership.Context.HiContext.Current.SiteSettings.Theme;
		}
	}
}
