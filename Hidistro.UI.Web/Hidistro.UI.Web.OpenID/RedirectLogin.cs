using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.SaleSystem.Member;
using Hishop.Plugins;
using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.OpenID
{
	public class RedirectLogin : System.Web.UI.Page
	{
		protected System.Web.UI.HtmlControls.HtmlForm form1;
		protected System.Web.UI.WebControls.Label lblMsg;
		protected void Page_Load(object sender, System.EventArgs e)
		{
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
			}
			string text = base.Request.QueryString["ot"];
			if (OpenIdPlugins.Instance().GetPluginItem(text) == null)
			{
				this.lblMsg.Text = "没有找到对应的插件，<a href=\"" + Globals.GetSiteUrls().Home + "\">返回首页</a>。";
				return;
			}
			OpenIdSettingsInfo openIdSettings = MemberProcessor.GetOpenIdSettings(text);
			if (openIdSettings == null)
			{
				this.lblMsg.Text = "请先配置此插件所需的信息，<a href=\"" + Globals.GetSiteUrls().Home + "\">返回首页</a>。";
				return;
			}
			string returnUrl = Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("OpenIdEntry_url", new object[]
			{
				text
			}));
			OpenIdService openIdService = OpenIdService.CreateInstance(text, HiCryptographer.Decrypt(openIdSettings.Settings), returnUrl);
			openIdService.Post();
		}
	}
}
