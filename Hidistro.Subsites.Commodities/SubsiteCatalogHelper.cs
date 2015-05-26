using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Caching;
namespace Hidistro.Subsites.Commodities
{
	public class SubsiteCatalogHelper
	{
		private const string CategoriesCachekey = "DataCache-SubsiteCategories{0}";
		private SubsiteCatalogHelper()
		{
		}
		public static IList<CategoryInfo> GetMainCategories()
		{
			IList<CategoryInfo> list = new List<CategoryInfo>();
			System.Data.DataTable categories = SubsiteCatalogHelper.GetCategories();
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
			System.Data.DataTable categories = SubsiteCatalogHelper.GetCategories();
			System.Data.DataRow[] array = categories.Select("ParentCategoryId = " + parentCategoryId.ToString());
			for (int i = 0; i < array.Length; i++)
			{
				list.Add(DataMapper.ConvertDataRowToProductCategory(array[i]));
			}
			return list;
		}
		public static CategoryInfo GetCategory(int categoryId)
		{
			return SubsiteProductProvider.Instance().GetCategory(categoryId);
		}
		public static string GetFullCategory(int categoryId)
		{
			CategoryInfo category = SubsiteCatalogHelper.GetCategory(categoryId);
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
					category = SubsiteCatalogHelper.GetCategory(category.ParentCategoryId.Value);
					if (category != null)
					{
						text = category.Name + " >> " + text;
					}
				}
				result = text;
			}
			return result;
		}
		private static System.Data.DataTable GetCategories()
		{
			System.Data.DataTable dataTable = new System.Data.DataTable();
			if (HiContext.Current.User.UserRole != UserRole.Anonymous)
			{
				dataTable = (HiCache.Get(string.Format("DataCache-SubsiteCategories{0}", HiContext.Current.User.UserId)) as System.Data.DataTable);
			}
			else
			{
				dataTable = (HiCache.Get(string.Format("DataCache-SubsiteCategories{0}", HiContext.Current.SiteSettings.UserId.Value)) as System.Data.DataTable);
			}
			if (dataTable == null)
			{
				dataTable = SubsiteProductProvider.Instance().GetCategories();
				if (HiContext.Current.User.UserRole != UserRole.Anonymous)
				{
					HiCache.Insert(string.Format("DataCache-SubsiteCategories{0}", HiContext.Current.User.UserId), dataTable, 360, CacheItemPriority.Normal);
				}
				else
				{
					HiCache.Insert(string.Format("DataCache-SubsiteCategories{0}", HiContext.Current.SiteSettings.UserId.Value), dataTable, 360, CacheItemPriority.Normal);
				}
			}
			return dataTable;
		}
		public static IList<CategoryInfo> GetSequenceCategories()
		{
			IList<CategoryInfo> list = new List<CategoryInfo>();
			IList<CategoryInfo> mainCategories = SubsiteCatalogHelper.GetMainCategories();
			foreach (CategoryInfo current in mainCategories)
			{
				list.Add(current);
				SubsiteCatalogHelper.LoadSubCategorys(current.CategoryId, list);
			}
			return list;
		}
		private static void LoadSubCategorys(int parentCategoryId, IList<CategoryInfo> categories)
		{
			IList<CategoryInfo> subCategories = SubsiteCatalogHelper.GetSubCategories(parentCategoryId);
			if (subCategories != null && subCategories.Count > 0)
			{
				foreach (CategoryInfo current in subCategories)
				{
					categories.Add(current);
					SubsiteCatalogHelper.LoadSubCategorys(current.CategoryId, categories);
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
				int num = SubsiteProductProvider.Instance().CreateCategory(category);
				if (num > 0)
				{
					HiCache.Remove(string.Format("DataCache-SubsiteCategories{0}", HiContext.Current.User.UserId));
				}
				result = CategoryActionStatus.Success;
			}
			return result;
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
				if (!Directory.Exists(HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + HiContext.Current.GetStoragePath() + "/CateGory/")))
				{
					Directory.CreateDirectory(HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + HiContext.Current.GetStoragePath() + "/CateGory/"));
				}
				string text = HiContext.Current.GetStoragePath() + "/CateGory/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
				postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + text));
				result = text;
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
				CategoryActionStatus categoryActionStatus = SubsiteProductProvider.Instance().UpdateCategory(category);
				if (categoryActionStatus == CategoryActionStatus.Success)
				{
					HiCache.Remove(string.Format("DataCache-SubsiteCategories{0}", HiContext.Current.User.UserId));
				}
				result = categoryActionStatus;
			}
			return result;
		}
		public static void SwapCategorySequence(int categoryId, int displaysequence)
		{
			if (categoryId > 0)
			{
				SubsiteProductProvider.Instance().SwapCategorySequence(categoryId, displaysequence);
				HiCache.Remove(string.Format("DataCache-SubsiteCategories{0}", HiContext.Current.User.UserId));
			}
		}
		public static bool DeleteCategory(int categoryId)
		{
			bool result;
			if (result = SubsiteProductProvider.Instance().DeleteCategory(categoryId))
			{
				HiCache.Remove(string.Format("DataCache-SubsiteCategories{0}", HiContext.Current.User.UserId));
			}
			return result;
		}
		public static int DisplaceCategory(int oldCategoryId, int newCategory)
		{
			return SubsiteProductProvider.Instance().DisplaceCategory(oldCategoryId, newCategory);
		}
		public static bool SetProductExtendCategory(int productId, string extendCategoryPath)
		{
			return SubsiteProductProvider.Instance().SetProductExtendCategory(productId, extendCategoryPath);
		}
		public static bool SetCategoryThemes(int categoryId, string themeName)
		{
			if (SubsiteProductProvider.Instance().SetCategoryThemes(categoryId, themeName))
			{
				HiCache.Remove(string.Format("DataCache-SubsiteCategories{0}", HiContext.Current.User.UserId));
			}
			return false;
		}
		public static int DownloadCategory()
		{
			int num = SubsiteProductProvider.Instance().DownloadCategory();
			if (num > 0)
			{
				HiCache.Remove(string.Format("DataCache-SubsiteCategories{0}", HiContext.Current.User.UserId));
			}
			return num;
		}
	}
}
