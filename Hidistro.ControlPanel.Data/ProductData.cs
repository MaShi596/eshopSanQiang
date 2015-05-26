using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.HOP;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
namespace Hidistro.ControlPanel.Data
{
	public class ProductData : ProductProvider
	{
		private Database database;
		public ProductData()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public override DbQueryResult GetUnclassifiedProducts(ProductQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Keywords));
			if (!string.IsNullOrEmpty(query.ProductCode))
			{
				stringBuilder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
			}
			if (query.ProductLineId.HasValue)
			{
				stringBuilder.AppendFormat(" AND LineId={0}", Convert.ToInt32(query.ProductLineId.Value));
			}
			if (query.BrandId.HasValue)
			{
				stringBuilder.AppendFormat(" AND BrandId={0}", Convert.ToInt32(query.BrandId.Value));
			}
			if (query.TypeId.HasValue)
			{
				stringBuilder.AppendFormat(" AND TypeId = {0}", query.TypeId.Value);
			}
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
			stringBuilder.AppendFormat(" AND SaleStatus!={0}", (int)query.SaleStatus);
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BrowseProductList", "ProductId", stringBuilder.ToString(), "CategoryId,MainCategoryPath,ExtendCategoryPath, ProductId, ProductCode, ProductName, ThumbnailUrl40, MarketPrice, SalePrice, Stock, DisplaySequence");
		}
		public override ProductInfo GetProductDetails(int productId, out Dictionary<int, IList<int>> attrs, out IList<int> distributorUserIds, out IList<int> tagsId)
		{
			ProductInfo productInfo = null;
			attrs = null;
			distributorUserIds = new List<int>();
			tagsId = new List<int>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Products WHERE ProductId = @ProductId;SELECT skus.ProductId, skus.SkuId, s.AttributeId, s.ValueId, skus.SKU, skus.SalePrice, skus.CostPrice, skus.PurchasePrice, skus.Stock, skus.AlertStock, skus.[Weight] FROM Hishop_SKUItems s right outer join Hishop_SKUs skus on s.SkuId = skus.SkuId WHERE skus.ProductId = @ProductId ORDER BY (SELECT DisplaySequence FROM Hishop_Attributes WHERE AttributeId = s.AttributeId) DESC;SELECT s.SkuId, smp.GradeId, smp.MemberSalePrice FROM Hishop_SKUMemberPrice smp INNER JOIN Hishop_SKUs s ON smp.SkuId=s.SkuId WHERE s.ProductId=@ProductId;SELECT s.SkuId, sdp.GradeId, sdp.DistributorPurchasePrice FROM Hishop_SKUDistributorPrice sdp INNER JOIN Hishop_SKUs s ON sdp.SkuId=s.SkuId WHERE s.ProductId=@ProductId;SELECT AttributeId, ValueId FROM Hishop_ProductAttributes WHERE ProductId = @ProductId; SELECT UserId FROM Hishop_DistributorProductLines WHERE LineId = (SELECT LineId FROM Hishop_Products WHERE ProductId = @ProductId);SELECT * FROM Hishop_ProductTag WHERE ProductId=@ProductId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					productInfo = DataMapper.PopulateProduct(dataReader);
				}
				if (productInfo != null)
				{
					dataReader.NextResult();
					while (dataReader.Read())
					{
						string key = (string)dataReader["SkuId"];
						if (!productInfo.Skus.ContainsKey(key))
						{
							productInfo.Skus.Add(key, DataMapper.PopulateSKU(dataReader));
						}
						if (dataReader["AttributeId"] != DBNull.Value && dataReader["ValueId"] != DBNull.Value)
						{
							productInfo.Skus[key].SkuItems.Add((int)dataReader["AttributeId"], (int)dataReader["ValueId"]);
						}
					}
					dataReader.NextResult();
					while (dataReader.Read())
					{
						string key = (string)dataReader["SkuId"];
						productInfo.Skus[key].MemberPrices.Add((int)dataReader["GradeId"], (decimal)dataReader["MemberSalePrice"]);
					}
					dataReader.NextResult();
					while (dataReader.Read())
					{
						string key = (string)dataReader["SkuId"];
						productInfo.Skus[key].DistributorPrices.Add((int)dataReader["GradeId"], (decimal)dataReader["DistributorPurchasePrice"]);
					}
					dataReader.NextResult();
					attrs = new Dictionary<int, IList<int>>();
					while (dataReader.Read())
					{
						int key2 = (int)dataReader["AttributeId"];
						int item = (int)dataReader["ValueId"];
						if (!attrs.ContainsKey(key2))
						{
							IList<int> list = new List<int>();
							list.Add(item);
							attrs.Add(key2, list);
						}
						else
						{
							attrs[key2].Add(item);
						}
					}
					dataReader.NextResult();
					while (dataReader.Read())
					{
						distributorUserIds.Add((int)dataReader["UserId"]);
					}
					dataReader.NextResult();
					while (dataReader.Read())
					{
						tagsId.Add((int)dataReader["TagId"]);
					}
				}
			}
			return productInfo;
		}
		public override DbQueryResult GetProducts(ProductQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1 ");
			if (query.SaleStatus != ProductSaleStatus.All)
			{
				stringBuilder.AppendFormat(" AND SaleStatus = {0}", (int)query.SaleStatus);
			}
			else
			{
				stringBuilder.AppendFormat(" AND SaleStatus <> ({0})", 0);
			}
			if (query.UserId.HasValue)
			{
				stringBuilder.AppendFormat(" AND ProductId IN(SELECT ProductId FROM distro_Products WHERE DistributorUserId = {0})", query.UserId.Value);
			}
			if (query.BrandId.HasValue)
			{
				stringBuilder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
			}
			if (query.TypeId.HasValue)
			{
				stringBuilder.AppendFormat(" AND TypeId = {0}", query.TypeId.Value);
			}
			if (query.TagId.HasValue)
			{
				stringBuilder.AppendFormat(" AND ProductId IN (SELECT ProductId FROM Hishop_ProductTag WHERE TagId={0})", query.TagId);
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
			if (query.ProductLineId.HasValue)
			{
				stringBuilder.AppendFormat(" AND LineId={0}", Convert.ToInt32(query.ProductLineId.Value));
			}
			if (query.PenetrationStatus != PenetrationStatus.NotSet)
			{
				stringBuilder.AppendFormat(" AND PenetrationStatus={0}", (int)query.PenetrationStatus);
			}
			if (query.IsMakeTaobao.HasValue && query.IsMakeTaobao.Value >= 0)
			{
				stringBuilder.AppendFormat(" AND IsMaketaobao={0}", query.IsMakeTaobao.Value);
			}
			if (query.IsIncludePromotionProduct.HasValue && !query.IsIncludePromotionProduct.Value)
			{
				stringBuilder.Append(" AND ProductId NOT IN (SELECT ProductId FROM Hishop_PromotionProducts)");
			}
			if (query.IsIncludeBundlingProduct.HasValue && !query.IsIncludeBundlingProduct.Value)
			{
				stringBuilder.Append(" AND ProductId NOT IN (SELECT ProductID FROM Hishop_BundlingProductItems)");
			}
			if (!string.IsNullOrEmpty(query.ProductCode))
			{
				stringBuilder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
			}
			if (query.CategoryId.HasValue && query.CategoryId.Value > 0)
			{
				stringBuilder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%'  OR ExtendCategoryPath LIKE '{0}|%' )", query.MaiCategoryPath);
			}
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
			}
			if (query.IsAlert)
			{
				stringBuilder.Append(" AND ProductId IN (SELECT DISTINCT ProductId FROM Hishop_SKUs WHERE Stock <= AlertStock)");
			}
			string selectFields = "ProductId, ProductCode,IsMakeTaobao,ProductName, ThumbnailUrl40, MarketPrice, SalePrice,(SELECT PurchasePrice FROM Hishop_SKUs WHERE SkuId = p.SkuId) AS  PurchasePrice, (SELECT CostPrice FROM Hishop_SKUs WHERE SkuId = p.SkuId) AS  CostPrice,  Stock, DisplaySequence,LowestSalePrice,SaleStatus,PenetrationStatus";
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BrowseProductList p", "ProductId", stringBuilder.ToString(), selectFields);
		}
		public override System.Data.DataTable GetGroupBuyProducts(ProductQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" WHERE SaleStatus = {0}", 1);
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ProductId,ProductName FROM Hishop_Products" + stringBuilder.ToString());
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
				text = text.Substring(0, text.Length - 1) + ")";
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Products WHERE ProductId IN " + text);
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					while (dataReader.Read())
					{
						list.Add(DataMapper.PopulateProduct(dataReader));
					}
				}
				result = list;
			}
			return result;
		}
		public override DbQueryResult GetBindProducts(ProductQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("  SaleStatus! = {0}", 0);
			if (query.CategoryId.HasValue && query.CategoryId.Value > 0)
			{
				stringBuilder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%'  OR ExtendCategoryPath LIKE '{0}|%' )", query.MaiCategoryPath);
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
			if (query.ProductLineId.HasValue && query.ProductLineId.Value > 0)
			{
				stringBuilder.AppendFormat(" AND LineId={0}", Convert.ToInt32(query.ProductLineId.Value));
			}
			if (query.BrandId.HasValue)
			{
				stringBuilder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
			}
			if (!string.IsNullOrEmpty(query.ProductCode))
			{
				stringBuilder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_Products", "ProductId", stringBuilder.ToString(), "ProductId, ProductCode, ProductName,ThumbnailUrl40, ThumbnailUrl60, ThumbnailUrl100, MarketPrice,DisplaySequence,LowestSalePrice, PenetrationStatus");
		}
		public override System.Data.DataTable GetSkusByProductId(int productId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock, AlertStock, CostPrice,saleprice");
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
		public override IList<int> GetProductIds(ProductQuery query)
		{
			IList<int> list = new List<int>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(ProductData.BuildProductQuery(query));
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add((int)dataReader["ProductId"]);
				}
			}
			return list;
		}
		public override DbQueryResult GetSubjectProducts(int tagId, Pagination page)
		{
			return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "vw_Hishop_BrowseProductList", "ProductId", string.Format("SaleStatus!={0} and ProductId IN (SELECT ProductId FROM Hishop_ProductTag WHERE TagId = {1})", 0, tagId), "ProductId, ProductCode, ProductName, ThumbnailUrl40, MarketPrice, SalePrice, Stock, DisplaySequence");
		}
		public override string GetProductNameByProductIds(string productIds, out int sumcount)
		{
			int num = 0;
			string text = "";
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT ProductName FROM Hishop_Products WHERE PenetrationStatus=1", new object[0]);
			stringBuilder.AppendFormat(" AND SaleStatus!={0}", 0);
			stringBuilder.AppendFormat(" AND ProductId IN ({0})", productIds);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					text = text + (string)dataReader["ProductName"] + ",";
					num++;
				}
			}
			if (text != "")
			{
				text = text.Substring(0, text.Length - 1);
			}
			sumcount = num;
			return text;
		}
		public override bool AddProductLine(ProductLineInfo productLine)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_ProductLines(Name, SupplierName) VALUES(@Name, @SupplierName)");
			this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, productLine.Name);
			this.database.AddInParameter(sqlStringCommand, "SupplierName", System.Data.DbType.String, productLine.SupplierName);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override bool UpdateProductLine(ProductLineInfo productLine)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_ProductLines SET Name=@Name, SupplierName=@SupplierName WHERE LineId=@LineId");
			this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, productLine.Name);
			this.database.AddInParameter(sqlStringCommand, "SupplierName", System.Data.DbType.String, productLine.SupplierName);
			this.database.AddInParameter(sqlStringCommand, "LineId", System.Data.DbType.Int32, productLine.LineId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override bool DeleteProductLine(int lineId)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ProductLine_Delete");
			this.database.AddInParameter(storedProcCommand, "LineId", System.Data.DbType.Int32, lineId);
			return this.database.ExecuteNonQuery(storedProcCommand) > 0;
		}
		public override ProductLineInfo GetProductLine(int lineId)
		{
			ProductLineInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ProductLines WHERE LineId=@LineId");
			this.database.AddInParameter(sqlStringCommand, "LineId", System.Data.DbType.Int32, lineId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateProductLine(dataReader);
				}
			}
			return result;
		}
		public override bool ReplaceProductLine(int fromlineId, int replacelineId)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ProductLine_Replace");
			this.database.AddInParameter(storedProcCommand, "OldLineId", System.Data.DbType.Int32, fromlineId);
			this.database.AddInParameter(storedProcCommand, "NewLineId", System.Data.DbType.Int32, replacelineId);
			return this.database.ExecuteNonQuery(storedProcCommand) > 0;
		}
		public override bool UpdateProductLine(int lineId, int productId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("delete from distro_Products where ProductId=@productId and DistributorUserId ");
			stringBuilder.Append(" in (select UserId from Hishop_DistributorProductLines where LineId in ");
			stringBuilder.Append("(select LineId from Hishop_Products where ProductId=@productId))");
			stringBuilder.Append("update Hishop_Products set LineId=@lineId where ProductId=@productId");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "@productId", System.Data.DbType.Int32, productId);
			this.database.AddInParameter(sqlStringCommand, "@lineId", System.Data.DbType.Int32, lineId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override System.Data.DataTable GetProductLines()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT LineId,Name,SupplierName,(SELECT count(*) From Hishop_Products WHERE LineId=pl.LineId AND SaleStatus<>0) AS ProductCount FROM Hishop_ProductLines pl");
			System.Data.DataTable result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
				dataReader.Close();
			}
			return result;
		}
		public override IList<ProductLineInfo> GetProductLineList()
		{
			IList<ProductLineInfo> list = new List<ProductLineInfo>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ProductLines");
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulateProductLine(dataReader));
				}
			}
			return list;
		}
		public override int AddProduct(ProductInfo product, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Product_Create");
			this.database.AddInParameter(storedProcCommand, "CategoryId", System.Data.DbType.Int32, product.CategoryId);
			this.database.AddInParameter(storedProcCommand, "MainCategoryPath", System.Data.DbType.String, product.MainCategoryPath);
			this.database.AddInParameter(storedProcCommand, "TypeId", System.Data.DbType.Int32, product.TypeId);
			this.database.AddInParameter(storedProcCommand, "ProductName", System.Data.DbType.String, product.ProductName);
			this.database.AddInParameter(storedProcCommand, "ProductCode", System.Data.DbType.String, product.ProductCode);
			this.database.AddInParameter(storedProcCommand, "ShortDescription", System.Data.DbType.String, product.ShortDescription);
			this.database.AddInParameter(storedProcCommand, "Unit", System.Data.DbType.String, product.Unit);
			this.database.AddInParameter(storedProcCommand, "Description", System.Data.DbType.String, product.Description);
			this.database.AddInParameter(storedProcCommand, "Title", System.Data.DbType.String, product.Title);
			this.database.AddInParameter(storedProcCommand, "Meta_Description", System.Data.DbType.String, product.MetaDescription);
			this.database.AddInParameter(storedProcCommand, "Meta_Keywords", System.Data.DbType.String, product.MetaKeywords);
			this.database.AddInParameter(storedProcCommand, "SaleStatus", System.Data.DbType.Int32, (int)product.SaleStatus);
			this.database.AddInParameter(storedProcCommand, "AddedDate", System.Data.DbType.DateTime, product.AddedDate);
			this.database.AddInParameter(storedProcCommand, "ImageUrl1", System.Data.DbType.String, product.ImageUrl1);
			this.database.AddInParameter(storedProcCommand, "ImageUrl2", System.Data.DbType.String, product.ImageUrl2);
			this.database.AddInParameter(storedProcCommand, "ImageUrl3", System.Data.DbType.String, product.ImageUrl3);
			this.database.AddInParameter(storedProcCommand, "ImageUrl4", System.Data.DbType.String, product.ImageUrl4);
			this.database.AddInParameter(storedProcCommand, "ImageUrl5", System.Data.DbType.String, product.ImageUrl5);
			this.database.AddInParameter(storedProcCommand, "ThumbnailUrl40", System.Data.DbType.String, product.ThumbnailUrl40);
			this.database.AddInParameter(storedProcCommand, "ThumbnailUrl60", System.Data.DbType.String, product.ThumbnailUrl60);
			this.database.AddInParameter(storedProcCommand, "ThumbnailUrl100", System.Data.DbType.String, product.ThumbnailUrl100);
			this.database.AddInParameter(storedProcCommand, "ThumbnailUrl160", System.Data.DbType.String, product.ThumbnailUrl160);
			this.database.AddInParameter(storedProcCommand, "ThumbnailUrl180", System.Data.DbType.String, product.ThumbnailUrl180);
			this.database.AddInParameter(storedProcCommand, "ThumbnailUrl220", System.Data.DbType.String, product.ThumbnailUrl220);
			this.database.AddInParameter(storedProcCommand, "ThumbnailUrl310", System.Data.DbType.String, product.ThumbnailUrl310);
			this.database.AddInParameter(storedProcCommand, "ThumbnailUrl410", System.Data.DbType.String, product.ThumbnailUrl410);
			this.database.AddInParameter(storedProcCommand, "LineId", System.Data.DbType.Int32, product.LineId);
			this.database.AddInParameter(storedProcCommand, "MarketPrice", System.Data.DbType.Currency, product.MarketPrice);
			this.database.AddInParameter(storedProcCommand, "LowestSalePrice", System.Data.DbType.Currency, product.LowestSalePrice);
			this.database.AddInParameter(storedProcCommand, "PenetrationStatus", System.Data.DbType.Int16, (int)product.PenetrationStatus);
			this.database.AddInParameter(storedProcCommand, "BrandId", System.Data.DbType.Int32, product.BrandId);
			this.database.AddInParameter(storedProcCommand, "HasSKU", System.Data.DbType.Boolean, product.HasSKU);
			this.database.AddInParameter(storedProcCommand, "TaobaoProductId", System.Data.DbType.Int64, product.TaobaoProductId);
			this.database.AddOutParameter(storedProcCommand, "ProductId", System.Data.DbType.Int32, 4);
			this.database.ExecuteNonQuery(storedProcCommand, dbTran);
			return (int)this.database.GetParameterValue(storedProcCommand, "ProductId");
		}
		private decimal Opreateion(decimal opreation1, decimal opreation2, string operation)
		{
			decimal result = 0m;
			if (operation != null)
			{
				if (!(operation == "+"))
				{
					if (!(operation == "-"))
					{
						if (!(operation == "*"))
						{
							if (operation == "/")
							{
								result = opreation1 * opreation2;
							}
						}
						else
						{
							result = opreation1 * opreation2;
						}
					}
					else
					{
						result = opreation1 - opreation2;
					}
				}
				else
				{
					result = opreation1 + opreation2;
				}
			}
			return result;
		}
		public override bool AddProductSKUs(int productId, Dictionary<string, SKUItem> skus, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_SKUs(SkuId, ProductId, SKU, Weight, Stock, AlertStock, CostPrice, SalePrice, PurchasePrice) VALUES(@SkuId, @ProductId, @SKU, @Weight, @Stock, @AlertStock, @CostPrice, @SalePrice, @PurchasePrice)");
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32);
			this.database.AddInParameter(sqlStringCommand, "SKU", System.Data.DbType.String);
			this.database.AddInParameter(sqlStringCommand, "Weight", System.Data.DbType.Decimal);
			this.database.AddInParameter(sqlStringCommand, "Stock", System.Data.DbType.Int32);
			this.database.AddInParameter(sqlStringCommand, "AlertStock", System.Data.DbType.Int32);
			this.database.AddInParameter(sqlStringCommand, "CostPrice", System.Data.DbType.Currency);
			this.database.AddInParameter(sqlStringCommand, "SalePrice", System.Data.DbType.Currency);
			this.database.AddInParameter(sqlStringCommand, "PurchasePrice", System.Data.DbType.Currency);
			System.Data.Common.DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand("INSERT INTO Hishop_SKUItems(SkuId, AttributeId, ValueId) VALUES(@SkuId, @AttributeId, @ValueId)");
			this.database.AddInParameter(sqlStringCommand2, "SkuId", System.Data.DbType.String);
			this.database.AddInParameter(sqlStringCommand2, "AttributeId", System.Data.DbType.Int32);
			this.database.AddInParameter(sqlStringCommand2, "ValueId", System.Data.DbType.Int32);
			System.Data.Common.DbCommand sqlStringCommand3 = this.database.GetSqlStringCommand("INSERT INTO Hishop_SKUMemberPrice(SkuId, GradeId, MemberSalePrice) VALUES(@SkuId, @GradeId, @MemberSalePrice)");
			this.database.AddInParameter(sqlStringCommand3, "SkuId", System.Data.DbType.String);
			this.database.AddInParameter(sqlStringCommand3, "GradeId", System.Data.DbType.Int32);
			this.database.AddInParameter(sqlStringCommand3, "MemberSalePrice", System.Data.DbType.Currency);
			System.Data.Common.DbCommand sqlStringCommand4 = this.database.GetSqlStringCommand("INSERT INTO Hishop_SKUDistributorPrice(SkuId, GradeId, DistributorPurchasePrice) VALUES(@SkuId, @GradeId, @DistributorPurchasePrice)");
			this.database.AddInParameter(sqlStringCommand4, "SkuId", System.Data.DbType.String);
			this.database.AddInParameter(sqlStringCommand4, "GradeId", System.Data.DbType.Int32);
			this.database.AddInParameter(sqlStringCommand4, "DistributorPurchasePrice", System.Data.DbType.Currency);
			bool result;
			try
			{
				this.database.SetParameterValue(sqlStringCommand, "ProductId", productId);
				foreach (SKUItem current in skus.Values)
				{
					string text = productId.ToString(CultureInfo.InvariantCulture) + "_" + current.SkuId;
					this.database.SetParameterValue(sqlStringCommand, "SkuId", text);
					this.database.SetParameterValue(sqlStringCommand, "SKU", current.SKU);
					this.database.SetParameterValue(sqlStringCommand, "Weight", current.Weight);
					this.database.SetParameterValue(sqlStringCommand, "Stock", current.Stock);
					this.database.SetParameterValue(sqlStringCommand, "AlertStock", current.AlertStock);
					this.database.SetParameterValue(sqlStringCommand, "CostPrice", current.CostPrice);
					this.database.SetParameterValue(sqlStringCommand, "SalePrice", Math.Round(current.SalePrice, HiContext.Current.SiteSettings.DecimalLength));
					this.database.SetParameterValue(sqlStringCommand, "PurchasePrice", current.PurchasePrice);
					if (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 0)
					{
						result = false;
						return result;
					}
					this.database.SetParameterValue(sqlStringCommand2, "SkuId", text);
					foreach (int current2 in current.SkuItems.Keys)
					{
						this.database.SetParameterValue(sqlStringCommand2, "AttributeId", current2);
						this.database.SetParameterValue(sqlStringCommand2, "ValueId", current.SkuItems[current2]);
						this.database.ExecuteNonQuery(sqlStringCommand2, dbTran);
					}
					this.database.SetParameterValue(sqlStringCommand3, "SkuId", text);
					foreach (int current3 in current.MemberPrices.Keys)
					{
						this.database.SetParameterValue(sqlStringCommand3, "GradeId", current3);
						this.database.SetParameterValue(sqlStringCommand3, "MemberSalePrice", current.MemberPrices[current3]);
						this.database.ExecuteNonQuery(sqlStringCommand3, dbTran);
					}
					this.database.SetParameterValue(sqlStringCommand4, "SkuId", text);
					foreach (int current3 in current.DistributorPrices.Keys)
					{
						this.database.SetParameterValue(sqlStringCommand4, "GradeId", current3);
						this.database.SetParameterValue(sqlStringCommand4, "DistributorPurchasePrice", current.DistributorPrices[current3]);
						this.database.ExecuteNonQuery(sqlStringCommand4, dbTran);
					}
				}
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}
		public override bool DeleteProductSKUS(int productId, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_SKUs WHERE ProductId = @ProductId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			bool result;
			try
			{
				if (dbTran == null)
				{
					this.database.ExecuteNonQuery(sqlStringCommand);
				}
				else
				{
					this.database.ExecuteNonQuery(sqlStringCommand, dbTran);
				}
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}
		public override bool AddProductAttributes(int productId, Dictionary<int, IList<int>> attributes, System.Data.Common.DbTransaction dbTran)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("DELETE FROM Hishop_ProductAttributes WHERE ProductId = {0};", productId);
			int num = 0;
			if (attributes != null)
			{
				foreach (int current in attributes.Keys)
				{
					foreach (int current2 in attributes[current])
					{
						num++;
						stringBuilder.AppendFormat(" INSERT INTO Hishop_ProductAttributes (ProductId, AttributeId, ValueId) VALUES ({0}, {1}, {2})", productId, current, current2);
					}
				}
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			bool result;
			if (dbTran == null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) >= 0);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) >= 0);
			}
			return result;
		}
		public override bool OffShelfProductExcludedSalePrice(int productId, decimal lowestSalePrice, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Products  SET SaleStatus=3 WHERE ProductId = @ProductId AND (SELECT MIN(SalePrice) FROM vw_distro_SkuPrices WHERE DistributoruserId = distro_Products.DistributoruserId AND SkuId IN (SELECT SkuId FROM Hishop_Skus WHERE ProductId = @ProductId)) < @LowestSalePrice");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			this.database.AddInParameter(sqlStringCommand, "LowestSalePrice", System.Data.DbType.Currency, lowestSalePrice);
			return this.database.ExecuteNonQuery(sqlStringCommand, dbTran) >= 0;
		}
		public override void DeleteNotinProductLines(int distributorUserId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_Products WHERE DistributorUserId=@DistributorUserId AND LineId NOT IN (SELECT LineId FROM Hishop_DistributorProductLines WHERE UserId=@DistributorUserId)");
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, distributorUserId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override void DeleteSkuUnderlingPrice()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_SKUMemberPrice WHERE SkuId NOT IN (SELECT SkuId FROM Hishop_SKUs)");
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override int GetMaxSequence()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT MAX(DisplaySequence) FROM Hishop_Products");
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			return (obj == DBNull.Value) ? 0 : ((int)obj);
		}
		public override bool UpdateProduct(ProductInfo product, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Product_Update");
			this.database.AddInParameter(storedProcCommand, "CategoryId", System.Data.DbType.Int32, product.CategoryId);
			this.database.AddInParameter(storedProcCommand, "MainCategoryPath", System.Data.DbType.String, product.MainCategoryPath);
			this.database.AddInParameter(storedProcCommand, "TypeId", System.Data.DbType.Int32, product.TypeId);
			this.database.AddInParameter(storedProcCommand, "ProductName", System.Data.DbType.String, product.ProductName);
			this.database.AddInParameter(storedProcCommand, "ProductCode", System.Data.DbType.String, product.ProductCode);
			this.database.AddInParameter(storedProcCommand, "ShortDescription", System.Data.DbType.String, product.ShortDescription);
			this.database.AddInParameter(storedProcCommand, "Unit", System.Data.DbType.String, product.Unit);
			this.database.AddInParameter(storedProcCommand, "Description", System.Data.DbType.String, product.Description);
			this.database.AddInParameter(storedProcCommand, "Title", System.Data.DbType.String, product.Title);
			this.database.AddInParameter(storedProcCommand, "Meta_Description", System.Data.DbType.String, product.MetaDescription);
			this.database.AddInParameter(storedProcCommand, "Meta_Keywords", System.Data.DbType.String, product.MetaKeywords);
			this.database.AddInParameter(storedProcCommand, "SaleStatus", System.Data.DbType.Int32, (int)product.SaleStatus);
			this.database.AddInParameter(storedProcCommand, "DisplaySequence", System.Data.DbType.Currency, product.DisplaySequence);
			this.database.AddInParameter(storedProcCommand, "ImageUrl1", System.Data.DbType.String, product.ImageUrl1);
			this.database.AddInParameter(storedProcCommand, "ImageUrl2", System.Data.DbType.String, product.ImageUrl2);
			this.database.AddInParameter(storedProcCommand, "ImageUrl3", System.Data.DbType.String, product.ImageUrl3);
			this.database.AddInParameter(storedProcCommand, "ImageUrl4", System.Data.DbType.String, product.ImageUrl4);
			this.database.AddInParameter(storedProcCommand, "ImageUrl5", System.Data.DbType.String, product.ImageUrl5);
			this.database.AddInParameter(storedProcCommand, "ThumbnailUrl40", System.Data.DbType.String, product.ThumbnailUrl40);
			this.database.AddInParameter(storedProcCommand, "ThumbnailUrl60", System.Data.DbType.String, product.ThumbnailUrl60);
			this.database.AddInParameter(storedProcCommand, "ThumbnailUrl100", System.Data.DbType.String, product.ThumbnailUrl100);
			this.database.AddInParameter(storedProcCommand, "ThumbnailUrl160", System.Data.DbType.String, product.ThumbnailUrl160);
			this.database.AddInParameter(storedProcCommand, "ThumbnailUrl180", System.Data.DbType.String, product.ThumbnailUrl180);
			this.database.AddInParameter(storedProcCommand, "ThumbnailUrl220", System.Data.DbType.String, product.ThumbnailUrl220);
			this.database.AddInParameter(storedProcCommand, "ThumbnailUrl310", System.Data.DbType.String, product.ThumbnailUrl310);
			this.database.AddInParameter(storedProcCommand, "ThumbnailUrl410", System.Data.DbType.String, product.ThumbnailUrl410);
			this.database.AddInParameter(storedProcCommand, "LineId", System.Data.DbType.Int32, product.LineId);
			this.database.AddInParameter(storedProcCommand, "MarketPrice", System.Data.DbType.Currency, product.MarketPrice);
			this.database.AddInParameter(storedProcCommand, "LowestSalePrice", System.Data.DbType.Currency, product.LowestSalePrice);
			this.database.AddInParameter(storedProcCommand, "PenetrationStatus", System.Data.DbType.Int16, (int)product.PenetrationStatus);
			this.database.AddInParameter(storedProcCommand, "BrandId", System.Data.DbType.Int32, product.BrandId);
			this.database.AddInParameter(storedProcCommand, "HasSKU", System.Data.DbType.Boolean, product.HasSKU);
			this.database.AddInParameter(storedProcCommand, "ProductId", System.Data.DbType.Int32, product.ProductId);
			return this.database.ExecuteNonQuery(storedProcCommand, dbTran) > 0;
		}
		public override bool UpdateProductCategory(int productId, int newCategoryId, string mainCategoryPath)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Products SET CategoryId = @CategoryId, MainCategoryPath = @MainCategoryPath WHERE ProductId = @ProductId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			this.database.AddInParameter(sqlStringCommand, "CategoryId", System.Data.DbType.Int32, newCategoryId);
			this.database.AddInParameter(sqlStringCommand, "MainCategoryPath", System.Data.DbType.String, mainCategoryPath);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override int DeleteProduct(string productIds)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("DELETE FROM Hishop_Products WHERE ProductId IN ({0}); DELETE FROM Hishop_RelatedProducts WHERE ProductId IN ({0}) OR RelatedProductId IN ({0});DELETE FROM Hishop_ProductTag WHERE ProductId IN ({0})", productIds) + string.Format(" DELETE FROM distro_RelatedProducts WHERE ProductId IN ({0}) OR RelatedProductId IN ({0})", productIds) + string.Format(" DELETE FROM distro_SKUMemberPrice WHERE SkuId NOT IN (SELECT SkuId FROM Hishop_Skus); DELETE FROM Hishop_BundlingProductItems WHERE ProductID IN ({0})", productIds));
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override int UpdateProductSaleStatus(string productIds, ProductSaleStatus saleStatus)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_Products SET SaleStatus = {0} WHERE ProductId IN ({1})", (int)saleStatus, productIds));
			return this.database.ExecuteNonQuery(sqlStringCommand);
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
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_ProductTag(TagId, ProductId) VALUES (@TagId, @ProductId)");
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
		public override IList<int> GetSubjectProductIds(int tagId)
		{
			IList<int> list = new List<int>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ProductId FROM Hishop_ProductTag WHERE TagId=@TagId");
			this.database.AddInParameter(sqlStringCommand, "TagId", System.Data.DbType.Int32, tagId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add((int)dataReader["ProductId"]);
				}
			}
			return list;
		}
		public override bool RemoveSubjectProduct(int tagId, int productId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ProductTag WHERE TagId = @TagId AND ProductId = @ProductId");
			this.database.AddInParameter(sqlStringCommand, "TagId", System.Data.DbType.Int32, tagId);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			return this.database.ExecuteNonQuery(sqlStringCommand) >= 1;
		}
		public override bool ClearSubjectProducts(int tagId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ProductTag WHERE TagId = @TagId");
			this.database.AddInParameter(sqlStringCommand, "TagId", System.Data.DbType.Int32, tagId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override int CreateCategory(CategoryInfo category)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Category_Create");
			this.database.AddOutParameter(storedProcCommand, "CategoryId", System.Data.DbType.Int32, 4);
			this.database.AddInParameter(storedProcCommand, "Name", System.Data.DbType.String, category.Name);
			this.database.AddInParameter(storedProcCommand, "SKUPrefix", System.Data.DbType.String, category.SKUPrefix);
			this.database.AddInParameter(storedProcCommand, "DisplaySequence", System.Data.DbType.Int32, category.DisplaySequence);
			if (!string.IsNullOrEmpty(category.MetaTitle))
			{
				this.database.AddInParameter(storedProcCommand, "Meta_Title", System.Data.DbType.String, category.MetaTitle);
			}
			if (!string.IsNullOrEmpty(category.MetaDescription))
			{
				this.database.AddInParameter(storedProcCommand, "Meta_Description", System.Data.DbType.String, category.MetaDescription);
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
			if (!string.IsNullOrEmpty(category.Icon))
			{
				this.database.AddInParameter(storedProcCommand, "Icon", System.Data.DbType.String, category.Icon);
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
		public override CategoryInfo GetCategory(int categoryId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Categories WHERE CategoryId =@CategoryId");
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
		public override bool DeleteCategory(int categoryId)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Category_Delete");
			this.database.AddInParameter(storedProcCommand, "CategoryId", System.Data.DbType.Int32, categoryId);
			return this.database.ExecuteNonQuery(storedProcCommand) > 0;
		}
		public override CategoryActionStatus UpdateCategory(CategoryInfo category)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Categories SET [Name] = @Name, SKUPrefix = @SKUPrefix,AssociatedProductType = @AssociatedProductType, Meta_Title=@Meta_Title,Meta_Description = @Meta_Description, Meta_Keywords = @Meta_Keywords, Notes1 = @Notes1, Notes2 = @Notes2, Notes3 = @Notes3,  Notes4 = @Notes4, Notes5 = @Notes5,Icon=@Icon,RewriteName = @RewriteName WHERE CategoryId = @CategoryId;UPDATE Hishop_Categories SET RewriteName = @RewriteName WHERE ParentCategoryId = @CategoryId");
			this.database.AddInParameter(sqlStringCommand, "CategoryId", System.Data.DbType.Int32, category.CategoryId);
			this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, category.Name);
			this.database.AddInParameter(sqlStringCommand, "SKUPrefix", System.Data.DbType.String, category.SKUPrefix);
			this.database.AddInParameter(sqlStringCommand, "AssociatedProductType", System.Data.DbType.Int32, category.AssociatedProductType);
			this.database.AddInParameter(sqlStringCommand, "Meta_Title", System.Data.DbType.String, category.MetaTitle);
			this.database.AddInParameter(sqlStringCommand, "Meta_Description", System.Data.DbType.String, category.MetaDescription);
			this.database.AddInParameter(sqlStringCommand, "Meta_Keywords", System.Data.DbType.String, category.MetaKeywords);
			this.database.AddInParameter(sqlStringCommand, "Notes1", System.Data.DbType.String, category.Notes1);
			this.database.AddInParameter(sqlStringCommand, "Notes2", System.Data.DbType.String, category.Notes2);
			this.database.AddInParameter(sqlStringCommand, "Notes3", System.Data.DbType.String, category.Notes3);
			this.database.AddInParameter(sqlStringCommand, "Notes4", System.Data.DbType.String, category.Notes4);
			this.database.AddInParameter(sqlStringCommand, "Notes5", System.Data.DbType.String, category.Notes5);
			this.database.AddInParameter(sqlStringCommand, "Icon", System.Data.DbType.String, category.Icon);
			this.database.AddInParameter(sqlStringCommand, "RewriteName", System.Data.DbType.String, category.RewriteName);
			return (this.database.ExecuteNonQuery(sqlStringCommand) >= 1) ? CategoryActionStatus.Success : CategoryActionStatus.UnknowError;
		}
		public override int DisplaceCategory(int oldCategoryId, int newCategory)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Products SET CategoryId=@newCategory, MainCategoryPath=(SELECT Path FROM Hishop_Categories WHERE CategoryId=@newCategory)+'|' WHERE CategoryId=@oldCategoryId");
			this.database.AddInParameter(sqlStringCommand, "oldCategoryId", System.Data.DbType.Int32, oldCategoryId);
			this.database.AddInParameter(sqlStringCommand, "newCategory", System.Data.DbType.Int32, newCategory);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override bool SwapCategorySequence(int categoryId, int displaysequence)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Hishop_Categories  set DisplaySequence=@DisplaySequence where CategoryId=@CategoryId");
			this.database.AddInParameter(sqlStringCommand, "@DisplaySequence", System.Data.DbType.Int32, displaysequence);
			this.database.AddInParameter(sqlStringCommand, "@CategoryId", System.Data.DbType.Int32, categoryId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool SetProductExtendCategory(int productId, string extendCategoryPath)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Products SET ExtendCategoryPath = @ExtendCategoryPath WHERE ProductId = @ProductId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			this.database.AddInParameter(sqlStringCommand, "ExtendCategoryPath", System.Data.DbType.String, extendCategoryPath);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override bool SetCategoryThemes(int categoryId, string themeName)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Categories SET Theme = @Theme WHERE CategoryId = @CategoryId");
			this.database.AddInParameter(sqlStringCommand, "CategoryId", System.Data.DbType.Int32, categoryId);
			this.database.AddInParameter(sqlStringCommand, "Theme", System.Data.DbType.String, themeName);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override System.Data.DataTable GetCategories()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT CategoryId,Name,DisplaySequence,ParentCategoryId,Depth,[Path],RewriteName,Theme,HasChildren,Icon FROM Hishop_Categories ORDER BY DisplaySequence");
			System.Data.DataTable result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override DbQueryResult GetProductTypes(ProductTypeQuery query)
		{
			return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_ProductTypes", "TypeId", string.IsNullOrEmpty(query.TypeName) ? string.Empty : string.Format("TypeName LIKE '%{0}%'", DataHelper.CleanSearchString(query.TypeName)), "*");
		}
		public override IList<ProductTypeInfo> GetProductTypes()
		{
			IList<ProductTypeInfo> list = new List<ProductTypeInfo>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ProductTypes");
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulateType(dataReader));
				}
			}
			return list;
		}
		public override ProductTypeInfo GetProductType(int typeId)
		{
			ProductTypeInfo productTypeInfo = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ProductTypes WHERE TypeId = @TypeId;SELECT * FROM Hishop_ProductTypeBrands WHERE ProductTypeId = @TypeId");
			this.database.AddInParameter(sqlStringCommand, "TypeId", System.Data.DbType.Int32, typeId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					productTypeInfo = DataMapper.PopulateType(dataReader);
				}
				IList<int> list = new List<int>();
				dataReader.NextResult();
				while (dataReader.Read())
				{
					list.Add((int)dataReader["BrandId"]);
				}
				productTypeInfo.Brands = list;
			}
			return productTypeInfo;
		}
		public override System.Data.DataTable GetBrandCategoriesByTypeId(int typeId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT B.BrandId,B.BrandName FROM Hishop_BrandCategories B INNER JOIN Hishop_ProductTypeBrands PB ON B.BrandId=PB.BrandId WHERE PB.ProductTypeId=@ProductTypeId ORDER BY B.DisplaySequence DESC");
			this.database.AddInParameter(sqlStringCommand, "ProductTypeId", System.Data.DbType.Int32, typeId);
			System.Data.DataTable result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override int GetTypeId(string typeName)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TypeId FROM Hishop_ProductTypes where TypeName = @TypeName");
			this.database.AddInParameter(sqlStringCommand, "TypeName", System.Data.DbType.String, typeName);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int result;
			if (obj != null)
			{
				result = (int)obj;
			}
			else
			{
				result = 0;
			}
			return result;
		}
		public override int AddProductType(ProductTypeInfo productType)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_ProductTypes(TypeName, Remark) VALUES (@TypeName, @Remark); SELECT @@IDENTITY");
			this.database.AddInParameter(sqlStringCommand, "TypeName", System.Data.DbType.String, productType.TypeName);
			this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, productType.Remark);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int result;
			if (obj != null)
			{
				result = Convert.ToInt32(obj);
			}
			else
			{
				result = 0;
			}
			return result;
		}
		public override void AddProductTypeBrands(int typeId, IList<int> brands)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_ProductTypeBrands(ProductTypeId,BrandId) VALUES(@ProductTypeId,@BrandId)");
			this.database.AddInParameter(sqlStringCommand, "ProductTypeId", System.Data.DbType.Int32, typeId);
			this.database.AddInParameter(sqlStringCommand, "BrandId", System.Data.DbType.Int32);
			foreach (int current in brands)
			{
				this.database.SetParameterValue(sqlStringCommand, "BrandId", current);
				this.database.ExecuteNonQuery(sqlStringCommand);
			}
		}
		public override bool UpdateProductType(ProductTypeInfo productType)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_ProductTypes SET TypeName = @TypeName, Remark = @Remark WHERE TypeId = @TypeId");
			this.database.AddInParameter(sqlStringCommand, "TypeName", System.Data.DbType.String, productType.TypeName);
			this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, productType.Remark);
			this.database.AddInParameter(sqlStringCommand, "TypeId", System.Data.DbType.Int32, productType.TypeId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool DeleteProductTypeBrands(int typeId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ProductTypeBrands WHERE ProductTypeId=@ProductTypeId");
			this.database.AddInParameter(sqlStringCommand, "ProductTypeId", System.Data.DbType.Int32, typeId);
			bool result;
			try
			{
				this.database.ExecuteNonQuery(sqlStringCommand);
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}
		public override bool DeleteProducType(int typeId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ProductTypes WHERE TypeId = @TypeId AND not exists (SELECT productId FROM Hishop_Products WHERE TypeId = @TypeId)");
			this.database.AddInParameter(sqlStringCommand, "TypeId", System.Data.DbType.Int32, typeId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override AttributeInfo GetAttribute(int attributeId)
		{
			AttributeInfo attributeInfo = new AttributeInfo();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_AttributeValues WHERE AttributeId = @AttributeId ORDER BY DisplaySequence DESC; SELECT * FROM Hishop_Attributes WHERE AttributeId = @AttributeId;");
			this.database.AddInParameter(sqlStringCommand, "AttributeId", System.Data.DbType.Int32, attributeId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				IList<AttributeValueInfo> list = new List<AttributeValueInfo>();
				while (dataReader.Read())
				{
					AttributeValueInfo attributeValueInfo = new AttributeValueInfo();
					attributeValueInfo.ValueId = (int)dataReader["ValueId"];
					attributeValueInfo.AttributeId = (int)dataReader["AttributeId"];
					attributeValueInfo.DisplaySequence = (int)dataReader["DisplaySequence"];
					attributeValueInfo.ValueStr = (string)dataReader["ValueStr"];
					if (dataReader["ImageUrl"] != DBNull.Value)
					{
						attributeValueInfo.ImageUrl = (string)dataReader["ImageUrl"];
					}
					list.Add(attributeValueInfo);
				}
				if (dataReader.NextResult() && dataReader.Read())
				{
					attributeInfo.AttributeId = (int)dataReader["AttributeId"];
					attributeInfo.AttributeName = (string)dataReader["AttributeName"];
					attributeInfo.DisplaySequence = (int)dataReader["DisplaySequence"];
					attributeInfo.TypeId = (int)dataReader["TypeId"];
					attributeInfo.UsageMode = (AttributeUseageMode)((int)dataReader["UsageMode"]);
					attributeInfo.UseAttributeImage = (bool)dataReader["UseAttributeImage"];
					attributeInfo.AttributeValues = list;
				}
			}
			return attributeInfo;
		}
		public override bool AddAttribute(AttributeInfo attribute)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_Attributes; INSERT INTO Hishop_Attributes(AttributeName, DisplaySequence, TypeId, UsageMode, UseAttributeImage) VALUES(@AttributeName, @DisplaySequence, @TypeId, @UsageMode, @UseAttributeImage); SELECT @@IDENTITY");
			this.database.AddInParameter(sqlStringCommand, "AttributeName", System.Data.DbType.String, attribute.AttributeName);
			this.database.AddInParameter(sqlStringCommand, "TypeId", System.Data.DbType.Int32, attribute.TypeId);
			this.database.AddInParameter(sqlStringCommand, "UsageMode", System.Data.DbType.Int32, (int)attribute.UsageMode);
			this.database.AddInParameter(sqlStringCommand, "UseAttributeImage", System.Data.DbType.Boolean, attribute.UseAttributeImage);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			if (attribute.AttributeValues.Count != 0 && obj != null)
			{
				int num = Convert.ToInt32(obj);
				foreach (AttributeValueInfo current in attribute.AttributeValues)
				{
					System.Data.Common.DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_AttributeValues; INSERT INTO Hishop_AttributeValues(AttributeId, DisplaySequence, ValueStr, ImageUrl) VALUES(@AttributeId, @DisplaySequence, @ValueStr, @ImageUrl)");
					this.database.AddInParameter(sqlStringCommand2, "AttributeId", System.Data.DbType.Int32, num);
					this.database.AddInParameter(sqlStringCommand2, "ValueStr", System.Data.DbType.String, current.ValueStr);
					this.database.AddInParameter(sqlStringCommand2, "ImageUrl", System.Data.DbType.String, current.ImageUrl);
					this.database.ExecuteNonQuery(sqlStringCommand2);
				}
			}
			return obj != null;
		}
		public override int GetSpecificationId(int typeId, string specificationName)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT AttributeId FROM Hishop_Attributes WHERE UsageMode = 2 AND TypeId = @TypeId AND AttributeName = @AttributeName");
			this.database.AddInParameter(sqlStringCommand, "TypeId", System.Data.DbType.Int32, typeId);
			this.database.AddInParameter(sqlStringCommand, "AttributeName", System.Data.DbType.String, specificationName);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int result = 0;
			if (obj != null)
			{
				result = (int)obj;
			}
			return result;
		}
		public override int AddAttributeName(AttributeInfo attribute)
		{
			int result = 0;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_Attributes; INSERT INTO Hishop_Attributes(AttributeName, DisplaySequence, TypeId, UsageMode, UseAttributeImage) VALUES(@AttributeName, @DisplaySequence, @TypeId, @UsageMode, @UseAttributeImage); SELECT @@IDENTITY");
			this.database.AddInParameter(sqlStringCommand, "AttributeName", System.Data.DbType.String, attribute.AttributeName);
			this.database.AddInParameter(sqlStringCommand, "TypeId", System.Data.DbType.Int32, attribute.TypeId);
			this.database.AddInParameter(sqlStringCommand, "UsageMode", System.Data.DbType.Int32, (int)attribute.UsageMode);
			this.database.AddInParameter(sqlStringCommand, "UseAttributeImage", System.Data.DbType.Boolean, attribute.UseAttributeImage);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			if (obj != null)
			{
				result = Convert.ToInt32(obj);
			}
			return result;
		}
		public override bool UpdateAttribute(AttributeInfo attribute)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Attributes SET AttributeName = @AttributeName, TypeId = @TypeId, UseAttributeImage = @UseAttributeImage WHERE AttributeId = @AttributeId; DELETE FROM Hishop_AttributeValues WHERE AttributeId = @AttributeId;");
			this.database.AddInParameter(sqlStringCommand, "AttributeId", System.Data.DbType.Int32, attribute.AttributeId);
			this.database.AddInParameter(sqlStringCommand, "AttributeName", System.Data.DbType.String, attribute.AttributeName);
			this.database.AddInParameter(sqlStringCommand, "TypeId", System.Data.DbType.Int32, attribute.TypeId);
			this.database.AddInParameter(sqlStringCommand, "UseAttributeImage", System.Data.DbType.Boolean, attribute.UseAttributeImage);
			bool result;
			if ((result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0)) && attribute.AttributeValues.Count != 0)
			{
				foreach (AttributeValueInfo current in attribute.AttributeValues)
				{
					System.Data.Common.DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_AttributeValues; INSERT INTO Hishop_AttributeValues(AttributeId, DisplaySequence, ValueStr, ImageUrl) VALUES(@AttributeId, @DisplaySequence, @ValueStr, @ImageUrl)");
					this.database.AddInParameter(sqlStringCommand2, "AttributeId", System.Data.DbType.Int32, attribute.AttributeId);
					this.database.AddInParameter(sqlStringCommand2, "ValueStr", System.Data.DbType.String, current.ValueStr);
					this.database.AddInParameter(sqlStringCommand2, "ImageUrl", System.Data.DbType.String, current.ImageUrl);
					this.database.ExecuteNonQuery(sqlStringCommand2);
				}
			}
			return result;
		}
		public override bool UpdateAttributeName(AttributeInfo attribute)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Attributes SET AttributeName = @AttributeName, UsageMode = @UsageMode WHERE AttributeId = @AttributeId;");
			this.database.AddInParameter(sqlStringCommand, "AttributeId", System.Data.DbType.Int32, attribute.AttributeId);
			this.database.AddInParameter(sqlStringCommand, "AttributeName", System.Data.DbType.String, attribute.AttributeName);
			this.database.AddInParameter(sqlStringCommand, "UsageMode", System.Data.DbType.Int32, (int)attribute.UsageMode);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool DeleteAttribute(int attributeId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Attributes WHERE AttributeId = @AttributeId AND not exists (SELECT * FROM Hishop_SKUItems WHERE AttributeId = @AttributeId)");
			this.database.AddInParameter(sqlStringCommand, "AttributeId", System.Data.DbType.Int32, attributeId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override void SwapAttributeSequence(int attributeId, int replaceAttributeId, int displaySequence, int replaceDisplaySequence)
		{
			DataHelper.SwapSequence("Hishop_Attributes", "AttributeId", "DisplaySequence", attributeId, replaceAttributeId, displaySequence, replaceDisplaySequence);
		}
		public override IList<AttributeInfo> GetAttributes(int typeId)
		{
			IList<AttributeInfo> list = new List<AttributeInfo>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Attributes WHERE TypeId = @TypeId ORDER BY DisplaySequence DESC SELECT * FROM Hishop_AttributeValues WHERE AttributeId IN (SELECT AttributeId FROM Hishop_Attributes WHERE TypeId = @TypeId) ORDER BY DisplaySequence DESC");
			this.database.AddInParameter(sqlStringCommand, "TypeId", System.Data.DbType.Int32, typeId);
			using (System.Data.DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand))
			{
				foreach (System.Data.DataRow dataRow in dataSet.Tables[0].Rows)
				{
					AttributeInfo attributeInfo = new AttributeInfo();
					attributeInfo.AttributeId = (int)dataRow["AttributeId"];
					attributeInfo.AttributeName = (string)dataRow["AttributeName"];
					attributeInfo.DisplaySequence = (int)dataRow["DisplaySequence"];
					attributeInfo.TypeId = (int)dataRow["TypeId"];
					attributeInfo.UsageMode = (AttributeUseageMode)((int)dataRow["UsageMode"]);
					attributeInfo.UseAttributeImage = (bool)dataRow["UseAttributeImage"];
					if (dataSet.Tables[1].Rows.Count > 0)
					{
						System.Data.DataRow[] array = dataSet.Tables[1].Select("AttributeId=" + attributeInfo.AttributeId.ToString(CultureInfo.InvariantCulture));
						System.Data.DataRow[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							System.Data.DataRow dataRow2 = array2[i];
							AttributeValueInfo attributeValueInfo = new AttributeValueInfo();
							attributeValueInfo.ValueId = (int)dataRow2["ValueId"];
							attributeValueInfo.AttributeId = attributeInfo.AttributeId;
							attributeValueInfo.ValueStr = (string)dataRow2["ValueStr"];
							attributeInfo.AttributeValues.Add(attributeValueInfo);
						}
					}
					list.Add(attributeInfo);
				}
			}
			return list;
		}
		public override IList<AttributeInfo> GetAttributes(AttributeUseageMode attributeUseageMode)
		{
			IList<AttributeInfo> list = new List<AttributeInfo>();
			string text;
			if (attributeUseageMode == AttributeUseageMode.Choose)
			{
				text = "UsageMode = 2";
			}
			else
			{
				text = "UsageMode <> 2";
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Concat(new string[]
			{
				"SELECT * FROM Hishop_Attributes WHERE ",
				text,
				" ORDER BY DisplaySequence Desc SELECT * FROM Hishop_AttributeValues WHERE AttributeId IN (SELECT AttributeId FROM Hishop_Attributes Where  ",
				text,
				" ) ORDER BY DisplaySequence Desc"
			}));
			using (System.Data.DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand))
			{
				foreach (System.Data.DataRow dataRow in dataSet.Tables[0].Rows)
				{
					AttributeInfo attributeInfo = new AttributeInfo();
					attributeInfo.AttributeId = (int)dataRow["AttributeId"];
					attributeInfo.AttributeName = (string)dataRow["AttributeName"];
					attributeInfo.DisplaySequence = (int)dataRow["DisplaySequence"];
					attributeInfo.TypeId = (int)dataRow["TypeId"];
					attributeInfo.UsageMode = (AttributeUseageMode)((int)dataRow["UsageMode"]);
					attributeInfo.UseAttributeImage = (bool)dataRow["UseAttributeImage"];
					if (dataSet.Tables[1].Rows.Count > 0)
					{
						System.Data.DataRow[] array = dataSet.Tables[1].Select("AttributeId=" + attributeInfo.AttributeId.ToString(CultureInfo.InvariantCulture));
						System.Data.DataRow[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							System.Data.DataRow dataRow2 = array2[i];
							AttributeValueInfo attributeValueInfo = new AttributeValueInfo();
							attributeValueInfo.ValueId = (int)dataRow2["ValueId"];
							attributeValueInfo.AttributeId = attributeInfo.AttributeId;
							if (dataRow2["ImageUrl"] != DBNull.Value)
							{
								attributeValueInfo.ImageUrl = (string)dataRow2["ImageUrl"];
							}
							attributeValueInfo.ValueStr = (string)dataRow2["ValueStr"];
							attributeInfo.AttributeValues.Add(attributeValueInfo);
						}
					}
					list.Add(attributeInfo);
				}
			}
			return list;
		}
		public override IList<AttributeInfo> GetAttributes(int typeId, AttributeUseageMode attributeUseageMode)
		{
			IList<AttributeInfo> list = new List<AttributeInfo>();
			string text;
			if (attributeUseageMode == AttributeUseageMode.Choose)
			{
				text = "UsageMode = 2";
			}
			else
			{
				text = "UsageMode <> 2";
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Concat(new string[]
			{
				"SELECT * FROM Hishop_Attributes WHERE TypeId = @TypeId AND ",
				text,
				" ORDER BY DisplaySequence Desc SELECT * FROM Hishop_AttributeValues WHERE AttributeId IN (SELECT AttributeId FROM Hishop_Attributes WHERE TypeId = @TypeId AND  ",
				text,
				" ) ORDER BY DisplaySequence Desc"
			}));
			this.database.AddInParameter(sqlStringCommand, "TypeId", System.Data.DbType.Int32, typeId);
			using (System.Data.DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand))
			{
				foreach (System.Data.DataRow dataRow in dataSet.Tables[0].Rows)
				{
					AttributeInfo attributeInfo = new AttributeInfo();
					attributeInfo.AttributeId = (int)dataRow["AttributeId"];
					attributeInfo.AttributeName = (string)dataRow["AttributeName"];
					attributeInfo.DisplaySequence = (int)dataRow["DisplaySequence"];
					attributeInfo.TypeId = (int)dataRow["TypeId"];
					attributeInfo.UsageMode = (AttributeUseageMode)((int)dataRow["UsageMode"]);
					attributeInfo.UseAttributeImage = (bool)dataRow["UseAttributeImage"];
					if (dataSet.Tables[1].Rows.Count > 0)
					{
						System.Data.DataRow[] array = dataSet.Tables[1].Select("AttributeId=" + attributeInfo.AttributeId.ToString(CultureInfo.InvariantCulture));
						System.Data.DataRow[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							System.Data.DataRow dataRow2 = array2[i];
							AttributeValueInfo attributeValueInfo = new AttributeValueInfo();
							attributeValueInfo.ValueId = (int)dataRow2["ValueId"];
							attributeValueInfo.AttributeId = attributeInfo.AttributeId;
							if (dataRow2["ImageUrl"] != DBNull.Value)
							{
								attributeValueInfo.ImageUrl = (string)dataRow2["ImageUrl"];
							}
							attributeValueInfo.ValueStr = (string)dataRow2["ValueStr"];
							attributeInfo.AttributeValues.Add(attributeValueInfo);
						}
					}
					list.Add(attributeInfo);
				}
			}
			return list;
		}
		public override bool UpdateSpecification(AttributeInfo attribute)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Attributes SET AttributeName = @AttributeName, UseAttributeImage = @UseAttributeImage WHERE AttributeId = @AttributeId");
			this.database.AddInParameter(sqlStringCommand, "AttributeId", System.Data.DbType.Int32, attribute.AttributeId);
			this.database.AddInParameter(sqlStringCommand, "AttributeName", System.Data.DbType.String, attribute.AttributeName);
			this.database.AddInParameter(sqlStringCommand, "UseAttributeImage", System.Data.DbType.Boolean, attribute.UseAttributeImage);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override int AddAttributeValue(AttributeValueInfo attributeValue)
		{
			int result = 0;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_AttributeValues; INSERT INTO Hishop_AttributeValues(AttributeId, DisplaySequence, ValueStr, ImageUrl) VALUES(@AttributeId, @DisplaySequence, @ValueStr, @ImageUrl);SELECT @@IDENTITY");
			this.database.AddInParameter(sqlStringCommand, "AttributeId", System.Data.DbType.Int32, attributeValue.AttributeId);
			this.database.AddInParameter(sqlStringCommand, "ValueStr", System.Data.DbType.String, attributeValue.ValueStr);
			this.database.AddInParameter(sqlStringCommand, "ImageUrl", System.Data.DbType.String, attributeValue.ImageUrl);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			if (obj != null)
			{
				result = Convert.ToInt32(obj.ToString());
			}
			return result;
		}
		public override int GetSpecificationValueId(int attributeId, string ValueStr)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ValueId FROM Hishop_AttributeValues WHERE AttributeId = @AttributeId AND ValueStr = @ValueStr");
			this.database.AddInParameter(sqlStringCommand, "AttributeId", System.Data.DbType.Int32, attributeId);
			this.database.AddInParameter(sqlStringCommand, "ValueStr", System.Data.DbType.String, ValueStr);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int result = 0;
			if (obj != null)
			{
				result = Convert.ToInt32(obj);
			}
			return result;
		}
		public override bool DeleteAttribute(int attributeId, int valueId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_AttributeValues WHERE ValueId=@ValueId AND AttributeId=@AttributeId;DELETE FROM Hishop_ProductAttributes WHERE AttributeId=@AttributeId AND ValueId=@ValueId");
			this.database.AddInParameter(sqlStringCommand, "ValueId", System.Data.DbType.Int32, valueId);
			this.database.AddInParameter(sqlStringCommand, "AttributeId", System.Data.DbType.Int32, attributeId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool UpdateAttributeValue(int attributeId, int valueId, string newValue)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_AttributeValues SET ValueStr=@ValueStr WHERE ValueId=@valueId AND AttributeId=@AttributeId");
			this.database.AddInParameter(sqlStringCommand, "AttributeId", System.Data.DbType.Int32, attributeId);
			this.database.AddInParameter(sqlStringCommand, "ValueStr", System.Data.DbType.String, newValue);
			this.database.AddInParameter(sqlStringCommand, "ValueId", System.Data.DbType.Int32, valueId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool UpdateSku(AttributeValueInfo attributeValue)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_AttributeValues SET  ValueStr=@ValueStr, ImageUrl=@ImageUrl WHERE ValueId=@valueId");
			this.database.AddInParameter(sqlStringCommand, "ValueStr", System.Data.DbType.String, attributeValue.ValueStr);
			this.database.AddInParameter(sqlStringCommand, "ValueId", System.Data.DbType.Int32, attributeValue.ValueId);
			this.database.AddInParameter(sqlStringCommand, "ImageUrl", System.Data.DbType.String, attributeValue.ImageUrl);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool DeleteAttributeValue(int attributeValueId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_AttributeValues WHERE ValueId = @ValueId AND not exists (SELECT * FROM Hishop_SKUItems WHERE ValueId = @ValueId) DELETE FROM Hishop_ProductAttributes WHERE ValueId = @ValueId");
			this.database.AddInParameter(sqlStringCommand, "ValueId", System.Data.DbType.Int32, attributeValueId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool ClearAttributeValue(int attributeId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_AttributeValues WHERE AttributeId = @AttributeId AND not exists (SELECT * FROM Hishop_SKUItems WHERE AttributeId = @AttributeId)");
			this.database.AddInParameter(sqlStringCommand, "AttributeId", System.Data.DbType.Int32, attributeId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override void SwapAttributeValueSequence(int attributeValueId, int replaceAttributeValueId, int displaySequence, int replaceDisplaySequence)
		{
			DataHelper.SwapSequence("Hishop_AttributeValues", "ValueId", "DisplaySequence", attributeValueId, replaceAttributeValueId, displaySequence, replaceDisplaySequence);
		}
		public override AttributeValueInfo GetAttributeValueInfo(int valueId)
		{
			AttributeValueInfo attributeValueInfo = new AttributeValueInfo();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_AttributeValues WHERE ValueId=@ValueId");
			this.database.AddInParameter(sqlStringCommand, "ValueId", System.Data.DbType.Int32, valueId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					attributeValueInfo = DataMapper.PopulateAttributeValue(dataReader);
					attributeValueInfo.ImageUrl = dataReader["ImageUrl"].ToString();
					attributeValueInfo.DisplaySequence = (int)dataReader["DisplaySequence"];
				}
			}
			return attributeValueInfo;
		}
		public override int AddBrandCategory(BrandCategoryInfo brandCategory)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_BrandCategories;INSERT INTO Hishop_BrandCategories(BrandName, Logo, CompanyUrl,RewriteName,MetaKeywords,MetaDescription, Description, DisplaySequence) VALUES(@BrandName, @Logo, @CompanyUrl,@RewriteName,@MetaKeywords,@MetaDescription, @Description, @DisplaySequence); SELECT @@IDENTITY");
			this.database.AddInParameter(sqlStringCommand, "BrandName", System.Data.DbType.String, brandCategory.BrandName);
			this.database.AddInParameter(sqlStringCommand, "Logo", System.Data.DbType.String, brandCategory.Logo);
			this.database.AddInParameter(sqlStringCommand, "CompanyUrl", System.Data.DbType.String, brandCategory.CompanyUrl);
			this.database.AddInParameter(sqlStringCommand, "RewriteName", System.Data.DbType.String, brandCategory.RewriteName);
			this.database.AddInParameter(sqlStringCommand, "MetaKeywords", System.Data.DbType.String, brandCategory.MetaKeywords);
			this.database.AddInParameter(sqlStringCommand, "MetaDescription", System.Data.DbType.String, brandCategory.MetaDescription);
			this.database.AddInParameter(sqlStringCommand, "Description", System.Data.DbType.String, brandCategory.Description);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int result;
			if (obj != null)
			{
				result = Convert.ToInt32(obj);
			}
			else
			{
				result = 0;
			}
			return result;
		}
		public override void AddBrandProductTypes(int brandId, IList<int> productTypes)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_ProductTypeBrands(ProductTypeId,BrandId) VALUES(@ProductTypeId,@BrandId)");
			this.database.AddInParameter(sqlStringCommand, "ProductTypeId", System.Data.DbType.Int32);
			this.database.AddInParameter(sqlStringCommand, "BrandId", System.Data.DbType.Int32, brandId);
			foreach (int current in productTypes)
			{
				this.database.SetParameterValue(sqlStringCommand, "ProductTypeId", current);
				this.database.ExecuteNonQuery(sqlStringCommand);
			}
		}
		public override bool DeleteBrandProductTypes(int brandId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ProductTypeBrands WHERE BrandId=@BrandId");
			this.database.AddInParameter(sqlStringCommand, "BrandId", System.Data.DbType.Int32, brandId);
			bool result;
			try
			{
				this.database.ExecuteNonQuery(sqlStringCommand);
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}
		public override System.Data.DataTable GetBrandCategories(string brandName)
		{
			string text = "1=1";
			if (!string.IsNullOrEmpty(brandName))
			{
				text = text + " AND BrandName LIKE '%" + DataHelper.CleanSearchString(brandName) + "%'";
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_BrandCategories  WHERE " + text + " ORDER BY DisplaySequence");
			System.Data.DataTable result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override System.Data.DataTable GetBrandCategories()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_BrandCategories ORDER BY DisplaySequence");
			System.Data.DataTable result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override BrandCategoryInfo GetBrandCategory(int brandId)
		{
			BrandCategoryInfo brandCategoryInfo = new BrandCategoryInfo();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_BrandCategories WHERE BrandId = @BrandId;SELECT * FROM Hishop_ProductTypeBrands WHERE BrandId = @BrandId");
			this.database.AddInParameter(sqlStringCommand, "BrandId", System.Data.DbType.Int32, brandId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					brandCategoryInfo = DataMapper.PopulateBrandCategory(dataReader);
				}
				IList<int> list = new List<int>();
				dataReader.NextResult();
				while (dataReader.Read())
				{
					list.Add((int)dataReader["ProductTypeId"]);
				}
				brandCategoryInfo.ProductTypes = list;
			}
			return brandCategoryInfo;
		}
		public override bool UpdateBrandCategory(BrandCategoryInfo brandCategory)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_BrandCategories SET BrandName = @BrandName, Logo = @Logo, CompanyUrl = @CompanyUrl,RewriteName=@RewriteName,MetaKeywords=@MetaKeywords,MetaDescription=@MetaDescription, Description = @Description WHERE BrandId = @BrandId");
			this.database.AddInParameter(sqlStringCommand, "BrandId", System.Data.DbType.Int32, brandCategory.BrandId);
			this.database.AddInParameter(sqlStringCommand, "BrandName", System.Data.DbType.String, brandCategory.BrandName);
			this.database.AddInParameter(sqlStringCommand, "Logo", System.Data.DbType.String, brandCategory.Logo);
			this.database.AddInParameter(sqlStringCommand, "CompanyUrl", System.Data.DbType.String, brandCategory.CompanyUrl);
			this.database.AddInParameter(sqlStringCommand, "RewriteName", System.Data.DbType.String, brandCategory.RewriteName);
			this.database.AddInParameter(sqlStringCommand, "MetaKeywords", System.Data.DbType.String, brandCategory.MetaKeywords);
			this.database.AddInParameter(sqlStringCommand, "MetaDescription", System.Data.DbType.String, brandCategory.MetaDescription);
			this.database.AddInParameter(sqlStringCommand, "Description", System.Data.DbType.String, brandCategory.Description);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool BrandHvaeProducts(int brandId)
		{
			bool result = false;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Count(ProductName) FROM Hishop_Products Where BrandId=@BrandId");
			this.database.AddInParameter(sqlStringCommand, "BrandId", System.Data.DbType.Int32, brandId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = (dataReader.GetInt32(0) > 0);
				}
			}
			return result;
		}
		public override bool DeleteBrandCategory(int brandId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_BrandCategories WHERE BrandId = @BrandId");
			this.database.AddInParameter(sqlStringCommand, "BrandId", System.Data.DbType.Int32, brandId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override void UpdateBrandCategoryDisplaySequence(int brandId, SortAction action)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_BrandCategory_DisplaySequence");
			this.database.AddInParameter(storedProcCommand, "BrandId", System.Data.DbType.Int32, brandId);
			this.database.AddInParameter(storedProcCommand, "Sort", System.Data.DbType.Int32, action);
			this.database.ExecuteNonQuery(storedProcCommand);
		}
		public override bool UpdateBrandCategoryDisplaySequence(int brandId, int displaysequence)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Hishop_BrandCategories set DisplaySequence=@DisplaySequence where BrandId=@BrandId");
			this.database.AddInParameter(sqlStringCommand, "@DisplaySequence", System.Data.DbType.Int32, displaysequence);
			this.database.AddInParameter(sqlStringCommand, "@BrandId", System.Data.DbType.Int32, brandId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool SetBrandCategoryThemes(int brandid, string themeName)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Hishop_BrandCategories set Theme = @Theme where BrandId = @BrandId");
			this.database.AddInParameter(sqlStringCommand, "BrandId", System.Data.DbType.Int32, brandid);
			this.database.AddInParameter(sqlStringCommand, "Theme", System.Data.DbType.String, themeName);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		private static string BuildProductQuery(ProductQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT p.ProductId FROM Hishop_Products p WHERE p.SaleStatus = {0}", (int)query.SaleStatus);
			if (!string.IsNullOrEmpty(query.ProductCode) && query.ProductCode.Length > 0)
			{
				stringBuilder.AppendFormat(" AND LOWER(p.ProductCode) LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
			}
			if (!string.IsNullOrEmpty(query.Keywords))
			{
				stringBuilder.AppendFormat(" AND LOWER(p.ProductName) LIKE '%{0}%'", DataHelper.CleanSearchString(query.Keywords));
			}
			if (query.CategoryId.HasValue)
			{
				stringBuilder.AppendFormat(" AND (p.CategoryId = {0}  OR  p.CategoryId IN (SELECT CategoryId FROM Hishop_Categories WHERE Path LIKE (SELECT Path FROM Hishop_Categories WHERE CategoryId = {0}) + '|%'))", query.CategoryId.Value);
			}
			if (!string.IsNullOrEmpty(query.SortBy))
			{
				stringBuilder.AppendFormat(" ORDER BY p.{0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
			}
			return stringBuilder.ToString();
		}
		public override int PenetrationProducts(string productIds)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Products SET PenetrationStatus = 1 WHERE ProductId IN (" + productIds + ")");
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override int CanclePenetrationProducts(string productIds, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Concat(new string[]
			{
				"UPDATE Hishop_Products SET PenetrationStatus = 2 WHERE ProductId IN (",
				productIds,
				") ;delete from Hishop_PurchaseShoppingCarts where productid in (",
				productIds,
				")"
			}));
			int result;
			if (dbTran != null)
			{
				result = this.database.ExecuteNonQuery(sqlStringCommand, dbTran);
			}
			else
			{
				result = this.database.ExecuteNonQuery(sqlStringCommand);
			}
			return result;
		}
		public override bool DeleteCanclePenetrationProducts(string productIds, System.Data.Common.DbTransaction dbTran)
		{
			bool result;
			try
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_Products WHERE ProductId IN (" + productIds + ")");
				if (dbTran != null)
				{
					this.database.ExecuteNonQuery(sqlStringCommand, dbTran);
				}
				else
				{
					this.database.ExecuteNonQuery(sqlStringCommand);
				}
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}
		public override IList<string> GetUserNameByProductId(string productIds)
		{
			List<string> list = new List<string>();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT UserName FROM aspnet_Users WHERE UserId IN (SELECT UserId from Hishop_DistributorProductLines WHERE LineId IN ");
			stringBuilder.AppendFormat(" (SELECT LineId from Hishop_Products WHERE SaleStatus!={0} AND PenetrationStatus=1 AND ProductId in ({1})))", 0, productIds);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(dataReader["UserName"].ToString());
				}
			}
			return list;
		}
		public override IList<string> GetUserIdByLineId(int lineId)
		{
			List<string> list = new List<string>();
			try
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT UserId FROM Hishop_DistributorProductLines WHERE LineId=" + lineId);
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					while (dataReader.Read())
					{
						list.Add(dataReader["UserId"].ToString());
					}
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return list;
		}
		public override string GetProductNamesByLineId(int lineId, out int count)
		{
			int num = 0;
			string text = "";
			try
			{
				StringBuilder stringBuilder = new StringBuilder("select ProductName from Hishop_Products where PenetrationStatus=1");
				stringBuilder.AppendFormat(" and SaleStatus!={0}", 0);
				stringBuilder.AppendFormat(" and LineId={0}", lineId);
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					while (dataReader.Read())
					{
						text = text + dataReader["ProductName"].ToString() + ",";
						num++;
					}
				}
				if (text != "")
				{
					text = text.Substring(0, text.Length);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			count = num;
			return text;
		}
		public override System.Data.DataTable GetProductBaseInfo(string productIds)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT ProductId, ProductName, ProductCode, MarketPrice, LowestSalePrice,ThumbnailUrl40, SaleCounts, ShowSaleCounts FROM Hishop_Products WHERE ProductId IN ({0})", DataHelper.CleanSearchString(productIds)));
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override bool UpdateProductNames(string productIds, string prefix, string suffix)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_Products SET ProductName = '{0}'+ProductName+'{1}' WHERE ProductId IN ({2})", DataHelper.CleanSearchString(prefix), DataHelper.CleanSearchString(suffix), productIds));
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool ReplaceProductNames(string productIds, string oldWord, string newWord)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_Products SET ProductName = REPLACE(ProductName, '{0}', '{1}') WHERE ProductId IN ({2})", DataHelper.CleanSearchString(oldWord), DataHelper.CleanSearchString(newWord), productIds));
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool UpdateProductBaseInfo(System.Data.DataTable dataTable_0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int num = 0;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
			foreach (System.Data.DataRow dataRow in dataTable_0.Rows)
			{
				num++;
				string text = num.ToString();
				stringBuilder.AppendFormat(" UPDATE Hishop_Products SET ProductName = @ProductName{0}, ProductCode = @ProductCode{0}, MarketPrice = @MarketPrice{0}", text);
				stringBuilder.AppendFormat(", LowestSalePrice = {0} WHERE ProductId = {1}", dataRow["LowestSalePrice"], dataRow["ProductId"]);
				stringBuilder.AppendFormat(" UPDATE distro_Products SET ProductCode = @ProductCode{0} WHERE ProductId = {1}", text, dataRow["ProductId"]);
				this.database.AddInParameter(sqlStringCommand, "ProductName" + text, System.Data.DbType.String, dataRow["ProductName"]);
				this.database.AddInParameter(sqlStringCommand, "ProductCode" + text, System.Data.DbType.String, dataRow["ProductCode"]);
				this.database.AddInParameter(sqlStringCommand, "MarketPrice" + text, System.Data.DbType.String, dataRow["MarketPrice"]);
			}
			sqlStringCommand.CommandText = stringBuilder.ToString();
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool UpdateShowSaleCounts(string productIds, int showSaleCounts)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_Products SET ShowSaleCounts = {0} WHERE ProductId IN ({1})", showSaleCounts, productIds));
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool UpdateShowSaleCounts(string productIds, int showSaleCounts, string operation)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_Products SET ShowSaleCounts = SaleCounts {0} {1} WHERE ProductId IN ({2})", operation, showSaleCounts, productIds));
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool UpdateShowSaleCounts(System.Data.DataTable dataTable_0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (System.Data.DataRow dataRow in dataTable_0.Rows)
			{
				stringBuilder.AppendFormat(" UPDATE Hishop_Products SET ShowSaleCounts = {0} WHERE ProductId = {1}", dataRow["ShowSaleCounts"], dataRow["ProductId"]);
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override System.Data.DataTable GetSkuStocks(string productIds)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT p.ProductId,ProductName, SkuId, SKU, Stock, AlertStock,ThumbnailUrl40 FROM Hishop_Products p JOIN Hishop_SKUs s ON p.ProductId = s.ProductId WHERE p.ProductId IN ({0})", DataHelper.CleanSearchString(productIds));
			stringBuilder.Append(" SELECT SkuId, AttributeName, ValueStr FROM Hishop_SKUItems si JOIN Hishop_Attributes a ON si.AttributeId = a.AttributeId JOIN Hishop_AttributeValues av ON si.ValueId = av.ValueId");
			stringBuilder.AppendFormat(" WHERE si.SkuId IN(SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({0}))", DataHelper.CleanSearchString(productIds));
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			System.Data.DataTable dataTable = null;
			System.Data.DataTable dataTable2 = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
				dataReader.NextResult();
				dataTable2 = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			dataTable.Columns.Add("SKUContent");
			if (dataTable != null && dataTable.Rows.Count > 0 && dataTable2 != null && dataTable2.Rows.Count > 0)
			{
				foreach (System.Data.DataRow dataRow in dataTable.Rows)
				{
					string text = string.Empty;
					foreach (System.Data.DataRow dataRow2 in dataTable2.Rows)
					{
						if ((string)dataRow["SkuId"] == (string)dataRow2["SkuId"])
						{
							object obj = text;
							text = string.Concat(new object[]
							{
								obj,
								dataRow2["AttributeName"],
								"：",
								dataRow2["ValueStr"],
								"; "
							});
						}
					}
					dataRow["SKUContent"] = text;
				}
			}
			return dataTable;
		}
		public override bool UpdateSkuStock(string productIds, int stock)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_SKUs SET Stock = {0} WHERE ProductId IN ({1})", stock, DataHelper.CleanSearchString(productIds)));
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool AddSkuStock(string productIds, int addStock)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("UPDATE Hishop_SKUs SET Stock = CASE WHEN Stock + ({0}) < 0 THEN 0 ELSE Stock + ({0}) END WHERE ProductId IN ({1})", addStock, DataHelper.CleanSearchString(productIds)));
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool UpdateSkuStock(Dictionary<string, int> skuStocks)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string current in skuStocks.Keys)
			{
				stringBuilder.AppendFormat(" UPDATE Hishop_SKUs SET Stock = {0} WHERE SkuId = '{1}'", skuStocks[current], DataHelper.CleanSearchString(current));
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override System.Data.DataTable GetSkuMemberPrices(string productIds)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT SkuId, ProductName, SKU, CostPrice, MarketPrice, SalePrice FROM Hishop_Products p JOIN Hishop_SKUs s ON p.ProductId = s.ProductId WHERE p.ProductId IN ({0})", DataHelper.CleanSearchString(productIds));
			stringBuilder.Append(" SELECT SkuId, AttributeName, ValueStr FROM Hishop_SKUItems si JOIN Hishop_Attributes a ON si.AttributeId = a.AttributeId JOIN Hishop_AttributeValues av ON si.ValueId = av.ValueId");
			stringBuilder.AppendFormat(" WHERE si.SkuId IN(SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({0}))", DataHelper.CleanSearchString(productIds));
			stringBuilder.AppendLine(" SELECT CAST(GradeId AS NVARCHAR) + '_' + [Name] AS MemberGradeName,Discount FROM aspnet_MemberGrades");
			stringBuilder.AppendLine(" SELECT SkuId, (SELECT CAST(GradeId AS NVARCHAR) + '_' + [Name] FROM aspnet_MemberGrades WHERE GradeId = sm.GradeId) AS MemberGradeName,MemberSalePrice");
			stringBuilder.AppendFormat(" FROM Hishop_SKUMemberPrice sm WHERE SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({0}))", DataHelper.CleanSearchString(productIds));
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
					if (dataTable4 != null && dataTable4.Rows.Count > 0)
					{
						foreach (System.Data.DataRow dataRow in dataTable4.Rows)
						{
							dataTable.Columns.Add((string)dataRow["MemberGradeName"]);
						}
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
							dataRow2[(string)dataRow4["MemberGradeName"]] = dataRow4["MemberSalePrice"];
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
		public override bool CheckPrice(string productIds, int baseGradeId, decimal checkPrice, bool isMember)
		{
			StringBuilder stringBuilder = new StringBuilder(" ");
			if (baseGradeId == -2)
			{
				stringBuilder.AppendFormat("SELECT COUNT(*) FROM Hishop_SKUs WHERE ProductId IN ({0}) AND CostPrice - {1} < 0", productIds, checkPrice);
			}
			else
			{
				if (baseGradeId == -3)
				{
					stringBuilder.AppendFormat("SELECT COUNT(*) FROM Hishop_SKUs WHERE ProductId IN ({0}) AND SalePrice - {1} < 0", productIds, checkPrice);
				}
				else
				{
					if (baseGradeId == -4)
					{
						stringBuilder.AppendFormat("SELECT COUNT(*) FROM Hishop_SKUs WHERE ProductId IN ({0}) AND PurchasePrice - {1} < 0", productIds, checkPrice);
					}
					else
					{
						if (isMember)
						{
							stringBuilder.AppendFormat("SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE MemberSalePrice - {0} < 0 AND GradeId = {1}", checkPrice, baseGradeId);
							stringBuilder.AppendFormat(" AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({0})) ", productIds);
						}
						else
						{
							stringBuilder.AppendFormat("SELECT COUNT(*) FROM Hishop_SKUDistributorPrice WHERE DistributorPurchasePrice - {0} < 0 AND GradeId = {1}", checkPrice, baseGradeId);
							stringBuilder.AppendFormat(" AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({0})) ", productIds);
						}
					}
				}
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
		}
		public override bool UpdateSkuMemberPrices(string productIds, int gradeId, decimal price)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (gradeId == -2)
			{
				stringBuilder.AppendFormat("UPDATE Hishop_SKUs SET CostPrice = {0} WHERE ProductId IN ({1})", price, DataHelper.CleanSearchString(productIds));
			}
			else
			{
				if (gradeId == -3)
				{
					stringBuilder.AppendFormat("UPDATE Hishop_SKUs SET SalePrice = {0} WHERE ProductId IN ({1})", price, DataHelper.CleanSearchString(productIds));
				}
				else
				{
					stringBuilder.AppendFormat("DELETE FROM Hishop_SKUMemberPrice WHERE GradeId = {0} AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({1}))", gradeId, DataHelper.CleanSearchString(productIds));
					stringBuilder.AppendFormat(" INSERT INTO Hishop_SKUMemberPrice (SkuId,GradeId,MemberSalePrice) SELECT SkuId, {0} AS GradeId, {1} AS MemberSalePrice FROM Hishop_SKUs WHERE ProductId IN ({2})", gradeId, price, DataHelper.CleanSearchString(productIds));
				}
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool UpdateSkuMemberPrices(string productIds, int gradeId, int baseGradeId, string operation, decimal price)
		{
			StringBuilder stringBuilder = new StringBuilder(" ");
			if (gradeId == -2)
			{
				if (baseGradeId == -2)
				{
					stringBuilder.AppendFormat("UPDATE Hishop_SKUs SET CostPrice = CostPrice {0} ({1}) WHERE ProductId IN ({2})", operation, price, DataHelper.CleanSearchString(productIds));
				}
				else
				{
					if (baseGradeId == -3)
					{
						stringBuilder.AppendFormat("UPDATE Hishop_SKUs SET CostPrice = SalePrice {0} ({1}) WHERE ProductId IN ({2})", operation, price, DataHelper.CleanSearchString(productIds));
					}
				}
			}
			else
			{
				if (gradeId == -3)
				{
					if (baseGradeId == -2)
					{
						stringBuilder.AppendFormat("UPDATE Hishop_SKUs SET SalePrice = CostPrice {0} ({1}) WHERE ProductId IN ({2})", operation, price, DataHelper.CleanSearchString(productIds));
					}
					else
					{
						if (baseGradeId == -3)
						{
							stringBuilder.AppendFormat("UPDATE Hishop_SKUs SET SalePrice = SalePrice {0} ({1}) WHERE ProductId IN ({2})", operation, price, DataHelper.CleanSearchString(productIds));
						}
					}
				}
				else
				{
					stringBuilder.AppendFormat("DELETE FROM Hishop_SKUMemberPrice WHERE GradeId = {0} AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({1}))", gradeId, DataHelper.CleanSearchString(productIds));
					if (baseGradeId == -2)
					{
						stringBuilder.AppendFormat(" INSERT INTO Hishop_SKUMemberPrice (SkuId,GradeId,MemberSalePrice) SELECT SkuId, {0} AS GradeId, CostPrice {1} ({2}) AS MemberSalePrice FROM Hishop_SKUs WHERE ProductId IN ({3})", new object[]
						{
							gradeId,
							operation,
							price,
							DataHelper.CleanSearchString(productIds)
						});
					}
					else
					{
						if (baseGradeId == -3)
						{
							stringBuilder.AppendFormat(" INSERT INTO Hishop_SKUMemberPrice (SkuId,GradeId,MemberSalePrice) SELECT SkuId, {0} AS GradeId, SalePrice {1} ({2}) AS MemberSalePrice FROM Hishop_SKUs WHERE ProductId IN ({3})", new object[]
							{
								gradeId,
								operation,
								price,
								DataHelper.CleanSearchString(productIds)
							});
						}
						else
						{
							stringBuilder.AppendFormat(" INSERT INTO Hishop_SKUMemberPrice (SkuId,GradeId,MemberSalePrice) SELECT SkuId, {0} AS GradeId, MemberSalePrice {1} ({2}) AS MemberSalePrice", gradeId, operation, price);
							stringBuilder.AppendFormat(" FROM Hishop_SKUMemberPrice WHERE GradeId = {0} AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({1}))", baseGradeId, DataHelper.CleanSearchString(productIds));
						}
					}
				}
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool UpdateSkuMemberPrices(System.Data.DataSet dataSet_0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			System.Data.DataTable dataTable = dataSet_0.Tables["skuPriceTable"];
			System.Data.DataTable dataTable2 = dataSet_0.Tables["skuMemberPriceTable"];
			string text = string.Empty;
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				foreach (System.Data.DataRow dataRow in dataTable.Rows)
				{
					object obj = text;
					text = string.Concat(new object[]
					{
						obj,
						"'",
						dataRow["skuId"],
						"',"
					});
					stringBuilder.AppendFormat(" UPDATE Hishop_SKUs SET CostPrice = {0}, SalePrice = {1} WHERE SkuId = '{2}'", dataRow["costPrice"], dataRow["salePrice"], dataRow["skuId"]);
				}
			}
			if (text.Length > 1)
			{
				stringBuilder.AppendFormat(" DELETE FROM Hishop_SKUMemberPrice WHERE SkuId IN ({0}) ", text.Remove(text.Length - 1));
			}
			if (dataTable2 != null && dataTable2.Rows.Count > 0)
			{
				foreach (System.Data.DataRow dataRow in dataTable2.Rows)
				{
					stringBuilder.AppendFormat(" INSERT INTO Hishop_SKUMemberPrice (SkuId, GradeId, MemberSalePrice) VALUES ('{0}', {1}, {2})", dataRow["skuId"], dataRow["gradeId"], dataRow["memberPrice"]);
				}
			}
			bool result;
			if (stringBuilder.Length <= 0)
			{
				result = false;
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
				result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
			}
			return result;
		}
		public override System.Data.DataTable GetSkuDistributorPrices(string productIds)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT SkuId, ProductName, SKU, MarketPrice, CostPrice, PurchasePrice");
			stringBuilder.AppendFormat(" FROM Hishop_Products p JOIN Hishop_SKUs s ON p.ProductId = s.ProductId WHERE p.ProductId IN ({0})", DataHelper.CleanSearchString(productIds));
			stringBuilder.Append(" SELECT SkuId, AttributeName, ValueStr FROM Hishop_SKUItems si JOIN Hishop_Attributes a ON si.AttributeId = a.AttributeId JOIN Hishop_AttributeValues av ON si.ValueId = av.ValueId");
			stringBuilder.AppendFormat(" WHERE si.SkuId IN(SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({0}))", DataHelper.CleanSearchString(productIds));
			stringBuilder.AppendLine(" SELECT CAST(GradeId AS NVARCHAR) + '_' + [Name] AS DistributorGradeName,Discount FROM aspnet_DistributorGrades");
			stringBuilder.AppendLine(" SELECT SkuId, (SELECT CAST(GradeId AS NVARCHAR) + '_' + [Name] FROM aspnet_DistributorGrades WHERE GradeId = sd.GradeId) AS DistributorGradeName,  DistributorPurchasePrice");
			stringBuilder.AppendFormat(" FROM Hishop_SKUDistributorPrice sd WHERE SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({0}))", DataHelper.CleanSearchString(productIds));
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
						goto IL_183;
					}
					IEnumerator enumerator = dataTable4.Rows.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							System.Data.DataRow dataRow = (System.Data.DataRow)enumerator.Current;
							dataTable.Columns.Add((string)dataRow["DistributorGradeName"]);
						}
						goto IL_183;
					}
					finally
					{
						IDisposable disposable = enumerator as IDisposable;
						if (disposable != null)
						{
							disposable.Dispose();
						}
					}
					IL_166:
					dataTable.Columns.Add((string)dataReader["DistributorGradeName"]);
					IL_183:
					if (dataReader.Read())
					{
						goto IL_166;
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
							dataRow2[(string)dataRow4["DistributorGradeName"]] = (decimal)dataRow4["DistributorPurchasePrice"];
						}
					}
				}
			}
			if (dataTable4 != null && dataTable4.Rows.Count > 0)
			{
				foreach (System.Data.DataRow dataRow2 in dataTable.Rows)
				{
					decimal d = decimal.Parse(dataRow2["PurchasePrice"].ToString());
					foreach (System.Data.DataRow dataRow4 in dataTable4.Rows)
					{
						decimal d2 = decimal.Parse(dataRow4["Discount"].ToString());
						string arg = (d * (d2 / 100m)).ToString("F2");
						dataRow2[(string)dataRow4["DistributorGradeName"]] = dataRow2[(string)dataRow4["DistributorGradeName"]] + "|" + arg;
					}
				}
			}
			return dataTable;
		}
		public override bool UpdateSkuDistributorPrices(string productIds, int gradeId, decimal price)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (gradeId == -2)
			{
				stringBuilder.AppendFormat("UPDATE Hishop_SKUs SET CostPrice = {0} WHERE ProductId IN ({1})", price, DataHelper.CleanSearchString(productIds));
			}
			else
			{
				if (gradeId == -4)
				{
					stringBuilder.AppendFormat("UPDATE Hishop_SKUs SET PurchasePrice = {0} WHERE ProductId IN ({1})", price, DataHelper.CleanSearchString(productIds));
				}
				else
				{
					stringBuilder.AppendFormat("DELETE FROM Hishop_SKUDistributorPrice WHERE GradeId = {0} AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({1}))", gradeId, DataHelper.CleanSearchString(productIds));
					stringBuilder.AppendFormat(" INSERT INTO Hishop_SKUDistributorPrice (SkuId,GradeId,DistributorPurchasePrice) SELECT SkuId, {0} AS GradeId, {1} AS DistributorPurchasePrice FROM Hishop_SKUs WHERE ProductId IN ({2})", gradeId, price, DataHelper.CleanSearchString(productIds));
				}
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool UpdateSkuDistributorPrices(string productIds, int gradeId, int baseGradeId, string operation, decimal price)
		{
			StringBuilder stringBuilder = new StringBuilder(" ");
			if (gradeId == -2)
			{
				if (baseGradeId == -2)
				{
					stringBuilder.AppendFormat("UPDATE Hishop_SKUs SET CostPrice = CostPrice {0} ({1}) WHERE ProductId IN ({2})", operation, price, DataHelper.CleanSearchString(productIds));
				}
				else
				{
					if (baseGradeId == -4)
					{
						stringBuilder.AppendFormat("UPDATE Hishop_SKUs SET CostPrice = PurchasePrice {0} ({1}) WHERE ProductId IN ({2})", operation, price, DataHelper.CleanSearchString(productIds));
					}
				}
			}
			else
			{
				if (gradeId == -4)
				{
					if (baseGradeId == -2)
					{
						stringBuilder.AppendFormat("UPDATE Hishop_SKUs SET PurchasePrice = CostPrice {0} ({1}) WHERE ProductId IN ({2})", operation, price, DataHelper.CleanSearchString(productIds));
					}
					else
					{
						if (baseGradeId == -4)
						{
							stringBuilder.AppendFormat("UPDATE Hishop_SKUs SET PurchasePrice = PurchasePrice {0} ({1}) WHERE ProductId IN ({2})", operation, price, DataHelper.CleanSearchString(productIds));
						}
					}
				}
				else
				{
					stringBuilder.AppendFormat("DELETE FROM Hishop_SKUDistributorPrice WHERE GradeId = {0} AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({1}))", gradeId, DataHelper.CleanSearchString(productIds));
					if (baseGradeId == -2)
					{
						stringBuilder.Append(" INSERT INTO Hishop_SKUDistributorPrice (SkuId,GradeId,DistributorPurchasePrice)");
						stringBuilder.AppendFormat("  SELECT SkuId, {0} AS GradeId, CostPrice {1} ({2}) AS DistributorPurchasePrice FROM Hishop_SKUs WHERE ProductId IN ({3})", new object[]
						{
							gradeId,
							operation,
							price,
							DataHelper.CleanSearchString(productIds)
						});
					}
					else
					{
						if (baseGradeId == -4)
						{
							stringBuilder.Append(" INSERT INTO Hishop_SKUDistributorPrice (SkuId,GradeId,DistributorPurchasePrice)");
							stringBuilder.AppendFormat("  SELECT SkuId, {0} AS GradeId, PurchasePrice {1} ({2}) AS DistributorPurchasePrice FROM Hishop_SKUs WHERE ProductId IN ({3})", new object[]
							{
								gradeId,
								operation,
								price,
								DataHelper.CleanSearchString(productIds)
							});
						}
						else
						{
							stringBuilder.Append(" INSERT INTO Hishop_SKUDistributorPrice (SkuId,GradeId,DistributorPurchasePrice)");
							stringBuilder.AppendFormat(" SELECT SkuId, {0} AS GradeId, DistributorPurchasePrice {1} ({2}) AS DistributorPurchasePrice FROM Hishop_SKUDistributorPrice", gradeId, operation, price);
							stringBuilder.AppendFormat(" WHERE GradeId = {0} AND SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId IN ({1}))", baseGradeId, DataHelper.CleanSearchString(productIds));
						}
					}
				}
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool UpdateSkuDistributorPrices(System.Data.DataSet dataSet_0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			System.Data.DataTable dataTable = dataSet_0.Tables["skuPriceTable"];
			System.Data.DataTable dataTable2 = dataSet_0.Tables["skuDistributorPriceTable"];
			string text = string.Empty;
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				foreach (System.Data.DataRow dataRow in dataTable.Rows)
				{
					object obj = text;
					text = string.Concat(new object[]
					{
						obj,
						"'",
						dataRow["skuId"],
						"',"
					});
					stringBuilder.AppendFormat(" UPDATE Hishop_SKUs SET CostPrice = {0}, PurchasePrice = {1} WHERE SkuId = '{2}'", dataRow["costPrice"], dataRow["purchasePrice"], dataRow["skuId"]);
				}
			}
			if (text.Length > 1)
			{
				stringBuilder.AppendFormat(" DELETE FROM Hishop_SKUDistributorPrice WHERE SkuId IN ({0}) ", text.Remove(text.Length - 1));
			}
			if (dataTable2 != null && dataTable2.Rows.Count > 0)
			{
				foreach (System.Data.DataRow dataRow in dataTable2.Rows)
				{
					stringBuilder.AppendFormat(" INSERT INTO Hishop_SKUDistributorPrice (SkuId, GradeId, DistributorPurchasePrice) VALUES ('{0}', {1}, {2})", dataRow["skuId"], dataRow["gradeId"], dataRow["distributorPrice"]);
				}
			}
			bool result;
			if (stringBuilder.Length <= 0)
			{
				result = false;
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
				result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
			}
			return result;
		}
		public override DbQueryResult GetRelatedProducts(Pagination page, int productId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" SaleStatus = {0}", 1);
			stringBuilder.AppendFormat(" AND ProductId IN (SELECT RelatedProductId FROM Hishop_RelatedProducts WHERE ProductId = {0})", productId);
			string selectFields = "ProductId, ProductCode, ProductName, ThumbnailUrl40, MarketPrice, SalePrice, Stock, DisplaySequence";
			return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "vw_Hishop_BrowseProductList p", "ProductId", stringBuilder.ToString(), selectFields);
		}
		public override bool AddRelatedProduct(int productId, int relatedProductId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_RelatedProducts(ProductId, RelatedProductId) VALUES (@ProductId, @RelatedProductId)");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_RelatedProducts WHERE ProductId = @ProductId AND RelatedProductId = @RelatedProductId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			this.database.AddInParameter(sqlStringCommand, "RelatedProductId", System.Data.DbType.Int32, relatedProductId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool ClearRelatedProducts(int productId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_RelatedProducts WHERE ProductId = @ProductId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool AddSupplier(string supplierName, string remark)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("IF (SELECT COUNT(*) FROM Hishop_Suppliers WHERE LOWER(SupplierName)=LOWER(@SupplierName)) = 0 INSERT INTO Hishop_Suppliers(SupplierName, Remark) VALUES(@SupplierName, @Remark)");
			this.database.AddInParameter(sqlStringCommand, "SupplierName", System.Data.DbType.String, supplierName);
			this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, remark);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override void DeleteSupplier(string supplierName)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Suppliers WHERE LOWER(SupplierName)=LOWER(@SupplierName)");
			this.database.AddInParameter(sqlStringCommand, "SupplierName", System.Data.DbType.String, supplierName);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override string GetSupplierRemark(string supplierName)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Remark FROM Hishop_Suppliers WHERE LOWER(SupplierName)=LOWER(@SupplierName)");
			this.database.AddInParameter(sqlStringCommand, "SupplierName", System.Data.DbType.String, supplierName);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			string result;
			if (obj != null && obj != DBNull.Value)
			{
				result = (string)obj;
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}
		public override DbQueryResult GetSuppliers(Pagination page)
		{
			return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, true, "Hishop_Suppliers", "SupplierName", "", "SupplierName,Remark");
		}
		public override IList<string> GetSuppliers()
		{
			IList<string> list = new List<string>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SupplierName FROM Hishop_Suppliers");
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(dataReader.GetString(0));
				}
			}
			return list;
		}
		public override bool UpdateSupplier(string oldName, string newName, string remark)
		{
			bool result;
			if (!oldName.Equals(newName))
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_Suppliers WHERE LOWER(SupplierName)=LOWER(@SupplierName)");
				this.database.AddInParameter(sqlStringCommand, "SupplierName", System.Data.DbType.String, newName);
				if ((int)this.database.ExecuteScalar(sqlStringCommand) == 0)
				{
					System.Data.Common.DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand("UPDATE Hishop_Suppliers SET SupplierName=@SupplierName,Remark=@Remark WHERE LOWER(SupplierName) = LOWER(@OldSupplierName)");
					this.database.AddInParameter(sqlStringCommand2, "SupplierName", System.Data.DbType.String, newName);
					this.database.AddInParameter(sqlStringCommand2, "Remark", System.Data.DbType.String, remark);
					this.database.AddInParameter(sqlStringCommand2, "OldSupplierName", System.Data.DbType.String, oldName);
					result = (this.database.ExecuteNonQuery(sqlStringCommand2) >= 1);
				}
				else
				{
					result = false;
				}
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand3 = this.database.GetSqlStringCommand("UPDATE Hishop_Suppliers SET Remark=@Remark WHERE LOWER(SupplierName)=LOWER(@SupplierName)");
				this.database.AddInParameter(sqlStringCommand3, "SupplierName", System.Data.DbType.String, newName);
				this.database.AddInParameter(sqlStringCommand3, "Remark", System.Data.DbType.String, remark);
				result = (this.database.ExecuteNonQuery(sqlStringCommand3) >= 1);
			}
			return result;
		}
		public override DbQueryResult GetExportProducts(AdvancedProductQuery query, string removeProductIds)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("(");
			if (query.IncludeOnSales)
			{
				stringBuilder.AppendFormat("SaleStatus = {0} OR ", 1);
			}
			if (query.IncludeUnSales)
			{
				stringBuilder.AppendFormat("SaleStatus = {0} OR ", 2);
			}
			if (query.IncludeInStock)
			{
				stringBuilder.AppendFormat("SaleStatus = {0} OR ", 3);
			}
			stringBuilder.Remove(stringBuilder.Length - 4, 4);
			stringBuilder.Append(")");
			if (query.BrandId.HasValue)
			{
				stringBuilder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
			}
			if (query.IsMakeTaobao.HasValue && query.IsMakeTaobao != -1)
			{
				stringBuilder.AppendFormat(" AND IsMakeTaobao={0}  ", query.IsMakeTaobao);
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
			if (query.ProductLineId.HasValue && query.ProductLineId.Value > 0)
			{
				stringBuilder.AppendFormat(" AND LineId={0}", Convert.ToInt32(query.ProductLineId.Value));
			}
			if (query.PenetrationStatus != PenetrationStatus.NotSet)
			{
				stringBuilder.AppendFormat(" AND PenetrationStatus={0}", (int)query.PenetrationStatus);
			}
			if (!string.IsNullOrEmpty(query.ProductCode))
			{
				stringBuilder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
			}
			if (query.CategoryId.HasValue && query.CategoryId.Value > 0)
			{
				stringBuilder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%'  OR ExtendCategoryPath LIKE '{0}|%' )", query.MaiCategoryPath);
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
			stringBuilder.Append("SELECT a.[ProductId], [TypeId], [ProductName], [ProductCode], [ShortDescription], [Unit], [Description], ").Append("[Title], [Meta_Description], [Meta_Keywords], [SaleStatus], [ImageUrl1], [ImageUrl2], [ImageUrl3], ").Append("[ImageUrl4], [ImageUrl5], [MarketPrice], [LowestSalePrice], [PenetrationStatus], [HasSKU] ").Append("FROM Hishop_Products a  left join Taobao_Products b on a.productid=b.productid WHERE ");
			stringBuilder.Append("(");
			if (query.IncludeOnSales)
			{
				stringBuilder.AppendFormat("SaleStatus = {0} OR ", 1);
			}
			if (query.IncludeUnSales)
			{
				stringBuilder.AppendFormat("SaleStatus = {0} OR ", 2);
			}
			if (query.IncludeInStock)
			{
				stringBuilder.AppendFormat("SaleStatus = {0} OR ", 3);
			}
			stringBuilder.Remove(stringBuilder.Length - 4, 4);
			stringBuilder.Append(")");
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
			if (query.BrandId.HasValue)
			{
				stringBuilder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
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
			if (query.ProductLineId.HasValue && query.ProductLineId.Value > 0)
			{
				stringBuilder.AppendFormat(" AND LineId={0}", Convert.ToInt32(query.ProductLineId.Value));
			}
			if (query.PenetrationStatus != PenetrationStatus.NotSet)
			{
				stringBuilder.AppendFormat(" AND PenetrationStatus={0}", (int)query.PenetrationStatus);
			}
			if (!string.IsNullOrEmpty(query.ProductCode))
			{
				stringBuilder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
			}
			if (query.CategoryId.HasValue && query.CategoryId.Value > 0)
			{
				stringBuilder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%'  OR ExtendCategoryPath LIKE '{0}|%' )", query.MaiCategoryPath);
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
		public override void EnsureMapping(System.Data.DataSet mappingSet)
		{
			using (System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO  Hishop_ProductTypes (TypeName, Remark) VALUES(@TypeName, @Remark);SELECT @@IDENTITY;"))
			{
				this.database.AddInParameter(sqlStringCommand, "TypeName", System.Data.DbType.String);
				this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String);
				System.Data.DataRow[] array = mappingSet.Tables["types"].Select("SelectedTypeId=0");
				System.Data.DataRow[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					System.Data.DataRow dataRow = array2[i];
					this.database.SetParameterValue(sqlStringCommand, "TypeName", dataRow["TypeName"]);
					this.database.SetParameterValue(sqlStringCommand, "Remark", dataRow["Remark"]);
					dataRow["SelectedTypeId"] = this.database.ExecuteScalar(sqlStringCommand);
				}
			}
			using (System.Data.Common.DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_Attributes; INSERT INTO Hishop_Attributes(AttributeName, DisplaySequence, TypeId, UsageMode, UseAttributeImage)  VALUES(@AttributeName, @DisplaySequence, @TypeId, @UsageMode, @UseAttributeImage);SELECT @@IDENTITY;"))
			{
				this.database.AddInParameter(sqlStringCommand2, "AttributeName", System.Data.DbType.String);
				this.database.AddInParameter(sqlStringCommand2, "TypeId", System.Data.DbType.Int32);
				this.database.AddInParameter(sqlStringCommand2, "UsageMode", System.Data.DbType.Int32);
				this.database.AddInParameter(sqlStringCommand2, "UseAttributeImage", System.Data.DbType.Boolean);
				System.Data.DataRow[] array3 = mappingSet.Tables["attributes"].Select("SelectedAttributeId=0");
				System.Data.DataRow[] array2 = array3;
				for (int i = 0; i < array2.Length; i++)
				{
					System.Data.DataRow dataRow2 = array2[i];
					int num = (int)mappingSet.Tables["types"].Select(string.Format("MappedTypeId={0}", dataRow2["MappedTypeId"]))[0]["SelectedTypeId"];
					this.database.SetParameterValue(sqlStringCommand2, "AttributeName", dataRow2["AttributeName"]);
					this.database.SetParameterValue(sqlStringCommand2, "TypeId", num);
					this.database.SetParameterValue(sqlStringCommand2, "UsageMode", int.Parse(dataRow2["UsageMode"].ToString()));
					this.database.SetParameterValue(sqlStringCommand2, "UseAttributeImage", bool.Parse(dataRow2["UseAttributeImage"].ToString()));
					dataRow2["SelectedAttributeId"] = this.database.ExecuteScalar(sqlStringCommand2);
				}
			}
			using (System.Data.Common.DbCommand sqlStringCommand3 = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_AttributeValues;INSERT INTO Hishop_AttributeValues(AttributeId, DisplaySequence, ValueStr, ImageUrl) VALUES(@AttributeId, @DisplaySequence, @ValueStr, @ImageUrl);SELECT @@IDENTITY;"))
			{
				this.database.AddInParameter(sqlStringCommand3, "AttributeId", System.Data.DbType.Int32);
				this.database.AddInParameter(sqlStringCommand3, "ValueStr", System.Data.DbType.String);
				this.database.AddInParameter(sqlStringCommand3, "ImageUrl", System.Data.DbType.String);
				System.Data.DataRow[] array4 = mappingSet.Tables["values"].Select("SelectedValueId=0");
				System.Data.DataRow[] array2 = array4;
				for (int i = 0; i < array2.Length; i++)
				{
					System.Data.DataRow dataRow3 = array2[i];
					int num2 = (int)mappingSet.Tables["attributes"].Select(string.Format("MappedAttributeId={0}", dataRow3["MappedAttributeId"]))[0]["SelectedAttributeId"];
					this.database.SetParameterValue(sqlStringCommand3, "AttributeId", num2);
					this.database.SetParameterValue(sqlStringCommand3, "ValueStr", dataRow3["ValueStr"]);
					this.database.SetParameterValue(sqlStringCommand3, "ImageUrl", dataRow3["ImageUrl"]);
					dataRow3["SelectedValueId"] = this.database.ExecuteScalar(sqlStringCommand3);
				}
			}
			mappingSet.AcceptChanges();
		}
		public override System.Data.DataSet GetTaobaoProductDetails(int productId)
		{
			System.Data.DataSet dataSet = new System.Data.DataSet();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ProductId, HasSKU, ProductName, ProductCode, MarketPrice, (SELECT [Name] FROM Hishop_Categories WHERE CategoryId = p.CategoryId) AS CategoryName, (SELECT [Name] FROM Hishop_ProductLines WHERE LineId = p.LineId) AS ProductLineName, (SELECT BrandName FROM Hishop_BrandCategories WHERE BrandId = p.BrandId) AS BrandName, (SELECT MIN(SalePrice) FROM Hishop_SKUs WHERE ProductId = p.ProductId) AS SalePrice, (SELECT MIN(CostPrice) FROM Hishop_SKUs WHERE ProductId = p.ProductId) AS CostPrice, (SELECT MIN(PurchasePrice) FROM Hishop_SKUs WHERE ProductId = p.ProductId) AS PurchasePrice, (SELECT SUM(Stock) FROM Hishop_SKUs WHERE ProductId = p.ProductId) AS Stock FROM Hishop_Products p WHERE ProductId = @ProductId SELECT AttributeName, ValueStr FROM Hishop_ProductAttributes pa join Hishop_Attributes a ON pa.AttributeId = a.AttributeId JOIN Hishop_AttributeValues v ON a.AttributeId = v.AttributeId AND pa.ValueId = v.ValueId WHERE ProductId = @ProductId ORDER BY a.DisplaySequence DESC, v.DisplaySequence DESC SELECT Weight AS '重量', Stock AS '库存', PurchasePrice AS '采购价', CostPrice AS '成本价', SalePrice AS '一口价', SkuId AS '商家编码' FROM Hishop_SKUs s WHERE ProductId = @ProductId; SELECT SkuId AS '商家编码',AttributeName,UseAttributeImage,ValueStr,ImageUrl FROM Hishop_SKUItems s join Hishop_Attributes a on s.AttributeId = a.AttributeId join Hishop_AttributeValues av on s.ValueId = av.ValueId WHERE SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId = @ProductId) ORDER BY a.DisplaySequence DESC SELECT * FROM Taobao_Products WHERE ProductId = @ProductId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			System.Data.DataTable table;
			System.Data.DataTable table2;
			System.Data.DataTable dataTable;
			System.Data.DataTable dataTable2;
			System.Data.DataTable table3;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				table = DataHelper.ConverDataReaderToDataTable(dataReader);
				dataReader.NextResult();
				table2 = DataHelper.ConverDataReaderToDataTable(dataReader);
				dataReader.NextResult();
				dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
				dataReader.NextResult();
				dataTable2 = DataHelper.ConverDataReaderToDataTable(dataReader);
				dataReader.NextResult();
				table3 = DataHelper.ConverDataReaderToDataTable(dataReader);
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
						if (string.Compare((string)dataRow2["商家编码"], (string)dataRow["商家编码"]) == 0)
						{
							dataRow2[(string)dataRow["AttributeName"]] = dataRow["ValueStr"];
						}
					}
				}
			}
			dataSet.Tables.Add(table);
			dataSet.Tables.Add(table2);
			dataSet.Tables.Add(dataTable);
			dataSet.Tables.Add(table3);
			return dataSet;
		}
		public override bool UpdateToaobProduct(TaobaoProductInfo taobaoProduct)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Taobao_Products WHERE ProductId = @ProductId;INSERT INTO Taobao_Products(Cid, StuffStatus, ProductId, ProTitle,Num, LocationState, LocationCity, FreightPayer, PostFee, ExpressFee, EMSFee, HasInvoice, HasWarranty, HasDiscount, ValidThru, ListTime, PropertyAlias,InputPids,InputStr, SkuProperties, SkuQuantities, SkuPrices, SkuOuterIds,FoodAttributes) VALUES(@Cid, @StuffStatus, @ProductId, @ProTitle,@Num, @LocationState, @LocationCity, @FreightPayer, @PostFee, @ExpressFee, @EMSFee, @HasInvoice, @HasWarranty, @HasDiscount, @ValidThru, @ListTime,@PropertyAlias,@InputPids, @InputStr, @SkuProperties, @SkuQuantities, @SkuPrices, @SkuOuterIds,@FoodAttributes);update Taobao_DistroProducts set  updatestatus=1 where  ProductId = @ProductId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, taobaoProduct.ProductId);
			this.database.AddInParameter(sqlStringCommand, "Cid", System.Data.DbType.Int64, taobaoProduct.Cid);
			this.database.AddInParameter(sqlStringCommand, "StuffStatus", System.Data.DbType.String, taobaoProduct.StuffStatus);
			this.database.AddInParameter(sqlStringCommand, "ProTitle", System.Data.DbType.String, taobaoProduct.ProTitle);
			this.database.AddInParameter(sqlStringCommand, "Num", System.Data.DbType.Int64, taobaoProduct.Num);
			this.database.AddInParameter(sqlStringCommand, "LocationState", System.Data.DbType.String, taobaoProduct.LocationState);
			this.database.AddInParameter(sqlStringCommand, "LocationCity", System.Data.DbType.String, taobaoProduct.LocationCity);
			this.database.AddInParameter(sqlStringCommand, "FreightPayer", System.Data.DbType.String, taobaoProduct.FreightPayer);
			this.database.AddInParameter(sqlStringCommand, "PostFee", System.Data.DbType.Currency, taobaoProduct.PostFee);
			this.database.AddInParameter(sqlStringCommand, "ExpressFee", System.Data.DbType.Currency, taobaoProduct.ExpressFee);
			this.database.AddInParameter(sqlStringCommand, "EMSFee", System.Data.DbType.Currency, taobaoProduct.EMSFee);
			this.database.AddInParameter(sqlStringCommand, "HasInvoice", System.Data.DbType.Boolean, taobaoProduct.HasInvoice);
			this.database.AddInParameter(sqlStringCommand, "HasWarranty", System.Data.DbType.Boolean, taobaoProduct.HasWarranty);
			this.database.AddInParameter(sqlStringCommand, "HasDiscount", System.Data.DbType.Boolean, taobaoProduct.HasDiscount);
			this.database.AddInParameter(sqlStringCommand, "ValidThru", System.Data.DbType.Int64, taobaoProduct.ValidThru);
			this.database.AddInParameter(sqlStringCommand, "ListTime", System.Data.DbType.DateTime, taobaoProduct.ListTime);
			this.database.AddInParameter(sqlStringCommand, "PropertyAlias", System.Data.DbType.String, taobaoProduct.PropertyAlias);
			this.database.AddInParameter(sqlStringCommand, "InputPids", System.Data.DbType.String, taobaoProduct.InputPids);
			this.database.AddInParameter(sqlStringCommand, "InputStr", System.Data.DbType.String, taobaoProduct.InputStr);
			this.database.AddInParameter(sqlStringCommand, "SkuProperties", System.Data.DbType.String, taobaoProduct.SkuProperties);
			this.database.AddInParameter(sqlStringCommand, "SkuQuantities", System.Data.DbType.String, taobaoProduct.SkuQuantities);
			this.database.AddInParameter(sqlStringCommand, "SkuPrices", System.Data.DbType.String, taobaoProduct.SkuPrices);
			this.database.AddInParameter(sqlStringCommand, "SkuOuterIds", System.Data.DbType.String, taobaoProduct.SkuOuterIds);
			this.database.AddInParameter(sqlStringCommand, "FoodAttributes", System.Data.DbType.String, taobaoProduct.FoodAttributes);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool IsExitTaobaoProduct(long taobaoProductId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT COUNT(*) FROM Hishop_Products WHERE TaobaoProductId = {0}", taobaoProductId));
			return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
		}
		public override DbQueryResult GetSubmitPuchaseProductsByDistorUserId(ProductQuery query, int distorUserId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("PenetrationStatus=1 AND LineId IN (SELECT LineId FROM Hishop_DistributorProductLines WHERE UserId={0}) AND SaleStatus<>{1} ", distorUserId, 0);
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
		public override System.Data.DataTable GetSkusByProductIdByDistorId(int productId, int distorUserId)
		{
			IUser user = Users.GetUser(distorUserId, false);
			System.Data.DataTable result;
			if (user == null || user.IsAnonymous || user.UserRole != UserRole.Distributor)
			{
				result = null;
			}
			else
			{
				Distributor distributor = user as Distributor;
				if (distributor == null)
				{
					result = null;
				}
				else
				{
					int distributorDiscount = this.GetDistributorDiscount(distributor.GradeId);
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock, AlertStock, CostPrice,");
					stringBuilder.AppendFormat(" ISNULL((SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = s.SkuId AND DistributoruserId = {0}), s.SalePrice) AS SalePrice,", distributor.UserId);
					stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", distributor.GradeId);
					stringBuilder.AppendFormat(" THEN (SELECT DistributorPurchasePrice FROM Hishop_SKUDistributorPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE PurchasePrice*{1}/100 END) AS PurchasePrice", distributor.GradeId, distributorDiscount);
					stringBuilder.Append(" FROM Hishop_SKUs s WHERE ProductId = @ProductId");
					System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
					this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
					System.Data.DataTable dataTable;
					using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
					{
						dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
					}
					result = dataTable;
				}
			}
			return result;
		}
		public override System.Data.DataTable GetSkuContentBySkuBuDistorUserId(string skuId, int distorUserId)
		{
			System.Data.DataTable dataTable = null;
			IUser user = Users.GetUser(distorUserId, false);
			System.Data.DataTable result;
			if (user == null || user.IsAnonymous || user.UserRole != UserRole.Distributor)
			{
				result = null;
			}
			else
			{
				Distributor distributor = user as Distributor;
				if (distributor == null)
				{
					result = null;
				}
				else
				{
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
						dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
					}
					result = dataTable;
				}
			}
			return result;
		}
		private int GetDistributorDiscount(int gradeId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Discount FROM aspnet_DistributorGrades WHERE GradeId=@GradeId");
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, gradeId);
			return (int)this.database.ExecuteScalar(sqlStringCommand);
		}
		public override System.Data.DataSet GetProductsByQuery(ProductQuery query, out int totalrecord)
		{
			System.Data.DataSet dataSet = new System.Data.DataSet();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1 ");
			if (query.SaleStatus != ProductSaleStatus.All)
			{
				stringBuilder.AppendFormat(" AND SaleStatus = {0}", (int)query.SaleStatus);
			}
			else
			{
				stringBuilder.AppendFormat(" AND SaleStatus not in ({0})", 0);
			}
			if (query.BrandId.HasValue)
			{
				stringBuilder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
			}
			if (query.TagId.HasValue)
			{
				stringBuilder.AppendFormat("AND ProductId IN (SELECT ProductId FROM Hishop_ProductTag WHERE TagId={0})", query.TagId);
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
				stringBuilder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%'  OR ExtendCategoryPath LIKE '{0}|%' )", query.MaiCategoryPath);
			}
			if (query.IsMakeTaobao.HasValue && query.IsMakeTaobao.Value >= 0)
			{
				stringBuilder.AppendFormat(" AND IsMaketaobao={0}", query.IsMakeTaobao.Value);
			}
			if (query.PublishStatus != PublishStatus.NotSet)
			{
				if (query.PublishStatus == PublishStatus.Notyet)
				{
					stringBuilder.Append(" AND TaobaoProductId = 0");
				}
				else
				{
					stringBuilder.Append(" AND TaobaoProductId <> 0");
				}
			}
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate >='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate <='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
			}
			string text = string.Concat(new object[]
			{
				"SELECT TOP ",
				query.PageSize,
				" ProductId,ProductName,ProductCode,ThumbnailUrl60,MarketPrice,SaleStatus,SalePrice,Weight from vw_Hishop_BrowseProductList WHERE ",
				stringBuilder.ToString(),
				";"
			});
			if (query.PageIndex > 1)
			{
				text = string.Concat(new object[]
				{
					"SELECT TOP ",
					query.PageSize,
					" ProductId,ProductName,ProductCode,ThumbnailUrl60,MarketPrice,SaleStatus,SalePrice,Weight from vw_Hishop_BrowseProductList WHERE (ProductId>(SELECT max(ProductId) from (SELECT TOP ",
					(query.PageIndex - 1) * query.PageSize,
					" ProductId FROM vw_Hishop_BrowseProductList WHERE ",
					stringBuilder.ToString(),
					" order by ProductId) as T)) AND ",
					stringBuilder.ToString(),
					" order by ProductId;"
				});
			}
			text += "select ProductId,SkuId,SKU,Stock,SalePrice from dbo.Hishop_SKUs;";
			text = text + "SELECT COUNT(*) AS SumRecord FROM vw_Hishop_BrowseProductList WHERE " + stringBuilder.ToString();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			System.Data.DataSet dataSet2;
			dataSet = (dataSet2 = this.database.ExecuteDataSet(sqlStringCommand));
			try
			{
				dataSet.Relations.Add("ProductRealation", dataSet.Tables[0].Columns["ProductId"], dataSet.Tables[1].Columns["ProductId"], false);
			}
			finally
			{
				if (dataSet2 != null)
				{
					((IDisposable)dataSet2).Dispose();
				}
			}
			totalrecord = Convert.ToInt32(dataSet.Tables[2].Rows[0]["SumRecord"].ToString());
			return dataSet;
		}
		public override System.Data.DataSet GetProductSkuDetials(int productId)
		{
			System.Data.DataSet result = new System.Data.DataSet();
			if (!string.IsNullOrEmpty(productId.ToString()) && Convert.ToInt32(productId) > 0)
			{
				string text = string.Concat(new object[]
				{
					"SELECT ProductId,ProductName,ProductCode,ThumbnailUrl60,MarketPrice,SaleStatus,SalePrice,Weight from vw_Hishop_BrowseProductList WHERE ProductId=",
					productId,
					";SELECT ProductId,SkuId,SKU,Stock,SalePrice from dbo.Hishop_SKUs WHERE ProductId=",
					productId
				});
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
				result = this.database.ExecuteDataSet(sqlStringCommand);
			}
			return result;
		}
		public override System.Data.DataTable GetTags()
		{
			System.Data.DataTable result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *  FROM  Hishop_Tags");
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override string GetTagName(int tagId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT TagName  FROM  Hishop_Tags WHERE TagID = {0}", tagId));
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			string result;
			if (obj != null)
			{
				result = obj.ToString();
			}
			else
			{
				result = string.Empty;
			}
			return result;
		}
		public override int AddTags(string tagname)
		{
			int result = 0;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_Tags VALUES(@TagName);SELECT @@IDENTITY");
			this.database.AddInParameter(sqlStringCommand, "TagName", System.Data.DbType.String, tagname);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			if (obj != null)
			{
				result = Convert.ToInt32(obj.ToString());
			}
			return result;
		}
		public override bool UpdateTags(int tagId, string tagName)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Tags SET TagName=@TagName WHERE TagID=@TagID");
			this.database.AddInParameter(sqlStringCommand, "TagName", System.Data.DbType.String, tagName);
			this.database.AddInParameter(sqlStringCommand, "TagID", System.Data.DbType.Int32, tagId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool DeleteTags(int tagId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ProductTag WHERE TagID=@TagID;DELETE FROM distro_ProductTag WHERE TagId=@TagID;DELETE FROM Hishop_Tags WHERE TagID=@TagID;");
			this.database.AddInParameter(sqlStringCommand, "TagID", System.Data.DbType.Int32, tagId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override int GetTags(string tagName)
		{
			int result = 0;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TagID  FROM  Hishop_Tags WHERE TagName=@TagName");
			this.database.AddInParameter(sqlStringCommand, "TagName", System.Data.DbType.String, tagName);
			System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand);
			if (dataReader.Read())
			{
				result = Convert.ToInt32(dataReader["TagID"].ToString());
			}
			return result;
		}
		public override bool AddProductTags(int productId, IList<int> tagIds, System.Data.Common.DbTransaction tran)
		{
			bool flag = false;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_ProductTag VALUES(@TagId,@ProductId)");
			this.database.AddInParameter(sqlStringCommand, "TagId", System.Data.DbType.Int32);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32);
			foreach (int current in tagIds)
			{
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ProductTag WHERE ProductId=@ProductId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
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
	}
}
