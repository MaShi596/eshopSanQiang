using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using System;
using System.Collections.Generic;
using System.Data;
namespace Hidistro.SaleSystem.Shopping
{
	public static class ShoppingProcessor
	{
		public static IList<string> GetSkuIdsBysku(string string_0)
		{
			return ShoppingProvider.Instance().GetSkuIdsBysku(string_0);
		}
		public static DataTable GetProductInfoBySku(string skuId)
		{
			return ShoppingProvider.Instance().GetProductInfoBySku(skuId);
		}
		public static SKUItem GetProductAndSku(int productId, string options)
		{
			return ShoppingProvider.Instance().GetProductAndSku(productId, options);
		}
		public static DataTable GetUnUpUnUpsellingSkus(int productId, int attributeId, int valueId)
		{
			return ShoppingProvider.Instance().GetUnUpUnUpsellingSkus(productId, attributeId, valueId);
		}
		public static IList<ShippingModeInfo> GetShippingModes()
		{
			return ShoppingProvider.Instance().GetShippingModes();
		}
		public static ShippingModeInfo GetShippingMode(int modeId, bool includeDetail)
		{
			return ShoppingProvider.Instance().GetShippingMode(modeId, includeDetail);
		}
		public static IList<string> GetExpressCompanysByMode(int modeId)
		{
			return ShoppingProvider.Instance().GetExpressCompanysByMode(modeId);
		}
		public static IList<PaymentModeInfo> GetPaymentModes()
		{
			return ShoppingProvider.Instance().GetPaymentModes();
		}
		public static PaymentModeInfo GetPaymentMode(int modeId)
		{
			return ShoppingProvider.Instance().GetPaymentMode(modeId);
		}
		public static OrderInfo ConvertShoppingCartToOrder(ShoppingCartInfo shoppingCart, bool isGroupBuy, bool isCountDown, bool isSignBuy)
		{
			OrderInfo result;
			if (shoppingCart.LineItems.Count == 0 && shoppingCart.LineGifts.Count == 0)
			{
				result = null;
			}
			else
			{
				OrderInfo orderInfo = new OrderInfo();
				orderInfo.Points = shoppingCart.GetPoint();
				orderInfo.ReducedPromotionId = shoppingCart.ReducedPromotionId;
				orderInfo.ReducedPromotionName = shoppingCart.ReducedPromotionName;
				orderInfo.ReducedPromotionAmount = shoppingCart.ReducedPromotionAmount;
				orderInfo.IsReduced = shoppingCart.IsReduced;
				orderInfo.SentTimesPointPromotionId = shoppingCart.SentTimesPointPromotionId;
				orderInfo.SentTimesPointPromotionName = shoppingCart.SentTimesPointPromotionName;
				orderInfo.IsSendTimesPoint = shoppingCart.IsSendTimesPoint;
				orderInfo.TimesPoint = shoppingCart.TimesPoint;
				orderInfo.FreightFreePromotionId = shoppingCart.FreightFreePromotionId;
				orderInfo.FreightFreePromotionName = shoppingCart.FreightFreePromotionName;
				orderInfo.IsFreightFree = shoppingCart.IsFreightFree;
				string text = string.Empty;
				if (shoppingCart.LineItems.Values.Count > 0)
				{
					foreach (ShoppingCartItemInfo current in shoppingCart.LineItems.Values)
					{
						text += string.Format("'{0}',", current.SkuId);
					}
				}
				Dictionary<string, decimal> dictionary = new Dictionary<string, decimal>();
				if (!string.IsNullOrEmpty(text))
				{
					text = text.Substring(0, text.Length - 1);
					dictionary = ShoppingProvider.Instance().GetCostPriceForItems(text);
				}
				if (shoppingCart.LineItems.Values.Count > 0)
				{
					foreach (ShoppingCartItemInfo current in shoppingCart.LineItems.Values)
					{
						decimal itemCostPrice = 0m;
						if (isGroupBuy || isCountDown || isSignBuy)
						{
							itemCostPrice = ShoppingProvider.Instance().GetCostPrice(current.SkuId);
						}
						else
						{
							if (dictionary.ContainsKey(current.SkuId))
							{
								itemCostPrice = dictionary[current.SkuId];
							}
						}
						LineItemInfo lineItemInfo = new LineItemInfo();
						lineItemInfo.SkuId = current.SkuId;
						lineItemInfo.ProductId = current.ProductId;
						lineItemInfo.SKU = current.SKU;
						lineItemInfo.Quantity = current.Quantity;
						lineItemInfo.ShipmentQuantity = current.ShippQuantity;
						lineItemInfo.ItemCostPrice = itemCostPrice;
						lineItemInfo.ItemListPrice = current.MemberPrice;
						lineItemInfo.ItemAdjustedPrice = current.AdjustedPrice;
						lineItemInfo.ItemDescription = current.Name;
						lineItemInfo.ThumbnailsUrl = current.ThumbnailUrl40;
						lineItemInfo.ItemWeight = current.Weight;
						lineItemInfo.SKUContent = current.SkuContent;
						lineItemInfo.PromotionId = current.PromotionId;
						lineItemInfo.PromotionName = current.PromotionName;
						orderInfo.LineItems.Add(lineItemInfo.SkuId, lineItemInfo);
					}
				}
				orderInfo.Tax = 0.00m;
				orderInfo.InvoiceTitle = "";
				if (shoppingCart.LineGifts.Count > 0)
				{
					foreach (ShoppingCartGiftInfo current2 in shoppingCart.LineGifts)
					{
						OrderGiftInfo orderGiftInfo = new OrderGiftInfo();
						orderGiftInfo.GiftId = current2.GiftId;
						orderGiftInfo.GiftName = current2.Name;
						orderGiftInfo.Quantity = current2.Quantity;
						orderGiftInfo.ThumbnailsUrl = current2.ThumbnailUrl40;
						orderGiftInfo.PromoteType = current2.PromoType;
						if (HiContext.Current.SiteSettings.IsDistributorSettings)
						{
							orderGiftInfo.CostPrice = current2.PurchasePrice;
						}
						else
						{
							orderGiftInfo.CostPrice = current2.CostPrice;
						}
						orderInfo.Gifts.Add(orderGiftInfo);
					}
				}
				result = orderInfo;
			}
			return result;
		}
		public static bool CreatOrder(OrderInfo orderInfo)
		{
			return orderInfo != null && ShoppingProvider.Instance().CreatOrder(orderInfo);
		}
		public static bool CutNeedPoint(int needPoint, string orderId)
		{
			Member member = HiContext.Current.User as Member;
			UserPointInfo userPointInfo = new UserPointInfo();
			userPointInfo.OrderId = orderId;
			userPointInfo.UserId = member.UserId;
			userPointInfo.TradeDate = DateTime.Now;
			userPointInfo.TradeType = UserPointTradeType.Change;
			userPointInfo.Reduced = new int?(needPoint);
			userPointInfo.Points = member.Points - needPoint;
			int arg_5B_0 = userPointInfo.Points;
			if (userPointInfo.Points < 0)
			{
				userPointInfo.Points = 0;
			}
			bool result;
			if (ShoppingProvider.Instance().AddMemberPoint(userPointInfo))
			{
				Users.ClearUserCache(member);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}
		public static int CountDownOrderCount(int productid)
		{
			return ShoppingProvider.Instance().CountDownOrderCount(productid);
		}
		public static OrderInfo GetOrderInfo(string orderId)
		{
			return ShoppingProvider.Instance().GetOrderInfo(orderId);
		}
		public static DataTable GetYetShipOrders(int days)
		{
			return ShoppingProvider.Instance().GetYetShipOrders(days);
		}
		public static PurchaseOrderInfo GetPurchaseOrder(string purchaseOrderId)
		{
			return ShoppingProvider.Instance().GetPurchaseOrder(purchaseOrderId);
		}
		public static CouponInfo GetCoupon(string couponCode)
		{
			return ShoppingProvider.Instance().GetCoupon(couponCode);
		}
		public static DataTable GetCoupon(decimal orderAmount)
		{
			return ShoppingProvider.Instance().GetCoupon(orderAmount);
		}
		public static CouponInfo UseCoupon(decimal orderAmount, string claimCode)
		{
			CouponInfo result;
			if (string.IsNullOrEmpty(claimCode))
			{
				result = null;
			}
			else
			{
				CouponInfo coupon = ShoppingProcessor.GetCoupon(claimCode);
				if (coupon != null && ((coupon.Amount.HasValue && coupon.Amount > 0m && orderAmount >= coupon.Amount.Value) || ((!coupon.Amount.HasValue || coupon.Amount == 0m) && orderAmount > coupon.DiscountValue)))
				{
					result = coupon;
				}
				else
				{
					result = null;
				}
			}
			return result;
		}
		public static DataTable UseCoupon(decimal orderAmount)
		{
			return ShoppingProcessor.GetCoupon(orderAmount);
		}
		public static decimal CalcFreight(int regionId, decimal totalWeight, ShippingModeInfo shippingModeInfo)
		{
			decimal result = 0m;
			int topRegionId = RegionHelper.GetTopRegionId(regionId);
			int value = 1;
			if (totalWeight > shippingModeInfo.Weight && shippingModeInfo.AddWeight.HasValue && shippingModeInfo.AddWeight.Value > 0m)
			{
				if ((totalWeight - shippingModeInfo.Weight) % shippingModeInfo.AddWeight == 0m)
				{
					value = Convert.ToInt32(Math.Truncate((totalWeight - shippingModeInfo.Weight) / shippingModeInfo.AddWeight.Value));
				}
				else
				{
					value = Convert.ToInt32(Math.Truncate((totalWeight - shippingModeInfo.Weight) / shippingModeInfo.AddWeight.Value)) + 1;
				}
			}
			if (shippingModeInfo.ModeGroup == null || shippingModeInfo.ModeGroup.Count == 0)
			{
				if (totalWeight > shippingModeInfo.Weight && shippingModeInfo.AddPrice.HasValue)
				{
					result = value * shippingModeInfo.AddPrice.Value + shippingModeInfo.Price;
				}
				else
				{
					result = shippingModeInfo.Price;
				}
			}
			else
			{
				int? num = null;
				foreach (ShippingModeGroupInfo current in shippingModeInfo.ModeGroup)
				{
					foreach (ShippingRegionInfo current2 in current.ModeRegions)
					{
						if (topRegionId == current2.RegionId)
						{
							num = new int?(current2.GroupId);
							break;
						}
					}
					if (num.HasValue)
					{
						if (totalWeight > shippingModeInfo.Weight)
						{
							result = value * current.AddPrice + current.Price;
						}
						else
						{
							result = current.Price;
						}
						break;
					}
				}
				if (!num.HasValue)
				{
					if (totalWeight > shippingModeInfo.Weight && shippingModeInfo.AddPrice.HasValue)
					{
						result = value * shippingModeInfo.AddPrice.Value + shippingModeInfo.Price;
					}
					else
					{
						result = shippingModeInfo.Price;
					}
				}
			}
			return result;
		}
		public static decimal CalcPayCharge(decimal cartMoney, PaymentModeInfo paymentModeInfo)
		{
			decimal result;
			if (!paymentModeInfo.IsPercent)
			{
				result = paymentModeInfo.Charge;
			}
			else
			{
				result = cartMoney * (paymentModeInfo.Charge / 100m);
			}
			return result;
		}
		public static int GetStock(int productId, string skuId)
		{
			return ShoppingProvider.Instance().GetStock(productId, skuId);
		}
	}
}
