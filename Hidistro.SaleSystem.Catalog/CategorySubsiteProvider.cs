using Hidistro.Core;
using System;
namespace Hidistro.SaleSystem.Catalog
{
	public abstract class CategorySubsiteProvider : CategoryProvider
	{
		private static readonly CategorySubsiteProvider _defaultInstance;
		static CategorySubsiteProvider()
		{
			CategorySubsiteProvider._defaultInstance = (DataProviders.CreateInstance("Hidistro.SaleSystem.DistributionData.CategoryData,Hidistro.SaleSystem.DistributionData") as CategorySubsiteProvider);
		}
		public static CategorySubsiteProvider CreateInstance()
		{
			return CategorySubsiteProvider._defaultInstance;
		}
	}
}
