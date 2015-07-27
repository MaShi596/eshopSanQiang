using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Messages;
using Hidistro.Subsites.Sales;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using Hishop.Plugins;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class SendMyGoods : DistributorPage
	{
		protected System.Web.UI.WebControls.Label lblOrderId;
		protected FormatedTimeLabel lblOrderTime;
		protected System.Web.UI.HtmlControls.HtmlGenericControl htmlContent1;
		protected ShippingModeRadioButtonList radioShippingMode;
		protected ExpressRadioButtonList expressRadioButtonList;
		protected System.Web.UI.WebControls.TextBox txtShipOrderNumber;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtShipOrderNumberTip;
		protected System.Web.UI.HtmlControls.HtmlGenericControl htmlContent2;
		protected System.Web.UI.WebControls.Literal litlShipOrderNumber;
		protected System.Web.UI.WebControls.Button btnSendGoods;
		protected Order_ItemsList itemsList;
		protected System.Web.UI.WebControls.Literal litShippingModeName;
		protected System.Web.UI.WebControls.Literal litReceivingInfo;
		protected System.Web.UI.WebControls.Label litShipToDate;
		protected System.Web.UI.WebControls.Literal litRemark;
		private string orderId;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.orderId = this.Page.Request.QueryString["OrderId"];
			this.btnSendGoods.Click += new System.EventHandler(this.btnSendGoods_Click);
			this.radioShippingMode.SelectedIndexChanged += new System.EventHandler(this.radioShippingMode_SelectedIndexChanged);
			OrderInfo orderInfo = SubsiteSalesHelper.GetOrderInfo(this.orderId);
			this.BindOrderItems(orderInfo);
			if (!this.Page.IsPostBack)
			{
				if (orderInfo == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				this.radioShippingMode.DataBind();
				this.BindShippingAddress(orderInfo);
				PurchaseOrderInfo purchaseByOrderId = SubsiteSalesHelper.GetPurchaseByOrderId(this.orderId);
				if (purchaseByOrderId != null && purchaseByOrderId.PurchaseStatus != OrderStatus.WaitBuyerPay && purchaseByOrderId.PurchaseStatus != OrderStatus.BuyerAlreadyPaid)
				{
					this.txtShipOrderNumber.Text = purchaseByOrderId.ShipOrderNumber;
					this.radioShippingMode.SelectedValue = new int?(purchaseByOrderId.RealShippingModeId);
					this.BindExpressCompany(purchaseByOrderId.RealShippingModeId);
					this.expressRadioButtonList.SelectedValue = purchaseByOrderId.ExpressCompanyName;
				}
				else
				{
					this.radioShippingMode.SelectedValue = new int?(orderInfo.ShippingModeId);
					this.BindExpressCompany(orderInfo.ShippingModeId);
					this.expressRadioButtonList.SelectedValue = orderInfo.ExpressCompanyName;
				}
				this.litShippingModeName.Text = orderInfo.ModeName;
				this.litShipToDate.Text = orderInfo.ShipToDate;
				this.litRemark.Text = orderInfo.Remark;
				if (this.txtShipOrderNumber.Text.IndexOf("showWindow_ShipInfoPage") != -1)
				{
					this.htmlContent1.Attributes.Add("style", "display:none");
					this.litlShipOrderNumber.Text = this.txtShipOrderNumber.Text;
					return;
				}
				//this.htmlContent2.Attributes.Add("style", "display:none");
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
			this.expressRadioButtonList.ExpressCompanies = SubsiteSalesHelper.GetExpressCompanysByMode(modeId);
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
			if (this.txtShipOrderNumber.Text.IndexOf("showWindow_ShipInfoPage") != -1)
			{
				OrderInfo orderInfo = SubsiteSalesHelper.GetOrderInfo(this.orderId);
				if (orderInfo == null)
				{
					return;
				}
				if (orderInfo.GroupBuyId > 0 && orderInfo.GroupBuyStatus != GroupBuyStatus.Success)
				{
					this.ShowMsg("当前订单为团购订单，团购活动还未成功结束，所以不能发货", false);
					return;
				}
				if (orderInfo.OrderStatus != OrderStatus.BuyerAlreadyPaid)
				{
					this.ShowMsg("当前订单状态没有付款或不是等待发货的订单，所以不能发货", false);
					return;
				}
				orderInfo.RealShippingModeId = 0;
				orderInfo.RealModeName = "配送方式(已实际发货单为准)";
				orderInfo.ShipOrderNumber = this.txtShipOrderNumber.Text;
				if (SubsiteSalesHelper.SendGoods(orderInfo))
				{
					if (!string.IsNullOrEmpty(orderInfo.GatewayOrderId) && orderInfo.GatewayOrderId.Trim().Length > 0)
					{
						PaymentModeInfo paymentMode = SubsiteSalesHelper.GetPaymentMode(orderInfo.PaymentTypeId);
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
				return;
			}
			else
			{
				if (string.IsNullOrEmpty(this.txtShipOrderNumber.Text.Trim()) || this.txtShipOrderNumber.Text.Trim().Length > 20)
				{
					this.ShowMsg("运单号码不能为空，在1至20个字符之间", false);
					return;
				}
				OrderInfo orderInfo2 = SubsiteSalesHelper.GetOrderInfo(this.orderId);
				if (orderInfo2 == null)
				{
					return;
				}
				if (orderInfo2.GroupBuyId > 0 && orderInfo2.GroupBuyStatus != GroupBuyStatus.Success)
				{
					this.ShowMsg("当前订单为团购订单，团购活动还未成功结束，所以不能发货", false);
					return;
				}
				if (orderInfo2.OrderStatus != OrderStatus.BuyerAlreadyPaid)
				{
					this.ShowMsg("当前订单状态没有付款或不是等待发货的订单，所以不能发货", false);
					return;
				}
				if (!this.radioShippingMode.SelectedValue.HasValue)
				{
					this.ShowMsg("请选择配送方式", false);
					return;
				}
				if (string.IsNullOrEmpty(this.expressRadioButtonList.SelectedValue))
				{
					this.ShowMsg("请选择物流公司", false);
					return;
				}
				ShippingModeInfo shippingMode = SubsiteSalesHelper.GetShippingMode(this.radioShippingMode.SelectedValue.Value, true);
				orderInfo2.RealShippingModeId = this.radioShippingMode.SelectedValue.Value;
				orderInfo2.RealModeName = shippingMode.Name;
				ExpressCompanyInfo expressCompanyInfo = ExpressHelper.FindNode(this.expressRadioButtonList.SelectedValue);
				if (expressCompanyInfo != null)
				{
					orderInfo2.ExpressCompanyAbb = expressCompanyInfo.Kuaidi100Code;
					orderInfo2.ExpressCompanyName = expressCompanyInfo.Name;
				}
				orderInfo2.ShipOrderNumber = this.txtShipOrderNumber.Text;
				if (SubsiteSalesHelper.SendGoods(orderInfo2))
				{
					if (!string.IsNullOrEmpty(orderInfo2.GatewayOrderId) && orderInfo2.GatewayOrderId.Trim().Length > 0)
					{
						PaymentModeInfo paymentMode2 = SubsiteSalesHelper.GetPaymentMode(orderInfo2.PaymentTypeId);
						if (paymentMode2 != null)
						{
							PaymentRequest paymentRequest2 = PaymentRequest.CreateInstance(paymentMode2.Gateway, HiCryptographer.Decrypt(paymentMode2.Settings), orderInfo2.OrderId, orderInfo2.GetTotal(), "订单发货", "订单号-" + orderInfo2.OrderId, orderInfo2.EmailAddress, orderInfo2.OrderDate, Globals.FullPath(Globals.GetSiteUrls().Home), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentReturn_url", new object[]
							{
								paymentMode2.Gateway
							})), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentNotify_url", new object[]
							{
								paymentMode2.Gateway
							})), "");
							paymentRequest2.SendGoods(orderInfo2.GatewayOrderId, orderInfo2.RealModeName, orderInfo2.ShipOrderNumber, "EXPRESS");
						}
					}
					int num2 = orderInfo2.UserId;
					if (num2 == 1100)
					{
						num2 = 0;
					}
					Hidistro.Membership.Core.IUser user2 = Hidistro.Membership.Context.Users.GetUser(num2);
					Messenger.OrderShipping(orderInfo2, user2);
					orderInfo2.OnDeliver();
					this.ShowMsg("发货成功", true);
					return;
				}
				this.ShowMsg("发货失败", false);
				return;
			}
		}
	}
}
