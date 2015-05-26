using Hidistro.ControlPanel.Comments;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Comments;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core.Enums;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
namespace Hidistro.ControlPanel.Data
{
	public class CommentData : CommentsProvider
	{
		private Database database;
		public CommentData()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public override bool AddArticle(ArticleInfo article)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_Articles(CategoryId, Title, Meta_Description, Meta_Keywords, IconUrl, Description, Content, AddedDate,IsRelease) VALUES (@CategoryId, @Title, @Meta_Description, @Meta_Keywords,  @IconUrl, @Description, @Content, @AddedDate,@IsRelease)");
			this.database.AddInParameter(sqlStringCommand, "CategoryId", System.Data.DbType.Int32, article.CategoryId);
			this.database.AddInParameter(sqlStringCommand, "Title", System.Data.DbType.String, article.Title);
			this.database.AddInParameter(sqlStringCommand, "Meta_Description", System.Data.DbType.String, article.MetaDescription);
			this.database.AddInParameter(sqlStringCommand, "Meta_Keywords", System.Data.DbType.String, article.MetaKeywords);
			this.database.AddInParameter(sqlStringCommand, "IconUrl", System.Data.DbType.String, article.IconUrl);
			this.database.AddInParameter(sqlStringCommand, "Description", System.Data.DbType.String, article.Description);
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, article.Content);
			this.database.AddInParameter(sqlStringCommand, "AddedDate", System.Data.DbType.DateTime, article.AddedDate);
			this.database.AddInParameter(sqlStringCommand, "IsRelease", System.Data.DbType.Boolean, article.IsRelease);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override bool UpdateArticle(ArticleInfo article)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Articles SET CategoryId = @CategoryId,AddedDate = @AddedDate,Title = @Title, Meta_Description = @Meta_Description, Meta_Keywords = @Meta_Keywords, IconUrl=@IconUrl,Description = @Description,Content = @Content,IsRelease=@IsRelease WHERE ArticleId = @ArticleId");
			this.database.AddInParameter(sqlStringCommand, "CategoryId", System.Data.DbType.Int32, article.CategoryId);
			this.database.AddInParameter(sqlStringCommand, "Title", System.Data.DbType.String, article.Title);
			this.database.AddInParameter(sqlStringCommand, "Meta_Description", System.Data.DbType.String, article.MetaDescription);
			this.database.AddInParameter(sqlStringCommand, "Meta_Keywords", System.Data.DbType.String, article.MetaKeywords);
			this.database.AddInParameter(sqlStringCommand, "IconUrl", System.Data.DbType.String, article.IconUrl);
			this.database.AddInParameter(sqlStringCommand, "Description", System.Data.DbType.String, article.Description);
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, article.Content);
			this.database.AddInParameter(sqlStringCommand, "IsRelease", System.Data.DbType.Boolean, article.IsRelease);
			this.database.AddInParameter(sqlStringCommand, "AddedDate", System.Data.DbType.DateTime, article.AddedDate);
			this.database.AddInParameter(sqlStringCommand, "ArticleId", System.Data.DbType.Int32, article.ArticleId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override bool DeleteArticle(int articleId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Articles WHERE ArticleId = @ArticleId");
			this.database.AddInParameter(sqlStringCommand, "ArticleId", System.Data.DbType.Int32, articleId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override void SwapArticleCategorySequence(int categoryId, int replaceCategoryId, int displaySequence, int replaceDisplaySequence)
		{
			DataHelper.SwapSequence("Hishop_ArticleCategories", "CategoryId", "DisplaySequence", categoryId, replaceCategoryId, displaySequence, replaceDisplaySequence);
		}
		public override bool CreateUpdateDeleteArticleCategory(ArticleCategoryInfo articleCategory, DataProviderAction action)
		{
			bool result;
			if (null == articleCategory)
			{
				result = false;
			}
			else
			{
				System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ArticleCategory_CreateUpdateDelete");
				this.database.AddInParameter(storedProcCommand, "Action", System.Data.DbType.Int32, (int)action);
				this.database.AddOutParameter(storedProcCommand, "Status", System.Data.DbType.Int32, 4);
				if (action != DataProviderAction.Create)
				{
					this.database.AddInParameter(storedProcCommand, "CategoryId", System.Data.DbType.Int32, articleCategory.CategoryId);
				}
				if (action != DataProviderAction.Delete)
				{
					this.database.AddInParameter(storedProcCommand, "Name", System.Data.DbType.String, articleCategory.Name);
					this.database.AddInParameter(storedProcCommand, "IconUrl", System.Data.DbType.String, articleCategory.IconUrl);
					this.database.AddInParameter(storedProcCommand, "Description", System.Data.DbType.String, articleCategory.Description);
				}
				this.database.ExecuteNonQuery(storedProcCommand);
				result = ((int)this.database.GetParameterValue(storedProcCommand, "Status") == 0);
			}
			return result;
		}
		public override int DeleteArticles(IList<int> articles)
		{
			int num = 0;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Articles WHERE ArticleId = @ArticleId");
			this.database.AddInParameter(sqlStringCommand, "ArticleId", System.Data.DbType.Int32);
			foreach (int current in articles)
			{
				this.database.SetParameterValue(sqlStringCommand, "ArticleId", current);
				this.database.ExecuteNonQuery(sqlStringCommand);
				num++;
			}
			return num;
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
		public override ArticleCategoryInfo GetArticleCategory(int categoryId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ArticleCategories WHERE CategoryId = @CategoryId ORDER BY [DisplaySequence]");
			this.database.AddInParameter(sqlStringCommand, "CategoryId", System.Data.DbType.Int32, categoryId);
			ArticleCategoryInfo result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateArticleCategory(dataReader);
				}
				else
				{
					result = null;
				}
			}
			return result;
		}
		public override DbQueryResult GetArticleList(ArticleQuery articleQuery)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("Title LIKE '%{0}%'", DataHelper.CleanSearchString(articleQuery.Keywords));
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
		public override IList<ArticleCategoryInfo> GetMainArticleCategories()
		{
			IList<ArticleCategoryInfo> list = new List<ArticleCategoryInfo>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * From Hishop_ArticleCategories ORDER BY [DisplaySequence]");
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
		public override DbQueryResult GetRelatedArticsProducts(Pagination page, int articId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" SaleStatus = {0}", 1);
			stringBuilder.AppendFormat(" AND ProductId IN (SELECT RelatedProductId FROM Hishop_RelatedArticsProducts WHERE ArticleId = {0})", articId);
			string selectFields = "ProductId, ProductCode, ProductName, ThumbnailUrl40, MarketPrice, SalePrice, Stock, DisplaySequence";
			return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "vw_Hishop_BrowseProductList p", "ProductId", stringBuilder.ToString(), selectFields);
		}
		public override bool AddReleatesProdcutByArticId(int articId, int prodcutId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_RelatedArticsProducts(ArticleId, RelatedProductId) VALUES (@ArticleId, @RelatedProductId)");
			this.database.AddInParameter(sqlStringCommand, "ArticleId", System.Data.DbType.Int32, articId);
			this.database.AddInParameter(sqlStringCommand, "RelatedProductId", System.Data.DbType.Int32, prodcutId);
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
		public override bool RemoveReleatesProductByArticId(int articId, int productId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_RelatedArticsProducts WHERE ArticleId = @ArticleId AND RelatedProductId = @RelatedProductId");
			this.database.AddInParameter(sqlStringCommand, "ArticleId", System.Data.DbType.Int32, articId);
			this.database.AddInParameter(sqlStringCommand, "RelatedProductId", System.Data.DbType.Int32, productId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool RemoveReleatesProductByArticId(int articId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_RelatedArticsProducts WHERE ArticleId = @ArticleId");
			this.database.AddInParameter(sqlStringCommand, "ArticleId", System.Data.DbType.Int32, articId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool UpdateRelease(int articId, bool release)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update Hishop_Articles set IsRelease=@IsRelease  where ArticleId = @ArticleId");
			this.database.AddInParameter(sqlStringCommand, "ArticleId", System.Data.DbType.Int32, articId);
			this.database.AddInParameter(sqlStringCommand, "IsRelease", System.Data.DbType.Boolean, release);
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
		public override bool CreateUpdateDeleteHelpCategory(HelpCategoryInfo helpCategory, DataProviderAction action)
		{
			bool result;
			if (null == helpCategory)
			{
				result = false;
			}
			else
			{
				System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_HelpCategory_CreateUpdateDelete");
				this.database.AddInParameter(storedProcCommand, "Action", System.Data.DbType.Int32, (int)action);
				this.database.AddOutParameter(storedProcCommand, "Status", System.Data.DbType.Int32, 4);
				if (action != DataProviderAction.Create)
				{
					this.database.AddInParameter(storedProcCommand, "CategoryId", System.Data.DbType.Int32, helpCategory.CategoryId);
				}
				if (action != DataProviderAction.Delete)
				{
					this.database.AddInParameter(storedProcCommand, "Name", System.Data.DbType.String, helpCategory.Name);
					this.database.AddInParameter(storedProcCommand, "IconUrl", System.Data.DbType.String, helpCategory.IconUrl);
					this.database.AddInParameter(storedProcCommand, "IndexChar", System.Data.DbType.String, helpCategory.IndexChar);
					this.database.AddInParameter(storedProcCommand, "Description", System.Data.DbType.String, helpCategory.Description);
					this.database.AddInParameter(storedProcCommand, "IsShowFooter", System.Data.DbType.Boolean, helpCategory.IsShowFooter);
				}
				this.database.ExecuteNonQuery(storedProcCommand);
				result = ((int)this.database.GetParameterValue(storedProcCommand, "Status") == 0);
			}
			return result;
		}
		public override int DeleteHelpCategorys(List<int> categoryIds)
		{
			int result;
			if (null == categoryIds)
			{
				result = 0;
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_HelpCategories WHERE CategoryId=@CategoryId");
				this.database.AddInParameter(sqlStringCommand, "CategoryId", System.Data.DbType.Int32);
				new StringBuilder();
				int num = 0;
				foreach (int current in categoryIds)
				{
					this.database.SetParameterValue(sqlStringCommand, "CategoryId", current);
					this.database.ExecuteNonQuery(sqlStringCommand);
					num++;
				}
				result = num;
			}
			return result;
		}
		public override bool AddHelp(HelpInfo help)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_Helps(CategoryId, Title, Meta_Description, Meta_Keywords, Description, Content, AddedDate, IsShowFooter) VALUES (@CategoryId, @Title, @Meta_Description, @Meta_Keywords, @Description, @Content, @AddedDate, @IsShowFooter)");
			this.database.AddInParameter(sqlStringCommand, "CategoryId", System.Data.DbType.Int32, help.CategoryId);
			this.database.AddInParameter(sqlStringCommand, "Title", System.Data.DbType.String, help.Title);
			this.database.AddInParameter(sqlStringCommand, "Meta_Description", System.Data.DbType.String, help.MetaDescription);
			this.database.AddInParameter(sqlStringCommand, "Meta_Keywords", System.Data.DbType.String, help.MetaKeywords);
			this.database.AddInParameter(sqlStringCommand, "Description", System.Data.DbType.String, help.Description);
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, help.Content);
			this.database.AddInParameter(sqlStringCommand, "AddedDate", System.Data.DbType.DateTime, help.AddedDate);
			this.database.AddInParameter(sqlStringCommand, "IsShowFooter", System.Data.DbType.Boolean, help.IsShowFooter);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override bool UpdateHelp(HelpInfo help)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Helps SET CategoryId = @CategoryId, AddedDate = @AddedDate, Title = @Title, Meta_Description = @Meta_Description, Meta_Keywords = @Meta_Keywords,  Description = @Description, Content = @Content, IsShowFooter = @IsShowFooter WHERE HelpId = @HelpId");
			this.database.AddInParameter(sqlStringCommand, "CategoryId", System.Data.DbType.Int32, help.CategoryId);
			this.database.AddInParameter(sqlStringCommand, "Title", System.Data.DbType.String, help.Title);
			this.database.AddInParameter(sqlStringCommand, "Meta_Description", System.Data.DbType.String, help.MetaDescription);
			this.database.AddInParameter(sqlStringCommand, "Meta_Keywords", System.Data.DbType.String, help.MetaKeywords);
			this.database.AddInParameter(sqlStringCommand, "Description", System.Data.DbType.String, help.Description);
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, help.Content);
			this.database.AddInParameter(sqlStringCommand, "AddedDate", System.Data.DbType.DateTime, help.AddedDate);
			this.database.AddInParameter(sqlStringCommand, "IsShowFooter", System.Data.DbType.Boolean, help.IsShowFooter);
			this.database.AddInParameter(sqlStringCommand, "HelpId", System.Data.DbType.Int32, help.HelpId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override bool DeleteHelp(int helpId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Helps WHERE HelpId = @HelpId");
			this.database.AddInParameter(sqlStringCommand, "HelpId", System.Data.DbType.Int32, helpId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override int DeleteHelps(IList<int> helps)
		{
			int num = 0;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Helps WHERE HelpId=@HelpId");
			this.database.AddInParameter(sqlStringCommand, "HelpId", System.Data.DbType.Int32);
			foreach (int current in helps)
			{
				this.database.SetParameterValue(sqlStringCommand, "HelpId", current);
				this.database.ExecuteNonQuery(sqlStringCommand);
				num++;
			}
			return num;
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
		public override void SwapHelpCategorySequence(int categoryId, int replaceCategoryId, int displaySequence, int replaceDisplaySequence)
		{
			DataHelper.SwapSequence("Hishop_HelpCategories", "CategoryId", "DisplaySequence", categoryId, replaceCategoryId, displaySequence, replaceDisplaySequence);
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
		public override DbQueryResult GetHelpList(HelpQuery helpQuery)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("Title LIKE '%{0}%'", DataHelper.CleanSearchString(helpQuery.Keywords));
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
		public override int DeleteAffiches(List<int> afficheIds)
		{
			int num = 0;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Affiche WHERE AfficheId=@AfficheId");
			this.database.AddInParameter(sqlStringCommand, "AfficheId", System.Data.DbType.Int32);
			foreach (int current in afficheIds)
			{
				this.database.SetParameterValue(sqlStringCommand, "AfficheId", current);
				this.database.ExecuteNonQuery(sqlStringCommand);
				num++;
			}
			return num;
		}
		public override bool AddAffiche(AfficheInfo affiche)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_Affiche(Title, Content, AddedDate) VALUES (@Title, @Content, @AddedDate)");
			this.database.AddInParameter(sqlStringCommand, "Title", System.Data.DbType.String, affiche.Title);
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, affiche.Content);
			this.database.AddInParameter(sqlStringCommand, "AddedDate", System.Data.DbType.DateTime, affiche.AddedDate);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override bool UpdateAffiche(AfficheInfo affiche)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Affiche SET Title = @Title, AddedDate = @AddedDate, Content = @Content WHERE AfficheId = @AfficheId");
			this.database.AddInParameter(sqlStringCommand, "Title", System.Data.DbType.String, affiche.Title);
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, affiche.Content);
			this.database.AddInParameter(sqlStringCommand, "AddedDate", System.Data.DbType.DateTime, affiche.AddedDate);
			this.database.AddInParameter(sqlStringCommand, "AfficheId", System.Data.DbType.Int32, affiche.AfficheId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override bool DeleteAffiche(int afficheId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Affiche WHERE AfficheId = @AfficheId");
			this.database.AddInParameter(sqlStringCommand, "AfficheId", System.Data.DbType.Int32, afficheId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
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
		public override List<AfficheInfo> GetAfficheList()
		{
			List<AfficheInfo> list = new List<AfficheInfo>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Affiche ORDER BY AddedDate DESC");
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
		public override LeaveCommentInfo GetLeaveComment(long leaveId)
		{
			LeaveCommentInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_LeaveComments WHERE LeaveId=@LeaveId;");
			this.database.AddInParameter(sqlStringCommand, "LeaveId", System.Data.DbType.Int64, leaveId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateLeaveComment(dataReader);
				}
			}
			return result;
		}
		public override DbQueryResult GetLeaveComments(LeaveCommentQuery query)
		{
			DbQueryResult dbQueryResult = new DbQueryResult();
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_LeaveComments_Get");
			this.database.AddInParameter(storedProcCommand, "PageIndex", System.Data.DbType.Int32, query.PageIndex);
			this.database.AddInParameter(storedProcCommand, "PageSize", System.Data.DbType.Int32, query.PageSize);
			this.database.AddInParameter(storedProcCommand, "IsCount", System.Data.DbType.Boolean, query.IsCount);
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", System.Data.DbType.String, CommentData.BuildLeaveCommentQuery(query));
			this.database.AddOutParameter(storedProcCommand, "Total", System.Data.DbType.Int32, 4);
			System.Data.DataSet dataSet = this.database.ExecuteDataSet(storedProcCommand);
			dataSet.Relations.Add("LeaveCommentReplays", dataSet.Tables[0].Columns["LeaveId"], dataSet.Tables[1].Columns["LeaveId"], false);
			dbQueryResult.Data = dataSet;
			dbQueryResult.TotalRecords = (int)this.database.GetParameterValue(storedProcCommand, "Total");
			return dbQueryResult;
		}
		public override bool DeleteLeaveComment(long leaveId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_LeaveCommentReplys WHERE LeaveId=@LeaveId;DELETE FROM Hishop_LeaveComments WHERE LeaveId=@LeaveId");
			this.database.AddInParameter(sqlStringCommand, "leaveId", System.Data.DbType.Int64, leaveId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override int DeleteLeaveComments(IList<long> leaveIds)
		{
			string text = string.Empty;
			foreach (long current in leaveIds)
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("DELETE FROM Hishop_LeaveCommentReplys WHERE LeaveId in ({0});DELETE FROM Hishop_LeaveComments WHERE LeaveId in ({1}) ", text, text));
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override int ReplyLeaveComment(LeaveCommentReplyInfo leaveReply)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_LeaveCommentReplys(LeaveId,UserId,ReplyContent,ReplyDate) VALUES(@LeaveId,@UserId,@ReplyContent,@ReplyDate);SELECT @@IDENTITY ");
			this.database.AddInParameter(sqlStringCommand, "leaveId", System.Data.DbType.Int64, leaveReply.LeaveId);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, leaveReply.UserId);
			this.database.AddInParameter(sqlStringCommand, "ReplyContent", System.Data.DbType.String, leaveReply.ReplyContent);
			this.database.AddInParameter(sqlStringCommand, "ReplyDate", System.Data.DbType.String, DataHelper.GetSafeDateTimeFormat(leaveReply.ReplyDate));
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
		public override bool DeleteLeaveCommentReply(long leaveReplyId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_LeaveCommentReplys WHERE replyId=@replyId;");
			this.database.AddInParameter(sqlStringCommand, "replyId", System.Data.DbType.Int64, leaveReplyId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override System.Data.DataTable GetReplyLeaveComments(long leaveId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_LeaveCommentReplys WHERE LeaveId=@LeaveId");
			this.database.AddInParameter(sqlStringCommand, "LeaveId", System.Data.DbType.Int64, leaveId);
			System.Data.DataTable result = new System.Data.DataTable();
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override DbQueryResult GetManagerReceivedMessages(MessageBoxQuery query, UserRole role)
		{
			string text = string.Format("Accepter='{0}' AND Sernder IN (SELECT UserName FROM aspnet_Users WHERE UserRole = {1})", query.Accepter, (int)role);
			if (query.MessageStatus == MessageStatus.Replied)
			{
				text += " AND IsRead = 1";
			}
			if (query.MessageStatus == MessageStatus.NoReply)
			{
				text += " AND IsRead = 0";
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, "MessageId", SortAction.Desc, query.IsCount, "vw_Hishop_ManagerMessageBox", "MessageId", text, "*");
		}
		public override DbQueryResult GetManagerSendedMessages(MessageBoxQuery query, UserRole role)
		{
			string filter = string.Format("Sernder='{0}' AND Accepter IN (SELECT UserName FROM aspnet_Users WHERE UserRole = {1})", query.Sernder, (int)role);
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, "MessageId", SortAction.Desc, query.IsCount, "vw_Hishop_ManagerMessageBox", "MessageId", filter, "*");
		}
		public override MessageBoxInfo GetManagerMessage(long messageId)
		{
			MessageBoxInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vw_Hishop_ManagerMessageBox WHERE MessageId=@MessageId;");
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
		public override bool InsertMessage(MessageBoxInfo messageBoxInfo, UserRole toRole)
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
			stringBuilder.Append("INSERT INTO [Hishop_ManagerMessageBox]([ContentId],[Sernder],[Accepter],[IsRead]) ");
			stringBuilder.Append("VALUES(@ContentId,@Sernder ,@Accepter,@IsRead) ");
			stringBuilder.Append("SET @errorSun=@errorSun+@@ERROR  ");
			stringBuilder.AppendFormat("INSERT INTO [{0}]([ContentId],[Sernder],[Accepter],[IsRead]) ", (toRole == UserRole.Distributor) ? "Hishop_DistributorMessageBox" : "Hishop_MemberMessageBox");
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
		public override bool AddMessage(MessageBoxInfo messageBoxInfo, UserRole toRole)
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
			stringBuilder.AppendFormat("INSERT INTO [{0}]([ContentId],[Sernder],[Accepter],[IsRead]) ", (toRole == UserRole.Distributor) ? "Hishop_DistributorMessageBox" : "Hishop_MemberMessageBox");
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
		public override bool PostManagerMessageIsRead(long messageId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_ManagerMessageBox set IsRead=1 where MessageId=@MessageId");
			this.database.AddInParameter(sqlStringCommand, "MessageId", System.Data.DbType.Int64, messageId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override int DeleteManagerMessages(IList<long> messageList)
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("delete from Hishop_ManagerMessageBox where MessageId in ({0}) ", text));
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override int GetMemberUnReadMessageNum()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ISNULL(COUNT(*),0) FROM Hishop_MemberMessageBox WHERE IsRead=0 and Accepter=@Accepter");
			this.database.AddInParameter(sqlStringCommand, "Accepter", System.Data.DbType.String, HiContext.Current.User.Username);
			return (int)this.database.ExecuteScalar(sqlStringCommand);
		}
		public override IList<Distributor> GetDistributorsByRank(int? gradeId)
		{
			IList<Distributor> list = new List<Distributor>();
			System.Data.Common.DbCommand sqlStringCommand;
			if (gradeId > 0)
			{
				sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vw_aspnet_Distributors WHERE GradeId=@GradeId");
				this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, gradeId);
			}
			else
			{
				sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vw_aspnet_Distributors");
			}
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(new Distributor
					{
						UserId = (int)dataReader["UserId"],
						Email = dataReader["Email"].ToString(),
						Username = dataReader["UserName"].ToString()
					});
				}
			}
			return list;
		}
		public override IList<Member> GetMembersByRank(int? gradeId)
		{
			IList<Member> list = new List<Member>();
			System.Data.Common.DbCommand sqlStringCommand;
			if (gradeId > 0)
			{
				sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vw_aspnet_Members WHERE GradeId=@GradeId");
				this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, gradeId);
			}
			else
			{
				sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vw_aspnet_Members");
			}
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(new Member(UserRole.Member)
					{
						UserId = (int)dataReader["UserId"],
						Email = dataReader["Email"].ToString(),
						Username = dataReader["UserName"].ToString()
					});
				}
			}
			return list;
		}
		public override DbQueryResult GetConsultationProducts(ProductConsultationAndReplyQuery consultationQuery)
		{
			DbQueryResult dbQueryResult = new DbQueryResult();
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ProductConsultation_Get");
			this.database.AddInParameter(storedProcCommand, "PageIndex", System.Data.DbType.Int32, consultationQuery.PageIndex);
			this.database.AddInParameter(storedProcCommand, "PageSize", System.Data.DbType.Int32, consultationQuery.PageSize);
			this.database.AddInParameter(storedProcCommand, "IsCount", System.Data.DbType.Boolean, consultationQuery.IsCount);
			if (consultationQuery.CategoryId.HasValue)
			{
				this.database.AddInParameter(storedProcCommand, "CategoryId", System.Data.DbType.Int32, consultationQuery.CategoryId.Value);
			}
			this.database.AddInParameter(storedProcCommand, "SqlPopulate", System.Data.DbType.String, CommentData.BuildProductConsultationQuery(consultationQuery));
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
			{
				dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (consultationQuery.IsCount && dataReader.NextResult())
				{
					dataReader.Read();
					dbQueryResult.TotalRecords = dataReader.GetInt32(0);
				}
			}
			return dbQueryResult;
		}
		public override bool ReplyProductConsultation(ProductConsultationInfo productConsultation)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_ProductConsultations SET ReplyText = @ReplyText, ReplyDate = @ReplyDate, ReplyUserId = @ReplyUserId WHERE ConsultationId = @ConsultationId");
			this.database.AddInParameter(sqlStringCommand, "ReplyText", System.Data.DbType.String, productConsultation.ReplyText);
			this.database.AddInParameter(sqlStringCommand, "ReplyDate", System.Data.DbType.DateTime, productConsultation.ReplyDate);
			this.database.AddInParameter(sqlStringCommand, "ReplyUserId", System.Data.DbType.Int32, productConsultation.ReplyUserId);
			this.database.AddInParameter(sqlStringCommand, "ConsultationId", System.Data.DbType.Int32, productConsultation.ConsultationId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override int DeleteProductConsultation(int consultationId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ProductConsultations WHERE consultationId = @consultationId");
			this.database.AddInParameter(sqlStringCommand, "ConsultationId", System.Data.DbType.Int64, consultationId);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override ProductConsultationInfo GetProductConsultation(int consultationId)
		{
			ProductConsultationInfo result = null;
			string text = "SELECT * FROM Hishop_ProductConsultations WHERE ConsultationId=@ConsultationId";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			this.database.AddInParameter(sqlStringCommand, "ConsultationId", System.Data.DbType.Int32, consultationId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateProductConsultation(dataReader);
				}
			}
			return result;
		}
		public override System.Data.DataSet GetProductReviews(out int total, ProductReviewQuery reviewQuery)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ProductReviews_Get");
			this.database.AddInParameter(storedProcCommand, "PageIndex", System.Data.DbType.Int32, reviewQuery.PageIndex);
			this.database.AddInParameter(storedProcCommand, "PageSize", System.Data.DbType.Int32, reviewQuery.PageSize);
			this.database.AddInParameter(storedProcCommand, "IsCount", System.Data.DbType.Boolean, reviewQuery.IsCount);
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", System.Data.DbType.String, CommentData.BuildReviewsQuery(reviewQuery));
			this.database.AddOutParameter(storedProcCommand, "Total", System.Data.DbType.Int32, 4);
			if (reviewQuery.CategoryId.HasValue)
			{
				this.database.AddInParameter(storedProcCommand, "CategoryId", System.Data.DbType.Int32, reviewQuery.CategoryId.Value);
			}
			System.Data.DataSet result = this.database.ExecuteDataSet(storedProcCommand);
			total = (int)this.database.GetParameterValue(storedProcCommand, "Total");
			return result;
		}
		public override ProductReviewInfo GetProductReview(int reviewId)
		{
			ProductReviewInfo result = new ProductReviewInfo();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ProductReviews WHERE ReviewId=@ReviewId");
			this.database.AddInParameter(sqlStringCommand, "ReviewId", System.Data.DbType.Int32, reviewId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateProductReview(dataReader);
				}
			}
			return result;
		}
		public override int DeleteProductReview(long reviewId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ProductReviews WHERE ReviewId = @ReviewId");
			this.database.AddInParameter(sqlStringCommand, "ReviewId", System.Data.DbType.Int64, reviewId);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override int DeleteReview(IList<int> reviews)
		{
			string text = string.Empty;
			using (IEnumerator<int> enumerator = reviews.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					long num = (long)enumerator.Current;
					if (string.IsNullOrEmpty(text))
					{
						text = num.ToString();
					}
					else
					{
						text = text + "," + num.ToString();
					}
				}
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("DELETE FROM Hishop_ProductReviews WHERE ReviewId in ({0})", text));
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}
		private static string BuildProductConsultationQuery(ProductConsultationAndReplyQuery consultationQuery)
		{
			HiContext arg_05_0 = HiContext.Current;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT c.ConsultationId FROM Hishop_Products p inner join Hishop_ProductConsultations c on p.productId=c.ProductId WHERE 0 = 0");
			if (consultationQuery.Type == ConsultationReplyType.NoReply)
			{
				stringBuilder.Append(" AND c.ReplyUserId IS NULL ");
			}
			else
			{
				if (consultationQuery.Type == ConsultationReplyType.Replyed)
				{
					stringBuilder.Append(" AND c.ReplyUserId IS NOT NULL");
				}
			}
			string result;
			if (consultationQuery.ProductId > 0)
			{
				stringBuilder.AppendFormat(" AND p.ProductId = {0}", consultationQuery.ProductId);
				result = stringBuilder.ToString();
			}
			else
			{
				if (!string.IsNullOrEmpty(consultationQuery.ProductCode))
				{
					stringBuilder.AppendFormat(" AND p.ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(consultationQuery.ProductCode));
				}
				if (!string.IsNullOrEmpty(consultationQuery.Keywords))
				{
					stringBuilder.AppendFormat(" AND p.ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(consultationQuery.Keywords));
				}
				if (consultationQuery.CategoryId.HasValue)
				{
					stringBuilder.AppendFormat(" AND (p.CategoryId = {0}", consultationQuery.CategoryId.Value);
					stringBuilder.AppendFormat(" OR p.CategoryId IN (SELECT CategoryId FROM Hishop_Categories WHERE Path LIKE (SELECT Path FROM Hishop_Categories WHERE CategoryId = {0}) + '%'))", consultationQuery.CategoryId.Value);
				}
				if (!string.IsNullOrEmpty(consultationQuery.SortBy))
				{
					stringBuilder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(consultationQuery.SortBy), consultationQuery.SortOrder.ToString());
				}
				result = stringBuilder.ToString();
			}
			return result;
		}
		private static string BuildReviewsQuery(ProductReviewQuery reviewQuery)
		{
			HiContext arg_05_0 = HiContext.Current;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT r.ReviewId FROM Hishop_Products p inner join Hishop_ProductReviews r on r.productId=p.ProductId WHERE 0 = 0");
			if (!string.IsNullOrEmpty(reviewQuery.ProductCode))
			{
				stringBuilder.AppendFormat(" AND ProductCode LIKE '%{0}%'", DataHelper.CleanSearchString(reviewQuery.ProductCode));
			}
			if (!string.IsNullOrEmpty(reviewQuery.Keywords))
			{
				stringBuilder.AppendFormat(" AND p.ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(reviewQuery.Keywords));
			}
			if (reviewQuery.CategoryId.HasValue)
			{
				stringBuilder.AppendFormat(" AND (p.CategoryId = {0}", reviewQuery.CategoryId.Value);
				stringBuilder.AppendFormat(" OR  p.CategoryId IN (SELECT CategoryId FROM Hishop_Categories WHERE Path LIKE (SELECT Path FROM Hishop_Categories WHERE CategoryId = {0}) + '%'))", reviewQuery.CategoryId.Value);
			}
			if (!string.IsNullOrEmpty(reviewQuery.SortBy))
			{
				stringBuilder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(reviewQuery.SortBy), reviewQuery.SortOrder.ToString());
			}
			return stringBuilder.ToString();
		}
		private static string BuildUserReviewsAndReplysQuery(UserProductReviewAndReplyQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" SELECT ProductId FROM Hishop_ProductReviews ");
			stringBuilder.AppendFormat(" AND ProductId IN (SELECT ProductId FROM Hishop_Products)", new object[0]);
			stringBuilder.Append(" GROUP BY ProductId");
			return stringBuilder.ToString();
		}
		private static string BuildConsultationAndReplyQuery(ProductConsultationAndReplyQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" SELECT ConsultationId FROM Hishop_ProductConsultations ");
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
				stringBuilder.Append(" AND ReplyUserId IS NULL");
			}
			else
			{
				if (query.Type == ConsultationReplyType.Replyed)
				{
					stringBuilder.Append(" AND ReplyUserId IS NOT NULL");
				}
			}
			stringBuilder.Append(" ORDER BY replydate DESC");
			return stringBuilder.ToString();
		}
		private static string BuildLeaveCommentQuery(LeaveCommentQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" SELECT l.LeaveId FROM Hishop_LeaveComments l where 0=0");
			if (query.MessageStatus == MessageStatus.Replied)
			{
				stringBuilder.Append(" and (select Count(ReplyId) from Hishop_LeaveCommentReplys where LeaveId=l.LeaveId) >0 ");
			}
			if (query.MessageStatus == MessageStatus.NoReply)
			{
				stringBuilder.Append(" and (select Count(ReplyId) from Hishop_LeaveCommentReplys where LeaveId=l.LeaveId) <=0 ");
			}
			if (!string.IsNullOrEmpty(query.SortBy))
			{
				stringBuilder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
			}
			else
			{
				stringBuilder.Append(" ORDER BY LastDate desc");
			}
			return stringBuilder.ToString();
		}
	}
}
