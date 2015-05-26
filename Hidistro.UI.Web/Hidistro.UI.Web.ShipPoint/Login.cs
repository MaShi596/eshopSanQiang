using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using Hidistro.UI.Common.Controls;
using System;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.ShipPoint
{
	public class Login : System.Web.UI.Page
	{
		private const string NoticeMsg = "<div class=\"checkInfo\">\r\n   <table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">\r\n      <tr>\r\n        <td class=\"td1\"><img src=\"images/checkInfo.gif\" width=\"30\" height=\"30\" /></td>\r\n        <td class=\"td2\" width=\"100%\">您正在使用的易分销系统已过授权有效期！<br/>请联系官方(www.shopefx.com)购买软件使用权。</td>\r\n      </tr>\r\n   </table>\r\n</div>";
		protected HeadContainer HeadContainer1;
		protected PageTitle PageTitle1;
		protected System.Web.UI.HtmlControls.HtmlForm form1;
		protected System.Web.UI.WebControls.Panel Panel1;
		protected System.Web.UI.WebControls.TextBox txtAdminName;
		protected System.Web.UI.WebControls.TextBox txtAdminPassWord;
		protected System.Web.UI.WebControls.TextBox txtCode;
		protected System.Web.UI.WebControls.Button btnAdminLogin;
		protected SmallStatusMessage lblStatus;
		private string verifyCodeKey = "VerifyCode";
		private readonly string licenseMsg = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <Hi:HeadContainer ID=\"HeadContainer1\" runat=\"server\" />\r\n    <Hi:PageTitle ID=\"PageTitle1\" runat=\"server\" />\r\n    <link rel=\"stylesheet\" href=\"css/login.css\" type=\"text/css\" media=\"screen\" />\r\n</head>\r\n<body>\r\n<div class=\"admin\">\r\n<div id=\"\" class=\"wrap\">\r\n<div class=\"main\" style=\"position:relative\">\r\n    <div class=\"LoginBack\">\r\n     <div>\r\n     <table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">\r\n      <tr>\r\n        <td class=\"td1\"><img src=\"images/comeBack.gif\" width=\"56\" height=\"49\" /></td>\r\n        <td class=\"td2\">您正在使用的易分销系统未经官方授权，无法登录后台管理。请联系易分销官方(www.shopefx.com)购买软件使用权。感谢您的关注！</td>\r\n      </tr>\r\n      <tr>\r\n        <th colspan=\"2\"><a href=\"" + Globals.GetSiteUrls().Home + "\">返回前台</a></th>\r\n        </tr>\r\n    </table>\r\n     </div>\r\n    </div>\r\n</div>\r\n</div><div class=\"footer\">Copyright 2009 ShopEFX.com all Rights Reserved. 本产品资源均为 海商网络技术有限公司 版权所有</div>\r\n</div>\r\n</body>\r\n</html>";
		private string ReferralLink
		{
			get
			{
				return this.ViewState["ReferralLink"] as string;
			}
			set
			{
				this.ViewState["ReferralLink"] = value;
			}
		}
		private bool CheckVerifyCode(string verifyCode)
		{
			return base.Request.Cookies[this.verifyCodeKey] != null && string.Compare(base.Request.Cookies[this.verifyCodeKey].Value, verifyCode, true, System.Globalization.CultureInfo.InvariantCulture) == 0;
		}
		protected override void OnInit(System.EventArgs e)
		{
			if (this.Page.Request.IsAuthenticated)
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
			base.OnInit(e);
		}
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnAdminLogin.Click += new System.EventHandler(this.btnAdminLogin_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!string.IsNullOrEmpty(base.Request["isCallback"]) && base.Request["isCallback"] == "true")
			{
				string verifyCode = base.Request["code"];
				string arg;
				if (!this.CheckVerifyCode(verifyCode))
				{
					arg = "0";
				}
				else
				{
					arg = "1";
				}
				base.Response.Clear();
				base.Response.ContentType = "application/json";
				base.Response.Write("{ ");
				base.Response.Write(string.Format("\"flag\":\"{0}\"", arg));
				base.Response.Write("}");
				base.Response.End();
			}
			if (!this.Page.IsPostBack)
			{
				System.Uri urlReferrer = this.Context.Request.UrlReferrer;
				if (urlReferrer != null)
				{
					this.ReferralLink = urlReferrer.ToString();
				}
				this.txtAdminName.Focus();
				PageTitle.AddSiteNameTitle("后台登录", Hidistro.Membership.Context.HiContext.Current.Context);
			}
		}
		private void btnAdminLogin_Click(object sender, System.EventArgs e)
		{
			if (!Hidistro.Membership.Context.HiContext.Current.CheckVerifyCode(this.txtCode.Text.Trim()))
			{
				this.ShowMessage("验证码不正确");
				return;
			}
			Hidistro.Membership.Core.IUser user = Hidistro.Membership.Context.Users.GetUser(0, this.txtAdminName.Text, false, true);
			if (user == null || user.IsAnonymous || user.UserRole != Hidistro.Membership.Core.Enums.UserRole.SiteManager || !user.IsInRole("区域发货点"))
			{
				this.ShowMessage("无效的用户信息");
				return;
			}
			string text = null;
			Hidistro.Membership.Context.SiteManager siteManager = user as Hidistro.Membership.Context.SiteManager;
			siteManager.Password = this.txtAdminPassWord.Text;
			Hidistro.Membership.Core.Enums.LoginUserStatus loginUserStatus = ManagerHelper.ValidLogin(siteManager);
			if (loginUserStatus == Hidistro.Membership.Core.Enums.LoginUserStatus.Success)
			{
				System.Web.HttpCookie authCookie = System.Web.Security.FormsAuthentication.GetAuthCookie(siteManager.Username, false);
				Hidistro.Membership.Core.IUserCookie userCookie = siteManager.GetUserCookie();
				userCookie.WriteCookie(authCookie, 30, false);
				Hidistro.Membership.Context.HiContext.Current.User = siteManager;
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["returnUrl"]))
				{
					text = this.Page.Request.QueryString["returnUrl"];
				}
				if (text == null && this.ReferralLink != null && !string.IsNullOrEmpty(this.ReferralLink.Trim()))
				{
					text = this.ReferralLink;
				}
				if (!string.IsNullOrEmpty(text) && (text.ToLower().IndexOf(Globals.GetSiteUrls().Logout.ToLower()) >= 0 || text.ToLower().IndexOf(Globals.GetSiteUrls().UrlData.FormatUrl("register").ToLower()) >= 0 || text.ToLower().IndexOf(Globals.GetSiteUrls().UrlData.FormatUrl("vote").ToLower()) >= 0 || text.ToLower().IndexOf("loginexit") >= 0))
				{
				}
				this.Page.Response.Redirect("default.html", true);
				return;
			}
			if (loginUserStatus == Hidistro.Membership.Core.Enums.LoginUserStatus.AccountPending)
			{
				this.ShowMessage("用户账号还没有通过审核");
				return;
			}
			if (loginUserStatus == Hidistro.Membership.Core.Enums.LoginUserStatus.AccountLockedOut)
			{
				this.ShowMessage("用户账号已被锁定，暂时不能登录系统");
				return;
			}
			if (loginUserStatus == Hidistro.Membership.Core.Enums.LoginUserStatus.InvalidCredentials)
			{
				this.ShowMessage("用户名或密码错误");
				return;
			}
			this.ShowMessage("登录失败，未知错误");
		}
		private void ShowMessage(string msg)
		{
			this.lblStatus.Text = msg;
			this.lblStatus.Success = false;
			this.lblStatus.Visible = true;
		}
		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			bool flag;
			bool flag2;
			int num;
			Hidistro.Membership.Context.LicenseChecker.Check(out flag, out flag2, out num);
			if (!flag)
			{
				writer.Write(this.licenseMsg);
				return;
			}
			if (flag2)
			{
				using (System.IO.StringWriter stringWriter = new System.IO.StringWriter())
				{
					using (System.Web.UI.HtmlTextWriter htmlTextWriter = new System.Web.UI.HtmlTextWriter(stringWriter))
					{
						base.Render(htmlTextWriter);
						string text = stringWriter.ToString();
						text = text.Insert(text.IndexOf("</body>"), "<div class=\"checkInfo\">\r\n   <table width=\"100%\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\">\r\n      <tr>\r\n        <td class=\"td1\"><img src=\"images/checkInfo.gif\" width=\"30\" height=\"30\" /></td>\r\n        <td class=\"td2\" width=\"100%\">您正在使用的易分销系统已过授权有效期！<br/>请联系官方(www.shopefx.com)购买软件使用权。</td>\r\n      </tr>\r\n   </table>\r\n</div>");
						writer.Write(text);
					}
				}
				return;
			}
			base.Render(writer);
		}
	}
}
