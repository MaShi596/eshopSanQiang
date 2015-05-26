using Hidistro.Core;
using System;
using System.Web.UI;
namespace Hidistro.UI.Web
{
	public class loginEntry : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			string text = this.Page.Request.QueryString["returnUrl"].ToLower();
			if (!string.IsNullOrEmpty(text) && text.ToLower().IndexOf("adminsupplier") != -1)
			{
				base.Response.Redirect(Globals.ApplicationPath + "/AdminSupplier/Login.aspx", true);
				return;
			}
			if (!string.IsNullOrEmpty(text) && text.ToLower().IndexOf("adminshippoint") != -1)
			{
				base.Response.Redirect(Globals.ApplicationPath + "/adminshippoint/Login.aspx", true);
				return;
			}
			if (!string.IsNullOrEmpty(text) && text.StartsWith("/admin"))
			{
				base.Response.Redirect(Globals.GetAdminAbsolutePath("/login.aspx?returnUrl=" + text), true);
				return;
			}
			if (!string.IsNullOrEmpty(text) && text.StartsWith(Globals.GetSiteUrls().Locations["distributor"].ToLower()))
			{
				base.Response.Redirect(Globals.ApplicationPath + "/shopadmin/DistributorLogin.aspx?returnUrl=" + text, true);
				return;
			}
			base.Response.Redirect(Globals.GetSiteUrls().Login, true);
		}
	}
}
