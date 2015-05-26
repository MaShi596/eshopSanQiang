using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Sales;
using Hidistro.Subsites.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class Default : DistributorPage
	{
		protected System.Web.UI.WebControls.Literal ltrAdminName;
		protected System.Web.UI.WebControls.HyperLink hpkMessages;
		protected System.Web.UI.WebControls.HyperLink hpkZiXun;
		protected System.Web.UI.WebControls.HyperLink hpkLiuYan;
		protected FormatedTimeLabel lblTime;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidStatus;
		protected Grid grdPurchaseOrders;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidPurchaseOrderId;
		protected DistributorClosePurchaseOrderReasonDropDownList ddlCloseReason;
		protected System.Web.UI.WebControls.Button btnClosePurchaseOrder;
		protected Grid grdOrders;
		protected System.Web.UI.WebControls.Label lblOrderNumbers;
		protected System.Web.UI.WebControls.HyperLink hpksendOrder;
		protected System.Web.UI.WebControls.HyperLink allorders;
		protected System.Web.UI.WebControls.Label lblPurchaseOrderNumbers;
		protected System.Web.UI.WebControls.HyperLink allPurchaseOrder;
		protected System.Web.UI.WebControls.HyperLink allPurchaseOrder2;
		protected ClassShowOnDataLitl lblTodayOrderAmout;
		protected ClassShowOnDataLitl lblTodaySalesProfile;
		protected ClassShowOnDataLitl ltrTodayAddMemberNumber;
		protected ClassShowOnDataLitl ltrWaitSendPurchaseOrdersNumber;
		protected ClassShowOnDataLitl ltrWaitSendOrdersNumber;
		protected ClassShowOnDataLitl lblProductCountTotal;
		protected ClassShowOnDataLitl lblMembersBalanceTotal;
		protected ClassShowOnDataLitl lblDistrosBalanceTotal;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.grdOrders.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.grdOrders_RowDataBound);
			this.grdPurchaseOrders.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.grdPurchaseOrders_RowDataBound);
			this.btnClosePurchaseOrder.Click += new System.EventHandler(this.btnClosePurchaseOrder_Click);
			if (!base.IsPostBack)
			{
				int num;
				if (int.TryParse(base.Request.QueryString["Status"], out num))
				{
					this.hidStatus.Value = num.ToString();
				}
				this.BindLabels();
				StatisticsInfo statistics = SubsiteSalesHelper.GetStatistics();
				this.BindBusinessInformation(statistics);
				this.BindPurchaseOrders();
				this.BindOrders();
			}
		}
		private void BindLabels()
		{
			Hidistro.Membership.Context.Distributor distributor = SubsiteStoreHelper.GetDistributor();
			this.ltrAdminName.Text = distributor.Username;
			this.lblTime.Time = distributor.LastLoginDate;
			AccountSummaryInfo myAccountSummary = SubsiteStoreHelper.GetMyAccountSummary();
			this.lblDistrosBalanceTotal.Text = ((myAccountSummary.AccountAmount > 0m) ? Globals.FormatMoney(myAccountSummary.AccountAmount) : string.Empty);
		}
		private void BindBusinessInformation(StatisticsInfo statisticsInfo)
		{
			this.ltrWaitSendOrdersNumber.Text = statisticsInfo.OrderNumbWaitConsignment.ToString();
			this.hpkZiXun.Text = statisticsInfo.ProductConsultations.ToString();
			this.hpkMessages.Text = statisticsInfo.Messages.ToString();
			this.hpkLiuYan.Text = statisticsInfo.LeaveComments.ToString();
			this.lblTodayOrderAmout.Text = Globals.FormatMoney(statisticsInfo.OrderPriceToday);
			this.lblTodaySalesProfile.Text = Globals.FormatMoney(statisticsInfo.OrderProfitToday);
			this.ltrTodayAddMemberNumber.Text = statisticsInfo.UserNewAddToday.ToString();
			this.lblMembersBalanceTotal.Text = Globals.FormatMoney(statisticsInfo.Balance);
			this.lblProductCountTotal.Text = ((statisticsInfo.ProductAlert > 0) ? string.Concat(new string[]
			{
				"<a href='",
				Globals.ApplicationPath,
				"/Shopadmin/product/myproductonsales.aspx?isAlert=True'>",
				statisticsInfo.ProductAlert.ToString(),
				"</a>"
			}) : "0");
			this.ltrWaitSendPurchaseOrdersNumber.Text = statisticsInfo.PurchaseOrderNumbWaitConsignment.ToString();
			this.hpkLiuYan.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/comment/ManageMyLeaveComments.aspx?MessageStatus=3";
			this.hpkZiXun.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/comment/MyProductConsultations.aspx";
			this.hpkMessages.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/comment/MyReceivedMessages.aspx?IsRead=0";
		}
		private void BindPurchaseOrders()
		{
			int num;
			System.Data.DataTable recentlyPurchaseOrders = SubsiteSalesHelper.GetRecentlyPurchaseOrders(out num);
			this.lblPurchaseOrderNumbers.Text = recentlyPurchaseOrders.Rows.Count.ToString();
			this.allPurchaseOrder.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/purchaseOrder/ManageMyPurchaseOrder.aspx";
			this.allPurchaseOrder2.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/purchaseOrder/ManageMyManualPurchaseOrder.aspx";
			this.grdPurchaseOrders.DataSource = recentlyPurchaseOrders;
			this.grdPurchaseOrders.DataBind();
		}
		private void BindOrders()
		{
			int num;
			System.Data.DataTable recentlyOrders = SubsiteSalesHelper.GetRecentlyOrders(out num);
			this.lblOrderNumbers.Text = recentlyOrders.Rows.Count.ToString();
			this.hpksendOrder.Text = num.ToString();
			this.hpksendOrder.NavigateUrl = Globals.ApplicationPath + string.Format("/Shopadmin/sales/ManageMyOrder.aspx?OrderStatus={0}", 2);
			this.allorders.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/sales/ManageMyOrder.aspx";
			this.grdOrders.DataSource = recentlyOrders;
			this.grdOrders.DataBind();
		}
		protected void grdOrders_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
		{
			if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
			{
				OrderStatus orderStatus = (OrderStatus)System.Web.UI.DataBinder.Eval(e.Row.DataItem, "OrderStatus");
				System.Web.UI.WebControls.HyperLink hyperLink = (System.Web.UI.WebControls.HyperLink)e.Row.FindControl("lkbtnEditPrice");
				System.Web.UI.WebControls.HyperLink hyperLink2 = (System.Web.UI.WebControls.HyperLink)e.Row.FindControl("lkbtnSendGoods");
				if (orderStatus == OrderStatus.WaitBuyerPay)
				{
					hyperLink.Visible = true;
					hyperLink.Text += "<br />";
				}
				if (orderStatus == OrderStatus.BuyerAlreadyPaid)
				{
					int num = (int)System.Web.UI.DataBinder.Eval(e.Row.DataItem, "GroupBuyId");
					if (num > 0)
					{
						GroupBuyStatus groupBuyStatus = (GroupBuyStatus)System.Web.UI.DataBinder.Eval(e.Row.DataItem, "GroupBuyStatus");
						if (groupBuyStatus == GroupBuyStatus.Success)
						{
							hyperLink2.Visible = true;
							hyperLink2.Text += "<br />";
							return;
						}
					}
					else
					{
						hyperLink2.Visible = true;
						hyperLink2.Text += "<br />";
					}
				}
			}
		}
		private void grdPurchaseOrders_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
		{
			if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
			{
				System.Web.UI.WebControls.HyperLink hyperLink = (System.Web.UI.WebControls.HyperLink)e.Row.FindControl("lkbtnSendGoods");
				System.Web.UI.HtmlControls.HtmlGenericControl htmlGenericControl = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Row.FindControl("lkBtnCancelPurchaseOrder");
				System.Web.UI.WebControls.HyperLink hyperLink2 = (System.Web.UI.WebControls.HyperLink)e.Row.FindControl("lkbtnPay");
				OrderStatus orderStatus = (OrderStatus)System.Web.UI.DataBinder.Eval(e.Row.DataItem, "PurchaseStatus");
				string purchaseOrderId = (string)System.Web.UI.DataBinder.Eval(e.Row.DataItem, "PurchaseOrderId");
				PurchaseOrderInfo purchaseOrder = SubsiteSalesHelper.GetPurchaseOrder(purchaseOrderId);
				if (!purchaseOrder.IsManualPurchaseOrder && orderStatus == OrderStatus.SellerAlreadySent)
				{
					OrderInfo orderInfo = SubsiteSalesHelper.GetOrderInfo(purchaseOrder.OrderId);
					if (orderInfo != null && orderInfo.OrderStatus == OrderStatus.BuyerAlreadyPaid)
					{
						hyperLink.Visible = true;
					}
				}
				if (orderStatus == OrderStatus.WaitBuyerPay)
				{
					htmlGenericControl.Visible = true;
					htmlGenericControl.InnerHtml += "<br />";
					hyperLink2.Visible = true;
				}
			}
		}
		private void btnClosePurchaseOrder_Click(object sender, System.EventArgs e)
		{
			if (!string.IsNullOrEmpty(this.hidPurchaseOrderId.Value))
			{
				string value = this.hidPurchaseOrderId.Value;
				PurchaseOrderInfo purchaseOrder = SubsiteSalesHelper.GetPurchaseOrder(value);
				purchaseOrder.CloseReason = this.ddlCloseReason.SelectedValue;
				if (SubsiteSalesHelper.ClosePurchaseOrder(purchaseOrder))
				{
					this.BindPurchaseOrders();
					this.ShowMsg("取消采购成功", true);
					return;
				}
				this.ShowMsg("取消采购失败", false);
			}
		}
	}
}
