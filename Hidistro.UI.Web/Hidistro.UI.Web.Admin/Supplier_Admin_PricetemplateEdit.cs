using Hidistro.UI.ControlPanel.Utility;
using Hishop.Web.CustomMade;
using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	public class Supplier_Admin_PricetemplateEdit : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtRankName;
		protected System.Web.UI.WebControls.TextBox txtSalePrice;
		protected System.Web.UI.WebControls.TextBox txtLowestSalePrice;
		protected System.Web.UI.WebControls.TextBox txtPurchasePrice;
		protected System.Web.UI.WebControls.TextBox txtRemark;
		protected System.Web.UI.WebControls.Button btnSubmitMemberRanks;
		private int gradeId;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnSubmitMemberRanks.Click += new System.EventHandler(this.btnSubmitMemberRanks_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["GradeId"], out this.gradeId))
			{
				base.GotoResourceNotFound();
				return;
			}
			if (!this.Page.IsPostBack)
			{
				System.Data.DataTable dataTable = Methods.Supplier_SupplierGradeInfoGet(this.gradeId);
				if (dataTable == null || dataTable.Rows.Count == 0)
				{
					return;
				}
				System.Data.DataRow dataRow = dataTable.Rows[0];
				this.txtRankName.Text = (string)dataRow["name"];
				this.txtSalePrice.Text = ((decimal)dataRow["SalePrice"]).ToString("f2");
				this.txtPurchasePrice.Text = ((decimal)dataRow["PurchasePrice"]).ToString("f2");
				this.txtLowestSalePrice.Text = ((decimal)dataRow["LowestSalePrice"]).ToString("f2");
				this.txtRemark.Text = (string)dataRow["remark"];
			}
		}
		private void btnSubmitMemberRanks_Click(object sender, System.EventArgs e)
		{
			decimal num = 0m;
			decimal num2 = 0m;
			decimal num3 = 0m;
			string text = this.txtRankName.Text.Trim();
			string remark = this.txtRemark.Text.Trim();
			decimal.TryParse(this.txtSalePrice.Text.Trim(), out num2);
			decimal.TryParse(this.txtLowestSalePrice.Text.Trim(), out num);
			decimal.TryParse(this.txtPurchasePrice.Text.Trim(), out num3);
			if (string.IsNullOrEmpty(text) || num2 == 0m || num3 == 0m || num == 0m)
			{
				this.ShowMsg("请确定，等级名称、商品一口价比例、分销采购价比例、分销商最低零售价比例都必填了", false);
				return;
			}
			string text2 = string.Empty;
			if (num2 < 100m)
			{
				text2 += "一口价，要高于100%";
			}
			if (num > num2 || num < num3)
			{
				text2 += "<br/>分销商最低零售价，要低于一口价及高于采购价";
			}
			if (num3 > num || num3 > num2)
			{
				text2 += "<br/>商品分销采购价，要低于一口价和采购价";
			}
			if (!string.IsNullOrEmpty(text2))
			{
				this.ShowMsg(text2, false);
				return;
			}
			Methods.Supplier_SupplierGradeInfoUpdate(this.gradeId, text, num, num2, num3, remark);
			this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), "sucess", string.Format("<script language=\"javascript\" >alert('修改成功');window.location.href=\"{0}\"</script>", System.Web.HttpContext.Current.Request.RawUrl));
		}
	}
}
