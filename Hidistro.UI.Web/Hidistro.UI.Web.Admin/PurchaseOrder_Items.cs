using Hidistro.Entities.Sales;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	public class PurchaseOrder_Items : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.DataList dlstOrderItems;
		protected PurchaseOrderItemUpdateHyperLink purchaseOrderItemUpdateHyperLink;
		protected FormatedMoneyLabel lblGoodsAmount;
		protected System.Web.UI.WebControls.Literal lblWeight;
		protected System.Web.UI.HtmlControls.HtmlGenericControl giftsList;
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
			if (this.purchaseOrder.PurchaseOrderGifts.Count <= 0)
			{
				this.giftsList.Visible = false;
			}
			else
			{
				this.grdOrderGift.DataSource = this.purchaseOrder.PurchaseOrderGifts;
				this.grdOrderGift.DataBind();
			}
			this.lblGoodsAmount.Money = this.purchaseOrder.GetProductAmount();
			this.lblWeight.Text = this.purchaseOrder.Weight.ToString(System.Globalization.CultureInfo.InvariantCulture);
			this.purchaseOrderItemUpdateHyperLink.PurchaseOrderId = this.purchaseOrder.PurchaseOrderId;
			this.purchaseOrderItemUpdateHyperLink.PurchaseStatusCode = this.purchaseOrder.PurchaseStatus;
			this.purchaseOrderItemUpdateHyperLink.DistorUserId = this.purchaseOrder.DistributorId;
			this.purchaseOrderItemUpdateHyperLink.DataBind();
		}
	}
}
