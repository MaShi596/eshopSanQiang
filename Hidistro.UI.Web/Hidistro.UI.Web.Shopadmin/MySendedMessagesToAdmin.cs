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
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class MySendedMessagesToAdmin : DistributorPage
	{
		protected PageSize hrefPageSize;
		protected Pager pager1;
		protected ImageLinkButton btnDeleteSelect;
		protected Grid messagesList;
		protected ImageLinkButton btnDeleteSelect1;
		protected Pager pager;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnDeleteSelect.Click += new System.EventHandler(this.btnDeleteSelect_Click);
			this.btnDeleteSelect1.Click += new System.EventHandler(this.btnDeleteSelect_Click);
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
			DbQueryResult sendedMessages = SubsiteCommentsHelper.GetSendedMessages(new MessageBoxQuery
			{
				PageIndex = this.pager.PageIndex,
				PageSize = this.pager.PageSize,
				Sernder = Hidistro.Membership.Context.HiContext.Current.User.Username
			}, Hidistro.Membership.Core.Enums.UserRole.SiteManager);
			this.messagesList.DataSource = sendedMessages.Data;
			this.messagesList.DataBind();
            this.pager.TotalRecords = sendedMessages.TotalRecords;
            this.pager1.TotalRecords = sendedMessages.TotalRecords;
		}
	}
}
