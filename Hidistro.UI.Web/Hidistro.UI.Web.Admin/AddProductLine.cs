using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
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
	[PrivilegeCheck(Privilege.AddProductLine)]
	public class AddProductLine : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtProductLineName;
		protected System.Web.UI.WebControls.DropDownList dropSuppliers;
		protected System.Web.UI.WebControls.Button btnCreate;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
			if (!this.Page.IsPostBack)
			{
				this.dropSuppliers.Items.Add(new System.Web.UI.WebControls.ListItem("-请选择-", ""));
				System.Collections.Generic.IList<string> list = Methods.Supplier_SupSGet();
				foreach (string current in list)
				{
					this.dropSuppliers.Items.Add(new System.Web.UI.WebControls.ListItem(current, current));
				}
			}
		}
		private void btnCreate_Click(object sender, System.EventArgs e)
		{
			ProductLineInfo productLineInfo = new ProductLineInfo
			{
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
			if (ProductLineHelper.AddProductLine(productLineInfo))
			{
				this.Reset();
				this.ShowMsg("成功的添加了一条产品线", true);
				return;
			}
			this.ShowMsg("添加产品线失败", false);
		}
		private void Reset()
		{
			this.txtProductLineName.Text = string.Empty;
		}
	}
}
