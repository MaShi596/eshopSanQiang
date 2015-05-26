using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using kindeditor.Net;
using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ShippingModes)]
	public class AddShippingType : AdminPage
	{
		protected Script Script1;
		protected System.Web.UI.WebControls.TextBox txtModeName;
		protected ExpressCheckBoxList expressCheckBoxList;
		protected ShippingTemplatesDropDownList shippingTemplatesDropDownList;
		protected KindeditorControl fck;
		protected System.Web.UI.WebControls.Button btnCreate;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
			if (!this.Page.IsPostBack)
			{
				this.shippingTemplatesDropDownList.DataBind();
				this.expressCheckBoxList.BindExpressCheckBoxList();
			}
		}
		private void btnCreate_Click(object sender, System.EventArgs e)
		{
			ShippingModeInfo shippingModeInfo = new ShippingModeInfo();
			shippingModeInfo.Name = Globals.HtmlEncode(this.txtModeName.Text.Trim());
			shippingModeInfo.Description = this.fck.Text.Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
			if (!this.shippingTemplatesDropDownList.SelectedValue.HasValue)
			{
				this.ShowMsg("请选择运费模板", false);
				return;
			}
			shippingModeInfo.TemplateId = this.shippingTemplatesDropDownList.SelectedValue.Value;
			foreach (System.Web.UI.WebControls.ListItem listItem in this.expressCheckBoxList.Items)
			{
				if (listItem.Selected)
				{
					shippingModeInfo.ExpressCompany.Add(listItem.Value);
				}
			}
			if (shippingModeInfo.ExpressCompany.Count == 0)
			{
				this.ShowMsg("至少要选择一个配送公司", false);
				return;
			}
			if (SalesHelper.CreateShippingMode(shippingModeInfo))
			{
				this.ClearControlValue();
				this.ShowMsg("成功添加了一个配送方式", true);
				return;
			}
			this.ShowMsg("添加失败，请确定填写了所有必填项", false);
		}
		private void ClearControlValue()
		{
			this.txtModeName.Text = string.Empty;
            this.fck.Text = string.Empty;
		}
	}
}
