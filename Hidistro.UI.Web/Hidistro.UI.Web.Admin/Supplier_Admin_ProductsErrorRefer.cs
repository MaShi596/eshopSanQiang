using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Web.CustomMade;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	public class Supplier_Admin_ProductsErrorRefer : AdminPage
	{
		private int checkedStatus;
		private string productName;
		private string productCode;
		private int? categoryId;
		private int? lineId;
		private int? tagId;
		private int? typeId;
		private System.DateTime? startDate;
		private System.DateTime? endDate;
		protected System.Web.UI.HtmlControls.HtmlGenericControl htmlDivNoCheck;
		protected System.Web.UI.WebControls.Literal litlChecked;
		protected System.Web.UI.WebControls.Literal litlNoCheckNum;
		protected System.Web.UI.WebControls.Literal litlErrorReferNum;
		protected System.Web.UI.WebControls.TextBox txtSearchText;
		protected ProductCategoriesDropDownList dropCategories;
		protected ProductLineDropDownList dropLines;
		protected BrandCategoriesDropDownList dropBrandList;
		protected ProductTagsDropDownList dropTagList;
		protected ProductTypeDownList dropType;
		protected System.Web.UI.WebControls.TextBox txtSKU;
		protected WebCalendar calendarStartDate;
		protected WebCalendar calendarEndDate;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected PageSize hrefPageSize;
		protected Pager pager1;
		protected ImageLinkButton btnDelete;
		protected System.Web.UI.WebControls.LinkButton btnUpShelf;
		protected ImageLinkButton btnCheck;
		protected ImageLinkButton btnCheckError;
		protected Grid grdProducts;
		protected Pager pager;
		protected System.Web.UI.WebControls.CheckBox chkDeleteImage;
		protected ProductTagsLiteral litralProductTag;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hdPenetrationStatus;
		protected TrimTextBox txtProductTag;
		protected System.Web.UI.WebControls.Button btnUpdateProductTags;
		protected System.Web.UI.WebControls.Button btnOK;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            this.grdProducts.ReBindData += new Grid.ReBindDataEventHandler(this.grdProducts_ReBindData);
			this.btnUpShelf.Click += new System.EventHandler(this.btnUpShelf_Click);
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			this.btnUpdateProductTags.Click += new System.EventHandler(this.btnUpdateProductTags_Click);
			this.grdProducts.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdProducts_RowDeleting);
			this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
			this.btnCheckError.Click += new System.EventHandler(this.btnCheckError_Click);
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
				this.dropBrandList.DataBind();
				this.dropTagList.DataBind();
				this.BindProducts();
			}
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
		}
		private void btnCheckError_Click(object sender, System.EventArgs e)
		{
			string text = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("错误：请先选择要提交的商品", false);
				return;
			}
			Methods.Supplier_PtCheckErrorReferError(text);
			this.ShowMsg("操作成功", true);
			this.BindProducts();
		}
		private void btnCheck_Click(object sender, System.EventArgs e)
		{
			string text = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("错误：请先选择要提交的商品", false);
				return;
			}
			Methods.Supplier_PtCheckErrorReferAgree(text);
			ProductHelper.CanclePenetrationProducts(text);
			this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), "sucess", string.Format("<script language=\"javascript\" >alert('操作成功');window.location.href=\"{0}\"</script>", System.Web.HttpContext.Current.Request.RawUrl));
		}
		private void grdProducts_ReBindData(object sender)
		{
			this.BindProducts();
		}
		private void grdProducts_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			System.Collections.Generic.List<int> list = new System.Collections.Generic.List<int>();
			string text = this.grdProducts.DataKeys[e.RowIndex].Value.ToString();
			if (text != "")
			{
				list.Add(System.Convert.ToInt32(text));
			}
			SendMessageHelper.SendMessageToDistributors(text, 3);
			if (this.hdPenetrationStatus.Value.Equals("1"))
			{
				ProductHelper.CanclePenetrationProducts(text);
			}
			if (ProductHelper.RemoveProduct(text) > 0)
			{
				this.ShowMsg("删除商品成功", true);
				this.BindProducts();
			}
		}
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReloadProductInStock(true);
		}
		private void btnDelete_Click(object sender, System.EventArgs e)
		{
			string text = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("请先选择要删除的商品", false);
				return;
			}
			System.Collections.Generic.List<int> list = new System.Collections.Generic.List<int>();
			string[] array = text.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				string value = array[i];
				list.Add(System.Convert.ToInt32(value));
			}
			SendMessageHelper.SendMessageToDistributors(text, 3);
			if (ProductHelper.CanclePenetrationProducts(text) >= 1)
			{
				int num = ProductHelper.RemoveProduct(text);
				if (num > 0)
				{
					this.ShowMsg("成功删除了选择的商品", true);
					this.BindProducts();
					return;
				}
				this.ShowMsg("删除商品失败，未知错误", false);
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
			int num = ProductHelper.UpShelf(text);
			if (num > 0)
			{
				this.ShowMsg("成功上架了选择的商品，您可以在出售中的商品里面找到上架以后的商品", true);
				this.BindProducts();
				return;
			}
			this.ShowMsg("上架商品失败，未知错误", false);
		}
		private void btnOK_Click(object sender, System.EventArgs e)
		{
			string text = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("请先选择要下架的商品", false);
				return;
			}
			if (this.hdPenetrationStatus.Value.Equals("1"))
			{
				System.Collections.Generic.List<int> list = new System.Collections.Generic.List<int>();
				string[] array = text.Split(new char[]
				{
					','
				});
				for (int i = 0; i < array.Length; i++)
				{
					string value = array[i];
					list.Add(System.Convert.ToInt32(value));
				}
				SendMessageHelper.SendMessageToDistributors(text, 1);
				if (ProductHelper.CanclePenetrationProducts(text) == 0)
				{
					this.ShowMsg("取消铺货失败！", false);
					return;
				}
			}
			int num = ProductHelper.OffShelf(text);
			if (num > 0)
			{
				this.ShowMsg("成功下架了选择的商品，您可以在下架区的商品里面找到下架以后的商品", true);
				this.BindProducts();
				return;
			}
			this.ShowMsg("下架商品失败，未知错误", false);
		}
		private void ReloadProductInStock(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("productName", Globals.UrlEncode(this.txtSearchText.Text.Trim()));
			if (this.dropCategories.SelectedValue.HasValue)
			{
				nameValueCollection.Add("categoryId", this.dropCategories.SelectedValue.ToString());
			}
			if (this.dropLines.SelectedValue.HasValue)
			{
				nameValueCollection.Add("lineId", this.dropLines.SelectedValue.ToString());
			}
			if (this.dropTagList.SelectedValue.HasValue)
			{
				nameValueCollection.Add("tagId", this.dropTagList.SelectedValue.ToString());
			}
			nameValueCollection.Add("productCode", Globals.UrlEncode(Globals.HtmlEncode(this.txtSKU.Text)));
			nameValueCollection.Add("pageSize", this.pager.PageSize.ToString());
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
			}
			if (this.calendarStartDate.SelectedDate.HasValue)
			{
				nameValueCollection.Add("startDate", this.calendarStartDate.SelectedDate.Value.ToString());
			}
			if (this.calendarEndDate.SelectedDate.HasValue)
			{
				nameValueCollection.Add("endDate", this.calendarEndDate.SelectedDate.Value.ToString());
			}
			if (this.dropBrandList.SelectedValue.HasValue)
			{
				nameValueCollection.Add("brandId", this.dropBrandList.SelectedValue.ToString());
			}
			if (this.dropType.SelectedValue.HasValue)
			{
				nameValueCollection.Add("typeId", this.dropType.SelectedValue.Value.ToString());
			}
			nameValueCollection.Add("checkedStatus", this.checkedStatus.ToString());
			base.ReloadPage(nameValueCollection);
		}
		private void LoadParameters()
		{
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productName"]))
			{
				this.productName = Globals.UrlDecode(this.Page.Request.QueryString["productName"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
			{
				this.productCode = Globals.UrlDecode(this.Page.Request.QueryString["productCode"]);
			}
			int value = 0;
			if (int.TryParse(this.Page.Request.QueryString["categoryId"], out value))
			{
				this.categoryId = new int?(value);
			}
			int value2 = 0;
			if (int.TryParse(this.Page.Request.QueryString["lineId"], out value2))
			{
				this.lineId = new int?(value2);
			}
			int value3 = 0;
			if (int.TryParse(this.Page.Request.QueryString["brandId"], out value3))
			{
				this.dropBrandList.SelectedValue = new int?(value3);
			}
			int value4 = 0;
			if (int.TryParse(this.Page.Request.QueryString["tagId"], out value4))
			{
				this.tagId = new int?(value4);
			}
			int value5 = 0;
			if (int.TryParse(this.Page.Request.QueryString["typeId"], out value5))
			{
				this.typeId = new int?(value5);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]))
			{
				this.startDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["startDate"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]))
			{
				this.endDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["endDate"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["checkedStatus"]))
			{
				int.TryParse(this.Page.Request.QueryString["checkedStatus"], out this.checkedStatus);
			}
			this.txtSearchText.Text = this.productName;
			this.txtSKU.Text = this.productCode;
			this.dropCategories.DataBind();
			this.dropCategories.SelectedValue = this.categoryId;
			this.dropLines.DataBind();
			this.dropLines.SelectedValue = this.lineId;
			this.dropTagList.DataBind();
			this.dropTagList.SelectedValue = this.tagId;
			this.dropType.DataBind();
			this.dropType.SelectedValue = this.typeId;
			this.calendarStartDate.SelectedDate=this.startDate;
            this.calendarEndDate.SelectedDate = this.endDate;
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
			System.Collections.Generic.IList<int> list2 = new System.Collections.Generic.List<int>();
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
					list2.Add(System.Convert.ToInt32(value));
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
				ProductHelper.DeleteProductTags(System.Convert.ToInt32(value2), null);
				if (list.Count > 0 && ProductHelper.AddProductTags(System.Convert.ToInt32(value2), list, null))
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
				this.ShowMsg("已成功取消了商品的关联商品标签", true);
			}
			this.txtProductTag.Text = "";
		}
		private void BindProducts()
		{
            this.LoadParameters();
            ProductQuery entity = new ProductQuery
            {
                Keywords = this.productName,
                ProductCode = this.productCode,
                CategoryId = this.categoryId,
                StartDate = this.startDate,
                EndDate = this.endDate,
                ProductLineId = this.lineId,
                PageSize = this.pager.PageSize,
                PageIndex = this.pager.PageIndex,
                BrandId = this.dropBrandList.SelectedValue.HasValue ? this.dropBrandList.SelectedValue : null,
                TagId = this.dropTagList.SelectedValue.HasValue ? this.dropTagList.SelectedValue : null,
                SaleStatus = ProductSaleStatus.OnStock,
                SortOrder = SortAction.Desc,
                SortBy = "DisplaySequence",
                TypeId = this.typeId
            };
            if (this.categoryId.HasValue)
            {
                entity.MaiCategoryPath = CatalogHelper.GetCategory(this.categoryId.Value).Path;
            }
            Globals.EntityCoding(entity, true);
            DbQueryResult result = Methods.Supplier_PtSGet(entity, 2, null, null);
            this.grdProducts.DataSource = result.Data;
            this.grdProducts.DataBind();
            this.pager1.TotalRecords = this.pager.TotalRecords = result.TotalRecords;
            int checkedNum = 0;
            int noCheckNum = 0;
            int checkNum = 0;
            int errorReferNum = 0;
            Methods.Supplier_PtCheckNumTjGetUpdate(0, out checkedNum, out noCheckNum, out checkNum, out errorReferNum);
            this.litlChecked.Text = checkedNum.ToString();
            this.litlNoCheckNum.Text = checkNum.ToString();
            this.litlErrorReferNum.Text = errorReferNum.ToString();
		}
	}
}
