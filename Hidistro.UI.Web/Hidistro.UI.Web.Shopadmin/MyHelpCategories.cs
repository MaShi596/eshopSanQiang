using ASPNET.WebControls;
using Hidistro.Entities.Comments;
using Hidistro.Subsites.Comments;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class MyHelpCategories : DistributorPage
	{
		protected Grid grdHelpCategories;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.grdHelpCategories.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdHelpCategories_RowCommand);
			this.grdHelpCategories.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdHelpCategories_RowDeleting);
			if (!this.Page.IsPostBack)
			{
				this.BindHelpCategory();
			}
		}
		private void grdHelpCategories_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			if (SubsiteCommentsHelper.DeleteHelpCategory((int)this.grdHelpCategories.DataKeys[e.RowIndex].Value))
			{
				this.BindHelpCategory();
				this.ShowMsg("成功删除了选择的帮助分类", true);
				return;
			}
			this.ShowMsg("未知错误", false);
		}
		private void grdHelpCategories_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			int rowIndex = ((System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer).RowIndex;
			int categoryId = (int)this.grdHelpCategories.DataKeys[rowIndex].Value;
			if (e.CommandName == "SetYesOrNo")
			{
				HelpCategoryInfo helpCategory = SubsiteCommentsHelper.GetHelpCategory(categoryId);
				if (helpCategory.IsShowFooter)
				{
					helpCategory.IsShowFooter = false;
				}
				else
				{
					helpCategory.IsShowFooter = true;
				}
				SubsiteCommentsHelper.UpdateHelpCategory(helpCategory);
				this.BindHelpCategory();
				return;
			}
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
				SubsiteCommentsHelper.SwapHelpCategorySequence(categoryId, num, displaySequence, replaceDisplaySequence);
				this.BindHelpCategory();
			}
		}
		private void BindHelpCategory()
		{
			System.Collections.Generic.IList<HelpCategoryInfo> helpCategorys = SubsiteCommentsHelper.GetHelpCategorys();
			this.grdHelpCategories.DataSource = helpCategorys;
			this.grdHelpCategories.DataBind();
		}
	}
}
