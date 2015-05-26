using ASPNET.WebControls;
using Hidistro.ControlPanel.Distribution;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities.Sales;
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
	[PrivilegeCheck(Privilege.PurchaseOrderStatistics)]
	public class PurchaseOrderStatistics : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtUserName;
		protected WebCalendar calendarStartDate;
		protected WebCalendar calendarEndDate;
		protected System.Web.UI.WebControls.Button btnSearchButton;
		protected System.Web.UI.WebControls.LinkButton btnCreateReport;
		protected Grid grdPurchaseOrderStatistics;
		protected System.Web.UI.WebControls.Label lblPageCount;
		protected System.Web.UI.WebControls.Label lblSearchCount;
		protected Pager pager;
		private string userName;
		private System.DateTime? dateStart;
		private System.DateTime? dateEnd;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
            this.grdPurchaseOrderStatistics.ReBindData += new Grid.ReBindDataEventHandler(this.grdPurchaseOrderStatistics_ReBindData);
			this.btnCreateReport.Click += new System.EventHandler(this.btnCreateReport_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
				this.BindPurchaseOrderStatistics();
			}
		}
		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["orderId"]))
				{
					this.userName = base.Server.UrlDecode(this.Page.Request.QueryString["orderId"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["userName"]))
				{
					this.userName = base.Server.UrlDecode(this.Page.Request.QueryString["userName"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dateStart"]))
				{
					this.dateStart = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["dateStart"]));
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dateEnd"]))
				{
					this.dateEnd = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["dateEnd"]));
				}
				this.txtUserName.Text = this.userName;
                this.calendarStartDate.SelectedDate = this.dateStart;
                this.calendarEndDate.SelectedDate = this.dateEnd;
				return;
			}
			this.userName = this.txtUserName.Text;
			this.dateStart = this.calendarStartDate.SelectedDate;
			this.dateEnd = this.calendarEndDate.SelectedDate;
		}
		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("userName", this.txtUserName.Text);
			nameValueCollection.Add("dateStart", this.calendarStartDate.SelectedDate.ToString());
			nameValueCollection.Add("dateEnd", this.calendarEndDate.SelectedDate.ToString());
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			base.ReloadPage(nameValueCollection);
		}
		private void BindPurchaseOrderStatistics()
		{
			OrderStatisticsInfo purchaseOrders = DistributorHelper.GetPurchaseOrders(new UserOrderQuery
			{
				UserName = this.userName,
				StartDate = this.dateStart,
				EndDate = this.dateEnd,
				PageSize = 10,
				PageIndex = this.pager.PageIndex,
				SortBy = "PurchaseDate",
				SortOrder = SortAction.Desc
			});
			this.grdPurchaseOrderStatistics.DataSource = purchaseOrders.OrderTbl;
			this.grdPurchaseOrderStatistics.DataBind();
			this.pager.TotalRecords=purchaseOrders.TotalCount;
			this.lblPageCount.Text = string.Format("当前页共计<span class=\"colorG\">{0}</span>个 <span style=\"padding-left:10px;\">采购单金额共计</span><span class=\"colorG\">{1}</span>元 <span style=\"padding-left:10px;\">采购单毛利润共计</span><span class=\"colorG\">{2}</span>元 ", purchaseOrders.OrderTbl.Rows.Count, Globals.FormatMoney(purchaseOrders.TotalOfPage), Globals.FormatMoney(purchaseOrders.ProfitsOfPage));
			this.lblSearchCount.Text = string.Format("当前查询结果共计<span class=\"colorG\">{0}</span>个 <span style=\"padding-left:10px;\">采购单金额共计</span><span class=\"colorG\">{1}</span>元 <span style=\"padding-left:10px;\">采购单毛利润共计</span><span class=\"colorG\">{2}</span>元 ", purchaseOrders.TotalCount, Globals.FormatMoney(purchaseOrders.TotalOfSearch), Globals.FormatMoney(purchaseOrders.ProfitsOfSearch));
		}
		private void btnCreateReport_Click(object sender, System.EventArgs e)
		{
			OrderStatisticsInfo purchaseOrdersNoPage = DistributorHelper.GetPurchaseOrdersNoPage(new UserOrderQuery
			{
				UserName = this.userName,
				StartDate = this.dateStart,
				EndDate = this.dateEnd,
				PageIndex = this.pager.PageIndex,
				SortBy = "PurchaseDate",
				SortOrder = SortAction.Desc
			});
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
			stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
			stringBuilder.AppendLine("<td>采购单号</td>");
			stringBuilder.AppendLine("<td>订单号</td>");
			stringBuilder.AppendLine("<td>下单时间</td>");
			stringBuilder.AppendLine("<td>分销商名称</td>");
			stringBuilder.AppendLine("<td>采购单金额</td>");
			stringBuilder.AppendLine("<td>利润</td>");
			stringBuilder.AppendLine("</tr>");
			foreach (System.Data.DataRow dataRow in purchaseOrdersNoPage.OrderTbl.Rows)
			{
				stringBuilder.AppendLine("<tr>");
				stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["PurchaseOrderId"].ToString() + "</td>");
				stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["OrderId"].ToString() + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["PurchaseDate"].ToString() + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["Distributorname"].ToString() + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["PurchaseTotal"].ToString() + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["PurchaseProfit"].ToString() + "</td>");
				stringBuilder.AppendLine("</tr>");
			}
			stringBuilder.AppendLine("<tr>");
			stringBuilder.AppendLine("<td>当前查询结果共计," + purchaseOrdersNoPage.TotalCount + "</td>");
			stringBuilder.AppendLine("<td>采购单金额共计," + purchaseOrdersNoPage.TotalOfSearch + "</td>");
			stringBuilder.AppendLine("<td>采购单毛利润共计," + purchaseOrdersNoPage.ProfitsOfSearch + "</td>");
			stringBuilder.AppendLine("<td></td>");
			stringBuilder.AppendLine("</tr>");
			stringBuilder.AppendLine("</table>");
			this.Page.Response.Clear();
			this.Page.Response.Buffer = false;
			this.Page.Response.Charset = "UTF-8";
			this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=PurchaseOrderStatistics.xls");
			this.Page.Response.ContentEncoding = System.Text.Encoding.UTF8;
			this.Page.Response.ContentType = "application/ms-excel";
			this.Page.EnableViewState = false;
			this.Page.Response.Write(stringBuilder.ToString());
			this.Page.Response.End();
		}
		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReBind(true);
		}
		private void grdPurchaseOrderStatistics_ReBindData(object sender)
		{
			this.ReBind(false);
		}
	}
}
