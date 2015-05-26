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
namespace Hidistro.SaleSystem.DistributionData
{
	public class ProductData : ProductSubsiteProvider
	{
		private Database database;
		public ProductData()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public override System.Data.DataTable GetSaleProductRanking(int? categoryId, int maxNum)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT TOP {0} ProductId, ProductName, ProductCode, ShowSaleCounts AS SaleCounts, ThumbnailUrl40, ThumbnailUrl60, ThumbnailUrl100, ThumbnailUrl160,", maxNum);
			stringBuilder.AppendFormat(" ThumbnailUrl180, ThumbnailUrl220, SalePrice, MarketPrice FROM vw_distro_BrowseProductList WHERE SaleStatus = {0} and DistributorUserId = {1}", 1, HiContext.Current.SiteSettings.UserId.Value);
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
			int value = HiContext.Current.SiteSettings.UserId.Value;
			if (HiContext.Current.User.UserRole == UserRole.Underling)
			{
                Hidistro.Membership.Context.Member member = HiContext.Current.User as Hidistro.Membership.Context.Member;
				int memberDiscount = MemberProvider.Instance().GetMemberDiscount(member.GradeId);
				stringBuilder.AppendFormat("SELECT TOP {0} ProductId,ProductName,ProductCode,ShowSaleCounts AS SaleCounts,ShortDescription,", query.MaxNum);
				stringBuilder.Append(" ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,");
				stringBuilder.AppendFormat(" (SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = p.SkuId AND DistributoruserId = {0}) AS SalePrice,", value);
				stringBuilder.AppendFormat(" CASE WHEN (SELECT COUNT(*) FROM distro_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0} AND DistributoruserId = {1}) = 1 ", member.GradeId, value);
				stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM distro_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0} AND DistributoruserId = {1})", member.GradeId, value);
				stringBuilder.AppendFormat(" ELSE (SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = p.SkuId AND DistributoruserId = {0})*{1}/100 END AS RankPrice", value, memberDiscount);
			}
			else
			{
				stringBuilder.AppendFormat("SELECT TOP {0} ProductId,ProductName,ProductCode,ShowSaleCounts AS SaleCounts,ShortDescription,", query.MaxNum);
				stringBuilder.Append(" ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220, ThumbnailUrl310, MarketPrice");
				stringBuilder.AppendFormat(" ,(SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = p.SkuId AND DistributoruserId = {0}) AS SalePrice", value);
				stringBuilder.AppendFormat(" ,(SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = p.SkuId AND DistributoruserId = {0}) AS RankPrice", value);
			}
			stringBuilder.Append(" FROM vw_distro_BrowseProductList p WHERE ");
			stringBuilder.Append(ProductSubsiteProvider.BuildProductSubjectQuerySearch(query));
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
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" SELECT dp.*, p.TaobaoProductId, p.Unit, p.ImageUrl1, p.ImageUrl2, p.ImageUrl3, p.ImageUrl4, p.ImageUrl5,p.LowestSalePrice, p.PenetrationStatus");
			stringBuilder.Append(",CASE WHEN dp.BrandId IS NULL THEN NULL ELSE (SELECT bc.BrandName FROM Hishop_BrandCategories bc WHERE bc.BrandId=dp.BrandId) END AS BrandName");
			stringBuilder.Append(" FROM distro_Products dp JOIN Hishop_Products p ON dp.ProductId = p.ProductId  where dp.ProductId=@ProductId AND dp.DistributorUserId = @DistributorUserId;");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
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
			int value = HiContext.Current.SiteSettings.UserId.Value;
			if (HiContext.Current.User.UserRole == UserRole.Underling)
			{
                Hidistro.Membership.Context.Member member = HiContext.Current.User as Hidistro.Membership.Context.Member;
				int memberDiscount = MemberProvider.Instance().GetMemberDiscount(member.GradeId);
				int arg_59_0 = member.GradeId;
			}
			ProductBrowseInfo productBrowseInfo = new ProductBrowseInfo();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE distro_Products SET VistiCounts = VistiCounts + 1 WHERE ProductId = @ProductId AND DistributorUserId = @DistributorUserId;");
			stringBuilder.Append(" SELECT dp.*, p.Unit, p.ImageUrl1, p.ImageUrl2, p.ImageUrl3, p.ImageUrl4, p.ImageUrl5, p.LowestSalePrice, p.PenetrationStatus, p.TaobaoProductId");
			stringBuilder.Append(",CASE WHEN dp.BrandId IS NULL THEN NULL ELSE (SELECT bc.BrandName FROM Hishop_BrandCategories bc WHERE bc.BrandId=dp.BrandId) END AS BrandName");
			stringBuilder.Append(" FROM distro_Products dp JOIN Hishop_Products p ON dp.ProductId = p.ProductId  where dp.ProductId=@ProductId AND dp.DistributorUserId = @DistributorUserId;");
			if (HiContext.Current.User.UserRole == UserRole.Underling)
			{
                Hidistro.Membership.Context.Member member = HiContext.Current.User as Hidistro.Membership.Context.Member;
				int memberDiscount = MemberProvider.Instance().GetMemberDiscount(member.GradeId);
				stringBuilder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock, AlertStock, CostPrice, PurchasePrice,");
				stringBuilder.AppendFormat(" CASE WHEN (SELECT COUNT(*) FROM distro_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0} AND DistributoruserId = {1}) = 1", member.GradeId, value);
				stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM distro_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0} AND DistributoruserId = {1})", member.GradeId, value);
				stringBuilder.AppendFormat(" ELSE (SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = s.SkuId AND DistributoruserId = {0})*{1}/100 END AS SalePrice", value, memberDiscount);
			}
			else
			{
				stringBuilder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock, AlertStock, CostPrice, PurchasePrice,");
				stringBuilder.AppendFormat(" (SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = s.SkuId AND DistributoruserId = {0}) AS SalePrice", value);
			}
			stringBuilder.Append(" FROM Hishop_SKUs s WHERE ProductId = @ProductId");
			if (maxReviewNum.HasValue)
			{
				stringBuilder.AppendFormat(" SELECT TOP {0} * FROM distro_ProductReviews where ProductId=@ProductId AND DistributorUserId=@DistributorUserId ORDER BY ReviewId DESC;", maxReviewNum);
			}
			if (maxConsultationNum.HasValue)
			{
				stringBuilder.AppendFormat(" SELECT TOP {0} * FROM distro_ProductConsultations where ProductId=@ProductId AND DistributorUserId=@DistributorUserId AND ReplyUserId IS NOT NULL ORDER BY ConsultationId DESC ;", maxConsultationNum);
			}
			stringBuilder.Append(" SELECT a.AttributeId, AttributeName, ValueStr FROM Hishop_ProductAttributes pa JOIN Hishop_Attributes a ON pa.AttributeId = a.AttributeId");
			stringBuilder.Append(" JOIN Hishop_AttributeValues v ON a.AttributeId = v.AttributeId AND pa.ValueId = v.ValueId  WHERE ProductId = @ProductId ORDER BY a.DisplaySequence DESC, v.DisplaySequence DESC");
			stringBuilder.Append(" SELECT SkuId, a.AttributeId, AttributeName, UseAttributeImage, av.ValueId, ValueStr, ImageUrl FROM Hishop_SKUItems s join Hishop_Attributes a on s.AttributeId = a.AttributeId join Hishop_AttributeValues av on s.ValueId = av.ValueId WHERE SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId = @ProductId) ORDER BY a.DisplaySequence DESC,av.DisplaySequence DESC;");
			stringBuilder.AppendFormat(" SELECT TOP 20 ProductId,ProductName,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,SalePrice FROM vw_distro_BrowseProductList WHERE SaleStatus = {0}", 1);
			stringBuilder.AppendFormat(" AND DistributorUserId = {0}  AND ProductId IN (SELECT RelatedProductId FROM distro_RelatedProducts WHERE ProductId = {1} AND DistributorUserId = {0})", value, productId);
			stringBuilder.AppendFormat(" UNION SELECT TOP 20 ProductId,ProductName,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,MarketPrice,SalePrice FROM vw_distro_BrowseProductList WHERE SaleStatus = {0}", 1);
			stringBuilder.AppendFormat(" AND DistributorUserId = {0} AND ProductId<>{1} AND CategoryId = (SELECT CategoryId FROM distro_Products WHERE ProductId={1} AND SaleStatus = {2} AND DistributorUserId = {0})", value, productId, 1);
			stringBuilder.AppendFormat(" AND ProductId NOT IN (SELECT RelatedProductId FROM distro_RelatedProducts WHERE ProductId = {0} AND DistributorUserId = {1})", productId, value);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, value);
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
				if (dataReader.NextResult() && productBrowseInfo.Product != null)
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
				if (dataReader.NextResult() && productBrowseInfo.Product != null)
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
			int value = HiContext.Current.SiteSettings.UserId.Value;
			string text = ProductSubsiteProvider.BuildProductBrowseQuerySearch(query);
			text += string.Format(" AND DistributorUserId = {0} ", HiContext.Current.SiteSettings.UserId.Value);
			string text2 = "ProductId,ProductName,ProductCode,ShowSaleCounts AS SaleCounts,ShortDescription,MarketPrice, ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,Stock";
			if (HiContext.Current.User.UserRole == UserRole.Underling)
			{
                Hidistro.Membership.Context.Member member = HiContext.Current.User as Hidistro.Membership.Context.Member;
				int memberDiscount = MemberProvider.Instance().GetMemberDiscount(member.GradeId);
				text2 += string.Format(",(SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = p.SkuId AND DistributoruserId = {0}) AS SalePrice", value);
				text2 += string.Format(",CASE WHEN (SELECT COUNT(*) FROM distro_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0} AND DistributoruserId = {1}) = 1 ", member.GradeId, value);
				text2 += string.Format(" THEN (SELECT MemberSalePrice FROM distro_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0} AND DistributoruserId = {1})", member.GradeId, value);
				text2 += string.Format(" ELSE (SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = p.SkuId AND DistributoruserId = {0})*{1}/100 END AS RankPrice", value, memberDiscount);
			}
			else
			{
				text2 += string.Format(",(SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = p.SkuId AND DistributoruserId = {0}) AS SalePrice", value);
				text2 += string.Format(",(SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = p.SkuId AND DistributoruserId = {0}) AS RankPrice", value);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_distro_BrowseProductList p", "ProductId", text, text2);
		}
		public override DbQueryResult GetUnSaleProductList(ProductBrowseQuery query)
		{
			int value = HiContext.Current.SiteSettings.UserId.Value;
			string text = ProductSubsiteProvider.BuildUnSaleProductBrowseQuerySearch(query);
			text += string.Format(" AND DistributorUserId = {0} ", HiContext.Current.SiteSettings.UserId.Value);
			string text2 = "ProductId,ProductName,ProductCode,ShowSaleCounts AS SaleCounts, ShortDescription,MarketPrice, ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220,ThumbnailUrl310,Stock";
			if (HiContext.Current.User.UserRole == UserRole.Underling)
			{
                Hidistro.Membership.Context.Member member = HiContext.Current.User as Hidistro.Membership.Context.Member;
				int memberDiscount = MemberProvider.Instance().GetMemberDiscount(member.GradeId);
				text2 += string.Format(",(SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = p.SkuId AND DistributoruserId = {0}) AS SalePrice", value);
				text2 += string.Format(",CASE WHEN (SELECT COUNT(*) FROM distro_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0} AND DistributoruserId = {1}) = 1 ", member.GradeId, value);
				text2 += string.Format(" THEN (SELECT MemberSalePrice FROM distro_SKUMemberPrice WHERE SkuId = p.SkuId AND GradeId = {0} AND DistributoruserId = {1})", member.GradeId, value);
				text2 += string.Format(" ELSE (SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = p.SkuId AND DistributoruserId = {0})*{1}/100 END AS RankPrice", value, memberDiscount);
			}
			else
			{
				text2 += string.Format(",(SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = p.SkuId AND DistributoruserId = {0}) AS SalePrice", value);
				text2 += string.Format(",(SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = p.SkuId AND DistributoruserId = {0}) AS RankPrice", value);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_distro_BrowseProductList p", "ProductId", text, text2);
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
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT ProductId,ProductName,ProductCode,ShowSaleCounts AS SaleCounts, ShortDescription,ThumbnailUrl40, ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180,MarketPrice,SalePrice FROM  vw_distro_BrowseProductList WHERE DistributorUserId = {0} AND ProductId IN({1})", HiContext.Current.SiteSettings.UserId.Value, text));
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
			stringBuilder.AppendFormat(" AND DistributorUserId={0}", HiContext.Current.SiteSettings.UserId.Value);
			return DataHelper.PagingByTopsort(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "distro_ProductReviews", "reviewId", stringBuilder.ToString(), "*");
		}
		public override System.Data.DataTable GetProductReviews(int maxNum)
		{
			System.Data.DataTable result = new System.Data.DataTable();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("select top " + maxNum + " review.*,products.ProductName,products.ThumBnailUrl40,products.ThumBnailUrl60,products.ThumBnailUrl100,products.ThumBnailUrl160,products.ThumBnailUrl180,products.ThumBnailUrl220 from distro_ProductReviews as review inner join distro_Products as products on review.ProductId=products.ProductId where review.DistributorUserId={0} order by review.ReviewDate desc", HiContext.Current.SiteSettings.UserId.Value));
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override bool InsertProductReview(ProductReviewInfo review)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO distro_ProductReviews (ProductId, UserId,DistributorUserId, ReviewText, UserName, UserEmail, ReviewDate) VALUES(@ProductId, @UserId,@DistributorUserId, @ReviewText, @UserName, @UserEmail, @ReviewDate)");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, review.ProductId);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM distro_ProductReviews WHERE ProductId=@ProductId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			return (int)this.database.ExecuteScalar(sqlStringCommand);
		}
		public override void LoadProductReview(int productId, out int buyNum, out int reviewNum)
		{
			buyNum = 0;
			reviewNum = 0;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM distro_ProductReviews WHERE ProductId=@ProductId AND UserId = @UserId AND DistributorUserId=@DistributorUserId SELECT ISNULL(SUM(Quantity), 0) FROM distro_OrderItems WHERE ProductId=@ProductId AND DistributorUserId=@DistributorUserId AND OrderId IN" + string.Format(" (SELECT OrderId FROM distro_Orders WHERE UserId = @UserId AND DistributorUserId=@DistributorUserId AND OrderStatus != {0} AND OrderStatus != {1})", 1, 4));
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
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
			stringBuilder.AppendFormat(" AND DistributorUserId={0}", HiContext.Current.SiteSettings.UserId.Value);
			stringBuilder.Append(" AND ReplyUserId IS NOT NULL ");
			return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "distro_ProductConsultations", "ConsultationId", stringBuilder.ToString(), "*");
		}
		public override bool InsertProductConsultation(ProductConsultationInfo productConsultation)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO distro_ProductConsultations(ProductId, UserId,DistributorUserId, UserName, UserEmail, ConsultationText, ConsultationDate)VALUES(@ProductId, @UserId,@DistributorUserId, @UserName, @UserEmail, @ConsultationText, @ConsultationDate)");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productConsultation.ProductId);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.String, productConsultation.UserId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
			this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, productConsultation.UserName);
			this.database.AddInParameter(sqlStringCommand, "UserEmail", System.Data.DbType.String, productConsultation.UserEmail);
			this.database.AddInParameter(sqlStringCommand, "ConsultationText", System.Data.DbType.String, productConsultation.ConsultationText);
			this.database.AddInParameter(sqlStringCommand, "ConsultationDate", System.Data.DbType.DateTime, productConsultation.ConsultationDate);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override int GetProductConsultationNumber(int productId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM distro_ProductConsultations WHERE ProductId=@ProductId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
			return (int)this.database.ExecuteScalar(sqlStringCommand);
		}
		public override System.Data.DataSet GetGroupByProductList(ProductBrowseQuery query, out int count)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_distro_GroupBuyProducts_Get");
			string text = string.Format("SELECT GroupBuyId,ProductId,StartDate,EndDate FROM distro_GroupBuy WHERE datediff(hh,EndDate,getdate())<=0 AND Status={0} AND DistributorUserId={1} AND ProductId IN(SELECT ProductId FROM distro_Products WHERE SaleStatus=1 AND DistributorUserId={2}) ORDER BY DisplaySequence DESC", 1, HiContext.Current.SiteSettings.UserId.Value, HiContext.Current.SiteSettings.UserId.Value);
			this.database.AddInParameter(storedProcCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
			this.database.AddInParameter(storedProcCommand, "PageIndex", System.Data.DbType.Int32, query.PageIndex);
			this.database.AddInParameter(storedProcCommand, "PageSize", System.Data.DbType.Int32, query.PageSize);
			this.database.AddInParameter(storedProcCommand, "IsCount", System.Data.DbType.Boolean, query.IsCount);
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", System.Data.DbType.String, text);
			this.database.AddOutParameter(storedProcCommand, "TotalGroupBuyProducts", System.Data.DbType.Int32, 4);
			System.Data.DataSet result = this.database.ExecuteDataSet(storedProcCommand);
			count = (int)this.database.GetParameterValue(storedProcCommand, "TotalGroupBuyProducts");
			return result;
		}
		public override System.Data.DataTable GetGroupByProductList(int maxnum)
		{
			System.Data.DataTable result = new System.Data.DataTable();
			string text = string.Format("SELECT top " + maxnum + "  S.GroupBuyId,S.StartDate,S.EndDate,P.ProductName,p.MarketPrice, P.SalePrice as OldPrice,ThumbnailUrl60,ThumbnailUrl100, ThumbnailUrl160,ThumbnailUrl180, ThumbnailUrl220,ThumbnailUrl310, P.ProductId,G.[Count],G.Price from vw_distro_BrowseProductList as P inner join distro_GroupBuy as S on P.ProductId=s.ProductId inner join  distro_GroupBuyCondition as G on G.GroupBuyId=S.GroupBuyId where datediff(hh,S.EndDate,getdate())<=0 and  S.DistributorUserId={0} and P.SaleStatus={1} order by S.DisplaySequence desc", HiContext.Current.SiteSettings.UserId.Value, 1);
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_GroupBuy WHERE ProductId=@ProductId AND DistributorUserId=@DistributorUserId AND Status = @Status; SELECT * FROM distro_GroupBuyCondition WHERE GroupBuyId=(SELECT GroupBuyId FROM distro_GroupBuy WHERE ProductId=@ProductId AND DistributorUserId=@DistributorUserId AND Status=@Status)");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SUM(Quantity) FROM distro_OrderItems WHERE OrderId IN (SELECT OrderId FROM distro_Orders WHERE GroupBuyId = @GroupBuyId AND OrderStatus <> 1 AND OrderStatus <> 4 AND DistributorUserId=@DistributorUserId)");
			this.database.AddInParameter(sqlStringCommand, "GroupBuyId", System.Data.DbType.Int32, groupBuyId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @price money;SELECT @price = MIN(price) FROM distro_GroupBuyCondition WHERE GroupBuyId=@GroupBuyId AND Count<=@prodcutQuantity AND DistributorUserId=@DistributorUserId;if @price IS NULL SELECT @price = max(price) FROM distro_GroupBuyCondition WHERE GroupBuyId=@GroupBuyId AND DistributorUserId=@DistributorUserId;select @price");
			this.database.AddInParameter(sqlStringCommand, "GroupBuyId", System.Data.DbType.Int32, groupBuyId);
			this.database.AddInParameter(sqlStringCommand, "prodcutQuantity", System.Data.DbType.Int32, prodcutQuantity);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
			return (decimal)this.database.ExecuteScalar(sqlStringCommand);
		}
		public override DbQueryResult GetOnlineGifts(Pagination page)
		{
			string selectFields = "GiftId,d_Name as [Name],ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180, ThumbnailUrl220, MarketPrice,d_NeedPoint as NeedPoint";
			return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "vw_distro_Gifts", "GiftId", "d_NeedPoint > 0 and d_DistributorUserId=" + HiContext.Current.SiteSettings.UserId.Value, selectFields);
		}
		public override GiftInfo GetGift(int giftId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select d_Name as [Name],d_Title as Title,d_Meta_Description as Meta_Description,d_Meta_Keywords as Meta_Keywords,d_NeedPoint as NeedPoint,GiftId,ShortDescription,Unit, LongDescription,CostPrice,ImageUrl, ThumbnailUrl40,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160,ThumbnailUrl180, ThumbnailUrl220,ThumbnailUrl310,ThumbnailUrl410, PurchasePrice,MarketPrice,IsDownLoad,IsPromotion from vw_distro_Gifts where GiftId=@GiftId and d_DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "GiftId", System.Data.DbType.Int32, giftId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
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
			string text = "SELECT TOP " + maxnum + " *  FROM vw_distro_Gifts WHERE d_DistributorUserId=@DistributorUserId";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
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
			string text = "SELECT * FROM vw_distro_Gifts WHERE d_IsPromotion=1 AND d_DistributorUserId=@DistributorUserId";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulateGift(dataReader));
				}
			}
			return list;
		}
		public override DbQueryResult GetCountDownProductList(ProductBrowseQuery query)
		{
			string filter = string.Format(" datediff(hh,EndDate,getdate())<0 AND DistributorUserId={1} AND ProductId IN(SELECT ProductId FROM distro_Products WHERE SaleStatus=1 AND DistributorUserId={2})", DateTime.Now, HiContext.Current.SiteSettings.UserId.Value, HiContext.Current.SiteSettings.UserId.Value);
			return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_distro_CountDown", "CountDownId", filter, "*");
		}
		public override CountDownInfo GetCountDownInfoByCountDownId(int countDownId)
		{
			CountDownInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_CountDown WHERE datediff(hh,EndDate,getdate())<=0 AND CountDownId=@CountDownId AND DistributorUserId = @DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "CountDownId", System.Data.DbType.Int32, countDownId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateCountDown(dataReader);
				}
			}
			return result;
		}
		public override System.Data.DataTable GetCounDownProducList(int maxnum)
		{
			System.Data.DataTable result = new System.Data.DataTable();
			string text = string.Format("select top " + maxnum + " CountDownId,ProductId,ProductName,SalePrice,CountDownPrice,StartDate,EndDate, ThumbnailUrl60,ThumbnailUrl100, ThumbnailUrl160,ThumbnailUrl180, ThumbnailUrl220,ThumbnailUrl310 from vw_distro_CountDown where datediff(hh,EndDate,getdate())<=0 AND DistributorUserId={0} AND ProductId IN(SELECT ProductId FROM Hishop_Products WHERE SaleStatus={1}) order by DisplaySequence desc", HiContext.Current.SiteSettings.UserId.Value, 1);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override CountDownInfo GetCountDownInfo(int productId)
		{
			CountDownInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_CountDown WHERE datediff(hh,EndDate,getdate())<=0 AND ProductId=@ProductId AND DistributorUserId = @DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateCountDown(dataReader);
				}
			}
			return result;
		}
		public override DbQueryResult GetBundlingProductList(BundlingInfoQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" salestatus=1 ");
			stringBuilder.AppendFormat(" AND  DistributorUserId={0} ", HiContext.Current.SiteSettings.UserId.Value);
			string selectFields = "Bundlingid,Name,Num,price,SaleStatus,AddTime,ShortDescription,DisplaySequence";
			return DataHelper.PagingByTopnotin(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "distro_BundlingProducts", "Bundlingid", stringBuilder.ToString(), selectFields);
		}
		public override BundlingInfo GetBundlingInfo(int bundlingID)
		{
			BundlingInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_BundlingProducts WHERE BundlingID=@BundlingID");
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
			stringBuilder.AppendFormat(" FROM  distro_BundlingProductItems a JOIN distro_Products p ON a.ProductID = p.ProductId AND p.DistributorUserId = {0}", HiContext.Current.SiteSettings.UserId.Value);
			stringBuilder.Append(" where BundlingID=@BundlingID AND p.SaleStatus = 1");
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
						ProductName = (string)dataReader["productName"],
						ProductPrice = (decimal)dataReader["ProductPrice"],
						BundlingID = bundlingID
					});
				}
			}
			return list;
		}
		public override int GetLineItemNumber(int productId)
		{
			string text = string.Format("select count(*) from dbo.distro_OrderItems as items left join distro_Orders orders on items.OrderId=orders.OrderId where orders.OrderStatus!={0} and orders.OrderStatus!={1} and items.ProductId=@ProductId and items.DistributorUserId=@DistributorUserId", 1, 4);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
			return (int)this.database.ExecuteScalar(sqlStringCommand);
		}
		public override DbQueryResult GetLineItems(Pagination page, int productId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("ProductId = {0}", productId);
			stringBuilder.AppendFormat(" AND DistributorUserId={0}", HiContext.Current.SiteSettings.UserId.Value);
			return DataHelper.PagingByTopsort(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "vw_distro_OrderItem", "OrderId", stringBuilder.ToString(), "*");
		}
		public override System.Data.DataTable GetLineItems(int productId, int maxNum)
		{
			System.Data.DataTable result = new System.Data.DataTable();
			string text = string.Format("select top " + maxNum + " items.*,orders.PayDate,orders.Username,orders.ShipTo from dbo.distro_OrderItems as items left join distro_Orders orders on items.OrderId=orders.OrderId where orders.OrderStatus!={0} and orders.OrderStatus!={1} and items.ProductId=@ProductId  and items.DistributorUserId=@DistributorUserId order by orders.PayDate desc", 1, 4);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
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
				string text = "select top 1 orders.UserId from distro_OrderItems as items left join distro_Orders orders on items.OrderId=orders.OrderId where ProductId=@ProductId and orders.OrderStatus=@OrderStatus and  orders.DistributorUserId=@DistributorUserId and orders.UserId=@UserId";
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
				this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
				this.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, 5);
				this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
				this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
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
