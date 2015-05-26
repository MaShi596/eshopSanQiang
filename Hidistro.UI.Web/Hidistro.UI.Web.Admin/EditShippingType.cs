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
	public class EditShippingType : AdminPage
	{
		private int modeId;
		protected Hidistro.UI.Common.Controls.Style Style1;
		protected System.Web.UI.WebControls.TextBox txtModeName;
		protected ExpressCheckBoxList expressCheckBoxList;
		protected ShippingTemplatesDropDownList shippingTemplatesDropDownList;
		protected KindeditorControl fck;
		protected System.Web.UI.WebControls.Button btnUpDate;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["modeId"], out this.modeId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.btnUpDate.Click += new System.EventHandler(this.btnUpDate_Click);
			if (!this.Page.IsPostBack)
			{
				if (this.Page.Request.QueryString["isUpdate"] != null && this.Page.Request.QueryString["isUpdate"] == "true")
				{
					this.ShowMsg("成功修改了一个配送方式", true);
				}
				ShippingModeInfo shippingMode = SalesHelper.GetShippingMode(this.modeId, true);
				if (shippingMode == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				this.BindControl(shippingMode);
				this.shippingTemplatesDropDownList.DataBind();
				this.BindControl(shippingMode);
				this.expressCheckBoxList.ExpressCompany = shippingMode.ExpressCompany;
				this.expressCheckBoxList.DataBind();
			}
		}
		private void BindControl(ShippingModeInfo modeItem)
		{
			this.txtModeName.Text = Globals.HtmlDecode(modeItem.Name);
            this.fck.Text = modeItem.Description;
			if (modeItem.TemplateId > 0)
			{
				this.shippingTemplatesDropDownList.SelectedValue = new int?(modeItem.TemplateId);
			}
		}
		private void btnUpDate_Click(object sender, System.EventArgs e)
		{
			ShippingModeInfo shippingModeInfo = new ShippingModeInfo();
			shippingModeInfo.Name = Globals.HtmlEncode(this.txtModeName.Text.Trim());
			shippingModeInfo.ModeId = this.modeId;
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
			if (SalesHelper.UpdateShippMode(shippingModeInfo))
			{
				this.Page.Response.Redirect("EditShippingType.aspx?modeId=" + shippingModeInfo.ModeId + "&isUpdate=true");
				return;
			}
			this.ShowMsg("更新失败", false);
		}
	}
}
