using Hidistro.ControlPanel.Sales;
using Hidistro.Entities.Sales;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.sales
{
	public class BatchPrintData : AdminPage
	{
		protected static string orderIds = string.Empty;
		protected System.Web.UI.WebControls.Panel pnlTask;
		protected System.Web.UI.WebControls.Literal litNumber;
		protected System.Web.UI.WebControls.Panel pnlTaskEmpty;
		protected System.Web.UI.WebControls.Panel pnlShipper;
		protected ShippersDropDownList ddlShoperTag;
		protected System.Web.UI.WebControls.TextBox txtShipTo;
		protected RegionSelector dropRegions;
		protected System.Web.UI.WebControls.TextBox txtAddress;
		protected System.Web.UI.WebControls.TextBox txtZipcode;
		protected System.Web.UI.WebControls.TextBox txtTelphone;
		protected System.Web.UI.WebControls.TextBox txtCellphone;
		protected System.Web.UI.WebControls.Button btnUpdateAddrdss;
		protected System.Web.UI.WebControls.Panel pnlEmptySender;
		protected System.Web.UI.WebControls.Panel pnlTemplates;
		protected System.Web.UI.WebControls.DropDownList ddlTemplates;
		protected System.Web.UI.WebControls.TextBox txtStartCode;
		protected System.Web.UI.WebControls.Button btnPrint;
		protected System.Web.UI.WebControls.Panel pnlEmptyTemplates;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!string.IsNullOrEmpty(base.Request["OrderIds"]))
			{
				BatchPrintData.orderIds = base.Request["OrderIds"];
				this.litNumber.Text = BatchPrintData.orderIds.Trim(new char[]
				{
					','
				}).Split(new char[]
				{
					','
				}).Length.ToString();
			}
			this.ddlShoperTag.SelectedIndexChanged += new System.EventHandler(this.ddlShoperTag_SelectedIndexChanged);
			this.btnUpdateAddrdss.Click += new System.EventHandler(this.btnUpdateAddrdss_Click);
			if (!this.Page.IsPostBack)
			{
				this.ddlShoperTag.DataBind();
				System.Collections.Generic.IList<ShippersInfo> shippers = SalesHelper.GetShippers(false);
				foreach (ShippersInfo current in shippers)
				{
					if (current.IsDefault)
					{
						this.ddlShoperTag.SelectedValue = current.ShipperId;
					}
				}
				this.LoadShipper();
				this.LoadTemplates();
			}
		}
		private void btnUpdateAddrdss_Click(object sender, System.EventArgs e)
		{
			if (!this.dropRegions.GetSelectedRegionId().HasValue)
			{
				this.ShowMsg("请选择发货地区！", false);
				return;
			}
			if (this.UpdateAddress())
			{
				this.ShowMsg("修改成功", true);
				return;
			}
			this.ShowMsg("修改失败，请确认信息填写正确或订单还未发货", false);
		}
		private bool UpdateAddress()
		{
			ShippersInfo shipper = SalesHelper.GetShipper(this.ddlShoperTag.SelectedValue);
			if (shipper != null)
			{
				shipper.Address = this.txtAddress.Text;
				shipper.CellPhone = this.txtCellphone.Text;
				shipper.RegionId = this.dropRegions.GetSelectedRegionId().Value;
				shipper.ShipperName = this.txtShipTo.Text;
				shipper.TelPhone = this.txtTelphone.Text;
				shipper.Zipcode = this.txtZipcode.Text;
				return SalesHelper.UpdateShipper(shipper);
			}
			return false;
		}
		private void ddlShoperTag_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.LoadShipper();
		}
		private void LoadShipper()
		{
			ShippersInfo shipper = SalesHelper.GetShipper(this.ddlShoperTag.SelectedValue);
			if (shipper != null)
			{
				this.txtAddress.Text = shipper.Address;
				this.txtCellphone.Text = shipper.CellPhone;
				this.txtShipTo.Text = shipper.ShipperName;
				this.txtTelphone.Text = shipper.TelPhone;
				this.txtZipcode.Text = shipper.Zipcode;
				this.dropRegions.SetSelectedRegionId(new int?(shipper.RegionId));
				this.pnlEmptySender.Visible = false;
				this.pnlShipper.Visible = true;
				return;
			}
			this.pnlShipper.Visible = false;
			this.pnlEmptySender.Visible = true;
		}
		private void LoadTemplates()
		{
			System.Data.DataTable isUserExpressTemplates = SalesHelper.GetIsUserExpressTemplates();
			if (isUserExpressTemplates != null && isUserExpressTemplates.Rows.Count > 0)
			{
				this.ddlTemplates.Items.Add(new System.Web.UI.WebControls.ListItem("-请选择-", ""));
				foreach (System.Data.DataRow dataRow in isUserExpressTemplates.Rows)
				{
					this.ddlTemplates.Items.Add(new System.Web.UI.WebControls.ListItem(dataRow["ExpressName"].ToString(), dataRow["XmlFile"].ToString()));
				}
				this.pnlEmptyTemplates.Visible = false;
				this.pnlTemplates.Visible = true;
				return;
			}
			this.pnlEmptyTemplates.Visible = true;
			this.pnlTemplates.Visible = false;
		}
	}
}
