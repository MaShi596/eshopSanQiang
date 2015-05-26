using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[AdministerCheck(true)]
	public class ManageLogs : AdminPage
	{
		protected LogsUserNameDropDownList dropOperationUserNames;
		protected WebCalendar calenderFromDate;
		protected WebCalendar calenderToDate;
		protected System.Web.UI.WebControls.Button btnQueryLogs;
		protected ImageLinkButton lkbDeleteAll;
		protected PageSize hrefPageSize;
		protected Pager pager;
		protected ImageLinkButton lkbDeleteCheck1;
		protected System.Web.UI.WebControls.DataList dlstLog;
		protected ImageLinkButton lkbDeleteCheck;
		protected Pager pager1;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.dlstLog.DeleteCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.dlstLog_DeleteCommand);
			this.btnQueryLogs.Click += new System.EventHandler(this.btnQueryLogs_Click);
			this.lkbDeleteCheck.Click += new System.EventHandler(this.lkbDeleteCheck_Click);
			this.lkbDeleteCheck1.Click += new System.EventHandler(this.lkbDeleteCheck1_Click);
			this.lkbDeleteAll.Click += new System.EventHandler(this.lkbDeleteAll_Click);
			if (!this.Page.IsPostBack)
			{
				this.dropOperationUserNames.DataBind();
				this.BindLogs();
			}
		}
		private void dlstLog_DeleteCommand(object sender, System.Web.UI.WebControls.DataListCommandEventArgs e)
		{
			long logId = (long)this.dlstLog.DataKeys[e.Item.ItemIndex];
			if (EventLogs.DeleteLog(logId))
			{
				this.BindLogs();
				this.ShowMsg("成功删除了单个操作日志", true);
				return;
			}
			this.ShowMsg("在删除过程中出现未知错误", false);
		}
		private void lkbDeleteAll_Click(object sender, System.EventArgs e)
		{
			if (EventLogs.DeleteAllLogs())
			{
				this.BindLogs();
				this.ShowMsg("成功删除了所有操作日志", true);
				return;
			}
			this.ShowMsg("在删除过程中出现未知错误", false);
		}
		private void lkbDeleteCheck1_Click(object sender, System.EventArgs e)
		{
			this.DeleteCheck();
		}
		private void DeleteCheck()
		{
			string text = "";
			if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
			{
				text = base.Request["CheckBoxGroup"];
			}
			if (text.Length <= 0)
			{
				this.ShowMsg("请先选择要删除的操作日志项", false);
				return;
			}
			int num = EventLogs.DeleteLogs(text);
			this.BindLogs();
			this.ShowMsg(string.Format("成功删除了{0}个操作日志", num), true);
		}
		private void lkbDeleteCheck_Click(object sender, System.EventArgs e)
		{
			this.DeleteCheck();
		}
		private void btnQueryLogs_Click(object sender, System.EventArgs e)
		{
			this.ReloadManagerLogs(true);
		}
		public void BindLogs()
		{
			OperationLogQuery operationLogQuery = this.GetOperationLogQuery();
			DbQueryResult logs = EventLogs.GetLogs(operationLogQuery);
			this.dlstLog.DataSource = logs.Data;
			this.dlstLog.DataBind();
			this.SetSearchControl();
			this.pager.TotalRecords=logs.TotalRecords;
			this.pager1.TotalRecords=logs.TotalRecords;
		}
		private void ReloadManagerLogs(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("OperationUserName", this.dropOperationUserNames.SelectedValue);
			if (this.calenderFromDate.SelectedDate.HasValue)
			{
				nameValueCollection.Add("FromDate", this.calenderFromDate.SelectedDate.ToString());
			}
			if (this.calenderToDate.SelectedDate.HasValue)
			{
				nameValueCollection.Add("ToDate", this.calenderToDate.SelectedDate.ToString());
			}
			if (!isSearch)
			{
				nameValueCollection.Add("PageIndex", this.pager.PageIndex.ToString());
			}
			nameValueCollection.Add("SortOrder", SortAction.Desc.ToString());
			nameValueCollection.Add("PageSize", this.pager.PageSize.ToString());
			base.ReloadPage(nameValueCollection);
		}
		private void SetSearchControl()
		{
			if (!this.Page.IsCallback)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OperationUserName"]))
				{
					this.dropOperationUserNames.SelectedValue = base.Server.UrlDecode(this.Page.Request.QueryString["OperationUserName"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["FromDate"]))
				{
                    this.calenderFromDate.SelectedDate = new System.DateTime?(System.Convert.ToDateTime(this.Page.Request.QueryString["FromDate"]));
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ToDate"]))
				{
                    this.calenderToDate.SelectedDate = new System.DateTime?(System.Convert.ToDateTime(this.Page.Request.QueryString["ToDate"]));
				}
			}
		}
		private OperationLogQuery GetOperationLogQuery()
		{
			OperationLogQuery operationLogQuery = new OperationLogQuery();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OperationUserName"]))
			{
				operationLogQuery.OperationUserName = base.Server.UrlDecode(this.Page.Request.QueryString["OperationUserName"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["FromDate"]))
			{
				operationLogQuery.FromDate = new System.DateTime?(System.Convert.ToDateTime(this.Page.Request.QueryString["FromDate"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ToDate"]))
			{
				operationLogQuery.ToDate = new System.DateTime?(System.Convert.ToDateTime(this.Page.Request.QueryString["ToDate"]));
			}
			operationLogQuery.Page.PageIndex = this.pager.PageIndex;
			operationLogQuery.Page.PageSize = this.pager.PageSize;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SortBy"]))
			{
				operationLogQuery.Page.SortBy = this.Page.Request.QueryString["SortBy"];
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["SortOrder"]))
			{
				operationLogQuery.Page.SortOrder = SortAction.Desc;
			}
			return operationLogQuery;
		}
	}
}
