using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Shippers)]
	public class AddShipper : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtShipperTag;
		protected System.Web.UI.WebControls.TextBox txtShipperName;
		protected RegionSelector ddlReggion;
		protected System.Web.UI.WebControls.TextBox txtAddress;
		protected System.Web.UI.WebControls.TextBox txtCellPhone;
		protected System.Web.UI.WebControls.TextBox txtTelPhone;
		protected System.Web.UI.WebControls.TextBox txtZipcode;
		protected YesNoRadioButtonList chkIsDefault;
		protected System.Web.UI.WebControls.TextBox txtRemark;
		protected System.Web.UI.WebControls.Button btnAddShipper;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnAddShipper.Click += new System.EventHandler(this.btnAddShipper_Click);
		}
		private void btnAddShipper_Click(object sender, System.EventArgs e)
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
			shippersInfo.IsDefault = this.chkIsDefault.SelectedValue;
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
			if (SalesHelper.AddShipper(shippersInfo))
			{
				this.ShowMsg("成功添加了一个发货信息", true);
				return;
			}
			this.ShowMsg("添加发货信息失败", false);
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
