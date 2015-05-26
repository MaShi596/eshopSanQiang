using Hidistro.Core;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class Logout : System.Web.UI.Page
	{
		protected override void OnLoad(System.EventArgs eventArgs_0)
		{
			base.OnLoad(eventArgs_0);
			System.Web.HttpCookie httpCookie = Hidistro.Membership.Context.HiContext.Current.Context.Request.Cookies["Token_" + Hidistro.Membership.Context.HiContext.Current.User.UserId.ToString()];
			if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
			{
				httpCookie.Expires = System.DateTime.Now;
				System.Web.HttpContext.Current.Response.Cookies.Add(httpCookie);
			}
			if (this.Context.Request.IsAuthenticated)
			{
				System.Web.Security.FormsAuthentication.SignOut();
				System.Web.HttpCookie authCookie = System.Web.Security.FormsAuthentication.GetAuthCookie(Hidistro.Membership.Context.HiContext.Current.User.Username, true);
				Hidistro.Membership.Core.IUserCookie userCookie = Hidistro.Membership.Context.HiContext.Current.User.GetUserCookie();
				if (userCookie != null)
				{
					userCookie.DeleteCookie(authCookie);
				}
				Hidistro.Membership.Core.RoleHelper.SignOut(Hidistro.Membership.Context.HiContext.Current.User.Username);
				this.Context.Response.Cookies["hishopLoginStatus"].Value = "";
			}
			this.Context.Response.Redirect(Globals.GetSiteUrls().Home, true);
		}
	}
}
