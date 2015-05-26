using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Shopping;
using Hishop.Plugins;
using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
namespace Hidistro.UI.Web.OpenID
{
	public class OpenIdEntry : System.Web.UI.Page
	{
		private string openIdType;
		private System.Collections.Specialized.NameValueCollection parameters;
		protected System.Web.UI.HtmlControls.HtmlForm form1;
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
			this.openIdType = this.Page.Request.QueryString["HIGW"];
			OpenIdSettingsInfo openIdSettings = MemberProcessor.GetOpenIdSettings(this.openIdType);
			if (openIdSettings == null)
			{
				base.Response.Write("登录失败，没有找到对应的插件配置信息。");
				return;
			}
			this.parameters = new System.Collections.Specialized.NameValueCollection
			{
				this.Page.Request.Form,
				this.Page.Request.QueryString
			};
			OpenIdNotify openIdNotify = OpenIdNotify.CreateInstance(this.openIdType, this.parameters);
			openIdNotify.Authenticated += new System.EventHandler<AuthenticatedEventArgs>(this.Notify_Authenticated);
			openIdNotify.Failed += new System.EventHandler<FailedEventArgs>(this.Notify_Failed);
			try
			{
				openIdNotify.Verify(30000, HiCryptographer.Decrypt(openIdSettings.Settings));
			}
			catch
			{
				this.Page.Response.Redirect(Globals.GetSiteUrls().Home);
			}
		}
		private void Notify_Failed(object sender, FailedEventArgs e)
		{
			base.Response.Write("登录失败，" + e.Message);
		}
		private void Notify_Authenticated(object sender, AuthenticatedEventArgs e)
		{
			this.parameters.Add("CurrentOpenId", e.OpenId);
			Hidistro.Membership.Context.HiContext current = Hidistro.Membership.Context.HiContext.Current;
			string usernameWithOpenId = Hidistro.Membership.Core.UserHelper.GetUsernameWithOpenId(e.OpenId, this.openIdType);
			if (!string.IsNullOrEmpty(usernameWithOpenId))
			{
				Hidistro.Membership.Context.Member member = Hidistro.Membership.Context.Users.GetUser(0, usernameWithOpenId, false, true) as Hidistro.Membership.Context.Member;
				if (member == null)
				{
					base.Response.Write("登录失败，信任登录只能用于会员登录。");
					return;
				}
				if (Hidistro.Membership.Context.HiContext.Current.SiteSettings.IsDistributorSettings)
				{
					if (!member.ParentUserId.HasValue || member.ParentUserId.Value != Hidistro.Membership.Context.HiContext.Current.SiteSettings.UserId)
					{
						base.Response.Write("账号已经与本平台的其它子站绑定，不能在此域名上登录。");
						return;
					}
				}
				else
				{
					if (member.ParentUserId.HasValue && member.ParentUserId.Value != 0)
					{
						base.Response.Write("账号已经与本平台的其它子站绑定，不能在此域名上登录。");
						return;
					}
				}
				System.Web.HttpCookie authCookie = System.Web.Security.FormsAuthentication.GetAuthCookie(member.Username, false);
				Hidistro.Membership.Core.IUserCookie userCookie = member.GetUserCookie();
				userCookie.WriteCookie(authCookie, 30, false);
				Hidistro.Membership.Context.HiContext.Current.User = member;
				ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
				CookieShoppingProvider cookieShoppingProvider = CookieShoppingProvider.Instance();
				cookieShoppingProvider.ClearShoppingCart();
				current.User = member;
				bool flag = false;
				if (cookieShoppingProvider.GetShoppingCart() != null && cookieShoppingProvider.GetShoppingCart().GetQuantity() > 0)
				{
					flag = true;
					cookieShoppingProvider.ClearShoppingCart();
				}
				if (shoppingCart != null && flag)
				{
					ShoppingCartProcessor.ConvertShoppingCartToDataBase(shoppingCart);
				}
				if (!string.IsNullOrEmpty(this.parameters["token"]))
				{
					System.Web.HttpCookie httpCookie = new System.Web.HttpCookie("Token_" + Hidistro.Membership.Context.HiContext.Current.User.UserId.ToString());
					httpCookie.Expires = System.DateTime.Now.AddMinutes(30.0);
					httpCookie.Value = this.parameters["token"];
					System.Web.HttpContext.Current.Response.Cookies.Add(httpCookie);
				}
			}
			else
			{
				string a;
				if ((a = this.openIdType.ToLower()) != null)
				{
					if (a == "hishop.plugins.openid.alipay.alipayservice")
					{
						this.SkipAlipayOpenId();
						goto IL_288;
					}
					if (a == "hishop.plugins.openid.qq.qqservice")
					{
						this.SkipQQOpenId();
						goto IL_288;
					}
					if (a == "hishop.plugins.openid.taobao.taobaoservice")
					{
						this.SkipTaoBaoOpenId();
						goto IL_288;
					}
					if (a == "hishop.plugins.openid.sina.sinaservice")
					{
						this.SkipSinaOpenId();
						goto IL_288;
					}
				}
				this.Page.Response.Redirect(Globals.GetSiteUrls().Home);
			}
			IL_288:
			string a2 = this.parameters["HITO"];
			if (a2 == "1")
			{
				this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("submitOrder"));
				return;
			}
			this.Page.Response.Redirect(Globals.GetSiteUrls().Home);
		}
		protected void SkipAlipayOpenId()
		{
			Hidistro.Membership.Context.Member member;
			if (Hidistro.Membership.Context.HiContext.Current.SiteSettings.IsDistributorSettings)
			{
				member = new Hidistro.Membership.Context.Member(Hidistro.Membership.Core.Enums.UserRole.Underling);
				member.ParentUserId = Hidistro.Membership.Context.HiContext.Current.SiteSettings.UserId;
			}
			else
			{
				member = new Hidistro.Membership.Context.Member(Hidistro.Membership.Core.Enums.UserRole.Member);
			}
			if (Hidistro.Membership.Context.HiContext.Current.ReferralUserId > 0)
			{
				member.ReferralUserId = new int?(Hidistro.Membership.Context.HiContext.Current.ReferralUserId);
			}
			member.GradeId = MemberProcessor.GetDefaultMemberGrade();
			member.Username = this.parameters["real_name"];
			if (string.IsNullOrEmpty(member.Username))
			{
				member.Username = "支付宝会员_" + this.parameters["user_id"];
			}
			member.Email = this.parameters["email"];
			if (string.IsNullOrEmpty(member.Email))
			{
				member.Email = this.GenerateUsername() + "@localhost.com";
			}
			string text = this.GeneratePassword();
			member.Password = text;
			member.PasswordFormat = System.Web.Security.MembershipPasswordFormat.Hashed;
			member.TradePasswordFormat = System.Web.Security.MembershipPasswordFormat.Hashed;
			member.TradePassword = text;
			member.IsApproved = true;
			member.RealName = string.Empty;
			member.Address = string.Empty;
			if (MemberProcessor.CreateMember(member) != Hidistro.Membership.Core.Enums.CreateUserStatus.Created)
			{
				member.Username = "支付宝会员_" + this.parameters["user_id"];
				member.Password = (member.TradePassword = text);
				if (MemberProcessor.CreateMember(member) != Hidistro.Membership.Core.Enums.CreateUserStatus.Created)
				{
					member.Username = this.GenerateUsername();
					member.Email = this.GenerateUsername() + "@localhost.com";
					member.Password = (member.TradePassword = text);
					if (MemberProcessor.CreateMember(member) != Hidistro.Membership.Core.Enums.CreateUserStatus.Created)
					{
						base.Response.Write("为您创建随机账户时失败，请重试。");
						return;
					}
				}
			}
			Hidistro.Membership.Core.UserHelper.BindOpenId(member.Username, this.parameters["CurrentOpenId"], this.parameters["HIGW"]);
			System.Web.HttpCookie authCookie = System.Web.Security.FormsAuthentication.GetAuthCookie(member.Username, false);
			Hidistro.Membership.Core.IUserCookie userCookie = member.GetUserCookie();
			userCookie.WriteCookie(authCookie, 30, false);
			ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
			CookieShoppingProvider cookieShoppingProvider = CookieShoppingProvider.Instance();
			Hidistro.Membership.Context.HiContext.Current.User = member;
			bool flag = false;
			if (cookieShoppingProvider.GetShoppingCart() != null && cookieShoppingProvider.GetShoppingCart().GetQuantity() > 0)
			{
				flag = true;
				cookieShoppingProvider.ClearShoppingCart();
			}
			if (shoppingCart != null && flag)
			{
				ShoppingCartProcessor.ConvertShoppingCartToDataBase(shoppingCart);
			}
			if (!string.IsNullOrEmpty(this.parameters["token"]))
			{
				System.Web.HttpCookie httpCookie = new System.Web.HttpCookie("Token_" + Hidistro.Membership.Context.HiContext.Current.User.UserId.ToString());
				httpCookie.Expires = System.DateTime.Now.AddMinutes(30.0);
				httpCookie.Value = this.parameters["token"];
				System.Web.HttpContext.Current.Response.Cookies.Add(httpCookie);
			}
			if (!string.IsNullOrEmpty(this.parameters["target_url"]))
			{
				this.Page.Response.Redirect(this.parameters["target_url"]);
			}
			this.Page.Response.Redirect(Globals.GetSiteUrls().Home);
		}
		protected void SkipQQOpenId()
		{
			Hidistro.Membership.Context.Member member;
			if (Hidistro.Membership.Context.HiContext.Current.SiteSettings.IsDistributorSettings)
			{
				member = new Hidistro.Membership.Context.Member(Hidistro.Membership.Core.Enums.UserRole.Underling);
				member.ParentUserId = Hidistro.Membership.Context.HiContext.Current.SiteSettings.UserId;
			}
			else
			{
				member = new Hidistro.Membership.Context.Member(Hidistro.Membership.Core.Enums.UserRole.Member);
			}
			if (Hidistro.Membership.Context.HiContext.Current.ReferralUserId > 0)
			{
				member.ReferralUserId = new int?(Hidistro.Membership.Context.HiContext.Current.ReferralUserId);
			}
			member.GradeId = MemberProcessor.GetDefaultMemberGrade();
			System.Web.HttpCookie httpCookie = System.Web.HttpContext.Current.Request.Cookies["NickName"];
			if (httpCookie != null)
			{
				member.Username = System.Web.HttpUtility.UrlDecode(httpCookie.Value);
			}
			if (string.IsNullOrEmpty(member.Username))
			{
				member.Username = "腾讯会员_" + this.GenerateUsername(8);
			}
			member.Email = this.GenerateUsername() + "@localhost.com";
			string text = this.GeneratePassword();
			member.Password = text;
			member.PasswordFormat = System.Web.Security.MembershipPasswordFormat.Hashed;
			member.TradePasswordFormat = System.Web.Security.MembershipPasswordFormat.Hashed;
			member.TradePassword = text;
			member.IsApproved = true;
			member.RealName = string.Empty;
			member.Address = string.Empty;
			if (MemberProcessor.CreateMember(member) != Hidistro.Membership.Core.Enums.CreateUserStatus.Created)
			{
				member.Username = "腾讯会员_" + this.GenerateUsername(8);
				member.Password = (member.TradePassword = text);
				if (MemberProcessor.CreateMember(member) != Hidistro.Membership.Core.Enums.CreateUserStatus.Created)
				{
					member.Username = this.GenerateUsername();
					member.Email = this.GenerateUsername() + "@localhost.com";
					member.Password = (member.TradePassword = text);
					if (MemberProcessor.CreateMember(member) != Hidistro.Membership.Core.Enums.CreateUserStatus.Created)
					{
						base.Response.Write("为您创建随机账户时失败，请重试。");
						return;
					}
				}
			}
			Hidistro.Membership.Core.UserHelper.BindOpenId(member.Username, this.parameters["CurrentOpenId"], this.parameters["HIGW"]);
			System.Web.HttpCookie authCookie = System.Web.Security.FormsAuthentication.GetAuthCookie(member.Username, false);
			Hidistro.Membership.Core.IUserCookie userCookie = member.GetUserCookie();
			userCookie.WriteCookie(authCookie, 30, false);
			ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
			CookieShoppingProvider cookieShoppingProvider = CookieShoppingProvider.Instance();
			cookieShoppingProvider.ClearShoppingCart();
			Hidistro.Membership.Context.HiContext.Current.User = member;
			if (shoppingCart != null)
			{
				ShoppingCartProcessor.ConvertShoppingCartToDataBase(shoppingCart);
			}
			if (!string.IsNullOrEmpty(this.parameters["token"]))
			{
				System.Web.HttpCookie httpCookie2 = new System.Web.HttpCookie("Token_" + Hidistro.Membership.Context.HiContext.Current.User.UserId.ToString());
				httpCookie2.Expires = System.DateTime.Now.AddMinutes(30.0);
				httpCookie2.Value = this.parameters["token"];
				System.Web.HttpContext.Current.Response.Cookies.Add(httpCookie2);
			}
			if (!string.IsNullOrEmpty(this.parameters["target_url"]))
			{
				this.Page.Response.Redirect(this.parameters["target_url"]);
			}
			this.Page.Response.Redirect(Globals.GetSiteUrls().Home);
		}
		protected void SkipTaoBaoOpenId()
		{
			Hidistro.Membership.Context.Member member;
			if (Hidistro.Membership.Context.HiContext.Current.SiteSettings.IsDistributorSettings)
			{
				member = new Hidistro.Membership.Context.Member(Hidistro.Membership.Core.Enums.UserRole.Underling);
				member.ParentUserId = Hidistro.Membership.Context.HiContext.Current.SiteSettings.UserId;
			}
			else
			{
				member = new Hidistro.Membership.Context.Member(Hidistro.Membership.Core.Enums.UserRole.Member);
			}
			if (Hidistro.Membership.Context.HiContext.Current.ReferralUserId > 0)
			{
				member.ReferralUserId = new int?(Hidistro.Membership.Context.HiContext.Current.ReferralUserId);
			}
			member.GradeId = MemberProcessor.GetDefaultMemberGrade();
			string text = this.parameters["CurrentOpenId"];
			if (!string.IsNullOrEmpty(text))
			{
				member.Username = System.Web.HttpUtility.UrlDecode(text);
			}
			if (string.IsNullOrEmpty(member.Username))
			{
				member.Username = "淘宝会员_" + this.GenerateUsername(8);
			}
			member.Email = this.GenerateUsername() + "@localhost.com";
			if (string.IsNullOrEmpty(member.Email))
			{
				member.Email = this.GenerateUsername() + "@localhost.com";
			}
			string text2 = this.GeneratePassword();
			member.Password = text2;
			member.PasswordFormat = System.Web.Security.MembershipPasswordFormat.Hashed;
			member.TradePasswordFormat = System.Web.Security.MembershipPasswordFormat.Hashed;
			member.TradePassword = text2;
			member.IsApproved = true;
			member.RealName = string.Empty;
			member.Address = string.Empty;
			if (MemberProcessor.CreateMember(member) != Hidistro.Membership.Core.Enums.CreateUserStatus.Created)
			{
				member.Username = "淘宝会员_" + this.GenerateUsername(8);
				member.Password = (member.TradePassword = text2);
				if (MemberProcessor.CreateMember(member) != Hidistro.Membership.Core.Enums.CreateUserStatus.Created)
				{
					member.Username = this.GenerateUsername();
					member.Email = this.GenerateUsername() + "@localhost.com";
					member.Password = (member.TradePassword = text2);
					if (MemberProcessor.CreateMember(member) != Hidistro.Membership.Core.Enums.CreateUserStatus.Created)
					{
						base.Response.Write("为您创建随机账户时失败，请重试。");
						return;
					}
				}
			}
			Hidistro.Membership.Core.UserHelper.BindOpenId(member.Username, this.parameters["CurrentOpenId"], this.parameters["HIGW"]);
			System.Web.HttpCookie authCookie = System.Web.Security.FormsAuthentication.GetAuthCookie(member.Username, false);
			Hidistro.Membership.Core.IUserCookie userCookie = member.GetUserCookie();
			userCookie.WriteCookie(authCookie, 30, false);
			ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
			CookieShoppingProvider cookieShoppingProvider = CookieShoppingProvider.Instance();
			cookieShoppingProvider.ClearShoppingCart();
			Hidistro.Membership.Context.HiContext.Current.User = member;
			if (shoppingCart != null)
			{
				ShoppingCartProcessor.ConvertShoppingCartToDataBase(shoppingCart);
			}
			if (!string.IsNullOrEmpty(this.parameters["token"]))
			{
				System.Web.HttpCookie httpCookie = new System.Web.HttpCookie("Token_" + Hidistro.Membership.Context.HiContext.Current.User.UserId.ToString());
				httpCookie.Expires = System.DateTime.Now.AddMinutes(30.0);
				httpCookie.Value = this.parameters["token"];
				System.Web.HttpContext.Current.Response.Cookies.Add(httpCookie);
			}
			if (!string.IsNullOrEmpty(this.parameters["target_url"]))
			{
				this.Page.Response.Redirect(this.parameters["target_url"]);
			}
			this.Page.Response.Redirect(Globals.GetSiteUrls().Home);
		}
		protected void SkipSinaOpenId()
		{
			Hidistro.Membership.Context.Member member;
			if (Hidistro.Membership.Context.HiContext.Current.SiteSettings.IsDistributorSettings)
			{
				member = new Hidistro.Membership.Context.Member(Hidistro.Membership.Core.Enums.UserRole.Underling);
				member.ParentUserId = Hidistro.Membership.Context.HiContext.Current.SiteSettings.UserId;
			}
			else
			{
				member = new Hidistro.Membership.Context.Member(Hidistro.Membership.Core.Enums.UserRole.Member);
			}
			if (Hidistro.Membership.Context.HiContext.Current.ReferralUserId > 0)
			{
				member.ReferralUserId = new int?(Hidistro.Membership.Context.HiContext.Current.ReferralUserId);
			}
			member.GradeId = MemberProcessor.GetDefaultMemberGrade();
			member.Username = this.parameters["CurrentOpenId"];
			if (string.IsNullOrEmpty(member.Username))
			{
				member.Username = "新浪微博会员_" + this.GenerateUsername(8);
			}
			member.Email = this.GenerateUsername() + "@localhost.com";
			string text = this.GeneratePassword();
			member.Password = text;
			member.PasswordFormat = System.Web.Security.MembershipPasswordFormat.Hashed;
			member.TradePasswordFormat = System.Web.Security.MembershipPasswordFormat.Hashed;
			member.TradePassword = text;
			member.IsApproved = true;
			member.RealName = string.Empty;
			member.Address = string.Empty;
			if (MemberProcessor.CreateMember(member) != Hidistro.Membership.Core.Enums.CreateUserStatus.Created)
			{
				member.Username = "新浪微博会员_" + this.GenerateUsername(9);
				member.Password = (member.TradePassword = text);
				if (MemberProcessor.CreateMember(member) != Hidistro.Membership.Core.Enums.CreateUserStatus.Created)
				{
					member.Username = this.GenerateUsername();
					member.Email = this.GenerateUsername() + "@localhost.com";
					member.Password = (member.TradePassword = text);
					if (MemberProcessor.CreateMember(member) != Hidistro.Membership.Core.Enums.CreateUserStatus.Created)
					{
						base.Response.Write("为您创建随机账户时失败，请重试。");
						return;
					}
				}
			}
			Hidistro.Membership.Core.UserHelper.BindOpenId(member.Username, this.parameters["CurrentOpenId"], this.parameters["HIGW"]);
			System.Web.HttpCookie authCookie = System.Web.Security.FormsAuthentication.GetAuthCookie(member.Username, false);
			Hidistro.Membership.Core.IUserCookie userCookie = member.GetUserCookie();
			userCookie.WriteCookie(authCookie, 30, false);
			ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
			CookieShoppingProvider cookieShoppingProvider = CookieShoppingProvider.Instance();
			cookieShoppingProvider.ClearShoppingCart();
			Hidistro.Membership.Context.HiContext.Current.User = member;
			if (shoppingCart != null)
			{
				ShoppingCartProcessor.ConvertShoppingCartToDataBase(shoppingCart);
			}
			if (!string.IsNullOrEmpty(this.parameters["token"]))
			{
				System.Web.HttpCookie httpCookie = new System.Web.HttpCookie("Token_" + Hidistro.Membership.Context.HiContext.Current.User.UserId.ToString());
				httpCookie.Expires = System.DateTime.Now.AddMinutes(30.0);
				httpCookie.Value = this.parameters["token"];
				System.Web.HttpContext.Current.Response.Cookies.Add(httpCookie);
			}
			this.Page.Response.Redirect(Globals.GetSiteUrls().Home);
		}
		private string GenerateUsername(int length)
		{
			return this.GenerateRndString(length, "u_");
		}
		private string GenerateUsername()
		{
			return this.GenerateRndString(10, "u_");
		}
		private string GeneratePassword()
		{
			return this.GenerateRndString(8, "");
		}
		private string GenerateRndString(int length, string prefix)
		{
			string text = string.Empty;
			System.Random random = new System.Random();
			while (text.Length < 10)
			{
				int num = random.Next();
				char c;
				if (num % 3 == 0)
				{
					c = (char)(97 + (ushort)(num % 26));
				}
				else
				{
					c = (char)(48 + (ushort)(num % 10));
				}
				text += c.ToString();
			}
			return prefix + text;
		}
	}
}
