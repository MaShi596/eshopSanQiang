using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Entities.Comments;
using Hidistro.Subsites.Comments;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class MyArticleCategories : DistributorPage
	{
		protected Grid grdArticleCategories;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.grdArticleCategories.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdArticleCategories_RowDeleting);
			this.grdArticleCategories.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdArticleCategories_RowCommand);
			if (!this.Page.IsPostBack)
			{
				this.BindArticleCategory();
			}
		}
		private void grdArticleCategories_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			int rowIndex = ((System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer).RowIndex;
			int categoryId = (int)this.grdArticleCategories.DataKeys[rowIndex].Value;
			int displaySequence = int.Parse((this.grdArticleCategories.Rows[rowIndex].FindControl("lblDisplaySequence") as System.Web.UI.WebControls.Literal).Text);
			int num = 0;
			int replaceDisplaySequence = 0;
			if (e.CommandName == "Fall")
			{
				if (rowIndex < this.grdArticleCategories.Rows.Count - 1)
				{
					num = (int)this.grdArticleCategories.DataKeys[rowIndex + 1].Value;
					replaceDisplaySequence = int.Parse((this.grdArticleCategories.Rows[rowIndex + 1].FindControl("lblDisplaySequence") as System.Web.UI.WebControls.Literal).Text);
				}
			}
			else
			{
				if (e.CommandName == "Rise" && rowIndex > 0)
				{
					num = (int)this.grdArticleCategories.DataKeys[rowIndex - 1].Value;
					replaceDisplaySequence = int.Parse((this.grdArticleCategories.Rows[rowIndex - 1].FindControl("lblDisplaySequence") as System.Web.UI.WebControls.Literal).Text);
				}
			}
			if (num > 0)
			{
				SubsiteCommentsHelper.SwapArticleCategorySequence(categoryId, num, displaySequence, replaceDisplaySequence);
				this.BindArticleCategory();
			}
		}
		private void BindArticleCategory()
		{
			this.grdArticleCategories.DataSource = SubsiteCommentsHelper.GetMainArticleCategories();
			this.grdArticleCategories.DataBind();
		}
		private void grdArticleCategories_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			int categoryId = (int)this.grdArticleCategories.DataKeys[e.RowIndex].Value;
			ArticleCategoryInfo articleCategory = SubsiteCommentsHelper.GetArticleCategory(categoryId);
			if (SubsiteCommentsHelper.DeleteArticleCategory(categoryId))
			{
				ResourcesHelper.DeleteImage(articleCategory.IconUrl);
				this.ShowMsg("成功删除了指定的文章分类", true);
			}
			else
			{
				this.ShowMsg("未知错误", false);
			}
			this.BindArticleCategory();
		}
	}
}
