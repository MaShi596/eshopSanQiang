using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Store;
using Hishop.Plugins;
using System;
using System.Collections.Specialized;
using System.Web.UI;
namespace Hidistro.UI.Subsites.Utility
{
	[System.Web.UI.ParseChildren(true), System.Web.UI.PersistChildren(false)]
	public abstract class DistributorInpourReturnBasePage : DistributorPage
	{
		private readonly bool isBackRequest;
		protected PaymentNotify Notify;
		protected string InpourId;
		protected InpourRequestInfo InpourRequest;
		private PaymentModeInfo paymode;
		protected decimal Amount;
		protected string Gateway;
		public DistributorInpourReturnBasePage(bool _isBackRequest)
		{
			this.isBackRequest = _isBackRequest;
		}
		protected override void CreateChildControls()
		{
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
				this.Notify.ReturnUrl = Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("DistributorInpourReturn_url", new object[]
				{
					this.Gateway
				}));
				PaymentNotify expr_AC = this.Notify;
				expr_AC.ReturnUrl = expr_AC.ReturnUrl + "?" + this.Page.Request.Url.Query;
			}
			this.InpourId = this.Notify.GetOrderId();
			this.Amount = this.Notify.GetOrderAmount();
			this.InpourRequest = SubsiteStoreHelper.GetInpouRequest(this.InpourId);
			if (this.InpourRequest == null)
			{
				this.ResponseStatus(true, "success");
			}
			else
			{
				this.Amount = this.InpourRequest.InpourBlance;
				this.paymode = SubsiteStoreHelper.GetPaymentMode(this.InpourRequest.PaymentId);
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
			Hidistro.Membership.Context.Distributor distributor = Hidistro.Membership.Context.Users.GetUser(this.InpourRequest.UserId, false) as Hidistro.Membership.Context.Distributor;
			decimal balance = distributor.Balance + this.InpourRequest.InpourBlance;
			BalanceDetailInfo balanceDetailInfo = new BalanceDetailInfo();
			balanceDetailInfo.UserId = this.InpourRequest.UserId;
			balanceDetailInfo.UserName = distributor.Username;
			balanceDetailInfo.TradeDate = now;
			balanceDetailInfo.TradeType = TradeTypes.SelfhelpInpour;
			balanceDetailInfo.Income = new decimal?(this.InpourRequest.InpourBlance);
			balanceDetailInfo.Balance = balance;
			balanceDetailInfo.InpourId = this.InpourRequest.InpourId;
			if (this.paymode != null)
			{
				balanceDetailInfo.Remark = "充值支付方式：" + this.paymode.Name;
			}
			if (SubsiteStoreHelper.Recharge(balanceDetailInfo))
			{
				Hidistro.Membership.Context.Users.ClearUserCache(distributor);
				this.ResponseStatus(true, "success");
			}
			else
			{
				SubsiteStoreHelper.RemoveInpourRequest(this.InpourId);
				this.ResponseStatus(false, "fail");
			}
		}
		protected abstract void DisplayMessage(string status);
		private void ResponseStatus(bool success, string status)
		{
			if (this.isBackRequest)
			{
				this.Notify.WriteBack(this.Context, success);
			}
			else
			{
				this.DisplayMessage(status);
			}
		}
	}
}
