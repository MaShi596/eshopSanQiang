using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Products)]
	public class ProductOnSales : AdminPage
	{
		private string productName;
		private string productCode;
		private int? categoryId;
		private int? lineId;
		private int? tagId;
		private int? typeId;
		private int? distributorId;
		private bool isAlert;
		private ProductSaleStatus saleStatus = ProductSaleStatus.All;
		private PenetrationStatus penetrationStatus;
		private System.DateTime? startDate;
		private System.DateTime? endDate;
		protected System.Web.UI.WebControls.TextBox txtSearchText;
		protected ProductCategoriesDropDownList dropCategories;
		protected ProductLineDropDownList dropLines;
		protected BrandCategoriesDropDownList dropBrandList;
		protected ProductTagsDropDownList dropTagList;
		protected ProductTypeDownList dropType;
		protected System.Web.UI.WebControls.TextBox txtSKU;
		protected WebCalendar calendarStartDate;
		protected WebCalendar calendarEndDate;
		protected DistributorDropDownList dropDistributor;
		protected System.Web.UI.WebControls.CheckBox chkIsAlert;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected PageSize hrefPageSize;
		protected Pager pager1;
		protected ImageLinkButton btnDelete;
		protected System.Web.UI.WebControls.HyperLink btnDownTaobao;
		protected SaleStatusDropDownList dropSaleStatus;
		protected PenetrationStatusDropDownList dropPenetrationStatus;
		protected Grid grdProducts;
		protected Pager pager;
		protected System.Web.UI.WebControls.CheckBox chkDeleteImage;
		protected System.Web.UI.WebControls.CheckBox chkInstock;
		protected ProductTagsLiteral litralProductTag;
		protected ProductLineDropDownList dropProductLines;
		protected System.Web.UI.WebControls.Button btnUpdateProductTags;
		protected TrimTextBox txtProductTag;
		protected System.Web.UI.WebControls.Button btnInStock;
		protected System.Web.UI.WebControls.Button btnUnSale;
		protected System.Web.UI.WebControls.Button btnUpSale;
		protected System.Web.UI.WebControls.Button btnPenetration;
		protected System.Web.UI.WebControls.Button btnCancle;
		protected System.Web.UI.WebControls.Button btnUpdateLine;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hdProductLine;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hdPenetrationStatus;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
			this.btnUpSale.Click += new System.EventHandler(this.btnUpSale_Click);
			this.btnUnSale.Click += new System.EventHandler(this.btnUnSale_Click);
			this.btnInStock.Click += new System.EventHandler(this.btnInStock_Click);
			this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
			this.btnPenetration.Click += new System.EventHandler(this.btnPenetration_Click);
			this.btnUpdateProductTags.Click += new System.EventHandler(this.btnUpdateProductTags_Click);
			this.btnUpdateLine.Click += new System.EventHandler(this.btnUpdateLine_Click);
			this.grdProducts.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.grdProducts_RowDataBound);
			this.grdProducts.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdProducts_RowDeleting);
			this.dropSaleStatus.SelectedIndexChanged += new System.EventHandler(this.dropSaleStatus_SelectedIndexChanged);
			this.dropPenetrationStatus.SelectedIndexChanged += new System.EventHandler(this.dropPenetrationStatus_SelectedIndexChanged);
			if (!this.Page.IsPostBack)
			{
				this.dropCategories.DataBind();
				this.dropLines.DataBind();
				this.dropBrandList.DataBind();
				this.dropTagList.DataBind();
				this.dropType.DataBind();
				this.dropDistributor.DataBind();
				this.dropPenetrationStatus.DataBind();
				this.dropSaleStatus.DataBind();
				this.dropProductLines.DataBind();
				this.btnDownTaobao.NavigateUrl = string.Format("http://order1.kuaidiangtong.com/TaoBaoApi.aspx?Host={0}&ApplicationPath={1}", Hidistro.Membership.Context.HiContext.Current.SiteSettings.SiteUrl, Globals.ApplicationPath);
				this.BindProducts();
			}
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
		}
		private void dropSaleStatus_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.ReloadProductOnSales(true);
		}
		private void dropPenetrationStatus_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.ReloadProductOnSales(true);
		}
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReloadProductOnSales(true);
		}
		private void grdProducts_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
		{
			if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
			{
				System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Row.FindControl("litSaleStatus");
				System.Web.UI.WebControls.Literal literal2 = (System.Web.UI.WebControls.Literal)e.Row.FindControl("litMarketPrice");
				if (literal.Text == "1")
				{
					literal.Text = "出售中";
				}
				else
				{
					if (literal.Text == "2")
					{
						literal.Text = "下架区";
					}
					else
					{
						literal.Text = "仓库中";
					}
				}
				if (string.IsNullOrEmpty(literal2.Text))
				{
					literal2.Text = "-";
				}
			}
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
				this.ReloadProductOnSales(false);
			}
		}
		private void btnUpSale_Click(object sender, System.EventArgs e)
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
		private void btnUnSale_Click(object sender, System.EventArgs e)
		{
			string text = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("请先选择要下架的商品", false);
				return;
			}
			if (this.hdPenetrationStatus.Value.Equals("1"))
			{
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
		private void btnInStock_Click(object sender, System.EventArgs e)
		{
			string text = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("请先选择要入库的商品", false);
				return;
			}
			if (this.hdPenetrationStatus.Value.Equals("1"))
			{
				SendMessageHelper.SendMessageToDistributors(text, 2);
				if (ProductHelper.CanclePenetrationProducts(text) == 0)
				{
					this.ShowMsg("取消铺货失败！", false);
					return;
				}
			}
			int num = ProductHelper.InStock(text);
			if (num > 0)
			{
				this.ShowMsg("成功入库选择的商品，您可以在仓库区的商品里面找到入库以后的商品", true);
				this.BindProducts();
				return;
			}
			this.ShowMsg("入库商品失败，未知错误", false);
		}
		private void btnPenetration_Click(object sender, System.EventArgs e)
		{
			string text = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("请先选择要下架的商品", false);
				return;
			}
			int num = ProductHelper.PenetrationProducts(text);
			if (num > 0)
			{
				this.ShowMsg("铺货成功", true);
				this.BindProducts();
				return;
			}
			this.ShowMsg("铺货失败，未知错误", false);
		}
		private void btnCancle_Click(object sender, System.EventArgs e)
		{
			string text = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("请先选择要下架的商品", false);
				return;
			}
			SendMessageHelper.SendMessageToDistributors(text, 5);
			int num = ProductHelper.CanclePenetrationProducts(text);
			if (num > 0)
			{
				this.ShowMsg("取消铺货成功", true);
				this.BindProducts();
				return;
			}
			this.ShowMsg("取消铺货失败，未知错误", false);
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
			int num = ProductHelper.RemoveProduct(text);
			if (num > 0)
			{
				this.ShowMsg("成功删除了选择的商品", true);
				this.BindProducts();
			}
			else
			{
				this.ShowMsg("删除商品失败，未知错误", false);
			}
			if (this.hdPenetrationStatus.Value.Equals("1"))
			{
				ProductHelper.CanclePenetrationProducts(text);
			}
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
		private void btnUpdateLine_Click(object sender, System.EventArgs e)
		{
			int num = 0;
			string text = base.Request.Form["CheckBoxGroup"];
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("请先选择要转移的商品", false);
				return;
			}
			SendMessageHelper.SendMessageToDistributors(text + "|" + this.dropProductLines.SelectedItem.Text, 6);
			string[] array = text.Split(new char[]
			{
				','
			});
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string s = array2[i];
				if (ProductLineHelper.UpdateProductLine(System.Convert.ToInt32(this.hdProductLine.Value), int.Parse(s)))
				{
					num++;
				}
			}
			if (num > 0)
			{
				this.BindProducts();
				this.ShowMsg(string.Format("成功转移了{0}件商品", num), true);
				return;
			}
			this.ShowMsg("转移商品失败", false);
		}
		private void BindProducts()
		{
            this.LoadParameters();
            ProductQuery entity = new ProductQuery
            {
                Keywords = this.productName,
                ProductCode = this.productCode,
                CategoryId = this.categoryId,
                ProductLineId = this.lineId,
                PageSize = this.pager.PageSize,
                PageIndex = this.pager.PageIndex,
                SortOrder = SortAction.Desc,
                SortBy = "DisplaySequence",
                StartDate = this.startDate,
                BrandId = this.dropBrandList.SelectedValue.HasValue ? this.dropBrandList.SelectedValue : null,
                TagId = this.dropTagList.SelectedValue.HasValue ? this.dropTagList.SelectedValue : null,
                TypeId = this.typeId,
                UserId = this.distributorId,
                IsAlert = this.isAlert,
                SaleStatus = this.saleStatus,
                PenetrationStatus = this.penetrationStatus,
                EndDate = this.endDate
            };
            if (this.categoryId.HasValue)
            {
                entity.MaiCategoryPath = CatalogHelper.GetCategory(this.categoryId.Value).Path;
            }
            Globals.EntityCoding(entity, true);
            DbQueryResult products = ProductHelper.GetProducts(entity);
            this.grdProducts.DataSource = products.Data;
            this.grdProducts.DataBind();
            this.txtSearchText.Text = entity.Keywords;
            this.txtSKU.Text = entity.ProductCode;
            this.dropCategories.SelectedValue = entity.CategoryId;
            this.dropLines.SelectedValue = entity.ProductLineId;
            this.dropType.SelectedValue = entity.TypeId;
            this.dropDistributor.SelectedValue = entity.UserId;
            this.chkIsAlert.Checked = entity.IsAlert;
            this.pager1.TotalRecords = this.pager.TotalRecords = products.TotalRecords;
		}
		private void ReloadProductOnSales(bool isSearch)
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
			nameValueCollection.Add("productCode", Globals.UrlEncode(Globals.HtmlEncode(this.txtSKU.Text.Trim())));
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
			if (this.dropTagList.SelectedValue.HasValue)
			{
				nameValueCollection.Add("tagId", this.dropTagList.SelectedValue.ToString());
			}
			if (this.dropType.SelectedValue.HasValue)
			{
				nameValueCollection.Add("typeId", this.dropType.SelectedValue.ToString());
			}
			if (this.dropDistributor.SelectedValue.HasValue)
			{
				nameValueCollection.Add("distributorId", this.dropDistributor.SelectedValue.ToString());
			}
			nameValueCollection.Add("isAlert", this.chkIsAlert.Checked.ToString());
			nameValueCollection.Add("SaleStatus", this.dropSaleStatus.SelectedValue.ToString());
			nameValueCollection.Add("PenetrationStatus", this.dropPenetrationStatus.SelectedValue.ToString());
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
			if (int.TryParse(this.Page.Request.QueryString["brandId"], out value2))
			{
				this.dropBrandList.SelectedValue = new int?(value2);
			}
			int value3 = 0;
			if (int.TryParse(this.Page.Request.QueryString["lineId"], out value3))
			{
				this.lineId = new int?(value3);
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
			int value6 = 0;
			if (int.TryParse(this.Page.Request.QueryString["distributorId"], out value6))
			{
				this.distributorId = new int?(value6);
			}
			bool.TryParse(this.Page.Request.QueryString["isAlert"], out this.isAlert);
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SaleStatus"]))
			{
				this.saleStatus = (ProductSaleStatus)System.Enum.Parse(typeof(ProductSaleStatus), this.Page.Request.QueryString["SaleStatus"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["PenetrationStatus"]))
			{
				this.penetrationStatus = (PenetrationStatus)System.Enum.Parse(typeof(PenetrationStatus), this.Page.Request.QueryString["PenetrationStatus"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]))
			{
				this.startDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["startDate"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]))
			{
				this.endDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["endDate"]));
			}
			this.txtSearchText.Text = this.productName;
			this.txtSKU.Text = this.productCode;
			this.dropCategories.DataBind();
			this.dropCategories.SelectedValue = this.categoryId;
			this.dropLines.DataBind();
			this.dropLines.SelectedValue = this.lineId;
			this.dropTagList.DataBind();
			this.dropTagList.SelectedValue = this.tagId;
			this.calendarStartDate.SelectedDate=this.startDate;
			this.calendarEndDate.SelectedDate=this.endDate;
			this.dropType.SelectedValue = this.typeId;
			this.dropDistributor.SelectedValue = this.distributorId;
			this.chkIsAlert.Checked = this.isAlert;
			this.dropPenetrationStatus.SelectedValue = this.penetrationStatus;
			this.dropSaleStatus.SelectedValue = this.saleStatus;
		}
	}
}
