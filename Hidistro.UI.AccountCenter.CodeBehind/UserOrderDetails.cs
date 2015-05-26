using Hidistro.AccountCenter.Business;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using System;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	public class UserOrderDetails : MemberTemplatedWebControl
	{
		private string orderId;
		private System.Web.UI.WebControls.Literal litOrderId;
		private FormatedTimeLabel litAddDate;
		private FormatedMoneyLabel lbltotalPrice;
		private OrderStatusLabel lblOrderStatus;
		private System.Web.UI.WebControls.Literal litCloseReason;
		private System.Web.UI.WebControls.Literal litRemark;
		private System.Web.UI.WebControls.Literal litShipTo;
		private System.Web.UI.WebControls.Literal litRegion;
		private System.Web.UI.WebControls.Literal litAddress;
		private System.Web.UI.WebControls.Literal litZipcode;
		private System.Web.UI.WebControls.Literal litEmail;
		private System.Web.UI.WebControls.Literal litPhone;
		private System.Web.UI.WebControls.Literal litTellPhone;
		private System.Web.UI.WebControls.Literal litShipToDate;
		private System.Web.UI.WebControls.Literal litUserName;
		private System.Web.UI.WebControls.Literal litUserAddress;
		private System.Web.UI.WebControls.Literal litUserEmail;
		private System.Web.UI.WebControls.Literal litUserPhone;
		private System.Web.UI.WebControls.Literal litUserTellPhone;
		private System.Web.UI.WebControls.Literal litUserQQ;
		private System.Web.UI.WebControls.Literal litUserMSN;
		private System.Web.UI.WebControls.Literal litPaymentType;
		private System.Web.UI.WebControls.Literal litModeName;
		private System.Web.UI.WebControls.Panel plOrderSended;
		private System.Web.UI.WebControls.Literal litRealModeName;
		private System.Web.UI.WebControls.Literal litShippNumber;
		private System.Web.UI.WebControls.HyperLink litDiscountName;
		private System.Web.UI.WebControls.HyperLink litFreeName;
		private FormatedMoneyLabel litTax;
		private System.Web.UI.WebControls.Literal litInvoiceTitle;
		private System.Web.UI.WebControls.Panel plExpress;
		private System.Web.UI.HtmlControls.HtmlAnchor power;
		private Common_OrderManage_OrderItems orderItems;
		private System.Web.UI.WebControls.GridView grdOrderGift;
		private System.Web.UI.WebControls.Panel plOrderGift;
		private FormatedMoneyLabel lblCartMoney;
		private System.Web.UI.WebControls.Literal lblBundlingPrice;
		private System.Web.UI.WebControls.Literal litPoints;
		private System.Web.UI.WebControls.HyperLink litSentTimesPointPromotion;
		private System.Web.UI.WebControls.Literal litWeight;
		private System.Web.UI.WebControls.Literal litFree;
		private FormatedMoneyLabel lblFreight;
		private FormatedMoneyLabel lblPayCharge;
		private System.Web.UI.WebControls.Literal litCouponValue;
		private FormatedMoneyLabel lblDiscount;
		private FormatedMoneyLabel litTotalPrice;
		private FormatedMoneyLabel lblAdjustedDiscount;
		private FormatedMoneyLabel lblRefundTotal;
		private System.Web.UI.WebControls.Label lbRefundMoney;
		private System.Web.UI.WebControls.Panel plRefund;
		private FormatedMoneyLabel lblTotalBalance;
		private System.Web.UI.WebControls.Literal litRefundOrderRemark;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserOrderDetails.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.orderId = this.Page.Request.QueryString["orderId"];
			this.litOrderId = (System.Web.UI.WebControls.Literal)this.FindControl("litOrderId");
			this.lbltotalPrice = (FormatedMoneyLabel)this.FindControl("lbltotalPrice");
			this.litAddDate = (FormatedTimeLabel)this.FindControl("litAddDate");
			this.lblOrderStatus = (OrderStatusLabel)this.FindControl("lblOrderStatus");
			this.litCloseReason = (System.Web.UI.WebControls.Literal)this.FindControl("litCloseReason");
			this.litRemark = (System.Web.UI.WebControls.Literal)this.FindControl("litRemark");
			this.litShipTo = (System.Web.UI.WebControls.Literal)this.FindControl("litShipTo");
			this.litRegion = (System.Web.UI.WebControls.Literal)this.FindControl("litRegion");
			this.litAddress = (System.Web.UI.WebControls.Literal)this.FindControl("litAddress");
			this.litZipcode = (System.Web.UI.WebControls.Literal)this.FindControl("litZipcode");
			this.litEmail = (System.Web.UI.WebControls.Literal)this.FindControl("litEmail");
			this.litPhone = (System.Web.UI.WebControls.Literal)this.FindControl("litPhone");
			this.litTellPhone = (System.Web.UI.WebControls.Literal)this.FindControl("litTellPhone");
			this.litShipToDate = (System.Web.UI.WebControls.Literal)this.FindControl("litShipToDate");
			this.litUserName = (System.Web.UI.WebControls.Literal)this.FindControl("litUserName");
			this.litUserAddress = (System.Web.UI.WebControls.Literal)this.FindControl("litUserAddress");
			this.litUserEmail = (System.Web.UI.WebControls.Literal)this.FindControl("litUserEmail");
			this.litUserPhone = (System.Web.UI.WebControls.Literal)this.FindControl("litUserPhone");
			this.litUserTellPhone = (System.Web.UI.WebControls.Literal)this.FindControl("litUserTellPhone");
			this.litUserQQ = (System.Web.UI.WebControls.Literal)this.FindControl("litUserQQ");
			this.litUserMSN = (System.Web.UI.WebControls.Literal)this.FindControl("litUserMSN");
			this.litPaymentType = (System.Web.UI.WebControls.Literal)this.FindControl("litPaymentType");
			this.litModeName = (System.Web.UI.WebControls.Literal)this.FindControl("litModeName");
			this.plOrderSended = (System.Web.UI.WebControls.Panel)this.FindControl("plOrderSended");
			this.litRealModeName = (System.Web.UI.WebControls.Literal)this.FindControl("litRealModeName");
			this.litShippNumber = (System.Web.UI.WebControls.Literal)this.FindControl("litShippNumber");
			this.litDiscountName = (System.Web.UI.WebControls.HyperLink)this.FindControl("litDiscountName");
			this.lblAdjustedDiscount = (FormatedMoneyLabel)this.FindControl("lblAdjustedDiscount");
			this.litFreeName = (System.Web.UI.WebControls.HyperLink)this.FindControl("litFreeName");
			this.plExpress = (System.Web.UI.WebControls.Panel)this.FindControl("plExpress");
			this.power = (System.Web.UI.HtmlControls.HtmlAnchor)this.FindControl("power");
			this.orderItems = (Common_OrderManage_OrderItems)this.FindControl("Common_OrderManage_OrderItems");
			this.grdOrderGift = (System.Web.UI.WebControls.GridView)this.FindControl("grdOrderGift");
			this.plOrderGift = (System.Web.UI.WebControls.Panel)this.FindControl("plOrderGift");
			this.lblCartMoney = (FormatedMoneyLabel)this.FindControl("lblCartMoney");
			this.lblBundlingPrice = (System.Web.UI.WebControls.Literal)this.FindControl("lblBundlingPrice");
			this.litPoints = (System.Web.UI.WebControls.Literal)this.FindControl("litPoints");
			this.litSentTimesPointPromotion = (System.Web.UI.WebControls.HyperLink)this.FindControl("litSentTimesPointPromotion");
			this.litWeight = (System.Web.UI.WebControls.Literal)this.FindControl("litWeight");
			this.litFree = (System.Web.UI.WebControls.Literal)this.FindControl("litFree");
			this.lblFreight = (FormatedMoneyLabel)this.FindControl("lblFreight");
			this.lblPayCharge = (FormatedMoneyLabel)this.FindControl("lblPayCharge");
			this.litCouponValue = (System.Web.UI.WebControls.Literal)this.FindControl("litCouponValue");
			this.lblDiscount = (FormatedMoneyLabel)this.FindControl("lblDiscount");
			this.litTotalPrice = (FormatedMoneyLabel)this.FindControl("litTotalPrice");
			this.lblRefundTotal = (FormatedMoneyLabel)this.FindControl("lblRefundTotal");
			this.lbRefundMoney = (System.Web.UI.WebControls.Label)this.FindControl("lbRefundMoney");
			this.plRefund = (System.Web.UI.WebControls.Panel)this.FindControl("plRefund");
			this.lblTotalBalance = (FormatedMoneyLabel)this.FindControl("lblTotalBalance");
			this.litRefundOrderRemark = (System.Web.UI.WebControls.Literal)this.FindControl("litRefundOrderRemark");
			this.litTax = (FormatedMoneyLabel)this.FindControl("litTax");
			this.litInvoiceTitle = (System.Web.UI.WebControls.Literal)this.FindControl("litInvoiceTitle");
			PageTitle.AddTitle("订单详细页", HiContext.Current.Context);
			if (!this.Page.IsPostBack)
			{
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(this.orderId);
				if (orderInfo == null || orderInfo.UserId != HiContext.Current.User.UserId)
				{
					this.Page.Response.Redirect(Globals.ApplicationPath + "/ResourceNotFound.aspx?errorMsg=" + Globals.UrlEncode("该订单不存在或者不属于当前用户的订单"));
				}
				else
				{
					this.BindOrderBase(orderInfo);
					this.BindOrderAddress(orderInfo);
					this.BindOrderItems(orderInfo);
					this.BindOrderRefund(orderInfo);
					this.BindOrderReturn(orderInfo);
				}
			}
		}
		private void BindOrderBase(OrderInfo order)
		{
			this.litOrderId.Text = order.OrderId;
			this.lbltotalPrice.Money = order.GetTotal();
			this.litAddDate.Time = order.OrderDate;
			this.lblOrderStatus.OrderStatusCode = order.OrderStatus;
			if (order.OrderStatus == OrderStatus.Closed)
			{
				this.litCloseReason.Text = order.CloseReason;
			}
			this.litRemark.Text = order.Remark;
		}
		private void BindOrderAddress(OrderInfo order)
		{
			this.litShipTo.Text = order.ShipTo;
			this.litRegion.Text = order.ShippingRegion;
			this.litAddress.Text = order.Address;
			this.litZipcode.Text = order.ZipCode;
			this.litEmail.Text = order.EmailAddress;
			this.litTellPhone.Text = order.TelPhone;
			this.litPhone.Text = order.CellPhone;
			this.litShipToDate.Text = order.ShipToDate;
			Member member = HiContext.Current.User as Member;
			this.litUserName.Text = member.Username;
			this.litUserAddress.Text = RegionHelper.GetFullRegion(member.RegionId, "") + member.Address;
			this.litUserTellPhone.Text = member.TelPhone;
			this.litUserPhone.Text = member.CellPhone;
			this.litUserEmail.Text = member.Email;
			this.litUserQQ.Text = member.QQ;
			this.litUserMSN.Text = member.MSN;
			this.litPaymentType.Text = order.PaymentType + "(" + Globals.FormatMoney(order.PayCharge) + ")";
			this.litModeName.Text = order.ModeName + "(" + Globals.FormatMoney(order.AdjustedFreight) + ")";
			if (order.OrderStatus == OrderStatus.SellerAlreadySent || order.OrderStatus == OrderStatus.Finished)
			{
				this.plOrderSended.Visible = true;
				this.litShippNumber.Text = order.ShipOrderNumber;
				this.litRealModeName.Text = order.ExpressCompanyName;
			}
			bool arg_1E4_0;
			if (order.OrderStatus != OrderStatus.SellerAlreadySent)
			{
				if (order.OrderStatus != OrderStatus.Finished)
				{
					arg_1E4_0 = true;
					goto IL_1E4;
				}
			}
			arg_1E4_0 = string.IsNullOrEmpty(order.ExpressCompanyAbb);
			IL_1E4:
			if (!arg_1E4_0)
			{
				if (this.plExpress != null)
				{
					this.plExpress.Visible = true;
				}
				if (Express.GetExpressType() == "kuaidi100" && this.power != null)
				{
					this.power.Visible = true;
				}
			}
		}
		private void BindOrderItems(OrderInfo order)
		{
			this.orderItems.DataSource = order.LineItems.Values;
			this.orderItems.DataBind();
			if (order.Gifts.Count > 0)
			{
				this.plOrderGift.Visible = true;
				this.grdOrderGift.DataSource = order.Gifts;
				this.grdOrderGift.DataBind();
			}
			if (order.BundlingID > 0)
			{
				this.lblBundlingPrice.Text = string.Format("<span style=\"color:Red;\">捆绑价格：{0}</span>", Globals.FormatMoney(order.BundlingPrice));
			}
			this.lblCartMoney.Money = order.GetAmount();
			this.litWeight.Text = order.Weight.ToString();
			this.lblPayCharge.Money = order.PayCharge;
			this.lblFreight.Money = order.AdjustedFreight;
			if (order.IsFreightFree)
			{
				this.litFreeName.Text = order.FreightFreePromotionName;
				this.litFreeName.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails", new object[]
				{
					order.FreightFreePromotionId
				});
			}
			this.litTax.Money = order.Tax;
			this.litInvoiceTitle.Text = order.InvoiceTitle;
			this.lblAdjustedDiscount.Money = order.AdjustedDiscount;
			this.litCouponValue.Text = order.CouponName + " -" + Globals.FormatMoney(order.CouponValue);
			this.lblDiscount.Money = order.ReducedPromotionAmount;
			if (order.IsReduced)
			{
				this.litDiscountName.Text = order.ReducedPromotionName;
				this.litDiscountName.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails", new object[]
				{
					order.ReducedPromotionId
				});
			}
			this.litPoints.Text = order.Points.ToString(System.Globalization.CultureInfo.InvariantCulture);
			if (order.IsSendTimesPoint)
			{
				this.litSentTimesPointPromotion.Text = order.SentTimesPointPromotionName;
				this.litSentTimesPointPromotion.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails", new object[]
				{
					order.SentTimesPointPromotionId
				});
			}
			this.litTotalPrice.Money = order.GetTotal();
		}
		private void BindOrderRefund(OrderInfo order)
		{
			if (order.RefundStatus == RefundStatus.Refund || order.RefundStatus == RefundStatus.Below)
			{
				this.plRefund.Visible = true;
				this.lblTotalBalance.Money = order.RefundAmount;
				this.litRefundOrderRemark.Text = order.RefundRemark;
			}
		}
		private void BindOrderReturn(OrderInfo order)
		{
			if (order.OrderStatus == OrderStatus.Returned)
			{
				decimal num;
				this.lblRefundTotal.Money = TradeHelper.GetRefundMoney(order, out num);
			}
			else
			{
				this.lbRefundMoney.Visible = false;
			}
		}
	}
}
