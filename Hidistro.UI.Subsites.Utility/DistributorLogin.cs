using Hidistro.Core;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using Hidistro.Subsites.Store;
using Hidistro.UI.Common.Controls;
using System;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Subsites.Utility
{
	public class DistributorLogin : HtmlTemplatedWebControl
	{
		private System.Web.UI.WebControls.TextBox txtUserName;
		private System.Web.UI.WebControls.TextBox txtPassword;
		private System.Web.UI.WebControls.TextBox txtCode;
		private System.Web.UI.WebControls.Button btnLogin;
		private string verifyCodeKey = "VerifyCode";
		private bool CheckVerifyCode(string verifyCode)
		{
			return System.Web.HttpContext.Current.Request.Cookies[this.verifyCodeKey] != null && string.Compare(System.Web.HttpContext.Current.Request.Cookies[this.verifyCodeKey].Value, verifyCode, true, System.Globalization.CultureInfo.InvariantCulture) == 0;
		}
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-DistributorLogin.html";
			}
			if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request["isCallback"]) && System.Web.HttpContext.Current.Request["isCallback"] == "true")
			{
				string verifyCode = System.Web.HttpContext.Current.Request["code"];
				string arg;
				if (!this.CheckVerifyCode(verifyCode))
				{
					arg = "0";
				}
				else
				{
					arg = "1";
				}
				System.Web.HttpContext.Current.Response.Clear();
				System.Web.HttpContext.Current.Response.ContentType = "application/json";
				System.Web.HttpContext.Current.Response.Write("{ ");
				System.Web.HttpContext.Current.Response.Write(string.Format("\"flag\":\"{0}\"", arg));
				System.Web.HttpContext.Current.Response.Write("}");
				System.Web.HttpContext.Current.Response.End();
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			Hidistro.Membership.Context.HiContext arg_05_0 = Hidistro.Membership.Context.HiContext.Current;
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
				this.Page.Response.Cookies["hishopLoginStatus"].Value = "";
			}
			this.txtUserName = (System.Web.UI.WebControls.TextBox)this.FindControl("txtUserName");
			this.txtPassword = (System.Web.UI.WebControls.TextBox)this.FindControl("txtPassword");
			this.btnLogin = (System.Web.UI.WebControls.Button)this.FindControl("btnLogin");
			this.txtCode = (System.Web.UI.WebControls.TextBox)this.FindControl("txtCode");
			this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
		}
		private void btnLogin_Click(object sender, System.EventArgs e)
		{
			if (!Hidistro.Membership.Context.HiContext.Current.CheckVerifyCode(this.txtCode.Text.Trim()))
			{
				this.ShowMessage("验证码不正确", false);
			}
			else
			{
				Hidistro.Membership.Core.IUser user = Hidistro.Membership.Context.Users.GetUser(0, this.txtUserName.Text, false, true);
				if (user == null || user.IsAnonymous || user.UserRole != Hidistro.Membership.Core.Enums.UserRole.Distributor)
				{
					this.ShowMessage("无效的用户信息", false);
				}
				else
				{
					Hidistro.Membership.Context.Distributor distributor = user as Hidistro.Membership.Context.Distributor;
					distributor.Password = this.txtPassword.Text;
					if (Hidistro.Membership.Context.HiContext.Current.SiteSettings.IsDistributorSettings && user.UserId != Hidistro.Membership.Context.HiContext.Current.SiteSettings.UserId.Value)
					{
						this.ShowMessage("分销商只能在自己的站点或主站上登录", false);
					}
					else
					{
						Hidistro.Membership.Core.Enums.LoginUserStatus loginUserStatus = SubsiteStoreHelper.ValidLogin(distributor);
						if (loginUserStatus == Hidistro.Membership.Core.Enums.LoginUserStatus.Success)
						{
							System.Web.HttpCookie authCookie = System.Web.Security.FormsAuthentication.GetAuthCookie(distributor.Username, false);
							Hidistro.Membership.Core.IUserCookie userCookie = distributor.GetUserCookie();
							userCookie.WriteCookie(authCookie, 30, false);
							this.Page.Response.Cookies["hishopLoginStatus"].Value = "true";
							Hidistro.Membership.Context.HiContext.Current.User = distributor;
							distributor.OnLogin();
							Hidistro.Membership.Context.SiteSettings siteSettings = Hidistro.Membership.Context.SettingsManager.GetSiteSettings(Hidistro.Membership.Context.HiContext.Current.User.UserId);
							if (siteSettings == null)
							{
								this.Page.Response.Redirect("nositedefault.aspx", true);
							}
							else
							{
								this.Page.Response.Redirect("default.aspx", true);
							}
						}
						else
						{
							if (loginUserStatus == Hidistro.Membership.Core.Enums.LoginUserStatus.AccountPending)
							{
								this.ShowMessage("用户账号还没有通过审核", false);
							}
							else
							{
								if (loginUserStatus == Hidistro.Membership.Core.Enums.LoginUserStatus.AccountLockedOut)
								{
									this.ShowMessage("用户账号已被锁定，暂时不能登录系统", false);
								}
								else
								{
									if (loginUserStatus == Hidistro.Membership.Core.Enums.LoginUserStatus.InvalidCredentials)
									{
										this.ShowMessage("用户名或密码错误", false);
									}
									else
									{
										this.ShowMessage("登录失败，未知错误", false);
									}
								}
							}
						}
					}
				}
			}
		}
		private void ShowMessage(string string_0)
		{
		}
	}
}
