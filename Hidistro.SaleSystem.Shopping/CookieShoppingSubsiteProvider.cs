using Hidistro.Core;
using System;
namespace Hidistro.SaleSystem.Shopping
{
	public abstract class CookieShoppingSubsiteProvider : CookieShoppingProvider
	{
		private static readonly CookieShoppingSubsiteProvider _defaultInstance;
		static CookieShoppingSubsiteProvider()
		{
			CookieShoppingSubsiteProvider._defaultInstance = (DataProviders.CreateInstance("Hidistro.SaleSystem.DistributionData.CookieShoppingData,Hidistro.SaleSystem.DistributionData") as CookieShoppingSubsiteProvider);
		}
		public static CookieShoppingSubsiteProvider CreateInstance()
		{
			return CookieShoppingSubsiteProvider._defaultInstance;
		}
	}
}
