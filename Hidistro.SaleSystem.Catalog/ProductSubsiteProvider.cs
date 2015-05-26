using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Membership.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
namespace Hidistro.SaleSystem.Catalog
{
	public abstract class ProductSubsiteProvider : ProductProvider
	{
		private static readonly ProductSubsiteProvider _defaultInstance;
		static ProductSubsiteProvider()
		{
			ProductSubsiteProvider._defaultInstance = (DataProviders.CreateInstance("Hidistro.SaleSystem.DistributionData.ProductData,Hidistro.SaleSystem.DistributionData") as ProductSubsiteProvider);
		}
		public static ProductSubsiteProvider CreateInstance()
		{
			return ProductSubsiteProvider._defaultInstance;
		}
		protected static string BuildProductBrowseQuerySearch(ProductBrowseQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SaleStatus = {0}", 1);
			if (!string.IsNullOrEmpty(query.ProductCode))
			{
				stringBuilder.AppendFormat(" AND LOWER(ProductCode) Like  '%{0}%'", DataHelper.CleanSearchString(query.ProductCode).ToLower());
			}
			if (query.AttributeValues.Count > 0)
			{
				foreach (AttributeValueInfo current in query.AttributeValues)
				{
					stringBuilder.AppendFormat(" AND ProductId IN ( SELECT ProductId FROM Hishop_ProductAttributes WHERE AttributeId={0} And ValueId={1}) ", current.AttributeId, current.ValueId);
				}
			}
			if (query.BrandId.HasValue)
			{
				if (query.BrandId.Value == 0)
				{
					stringBuilder.Append(" AND BrandId IS NOT NULL");
				}
				else
				{
					stringBuilder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
				}
			}
			if (query.MinSalePrice.HasValue)
			{
				stringBuilder.AppendFormat(" AND SalePrice >= {0}", query.MinSalePrice.Value);
			}
			if (query.MaxSalePrice.HasValue)
			{
				stringBuilder.AppendFormat(" AND SalePrice <= {0}", query.MaxSalePrice.Value);
			}
			if (!string.IsNullOrEmpty(query.Keywords) && query.Keywords.Trim().Length > 0)
			{
				if (!query.IsPrecise)
				{
					query.Keywords = DataHelper.CleanSearchString(query.Keywords);
					string[] array = Regex.Split(query.Keywords.Trim(), "\\s+");
					List<string> list = new List<string>();
					list.Add(string.Format("(replace(ProductName,' ','') LIKE '%{0}%' or LOWER(ProductCode) LIKE '%{0}%')", DataHelper.CleanSearchString(array[0])));
					int num = 1;
					while (num < array.Length && num <= 4)
					{
						list.Add(string.Format("(replace(ProductName,' ','') LIKE '%{0}%' or LOWER(ProductCode) LIKE '%{0}%')", DataHelper.CleanSearchString(array[num])));
						num++;
					}
					stringBuilder.Append(" and (" + string.Join(" and ", list.ToArray()) + ")");
				}
				else
				{
					stringBuilder.AppendFormat(" AND (ProductName = '{0}' or LOWER(ProductCode)='{0}')", DataHelper.CleanSearchString(query.Keywords));
				}
			}
			if (query.CategoryId.HasValue)
			{
				CategoryInfo category = CategoryBrowser.GetCategory(query.CategoryId.Value);
				if (category != null)
				{
					stringBuilder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%') ", category.Path);
				}
				else
				{
					stringBuilder.Append(" and 1>2 ");
				}
			}
			if (!string.IsNullOrEmpty(query.TagIds))
			{
				string[] array2 = query.TagIds.Split(new char[]
				{
					'_'
				});
				string[] array3 = array2;
				for (int i = 0; i < array3.Length; i++)
				{
					string text = array3[i];
					if (!string.IsNullOrEmpty(text))
					{
						stringBuilder.AppendFormat(" AND ProductId IN(SELECT ProductId FROM distro_ProductTag WHERE TagId = {0}  AND DistributorUserId={1})", text, HiContext.Current.SiteSettings.UserId);
					}
				}
			}
			return stringBuilder.ToString();
		}
		protected static string BuildUnSaleProductBrowseQuerySearch(ProductBrowseQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SaleStatus = {0}", 2);
			if (!string.IsNullOrEmpty(query.ProductCode))
			{
				stringBuilder.AppendFormat(" AND LOWER(ProductCode) Like  '%{0}%'", DataHelper.CleanSearchString(query.ProductCode).ToLower());
			}
			if (query.AttributeValues.Count > 0)
			{
				foreach (AttributeValueInfo current in query.AttributeValues)
				{
					stringBuilder.AppendFormat(" AND ProductId IN ( SELECT ProductId FROM Hishop_ProductAttributes WHERE AttributeId={0} And ValueId={1}) ", current.AttributeId, current.ValueId);
				}
			}
			if (query.BrandId.HasValue)
			{
				if (query.BrandId.Value == 0)
				{
					stringBuilder.Append(" AND BrandId IS NOT NULL");
				}
				else
				{
					stringBuilder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
				}
			}
			if (query.MinSalePrice.HasValue)
			{
				stringBuilder.AppendFormat(" AND SalePrice >= {0}", query.MinSalePrice.Value);
			}
			if (query.MaxSalePrice.HasValue)
			{
				stringBuilder.AppendFormat(" AND SalePrice <= {0}", query.MaxSalePrice.Value);
			}
			if (!string.IsNullOrEmpty(query.Keywords) && query.Keywords.Trim().Length > 0)
			{
				if (!query.IsPrecise)
				{
					query.Keywords = DataHelper.CleanSearchString(query.Keywords);
					string[] array = Regex.Split(query.Keywords.Trim(), "\\s+");
					StringBuilder stringBuilder2 = new StringBuilder();
					stringBuilder2.AppendFormat(" OR (LOWER(ProductCode) LIKE '%{0}%'", DataHelper.CleanSearchString(array[0]));
					stringBuilder.AppendFormat(" AND ((ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[0]));
					int num = 1;
					while (num < array.Length && num <= 4)
					{
						stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[num]));
						stringBuilder2.AppendFormat(" AND LOWER(ProductCode)  LIKE '%{0}%'", DataHelper.CleanSearchString(array[num]));
						num++;
					}
					stringBuilder.Append(")" + stringBuilder2.ToString() + "))");
				}
				else
				{
					stringBuilder.AppendFormat(" AND (ProductName = '{0}' or LOWER(ProductCode)='{0}')", DataHelper.CleanSearchString(query.Keywords));
				}
			}
			if (query.CategoryId.HasValue)
			{
				CategoryInfo category = CategoryBrowser.GetCategory(query.CategoryId.Value);
				if (category != null)
				{
					stringBuilder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%') ", category.Path);
				}
			}
			if (!string.IsNullOrEmpty(query.TagIds))
			{
				string[] array2 = query.TagIds.Split(new char[]
				{
					','
				});
				for (int i = 0; i < array2.Length; i++)
				{
					string arg = array2[i];
					stringBuilder.AppendFormat(" and ProductId IN (SELECT ProductId FROM distro_ProductTag WHERE TagId={0} AND DistributorUserId={1} )", arg, HiContext.Current.SiteSettings.UserId);
				}
			}
			return stringBuilder.ToString();
		}
		protected static string BuildProductSubjectQuerySearch(SubjectListQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" SaleStatus = {0}", 1);
			stringBuilder.AppendFormat(" AND DistributorUserId={0}", HiContext.Current.SiteSettings.UserId);
			if (query.TagId != 0)
			{
				stringBuilder.AppendFormat(" AND ProductId IN(SELECT ProductId FROM distro_ProductTag WHERE TagId = {0} AND DistributorUserId={1})", query.TagId, HiContext.Current.SiteSettings.UserId);
			}
			if (!string.IsNullOrEmpty(query.CategoryIds))
			{
				string[] array = query.CategoryIds.Split(new char[]
				{
					','
				});
				int categoryId = 0;
				bool flag = false;
				stringBuilder.AppendFormat(" AND (", new object[0]);
				for (int i = 0; i < array.Length; i++)
				{
					categoryId = 0;
					int.TryParse(array[i], out categoryId);
					CategoryInfo category = CategoryBrowser.GetCategory(categoryId);
					if (category != null)
					{
						if (flag)
						{
							stringBuilder.Append(" OR ");
						}
						stringBuilder.AppendFormat(" ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%') ", category.Path);
						flag = true;
					}
				}
				if (!flag)
				{
					stringBuilder.Append("1=1");
				}
				stringBuilder.Append(")");
			}
			if (query.BrandCategoryId.HasValue)
			{
				stringBuilder.AppendFormat(" AND BrandId = {0}", query.BrandCategoryId.Value);
			}
			if (query.ProductTypeId.HasValue)
			{
				stringBuilder.AppendFormat(" AND TypeId = {0}", query.ProductTypeId.Value);
			}
			if (query.AttributeValues.Count > 0)
			{
				foreach (AttributeValueInfo current in query.AttributeValues)
				{
					stringBuilder.AppendFormat(" AND (ProductId IN ( SELECT ProductId FROM Hishop_ProductAttributes WHERE AttributeId={0} And ValueId={1}))", current.AttributeId, current.ValueId);
				}
			}
			if (query.MinPrice.HasValue)
			{
				stringBuilder.AppendFormat(" AND SalePrice >= {0}", query.MinPrice.Value);
			}
			if (query.MaxPrice.HasValue)
			{
				stringBuilder.AppendFormat(" AND SalePrice <= {0}", query.MaxPrice.Value);
			}
			if (!string.IsNullOrEmpty(query.Keywords) && query.Keywords.Trim().Length > 0)
			{
				query.Keywords = DataHelper.CleanSearchString(query.Keywords);
				string[] array2 = Regex.Split(query.Keywords.Trim(), "\\s+");
				stringBuilder.AppendFormat(" AND (ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array2[0]));
				int i = 1;
				while (i < array2.Length && i <= 5)
				{
					stringBuilder.AppendFormat(" OR ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array2[i]));
					i++;
				}
				stringBuilder.Append(")");
			}
			return stringBuilder.ToString();
		}
	}
}
