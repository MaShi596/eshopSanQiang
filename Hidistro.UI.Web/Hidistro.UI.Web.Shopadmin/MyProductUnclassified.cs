using ASPNET.WebControls;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Subsites.Commodities;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class MyProductUnclassified : DistributorPage
	{
		protected System.Web.UI.WebControls.TextBox txtSearchText;
		protected DistributorProductCategoriesDropDownList dropCategories;
		protected System.Web.UI.WebControls.TextBox txtSKU;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected PageSize hrefPageSize;
		protected Pager pager1;
		protected ImageLinkButton btnDelete;
		protected Grid grdProducts;
		protected Pager pager;
		protected DistributorProductCategoriesDropDownList dropMoveToCategories;
		protected System.Web.UI.WebControls.Button btnMove;
		private int? classId;
		private string searchkey;
		private string productcode;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            this.grdProducts.ReBindData += new Grid.ReBindDataEventHandler(this.grdProducts_ReBindData);
			this.grdProducts.RowCreated += new System.Web.UI.WebControls.GridViewRowEventHandler(this.grdProducts_RowCreated);
			this.grdProducts.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.grdProducts_RowDataBound);
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			this.btnMove.Click += new System.EventHandler(this.btnMove_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
				this.BindProducts();
			}
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
		}
		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				this.dropCategories.DataBind();
				this.dropMoveToCategories.DataBind();
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["classId"]))
				{
					int value = 0;
					if (int.TryParse(this.Page.Request.QueryString["classId"], out value))
					{
						this.classId = new int?(value);
					}
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["searchKey"]))
				{
					this.searchkey = base.Server.UrlDecode(this.Page.Request.QueryString["searchKey"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
				{
					this.productcode = base.Server.UrlDecode(this.Page.Request.QueryString["productCode"]);
				}
				this.dropCategories.SelectedValue = this.classId;
				this.txtSearchText.Text = this.searchkey;
				this.txtSKU.Text = this.productcode;
				return;
			}
			this.searchkey = this.txtSearchText.Text;
			this.classId = this.dropCategories.SelectedValue;
			this.productcode = this.txtSKU.Text;
		}
		private void ReBind()
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("searchKey", this.txtSearchText.Text);
			nameValueCollection.Add("productCode", this.txtSKU.Text);
			if (this.dropCategories.SelectedValue.HasValue)
			{
				nameValueCollection.Add("classId", this.dropCategories.SelectedValue.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			nameValueCollection.Add("pageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
			nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			base.ReloadPage(nameValueCollection);
		}
		private void BindProducts()
		{
			ProductQuery productQuery = new ProductQuery();
			productQuery.Keywords = this.searchkey;
			productQuery.PageSize = this.pager.PageSize;
			productQuery.ProductCode = this.productcode;
			if (this.dropCategories.SelectedValue.HasValue)
			{
				productQuery.CategoryId = new int?(this.dropCategories.SelectedValue.Value);
				if (this.dropCategories.SelectedValue.Value != 0)
				{
					productQuery.MaiCategoryPath = SubsiteCatalogHelper.GetCategory(productQuery.CategoryId.Value).Path;
				}
			}
			productQuery.PageIndex = this.pager.PageIndex;
			productQuery.SortOrder = SortAction.Desc;
			productQuery.SortBy = "DisplaySequence";
			DbQueryResult unclassifiedProducts = SubSiteProducthelper.GetUnclassifiedProducts(productQuery);
			this.grdProducts.DataSource = unclassifiedProducts.Data;
			this.grdProducts.DataBind();
            this.pager.TotalRecords = unclassifiedProducts.TotalRecords;
		}
		private void grdProducts_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
		{
			if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
			{
				System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Row.FindControl("litMainCategory");
				literal.Text = "-";
				object obj = System.Web.UI.DataBinder.Eval(e.Row.DataItem, "CategoryId");
				if (obj != null && obj != System.DBNull.Value)
				{
					literal.Text = SubsiteCatalogHelper.GetFullCategory((int)obj);
				}
				DistributorProductCategoriesDropDownList distributorProductCategoriesDropDownList = (DistributorProductCategoriesDropDownList)e.Row.FindControl("dropAddToCategories");
				distributorProductCategoriesDropDownList.DataBind();
				System.Web.UI.WebControls.Literal literal2 = (System.Web.UI.WebControls.Literal)e.Row.FindControl("litExtendCategory");
				literal2.Text = "-";
				object obj2 = System.Web.UI.DataBinder.Eval(e.Row.DataItem, "ExtendCategoryPath");
				if (obj2 != null && obj2 != System.DBNull.Value)
				{
					string text = (string)obj2;
					if (text.Length > 0)
					{
						text = text.Substring(0, text.Length - 1);
						if (text.Contains("|"))
						{
							text = text.Substring(text.LastIndexOf('|') + 1);
						}
						literal2.Text = SubsiteCatalogHelper.GetFullCategory(int.Parse(text));
						distributorProductCategoriesDropDownList.SelectedValue = new int?(int.Parse(text));
					}
				}
			}
		}
		private void grdProducts_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
		{
			if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
			{
				DistributorProductCategoriesDropDownList distributorProductCategoriesDropDownList = (DistributorProductCategoriesDropDownList)e.Row.FindControl("dropAddToCategories");
				distributorProductCategoriesDropDownList.SelectedIndexChanged += new System.EventHandler(this.dropAddToCategories_SelectedIndexChanged);
			}
		}
		private void dropAddToCategories_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DistributorProductCategoriesDropDownList distributorProductCategoriesDropDownList = (DistributorProductCategoriesDropDownList)sender;
			System.Web.UI.WebControls.GridViewRow gridViewRow = (System.Web.UI.WebControls.GridViewRow)distributorProductCategoriesDropDownList.NamingContainer;
			if (distributorProductCategoriesDropDownList.SelectedValue.HasValue)
			{
				SubsiteCatalogHelper.SetProductExtendCategory((int)this.grdProducts.DataKeys[gridViewRow.RowIndex].Value, SubsiteCatalogHelper.GetCategory(distributorProductCategoriesDropDownList.SelectedValue.Value).Path + "|");
				this.ReBind();
				return;
			}
			SubsiteCatalogHelper.SetProductExtendCategory((int)this.grdProducts.DataKeys[gridViewRow.RowIndex].Value, null);
			this.ReBind();
		}
		private void btnMove_Click(object sender, System.EventArgs e)
		{
			int num = this.dropMoveToCategories.SelectedValue.HasValue ? this.dropMoveToCategories.SelectedValue.Value : 0;
			string text = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("请选择要转移的商品", false);
				return;
			}
			string[] array;
			if (!text.Contains(","))
			{
				array = new string[]
				{
					text
				};
			}
			else
			{
				array = text.Split(new char[]
				{
					','
				});
			}
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string value = array2[i];
				SubSiteProducthelper.UpdateProductCategory(System.Convert.ToInt32(value), num);
			}
			this.dropCategories.SelectedValue = new int?(num);
			this.BindProducts();
			this.ShowMsg("转移商品类型成功", true);
		}
		private void grdProducts_ReBindData(object sender)
		{
			this.ReBind();
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
				this.ShowMsg(string.Format("成功删除了选择的{0}件商品", num), true);
				this.BindProducts();
				return;
			}
			this.ShowMsg("删除商品失败，未知错误", false);
		}
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("searchKey", this.txtSearchText.Text);
			nameValueCollection.Add("productCode", this.txtSKU.Text);
			if (this.dropCategories.SelectedValue.HasValue)
			{
				nameValueCollection.Add("classId", this.dropCategories.SelectedValue.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			nameValueCollection.Add("pageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
			base.ReloadPage(nameValueCollection);
		}
	}
}
