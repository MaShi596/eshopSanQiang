using ASPNET.WebControls;
using Hidistro.AccountCenter.Business;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	public class UserOrders : MemberTemplatedWebControl
	{
		private WebCalendar calendarStartDate;
		private WebCalendar calendarEndDate;
		private System.Web.UI.WebControls.TextBox txtOrderId;
		private System.Web.UI.WebControls.TextBox txtShipId;
		private System.Web.UI.WebControls.TextBox txtShipTo;
		private OrderStautsDropDownList dropOrderStatus;
		private System.Web.UI.WebControls.DropDownList dropPayType;
		private IButton btnSearch;
		private IButton btnPay;
		private IButton btnOk;
		private IButton btnReturn;
		private IButton btnReplace;
		private System.Web.UI.WebControls.Literal litOrderTotal;
		private System.Web.UI.HtmlControls.HtmlInputHidden hdorderId;
		private System.Web.UI.WebControls.TextBox txtRemark;
		private System.Web.UI.WebControls.TextBox txtReturnRemark;
		private System.Web.UI.WebControls.TextBox txtReplaceRemark;
		private Common_OrderManage_OrderList listOrders;
		private System.Web.UI.WebControls.DropDownList dropRefundType;
		private System.Web.UI.WebControls.DropDownList dropReturnRefundType;
		private Pager pager;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserOrders.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.calendarStartDate = (WebCalendar)this.FindControl("calendarStartDate");
			this.calendarEndDate = (WebCalendar)this.FindControl("calendarEndDate");
			this.hdorderId = (System.Web.UI.HtmlControls.HtmlInputHidden)this.FindControl("hdorderId");
			this.txtOrderId = (System.Web.UI.WebControls.TextBox)this.FindControl("txtOrderId");
			this.txtShipId = (System.Web.UI.WebControls.TextBox)this.FindControl("txtShipId");
			this.txtShipTo = (System.Web.UI.WebControls.TextBox)this.FindControl("txtShipTo");
			this.txtRemark = (System.Web.UI.WebControls.TextBox)this.FindControl("txtRemark");
			this.txtReturnRemark = (System.Web.UI.WebControls.TextBox)this.FindControl("txtReturnRemark");
			this.txtReplaceRemark = (System.Web.UI.WebControls.TextBox)this.FindControl("txtReplaceRemark");
			this.dropOrderStatus = (OrderStautsDropDownList)this.FindControl("dropOrderStatus");
			this.dropPayType = (System.Web.UI.WebControls.DropDownList)this.FindControl("dropPayType");
			this.btnPay = ButtonManager.Create(this.FindControl("btnPay"));
			this.btnSearch = ButtonManager.Create(this.FindControl("btnSearch"));
			this.btnOk = ButtonManager.Create(this.FindControl("btnOk"));
			this.btnReturn = ButtonManager.Create(this.FindControl("btnReturn"));
			this.btnReplace = ButtonManager.Create(this.FindControl("btnReplace"));
			this.litOrderTotal = (System.Web.UI.WebControls.Literal)this.FindControl("litOrderTotal");
			this.dropRefundType = (System.Web.UI.WebControls.DropDownList)this.FindControl("dropRefundType");
			this.dropReturnRefundType = (System.Web.UI.WebControls.DropDownList)this.FindControl("dropReturnRefundType");
			this.listOrders = (Common_OrderManage_OrderList)this.FindControl("Common_OrderManage_OrderList");
			this.pager = (Pager)this.FindControl("pager");
			this.btnSearch.Click += new System.EventHandler(this.lbtnSearch_Click);
			this.btnPay.Click += new System.EventHandler(this.btnPay_Click);
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
			this.btnReplace.Click += new System.EventHandler(this.btnReplace_Click);
			this.listOrders.ItemDataBound += new Common_OrderManage_OrderList.DataBindEventHandler(this.listOrders_ItemDataBound);
			this.listOrders.ItemCommand += new Common_OrderManage_OrderList.CommandEventHandler(this.listOrders_ItemCommand);
			this.listOrders.ReBindData += new Common_OrderManage_OrderList.ReBindDataEventHandler(this.listOrders_ReBindData);
			PageTitle.AddSiteNameTitle("我的订单", HiContext.Current.Context);
			if (!this.Page.IsPostBack)
			{
				this.dropPayType.DataSource = TradeHelper.GetPaymentModes();
				this.dropPayType.DataTextField = "Name";
				this.dropPayType.DataValueField = "ModeId";
				this.dropPayType.DataBind();
				this.BindOrders();
			}
		}
		private void btnPay_Click(object sender, System.EventArgs e)
		{
			string value = this.hdorderId.Value;
			int modeId = 0;
			int.TryParse(this.dropPayType.SelectedValue, out modeId);
			PaymentModeInfo paymentMode = TradeHelper.GetPaymentMode(modeId);
			if (paymentMode != null)
			{
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(value);
				orderInfo.PaymentTypeId = paymentMode.ModeId;
				orderInfo.PaymentType = paymentMode.Name;
				orderInfo.Gateway = paymentMode.Gateway;
				TradeHelper.UpdateOrderPaymentType(orderInfo);
			}
			this.Page.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("sendPayment", new object[]
			{
				value
			}));
		}
		private void btnReplace_Click(object sender, System.EventArgs e)
		{
			if (!TradeHelper.CanReplace(this.hdorderId.Value))
			{
				this.ShowMessage("已有待确认的申请！", false);
			}
			else
			{
				if (TradeHelper.ApplyForReplace(this.hdorderId.Value, this.txtReplaceRemark.Text))
				{
					this.BindOrders();
					this.ShowMessage("成功的申请了换货", true);
				}
				else
				{
					this.ShowMessage("申请换货失败", false);
				}
			}
		}
		private void btnReturn_Click(object sender, System.EventArgs e)
		{
			if (!TradeHelper.CanReturn(this.hdorderId.Value))
			{
				this.ShowMessage("已有待确认的申请！", false);
			}
			else
			{
				if (!this.CanReturnBalance())
				{
					this.ShowMessage("请先开通预付款账户", false);
				}
				else
				{
					if (TradeHelper.ApplyForReturn(this.hdorderId.Value, this.txtReturnRemark.Text, int.Parse(this.dropReturnRefundType.SelectedValue)))
					{
						this.BindOrders();
						this.ShowMessage("成功的申请了退货", true);
					}
					else
					{
						this.ShowMessage("申请退货失败", false);
					}
				}
			}
		}
		private void btnOk_Click(object sender, System.EventArgs e)
		{
			if (!TradeHelper.CanRefund(this.hdorderId.Value))
			{
				this.ShowMessage("已有待确认的申请！", false);
			}
			else
			{
				if (!this.CanRefundBalance())
				{
					this.ShowMessage("请先开通预付款账户", false);
				}
				else
				{
					if (TradeHelper.ApplyForRefund(this.hdorderId.Value, this.txtRemark.Text, int.Parse(this.dropRefundType.SelectedValue)))
					{
						this.BindOrders();
						this.ShowMessage("成功的申请了退款", true);
					}
					else
					{
						this.ShowMessage("申请退款失败", false);
					}
				}
			}
		}
		private bool CanReturnBalance()
		{
			bool result;
			if (System.Convert.ToInt32(this.dropReturnRefundType.SelectedValue) != 1)
			{
				result = true;
			}
			else
			{
				Member member = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
				result = member.IsOpenBalance;
			}
			return result;
		}
		private bool CanRefundBalance()
		{
			bool result;
			if (System.Convert.ToInt32(this.dropRefundType.SelectedValue) != 1)
			{
				result = true;
			}
			else
			{
				Member member = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
				result = member.IsOpenBalance;
			}
			return result;
		}
		protected void listOrders_ItemDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
		{
			if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
			{
				OrderStatus orderStatus = (OrderStatus)System.Web.UI.DataBinder.Eval(e.Row.DataItem, "OrderStatus");
				string a = "";
				if (System.Web.UI.DataBinder.Eval(e.Row.DataItem, "Gateway") != null && !(System.Web.UI.DataBinder.Eval(e.Row.DataItem, "Gateway") is System.DBNull))
				{
					a = (string)System.Web.UI.DataBinder.Eval(e.Row.DataItem, "Gateway");
				}
				System.Web.UI.WebControls.HyperLink hyperLink = (System.Web.UI.WebControls.HyperLink)e.Row.FindControl("hplinkorderreview");
				System.Web.UI.HtmlControls.HtmlAnchor htmlAnchor = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("hlinkPay");
				ImageLinkButton imageLinkButton = (ImageLinkButton)e.Row.FindControl("lkbtnConfirmOrder");
				ImageLinkButton imageLinkButton2 = (ImageLinkButton)e.Row.FindControl("lkbtnCloseOrder");
				System.Web.UI.HtmlControls.HtmlAnchor htmlAnchor2 = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("lkbtnApplyForRefund");
				System.Web.UI.HtmlControls.HtmlAnchor htmlAnchor3 = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("lkbtnApplyForReturn");
				System.Web.UI.HtmlControls.HtmlAnchor htmlAnchor4 = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("lkbtnApplyForReplace");
				if (hyperLink != null)
				{
					hyperLink.Visible = (orderStatus == OrderStatus.Finished);
				}
				htmlAnchor.Visible = (orderStatus == OrderStatus.WaitBuyerPay && a != "hishop.plugins.payment.podrequest");
				imageLinkButton.Visible = (orderStatus == OrderStatus.SellerAlreadySent);
				imageLinkButton2.Visible = (orderStatus == OrderStatus.WaitBuyerPay);
				htmlAnchor2.Visible = (orderStatus == OrderStatus.BuyerAlreadyPaid);
				htmlAnchor3.Visible = (orderStatus == OrderStatus.SellerAlreadySent);
				htmlAnchor4.Visible = (orderStatus == OrderStatus.SellerAlreadySent);
			}
		}
		protected void listOrders_ItemCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			OrderInfo orderInfo = TradeHelper.GetOrderInfo(e.CommandArgument.ToString());
			if (orderInfo != null)
			{
				if (e.CommandName == "FINISH_TRADE" && orderInfo.CheckAction(OrderActions.SELLER_FINISH_TRADE))
				{
					if (TradeHelper.ConfirmOrderFinish(orderInfo))
					{
						this.BindOrders();
						this.ShowMessage("成功的完成了该订单", true);
					}
					else
					{
						this.ShowMessage("完成订单失败", false);
					}
				}
				if (e.CommandName == "CLOSE_TRADE" && orderInfo.CheckAction(OrderActions.SELLER_CLOSE))
				{
					if (TradeHelper.CloseOrder(orderInfo.OrderId))
					{
						this.BindOrders();
						this.ShowMessage("成功的关闭了该订单", true);
					}
					else
					{
						this.ShowMessage("关闭订单失败", false);
					}
				}
			}
		}
		protected void listOrders_ReBindData(object sender)
		{
			this.ReloadUserOrders(false);
		}
		protected void lbtnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReloadUserOrders(true);
		}
		private void BindOrders()
		{
			OrderQuery orderQuery = this.GetOrderQuery();
			DbQueryResult userOrder = TradeHelper.GetUserOrder(HiContext.Current.User.UserId, orderQuery);
			this.listOrders.DataSource = userOrder.Data;
			this.listOrders.DataBind();
			this.txtOrderId.Text = orderQuery.OrderId;
			this.txtShipId.Text = orderQuery.ShipId;
			this.txtShipTo.Text = orderQuery.ShipTo;
			this.dropOrderStatus.SelectedValue = orderQuery.Status;
            this.calendarStartDate.SelectedDate = orderQuery.StartDate;
            this.calendarEndDate.SelectedDate = orderQuery.EndDate;
            this.pager.TotalRecords = userOrder.TotalRecords;
		}
		private OrderQuery GetOrderQuery()
		{
			OrderQuery orderQuery = new OrderQuery();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["orderId"]))
			{
				orderQuery.OrderId = this.Page.Request.QueryString["orderId"];
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["shipId"]))
			{
				orderQuery.ShipId = this.Page.Request.QueryString["shipId"];
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["shipTo"]))
			{
				orderQuery.ShipTo = this.Page.Request.QueryString["shipTo"];
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["orderId"]))
			{
				orderQuery.OrderId = this.Page.Request.QueryString["orderId"];
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]))
			{
				orderQuery.StartDate = new System.DateTime?(System.Convert.ToDateTime(this.Page.Server.UrlDecode(this.Page.Request.QueryString["startDate"])));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]))
			{
				orderQuery.EndDate = new System.DateTime?(System.Convert.ToDateTime(this.Page.Server.UrlDecode(this.Page.Request.QueryString["endDate"])));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["orderStatus"]))
			{
				int status = 0;
				if (int.TryParse(this.Page.Request.QueryString["orderStatus"], out status))
				{
					orderQuery.Status = (OrderStatus)status;
				}
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortBy"]))
			{
				orderQuery.SortBy = this.Page.Request.QueryString["sortBy"];
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["sortOrder"]))
			{
				int sortOrder = 0;
				if (int.TryParse(this.Page.Request.QueryString["sortOrder"], out sortOrder))
				{
					orderQuery.SortOrder = (SortAction)sortOrder;
				}
			}
			orderQuery.PageIndex = this.pager.PageIndex;
			orderQuery.PageSize = this.pager.PageSize;
			return orderQuery;
		}
		private void ReloadUserOrders(bool isSearch)
		{
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("orderId", this.txtOrderId.Text.Trim());
			nameValueCollection.Add("shipId", this.txtShipId.Text.Trim());
			nameValueCollection.Add("shipTo", this.txtShipTo.Text.Trim());
			nameValueCollection.Add("startDate", this.calendarStartDate.SelectedDate.ToString());
			nameValueCollection.Add("endDate", this.calendarEndDate.SelectedDate.ToString());
			nameValueCollection.Add("orderStatus", ((int)this.dropOrderStatus.SelectedValue).ToString());
			nameValueCollection.Add("sortBy", this.listOrders.SortOrderBy);
			nameValueCollection.Add("sortOrder", ((int)this.listOrders.SortOrder).ToString());
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
			}
			base.ReloadPage(nameValueCollection);
		}
	}
}
