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
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin.purchaseOrder
{
	public class ChangePurchaseOrderItems : DistributorPage
	{
		protected Grid grdOrderItems;
		protected System.Web.UI.WebControls.Panel pnlEmpty;
		protected System.Web.UI.WebControls.Panel pnlHasStatus;
		protected AuthorizeProductLineDropDownList ddlProductLine;
		protected System.Web.UI.WebControls.TextBox txtProductName;
		protected System.Web.UI.WebControls.TextBox txtProductCode;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected PageSize hrefPageSize;
		protected Pager pager;
		protected Grid grdAuthorizeProducts;
		protected Pager pager1;
		private string purchaseOrderId = string.Empty;
		private string isAdd = string.Empty;
		private int? productLineId;
		private string productCode;
		private string productName;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			this.grdOrderItems.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdOrderItems_RowCommand);
			this.grdAuthorizeProducts.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.grdAuthorizeProducts_RowDataBound);
			if (!this.Page.IsPostBack)
			{
				this.ddlProductLine.DataBind();
				this.ddlProductLine.SelectedValue = this.productLineId;
				this.BindOrderItems();
				if (!string.IsNullOrEmpty(this.isAdd))
				{
					this.BindData();
				}
			}
		}
		private void grdOrderItems_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			string skuId = e.CommandArgument.ToString();
			if (e.CommandName == "UPDATE_ITEMS")
			{
				PurchaseOrderInfo purchaseOrder = SubsiteSalesHelper.GetPurchaseOrder(this.purchaseOrderId);
				if (purchaseOrder.PurchaseOrderItems.Count <= 1)
				{
					this.ShowMsg("采购单的最后一件商品不允许删除", false);
					return;
				}
				SubsiteSalesHelper.DeletePurchaseOrderItem(this.purchaseOrderId, skuId);
			}
			else
			{
				if (e.CommandName == "UPDATE_QUANTITY")
				{
					int rowIndex = ((System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer).RowIndex;
					System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)this.grdOrderItems.Rows[rowIndex].Cells[3].FindControl("txtItemNumber");
					int num;
					if (!int.TryParse(textBox.Text.Trim(), out num))
					{
						this.ShowMsg("商品数量填写错误", false);
						return;
					}
					int skuStock = SubsiteSalesHelper.GetSkuStock(skuId);
					if (num > skuStock)
					{
						this.ShowMsg("此商品库存不够", false);
						return;
					}
					if (num <= 0)
					{
						this.ShowMsg("商品购买数量不能小于等于0", false);
						return;
					}
					SubsiteSalesHelper.UpdatePurchaseOrderItemQuantity(this.purchaseOrderId, skuId, num);
				}
			}
			this.UpdatePurchaseOrder();
			this.BindOrderItems();
			this.ShowMsg("商品数量更新成功！", true);
			Pager arg_13A_0 = this.pager1;
			this.pager.TotalRecords=0;
			arg_13A_0.TotalRecords=0;
			this.grdAuthorizeProducts.DataSource = null;
			this.grdAuthorizeProducts.DataBind();
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
		public void grdSkus_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			Grid grid = (Grid)sender;
			int rowIndex = ((System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer).RowIndex;
			string skuId = (string)grid.DataKeys[rowIndex].Value;
			System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)grid.Rows[rowIndex].Cells[1].FindControl("txtNum");
			System.Web.UI.WebControls.LinkButton linkButton = (System.Web.UI.WebControls.LinkButton)grid.Rows[rowIndex].Cells[2].FindControl("lbtnAdd");
			int num;
			if (int.TryParse(textBox.Text.Trim(), out num) && int.Parse(textBox.Text.Trim()) > 0 && !textBox.Text.Trim().Contains("."))
			{
				if (e.CommandName == "add" && linkButton.Text == "添加")
				{
					PurchaseShoppingCartItemInfo purchaseShoppingCartItemInfo = new PurchaseShoppingCartItemInfo();
					System.Data.DataTable skuContentBySku = SubSiteProducthelper.GetSkuContentBySku(skuId);
					if (num > (int)skuContentBySku.Rows[0]["Stock"])
					{
						this.ShowMsg("商品库存不够", false);
						return;
					}
					foreach (System.Data.DataRow dataRow in skuContentBySku.Rows)
					{
						if (!string.IsNullOrEmpty(dataRow["AttributeName"].ToString()) && !string.IsNullOrEmpty(dataRow["ValueStr"].ToString()))
						{
							PurchaseShoppingCartItemInfo expr_18A = purchaseShoppingCartItemInfo;
							object sKUContent = expr_18A.SKUContent;
							expr_18A.SKUContent = string.Concat(new object[]
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
					purchaseShoppingCartItemInfo.Quantity = num;
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
					if (SubsiteSalesHelper.AddPurchaseOrderItem(purchaseShoppingCartItemInfo, this.purchaseOrderId))
					{
						this.UpdatePurchaseOrder();
						this.BindOrderItems();
						this.ReBindData(true, false);
						return;
					}
					this.ShowMsg("添加商品失败", false);
				}
				return;
			}
			this.ShowMsg("数量不能为空,必需为大于零的正整数", false);
		}
		private void LoadParameters()
		{
			this.purchaseOrderId = this.Page.Request.QueryString["PurchaseOrderId"];
			this.isAdd = this.Page.Request.QueryString["isAdd"];
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
			nameValueCollection.Add("PurchaseOrderId", this.purchaseOrderId);
			nameValueCollection.Add("isAdd", "true");
			base.ReloadPage(nameValueCollection);
		}
		private void UpdatePurchaseOrder()
		{
			decimal num = 0m;
			PurchaseOrderInfo purchaseOrder = SubsiteSalesHelper.GetPurchaseOrder(this.purchaseOrderId);
			foreach (PurchaseOrderItemInfo current in purchaseOrder.PurchaseOrderItems)
			{
				new PurchaseOrderItemInfo();
				num += current.ItemWeight * current.Quantity;
			}
			purchaseOrder.Weight = num;
			SubsiteSalesHelper.UpdatePurchaseOrder(purchaseOrder);
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
		private void BindOrderItems()
		{
			PurchaseOrderInfo purchaseOrder = SubsiteSalesHelper.GetPurchaseOrder(this.purchaseOrderId);
			if (!purchaseOrder.CheckAction(PurchaseOrderActions.DISTRIBUTOR_CONFIRM_PAY))
			{
				this.pnlEmpty.Visible = true;
				this.pnlHasStatus.Visible = false;
				return;
			}
			this.pnlHasStatus.Visible = true;
			this.pnlEmpty.Visible = false;
			if (purchaseOrder != null && purchaseOrder.PurchaseOrderItems.Count > 0)
			{
				this.grdOrderItems.DataSource = purchaseOrder.PurchaseOrderItems;
				this.grdOrderItems.DataBind();
			}
		}
	}
}
