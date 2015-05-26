using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ProductUnclassified)]
	public class ProductUnclassified : AdminPage
	{
		private int? categoryId;
		private int? typeId;
		private int? lineId;
		private string searchkey;
		private string productCode;
		private System.DateTime? startDate;
		private System.DateTime? endDate;
		protected System.Web.UI.WebControls.TextBox txtSearchText;
		protected ProductCategoriesDropDownList dropCategories;
		protected ProductTypeDownList dropType;
		protected ProductLineDropDownList dropLines;
		protected BrandCategoriesDropDownList dropBrandList;
		protected System.Web.UI.WebControls.TextBox txtSKU;
		protected WebCalendar calendarStartDate;
		protected WebCalendar calendarEndDate;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected PageSize hrefPageSize;
		protected Pager pager1;
		protected ImageLinkButton btnDelete;
		protected Grid grdProducts;
		protected Pager pager;
		protected ProductCategoriesDropDownList dropMoveToCategories;
		protected System.Web.UI.WebControls.Button btnMove;
		protected ProductCategoriesDropDownList dropAddToAllCategories;
		protected System.Web.UI.WebControls.Button btnSetCategories;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            this.grdProducts.ReBindData += new Grid.ReBindDataEventHandler(this.grdProducts_ReBindData);
			this.grdProducts.RowCreated += new System.Web.UI.WebControls.GridViewRowEventHandler(this.grdProducts_RowCreated);
			this.grdProducts.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.grdProducts_RowDataBound);
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			this.btnMove.Click += new System.EventHandler(this.btnMove_Click);
			this.btnSetCategories.Click += new System.EventHandler(this.btnSetCategories_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.dropBrandList.DataBind();
				this.dropLines.DataBind();
				this.dropMoveToCategories.DataBind();
				this.BindProducts();
				this.dropAddToAllCategories.DataBind();
			}
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
		}
		private void LoadParameters()
		{
			int value = 0;
			if (int.TryParse(this.Page.Request.QueryString["categoryId"], out value))
			{
				this.categoryId = new int?(value);
			}
			int value2 = 0;
			if (int.TryParse(this.Page.Request.QueryString["typeId"], out value2))
			{
				this.typeId = new int?(value2);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
			{
				this.productCode = Globals.UrlDecode(this.Page.Request.QueryString["productCode"]);
			}
			int value3 = 0;
			if (int.TryParse(this.Page.Request.QueryString["brandId"], out value3))
			{
				this.dropBrandList.SelectedValue = new int?(value3);
			}
			int value4 = 0;
			if (int.TryParse(this.Page.Request.QueryString["lineId"], out value4))
			{
				this.lineId = new int?(value4);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["searchKey"]))
			{
				this.searchkey = Globals.UrlDecode(this.Page.Request.QueryString["searchKey"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]))
			{
				this.startDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["startDate"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]))
			{
				this.endDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["endDate"]));
			}
			this.dropCategories.DataBind();
			this.dropCategories.SelectedValue = this.categoryId;
			this.dropType.DataBind();
			this.dropType.SelectedValue = this.typeId;
			this.txtSearchText.Text = this.searchkey;
			this.txtSKU.Text = this.productCode;
			this.dropLines.DataBind();
			this.dropLines.SelectedValue = this.lineId;
            this.calendarStartDate.SelectedDate = this.startDate;
            this.calendarEndDate.SelectedDate = this.endDate;
		}
		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("searchKey", Globals.UrlEncode(this.txtSearchText.Text.Trim()));
			if (this.dropCategories.SelectedValue.HasValue)
			{
				nameValueCollection.Add("categoryId", this.dropCategories.SelectedValue.ToString());
			}
			if (this.dropType.SelectedValue.HasValue)
			{
				nameValueCollection.Add("typeId", this.dropType.SelectedValue.ToString());
			}
			if (this.dropLines.SelectedValue.HasValue)
			{
				nameValueCollection.Add("lineId", this.dropLines.SelectedValue.ToString());
			}
			if (this.dropBrandList.SelectedValue.HasValue)
			{
				nameValueCollection.Add("brandId", this.dropBrandList.SelectedValue.ToString());
			}
			nameValueCollection.Add("pageSize", this.pager.PageSize.ToString(System.Globalization.CultureInfo.InvariantCulture));
			if (!string.IsNullOrEmpty(this.txtSKU.Text.Trim()))
			{
				nameValueCollection.Add("productCode", this.txtSKU.Text.Trim());
			}
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			if (this.calendarStartDate.SelectedDate.HasValue)
			{
				nameValueCollection.Add("startDate", this.calendarStartDate.SelectedDate.Value.ToString());
			}
			if (this.calendarEndDate.SelectedDate.HasValue)
			{
				nameValueCollection.Add("endDate", this.calendarEndDate.SelectedDate.Value.ToString());
			}
			base.ReloadPage(nameValueCollection);
		}
		private void BindProducts()
		{
            this.LoadParameters();
            ProductQuery query2 = new ProductQuery
            {
                Keywords = this.searchkey,
                ProductCode = this.productCode,
                ProductLineId = this.lineId,
                PageSize = this.pager.PageSize,
                CategoryId = this.categoryId,
                StartDate = this.startDate,
                EndDate = this.endDate,
                PageIndex = this.pager.PageIndex,
                SortOrder = SortAction.Desc,
                SortBy = "DisplaySequence",
                BrandId = this.dropBrandList.SelectedValue.HasValue ? this.dropBrandList.SelectedValue : null,
                TypeId = this.typeId
            };
            if (this.categoryId.HasValue && (this.categoryId.Value != 0))
            {
                query2.MaiCategoryPath = CatalogHelper.GetCategory(query2.CategoryId.Value).Path;
            }
            DbQueryResult unclassifiedProducts = ProductHelper.GetUnclassifiedProducts(query2);
            this.grdProducts.DataSource = unclassifiedProducts.Data;
            this.grdProducts.DataBind();
            this.dropType.SelectedValue = query2.TypeId;
            this.pager1.TotalRecords = this.pager.TotalRecords = unclassifiedProducts.TotalRecords;
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
					literal.Text = CatalogHelper.GetFullCategory((int)obj);
				}
				ProductCategoriesDropDownList productCategoriesDropDownList = (ProductCategoriesDropDownList)e.Row.FindControl("dropAddToCategories");
				productCategoriesDropDownList.DataBind();
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
						literal2.Text = CatalogHelper.GetFullCategory(int.Parse(text));
						productCategoriesDropDownList.SelectedValue = new int?(int.Parse(text));
					}
				}
			}
		}
		private void grdProducts_RowCreated(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
		{
			if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
			{
				ProductCategoriesDropDownList productCategoriesDropDownList = (ProductCategoriesDropDownList)e.Row.FindControl("dropAddToCategories");
				productCategoriesDropDownList.SelectedIndexChanged += new System.EventHandler(this.dropAddToCategories_SelectedIndexChanged);
			}
		}
		private void dropAddToCategories_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			ProductCategoriesDropDownList productCategoriesDropDownList = (ProductCategoriesDropDownList)sender;
			System.Web.UI.WebControls.GridViewRow gridViewRow = (System.Web.UI.WebControls.GridViewRow)productCategoriesDropDownList.NamingContainer;
			if (productCategoriesDropDownList.SelectedValue.HasValue)
			{
				CatalogHelper.SetProductExtendCategory((int)this.grdProducts.DataKeys[gridViewRow.RowIndex].Value, CatalogHelper.GetCategory(productCategoriesDropDownList.SelectedValue.Value).Path + "|");
				this.ReBind(false);
				return;
			}
			CatalogHelper.SetProductExtendCategory((int)this.grdProducts.DataKeys[gridViewRow.RowIndex].Value, null);
			this.ReBind(false);
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
				ProductHelper.UpdateProductCategory(System.Convert.ToInt32(value), num);
			}
			this.dropCategories.SelectedValue = new int?(num);
			this.ReBind(false);
			this.ShowMsg("转移商品类型成功", true);
		}
		private void btnSetCategories_Click(object sender, System.EventArgs e)
		{
			if (this.dropMoveToCategories.SelectedValue.HasValue)
			{
				int arg_28_0 = this.dropMoveToCategories.SelectedValue.Value;
			}
			string text = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("请选择要设置的商品", false);
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
				if (this.dropAddToAllCategories.SelectedValue.HasValue)
				{
					CatalogHelper.SetProductExtendCategory(System.Convert.ToInt32(value), CatalogHelper.GetCategory(this.dropAddToAllCategories.SelectedValue.Value).Path + "|");
				}
				else
				{
					CatalogHelper.SetProductExtendCategory(System.Convert.ToInt32(value), null);
				}
			}
			this.ReBind(false);
			this.ShowMsg("批量设置扩展分类成功", true);
		}
		private void grdProducts_ReBindData(object sender)
		{
			this.ReBind(false);
		}
		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			string text = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("请先选择要删除的商品", false);
				return;
			}
			int num = ProductHelper.RemoveProduct(text);
			if (num > 0)
			{
				this.ShowMsg(string.Format("成功删除了选择的商品", num), true);
				this.BindProducts();
				return;
			}
			this.ShowMsg("删除商品失败，未知错误", false);
		}
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReBind(true);
		}
	}
}
