using ASPNET.WebControls;
using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ProductCategory)]
	public class ManageCategories : AdminPage
	{
		protected Grid grdTopCategries;
		protected System.Web.UI.WebControls.LinkButton btnOrder;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.grdTopCategries.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdTopCategries_RowCommand);
			this.grdTopCategries.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.grdTopCategries_RowDataBound);
			this.btnOrder.Click += new System.EventHandler(this.btnOrder_Click);
			if (!this.Page.IsPostBack)
			{
				this.BindData();
			}
		}
		private void btnOrder_Click(object sender, System.EventArgs e)
		{
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdTopCategries.Rows)
			{
				int num = 0;
				System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)gridViewRow.FindControl("txtSequence");
				if (int.TryParse(textBox.Text.Trim(), out num))
				{
					int categoryId = (int)this.grdTopCategries.DataKeys[gridViewRow.RowIndex].Value;
					CategoryInfo category = CatalogHelper.GetCategory(categoryId);
					if (category.DisplaySequence != num)
					{
						CatalogHelper.SwapCategorySequence(categoryId, num);
					}
				}
			}
			HiCache.Remove("DataCache-Categories");
			this.BindData();
		}
		private void grdTopCategries_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
		{
			if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
			{
				int num = (int)System.Web.UI.DataBinder.Eval(e.Row.DataItem, "Depth");
				string text = System.Web.UI.DataBinder.Eval(e.Row.DataItem, "Name").ToString();
				if (num == 1)
				{
					text = "<b>" + text + "</b>";
				}
				else
				{
					System.Web.UI.HtmlControls.HtmlGenericControl htmlGenericControl = e.Row.FindControl("spShowImage") as System.Web.UI.HtmlControls.HtmlGenericControl;
					htmlGenericControl.Visible = false;
				}
				for (int i = 1; i < num; i++)
				{
					text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + text;
				}
				System.Web.UI.WebControls.Literal literal = e.Row.FindControl("lblCategoryName") as System.Web.UI.WebControls.Literal;
				literal.Text = text;
			}
		}
		private void BindData()
		{
			this.grdTopCategries.DataSource = CatalogHelper.GetSequenceCategories();
			this.grdTopCategries.DataBind();
		}
		private void grdTopCategries_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			int rowIndex = ((System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer).RowIndex;
			int categoryId = (int)this.grdTopCategries.DataKeys[rowIndex].Value;
			if (e.CommandName == "DeleteCagetory")
			{
				if (CatalogHelper.DeleteCategory(categoryId))
				{
					this.ShowMsg("成功删除了指定的分类", true);
				}
				else
				{
					this.ShowMsg("分类删除失败，未知错误", false);
				}
			}
			this.BindData();
		}
	}
}
