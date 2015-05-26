using Hidistro.Entities.Sales;
using Hishop.Web.CustomMade;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	public class Supplier_Admin_OrderMatchItemsList : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.DataList dlstOrderItems;
		private OrderInfo order;
		public OrderInfo Order
		{
			get
			{
				return this.order;
			}
			set
			{
				this.order = value;
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
			this.dlstOrderItems.DataSource = Methods.Supplier_OrderItemsGet(this.order.OrderId);
			this.dlstOrderItems.DataBind();
		}
	}
}
