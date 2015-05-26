using Hidistro.Core;
using Hidistro.Entities.Sales;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	public class PurchaseOrder_Charges : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Literal litFreight;
		protected System.Web.UI.WebControls.Literal lblModeName;
		protected System.Web.UI.HtmlControls.HtmlAnchor lkBtnEditshipingMode;
		protected System.Web.UI.WebControls.Literal litDiscount;
		protected System.Web.UI.WebControls.Literal litTax;
		protected System.Web.UI.WebControls.Literal litInvoiceTitle;
		protected System.Web.UI.WebControls.Literal litTotalPrice;
		private PurchaseOrderInfo purchaseOrder;
		public PurchaseOrderInfo PurchaseOrder
		{
			get
			{
				return this.purchaseOrder;
			}
			set
			{
				this.purchaseOrder = value;
			}
		}
		protected override void OnLoad(System.EventArgs e)
		{
			this.LoadControl();
		}
		public void LoadControl()
		{
			if (this.purchaseOrder.PurchaseStatus == OrderStatus.WaitBuyerPay || this.purchaseOrder.PurchaseStatus == OrderStatus.BuyerAlreadyPaid)
			{
				this.lkBtnEditshipingMode.Visible = true;
			}
			if (this.purchaseOrder.PurchaseStatus != OrderStatus.Finished)
			{
				if (this.purchaseOrder.PurchaseStatus != OrderStatus.SellerAlreadySent)
				{
					this.lblModeName.Text = this.purchaseOrder.ModeName;
					goto IL_74;
				}
			}
			this.lblModeName.Text = this.purchaseOrder.RealModeName;
			IL_74:
			this.litFreight.Text = Globals.FormatMoney(this.purchaseOrder.AdjustedFreight);
			this.litDiscount.Text = Globals.FormatMoney(this.purchaseOrder.AdjustedDiscount);
			this.litTotalPrice.Text = Globals.FormatMoney(this.purchaseOrder.GetPurchaseTotal());
			if (this.purchaseOrder.Tax > 0m)
			{
				this.litTax.Text = "<tr class=\"bg\"><td align=\"right\">税金(元)：</td><td colspan=\"2\"><span class='Name'>" + Globals.FormatMoney(this.purchaseOrder.Tax);
				System.Web.UI.WebControls.Literal expr_108 = this.litTax;
				expr_108.Text += "</span></td></tr>";
			}
			if (!string.IsNullOrEmpty(this.purchaseOrder.InvoiceTitle))
			{
				this.litInvoiceTitle.Text = "<tr class=\"bg\"><td align=\"right\">发票抬头：</td><td colspan=\"2\"><span class='Name'>" + this.purchaseOrder.InvoiceTitle;
				System.Web.UI.WebControls.Literal expr_155 = this.litInvoiceTitle;
				expr_155.Text += "</span></td></tr>";
			}
		}
	}
}
