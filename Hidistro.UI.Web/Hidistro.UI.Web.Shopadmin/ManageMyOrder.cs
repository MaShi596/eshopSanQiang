using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Messages;
using Hidistro.Subsites.Promotions;
using Hidistro.Subsites.Sales;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Specialized;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class ManageMyOrder : DistributorPage
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
		protected System.Web.UI.WebControls.Button btnSearchButton;
		protected PageSize hrefPageSize;
		protected Pager pager1;
		protected ImageLinkButton lkbtnDeleteCheck;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidOrderId;
		protected System.Web.UI.WebControls.DataList dlstOrders;
		protected Pager pager;
		protected System.Web.UI.HtmlControls.HtmlGenericControl spanOrderId;
		protected System.Web.UI.HtmlControls.HtmlGenericControl lblOrderDateForRemark;
		protected FormatedMoneyLabel lblOrderTotalForRemark;
		protected OrderRemarkImageRadioButtonList orderRemarkImageForRemark;
		protected System.Web.UI.WebControls.TextBox txtRemark;
		protected System.Web.UI.WebControls.Button btnRemark;
		protected CloseTranReasonDropDownList ddlCloseReason;
		protected System.Web.UI.WebControls.Button btnCloseOrder;
		protected System.Web.UI.WebControls.Label lblOrderId;
		protected System.Web.UI.WebControls.Label lblOrderTotal;
		protected System.Web.UI.WebControls.Label lblContacts;
		protected System.Web.UI.WebControls.Label lblEmail;
		protected System.Web.UI.WebControls.Label lblTelephone;
		protected System.Web.UI.WebControls.Label lblAddress;
		protected System.Web.UI.WebControls.Label lblRefundType;
		protected System.Web.UI.WebControls.Label lblRefundRemark;
		protected System.Web.UI.HtmlControls.HtmlTextArea txtAdminRemark;
		protected System.Web.UI.WebControls.Button btnAcceptRefund;
		protected System.Web.UI.WebControls.Button btnRefuseRefund;
		protected System.Web.UI.WebControls.Label return_lblOrderId;
		protected System.Web.UI.WebControls.Label return_lblOrderTotal;
		protected System.Web.UI.WebControls.Label return_lblContacts;
		protected System.Web.UI.WebControls.Label return_lblEmail;
		protected System.Web.UI.WebControls.Label return_lblTelephone;
		protected System.Web.UI.WebControls.Label return_lblAddress;
		protected System.Web.UI.WebControls.Label return_lblRefundType;
		protected System.Web.UI.WebControls.Label return_lblReturnRemark;
		protected System.Web.UI.WebControls.TextBox return_txtRefundMoney;
		protected System.Web.UI.HtmlControls.HtmlTextArea return_txtAdminRemark;
		protected System.Web.UI.WebControls.Button btnAcceptReturn;
		protected System.Web.UI.WebControls.Button btnRefuseReturn;
		protected System.Web.UI.WebControls.Label replace_lblOrderId;
		protected System.Web.UI.WebControls.Label replace_lblOrderTotal;
		protected System.Web.UI.WebControls.Label replace_lblContacts;
		protected System.Web.UI.WebControls.Label replace_lblEmail;
		protected System.Web.UI.WebControls.Label replace_lblTelephone;
		protected System.Web.UI.WebControls.Label replace_lblAddress;
		protected System.Web.UI.WebControls.Label replace_lblPostCode;
		protected System.Web.UI.WebControls.Label replace_lblComments;
		protected System.Web.UI.HtmlControls.HtmlTextArea replace_txtAdminRemark;
		protected System.Web.UI.WebControls.Button btnAcceptReplace;
		protected System.Web.UI.WebControls.Button btnRefuseReplace;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidRefundType;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidOrderTotal;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.dlstOrders.ItemDataBound += new System.Web.UI.WebControls.DataListItemEventHandler(this.dlstOrders_ItemDataBound);
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
			this.dlstOrders.ItemCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.dlstOrders_ItemCommand);
			this.btnRemark.Click += new System.EventHandler(this.btnRemark_Click);
			this.btnCloseOrder.Click += new System.EventHandler(this.btnCloseOrder_Click);
			this.lkbtnDeleteCheck.Click += new System.EventHandler(this.lkbtnDeleteCheck_Click);
			this.btnAcceptRefund.Click += new System.EventHandler(this.btnAcceptRefund_Click);
			this.btnRefuseRefund.Click += new System.EventHandler(this.btnRefuseRefund_Click);
			this.btnAcceptReturn.Click += new System.EventHandler(this.btnAcceptReturn_Click);
			this.btnRefuseReturn.Click += new System.EventHandler(this.btnRefuseReturn_Click);
			this.btnAcceptReplace.Click += new System.EventHandler(this.btnAcceptReplace_Click);
			this.btnRefuseReplace.Click += new System.EventHandler(this.btnRefuseReplace_Click);
			if (!string.IsNullOrEmpty(base.Request["isCallback"]) && base.Request["isCallback"] == "true")
			{
				if (string.IsNullOrEmpty(base.Request["orderId"]))
				{
					base.Response.Write("{\"Status\":\"0\"}");
					base.Response.End();
					return;
				}
				OrderInfo orderInfo = SubsiteSalesHelper.GetOrderInfo(base.Request["orderId"]);
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
				int num;
				string text;
				if (base.Request["type"] == "refund")
				{
					SubsiteSalesHelper.GetRefundType(base.Request["orderId"], out num, out text);
				}
				else
				{
					if (base.Request["type"] == "return")
					{
						SubsiteSalesHelper.GetRefundTypeFromReturn(base.Request["orderId"], out num, out text);
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
					string replaceComments = SubsiteSalesHelper.GetReplaceComments(base.Request["orderId"]);
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
			if (!this.Page.IsPostBack)
			{
				this.SetOrderStatusLink();
				this.BindOrders();
			}
			CheckBoxColumn.RegisterClientCheckEvents(this.Page, this.Page.Form.ClientID);
		}
		private void btnRefuseReplace_Click(object sender, System.EventArgs e)
		{
			SubsiteSalesHelper.CheckReplace(this.hidOrderId.Value, this.replace_txtAdminRemark.Value, false);
			this.BindOrders();
			this.ShowMsg("成功的拒绝了订单换货", true);
		}
		private void btnAcceptReplace_Click(object sender, System.EventArgs e)
		{
			SubsiteSalesHelper.CheckReplace(this.hidOrderId.Value, this.replace_txtAdminRemark.Value, true);
			this.BindOrders();
			this.ShowMsg("成功的确认了订单换货", true);
		}
		private void btnRefuseReturn_Click(object sender, System.EventArgs e)
		{
			OrderInfo orderInfo = SubsiteSalesHelper.GetOrderInfo(this.hidOrderId.Value);
			SubsiteSalesHelper.CheckReturn(orderInfo, 0m, this.return_txtAdminRemark.Value, int.Parse(this.hidRefundType.Value), false);
			this.BindOrders();
			this.ShowMsg("成功的拒绝了订单退货", true);
		}
		private void btnAcceptReturn_Click(object sender, System.EventArgs e)
		{
			decimal num;
			if (!decimal.TryParse(this.return_txtRefundMoney.Text, out num))
			{
				this.ShowMsg("退款金额需为数字格式", false);
				return;
			}
			decimal d;
			decimal.TryParse(this.hidOrderTotal.Value, out d);
			if (num > d)
			{
				this.ShowMsg("退款金额不能大于订单金额", false);
				return;
			}
			OrderInfo orderInfo = SubsiteSalesHelper.GetOrderInfo(this.hidOrderId.Value);
			SubsiteSalesHelper.CheckReturn(orderInfo, num, this.return_txtAdminRemark.Value, int.Parse(this.hidRefundType.Value), true);
			this.BindOrders();
			this.ShowMsg("成功的确认了订单退货", true);
		}
		private void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReloadOrders(true);
		}
		private void btnBatchSendGoods_Click(object sender, System.EventArgs e)
		{
			this.LoadSendGoodsPage();
		}
		private void dlstOrders_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				OrderStatus orderStatus = (OrderStatus)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "OrderStatus");
				System.Web.UI.WebControls.HyperLink hyperLink = (System.Web.UI.WebControls.HyperLink)e.Item.FindControl("lkbtnEditPrice");
				System.Web.UI.WebControls.HyperLink hyperLink2 = (System.Web.UI.WebControls.HyperLink)e.Item.FindControl("lkbtnSendGoods");
				ImageLinkButton imageLinkButton = (ImageLinkButton)e.Item.FindControl("lkbtnPayOrder");
				ImageLinkButton imageLinkButton2 = (ImageLinkButton)e.Item.FindControl("lkbtnCreatePurchaseOrder");
				ImageLinkButton imageLinkButton3 = (ImageLinkButton)e.Item.FindControl("lkbtnConfirmOrder");
				System.Web.UI.WebControls.Literal literal = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litCloseOrder");
				int num = (int)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "GroupBuyId");
				int num2 = (int)System.Web.UI.DataBinder.Eval(e.Item.DataItem, "PurchaseOrders");
				System.Web.UI.HtmlControls.HtmlAnchor htmlAnchor = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lkbtnCheckRefund");
				System.Web.UI.HtmlControls.HtmlAnchor htmlAnchor2 = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lkbtnCheckReturn");
				System.Web.UI.HtmlControls.HtmlAnchor htmlAnchor3 = (System.Web.UI.HtmlControls.HtmlAnchor)e.Item.FindControl("lkbtnCheckReplace");
				if (orderStatus == OrderStatus.WaitBuyerPay)
				{
					literal.Visible = true;
					hyperLink.Visible = true;
					imageLinkButton.Visible = true;
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
					hyperLink2.Visible = (orderStatus == OrderStatus.BuyerAlreadyPaid && groupBuyStatus == GroupBuyStatus.Success);
					imageLinkButton2.Visible = (orderStatus == OrderStatus.BuyerAlreadyPaid && groupBuyStatus == GroupBuyStatus.Success && num2 == 0);
				}
				else
				{
					hyperLink2.Visible = (orderStatus == OrderStatus.BuyerAlreadyPaid);
				}
				imageLinkButton3.Visible = (orderStatus == OrderStatus.SellerAlreadySent);
			}
		}
		private void dlstOrders_ItemCommand(object sender, System.Web.UI.WebControls.DataListCommandEventArgs e)
		{
			OrderInfo orderInfo = SubsiteSalesHelper.GetOrderInfo(e.CommandArgument.ToString());
			if (orderInfo != null)
			{
				if (e.CommandName == "CONFIRM_PAY" && orderInfo.CheckAction(OrderActions.SELLER_CONFIRM_PAY))
				{
					int num = 0;
					int num2 = 0;
					int num3 = 0;
					if (orderInfo.CountDownBuyId > 0)
					{
						CountDownInfo countDownInfo = SubsitePromoteHelper.GetCountDownInfo(orderInfo.CountDownBuyId);
						if (countDownInfo == null || countDownInfo.EndDate < System.DateTime.Now)
						{
							this.ShowMsg("当前的订单为限时抢购订单，此活动已结束，所以不能支付", false);
							return;
						}
					}
					if (orderInfo.GroupBuyId > 0)
					{
						GroupBuyInfo groupBuy = SubsitePromoteHelper.GetGroupBuy(orderInfo.GroupBuyId);
						if (groupBuy != null)
						{
							if (groupBuy.Status == GroupBuyStatus.UnderWay)
							{
								num2 = SubsitePromoteHelper.GetOrderCount(orderInfo.GroupBuyId);
								num = groupBuy.MaxCount;
								num3 = orderInfo.GetGroupBuyOerderNumber();
								if (num < num2 + num3)
								{
									this.ShowMsg("当前的订单为团购订单，订购数量已超过订购总数，所以不能支付", false);
									return;
								}
								goto IL_DB;
							}
						}
						this.ShowMsg("当前的订单为团购订单，此团购活动已结束，所以不能支付", false);
						return;
					}
					IL_DB:
					if (SubsiteSalesHelper.ConfirmPay(orderInfo))
					{
						DebitNote debitNote = new DebitNote();
						debitNote.NoteId = Globals.GetGenerateId();
						debitNote.OrderId = e.CommandArgument.ToString();
						debitNote.Operator = Hidistro.Membership.Context.HiContext.Current.User.Username;
						debitNote.Remark = "后台" + debitNote.Operator + "支付成功";
						SubsiteSalesHelper.SaveDebitNote(debitNote);
						if (orderInfo.GroupBuyId > 0 && num == num2 + num3)
						{
							SubsitePromoteHelper.SetGroupBuyEndUntreated(orderInfo.GroupBuyId);
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
						this.ShowMsg("成功的确认了订单收款", true);
						return;
					}
					this.ShowMsg("确认订单收款失败", false);
					return;
				}
				else
				{
					if (e.CommandName == "FINISH_TRADE" && orderInfo.CheckAction(OrderActions.SELLER_FINISH_TRADE))
					{
						if (SubsiteSalesHelper.ConfirmOrderFinish(orderInfo))
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
						if (e.CommandName == "CREATE_PURCHASEORDER" && orderInfo.CheckAction(OrderActions.SUBSITE_CREATE_PURCHASEORDER))
						{
							if (SubsiteSalesHelper.CreatePurchaseOrder(orderInfo))
							{
								this.BindOrders();
								this.ShowMsg("生成采购单成功", true);
								return;
							}
							this.ShowMsg(" 生成采购单失败", false);
						}
					}
				}
			}
		}
		protected void btnAcceptRefund_Click(object sender, System.EventArgs e)
		{
			OrderInfo orderInfo = SubsiteSalesHelper.GetOrderInfo(this.hidOrderId.Value);
			if (SubsiteSalesHelper.CheckRefund(orderInfo, this.txtAdminRemark.Value, int.Parse(this.hidRefundType.Value), true))
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
			OrderInfo orderInfo = SubsiteSalesHelper.GetOrderInfo(this.hidOrderId.Value);
			if (SubsiteSalesHelper.CheckRefund(orderInfo, this.txtAdminRemark.Value, int.Parse(this.hidRefundType.Value), false))
			{
				this.BindOrders();
				this.ShowMsg("成功的拒绝了订单退款", true);
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
			int num = SubsiteSalesHelper.DeleteOrders(text);
			this.BindOrders();
			this.ShowMsg(string.Format("成功删除了{0}个订单", num), true);
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
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["GroupBuyId"]))
			{
				orderQuery.GroupBuyId = new int?(int.Parse(this.Page.Request.QueryString["GroupBuyId"]));
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
			nameValueCollection.Add("UserName", Globals.UrlEncode(this.txtUserName.Text));
			nameValueCollection.Add("OrderId", Globals.UrlEncode(this.txtOrderId.Text));
			nameValueCollection.Add("ProductName", Globals.UrlEncode(this.txtProductName.Text));
			nameValueCollection.Add("ShipTo", Globals.UrlEncode(this.txtShopTo.Text));
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
			base.ReloadPage(nameValueCollection);
		}
		private void LoadSendGoodsPage()
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder("~/ShopAdmin/Sales/BatchSendMyGoods.aspx?PageSize=10");
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
			{
				stringBuilder.AppendFormat("&OrderId={0}", this.Page.Request.QueryString["OrderId"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["PurchaseOrderId"]))
			{
				stringBuilder.AppendFormat("&PurchaseOrderId={0}", this.Page.Request.QueryString["PurchaseOrderId"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ProductName"]))
			{
				stringBuilder.AppendFormat("&ProductName={0}", this.Page.Request.QueryString["ProductName"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ShipTo"]))
			{
				stringBuilder.AppendFormat("&ShipTo={0}", this.Page.Request.QueryString["ShipTo"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["UserName"]))
			{
				stringBuilder.AppendFormat("&UserName={0}", this.Page.Request.QueryString["UserName"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartDate"]))
			{
				stringBuilder.AppendFormat("&StartDate={0}", System.DateTime.Parse(this.Page.Request.QueryString["StartDate"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["EndDate"]))
			{
				stringBuilder.AppendFormat("&EndDate={0}", System.DateTime.Parse(this.Page.Request.QueryString["EndDate"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderStatus"]))
			{
				int num = 0;
				if (int.TryParse(this.Page.Request.QueryString["OrderStatus"], out num))
				{
					stringBuilder.AppendFormat("&OrderStatus={0}", num);
				}
			}
			this.Page.Response.Redirect(stringBuilder.ToString());
		}
		private void BindOrders()
		{
			OrderQuery orderQuery = this.GetOrderQuery();
			DbQueryResult orders = SubsiteSalesHelper.GetOrders(orderQuery);
			this.dlstOrders.DataSource = orders.Data;
			this.dlstOrders.DataBind();
            this.pager.TotalRecords = orders.TotalRecords;
            this.pager1.TotalRecords = orders.TotalRecords;
			this.txtUserName.Text = orderQuery.UserName;
			this.txtOrderId.Text = orderQuery.OrderId;
			this.txtProductName.Text = orderQuery.ProductName;
			this.txtShopTo.Text = orderQuery.ShipTo;
            this.calendarStartDate.SelectedDate = orderQuery.StartDate;
            this.calendarEndDate.SelectedDate = orderQuery.EndDate;
			this.lblStatus.Text = ((int)orderQuery.Status).ToString();
		}
		private void SetOrderStatusLink()
		{
			string format = Globals.ApplicationPath + "/Shopadmin/sales/ManageMyOrder.aspx?OrderStatus={0}";
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
			orderInfo.ManagerRemark = Globals.HtmlEncode(this.txtRemark.Text.Trim());
			if (SubsiteSalesHelper.SaveRemark(orderInfo))
			{
				this.BindOrders();
				this.ShowMsg("保存备忘录成功", true);
				return;
			}
			this.ShowMsg("保存失败", false);
		}
		private void btnCloseOrder_Click(object sender, System.EventArgs e)
		{
			OrderInfo orderInfo = SubsiteSalesHelper.GetOrderInfo(this.hidOrderId.Value);
			orderInfo.CloseReason = this.ddlCloseReason.SelectedValue;
			if (SubsiteSalesHelper.CloseTransaction(orderInfo))
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
