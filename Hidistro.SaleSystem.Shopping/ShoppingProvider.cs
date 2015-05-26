using Hidistro.Entities.Commodities;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using System;
using System.Collections.Generic;
using System.Data;
namespace Hidistro.SaleSystem.Shopping
{
	public abstract class ShoppingProvider
	{
		public static ShoppingProvider Instance()
		{
			ShoppingProvider result;
			if (HiContext.Current.SiteSettings.IsDistributorSettings)
			{
				result = ShoppingSubsiteProvider.CreateInstance();
			}
			else
			{
				result = ShoppingMasterProvider.CreateInstance();
			}
			return result;
		}
		public abstract IList<string> GetSkuIdsBysku(string string_0);
		public abstract DataTable GetProductInfoBySku(string skuId);
		public abstract SKUItem GetProductAndSku(int productId, string options);
		public abstract DataTable GetUnUpUnUpsellingSkus(int productId, int attributeId, int valueId);
		public abstract ShoppingCartInfo GetShoppingCart(int userId);
		public abstract PromotionInfo GetReducedPromotion(Member member, decimal amount, int quantity, out decimal reducedAmount);
		public abstract PromotionInfo GetSendPromotion(Member member, decimal amount, PromoteType promoteType);
		public abstract void AddLineItem(Member member, string skuId, int quantity);
		public abstract void RemoveLineItem(int userId, string skuId);
		public abstract void UpdateLineItemQuantity(Member member, string skuId, int quantity);
		public abstract bool AddGiftItem(int giftId, int quantity, PromoteType promotype);
		public abstract void UpdateGiftItemQuantity(int giftId, int quantity, PromoteType promotype);
		public abstract void RemoveGiftItem(int giftId, PromoteType promotype);
		public abstract int GetGiftItemQuantity(PromoteType promotype);
		public abstract void ClearShoppingCart(int userId);
		public abstract ShoppingCartItemInfo GetCartItemInfo(Member member, string skuId, int quantity);
		public abstract Dictionary<string, decimal> GetCostPriceForItems(string skuIds);
		public abstract decimal GetCostPrice(string skuId);
		public abstract int GetSkuStock(string skuId);
		public abstract IList<ShippingModeInfo> GetShippingModes();
		public abstract ShippingModeInfo GetShippingMode(int modeId, bool includeDetail);
		public abstract IList<string> GetExpressCompanysByMode(int modeId);
		public abstract IList<PaymentModeInfo> GetPaymentModes();
		public abstract PaymentModeInfo GetPaymentMode(int modeId);
		public abstract CouponInfo GetCoupon(string couponCode);
		public abstract DataTable GetCoupon(decimal orderAmount);
		public abstract bool CreatOrder(OrderInfo orderInfo);
		public abstract bool AddMemberPoint(UserPointInfo point);
		public abstract OrderInfo GetOrderInfo(string orderId);
		public abstract DataTable GetYetShipOrders(int days);
		public abstract PurchaseOrderInfo GetPurchaseOrder(string purchaseOrderId);
		public abstract int GetStock(int productId, string skuId);
		public abstract int CountDownOrderCount(int productid);
	}
}
