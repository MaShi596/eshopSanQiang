using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
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
	[PrivilegeCheck(Privilege.OrderStatistics)]
	public class OrderStatistics : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtUserName;
		protected System.Web.UI.WebControls.TextBox txtShipTo;
		protected System.Web.UI.WebControls.TextBox txtOrderId;
		protected WebCalendar calendarStartDate;
		protected WebCalendar calendarEndDate;
		protected System.Web.UI.WebControls.Button btnSearchButton;
		protected System.Web.UI.WebControls.LinkButton btnCreateReport;
		protected System.Web.UI.WebControls.Repeater rp_orderStatistics;
		protected System.Web.UI.WebControls.Label lblPageCount;
		protected System.Web.UI.WebControls.Label lblSearchCount;
		protected Pager pager;
		private string userName;
		private string shipTo;
		private System.DateTime? dateStart;
		private System.DateTime? dateEnd;
		private string orderId;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
			this.btnCreateReport.Click += new System.EventHandler(this.btnCreateReport_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
				this.BindUserOrderStatistics();
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
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["shipTo"]))
				{
					this.shipTo = base.Server.UrlDecode(this.Page.Request.QueryString["shipTo"]);
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dateStart"]))
				{
					this.dateStart = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["dateStart"]));
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["dateEnd"]))
				{
					this.dateEnd = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["dateEnd"]));
				}
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["orderId"]))
				{
					this.orderId = Globals.UrlDecode(this.Page.Request.QueryString["orderId"]);
				}
				this.txtUserName.Text = this.userName;
				this.txtShipTo.Text = this.shipTo;
                this.calendarStartDate.SelectedDate = this.dateStart;
                this.calendarEndDate.SelectedDate = this.dateEnd;
				this.txtOrderId.Text = this.orderId;
				return;
			}
			this.userName = this.txtUserName.Text;
			this.shipTo = this.txtShipTo.Text;
			this.dateStart = this.calendarStartDate.SelectedDate;
			this.dateEnd = this.calendarEndDate.SelectedDate;
			this.orderId = this.txtOrderId.Text;
		}
		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("userName", this.txtUserName.Text);
			nameValueCollection.Add("shipTo", this.txtShipTo.Text);
			nameValueCollection.Add("dateStart", this.calendarStartDate.SelectedDate.ToString());
			nameValueCollection.Add("dateEnd", this.calendarEndDate.SelectedDate.ToString());
			nameValueCollection.Add("orderId", this.txtOrderId.Text);
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			base.ReloadPage(nameValueCollection);
		}
		protected void rp_orderStatistics_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			System.Web.UI.WebControls.Repeater repeater = (System.Web.UI.WebControls.Repeater)e.Item.FindControl("rp_orderitem");
			string a = ((System.Data.DataRowView)e.Item.DataItem)["OrderId"].ToString();
			if (a != "")
			{
				repeater.DataSource = OrderHelper.GetLineItemInfo(a);
				repeater.DataBind();
			}
		}
		private void BindUserOrderStatistics()
		{
			OrderStatisticsInfo userOrders = SalesHelper.GetUserOrders(new UserOrderQuery
			{
				UserName = this.userName,
				ShipTo = this.shipTo,
				StartDate = this.dateStart,
				EndDate = this.dateEnd,
				OrderId = this.orderId,
				PageSize = this.pager.PageSize,
				PageIndex = this.pager.PageIndex,
				SortBy = "OrderDate",
				SortOrder = SortAction.Desc
			});
			this.rp_orderStatistics.DataSource = userOrders.OrderTbl;
			this.rp_orderStatistics.DataBind();
            this.pager.TotalRecords = userOrders.TotalCount;
			this.lblPageCount.Text = string.Format("当前页共计<span class=\"colorG\">{0}</span>个 <span style=\"padding-left:10px;\">订单金额共计</span><span class=\"colorG\">{1}</span>元 <span style=\"padding-left:10px;\">订单毛利润共计</span><span class=\"colorG\">{2}</span>元 ", userOrders.OrderTbl.Rows.Count, Globals.FormatMoney(userOrders.TotalOfPage), Globals.FormatMoney(userOrders.ProfitsOfPage));
			this.lblSearchCount.Text = string.Format("当前查询结果共计<span class=\"colorG\">{0}</span>个 <span style=\"padding-left:10px;\">订单金额共计</span><span class=\"colorG\">{1}</span>元 <span style=\"padding-left:10px;\">订单毛利润共计</span><span class=\"colorG\">{2}</span>元 ", userOrders.TotalCount, Globals.FormatMoney(userOrders.TotalOfSearch), Globals.FormatMoney(userOrders.ProfitsOfSearch));
		}
		private void btnCreateReport_Click(object sender, System.EventArgs e)
		{
			OrderStatisticsInfo userOrdersNoPage = SalesHelper.GetUserOrdersNoPage(new UserOrderQuery
			{
				UserName = this.userName,
				ShipTo = this.shipTo,
				StartDate = this.dateStart,
				EndDate = this.dateEnd,
				OrderId = this.orderId,
				PageSize = this.pager.PageSize,
				PageIndex = this.pager.PageIndex,
				SortBy = "OrderDate",
				SortOrder = SortAction.Desc
			});
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
			stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
			stringBuilder.AppendLine("<td>订单号</td>");
			stringBuilder.AppendLine("<td>下单时间</td>");
			stringBuilder.AppendLine("<td>总订单金额</td>");
			stringBuilder.AppendLine("<td>用户名</td>");
			stringBuilder.AppendLine("<td>收货人</td>");
			stringBuilder.AppendLine("<td>利润</td>");
			stringBuilder.AppendLine("</tr>");
			foreach (System.Data.DataRow dataRow in userOrdersNoPage.OrderTbl.Rows)
			{
				stringBuilder.AppendLine("<tr>");
				stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["OrderId"].ToString() + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["OrderDate"].ToString() + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["Total"].ToString() + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["UserName"].ToString() + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["ShipTo"].ToString() + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["Profits"].ToString() + "</td>");
				stringBuilder.AppendLine("</tr>");
			}
			stringBuilder.AppendLine("<tr>");
			stringBuilder.AppendLine("<td>当前查询结果共计," + userOrdersNoPage.TotalCount + "</td>");
			stringBuilder.AppendLine("<td>订单金额共计," + userOrdersNoPage.TotalOfSearch + "</td>");
			stringBuilder.AppendLine("<td>订单毛利润共计," + userOrdersNoPage.ProfitsOfSearch + "</td>");
			stringBuilder.AppendLine("<td></td>");
			stringBuilder.AppendLine("</tr>");
			stringBuilder.AppendLine("</table>");
			this.Page.Response.Clear();
			this.Page.Response.Buffer = false;
			this.Page.Response.Charset = "GB2312";
			this.Page.Response.AppendHeader("Content-Disposition", "attachment;filename=UserOrderStatistics.xls");
			this.Page.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
			this.Page.Response.ContentType = "application/ms-excel";
			this.Page.EnableViewState = false;
			this.Page.Response.Write(stringBuilder.ToString());
			this.Page.Response.End();
		}
		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReBind(true);
		}
	}
}
