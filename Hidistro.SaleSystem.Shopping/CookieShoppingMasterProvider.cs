using Hidistro.Core;
using System;
namespace Hidistro.SaleSystem.Shopping
{
	public abstract class CookieShoppingMasterProvider : CookieShoppingProvider
	{
		private static readonly CookieShoppingMasterProvider _defaultInstance;
		static CookieShoppingMasterProvider()
		{
			CookieShoppingMasterProvider._defaultInstance = (DataProviders.CreateInstance("Hidistro.SaleSystem.Data.CookieShoppingData,Hidistro.SaleSystem.Data") as CookieShoppingMasterProvider);
		}
		public static CookieShoppingMasterProvider CreateInstance()
		{
			return CookieShoppingMasterProvider._defaultInstance;
		}
	}
}
