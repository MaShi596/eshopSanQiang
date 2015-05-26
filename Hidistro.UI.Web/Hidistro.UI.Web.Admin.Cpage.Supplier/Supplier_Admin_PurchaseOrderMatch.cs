using Hidistro.Entities.Sales;
using Hishop.Web.CustomMade;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.Cpage.Supplier
{
	public class Supplier_Admin_PurchaseOrderMatch : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.DataList dlstOrderItems;
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
			if (!this.Page.IsPostBack)
			{
				this.BindData();
			}
		}
		private void BindData()
		{
			this.dlstOrderItems.DataSource = Methods.Supplier_POrderItemsGet(this.purchaseOrder.PurchaseOrderId);
			this.dlstOrderItems.DataBind();
		}
	}
}
