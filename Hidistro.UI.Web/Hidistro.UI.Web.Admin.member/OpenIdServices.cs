using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Plugins;
using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.member
{
	[PrivilegeCheck(Privilege.OpenIdServices)]
	public class OpenIdServices : AdminPage
	{
		protected System.Web.UI.WebControls.Panel pnlConfigedList;
		protected System.Web.UI.WebControls.GridView grdConfigedItems;
		protected System.Web.UI.WebControls.Panel pnlConfigedNote;
		protected System.Web.UI.WebControls.Panel pnlEmptyList;
		protected System.Web.UI.WebControls.GridView grdEmptyList;
		protected System.Web.UI.WebControls.Panel pnlEmptyNote;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.grdConfigedItems.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdConfigedItems_RowDeleting);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.BindData();
			}
		}
		private void BindData()
		{
			this.BindConfigedList();
			this.BindEmptyList();
		}
		private void BindConfigedList()
		{
			PluginItemCollection configedItems = OpenIdHelper.GetConfigedItems();
			if (configedItems != null && configedItems.Count > 0)
			{
				this.grdConfigedItems.DataSource = configedItems.Items;
				this.grdConfigedItems.DataBind();
				this.pnlConfigedList.Visible = true;
				this.pnlConfigedNote.Visible = false;
				return;
			}
			this.pnlConfigedList.Visible = false;
			this.pnlConfigedNote.Visible = true;
		}
		private void BindEmptyList()
		{
			PluginItemCollection emptyItems = OpenIdHelper.GetEmptyItems();
			if (emptyItems != null && emptyItems.Count > 0)
			{
				this.grdEmptyList.DataSource = emptyItems.Items;
				this.grdEmptyList.DataBind();
				this.pnlEmptyList.Visible = true;
				this.pnlEmptyNote.Visible = false;
				return;
			}
			this.pnlEmptyList.Visible = false;
			this.pnlEmptyNote.Visible = true;
		}
		private void grdConfigedItems_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			string openIdType = this.grdConfigedItems.DataKeys[e.RowIndex]["FullName"].ToString();
			OpenIdHelper.DeleteSettings(openIdType);
			this.BindData();
		}
	}
}
