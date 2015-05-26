using ASPNET.WebControls;
using Hidistro.Core.Entities;
using Hidistro.Entities.Sales;
using Hidistro.Subsites.Sales;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class SaleDetails : DistributorPage
	{
		private System.DateTime? startTime;
		private System.DateTime? endTime;
		protected WebCalendar calendarStart;
		protected WebCalendar calendarEnd;
		protected System.Web.UI.WebControls.Button btnQuery;
		protected System.Web.UI.WebControls.GridView grdOrderLineItem;
		protected Pager pager;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
				this.BindList();
			}
		}
		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startTime"]))
				{
					this.startTime = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["startTime"]));
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endTime"]))
				{
					this.endTime = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["endTime"]));
				}
                this.calendarStart.SelectedDate = this.startTime;
                this.calendarEnd.SelectedDate = this.endTime;
				return;
			}
			this.startTime = this.calendarStart.SelectedDate;
			this.endTime = this.calendarEnd.SelectedDate;
		}
		private void ReBind()
		{
			base.ReloadPage(new System.Collections.Specialized.NameValueCollection
			{

				{
					"startTime",
					this.calendarStart.SelectedDate.ToString()
				},

				{
					"endTime",
					this.calendarEnd.SelectedDate.ToString()
				},

				{
					"pageIndex",
					this.pager.PageIndex.ToString()
				}
			});
		}
		private void btnQuery_Click(object sender, System.EventArgs e)
		{
			this.ReBind();
		}
		private void BindList()
		{
            SaleStatisticsQuery query = new SaleStatisticsQuery
            {
                PageIndex = this.pager.PageIndex,
                StartDate = this.startTime,
                EndDate = this.endTime
            };
            DbQueryResult saleOrderLineItemsStatistics = SubsiteSalesHelper.GetSaleOrderLineItemsStatistics(query);
            this.grdOrderLineItem.DataSource = saleOrderLineItemsStatistics.Data;
            this.grdOrderLineItem.DataBind();
            this.pager.TotalRecords = saleOrderLineItemsStatistics.TotalRecords;
            this.grdOrderLineItem.PageSize = query.PageSize;
		}
	}
}
