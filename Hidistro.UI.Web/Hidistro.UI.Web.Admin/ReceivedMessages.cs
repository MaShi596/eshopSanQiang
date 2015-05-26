using ASPNET.WebControls;
using Hidistro.ControlPanel.Comments;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Entities;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Store;
using Hidistro.Membership.Core.Enums;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ReceivedMessages)]
	public class ReceivedMessages : AdminPage
	{
		protected PageSize hrefPageSize;
		protected Pager pager1;
		protected ImageLinkButton btnDeleteSelect1;
		protected MessageStatusDropDownList statusList;
		protected Grid messagesList;
		protected ImageLinkButton btnDeleteSelect;
		protected Pager pager;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnDeleteSelect.Click += new System.EventHandler(this.btnDeleteSelect_Click);
			this.btnDeleteSelect1.Click += new System.EventHandler(this.btnDeleteSelect_Click);
			this.statusList.SelectedIndexChanged += new System.EventHandler(this.statusList_SelectedIndexChanged);
			if (!this.Page.IsPostBack)
			{
				this.statusList.DataBind();
				this.statusList.AutoPostBack = true;
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
					long item = (long)this.messagesList.DataKeys[gridViewRow.RowIndex].Value;
					list.Add(item);
				}
			}
			if (list.Count > 0)
			{
				NoticeHelper.DeleteManagerMessages(list);
				this.ShowMsg("成功删除了选择的消息.", true);
			}
			else
			{
				this.ShowMsg("请选择需要删除的消息.", false);
			}
			this.BindData();
		}
		private void BindData()
		{
			MessageBoxQuery messageBoxQuery = new MessageBoxQuery();
			messageBoxQuery.PageIndex = this.pager.PageIndex;
			messageBoxQuery.PageSize = this.pager.PageSize;
			messageBoxQuery.Accepter = "admin";
			if (!string.IsNullOrEmpty(base.Request.QueryString["MessageStatus"]))
			{
				messageBoxQuery.MessageStatus = (MessageStatus)int.Parse(base.Request.QueryString["MessageStatus"]);
				this.statusList.SelectedValue = messageBoxQuery.MessageStatus;
			}
			DbQueryResult managerReceivedMessages = NoticeHelper.GetManagerReceivedMessages(messageBoxQuery, Hidistro.Membership.Core.Enums.UserRole.Member);
			this.messagesList.DataSource = managerReceivedMessages.Data;
			this.messagesList.DataBind();
			this.pager.TotalRecords=managerReceivedMessages.TotalRecords;
			this.pager1.TotalRecords=managerReceivedMessages.TotalRecords;
		}
		private void statusList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("MessageStatus", ((int)this.statusList.SelectedValue).ToString());
			if (!string.IsNullOrEmpty(base.Request.QueryString["IsRead"]))
			{
				nameValueCollection.Add("IsRead", base.Request.QueryString["IsRead"]);
			}
			base.ReloadPage(nameValueCollection);
		}
	}
}
