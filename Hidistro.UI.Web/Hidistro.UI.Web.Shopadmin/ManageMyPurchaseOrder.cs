using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Sales;
using Hidistro.Subsites.Sales;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class ManageMyPurchaseOrder : DistributorPage
	{
		protected System.Web.UI.WebControls.HyperLink hlinkAllOrder;
		protected System.Web.UI.WebControls.HyperLink hlinkNotPay;
		protected System.Web.UI.WebControls.HyperLink hlinkYetPay;
		protected System.Web.UI.WebControls.HyperLink hlinkSendGoods;
		protected System.Web.UI.WebControls.HyperLink hlinkFinish;
		protected System.Web.UI.WebControls.HyperLink hlinkClose;
		protected System.Web.UI.WebControls.HyperLink hlinkHistory;
		protected WebCalendar calendarStartDate;
		protected WebCalendar calendarEndDate;
		protected System.Web.UI.WebControls.TextBox txtProductName;
		protected System.Web.UI.WebControls.TextBox txtOrderId;
		protected System.Web.UI.WebControls.TextBox txtPurchaseOrderId;
		protected System.Web.UI.WebControls.Label lblStatus;
		protected System.Web.UI.WebControls.Button btnSearchButton;
		protected PageSize hrefPageSize;
		protected Pager pager;
		protected ImageLinkButton btnBatchPayMoney;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidPurchaseOrderId;
		protected System.Web.UI.WebControls.DataList dlstPurchaseOrders;
		protected Pager pager1;
		protected DistributorClosePurchaseOrderReasonDropDownList ddlCloseReason;
		protected System.Web.UI.WebControls.Button btnClosePurchaseOrder;
		protected System.Web.UI.WebControls.DropDownList dropRefundType;
		protected System.Web.UI.WebControls.TextBox txtRemark;
		protected System.Web.UI.WebControls.Button btnOk;
		protected System.Web.UI.WebControls.DropDownList dropReturnRefundType;
		protected System.Web.UI.WebControls.TextBox txtReturnRemark;
		protected System.Web.UI.WebControls.Button btnReturn;
		protected System.Web.UI.WebControls.TextBox txtReplaceRemark;
		protected System.Web.UI.WebControls.Button btnReplace;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.dlstPurchaseOrders.ItemDataBound += new System.Web.UI.WebControls.DataListItemEventHandler(this.dlstPurchaseOrders_ItemDataBound);
			this.dlstPurchaseOrders.ItemCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.dlstPurchaseOrders_ItemCommand);
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
			this.btnBatchPayMoney.Click += new System.EventHandler(this.btnBatchPayMoney_Click);
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			this.btnReturn.Click += new System.EventHandler(this.btnReturn_Click);
			this.btnReplace.Click += new System.EventHandler(this.btnReplace_Click);
			this.btnClosePurchaseOrder.Click += new System.EventHandler(this.btnClosePurchaseOrder_Click);
			if (!this.Page.IsPostBack)
			{
				this.SetPurchaseOrderStatusLink();
				this.BindPurchaseOrders();
			}
		}
		private void btnReplace_Click(object sender, System.EventArgs e)
		{
			if (!SubsiteSalesHelper.CanPurchaseReplace(this.hidPurchaseOrderId.Value))
			{
				this.ShowMsg("已有待确认的申请！", false);
				return;
			}
			if (SubsiteSalesHelper.ApplyForPurchaseReplace(this.hidPurchaseOrderId.Value, this.txtReplaceRemark.Text))
			{
				this.BindPurchaseOrders();
				this.ShowMsg("成功的申请了换货", true);
				return;
			}
			this.ShowMsg("申请换货失败", false);
		}
		private void btnReturn_Click(object sender, System.EventArgs e)
		{
			if (!SubsiteSalesHelper.CanPurchaseReturn(this.hidPurchaseOrderId.Value))
			{
				this.ShowMsg("已有待确认的申请！", false);
				return;
			}
			if (SubsiteSalesHelper.ApplyForPurchaseReturn(this.hidPurchaseOrderId.Value, this.txtReturnRemark.Text, int.Parse(this.dropReturnRefundType.SelectedValue)))
			{
				this.BindPurchaseOrders();
				this.ShowMsg("成功的申请了退货", true);
				return;
			}
			this.ShowMsg("申请退货失败", false);
		}
		private void btnOk_Click(object sender, System.EventArgs e)
		{
			if (!SubsiteSalesHelper.CanPurchaseRefund(this.hidPurchaseOrderId.Value))
			{
				this.ShowMsg("已有待确认的申请！", false);
				return;
			}
			if (SubsiteSalesHelper.ApplyForPurchaseRefund(this.hidPurchaseOrderId.Value, this.txtRemark.Text, int.Parse(this.dropRefundType.SelectedValue)))
			{
				this.BindPurchaseOrders();
				this.ShowMsg("成功的申请了退款", true);
				return;
			}
			this.ShowMsg("申请退款失败", false);
		}
		private void SetPurchaseOrderStatusLink()
		{
			string format = Globals.ApplicationPath + "/Shopadmin/purchaseOrder/ManageMyPurchaseOrder.aspx?PurchaseStatus={0}";
			this.hlinkAllOrder.NavigateUrl = string.Format(format, 0);
			this.hlinkNotPay.NavigateUrl = string.Format(format, 1);
			this.hlinkYetPay.NavigateUrl = string.Format(format, 2);
			this.hlinkSendGoods.NavigateUrl = string.Format(format, 3);
			this.hlinkClose.NavigateUrl = string.Format(format, 4);
			this.hlinkHistory.NavigateUrl = string.Format(format, 99);
			this.hlinkFinish.NavigateUrl = string.Format(format, 5);
		}
		private PurchaseOrderQuery GetPurchaseOrderQuery()
		{
			PurchaseOrderQuery purchaseOrderQuery = new PurchaseOrderQuery();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
			{
				purchaseOrderQuery.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["PurchaseOrderId"]))
			{
				purchaseOrderQuery.PurchaseOrderId = Globals.UrlDecode(this.Page.Request.QueryString["PurchaseOrderId"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ProductName"]))
			{
				purchaseOrderQuery.ProductName = Globals.UrlDecode(this.Page.Request.QueryString["ProductName"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["startDate"]))
			{
				purchaseOrderQuery.StartDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["startDate"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["endDate"]))
			{
				purchaseOrderQuery.EndDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["endDate"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["PurchaseStatus"]))
			{
				int purchaseStatus = 0;
				if (int.TryParse(this.Page.Request.QueryString["PurchaseStatus"], out purchaseStatus))
				{
					purchaseOrderQuery.PurchaseStatus = (OrderStatus)purchaseStatus;
				}
			}
			purchaseOrderQuery.PageIndex = this.pager.PageIndex;
			purchaseOrderQuery.PageSize = this.pager.PageSize;
			return purchaseOrderQuery;
		}
		private void ReBinderPurchaseOrders(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("OrderId", this.txtOrderId.Text.Trim());
			nameValueCollection.Add("PurchaseOrderId", this.txtPurchaseOrderId.Text.Trim());
			nameValueCollection.Add("ProductName", this.txtProductName.Text.Trim());
			nameValueCollection.Add("PurchaseStatus", this.lblStatus.Text);
			if (this.calendarStartDate.SelectedDate.HasValue)
			{
				nameValueCollection.Add("StartDate", this.calendarStartDate.SelectedDate.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			if (this.calendarEndDate.SelectedDate.HasValue)
			{
				nameValueCollection.Add("EndDate", this.calendarEndDate.SelectedDate.Value.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			nameValueCollection.Add("PageSize", this.pager.PageSize.ToString());
			base.ReloadPage(nameValueCollection);
		}
		private void BindPurchaseOrders()
		{
			PurchaseOrderQuery purchaseOrderQuery = this.GetPurchaseOrderQuery();
			purchaseOrderQuery.SortOrder = SortAction.Desc;
			purchaseOrderQuery.SortBy = "PurchaseDate";
			purchaseOrderQuery.IsManualPurchaseOrder = false;
			DbQueryResult purchaseOrders = SubsiteSalesHelper.GetPurchaseOrders(purchaseOrderQuery);
			this.dlstPurchaseOrders.DataSource = purchaseOrders.Data;
			this.dlstPurchaseOrders.DataBind();
            this.pager.TotalRecords = purchaseOrders.TotalRecords;
            this.pager1.TotalRecords = purchaseOrders.TotalRecords;
			this.txtOrderId.Text = purchaseOrderQuery.OrderId;
			this.txtProductName.Text = purchaseOrderQuery.ProductName;
			this.txtPurchaseOrderId.Text = purchaseOrderQuery.PurchaseOrderId;
            this.calendarStartDate.SelectedDate = purchaseOrderQuery.StartDate;
            this.calendarEndDate.SelectedDate = purchaseOrderQuery.EndDate;
			this.lblStatus.Text = ((int)purchaseOrderQuery.PurchaseStatus).ToString();
		}
		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReBinderPurchaseOrders(false);
		}
		private void btnBatchPayMoney_Click(object sender, System.EventArgs e)
		{
			string text = "";
			if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
			{
				text = base.Request["CheckBoxGroup"];
			}
			if (text.Length <= 0)
			{
				this.ShowMsg("请选要付款的采购单", false);
				return;
			}
			this.Page.Response.Redirect("BatchPay.aspx?PurchaseOrderIds=" + text);
		}
		private void dlstPurchaseOrders_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				System.Web.UI.WebControls.HyperLink hyperLink = (System.Web.UI.WebControls.HyperLink)e.Item.FindControl("lkbtnPay");
				System.Web.UI.WebControls.HyperLink hyperLink2 = (System.Web.UI.WebControls.HyperLink)e.Item.FindControl("lkbtnSendGoods");
				System.Web.UI.HtmlControls.HtmlGenericControl htmlGenericControl = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("lkBtnCancelPurchaseOrder");
				ImageLinkButton imageLinkButton = (ImageLinkButton)e.Item.FindControl("lkbtnConfirmPurchaseOrder");
				System.Web.UI.HtmlControls.HtmlAnchor htmlAnchor = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lkbtnApplyForPurchaseRefund");
				System.Web.UI.HtmlControls.HtmlAnchor htmlAnchor2 = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lkbtnApplyForPurchaseReturn");
				System.Web.UI.HtmlControls.HtmlAnchor htmlAnchor3 = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lkbtnApplyForPurchaseReplace");
				OrderStatus orderStatus = (OrderStatus)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "PurchaseStatus");
				string orderId = (string)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "OrderId");
				System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litPayment");
				if (orderStatus == OrderStatus.SellerAlreadySent)
				{
					OrderInfo orderInfo = SubsiteSalesHelper.GetOrderInfo(orderId);
					if (orderInfo != null && orderInfo.OrderStatus == OrderStatus.BuyerAlreadyPaid)
					{
						hyperLink2.Visible = true;
					}
					else
					{
						imageLinkButton.Visible = true;
					}
					htmlAnchor2.Visible = true;
					htmlAnchor3.Visible = true;
				}
				if (orderStatus == OrderStatus.WaitBuyerPay)
				{
					hyperLink.Visible = true;
					htmlGenericControl.Visible = true;
					if (System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Gateway") != System.DBNull.Value && "hishop.plugins.payment.podrequest" == (string)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Gateway"))
					{
						hyperLink.Visible = false;
					}
				}
				if (orderStatus == OrderStatus.BuyerAlreadyPaid)
				{
					htmlAnchor.Visible = true;
				}
				string purchaseOrderId = this.dlstPurchaseOrders.DataKeys[e.Item.ItemIndex].ToString();
				PurchaseOrderInfo purchaseOrder = SubsiteSalesHelper.GetPurchaseOrder(purchaseOrderId);
				if (string.IsNullOrEmpty(purchaseOrder.PaymentType))
				{
					if (orderStatus == OrderStatus.BuyerAlreadyPaid)
					{
						literal.Text = "<br>支付方式：预付款";
						return;
					}
				}
				else
				{
					literal.Text = "<br>支付方式：" + purchaseOrder.PaymentType;
				}
			}
		}
		private void dlstPurchaseOrders_ItemCommand(object sender, System.Web.UI.WebControls.DataListCommandEventArgs e)
		{
			PurchaseOrderInfo purchaseOrder = SubsiteSalesHelper.GetPurchaseOrder(e.CommandArgument.ToString());
			if (purchaseOrder != null && e.CommandName == "FINISH_TRADE" && purchaseOrder.CheckAction(PurchaseOrderActions.MASTER_FINISH_TRADE))
			{
				if (SubsiteSalesHelper.ConfirmPurchaseOrderFinish(purchaseOrder))
				{
					this.BindPurchaseOrders();
					this.ShowMsg("成功的完成了该采购单", true);
					return;
				}
				this.ShowMsg("完成采购单失败", false);
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
