using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Promotions;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core.Enums;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Member;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
namespace Hidistro.SaleSystem.Data
{
	public class ProductData : ProductMasterProvider
	{
		private Database database;
		public ProductData()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public override System.Data.DataTable GetSaleProductRanking(int? categoryId, int maxNum)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT TOP {0} ProductId, ProductName, ProductCode, ShowSaleCounts AS SaleCounts, ThumbnailUrl40, ThumbnailUrl60, ThumbnailUrl100,", maxNum);
			stringBuilder.AppendFormat("  ThumbnailUrl160, ThumbnailUrl180, ThumbnailUrl220, SalePrice, MarketPrice FROM vw_Hishop_BrowseProductList WHERE SaleStatus = {0}", 1);
			if (categoryId.HasValue)
			{
				CategoryInfo category = CategoryBrowser.GetCategory(categoryId.Value);
				if (category != null)
				{
					stringBuilder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%') ", category.Path);
				}
			}
			stringBuilder.Append("ORDER BY ShowSaleCounts DESC");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			System.Data.DataTable result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override System.Data.DataTable GetSubjectList(SubjectListQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (HiContext.Current.User.UserRole == UserRole.Member)
			{
                Hidistro.Membership.Context.Member member = HiContext.Current.User as Hidistro.Membership.Context.Member;
				int memberDiscount = MemberProvider.Instance().GetMemberDiscount(member.GradeId);
				stringBuilder.AppendFormat("SELECT TOP {0} ProductId,ProductName,ProductCode,ShowSaleCounts AS SaleCounts,ShortDescription,", query.MaxNum);
				stringBuilder.Append(" ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,SalePrice,");
				stringBuilder.AppendFormat(" CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) = 1 ", member.GradeId);
				stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END AS RankPrice", member.GradeId, memberDiscount);
			}
			else
			{
				stringBuilder.AppendFormat("SELECT TOP {0} ProductId,ProductName,ProductCode,ShowSaleCounts AS SaleCounts,ShortDescription,", query.MaxNum);
				stringBuilder.Append(" ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,SalePrice,SalePrice AS RankPrice");
			}
			stringBuilder.Append(" FROM vw_Hishop_BrowseProductList p WHERE ");
			stringBuilder.Append(ProductMasterProvider.BuildProductSubjectQuerySearch(query));
			if (!string.IsNullOrEmpty(query.SortBy))
			{
				stringBuilder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), DataHelper.CleanSearchString(query.SortOrder.ToString()));
			}
			System.Data.DataTable result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override ProductInfo GetProductSimpleInfo(int productId)
		{
			ProductInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Products WHERE ProductId = @ProductId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateProduct(dataReader);
				}
			}
			return result;
		}
		public override ProductBrowseInfo GetProductBrowseInfo(int productId, int? maxReviewNum, int? maxConsultationNum)
		{
			ProductBrowseInfo productBrowseInfo = new ProductBrowseInfo();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE Hishop_Products SET VistiCounts = VistiCounts + 1 WHERE ProductId = @ProductId;");
			stringBuilder.Append(" SELECT * ,CASE WHEN BrandId IS NULL THEN NULL ELSE (SELECT BrandName FROM Hishop_BrandCategories WHERE BrandId= p.BrandId) END AS BrandName");
			stringBuilder.Append(" FROM Hishop_Products p where ProductId=@ProductId;");
			if (HiContext.Current.User.UserRole == UserRole.Member)
			{
                Hidistro.Membership.Context.Member member = HiContext.Current.User as Hidistro.Membership.Context.Member;
				int memberDiscount = MemberProvider.Instance().GetMemberDiscount(member.GradeId);
				stringBuilder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock, AlertStock, CostPrice, PurchasePrice,");
				stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", member.GradeId);
				stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", member.GradeId, memberDiscount);
				stringBuilder.Append(" FROM Hishop_SKUs s WHERE ProductId = @ProductId");
			}
			else
			{
				stringBuilder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock, AlertStock, CostPrice, PurchasePrice, SalePrice FROM Hishop_SKUs WHERE ProductId = @ProductId");
			}
			if (maxReviewNum.HasValue)
			{
				stringBuilder.AppendFormat(" SELECT TOP {0} * FROM Hishop_ProductReviews where ProductId=@ProductId ORDER BY ReviewId DESC ; ", maxReviewNum);
			}
			if (maxConsultationNum.HasValue)
			{
				stringBuilder.AppendFormat(" SELECT TOP {0} * FROM Hishop_ProductConsultations where ProductId=@ProductId AND ReplyUserId IS NOT NULL ORDER BY ConsultationId DESC ;", maxConsultationNum);
			}
			stringBuilder.Append(" SELECT a.AttributeId, AttributeName, ValueStr FROM Hishop_ProductAttributes pa JOIN Hishop_Attributes a ON pa.AttributeId = a.AttributeId");
			stringBuilder.Append(" JOIN Hishop_AttributeValues v ON a.AttributeId = v.AttributeId AND pa.ValueId = v.ValueId  WHERE ProductId = @ProductId ORDER BY a.DisplaySequence DESC, v.DisplaySequence DESC");
			stringBuilder.Append(" SELECT SkuId, a.AttributeId, AttributeName, UseAttributeImage, av.ValueId, ValueStr, ImageUrl FROM Hishop_SKUItems s join Hishop_Attributes a on s.AttributeId = a.AttributeId join Hishop_AttributeValues av on s.ValueId = av.ValueId WHERE SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId = @ProductId) ORDER BY a.DisplaySequence DESC,av.DisplaySequence DESC;");
			stringBuilder.Append(" SELECT TOP 20 ProductId,ProductName,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,SalePrice FROM vw_Hishop_BrowseProductList");
			stringBuilder.AppendFormat(" WHERE SaleStatus = {0} AND ProductId IN (SELECT RelatedProductId FROM Hishop_RelatedProducts WHERE ProductId = {1})", 1, productId);
			stringBuilder.Append(" UNION SELECT TOP 20 ProductId,ProductName,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,SalePrice FROM vw_Hishop_BrowseProductList");
			stringBuilder.AppendFormat(" WHERE SaleStatus = {0} AND ProductId<>{1}  AND CategoryId = (SELECT CategoryId FROM Hishop_Products WHERE ProductId={1} AND SaleStatus = {0})", 1, productId);
			stringBuilder.AppendFormat(" AND ProductId NOT IN (SELECT RelatedProductId FROM Hishop_RelatedProducts WHERE ProductId = {0})", productId);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					productBrowseInfo.Product = DataMapper.PopulateProduct(dataReader);
					if (dataReader["BrandName"] != DBNull.Value)
					{
						productBrowseInfo.BrandName = (string)dataReader["BrandName"];
					}
				}
				if (dataReader.NextResult())
				{
					while (dataReader.Read())
					{
						productBrowseInfo.Product.Skus.Add((string)dataReader["SkuId"], DataMapper.PopulateSKU(dataReader));
					}
				}
				if (maxReviewNum.HasValue && dataReader.NextResult())
				{
					productBrowseInfo.DBReviews = DataHelper.ConverDataReaderToDataTable(dataReader);
				}
				if (maxConsultationNum.HasValue && dataReader.NextResult())
				{
					productBrowseInfo.DBConsultations = DataHelper.ConverDataReaderToDataTable(dataReader);
				}
				if (dataReader.NextResult())
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
						productBrowseInfo.DbAttribute = dataTable2;
					}
				}
				if (dataReader.NextResult())
				{
					productBrowseInfo.DbSKUs = DataHelper.ConverDataReaderToDataTable(dataReader);
				}
				if (dataReader.NextResult())
				{
					productBrowseInfo.DbCorrelatives = DataHelper.ConverDataReaderToDataTable(dataReader);
				}
			}
			return productBrowseInfo;
		}
		public override DbQueryResult GetBrowseProductList(ProductBrowseQuery query)
		{
			string filter = ProductMasterProvider.BuildProductBrowseQuerySearch(query);
			string text = "ProductId,ProductName,ProductCode, ShowSaleCounts AS SaleCounts, ShortDescription, ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice, SalePrice,Stock";
			if (HiContext.Current.User.UserRole == UserRole.Member)
			{
                Hidistro.Membership.Context.Member member = HiContext.Current.User as Hidistro.Membership.Context.Member;
				int memberDiscount = MemberProvider.Instance().GetMemberDiscount(member.GradeId);
				text += string.Format(",CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) = 1 ", member.GradeId);
				text += string.Format("THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END AS RankPrice", member.GradeId, memberDiscount);
			}
			else
			{
				text += ",SalePrice as RankPrice";
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BrowseProductList p", "ProductId", filter, text);
		}
		public override DbQueryResult GetUnSaleProductList(ProductBrowseQuery query)
		{
			string filter = ProductMasterProvider.BuildUnSaleProductBrowseQuerySearch(query);
			string text = "ProductId,ProductName,ProductCode,ShowSaleCounts AS SaleCounts, ShortDescription,MarketPrice, ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,SalePrice,Stock";
			if (HiContext.Current.User.UserRole == UserRole.Member)
			{
                Hidistro.Membership.Context.Member member = HiContext.Current.User as Hidistro.Membership.Context.Member;
				int memberDiscount = MemberProvider.Instance().GetMemberDiscount(member.GradeId);
				text += string.Format(",CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) = 1 ", member.GradeId);
				text += string.Format("THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END AS RankPrice", member.GradeId, memberDiscount);
			}
			else
			{
				text += ",SalePrice as RankPrice";
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BrowseProductList p", "ProductId", filter, text);
		}
		public override System.Data.DataTable GetVistiedProducts(IList<int> productIds)
		{
			System.Data.DataTable result;
			if (productIds.Count == 0)
			{
				result = null;
			}
			else
			{
				string text = string.Empty;
				for (int i = 0; i < productIds.Count; i++)
				{
					text = text + productIds[i] + ",";
				}
				text = text.Remove(text.Length - 1);
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT ProductId,ProductName,ProductCode,ShowSaleCounts AS SaleCounts, ShortDescription,ThumbnailUrl40, ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,MarketPrice,SalePrice  FROM  vw_Hishop_BrowseProductList WHERE ProductId IN({0})", text));
				System.Data.DataTable dataTable;
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
				}
				result = dataTable;
			}
			return result;
		}
		public override DbQueryResult GetProductReviews(Pagination page, int productId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("ProductId = {0}", productId);
			return DataHelper.PagingByTopsort(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Hishop_ProductReviews", "reviewId", stringBuilder.ToString(), "*");
		}
		public override System.Data.DataTable GetProductReviews(int maxNum)
		{
			System.Data.DataTable result = new System.Data.DataTable();
			string text = "select top " + maxNum + " review.*,products.ProductName,products.ThumBnailUrl40,products.ThumBnailUrl60,products.ThumBnailUrl100,products.ThumBnailUrl160,products.ThumBnailUrl180,products.ThumBnailUrl220 from Hishop_ProductReviews as review inner join Hishop_Products as products on review.ProductId=products.ProductId order by review.ReviewDate desc";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override bool InsertProductReview(ProductReviewInfo review)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_ProductReviews (ProductId, UserId, ReviewText, UserName, UserEmail, ReviewDate) VALUES(@ProductId, @UserId, @ReviewText, @UserName, @UserEmail, @ReviewDate)");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, review.ProductId);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "ReviewText", System.Data.DbType.String, review.ReviewText);
			this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, review.UserName);
			this.database.AddInParameter(sqlStringCommand, "UserEmail", System.Data.DbType.String, review.UserEmail);
			this.database.AddInParameter(sqlStringCommand, "ReviewDate", System.Data.DbType.DateTime, DateTime.Now);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool ProductExists(int productId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select count(ProductId) from dbo.Hishop_Products where ProductId=@ProductId ");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
		}
		public override int GetProductReviewNumber(int productId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_ProductReviews WHERE ProductId=@ProductId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			return (int)this.database.ExecuteScalar(sqlStringCommand);
		}
		public override void LoadProductReview(int productId, out int buyNum, out int reviewNum)
		{
			buyNum = 0;
			reviewNum = 0;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_ProductReviews WHERE ProductId=@ProductId AND UserId = @UserId SELECT ISNULL(SUM(Quantity), 0) FROM Hishop_OrderItems WHERE ProductId=@ProductId AND OrderId IN" + string.Format(" (SELECT OrderId FROM Hishop_Orders WHERE UserId = @UserId AND OrderStatus != {0} AND OrderStatus != {1})", 1, 4));
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					reviewNum = (int)dataReader[0];
				}
				dataReader.NextResult();
				if (dataReader.Read())
				{
					buyNum = (int)dataReader[0];
				}
			}
		}
		public override DbQueryResult GetProductConsultations(Pagination page, int productId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("ProductId = {0}", productId);
			stringBuilder.Append(" AND ReplyUserId IS NOT NULL ");
			return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Hishop_ProductConsultations", "ConsultationId", stringBuilder.ToString(), "*");
		}
		public override bool InsertProductConsultation(ProductConsultationInfo productConsultation)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_ProductConsultations(ProductId, UserId, UserName, UserEmail, ConsultationText, ConsultationDate)VALUES(@ProductId, @UserId, @UserName, @UserEmail, @ConsultationText, @ConsultationDate)");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productConsultation.ProductId);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.String, productConsultation.UserId);
			this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, productConsultation.UserName);
			this.database.AddInParameter(sqlStringCommand, "UserEmail", System.Data.DbType.String, productConsultation.UserEmail);
			this.database.AddInParameter(sqlStringCommand, "ConsultationText", System.Data.DbType.String, productConsultation.ConsultationText);
			this.database.AddInParameter(sqlStringCommand, "ConsultationDate", System.Data.DbType.DateTime, productConsultation.ConsultationDate);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override int GetProductConsultationNumber(int productId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_ProductConsultations WHERE ProductId=@ProductId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			return (int)this.database.ExecuteScalar(sqlStringCommand);
		}
		public override System.Data.DataSet GetGroupByProductList(ProductBrowseQuery query, out int TotalGroupBuyProducts)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_GroupBuyProducts_Get");
			string text = string.Format("SELECT GroupBuyId,ProductId,StartDate,EndDate FROM Hishop_GroupBuy WHERE datediff(hh,EndDate,getdate())<=0 AND Status={0}", 1);
			text += string.Format(" AND ProductId IN(SELECT ProductId FROM Hishop_Products WHERE SaleStatus={0}) ORDER BY DisplaySequence DESC", 1);
			this.database.AddInParameter(storedProcCommand, "PageIndex", System.Data.DbType.Int32, query.PageIndex);
			this.database.AddInParameter(storedProcCommand, "PageSize", System.Data.DbType.Int32, query.PageSize);
			this.database.AddInParameter(storedProcCommand, "IsCount", System.Data.DbType.Boolean, query.IsCount);
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", System.Data.DbType.String, text);
			this.database.AddOutParameter(storedProcCommand, "TotalGroupBuyProducts", System.Data.DbType.Int32, 4);
			System.Data.DataSet result = this.database.ExecuteDataSet(storedProcCommand);
			TotalGroupBuyProducts = (int)this.database.GetParameterValue(storedProcCommand, "TotalGroupBuyProducts");
			return result;
		}
		public override System.Data.DataTable GetGroupByProductList(int maxnum)
		{
			System.Data.DataTable result = new System.Data.DataTable();
			string text = string.Format("SELECT top " + maxnum + "  S.GroupBuyId,S.StartDate,S.EndDate,P.ProductName,p.MarketPrice, P.SalePrice as OldPrice,ThumbnailUrl60,ThumbnailUrl100, ThumbnailUrl160,ThumbnailUrl180, ThumbnailUrl220,ThumbnailUrl310, P.ProductId,G.[Count],G.Price from vw_Hishop_BrowseProductList as P inner join Hishop_GroupBuy as S on P.ProductId=s.ProductId inner join  Hishop_GroupBuyCondition as G on G.GroupBuyId=S.GroupBuyId where datediff(hh,S.EndDate,getdate())<=0 and P.SaleStatus={0} order by S.DisplaySequence desc", 1);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override GroupBuyInfo GetProductGroupBuyInfo(int productId)
		{
			GroupBuyInfo groupBuyInfo = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_GroupBuy WHERE ProductId=@ProductId AND Status = @Status; SELECT * FROM Hishop_GroupBuyCondition WHERE GroupBuyId=(SELECT GroupBuyId FROM Hishop_GroupBuy WHERE ProductId=@ProductId AND Status = @Status)");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			this.database.AddInParameter(sqlStringCommand, "Status", System.Data.DbType.Int32, 1);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					groupBuyInfo = DataMapper.PopulateGroupBuy(dataReader);
				}
				dataReader.NextResult();
				while (dataReader.Read())
				{
					GropBuyConditionInfo gropBuyConditionInfo = new GropBuyConditionInfo();
					gropBuyConditionInfo.Count = (int)dataReader["Count"];
					gropBuyConditionInfo.Price = (decimal)dataReader["Price"];
					if (groupBuyInfo != null)
					{
						groupBuyInfo.GroupBuyConditions.Add(gropBuyConditionInfo);
					}
				}
			}
			return groupBuyInfo;
		}
		public override int GetOrderCount(int groupBuyId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SUM(Quantity) FROM Hishop_OrderItems WHERE OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE GroupBuyId = @GroupBuyId AND OrderStatus <> 1 AND OrderStatus <> 4)");
			this.database.AddInParameter(sqlStringCommand, "GroupBuyId", System.Data.DbType.Int32, groupBuyId);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int result;
			if (obj != null && obj != DBNull.Value)
			{
				result = (int)obj;
			}
			else
			{
				result = 0;
			}
			return result;
		}
		public override decimal GetCurrentPrice(int groupBuyId, int prodcutQuantity)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @price money;SELECT @price = MIN(price) FROM Hishop_GroupBuyCondition WHERE GroupBuyId=@GroupBuyId AND Count<=@prodcutQuantity;if @price IS NULL SELECT @price = max(price) FROM Hishop_GroupBuyCondition WHERE GroupBuyId=@GroupBuyId ;select @price");
			this.database.AddInParameter(sqlStringCommand, "GroupBuyId", System.Data.DbType.Int32, groupBuyId);
			this.database.AddInParameter(sqlStringCommand, "prodcutQuantity", System.Data.DbType.Int32, prodcutQuantity);
			return (decimal)this.database.ExecuteScalar(sqlStringCommand);
		}
		public override DbQueryResult GetCountDownProductList(ProductBrowseQuery query)
		{
			string filter = string.Format(" datediff(hh,EndDate,getdate())<0 AND ProductId IN(SELECT ProductId FROM Hishop_Products WHERE SaleStatus={0})", 1);
			return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_CountDown", "CountDownId", filter, "*");
		}
		public override System.Data.DataTable GetCounDownProducList(int maxnum)
		{
			System.Data.DataTable result = new System.Data.DataTable();
			string text = string.Format("select top " + maxnum + " CountDownId,ProductId,ProductName,SalePrice,CountDownPrice,StartDate,EndDate, ThumbnailUrl60,ThumbnailUrl100, ThumbnailUrl160,ThumbnailUrl180, ThumbnailUrl220,ThumbnailUrl310 from vw_Hishop_CountDown where datediff(hh,EndDate,getdate())<=0 AND ProductId IN(SELECT ProductId FROM Hishop_Products WHERE SaleStatus={0}) order by DisplaySequence desc", 1);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override CountDownInfo GetCountDownInfoByCountDownId(int countDownId)
		{
			CountDownInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_CountDown WHERE datediff(hh,EndDate,getdate())<=0 AND CountDownId=@CountDownId");
			this.database.AddInParameter(sqlStringCommand, "CountDownId", System.Data.DbType.Int32, countDownId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateCountDown(dataReader);
				}
			}
			return result;
		}
		public override CountDownInfo GetCountDownInfo(int productId)
		{
			CountDownInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_CountDown WHERE datediff(hh,EndDate,getdate())<=0 AND ProductId=@ProductId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateCountDown(dataReader);
				}
			}
			return result;
		}
		public override DbQueryResult GetOnlineGifts(Pagination page)
		{
			string selectFields = "GiftId,Name,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180, ThumbnailUrl220, MarketPrice,NeedPoint";
			return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Hishop_Gifts", "GiftId", "NeedPoint > 0", selectFields);
		}
		public override GiftInfo GetGift(int giftId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Gifts WHERE GiftId = @GiftId");
			this.database.AddInParameter(sqlStringCommand, "GiftId", System.Data.DbType.Int32, giftId);
			GiftInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateGift(dataReader);
				}
			}
			return result;
		}
		public override IList<GiftInfo> GetGifts(int maxnum)
		{
			List<GiftInfo> list = new List<GiftInfo>();
			string text = "SELECT TOP " + maxnum + " * FROM Hishop_Gifts";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulateGift(dataReader));
				}
			}
			return list;
		}
		public override IList<GiftInfo> GetOnlinePromotionGifts()
		{
			List<GiftInfo> list = new List<GiftInfo>();
			string text = "SELECT * FROM Hishop_Gifts WHERE IsPromotion=1";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulateGift(dataReader));
				}
			}
			return list;
		}
		public override DbQueryResult GetBundlingProductList(BundlingInfoQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" salestatus=1 ");
			if (!string.IsNullOrEmpty(query.ProductName))
			{
				stringBuilder.AppendFormat(" AND Name Like '%{0}%'", DataHelper.CleanSearchString(query.ProductName));
			}
			string selectFields = "Bundlingid,Name,Num,price,SaleStatus,AddTime,ShortDescription,DisplaySequence";
			return DataHelper.PagingByTopnotin(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_BundlingProducts", "Bundlingid", stringBuilder.ToString(), selectFields);
		}
		public override BundlingInfo GetBundlingInfo(int bundlingID)
		{
			BundlingInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_BundlingProducts WHERE BundlingID=@BundlingID");
			this.database.AddInParameter(sqlStringCommand, "BundlingID", System.Data.DbType.Int32, bundlingID);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateBindInfo(dataReader);
				}
			}
			return result;
		}
		public override List<BundlingItemInfo> GetBundlingItemsByID(int bundlingID)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT [BundlingID] ,a.[ProductId] ,[SkuId] ,[ProductNum], productName, ");
			stringBuilder.Append(" (select saleprice FROM  Hishop_SKUs c where c.SkuId= a.SkuId ) as ProductPrice");
			stringBuilder.Append(" FROM  Hishop_BundlingProductItems a JOIN Hishop_Products p ON a.ProductID = p.ProductId where BundlingID=@BundlingID AND p.SaleStatus = 1");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "BundlingID", System.Data.DbType.Int32, bundlingID);
			List<BundlingItemInfo> list = new List<BundlingItemInfo>();
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(new BundlingItemInfo
					{
						ProductID = (int)dataReader["ProductID"],
						ProductNum = (int)dataReader["ProductNum"],
						SkuId = (string)dataReader["SkuId"],
						ProductName = (string)dataReader["ProductName"],
						ProductPrice = (decimal)dataReader["ProductPrice"],
						BundlingID = bundlingID
					});
				}
			}
			return list;
		}
		public override int GetLineItemNumber(int productId)
		{
			string text = string.Format("select count(*) from dbo.Hishop_OrderItems as items left join Hishop_Orders orders on items.OrderId=orders.OrderId where orders.OrderStatus!={0} and orders.OrderStatus!={1} and items.ProductId=@ProductId", 1, 4);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			return (int)this.database.ExecuteScalar(sqlStringCommand);
		}
		public override DbQueryResult GetLineItems(Pagination page, int productId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("ProductId = {0} ", productId);
			return DataHelper.PagingByTopsort(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "vw_Hishop_OrderItem", "OrderId", stringBuilder.ToString(), "*");
		}
		public override System.Data.DataTable GetLineItems(int productId, int maxNum)
		{
			System.Data.DataTable result = new System.Data.DataTable();
			string text = string.Format("select top " + maxNum + " items.*,orders.PayDate,orders.Username,orders.ShipTo from dbo.Hishop_OrderItems as items left join Hishop_Orders orders on items.OrderId=orders.OrderId where orders.OrderStatus!={0} and orders.OrderStatus!={1} and items.ProductId=@ProductId  order by orders.PayDate desc", 1, 4);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override bool IsBuyProduct(int productId)
		{
			bool result = false;
			try
			{
				string text = "select top 1 orders.UserId from Hishop_OrderItems as items left join Hishop_Orders orders on items.OrderId=orders.OrderId where ProductId=@ProductId and orders.UserId=@UserId and orders.OrderStatus=@OrderStatus";
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
				this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
				this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
				this.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, 5);
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					if (dataReader.Read())
					{
						result = true;
					}
				}
			}
			catch (Exception)
			{
				result = true;
			}
			return result;
		}
	}
}
