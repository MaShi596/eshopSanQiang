using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Hishop.Web.CustomMade;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ManagePurchaseorder)]
	public class ManagePurchaseOrder : AdminPage
	{
		protected System.Web.UI.WebControls.HyperLink hlinkAllOrder;
		protected System.Web.UI.WebControls.HyperLink hlinkNotPay;
		protected System.Web.UI.WebControls.HyperLink hlinkYetPay;
		protected System.Web.UI.WebControls.HyperLink hlinkSendGoods;
		protected System.Web.UI.WebControls.HyperLink hlinkTradeFinished;
		protected System.Web.UI.WebControls.HyperLink hlinkClose;
		protected System.Web.UI.WebControls.HyperLink hlinkHistory;
		protected System.Web.UI.HtmlControls.HtmlInputHidden lblPurchaseOrderId;
		protected WebCalendar calendarStartDate;
		protected WebCalendar calendarEndDate;
		protected System.Web.UI.WebControls.TextBox txtDistributorName;
		protected System.Web.UI.WebControls.TextBox txtProductName;
		protected System.Web.UI.WebControls.TextBox txtOrderId;
		protected System.Web.UI.WebControls.TextBox txtPurchaseOrderId;
		protected System.Web.UI.WebControls.Label lblStatus;
		protected System.Web.UI.WebControls.TextBox txtShopTo;
		protected ShippingModeDropDownList shippingModeDropDownList;
		protected System.Web.UI.WebControls.DropDownList ddlIsPrinted;
		protected System.Web.UI.WebControls.Button btnSearchButton;
		protected PageSize hrefPageSize;
		protected Pager pager;
		protected ImageLinkButton lkbtnDeleteCheck;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidPurchaseOrderId;
		protected System.Web.UI.WebControls.DataList dlstPurchaseOrders;
		protected Pager pager1;
		protected System.Web.UI.WebControls.Label lblPurchaseOrderAmount;
		protected System.Web.UI.WebControls.TextBox txtPurchaseOrderDiscount;
		protected System.Web.UI.WebControls.Label lblPurchaseOrderAmount1;
		protected System.Web.UI.WebControls.Label lblPurchaseOrderAmount2;
		protected System.Web.UI.WebControls.Label lblPurchaseOrderAmount3;
		protected System.Web.UI.HtmlControls.HtmlGenericControl spanOrderId;
		protected System.Web.UI.HtmlControls.HtmlGenericControl spanpurcharseOrderId;
		protected System.Web.UI.HtmlControls.HtmlGenericControl lblpurchaseDateForRemark;
		protected FormatedMoneyLabel lblpurchaseTotalForRemark;
		protected OrderRemarkImageRadioButtonList orderRemarkImageForRemark;
		protected System.Web.UI.WebControls.TextBox txtRemark;
		protected ClosePurchaseOrderReasonDropDownList ddlCloseReason;
		protected System.Web.UI.WebControls.Label refund_lblPurchaseOrderId;
		protected System.Web.UI.WebControls.Label lblPurchaseOrderTotal;
		protected System.Web.UI.WebControls.Label lblRefundType;
		protected System.Web.UI.WebControls.Label lblRefundRemark;
		protected System.Web.UI.WebControls.Label lblContacts;
		protected System.Web.UI.WebControls.Label lblEmail;
		protected System.Web.UI.WebControls.Label lblTelephone;
		protected System.Web.UI.WebControls.Label lblAddress;
		protected System.Web.UI.WebControls.TextBox txtAdminRemark;
		protected System.Web.UI.WebControls.Label return_lblPurchaseOrderId;
		protected System.Web.UI.WebControls.Label return_lblPurchaseOrderTotal;
		protected System.Web.UI.WebControls.Label return_lblRefundType;
		protected System.Web.UI.WebControls.Label return_lblReturnRemark;
		protected System.Web.UI.WebControls.Label return_lblContacts;
		protected System.Web.UI.WebControls.Label return_lblEmail;
		protected System.Web.UI.WebControls.Label return_lblTelephone;
		protected System.Web.UI.WebControls.Label return_lblAddress;
		protected System.Web.UI.WebControls.TextBox return_txtRefundMoney;
		protected System.Web.UI.WebControls.TextBox return_txtAdminRemark;
		protected System.Web.UI.WebControls.Label replace_lblOrderId;
		protected System.Web.UI.WebControls.Label replace_lblOrderTotal;
		protected System.Web.UI.WebControls.Label replace_lblComments;
		protected System.Web.UI.WebControls.Label replace_lblContacts;
		protected System.Web.UI.WebControls.Label replace_lblEmail;
		protected System.Web.UI.WebControls.Label replace_lblTelephone;
		protected System.Web.UI.WebControls.Label replace_lblAddress;
		protected System.Web.UI.WebControls.Label replace_lblPostCode;
		protected System.Web.UI.WebControls.TextBox replace_txtAdminRemark;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidOrderTotal;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidRefundType;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidRefundMoney;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidAdminRemark;
		protected System.Web.UI.WebControls.Button btnCloseOrder;
		protected System.Web.UI.WebControls.Button btnAcceptRefund;
		protected System.Web.UI.WebControls.Button btnRefuseRefund;
		protected System.Web.UI.WebControls.Button btnAcceptReturn;
		protected System.Web.UI.WebControls.Button btnRefuseReturn;
		protected System.Web.UI.WebControls.Button btnAcceptReplace;
		protected System.Web.UI.WebControls.Button btnRefuseReplace;
		protected System.Web.UI.WebControls.Button btnClosePurchaseOrder;
		protected System.Web.UI.WebControls.Button btnRemark;
		protected System.Web.UI.WebControls.Button btnEditOrder;
		protected System.Web.UI.WebControls.Button btnOrderGoods;
		protected System.Web.UI.WebControls.Button btnProductGoods;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.dlstPurchaseOrders.ItemDataBound += new System.Web.UI.WebControls.DataListItemEventHandler(this.dlstPurchaseOrders_ItemDataBound);
			this.dlstPurchaseOrders.ItemCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.dlstPurchaseOrders_ItemCommand);
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
			this.btnRemark.Click += new System.EventHandler(this.btnRemark_Click);
			this.btnClosePurchaseOrder.Click += new System.EventHandler(this.btnClosePurchaseOrder_Click);
			this.lkbtnDeleteCheck.Click += new System.EventHandler(this.lkbtnDeleteCheck_Click);
			this.btnEditOrder.Click += new System.EventHandler(this.btnEditOrder_Click);
			this.btnOrderGoods.Click += new System.EventHandler(this.btnOrderGoods_Click);
			this.btnProductGoods.Click += new System.EventHandler(this.btnProductGoods_Click);
			this.btnAcceptRefund.Click += new System.EventHandler(this.btnAcceptRefund_Click);
			this.btnRefuseRefund.Click += new System.EventHandler(this.btnRefuseRefund_Click);
			this.btnAcceptReturn.Click += new System.EventHandler(this.btnAcceptReturn_Click);
			this.btnRefuseReturn.Click += new System.EventHandler(this.btnRefuseReturn_Click);
			this.btnAcceptReplace.Click += new System.EventHandler(this.btnAcceptReplace_Click);
			this.btnRefuseReplace.Click += new System.EventHandler(this.btnRefuseReplace_Click);
			if (!string.IsNullOrEmpty(base.Request["isCallback"]) && base.Request["isCallback"] == "true")
			{
				if (string.IsNullOrEmpty(base.Request["purchaseOrderId"]))
				{
					base.Response.Write("{\"Status\":\"0\"}");
					base.Response.End();
					return;
				}
				PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(base.Request["purchaseOrderId"]);
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
				int num;
				string text;
				if (base.Request["type"] == "refund")
				{
					SalesHelper.GetPurchaseRefundType(base.Request["purchaseOrderId"], out num, out text);
				}
				else
				{
					if (base.Request["type"] == "return")
					{
						SalesHelper.GetPurchaseRefundTypeFromReturn(base.Request["purchaseOrderId"], out num, out text);
					}
					else
					{
						num = 0;
						text = "";
					}
				}
				string arg;
				if (num == 1)
				{
					arg = "退到预存款";
				}
				else
				{
					arg = "银行转帐";
				}
				stringBuilder.AppendFormat(",\"OrderTotal\":\"{0}\"", Globals.FormatMoney(purchaseOrder.GetPurchaseTotal()));
				if (base.Request["type"] == "replace")
				{
					string purchaseReplaceComments = SalesHelper.GetPurchaseReplaceComments(base.Request["purchaseOrderId"]);
					stringBuilder.AppendFormat(",\"Comments\":\"{0}\"", purchaseReplaceComments.Replace("\r\n", ""));
				}
				else
				{
					stringBuilder.AppendFormat(",\"RefundType\":\"{0}\"", num);
					stringBuilder.AppendFormat(",\"RefundTypeStr\":\"{0}\"", arg);
				}
				stringBuilder.AppendFormat(",\"Remark\":\"{0}\"", text.Replace("\r\n", ""));
				stringBuilder.AppendFormat(",\"Contacts\":\"{0}\"", purchaseOrder.DistributorRealName);
				stringBuilder.AppendFormat(",\"Email\":\"{0}\"", purchaseOrder.DistributorEmail);
				stringBuilder.AppendFormat(",\"Telephone\":\"{0}\"", purchaseOrder.TelPhone);
				stringBuilder.AppendFormat(",\"Address\":\"{0}\"", purchaseOrder.Address);
				stringBuilder.AppendFormat(",\"PostCode\":\"{0}\"", purchaseOrder.ZipCode);
				base.Response.Clear();
				base.Response.ContentType = "application/json";
				base.Response.Write("{\"Status\":\"1\"" + stringBuilder.ToString() + "}");
				base.Response.End();
			}
			if (!this.Page.IsPostBack)
			{
				this.shippingModeDropDownList.DataBind();
				this.ddlIsPrinted.Items.Clear();
				this.ddlIsPrinted.Items.Add(new System.Web.UI.WebControls.ListItem("全部", string.Empty));
				this.ddlIsPrinted.Items.Add(new System.Web.UI.WebControls.ListItem("已打印", "1"));
				this.ddlIsPrinted.Items.Add(new System.Web.UI.WebControls.ListItem("未打印", "0"));
				this.SetPurchaseOrderStatusLink();
				this.BindPurchaseOrders();
			}
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
		}
		private void btnRefuseReplace_Click(object sender, System.EventArgs e)
		{
			SalesHelper.CheckPurchaseReplace(this.hidPurchaseOrderId.Value, this.hidAdminRemark.Value, false);
			this.BindPurchaseOrders();
			this.ShowMsg("成功的拒绝了采购单换货", true);
		}
		private void btnAcceptReplace_Click(object sender, System.EventArgs e)
		{
			SalesHelper.CheckPurchaseReplace(this.hidPurchaseOrderId.Value, this.hidAdminRemark.Value, true);
			this.BindPurchaseOrders();
			this.ShowMsg("成功的确认了采购单换货", true);
		}
		private void btnRefuseReturn_Click(object sender, System.EventArgs e)
		{
			SalesHelper.CheckPurchaseReturn(this.hidPurchaseOrderId.Value, Hidistro.Membership.Context.HiContext.Current.User.Username, 0m, this.hidAdminRemark.Value, int.Parse(this.hidRefundType.Value), false);
			this.BindPurchaseOrders();
			this.ShowMsg("成功的拒绝了采购单退货", true);
		}
		private void btnAcceptReturn_Click(object sender, System.EventArgs e)
		{
			decimal refundMoney;
			if (!decimal.TryParse(this.hidRefundMoney.Value, out refundMoney))
			{
				this.ShowMsg("请输入正确的退款金额", false);
				return;
			}
			SalesHelper.CheckPurchaseReturn(this.hidPurchaseOrderId.Value, Hidistro.Membership.Context.HiContext.Current.User.Username, refundMoney, this.hidAdminRemark.Value, int.Parse(this.hidRefundType.Value), true);
			this.BindPurchaseOrders();
			this.ShowMsg("成功的确认了采购单退货", true);
		}
		protected void btnAcceptRefund_Click(object sender, System.EventArgs e)
		{
			PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(this.hidPurchaseOrderId.Value);
			SalesHelper.CheckPurchaseRefund(purchaseOrder, Hidistro.Membership.Context.HiContext.Current.User.Username, this.hidAdminRemark.Value, int.Parse(this.hidRefundType.Value), true);
			this.BindPurchaseOrders();
			this.ShowMsg("成功的确认了采购单退款", true);
		}
		private void btnRefuseRefund_Click(object sender, System.EventArgs e)
		{
			PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(this.hidPurchaseOrderId.Value);
			SalesHelper.CheckPurchaseRefund(purchaseOrder, Hidistro.Membership.Context.HiContext.Current.User.Username, this.hidAdminRemark.Value, int.Parse(this.hidRefundType.Value), false);
			this.BindPurchaseOrders();
			this.ShowMsg("成功的拒绝了采购单退款", true);
		}
		private void SetPurchaseOrderStatusLink()
		{
			string format = Globals.ApplicationPath + "/Admin/purchaseOrder/ManagePurchaseOrder.aspx?PurchaseStatus={0}";
			this.hlinkAllOrder.NavigateUrl = string.Format(format, 0);
			this.hlinkNotPay.NavigateUrl = string.Format(format, 1);
			this.hlinkYetPay.NavigateUrl = string.Format(format, 2);
			this.hlinkSendGoods.NavigateUrl = string.Format(format, 3);
			this.hlinkTradeFinished.NavigateUrl = string.Format(format, 5);
			this.hlinkClose.NavigateUrl = string.Format(format, 4);
			this.hlinkHistory.NavigateUrl = string.Format(format, 99);
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
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ShopTo"]))
			{
				purchaseOrderQuery.ShipTo = Globals.UrlDecode(this.Page.Request.QueryString["ShopTo"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ProductName"]))
			{
				purchaseOrderQuery.ProductName = Globals.UrlDecode(this.Page.Request.QueryString["ProductName"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["DistributorName"]))
			{
				purchaseOrderQuery.DistributorName = Globals.UrlDecode(this.Page.Request.QueryString["DistributorName"]);
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
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["IsPrinted"]))
			{
				int value = 0;
				if (int.TryParse(this.Page.Request.QueryString["IsPrinted"], out value))
				{
					purchaseOrderQuery.IsPrinted = new int?(value);
				}
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ModeId"]))
			{
				int value2 = 0;
				if (int.TryParse(this.Page.Request.QueryString["ModeId"], out value2))
				{
					purchaseOrderQuery.ShippingModeId = new int?(value2);
				}
			}
			purchaseOrderQuery.PageIndex = this.pager.PageIndex;
			purchaseOrderQuery.PageSize = this.pager.PageSize;
			purchaseOrderQuery.SortOrder = SortAction.Desc;
			purchaseOrderQuery.SortBy = "PurchaseDate";
			return purchaseOrderQuery;
		}
		private void ReBinderPurchaseOrders(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("OrderId", this.txtOrderId.Text.Trim());
			nameValueCollection.Add("PurchaseOrderId", this.txtPurchaseOrderId.Text.Trim());
			nameValueCollection.Add("ShopTo", this.txtShopTo.Text.Trim());
			nameValueCollection.Add("ProductName", this.txtProductName.Text.Trim());
			nameValueCollection.Add("DistributorName", this.txtDistributorName.Text.Trim());
			nameValueCollection.Add("PurchaseStatus", this.lblStatus.Text);
			nameValueCollection.Add("PageSize", this.pager.PageSize.ToString());
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
			if (this.shippingModeDropDownList.SelectedValue.HasValue)
			{
				nameValueCollection.Add("ModeId", this.shippingModeDropDownList.SelectedValue.Value.ToString());
			}
			if (!string.IsNullOrEmpty(this.ddlIsPrinted.SelectedValue))
			{
				nameValueCollection.Add("IsPrinted", this.ddlIsPrinted.SelectedValue);
			}
			base.ReloadPage(nameValueCollection);
		}
		private void BindPurchaseOrders()
		{
			PurchaseOrderQuery purchaseOrderQuery = this.GetPurchaseOrderQuery();
			purchaseOrderQuery.SortBy = "PurchaseDate";
			purchaseOrderQuery.SortOrder = SortAction.Desc;
			DbQueryResult purchaseOrders = SalesHelper.GetPurchaseOrders(purchaseOrderQuery);
			this.dlstPurchaseOrders.DataSource = purchaseOrders.Data;
			this.dlstPurchaseOrders.DataBind();
            this.pager.TotalRecords = purchaseOrders.TotalRecords;
            this.pager1.TotalRecords = purchaseOrders.TotalRecords;
			this.txtOrderId.Text = purchaseOrderQuery.OrderId;
			this.txtProductName.Text = purchaseOrderQuery.ProductName;
			this.txtDistributorName.Text = purchaseOrderQuery.DistributorName;
			this.txtPurchaseOrderId.Text = purchaseOrderQuery.PurchaseOrderId;
			this.txtShopTo.Text = purchaseOrderQuery.ShipTo;
            this.calendarStartDate.SelectedDate = purchaseOrderQuery.StartDate;
            this.calendarEndDate.SelectedDate = purchaseOrderQuery.EndDate;
			this.lblStatus.Text = ((int)purchaseOrderQuery.PurchaseStatus).ToString();
			this.shippingModeDropDownList.SelectedValue = purchaseOrderQuery.ShippingModeId;
			if (purchaseOrderQuery.IsPrinted.HasValue)
			{
				this.ddlIsPrinted.SelectedValue = purchaseOrderQuery.IsPrinted.Value.ToString();
			}
		}
		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReBinderPurchaseOrders(true);
		}
		private void dlstPurchaseOrders_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				System.Web.UI.WebControls.Label label = (System.Web.UI.WebControls.Label)e.Item.FindControl("lkbtnSendGoods");
				object obj = System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Gateway");
				string a = "";
				if (obj != null && !(obj is System.DBNull))
				{
					a = obj.ToString();
				}
				ImageLinkButton imageLinkButton = (ImageLinkButton)e.Item.FindControl("lkbtnConfirmPurchaseOrder");
				System.Web.UI.HtmlControls.HtmlGenericControl htmlGenericControl = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("lkbtnEditPurchaseOrder");
				System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litClosePurchaseOrder");
				ImageLinkButton imageLinkButton2 = (ImageLinkButton)e.Item.FindControl("lkbtnPayOrder");
				OrderStatus orderStatus = (OrderStatus)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "PurchaseStatus");
				System.Web.UI.HtmlControls.HtmlAnchor htmlAnchor = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lkbtnCheckPurchaseRefund");
				System.Web.UI.HtmlControls.HtmlAnchor htmlAnchor2 = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lkbtnCheckPurchaseReturn");
				System.Web.UI.HtmlControls.HtmlAnchor htmlAnchor3 = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lkbtnCheckPurchaseReplace");
				ImageLinkButton imageLinkButton3 = (ImageLinkButton)e.Item.FindControl("lkbtnOrderMatch");
				if (orderStatus == OrderStatus.WaitBuyerPay)
				{
					literal.Visible = true;
					htmlGenericControl.Visible = true;
					if (a != "hishop.plugins.payment.podrequest")
					{
						imageLinkButton2.Visible = true;
					}
				}
				if (orderStatus == OrderStatus.BuyerAlreadyPaid || (orderStatus == OrderStatus.WaitBuyerPay && a == "hishop.plugins.payment.podrequest"))
				{
					imageLinkButton3.Visible = true;
				}
				if (orderStatus == OrderStatus.ApplyForRefund)
				{
					htmlAnchor.Visible = true;
				}
				if (orderStatus == OrderStatus.ApplyForReturns)
				{
					htmlAnchor2.Visible = true;
				}
				if (orderStatus == OrderStatus.ApplyForReplacement)
				{
					htmlAnchor3.Visible = true;
				}
				string purchaseOrderId = this.dlstPurchaseOrders.DataKeys[e.Item.ItemIndex].ToString();
				SalesHelper.GetPurchaseOrder(purchaseOrderId);
				label.Visible = (orderStatus == OrderStatus.BuyerAlreadyPaid || (orderStatus == OrderStatus.WaitBuyerPay && a == "hishop.plugins.payment.podrequest"));
				imageLinkButton.Visible = (orderStatus == OrderStatus.SellerAlreadySent);
				string orderid = (string)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "PurchaseOrderId");
				if (Methods.Supplier_ShipOrderHasAllSendGood(orderid) && orderStatus == OrderStatus.SellerAlreadySent)
				{
					imageLinkButton.Visible = true;
					return;
				}
				imageLinkButton.Visible = false;
			}
		}
		private void dlstPurchaseOrders_ItemCommand(object sender, System.Web.UI.WebControls.DataListCommandEventArgs e)
		{
			PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(e.CommandArgument.ToString());
			if (purchaseOrder != null)
			{
				if (e.CommandName == "FINISH_TRADE" && purchaseOrder.CheckAction(PurchaseOrderActions.MASTER_FINISH_TRADE))
				{
					if (SalesHelper.ConfirmPurchaseOrderFinish(purchaseOrder))
					{
						this.BindPurchaseOrders();
						this.ShowMsg("成功的完成了该采购单", true);
					}
					else
					{
						this.ShowMsg("完成采购单失败", false);
					}
				}
				if (e.CommandName == "CONFIRM_PAY" && purchaseOrder.CheckAction(PurchaseOrderActions.MASTER_CONFIRM_PAY))
				{
					if (SalesHelper.ConfirmPayPurchaseOrder(purchaseOrder))
					{
						PurchaseDebitNote purchaseDebitNote = new PurchaseDebitNote();
						purchaseDebitNote.NoteId = Globals.GetGenerateId();
						purchaseDebitNote.PurchaseOrderId = e.CommandArgument.ToString();
						purchaseDebitNote.Operator = Hidistro.Membership.Context.HiContext.Current.User.Username;
						purchaseDebitNote.Remark = "后台" + purchaseDebitNote.Operator + "收款成功";
						SalesHelper.SavePurchaseDebitNote(purchaseDebitNote);
						this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), "sucess", string.Format("<script language=\"javascript\" >alert('确认收款成功');window.location.href=\"{0}\"</script>", System.Web.HttpContext.Current.Request.RawUrl));
					}
					else
					{
						this.ShowMsg("确认采购单收款失败", false);
					}
				}
				if (e.CommandName == "Match_Order")
				{
					if (purchaseOrder == null)
					{
						return;
					}
					if (!purchaseOrder.CheckAction(PurchaseOrderActions.MASTER_SEND_GOODS))
					{
						this.ShowMsg("当前订单状态没有付款或不是等待发货的订单，所以不能发货", false);
						return;
					}
					if (Methods.Supplier_POrderIsFenPei(purchaseOrder.PurchaseOrderId))
					{
						this.BindPurchaseOrders();
						this.ShowMsg("生成成功", true);
						return;
					}
					string text = Methods.Supplier_POrderItemSupplierUpdate(purchaseOrder);
					if (text != "true")
					{
						this.ShowMsg(text, false);
						return;
					}
					purchaseOrder.RealShippingModeId = 0;
					purchaseOrder.RealModeName = "配送方式(已实际发货单为准)";
					purchaseOrder.ShipOrderNumber = string.Format("{0}", string.Format(" <a style=\"color:red;cursor:pointer;\" target=\"_blank\" onclick=\"{0}\">物流详细</a>", "showWindow_ShipInfoPage('" + purchaseOrder.PurchaseOrderId + "')"));
					if (SalesHelper.SendPurchaseOrderGoods(purchaseOrder))
					{
						Methods.Supplier_POrderItemsSupplierFenPeiOverUpdate(purchaseOrder.PurchaseOrderId);
						this.BindPurchaseOrders();
						this.ShowMsg("生成成功", true);
						return;
					}
					this.ShowMsg("发货失败", false);
					this.BindPurchaseOrders();
					this.ShowMsg("生成成功", true);
				}
			}
		}
		private void lkbtnDeleteCheck_Click(object sender, System.EventArgs e)
		{
			string text = "";
			if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
			{
				text = base.Request["CheckBoxGroup"];
			}
			if (text.Length <= 0)
			{
				this.ShowMsg("请选要删除的采购单", false);
				return;
			}
			text = "'" + text.Replace(",", "','") + "'";
			int num = SalesHelper.DeletePurchaseOrders(text);
			this.BindPurchaseOrders();
			this.ShowMsg(string.Format("成功删除了{0}个采购单", num), true);
		}
		private void btnEditOrder_Click(object sender, System.EventArgs e)
		{
			string value = this.lblPurchaseOrderId.Value;
			PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(value);
			string text = string.Empty;
			decimal adjustedDiscount;
			if (!this.ValidateValues(out adjustedDiscount))
			{
				return;
			}
			purchaseOrder.AdjustedDiscount = adjustedDiscount;
			ValidationResults validationResults = Validation.Validate<PurchaseOrderInfo>(purchaseOrder, new string[]
			{
				"ValPurchaseOrder"
			});
			if (!validationResults.IsValid)
			{
				using (System.Collections.Generic.IEnumerator<ValidationResult> enumerator = ((System.Collections.Generic.IEnumerable<ValidationResult>)validationResults).GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						ValidationResult current = enumerator.Current;
						text += Formatter.FormatErrorMessage(current.Message);
						this.ShowMsg(text, false);
						return;
					}
				}
			}
			if (purchaseOrder.GetPurchaseTotal() < 0m)
			{
				this.ShowMsg("折扣值不能使得采购单总金额为负", false);
			}
			else
			{
				if (SalesHelper.UpdatePurchaseOrderAmount(purchaseOrder))
				{
					this.BindPurchaseOrders();
					this.ShowMsg("修改成功", true);
					return;
				}
				this.ShowMsg("修改失败", false);
				return;
			}
		}
		private bool ValidateValues(out decimal discountAmout)
		{
			string text = string.Empty;
			int num = 0;
			if (this.txtPurchaseOrderDiscount.Text.Trim().IndexOf(".") > 0)
			{
				num = this.txtPurchaseOrderDiscount.Text.Trim().Substring(this.txtPurchaseOrderDiscount.Text.Trim().IndexOf(".") + 1).Length;
			}
			if (!decimal.TryParse(this.txtPurchaseOrderDiscount.Text.Trim(), out discountAmout) || num > 2)
			{
				text += Formatter.FormatErrorMessage("采购单折扣填写错误,采购单折扣只能是数值，且不能超过2位小数");
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowMsg(text, false);
				return false;
			}
			return true;
		}
		private void btnProductGoods_Click(object sender, System.EventArgs e)
		{
			string text = "";
			if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
			{
				text = base.Request["CheckBoxGroup"];
			}
			if (text.Length <= 0)
			{
				this.ShowMsg("请选要下载配货表的采购单", false);
				return;
			}
			System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
			string[] array = text.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				string str = array[i];
				list.Add("'" + str + "'");
			}
			System.Data.DataSet purchaseProductGoods = OrderHelper.GetPurchaseProductGoods(string.Join(",", list.ToArray()));
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.AppendLine("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><body>");
			stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
			stringBuilder.AppendLine("<caption style='text-align:center;'>配货单(仓库拣货表)</caption>");
			stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
			if (purchaseProductGoods.Tables[1].Rows.Count <= 0)
			{
				stringBuilder.AppendLine("<td>商品名称</td>");
			}
			else
			{
				stringBuilder.AppendLine("<td>商品(礼品)名称</td>");
			}
			stringBuilder.AppendLine("<td>货号</td>");
			stringBuilder.AppendLine("<td>规格</td>");
			stringBuilder.AppendLine("<td>拣货数量</td>");
			stringBuilder.AppendLine("<td>现库存数</td>");
			stringBuilder.AppendLine("</tr>");
			foreach (System.Data.DataRow dataRow in purchaseProductGoods.Tables[0].Rows)
			{
				stringBuilder.AppendLine("<tr>");
				stringBuilder.AppendLine("<td>" + dataRow["ProductName"] + "</td>");
				stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["SKU"] + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["SKUContent"] + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["ShipmentQuantity"] + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["Stock"] + "</td>");
				stringBuilder.AppendLine("</tr>");
			}
			foreach (System.Data.DataRow dataRow2 in purchaseProductGoods.Tables[1].Rows)
			{
				stringBuilder.AppendLine("<tr>");
				stringBuilder.AppendLine("<td>" + dataRow2["GiftName"] + "[礼品]</td>");
				stringBuilder.AppendLine("<td></td>");
				stringBuilder.AppendLine("<td></td>");
				stringBuilder.AppendLine("<td>" + dataRow2["Quantity"] + "</td>");
				stringBuilder.AppendLine("<td></td>");
				stringBuilder.AppendLine("</tr>");
			}
			stringBuilder.AppendLine("</table>");
			stringBuilder.AppendLine("</body></html>");
			base.Response.Clear();
			base.Response.Buffer = false;
			base.Response.Charset = "GB2312";
			base.Response.AppendHeader("Content-Disposition", "attachment;filename=productgoods_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
			base.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
			base.Response.ContentType = "application/ms-excel";
			this.EnableViewState = false;
			base.Response.Write(stringBuilder.ToString());
			base.Response.End();
		}
		private void btnOrderGoods_Click(object sender, System.EventArgs e)
		{
			string text = "";
			if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
			{
				text = base.Request["CheckBoxGroup"];
			}
			if (text.Length <= 0)
			{
				this.ShowMsg("请选要下载配货表的采购单", false);
				return;
			}
			System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
			string[] array = text.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				string str = array[i];
				list.Add("'" + str + "'");
			}
			System.Data.DataSet purchaseOrderGoods = OrderHelper.GetPurchaseOrderGoods(string.Join(",", list.ToArray()));
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
			stringBuilder.AppendLine("<caption style='text-align:center;'>配货单(仓库拣货表)</caption>");
			stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
			stringBuilder.AppendLine("<td>采购单编号</td>");
			if (purchaseOrderGoods.Tables[1].Rows.Count <= 0)
			{
				stringBuilder.AppendLine("<td>商品名称</td>");
			}
			else
			{
				stringBuilder.AppendLine("<td>商品(礼品)名称</td>");
			}
			stringBuilder.AppendLine("<td>货号</td>");
			stringBuilder.AppendLine("<td>规格</td>");
			stringBuilder.AppendLine("<td>拣货数量</td>");
			stringBuilder.AppendLine("<td>现库存数</td>");
			stringBuilder.AppendLine("<td>备注</td>");
			stringBuilder.AppendLine("</tr>");
			foreach (System.Data.DataRow dataRow in purchaseOrderGoods.Tables[0].Rows)
			{
				stringBuilder.AppendLine("<tr>");
				stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["PurchaseOrderId"] + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["ProductName"] + "</td>");
				stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["SKU"] + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["SKUContent"] + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["ShipmentQuantity"] + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["Stock"] + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["Remark"] + "</td>");
				stringBuilder.AppendLine("</tr>");
			}
			foreach (System.Data.DataRow dataRow2 in purchaseOrderGoods.Tables[1].Rows)
			{
				stringBuilder.AppendLine("<tr>");
				stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow2["GiftPurchaseOrderId"] + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow2["GiftName"] + "[礼品]</td>");
				stringBuilder.AppendLine("<td></td>");
				stringBuilder.AppendLine("<td></td>");
				stringBuilder.AppendLine("<td>" + dataRow2["Quantity"] + "</td>");
				stringBuilder.AppendLine("<td></td>");
				stringBuilder.AppendLine("<td></td>");
				stringBuilder.AppendLine("</tr>");
			}
			stringBuilder.AppendLine("</table>");
			base.Response.Clear();
			base.Response.Buffer = false;
			base.Response.Charset = "GB2312";
			base.Response.AppendHeader("Content-Disposition", "attachment;filename=purchaseordergoods_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
			base.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
			base.Response.ContentType = "application/ms-excel";
			this.EnableViewState = false;
			base.Response.Write(stringBuilder.ToString());
			base.Response.End();
		}
		private void btnPrintOrder_Click(object sender, System.EventArgs e)
		{
			string text = "";
			if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
			{
				text = base.Request["CheckBoxGroup"];
			}
			if (text.Length <= 0)
			{
				this.ShowMsg("请选要打印的采购单", false);
				return;
			}
			this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("/Sales/ChoosePrintOrders.aspx?OrderIds=" + text));
		}
		private void btnSendGoods_Click(object sender, System.EventArgs e)
		{
			string text = "";
			if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
			{
				text = base.Request["CheckBoxGroup"];
			}
			if (text.Length <= 0)
			{
				this.ShowMsg("请选要发货的订单", false);
				return;
			}
			this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("/purchaseOrder/BatchSendPurchaseOrderGoods.aspx?PurchaseOrderIds=" + text));
		}
		private void btnRemark_Click(object sender, System.EventArgs e)
		{
			if (this.txtRemark.Text.Length > 300)
			{
				this.ShowMsg("备忘录长度限制在300个字符以内", false);
				return;
			}
			System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("^(?!_)(?!.*?_$)(?!-)(?!.*?-$)[a-zA-Z0-9_一-龥-]+$");
			if (!regex.IsMatch(this.txtRemark.Text))
			{
				this.ShowMsg("备忘录只能输入汉字,数字,英文,下划线,减号,不能以下划线、减号开头或结尾", false);
				return;
			}
			PurchaseOrderInfo purchaseOrderInfo = new PurchaseOrderInfo();
			purchaseOrderInfo.PurchaseOrderId = this.hidPurchaseOrderId.Value;
			if (this.orderRemarkImageForRemark.SelectedItem != null)
			{
				purchaseOrderInfo.ManagerMark = this.orderRemarkImageForRemark.SelectedValue;
			}
			purchaseOrderInfo.ManagerRemark = Globals.HtmlEncode(this.txtRemark.Text);
			if (SalesHelper.SavePurchaseOrderRemark(purchaseOrderInfo))
			{
				this.BindPurchaseOrders();
				this.ShowMsg("保存备忘录成功", true);
				return;
			}
			this.ShowMsg("保存失败", false);
		}
		private void btnClosePurchaseOrder_Click(object sender, System.EventArgs e)
		{
			PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(this.hidPurchaseOrderId.Value);
			purchaseOrder.CloseReason = this.ddlCloseReason.SelectedValue;
			if (SalesHelper.ClosePurchaseOrder(purchaseOrder))
			{
				this.BindPurchaseOrders();
				this.ShowMsg("关闭采购单成功", true);
				return;
			}
			this.ShowMsg("关闭采购单失败", false);
		}
	}
}
