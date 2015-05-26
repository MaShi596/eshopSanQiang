using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Membership.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web.Caching;
namespace Hidistro.SaleSystem.Catalog
{
	public static class CategoryBrowser
	{
		private const string CategoriesCachekey = "DataCache-SubsiteCategories{0}";
		private const string MainCategoriesCachekey = "DataCache-Categories";
		public static IList<CategoryInfo> GetMaxSubCategories(int parentCategoryId, int maxNum = 1000)
		{
			IList<CategoryInfo> list = new List<CategoryInfo>();
			DataTable categories = CategoryBrowser.GetCategories();
			DataRow[] array = categories.Select("ParentCategoryId = " + parentCategoryId);
			int num = 0;
			while (num < maxNum && num < array.Length)
			{
				list.Add(DataMapper.ConvertDataRowToProductCategory(array[num]));
				num++;
			}
			return list;
		}
		public static IList<CategoryInfo> GetMaxMainCategories(int maxNum = 1000)
		{
			IList<CategoryInfo> list = new List<CategoryInfo>();
			DataTable categories = CategoryBrowser.GetCategories();
			DataRow[] array = categories.Select("Depth = 1");
			int num = 0;
			while (num < maxNum && num < array.Length)
			{
				list.Add(DataMapper.ConvertDataRowToProductCategory(array[num]));
				num++;
			}
			return list;
		}
		public static IList<CategoryInfo> GetSequenceCategories()
		{
			IList<CategoryInfo> list = new List<CategoryInfo>();
			IList<CategoryInfo> mainCategories = CategoryBrowser.GetMainCategories();
			foreach (CategoryInfo current in mainCategories)
			{
				list.Add(current);
				CategoryBrowser.LoadSubCategorys(current.CategoryId, list);
			}
			return list;
		}
		public static IList<CategoryInfo> GetMainCategories()
		{
			IList<CategoryInfo> list = new List<CategoryInfo>();
			DataTable categories = CategoryBrowser.GetCategories();
			DataRow[] array = categories.Select("Depth = 1");
			for (int i = 0; i < array.Length; i++)
			{
				list.Add(DataMapper.ConvertDataRowToProductCategory(array[i]));
			}
			return list;
		}
		private static void LoadSubCategorys(int parentCategoryId, IList<CategoryInfo> categories)
		{
			IList<CategoryInfo> subCategories = CategoryBrowser.GetSubCategories(parentCategoryId);
			if (subCategories != null && subCategories.Count > 0)
			{
				foreach (CategoryInfo current in subCategories)
				{
					categories.Add(current);
					CategoryBrowser.LoadSubCategorys(current.CategoryId, categories);
				}
			}
		}
		public static IList<CategoryInfo> GetSubCategories(int parentCategoryId)
		{
			IList<CategoryInfo> list = new List<CategoryInfo>();
			string filterExpression = "ParentCategoryId = " + parentCategoryId.ToString(CultureInfo.InvariantCulture);
			DataTable categories = CategoryBrowser.GetCategories();
			DataRow[] array = categories.Select(filterExpression);
			for (int i = 0; i < array.Length; i++)
			{
				list.Add(DataMapper.ConvertDataRowToProductCategory(array[i]));
			}
			return list;
		}
		public static DataTable GetCategories()
		{
			DataTable dataTable;
			if (HiContext.Current.SiteSettings.IsDistributorSettings)
			{
				dataTable = (HiCache.Get(string.Format("DataCache-SubsiteCategories{0}", HiContext.Current.SiteSettings.UserId.Value)) as DataTable);
			}
			else
			{
				dataTable = (HiCache.Get("DataCache-Categories") as DataTable);
			}
			if (dataTable == null)
			{
				dataTable = CategoryProvider.Instance().GetCategories();
				if (HiContext.Current.SiteSettings.IsDistributorSettings)
				{
					HiCache.Insert(string.Format("DataCache-SubsiteCategories{0}", HiContext.Current.SiteSettings.UserId.Value), dataTable, 360, CacheItemPriority.Normal);
				}
				else
				{
					HiCache.Insert("DataCache-Categories", dataTable, 360, CacheItemPriority.Normal);
				}
			}
			return dataTable;
		}
		public static CategoryInfo GetCategory(int categoryId)
		{
			return CategoryProvider.Instance().GetCategory(categoryId);
		}
		public static DataSet GetThreeLayerCategories()
		{
			return CategoryProvider.Instance().GetThreeLayerCategories();
		}
		public static DataTable GetBrandCategories(int categoryId, int maxNum = 1000)
		{
			return CategoryProvider.Instance().GetBrandCategories(categoryId, maxNum);
		}
		public static BrandCategoryInfo GetBrandCategory(int brandId)
		{
			return CategoryProvider.Instance().GetBrandCategory(brandId);
		}
		public static IList<AttributeInfo> GetAttributeInfoByCategoryId(int categoryId, int maxNum = 1000)
		{
			return CategoryProvider.Instance().GetAttributeInfoByCategoryId(categoryId, maxNum);
		}
	}
}
