using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using System;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class Order_ItemsList : System.Web.UI.UserControl
	{
		private OrderInfo order;
		protected System.Web.UI.WebControls.DataList dlstOrderItems;
		protected System.Web.UI.WebControls.Literal litWeight;
		protected System.Web.UI.WebControls.Literal lblAmoutPrice;
		protected System.Web.UI.WebControls.HyperLink hlkReducedPromotion;
		protected FormatedMoneyLabel lblTotalPrice;
		protected System.Web.UI.WebControls.Literal lblBundlingPrice;
		protected System.Web.UI.WebControls.Label lblOrderGifts;
		protected System.Web.UI.WebControls.DataList grdOrderGift;
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
			this.dlstOrderItems.ItemDataBound += new System.Web.UI.WebControls.DataListItemEventHandler(this.dlstOrderItems_ItemDataBound);
			this.dlstOrderItems.DataSource = this.order.LineItems.Values;
			this.dlstOrderItems.DataBind();
			if (this.order.Gifts.Count == 0)
			{
				this.grdOrderGift.Visible = false;
				this.lblOrderGifts.Visible = false;
			}
			else
			{
				this.grdOrderGift.DataSource = this.order.Gifts;
				this.grdOrderGift.DataBind();
			}
			this.litWeight.Text = this.order.Weight.ToString(System.Globalization.CultureInfo.InvariantCulture);
			if (this.order.IsReduced)
			{
				this.lblAmoutPrice.Text = string.Format("商品金额：{0}", Globals.FormatMoney(this.order.GetAmount()));
				this.hlkReducedPromotion.Text = this.order.ReducedPromotionName + string.Format(" 优惠：{0}", Globals.FormatMoney(this.order.ReducedPromotionAmount));
				this.hlkReducedPromotion.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails", new object[]
				{
					this.order.ReducedPromotionId
				});
			}
			if (this.order.BundlingID > 0)
			{
				this.lblBundlingPrice.Text = string.Format("<span style=\"color:Red;\">捆绑价格：{0}</span>", Globals.FormatMoney(this.order.BundlingPrice));
			}
			this.lblTotalPrice.Money = this.order.GetAmount() - this.order.ReducedPromotionAmount;
		}
		private void dlstOrderItems_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
		{
			Hidistro.Membership.Context.SettingsManager.GetSiteSettings(Hidistro.Membership.Context.HiContext.Current.User.UserId);
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				System.Web.UI.WebControls.HyperLink arg_46_0 = (System.Web.UI.WebControls.HyperLink)e.Item.FindControl("hpkBuyToSend");
				System.Web.UI.WebControls.HyperLink arg_5C_0 = (System.Web.UI.WebControls.HyperLink)e.Item.FindControl("hpkBuyDiscount");
				System.Web.UI.WebControls.Literal arg_72_0 = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litPurchaseGiftId");
				System.Web.UI.WebControls.Literal arg_88_0 = (System.Web.UI.WebControls.Literal)e.Item.FindControl("litWholesaleDiscountId");
			}
		}
	}
}
