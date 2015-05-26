using ASPNET.WebControls;
using Hidistro.Core.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Promotions;
using Hidistro.Subsites.Commodities;
using Hidistro.Subsites.Promotions;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class SetMyPromotionProducts : DistributorPage
	{
		private int activityId;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hdactivy;
		protected System.Web.UI.WebControls.Literal litPromotionName;
		protected ImageLinkButton btnDeleteAll;
		protected Grid grdPromotionProducts;
		protected System.Web.UI.WebControls.Button btnFinesh;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!string.IsNullOrEmpty(base.Request.QueryString["isCallback"]) && base.Request.QueryString["isCallback"] == "true")
			{
				this.DoCallback();
				return;
			}
			if (!int.TryParse(this.Page.Request.QueryString["activityId"], out this.activityId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.btnFinesh.Click += new System.EventHandler(this.btnFinesh_Click);
			this.hdactivy.Value = this.activityId.ToString();
			this.btnDeleteAll.Click += new System.EventHandler(this.btnDeleteAll_Click);
			this.grdPromotionProducts.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdPromotionProducts_RowDeleting);
			if (!this.Page.IsPostBack)
			{
				PromotionInfo promotion = SubsitePromoteHelper.GetPromotion(this.activityId);
				if (promotion == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				this.litPromotionName.Text = promotion.Name;
				this.BindPromotionProducts();
			}
		}
		private void btnFinesh_Click(object sender, System.EventArgs e)
		{
			base.Response.Redirect("myproductpromotions.aspx", true);
		}
		private void grdPromotionProducts_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			if (SubsitePromoteHelper.DeletePromotionProducts(this.activityId, new int?((int)this.grdPromotionProducts.DataKeys[e.RowIndex].Value)))
			{
				base.Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString(), true);
			}
		}
		private void BindPromotionProducts()
		{
			this.grdPromotionProducts.DataSource = SubsitePromoteHelper.GetPromotionProducts(this.activityId);
			this.grdPromotionProducts.DataBind();
		}
		private void btnDeleteAll_Click(object sender, System.EventArgs e)
		{
			if (SubsitePromoteHelper.DeletePromotionProducts(this.activityId, null))
			{
				base.Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString(), true);
			}
		}
		protected void DoCallback()
		{
			this.Page.Response.Clear();
			base.Response.ContentType = "text/json";
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			int num = 0;
			int num2 = 0;
			int pageIndex = 1;
			int.TryParse(base.Request.Params["categoryId"], out num);
			int.TryParse(base.Request.Params["brandId"], out num2);
			int.TryParse(base.Request.Params["page"], out pageIndex);
			ProductQuery productQuery = new ProductQuery();
			productQuery.PageSize = 15;
			productQuery.PageIndex = pageIndex;
			productQuery.SaleStatus = ProductSaleStatus.OnSale;
			productQuery.IsIncludePromotionProduct = new bool?(false);
			productQuery.IsIncludeBundlingProduct = new bool?(false);
			productQuery.Keywords = base.Request.Params["serachName"];
			if (num2 != 0)
			{
				productQuery.BrandId = new int?(num2);
			}
			productQuery.CategoryId = new int?(num);
			if (num != 0)
			{
				productQuery.MaiCategoryPath = SubsiteCatalogHelper.GetCategory(num).Path;
			}
			DbQueryResult products = SubSiteProducthelper.GetProducts(productQuery);
			System.Data.DataTable dataTable = (System.Data.DataTable)products.Data;
			stringBuilder.Append("{'data':[");
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				stringBuilder.Append("{'ProductId':'");
				stringBuilder.Append(dataTable.Rows[i]["ProductId"].ToString().Trim());
				stringBuilder.Append("','Name':'");
				stringBuilder.Append(dataTable.Rows[i]["ProductName"].ToString());
				stringBuilder.Append("','Price':'");
				stringBuilder.Append(((decimal)dataTable.Rows[i]["SalePrice"]).ToString("F2"));
				stringBuilder.Append("','Stock':'");
				stringBuilder.Append(dataTable.Rows[i]["Stock"].ToString());
				stringBuilder.Append("'},");
			}
			stringBuilder.Append("],'recCount':'");
			stringBuilder.Append(products.TotalRecords);
			stringBuilder.Append("'}");
			base.Response.Write(stringBuilder.ToString());
			base.Response.End();
		}
	}
}
