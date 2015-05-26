using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Subsites.Sales;
using Hidistro.Subsites.Store;
using Hidistro.UI.Subsites.Utility;
using Hishop.Plugins;
using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Pay
{
	public class DistributorPaymentNotify_url : DistributorPage
	{
		private readonly bool isBackRequest;
		private PaymentNotify Notify;
		private PurchaseOrderInfo PurchaseOrder;
		protected string PurchaseOrderId;
		protected decimal Amount;
		protected string Gateway;
		protected System.Web.UI.WebControls.Literal litMessage;
		public DistributorPaymentNotify_url()
		{
			this.isBackRequest = false;
		}
		public DistributorPaymentNotify_url(bool _isBackRequest)
		{
			this.isBackRequest = _isBackRequest;
		}
		private void Notify_Payment(object sender, System.EventArgs e)
		{
			this.UserPayOrder();
		}
		private void UserPayOrder()
		{
			if (this.PurchaseOrder.PurchaseStatus == OrderStatus.BuyerAlreadyPaid)
			{
				this.ResponseStatus(true, "success");
				return;
			}
			if (this.PurchaseOrder.CheckAction(PurchaseOrderActions.DISTRIBUTOR_CONFIRM_PAY) && SubsiteSalesHelper.ConfirmPay(this.PurchaseOrder))
			{
				SubsiteSalesHelper.SavePurchaseDebitNote(new PurchaseDebitNote
				{
					NoteId = Globals.GetGenerateId(),
					PurchaseOrderId = this.PurchaseOrder.PurchaseOrderId,
					Operator = this.PurchaseOrder.Distributorname,
					Remark = "分销商采购单在线支付成功"
				});
				this.ResponseStatus(true, "success");
			}
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
				return;
			}
			this.UserPayOrder();
		}
		private void FinishOrder()
		{
			if (this.PurchaseOrder.PurchaseStatus == OrderStatus.Finished)
			{
				this.ResponseStatus(true, "success");
				return;
			}
			if (this.PurchaseOrder.CheckAction(PurchaseOrderActions.MASTER_FINISH_TRADE) && SubsiteSalesHelper.ConfirmPurchaseOrderFinish(this.PurchaseOrder))
			{
				this.ResponseStatus(true, "success");
				return;
			}
			this.ResponseStatus(false, "fail");
		}
		protected void DisplayMessage(string status)
		{
			switch (status)
			{
			case "ordernotfound":
				this.litMessage.Text = string.Format("没有找到对应的采购单信息，采购单号：{0}", this.PurchaseOrderId);
				return;
			case "gatewaynotfound":
				this.litMessage.Text = "没有找到与此采购单对应的支付方式，系统无法自动完成操作，请联系管理员";
				return;
			case "verifyfaild":
				this.litMessage.Text = "支付返回验证失败，操作已停止";
				return;
			case "success":
				this.litMessage.Text = string.Format("恭喜您，采购单已成功完成支付：{0}</br>支付金额：{1}", this.PurchaseOrderId, this.Amount.ToString("F"));
				return;
			case "exceedordermax":
				this.litMessage.Text = "订单为团购订单，订购数量超过订购总数，支付失败";
				return;
			case "groupbuyalreadyfinished":
				this.litMessage.Text = "订单为团购订单，团购活动已结束，支付失败";
				return;
			case "fail":
				this.litMessage.Text = string.Format("订单支付已成功，但是系统在处理过程中遇到问题，请联系管理员</br>支付金额：{0}", this.Amount.ToString("F"));
				return;
			}
			this.litMessage.Text = "未知错误，操作已停止";
		}
		private void ResponseStatus(bool success, string status)
		{
			this.DisplayMessage(status);
		}
		protected void Page_Load(object sender, System.EventArgs e)
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
				this.Notify.ReturnUrl = Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("DistributorPaymentReturn_url", new object[]
				{
					this.Gateway
				})) + "?" + this.Page.Request.Url.Query;
			}
			this.PurchaseOrderId = this.Notify.GetOrderId();
			this.PurchaseOrder = SubsiteSalesHelper.GetPurchaseOrder(this.PurchaseOrderId);
			if (this.PurchaseOrder == null)
			{
				this.ResponseStatus(true, "purchaseordernotfound");
				return;
			}
			this.Amount = this.Notify.GetOrderAmount();
			if (this.Amount <= 0m)
			{
				this.Amount = this.PurchaseOrder.GetPurchaseTotal();
			}
			PaymentModeInfo paymentMode = SubsiteStoreHelper.GetPaymentMode(this.PurchaseOrder.PaymentTypeId);
			if (paymentMode == null)
			{
				this.ResponseStatus(true, "gatewaynotfound");
				return;
			}
			this.Notify.Finished += new System.EventHandler<FinishedEventArgs>(this.Notify_Finished);
			this.Notify.NotifyVerifyFaild += new System.EventHandler(this.Notify_NotifyVerifyFaild);
			this.Notify.Payment += new System.EventHandler(this.Notify_Payment);
			this.Notify.VerifyNotify(30000, HiCryptographer.Decrypt(paymentMode.Settings));
		}
	}
}
