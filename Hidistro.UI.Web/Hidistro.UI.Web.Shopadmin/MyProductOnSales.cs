using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Subsites.Commodities;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class MyProductOnSales : DistributorPage
	{
		private string productName;
		private string productCode;
		private int? categoryId;
		private int? tagId;
		private bool isAlert;
		private ProductSaleStatus saleStatus = ProductSaleStatus.All;
		protected System.Web.UI.WebControls.TextBox txtSearchText;
		protected DistributorProductCategoriesDropDownList dropCategories;
		protected ProductTagsDropDownList dropTagList;
		protected System.Web.UI.WebControls.TextBox txtSKU;
		protected System.Web.UI.WebControls.CheckBox chkIsAlert;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected PageSize hrefPageSize;
		protected Pager pager1;
		protected ImageLinkButton btnDelete;
		protected System.Web.UI.WebControls.LinkButton btnUpShelf;
		protected System.Web.UI.WebControls.LinkButton btnOffShelf;
		protected System.Web.UI.WebControls.LinkButton btnInStock;
		protected SaleStatusDropDownList dropSaleStatus;
		protected Grid grdProducts;
		protected Pager pager;
		protected System.Web.UI.WebControls.TextBox txtPrefix;
		protected System.Web.UI.WebControls.TextBox txtSuffix;
		protected System.Web.UI.WebControls.Button btnAddOK;
		protected System.Web.UI.WebControls.TextBox txtOleWord;
		protected System.Web.UI.WebControls.TextBox txtNewWord;
		protected System.Web.UI.WebControls.Button btnReplaceOK;
		protected ProductTagsLiteral litralProductTag;
		protected TrimTextBox txtProductTag;
		protected System.Web.UI.WebControls.Button btnUpdateProductTags;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            this.grdProducts.ReBindData += new Grid.ReBindDataEventHandler(this.grdProducts_ReBindData);
			this.grdProducts.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.grdProducts_RowDataBound);
			this.btnUpShelf.Click += new System.EventHandler(this.btnUpShelf_Click);
			this.btnOffShelf.Click += new System.EventHandler(this.btnOffShelf_Click);
			this.btnInStock.Click += new System.EventHandler(this.btnInStock_Click);
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			this.btnAddOK.Click += new System.EventHandler(this.btnAddOK_Click);
			this.btnReplaceOK.Click += new System.EventHandler(this.btnReplaceOK_Click);
			this.btnUpdateProductTags.Click += new System.EventHandler(this.btnUpdateProductTags_Click);
			this.grdProducts.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdProducts_RowDeleting);
			this.dropSaleStatus.SelectedIndexChanged += new System.EventHandler(this.dropSaleStatus_SelectedIndexChanged);
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SortOrder"]))
				{
                    this.grdProducts.SortOrder = this.Page.Request.QueryString["SortOrder"];
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SortOrderBy"]))
				{
                    this.grdProducts.SortOrderBy = this.Page.Request.QueryString["SortOrderBy"];
				}
				this.dropSaleStatus.DataBind();
				this.BindProducts();
			}
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
		}
		private void dropSaleStatus_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.ReBindProducts(true);
		}
		private void btnAddOK_Click(object sender, System.EventArgs e)
		{
			string text = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("请先选择要下架的商品", false);
				return;
			}
			if (string.IsNullOrEmpty(this.txtPrefix.Text.Trim()) && string.IsNullOrEmpty(this.txtSuffix.Text.Trim()))
			{
				this.ShowMsg("前后缀不能同时为空", false);
				return;
			}
			if (SubSiteProducthelper.UpdateProductNames(text, Globals.HtmlEncode(this.txtPrefix.Text.Trim()), Globals.HtmlEncode(this.txtSuffix.Text.Trim())))
			{
				this.ShowMsg("为商品名称添加前后缀成功", true);
			}
			else
			{
				this.ShowMsg("为商品名称添加前后缀失败", false);
			}
			this.BindProducts();
		}
		private void btnReplaceOK_Click(object sender, System.EventArgs e)
		{
			string text = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("请先选择要下架的商品", false);
				return;
			}
			if (string.IsNullOrEmpty(this.txtOleWord.Text.Trim()))
			{
				this.ShowMsg("查找字符串不能为空", false);
				return;
			}
			if (SubSiteProducthelper.ReplaceProductNames(text, Globals.HtmlEncode(this.txtOleWord.Text.Trim()), Globals.HtmlEncode(this.txtNewWord.Text.Trim())))
			{
				this.ShowMsg("为商品名称替换字符串缀成功", true);
			}
			else
			{
				this.ShowMsg("为商品名称替换字符串缀失败", false);
			}
			this.BindProducts();
		}
		private void grdProducts_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			SubSiteProducthelper.DeleteProducts(this.grdProducts.DataKeys[e.RowIndex].Value.ToString());
			this.BindProducts();
		}
		private void grdProducts_ReBindData(object sender)
		{
			this.BindProducts();
		}
		private void grdProducts_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
		{
			if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
			{
				System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Row.FindControl("litSaleStatus");
				if (literal.Text == "1")
				{
					literal.Text = "出售中";
					return;
				}
				if (literal.Text == "2")
				{
					literal.Text = "下架区";
					return;
				}
				literal.Text = "仓库中";
			}
		}
		private void btnUpShelf_Click(object sender, System.EventArgs e)
		{
			string text = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("请先选择要上架的商品", false);
				return;
			}
			if (!SubSiteProducthelper.IsOnSale(text))
			{
				this.ShowMsg("选择要上架的商品中有一口价低于最低零售价的情况", false);
				return;
			}
			int num = SubSiteProducthelper.UpdateProductSaleStatus(text, ProductSaleStatus.OnSale);
			if (num > 0)
			{
				this.ShowMsg("成功上架了选择的商品,您可以到出售中的商品中找到上架的商品", true);
				this.BindProducts();
				return;
			}
			this.ShowMsg("上架商品失败，未知错误", false);
		}
		private void btnOffShelf_Click(object sender, System.EventArgs e)
		{
			string text = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("请先选择要下架的商品", false);
				return;
			}
			int num = SubSiteProducthelper.UpdateProductSaleStatus(text, ProductSaleStatus.UnSale);
			if (num > 0)
			{
				this.ShowMsg("成功下架了选择的商品，您可以在下架区的商品里面找到下架以后的商品", true);
				this.BindProducts();
				return;
			}
			this.ShowMsg("下架商品失败，未知错误", false);
		}
		private void btnInStock_Click(object sender, System.EventArgs e)
		{
			string text = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("请先选择要入库的商品", false);
				return;
			}
			int num = SubSiteProducthelper.UpdateProductSaleStatus(text, ProductSaleStatus.OnStock);
			if (num > 0)
			{
				this.ShowMsg("成功入库了选择的商品，您可以在仓库里的商品里面找到入库以后的商品", true);
				this.BindProducts();
				return;
			}
			this.ShowMsg("入库商品失败，未知错误", false);
		}
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReBindProducts(true);
		}
		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			string text = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("请先选择要删除的商品", false);
				return;
			}
			int num = SubSiteProducthelper.DeleteProducts(text);
			if (num > 0)
			{
				this.ShowMsg("成功删除了选择的商品", true);
				this.ReBindProducts(false);
				return;
			}
			this.ShowMsg("删除商品失败，未知错误", false);
		}
		private void btnUpdateProductTags_Click(object sender, System.EventArgs e)
		{
			string text = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("请先选择要关联的商品", false);
				return;
			}
			System.Collections.Generic.IList<int> list = new System.Collections.Generic.List<int>();
			if (!string.IsNullOrEmpty(this.txtProductTag.Text.Trim()))
			{
				string text2 = this.txtProductTag.Text.Trim();
				string[] array;
				if (text2.Contains(","))
				{
					array = text2.Split(new char[]
					{
						','
					});
				}
				else
				{
					array = new string[]
					{
						text2
					};
				}
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string value = array2[i];
					list.Add(System.Convert.ToInt32(value));
				}
			}
			string[] array3;
			if (text.Contains(","))
			{
				array3 = text.Split(new char[]
				{
					','
				});
			}
			else
			{
				array3 = new string[]
				{
					text
				};
			}
			int num = 0;
			string[] array4 = array3;
			for (int j = 0; j < array4.Length; j++)
			{
				string value2 = array4[j];
				SubSiteProducthelper.DeleteProductTags(System.Convert.ToInt32(value2), null);
				if (list.Count > 0 && SubSiteProducthelper.AddProductTags(System.Convert.ToInt32(value2), list, null))
				{
					num++;
				}
			}
			if (num > 0)
			{
				this.ShowMsg(string.Format("已成功修改了{0}件商品的商品标签", num), true);
			}
			else
			{
				this.ShowMsg("已成功取消了商品关联的商品标签", true);
			}
			this.txtProductTag.Text = "";
		}
		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
				{
					this.productName = Globals.UrlDecode(this.Page.Request.QueryString["productName"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
				{
					this.productCode = Globals.UrlDecode(this.Page.Request.QueryString["productCode"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["categoryId"]))
				{
					int value = 0;
					if (int.TryParse(this.Page.Request.QueryString["categoryId"], out value))
					{
						this.categoryId = new int?(value);
					}
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["tagId"]))
				{
					int value2 = 0;
					if (int.TryParse(this.Page.Request.QueryString["tagId"], out value2))
					{
						this.tagId = new int?(value2);
					}
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SaleStatus"]))
				{
					this.saleStatus = (ProductSaleStatus)System.Enum.Parse(typeof(ProductSaleStatus), this.Page.Request.QueryString["SaleStatus"]);
				}
				bool.TryParse(this.Page.Request.QueryString["isAlert"], out this.isAlert);
				this.txtSearchText.Text = this.productName;
				this.txtSKU.Text = this.productCode;
				this.dropCategories.DataBind();
				this.dropCategories.SelectedValue = this.categoryId;
				this.dropTagList.DataBind();
				this.dropTagList.SelectedValue = this.tagId;
				this.chkIsAlert.Checked = this.isAlert;
				this.dropSaleStatus.SelectedValue = this.saleStatus;
				return;
			}
			this.productName = this.txtSearchText.Text;
			this.productCode = this.txtSKU.Text;
			this.categoryId = this.dropCategories.SelectedValue;
			this.tagId = this.dropTagList.SelectedValue;
			this.saleStatus = this.dropSaleStatus.SelectedValue;
			this.isAlert = this.chkIsAlert.Checked;
		}
		private void ReBindProducts(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("productName", Globals.UrlEncode(this.txtSearchText.Text));
			if (this.dropCategories.SelectedValue.HasValue)
			{
				nameValueCollection.Add("categoryId", this.dropCategories.SelectedValue.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			if (this.dropTagList.SelectedValue.HasValue)
			{
				nameValueCollection.Add("tagId", this.dropTagList.SelectedValue.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			nameValueCollection.Add("pageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
			nameValueCollection.Add("productCode", Globals.UrlEncode(this.txtSKU.Text));
			if (isSearch)
			{
				nameValueCollection.Add("PageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			nameValueCollection.Add("isAlert", this.chkIsAlert.Checked.ToString());
			nameValueCollection.Add("SaleStatus", this.dropSaleStatus.SelectedValue.ToString());
			base.ReloadPage(nameValueCollection);
		}
		private void BindProducts()
		{
			ProductQuery productQuery = new ProductQuery();
			productQuery.Keywords = this.productName;
			productQuery.ProductCode = this.productCode;
			productQuery.CategoryId = this.categoryId;
			productQuery.TagId = this.tagId;
			if (this.categoryId.HasValue)
			{
				productQuery.MaiCategoryPath = SubsiteCatalogHelper.GetCategory(this.categoryId.Value).Path;
			}
			productQuery.PageSize = this.pager.PageSize;
			productQuery.PageIndex = this.pager.PageIndex;
			productQuery.IsAlert = this.isAlert;
			productQuery.SaleStatus = this.saleStatus;
			productQuery.SortOrder = SortAction.Desc;
			productQuery.SortBy = "DisplaySequence";
			Globals.EntityCoding(productQuery, true);
			DbQueryResult products = SubSiteProducthelper.GetProducts(productQuery);
			this.grdProducts.DataSource = products.Data;
			this.grdProducts.DataBind();
            this.pager.TotalRecords = products.TotalRecords;
            this.pager1.TotalRecords = products.TotalRecords;
		}
	}
}
