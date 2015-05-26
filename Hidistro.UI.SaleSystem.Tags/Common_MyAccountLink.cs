using Hidistro.Core;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core.Enums;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_MyAccountLink : HyperLink
	{
		protected override void Render(HtmlTextWriter writer)
		{
			if (HiContext.Current.User.UserRole != UserRole.Member)
			{
				if (HiContext.Current.User.UserRole != UserRole.Underling)
				{
					base.Text = "注册";
					base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("register");
					goto IL_72;
				}
			}
			base.Text = "我的账户";
			base.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("user_UserDefault");
			IL_72:
			base.Render(writer);
		}
	}
}
