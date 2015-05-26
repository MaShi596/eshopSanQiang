using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.FriendlyLinks)]
	public class FriendlyLinks : AdminPage
	{
		protected Grid grdGroupList;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.grdGroupList.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdGroupList_RowCommand);
			this.grdGroupList.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdGroupList_RowDeleting);
			if (!this.Page.IsPostBack)
			{
				this.BindFriendlyLinks();
			}
		}
		private void BindFriendlyLinks()
		{
			System.Collections.Generic.IList<FriendlyLinksInfo> friendlyLinks = StoreHelper.GetFriendlyLinks();
			this.grdGroupList.DataSource = friendlyLinks;
			this.grdGroupList.DataBind();
		}
		private void grdGroupList_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			int linkId = (int)this.grdGroupList.DataKeys[e.RowIndex].Value;
			FriendlyLinksInfo friendlyLink = StoreHelper.GetFriendlyLink(linkId);
			if (StoreHelper.FriendlyLinkDelete(linkId) > 0)
			{
				try
				{
					StoreHelper.DeleteImage(friendlyLink.ImageUrl);
				}
				catch
				{
				}
				this.BindFriendlyLinks();
				this.ShowMsg("成功删除了选择的友情链接", true);
				return;
			}
			this.ShowMsg("未知错误", false);
		}
		private void grdGroupList_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			int rowIndex = ((System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer).RowIndex;
			int linkId = (int)this.grdGroupList.DataKeys[rowIndex].Value;
			if (e.CommandName == "SetYesOrNo")
			{
				FriendlyLinksInfo friendlyLink = StoreHelper.GetFriendlyLink(linkId);
				if (friendlyLink.Visible)
				{
					friendlyLink.Visible = false;
				}
				else
				{
					friendlyLink.Visible = true;
				}
				StoreHelper.UpdateFriendlyLink(friendlyLink);
				this.BindFriendlyLinks();
				return;
			}
			int displaySequence = int.Parse((this.grdGroupList.Rows[rowIndex].FindControl("lblDisplaySequence") as System.Web.UI.WebControls.Literal).Text);
			int num = 0;
			int replaceDisplaySequence = 0;
			if (e.CommandName == "Fall")
			{
				if (rowIndex < this.grdGroupList.Rows.Count - 1)
				{
					num = (int)this.grdGroupList.DataKeys[rowIndex + 1].Value;
					replaceDisplaySequence = int.Parse((this.grdGroupList.Rows[rowIndex + 1].FindControl("lblDisplaySequence") as System.Web.UI.WebControls.Literal).Text);
				}
			}
			else
			{
				if (e.CommandName == "Rise" && rowIndex > 0)
				{
					num = (int)this.grdGroupList.DataKeys[rowIndex - 1].Value;
					replaceDisplaySequence = int.Parse((this.grdGroupList.Rows[rowIndex - 1].FindControl("lblDisplaySequence") as System.Web.UI.WebControls.Literal).Text);
				}
			}
			if (num > 0)
			{
				StoreHelper.SwapFriendlyLinkSequence(linkId, num, displaySequence, replaceDisplaySequence);
				this.BindFriendlyLinks();
			}
		}
	}
}
