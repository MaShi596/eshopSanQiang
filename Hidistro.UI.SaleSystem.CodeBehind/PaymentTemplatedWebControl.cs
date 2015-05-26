using Hidistro.AccountCenter.Business;
using Hidistro.Core;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using Hidistro.Messages;
using Hidistro.UI.Common.Controls;
using Hishop.Plugins;
using System;
using System.Collections.Specialized;
using System.Web.UI;
namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true), System.Web.UI.PersistChildren(false)]
	public abstract class PaymentTemplatedWebControl : HtmlTemplatedWebControl
	{
		private readonly bool isBackRequest;
		protected PaymentNotify Notify;
		protected OrderInfo Order;
		protected string OrderId;
		protected decimal Amount;
		protected string Gateway;
		public PaymentTemplatedWebControl(bool _isBackRequest)
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
				this.Notify.ReturnUrl = Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentReturn_url", new object[]
				{
					this.Gateway
				})) + "?" + this.Page.Request.Url.Query;
			}
			this.OrderId = this.Notify.GetOrderId();
			this.Order = TradeHelper.GetOrderInfo(this.OrderId);
			if (this.Order == null)
			{
				this.ResponseStatus(true, "ordernotfound");
			}
			else
			{
				this.Amount = this.Notify.GetOrderAmount();
				if (this.Amount <= 0m)
				{
					this.Amount = this.Order.GetTotal();
				}
				this.Order.GatewayOrderId = this.Notify.GetGatewayOrderId();
				PaymentModeInfo paymentMode = TradeHelper.GetPaymentMode(this.Order.PaymentTypeId);
				if (paymentMode == null)
				{
					this.ResponseStatus(true, "gatewaynotfound");
				}
				else
				{
					this.Notify.Finished += new System.EventHandler<FinishedEventArgs>(this.Notify_Finished);
					this.Notify.NotifyVerifyFaild += new System.EventHandler(this.Notify_NotifyVerifyFaild);
					this.Notify.Payment += new System.EventHandler(this.Notify_Payment);
					this.Notify.VerifyNotify(30000, HiCryptographer.Decrypt(paymentMode.Settings));
				}
			}
		}
		private void Notify_Payment(object sender, System.EventArgs e)
		{
			this.UserPayOrder();
		}
		private void Notify_NotifyVerifyFaild(object sender, System.EventArgs e)
		{
			this.ResponseStatus(false, "verifyfaild");
		}
		private void Notify_Finished(object sender, FinishedEventArgs e)
		{
			if (e.IsMedTrade)
			{
				this.FinishOrder();
			}
			else
			{
				this.UserPayOrder();
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
		private void UserPayOrder()
		{
			if (this.Order.OrderStatus == OrderStatus.BuyerAlreadyPaid)
			{
				this.ResponseStatus(true, "success");
			}
			else
			{
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				if (this.Order.GroupBuyId > 0)
				{
					GroupBuyInfo groupBuy = TradeHelper.GetGroupBuy(this.Order.GroupBuyId);
					if (groupBuy == null || groupBuy.Status != GroupBuyStatus.UnderWay)
					{
						this.ResponseStatus(false, "groupbuyalreadyfinished");
						return;
					}
					num2 = TradeHelper.GetOrderCount(this.Order.GroupBuyId);
					num3 = this.Order.GetGroupBuyOerderNumber();
					num = groupBuy.MaxCount;
					if (num < num2 + num3)
					{
						this.ResponseStatus(false, "exceedordermax");
						return;
					}
				}
				if (this.Order.CheckAction(OrderActions.BUYER_PAY) && TradeHelper.UserPayOrder(this.Order, false))
				{
					TradeHelper.SaveDebitNote(new DebitNote
					{
						NoteId = Globals.GetGenerateId(),
						OrderId = this.Order.OrderId,
						Operator = this.Order.Username,
						Remark = "客户订单在线支付成功"
					});
					if (this.Order.GroupBuyId > 0 && num == num2 + num3)
					{
						TradeHelper.SetGroupBuyEndUntreated(this.Order.GroupBuyId);
					}
					if (this.Order.UserId != 0 && this.Order.UserId != 1100)
					{
						Hidistro.Membership.Core.IUser user = Hidistro.Membership.Context.Users.GetUser(this.Order.UserId);
						if (user != null && (user.UserRole == Hidistro.Membership.Core.Enums.UserRole.Member || user.UserRole == Hidistro.Membership.Core.Enums.UserRole.Underling))
						{
							Messenger.OrderPayment(user, this.Order.OrderId, this.Order.GetTotal());
						}
					}
					this.Order.OnPayment();
					this.ResponseStatus(true, "success");
				}
				else
				{
					this.ResponseStatus(false, "fail");
				}
			}
		}
		private void FinishOrder()
		{
			if (this.Order.OrderStatus == OrderStatus.Finished)
			{
				this.ResponseStatus(true, "success");
			}
			else
			{
				if (this.Order.CheckAction(OrderActions.BUYER_CONFIRM_GOODS) && TradeHelper.ConfirmOrderFinish(this.Order))
				{
					this.ResponseStatus(true, "success");
				}
				else
				{
					this.ResponseStatus(false, "fail");
				}
			}
		}
	}
}
