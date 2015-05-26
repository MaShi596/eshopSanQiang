using Hidistro.AccountCenter.Business;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.SaleSystem.Catalog;
using Hishop.Plugins;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
namespace Hidistro.UI.Web
{
	public class SendPayment : System.Web.UI.Page
	{
		protected System.Web.UI.HtmlControls.HtmlForm form1;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			string orderId = this.Page.Request.QueryString["orderId"];
			OrderInfo orderInfo = TradeHelper.GetOrderInfo(orderId);
			if (orderInfo == null)
			{
				base.Response.Write("<div><font color='red'>您要付款的订单已经不存在，请联系管理员确定</font></div>");
				return;
			}
			if (orderInfo.OrderStatus != OrderStatus.WaitBuyerPay)
			{
				this.Page.Response.Write("订单当前状态不能支付");
				return;
			}
			if (orderInfo.CountDownBuyId > 0)
			{
				CountDownInfo countDownInfoByCountDownId = ProductBrowser.GetCountDownInfoByCountDownId(orderInfo.CountDownBuyId);
				if (countDownInfoByCountDownId == null || countDownInfoByCountDownId.EndDate < System.DateTime.Now)
				{
					this.Page.Response.Write("此订单属于限时抢购类型订单，但限时抢购活动已经结束或不存在");
					return;
				}
			}
			PaymentModeInfo paymentMode = TradeHelper.GetPaymentMode(orderInfo.PaymentTypeId);
			if (paymentMode == null)
			{
				base.Response.Write("<div><font color='red'>您之前选择的支付方式已经不存在，请联系管理员修改支付方式</font></div>");
				return;
			}
			System.Collections.Generic.Dictionary<string, LineItemInfo> lineItems = orderInfo.LineItems;
			foreach (LineItemInfo current in lineItems.Values)
			{
				int productId = current.ProductId;
				ProductBrowseInfo productBrowseInfo = ProductBrowser.GetProductBrowseInfo(productId, new int?(6), new int?(6));
				if (productBrowseInfo.Product == null || productBrowseInfo.Product.SaleStatus == ProductSaleStatus.Delete)
				{
					base.Response.Redirect(Globals.ApplicationPath + "/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("订单内商品已经被管理员删除"));
					return;
				}
				if (productBrowseInfo.Product.SaleStatus == ProductSaleStatus.OnStock)
				{
					base.Response.Redirect(Globals.ApplicationPath + "/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("订单内商品已入库"));
					return;
				}
				ProductInfo product = productBrowseInfo.Product;
				int stock = product.Stock;
				int shipmentQuantity = current.ShipmentQuantity;
				if (shipmentQuantity > stock)
				{
					base.Response.Redirect(Globals.ApplicationPath + "/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("订单内商品库存不足"));
					return;
				}
			}
			string showUrl = Globals.GetSiteUrls().UrlData.FormatUrl("user_UserOrders");
			if (paymentMode.Gateway.ToLower() != "hishop.plugins.payment.podrequest")
			{
				showUrl = base.Server.UrlEncode(string.Format("http://{0}/user/OrderDetails.aspx?OrderId={1}", base.Request.Url.Host, orderInfo.OrderId));
			}
			if (string.Compare(paymentMode.Gateway, "Hishop.Plugins.Payment.BankRequest", true) == 0)
			{
				showUrl = Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("bank_pay", new object[]
				{
					orderInfo.OrderId
				}));
			}
			if (string.Compare(paymentMode.Gateway, "Hishop.Plugins.Payment.AdvanceRequest", true) == 0)
			{
				showUrl = Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("advance_pay", new object[]
				{
					orderInfo.OrderId
				}));
			}
			string attach = "";
			System.Web.HttpCookie httpCookie = Hidistro.Membership.Context.HiContext.Current.Context.Request.Cookies["Token_" + Hidistro.Membership.Context.HiContext.Current.User.UserId.ToString()];
			if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
			{
				attach = httpCookie.Value;
			}
			PaymentRequest paymentRequest = PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), orderInfo.OrderId, orderInfo.GetTotal(), "订单支付", "订单号-" + orderInfo.OrderId, orderInfo.EmailAddress, orderInfo.OrderDate, showUrl, Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentReturn_url", new object[]
			{
				paymentMode.Gateway
			})), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentNotify_url", new object[]
			{
				paymentMode.Gateway
			})), attach);
			paymentRequest.SendRequest();
		}
	}
}
