using Hidistro.Entities.Commodities;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using System;
using System.Collections.Generic;
namespace Hidistro.SaleSystem.Shopping
{
	public abstract class CookieShoppingProvider
	{
		public static CookieShoppingProvider Instance()
		{
			CookieShoppingProvider result;
			if (HiContext.Current.SiteSettings.IsDistributorSettings)
			{
				result = CookieShoppingSubsiteProvider.CreateInstance();
			}
			else
			{
				result = CookieShoppingMasterProvider.CreateInstance();
			}
			return result;
		}
		public abstract ShoppingCartInfo GetShoppingCart();
		public abstract void AddLineItem(string skuId, int quantity);
		public abstract void RemoveLineItem(string skuId);
		public abstract void UpdateLineItemQuantity(string skuId, int quantity);
		public abstract bool AddGiftItem(int giftId, int quantity);
		public abstract void UpdateGiftItemQuantity(int giftId, int quantity);
		public abstract void RemoveGiftItem(int giftId);
		public abstract void ClearShoppingCart();
		public abstract Dictionary<string, decimal> GetCostPriceForItems(int userId);
		public abstract bool GetShoppingProductInfo(int productId, string skuId, out ProductSaleStatus saleStatus, out int stock, out int totalQuantity);
		public abstract decimal GetCostPrice(string skuId);
		public abstract int GetSkuStock(string skuId);
	}
}
