using Hidistro.AccountCenter.Business;
using Hidistro.AccountCenter.Profile;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hishop.Plugins;
using System;
using System.Collections.Specialized;
using System.Web.UI;
namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true), System.Web.UI.PersistChildren(false)]
	public abstract class InpourTemplatedWebControl : HtmlTemplatedWebControl
	{
		private readonly bool isBackRequest;
		protected PaymentNotify Notify;
		protected string InpourId;
		protected InpourRequestInfo InpourRequest;
		protected PaymentModeInfo paymode;
		protected decimal Amount;
		protected string Gateway;
		public InpourTemplatedWebControl(bool _isBackRequest)
		{
			this.isBackRequest = _isBackRequest;
		}
		protected override void CreateChildControls()
		{
			this.Controls.Clear();
			if (!this.isBackRequest)
			{
				if (!base.LoadHtmlThemedControl())
				{
					throw new SkinNotFoundException(this.SkinPath);
				}
				this.AttachChildControls();
			}
			this.DoValidate();
		}
		private void DoValidate()
		{
			System.Collections.Specialized.NameValueCollection parameters = new System.Collections.Specialized.NameValueCollection
			{
				this.Page.Request.Form,
				this.Page.Request.QueryString
			};
			this.Gateway = this.Page.Request.QueryString["HIGW"];
			this.Notify = PaymentNotify.CreateInstance(this.Gateway, parameters);
			if (this.isBackRequest)
			{
				this.Notify.ReturnUrl = Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("InpourReturn_url", new object[]
				{
					this.Gateway
				})) + "?" + this.Page.Request.Url.Query;
			}
			this.InpourId = this.Notify.GetOrderId();
			this.Amount = this.Notify.GetOrderAmount();
			this.InpourRequest = PersonalHelper.GetInpourBlance(this.InpourId);
			if (this.InpourRequest == null)
			{
				this.ResponseStatus(true, "success");
			}
			else
			{
				this.Amount = this.InpourRequest.InpourBlance;
				this.paymode = TradeHelper.GetPaymentMode(this.InpourRequest.PaymentId);
				if (this.paymode == null)
				{
					this.ResponseStatus(true, "gatewaynotfound");
				}
				else
				{
					this.Notify.Finished += new System.EventHandler<FinishedEventArgs>(this.Notify_Finished);
					this.Notify.NotifyVerifyFaild += new System.EventHandler(this.Notify_NotifyVerifyFaild);
					this.Notify.Payment += new System.EventHandler(this.Notify_Payment);
					this.Notify.VerifyNotify(30000, HiCryptographer.Decrypt(this.paymode.Settings));
				}
			}
		}
		private void Notify_Payment(object sender, System.EventArgs e)
		{
			this.ResponseStatus(false, "waitconfirm");
		}
		private void Notify_NotifyVerifyFaild(object sender, System.EventArgs e)
		{
			this.ResponseStatus(false, "verifyfaild");
		}
		private void Notify_Finished(object sender, FinishedEventArgs e)
		{
			System.DateTime now = System.DateTime.Now;
			Hidistro.Membership.Context.Member member = Hidistro.Membership.Context.Users.GetUser(this.InpourRequest.UserId, false) as Hidistro.Membership.Context.Member;
			decimal balance = member.Balance + this.InpourRequest.InpourBlance;
			BalanceDetailInfo balanceDetailInfo = new BalanceDetailInfo();
			balanceDetailInfo.UserId = this.InpourRequest.UserId;
			balanceDetailInfo.UserName = member.Username;
			balanceDetailInfo.TradeDate = now;
			balanceDetailInfo.TradeType = TradeTypes.SelfhelpInpour;
			balanceDetailInfo.Income = new decimal?(this.InpourRequest.InpourBlance);
			balanceDetailInfo.Balance = balance;
			balanceDetailInfo.InpourId = this.InpourRequest.InpourId;
			if (this.paymode != null)
			{
				balanceDetailInfo.Remark = "充值支付方式：" + this.paymode.Name;
			}
			if (PersonalHelper.Recharge(balanceDetailInfo))
			{
				Hidistro.Membership.Context.Users.ClearUserCache(member);
				this.ResponseStatus(true, "success");
			}
			else
			{
				PersonalHelper.RemoveInpourRequest(this.InpourId);
				this.ResponseStatus(false, "fail");
			}
		}
		protected abstract void DisplayMessage(string status);
		private void ResponseStatus(bool success, string status)
		{
			if (this.isBackRequest)
			{
				this.Notify.WriteBack(Hidistro.Membership.Context.HiContext.Current.Context, success);
			}
			else
			{
				this.DisplayMessage(status);
			}
		}
	}
}
