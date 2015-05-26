using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Subsites.Sales;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.ShopAdmin
{
	public class SetMyShipper : DistributorPage
	{
		protected System.Web.UI.WebControls.TextBox txtShipperTag;
		protected System.Web.UI.WebControls.TextBox txtShipperName;
		protected RegionSelector ddlReggion;
		protected System.Web.UI.WebControls.TextBox txtAddress;
		protected System.Web.UI.WebControls.TextBox txtCellPhone;
		protected System.Web.UI.WebControls.TextBox txtTelPhone;
		protected System.Web.UI.WebControls.TextBox txtZipcode;
		protected System.Web.UI.WebControls.TextBox txtRemark;
		protected System.Web.UI.WebControls.Button btnEditShipper;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnEditShipper.Click += new System.EventHandler(this.btnEditShipper_Click);
			if (!this.Page.IsPostBack)
			{
				ShippersInfo myShipper = SubsiteSalesHelper.GetMyShipper();
				if (myShipper != null)
				{
					Globals.EntityCoding(myShipper, false);
					this.txtShipperTag.Text = myShipper.ShipperTag;
					this.txtShipperName.Text = myShipper.ShipperName;
					this.ddlReggion.SetSelectedRegionId(new int?(myShipper.RegionId));
					this.txtAddress.Text = myShipper.Address;
					this.txtCellPhone.Text = myShipper.CellPhone;
					this.txtTelPhone.Text = myShipper.TelPhone;
					this.txtZipcode.Text = myShipper.Zipcode;
					this.txtRemark.Text = myShipper.Remark;
				}
			}
		}
		private void btnEditShipper_Click(object sender, System.EventArgs e)
		{
			ShippersInfo shippersInfo = new ShippersInfo();
			shippersInfo.ShipperTag = this.txtShipperTag.Text.Trim();
			shippersInfo.ShipperName = this.txtShipperName.Text.Trim();
			if (!this.ddlReggion.GetSelectedRegionId().HasValue)
			{
				this.ShowMsg("请选择地区", false);
				return;
			}
			shippersInfo.RegionId = this.ddlReggion.GetSelectedRegionId().Value;
			shippersInfo.Address = this.txtAddress.Text.Trim();
			shippersInfo.CellPhone = this.txtCellPhone.Text.Trim();
			shippersInfo.TelPhone = this.txtTelPhone.Text.Trim();
			shippersInfo.Zipcode = this.txtZipcode.Text.Trim();
			shippersInfo.Remark = this.txtRemark.Text.Trim();
			if (!this.ValidationShipper(shippersInfo))
			{
				return;
			}
			if (string.IsNullOrEmpty(shippersInfo.CellPhone) && string.IsNullOrEmpty(shippersInfo.TelPhone))
			{
				this.ShowMsg("手机号码和电话号码必填其一", false);
				return;
			}
			if (SubsiteSalesHelper.SetMyShipper(shippersInfo))
			{
				this.ShowMsg("成功保存了发货信息", true);
				return;
			}
			this.ShowMsg("保存发货信息失败", false);
		}
		private bool ValidationShipper(ShippersInfo shipper)
		{
			ValidationResults validationResults = Validation.Validate<ShippersInfo>(shipper, new string[]
			{
				"Valshipper"
			});
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(current.Message);
				}
				this.ShowMsg(text, false);
			}
			return validationResults.IsValid;
		}
	}
}
