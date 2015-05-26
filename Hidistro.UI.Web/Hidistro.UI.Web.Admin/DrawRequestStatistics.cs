using ASPNET.WebControls;
using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.MemberDrawRequestStatistics)]
	public class DrawRequestStatistics : AdminPage
	{
		private string userName;
		private System.DateTime? dateStart;
		private System.DateTime? dateEnd;
		protected System.Web.UI.WebControls.TextBox txtUserName;
		protected WebCalendar calendarStart;
		protected WebCalendar calendarEnd;
		protected System.Web.UI.WebControls.Button btnQueryBalanceDrawRequest;
		protected System.Web.UI.WebControls.LinkButton btnCreateReport;
		protected PageSize hrefPageSize;
		protected Pager pager;
		protected Grid grdBalanceDrawRequest;
		protected Pager pager1;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnQueryBalanceDrawRequest.Click += new System.EventHandler(this.btnQueryBalanceDrawRequest_Click);
			this.btnCreateReport.Click += new System.EventHandler(this.btnCreateReport_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
				this.GetBalanceDrawRequest();
			}
		}
		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["userName"]))
				{
					this.userName = base.Server.UrlDecode(this.Page.Request.QueryString["userName"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dateStart"]))
				{
					this.dateStart = new System.DateTime?(System.Convert.ToDateTime(base.Server.UrlDecode(this.Page.Request.QueryString["dateStart"])));
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dateEnd"]))
				{
					this.dateEnd = new System.DateTime?(System.Convert.ToDateTime(base.Server.UrlDecode(this.Page.Request.QueryString["dateEnd"])));
				}
				this.txtUserName.Text = this.userName;
                this.calendarStart.SelectedDate = this.dateStart;
                this.calendarEnd.SelectedDate = this.dateEnd;
				return;
			}
			this.userName = this.txtUserName.Text;
			this.dateStart = this.calendarStart.SelectedDate;
			this.dateEnd = this.calendarEnd.SelectedDate;
		}
		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("userName", this.txtUserName.Text);
			nameValueCollection.Add("dateStart", this.calendarStart.SelectedDate.ToString());
			nameValueCollection.Add("dateEnd", this.calendarEnd.SelectedDate.ToString());
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			base.ReloadPage(nameValueCollection);
		}
		public void GetBalanceDrawRequest()
		{
            BalanceDetailQuery query = new BalanceDetailQuery
            {
                UserName = this.userName,
                FromDate = this.dateStart,
                ToDate = this.dateEnd,
                PageIndex = this.pager.PageIndex,
                TradeType = TradeTypes.DrawRequest
            };
            DbQueryResult balanceDetails = MemberHelper.GetBalanceDetails(query);
            this.grdBalanceDrawRequest.DataSource = balanceDetails.Data;
            this.grdBalanceDrawRequest.DataBind();
            this.pager1.TotalRecords = this.pager.TotalRecords = balanceDetails.TotalRecords;
		}
		private void btnCreateReport_Click(object sender, System.EventArgs e)
		{
			DbQueryResult balanceDetailsNoPage = MemberHelper.GetBalanceDetailsNoPage(new BalanceDetailQuery
			{
				UserName = this.userName,
				FromDate = this.dateStart,
				ToDate = this.dateEnd,
				SortBy = "TradeDate",
				SortOrder = SortAction.Desc,
				TradeType = TradeTypes.DrawRequest
			});
			string text = string.Empty;
			text += "用户名";
			text += ",交易时间";
			text += ",业务摘要";
			text += ",转出金额";
			text += ",当前余额\r\n";
			foreach (System.Data.DataRow dataRow in ((System.Data.DataTable)balanceDetailsNoPage.Data).Rows)
			{
				text += dataRow["UserName"];
				text = text + "," + dataRow["TradeDate"];
				text += ",提现";
				text = text + "," + dataRow["Expenses"];
				object obj = text;
				text = string.Concat(new object[]
				{
					obj,
					",",
					dataRow["Balance"],
					"\r\n"
				});
			}
			this.Page.Response.Clear();
			this.Page.Response.Buffer = false;
			this.Page.Response.Charset = "GB2312";
			this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=BalanceDetailsStatistics.csv");
			this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
			this.Page.Response.ContentType = "application/octet-stream";
			this.Page.EnableViewState = false;
			this.Page.Response.Write(text);
			this.Page.Response.End();
		}
		private void btnQueryBalanceDrawRequest_Click(object sender, System.EventArgs e)
		{
			this.ReBind(true);
		}
	}
}
