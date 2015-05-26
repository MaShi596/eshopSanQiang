using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Hishop.Web.CustomMade;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.EditProductLine)]
	public class EditProductLine : AdminPage
	{
		private int lineId;
		protected System.Web.UI.WebControls.TextBox txtProductLineName;
		protected System.Web.UI.WebControls.DropDownList dropSuppliers;
		protected System.Web.UI.WebControls.Button btnSave;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["LineId"], out this.lineId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			if (!base.IsPostBack)
			{
				this.dropSuppliers.Items.Add(new System.Web.UI.WebControls.ListItem("-请选择-", ""));
				System.Collections.Generic.IList<string> list = Methods.Supplier_SupSGet();
				foreach (string current in list)
				{
					this.dropSuppliers.Items.Add(new System.Web.UI.WebControls.ListItem(current, current));
				}
				this.LoadControl();
			}
		}
		private void LoadControl()
		{
			ProductLineInfo productLine = ProductLineHelper.GetProductLine(this.lineId);
			if (productLine == null)
			{
				base.GotoResourceNotFound();
				return;
			}
			Globals.EntityCoding(productLine, false);
			this.txtProductLineName.Text = productLine.Name;
			this.dropSuppliers.SelectedValue = productLine.SupplierName;
		}
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			ProductLineInfo productLineInfo = new ProductLineInfo
			{
				LineId = this.lineId,
				Name = this.txtProductLineName.Text.Trim(),
				SupplierName = (this.dropSuppliers.SelectedValue.Length > 0) ? this.dropSuppliers.SelectedValue : null
			};
			ValidationResults validationResults = Validation.Validate<ProductLineInfo>(productLineInfo, new string[]
			{
				"ValProductLine"
			});
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(current.Message);
				}
				this.ShowMsg(text, false);
				return;
			}
			if (ProductLineHelper.UpdateProductLine(productLineInfo))
			{
				this.ShowMsg("已经成功修改当前产品线信息", true);
				return;
			}
			this.ShowMsg("修改产品线失败", false);
		}
	}
}
