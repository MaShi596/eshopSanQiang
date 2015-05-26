using Hidistro.Entities.Commodities;
using Hidistro.Membership.Context;
using System;
using System.Collections.Generic;
using System.Data;
namespace Hidistro.SaleSystem.Catalog
{
	public abstract class CategoryProvider
	{
		public static CategoryProvider Instance()
		{
			CategoryProvider result;
			if (HiContext.Current.SiteSettings.IsDistributorSettings)
			{
				result = CategorySubsiteProvider.CreateInstance();
			}
			else
			{
				result = CategoryMasterProvider.CreateInstance();
			}
			return result;
		}
		public abstract DataTable GetCategories();
		public abstract DataSet GetThreeLayerCategories();
		public abstract CategoryInfo GetCategory(int categoryId);
		public abstract DataTable GetBrandCategories(int categoryId, int maxNum);
		public abstract BrandCategoryInfo GetBrandCategory(int brandId);
		public abstract IList<AttributeInfo> GetAttributeInfoByCategoryId(int categoryId, int maxNum);
	}
}
