using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.SaleSystem.Comments;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
namespace Hidistro.SaleSystem.Data
{
	public class CommentData : CommentMasterProvider
	{
		private Database database;
		public CommentData()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public override IList<FriendlyLinksInfo> GetFriendlyLinksIsVisible(int? number)
		{
			IList<FriendlyLinksInfo> list = new List<FriendlyLinksInfo>();
			string text = string.Empty;
			if (number.HasValue)
			{
				text = string.Format("SELECT Top {0} * FROM Hishop_FriendlyLinks WHERE  Visible = 1 ORDER BY DisplaySequence DESC", number.Value);
			}
			else
			{
				text = string.Format("SELECT * FROM Hishop_FriendlyLinks WHERE  Visible = 1 ORDER BY DisplaySequence DESC", new object[0]);
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulateFriendlyLink(dataReader));
				}
			}
			return list;
		}
		public override System.Data.DataSet GetHelps()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_HelpCategories WHERE IsShowFooter = 1 ORDER BY DisplaySequence SELECT * FROM Hishop_Helps WHERE IsShowFooter = 1  AND CategoryId IN (SELECT CategoryId FROM Hishop_HelpCategories WHERE IsShowFooter = 1)");
			System.Data.DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand);
			System.Data.DataColumn parentColumn = dataSet.Tables[0].Columns["CateGoryId"];
			System.Data.DataColumn childColumn = dataSet.Tables[1].Columns["CateGoryId"];
			System.Data.DataRelation relation = new System.Data.DataRelation("CateGory", parentColumn, childColumn);
			dataSet.Relations.Add(relation);
			return dataSet;
		}
		public override List<AfficheInfo> GetAfficheList()
		{
			List<AfficheInfo> list = new List<AfficheInfo>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Affiche  ORDER BY AddedDate DESC");
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					AfficheInfo item = DataMapper.PopulateAffiche(dataReader);
					list.Add(item);
				}
			}
			return list;
		}
		public override AfficheInfo GetAffiche(int afficheId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Affiche WHERE AfficheId = @AfficheId");
			this.database.AddInParameter(sqlStringCommand, "AfficheId", System.Data.DbType.Int32, afficheId);
			AfficheInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateAffiche(dataReader);
				}
			}
			return result;
		}
		public override AfficheInfo GetFrontOrNextAffiche(int afficheId, string type)
		{
			string text = string.Empty;
			if (type == "Next")
			{
				text = "SELECT TOP 1 * FROM Hishop_Affiche WHERE AfficheId < @AfficheId  ORDER BY AfficheId DESC";
			}
			else
			{
				text = "SELECT TOP 1 * FROM Hishop_Affiche WHERE AfficheId > @AfficheId ORDER BY AfficheId ASC";
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			this.database.AddInParameter(sqlStringCommand, "AfficheId", System.Data.DbType.Int32, afficheId);
			AfficheInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateAffiche(dataReader);
				}
			}
			return result;
		}
		public override System.Data.DataSet GetVoteByIsShow()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Votes WHERE IsBackup = 1 SELECT * FROM Hishop_VoteItems WHERE  voteId IN (SELECT voteId FROM Hishop_Votes WHERE IsBackup = 1)");
			System.Data.DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand);
			System.Data.DataColumn parentColumn = dataSet.Tables[0].Columns["VoteId"];
			System.Data.DataColumn childColumn = dataSet.Tables[1].Columns["VoteId"];
			System.Data.DataRelation relation = new System.Data.DataRelation("Vote", parentColumn, childColumn);
			dataSet.Relations.Add(relation);
			return dataSet;
		}
		public override VoteInfo GetVoteById(long voteId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *, (SELECT ISNULL(SUM(ItemCount),0) FROM Hishop_VoteItems WHERE VoteId = @VoteId) AS VoteCounts FROM Hishop_Votes WHERE VoteId = @VoteId");
			this.database.AddInParameter(sqlStringCommand, "VoteId", System.Data.DbType.Int64, voteId);
			VoteInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateVote(dataReader);
				}
			}
			return result;
		}
		public override IList<VoteItemInfo> GetVoteItems(long voteId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_VoteItems WHERE VoteId = @VoteId");
			this.database.AddInParameter(sqlStringCommand, "VoteId", System.Data.DbType.Int64, voteId);
			IList<VoteItemInfo> list = new List<VoteItemInfo>();
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					VoteItemInfo item = DataMapper.PopulateVoteItem(dataReader);
					list.Add(item);
				}
			}
			return list;
		}
		public override VoteItemInfo GetVoteItemById(long voteItemId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_VoteItems WHERE VoteItemId = @VoteItemId");
			this.database.AddInParameter(sqlStringCommand, "VoteItemId", System.Data.DbType.Int64, voteItemId);
			VoteItemInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateVoteItem(dataReader);
				}
			}
			return result;
		}
		public override int Vote(long voteItemId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_VoteItems SET ItemCount = ItemCount + 1 WHERE VoteItemId = @VoteItemId");
			this.database.AddInParameter(sqlStringCommand, "VoteItemId", System.Data.DbType.Int32, voteItemId);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override System.Data.DataTable GetHotKeywords(int categoryId, int hotKeywordsNum)
		{
			System.Data.DataTable result = null;
			string text = string.Format("SELECT TOP {0} * FROM Hishop_Hotkeywords", hotKeywordsNum);
			if (categoryId != 0)
			{
				text += string.Format(" WHERE CategoryId = {0}", categoryId);
			}
			text += " ORDER BY Frequency DESC";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override System.Data.DataSet GetAllHotKeywords()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT CategoryId, Name AS CategoryName, RewriteName FROM Hishop_Categories WHERE Depth = 1 ORDER BY DisplaySequence; SELECT * FROM Hishop_Hotkeywords ORDER BY Frequency DESC");
			System.Data.DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand);
			dataSet.Relations.Add("relation", dataSet.Tables[0].Columns["CategoryId"], dataSet.Tables[1].Columns["CategoryId"], false);
			return dataSet;
		}
		public override ArticleCategoryInfo GetArticleCategory(int categoryId)
		{
			ArticleCategoryInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * From Hishop_ArticleCategories WHERE CategoryId=@CategoryId");
			this.database.AddInParameter(sqlStringCommand, "CategoryId", System.Data.DbType.Int32, categoryId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateArticleCategory(dataReader);
				}
			}
			return result;
		}
		public override ArticleInfo GetArticle(int articleId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Articles WHERE ArticleId = @ArticleId");
			this.database.AddInParameter(sqlStringCommand, "ArticleId", System.Data.DbType.Int32, articleId);
			ArticleInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateArticle(dataReader);
				}
			}
			return result;
		}
		public override ArticleInfo GetFrontOrNextArticle(int articleId, string type, int categoryId)
		{
			string text = string.Empty;
			if (type == "Next")
			{
				text = "SELECT TOP 1 * FROM Hishop_Articles WHERE ArticleId < @ArticleId AND CategoryId=@CategoryId AND IsRelease=1 ORDER BY ArticleId DESC";
			}
			else
			{
				text = "SELECT TOP 1 * FROM Hishop_Articles WHERE  ArticleId > @ArticleId AND CategoryId=@CategoryId AND IsRelease=1 ORDER BY ArticleId ASC";
			}
			ArticleInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			this.database.AddInParameter(sqlStringCommand, "ArticleId", System.Data.DbType.Int32, articleId);
			this.database.AddInParameter(sqlStringCommand, "CategoryId", System.Data.DbType.Int32, categoryId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateArticle(dataReader);
				}
			}
			return result;
		}
		public override IList<ArticleInfo> GetArticleList(int categoryId, int maxNum)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT TOP {0} * FROM vw_Hishop_Articles WHERE IsRelease=1", maxNum);
			if (categoryId != 0)
			{
				stringBuilder.AppendFormat(" AND CategoryId = {0}", categoryId);
			}
			stringBuilder.Append(" ORDER BY AddedDate DESC");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			IList<ArticleInfo> list = new List<ArticleInfo>();
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					ArticleInfo item = DataMapper.PopulateArticle(dataReader);
					list.Add(item);
				}
			}
			return list;
		}
		public override DbQueryResult GetArticleList(ArticleQuery articleQuery)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("IsRelease=1");
			stringBuilder.AppendFormat(" and Title LIKE '%{0}%'", articleQuery.Keywords);
			if (articleQuery.CategoryId.HasValue)
			{
				stringBuilder.AppendFormat(" AND CategoryId = {0}", articleQuery.CategoryId.Value);
			}
			if (articleQuery.StartArticleTime.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate >= '{0}'", articleQuery.StartArticleTime.Value);
			}
			if (articleQuery.EndArticleTime.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate <= '{0}'", articleQuery.EndArticleTime.Value);
			}
			return DataHelper.PagingByRownumber(articleQuery.PageIndex, articleQuery.PageSize, articleQuery.SortBy, articleQuery.SortOrder, articleQuery.IsCount, "vw_Hishop_Articles", "ArticleId", stringBuilder.ToString(), "*");
		}
		public override IList<ArticleCategoryInfo> GetArticleMainCategories()
		{
			IList<ArticleCategoryInfo> list = new List<ArticleCategoryInfo>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ArticleCategories ORDER BY [DisplaySequence]");
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					ArticleCategoryInfo item = DataMapper.PopulateArticleCategory(dataReader);
					list.Add(item);
				}
			}
			return list;
		}
		public override System.Data.DataTable GetArticlProductList(int articlId)
		{
			System.Data.DataTable result = null;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" SELECT TOP 20 ProductId,ProductName,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180,ThumbnailUrl220 FROM Hishop_Products");
			stringBuilder.AppendFormat(" WHERE SaleStatus = {0} AND ProductId IN (SELECT RelatedProductId FROM Hishop_RelatedArticsProducts WHERE ArticleId = {1})", 1, articlId);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override HelpCategoryInfo GetHelpCategory(int categoryId)
		{
			HelpCategoryInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_HelpCategories WHERE CategoryId=@CategoryId");
			this.database.AddInParameter(sqlStringCommand, "CategoryId", System.Data.DbType.Int32, categoryId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateHelpCategory(dataReader);
				}
			}
			return result;
		}
		public override DbQueryResult GetHelpList(HelpQuery helpQuery)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("Title LIKE '%{0}%'", helpQuery.Keywords);
			if (helpQuery.CategoryId.HasValue)
			{
				stringBuilder.AppendFormat(" AND CategoryId = {0}", helpQuery.CategoryId.Value);
			}
			if (helpQuery.StartArticleTime.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate >= '{0}'", helpQuery.StartArticleTime.Value);
			}
			if (helpQuery.EndArticleTime.HasValue)
			{
				stringBuilder.AppendFormat(" AND AddedDate <= '{0}'", helpQuery.EndArticleTime.Value);
			}
			return DataHelper.PagingByTopnotin(helpQuery.PageIndex, helpQuery.PageSize, helpQuery.SortBy, helpQuery.SortOrder, helpQuery.IsCount, "vw_Hishop_Helps", "HelpId", stringBuilder.ToString(), "*");
		}
		public override System.Data.DataSet GetHelpTitleList()
		{
			string text = "SELECT * FROM Hishop_HelpCategories order by DisplaySequence";
			text += " SELECT HelpId,Title,CategoryId FROM Hishop_Helps where CategoryId IN (SELECT CategoryId FROM Hishop_HelpCategories)";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format(text, new object[0]));
			System.Data.DataSet dataSet = this.database.ExecuteDataSet(sqlStringCommand);
			System.Data.DataColumn parentColumn = dataSet.Tables[0].Columns["CateGoryId"];
			System.Data.DataColumn childColumn = dataSet.Tables[1].Columns["CateGoryId"];
			System.Data.DataRelation relation = new System.Data.DataRelation("CateGory", parentColumn, childColumn);
			dataSet.Relations.Add(relation);
			return dataSet;
		}
		public override IList<HelpCategoryInfo> GetHelpCategorys()
		{
			IList<HelpCategoryInfo> list = new List<HelpCategoryInfo>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_HelpCategories ORDER BY DisplaySequence");
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulateHelpCategory(dataReader));
				}
			}
			return list;
		}
		public override HelpInfo GetHelp(int helpId)
		{
			HelpInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Helps WHERE HelpId=@HelpId");
			this.database.AddInParameter(sqlStringCommand, "HelpId", System.Data.DbType.Int32, helpId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateHelp(dataReader);
				}
			}
			return result;
		}
		public override HelpInfo GetFrontOrNextHelp(int helpId, int categoryId, string type)
		{
			string text = string.Empty;
			if (type == "Next")
			{
				text = "SELECT TOP 1 * FROM Hishop_Helps WHERE HelpId <@HelpId AND CategoryId=@CategoryId ORDER BY HelpId DESC";
			}
			else
			{
				text = "SELECT TOP 1 * FROM Hishop_Helps WHERE HelpId >@HelpId AND CategoryId=@CategoryId ORDER BY HelpId ASC";
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			this.database.AddInParameter(sqlStringCommand, "HelpId", System.Data.DbType.Int32, helpId);
			this.database.AddInParameter(sqlStringCommand, "CategoryId", System.Data.DbType.Int32, categoryId);
			HelpInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateHelp(dataReader);
				}
				dataReader.Close();
			}
			return result;
		}
		public override System.Data.DataTable GetPromotes(Pagination pagination, int promotionType, out int totalPromotes)
		{
			System.Data.DataTable result = null;
			string text = string.Format("SELECT COUNT(*) FROM Hishop_Promotions WHERE 1=1 ", new object[0]);
			if (promotionType != 0)
			{
				text += string.Format(" AND PromoteType={0} ", promotionType);
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			totalPromotes = Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
			string text2 = string.Empty;
			StringBuilder stringBuilder = new StringBuilder("case Hishop_Promotions.PromoteType");
			stringBuilder.AppendFormat(" when 1 then '商品直接打折'", new object[0]);
			stringBuilder.AppendFormat(" when 2 then '商品固定金额出售'", new object[0]);
			stringBuilder.AppendFormat(" when 3 then '商品减价优惠'", new object[0]);
			stringBuilder.AppendFormat(" when 4 then '批发打折'", new object[0]);
			stringBuilder.AppendFormat(" when 5 then '买商品赠送礼品'", new object[0]);
			stringBuilder.AppendFormat(" when 6 then '商品有买有送'", new object[0]);
			stringBuilder.AppendFormat(" when 11 then '订单满额打折'", new object[0]);
			stringBuilder.AppendFormat(" when 12 then '订单满额优惠金额'", new object[0]);
			stringBuilder.AppendFormat(" when 13 then '混合批发打折'", new object[0]);
			stringBuilder.AppendFormat(" when 14 then '混合批发优惠金额'", new object[0]);
			stringBuilder.AppendFormat(" when 15 then '订单满额送礼品'", new object[0]);
			stringBuilder.AppendFormat(" when 16 then '订单满额送倍数积分'", new object[0]);
			stringBuilder.AppendFormat(" when 17 then '订单满额免运费'", new object[0]);
			stringBuilder.Append(" end as PromoteTypeName");
			if (pagination.PageIndex == 1)
			{
				text2 = "SELECT TOP 10 *," + stringBuilder + " FROM Hishop_Promotions WHERE 1=1 ";
			}
			else
			{
				text2 = string.Format("SELECT TOP {0} *," + stringBuilder + " FROM Hishop_Promotions WHERE ActivityId NOT IN (SELECT TOP {1} ActivityId FROM Hishop_Promotions) ", pagination.PageSize, pagination.PageSize * (pagination.PageIndex - 1));
			}
			if (promotionType != 0)
			{
				text2 += string.Format(" AND PromoteType={0} ", promotionType);
			}
			text2 += " ORDER BY ActivityId DESC";
			sqlStringCommand = this.database.GetSqlStringCommand(text2);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override PromotionInfo GetPromote(int activityId)
		{
			PromotionInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Promotions WHERE ActivityId=@ActivityId");
			this.database.AddInParameter(sqlStringCommand, "ActivityId", System.Data.DbType.Int32, activityId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulatePromote(dataReader);
				}
				dataReader.Close();
			}
			return result;
		}
		public override PromotionInfo GetFrontOrNextPromoteInfo(PromotionInfo promote, string type)
		{
			string text = string.Empty;
			if (type == "Next")
			{
				text = "SELECT TOP 1 * FROM Hishop_Promotions WHERE activityId<@activityId AND PromoteType=@PromoteType  ORDER BY activityId DESC";
			}
			else
			{
				text = "SELECT TOP 1 * FROM Hishop_Promotions WHERE activityId>@activityId AND PromoteType=@PromoteType ORDER BY activityId ASC";
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			this.database.AddInParameter(sqlStringCommand, "activityId", System.Data.DbType.Int32, promote.ActivityId);
			this.database.AddInParameter(sqlStringCommand, "PromoteType", System.Data.DbType.Int32, Convert.ToInt32(promote.PromoteType));
			PromotionInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulatePromote(dataReader);
				}
				dataReader.Close();
			}
			return result;
		}
		public override DbQueryResult GetLeaveComments(LeaveCommentQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" SELECT l.LeaveId FROM Hishop_LeaveComments l ");
			stringBuilder.Append(" WHERE IsReply = 1 ");
			stringBuilder.Append(" ORDER BY LastDate desc");
			DbQueryResult dbQueryResult = new DbQueryResult();
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_LeaveComments_Get");
			this.database.AddInParameter(storedProcCommand, "PageIndex", System.Data.DbType.Int32, query.PageIndex);
			this.database.AddInParameter(storedProcCommand, "PageSize", System.Data.DbType.Int32, query.PageSize);
			this.database.AddInParameter(storedProcCommand, "IsCount", System.Data.DbType.Boolean, query.IsCount);
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", System.Data.DbType.String, stringBuilder.ToString());
			this.database.AddOutParameter(storedProcCommand, "Total", System.Data.DbType.Int32, 4);
			System.Data.DataSet dataSet = this.database.ExecuteDataSet(storedProcCommand);
			dataSet.Relations.Add("LeaveCommentReplays", dataSet.Tables[0].Columns["LeaveId"], dataSet.Tables[1].Columns["LeaveId"], false);
			dbQueryResult.Data = dataSet;
			dbQueryResult.TotalRecords = (int)this.database.GetParameterValue(storedProcCommand, "Total");
			return dbQueryResult;
		}
		public override bool InsertLeaveComment(LeaveCommentInfo leave)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Insert into Hishop_LeaveComments(UserId,UserName,Title,PublishContent,PublishDate,LastDate)  values(@UserId,@UserName,@Title,@PublishContent,@PublishDate,@LastDate)");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, leave.UserId);
			this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, leave.UserName);
			this.database.AddInParameter(sqlStringCommand, "Title", System.Data.DbType.String, leave.Title);
			this.database.AddInParameter(sqlStringCommand, "PublishContent", System.Data.DbType.String, leave.PublishContent);
			this.database.AddInParameter(sqlStringCommand, "PublishDate", System.Data.DbType.DateTime, DataHelper.GetSafeDateTimeFormat(DateTime.Now));
			this.database.AddInParameter(sqlStringCommand, "LastDate", System.Data.DbType.DateTime, DataHelper.GetSafeDateTimeFormat(DateTime.Now));
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
	}
}
