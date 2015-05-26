using ASPNET.WebControls;
using Hidistro.ControlPanel.Comments;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.HelpCategories)]
	public class HelpCategories : AdminPage
	{
		protected System.Web.UI.WebControls.LinkButton btnorder;
		protected Grid grdHelpCategories;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.grdHelpCategories.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdHelpCategories_RowCommand);
			this.btnorder.Click += new System.EventHandler(this.btnorder_Click);
			if (!this.Page.IsPostBack)
			{
				this.BindHelpCategory();
			}
		}
		private void grdHelpCategories_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			int rowIndex = ((System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer).RowIndex;
			int categoryId = (int)this.grdHelpCategories.DataKeys[rowIndex].Value;
			if (e.CommandName == "Delete")
			{
				ArticleHelper.DeleteHelpCategory(categoryId);
			}
			else
			{
				if (e.CommandName == "SetYesOrNo")
				{
					HelpCategoryInfo helpCategory = ArticleHelper.GetHelpCategory(categoryId);
					if (helpCategory.IsShowFooter)
					{
						helpCategory.IsShowFooter = false;
					}
					else
					{
						helpCategory.IsShowFooter = true;
					}
					ArticleHelper.UpdateHelpCategory(helpCategory);
				}
				else
				{
					int displaySequence = int.Parse((this.grdHelpCategories.Rows[rowIndex].FindControl("lblDisplaySequence") as System.Web.UI.WebControls.Literal).Text);
					int num = 0;
					int replaceDisplaySequence = 0;
					if (e.CommandName == "Fall")
					{
						if (rowIndex < this.grdHelpCategories.Rows.Count - 1)
						{
							num = (int)this.grdHelpCategories.DataKeys[rowIndex + 1].Value;
							replaceDisplaySequence = int.Parse((this.grdHelpCategories.Rows[rowIndex + 1].FindControl("lblDisplaySequence") as System.Web.UI.WebControls.Literal).Text);
						}
					}
					else
					{
						if (e.CommandName == "Rise" && rowIndex > 0)
						{
							num = (int)this.grdHelpCategories.DataKeys[rowIndex - 1].Value;
							replaceDisplaySequence = int.Parse((this.grdHelpCategories.Rows[rowIndex - 1].FindControl("lblDisplaySequence") as System.Web.UI.WebControls.Literal).Text);
						}
					}
					if (num > 0)
					{
						ArticleHelper.SwapHelpCategorySequence(categoryId, num, displaySequence, replaceDisplaySequence);
					}
				}
			}
			base.Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString(), true);
		}
		protected void btnorder_Click(object sender, System.EventArgs e)
		{
			int num = 0;
			for (int i = 0; i < this.grdHelpCategories.Rows.Count; i++)
			{
				int categoryId = (int)this.grdHelpCategories.DataKeys[i].Value;
				int replaceDisplaySequence = int.Parse((this.grdHelpCategories.Rows[i].Cells[2].Controls[1] as System.Web.UI.HtmlControls.HtmlInputText).Value);
				ArticleHelper.SwapHelpCategorySequence(categoryId, 0, 0, replaceDisplaySequence);
				num++;
			}
			base.Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString(), true);
		}
		private void BindHelpCategory()
		{
			System.Collections.Generic.IList<HelpCategoryInfo> helpCategorys = ArticleHelper.GetHelpCategorys();
			this.grdHelpCategories.DataSource = helpCategorys;
			this.grdHelpCategories.DataBind();
		}
	}
}
