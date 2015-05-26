using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Sales;
using Hidistro.Subsites.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using Hishop.Plugins;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class Pay : DistributorPage
	{
		protected System.Web.UI.HtmlControls.HtmlGenericControl abbrInfo;
		protected System.Web.UI.WebControls.Literal litorder;
		protected System.Web.UI.WebControls.Literal litOrderId;
		protected System.Web.UI.WebControls.Literal litPurchaseOrderId;
		protected FormatedTimeLabel lblPurchaseDate;
		protected FormatedMoneyLabel lblUseableBalance;
		protected FormatedMoneyLabel lblTotalPrice;
		protected System.Web.UI.WebControls.TextBox txtTradePassword;
		protected System.Web.UI.WebControls.Button btnConfirmPay;
		protected System.Web.UI.WebControls.Button btnBack1;
		protected System.Web.UI.HtmlControls.HtmlGenericControl PaySucceess;
		protected System.Web.UI.WebControls.ImageButton imgBtnBack;
		protected System.Web.UI.WebControls.Button btnBack;
		private string purchaseOrderId;
		private PurchaseOrderInfo purchaseOrder;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnConfirmPay.Click += new System.EventHandler(this.btnConfirmPay_Click);
			this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
			this.btnBack1.Click += new System.EventHandler(this.btnBack_Click);
			this.imgBtnBack.Click += new System.Web.UI.ImageClickEventHandler(this.btnBack_Click);
			if (string.IsNullOrEmpty(base.Request.QueryString["PurchaseOrderId"]))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.purchaseOrderId = base.Request.QueryString["PurchaseOrderId"];
			this.purchaseOrder = SubsiteSalesHelper.GetPurchaseOrder(this.purchaseOrderId);
			if (!base.IsPostBack)
			{
				if (this.purchaseOrder == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				int num;
				int.TryParse(base.Request["PayMode"], out num);
				PaymentModeInfo paymentMode = SubsiteStoreHelper.GetPaymentMode(num);
				if (num > 0 && paymentMode.Gateway != "hishop.plugins.payment.advancerequest")
				{
					SubsiteStoreHelper.GetDistributor();
					string showUrl = Globals.FullPath(Globals.GetSiteUrls().Home);
					if (paymentMode.Gateway.ToLower() != "hishop.plugins.payment.podrequest")
					{
						showUrl = base.Server.UrlEncode(string.Format("http://{0}/shopadmin/purchaseorder/MyPurchaseOrderDetails.aspx?purchaseOrderId={1}", base.Request.Url.Host, this.purchaseOrder.PurchaseOrderId));
					}
					if (string.Compare(paymentMode.Gateway, "Hishop.Plugins.Payment.BankRequest", true) == 0)
					{
						showUrl = Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("bank_pay", new object[]
						{
							this.purchaseOrder.PurchaseOrderId
						}));
					}
					if (string.Compare(paymentMode.Gateway, "Hishop.Plugins.Payment.AdvanceRequest", true) == 0)
					{
						showUrl = Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("advance_pay", new object[]
						{
							this.purchaseOrder.PurchaseOrderId
						}));
					}
					string attach = "";
					System.Web.HttpCookie httpCookie = Hidistro.Membership.Context.HiContext.Current.Context.Request.Cookies["Token_" + Hidistro.Membership.Context.HiContext.Current.User.UserId.ToString()];
					if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
					{
						attach = httpCookie.Value;
					}
					PaymentRequest paymentRequest = PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), this.purchaseOrder.PurchaseOrderId, this.purchaseOrder.GetPurchaseTotal(), "采购单支付", "采购单号-" + this.purchaseOrder.PurchaseOrderId, "", this.purchaseOrder.PurchaseDate, showUrl, Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("DistributorPaymentNotify_url", new object[]
					{
						paymentMode.Gateway
					})), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("DistributorPaymentNotify_url", new object[]
					{
						paymentMode.Gateway
					})), attach);
					paymentRequest.SendRequest();
				}
				if (this.purchaseOrder.IsManualPurchaseOrder)
				{
					this.litorder.Visible = false;
					this.litOrderId.Visible = false;
				}
				else
				{
					this.litOrderId.Text = this.purchaseOrder.OrderId;
				}
				this.litPurchaseOrderId.Text = this.purchaseOrder.PurchaseOrderId;
				this.lblPurchaseDate.Time = this.purchaseOrder.PurchaseDate;
				this.lblTotalPrice.Money = this.purchaseOrder.GetPurchaseTotal();
				AccountSummaryInfo myAccountSummary = SubsiteStoreHelper.GetMyAccountSummary();
				this.lblUseableBalance.Money = myAccountSummary.UseableBalance;
			}
		}
		private void btnConfirmPay_Click(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.txtTradePassword.Text))
			{
				this.ShowMsg("请输入交易密码", false);
				return;
			}
			int modeId;
			int.TryParse(base.Request["PayMode"], out modeId);
			SubsiteStoreHelper.GetPaymentMode(modeId);
			if ((decimal)this.lblUseableBalance.Money < (decimal)this.lblTotalPrice.Money)
			{
				this.ShowMsg("您的预付款金额不足", false);
				return;
			}
			Hidistro.Membership.Context.Distributor distributor = SubsiteStoreHelper.GetDistributor();
			if (distributor.Balance - distributor.RequestBalance < this.purchaseOrder.GetPurchaseTotal())
			{
				this.ShowMsg("您的预付款金额不足", false);
				return;
			}
			BalanceDetailInfo balanceDetailInfo = new BalanceDetailInfo();
			balanceDetailInfo.UserId = distributor.UserId;
			balanceDetailInfo.UserName = distributor.Username;
			balanceDetailInfo.TradeType = TradeTypes.Consume;
			balanceDetailInfo.TradeDate = System.DateTime.Now;
			balanceDetailInfo.Expenses = new decimal?(this.purchaseOrder.GetPurchaseTotal());
			balanceDetailInfo.Balance = distributor.Balance - this.purchaseOrder.GetPurchaseTotal();
			balanceDetailInfo.Remark = string.Format("采购单{0}的付款", this.purchaseOrder.PurchaseOrderId);
			distributor.TradePassword = this.txtTradePassword.Text;
			if (!Hidistro.Membership.Context.Users.ValidTradePassword(distributor))
			{
				this.ShowMsg("交易密码错误", false);
				return;
			}
			if (!SubsiteSalesHelper.ConfirmPay(balanceDetailInfo, this.purchaseOrder))
			{
				this.ShowMsg("付款失败", false);
				return;
			}
			SubsiteSalesHelper.SavePurchaseDebitNote(new PurchaseDebitNote
			{
				NoteId = Globals.GetGenerateId(),
				PurchaseOrderId = this.purchaseOrderId,
				Operator = Hidistro.Membership.Context.HiContext.Current.User.Username,
				Remark = "分销商采购单预付款支付成功"
			});
			this.ShowMsg("采购单预付款支付成功", true);
		}
		private void btnBack_Click(object sender, System.EventArgs e)
		{
			if (!this.purchaseOrder.IsManualPurchaseOrder)
			{
				base.Response.Redirect("MyPurchaseOrderDetails.aspx?PurchaseOrderId=" + this.purchaseOrderId);
				return;
			}
			base.Response.Redirect("ManualPurchaseOrderDetails.aspx?PurchaseOrderId=" + this.purchaseOrderId);
		}
	}
}
