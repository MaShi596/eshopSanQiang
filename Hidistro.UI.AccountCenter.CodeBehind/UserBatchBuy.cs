using ASPNET.WebControls;
using Hidistro.AccountCenter.Comments;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Membership.Context;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Shopping;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	public class UserBatchBuy : MemberTemplatedWebControl
	{
		private Common_BatchBuy_ProductList batchbuys;
		private IButton btnBatchBuy;
		private IButton btnSearch;
		private BrandCategoriesDropDownList dropBrandCategories;
		private Common_CategoriesDropDownList ddlCategories;
		private Pager pager;
		private System.Web.UI.WebControls.TextBox txtProductName;
		private System.Web.UI.WebControls.TextBox txtProductCode;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserBatchBuy.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.batchbuys = (Common_BatchBuy_ProductList)this.FindControl("Common_BatchBuy_ProductList");
			this.btnBatchBuy = ButtonManager.Create(this.FindControl("btnBatchBuy"));
			this.btnSearch = ButtonManager.Create(this.FindControl("btnSearch"));
			this.dropBrandCategories = (BrandCategoriesDropDownList)this.FindControl("dropBrandCategories");
			this.ddlCategories = (Common_CategoriesDropDownList)this.FindControl("ddlCategories");
			this.pager = (Pager)this.FindControl("pager");
			this.txtProductName = (System.Web.UI.WebControls.TextBox)this.FindControl("txtProductName");
			this.txtProductCode = (System.Web.UI.WebControls.TextBox)this.FindControl("txtProductCode");
			this.btnBatchBuy.Click += new System.EventHandler(this.btnBatchBuy_Click);
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			if (!HiContext.Current.SiteSettings.IsOpenSiteSale && !HiContext.Current.SiteSettings.IsDistributorSettings)
			{
				this.btnBatchBuy.Visible = false;
			}
			if (!this.Page.IsPostBack)
			{
				this.dropBrandCategories.DataBind();
				this.ddlCategories.DataBind();
				this.BindProducts();
			}
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
		}
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReloadProducts(true);
		}
		private void btnBatchBuy_Click(object sender, System.EventArgs e)
		{
			int num = 0;
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.batchbuys.Rows)
			{
				System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)gridViewRow.FindControl("checkboxCol");
				if (checkBox.Checked)
				{
					string skuId = this.batchbuys.DataKeys[gridViewRow.RowIndex].Value.ToString();
					System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)gridViewRow.FindControl("txtBuyNum");
					if (string.IsNullOrEmpty(textBox.Text.Trim()) || int.Parse(textBox.Text.Trim()) <= 0)
					{
						this.ShowMessage("购买数量值不存在或为非法值", true);
						return;
					}
					ShoppingCartProcessor.AddLineItem(skuId, int.Parse(textBox.Text.Trim()));
					num++;
				}
			}
			if (num > 0)
			{
				this.ShowMessage("选择的商品已经放入购物车", true);
			}
			else
			{
				this.ShowMessage("请选择要购买的商品！", false);
			}
		}
		private void LoadParameters()
		{
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["prodcutcode"]))
			{
				this.txtProductCode.Text = Globals.UrlDecode(this.Page.Request.QueryString["prodcutcode"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keyname"]))
			{
				this.txtProductName.Text = Globals.UrlDecode(this.Page.Request.QueryString["keyname"]);
			}
			int value;
			if (int.TryParse(Globals.UrlDecode(this.Page.Request.QueryString["brandId"]), out value))
			{
				this.dropBrandCategories.SelectedValue = new int?(value);
			}
			int value2;
			if (int.TryParse(Globals.UrlDecode(this.Page.Request.QueryString["categoryId"]), out value2))
			{
				this.ddlCategories.SelectedValue = new int?(value2);
			}
		}
		private void BindProducts()
		{
			this.LoadParameters();
			ProductQuery productQuery = new ProductQuery();
			productQuery.PageSize = this.pager.PageSize;
			productQuery.PageIndex = this.pager.PageIndex;
			productQuery.ProductCode = this.txtProductCode.Text;
			productQuery.Keywords = this.txtProductName.Text;
			productQuery.BrandId = this.dropBrandCategories.SelectedValue;
			productQuery.CategoryId = this.ddlCategories.SelectedValue;
			if (productQuery.CategoryId.HasValue)
			{
				productQuery.MaiCategoryPath = CategoryBrowser.GetCategory(productQuery.CategoryId.Value).Path;
			}
			productQuery.SortOrder = SortAction.Desc;
			productQuery.SortBy = "DisplaySequence";
			Globals.EntityCoding(productQuery, true);
			DbQueryResult batchBuyProducts = CommentsHelper.GetBatchBuyProducts(productQuery);
			this.batchbuys.DataSource = batchBuyProducts.Data;
			this.batchbuys.DataBind();
            this.pager.TotalRecords = batchBuyProducts.TotalRecords;
		}
		private void ReloadProducts(bool isSearch)
		{
			NameValueCollection nameValueCollection = new NameValueCollection();
			new ProductQuery();
			if (!string.IsNullOrEmpty(this.txtProductCode.Text.Trim()))
			{
				nameValueCollection.Add("prodcutcode", Globals.UrlEncode(this.txtProductCode.Text.Trim()));
			}
			if (!string.IsNullOrEmpty(this.txtProductName.Text.Trim()))
			{
				nameValueCollection.Add("keyname", Globals.UrlEncode(this.txtProductName.Text.Trim()));
			}
			if (this.dropBrandCategories.SelectedValue.HasValue)
			{
				nameValueCollection.Add("brandId", Globals.UrlEncode(this.dropBrandCategories.SelectedValue.Value.ToString()));
			}
			if (this.ddlCategories.SelectedValue.HasValue)
			{
				nameValueCollection.Add("categoryId", Globals.UrlEncode(this.ddlCategories.SelectedValue.Value.ToString()));
			}
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
			}
			base.ReloadPage(nameValueCollection);
		}
	}
}
