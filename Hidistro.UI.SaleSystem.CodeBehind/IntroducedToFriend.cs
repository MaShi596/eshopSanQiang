using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using Hidistro.Messages;
using Hidistro.SaleSystem.Catalog;
using Hidistro.UI.Common.Controls;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class IntroducedToFriend : HtmlTemplatedWebControl
	{
		private int productId = 0;
		private System.Web.UI.WebControls.HyperLink hlinkProductOfTitle;
		private System.Web.UI.WebControls.HyperLink hlinkProductOfContext;
		private System.Web.UI.WebControls.HyperLink hlinkHome;
		private System.Web.UI.WebControls.Literal litProductUrl;
		private System.Web.UI.WebControls.TextBox txtFriendEmail;
		private System.Web.UI.WebControls.TextBox txtFriendName;
		private System.Web.UI.WebControls.TextBox txtUserName;
		private System.Web.UI.WebControls.TextBox txtMessage;
		private System.Web.UI.WebControls.Button btnRefer;
		private System.Web.UI.HtmlControls.HtmlInputText txtTJCode;
		private string verifyCodeKey = "VerifyCode";
		private bool CheckVerifyCode(string verifyCode)
		{
			return System.Web.HttpContext.Current.Request.Cookies[this.verifyCodeKey] != null && string.Compare(System.Web.HttpContext.Current.Request.Cookies[this.verifyCodeKey].Value, verifyCode, true, System.Globalization.CultureInfo.InvariantCulture) == 0;
		}
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-IntroducedToFriend.html";
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
			if (!int.TryParse(this.Page.Request.QueryString["productId"], out this.productId))
			{
				base.GotoResourceNotFound();
			}
			this.hlinkProductOfTitle = (System.Web.UI.WebControls.HyperLink)this.FindControl("hlinkProductOfTitle");
			this.hlinkProductOfContext = (System.Web.UI.WebControls.HyperLink)this.FindControl("hlinkProductOfContext");
			this.hlinkHome = (System.Web.UI.WebControls.HyperLink)this.FindControl("hlinkHome");
			this.litProductUrl = (System.Web.UI.WebControls.Literal)this.FindControl("litProductUrl");
			this.txtFriendEmail = (System.Web.UI.WebControls.TextBox)this.FindControl("txtFriendEmail");
			this.txtFriendName = (System.Web.UI.WebControls.TextBox)this.FindControl("txtFriendName");
			this.txtUserName = (System.Web.UI.WebControls.TextBox)this.FindControl("txtUserName");
			this.txtMessage = (System.Web.UI.WebControls.TextBox)this.FindControl("txtMessage");
			this.btnRefer = (System.Web.UI.WebControls.Button)this.FindControl("btnRefer");
			this.txtTJCode = (System.Web.UI.HtmlControls.HtmlInputText)this.FindControl("txtTJCode");
			this.btnRefer.Click += new System.EventHandler(this.btnRefer_Click);
			if (!this.Page.IsPostBack)
			{
				ProductInfo productSimpleInfo = ProductBrowser.GetProductSimpleInfo(this.productId);
				if (productSimpleInfo != null)
				{
					Hidistro.Membership.Core.IUser user = Hidistro.Membership.Context.HiContext.Current.User;
					this.txtUserName.Text = user.Username;
					this.hlinkProductOfTitle.Text = productSimpleInfo.ProductName;
					this.hlinkProductOfTitle.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[]
					{
						this.productId
					});
					this.hlinkProductOfContext.Text = productSimpleInfo.ProductName;
					this.hlinkProductOfContext.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[]
					{
						this.productId
					});
					this.hlinkHome.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("home");
					this.hlinkHome.Text = Hidistro.Membership.Context.HiContext.Current.SiteSettings.SiteName;
					this.txtTJCode.Value = string.Empty;
					if (user.UserRole == Hidistro.Membership.Core.Enums.UserRole.Member || user.UserRole == Hidistro.Membership.Core.Enums.UserRole.Underling)
					{
						this.litProductUrl.Text = Globals.FullPath(System.Web.HttpContext.Current.Request.Url.PathAndQuery).Replace("IntroducedToFriend", "productDetails") + "&ReferralUserId=" + user.UserId;
					}
					else
					{
						this.litProductUrl.Text = Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[]
						{
							this.productId
						}));
					}
					PageTitle.AddSiteNameTitle(productSimpleInfo.ProductName + " 推荐给好友", Hidistro.Membership.Context.HiContext.Current.Context);
				}
			}
		}
		private void btnRefer_Click(object sender, System.EventArgs e)
		{
			if (!Hidistro.Membership.Context.HiContext.Current.CheckVerifyCode(this.txtTJCode.Value))
			{
				this.ShowMessage("验证码不正确", false);
			}
			else
			{
				if (!Hidistro.Membership.Context.HiContext.Current.SiteSettings.EmailEnabled)
				{
					this.ShowMessage("系统还未设置电子邮件，暂时不能发送邮件", false);
				}
				else
				{
					if (this.ValidateConvert())
					{
						string subject = string.Format("您的好友{0}给您推荐了一个好宝贝", this.txtUserName.Text);
						string body = string.Format("{0}，您好！<br/>我在{1}网站上看到{2}，快点出手吧 ，棒极了！你去看看吧！这个东东的网址是：<br/>{3}<br>{4}", new object[]
						{
							this.txtFriendName.Text,
							Hidistro.Membership.Context.HiContext.Current.SiteSettings.SiteName,
							this.hlinkProductOfTitle.Text,
							this.litProductUrl.Text,
							this.txtMessage.Text
						});
						string msg;
						if (Messenger.SendMail(subject, body, this.txtFriendEmail.Text.Trim(), Hidistro.Membership.Context.HiContext.Current.SiteSettings, out msg) != SendStatus.Success)
						{
							this.ShowMessage(msg, false);
						}
						this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), "success", string.Format("<script>alert(\"{0}\");window.location.href=\"{1}\"</script>", "发送成功", Globals.GetSiteUrls().UrlData.FormatUrl("IntroducedToFriend", new object[]
						{
							this.productId
						})));
					}
				}
			}
		}
		private bool ValidateConvert()
		{
			string text = string.Empty;
			if (string.IsNullOrEmpty(this.txtMessage.Text) || this.txtMessage.Text.Length > 30)
			{
				text += Formatter.FormatErrorMessage("请输入您的留言，不能为空长度限制在1-300之间");
			}
			if (string.IsNullOrEmpty(this.txtUserName.Text) || this.txtUserName.Text.Length > 30)
			{
				text += Formatter.FormatErrorMessage("请输入您的名字，不能为空长度限制在1-30之间");
			}
			if (string.IsNullOrEmpty(this.txtFriendName.Text) || this.txtFriendName.Text.Length > 30)
			{
				text += Formatter.FormatErrorMessage("请输入好友的名字，不能为空长度限制在1-30之间");
			}
			if (string.IsNullOrEmpty(this.txtFriendEmail.Text) || this.txtFriendEmail.Text.Length > 256 || !System.Text.RegularExpressions.Regex.IsMatch(this.txtFriendEmail.Text, "([a-zA-Z\\.0-9_-])+@([a-zA-Z0-9_-])+((\\.[a-zA-Z0-9_-]{2,4}){1,2})"))
			{
				text += Formatter.FormatErrorMessage("请输入正确好友邮箱，不能为空长度限制在1-256之间");
			}
			bool result;
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowMessage(text, false);
				result = false;
			}
			else
			{
				result = true;
			}
			return result;
		}
	}
}
