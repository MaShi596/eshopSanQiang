using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	public class MondifyAddressFrame : AdminPage
	{
		private string action = "";
		private string purchaseOrderId;
		protected PurchaseOrderInfo purchaseOrder;
		protected System.Web.UI.WebControls.TextBox txtShipTo;
		protected RegionSelector dropRegions;
		protected System.Web.UI.WebControls.TextBox txtAddress;
		protected System.Web.UI.WebControls.TextBox txtZipcode;
		protected System.Web.UI.WebControls.TextBox txtTelPhone;
		protected System.Web.UI.WebControls.TextBox txtCellPhone;
		protected System.Web.UI.WebControls.Button btnMondifyAddress;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.Page.Request.QueryString["action"]))
			{
				base.GotoResourceNotFound();
				return;
			}
			if (string.IsNullOrEmpty(this.Page.Request.QueryString["PurchaseOrderId"]))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.purchaseOrderId = this.Page.Request.QueryString["PurchaseOrderId"];
			this.purchaseOrder = SalesHelper.GetPurchaseOrder(this.purchaseOrderId);
			this.action = this.Page.Request.QueryString["action"];
			if (this.action == "update" && !base.IsPostBack)
			{
				this.BindUpdateSippingAddress();
			}
			this.btnMondifyAddress.Click += new System.EventHandler(this.btnMondifyAddress_Click);
		}
		private void BindUpdateSippingAddress()
		{
			this.txtShipTo.Text = Globals.HtmlDecode(this.purchaseOrder.ShipTo);
			this.dropRegions.SetSelectedRegionId(new int?(this.purchaseOrder.RegionId));
			this.txtAddress.Text = Globals.HtmlDecode(this.purchaseOrder.Address);
			this.txtZipcode.Text = this.purchaseOrder.ZipCode;
			this.txtTelPhone.Text = this.purchaseOrder.TelPhone;
			this.txtCellPhone.Text = this.purchaseOrder.CellPhone;
		}
		protected void btnMondifyAddress_Click(object sender, System.EventArgs e)
		{
			this.purchaseOrder.ShipTo = Globals.HtmlEncode(this.txtShipTo.Text.Trim());
			if (!this.dropRegions.GetSelectedRegionId().HasValue)
			{
				this.ShowMsg("收货人地址必选", false);
				return;
			}
			this.purchaseOrder.RegionId = this.dropRegions.GetSelectedRegionId().Value;
			this.purchaseOrder.Address = Globals.HtmlEncode(this.txtAddress.Text.Trim());
			this.purchaseOrder.TelPhone = this.txtTelPhone.Text.Trim();
			this.purchaseOrder.CellPhone = this.txtCellPhone.Text.Trim();
			this.purchaseOrder.ZipCode = this.txtZipcode.Text.Trim();
			this.purchaseOrder.ShippingRegion = this.dropRegions.SelectedRegions;
			if (SalesHelper.SavePurchaseOrderShippingAddress(this.purchaseOrder))
			{
				this.ShowMsg("修改成功", true);
				return;
			}
			this.ShowMsg("修改失败", false);
		}
	}
}
