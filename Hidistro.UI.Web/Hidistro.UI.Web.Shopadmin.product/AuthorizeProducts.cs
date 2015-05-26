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
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin.product
{
	public class AuthorizeProducts : DistributorPage
	{
		private string productCode;
		private string name;
		private int? lineId;
		protected System.Web.UI.WebControls.TextBox txtName;
		protected System.Web.UI.WebControls.TextBox txtSKU;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected PageSize hrefPageSize;
		protected Pager pager;
		protected System.Web.UI.WebControls.LinkButton lkbtnDownloadCheck;
		protected System.Web.UI.HtmlControls.HtmlInputCheckBox isDownCategory;
		protected Grid grdAuthorizeProducts;
		protected System.Web.UI.WebControls.LinkButton lkbtnDownloadCheck1;
		protected Pager pager1;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			this.grdAuthorizeProducts.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdAuthorizeProducts_RowCommand);
			this.lkbtnDownloadCheck.Click += new System.EventHandler(this.lkbtnDownloadCheck_Click);
			this.lkbtnDownloadCheck1.Click += new System.EventHandler(this.lkbtnDownloadCheck_Click);
			if (!this.Page.IsPostBack)
			{
				this.BindData();
			}
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
		}
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReBindData(true);
		}
		private void lkbtnDownloadCheck_Click(object sender, System.EventArgs e)
		{
			int num = 0;
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdAuthorizeProducts.Rows)
			{
				System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)gridViewRow.FindControl("checkboxCol");
				if (checkBox.Checked)
				{
					num++;
					int productId = (int)this.grdAuthorizeProducts.DataKeys[gridViewRow.RowIndex].Value;
					SubSiteProducthelper.DownloadProduct(productId, this.isDownCategory.Checked);
				}
			}
			if (num == 0)
			{
				this.ShowMsg("请先选择要下载的商品", false);
				return;
			}
			this.ReBindData(false);
		}
		private void grdAuthorizeProducts_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			System.Web.UI.WebControls.GridViewRow gridViewRow = (System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer;
			int productId = (int)this.grdAuthorizeProducts.DataKeys[gridViewRow.RowIndex].Value;
			if (e.CommandName == "download")
			{
				SubSiteProducthelper.DownloadProduct(productId, this.isDownCategory.Checked);
				this.ReBindData(false);
			}
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
			productQuery.SortOrder = SortAction.Desc;
			productQuery.SortBy = "DisplaySequence";
			Globals.EntityCoding(productQuery, true);
			DbQueryResult authorizeProducts = SubSiteProducthelper.GetAuthorizeProducts(productQuery, true);
			this.grdAuthorizeProducts.DataSource = authorizeProducts.Data;
			this.grdAuthorizeProducts.DataBind();
            this.pager.TotalRecords = authorizeProducts.TotalRecords;
            this.pager1.TotalRecords = authorizeProducts.TotalRecords;
		}
	}
}
