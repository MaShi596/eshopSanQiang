using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Data;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.product
{
	public class Supplier_ProductBaseInfoEdit : AdminPage
	{
		private string productIds = string.Empty;
		protected System.Web.UI.WebControls.TextBox txtPrefix;
		protected System.Web.UI.WebControls.TextBox txtSuffix;
		protected System.Web.UI.WebControls.Button btnAddOK;
		protected System.Web.UI.WebControls.TextBox txtOleWord;
		protected System.Web.UI.WebControls.TextBox txtNewWord;
		protected System.Web.UI.WebControls.Button btnReplaceOK;
		protected Grid grdSelectedProducts;
		protected System.Web.UI.WebControls.Button btnSaveInfo;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.productIds = this.Page.Request.QueryString["productIds"];
			this.btnSaveInfo.Click += new System.EventHandler(this.btnSaveInfo_Click);
			this.btnAddOK.Click += new System.EventHandler(this.btnAddOK_Click);
			this.btnReplaceOK.Click += new System.EventHandler(this.btnReplaceOK_Click);
			if (!this.Page.IsPostBack)
			{
				this.BindProduct();
			}
		}
		private void btnAddOK_Click(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.txtPrefix.Text.Trim()) && string.IsNullOrEmpty(this.txtSuffix.Text.Trim()))
			{
				this.ShowMsg("前后缀不能同时为空", false);
				return;
			}
			if (ProductHelper.UpdateProductNames(this.productIds, this.txtPrefix.Text.Trim(), this.txtSuffix.Text.Trim()))
			{
				this.ShowMsg("为商品名称添加前后缀成功", true);
			}
			else
			{
				this.ShowMsg("为商品名称添加前后缀失败", false);
			}
			this.BindProduct();
		}
		private void btnReplaceOK_Click(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.txtOleWord.Text.Trim()))
			{
				this.ShowMsg("查找字符串不能为空", false);
				return;
			}
			if (ProductHelper.ReplaceProductNames(this.productIds, this.txtOleWord.Text.Trim(), this.txtNewWord.Text.Trim()))
			{
				this.ShowMsg("为商品名称替换字符串缀成功", true);
			}
			else
			{
				this.ShowMsg("为商品名称替换字符串缀失败", false);
			}
			this.BindProduct();
		}
		private void btnSaveInfo_Click(object sender, System.EventArgs e)
		{
			System.Data.DataTable dataTable = new System.Data.DataTable();
			dataTable.Columns.Add("ProductId");
			dataTable.Columns.Add("ProductName");
			dataTable.Columns.Add("ProductCode");
			dataTable.Columns.Add("MarketPrice");
			dataTable.Columns.Add("LowestSalePrice");
			if (this.grdSelectedProducts.Rows.Count > 0)
			{
				decimal num = 0m;
				decimal num2 = 0m;
				foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdSelectedProducts.Rows)
				{
					int num3 = (int)this.grdSelectedProducts.DataKeys[gridViewRow.RowIndex].Value;
					System.Web.UI.WebControls.TextBox textBox = gridViewRow.FindControl("txtProductName") as System.Web.UI.WebControls.TextBox;
					System.Web.UI.WebControls.TextBox textBox2 = gridViewRow.FindControl("txtProductCode") as System.Web.UI.WebControls.TextBox;
					System.Web.UI.WebControls.TextBox textBox3 = gridViewRow.FindControl("txtMarketPrice") as System.Web.UI.WebControls.TextBox;
					System.Web.UI.WebControls.TextBox textBox4 = gridViewRow.FindControl("txtLowestSalePrice") as System.Web.UI.WebControls.TextBox;
					if (string.IsNullOrEmpty(textBox.Text.Trim()) || string.IsNullOrEmpty(textBox4.Text.Trim()) || (!string.IsNullOrEmpty(textBox3.Text.Trim()) && !decimal.TryParse(textBox3.Text.Trim(), out num)))
					{
						break;
					}
					if (string.IsNullOrEmpty(textBox3.Text.Trim()))
					{
						num = 0m;
					}
					if (decimal.TryParse(textBox4.Text.Trim(), out num2))
					{
						System.Data.DataRow dataRow = dataTable.NewRow();
						dataRow["ProductId"] = num3;
						dataRow["ProductName"] = Globals.HtmlEncode(textBox.Text.Trim());
						dataRow["ProductCode"] = Globals.HtmlEncode(textBox2.Text.Trim());
						if (num >= 0m)
						{
							dataRow["MarketPrice"] = num;
						}
						dataRow["LowestSalePrice"] = num2;
						dataTable.Rows.Add(dataRow);
					}
				}
				if (ProductHelper.UpdateProductBaseInfo(dataTable))
				{
					this.CloseWindow();
				}
				else
				{
					this.ShowMsg("批量修改商品信息失败", false);
				}
				this.BindProduct();
			}
		}
		private void BindProduct()
		{
			string value = this.Page.Request.QueryString["ProductIds"];
			if (!string.IsNullOrEmpty(value))
			{
				this.grdSelectedProducts.DataSource = ProductHelper.GetProductBaseInfo(value);
				this.grdSelectedProducts.DataBind();
			}
		}
	}
}
