using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Web.CustomMade;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	public class Supplier_Admin_SearchBindProduct : AdminPage
	{
		private string productName;
		private int? categoryId;
		private int? brandId;
		private string productCode;
		protected System.Web.UI.WebControls.TextBox txtSearchText;
		protected ProductCategoriesDropDownList dropCategories;
		protected BrandCategoriesDropDownList dropBrandList;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected PageSize hrefPageSize;
		protected Pager pager;
		protected System.Web.UI.WebControls.Repeater rp_bindproduct;
		protected Pager pager1;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			this.rp_bindproduct.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(this.rp_bindproduct_ItemDataBound);
			if (base.Request.QueryString["productCode"] != null)
			{
				this.productCode = base.Request.QueryString["productCode"].ToString();
			}
			if (!base.IsPostBack)
			{
				this.DoCallback();
			}
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
		}
		protected void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReloadProductOnSales(true);
		}
		protected void btnAdd_Click(object sender, System.EventArgs e)
		{
			string value = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(value))
			{
				this.ShowMsg("请选择一件商品！", false);
			}
		}
		private void ReloadProductOnSales(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("productName", Globals.UrlEncode(this.txtSearchText.Text.Trim()));
			if (this.dropCategories.SelectedValue.HasValue)
			{
				nameValueCollection.Add("categoryId", this.dropCategories.SelectedValue.ToString());
			}
			nameValueCollection.Add("pageSize", this.pager.PageSize.ToString());
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
			}
			if (this.dropBrandList.SelectedValue.HasValue)
			{
				nameValueCollection.Add("brandId", this.dropBrandList.SelectedValue.ToString());
			}
			base.ReloadPage(nameValueCollection);
		}
		public string GetSkuContent(string skuId)
		{
			string text = "";
			if (!string.IsNullOrEmpty(skuId.Trim()))
			{
				System.Data.DataTable skuContentBySku = ControlProvider.Instance().GetSkuContentBySku(skuId);
				foreach (System.Data.DataRow dataRow in skuContentBySku.Rows)
				{
					if (!string.IsNullOrEmpty(dataRow["AttributeName"].ToString()) && !string.IsNullOrEmpty(dataRow["ValueStr"].ToString()))
					{
						object obj = text;
						text = string.Concat(new object[]
						{
							obj,
							dataRow["AttributeName"],
							":",
							dataRow["ValueStr"],
							"; "
						});
					}
				}
			}
			if (!(text == ""))
			{
				return text;
			}
			return "无规格";
		}
		protected void DoCallback()
		{
            this.LoadParameters();
            ProductQuery query = new ProductQuery
            {
                PageSize = this.pager.PageSize,
                PageIndex = this.pager.PageIndex,
                SaleStatus = ProductSaleStatus.OnSale,
                IsIncludePromotionProduct = false,
                Keywords = this.productName
            };
            if (!string.IsNullOrEmpty(this.productCode))
            {
                query.ProductCode = this.productCode;
            }
            if (this.brandId.HasValue)
            {
                query.BrandId = new int?(this.brandId.Value);
            }
            query.CategoryId = this.categoryId;
            if (this.categoryId.HasValue)
            {
                query.MaiCategoryPath = CatalogHelper.GetCategory(this.categoryId.Value).Path;
            }
            int userId = HiContext.Current.User.UserId;
            DbQueryResult result = Methods.Supplier_PtSGet(query, null, new int?(userId), null);
            DataTable data = (DataTable)result.Data;
            this.pager1.TotalRecords = this.pager.TotalRecords = result.TotalRecords;
            this.rp_bindproduct.DataSource = data;
            this.rp_bindproduct.DataBind();
		}
		private void LoadParameters()
		{
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
			{
				this.productName = Globals.UrlDecode(this.Page.Request.QueryString["productName"]);
			}
			int value = 0;
			if (int.TryParse(this.Page.Request.QueryString["categoryId"], out value))
			{
				this.categoryId = new int?(value);
			}
			int value2 = 0;
			if (int.TryParse(this.Page.Request.QueryString["brandId"], out value2))
			{
				this.brandId = new int?(value2);
			}
			this.txtSearchText.Text = this.productName;
			this.dropCategories.DataBind();
			this.dropCategories.SelectedValue = this.categoryId;
			this.dropBrandList.DataBind();
			this.dropBrandList.SelectedValue = new int?(value2);
		}
		protected void rp_bindproduct_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			int num = (int)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "ProductId");
			if (num > 0)
			{
				System.Web.UI.WebControls.Repeater repeater = e.Item.FindControl("rp_sku") as System.Web.UI.WebControls.Repeater;
				System.Data.DataTable dataSource = Methods.Supplier_Hishop_Products_ProductId(num);
				repeater.DataSource = dataSource;
				repeater.DataBind();
			}
		}
	}
}
