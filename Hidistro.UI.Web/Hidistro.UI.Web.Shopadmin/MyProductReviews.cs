using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities.Comments;
using Hidistro.Subsites.Comments;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class MyProductReviews : DistributorPage
	{
		private string keywords = string.Empty;
		private int? categoryId;
		private string productCode;
		protected System.Web.UI.WebControls.TextBox txtSearchText;
		protected DistributorProductCategoriesDropDownList dropCategories;
		protected System.Web.UI.WebControls.TextBox txtSKU;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected PageSize hrefPageSize;
		protected Pager pager1;
		protected System.Web.UI.WebControls.DataList dlstPtReviews;
		protected Pager pager;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.SetSearchControl();
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			this.dlstPtReviews.DeleteCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.dlstPtReviews_DeleteCommand);
		}
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReloadProductReviews(true);
		}
		private void dlstPtReviews_DeleteCommand(object sender, System.Web.UI.WebControls.DataListCommandEventArgs e)
		{
			int num = System.Convert.ToInt32(e.CommandArgument, System.Globalization.CultureInfo.InvariantCulture);
			if (SubsiteCommentsHelper.DeleteProductReview((long)num) > 0)
			{
				this.ShowMsg("成功删除了选择的商品评论回复", true);
				this.BindPtReview();
				return;
			}
			this.ShowMsg("删除失败", false);
		}
		private void BindPtReview()
		{
			ProductReviewQuery productReviewQuery = new ProductReviewQuery();
			productReviewQuery.Keywords = this.keywords;
			productReviewQuery.CategoryId = this.categoryId;
			productReviewQuery.ProductCode = this.productCode;
			productReviewQuery.PageIndex = this.pager.PageIndex;
			productReviewQuery.PageSize = this.pager.PageSize;
			productReviewQuery.SortOrder = SortAction.Desc;
			productReviewQuery.SortBy = "ReviewDate";
			int totalRecords = 0;
			Globals.EntityCoding(productReviewQuery, true);
			System.Data.DataSet productReviews = SubsiteCommentsHelper.GetProductReviews(out totalRecords, productReviewQuery);
			this.dlstPtReviews.DataSource = productReviews.Tables[0].DefaultView;
			this.dlstPtReviews.DataBind();
            this.pager.TotalRecords = totalRecords;
            this.pager1.TotalRecords = totalRecords;
		}
		private void ReloadProductReviews(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("Keywords", this.txtSearchText.Text.Trim());
			nameValueCollection.Add("CategoryId", this.dropCategories.SelectedValue.ToString());
			nameValueCollection.Add("productCode", this.txtSKU.Text.Trim());
			if (!isSearch)
			{
				nameValueCollection.Add("PageIndex", this.pager.PageIndex.ToString());
			}
			nameValueCollection.Add("PageSize", this.pager.PageSize.ToString());
			base.ReloadPage(nameValueCollection);
		}
		private void SetSearchControl()
		{
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Keywords"]))
				{
					this.keywords = base.Server.UrlDecode(this.Page.Request.QueryString["Keywords"]);
				}
				int value = 0;
				if (int.TryParse(this.Page.Request.QueryString["CategoryId"], out value))
				{
					this.categoryId = new int?(value);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["productCode"]))
				{
					this.productCode = base.Server.UrlDecode(this.Page.Request.QueryString["productCode"]);
				}
				this.txtSearchText.Text = this.keywords;
				this.txtSKU.Text = this.productCode;
				this.dropCategories.DataBind();
				this.dropCategories.SelectedValue = this.categoryId;
				this.BindPtReview();
				return;
			}
			this.keywords = this.txtSearchText.Text;
			this.productCode = this.txtSKU.Text;
			this.categoryId = this.dropCategories.SelectedValue;
		}
	}
}
