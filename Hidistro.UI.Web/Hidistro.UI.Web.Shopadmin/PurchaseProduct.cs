using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Sales;
using Hidistro.Subsites.Commodities;
using Hidistro.Subsites.Sales;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class PurchaseProduct : DistributorPage
	{
		protected AuthorizeProductLineDropDownList ddlProductLine;
		protected System.Web.UI.WebControls.TextBox txtProductName;
		protected System.Web.UI.WebControls.TextBox txtProductCode;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected PageSize hrefPageSize;
		protected Pager pager;
		protected System.Web.UI.WebControls.LinkButton lkbtnAdddCheck;
		protected System.Web.UI.WebControls.LinkButton lkbtncancleCheck;
		protected Grid grdAuthorizeProducts;
		protected Pager pager1;
		private int? productLineId;
		private string productCode;
		private string productName;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			this.lkbtnAdddCheck.Click += new System.EventHandler(this.lkbtnAdddCheck_Click);
			this.lkbtncancleCheck.Click += new System.EventHandler(this.lkbtncancleCheck_Click);
			this.grdAuthorizeProducts.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.grdAuthorizeProducts_RowDataBound);
			if (!base.IsPostBack)
			{
				this.ddlProductLine.DataBind();
				this.ddlProductLine.SelectedValue = this.productLineId;
				this.BindData();
			}
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
		}
		private void lkbtnAdddCheck_Click(object sender, System.EventArgs e)
		{
			int num = 0;
			bool flag = true;
			System.Collections.Generic.Dictionary<string, int> dictionary = new System.Collections.Generic.Dictionary<string, int>();
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdAuthorizeProducts.Rows)
			{
				System.Web.UI.WebControls.GridView gridView = gridViewRow.FindControl("grdSkus") as System.Web.UI.WebControls.GridView;
				foreach (System.Web.UI.WebControls.GridViewRow gridViewRow2 in gridView.Rows)
				{
					System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)gridViewRow2.FindControl("checkboxCol");
					System.Web.UI.WebControls.TextBox textBox = gridViewRow2.FindControl("txtNum") as System.Web.UI.WebControls.TextBox;
					if (checkBox != null && checkBox.Checked)
					{
						num++;
						int value;
						if (!int.TryParse(textBox.Text.Trim(), out value) || int.Parse(textBox.Text.Trim()) <= 0 || textBox.Text.Trim().Contains("."))
						{
							flag = false;
							break;
						}
						dictionary.Add(gridView.DataKeys[gridViewRow2.RowIndex].Value.ToString(), value);
					}
				}
				if (!flag)
				{
					break;
				}
			}
			if (num == 0)
			{
				this.ShowMsg("请先选择要添加的商品", false);
				return;
			}
			if (!flag)
			{
				this.ShowMsg("数量不能为空,必需为大于零的正整数", false);
				return;
			}
			int num2 = 0;
			foreach (System.Collections.Generic.KeyValuePair<string, int> current in dictionary)
			{
				PurchaseShoppingCartItemInfo purchaseShoppingCartItemInfo = new PurchaseShoppingCartItemInfo();
				purchaseShoppingCartItemInfo = this.AddPurchaseShoppingCartItemInfo(current.Key, System.Convert.ToInt32(current.Value));
				if (purchaseShoppingCartItemInfo == null)
				{
					this.ShowMsg("商品库存不够", false);
					break;
				}
				if (SubsiteSalesHelper.AddPurchaseItem(purchaseShoppingCartItemInfo))
				{
					num2++;
				}
			}
			if (num2 > 0)
			{
				this.ShowMsg(string.Format("成功添加了{0}件商品", num2), true);
				this.BindData();
				return;
			}
			this.ShowMsg("添加商品失败", false);
		}
		private void lkbtncancleCheck_Click(object sender, System.EventArgs e)
		{
			int num = 0;
			bool flag = true;
			System.Collections.Generic.Dictionary<string, int> dictionary = new System.Collections.Generic.Dictionary<string, int>();
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdAuthorizeProducts.Rows)
			{
				System.Web.UI.WebControls.GridView gridView = gridViewRow.FindControl("grdSkus") as System.Web.UI.WebControls.GridView;
				foreach (System.Web.UI.WebControls.GridViewRow gridViewRow2 in gridView.Rows)
				{
					System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)gridViewRow2.FindControl("checkboxCol");
					System.Web.UI.WebControls.TextBox textBox = gridViewRow2.FindControl("txtNum") as System.Web.UI.WebControls.TextBox;
					if (checkBox != null && checkBox.Checked)
					{
						num++;
						int value;
						if (!int.TryParse(textBox.Text.Trim(), out value) || int.Parse(textBox.Text.Trim()) <= 0 || textBox.Text.Trim().Contains("."))
						{
							flag = false;
							break;
						}
						dictionary.Add((string)gridView.DataKeys[gridViewRow2.RowIndex].Value, value);
					}
				}
				if (!flag)
				{
					break;
				}
			}
			if (num == 0)
			{
				this.ShowMsg("请先选择要添加的商品", false);
				return;
			}
			if (!flag)
			{
				this.ShowMsg("数量不能为空,必需为大于零的正整数", false);
				return;
			}
			int num2 = 0;
			foreach (System.Collections.Generic.KeyValuePair<string, int> current in dictionary)
			{
				if (SubsiteSalesHelper.DeletePurchaseShoppingCartItem(current.Key))
				{
					num2++;
				}
			}
			if (num2 > 0)
			{
				this.ShowMsg(string.Format("成功取消了{0}件商品", num2), true);
				this.BindData();
				return;
			}
			this.ShowMsg("取消商品失败", false);
		}
		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
				{
					this.productCode = base.Server.UrlDecode(this.Page.Request.QueryString["productCode"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
				{
					this.productName = base.Server.UrlDecode(this.Page.Request.QueryString["productName"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productLineId"]))
				{
					this.productLineId = new int?(int.Parse(this.Page.Request.QueryString["productLineId"], System.Globalization.NumberStyles.None));
				}
				this.txtProductCode.Text = this.productCode;
				this.txtProductName.Text = this.productName;
				return;
			}
			this.productCode = this.txtProductCode.Text;
			this.productName = this.txtProductName.Text;
			this.productLineId = this.ddlProductLine.SelectedValue;
		}
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReBindData(true, false);
		}
		private void grdAuthorizeProducts_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
		{
			if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
			{
				int productId = System.Convert.ToInt32(this.grdAuthorizeProducts.DataKeys[e.Row.RowIndex].Value);
				System.Data.DataTable skusByProductId = SubSiteProducthelper.GetSkusByProductId(productId);
				Grid grid = (Grid)e.Row.FindControl("grdSkus");
				if (grid != null)
				{
					grid.DataSource = skusByProductId;
					grid.DataBind();
				}
			}
		}
		public void grdSkus_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
		{
			if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
			{
				Grid grid = (Grid)sender;
				string b = grid.DataKeys[e.Row.RowIndex].Value.ToString();
				System.Web.UI.WebControls.LinkButton linkButton = (System.Web.UI.WebControls.LinkButton)e.Row.FindControl("lbtnAdd");
				System.Collections.Generic.IList<PurchaseShoppingCartItemInfo> purchaseShoppingCartItemInfos = SubsiteSalesHelper.GetPurchaseShoppingCartItemInfos();
				foreach (PurchaseShoppingCartItemInfo current in purchaseShoppingCartItemInfos)
				{
					if (current.SkuId == b)
					{
						linkButton.Text = "取消";
						linkButton.Attributes.Add("style", "color:Red");
					}
				}
			}
		}
		public void grdSkus_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			Grid grid = (Grid)sender;
			int rowIndex = ((System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer).RowIndex;
			string text = (string)grid.DataKeys[rowIndex].Value;
			System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)grid.Rows[rowIndex].Cells[1].FindControl("txtNum");
			System.Web.UI.WebControls.LinkButton linkButton = (System.Web.UI.WebControls.LinkButton)grid.Rows[rowIndex].Cells[2].FindControl("lbtnAdd");
			int quantity;
			if (int.TryParse(textBox.Text.Trim(), out quantity) && int.Parse(textBox.Text.Trim()) > 0 && !textBox.Text.Trim().Contains("."))
			{
				if (e.CommandName == "add")
				{
					if (linkButton.Text == "添加")
					{
						PurchaseShoppingCartItemInfo purchaseShoppingCartItemInfo = new PurchaseShoppingCartItemInfo();
						purchaseShoppingCartItemInfo = this.AddPurchaseShoppingCartItemInfo(text, quantity);
						if (purchaseShoppingCartItemInfo == null)
						{
							this.ShowMsg("商品库存不够", false);
							return;
						}
						if (SubsiteSalesHelper.AddPurchaseItem(purchaseShoppingCartItemInfo))
						{
							this.BindData();
							return;
						}
						this.ShowMsg("添加商品失败", false);
						return;
					}
					else
					{
						if (SubsiteSalesHelper.DeletePurchaseShoppingCartItem(text))
						{
							this.BindData();
							return;
						}
						this.ShowMsg("删除失败", false);
					}
				}
				return;
			}
			this.ShowMsg("数量不能为空,必需为大于零的正整数", false);
		}
		private PurchaseShoppingCartItemInfo AddPurchaseShoppingCartItemInfo(string skuId, int quantity)
		{
			PurchaseShoppingCartItemInfo purchaseShoppingCartItemInfo = new PurchaseShoppingCartItemInfo();
			System.Data.DataTable skuContentBySku = SubSiteProducthelper.GetSkuContentBySku(skuId);
			if (quantity > (int)skuContentBySku.Rows[0]["Stock"])
			{
				return null;
			}
			foreach (System.Data.DataRow dataRow in skuContentBySku.Rows)
			{
				if (!string.IsNullOrEmpty(dataRow["AttributeName"].ToString()) && !string.IsNullOrEmpty(dataRow["ValueStr"].ToString()))
				{
					PurchaseShoppingCartItemInfo expr_7A = purchaseShoppingCartItemInfo;
					object sKUContent = expr_7A.SKUContent;
					expr_7A.SKUContent = string.Concat(new object[]
					{
						sKUContent,
						dataRow["AttributeName"],
						":",
						dataRow["ValueStr"],
						"; "
					});
				}
			}
			purchaseShoppingCartItemInfo.SkuId = skuId;
			purchaseShoppingCartItemInfo.ProductId = (int)skuContentBySku.Rows[0]["ProductId"];
			if (skuContentBySku.Rows[0]["SKU"] != System.DBNull.Value)
			{
				purchaseShoppingCartItemInfo.SKU = (string)skuContentBySku.Rows[0]["SKU"];
			}
			if (skuContentBySku.Rows[0]["Weight"] != System.DBNull.Value)
			{
				purchaseShoppingCartItemInfo.ItemWeight = (decimal)skuContentBySku.Rows[0]["Weight"];
			}
			purchaseShoppingCartItemInfo.ItemPurchasePrice = (decimal)skuContentBySku.Rows[0]["PurchasePrice"];
			purchaseShoppingCartItemInfo.Quantity = quantity;
			purchaseShoppingCartItemInfo.ItemListPrice = (decimal)skuContentBySku.Rows[0]["SalePrice"];
			purchaseShoppingCartItemInfo.ItemDescription = (string)skuContentBySku.Rows[0]["ProductName"];
			if (skuContentBySku.Rows[0]["CostPrice"] != System.DBNull.Value)
			{
				purchaseShoppingCartItemInfo.CostPrice = (decimal)skuContentBySku.Rows[0]["CostPrice"];
			}
			if (skuContentBySku.Rows[0]["ThumbnailUrl40"] != System.DBNull.Value)
			{
				purchaseShoppingCartItemInfo.ThumbnailsUrl = (string)skuContentBySku.Rows[0]["ThumbnailUrl40"];
			}
			return purchaseShoppingCartItemInfo;
		}
		private void ReBindData(bool isSearch, bool reBindByGet)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			if (!string.IsNullOrEmpty(this.txtProductCode.Text))
			{
				nameValueCollection.Add("productCode", this.txtProductCode.Text);
			}
			if (!string.IsNullOrEmpty(this.txtProductName.Text))
			{
				nameValueCollection.Add("productName", this.txtProductName.Text);
			}
			nameValueCollection.Add("productLineId", this.ddlProductLine.SelectedValue.ToString());
			if (!isSearch)
			{
				nameValueCollection.Add("PageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			nameValueCollection.Add("pageSize", this.pager.PageSize.ToString());
			base.ReloadPage(nameValueCollection);
		}
		private void BindData()
		{
			ProductQuery productQuery = new ProductQuery();
			productQuery.PageSize = this.pager.PageSize;
			productQuery.PageIndex = this.pager.PageIndex;
			productQuery.ProductCode = this.productCode;
			productQuery.Keywords = this.productName;
			productQuery.ProductLineId = this.productLineId;
			if (this.grdAuthorizeProducts.SortOrder.ToLower() == "desc")
			{
				productQuery.SortOrder = SortAction.Desc;
			}
			productQuery.SortBy = this.grdAuthorizeProducts.SortOrderBy;
			Globals.EntityCoding(productQuery, true);
			DbQueryResult submitPuchaseProducts = SubSiteProducthelper.GetSubmitPuchaseProducts(productQuery);
			this.grdAuthorizeProducts.DataSource = submitPuchaseProducts.Data;
			this.grdAuthorizeProducts.DataBind();
            this.pager.TotalRecords = submitPuchaseProducts.TotalRecords;
            this.pager1.TotalRecords = submitPuchaseProducts.TotalRecords;
		}
	}
}
