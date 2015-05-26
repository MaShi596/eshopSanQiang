using Hidistro.ControlPanel.Sales;
using Hidistro.Entities.Sales;
using Hishop.Web.CustomMade;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.Cpage.Supplier
{
	public class ShipPurchaseOrdersMatch : System.Web.UI.Page
	{
		protected System.Web.UI.HtmlControls.HtmlHead Head1;
		protected System.Web.UI.HtmlControls.HtmlForm form1;
		protected System.Web.UI.WebControls.Literal txtShipPointNameAuto;
		protected System.Web.UI.WebControls.Literal txtShipPointNameAuto2;
		protected Supplier_Admin_PurchaseOrderMatch itemsList;
		private string purchaseorderId;
		private PurchaseOrderInfo purchaseorder;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!base.IsPostBack)
			{
				if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["PurchaseOrderId"]))
				{
					this.purchaseorderId = System.Web.HttpContext.Current.Request.QueryString["PurchaseOrderId"];
				}
				if (this.purchaseorderId == string.Empty)
				{
					return;
				}
				this.purchaseorderId = this.Page.Request.QueryString["PurchaseOrderId"];
				this.purchaseorder = SalesHelper.GetPurchaseOrder(this.purchaseorderId);
				this.BindOrderItems(this.purchaseorder);
				string[] array = this.purchaseorder.ShippingRegion.Split("，".ToCharArray());
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
					string text = "<b>" + (string)dataTable.Rows[0]["username"] + "</b> " + (string)dataTable.Rows[0]["Supplier_RegionName"];
					this.txtShipPointNameAuto.Text = text;
					if (dataTable.Rows[0]["comment"] != System.DBNull.Value)
					{
						this.txtShipPointNameAuto2.Text = (string)dataTable.Rows[0]["comment"];
					}
				}
			}
		}
		private void BindOrderItems(PurchaseOrderInfo order)
		{
			this.itemsList.PurchaseOrder = this.purchaseorder;
		}
	}
}
