using Hidistro.AccountCenter.Business;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class RechargeRequest : MemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litUserName;
		private System.Web.UI.WebControls.RadioButtonList rbtnPaymentMode;
		private FormatedMoneyLabel litUseableBalance;
		private System.Web.UI.WebControls.TextBox txtReChargeBalance;
		private IButton btnReCharge;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-RechargeRequest.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.litUserName = (System.Web.UI.WebControls.Literal)this.FindControl("litUserName");
			this.rbtnPaymentMode = (System.Web.UI.WebControls.RadioButtonList)this.FindControl("rbtnPaymentMode");
			this.txtReChargeBalance = (System.Web.UI.WebControls.TextBox)this.FindControl("txtReChargeBalance");
			this.btnReCharge = ButtonManager.Create(this.FindControl("btnReCharge"));
			this.litUseableBalance = (FormatedMoneyLabel)this.FindControl("litUseableBalance");
			PageTitle.AddSiteNameTitle("预付款充值", HiContext.Current.Context);
			this.btnReCharge.Click += new System.EventHandler(this.btnReCharge_Click);
			if (!this.Page.IsPostBack)
			{
				Member member = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
				if (!member.IsOpenBalance)
				{
					this.Page.Response.Redirect(Globals.ApplicationPath + string.Format("/user/OpenBalance.aspx?ReturnUrl={0}", System.Web.HttpContext.Current.Request.Url));
				}
				this.BindPaymentMode();
				this.litUserName.Text = HiContext.Current.User.Username;
				this.litUseableBalance.Money = member.Balance - member.RequestBalance;
			}
		}
		protected void btnReCharge_Click(object sender, System.EventArgs e)
		{
			if (this.rbtnPaymentMode.Items.Count == 0)
			{
				this.ShowMessage("无法充值,因为后台没有添加支付方式", false);
			}
			else
			{
				if (this.rbtnPaymentMode.SelectedValue == null)
				{
					this.ShowMessage("选择要充值使用的支付方式", false);
				}
				else
				{
					int num = 0;
					if (this.txtReChargeBalance.Text.Trim().IndexOf(".") > 0)
					{
						num = this.txtReChargeBalance.Text.Trim().Substring(this.txtReChargeBalance.Text.Trim().IndexOf(".") + 1).Length;
					}
					decimal num2;
					if (!decimal.TryParse(this.txtReChargeBalance.Text, out num2) || decimal.Parse(this.txtReChargeBalance.Text) <= 0m || num > 2)
					{
						this.ShowMessage("请输入大于0的充值金额且金额整数位数在1到10之间,且不能超过2位小数", false);
					}
					else
					{
						this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("user_RechargeConfirm", new object[]
						{
							this.Page.Server.UrlEncode(this.rbtnPaymentMode.SelectedValue),
							this.Page.Server.UrlEncode(this.txtReChargeBalance.Text)
						}));
					}
				}
			}
		}
		private void BindPaymentMode()
		{
			System.Collections.Generic.IList<PaymentModeInfo> paymentModes = TradeHelper.GetPaymentModes();
			if (paymentModes.Count > 0)
			{
				foreach (PaymentModeInfo current in paymentModes)
				{
					string text = current.Gateway.ToLower();
					if (current.IsUseInpour && !text.Equals("hishop.plugins.payment.advancerequest") && !text.Equals("hishop.plugins.payment.bankrequest"))
					{
						if (text.Equals("hishop.plugins.payment.alipay_shortcut.shortcutrequest"))
						{
							System.Web.HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["Token_" + HiContext.Current.User.UserId.ToString()];
							if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
							{
								this.rbtnPaymentMode.Items.Add(new System.Web.UI.WebControls.ListItem(current.Name, current.ModeId.ToString()));
							}
						}
						else
						{
							this.rbtnPaymentMode.Items.Add(new System.Web.UI.WebControls.ListItem(current.Name, current.ModeId.ToString()));
						}
					}
				}
				this.rbtnPaymentMode.SelectedIndex = 0;
			}
		}
	}
}
