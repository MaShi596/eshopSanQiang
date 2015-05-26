using Hidistro.Core;
using System;
namespace Hidistro.SaleSystem.Shopping
{
	public abstract class ShoppingMasterProvider : ShoppingProvider
	{
		private static readonly ShoppingMasterProvider _defaultInstance;
		static ShoppingMasterProvider()
		{
			ShoppingMasterProvider._defaultInstance = (DataProviders.CreateInstance("Hidistro.SaleSystem.Data.ShoppingData,Hidistro.SaleSystem.Data") as ShoppingMasterProvider);
		}
		public static ShoppingMasterProvider CreateInstance()
		{
			return ShoppingMasterProvider._defaultInstance;
		}
	}
}
