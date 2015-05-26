using Hidistro.Core;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core.Enums;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_LoginLink : HyperLink
	{
		protected override void Render(HtmlTextWriter writer)
		{
			if (HiContext.Current.User.UserRole != UserRole.Member)
			{
				if (HiContext.Current.User.UserRole != UserRole.Underling)
				{
					base.Text = "登录";
					base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("login_clean");
					goto IL_72;
				}
			}
			base.Text = "退出";
			base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("logout");
			IL_72:
			base.Render(writer);
		}
	}
}
