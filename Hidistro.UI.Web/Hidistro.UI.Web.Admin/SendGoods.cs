using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Messages;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Plugins;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.OrderSendGoods)]
	public class SendGoods : AdminPage
	{
		private string orderId;
		protected System.Web.UI.WebControls.Label lblOrderId;
		protected FormatedTimeLabel lblOrderTime;
		protected ShippingModeRadioButtonList radioShippingMode;
		protected ExpressRadioButtonList expressRadioButtonList;
		protected System.Web.UI.WebControls.TextBox txtShipOrderNumber;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtShipOrderNumberTip;
		protected System.Web.UI.WebControls.Button btnSendGoods;
		protected Order_ItemsList itemsList;
		protected System.Web.UI.WebControls.Literal litShippingModeName;
		protected System.Web.UI.WebControls.Literal litReceivingInfo;
		protected System.Web.UI.WebControls.Label litShipToDate;
		protected System.Web.UI.WebControls.Label litRemark;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.orderId = this.Page.Request.QueryString["OrderId"];
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.orderId);
			this.BindOrderItems(orderInfo);
			this.btnSendGoods.Click += new System.EventHandler(this.btnSendGoods_Click);
			this.radioShippingMode.SelectedIndexChanged += new System.EventHandler(this.radioShippingMode_SelectedIndexChanged);
			if (!this.Page.IsPostBack)
			{
				if (orderInfo == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				this.radioShippingMode.DataBind();
				this.radioShippingMode.SelectedValue = new int?(orderInfo.ShippingModeId);
				this.BindExpressCompany(orderInfo.ShippingModeId);
				this.expressRadioButtonList.SelectedValue = orderInfo.ExpressCompanyAbb;
				this.BindShippingAddress(orderInfo);
				this.litShippingModeName.Text = orderInfo.ModeName;
				this.litShipToDate.Text = orderInfo.ShipToDate;
				this.litRemark.Text = orderInfo.Remark;
				this.txtShipOrderNumber.Text = orderInfo.ShipOrderNumber;
			}
		}
		private void BindOrderItems(OrderInfo order)
		{
			this.lblOrderId.Text = order.OrderId;
			this.lblOrderTime.Time = order.OrderDate;
			this.itemsList.Order = order;
		}
		private void BindShippingAddress(OrderInfo order)
		{
			string text = string.Empty;
			if (!string.IsNullOrEmpty(order.ShippingRegion))
			{
				text = order.ShippingRegion;
			}
			if (!string.IsNullOrEmpty(order.Address))
			{
				text += order.Address;
			}
			if (!string.IsNullOrEmpty(order.ShipTo))
			{
				text = text + "  " + order.ShipTo;
			}
			if (!string.IsNullOrEmpty(order.ZipCode))
			{
				text = text + "  " + order.ZipCode;
			}
			if (!string.IsNullOrEmpty(order.TelPhone))
			{
				text = text + "  " + order.TelPhone;
			}
			if (!string.IsNullOrEmpty(order.CellPhone))
			{
				text = text + "  " + order.CellPhone;
			}
			this.litReceivingInfo.Text = text;
		}
		private void BindExpressCompany(int modeId)
		{
			this.expressRadioButtonList.ExpressCompanies = SalesHelper.GetExpressCompanysByMode(modeId);
			this.expressRadioButtonList.DataBind();
		}
		private void radioShippingMode_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (this.radioShippingMode.SelectedValue.HasValue)
			{
				this.BindExpressCompany(this.radioShippingMode.SelectedValue.Value);
			}
		}
		private void btnSendGoods_Click(object sender, System.EventArgs e)
		{
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.orderId);
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
			if (!this.radioShippingMode.SelectedValue.HasValue)
			{
				this.ShowMsg("请选择配送方式", false);
				return;
			}
			if (string.IsNullOrEmpty(this.txtShipOrderNumber.Text.Trim()) || this.txtShipOrderNumber.Text.Trim().Length > 20)
			{
				this.ShowMsg("运单号码不能为空，在1至20个字符之间", false);
				return;
			}
			if (string.IsNullOrEmpty(this.expressRadioButtonList.SelectedValue))
			{
				this.ShowMsg("请选择物流公司", false);
				return;
			}
			ShippingModeInfo shippingMode = SalesHelper.GetShippingMode(this.radioShippingMode.SelectedValue.Value, true);
			orderInfo.RealShippingModeId = this.radioShippingMode.SelectedValue.Value;
			orderInfo.RealModeName = shippingMode.Name;
			ExpressCompanyInfo expressCompanyInfo = ExpressHelper.FindNode(this.expressRadioButtonList.SelectedValue);
			if (expressCompanyInfo != null)
			{
				orderInfo.ExpressCompanyAbb = expressCompanyInfo.Kuaidi100Code;
				orderInfo.ExpressCompanyName = expressCompanyInfo.Name;
			}
			orderInfo.ShipOrderNumber = this.txtShipOrderNumber.Text;
			if (OrderHelper.SendGoods(orderInfo))
			{
				SendNote sendNote = new SendNote();
				sendNote.NoteId = Globals.GetGenerateId();
				sendNote.OrderId = this.orderId;
				sendNote.Operator = Hidistro.Membership.Context.HiContext.Current.User.Username;
				sendNote.Remark = "后台" + sendNote.Operator + "发货成功";
				OrderHelper.SaveSendNote(sendNote);
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
				int num = orderInfo.UserId;
				if (num == 1100)
				{
					num = 0;
				}
				Hidistro.Membership.Core.IUser user = Hidistro.Membership.Context.Users.GetUser(num);
				Messenger.OrderShipping(orderInfo, user);
				orderInfo.OnDeliver();
				this.ShowMsg("发货成功", true);
				return;
			}
			this.ShowMsg("发货失败", false);
		}
	}
}
