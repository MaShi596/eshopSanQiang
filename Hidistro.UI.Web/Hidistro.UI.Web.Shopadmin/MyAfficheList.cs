using ASPNET.WebControls;
using Hidistro.Subsites.Comments;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class MyAfficheList : DistributorPage
	{
		protected ImageLinkButton lkbtnDeleteSelect;
		protected Grid grdAfficheList;
		protected ImageLinkButton lkbtnDeleteSelect1;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.grdAfficheList.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdAfficheList_RowDeleting);
			this.lkbtnDeleteSelect.Click += new System.EventHandler(this.lkbtnDeleteSelect_Click);
			this.lkbtnDeleteSelect1.Click += new System.EventHandler(this.lkbtnDeleteSelect_Click);
			if (!this.Page.IsPostBack)
			{
				this.BindAffiche();
			}
		}
		private void BindAffiche()
		{
			this.grdAfficheList.DataSource = SubsiteCommentsHelper.GetAfficheList();
			this.grdAfficheList.DataBind();
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
		}
		private void lkbtnDeleteSelect_Click(object sender, System.EventArgs e)
		{
			System.Collections.Generic.List<int> list = new System.Collections.Generic.List<int>();
			int num = 0;
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdAfficheList.Rows)
			{
				System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)gridViewRow.FindControl("checkboxCol");
				if (checkBox.Checked)
				{
					num++;
					int item = System.Convert.ToInt32(this.grdAfficheList.DataKeys[gridViewRow.RowIndex].Value, System.Globalization.CultureInfo.InvariantCulture);
					list.Add(item);
				}
			}
			if (num != 0)
			{
				int num2 = SubsiteCommentsHelper.DeleteAffiches(list);
				this.BindAffiche();
				this.ShowMsg(string.Format(System.Globalization.CultureInfo.InvariantCulture, "成功删除了\"{0}\"公告", new object[]
				{
					num2
				}), true);
				return;
			}
			this.ShowMsg("请先选择要删除的公告", false);
		}
		private void grdAfficheList_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			if (SubsiteCommentsHelper.DeleteAffiche((int)this.grdAfficheList.DataKeys[e.RowIndex].Value))
			{
				this.BindAffiche();
				this.ShowMsg("成功删除了选择的公告", true);
				return;
			}
			this.ShowMsg("删除失败", false);
		}
	}
}
