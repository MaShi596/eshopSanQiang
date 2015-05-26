using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
namespace Hidistro.UI.Web.Admin
{
	public class LoginExit : System.Web.UI.Page
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			System.Web.Security.FormsAuthentication.SignOut();
			System.Web.HttpCookie authCookie = System.Web.Security.FormsAuthentication.GetAuthCookie(Hidistro.Membership.Context.HiContext.Current.User.Username, true);
			Hidistro.Membership.Core.IUserCookie userCookie = Hidistro.Membership.Context.HiContext.Current.User.GetUserCookie();
			if (userCookie != null)
			{
				userCookie.DeleteCookie(authCookie);
			}
			Hidistro.Membership.Core.RoleHelper.SignOut(Hidistro.Membership.Context.HiContext.Current.User.Username);
			base.Response.Redirect("Login.aspx", true);
		}
	}
}
