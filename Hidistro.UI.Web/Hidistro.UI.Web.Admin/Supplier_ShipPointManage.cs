using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Web.CustomMade;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	public class Supplier_ShipPointManage : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtSearchText;
		protected RoleDropDownList dropRolesList;
		protected System.Web.UI.WebControls.Button btnSearchButton;
		protected Pager pager;
		protected Grid grdManager;
		protected Pager pager1;
		protected void Page_Load(object sender, System.EventArgs e)
		{
            this.grdManager.ReBindData += new Grid.ReBindDataEventHandler(this.grdManager_ReBindData);
			this.grdManager.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdManager_RowDeleting);
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
			if (!this.Page.IsPostBack)
			{
				this.BindData();
			}
		}
		private void grdManager_ReBindData(object sender)
		{
			this.ReloadManagerLogs(false);
		}
		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReloadManagerLogs(true);
		}
		private void grdManager_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			int num = (int)this.grdManager.DataKeys[e.RowIndex].Value;
			if (Hidistro.Membership.Context.HiContext.Current.User.UserId == num)
			{
				this.ShowMsg("不能删除自己", false);
				return;
			}
			Hidistro.Membership.Context.SiteManager manager = ManagerHelper.GetManager(num);
			if (!ManagerHelper.Delete(manager.UserId))
			{
				this.ShowMsg("未知错误", false);
				return;
			}
			this.BindData();
			this.ShowMsg("成功删除了一个区域发货点", true);
		}
		private void BindData()
		{
			ManagerQuery managerQuery = this.GetManagerQuery();
			DbQueryResult dbQueryResult = Methods.Supplier_SGet(managerQuery, null, null);
			this.grdManager.DataSource = dbQueryResult.Data;
			this.grdManager.DataBind();
			this.txtSearchText.Text = managerQuery.Username;
            this.pager.TotalRecords = dbQueryResult.TotalRecords;
            this.pager1.TotalRecords = dbQueryResult.TotalRecords;
		}
		private void ReloadManagerLogs(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("Username", this.txtSearchText.Text);
			nameValueCollection.Add("RoleId", System.Convert.ToString(this.dropRolesList.SelectedValue));
			if (!isSearch)
			{
				nameValueCollection.Add("PageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			nameValueCollection.Add("SortBy", this.grdManager.SortOrderBy);
			nameValueCollection.Add("SortOrder", SortAction.Desc.ToString());
			base.ReloadPage(nameValueCollection);
		}
		private ManagerQuery GetManagerQuery()
		{
			ManagerQuery managerQuery = new ManagerQuery();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Username"]))
			{
				managerQuery.Username = base.Server.UrlDecode(this.Page.Request.QueryString["Username"]);
			}
			managerQuery.RoleId = new System.Guid("5a26c830-b998-4569-bffc-c5ceae774a7a");
			managerQuery.PageSize = this.pager.PageSize;
			managerQuery.PageIndex = this.pager.PageIndex;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SortBy"]))
			{
				managerQuery.SortBy = this.Page.Request.QueryString["SortBy"];
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SortOrder"]))
			{
				managerQuery.SortOrder = SortAction.Desc;
			}
			return managerQuery;
		}
	}
}
