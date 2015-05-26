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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	public class Supplier_SupManage : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtSearchText;
		protected RoleDropDownList dropRolesList;
		protected System.Web.UI.WebControls.Button btnSearchButton;
		protected Pager pager;
		protected Grid grdManager;
		protected System.Web.UI.WebControls.DropDownList ddl_UserIdList;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hid_UserId;
		protected System.Web.UI.WebControls.Button btnRemark;
		protected Pager pager1;
		protected void Page_Load(object sender, System.EventArgs e)
		{
            this.grdManager.ReBindData += new Grid.ReBindDataEventHandler(this.grdManager_ReBindData);
			this.grdManager.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdManager_RowDeleting);
			this.btnRemark.Click += new System.EventHandler(this.btnRemark_Click);
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
			this.ShowMsg("成功删除了一个供应商", true);
		}
		private void BindData()
		{
			ManagerQuery managerQuery = this.GetManagerQuery();
			DbQueryResult dbQueryResult = Methods.Supplier_SGet(managerQuery, null, null);
			this.grdManager.DataSource = dbQueryResult.Data;
			this.grdManager.DataBind();
			this.txtSearchText.Text = managerQuery.Username;
			this.ddl_UserIdList.DataSource = dbQueryResult.Data;
			this.ddl_UserIdList.DataTextField = "UserName";
			this.ddl_UserIdList.DataValueField = "UserId";
			this.ddl_UserIdList.DataBind();
			this.pager.TotalRecords=dbQueryResult.TotalRecords;
			this.pager1.TotalRecords=dbQueryResult.TotalRecords;
		}
		private void btnRemark_Click(object sender, System.EventArgs e)
		{
			string value = this.hid_UserId.Value;
			if (this.ddl_UserIdList.SelectedValue == value)
			{
				this.ShowMsg("不能转移给自己", false);
				return;
			}
			Methods.Supplier_UpdateSupProjectsByUserId(int.Parse(value), int.Parse(this.ddl_UserIdList.SelectedValue), this.ddl_UserIdList.SelectedItem.Text);
			Hidistro.Membership.Context.SiteManager manager = ManagerHelper.GetManager(int.Parse(value));
			if (!ManagerHelper.Delete(manager.UserId))
			{
				this.ShowMsg("未知错误", false);
				return;
			}
			this.BindData();
			this.ShowMsg("转移成功", true);
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
			this.BindData();
		}
		private ManagerQuery GetManagerQuery()
		{
			ManagerQuery managerQuery = new ManagerQuery();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Username"]))
			{
				managerQuery.Username = base.Server.UrlDecode(this.Page.Request.QueryString["Username"]);
			}
			managerQuery.RoleId = new System.Guid("625a27cc-7a55-41d6-8449-c6fe736003e5");
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
