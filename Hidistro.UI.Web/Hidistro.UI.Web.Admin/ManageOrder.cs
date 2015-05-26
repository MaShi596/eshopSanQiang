using ASPNET.WebControls;
using Hidistro.AccountCenter.Business;
using Hidistro.ControlPanel.Promotions;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Messages;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Plugins;
using Hishop.Web.CustomMade;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Orders)]
	public class ManageOrder : AdminPage
	{
		protected System.Web.UI.WebControls.HyperLink hlinkAllOrder;
		protected System.Web.UI.WebControls.HyperLink hlinkNotPay;
		protected System.Web.UI.WebControls.HyperLink hlinkYetPay;
		protected System.Web.UI.WebControls.HyperLink hlinkSendGoods;
		protected System.Web.UI.WebControls.HyperLink hlinkTradeFinished;
		protected System.Web.UI.WebControls.HyperLink hlinkClose;
		protected System.Web.UI.WebControls.HyperLink hlinkHistory;
		protected WebCalendar calendarStartDate;
		protected WebCalendar calendarEndDate;
		protected System.Web.UI.WebControls.TextBox txtUserName;
		protected System.Web.UI.WebControls.TextBox txtOrderId;
		protected System.Web.UI.WebControls.Label lblStatus;
		protected System.Web.UI.WebControls.TextBox txtProductName;
		protected System.Web.UI.WebControls.TextBox txtShopTo;
		protected System.Web.UI.WebControls.DropDownList ddlIsPrinted;
		protected ShippingModeDropDownList shippingModeDropDownList;
		protected RegionSelector dropRegion;
		protected System.Web.UI.WebControls.Button btnSearchButton;
		protected PageSize hrefPageSize;
		protected Pager pager1;
		protected ImageLinkButton lkbtnDeleteCheck;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidOrderId;
		protected System.Web.UI.WebControls.DataList dlstOrders;
		protected Pager pager;
		protected CloseTranReasonDropDownList ddlCloseReason;
		protected FormatedMoneyLabel lblOrderTotalForRemark;
		protected OrderRemarkImageRadioButtonList orderRemarkImageForRemark;
		protected System.Web.UI.WebControls.TextBox txtRemark;
		protected System.Web.UI.WebControls.Label lblOrderId;
		protected System.Web.UI.WebControls.Label lblOrderTotal;
		protected System.Web.UI.WebControls.Label lblRefundType;
		protected System.Web.UI.WebControls.Label lblRefundRemark;
		protected System.Web.UI.WebControls.Label lblContacts;
		protected System.Web.UI.WebControls.Label lblEmail;
		protected System.Web.UI.WebControls.Label lblTelephone;
		protected System.Web.UI.WebControls.Label lblAddress;
		protected System.Web.UI.WebControls.TextBox txtAdminRemark;
		protected System.Web.UI.WebControls.Label return_lblOrderId;
		protected System.Web.UI.WebControls.Label return_lblOrderTotal;
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
		protected System.Web.UI.WebControls.Button btnRemark;
		protected System.Web.UI.WebControls.Button btnOrderGoods;
		protected System.Web.UI.WebControls.Button btnProductGoods;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!string.IsNullOrEmpty(base.Request["isCallback"]) && base.Request["isCallback"] == "true")
			{
				if (string.IsNullOrEmpty(base.Request["orderId"]))
				{
					base.Response.Write("{\"Status\":\"0\"}");
					base.Response.End();
					return;
				}
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(base.Request["orderId"]);
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
				int num;
				string text;
				if (base.Request["type"] == "refund")
				{
					OrderHelper.GetRefundType(base.Request["orderId"], out num, out text);
				}
				else
				{
					if (base.Request["type"] == "return")
					{
						OrderHelper.GetRefundTypeFromReturn(base.Request["orderId"], out num, out text);
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
				stringBuilder.AppendFormat(",\"OrderTotal\":\"{0}\"", Globals.FormatMoney(orderInfo.GetTotal()));
				if (base.Request["type"] == "replace")
				{
					string replaceComments = OrderHelper.GetReplaceComments(base.Request["orderId"]);
					stringBuilder.AppendFormat(",\"Comments\":\"{0}\"", replaceComments.Replace("\r\n", ""));
				}
				else
				{
					stringBuilder.AppendFormat(",\"RefundType\":\"{0}\"", num);
					stringBuilder.AppendFormat(",\"RefundTypeStr\":\"{0}\"", arg);
				}
				stringBuilder.AppendFormat(",\"Contacts\":\"{0}\"", orderInfo.RealName);
				stringBuilder.AppendFormat(",\"Email\":\"{0}\"", orderInfo.EmailAddress);
				stringBuilder.AppendFormat(",\"Telephone\":\"{0}\"", orderInfo.TelPhone);
				stringBuilder.AppendFormat(",\"Address\":\"{0}\"", orderInfo.Address);
				stringBuilder.AppendFormat(",\"Remark\":\"{0}\"", text.Replace("\r\n", ""));
				stringBuilder.AppendFormat(",\"PostCode\":\"{0}\"", orderInfo.ZipCode);
				base.Response.Clear();
				base.Response.ContentType = "application/json";
				base.Response.Write("{\"Status\":\"1\"" + stringBuilder.ToString() + "}");
				base.Response.End();
			}
			this.dlstOrders.ItemDataBound += new System.Web.UI.WebControls.DataListItemEventHandler(this.dlstOrders_ItemDataBound);
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
			this.dlstOrders.ItemCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.dlstOrders_ItemCommand);
			this.btnRemark.Click += new System.EventHandler(this.btnRemark_Click);
			this.btnCloseOrder.Click += new System.EventHandler(this.btnCloseOrder_Click);
			this.lkbtnDeleteCheck.Click += new System.EventHandler(this.lkbtnDeleteCheck_Click);
			this.btnOrderGoods.Click += new System.EventHandler(this.btnOrderGoods_Click);
			this.btnProductGoods.Click += new System.EventHandler(this.btnProductGoods_Click);
			this.btnAcceptRefund.Click += new System.EventHandler(this.btnAcceptRefund_Click);
			this.btnRefuseRefund.Click += new System.EventHandler(this.btnRefuseRefund_Click);
			this.btnAcceptReturn.Click += new System.EventHandler(this.btnAcceptReturn_Click);
			this.btnRefuseReturn.Click += new System.EventHandler(this.btnRefuseReturn_Click);
			this.btnAcceptReplace.Click += new System.EventHandler(this.btnAcceptReplace_Click);
			this.btnRefuseReplace.Click += new System.EventHandler(this.btnRefuseReplace_Click);
			if (!this.Page.IsPostBack)
			{
				this.shippingModeDropDownList.DataBind();
				this.ddlIsPrinted.Items.Clear();
				this.ddlIsPrinted.Items.Add(new System.Web.UI.WebControls.ListItem("全部", string.Empty));
				this.ddlIsPrinted.Items.Add(new System.Web.UI.WebControls.ListItem("已打印", "1"));
				this.ddlIsPrinted.Items.Add(new System.Web.UI.WebControls.ListItem("未打印", "0"));
				this.SetOrderStatusLink();
				this.BindOrders();
			}
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
		}
		private void btnRefuseReplace_Click(object sender, System.EventArgs e)
		{
			OrderHelper.CheckReplace(this.hidOrderId.Value, this.hidAdminRemark.Value, false);
			this.BindOrders();
			this.ShowMsg("成功的拒绝了订单换货", true);
		}
		private void btnAcceptReplace_Click(object sender, System.EventArgs e)
		{
			OrderHelper.CheckReplace(this.hidOrderId.Value, this.hidAdminRemark.Value, true);
			this.BindOrders();
			this.ShowMsg("成功的确认了订单换货", true);
		}
		private void btnRefuseReturn_Click(object sender, System.EventArgs e)
		{
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.hidOrderId.Value);
			OrderHelper.CheckReturn(orderInfo, Hidistro.Membership.Context.HiContext.Current.User.Username, 0m, this.hidAdminRemark.Value, int.Parse(this.hidRefundType.Value), false);
			this.BindOrders();
			this.ShowMsg("成功的拒绝了订单退货", true);
		}
		private void btnAcceptReturn_Click(object sender, System.EventArgs e)
		{
			decimal num;
			if (!decimal.TryParse(this.hidRefundMoney.Value, out num))
			{
				this.ShowMsg("退款金额需为数字格式！", false);
				return;
			}
			decimal d;
			decimal.TryParse(this.hidOrderTotal.Value, out d);
			if (num > d)
			{
				this.ShowMsg("退款金额不能大于订单金额！", false);
				return;
			}
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.hidOrderId.Value);
			OrderHelper.CheckReturn(orderInfo, Hidistro.Membership.Context.HiContext.Current.User.Username, num, this.hidAdminRemark.Value, int.Parse(this.hidRefundType.Value), true);
			this.BindOrders();
			this.ShowMsg("成功的确认了订单退货", true);
		}
		protected void btnAcceptRefund_Click(object sender, System.EventArgs e)
		{
			string username = Hidistro.Membership.Context.HiContext.Current.User.Username;
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.hidOrderId.Value);
			if (OrderHelper.CheckRefund(orderInfo, username, this.hidAdminRemark.Value, int.Parse(this.hidRefundType.Value), true))
			{
				this.BindOrders();
				decimal amount = orderInfo.GetTotal();
				if (orderInfo.GroupBuyId > 0 && orderInfo.GroupBuyStatus != GroupBuyStatus.Failed)
				{
					amount = orderInfo.GetTotal() - orderInfo.NeedPrice;
				}
				Hidistro.Membership.Context.Member user = Hidistro.Membership.Context.Users.GetUser(orderInfo.UserId) as Hidistro.Membership.Context.Member;
				Messenger.OrderRefund(user, orderInfo.OrderId, amount);
				this.ShowMsg("成功的确认了订单退款", true);
			}
		}
		private void btnRefuseRefund_Click(object sender, System.EventArgs e)
		{
			string username = Hidistro.Membership.Context.HiContext.Current.User.Username;
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.hidOrderId.Value);
			OrderHelper.CheckRefund(orderInfo, username, this.hidAdminRemark.Value, int.Parse(this.hidRefundType.Value), false);
			this.BindOrders();
			this.ShowMsg("成功的拒绝了订单退款", true);
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
				this.ShowMsg("请选要下载配货表的订单", false);
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
			System.Data.DataSet productGoods = OrderHelper.GetProductGoods(string.Join(",", list.ToArray()));
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.AppendLine("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><body>");
			stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
			stringBuilder.AppendLine("<caption style='text-align:center;'>配货单(仓库拣货表)</caption>");
			stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
			if (productGoods.Tables[1].Rows.Count <= 0)
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
			foreach (System.Data.DataRow dataRow in productGoods.Tables[0].Rows)
			{
				stringBuilder.AppendLine("<tr>");
				stringBuilder.AppendLine("<td>" + dataRow["ProductName"] + "</td>");
				stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["SKU"] + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["SKUContent"] + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["ShipmentQuantity"] + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["Stock"] + "</td>");
				stringBuilder.AppendLine("</tr>");
			}
			foreach (System.Data.DataRow dataRow2 in productGoods.Tables[1].Rows)
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
				this.ShowMsg("请选要下载配货表的订单", false);
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
			System.Data.DataSet orderGoods = OrderHelper.GetOrderGoods(string.Join(",", list.ToArray()));
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.AppendLine("<html><head><meta http-equiv=Content-Type content=\"text/html; charset=gb2312\"></head><body>");
			stringBuilder.AppendLine("<table cellspacing=\"0\" cellpadding=\"5\" rules=\"all\" border=\"1\">");
			stringBuilder.AppendLine("<caption style='text-align:center;'>配货单(仓库拣货表)</caption>");
			stringBuilder.AppendLine("<tr style=\"font-weight: bold; white-space: nowrap;\">");
			stringBuilder.AppendLine("<td>订单单号</td>");
			if (orderGoods.Tables[1].Rows.Count <= 0)
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
			foreach (System.Data.DataRow dataRow in orderGoods.Tables[0].Rows)
			{
				stringBuilder.AppendLine("<tr>");
				stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["OrderId"] + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["ProductName"] + "</td>");
				stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow["SKU"] + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["SKUContent"] + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["ShipmentQuantity"] + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["Stock"] + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow["Remark"] + "</td>");
				stringBuilder.AppendLine("</tr>");
			}
			foreach (System.Data.DataRow dataRow2 in orderGoods.Tables[1].Rows)
			{
				stringBuilder.AppendLine("<tr>");
				stringBuilder.AppendLine("<td style=\"vnd.ms-excel.numberformat:@\">" + dataRow2["GiftOrderId"] + "</td>");
				stringBuilder.AppendLine("<td>" + dataRow2["GiftName"] + "[礼品]</td>");
				stringBuilder.AppendLine("<td></td>");
				stringBuilder.AppendLine("<td></td>");
				stringBuilder.AppendLine("<td>" + dataRow2["Quantity"] + "</td>");
				stringBuilder.AppendLine("<td></td>");
				stringBuilder.AppendLine("<td></td>");
				stringBuilder.AppendLine("</tr>");
			}
			stringBuilder.AppendLine("</table>");
			stringBuilder.AppendLine("</body></html>");
			base.Response.Clear();
			base.Response.Buffer = false;
			base.Response.Charset = "GB2312";
			base.Response.AppendHeader("Content-Disposition", "attachment;filename=ordergoods_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls");
			base.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
			base.Response.ContentType = "application/ms-excel";
			this.EnableViewState = false;
			base.Response.Write(stringBuilder.ToString());
			base.Response.End();
		}
		protected void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReloadOrders(true);
		}
		private void dlstOrders_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				OrderStatus orderStatus = (OrderStatus)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "OrderStatus");
				string a = "";
				if (!(System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Gateway") is System.DBNull))
				{
					a = (string)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "Gateway");
				}
				int num = (int)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "GroupBuyId");
				System.Web.UI.WebControls.HyperLink hyperLink = (System.Web.UI.WebControls.HyperLink)e.Item.FindControl("lkbtnEditPrice");
				System.Web.UI.WebControls.Label label = (System.Web.UI.WebControls.Label)e.Item.FindControl("lkbtnSendGoods");
				ImageLinkButton imageLinkButton = (ImageLinkButton)e.Item.FindControl("lkbtnPayOrder");
				ImageLinkButton imageLinkButton2 = (ImageLinkButton)e.Item.FindControl("lkbtnConfirmOrder");
				System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litCloseOrder");
				System.Web.UI.HtmlControls.HtmlAnchor htmlAnchor = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lkbtnCheckRefund");
				System.Web.UI.HtmlControls.HtmlAnchor htmlAnchor2 = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lkbtnCheckReturn");
				System.Web.UI.HtmlControls.HtmlAnchor htmlAnchor3 = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lkbtnCheckReplace");
				ImageLinkButton imageLinkButton3 = (ImageLinkButton)e.Item.FindControl("lkbtnOrderMatch");
				if (orderStatus == OrderStatus.BuyerAlreadyPaid || (orderStatus == OrderStatus.WaitBuyerPay && a == "hishop.plugins.payment.podrequest"))
				{
					imageLinkButton3.Visible = true;
				}
				if (orderStatus == OrderStatus.WaitBuyerPay)
				{
					hyperLink.Visible = true;
					literal.Visible = true;
					if (a != "hishop.plugins.payment.podrequest")
					{
						imageLinkButton.Visible = true;
					}
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
				if (num > 0)
				{
					GroupBuyStatus groupBuyStatus = (GroupBuyStatus)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "GroupBuyStatus");
					label.Visible = ((orderStatus == OrderStatus.BuyerAlreadyPaid || (orderStatus == OrderStatus.WaitBuyerPay && a == "hishop.plugins.payment.podrequest")) && groupBuyStatus == GroupBuyStatus.Success);
				}
				else
				{
					label.Visible = (orderStatus == OrderStatus.BuyerAlreadyPaid || (orderStatus == OrderStatus.WaitBuyerPay && a == "hishop.plugins.payment.podrequest"));
				}
				imageLinkButton2.Visible = (orderStatus == OrderStatus.SellerAlreadySent);
				string orderid = (string)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "OrderId");
				if (Methods.Supplier_ShipOrderHasAllSendGood(orderid) && orderStatus == OrderStatus.SellerAlreadySent)
				{
					imageLinkButton2.Visible = true;
					return;
				}
				imageLinkButton2.Visible = false;
			}
		}
		protected void dlstOrders_ItemCommand(object sender, System.Web.UI.WebControls.DataListCommandEventArgs e)
		{
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(e.CommandArgument.ToString());
			if (orderInfo != null)
			{
				if (e.CommandName == "CONFIRM_PAY" && orderInfo.CheckAction(OrderActions.SELLER_CONFIRM_PAY))
				{
					int num = 0;
					int num2 = 0;
					int num3 = 0;
					if (orderInfo.CountDownBuyId > 0)
					{
						CountDownInfo countDownBuy = TradeHelper.GetCountDownBuy(orderInfo.CountDownBuyId);
						if (countDownBuy == null || countDownBuy.EndDate < System.DateTime.Now)
						{
							this.ShowMsg("当前的订单为限时抢购订单，此活动已结束，所以不能支付", false);
							return;
						}
					}
					if (orderInfo.GroupBuyId > 0)
					{
						GroupBuyInfo groupBuy = PromoteHelper.GetGroupBuy(orderInfo.GroupBuyId);
						if (groupBuy != null)
						{
							if (groupBuy.Status == GroupBuyStatus.UnderWay)
							{
								num2 = PromoteHelper.GetOrderCount(orderInfo.GroupBuyId);
								num = groupBuy.MaxCount;
								num3 = orderInfo.GetGroupBuyOerderNumber();
								if (num < num2 + num3)
								{
									this.ShowMsg("当前的订单为团购订单，订购数量已超过订购总数，所以不能支付", false);
									return;
								}
								goto IL_E2;
							}
						}
						this.ShowMsg("当前的订单为团购订单，此团购活动已结束，所以不能支付", false);
						return;
					}
					IL_E2:
					if (OrderHelper.ConfirmPay(orderInfo))
					{
						DebitNote debitNote = new DebitNote();
						debitNote.NoteId = Globals.GetGenerateId();
						debitNote.OrderId = e.CommandArgument.ToString();
						debitNote.Operator = Hidistro.Membership.Context.HiContext.Current.User.Username;
						debitNote.Remark = "后台" + debitNote.Operator + "收款成功";
						OrderHelper.SaveDebitNote(debitNote);
						if (orderInfo.GroupBuyId > 0 && num == num2 + num3)
						{
							PromoteHelper.SetGroupBuyEndUntreated(orderInfo.GroupBuyId);
						}
						this.BindOrders();
						int num4 = orderInfo.UserId;
						if (num4 == 1100)
						{
							num4 = 0;
						}
						Hidistro.Membership.Core.IUser user = Hidistro.Membership.Context.Users.GetUser(num4);
						Messenger.OrderPayment(user, orderInfo.OrderId, orderInfo.GetTotal());
						orderInfo.OnPayment();
						this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), "sucess", string.Format("<script language=\"javascript\" >alert('确认收款成功');window.location.href=\"{0}\"</script>", System.Web.HttpContext.Current.Request.RawUrl));
						return;
					}
					this.ShowMsg("确认订单收款失败", false);
					return;
				}
				else
				{
					if (e.CommandName == "FINISH_TRADE" && orderInfo.CheckAction(OrderActions.SELLER_FINISH_TRADE))
					{
						if (OrderHelper.ConfirmOrderFinish(orderInfo))
						{
							this.BindOrders();
							this.ShowMsg("成功的完成了该订单", true);
							return;
						}
						this.ShowMsg("完成订单失败", false);
						return;
					}
					else
					{
						if (e.CommandName == "Match_Order")
						{
							if (orderInfo == null)
							{
								return;
							}
							if (orderInfo.GroupBuyId > 0 && orderInfo.GroupBuyStatus != GroupBuyStatus.Success)
							{
								this.ShowMsg("当前订单为团购订单，团购活动还未成功结束，所以不能发货", false);
								return;
							}
							if (!orderInfo.CheckAction(OrderActions.SELLER_SEND_GOODS))
							{
								this.ShowMsg("当前订单状态没有付款或不是等待发货的订单，所以不能发货", false);
								return;
							}
							if (Methods.Supplier_OrderIsFenPei(e.CommandArgument.ToString()))
							{
								this.ShowMsg("生成成功", true);
								return;
							}
							string text = Methods.Supplier_OrderItemSupplierUpdate(orderInfo);
							if (text != "true")
							{
								this.ShowMsg(text, false);
								return;
							}
							orderInfo.RealShippingModeId = 0;
							orderInfo.RealModeName = "配送方式(已实际发货单为准)";
							orderInfo.ShipOrderNumber = string.Format("{0}", string.Format(" <a style=\"color:red;cursor:pointer;\" target=\"_blank\" onclick=\"{0}\">物流详细</a>", "showWindow_ShipInfoPage('" + orderInfo.OrderId + "')"));
							if (OrderHelper.SendGoods(orderInfo))
							{
								Methods.Supplier_OrderItemsSupplierFenPeiOverUpdate(orderInfo.OrderId);
								if (!string.IsNullOrEmpty(orderInfo.GatewayOrderId) && orderInfo.GatewayOrderId.Trim().Length > 0)
								{
									PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode(orderInfo.PaymentTypeId);
									if (paymentMode != null)
									{
										PaymentRequest paymentRequest = PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), orderInfo.OrderId, orderInfo.GetTotal(), "订单发货", "订单号-" + orderInfo.OrderId, orderInfo.EmailAddress, orderInfo.OrderDate, Globals.FullPath(Globals.GetSiteUrls().Home), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentReturn_url", new object[]
										{
											paymentMode.Gateway
										})), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentNotify_url", new object[]
										{
											paymentMode.Gateway
										})), "");
										paymentRequest.SendGoods(orderInfo.GatewayOrderId, orderInfo.RealModeName, orderInfo.ShipOrderNumber, "EXPRESS");
									}
								}
								int num5 = orderInfo.UserId;
								if (num5 == 1100)
								{
									num5 = 0;
								}
								Hidistro.Membership.Core.IUser user2 = Hidistro.Membership.Context.Users.GetUser(num5);
								Messenger.OrderShipping(orderInfo, user2);
								orderInfo.OnDeliver();
								this.ShowMsg("生成成功", true);
								this.BindOrders();
								return;
							}
							this.ShowMsg("发货失败", false);
							this.ShowMsg("生成成功", true);
						}
					}
				}
			}
		}
		protected void lkbtnDeleteCheck_Click(object sender, System.EventArgs e)
		{
			string text = "";
			if (!string.IsNullOrEmpty(base.Request["CheckBoxGroup"]))
			{
				text = base.Request["CheckBoxGroup"];
			}
			if (text.Length <= 0)
			{
				this.ShowMsg("请选要删除的订单", false);
				return;
			}
			text = "'" + text.Replace(",", "','") + "'";
			int num = OrderHelper.DeleteOrders(text);
			this.BindOrders();
			this.ShowMsg(string.Format("成功删除了{0}个订单", num), true);
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
			this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("/Sales/BatchSendOrderGoods.aspx?OrderIds=" + text));
		}
		private void BindOrders()
		{
			OrderQuery orderQuery = this.GetOrderQuery();
			DbQueryResult dbQueryResult = Methods.Supplier_OrderSGetForAdmin(orderQuery);
			this.dlstOrders.DataSource = dbQueryResult.Data;
			this.dlstOrders.DataBind();
			this.pager.TotalRecords=dbQueryResult.TotalRecords;
			this.pager1.TotalRecords=dbQueryResult.TotalRecords;
			this.txtUserName.Text = orderQuery.UserName;
			this.txtOrderId.Text = orderQuery.OrderId;
			this.txtProductName.Text = orderQuery.ProductName;
			this.txtShopTo.Text = orderQuery.ShipTo;
            this.calendarStartDate.SelectedDate = orderQuery.StartDate;
            this.calendarEndDate.SelectedDate = orderQuery.EndDate;
			this.lblStatus.Text = ((int)orderQuery.Status).ToString();
			this.shippingModeDropDownList.SelectedValue = orderQuery.ShippingModeId;
			if (orderQuery.IsPrinted.HasValue)
			{
				this.ddlIsPrinted.SelectedValue = orderQuery.IsPrinted.Value.ToString();
			}
			if (orderQuery.RegionId.HasValue)
			{
				this.dropRegion.SetSelectedRegionId(orderQuery.RegionId);
			}
		}
		private OrderQuery GetOrderQuery()
		{
			OrderQuery orderQuery = new OrderQuery();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
			{
				orderQuery.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ProductName"]))
			{
				orderQuery.ProductName = Globals.UrlDecode(this.Page.Request.QueryString["ProductName"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ShipTo"]))
			{
				orderQuery.ShipTo = Globals.UrlDecode(this.Page.Request.QueryString["ShipTo"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["UserName"]))
			{
				orderQuery.UserName = Globals.UrlDecode(this.Page.Request.QueryString["UserName"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartDate"]))
			{
				orderQuery.StartDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["StartDate"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["GroupBuyId"]))
			{
				orderQuery.GroupBuyId = new int?(int.Parse(this.Page.Request.QueryString["GroupBuyId"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["EndDate"]))
			{
				orderQuery.EndDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["EndDate"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderStatus"]))
			{
				int status = 0;
				if (int.TryParse(this.Page.Request.QueryString["OrderStatus"], out status))
				{
					orderQuery.Status = (OrderStatus)status;
				}
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["IsPrinted"]))
			{
				int value = 0;
				if (int.TryParse(this.Page.Request.QueryString["IsPrinted"], out value))
				{
					orderQuery.IsPrinted = new int?(value);
				}
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ModeId"]))
			{
				int value2 = 0;
				if (int.TryParse(this.Page.Request.QueryString["ModeId"], out value2))
				{
					orderQuery.ShippingModeId = new int?(value2);
				}
			}
			int value3;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["region"]) && int.TryParse(this.Page.Request.QueryString["region"], out value3))
			{
				orderQuery.RegionId = new int?(value3);
			}
			orderQuery.PageIndex = this.pager.PageIndex;
			orderQuery.PageSize = this.pager.PageSize;
			orderQuery.SortBy = "OrderDate";
			orderQuery.SortOrder = SortAction.Desc;
			return orderQuery;
		}
		private void ReloadOrders(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("UserName", this.txtUserName.Text);
			nameValueCollection.Add("OrderId", this.txtOrderId.Text);
			nameValueCollection.Add("ProductName", this.txtProductName.Text);
			nameValueCollection.Add("ShipTo", this.txtShopTo.Text);
			nameValueCollection.Add("PageSize", this.pager.PageSize.ToString());
			nameValueCollection.Add("OrderStatus", this.lblStatus.Text);
			if (this.calendarStartDate.SelectedDate.HasValue)
			{
				nameValueCollection.Add("StartDate", this.calendarStartDate.SelectedDate.Value.ToString());
			}
			if (this.calendarEndDate.SelectedDate.HasValue)
			{
				nameValueCollection.Add("EndDate", this.calendarEndDate.SelectedDate.Value.ToString());
			}
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["GroupBuyId"]))
			{
				nameValueCollection.Add("GroupBuyId", this.Page.Request.QueryString["GroupBuyId"]);
			}
			if (this.shippingModeDropDownList.SelectedValue.HasValue)
			{
				nameValueCollection.Add("ModeId", this.shippingModeDropDownList.SelectedValue.Value.ToString());
			}
			if (!string.IsNullOrEmpty(this.ddlIsPrinted.SelectedValue))
			{
				nameValueCollection.Add("IsPrinted", this.ddlIsPrinted.SelectedValue);
			}
			if (this.dropRegion.GetSelectedRegionId().HasValue)
			{
				nameValueCollection.Add("region", this.dropRegion.GetSelectedRegionId().Value.ToString());
			}
			base.ReloadPage(nameValueCollection);
		}
		private void SetOrderStatusLink()
		{
			string format = Globals.ApplicationPath + "/Admin/sales/ManageOrder.aspx?orderStatus={0}";
			this.hlinkAllOrder.NavigateUrl = string.Format(format, 0);
			this.hlinkNotPay.NavigateUrl = string.Format(format, 1);
			this.hlinkYetPay.NavigateUrl = string.Format(format, 2);
			this.hlinkSendGoods.NavigateUrl = string.Format(format, 3);
			this.hlinkClose.NavigateUrl = string.Format(format, 4);
			this.hlinkTradeFinished.NavigateUrl = string.Format(format, 5);
			this.hlinkHistory.NavigateUrl = string.Format(format, 99);
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
			OrderInfo orderInfo = new OrderInfo();
			orderInfo.OrderId = this.hidOrderId.Value;
			if (this.orderRemarkImageForRemark.SelectedItem != null)
			{
				orderInfo.ManagerMark = this.orderRemarkImageForRemark.SelectedValue;
			}
			orderInfo.ManagerRemark = Globals.HtmlEncode(this.txtRemark.Text);
			if (OrderHelper.SaveRemark(orderInfo))
			{
				this.BindOrders();
				this.ShowMsg("保存备忘录成功", true);
				return;
			}
			this.ShowMsg("保存失败", false);
		}
		private void btnCloseOrder_Click(object sender, System.EventArgs e)
		{
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.hidOrderId.Value);
			orderInfo.CloseReason = this.ddlCloseReason.SelectedValue;
			if (OrderHelper.CloseTransaction(orderInfo))
			{
				int num = orderInfo.UserId;
				if (num == 1100)
				{
					num = 0;
				}
				Hidistro.Membership.Core.IUser user = Hidistro.Membership.Context.Users.GetUser(num);
				Messenger.OrderClosed(user, orderInfo.OrderId, orderInfo.CloseReason);
				orderInfo.OnClosed();
				this.BindOrders();
				this.ShowMsg("关闭订单成功", true);
				return;
			}
			this.ShowMsg("关闭订单失败", false);
		}
	}
}
