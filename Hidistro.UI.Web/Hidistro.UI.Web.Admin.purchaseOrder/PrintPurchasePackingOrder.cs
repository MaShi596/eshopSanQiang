using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.Entities.Sales;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.purchaseOrder
{
	public class PrintPurchasePackingOrder : AdminPage
	{
		private string purchaseOrderId = string.Empty;
		protected PageTitle PageTitle1;
		protected Script Script2;
		protected System.Web.UI.HtmlControls.HtmlForm form1;
		protected System.Web.UI.WebControls.Literal litOrderDate;
		protected System.Web.UI.WebControls.Literal litOrderId;
		protected System.Web.UI.WebControls.Literal litShipperMode;
		protected System.Web.UI.WebControls.Literal litPayType;
		protected System.Web.UI.WebControls.Literal litShippNo;
		protected System.Web.UI.WebControls.Literal litSkipTo;
		protected System.Web.UI.WebControls.Literal litAddress;
		protected System.Web.UI.WebControls.Literal litZipCode;
		protected System.Web.UI.WebControls.Literal litCellPhone;
		protected System.Web.UI.WebControls.Literal litTelPhone;
		protected System.Web.UI.WebControls.Literal litRemark;
		protected System.Web.UI.WebControls.Literal litOrderStatus;
		protected Grid grdOrderItems;
		protected Grid grdOrderGifts;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.purchaseOrderId = this.Page.Request.Params["PurchaseOrderId"];
			if (!this.Page.IsPostBack)
			{
				if (string.IsNullOrEmpty(this.purchaseOrderId))
				{
					return;
				}
				PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(this.purchaseOrderId);
				this.BindOrderInfo(purchaseOrder);
				this.BindOrderItems(purchaseOrder);
			}
		}
		private void BindOrderItems(PurchaseOrderInfo order)
		{
			this.grdOrderItems.DataSource = order.PurchaseOrderItems;
			this.grdOrderItems.DataBind();
			if (order.PurchaseOrderGifts.Count > 0)
			{
				this.grdOrderGifts.DataSource = order.PurchaseOrderGifts;
				this.grdOrderGifts.DataBind();
				return;
			}
			this.grdOrderGifts.Visible = false;
		}
		private void BindOrderInfo(PurchaseOrderInfo order)
		{
			this.litAddress.Text = order.ShippingRegion + order.Address;
			this.litCellPhone.Text = order.CellPhone;
			this.litTelPhone.Text = order.TelPhone;
			this.litZipCode.Text = order.ZipCode;
			this.litOrderId.Text = order.OrderId;
			this.litOrderDate.Text = order.PurchaseDate.ToString();
			this.litRemark.Text = order.Remark;
			this.litShipperMode.Text = order.RealModeName;
			this.litShippNo.Text = order.ShipOrderNumber;
			this.litSkipTo.Text = order.ShipTo;
			switch (order.PurchaseStatus)
			{
			case OrderStatus.WaitBuyerPay:
				this.litOrderStatus.Text = "等待付款";
				return;
			case OrderStatus.BuyerAlreadyPaid:
				this.litOrderStatus.Text = "已付款等待发货";
				return;
			case OrderStatus.SellerAlreadySent:
				this.litOrderStatus.Text = "已发货";
				return;
			case OrderStatus.Closed:
				this.litOrderStatus.Text = "已关闭";
				return;
			case OrderStatus.Finished:
				this.litOrderStatus.Text = "已完成";
				return;
			default:
				return;
			}
		}
	}
}
