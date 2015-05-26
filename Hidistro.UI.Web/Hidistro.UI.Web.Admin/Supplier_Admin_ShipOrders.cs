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
using Hishop.Web.CustomMade;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Orders)]
	public class Supplier_Admin_ShipOrders : AdminPage
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
		protected System.Web.UI.HtmlControls.HtmlGenericControl htmlLiShipOrderPriceAll;
		protected System.Web.UI.WebControls.Literal litlShipOrderPriceAll;
		protected Pager pager1;
		protected ImageLinkButton lkbtnDeleteCheck;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidOrderId;
		protected System.Web.UI.WebControls.DataList dlstOrders;
		protected Pager pager;
		protected CloseTranReasonDropDownList ddlCloseReason;
		protected FormatedMoneyLabel lblOrderTotalForRemark;
		protected OrderRemarkImageRadioButtonList orderRemarkImageForRemark;
		protected System.Web.UI.WebControls.TextBox txtRemark;
		protected System.Web.UI.WebControls.Button btnCloseOrder;
		protected System.Web.UI.WebControls.Button btnRemark;
		protected System.Web.UI.WebControls.Button btnOrderGoods;
		protected System.Web.UI.WebControls.Button btnProductGoods;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.dlstOrders.ItemDataBound += new System.Web.UI.WebControls.DataListItemEventHandler(this.dlstOrders_ItemDataBound);
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
			this.dlstOrders.ItemCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.dlstOrders_ItemCommand);
			this.btnRemark.Click += new System.EventHandler(this.btnRemark_Click);
			this.btnCloseOrder.Click += new System.EventHandler(this.btnCloseOrder_Click);
			this.lkbtnDeleteCheck.Click += new System.EventHandler(this.lkbtnDeleteCheck_Click);
			this.btnOrderGoods.Click += new System.EventHandler(this.btnOrderGoods_Click);
			this.btnProductGoods.Click += new System.EventHandler(this.btnProductGoods_Click);
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
			stringBuilder.AppendLine("<td>发货单单号</td>");
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
				System.Web.UI.WebControls.Label label = (System.Web.UI.WebControls.Label)e.Item.FindControl("lkbtnSendGoods");
				label.Visible = (orderStatus == OrderStatus.BuyerAlreadyPaid);
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
								goto IL_D9;
							}
						}
						this.ShowMsg("当前的订单为团购订单，此团购活动已结束，所以不能支付", false);
						return;
					}
					IL_D9:
					if (OrderHelper.ConfirmPay(orderInfo))
					{
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
						if (OrderHelper.ConfirmOrderFinish(orderInfo))
						{
							this.BindOrders();
							this.ShowMsg("成功的完成了该订单", true);
							return;
						}
						this.ShowMsg("完成订单失败", false);
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
			orderQuery.UserName = "主站";
			if (Hidistro.Membership.Context.HiContext.Current.User.IsInRole("供应商") || Hidistro.Membership.Context.HiContext.Current.User.IsInRole("区域发货点"))
			{
				orderQuery.UserName = Hidistro.Membership.Context.HiContext.Current.User.Username;
			}
			DbQueryResult dbQueryResult = Methods.Supplier_ShipOrderSGet(orderQuery, 0);
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
			if (orderQuery.Status == OrderStatus.BuyerAlreadyPaid)
			{
				this.htmlLiShipOrderPriceAll.Visible = false;
				return;
			}
			this.htmlLiShipOrderPriceAll.Visible = true;
			this.litlShipOrderPriceAll.Text = Methods.Supplier_ShipOrderPriceAllGet(orderQuery).ToString("f2");
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
			string format = Globals.ApplicationPath + "/admin/Cpage/Supplier/Supplier_Admin_ShipOrders.aspx?orderStatus={0}";
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
