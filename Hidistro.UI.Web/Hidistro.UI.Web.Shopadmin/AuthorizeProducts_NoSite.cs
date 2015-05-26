using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Subsites.Commodities;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class AuthorizeProducts_NoSite : DistributorPage
	{
		private string productCode;
		private string name;
		private int? lineId;
		protected System.Web.UI.WebControls.Literal litPageTitle;
		protected System.Web.UI.WebControls.TextBox txtName;
		protected System.Web.UI.WebControls.TextBox txtSKU;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected PageSize hrefPageSize;
		protected Pager pager;
		protected Grid grdAuthorizeProducts;
		protected Pager pager1;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			if (!this.Page.IsPostBack)
			{
				this.BindData();
			}
		}
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReBindData(true);
		}
		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
				{
					this.productCode = base.Server.UrlDecode(this.Page.Request.QueryString["productCode"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["name"]))
				{
					this.name = base.Server.UrlDecode(this.Page.Request.QueryString["name"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["lineId"]))
				{
					int value = 0;
					if (int.TryParse(this.Page.Request.QueryString["lineId"], out value))
					{
						this.lineId = new int?(value);
					}
					this.litPageTitle.Text = "＂" + base.Server.UrlDecode(this.Page.Request.QueryString["lineName"]) + "＂产品线下商品列表";
				}
				this.txtSKU.Text = this.productCode;
				this.txtName.Text = this.name;
				return;
			}
			this.productCode = this.txtSKU.Text;
			this.name = this.txtName.Text;
		}
		private void ReBindData(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			if (!string.IsNullOrEmpty(this.txtSKU.Text))
			{
				nameValueCollection.Add("ProductCode", this.txtSKU.Text);
			}
			if (!string.IsNullOrEmpty(this.txtName.Text))
			{
				nameValueCollection.Add("name", this.txtName.Text);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["lineId"]))
			{
				nameValueCollection.Add("lineId", this.Page.Request.QueryString["lineId"]);
				nameValueCollection.Add("lineName", base.Server.UrlDecode(this.Page.Request.QueryString["lineName"]));
			}
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
			productQuery.Keywords = this.name;
			productQuery.ProductLineId = this.lineId;
			if (this.grdAuthorizeProducts.SortOrder.ToLower() == "desc")
			{
				productQuery.SortOrder = SortAction.Desc;
			}
			productQuery.SortBy = this.grdAuthorizeProducts.SortOrderBy;
			Globals.EntityCoding(productQuery, true);
			DbQueryResult authorizeProducts = SubSiteProducthelper.GetAuthorizeProducts(productQuery, false);
			this.grdAuthorizeProducts.DataSource = authorizeProducts.Data;
			this.grdAuthorizeProducts.DataBind();
            this.pager.TotalRecords = authorizeProducts.TotalRecords;
            this.pager1.TotalRecords = authorizeProducts.TotalRecords;
		}
	}
}
