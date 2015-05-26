using ASPNET.WebControls;
using Hidistro.ControlPanel.Distribution;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Enums;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.DistributorAchievementsRanking)]
	public class DistributorAchievementsRanking : AdminPage
	{
		private System.DateTime? dateStart;
		private System.DateTime? dateEnd;
		protected WebCalendar calendarStartDate;
		protected WebCalendar calendarEndDate;
		protected System.Web.UI.WebControls.Button btnSearchButton;
		protected System.Web.UI.WebControls.LinkButton btnCreateReport;
		protected Grid grdDistributorStatistics;
		protected FormatedMoneyLabel lblTotal;
		protected FormatedMoneyLabel lblProfitTotal;
		protected Pager pager;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
            this.grdDistributorStatistics.ReBindData += new Grid.ReBindDataEventHandler(this.grdDistributorStatistics_ReBindData);
			this.btnCreateReport.Click += new System.EventHandler(this.btnCreateReport_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
				this.BindDistributorRanking();
			}
		}
		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dateStart"]))
				{
					this.dateStart = new System.DateTime?(System.Convert.ToDateTime(this.Page.Request.QueryString["dateStart"]));
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dateEnd"]))
				{
					this.dateEnd = new System.DateTime?(System.Convert.ToDateTime(this.Page.Request.QueryString["dateEnd"]));
				}
                this.calendarStartDate.SelectedDate = this.dateStart;
                this.calendarEndDate.SelectedDate = this.dateEnd;
				return;
			}
			this.dateStart = this.calendarStartDate.SelectedDate;
			this.dateEnd = this.calendarEndDate.SelectedDate;
		}
		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("dateStart", this.calendarStartDate.SelectedDate.ToString());
			nameValueCollection.Add("dateEnd", this.calendarEndDate.SelectedDate.ToString());
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			base.ReloadPage(nameValueCollection);
		}
		private void BindDistributorRanking()
		{
			SaleStatisticsQuery saleStatisticsQuery = new SaleStatisticsQuery();
			saleStatisticsQuery.StartDate = this.dateStart;
			saleStatisticsQuery.EndDate = this.dateEnd;
			saleStatisticsQuery.PageSize = this.pager.PageSize;
			saleStatisticsQuery.PageIndex = this.pager.PageIndex;
			saleStatisticsQuery.SortBy = "SaleTotals";
			saleStatisticsQuery.SortOrder = SortAction.Desc;
			int totalRecords = 0;
			OrderStatisticsInfo distributorStatistics = DistributorHelper.GetDistributorStatistics(saleStatisticsQuery, out totalRecords);
			this.grdDistributorStatistics.DataSource = distributorStatistics.OrderTbl;
			this.grdDistributorStatistics.DataBind();
			this.lblTotal.Money = distributorStatistics.TotalOfSearch;
			this.lblProfitTotal.Money = distributorStatistics.ProfitsOfSearch;
			this.pager.TotalRecords=totalRecords;
		}
		private void btnCreateReport_Click(object sender, System.EventArgs e)
		{
			OrderStatisticsInfo distributorStatisticsNoPage = DistributorHelper.GetDistributorStatisticsNoPage(new SaleStatisticsQuery
			{
				StartDate = this.dateStart,
				EndDate = this.dateEnd,
				PageSize = this.pager.PageSize,
				SortBy = "SaleTotals",
				SortOrder = SortAction.Desc
			});
			System.Data.DataTable orderTbl = distributorStatisticsNoPage.OrderTbl;
			string text = string.Empty;
			text += "排行,分销商名称,交易量,交易金额,利润\r\n";
			foreach (System.Data.DataRow dataRow in orderTbl.Rows)
			{
				if (System.Convert.ToDecimal(dataRow["SaleTotals"]) > 0m)
				{
					text += dataRow["IndexId"].ToString();
				}
				else
				{
					text = (text ?? "");
				}
				text = text + "," + dataRow["UserName"].ToString();
				text = text + "," + dataRow["PurchaseOrderCount"].ToString();
				text = text + "," + dataRow["SaleTotals"].ToString();
				text = text + "," + dataRow["Profits"].ToString();
				text += "\r\n";
			}
			this.Page.Response.Clear();
			this.Page.Response.Buffer = false;
			this.Page.Response.Charset = "GB2312";
			this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=DistributorRanking.CSV");
			this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
			this.Page.Response.ContentType = "application/octet-stream";
			this.Page.EnableViewState = false;
			this.Page.Response.Write(text);
			this.Page.Response.End();
		}
		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReBind(true);
		}
		private void grdDistributorStatistics_ReBindData(object sender)
		{
			this.ReBind(false);
		}
	}
}
