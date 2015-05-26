using Hidistro.ControlPanel.Sales;
using Hidistro.Entities.Sales;
using Hishop.Web.CustomMade;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	public class ShipOrdersMatch : System.Web.UI.Page
	{
		private string orderId;
		private OrderInfo order;
		protected System.Web.UI.HtmlControls.HtmlHead Head1;
		protected System.Web.UI.HtmlControls.HtmlForm form1;
		protected System.Web.UI.WebControls.Literal txtShipPointNameAuto;
		protected System.Web.UI.WebControls.Literal txtShipPointNameAuto2;
		protected Supplier_Admin_OrderMatchItemsList itemsList;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!base.IsPostBack)
			{
				if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["OrderId"]))
				{
					this.orderId = System.Web.HttpContext.Current.Request.QueryString["OrderId"];
				}
				if (this.orderId == string.Empty)
				{
					return;
				}
				this.order = OrderHelper.GetOrderInfo(this.orderId);
				this.BindOrderItems(this.order);
				string[] array = this.order.ShippingRegion.Split("，".ToCharArray());
				System.Data.DataTable dataTable = null;
				if (array.Length == 3)
				{
					dataTable = Methods.Supplier_ShipPointGetByRegionName(array[0], array[1], array[2]);
					if (dataTable == null || dataTable.Rows.Count == 0)
					{
						dataTable = Methods.Supplier_ShipPointGetByRegionName(array[0], array[1], "");
					}
					if (dataTable == null || dataTable.Rows.Count == 0)
					{
						dataTable = Methods.Supplier_ShipPointGetByRegionName(array[0], "", "");
					}
				}
				if (array.Length == 2)
				{
					dataTable = Methods.Supplier_ShipPointGetByRegionName(array[0], array[1], "");
					if (dataTable == null || dataTable.Rows.Count == 0)
					{
						dataTable = Methods.Supplier_ShipPointGetByRegionName(array[0]);
					}
				}
				if (array.Length == 1)
				{
					dataTable = Methods.Supplier_ShipPointGetByRegionName(array[0], "", "");
				}
				if (dataTable != null && dataTable.Rows.Count > 0)
				{
					dataTable = Methods.Supplier_ShipPointGetByUserId(dataTable.Rows[0]["UserId"].ToString());
					if (dataTable != null && dataTable.Rows.Count > 0)
					{
						string text = "<b>" + (string)dataTable.Rows[0]["username"] + "</b> " + (string)dataTable.Rows[0]["Supplier_RegionName"];
						this.txtShipPointNameAuto.Text = text;
						if (dataTable.Rows[0]["comment"] != System.DBNull.Value)
						{
							this.txtShipPointNameAuto2.Text = (string)dataTable.Rows[0]["comment"];
						}
					}
				}
			}
		}
		private void BindOrderItems(OrderInfo order)
		{
			this.itemsList.Order = order;
		}
	}
}
