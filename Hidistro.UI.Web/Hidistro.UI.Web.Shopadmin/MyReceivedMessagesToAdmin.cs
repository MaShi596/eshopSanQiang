using ASPNET.WebControls;
using Hidistro.Core.Entities;
using Hidistro.Entities.Comments;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core.Enums;
using Hidistro.Subsites.Comments;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class MyReceivedMessagesToAdmin : DistributorPage
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
			this.statusList.AutoPostBack = true;
			if (!this.Page.IsPostBack)
			{
				this.BindData();
			}
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
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
				SubsiteCommentsHelper.DeleteMessages(list);
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
			messageBoxQuery.Accepter = Hidistro.Membership.Context.HiContext.Current.User.Username;
			if (!string.IsNullOrEmpty(base.Request.QueryString["MessageStatus"]))
			{
				messageBoxQuery.MessageStatus = (MessageStatus)int.Parse(base.Request.QueryString["MessageStatus"]);
				this.statusList.SelectedValue = messageBoxQuery.MessageStatus;
			}
			DbQueryResult receivedMessages = SubsiteCommentsHelper.GetReceivedMessages(messageBoxQuery, Hidistro.Membership.Core.Enums.UserRole.SiteManager);
			this.messagesList.DataSource = receivedMessages.Data;
			this.messagesList.DataBind();
			this.pager.TotalRecords=receivedMessages.TotalRecords;
			this.pager1.TotalRecords=receivedMessages.TotalRecords;
		}
	}
}
