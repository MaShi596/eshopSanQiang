using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Shopping;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class Login : HtmlTemplatedWebControl
	{
		private System.Web.UI.WebControls.TextBox txtUserName;
		private System.Web.UI.WebControls.TextBox txtPassword;
		private IButton btnLogin;
		private System.Web.UI.WebControls.DropDownList ddlPlugins;
		private static string ReturnURL = string.Empty;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-Login.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
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
			if (!string.IsNullOrEmpty(this.Page.Request["action"]) && this.Page.Request["action"] == "Common_UserLogin")
			{
				string text = this.UserLogin(this.Page.Request["username"], this.Page.Request["password"]);
				string text2 = string.IsNullOrEmpty(text) ? "Succes" : "Fail";
				this.Page.Response.Clear();
				this.Page.Response.ContentType = "application/json";
				this.Page.Response.Write(string.Concat(new string[]
				{
					"{\"Status\":\"",
					text2,
					"\",\"Msg\":\"",
					text,
					"\"}"
				}));
				this.Page.Response.End();
			}
			this.txtUserName = (System.Web.UI.WebControls.TextBox)this.FindControl("txtUserName");
			this.txtPassword = (System.Web.UI.WebControls.TextBox)this.FindControl("txtPassword");
			this.btnLogin = ButtonManager.Create(this.FindControl("btnLogin"));
			this.ddlPlugins = (System.Web.UI.WebControls.DropDownList)this.FindControl("ddlPlugins");
			if (this.ddlPlugins != null)
			{
				this.ddlPlugins.Items.Add(new System.Web.UI.WebControls.ListItem("请选择登录方式", ""));
				System.Collections.Generic.IList<OpenIdSettingsInfo> configedItems = MemberProcessor.GetConfigedItems();
				if (configedItems != null && configedItems.Count > 0)
				{
					foreach (OpenIdSettingsInfo current in configedItems)
					{
						this.ddlPlugins.Items.Add(new System.Web.UI.WebControls.ListItem(current.Name, current.OpenIdType));
					}
				}
				this.ddlPlugins.SelectedIndexChanged += new System.EventHandler(this.ddlPlugins_SelectedIndexChanged);
			}
			if (this.Page.Request.UrlReferrer != null && !string.IsNullOrEmpty(this.Page.Request.UrlReferrer.OriginalString))
			{
				Login.ReturnURL = this.Page.Request.UrlReferrer.OriginalString;
			}
			this.txtUserName.Focus();
			PageTitle.AddSiteNameTitle("用户登录", Hidistro.Membership.Context.HiContext.Current.Context);
			this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
		}
		private void ddlPlugins_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (this.ddlPlugins.SelectedValue.Length > 0)
			{
				this.Page.Response.Redirect("OpenId/RedirectLogin.aspx?ot=" + this.ddlPlugins.SelectedValue);
			}
		}
		protected void btnLogin_Click(object sender, System.EventArgs e)
		{
			if (this.Page.IsValid)
			{
				string pattern = "[\\u4e00-\\u9fa5a-zA-Z0-9]+[\\u4e00-\\u9fa5_a-zA-Z0-9]*";
				System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern);
				if (!regex.IsMatch(this.txtUserName.Text.Trim()) || this.txtUserName.Text.Trim().Length < 2 || this.txtUserName.Text.Trim().Length > 20)
				{
					this.ShowMessage("用户名不能为空，必须以汉字或是字母开头,且在2-20个字符之间", false);
				}
				else
				{
					if (this.txtUserName.Text.Contains(","))
					{
						this.ShowMessage("用户名不能包含逗号", false);
					}
					else
					{
						string text = this.UserLogin(this.txtUserName.Text.Trim(), this.txtPassword.Text);
						if (!string.IsNullOrEmpty(text))
						{
							this.ShowMessage(text, false);
						}
						else
						{
							string text2 = this.Page.Request.QueryString["ReturnUrl"];
							if (string.IsNullOrEmpty(text2))
							{
								text2 = Globals.ApplicationPath + "/User/UserDefault.aspx";
							}
							else
							{
								if (string.IsNullOrEmpty(Login.ReturnURL))
								{
									text2 = Login.ReturnURL;
								}
							}
							this.Page.Response.Redirect(text2);
						}
					}
				}
			}
		}
		private string UserLogin(string userName, string password)
		{
			string text = string.Empty;
			Hidistro.Membership.Context.Member member = Hidistro.Membership.Context.Users.GetUser(0, userName, false, true) as Hidistro.Membership.Context.Member;
			string result;
			if (member == null || member.IsAnonymous)
			{
				result = "用户名或密码错误";
			}
			else
			{
				if (Hidistro.Membership.Context.HiContext.Current.SiteSettings.IsDistributorSettings)
				{
					if (!member.ParentUserId.HasValue || member.ParentUserId.Value != Hidistro.Membership.Context.HiContext.Current.SiteSettings.UserId)
					{
						result = "您不是本站会员，请您进行注册";
						return result;
					}
				}
				else
				{
					if (member.ParentUserId.HasValue && member.ParentUserId.Value != 0)
					{
						result = "您不是本站会员，请您进行注册";
						return result;
					}
				}
				member.Password = password;
				Hidistro.Membership.Core.Enums.LoginUserStatus loginUserStatus = MemberProcessor.ValidLogin(member);
				if (loginUserStatus == Hidistro.Membership.Core.Enums.LoginUserStatus.Success)
				{
					System.Web.HttpCookie authCookie = System.Web.Security.FormsAuthentication.GetAuthCookie(member.Username, false);
					Hidistro.Membership.Core.IUserCookie userCookie = member.GetUserCookie();
					userCookie.WriteCookie(authCookie, 30, false);
					ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
					CookieShoppingProvider cookieShoppingProvider = CookieShoppingProvider.Instance();
					bool flag = false;
					if (cookieShoppingProvider.GetShoppingCart() != null && cookieShoppingProvider.GetShoppingCart().GetQuantity() > 0)
					{
						flag = true;
					}
					cookieShoppingProvider.ClearShoppingCart();
					Hidistro.Membership.Context.HiContext.Current.User = member;
					if (shoppingCart != null && flag)
					{
						ShoppingCartProcessor.ConvertShoppingCartToDataBase(shoppingCart);
					}
					member.OnLogin();
				}
				else
				{
					if (loginUserStatus == Hidistro.Membership.Core.Enums.LoginUserStatus.AccountPending)
					{
						text = "用户账号还没有通过审核";
					}
					else
					{
						if (loginUserStatus == Hidistro.Membership.Core.Enums.LoginUserStatus.InvalidCredentials)
						{
							text = "用户名或密码错误";
						}
						else
						{
							text = "未知错误";
						}
					}
				}
				result = text;
			}
			return result;
		}
	}
}
