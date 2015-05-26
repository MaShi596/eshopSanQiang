using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Subsites.Sales;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class ManualPurchaseOrderDetails : DistributorPage
	{
		private string purchaseOrderId;
		private PurchaseOrderInfo purchaseOrder;
		protected System.Web.UI.WebControls.Literal litPurchaseOrderId;
		protected PuchaseStatusLabel lblPurchaseStatus;
		protected System.Web.UI.WebControls.Label lbCloseReason;
		protected System.Web.UI.WebControls.Label lbReason;
		protected System.Web.UI.WebControls.Label lbPurchaseOrderReturn;
		protected FormatedMoneyLabel lblPurchaseOrderRefundMoney;
		protected System.Web.UI.WebControls.HyperLink lkbtnPay;
		protected System.Web.UI.HtmlControls.HtmlAnchor lkbtnClosePurchaseOrder;
		protected ManualPurchaseOrder_Items itemsList;
		protected System.Web.UI.WebControls.HyperLink hlkOrderGifts;
		protected PurchaseOrder_Charges chargesList;
		protected PurchaseOrder_ShippingAddress shippingAddress;
		protected DistributorClosePurchaseOrderReasonDropDownList ddlCloseReason;
		protected System.Web.UI.WebControls.Button btnClosePurchaseOrder;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.Page.Request.QueryString["purchaseOrderId"]))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.purchaseOrderId = this.Page.Request.QueryString["purchaseOrderId"];
			this.btnClosePurchaseOrder.Click += new System.EventHandler(this.btnClosePurchaseOrder_Click);
			this.purchaseOrder = SubsiteSalesHelper.GetPurchaseOrder(this.purchaseOrderId);
			this.LoadUserControl(this.purchaseOrder);
			if (!base.IsPostBack)
			{
				if (this.purchaseOrder.PurchaseStatus == OrderStatus.WaitBuyerPay)
				{
					this.lkbtnClosePurchaseOrder.Visible = true;
					this.lkbtnPay.Visible = true;
					this.lkbtnPay.NavigateUrl = string.Concat(new object[]
					{
						Globals.ApplicationPath,
						"/Shopadmin/purchaseOrder/ChoosePayment.aspx?PurchaseOrderId=",
						this.purchaseOrder.PurchaseOrderId,
						"&PayMode=",
						this.purchaseOrder.PaymentTypeId
					});
				}
				else
				{
					this.lkbtnClosePurchaseOrder.Visible = false;
					this.lkbtnPay.Visible = false;
				}
				this.lblPurchaseStatus.PuchaseStatusCode = this.purchaseOrder.PurchaseStatus;
				this.litPurchaseOrderId.Text = this.purchaseOrder.PurchaseOrderId;
				if ((int)this.lblPurchaseStatus.PuchaseStatusCode != 4)
				{
					this.lbCloseReason.Visible = false;
				}
				else
				{
					this.lbReason.Text = this.purchaseOrder.CloseReason;
				}
				if ((int)this.lblPurchaseStatus.PuchaseStatusCode != 10)
				{
					this.lbPurchaseOrderReturn.Visible = false;
				}
				else
				{
					decimal num;
					this.lblPurchaseOrderRefundMoney.Money = SubsiteSalesHelper.GetRefundMoney(this.purchaseOrder, out num);
				}
				if (this.purchaseOrder.PurchaseStatus == OrderStatus.WaitBuyerPay)
				{
					if (this.purchaseOrder.PurchaseOrderGifts.Count > 0)
					{
						this.hlkOrderGifts.Text = "编辑礼品";
					}
					this.hlkOrderGifts.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/purchaseOrder/PurchaseOrderGifts.aspx?PurchaseOrderId=" + this.purchaseOrder.PurchaseOrderId;
					return;
				}
				this.hlkOrderGifts.Visible = false;
			}
		}
		private void LoadUserControl(PurchaseOrderInfo purchaseOrder)
		{
			this.itemsList.PurchaseOrder = purchaseOrder;
			this.chargesList.PurchaseOrder = purchaseOrder;
			this.shippingAddress.PurchaseOrder = purchaseOrder;
		}
		private void btnClosePurchaseOrder_Click(object sender, System.EventArgs e)
		{
			PurchaseOrderInfo purchaseOrderInfo = this.purchaseOrder;
			purchaseOrderInfo.CloseReason = this.ddlCloseReason.SelectedValue;
			if (SubsiteSalesHelper.ClosePurchaseOrder(purchaseOrderInfo))
			{
				this.ShowMsg("取消采购成功", true);
				return;
			}
			this.ShowMsg("取消采购失败", false);
		}
	}
}
