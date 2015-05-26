using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Membership.Context;
using Hidistro.SaleSystem.Catalog;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.SaleSystem.CodeBehind
{
	[System.Web.UI.ParseChildren(true)]
	public class SubCategory : HtmlTemplatedWebControl
	{
		private int categoryId;
		private System.Web.UI.WebControls.Literal litLeadBuy;
		private Common_Location common_Location;
		private ThemedTemplatedRepeater rptProducts;
		private Pager pager;
		private Common_CutdownSearch cutdownSearch;
		private System.Web.UI.WebControls.Literal litSearchResultPage;
		private Common_Search_SortPrice btnSortPrice;
		private Common_Search_SortTime btnSortTime;
		private Common_Search_SortPopularity btnSortPopularity;
		private Common_Search_SortSaleCounts btnSortSaleCounts;
		public SubCategory()
		{
			int.TryParse(this.Page.Request.QueryString["CategoryId"], out this.categoryId);
			CategoryInfo category = CategoryBrowser.GetCategory(this.categoryId);
			if (category != null && category.Depth == 1 && !string.IsNullOrEmpty(category.Theme) && System.IO.File.Exists(Hidistro.Membership.Context.HiContext.Current.Context.Request.MapPath(Hidistro.Membership.Context.HiContext.Current.GetSkinPath() + "/categorythemes/" + category.Theme)))
			{
				this.SkinName = "/categorythemes/" + category.Theme;
			}
		}
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-SubCategory.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.litLeadBuy = (System.Web.UI.WebControls.Literal)this.FindControl("litLeadBuy");
			this.common_Location = (Common_Location)this.FindControl("common_Location");
			this.rptProducts = (ThemedTemplatedRepeater)this.FindControl("rptProducts");
			this.pager = (Pager)this.FindControl("pager");
			this.litSearchResultPage = (System.Web.UI.WebControls.Literal)this.FindControl("litSearchResultPage");
			this.btnSortPrice = (Common_Search_SortPrice)this.FindControl("btn_Common_Search_SortPrice");
			this.btnSortTime = (Common_Search_SortTime)this.FindControl("btn_Common_Search_SortTime");
			this.btnSortPopularity = (Common_Search_SortPopularity)this.FindControl("btn_Common_Search_SortPopularity");
			this.btnSortSaleCounts = (Common_Search_SortSaleCounts)this.FindControl("btn_Common_Search_SortSaleCounts");
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
			this.cutdownSearch = (Common_CutdownSearch)this.FindControl("search_Common_CutdownSearch");
			this.cutdownSearch.ReSearch += new Common_CutdownSearch.ReSearchEventHandler(this.cutdownSearch_ReSearch);
			if (!this.Page.IsPostBack)
			{
				CategoryInfo category = CategoryBrowser.GetCategory(this.categoryId);
				if (category != null)
				{
					if (this.common_Location != null)
					{
						this.common_Location.CateGoryPath = category.Path;
					}
					if (this.litLeadBuy != null)
					{
						this.litLeadBuy.Text = category.Notes1;
					}
					this.LoadPageSearch(category);
				}
				this.BindSearch();
			}
		}
		private void LoadPageSearch(CategoryInfo category)
		{
			if (!string.IsNullOrEmpty(category.MetaKeywords))
			{
				MetaTags.AddMetaKeywords(category.MetaKeywords, Hidistro.Membership.Context.HiContext.Current.Context);
			}
			if (!string.IsNullOrEmpty(category.MetaDescription))
			{
				MetaTags.AddMetaDescription(category.MetaDescription, Hidistro.Membership.Context.HiContext.Current.Context);
			}
			if (!string.IsNullOrEmpty(category.MetaTitle))
			{
				PageTitle.AddSiteNameTitle(category.MetaTitle, Hidistro.Membership.Context.HiContext.Current.Context);
			}
			else
			{
				PageTitle.AddSiteNameTitle(category.Name, Hidistro.Membership.Context.HiContext.Current.Context);
			}
		}
		private void btnSortTime_Sorting(string sortOrder, string sortOrderBy)
		{
			this.ReloadSearchResult(sortOrder, sortOrderBy);
		}
		private void btnSortSaleCounts_Sorting(string sortOrder, string sortOrderBy)
		{
			this.ReloadSearchResult(sortOrder, sortOrderBy);
		}
		private void btnSortPopularity_Sorting(string sortOrder, string sortOrderBy)
		{
			this.ReloadSearchResult(sortOrder, sortOrderBy);
		}
		private void btnSortPrice_Sorting(string sortOrder, string sortOrderBy)
		{
			this.ReloadSearchResult(sortOrder, sortOrderBy);
		}
		protected void cutdownSearch_ReSearch(object sender, System.EventArgs e)
		{
			this.ReloadSearchResult(string.Empty, string.Empty);
		}
		protected void ReloadSearchResult(string sortOrder, string sortOrderBy)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["categoryId"]))
			{
				nameValueCollection.Add("categoryId", Globals.UrlEncode(this.Page.Request.QueryString["categoryId"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["brand"]))
			{
				nameValueCollection.Add("brand", Globals.UrlEncode(this.Page.Request.QueryString["brand"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["valueStr"]))
			{
				nameValueCollection.Add("valueStr", Globals.UrlEncode(this.Page.Request.QueryString["valueStr"]));
			}
			nameValueCollection.Add("TagIds", Globals.UrlEncode(this.cutdownSearch.Item.TagIds));
			nameValueCollection.Add("keywords", Globals.UrlEncode(DataHelper.CleanSearchString(this.cutdownSearch.Item.Keywords)));
			nameValueCollection.Add("minSalePrice", Globals.UrlEncode(this.cutdownSearch.Item.MinSalePrice.ToString()));
			nameValueCollection.Add("maxSalePrice", Globals.UrlEncode(this.cutdownSearch.Item.MaxSalePrice.ToString()));
			nameValueCollection.Add("productCode", Globals.UrlEncode(this.cutdownSearch.Item.ProductCode));
			nameValueCollection.Add("pageIndex", "1");
			nameValueCollection.Add("sortOrderBy", sortOrderBy);
			nameValueCollection.Add("sortOrder", sortOrder);
			base.ReloadPage(nameValueCollection);
		}
		protected void BindSearch()
		{
			ProductBrowseQuery productBrowseQuery = this.GetProductBrowseQuery();
			DbQueryResult browseProductList = ProductBrowser.GetBrowseProductList(productBrowseQuery);
			this.rptProducts.DataSource = browseProductList.Data;
			this.rptProducts.DataBind();
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
			this.litSearchResultPage.Text = string.Format("总共有{0}件商品,{1}件商品为一页,共{2}页第 {3}页", new object[]
			{
				browseProductList.TotalRecords,
				this.pager.PageSize,
				num,
				this.pager.PageIndex
			});
		}
		protected ProductBrowseQuery GetProductBrowseQuery()
		{
			ProductBrowseQuery productBrowseQuery = new ProductBrowseQuery();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["categoryId"]))
			{
				int value = 0;
				if (int.TryParse(this.Page.Request.QueryString["categoryId"], out value))
				{
					productBrowseQuery.CategoryId = new int?(value);
				}
			}
			int value2;
			if (int.TryParse(this.Page.Request.QueryString["brand"], out value2))
			{
				productBrowseQuery.BrandId = new int?(value2);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["valueStr"]))
			{
				System.Collections.Generic.IList<AttributeValueInfo> list = new System.Collections.Generic.List<AttributeValueInfo>();
				string text = Globals.UrlDecode(this.Page.Request.QueryString["valueStr"]);
				text = Globals.HtmlEncode(text);
				string[] array = text.Split(new char[]
				{
					'-'
				});
				if (!string.IsNullOrEmpty(text))
				{
					for (int i = 0; i < array.Length; i++)
					{
						string[] array2 = array[i].Split(new char[]
						{
							'_'
						});
						if (array2.Length > 0 && !string.IsNullOrEmpty(array2[1]) && array2[1] != "0")
						{
							list.Add(new AttributeValueInfo
							{
								AttributeId = System.Convert.ToInt32(array2[0]),
								ValueId = System.Convert.ToInt32(array2[1])
							});
						}
					}
				}
				productBrowseQuery.AttributeValues = list;
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["isPrecise"]))
			{
				productBrowseQuery.IsPrecise = bool.Parse(Globals.UrlDecode(this.Page.Request.QueryString["isPrecise"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keywords"]))
			{
				productBrowseQuery.Keywords = DataHelper.CleanSearchString(Globals.HtmlEncode(Globals.UrlDecode(this.Page.Request.QueryString["keywords"])));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["minSalePrice"]))
			{
				decimal value3 = 0m;
				if (decimal.TryParse(Globals.UrlDecode(this.Page.Request.QueryString["minSalePrice"]), out value3))
				{
					productBrowseQuery.MinSalePrice = new decimal?(value3);
				}
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["maxSalePrice"]))
			{
				decimal value4 = 0m;
				if (decimal.TryParse(Globals.UrlDecode(this.Page.Request.QueryString["maxSalePrice"]), out value4))
				{
					productBrowseQuery.MaxSalePrice = new decimal?(value4);
				}
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["TagIds"]))
			{
				productBrowseQuery.TagIds = Globals.UrlDecode(this.Page.Request.QueryString["TagIds"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
			{
				productBrowseQuery.ProductCode = Globals.HtmlEncode(Globals.UrlDecode(this.Page.Request.QueryString["productCode"]));
			}
			productBrowseQuery.PageIndex = this.pager.PageIndex;
			productBrowseQuery.PageSize = this.pager.PageSize;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortOrderBy"]))
			{
				productBrowseQuery.SortBy = Globals.HtmlEncode(this.Page.Request.QueryString["sortOrderBy"]);
			}
			else
			{
				productBrowseQuery.SortBy = "DisplaySequence";
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortOrder"]))
			{
				productBrowseQuery.SortOrder = (SortAction)System.Enum.Parse(typeof(SortAction), Globals.HtmlEncode(this.Page.Request.QueryString["sortOrder"]));
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
