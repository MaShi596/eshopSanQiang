using ASPNET.WebControls;
using Hidistro.Core.Enums;
using Hidistro.Entities.Sales;
using Hidistro.Subsites.Members;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class UnderlingRanking : DistributorPage
	{
		private System.DateTime? dateStart;
		private System.DateTime? dateEnd;
		private string sortBy = "SaleTotals";
		protected WebCalendar calendarStartDate;
		protected WebCalendar calendarEndDate;
		protected System.Web.UI.WebControls.DropDownList ddlSort;
		protected System.Web.UI.WebControls.Button btnSearchButton;
		protected System.Web.UI.WebControls.LinkButton btnCreateReport;
		protected PageSize hrefPageSize;
		protected Pager pager;
		protected Grid grdProductSaleStatistics;
		protected Pager pager1;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
            this.grdProductSaleStatistics.ReBindData += new Grid.ReBindDataEventHandler(this.grdProductSaleStatistics_ReBindData);
			this.btnCreateReport.Click += new System.EventHandler(this.btnCreateReport_Click);
			this.ddlSort.Items.Add(new System.Web.UI.WebControls.ListItem("消费金额", "SaleTotals"));
			this.ddlSort.Items.Add(new System.Web.UI.WebControls.ListItem("订单数", "OrderCount"));
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
				this.BindUnderlingRanking();
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
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortBy"]))
				{
					this.sortBy = base.Server.UrlDecode(this.Page.Request.QueryString["sortBy"]);
				}
                this.calendarStartDate.SelectedDate = this.dateStart;
                this.calendarEndDate.SelectedDate = this.dateEnd;
				this.ddlSort.SelectedValue = this.sortBy;
				return;
			}
			this.dateStart = this.calendarStartDate.SelectedDate;
			this.dateEnd = this.calendarEndDate.SelectedDate;
			this.sortBy = this.ddlSort.SelectedValue;
		}
		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReloadUnderlingRanking(true);
		}
		private void grdProductSaleStatistics_ReBindData(object sender)
		{
			this.ReloadUnderlingRanking(false);
		}
		private void btnCreateReport_Click(object sender, System.EventArgs e)
		{
			System.Data.DataTable underlingStatisticsNoPage = UnderlingHelper.GetUnderlingStatisticsNoPage(new SaleStatisticsQuery
			{
				StartDate = this.dateStart,
				EndDate = this.dateEnd,
				SortBy = this.sortBy,
				SortOrder = SortAction.Desc
			});
			string text = string.Empty;
			text += "会员";
			text += ",订单数";
			text += ",消费金额\r\n";
			foreach (System.Data.DataRow dataRow in underlingStatisticsNoPage.Rows)
			{
				text += dataRow["UserName"].ToString();
				text = text + "," + dataRow["OrderCount"].ToString();
				text = text + "," + dataRow["SaleTotals"].ToString() + "\r\n";
			}
			this.Page.Response.Clear();
			this.Page.Response.Buffer = false;
			this.Page.Response.Charset = "GB2312";
			this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=MemberRanking.csv");
			this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
			this.Page.Response.ContentType = "application/octet-stream";
			this.Page.EnableViewState = false;
			this.Page.Response.Write(text);
			this.Page.Response.End();
		}
		private void BindUnderlingRanking()
		{
			SaleStatisticsQuery saleStatisticsQuery = new SaleStatisticsQuery();
			saleStatisticsQuery.StartDate = this.dateStart;
			saleStatisticsQuery.EndDate = this.dateEnd;
			saleStatisticsQuery.PageSize = this.pager.PageSize;
			saleStatisticsQuery.PageIndex = this.pager.PageIndex;
			saleStatisticsQuery.SortBy = this.sortBy;
			saleStatisticsQuery.SortOrder = SortAction.Desc;
			int totalRecords = 0;
			System.Data.DataTable underlingStatistics = UnderlingHelper.GetUnderlingStatistics(saleStatisticsQuery, out totalRecords);
			this.grdProductSaleStatistics.DataSource = underlingStatistics;
			this.grdProductSaleStatistics.DataBind();
            this.calendarStartDate.SelectedDate = saleStatisticsQuery.StartDate;
            this.calendarEndDate.SelectedDate = saleStatisticsQuery.EndDate;
			this.pager.TotalRecords=totalRecords;
			this.pager1.TotalRecords=totalRecords;
		}
		private void ReloadUnderlingRanking(bool isSeach)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("dateStart", this.calendarStartDate.SelectedDate.ToString());
			nameValueCollection.Add("dateEnd", this.calendarEndDate.SelectedDate.ToString());
			nameValueCollection.Add("sortBy", this.ddlSort.SelectedValue);
			nameValueCollection.Add("pageSize", this.pager.PageSize.ToString());
			if (!isSeach)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
			}
			base.ReloadPage(nameValueCollection);
		}
	}
}
