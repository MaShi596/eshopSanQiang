using ASPNET.WebControls;
using Hidistro.AccountCenter.Comments;
using Hidistro.Core.Entities;
using Hidistro.Entities.Comments;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	public class UserReceivedMessages : MemberTemplatedWebControl
	{
		private Common_Messages_UserReceivedMessageList CmessagesList;
		private Grid messagesList;
		private Pager pager;
		private IButton btnDeleteSelect;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserReceivedMessages.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.CmessagesList = (Common_Messages_UserReceivedMessageList)this.FindControl("Grid_Common_Messages_UserReceivedMessageList");
			this.messagesList = (Grid)this.CmessagesList.FindControl("gridMessageList");
			this.pager = (Pager)this.FindControl("pager");
			this.btnDeleteSelect = ButtonManager.Create(this.FindControl("btnDeleteSelect"));
			this.btnDeleteSelect.Click += new System.EventHandler(this.btnDeleteSelect_Click);
			if (!this.Page.IsPostBack)
			{
				PageTitle.AddSiteNameTitle("收件箱", HiContext.Current.Context);
				this.BindData();
			}
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
		}
		private void btnDeleteSelect_Click(object sender, System.EventArgs e)
		{
			System.Collections.Generic.IList<long> list = new System.Collections.Generic.List<long>();
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.messagesList.Rows)
			{
				System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)gridViewRow.FindControl("checkboxCol");
				if (checkBox != null && checkBox.Checked)
				{
					System.Web.UI.WebControls.Label label = (System.Web.UI.WebControls.Label)gridViewRow.FindControl("lblMessage");
					if (label != null)
					{
						list.Add(System.Convert.ToInt64(label.Text));
					}
				}
			}
			if (list.Count > 0)
			{
				CommentsHelper.DeleteMemberMessages(list);
			}
			else
			{
				this.ShowMessage("请选中要删除的收件", false);
			}
			this.BindData();
		}
		private void BindData()
		{
			MessageBoxQuery messageBoxQuery = new MessageBoxQuery();
			messageBoxQuery.PageIndex = this.pager.PageIndex;
			messageBoxQuery.PageSize = this.pager.PageSize;
			messageBoxQuery.Accepter = HiContext.Current.User.Username;
			DbQueryResult memberReceivedMessages = CommentsHelper.GetMemberReceivedMessages(messageBoxQuery);
			if (((DataTable)memberReceivedMessages.Data).Rows.Count <= 0)
			{
				messageBoxQuery.PageIndex = this.messagesList.PageIndex - 1;
				memberReceivedMessages = CommentsHelper.GetMemberReceivedMessages(messageBoxQuery);
				this.messagesList.DataSource = memberReceivedMessages.Data;
			}
			this.messagesList.DataSource = memberReceivedMessages.Data;
			this.messagesList.DataBind();
            this.pager.TotalRecords = memberReceivedMessages.TotalRecords;
		}
	}
}
