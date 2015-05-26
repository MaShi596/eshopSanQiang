using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Membership.Context;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Web.CustomMade;
using Hishop.Web.CustomMade.Supplier;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.Cpage.Supplier
{
	public class supplier_Admin_StockCheck : AdminPage
	{
		protected System.Web.UI.WebControls.RadioButtonList rdo_ListDate;
		protected System.Web.UI.WebControls.TextBox txtStockCode;
		protected RoleDropDownList dropRolesList;
		protected WebCalendar calendarStartDate;
		protected WebCalendar calendarEndDate;
		protected System.Web.UI.WebControls.DropDownList ddl_status;
		protected System.Web.UI.WebControls.Button btnSearchButton;
		protected Pager pager;
		protected Grid grdManager;
		protected Pager pager1;
		protected void Page_Load(object sender, System.EventArgs e)
		{
            this.grdManager.ReBindData += new Grid.ReBindDataEventHandler(this.grdManager_ReBindData);
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
		protected void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReloadManagerLogs(true);
		}
		private void BindData()
		{
			int userId = Hidistro.Membership.Context.HiContext.Current.User.UserId;
			Supplier_QueryInfo managerQuery = this.GetManagerQuery();
			DbQueryResult dbQueryResult = Methods.Supplier_S_StockInfoGet(managerQuery, null, new int?(userId));
			this.grdManager.DataSource = dbQueryResult.Data;
			this.grdManager.DataBind();
			this.txtStockCode.Text = managerQuery.Code;
            this.pager.TotalRecords = dbQueryResult.TotalRecords;
            this.pager1.TotalRecords = dbQueryResult.TotalRecords;
		}
		private void ReloadManagerLogs(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("DateDay", this.rdo_ListDate.SelectedValue);
			nameValueCollection.Add("PageSize", this.pager.PageSize.ToString());
			nameValueCollection.Add("Code", this.txtStockCode.Text.Trim());
			nameValueCollection.Add("Status", this.ddl_status.SelectedValue);
			if (this.calendarStartDate.SelectedDate.HasValue)
			{
				nameValueCollection.Add("StartDate", this.calendarStartDate.SelectedDate.Value.ToString());
			}
			if (this.calendarEndDate.SelectedDate.HasValue)
			{
				nameValueCollection.Add("EndDate", this.calendarEndDate.SelectedDate.Value.ToString());
			}
			if (!isSearch)
			{
				nameValueCollection.Add("PageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			nameValueCollection.Add("SortBy", this.grdManager.SortOrderBy);
			nameValueCollection.Add("SortOrder", SortAction.Desc.ToString());
			base.ReloadPage(nameValueCollection);
		}
		private Supplier_QueryInfo GetManagerQuery()
		{
			Supplier_QueryInfo supplier_QueryInfo = new Supplier_QueryInfo();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["DateDay"]))
			{
				supplier_QueryInfo.DateDay = this.Page.Request.QueryString["DateDay"];
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartDate"]))
			{
				supplier_QueryInfo.StartDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["StartDate"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["EndDate"]))
			{
				supplier_QueryInfo.EndDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["EndDate"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Code"]))
			{
				supplier_QueryInfo.Code = Globals.UrlDecode(this.Page.Request.QueryString["Code"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["Status"]))
			{
				supplier_QueryInfo.Status = this.Page.Request.QueryString["Status"];
			}
			supplier_QueryInfo.PageSize = this.pager.PageSize;
			supplier_QueryInfo.PageIndex = this.pager.PageIndex;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SortBy"]))
			{
				supplier_QueryInfo.SortBy = this.Page.Request.QueryString["SortBy"];
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SortOrder"]))
			{
				supplier_QueryInfo.SortOrder = SortAction.Desc;
			}
			return supplier_QueryInfo;
		}
	}
}
