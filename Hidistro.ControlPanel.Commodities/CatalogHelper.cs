using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Caching;
namespace Hidistro.ControlPanel.Commodities
{
	public sealed class CatalogHelper
	{
		private const string CategoriesCachekey = "DataCache-Categories";
		private CatalogHelper()
		{
		}
		public static IList<CategoryInfo> GetMainCategories()
		{
			IList<CategoryInfo> list = new List<CategoryInfo>();
			System.Data.DataTable categories = CatalogHelper.GetCategories();
			System.Data.DataRow[] array = categories.Select("Depth = 1");
			for (int i = 0; i < array.Length; i++)
			{
				list.Add(DataMapper.ConvertDataRowToProductCategory(array[i]));
			}
			return list;
		}
		public static IList<CategoryInfo> GetSubCategories(int parentCategoryId)
		{
			IList<CategoryInfo> list = new List<CategoryInfo>();
			string filterExpression = "ParentCategoryId = " + parentCategoryId.ToString(CultureInfo.InvariantCulture);
			System.Data.DataTable categories = CatalogHelper.GetCategories();
			System.Data.DataRow[] array = categories.Select(filterExpression);
			for (int i = 0; i < array.Length; i++)
			{
				list.Add(DataMapper.ConvertDataRowToProductCategory(array[i]));
			}
			return list;
		}
		public static CategoryInfo GetCategory(int categoryId)
		{
			return ProductProvider.Instance().GetCategory(categoryId);
		}
		public static string GetFullCategory(int categoryId)
		{
			CategoryInfo category = CatalogHelper.GetCategory(categoryId);
			string result;
			if (category == null)
			{
				result = null;
			}
			else
			{
				string text = category.Name;
				while (category != null && category.ParentCategoryId.HasValue)
				{
					category = CatalogHelper.GetCategory(category.ParentCategoryId.Value);
					if (category != null)
					{
						text = category.Name + " &raquo; " + text;
					}
				}
				result = text;
			}
			return result;
		}
		private static System.Data.DataTable GetCategories()
		{
			System.Data.DataTable dataTable = HiCache.Get("DataCache-Categories") as System.Data.DataTable;
			if (null == dataTable)
			{
				dataTable = ProductProvider.Instance().GetCategories();
				HiCache.Insert("DataCache-Categories", dataTable, 360, CacheItemPriority.Normal);
			}
			return dataTable;
		}
		public static IList<CategoryInfo> GetSequenceCategories()
		{
			IList<CategoryInfo> list = new List<CategoryInfo>();
			IList<CategoryInfo> mainCategories = CatalogHelper.GetMainCategories();
			foreach (CategoryInfo current in mainCategories)
			{
				list.Add(current);
				CatalogHelper.LoadSubCategorys(current.CategoryId, list);
			}
			return list;
		}
		private static void LoadSubCategorys(int parentCategoryId, IList<CategoryInfo> categories)
		{
			IList<CategoryInfo> subCategories = CatalogHelper.GetSubCategories(parentCategoryId);
			if (subCategories != null && subCategories.Count > 0)
			{
				foreach (CategoryInfo current in subCategories)
				{
					categories.Add(current);
					CatalogHelper.LoadSubCategorys(current.CategoryId, categories);
				}
			}
		}
		public static CategoryActionStatus AddCategory(CategoryInfo category)
		{
			CategoryActionStatus result;
			if (null == category)
			{
				result = CategoryActionStatus.UnknowError;
			}
			else
			{
				Globals.EntityCoding(category, true);
				int num = ProductProvider.Instance().CreateCategory(category);
				if (num > 0)
				{
					EventLogs.WriteOperationLog(Privilege.AddProductCategory, string.Format(CultureInfo.InvariantCulture, "创建了一个新的店铺分类:”{0}”", new object[]
					{
						category.Name
					}));
					HiCache.Remove("DataCache-Categories");
				}
				result = CategoryActionStatus.Success;
			}
			return result;
		}
		public static CategoryActionStatus UpdateCategory(CategoryInfo category)
		{
			CategoryActionStatus result;
			if (null == category)
			{
				result = CategoryActionStatus.UnknowError;
			}
			else
			{
				Globals.EntityCoding(category, true);
				CategoryActionStatus categoryActionStatus = ProductProvider.Instance().UpdateCategory(category);
				if (categoryActionStatus == CategoryActionStatus.Success)
				{
					EventLogs.WriteOperationLog(Privilege.EditProductCategory, string.Format(CultureInfo.InvariantCulture, "修改了编号为 “{0}” 的店铺分类", new object[]
					{
						category.CategoryId
					}));
					HiCache.Remove("DataCache-Categories");
				}
				result = categoryActionStatus;
			}
			return result;
		}
		public static bool SwapCategorySequence(int categoryId, int displaysequence)
		{
			return ProductProvider.Instance().SwapCategorySequence(categoryId, displaysequence);
		}
		public static string UploadCategoryIcon(HttpPostedFile postedFile)
		{
			string result;
			if (!ResourcesHelper.CheckPostedFile(postedFile))
			{
				result = string.Empty;
			}
			else
			{
				string text = HiContext.Current.GetStoragePath() + "/CateGory/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
				postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + text));
				result = text;
			}
			return result;
		}
		public static bool DeleteCategory(int categoryId)
		{
			ManagerHelper.CheckPrivilege(Privilege.DeleteProductCategory);
			bool result;
			if (result = ProductProvider.Instance().DeleteCategory(categoryId))
			{
				EventLogs.WriteOperationLog(Privilege.DeleteProductCategory, string.Format(CultureInfo.InvariantCulture, "删除了编号为 “{0}” 的店铺分类", new object[]
				{
					categoryId
				}));
				HiCache.Remove("DataCache-Categories");
			}
			return result;
		}
		public static int DisplaceCategory(int oldCategoryId, int newCategory)
		{
			return ProductProvider.Instance().DisplaceCategory(oldCategoryId, newCategory);
		}
		public static bool SetProductExtendCategory(int productId, string extendCategoryPath)
		{
			return ProductProvider.Instance().SetProductExtendCategory(productId, extendCategoryPath);
		}
		public static bool SetCategoryThemes(int categoryId, string themeName)
		{
			if (ProductProvider.Instance().SetCategoryThemes(categoryId, themeName))
			{
				HiCache.Remove("DataCache-Categories");
			}
			return false;
		}
		public static bool AddBrandCategory(BrandCategoryInfo brandCategory)
		{
			int num = ProductProvider.Instance().AddBrandCategory(brandCategory);
			bool result;
			if (num <= 0)
			{
				result = false;
			}
			else
			{
				if (brandCategory.ProductTypes.Count > 0)
				{
					ProductProvider.Instance().AddBrandProductTypes(num, brandCategory.ProductTypes);
				}
				result = true;
			}
			return result;
		}
		public static System.Data.DataTable GetBrandCategories()
		{
			return ProductProvider.Instance().GetBrandCategories();
		}
		public static BrandCategoryInfo GetBrandCategory(int brandId)
		{
			return ProductProvider.Instance().GetBrandCategory(brandId);
		}
		public static bool UpdateBrandCategory(BrandCategoryInfo brandCategory)
		{
			bool result;
			if ((result = ProductProvider.Instance().UpdateBrandCategory(brandCategory)) && ProductProvider.Instance().DeleteBrandProductTypes(brandCategory.BrandId))
			{
				ProductProvider.Instance().AddBrandProductTypes(brandCategory.BrandId, brandCategory.ProductTypes);
			}
			return result;
		}
		public static bool BrandHvaeProducts(int brandId)
		{
			return ProductProvider.Instance().BrandHvaeProducts(brandId);
		}
		public static bool DeleteBrandCategory(int brandId)
		{
			return ProductProvider.Instance().DeleteBrandCategory(brandId);
		}
		public static void UpdateBrandCategorieDisplaySequence(int brandId, SortAction action)
		{
			ProductProvider.Instance().UpdateBrandCategoryDisplaySequence(brandId, action);
		}
		public static bool UpdateBrandCategoryDisplaySequence(int barndId, int displaysequence)
		{
			return ProductProvider.Instance().UpdateBrandCategoryDisplaySequence(barndId, displaysequence);
		}
		public static string UploadBrandCategorieImage(HttpPostedFile postedFile)
		{
			string result;
			if (!ResourcesHelper.CheckPostedFile(postedFile))
			{
				result = string.Empty;
			}
			else
			{
				string text = HiContext.Current.GetStoragePath() + "/brand/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
				postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + text));
				result = text;
			}
			return result;
		}
		public static bool SetBrandCategoryThemes(int brandid, string themeName)
		{
			bool result;
			if (result = ProductProvider.Instance().SetBrandCategoryThemes(brandid, themeName))
			{
				HiCache.Remove("DataCache-Categories");
			}
			return result;
		}
		public static System.Data.DataTable GetBrandCategories(string brandName)
		{
			return ProductProvider.Instance().GetBrandCategories(brandName);
		}
		public static System.Data.DataTable GetTags()
		{
			return ProductProvider.Instance().GetTags();
		}
		public static string GetTagName(int tagId)
		{
			return ProductProvider.Instance().GetTagName(tagId);
		}
		public static int AddTags(string tagName)
		{
			int result = 0;
			if (ProductProvider.Instance().GetTags(tagName) <= 0)
			{
				result = ProductProvider.Instance().AddTags(tagName);
			}
			return result;
		}
		public static bool UpdateTags(int tagId, string tagName)
		{
			bool result = false;
			int tags = ProductProvider.Instance().GetTags(tagName);
			if (tags == tagId || tags <= 0)
			{
				result = ProductProvider.Instance().UpdateTags(tagId, tagName);
			}
			return result;
		}
		public static bool DeleteTags(int tagId)
		{
			return ProductProvider.Instance().DeleteTags(tagId);
		}
	}
}
