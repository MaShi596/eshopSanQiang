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
	public class MyProducts : DistributorPage
	{
		protected System.Web.UI.WebControls.TextBox txtSearchText;
		protected DistributorProductCategoriesDropDownList dropCategories;
		protected System.Web.UI.WebControls.TextBox txtSKU;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected PageSize hrefPageSize;
		protected Pager pager;
		protected ImageLinkButton btnDelete;
		protected Grid grdProducts;
		protected Pager pager1;
		private string productName;
		private string productCode;
		private int? categoryId;
		private System.Collections.Generic.IList<int> SelectedProducts
		{
			get
			{
				System.Collections.Generic.IList<int> list = new System.Collections.Generic.List<int>();
				foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdProducts.Rows)
				{
					System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)gridViewRow.FindControl("checkboxCol");
					if (checkBox.Checked)
					{
						int item = (int)this.grdProducts.DataKeys[gridViewRow.RowIndex].Value;
						list.Add(item);
					}
				}
				return list;
			}
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            this.grdProducts.ReBindData += new Grid.ReBindDataEventHandler(this.grdProducts_ReBindData);
			this.grdProducts.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdProducts_RowDeleting);
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
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
				this.BindProducts();
			}
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
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
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.Page.Response.Redirect(string.Concat(new object[]
			{
				Globals.ApplicationPath,
				"/Shopadmin/product/MyProducts.aspx?productName=",
				Globals.UrlEncode(this.txtSearchText.Text),
				"&categoryId=",
				this.dropCategories.SelectedValue,
				"&productCode=",
				Globals.UrlEncode(this.txtSKU.Text),
				"&pageSize=",
				this.pager.PageSize
			}));
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
				this.ReBindProducts();
				return;
			}
			this.ShowMsg("删除商品失败，未知错误", false);
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
				this.txtSearchText.Text = this.productName;
				this.txtSKU.Text = this.productCode;
				this.dropCategories.DataBind();
				this.dropCategories.SelectedValue = this.categoryId;
				return;
			}
			this.productName = this.txtSearchText.Text;
			this.productCode = this.txtSKU.Text;
			this.categoryId = this.dropCategories.SelectedValue;
		}
		private void ReBindProducts()
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("productName", this.txtSearchText.Text);
			if (this.dropCategories.SelectedValue.HasValue)
			{
				nameValueCollection.Add("categoryId", this.dropCategories.SelectedValue.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			nameValueCollection.Add("pageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
			nameValueCollection.Add("productCode", this.txtSKU.Text);
			nameValueCollection.Add("PageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			base.ReloadPage(nameValueCollection);
		}
		private void BindProducts()
		{
			ProductQuery productQuery = new ProductQuery();
			productQuery.Keywords = this.productName;
			productQuery.ProductCode = this.productCode;
			productQuery.PageIndex = this.pager.PageIndex;
			productQuery.PageSize = this.pager.PageSize;
			productQuery.CategoryId = this.categoryId;
			if (this.categoryId.HasValue)
			{
				productQuery.MaiCategoryPath = SubsiteCatalogHelper.GetCategory(this.categoryId.Value).Path;
			}
			productQuery.SaleStatus = ProductSaleStatus.OnSale;
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
