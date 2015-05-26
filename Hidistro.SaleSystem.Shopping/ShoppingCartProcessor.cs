using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.SaleSystem.Catalog;
using System;
using System.Collections.Generic;
namespace Hidistro.SaleSystem.Shopping
{
	public static class ShoppingCartProcessor
	{
		public static ShoppingCartInfo GetShoppingCart()
		{
			Member member = HiContext.Current.User as Member;
			ShoppingCartInfo result;
			if (member != null)
			{
				ShoppingCartInfo shoppingCart = ShoppingProvider.Instance().GetShoppingCart(HiContext.Current.User.UserId);
				if (shoppingCart.LineItems.Count == 0 && shoppingCart.LineGifts.Count == 0)
				{
					result = null;
				}
				else
				{
					decimal reducedPromotionAmount = 0m;
					PromotionInfo reducedPromotion = ShoppingProvider.Instance().GetReducedPromotion(member, shoppingCart.GetAmount(), shoppingCart.GetQuantity(), out reducedPromotionAmount);
					if (reducedPromotion != null)
					{
						shoppingCart.ReducedPromotionId = reducedPromotion.ActivityId;
						shoppingCart.ReducedPromotionName = reducedPromotion.Name;
						shoppingCart.ReducedPromotionAmount = reducedPromotionAmount;
						shoppingCart.IsReduced = true;
					}
					PromotionInfo sendPromotion = ShoppingProvider.Instance().GetSendPromotion(member, shoppingCart.GetTotal(), PromoteType.FullAmountSentGift);
					if (sendPromotion != null)
					{
						shoppingCart.SendGiftPromotionId = sendPromotion.ActivityId;
						shoppingCart.SendGiftPromotionName = sendPromotion.Name;
						shoppingCart.IsSendGift = true;
					}
					PromotionInfo sendPromotion2 = ShoppingProvider.Instance().GetSendPromotion(member, shoppingCart.GetTotal(), PromoteType.FullAmountSentTimesPoint);
					if (sendPromotion2 != null)
					{
						shoppingCart.SentTimesPointPromotionId = sendPromotion2.ActivityId;
						shoppingCart.SentTimesPointPromotionName = sendPromotion2.Name;
						shoppingCart.IsSendTimesPoint = true;
						shoppingCart.TimesPoint = sendPromotion2.DiscountValue;
					}
					PromotionInfo sendPromotion3 = ShoppingProvider.Instance().GetSendPromotion(member, shoppingCart.GetTotal(), PromoteType.FullAmountSentFreight);
					if (sendPromotion3 != null)
					{
						shoppingCart.FreightFreePromotionId = sendPromotion3.ActivityId;
						shoppingCart.FreightFreePromotionName = sendPromotion3.Name;
						shoppingCart.IsFreightFree = true;
					}
					result = shoppingCart;
				}
			}
			else
			{
				result = CookieShoppingProvider.Instance().GetShoppingCart();
			}
			return result;
		}
		public static void AddLineItem(string skuId, int quantity)
		{
			Member member = HiContext.Current.User as Member;
			if (quantity <= 0)
			{
				quantity = 1;
			}
			if (member != null)
			{
				ShoppingProvider.Instance().AddLineItem(member, skuId, quantity);
			}
			else
			{
				CookieShoppingProvider.Instance().AddLineItem(skuId, quantity);
			}
		}
		public static void ConvertShoppingCartToDataBase(ShoppingCartInfo shoppingCart)
		{
			Member member = HiContext.Current.User as Member;
			if (member != null)
			{
				if (shoppingCart.LineItems.Count > 0)
				{
					foreach (ShoppingCartItemInfo current in shoppingCart.LineItems.Values)
					{
						ShoppingProvider.Instance().AddLineItem(member, current.SkuId, current.Quantity);
					}
				}
				if (shoppingCart.LineGifts.Count > 0)
				{
					foreach (ShoppingCartGiftInfo current2 in shoppingCart.LineGifts)
					{
						ShoppingProvider.Instance().AddGiftItem(current2.GiftId, current2.Quantity, (PromoteType)current2.PromoType);
					}
				}
			}
		}
		public static void RemoveLineItem(string skuId)
		{
			if (HiContext.Current.User.IsAnonymous)
			{
				CookieShoppingProvider.Instance().RemoveLineItem(skuId);
			}
			else
			{
				ShoppingProvider.Instance().RemoveLineItem(HiContext.Current.User.UserId, skuId);
			}
		}
		public static void UpdateLineItemQuantity(string skuId, int quantity)
		{
			Member member = HiContext.Current.User as Member;
			if (quantity <= 0)
			{
				ShoppingCartProcessor.RemoveLineItem(skuId);
			}
			if (member == null)
			{
				CookieShoppingProvider.Instance().UpdateLineItemQuantity(skuId, quantity);
			}
			else
			{
				ShoppingProvider.Instance().UpdateLineItemQuantity(member, skuId, quantity);
			}
		}
		public static bool AddGiftItem(int giftId, int quantity, PromoteType promotype)
		{
			bool result;
			if (HiContext.Current.User.IsAnonymous)
			{
				result = CookieShoppingProvider.Instance().AddGiftItem(giftId, quantity);
			}
			else
			{
				result = ShoppingProvider.Instance().AddGiftItem(giftId, quantity, promotype);
			}
			return result;
		}
		public static int GetGiftItemQuantity(PromoteType promotype)
		{
			return ShoppingProvider.Instance().GetGiftItemQuantity(promotype);
		}
		public static void RemoveGiftItem(int giftId, PromoteType promotype)
		{
			if (HiContext.Current.User.IsAnonymous)
			{
				CookieShoppingProvider.Instance().RemoveGiftItem(giftId);
			}
			else
			{
				ShoppingProvider.Instance().RemoveGiftItem(giftId, promotype);
			}
		}
		public static void UpdateGiftItemQuantity(int giftId, int quantity, PromoteType promotype)
		{
			Member member = HiContext.Current.User as Member;
			if (quantity <= 0)
			{
				ShoppingCartProcessor.RemoveGiftItem(giftId, promotype);
			}
			if (member == null)
			{
				CookieShoppingProvider.Instance().UpdateGiftItemQuantity(giftId, quantity);
			}
			else
			{
				ShoppingProvider.Instance().UpdateGiftItemQuantity(giftId, quantity, promotype);
			}
		}
		public static void ClearShoppingCart()
		{
			if (HiContext.Current.User.IsAnonymous)
			{
				CookieShoppingProvider.Instance().ClearShoppingCart();
			}
			else
			{
				ShoppingProvider.Instance().ClearShoppingCart(HiContext.Current.User.UserId);
			}
		}
		public static int GetSkuStock(string skuId)
		{
			return ShoppingProvider.Instance().GetSkuStock(skuId);
		}
		public static ShoppingCartInfo GetGroupBuyShoppingCart(string productSkuId, int buyAmount)
		{
			ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo();
			Member member = HiContext.Current.User as Member;
			ShoppingCartItemInfo cartItemInfo = ShoppingProvider.Instance().GetCartItemInfo(member, productSkuId, buyAmount);
			ShoppingCartInfo result;
			if (cartItemInfo == null)
			{
				result = null;
			}
			else
			{
				GroupBuyInfo productGroupBuyInfo = ProductBrowser.GetProductGroupBuyInfo(cartItemInfo.ProductId);
				if (productGroupBuyInfo == null)
				{
					result = null;
				}
				else
				{
					int orderCount = ProductBrowser.GetOrderCount(productGroupBuyInfo.GroupBuyId);
					decimal currentPrice = ProductBrowser.GetCurrentPrice(productGroupBuyInfo.GroupBuyId, orderCount);
					ShoppingCartItemInfo shoppingCartItemInfo = new ShoppingCartItemInfo();
					shoppingCartItemInfo.SkuId = cartItemInfo.SkuId;
					shoppingCartItemInfo.ProductId = cartItemInfo.ProductId;
					shoppingCartItemInfo.SKU = cartItemInfo.SKU;
					shoppingCartItemInfo.Name = cartItemInfo.Name;
					shoppingCartItemInfo.MemberPrice = (shoppingCartItemInfo.AdjustedPrice = currentPrice);
					shoppingCartItemInfo.SkuContent = cartItemInfo.SkuContent;
					ShoppingCartItemInfo arg_DC_0 = shoppingCartItemInfo;
					shoppingCartItemInfo.ShippQuantity = buyAmount;
					arg_DC_0.Quantity = buyAmount;
					shoppingCartItemInfo.Weight = cartItemInfo.Weight;
					shoppingCartItemInfo.ThumbnailUrl40 = cartItemInfo.ThumbnailUrl40;
					shoppingCartItemInfo.ThumbnailUrl60 = cartItemInfo.ThumbnailUrl60;
					shoppingCartItemInfo.ThumbnailUrl100 = cartItemInfo.ThumbnailUrl100;
					shoppingCartInfo.LineItems.Add(productSkuId, shoppingCartItemInfo);
					result = shoppingCartInfo;
				}
			}
			return result;
		}
		public static ShoppingCartInfo GetCountDownShoppingCart(string productSkuId, int buyAmount)
		{
			ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo();
			Member member = HiContext.Current.User as Member;
			ShoppingCartItemInfo cartItemInfo = ShoppingProvider.Instance().GetCartItemInfo(member, productSkuId, buyAmount);
			ShoppingCartInfo result;
			if (cartItemInfo == null)
			{
				result = null;
			}
			else
			{
				CountDownInfo countDownInfo = ProductBrowser.GetCountDownInfo(cartItemInfo.ProductId);
				if (countDownInfo == null)
				{
					result = null;
				}
				else
				{
					ShoppingCartItemInfo shoppingCartItemInfo = new ShoppingCartItemInfo();
					shoppingCartItemInfo.SkuId = cartItemInfo.SkuId;
					shoppingCartItemInfo.ProductId = cartItemInfo.ProductId;
					shoppingCartItemInfo.SKU = cartItemInfo.SKU;
					shoppingCartItemInfo.Name = cartItemInfo.Name;
					shoppingCartItemInfo.MemberPrice = (shoppingCartItemInfo.AdjustedPrice = countDownInfo.CountDownPrice);
					shoppingCartItemInfo.SkuContent = cartItemInfo.SkuContent;
					ShoppingCartItemInfo arg_C3_0 = shoppingCartItemInfo;
					shoppingCartItemInfo.ShippQuantity = buyAmount;
					arg_C3_0.Quantity = buyAmount;
					shoppingCartItemInfo.Weight = cartItemInfo.Weight;
					shoppingCartItemInfo.ThumbnailUrl40 = cartItemInfo.ThumbnailUrl40;
					shoppingCartItemInfo.ThumbnailUrl60 = cartItemInfo.ThumbnailUrl60;
					shoppingCartItemInfo.ThumbnailUrl100 = cartItemInfo.ThumbnailUrl100;
					shoppingCartInfo.LineItems.Add(productSkuId, shoppingCartItemInfo);
					result = shoppingCartInfo;
				}
			}
			return result;
		}
		public static ShoppingCartInfo GetShoppingCart(string productSkuId, int buyAmount)
		{
			ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo();
			Member member = HiContext.Current.User as Member;
			ShoppingCartItemInfo cartItemInfo = ShoppingProvider.Instance().GetCartItemInfo(member, productSkuId, buyAmount);
			ShoppingCartInfo result;
			if (cartItemInfo == null)
			{
				result = null;
			}
			else
			{
				shoppingCartInfo.LineItems.Add(productSkuId, cartItemInfo);
				if (member != null)
				{
					decimal reducedPromotionAmount = 0m;
					PromotionInfo reducedPromotion = ShoppingProvider.Instance().GetReducedPromotion(member, shoppingCartInfo.GetAmount(), shoppingCartInfo.GetQuantity(), out reducedPromotionAmount);
					if (reducedPromotion != null)
					{
						shoppingCartInfo.ReducedPromotionId = reducedPromotion.ActivityId;
						shoppingCartInfo.ReducedPromotionName = reducedPromotion.Name;
						shoppingCartInfo.ReducedPromotionAmount = reducedPromotionAmount;
						shoppingCartInfo.IsReduced = true;
					}
					PromotionInfo sendPromotion = ShoppingProvider.Instance().GetSendPromotion(member, shoppingCartInfo.GetTotal(), PromoteType.FullAmountSentGift);
					if (sendPromotion != null)
					{
						shoppingCartInfo.SendGiftPromotionId = sendPromotion.ActivityId;
						shoppingCartInfo.SendGiftPromotionName = sendPromotion.Name;
						shoppingCartInfo.IsSendGift = true;
					}
					PromotionInfo sendPromotion2 = ShoppingProvider.Instance().GetSendPromotion(member, shoppingCartInfo.GetTotal(), PromoteType.FullAmountSentTimesPoint);
					if (sendPromotion2 != null)
					{
						shoppingCartInfo.SentTimesPointPromotionId = sendPromotion2.ActivityId;
						shoppingCartInfo.SentTimesPointPromotionName = sendPromotion2.Name;
						shoppingCartInfo.IsSendTimesPoint = true;
						shoppingCartInfo.TimesPoint = sendPromotion2.DiscountValue;
					}
					PromotionInfo sendPromotion3 = ShoppingProvider.Instance().GetSendPromotion(member, shoppingCartInfo.GetTotal(), PromoteType.FullAmountSentFreight);
					if (sendPromotion3 != null)
					{
						shoppingCartInfo.FreightFreePromotionId = sendPromotion3.ActivityId;
						shoppingCartInfo.FreightFreePromotionName = sendPromotion3.Name;
						shoppingCartInfo.IsFreightFree = true;
					}
				}
				result = shoppingCartInfo;
			}
			return result;
		}
		public static ShoppingCartInfo GetShoppingCart(int Boundlingid, int buyAmount)
		{
			ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo();
			List<BundlingItemInfo> bundlingItemsByID = ProductBrowser.GetBundlingItemsByID(Boundlingid);
			Member member = HiContext.Current.User as Member;
			foreach (BundlingItemInfo current in bundlingItemsByID)
			{
				ShoppingCartItemInfo cartItemInfo = ShoppingProvider.Instance().GetCartItemInfo(member, current.SkuId, buyAmount * current.ProductNum);
				if (cartItemInfo != null)
				{
					shoppingCartInfo.LineItems.Add(current.SkuId, cartItemInfo);
				}
			}
			return shoppingCartInfo;
		}
	}
}
