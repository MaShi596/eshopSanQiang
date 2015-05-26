using Hidistro.Core;
using Hidistro.Entities.Sales;
using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class Order_ChargesList : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Literal litFreight;
		protected System.Web.UI.WebControls.Literal litShippingMode;
		protected System.Web.UI.WebControls.HyperLink hlkFreightFreePromotion;
		protected System.Web.UI.WebControls.LinkButton lkBtnEditshipingMode;
		protected System.Web.UI.WebControls.Literal litPayCharge;
		protected System.Web.UI.WebControls.Literal litPayMode;
		protected System.Web.UI.WebControls.LinkButton lkBtnEditPayMode;
		protected System.Web.UI.WebControls.Literal litCouponValue;
		protected System.Web.UI.WebControls.Literal litCoupon;
		protected System.Web.UI.WebControls.Literal litDiscount;
		protected System.Web.UI.WebControls.Literal litTax;
		protected System.Web.UI.WebControls.Literal litInvoiceTitle;
		protected System.Web.UI.WebControls.Literal litPoints;
		protected System.Web.UI.WebControls.HyperLink hlkSentTimesPointPromotion;
		protected System.Web.UI.WebControls.Literal litTotalPrice;
		private OrderInfo order;
		public OrderInfo Order
		{
			get
			{
				return this.order;
			}
			set
			{
				this.order = value;
			}
		}
		protected override void OnLoad(System.EventArgs e)
		{
			this.LoadControl();
		}
		public void LoadControl()
		{
			if (this.order.OrderStatus == OrderStatus.WaitBuyerPay)
			{
				this.lkBtnEditPayMode.Visible = true;
				this.lkBtnEditshipingMode.Visible = true;
			}
			this.litFreight.Text = Globals.FormatMoney(this.order.AdjustedFreight);
			if (this.order.OrderStatus != OrderStatus.Finished)
			{
				if (this.order.OrderStatus != OrderStatus.SellerAlreadySent)
				{
					this.litShippingMode.Text = this.order.ModeName;
					goto IL_8D;
				}
			}
			this.litShippingMode.Text = this.order.RealModeName;
			IL_8D:
			this.litPayMode.Text = this.order.PaymentType;
			if (this.order.IsFreightFree)
			{
				this.hlkFreightFreePromotion.Text = this.order.FreightFreePromotionName;
				this.hlkFreightFreePromotion.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails", new object[]
				{
					this.order.FreightFreePromotionId
				});
			}
			this.litPayCharge.Text = Globals.FormatMoney(this.order.PayCharge);
			if (this.order.Tax > 0m)
			{
				this.litTax.Text = "<tr class=\"bg\"><td align=\"right\">税金(元)：</td><td colspan=\"2\"><span class='Name'>" + Globals.FormatMoney(this.order.Tax);
				System.Web.UI.WebControls.Literal expr_15E = this.litTax;
				expr_15E.Text += "</span></td></tr>";
			}
			if (this.order.InvoiceTitle.Length > 0)
			{
				this.litInvoiceTitle.Text = "<tr class=\"bg\"><td align=\"right\">发票抬头：</td><td colspan=\"2\"><span class='Name'>" + this.order.InvoiceTitle;
				System.Web.UI.WebControls.Literal expr_1AC = this.litInvoiceTitle;
				expr_1AC.Text += "</span></td></tr>";
			}
			if (!string.IsNullOrEmpty(this.order.CouponName))
			{
				this.litCoupon.Text = "[" + this.order.CouponName + "]-" + Globals.FormatMoney(this.order.CouponValue);
			}
			else
			{
				this.litCoupon.Text = "-" + Globals.FormatMoney(this.order.CouponValue);
			}
			this.litCouponValue.Text = "-" + Globals.FormatMoney(this.order.CouponValue);
			this.litDiscount.Text = Globals.FormatMoney(this.order.AdjustedDiscount);
			this.litPoints.Text = this.order.Points.ToString(System.Globalization.CultureInfo.InvariantCulture);
			if (this.order.IsSendTimesPoint)
			{
				this.hlkSentTimesPointPromotion.Text = this.order.SentTimesPointPromotionName;
				this.hlkSentTimesPointPromotion.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails", new object[]
				{
					this.order.SentTimesPointPromotionId
				});
			}
			this.litTotalPrice.Text = Globals.FormatMoney(this.order.GetTotal());
		}
	}
}
