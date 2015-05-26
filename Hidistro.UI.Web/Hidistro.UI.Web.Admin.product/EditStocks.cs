using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.product
{
	[PrivilegeCheck(Privilege.EditProducts)]
	public class EditStocks : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtTagetStock;
		protected System.Web.UI.WebControls.Button btnTargetOK;
		protected System.Web.UI.WebControls.TextBox txtAddStock;
		protected System.Web.UI.WebControls.Button btnOperationOK;
		protected Grid grdSelectedProducts;
		protected System.Web.UI.WebControls.Button btnSaveStock;
		private string productIds = string.Empty;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.productIds = this.Page.Request.QueryString["productIds"];
			this.btnSaveStock.Click += new System.EventHandler(this.btnSaveStock_Click);
			this.btnTargetOK.Click += new System.EventHandler(this.btnTargetOK_Click);
			this.btnOperationOK.Click += new System.EventHandler(this.btnOperationOK_Click);
			if (!this.Page.IsPostBack)
			{
				this.BindProduct();
			}
		}
		private void btnOperationOK_Click(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.productIds))
			{
				this.ShowMsg("没有要修改的商品", false);
				return;
			}
			int addStock = 0;
			if (!int.TryParse(this.txtAddStock.Text, out addStock))
			{
				this.ShowMsg("请输入正确的库存格式", false);
				return;
			}
			if (ProductHelper.AddSkuStock(this.productIds, addStock))
			{
				this.BindProduct();
				this.ShowMsg("修改商品的库存成功", true);
				return;
			}
			this.ShowMsg("修改商品的库存失败", false);
		}
		private void btnTargetOK_Click(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.productIds))
			{
				this.ShowMsg("没有要修改的商品", false);
				return;
			}
			int num = 0;
			if (!int.TryParse(this.txtTagetStock.Text, out num))
			{
				this.ShowMsg("请输入正确的库存格式", false);
				return;
			}
			if (num < 0)
			{
				this.ShowMsg("商品库存不能小于0", false);
				return;
			}
			if (ProductHelper.UpdateSkuStock(this.productIds, num))
			{
				this.BindProduct();
				this.ShowMsg("修改商品的库存成功", true);
				return;
			}
			this.ShowMsg("修改商品的库存失败", true);
		}
		private void btnSaveStock_Click(object sender, System.EventArgs e)
		{
			System.Collections.Generic.Dictionary<string, int> dictionary = null;
			if (this.grdSelectedProducts.Rows.Count > 0)
			{
				dictionary = new System.Collections.Generic.Dictionary<string, int>();
				foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdSelectedProducts.Rows)
				{
					int value = 0;
					System.Web.UI.WebControls.TextBox textBox = gridViewRow.FindControl("txtStock") as System.Web.UI.WebControls.TextBox;
					if (int.TryParse(textBox.Text, out value))
					{
						string key = (string)this.grdSelectedProducts.DataKeys[gridViewRow.RowIndex].Value;
						dictionary.Add(key, value);
					}
				}
				if (dictionary.Count > 0)
				{
					if (ProductHelper.UpdateSkuStock(dictionary))
					{
						this.CloseWindow();
					}
					else
					{
						this.ShowMsg("批量修改库存失败", false);
					}
				}
				this.BindProduct();
			}
		}
		private void BindProduct()
		{
			string value = this.Page.Request.QueryString["ProductIds"];
			if (!string.IsNullOrEmpty(value))
			{
				this.grdSelectedProducts.DataSource = ProductHelper.GetSkuStocks(value);
				this.grdSelectedProducts.DataBind();
			}
		}
	}
}
