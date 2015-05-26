using Hidistro.AccountCenter.Comments;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Commodities;
using Hidistro.Membership.Context;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
namespace Hidistro.AccountCenter.DistributionData
{
	public class CommentData : CommentSubsiteDataProvider
	{
		private Database database;
		public CommentData()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public override bool InsertMessage(MessageBoxInfo messageBoxInfo)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("BEGIN TRAN ");
			stringBuilder.Append("DECLARE @ContentId int ");
			stringBuilder.Append("DECLARE @errorSun INT ");
			stringBuilder.Append("SET @errorSun=0 ");
			stringBuilder.Append("INSERT INTO [Hishop_MessageContent]([Title],[Content],[Date]) ");
			stringBuilder.Append("VALUES(@Title,@Content,@Date) ");
			stringBuilder.Append("SET @ContentId = @@IDENTITY  ");
			stringBuilder.Append("SET @errorSun=@errorSun+@@ERROR  ");
			stringBuilder.Append("INSERT INTO [Hishop_MemberMessageBox]([ContentId],[Sernder],[Accepter],[IsRead]) ");
			stringBuilder.Append("VALUES(@ContentId,@Sernder ,@Accepter,@IsRead) ");
			stringBuilder.Append("SET @errorSun=@errorSun+@@ERROR  ");
			stringBuilder.Append("INSERT INTO [Hishop_DistributorMessageBox]([ContentId],[Sernder],[Accepter],[IsRead]) ");
			stringBuilder.Append("VALUES(@ContentId,@Sernder ,@Accepter,@IsRead) ");
			stringBuilder.Append("SET @errorSun=@errorSun+@@ERROR  ");
			stringBuilder.Append("IF @errorSun<>0 ");
			stringBuilder.Append("BEGIN ");
			stringBuilder.Append("ROLLBACK TRANSACTION  ");
			stringBuilder.Append("END ");
			stringBuilder.Append("ELSE ");
			stringBuilder.Append("BEGIN ");
			stringBuilder.Append("COMMIT TRANSACTION  ");
			stringBuilder.Append("END ");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "Title", System.Data.DbType.String, messageBoxInfo.Title);
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, messageBoxInfo.Content);
			this.database.AddInParameter(sqlStringCommand, "Date", System.Data.DbType.DateTime, DataHelper.GetSafeDateTimeFormat(DateTime.Now));
			this.database.AddInParameter(sqlStringCommand, "Sernder", System.Data.DbType.String, messageBoxInfo.Sernder);
			this.database.AddInParameter(sqlStringCommand, "Accepter", System.Data.DbType.String, messageBoxInfo.Accepter);
			this.database.AddInParameter(sqlStringCommand, "IsRead", System.Data.DbType.Boolean, messageBoxInfo.IsRead);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override int DeleteMemberMessages(IList<long> messageList)
		{
			string text = string.Empty;
			foreach (long current in messageList)
			{
				if (string.IsNullOrEmpty(text))
				{
					text += current.ToString(CultureInfo.InvariantCulture);
				}
				else
				{
					text = text + "," + current.ToString(CultureInfo.InvariantCulture);
				}
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("delete from Hishop_MemberMessageBox where MessageId in ({0}) ", text));
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override DbQueryResult GetMemberSendedMessages(MessageBoxQuery query)
		{
			string filter = string.Format("Sernder='{0}'", query.Sernder);
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, "MessageId", SortAction.Desc, query.IsCount, "vw_Hishop_MemberMessageBox", "MessageId", filter, "*");
		}
		public override DbQueryResult GetMemberReceivedMessages(MessageBoxQuery query)
		{
			string filter = string.Format("Accepter='{0}'", query.Accepter);
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, "MessageId", SortAction.Desc, query.IsCount, "vw_Hishop_MemberMessageBox", "MessageId", filter, "*");
		}
		public override MessageBoxInfo GetMemberMessage(long messageId)
		{
			MessageBoxInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vw_Hishop_MemberMessageBox WHERE MessageId=@MessageId;");
			this.database.AddInParameter(sqlStringCommand, "MessageId", System.Data.DbType.Int64, messageId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateMessageBox(dataReader);
				}
			}
			return result;
		}
		public override bool PostMemberMessageIsRead(long messageId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_MemberMessageBox set IsRead=1 where MessageId=@MessageId");
			this.database.AddInParameter(sqlStringCommand, "MessageId", System.Data.DbType.Int64, messageId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool AddProductToFavorite(int productId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO distro_Favorite(ProductId, UserId, Tags, Remark)VALUES(@ProductId, @UserId, @Tags, @Remark)");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			this.database.AddInParameter(sqlStringCommand, "Tags", System.Data.DbType.String, string.Empty);
			this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, string.Empty);
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
		public override bool ExistsProduct(int productId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM distro_Favorite WHERE UserId=@UserId AND ProductId=@ProductId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
		}
		public override int UpdateFavorite(int favoriteId, string tags, string remark)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Favorite SET Tags = @Tags, Remark = @Remark WHERE FavoriteId = @FavoriteId");
			this.database.AddInParameter(sqlStringCommand, "Tags", System.Data.DbType.String, tags);
			this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, remark);
			this.database.AddInParameter(sqlStringCommand, "FavoriteId", System.Data.DbType.Int32, favoriteId);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override int DeleteFavorite(int favoriteId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_Favorite WHERE FavoriteId = @FavoriteId");
			this.database.AddInParameter(sqlStringCommand, "FavoriteId", System.Data.DbType.Int32, favoriteId);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override DbQueryResult GetFavorites(int userId, string tags, Pagination page)
		{
			DbQueryResult dbQueryResult = new DbQueryResult();
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("ac_Underling_Favorites_Get");
			this.database.AddInParameter(storedProcCommand, "PageIndex", System.Data.DbType.Int32, page.PageIndex);
			this.database.AddInParameter(storedProcCommand, "PageSize", System.Data.DbType.Int32, page.PageSize);
			this.database.AddInParameter(storedProcCommand, "IsCount", System.Data.DbType.Boolean, page.IsCount);
			Member member = HiContext.Current.User as Member;
			this.database.AddInParameter(storedProcCommand, "GradeId", System.Data.DbType.Int32, member.GradeId);
			this.database.AddInParameter(storedProcCommand, "SqlPopulate", System.Data.DbType.String, CommentData.BuildFavoriteQuery(userId, tags));
			this.database.AddOutParameter(storedProcCommand, "TotalFavorites", System.Data.DbType.Int32, 4);
			this.database.AddInParameter(storedProcCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
			{
				dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (page.IsCount && dataReader.NextResult())
				{
					dataReader.Read();
					dbQueryResult.TotalRecords = dataReader.GetInt32(0);
				}
			}
			return dbQueryResult;
		}
		public override bool DeleteFavorites(string string_0)
		{
			bool result = false;
			string text = "DELETE from distro_Favorite WHERE FavoriteId IN (" + string_0 + ")";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
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
		public override ProductInfo GetProductDetails(int productId)
		{
			ProductInfo result = new ProductInfo();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_Products WHERE ProductId = @ProductId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateSubProduct(dataReader);
				}
			}
			return result;
		}
		public override int GetUserProductReviewsCount()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(ReviewId) AS Count FROM distro_ProductReviews WHERE UserId=@UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			int result = 0;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read() && DBNull.Value != dataReader["Count"])
				{
					result = (int)dataReader["Count"];
				}
			}
			return result;
		}
		public override System.Data.DataSet GetUserProductReviewsAndReplys(UserProductReviewAndReplyQuery query, out int total)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("ac_Underling_UserReviewsAndReplys_Get");
			this.database.AddInParameter(storedProcCommand, "PageIndex", System.Data.DbType.Int32, query.PageIndex);
			this.database.AddInParameter(storedProcCommand, "PageSize", System.Data.DbType.Int32, query.PageSize);
			this.database.AddInParameter(storedProcCommand, "IsCount", System.Data.DbType.Boolean, query.IsCount);
			this.database.AddInParameter(storedProcCommand, "UserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", System.Data.DbType.String, CommentData.BuildUserReviewsAndReplysQuery(query));
			this.database.AddInParameter(storedProcCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId);
			this.database.AddOutParameter(storedProcCommand, "Total", System.Data.DbType.Int32, 4);
			System.Data.DataSet dataSet = this.database.ExecuteDataSet(storedProcCommand);
			dataSet.Relations.Add("PtReviews", dataSet.Tables[0].Columns["ProductId"], dataSet.Tables[1].Columns["ProductId"], false);
			total = (int)this.database.GetParameterValue(storedProcCommand, "Total");
			return dataSet;
		}
		public override System.Data.DataSet GetProductConsultationsAndReplys(ProductConsultationAndReplyQuery query, out int total)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("ac_Underling_ConsultationsAndReplys_Get");
			this.database.AddInParameter(storedProcCommand, "PageIndex", System.Data.DbType.Int32, query.PageIndex);
			this.database.AddInParameter(storedProcCommand, "PageSize", System.Data.DbType.Int32, query.PageSize);
			this.database.AddInParameter(storedProcCommand, "IsCount", System.Data.DbType.Boolean, query.IsCount);
			this.database.AddInParameter(storedProcCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId);
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", System.Data.DbType.String, CommentData.BuildConsultationAndReplyQuery(query));
			this.database.AddOutParameter(storedProcCommand, "Total", System.Data.DbType.Int32, 4);
			System.Data.DataSet dataSet = this.database.ExecuteDataSet(storedProcCommand);
			dataSet.Relations.Add("ConsultationReplys", dataSet.Tables[0].Columns["ConsultationId"], dataSet.Tables[1].Columns["ConsultationId"], false);
			total = (int)this.database.GetParameterValue(storedProcCommand, "Total");
			return dataSet;
		}
		public override DbQueryResult GetBatchBuyProducts(ProductQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SaleStatus=1 AND DistributorUserId={0}", HiContext.Current.SiteSettings.UserId);
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
			if (query.CategoryId.HasValue && query.CategoryId.Value > 0)
			{
				stringBuilder.AppendFormat(" AND ( MainCategoryPath LIKE '{0}|%' OR ExtendCategoryPath LIKE '{0}|%')", query.MaiCategoryPath);
			}
			if (query.BrandId.HasValue)
			{
				stringBuilder.AppendFormat(" AND BrandId = {0}", query.BrandId.Value);
			}
			if (!string.IsNullOrEmpty(query.ProductCode))
			{
				stringBuilder.AppendFormat(" AND SKU LIKE '%{0}%'", DataHelper.CleanSearchString(query.ProductCode));
			}
			Member member = HiContext.Current.User as Member;
			int memberDiscount = this.GetMemberDiscount(member.GradeId);
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.Append("SkuId, ProductId, SKU,ProductName, ThumbnailUrl40, DisplaySequence, Stock,");
			stringBuilder2.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM distro_SKUMemberPrice WHERE SkuId = s.SkuId AND DistributoruserId = {0} AND GradeId = {1}) = 1", HiContext.Current.SiteSettings.UserId, member.GradeId);
			stringBuilder2.AppendFormat(" THEN (SELECT MemberSalePrice FROM distro_SKUMemberPrice WHERE SkuId = s.SkuId AND DistributoruserId = {0} AND GradeId = {1})", HiContext.Current.SiteSettings.UserId, member.GradeId);
			stringBuilder2.AppendFormat(" ELSE (SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = s.SkuId AND DistributoruserId = {0}) * {1} /100 END) AS SalePrice", HiContext.Current.SiteSettings.UserId, memberDiscount);
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_distro_ProductSkuList s", "ProductId", stringBuilder.ToString(), stringBuilder2.ToString());
		}
		private int GetMemberDiscount(int gradeId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Discount FROM distro_MemberGrades WHERE GradeId=@GradeId AND CreateUserId = @CreateUserId");
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, gradeId);
			this.database.AddInParameter(sqlStringCommand, "CreateUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId);
			return (int)this.database.ExecuteScalar(sqlStringCommand);
		}
		private static string BuildFavoriteQuery(int userId, string tags)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" SELECT FavoriteId FROM distro_Favorite WHERE UserId = {0} ", userId);
			stringBuilder.AppendFormat(" AND ProductId IN (SELECT ProductId FROM distro_Products WHERE SaleStatus=1 AND DistributorUserId={0}) ", HiContext.Current.SiteSettings.UserId);
			if (!string.IsNullOrEmpty(tags))
			{
				stringBuilder.AppendFormat(" AND (ProductId IN (SELECT ProductId FROM distro_Products WHERE SaleStatus=1 AND DistributorUserId={0} AND  ProductName LIKE '%{1}%') ", HiContext.Current.SiteSettings.UserId, DataHelper.CleanSearchString(tags));
				stringBuilder.AppendFormat(" OR Tags LIKE '%{0}%')", DataHelper.CleanSearchString(tags));
			}
			stringBuilder.AppendFormat(" ORDER BY FavoriteId DESC", new object[0]);
			return stringBuilder.ToString();
		}
		private static string BuildUserReviewsAndReplysQuery(UserProductReviewAndReplyQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" SELECT ProductId FROM distro_ProductReviews ");
			stringBuilder.AppendFormat(" WHERE UserId = {0} ", HiContext.Current.User.UserId);
			stringBuilder.AppendFormat(" AND ProductId IN (SELECT ProductId FROM distro_Products WHERE DistributorUserId={0} )", HiContext.Current.SiteSettings.UserId);
			stringBuilder.Append(" GROUP BY ProductId");
			return stringBuilder.ToString();
		}
		private static string BuildConsultationAndReplyQuery(ProductConsultationAndReplyQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" SELECT ConsultationId FROM distro_ProductConsultations ");
			stringBuilder.Append(" WHERE 1 = 1");
			if (query.ProductId > 0)
			{
				stringBuilder.AppendFormat(" AND ProductId = {0} ", query.ProductId);
			}
			if (query.UserId > 0)
			{
				stringBuilder.AppendFormat(" AND UserId = {0} ", query.UserId);
			}
			if (query.Type == ConsultationReplyType.NoReply)
			{
				stringBuilder.Append(" AND ReplyText IS NULL");
			}
			else
			{
				if (query.Type == ConsultationReplyType.Replyed)
				{
					stringBuilder.Append(" AND ReplyText IS NOT NULL");
				}
			}
			stringBuilder.Append(" ORDER BY replydate DESC");
			return stringBuilder.ToString();
		}
	}
}
