using Hidistro.Core;
using Hidistro.Entities.Sales;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class PurchaseOrder_Charges : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Literal litFreight;
		protected System.Web.UI.WebControls.Literal lblModeName;
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
			if (this.purchaseOrder.PurchaseStatus != OrderStatus.Finished)
			{
				if (this.purchaseOrder.PurchaseStatus != OrderStatus.SellerAlreadySent)
				{
					this.lblModeName.Text = this.purchaseOrder.ModeName;
					goto IL_4C;
				}
			}
			this.lblModeName.Text = this.purchaseOrder.RealModeName;
			IL_4C:
			this.litFreight.Text = Globals.FormatMoney(this.purchaseOrder.AdjustedFreight);
			this.litDiscount.Text = this.purchaseOrder.AdjustedDiscount.ToString();
			this.litTotalPrice.Text = Globals.FormatMoney(this.purchaseOrder.GetPurchaseTotal());
			if (this.purchaseOrder.Tax > 0m)
			{
				this.litTax.Text = "<tr class=\"bg\"><td align=\"right\">税金(元)：</td><td colspan=\"2\"><span class='Name'>" + Globals.FormatMoney(this.purchaseOrder.Tax);
				System.Web.UI.WebControls.Literal expr_E3 = this.litTax;
				expr_E3.Text += "</span></td></tr>";
			}
			if (!string.IsNullOrEmpty(this.purchaseOrder.InvoiceTitle))
			{
				this.litInvoiceTitle.Text = "<tr class=\"bg\"><td align=\"right\">发票抬头：</td><td colspan=\"2\"><span class='Name'>" + this.purchaseOrder.InvoiceTitle;
				System.Web.UI.WebControls.Literal expr_130 = this.litInvoiceTitle;
				expr_130.Text += "</span></td></tr>";
			}
		}
	}
}
