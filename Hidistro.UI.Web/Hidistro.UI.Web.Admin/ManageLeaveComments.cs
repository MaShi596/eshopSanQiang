using ASPNET.WebControls;
using Hidistro.ControlPanel.Comments;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Entities;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ManageLeaveComments)]
	public class ManageLeaveComments : AdminPage
	{
		protected PageSize hrefPageSize;
		protected Pager pager1;
		protected ImageLinkButton btnDeleteSelect1;
		protected MessageStatusDropDownList statusList;
		protected Grid leaveList;
		protected ImageLinkButton btnDeleteSelect;
		protected Pager pager;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.leaveList.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.leaveList_RowDeleting);
			this.btnDeleteSelect.Click += new System.EventHandler(this.btnDeleteSelect_Click);
			this.btnDeleteSelect1.Click += new System.EventHandler(this.btnDeleteSelect_Click);
			this.statusList.SelectedIndexChanged += new System.EventHandler(this.statusList_SelectedIndexChanged);
			this.statusList.AutoPostBack = true;
			if (!this.Page.IsPostBack)
			{
				this.BindList();
			}
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
		}
		private void statusList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			base.ReloadPage(new System.Collections.Specialized.NameValueCollection
			{

				{
					"MessageStatus",
					((int)this.statusList.SelectedValue).ToString()
				}
			});
		}
		private void leaveList_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			long leaveId = (long)this.leaveList.DataKeys[e.RowIndex].Value;
			if (NoticeHelper.DeleteLeaveComment(leaveId))
			{
				this.ShowMsg("删除成功", true);
				this.BindList();
				return;
			}
			this.ShowMsg("删除失败", true);
		}
		private void BindList()
		{
			LeaveCommentQuery leaveCommentQuery = new LeaveCommentQuery();
			leaveCommentQuery.PageIndex = this.pager.PageIndex;
			leaveCommentQuery.PageSize = this.pager.PageSize;
			if (!string.IsNullOrEmpty(base.Request.QueryString["MessageStatus"]))
			{
				leaveCommentQuery.MessageStatus = (MessageStatus)int.Parse(base.Request.QueryString["MessageStatus"]);
				this.statusList.SelectedValue = leaveCommentQuery.MessageStatus;
			}
			DbQueryResult leaveComments = NoticeHelper.GetLeaveComments(leaveCommentQuery);
			this.leaveList.DataSource = leaveComments.Data;
			this.leaveList.DataBind();
			this.pager.TotalRecords=leaveComments.TotalRecords;
			this.pager1.TotalRecords=leaveComments.TotalRecords;
		}
		private void btnDeleteSelect_Click(object sender, System.EventArgs e)
		{
			System.Collections.Generic.IList<long> list = new System.Collections.Generic.List<long>();
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.leaveList.Rows)
			{
				System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)gridViewRow.FindControl("checkboxCol");
				if (checkBox != null && checkBox.Checked)
				{
					long item = (long)this.leaveList.DataKeys[gridViewRow.RowIndex].Value;
					list.Add(item);
				}
			}
			if (list.Count > 0)
			{
				NoticeHelper.DeleteLeaveComments(list);
				this.ShowMsg("成功删除了选择的消息.", true);
			}
			else
			{
				this.ShowMsg("请选择需要删除的消息.", false);
			}
			this.BindList();
		}
	}
}
