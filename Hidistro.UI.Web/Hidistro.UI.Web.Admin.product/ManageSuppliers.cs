using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.product
{
	[AdministerCheck(true)]
	public class ManageSuppliers : AdminPage
	{
		protected PageSize hrefPageSize;
		protected Pager pager1;
		protected ImageLinkButton btnDelete;
		protected Grid grdSuppliers;
		protected ImageLinkButton btnDelete1;
		protected Pager pager;
		protected TrimTextBox txtSupplierName;
		protected TrimTextBox txtOldSupplierName;
		protected TrimTextBox txtSupplierDescription;
		protected System.Web.UI.WebControls.CheckBox chkAddFlag;
		protected System.Web.UI.WebControls.Button btnOk;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnDelete1.Click += new System.EventHandler(this.btnDelete1_Click);
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.BindSuppliers();
			}
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
		}
		private void BindSuppliers()
		{
			Pagination page = new Pagination
			{
				PageIndex = this.pager.PageIndex,
				PageSize = this.hrefPageSize.SelectedSize
			};
			DbQueryResult suppliers = ProductLineHelper.GetSuppliers(page);
			this.grdSuppliers.DataSource = suppliers.Data;
			this.grdSuppliers.DataBind();
			Pager arg_67_0 = this.pager1;
			int totalRecords;
            this.pager.TotalRecords = totalRecords = suppliers.TotalRecords;
            arg_67_0.TotalRecords = totalRecords;
		}
		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			this.DeleteSelected();
			this.BindSuppliers();
		}
		private void btnOk_Click(object sender, System.EventArgs e)
		{
			if (!this.CheckValues())
			{
				return;
			}
			bool flag;
			if (this.chkAddFlag.Checked)
			{
				flag = ProductLineHelper.AddSupplier(Globals.HtmlEncode(this.txtSupplierName.Text.Replace(',', ' ')), Globals.HtmlEncode(this.txtSupplierDescription.Text));
			}
			else
			{
				flag = ProductLineHelper.UpdateSupplier(Globals.HtmlEncode(this.txtOldSupplierName.Text), Globals.HtmlEncode(this.txtSupplierName.Text.Replace(',', ' ')), Globals.HtmlEncode(this.txtSupplierDescription.Text));
			}
			if (!flag)
			{
				this.ShowMsg("操作失败，供货商名称不能重复！", false);
				return;
			}
			this.BindSuppliers();
		}
		private void btnDelete1_Click(object sender, System.EventArgs e)
		{
			this.DeleteSelected();
			this.BindSuppliers();
		}
		private void DeleteSelected()
		{
			if (string.IsNullOrEmpty(base.Request.Form["CheckBoxGroup"]))
			{
				this.ShowMsg("请先勾选要删除的供货商！", false);
				return;
			}
			string[] array = base.Request.Form["CheckBoxGroup"].Split(new char[]
			{
				','
			});
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string textToFormat = array2[i];
				ProductLineHelper.DeleteSupplier(Globals.HtmlEncode(textToFormat));
			}
		}
		private bool CheckValues()
		{
			if (this.txtSupplierName.Text.Length == 0)
			{
				this.ShowMsg("请填写供货商名称！", false);
				return false;
			}
			if (this.txtSupplierName.Text.Length > 50)
			{
				this.ShowMsg("供货商名称的长度不能超过50个字符！", false);
				return false;
			}
			if (this.txtSupplierDescription.Text.Length > 500)
			{
				this.ShowMsg("供货商描述的长度不能超过500个字符！", false);
				return false;
			}
			return true;
		}
	}
}
