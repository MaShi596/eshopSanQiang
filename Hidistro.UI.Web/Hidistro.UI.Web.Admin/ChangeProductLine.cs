using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ProductLines)]
	public class ChangeProductLine : AdminPage
	{
		protected ProductLineDropDownList dropProductLineFrom;
		protected ProductLineDropDownList dropProductLineFromTo;
		protected System.Web.UI.WebControls.Button btnSaveCategory;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSaveCategory.Click += new System.EventHandler(this.btnSaveCategory_Click);
			if (!base.IsPostBack)
			{
				this.dropProductLineFrom.DataBind();
				this.dropProductLineFromTo.DataBind();
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["LineId"]))
				{
					int value = 0;
					if (int.TryParse(this.Page.Request.QueryString["LineId"], out value))
					{
						this.dropProductLineFrom.SelectedValue = new int?(value);
					}
				}
			}
		}
		protected void btnSaveCategory_Click(object sender, System.EventArgs e)
		{
			if (!this.dropProductLineFrom.SelectedValue.HasValue || !this.dropProductLineFromTo.SelectedValue.HasValue)
			{
				this.ShowMsg("请选择需要替换的产品线或需要替换至的产品线", false);
				return;
			}
			if (this.dropProductLineFrom.SelectedValue.Value == this.dropProductLineFromTo.SelectedValue.Value)
			{
				this.ShowMsg("请选择不同的产品进行替换", false);
				return;
			}
			string text = this.dropProductLineFrom.SelectedItem.Text;
			string text2 = this.dropProductLineFromTo.SelectedItem.Text;
			string text3 = this.dropProductLineFrom.SelectedValue.ToString();
			SendMessageHelper.SendMessageToDistributors(string.Concat(new string[]
			{
				text3,
				"|",
				text,
				"|",
				text2
			}), 4);
			if (!ProductLineHelper.ReplaceProductLine(System.Convert.ToInt32(text3), System.Convert.ToInt32(this.dropProductLineFromTo.SelectedValue)))
			{
				this.ShowMsg("产品线批量转移商品失败", false);
				return;
			}
			this.ShowMsg("产品线批量转移商品成功", true);
		}
	}
}
