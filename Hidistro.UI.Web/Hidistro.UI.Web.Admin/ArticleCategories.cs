using ASPNET.WebControls;
using Hidistro.ControlPanel.Comments;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ArticleCategories)]
	public class ArticleCategories : AdminPage
	{
		protected System.Web.UI.WebControls.LinkButton btnorder;
		protected Grid grdArticleCategories;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.grdArticleCategories.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdArticleCategories_RowCommand);
			this.btnorder.Click += new System.EventHandler(this.btnorder_Click);
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
				ArticleHelper.SwapArticleCategorySequence(categoryId, num, displaySequence, replaceDisplaySequence);
			}
			if (e.CommandName == "Delete")
			{
				ArticleCategoryInfo articleCategory = ArticleHelper.GetArticleCategory(categoryId);
				if (ArticleHelper.DeleteArticleCategory(categoryId))
				{
					ResourcesHelper.DeleteImage(articleCategory.IconUrl);
				}
			}
			base.Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString(), true);
		}
		protected void btnorder_Click(object sender, System.EventArgs e)
		{
			int num = 0;
			for (int i = 0; i < this.grdArticleCategories.Rows.Count; i++)
			{
				int categoryId = (int)this.grdArticleCategories.DataKeys[i].Value;
				int replaceDisplaySequence = int.Parse((this.grdArticleCategories.Rows[i].Cells[3].Controls[1] as System.Web.UI.HtmlControls.HtmlInputText).Value);
				ArticleHelper.SwapArticleCategorySequence(categoryId, 0, 0, replaceDisplaySequence);
				num++;
			}
			base.Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString(), true);
		}
		private void BindArticleCategory()
		{
			this.grdArticleCategories.DataSource = ArticleHelper.GetMainArticleCategories();
			this.grdArticleCategories.DataBind();
		}
	}
}
