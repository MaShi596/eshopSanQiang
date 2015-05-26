using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Commodities;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class ManageMyCategories : DistributorPage
	{
		protected System.Web.UI.WebControls.LinkButton btnDownload;
		protected System.Web.UI.WebControls.LinkButton btnOrder;
		protected Grid grdTopCategries;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.grdTopCategries.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdTopCategries_RowCommand);
			this.grdTopCategries.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.grdTopCategries_RowDataBound);
			this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
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
					CategoryInfo category = SubsiteCatalogHelper.GetCategory(categoryId);
					if (category.DisplaySequence != num)
					{
						SubsiteCatalogHelper.SwapCategorySequence(categoryId, num);
					}
				}
			}
			HiCache.Remove(string.Format("DataCache-SubsiteCategories{0}", Hidistro.Membership.Context.HiContext.Current.User.UserId));
			this.BindData();
		}
		private void btnDownload_Click(object sender, System.EventArgs e)
		{
			if (this.grdTopCategries.Rows.Count > 0)
			{
				this.ShowMsg("子站已有商品分类，请先删除所有商品分类再下载！", false);
				return;
			}
			int num = SubsiteCatalogHelper.DownloadCategory();
			if (num > 0)
			{
				this.grdTopCategries.SelectedIndex = -1;
				this.BindData();
				this.ShowMsg("成功下载了主站分类", true);
				return;
			}
			this.ShowMsg("商品没有铺货，主站商品分类下载失败", false);
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
					text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + text;
				}
				System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Row.FindControl("lblCategoryName");
				literal.Text = text;
			}
		}
		private void BindData()
		{
			this.grdTopCategries.DataSource = SubsiteCatalogHelper.GetSequenceCategories();
			this.grdTopCategries.DataBind();
		}
		private void grdTopCategries_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			int rowIndex = ((System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer).RowIndex;
			int categoryId = (int)this.grdTopCategries.DataKeys[rowIndex].Value;
			if (e.CommandName == "DeleteCategory")
			{
				if (SubsiteCatalogHelper.DeleteCategory(categoryId))
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
