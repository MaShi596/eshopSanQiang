using Hidistro.Core;
using System;
namespace Hidistro.SaleSystem.Catalog
{
	public abstract class CategoryMasterProvider : CategoryProvider
	{
		private static readonly CategoryMasterProvider _defaultInstance;
		static CategoryMasterProvider()
		{
			CategoryMasterProvider._defaultInstance = (DataProviders.CreateInstance("Hidistro.SaleSystem.Data.CategoryData,Hidistro.SaleSystem.Data") as CategoryMasterProvider);
		}
		public static CategoryMasterProvider CreateInstance()
		{
			return CategoryMasterProvider._defaultInstance;
		}
	}
}
