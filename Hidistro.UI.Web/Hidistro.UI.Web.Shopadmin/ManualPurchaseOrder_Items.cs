using Hidistro.Entities.Sales;
using Hidistro.UI.Common.Controls;
using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class ManualPurchaseOrder_Items : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.DataList dlstOrderItems;
		protected FormatedMoneyLabel lblGoodsAmount;
		protected System.Web.UI.WebControls.Literal lblWeight;
		protected System.Web.UI.WebControls.DataList grdOrderGift;
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
			this.dlstOrderItems.DataSource = this.purchaseOrder.PurchaseOrderItems;
			this.dlstOrderItems.DataBind();
			this.grdOrderGift.DataSource = this.purchaseOrder.PurchaseOrderGifts;
			this.grdOrderGift.DataBind();
			this.lblGoodsAmount.Money = this.purchaseOrder.GetProductAmount();
			this.lblWeight.Text = this.purchaseOrder.Weight.ToString(System.Globalization.CultureInfo.InvariantCulture);
		}
	}
}
