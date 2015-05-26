using ASPNET.WebControls;
using Hidistro.AccountCenter.Comments;
using Hidistro.Core.Entities;
using Hidistro.Entities.Comments;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	public class UserSendedMessages : MemberTemplatedWebControl
	{
		private Common_Messages_UserSendedMessageList CmessagesList;
		private Grid messagesList;
		private Pager pager;
		private IButton btnDeleteSelect;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserSendedMessages.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.CmessagesList = (Common_Messages_UserSendedMessageList)this.FindControl("Grid_Common_Messages_UserSendedMessageList");
			this.messagesList = (Grid)this.CmessagesList.FindControl("messagesList");
			this.pager = (Pager)this.FindControl("pager");
			this.btnDeleteSelect = ButtonManager.Create(this.FindControl("btnDeleteSelect"));
			this.btnDeleteSelect.Click += new System.EventHandler(this.btnDeleteSelect_Click);
			if (!this.Page.IsPostBack)
			{
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
					list.Add(System.Convert.ToInt64(this.messagesList.DataKeys[gridViewRow.RowIndex].Value));
				}
			}
			if (list.Count > 0)
			{
				CommentsHelper.DeleteMemberMessages(list);
				this.BindData();
			}
			else
			{
				this.ShowMessage("请选中要删除的信息", false);
			}
		}
		private void BindData()
		{
			DbQueryResult memberSendedMessages = CommentsHelper.GetMemberSendedMessages(new MessageBoxQuery
			{
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize,
				Sernder = HiContext.Current.User.Username
			});
			this.messagesList.DataSource = memberSendedMessages.Data;
			this.messagesList.DataBind();
            this.pager.TotalRecords = memberSendedMessages.TotalRecords;
		}
	}
}
