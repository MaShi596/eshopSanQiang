using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.HOP;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Commodities;
using Hidistro.Subsites.Store;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using System.Text.RegularExpressions;
namespace Hidistro.Subsites.Data
{
	public class ProductData : SubsiteProductProvider
	{
		private Database database;
		public ProductData()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public override int CreateCategory(CategoryInfo category)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("sub_Category_Create");
			this.database.AddOutParameter(storedProcCommand, "CategoryId", System.Data.DbType.Int32, 4);
			this.database.AddInParameter(storedProcCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(storedProcCommand, "Name", System.Data.DbType.String, category.Name);
			this.database.AddInParameter(storedProcCommand, "DisplaySequence", System.Data.DbType.Int32, category.DisplaySequence);
			if (!string.IsNullOrEmpty(category.MetaTitle))
			{
				this.database.AddInParameter(storedProcCommand, "Meta_Title", System.Data.DbType.String, category.MetaTitle);
			}
			if (!string.IsNullOrEmpty(category.MetaDescription))
			{
				this.database.AddInParameter(storedProcCommand, "Meta_Description", System.Data.DbType.String, category.MetaDescription);
			}
			if (!string.IsNullOrEmpty(category.Icon))
			{
				this.database.AddInParameter(storedProcCommand, "Icon", System.Data.DbType.String, category.Icon);
			}
			if (!string.IsNullOrEmpty(category.MetaKeywords))
			{
				this.database.AddInParameter(storedProcCommand, "Meta_Keywords", System.Data.DbType.String, category.MetaKeywords);
			}
			if (!string.IsNullOrEmpty(category.Notes1))
			{
				this.database.AddInParameter(storedProcCommand, "Notes1", System.Data.DbType.String, category.Notes1);
			}
			if (!string.IsNullOrEmpty(category.Notes2))
			{
				this.database.AddInParameter(storedProcCommand, "Notes2", System.Data.DbType.String, category.Notes2);
			}
			if (!string.IsNullOrEmpty(category.Notes3))
			{
				this.database.AddInParameter(storedProcCommand, "Notes3", System.Data.DbType.String, category.Notes3);
			}
			if (!string.IsNullOrEmpty(category.Notes4))
			{
				this.database.AddInParameter(storedProcCommand, "Notes4", System.Data.DbType.String, category.Notes4);
			}
			if (!string.IsNullOrEmpty(category.Notes5))
			{
				this.database.AddInParameter(storedProcCommand, "Notes5", System.Data.DbType.String, category.Notes5);
			}
			if (category.ParentCategoryId.HasValue)
			{
				this.database.AddInParameter(storedProcCommand, "ParentCategoryId", System.Data.DbType.Int32, category.ParentCategoryId.Value);
			}
			else
			{
				this.database.AddInParameter(storedProcCommand, "ParentCategoryId", System.Data.DbType.Int32, 0);
			}
			if (category.AssociatedProductType.HasValue)
			{
				this.database.AddInParameter(storedProcCommand, "AssociatedProductType", System.Data.DbType.Int32, category.AssociatedProductType.Value);
			}
			if (!string.IsNullOrEmpty(category.RewriteName))
			{
				this.database.AddInParameter(storedProcCommand, "RewriteName", System.Data.DbType.String, category.RewriteName);
			}
			this.database.ExecuteNonQuery(storedProcCommand);
			return (int)this.database.GetParameterValue(storedProcCommand, "CategoryId");
		}
		public override bool DeleteCategory(int categoryId)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("sub_Category_Delete");
			this.database.AddInParameter(storedProcCommand, "CategoryId", System.Data.DbType.Int32, categoryId);
			this.database.AddInParameter(storedProcCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return this.database.ExecuteNonQuery(storedProcCommand) > 0;
		}
		public override CategoryActionStatus UpdateCategory(CategoryInfo category)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Categories SET [Name] = @Name,Icon=@Icon,Meta_Description = @Meta_Description,AssociatedProductType = @AssociatedProductType, Meta_Title=@Meta_Title,Meta_Keywords = @Meta_Keywords, Notes1 = @Notes1, Notes2 = @Notes2, Notes3 = @Notes3,  Notes4 = @Notes4, Notes5 = @Notes5, RewriteName = @RewriteName WHERE CategoryId = @CategoryId AND DistributorUserId=@DistributorUserId; UPDATE distro_Categories SET RewriteName = @RewriteName WHERE ParentCategoryId = @CategoryId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "CategoryId", System.Data.DbType.Int32, category.CategoryId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, category.Name);
			this.database.AddInParameter(sqlStringCommand, "Icon", System.Data.DbType.String, category.Icon);
			this.database.AddInParameter(sqlStringCommand, "AssociatedProductType", System.Data.DbType.Int32, category.AssociatedProductType);
			this.database.AddInParameter(sqlStringCommand, "Meta_Title", System.Data.DbType.String, category.MetaTitle);
			this.database.AddInParameter(sqlStringCommand, "Meta_Description", System.Data.DbType.String, category.MetaDescription);
			this.database.AddInParameter(sqlStringCommand, "Meta_Keywords", System.Data.DbType.String, category.MetaKeywords);
			this.database.AddInParameter(sqlStringCommand, "Notes1", System.Data.DbType.String, category.Notes1);
			this.database.AddInParameter(sqlStringCommand, "Notes2", System.Data.DbType.String, category.Notes2);
			this.database.AddInParameter(sqlStringCommand, "Notes3", System.Data.DbType.String, category.Notes3);
			this.database.AddInParameter(sqlStringCommand, "Notes4", System.Data.DbType.String, category.Notes4);
			this.database.AddInParameter(sqlStringCommand, "Notes5", System.Data.DbType.String, category.Notes5);
			this.database.AddInParameter(sqlStringCommand, "RewriteName", System.Data.DbType.String, category.RewriteName);
			return (this.database.ExecuteNonQuery(sqlStringCommand) >= 1) ? CategoryActionStatus.Success : CategoryActionStatus.UnknowError;
		}
		public override int DisplaceCategory(int oldCategoryId, int newCategory)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Products SET CategoryId=@newCategory,MainCategoryPath=(SELECT Path FROM distro_Categories WHERE CategoryId=@newCategory AND DistributorUserId=@DistributorUserId)+'|' WHERE CategoryId=@oldCategoryId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "oldCategoryId", System.Data.DbType.Int32, oldCategoryId);
			this.database.AddInParameter(sqlStringCommand, "newCategory", System.Data.DbType.Int32, newCategory);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override bool SwapCategorySequence(int categoryId, int displaysequence)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update distro_Categories  set DisplaySequence=@DisplaySequence where CategoryId=@CategoryId AND DistributorUserId = @DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "@DisplaySequence", System.Data.DbType.Int32, displaysequence);
			this.database.AddInParameter(sqlStringCommand, "@CategoryId", System.Data.DbType.Int32, categoryId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool SetProductExtendCategory(int productId, string extendCategoryPath)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Products SET ExtendCategoryPath = @ExtendCategoryPath WHERE ProductId = @ProductId AND DistributorUserId = @DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "ExtendCategoryPath", System.Data.DbType.String, extendCategoryPath);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override bool SetCategoryThemes(int categoryId, string themeName)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Categories SET Theme = @Theme WHERE CategoryId = @CategoryId AND DistributorUserId = @DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "CategoryId", System.Data.DbType.Int32, categoryId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "Theme", System.Data.DbType.String, themeName);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override int DownloadCategory()
		{
			string text = string.Format("INSERT INTO distro_Categories SELECT CategoryId, {0},Name,Icon, DisplaySequence,Meta_Description,Meta_Title,Meta_Keywords,", HiContext.Current.User.UserId) + " ParentCategoryId,Depth,[Path],RewriteName,AssociatedProductType,Notes1,Notes2, Notes3,Notes4, Notes5,Theme,HasChildren FROM Hishop_Categories" + string.Format(" DELETE FROM distro_Categories WHERE DistributorUserId = {0} AND HasChildren = 0 AND CategoryId NOT IN (SELECT CategoryId FROM Hishop_Products ", HiContext.Current.User.UserId) + string.Format(" WHERE PenetrationStatus=1 AND SaleStatus <> 0 AND LineId IN (SELECT LineId FROM Hishop_DistributorProductLines WHERE UserId = {0}))", HiContext.Current.User.UserId);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			int result;
			try
			{
				result = this.database.ExecuteNonQuery(sqlStringCommand);
			}
			catch
			{
				result = 0;
			}
			return result;
		}
		public override System.Data.DataTable GetCategories()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT CategoryId,DistributorUserId,Name,Icon,DisplaySequence,ParentCategoryId,'' AS SKUPrefix,Depth,[Path],RewriteName,Theme,HasChildren FROM distro_Categories WHERE DistributorUserId = @DistributorUserId ORDER BY DisplaySequence");
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			System.Data.DataTable result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override CategoryInfo GetCategory(int categoryId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *,'' AS SKUPrefix FROM distro_Categories WHERE DistributorUserId=@DistributorUserId AND CategoryId =@CategoryId");
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "CategoryId", System.Data.DbType.Int32, categoryId);
			CategoryInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateProductCategory(dataReader);
				}
			}
			return result;
		}
		public override DbQueryResult GetAuthorizeProducts(ProductQuery query, bool onlyNotDownload)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (onlyNotDownload)
			{
				stringBuilder.AppendFormat("ProductId NOT IN (SELECT ProductId FROM distro_Products WHERE DistributorUserId = {0}) AND", HiContext.Current.User.UserId);
			}
			stringBuilder.AppendFormat(" PenetrationStatus=1 AND LineId IN (SELECT LineId FROM Hishop_DistributorProductLines WHERE UserId={0}) And SaleStatus<>{1}", HiContext.Current.User.UserId, 0);
			if (!string.IsNullOrEmpty(query.ProductCode))
			{
				stringBuilder.AppendFormat(" AND ProductCode = '{0}'", DataHelper.CleanSearchString(query.ProductCode));
			}
			if (!string.IsNullOrEmpty(query.Keywords))
			{
				stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Keywords));
			}
			if (query.ProductLineId.HasValue)
			{
				stringBuilder.AppendFormat(" AND LineId = {0}", query.ProductLineId);
			}
			Distributor distributor = HiContext.Current.User as Distributor;
			int distributorDiscount = this.GetDistributorDiscount(distributor.GradeId);
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.Append("ProductId, ProductCode, ProductName, ThumbnailUrl40, MarketPrice, SalePrice,LowestSalePrice, Stock, DisplaySequence,");
			stringBuilder2.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUDistributorPrice WHERE SkuId = p.SkuId AND GradeId = {0}) = 1", distributor.GradeId);
			stringBuilder2.AppendFormat(" THEN (SELECT DistributorPurchasePrice FROM Hishop_SKUDistributorPrice WHERE SkuId = p.SkuId AND GradeId = {0})", distributor.GradeId);
			stringBuilder2.AppendFormat(" ELSE (SELECT PurchasePrice FROM Hishop_SKUs WHERE SkuId = p.SkuId)*{0}/100 END) AS PurchasePrice", distributorDiscount);
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BrowseProductList p", "ProductId", stringBuilder.ToString(), stringBuilder2.ToString());
		}
		public override DbQueryResult GetSubmitPuchaseProducts(ProductQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("PenetrationStatus=1 AND LineId IN (SELECT LineId FROM Hishop_DistributorProductLines WHERE UserId={0}) AND SaleStatus<>{1} ", HiContext.Current.User.UserId, 0);
			if (!string.IsNullOrEmpty(query.Keywords))
			{
				query.Keywords = DataHelper.CleanSearchString(query.Keywords);
				string[] array = Regex.Split(query.Keywords.Trim(), "\\s+");
				stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[0]));
				int num = 1;
				while (num < array.Length && num <= 4)
				{
					stringBuilder.AppendFormat("AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[num]));
					num++;
				}
			}
			if (query.ProductLineId.HasValue && query.ProductLineId.Value > 0)
			{
				stringBuilder.AppendFormat(" AND LineId={0}", Convert.ToInt32(query.ProductLineId.Value));
			}
			if (!string.IsNullOrEmpty(query.ProductCode))
			{
				stringBuilder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_Products", "ProductId", stringBuilder.ToString(), "ProductId, ProductCode, ProductName,ThumbnailUrl40, ThumbnailUrl60, ThumbnailUrl100, MarketPrice,DisplaySequence,LowestSalePrice, PenetrationStatus");
		}
		public override System.Data.DataTable GetPuchaseProduct(string skuId)
		{
			System.Data.DataTable result = null;
			Distributor distributor = HiContext.Current.User as Distributor;
			int distributorDiscount = this.GetDistributorDiscount(distributor.GradeId);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT hp.ProductId, hp.ProductCode, hp.ProductName, hp.ThumbnailUrl40,hp.ThumbnailUrl60, hp.ThumbnailUrl100, SkuId, SKU," + string.Format(" SalePrice, PurchasePrice*{0}/100 AS PurchasePrice, Stock, hp.DisplaySequence", distributorDiscount) + " FROM Hishop_SKUs hs right join Hishop_Products hp on hs.ProductId = hp.ProductId WHERE SkuId = @SkuId");
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, skuId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override System.Data.DataTable GetPuchaseProducts(ProductQuery query, out int count)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("AND hp.PenetrationStatus=1 AND hp.LineId IN (SELECT hd.LineId FROM Hishop_DistributorProductLines hd WHERE hd.UserId={0})", HiContext.Current.User.UserId);
			if (!string.IsNullOrEmpty(query.ProductCode))
			{
				stringBuilder.AppendFormat(" AND ProductCode = '{0}'", DataHelper.CleanSearchString(query.ProductCode));
			}
			if (!string.IsNullOrEmpty(query.Keywords) && query.PageSize == 1)
			{
				stringBuilder.AppendFormat(" AND hp.ProductName = '{0}'", DataHelper.CleanSearchString(query.Keywords));
			}
			if (!string.IsNullOrEmpty(query.Keywords) && query.PageSize != 1)
			{
				stringBuilder.AppendFormat(" AND hp.ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Keywords));
			}
			if (query.ProductLineId.HasValue)
			{
				stringBuilder.AppendFormat(" AND hp.LineId = {0}", query.ProductLineId);
			}
			StringBuilder stringBuilder2 = new StringBuilder();
			Distributor distributor = HiContext.Current.User as Distributor;
			int distributorDiscount = this.GetDistributorDiscount(distributor.GradeId);
			if (query.PageIndex == 1)
			{
				stringBuilder2.AppendFormat("SELECT TOP {0} hp.ProductId, hp.ProductCode, hp.ProductName, hp.ThumbnailUrl40,hp.ThumbnailUrl60, hp.ThumbnailUrl100, SkuId, SKU, CostPrice,MarketPrice,  SalePrice, PurchasePrice*{1}/100 AS PurchasePrice, LowestSalePrice, Stock, hp.DisplaySequence  FROM Hishop_SKUs hs right join Hishop_Products hp on hs.ProductId = hp.ProductId WHERE 1=1", query.PageSize, distributorDiscount);
			}
			else
			{
				stringBuilder2.AppendFormat("SELECT TOP {0} hp.ProductId, hp.ProductCode, hp.ProductName, hp.ThumbnailUrl40,hp.ThumbnailUrl60, hp.ThumbnailUrl100, SkuId, SKU, CostPrice,MarketPrice,  SalePrice, PurchasePrice*{1}/100 AS PurchasePrice, LowestSalePrice, Stock, hp.DisplaySequence FROM Hishop_SKUs hs right join Hishop_Products hp on hs.ProductId = hp.ProductId where SkuId NOT IN(SELECT top {2} SkuId FROM Hishop_SKUs hs right join Hishop_Products hp on hs.ProductId = hp.ProductId where 1=1 {3})", new object[]
				{
					query.PageSize,
					distributorDiscount,
					query.PageSize * (query.PageIndex - 1),
					stringBuilder
				});
			}
			stringBuilder2.Append(stringBuilder.ToString());
			stringBuilder2.AppendFormat(";SELECT COUNT(*) as count FROM Hishop_SKUs hs right join Hishop_Products hp on hs.ProductId = hp.ProductId where 1=1 {0}", stringBuilder);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder2.ToString());
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (dataReader.NextResult())
				{
					dataReader.Read();
					count = (int)dataReader["count"];
				}
				else
				{
					count = 0;
				}
			}
			return result;
		}
		public override bool DownloadProduct(int productId, bool isDownCategory)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("INSERT INTO distro_Products (TypeId, ProductId, DistributorUserId, ProductCode, ProductName, ShortDescription, Description, CategoryId, MainCategoryPath, ExtendCategoryPath,");
			stringBuilder.Append(" Title, Meta_Description, Meta_Keywords,  MarketPrice, SaleStatus, AddedDate,VistiCounts, SaleCounts, ShowSaleCounts, DisplaySequence,");
			stringBuilder.Append(" LineId, BrandId, ThumbnailUrl40, ThumbnailUrl60,ThumbnailUrl100, ThumbnailUrl160, ThumbnailUrl180, ThumbnailUrl220, ThumbnailUrl310, ThumbnailUrl410, HasSKU)");
			stringBuilder.Append(" SELECT TypeId, ProductId, @DistributorUserId, ProductCode, ProductName, ShortDescription,Description,");
			if (isDownCategory)
			{
				stringBuilder.Append(" CategoryId, MainCategoryPath, ExtendCategoryPath,");
			}
			else
			{
				stringBuilder.Append(" 0, null, null,");
			}
			stringBuilder.Append(" Title, Meta_Description, Meta_Keywords, MarketPrice, 3, AddedDate,0, 0, 0, DisplaySequence,");
			stringBuilder.Append(" LineId, BrandId, ThumbnailUrl40, ThumbnailUrl60,ThumbnailUrl100, ThumbnailUrl160, ThumbnailUrl180, ThumbnailUrl220, ThumbnailUrl310, ThumbnailUrl410, HasSKU");
			stringBuilder.Append(" FROM Hishop_Products WHERE ProductId = @ProductId;");
			stringBuilder.Append(" INSERT INTO distro_ProductTag (DistributorUserId, TagId, ProductId) SELECT @DistributorUserId, TagId, ProductId FROM Hishop_ProductTag WHERE ProductId = @ProductId;");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			return this.database.ExecuteNonQuery(sqlStringCommand) >= 1;
		}
		public override DbQueryResult GetUnclassifiedProducts(ProductQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("DistributorUserId = {0} AND ProductName LIKE '%{1}%'", HiContext.Current.User.UserId, DataHelper.CleanSearchString(query.Keywords));
			if (query.CategoryId.HasValue)
			{
				if (query.CategoryId.Value > 0)
				{
					stringBuilder.AppendFormat(" AND (MainCategoryPath LIKE '{0}|%'  OR ExtendCategoryPath LIKE '{0}|%') ", query.MaiCategoryPath);
				}
				else
				{
					stringBuilder.Append(" AND (CategoryId = 0 OR CategoryId IS NULL)");
				}
			}
			if (!string.IsNullOrEmpty(query.ProductCode))
			{
				stringBuilder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
			}
			Distributor distributor = HiContext.Current.User as Distributor;
			int distributorDiscount = this.GetDistributorDiscount(distributor.GradeId);
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_distro_BrowseProductList", "ProductId", stringBuilder.ToString(), string.Format("CategoryId,MainCategoryPath,ExtendCategoryPath, ProductId, ProductCode, ProductName, ThumbnailUrl40, MarketPrice, SalePrice, Stock, DisplaySequence", distributorDiscount));
		}
		public override DbQueryResult GetProducts(ProductQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("DistributorUserId = {0}", HiContext.Current.User.UserId);
			if (query.SaleStatus != ProductSaleStatus.All)
			{
				stringBuilder.AppendFormat(" AND SaleStatus = {0} ", (int)query.SaleStatus);
			}
			if (query.TagId.HasValue)
			{
				stringBuilder.AppendFormat(" AND ProductId IN (SELECT ProductId FROM distro_ProductTag WHERE TagId={0} AND DistributorUserId={1})", query.TagId, HiContext.Current.User.UserId);
			}
			if (!string.IsNullOrEmpty(query.Keywords))
			{
				query.Keywords = DataHelper.CleanSearchString(query.Keywords);
				string[] array = Regex.Split(query.Keywords.Trim(), "\\s+");
				stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[0]));
				int num = 1;
				while (num < array.Length && num <= 4)
				{
					stringBuilder.AppendFormat("AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[num]));
					num++;
				}
			}
			if (!string.IsNullOrEmpty(query.ProductCode))
			{
				stringBuilder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
			}
			if (query.CategoryId.HasValue && query.CategoryId.Value > 0)
			{
				stringBuilder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%')", query.MaiCategoryPath);
			}
			if (query.IsIncludePromotionProduct.HasValue && query.IsIncludePromotionProduct == false)
			{
				stringBuilder.AppendFormat(" AND ProductId NOT IN (SELECT ProductId from distro_PromotionProducts WHERE DistributorUserId=" + HiContext.Current.User.UserId + ")", new object[0]);
			}
			if (query.IsIncludeBundlingProduct.HasValue && !query.IsIncludeBundlingProduct.Value)
			{
				stringBuilder.AppendFormat(" AND ProductId NOT IN (SELECT ProductID from distro_BundlingProductItems where BundlingID in (select BundlingID from distro_BundlingProducts where DistributorUserId=" + HiContext.Current.User.UserId + "))", new object[0]);
			}
			if (query.BrandId.HasValue)
			{
				stringBuilder.AppendFormat(" AND BrandId={0}", query.BrandId.Value);
			}
			if (query.IsAlert)
			{
				stringBuilder.Append(" AND ProductId IN (SELECT DISTINCT ProductId FROM Hishop_SKUs WHERE Stock <= AlertStock)");
			}
			Distributor distributor = HiContext.Current.User as Distributor;
			int distributorDiscount = this.GetDistributorDiscount(distributor.GradeId);
			string selectFields = string.Concat(new string[]
			{
				"ProductId, ProductCode, ProductName, ThumbnailUrl40,  MarketPrice, Stock, DisplaySequence,SaleStatus,",
				string.Format(" (SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = p.SkuId AND DistributoruserId = {0}) AS SalePrice,LowestSalePrice,", distributor.UserId),
				string.Format(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUDistributorPrice WHERE SkuId = p.SkuId AND GradeId = {0}) = 1", distributor.GradeId),
				string.Format(" THEN (SELECT DistributorPurchasePrice FROM Hishop_SKUDistributorPrice WHERE SkuId = p.SkuId AND GradeId = {0})", distributor.GradeId),
				string.Format(" ELSE (SELECT PurchasePrice FROM Hishop_SKUs WHERE SkuId = p.SkuId)*{0}/100 END) AS PurchasePrice", distributorDiscount)
			});
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_distro_BrowseProductList p", "ProductId", stringBuilder.ToString(), selectFields);
		}
		public override System.Data.DataTable GetGroupBuyProducts(ProductQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder(string.Format(" WHERE DistributorUserId = {0}", HiContext.Current.User.UserId));
			stringBuilder.AppendFormat(" AND SaleStatus = {0}", (int)query.SaleStatus);
			if (!string.IsNullOrEmpty(query.Keywords))
			{
				query.Keywords = DataHelper.CleanSearchString(query.Keywords);
				string[] array = Regex.Split(query.Keywords.Trim(), "\\s+");
				stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[0]));
				int num = 1;
				while (num < array.Length && num <= 4)
				{
					stringBuilder.AppendFormat("AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[num]));
					num++;
				}
			}
			if (!string.IsNullOrEmpty(query.ProductCode))
			{
				stringBuilder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
			}
			if (query.CategoryId.HasValue && query.CategoryId.Value > 0)
			{
				stringBuilder.AppendFormat(" AND MainCategoryPath LIKE '{0}|%'", query.MaiCategoryPath);
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ProductId,ProductName FROM distro_Products" + stringBuilder.ToString());
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override IList<ProductInfo> GetProducts(IList<int> productIds)
		{
			IList<ProductInfo> list = new List<ProductInfo>();
			string text = "(";
			foreach (int current in productIds)
			{
				text = text + current + ",";
			}
			IList<ProductInfo> result;
			if (text.Length <= 1)
			{
				result = list;
			}
			else
			{
				Distributor distributor = HiContext.Current.User as Distributor;
				int distributorDiscount = this.GetDistributorDiscount(distributor.GradeId);
				text = text.Substring(0, text.Length - 1) + ")";
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT  PurchasePrice*{0}/100  AS PurchasePrice,0 as PenetrationStatus,*  FROM distro_Products WHERE ProductId IN ", distributorDiscount) + text + " AND DistributorUserId=@DistributorUserId");
				this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					while (dataReader.Read())
					{
						list.Add(DataMapper.PopulateSubProduct(dataReader));
					}
				}
				result = list;
			}
			return result;
		}
		private int GetDistributorDiscount(int gradeId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Discount FROM aspnet_DistributorGrades WHERE GradeId=@GradeId");
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, gradeId);
			return (int)this.database.ExecuteScalar(sqlStringCommand);
		}
		public override DbQueryResult GetSubjectProducts(int tagId, Pagination page)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("DistributorUserId = {0}", HiContext.Current.User.UserId);
			stringBuilder.AppendFormat(" AND SaleStatus!={0} AND ProductId IN (SELECT ProductId FROM distro_ProductTag WHERE TagId = {1} AND DistributorUserId = {2}) ", 0, tagId, HiContext.Current.User.UserId);
			Distributor distributor = HiContext.Current.User as Distributor;
			int distributorDiscount = this.GetDistributorDiscount(distributor.GradeId);
			string selectFields = "ProductId, ProductCode, ProductName, ThumbnailUrl40,  MarketPrice, SalePrice, Stock, DisplaySequence," + string.Format(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUDistributorPrice WHERE SkuId = p.SkuId AND GradeId = {0}) = 1", distributor.GradeId) + string.Format(" THEN (SELECT DistributorPurchasePrice FROM Hishop_SKUDistributorPrice WHERE SkuId = p.SkuId AND GradeId = {0})", distributor.GradeId) + string.Format(" ELSE (SELECT PurchasePrice FROM Hishop_SKUs WHERE SkuId = p.SkuId)*{0}/100 END) AS PurchasePrice", distributorDiscount);
			return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "vw_distro_BrowseProductList p", "ProductId", stringBuilder.ToString(), selectFields);
		}
		public override bool IsOnSale(string productIds)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT COUNT(*) FROM Hishop_Products p WHERE ProductId IN ({0}) AND LowestSalePrice <= (SELECT MIN(SalePrice) FROM vw_distro_SkuPrices", productIds) + string.Format(" WHERE DistributoruserId = {0} AND SkuId IN (SELECT SkuId FROM Hishop_Skus WHERE ProductId = p.ProductId))", HiContext.Current.User.UserId));
			return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
		}
		public override int UpdateProductSaleStatus(string productIds, ProductSaleStatus saleStatus)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE distro_Products SET SaleStatus = {0} WHERE DistributorUserId = {1} AND ProductId IN ({2})", (int)saleStatus, HiContext.Current.User.UserId, productIds));
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override int GetUpProducts()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Count(*) FROM Taobao_DistroProducts WHERE updatestatus=1 and DistributorUserId = @DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return (int)this.database.ExecuteScalar(sqlStringCommand);
		}
		public override bool UpdateProduct(ProductInfo product, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("sub_Product_Update");
			this.database.AddInParameter(storedProcCommand, "CategoryId", System.Data.DbType.Int32, product.CategoryId);
			this.database.AddInParameter(storedProcCommand, "MainCategoryPath", System.Data.DbType.String, product.MainCategoryPath);
			this.database.AddInParameter(storedProcCommand, "ProductName", System.Data.DbType.String, product.ProductName);
			this.database.AddInParameter(storedProcCommand, "ShortDescription", System.Data.DbType.String, product.ShortDescription);
			this.database.AddInParameter(storedProcCommand, "Description", System.Data.DbType.String, product.Description);
			this.database.AddInParameter(storedProcCommand, "Title", System.Data.DbType.String, product.Title);
			this.database.AddInParameter(storedProcCommand, "Meta_Description", System.Data.DbType.String, product.MetaDescription);
			this.database.AddInParameter(storedProcCommand, "Meta_Keywords", System.Data.DbType.String, product.MetaKeywords);
			this.database.AddInParameter(storedProcCommand, "MarketPrice", System.Data.DbType.Currency, product.MarketPrice);
			this.database.AddInParameter(storedProcCommand, "SaleStatus", System.Data.DbType.Int32, (int)product.SaleStatus);
			this.database.AddInParameter(storedProcCommand, "DisplaySequence", System.Data.DbType.Currency, product.DisplaySequence);
			this.database.AddInParameter(storedProcCommand, "ProductId", System.Data.DbType.Int32, product.ProductId);
			this.database.AddInParameter(storedProcCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			bool result;
			if (dbTran != null)
			{
				result = (this.database.ExecuteNonQuery(storedProcCommand, dbTran) > 0);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(storedProcCommand) > 0);
			}
			return result;
		}
		public override bool AddSkuSalePrice(int productId, Dictionary<string, decimal> skuSalePrice, System.Data.Common.DbTransaction dbTran)
		{
			SiteSettings siteSettings = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("DELETE FROM distro_SKUMemberPrice WHERE DistributoruserId = {0} AND GradeId = 0 AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId = {1});", HiContext.Current.User.UserId, productId);
			if (skuSalePrice != null && skuSalePrice.Count > 0)
			{
				foreach (string current in skuSalePrice.Keys)
				{
					stringBuilder.AppendFormat(" INSERT INTO distro_SKUMemberPrice(SkuId, DistributoruserId, GradeId, MemberSalePrice) VALUES ('{0}', {1}, 0, {2})", current, HiContext.Current.User.UserId, Math.Round(skuSalePrice[current], siteSettings.DecimalLength));
				}
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			bool result;
			if (dbTran != null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) >= 0);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) >= 0);
			}
			return result;
		}
		public override bool UpdateProductCategory(int productId, int newCategoryId, string maiCategoryPath)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Products SET CategoryId = @CategoryId, MainCategoryPath = @MainCategoryPath WHERE DistributorUserId = @DistributorUserId AND ProductId = @ProductId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "CategoryId", System.Data.DbType.Int32, newCategoryId);
			this.database.AddInParameter(sqlStringCommand, "MainCategoryPath", System.Data.DbType.String, maiCategoryPath);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override int DeleteProducts(string productIds)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("DELETE FROM distro_Products WHERE DistributorUserId = {0} AND ProductId IN ({1})", HiContext.Current.User.UserId, productIds) + string.Format(" DELETE FROM distro_RelatedProducts WHERE DistributorUserId = {0} AND (ProductId IN ({1}) OR RelatedProductId IN ({1}));DELETE FROM distro_ProductTag WHERE ProductId IN ({0})", HiContext.Current.User.UserId, productIds));
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override ProductInfo GetProduct(int productId)
		{
			Distributor distributor = HiContext.Current.User as Distributor;
			int arg_25_0 = SubsiteStoreProvider.Instance().GetDistributorGradeInfo(distributor.GradeId).Discount;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT dp.*, p.Unit, p.ImageUrl1, p.ImageUrl2, p.ImageUrl3, p.ImageUrl4, p.ImageUrl5, p.LowestSalePrice, p.PenetrationStatus FROM distro_Products dp join Hishop_Products p ON dp.ProductId = p.ProductId WHERE dp.DistributorUserId = @DistributorUserId AND dp.ProductId = @ProductId");
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			ProductInfo productInfo = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					productInfo = DataMapper.PopulateSubProduct(dataReader);
					if (DBNull.Value != dataReader["ImageUrl1"])
					{
						productInfo.ImageUrl1 = (string)dataReader["ImageUrl1"];
					}
					if (DBNull.Value != dataReader["ImageUrl2"])
					{
						productInfo.ImageUrl2 = (string)dataReader["ImageUrl2"];
					}
					if (DBNull.Value != dataReader["ImageUrl3"])
					{
						productInfo.ImageUrl3 = (string)dataReader["ImageUrl3"];
					}
					if (DBNull.Value != dataReader["ImageUrl4"])
					{
						productInfo.ImageUrl4 = (string)dataReader["ImageUrl4"];
					}
					if (DBNull.Value != dataReader["ImageUrl5"])
					{
						productInfo.ImageUrl5 = (string)dataReader["ImageUrl5"];
					}
					if (DBNull.Value != dataReader["Unit"])
					{
						productInfo.Unit = (string)dataReader["Unit"];
					}
				}
			}
			return productInfo;
		}
		public override System.Data.DataTable GetSkuContentBySku(string skuId)
		{
			System.Data.DataTable result = null;
			Distributor distributor = HiContext.Current.User as Distributor;
			int distributorDiscount = this.GetDistributorDiscount(distributor.GradeId);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT s.SkuId, ProductId, SKU,Weight, Stock, AlertStock, CostPrice,");
			stringBuilder.AppendFormat(" ISNULL((SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = s.SkuId AND DistributoruserId = {0}), s.SalePrice) AS SalePrice,", distributor.UserId);
			stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", distributor.GradeId);
			stringBuilder.AppendFormat(" THEN (SELECT DistributorPurchasePrice FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE PurchasePrice*{1}/100 END) AS PurchasePrice,", distributor.GradeId, distributorDiscount);
			stringBuilder.Append(" (SELECT ProductName FROM Hishop_Products WHERE ProductId = s.ProductId) AS ProductName,");
			stringBuilder.Append(" (SELECT ThumbnailUrl40 FROM Hishop_Products WHERE ProductId = s.ProductId) AS ThumbnailUrl40,AttributeName, ValueStr");
			stringBuilder.Append(" FROM Hishop_SKUs s left join Hishop_SKUItems si on s.SkuId = si.SkuId");
			stringBuilder.Append(" left join Hishop_Attributes a on si.AttributeId = a.AttributeId left join Hishop_AttributeValues av on si.ValueId = av.ValueId WHERE s.SkuId = @SkuId");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, skuId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override IList<int> GetProductIds(ProductQuery query)
		{
			IList<int> list = new List<int>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(SubsiteProductProvider.BuildProductQuery(query));
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add((int)dataReader["ProductId"]);
				}
			}
			return list;
		}
		public override bool AddSubjectProducts(int tagId, IList<int> productIds)
		{
			bool result;
			if (productIds.Count <= 0)
			{
				result = false;
			}
			else
			{
				foreach (int current in productIds)
				{
					this.RemoveSubjectProduct(tagId, current);
				}
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO distro_ProductTag(DistributorUserId, TagId, ProductId) VALUES (@DistributorUserId, @TagId, @ProductId)");
				this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
				this.database.AddInParameter(sqlStringCommand, "TagId", System.Data.DbType.Int32, tagId);
				this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32);
				try
				{
					foreach (int current2 in productIds)
					{
						this.database.SetParameterValue(sqlStringCommand, "ProductId", current2);
						this.database.ExecuteNonQuery(sqlStringCommand);
					}
					result = true;
				}
				catch
				{
					result = false;
				}
			}
			return result;
		}
		public override bool RemoveSubjectProduct(int tagId, int productId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_ProductTag WHERE TagId = @TagId AND ProductId = @ProductId AND DistributorUserId = @DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "TagId", System.Data.DbType.Int32, tagId);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override bool ClearSubjectProducts(int tagId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_ProductTag WHERE TagId = @TagId AND DistributorUserId = @DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "TagId", System.Data.DbType.Int32, tagId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override System.Data.DataTable GetProductAttribute(int productId)
		{
			System.Data.DataTable result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT a.AttributeId, AttributeName, ValueStr FROM Hishop_ProductAttributes pa join Hishop_Attributes a ON pa.AttributeId = a.AttributeId JOIN Hishop_AttributeValues v ON a.AttributeId = v.AttributeId AND pa.ValueId = v.ValueId WHERE ProductId = @ProductId ORDER BY a.DisplaySequence DESC, v.DisplaySequence DESC");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				System.Data.DataTable dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (dataTable != null && dataTable.Rows.Count > 0)
				{
					System.Data.DataTable dataTable2 = dataTable.Clone();
					foreach (System.Data.DataRow dataRow in dataTable.Rows)
					{
						bool flag = false;
						if (dataTable2.Rows.Count > 0)
						{
							foreach (System.Data.DataRow dataRow2 in dataTable2.Rows)
							{
								if ((int)dataRow2["AttributeId"] == (int)dataRow["AttributeId"])
								{
									flag = true;
									System.Data.DataRow dataRow3;
									(dataRow3 = dataRow2)["ValueStr"] = dataRow3["ValueStr"] + ", " + dataRow["ValueStr"];
								}
							}
						}
						if (!flag)
						{
							System.Data.DataRow dataRow4 = dataTable2.NewRow();
							dataRow4["AttributeId"] = dataRow["AttributeId"];
							dataRow4["AttributeName"] = dataRow["AttributeName"];
							dataRow4["ValueStr"] = dataRow["ValueStr"];
							dataTable2.Rows.Add(dataRow4);
						}
					}
					result = dataTable2;
				}
			}
			return result;
		}
		public override System.Data.DataTable GetProductSKU(int productId)
		{
			Distributor distributor = HiContext.Current.User as Distributor;
			int distributorDiscount = this.GetDistributorDiscount(distributor.GradeId);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT SkuId, (SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = s.SkuId AND DistributoruserId = {0}) AS 一口价,", HiContext.Current.User.UserId) + string.Format(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", distributor.GradeId) + string.Format(" THEN (SELECT DistributorPurchasePrice FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE PurchasePrice*{1}/100 END) AS '采购价',", distributor.GradeId, distributorDiscount) + " AlertStock AS '警戒库存', Stock AS '库存', Weight AS '重量', SKU AS '货号' FROM Hishop_SKUs s WHERE ProductId = @ProductId; SELECT SkuId,AttributeName,UseAttributeImage,ValueStr,ImageUrl FROM Hishop_SKUItems s join Hishop_Attributes a on s.AttributeId = a.AttributeId join Hishop_AttributeValues av on s.ValueId = av.ValueId WHERE SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId = @ProductId) ORDER BY a.DisplaySequence DESC");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			System.Data.DataTable dataTable;
			System.Data.DataTable dataTable2;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
				dataReader.NextResult();
				dataTable2 = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			if (dataTable != null && dataTable.Rows.Count > 0 && dataTable2 != null && dataTable2.Rows.Count > 0)
			{
				foreach (System.Data.DataRow dataRow in dataTable2.Rows)
				{
					System.Data.DataColumn dataColumn = new System.Data.DataColumn();
					dataColumn.ColumnName = (string)dataRow["AttributeName"];
					if (!dataTable.Columns.Contains(dataColumn.ColumnName))
					{
						dataTable.Columns.Add(dataColumn);
					}
				}
				foreach (System.Data.DataRow dataRow2 in dataTable.Rows)
				{
					foreach (System.Data.DataRow dataRow in dataTable2.Rows)
					{
						if (string.Compare((string)dataRow2["SkuId"], (string)dataRow["SkuId"]) == 0)
						{
							if ((bool)dataRow["UseAttributeImage"] && dataRow["ImageUrl"] != DBNull.Value)
							{
								dataRow2[(string)dataRow["AttributeName"]] = dataRow["ImageUrl"];
							}
							else
							{
								dataRow2[(string)dataRow["AttributeName"]] = dataRow["ValueStr"];
							}
						}
					}
				}
			}
			return dataTable;
		}
		public override IList<SKUItem> GetSkus(string productIds)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT * FROM Hishop_SKUs WHERE ProductId IN ({0})", DataHelper.CleanSearchString(productIds)));
			IList<SKUItem> list = new List<SKUItem>();
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulateSKU(dataReader));
				}
			}
			return list;
		}
		public override System.Data.DataTable GetAuthorizeProductLines()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT pl.LineId,Name,(SELECT count(*) From Hishop_Products WHERE LineId=pl.LineId AND PenetrationStatus = 1) AS ProductCount FROM Hishop_DistributorProductLines dpl join Hishop_ProductLines pl on dpl.LineId=pl.LineId WHERE UserId=@UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			System.Data.DataTable result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
				dataReader.Close();
			}
			return result;
		}
		public override IList<ProductLineInfo> GetAuthorizeProductLineList()
		{
			IList<ProductLineInfo> list = new List<ProductLineInfo>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ProductLines pl join Hishop_DistributorProductLines dpl on dpl.LineId=pl.LineId WHERE UserId=@UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulateProductLine(dataReader));
				}
			}
			return list;
		}
		public override System.Data.DataTable GetSkusByProductId(int productId)
		{
			Distributor distributor = HiContext.Current.User as Distributor;
			int distributorDiscount = this.GetDistributorDiscount(distributor.GradeId);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock, AlertStock, CostPrice,");
			stringBuilder.AppendFormat(" ISNULL((SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = s.SkuId AND DistributoruserId = {0}), s.SalePrice) AS SalePrice,", distributor.UserId);
			stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", distributor.GradeId);
			stringBuilder.AppendFormat(" THEN (SELECT DistributorPurchasePrice FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE PurchasePrice*{1}/100 END) AS PurchasePrice", distributor.GradeId, distributorDiscount);
			stringBuilder.Append(" FROM Hishop_SKUs s WHERE ProductId = @ProductId");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			System.Data.DataTable result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override System.Data.DataTable GetProductBaseInfo(string productIds)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT ProductId, ProductName, ProductCode, MarketPrice, ThumbnailUrl40, SaleCounts, ShowSaleCounts FROM distro_Products WHERE DistributorUserId = {0} AND ProductId IN ({1})", HiContext.Current.User.UserId, DataHelper.CleanSearchString(productIds)));
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override bool UpdateShowSaleCounts(string productIds, int showSaleCounts)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE distro_Products SET ShowSaleCounts = {0} WHERE DistributorUserId = {1} AND ProductId IN ({2})", showSaleCounts, HiContext.Current.User.UserId, productIds));
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool UpdateShowSaleCounts(string productIds, int showSaleCounts, string operation)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE distro_Products SET ShowSaleCounts = SaleCounts {0} {1} WHERE DistributorUserId = {2} AND ProductId IN ({3})", new object[]
			{
				operation,
				showSaleCounts,
				HiContext.Current.User.UserId,
				productIds
			}));
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool UpdateShowSaleCounts(System.Data.DataTable dataTable_0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (System.Data.DataRow dataRow in dataTable_0.Rows)
			{
				stringBuilder.AppendFormat(" UPDATE distro_Products SET ShowSaleCounts = {0} WHERE DistributorUserId = {1} AND ProductId = {2}", dataRow["ShowSaleCounts"], HiContext.Current.User.UserId, dataRow["ProductId"]);
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override System.Data.DataTable GetSkuUnderlingPrices(string productIds)
		{
			Distributor distributor = HiContext.Current.User as Distributor;
			int distributorDiscount = this.GetDistributorDiscount(distributor.GradeId);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT SkuId, ProductName, SKU, MarketPrice,");
			stringBuilder.AppendFormat(" (SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = s.SkuId AND DistributoruserId = {0}) AS SalePrice,", distributor.UserId);
			stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", distributor.GradeId);
			stringBuilder.AppendFormat(" THEN (SELECT DistributorPurchasePrice FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE PurchasePrice*{1}/100 END) AS PurchasePrice", distributor.GradeId, distributorDiscount);
			stringBuilder.AppendFormat(" FROM distro_Products p JOIN Hishop_SKUs s ON p.ProductId = s.ProductId WHERE p.DistributorUserId = {0} AND p.ProductId IN ({1})", distributor.UserId, DataHelper.CleanSearchString(productIds));
			stringBuilder.Append(" SELECT SkuId, AttributeName, ValueStr FROM Hishop_SKUItems si JOIN Hishop_Attributes a ON si.AttributeId = a.AttributeId JOIN Hishop_AttributeValues av ON si.ValueId = av.ValueId");
			stringBuilder.AppendFormat(" WHERE si.SkuId IN(SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({0}))", DataHelper.CleanSearchString(productIds));
			stringBuilder.AppendFormat(" SELECT CAST(GradeId AS NVARCHAR) + '_' + [Name] AS MemberGradeName,Discount FROM distro_MemberGrades WHERE CreateUserId = {0}", distributor.UserId);
			stringBuilder.AppendFormat(" SELECT SkuId, (SELECT CAST(GradeId AS NVARCHAR) + '_' + [Name] FROM distro_MemberGrades WHERE GradeId = sm.GradeId AND CreateUserId = {0}) AS MemberGradeName", distributor.UserId);
			stringBuilder.AppendFormat(" ,  MemberSalePrice FROM distro_SKUMemberPrice sm WHERE GradeId <> 0 AND  DistributoruserId = {0} AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({1}))", distributor.UserId, DataHelper.CleanSearchString(productIds));
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			System.Data.DataTable dataTable = null;
			System.Data.DataTable dataTable2 = null;
			System.Data.DataTable dataTable3 = null;
			System.Data.DataTable dataTable4 = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (dataTable != null && dataTable.Rows.Count > 0)
				{
					dataTable.Columns.Add("SKUContent");
					dataReader.NextResult();
					dataTable2 = DataHelper.ConverDataReaderToDataTable(dataReader);
					dataReader.NextResult();
					dataTable4 = DataHelper.ConverDataReaderToDataTable(dataReader);
					if (dataTable4 == null || dataTable4.Rows.Count <= 0)
					{
						goto IL_220;
					}
					IEnumerator enumerator = dataTable4.Rows.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							System.Data.DataRow dataRow = (System.Data.DataRow)enumerator.Current;
							dataTable.Columns.Add((string)dataRow["MemberGradeName"]);
						}
						goto IL_220;
					}
					finally
					{
						IDisposable disposable = enumerator as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
					IL_202:
					dataTable.Columns.Add((string)dataReader["MemberGradeName"]);
					IL_220:
					if (dataReader.Read())
					{
						goto IL_202;
					}
					dataReader.NextResult();
					dataTable3 = DataHelper.ConverDataReaderToDataTable(dataReader);
				}
			}
			if (dataTable2 != null && dataTable2.Rows.Count > 0)
			{
				foreach (System.Data.DataRow dataRow2 in dataTable.Rows)
				{
					string text = string.Empty;
					foreach (System.Data.DataRow dataRow3 in dataTable2.Rows)
					{
						if ((string)dataRow2["SkuId"] == (string)dataRow3["SkuId"])
						{
							object obj = text;
							text = string.Concat(new object[]
							{
								obj,
								dataRow3["AttributeName"],
								"：",
								dataRow3["ValueStr"],
								"; "
							});
						}
					}
					dataRow2["SKUContent"] = text;
				}
			}
			if (dataTable3 != null && dataTable3.Rows.Count > 0)
			{
				foreach (System.Data.DataRow dataRow2 in dataTable.Rows)
				{
					foreach (System.Data.DataRow dataRow4 in dataTable3.Rows)
					{
						if ((string)dataRow2["SkuId"] == (string)dataRow4["SkuId"])
						{
							dataRow2[(string)dataRow4["MemberGradeName"]] = (decimal)dataRow4["MemberSalePrice"];
						}
					}
				}
			}
			if (dataTable4 != null && dataTable4.Rows.Count > 0)
			{
				foreach (System.Data.DataRow dataRow2 in dataTable.Rows)
				{
					decimal d = decimal.Parse(dataRow2["SalePrice"].ToString());
					foreach (System.Data.DataRow dataRow5 in dataTable4.Rows)
					{
						decimal d2 = decimal.Parse(dataRow5["Discount"].ToString());
						string arg = (d * (d2 / 100m)).ToString("F2");
						dataRow2[(string)dataRow5["MemberGradeName"]] = dataRow2[(string)dataRow5["MemberGradeName"]] + "|" + arg;
					}
				}
			}
			return dataTable;
		}
		public override bool CheckPrice(string productIds, string basePriceName, decimal checkPrice)
		{
			Distributor distributor = HiContext.Current.User as Distributor;
			int distributorDiscount = this.GetDistributorDiscount(distributor.GradeId);
			StringBuilder stringBuilder = new StringBuilder();
			if (basePriceName == "PurchasePrice")
			{
				stringBuilder.AppendFormat("SELECT COUNT(*) FROM Hishop_SKUs s WHERE ProductId IN ({0}) AND", DataHelper.CleanSearchString(productIds));
				stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", distributor.GradeId);
				stringBuilder.AppendFormat(" THEN (SELECT DistributorPurchasePrice FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE PurchasePrice*{1}/100 END) - {2} < 0", distributor.GradeId, distributorDiscount, checkPrice);
			}
			else
			{
				if (basePriceName == "SalePrice")
				{
					stringBuilder.AppendFormat("SELECT COUNT(*) FROM Hishop_SKUs s WHERE ProductId IN ({0}) AND (SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = s.SkuId AND DistributoruserId = {1}) - {2} < 0", DataHelper.CleanSearchString(productIds), distributor.UserId, checkPrice);
				}
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
		}
		public override bool CheckPrice(string productIds, string basePriceName, decimal checkPrice, string operation)
		{
			Distributor distributor = HiContext.Current.User as Distributor;
			int distributorDiscount = this.GetDistributorDiscount(distributor.GradeId);
			StringBuilder stringBuilder = new StringBuilder();
			if (basePriceName == "PurchasePrice")
			{
				stringBuilder.AppendFormat("SELECT COUNT(*) FROM Hishop_SKUs s WHERE ProductId IN ({0}) AND(", DataHelper.CleanSearchString(productIds));
				stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", distributor.GradeId);
				stringBuilder.AppendFormat(" THEN (SELECT DistributorPurchasePrice FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE PurchasePrice*{1}/100 END) {3} {2} < 0 or ", new object[]
				{
					distributor.GradeId,
					distributorDiscount,
					checkPrice,
					operation
				});
				stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", distributor.GradeId);
				stringBuilder.AppendFormat(" THEN (SELECT DistributorPurchasePrice FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE PurchasePrice*{1}/100 END) {3} {2} > 10000000 ) ", new object[]
				{
					distributor.GradeId,
					distributorDiscount,
					checkPrice,
					operation
				});
			}
			else
			{
				if (basePriceName == "SalePrice")
				{
					stringBuilder.AppendFormat("SELECT COUNT(*) FROM Hishop_SKUs s WHERE ProductId IN ({0}) AND ((SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = s.SkuId AND DistributoruserId = {1}) {3} {2} < 0 or (SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = s.SkuId AND DistributoruserId = {1}) {3} {2} > 10000000)", new object[]
					{
						DataHelper.CleanSearchString(productIds),
						distributor.UserId,
						checkPrice,
						operation
					});
				}
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
		}
		public override bool UpdateSkuUnderlingPrices(string productIds, int gradeId, decimal price)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (gradeId == -3)
			{
				stringBuilder.AppendFormat("DELETE FROM distro_SKUMemberPrice WHERE GradeId = 0 AND DistributoruserId = {0} AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({1}))", HiContext.Current.User.UserId, DataHelper.CleanSearchString(productIds));
				stringBuilder.AppendFormat(" INSERT INTO distro_SKUMemberPrice (SkuId,DistributoruserId, GradeId,MemberSalePrice) SELECT SkuId, {0} AS DistributoruserId, 0 AS GradeId, {1} AS MemberSalePrice", HiContext.Current.User.UserId, price);
				stringBuilder.AppendFormat(" FROM Hishop_SKUs WHERE SalePrice <> {0} AND ProductId IN ({1})", price, DataHelper.CleanSearchString(productIds));
			}
			else
			{
				stringBuilder.AppendFormat("DELETE FROM distro_SKUMemberPrice WHERE GradeId = {0} AND DistributoruserId = {1} AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({2}))", gradeId, HiContext.Current.User.UserId, DataHelper.CleanSearchString(productIds));
				stringBuilder.AppendFormat(" INSERT INTO distro_SKUMemberPrice (SkuId,DistributoruserId, GradeId,MemberSalePrice) SELECT SkuId, {0} AS DistributoruserId, {1} AS GradeId, {2} AS MemberSalePrice", HiContext.Current.User.UserId, gradeId, price);
				stringBuilder.AppendFormat(" FROM Hishop_SKUs WHERE ProductId IN ({0})", DataHelper.CleanSearchString(productIds));
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool UpdateSkuUnderlingPrices(string productIds, int gradeId, string basePriceName, string operation, decimal price)
		{
			Distributor distributor = HiContext.Current.User as Distributor;
			int distributorDiscount = this.GetDistributorDiscount(distributor.GradeId);
			StringBuilder stringBuilder = new StringBuilder();
			if (gradeId == -3)
			{
				if (basePriceName == "PurchasePrice")
				{
					stringBuilder.AppendFormat("DELETE FROM distro_SKUMemberPrice WHERE GradeId = 0 AND DistributoruserId = {0} AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({1}))", HiContext.Current.User.UserId, DataHelper.CleanSearchString(productIds));
					stringBuilder.AppendFormat(" INSERT INTO distro_SKUMemberPrice (SkuId,DistributoruserId, GradeId,MemberSalePrice) SELECT SkuId, {0} AS DistributoruserId, 0 AS GradeId,", distributor.UserId);
					stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", distributor.GradeId);
					stringBuilder.AppendFormat(" THEN (SELECT DistributorPurchasePrice FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE PurchasePrice*{1}/100 END) {2} {3} AS MemberSalePrice", new object[]
					{
						distributor.GradeId,
						distributorDiscount,
						operation,
						price
					});
					stringBuilder.AppendFormat(" FROM Hishop_SKUs s WHERE SalePrice <>", new object[0]);
					stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", distributor.GradeId);
					stringBuilder.AppendFormat(" THEN (SELECT DistributorPurchasePrice FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE PurchasePrice*{1}/100 END) {2} {3}", new object[]
					{
						distributor.GradeId,
						distributorDiscount,
						operation,
						price
					});
					stringBuilder.AppendFormat(" AND ProductId IN ({0})", DataHelper.CleanSearchString(productIds));
				}
				else
				{
					if (basePriceName == "SalePrice")
					{
						stringBuilder.AppendFormat("  SELECT SkuId, {0} AS DistributoruserId, 0 AS GradeId,", distributor.UserId);
						stringBuilder.AppendFormat(" (SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = s.SkuId AND DistributoruserId = {0}) {1} {2} AS MemberSalePrice", distributor.UserId, operation, price);
						stringBuilder.AppendFormat(" INTO #myTemp FROM Hishop_SKUs s WHERE SalePrice <> (SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = s.SkuId AND DistributoruserId = {0}) {1} {2}", distributor.UserId, operation, price);
						stringBuilder.AppendFormat(" AND ProductId IN ({0})", DataHelper.CleanSearchString(productIds));
						stringBuilder.AppendFormat("DELETE FROM distro_SKUMemberPrice WHERE GradeId = 0 AND DistributoruserId = {0} AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({1}))", HiContext.Current.User.UserId, DataHelper.CleanSearchString(productIds));
						stringBuilder.AppendFormat(" INSERT INTO distro_SKUMemberPrice (SkuId,DistributoruserId, GradeId,MemberSalePrice) SELECT * FROM #myTemp", new object[0]);
						stringBuilder.Append(" DROP TABLE #myTemp");
					}
				}
			}
			else
			{
				stringBuilder.AppendFormat("DELETE FROM distro_SKUMemberPrice WHERE GradeId ={0} AND DistributoruserId = {1} AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({2}))", gradeId, distributor.UserId, DataHelper.CleanSearchString(productIds));
				if (basePriceName == "PurchasePrice")
				{
					stringBuilder.AppendFormat(" INSERT INTO distro_SKUMemberPrice (SkuId,DistributoruserId, GradeId,MemberSalePrice) SELECT SkuId, {0} AS DistributoruserId, {1} AS GradeId,", distributor.UserId, gradeId);
					stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", distributor.GradeId);
					stringBuilder.AppendFormat(" THEN (SELECT DistributorPurchasePrice FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE PurchasePrice*{1}/100 END) {2} {3} AS MemberSalePrice", new object[]
					{
						distributor.GradeId,
						distributorDiscount,
						operation,
						price
					});
					stringBuilder.AppendFormat(" FROM Hishop_SKUs s WHERE ProductId IN ({0})", DataHelper.CleanSearchString(productIds));
				}
				else
				{
					if (basePriceName == "SalePrice")
					{
						stringBuilder.AppendFormat(" INSERT INTO distro_SKUMemberPrice (SkuId,DistributoruserId, GradeId,MemberSalePrice) SELECT SkuId, {0} AS DistributoruserId, {1} AS GradeId,", distributor.UserId, gradeId);
						stringBuilder.AppendFormat(" (SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = s.SkuId AND DistributoruserId = {0}) {1} {2} AS MemberSalePrice", distributor.UserId, operation, price);
						stringBuilder.AppendFormat(" FROM Hishop_SKUs s WHERE ProductId IN ({0})", DataHelper.CleanSearchString(productIds));
					}
				}
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool UpdateSkuUnderlingPrices(System.Data.DataSet dataSet_0, string skuIds)
		{
			StringBuilder stringBuilder = new StringBuilder();
			System.Data.DataTable dataTable = dataSet_0.Tables["skuPriceTable"];
			System.Data.DataTable dataTable2 = dataSet_0.Tables["skuMemberPriceTable"];
			stringBuilder.AppendFormat(" DELETE FROM distro_SKUMemberPrice WHERE DistributoruserId = {0} AND SkuId IN ({1}) ", HiContext.Current.User.UserId, skuIds);
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				foreach (System.Data.DataRow dataRow in dataTable.Rows)
				{
					stringBuilder.AppendFormat(" INSERT INTO distro_SKUMemberPrice (SkuId, DistributoruserId, GradeId, MemberSalePrice) VALUES ('{0}', {1}, 0, {2})", dataRow["skuId"], HiContext.Current.User.UserId, dataRow["salePrice"]);
				}
			}
			if (dataTable2 != null && dataTable2.Rows.Count > 0)
			{
				foreach (System.Data.DataRow dataRow in dataTable2.Rows)
				{
					stringBuilder.AppendFormat(" INSERT INTO distro_SKUMemberPrice (SkuId, DistributoruserId, GradeId, MemberSalePrice) VALUES ('{0}', {1}, {2}, {3})", new object[]
					{
						dataRow["skuId"],
						HiContext.Current.User.UserId,
						dataRow["gradeId"],
						dataRow["memberPrice"]
					});
				}
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteNonQuery(sqlStringCommand) >= 0;
		}
		public override bool UpdateProductNames(string productIds, string prefix, string suffix)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE distro_Products SET ProductName = '{0}'+ProductName+'{1}' WHERE DistributorUserId = {2} AND ProductId IN ({3})", new object[]
			{
				DataHelper.CleanSearchString(prefix),
				DataHelper.CleanSearchString(suffix),
				HiContext.Current.User.UserId,
				productIds
			}));
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool ReplaceProductNames(string productIds, string oldWord, string newWord)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE distro_Products SET ProductName = REPLACE(ProductName, '{0}', '{1}') WHERE DistributorUserId = {2} AND ProductId IN ({3})", new object[]
			{
				DataHelper.CleanSearchString(oldWord),
				DataHelper.CleanSearchString(newWord),
				HiContext.Current.User.UserId,
				productIds
			}));
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override DbQueryResult GetRelatedProducts(Pagination page, int productId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			Distributor distributor = HiContext.Current.User as Distributor;
			stringBuilder.AppendFormat(" SaleStatus = {0} AND DistributorUserId = {1}", 1, distributor.UserId);
			stringBuilder.AppendFormat(" AND ProductId IN (SELECT RelatedProductId FROM distro_RelatedProducts WHERE ProductId = {0} AND DistributorUserId = {1})", productId, distributor.UserId);
			string text = "ProductId, ProductCode, ProductName, ThumbnailUrl40, MarketPrice, Stock, DisplaySequence,";
			text += string.Format(" (SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = p.SkuId AND DistributoruserId = {0}) AS SalePrice", distributor.UserId);
			return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "vw_distro_BrowseProductList p", "ProductId", stringBuilder.ToString(), text);
		}
		public override bool AddRelatedProduct(int productId, int relatedProductId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO distro_RelatedProducts(ProductId, DistributorUserId, RelatedProductId) VALUES (@ProductId, @DistributorUserId, @RelatedProductId)");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "RelatedProductId", System.Data.DbType.Int32, relatedProductId);
			bool result;
			try
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
			}
			catch
			{
				result = false;
			}
			return result;
		}
		public override bool RemoveRelatedProduct(int productId, int relatedProductId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_RelatedProducts WHERE ProductId = @ProductId AND RelatedProductId = @RelatedProductId AND DistributorUserId = @DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			this.database.AddInParameter(sqlStringCommand, "RelatedProductId", System.Data.DbType.Int32, relatedProductId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool ClearRelatedProducts(int productId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_RelatedProducts WHERE ProductId = @ProductId AND DistributorUserId = @DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override DbQueryResult GetExportProducts(AdvancedProductQuery query, string removeProductIds)
		{
			StringBuilder stringBuilder = new StringBuilder("");
			stringBuilder.AppendFormat("PenetrationStatus = 1 AND LineId IN (SELECT LineId FROM Hishop_DistributorProductLines WHERE UserId = {0})", HiContext.Current.User.UserId);
			if (!string.IsNullOrEmpty(query.Keywords))
			{
				query.Keywords = DataHelper.CleanSearchString(query.Keywords);
				string[] array = Regex.Split(query.Keywords.Trim(), "\\s+");
				stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[0]));
				int num = 1;
				while (num < array.Length && num <= 4)
				{
					stringBuilder.AppendFormat("AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[num]));
					num++;
				}
			}
			if (query.ProductLineId.HasValue && query.ProductLineId.Value > 0)
			{
				stringBuilder.AppendFormat(" AND LineId={0}", Convert.ToInt32(query.ProductLineId.Value));
			}
			if (query.IsMakeTaobao.HasValue && query.IsMakeTaobao != -1)
			{
				stringBuilder.AppendFormat(" AND IsMakeTaobao={0}  ", query.IsMakeTaobao);
			}
			if (query.PenetrationStatus != PenetrationStatus.NotSet)
			{
				stringBuilder.AppendFormat(" AND PenetrationStatus={0}", (int)query.PenetrationStatus);
			}
			if (!string.IsNullOrEmpty(query.ProductCode))
			{
				stringBuilder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
			}
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
			}
			if (!string.IsNullOrEmpty(removeProductIds))
			{
				stringBuilder.AppendFormat(" AND ProductId NOT IN ({0})", removeProductIds);
			}
			string selectFields = "ProductId, ProductCode, ProductName, ThumbnailUrl40, MarketPrice, SalePrice,(SELECT PurchasePrice FROM Hishop_SKUs WHERE SkuId = p.SkuId) AS  PurchasePrice, (SELECT CostPrice FROM Hishop_SKUs WHERE SkuId = p.SkuId) AS  CostPrice,  Stock, DisplaySequence,LowestSalePrice,PenetrationStatus";
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BrowseProductList p", "ProductId", stringBuilder.ToString(), selectFields);
		}
		public override System.Data.DataSet GetExportProducts(AdvancedProductQuery query, bool includeCostPrice, bool includeStock, string removeProductIds)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT a.[ProductId], [TypeId], [ProductName], [ProductCode], [ShortDescription], [Unit], [Description], ").Append("[Title], [Meta_Description], [Meta_Keywords], [SaleStatus], [ImageUrl1], [ImageUrl2], [ImageUrl3], ").Append("[ImageUrl4], [ImageUrl5], [MarketPrice], [LowestSalePrice], [PenetrationStatus], [HasSKU] ").AppendFormat("FROM Hishop_Products  a  left join Taobao_Products b on a.productid=b.productid   WHERE PenetrationStatus = 1 AND LineId IN (SELECT LineId FROM Hishop_DistributorProductLines WHERE UserId = {0})", HiContext.Current.User.UserId);
			if (!string.IsNullOrEmpty(query.Keywords))
			{
				query.Keywords = DataHelper.CleanSearchString(query.Keywords);
				string[] array = Regex.Split(query.Keywords.Trim(), "\\s+");
				stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[0]));
				int num = 1;
				while (num < array.Length && num <= 4)
				{
					stringBuilder.AppendFormat("AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(array[num]));
					num++;
				}
			}
			if (query.ProductLineId.HasValue && query.ProductLineId.Value > 0)
			{
				stringBuilder.AppendFormat(" AND LineId={0}", Convert.ToInt32(query.ProductLineId.Value));
			}
			if (query.IsMakeTaobao.HasValue && query.IsMakeTaobao != -1)
			{
				if (query.IsMakeTaobao == 1)
				{
					stringBuilder.AppendFormat(" AND a.ProductId IN (SELECT ProductId FROM Taobao_Products)", new object[0]);
				}
				else
				{
					stringBuilder.AppendFormat(" AND a.ProductId NOT IN (SELECT ProductId FROM Taobao_Products)", new object[0]);
				}
			}
			if (!string.IsNullOrEmpty(query.ProductCode))
			{
				stringBuilder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
			}
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
			}
			if (!string.IsNullOrEmpty(removeProductIds))
			{
				stringBuilder.AppendFormat(" AND a.ProductId NOT IN ({0})", removeProductIds);
			}
			stringBuilder.AppendFormat(" ORDER BY {0} {1}", query.SortBy, query.SortOrder);
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Product_GetExportList");
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", System.Data.DbType.String, stringBuilder.ToString());
			return this.database.ExecuteDataSet(storedProcCommand);
		}
		public override System.Data.DataTable GetTags()
		{
			System.Data.DataTable result = new System.Data.DataTable();
			try
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *  FROM  Hishop_Tags");
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					result = DataHelper.ConverDataReaderToDataTable(dataReader);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return result;
		}
		public override bool AddProductTags(int productId, IList<int> tagIds, System.Data.Common.DbTransaction tran)
		{
			bool flag = false;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO distro_ProductTag VALUES(@DistributorUserId,@TagId,@ProductId)");
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32);
			this.database.AddInParameter(sqlStringCommand, "TagId", System.Data.DbType.Int32);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32);
			foreach (int current in tagIds)
			{
				this.database.SetParameterValue(sqlStringCommand, "DistributorUserId", HiContext.Current.User.UserId);
				this.database.SetParameterValue(sqlStringCommand, "ProductId", productId);
				this.database.SetParameterValue(sqlStringCommand, "TagId", current);
				if (tran != null)
				{
					flag = (this.database.ExecuteNonQuery(sqlStringCommand, tran) > 0);
				}
				else
				{
					flag = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
				}
				if (!flag)
				{
					break;
				}
			}
			return flag;
		}
		public override bool DeleteProductTags(int productId, System.Data.Common.DbTransaction tran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_ProductTag WHERE ProductId=@ProductId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			bool result;
			if (tran != null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, tran) >= 0);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) >= 0);
			}
			return result;
		}
		public override IList<int> GetProductTags(int productId)
		{
			IList<int> list = new List<int>();
			try
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *  FROM  distro_ProductTag WHERE ProductId=@ProductId AND DistributorUserId=@DistributorUserId");
				this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
				this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					while (dataReader.Read())
					{
						list.Add((int)dataReader["TagId"]);
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return list;
		}
		public override DbQueryResult GetToTaobaoProducts(ProductQuery query)
		{
			Distributor distributor = Users.GetUser(query.UserId.Value) as Distributor;
			int distributorDiscount = this.GetDistributorDiscount(distributor.GradeId);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("ProductId IN (SELECT ProductId FROM Taobao_Products)");
			stringBuilder.AppendFormat(" AND PenetrationStatus=1 AND LineId IN (SELECT LineId FROM Hishop_DistributorProductLines WHERE UserId={0}) And SaleStatus<>{1}", query.UserId.Value, 0);
			if (!string.IsNullOrEmpty(query.ProductCode))
			{
				stringBuilder.AppendFormat(" AND ProductCode = '{0}'", DataHelper.CleanSearchString(query.ProductCode));
			}
			if (!string.IsNullOrEmpty(query.Keywords))
			{
				stringBuilder.AppendFormat(" AND ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Keywords));
			}
			if (query.CategoryId.HasValue && query.CategoryId.Value > 0)
			{
				stringBuilder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%')", query.MaiCategoryPath);
			}
			if (query.ProductLineId.HasValue)
			{
				stringBuilder.AppendFormat(" AND LineId = {0}", query.ProductLineId);
			}
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
			}
			if (query.PublishStatus == PublishStatus.Already)
			{
				stringBuilder.AppendFormat(" AND ProductId IN (SELECT ProductId FROM Taobao_DistroProducts WHERE DistributorUserId = {0})", distributor.UserId);
			}
			else
			{
				if (query.PublishStatus == PublishStatus.Notyet)
				{
					stringBuilder.AppendFormat(" AND ProductId NOT IN (SELECT ProductId FROM Taobao_DistroProducts WHERE DistributorUserId = {0})", distributor.UserId);
				}
				else
				{
					if (query.PublishStatus == PublishStatus.Update)
					{
						stringBuilder.AppendFormat(" AND ProductId  IN (SELECT ProductId FROM Taobao_DistroProducts WHERE DistributorUserId = {0} and updatestatus=1)", distributor.UserId);
					}
				}
			}
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.Append("ProductId, ProductCode, ProductName, ThumbnailUrl40, MarketPrice, SalePrice,LowestSalePrice, Stock, DisplaySequence,");
			stringBuilder2.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUDistributorPrice WHERE SkuId = p.SkuId AND GradeId = {0}) = 1", distributor.GradeId);
			stringBuilder2.AppendFormat(" THEN (SELECT DistributorPurchasePrice FROM Hishop_SKUDistributorPrice WHERE SkuId = p.SkuId AND GradeId = {0})", distributor.GradeId);
			stringBuilder2.AppendFormat(" ELSE (SELECT PurchasePrice FROM Hishop_SKUs WHERE SkuId = p.SkuId)*{0}/100 END) AS PurchasePrice,", distributorDiscount);
			stringBuilder2.AppendFormat(" (SELECT updatestatus FROM Taobao_DistroProducts WHERE DistributorUserId ={0} AND ProductId = p.ProductId) AS IsPublish", distributor.UserId);
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BrowseProductList p", "ProductId", stringBuilder.ToString(), stringBuilder2.ToString());
		}
		public override PublishToTaobaoProductInfo GetTaobaoProduct(int productId, int distributorId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT tp.*, b.taobaoproductid,p.ProductCode, p.Description, p.ImageUrl1, p.ImageUrl2, p.ImageUrl3, p.ImageUrl4, p.ImageUrl5,");
			stringBuilder.Append(" (SELECT MIN(SalePrice) FROM Hishop_SKUs WHERE ProductId = p.ProductId) AS SalePrice,");
			stringBuilder.Append(" (SELECT MIN(Weight) FROM Hishop_SKUs WHERE ProductId = p.ProductId) AS Weight");
			stringBuilder.Append(" FROM Hishop_Products p JOIN Taobao_Products tp ON p.ProductId = tp.ProductId");
			stringBuilder.AppendFormat(" left join (select * from taobao_distroproducts where distributoruserid={0}) b", distributorId);
			stringBuilder.AppendFormat(" on b.ProductId=tp.ProductId  WHERE p.ProductId = {0}", productId);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			PublishToTaobaoProductInfo publishToTaobaoProductInfo = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					publishToTaobaoProductInfo = new PublishToTaobaoProductInfo();
					publishToTaobaoProductInfo.Cid = (long)dataReader["Cid"];
					if (dataReader["StuffStatus"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.StuffStatus = (string)dataReader["StuffStatus"];
					}
					publishToTaobaoProductInfo.ProductId = (int)dataReader["ProductId"];
					publishToTaobaoProductInfo.ProTitle = (string)dataReader["ProTitle"];
					publishToTaobaoProductInfo.Num = (long)dataReader["Num"];
					publishToTaobaoProductInfo.LocationState = (string)dataReader["LocationState"];
					publishToTaobaoProductInfo.LocationCity = (string)dataReader["LocationCity"];
					publishToTaobaoProductInfo.FreightPayer = (string)dataReader["FreightPayer"];
					if (dataReader["PostFee"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.PostFee = (decimal)dataReader["PostFee"];
					}
					if (dataReader["ExpressFee"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.ExpressFee = (decimal)dataReader["ExpressFee"];
					}
					if (dataReader["EMSFee"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.EMSFee = (decimal)dataReader["EMSFee"];
					}
					publishToTaobaoProductInfo.HasInvoice = (bool)dataReader["HasInvoice"];
					publishToTaobaoProductInfo.HasWarranty = (bool)dataReader["HasWarranty"];
					publishToTaobaoProductInfo.HasDiscount = (bool)dataReader["HasDiscount"];
					publishToTaobaoProductInfo.ValidThru = (long)dataReader["ValidThru"];
					if (dataReader["ListTime"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.ListTime = (DateTime)dataReader["ListTime"];
					}
					if (dataReader["PropertyAlias"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.PropertyAlias = (string)dataReader["PropertyAlias"];
					}
					if (dataReader["InputPids"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.InputPids = (string)dataReader["InputPids"];
					}
					if (dataReader["InputStr"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.InputStr = (string)dataReader["InputStr"];
					}
					if (dataReader["SkuProperties"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.SkuProperties = (string)dataReader["SkuProperties"];
					}
					if (dataReader["SkuQuantities"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.SkuQuantities = (string)dataReader["SkuQuantities"];
					}
					if (dataReader["SkuPrices"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.SkuPrices = (string)dataReader["SkuPrices"];
					}
					if (dataReader["SkuOuterIds"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.SkuOuterIds = (string)dataReader["SkuOuterIds"];
					}
					if (dataReader["FoodAttributes"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.FoodAttributes = (string)dataReader["FoodAttributes"];
					}
					if (dataReader["TaobaoProductId"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.TaobaoProductId = (long)dataReader["TaobaoProductId"];
					}
					if (dataReader["ProductCode"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.ProductCode = (string)dataReader["ProductCode"];
					}
					if (dataReader["Description"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.Description = (string)dataReader["Description"];
					}
					if (dataReader["ImageUrl1"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.ImageUrl1 = (string)dataReader["ImageUrl1"];
					}
					if (dataReader["ImageUrl2"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.ImageUrl2 = (string)dataReader["ImageUrl2"];
					}
					if (dataReader["ImageUrl3"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.ImageUrl3 = (string)dataReader["ImageUrl3"];
					}
					if (dataReader["ImageUrl4"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.ImageUrl4 = (string)dataReader["ImageUrl4"];
					}
					if (dataReader["ImageUrl5"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.ImageUrl5 = (string)dataReader["ImageUrl5"];
					}
					publishToTaobaoProductInfo.SalePrice = (decimal)dataReader["SalePrice"];
					if (dataReader["Weight"] != DBNull.Value)
					{
						publishToTaobaoProductInfo.Weight = (decimal)dataReader["Weight"];
					}
				}
			}
			return publishToTaobaoProductInfo;
		}
		public override bool AddTaobaoProductId(int productId, long taobaoProductId, int distributorId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("DELETE FROM Taobao_DistroProducts WHERE DistributorUserId = {0} AND ProductId  = {1};", distributorId, productId) + string.Format(" INSERT INTO Taobao_DistroProducts (DistributorUserId, ProductId, TaobaoProductId,UpdateStatus) VALUES ({0}, {1}, {2}, 0)", distributorId, productId, taobaoProductId));
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override string GetSkuIdByTaobao(long taobaoProductId, string taobaoSkuId, int distributorId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN (SELECT ProductId FROM Taobao_DistroProducts WHERE DistributorUserId = @DistributorUserId AND TaobaoProductId = @TaobaoProductId)");
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, distributorId);
			this.database.AddInParameter(sqlStringCommand, "TaobaoProductId", System.Data.DbType.Int64, taobaoProductId);
			string text = string.Empty;
			string result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					text = (string)dataReader["SkuId"];
					if (taobaoSkuId.ToLower() == text.ToLower())
					{
						result = text;
						return result;
					}
				}
			}
			result = text;
			return result;
		}
		public override System.Data.DataTable GetSkuContentBySku(string skuId, int distributorId)
		{
			System.Data.DataTable result = null;
			Distributor distributor = Users.GetUser(distributorId) as Distributor;
			int distributorDiscount = this.GetDistributorDiscount(distributor.GradeId);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT s.SkuId, ProductId, SKU,Weight, Stock, AlertStock, CostPrice,");
			stringBuilder.AppendFormat(" ISNULL((SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = s.SkuId AND DistributoruserId = {0}), s.SalePrice) AS SalePrice,", distributor.UserId);
			stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", distributor.GradeId);
			stringBuilder.AppendFormat(" THEN (SELECT DistributorPurchasePrice FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE PurchasePrice*{1}/100 END) AS PurchasePrice,", distributor.GradeId, distributorDiscount);
			stringBuilder.Append(" (SELECT ProductName FROM Hishop_Products WHERE ProductId = s.ProductId) AS ProductName,");
			stringBuilder.Append(" (SELECT ThumbnailUrl40 FROM Hishop_Products WHERE ProductId = s.ProductId) AS ThumbnailUrl40,AttributeName, ValueStr");
			stringBuilder.Append(" FROM Hishop_SKUs s left join Hishop_SKUItems si on s.SkuId = si.SkuId");
			stringBuilder.Append(" left join Hishop_Attributes a on si.AttributeId = a.AttributeId left join Hishop_AttributeValues av on si.ValueId = av.ValueId WHERE s.SkuId = @SkuId");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, skuId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
	}
}
