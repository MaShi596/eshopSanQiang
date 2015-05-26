using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Catalog;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class Brand : HtmlTemplatedWebControl
	{
		private ThemedTemplatedRepeater rptProduct;
		private Pager pager;
		private System.Web.UI.WebControls.Literal litBrandProductResult;
		private Common_CutdownSearch cutdownSearch;
		private Common_Search_SortPrice btnSortPrice;
		private Common_Search_SortTime btnSortTime;
		private Common_Search_SortPopularity btnSortPopularity;
		private Common_Search_SortSaleCounts btnSortSaleCounts;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-Brand.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.rptProduct = (ThemedTemplatedRepeater)this.FindControl("rptProduct");
			this.pager = (Pager)this.FindControl("pager");
			this.litBrandProductResult = (System.Web.UI.WebControls.Literal)this.FindControl("litBrandProductResult");
			this.cutdownSearch = (Common_CutdownSearch)this.FindControl("search_Common_CutdownSearch");
			this.btnSortPrice = (Common_Search_SortPrice)this.FindControl("btn_Common_Search_SortPrice");
			this.btnSortTime = (Common_Search_SortTime)this.FindControl("btn_Common_Search_SortTime");
			this.btnSortPopularity = (Common_Search_SortPopularity)this.FindControl("btn_Common_Search_SortPopularity");
			this.btnSortSaleCounts = (Common_Search_SortSaleCounts)this.FindControl("btn_Common_Search_SortSaleCounts");
			this.cutdownSearch.ReSearch += new Common_CutdownSearch.ReSearchEventHandler(this.cutdownSearch_ReSearch);
			this.btnSortPrice.Sorting += new Common_Search_SortTime.SortingHandler(this.btnSortPrice_Sorting);
			this.btnSortTime.Sorting += new Common_Search_SortTime.SortingHandler(this.btnSortTime_Sorting);
			if (this.btnSortPopularity != null)
			{
				this.btnSortPopularity.Sorting += new Common_Search_SortPopularity.SortingHandler(this.btnSortPopularity_Sorting);
			}
			if (this.btnSortSaleCounts != null)
			{
				this.btnSortSaleCounts.Sorting += new Common_Search_SortSaleCounts.SortingHandler(this.btnSortSaleCounts_Sorting);
			}
			if (!this.Page.IsPostBack)
			{
				this.BindBrandProduct();
			}
		}
		private void cutdownSearch_ReSearch(object sender, System.EventArgs e)
		{
			this.ReloadBrand(string.Empty, string.Empty);
		}
		private void btnSortTime_Sorting(string sortOrder, string sortOrderBy)
		{
			this.ReloadBrand(sortOrder, sortOrderBy);
		}
		private void btnSortSaleCounts_Sorting(string sortOrder, string sortOrderBy)
		{
			this.ReloadBrand(sortOrder, sortOrderBy);
		}
		private void btnSortPopularity_Sorting(string sortOrder, string sortOrderBy)
		{
			this.ReloadBrand(sortOrder, sortOrderBy);
		}
		private void btnSortPrice_Sorting(string sortOrder, string sortOrderBy)
		{
			this.ReloadBrand(sortOrder, sortOrderBy);
		}
		private void BindBrandProduct()
		{
			ProductBrowseQuery productBrowseQuery = this.GetProductBrowseQuery();
			DbQueryResult browseProductList = ProductBrowser.GetBrowseProductList(productBrowseQuery);
			this.rptProduct.DataSource = browseProductList.Data;
			this.rptProduct.DataBind();
            this.pager.TotalRecords = browseProductList.TotalRecords;
			int num;
			if (System.Convert.ToDouble(browseProductList.TotalRecords) % (double)this.pager.PageSize > 0.0)
			{
				num = browseProductList.TotalRecords / this.pager.PageSize + 1;
			}
			else
			{
				num = browseProductList.TotalRecords / this.pager.PageSize;
			}
			this.litBrandProductResult.Text = string.Format("总共有{0}件商品,{1}件商品为一页,共{2}页第 {3}页", new object[]
			{
				browseProductList.TotalRecords,
				this.pager.PageSize,
				num,
				this.pager.PageIndex
			});
		}
		private void ReloadBrand(string sortOrder, string sortOrderBy)
		{
			base.ReloadPage(new System.Collections.Specialized.NameValueCollection
			{

				{
					"keywords",
					Globals.UrlEncode(this.cutdownSearch.Item.Keywords)
				},

				{
					"minSalePrice",
					Globals.UrlEncode(this.cutdownSearch.Item.MinSalePrice.ToString())
				},

				{
					"maxSalePrice",
					Globals.UrlEncode(this.cutdownSearch.Item.MaxSalePrice.ToString())
				},

				{
					"productCode",
					Globals.UrlEncode(this.cutdownSearch.Item.ProductCode)
				},

				{
					"pageIndex",
					this.pager.PageIndex.ToString()
				},

				{
					"sortOrderBy",
					sortOrderBy
				},

				{
					"sortOrder",
					sortOrder
				},

				{
					"TagIds",
					Globals.UrlEncode(this.cutdownSearch.Item.TagIds)
				}
			});
		}
		private ProductBrowseQuery GetProductBrowseQuery()
		{
			ProductBrowseQuery productBrowseQuery = new ProductBrowseQuery();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keywords"]))
			{
				productBrowseQuery.Keywords = Globals.UrlDecode(this.Page.Request.QueryString["keywords"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["minSalePrice"]))
			{
				decimal value = 0m;
				if (decimal.TryParse(Globals.UrlDecode(this.Page.Request.QueryString["minSalePrice"]), out value))
				{
					productBrowseQuery.MinSalePrice = new decimal?(value);
				}
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["maxSalePrice"]))
			{
				decimal value2 = 0m;
				if (decimal.TryParse(Globals.UrlDecode(this.Page.Request.QueryString["maxSalePrice"]), out value2))
				{
					productBrowseQuery.MaxSalePrice = new decimal?(value2);
				}
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
			{
				productBrowseQuery.ProductCode = Globals.UrlDecode(this.Page.Request.QueryString["productCode"]);
			}
			productBrowseQuery.PageIndex = this.pager.PageIndex;
			productBrowseQuery.PageSize = this.pager.PageSize;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortOrderBy"]))
			{
				productBrowseQuery.SortBy = this.Page.Request.QueryString["sortOrderBy"];
			}
			else
			{
				productBrowseQuery.SortBy = "DisplaySequence";
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["TagIds"]))
			{
				productBrowseQuery.TagIds = Globals.UrlDecode(this.Page.Request.QueryString["TagIds"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortOrder"]))
			{
				productBrowseQuery.SortOrder = (SortAction)System.Enum.Parse(typeof(SortAction), this.Page.Request.QueryString["sortOrder"]);
			}
			else
			{
				productBrowseQuery.SortOrder = SortAction.Desc;
			}
			Globals.EntityCoding(productBrowseQuery, true);
			return productBrowseQuery;
		}
	}
}
