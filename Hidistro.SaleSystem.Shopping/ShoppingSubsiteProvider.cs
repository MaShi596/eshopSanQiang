using Hidistro.Core;
using System;
namespace Hidistro.SaleSystem.Shopping
{
	public abstract class ShoppingSubsiteProvider : ShoppingProvider
	{
		private static readonly ShoppingSubsiteProvider _defaultInstance;
		static ShoppingSubsiteProvider()
		{
			ShoppingSubsiteProvider._defaultInstance = (DataProviders.CreateInstance("Hidistro.SaleSystem.DistributionData.ShoppingData,Hidistro.SaleSystem.DistributionData") as ShoppingSubsiteProvider);
		}
		public static ShoppingSubsiteProvider CreateInstance()
		{
			return ShoppingSubsiteProvider._defaultInstance;
		}
	}
}
