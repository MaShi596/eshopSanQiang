using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Comments;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core.Enums;
using Hidistro.Subsites.Comments;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
namespace Hidistro.Subsites.Data
{
	public class CommentData : SubsiteCommentsProvider
	{
		private Database database;
		public CommentData()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public override int DeleteAffiches(List<int> afficheIds)
		{
			int num = 0;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_Affiche WHERE AfficheId=@AfficheId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "AfficheId", System.Data.DbType.Int32);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO distro_Affiche(DistributorUserId,Title, Content, AddedDate) VALUES (@DistributorUserId,@Title, @Content, @AddedDate)");
			this.database.AddInParameter(sqlStringCommand, "Title", System.Data.DbType.String, affiche.Title);
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, affiche.Content);
			this.database.AddInParameter(sqlStringCommand, "AddedDate", System.Data.DbType.DateTime, affiche.AddedDate);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override bool UpdateAffiche(AfficheInfo affiche)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Affiche SET Title = @Title, AddedDate = @AddedDate, Content = @Content WHERE AfficheId = @AfficheId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "Title", System.Data.DbType.String, affiche.Title);
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, affiche.Content);
			this.database.AddInParameter(sqlStringCommand, "AddedDate", System.Data.DbType.DateTime, affiche.AddedDate);
			this.database.AddInParameter(sqlStringCommand, "AfficheId", System.Data.DbType.Int32, affiche.AfficheId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override bool DeleteAffiche(int afficheId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_Affiche WHERE AfficheId = @AfficheId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "AfficheId", System.Data.DbType.Int32, afficheId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override AfficheInfo GetAffiche(int afficheId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_Affiche WHERE AfficheId = @AfficheId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "AfficheId", System.Data.DbType.Int32, afficheId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_Affiche WHERE DistributorUserId=@DistributorUserId ORDER BY AddedDate DESC");
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
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
		public override bool AddArticle(ArticleInfo article)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO distro_Articles(CategoryId,DistributorUserId, Title, Meta_Description, Meta_Keywords, IconUrl, Description, Content, AddedDate,IsRelease) VALUES (@CategoryId, @DistributorUserId, @Title, @Meta_Description, @Meta_Keywords, @IconUrl, @Description, @Content, @AddedDate,@IsRelease)");
			this.database.AddInParameter(sqlStringCommand, "CategoryId", System.Data.DbType.Int32, article.CategoryId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Articles SET CategoryId = @CategoryId,AddedDate = @AddedDate,Title = @Title, Meta_Description = @Meta_Description, Meta_Keywords = @Meta_Keywords,  IconUrl=@IconUrl,Description = @Description,Content = @Content,IsRelease=@IsRelease WHERE ArticleId = @ArticleId AND DistributorUserId = @DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "CategoryId", System.Data.DbType.Int32, article.CategoryId);
			this.database.AddInParameter(sqlStringCommand, "Title", System.Data.DbType.String, article.Title);
			this.database.AddInParameter(sqlStringCommand, "Meta_Description", System.Data.DbType.String, article.MetaDescription);
			this.database.AddInParameter(sqlStringCommand, "Meta_Keywords", System.Data.DbType.String, article.MetaKeywords);
			this.database.AddInParameter(sqlStringCommand, "IconUrl", System.Data.DbType.String, article.IconUrl);
			this.database.AddInParameter(sqlStringCommand, "Description", System.Data.DbType.String, article.Description);
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, article.Content);
			this.database.AddInParameter(sqlStringCommand, "AddedDate", System.Data.DbType.DateTime, article.AddedDate);
			this.database.AddInParameter(sqlStringCommand, "IsRelease", System.Data.DbType.Boolean, article.IsRelease);
			this.database.AddInParameter(sqlStringCommand, "ArticleId", System.Data.DbType.Int32, article.ArticleId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override bool DeleteArticle(int articleId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_Articles WHERE ArticleId = @ArticleId AND DistributorUserId = @DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "ArticleId", System.Data.DbType.Int32, articleId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override void SwapArticleCategorySequence(int categoryId, int replaceCategoryId, int displaySequence, int replaceDisplaySequence)
		{
			DataHelper.SwapSequence("distro_ArticleCategories", "CategoryId", "DisplaySequence", categoryId, replaceCategoryId, displaySequence, replaceDisplaySequence);
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
				System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("sub_ArticleCategory_CreateUpdateDelete");
				this.database.AddInParameter(storedProcCommand, "Action", System.Data.DbType.Int32, (int)action);
				this.database.AddOutParameter(storedProcCommand, "Status", System.Data.DbType.Int32, 4);
				if (action != DataProviderAction.Create)
				{
					this.database.AddInParameter(storedProcCommand, "CategoryId", System.Data.DbType.Int32, articleCategory.CategoryId);
				}
				this.database.AddInParameter(storedProcCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_Articles WHERE ArticleId = @ArticleId AND DistributorUserId = @DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "ArticleId", System.Data.DbType.Int32);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_Articles WHERE ArticleId = @ArticleId AND DistributorUserId = @DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "ArticleId", System.Data.DbType.Int32, articleId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_ArticleCategories WHERE CategoryId = @CategoryId AND DistributorUserId=@DistributorUserId ORDER BY [DisplaySequence]");
			this.database.AddInParameter(sqlStringCommand, "CategoryId", System.Data.DbType.Int32, categoryId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
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
			stringBuilder.AppendFormat("AND DistributorUserId = {0}", HiContext.Current.User.UserId);
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
			return DataHelper.PagingByRownumber(articleQuery.PageIndex, articleQuery.PageSize, articleQuery.SortBy, articleQuery.SortOrder, articleQuery.IsCount, "vw_distro_Articles", "ArticleId", stringBuilder.ToString(), "*");
		}
		public override IList<ArticleCategoryInfo> GetMainArticleCategories()
		{
			IList<ArticleCategoryInfo> list = new List<ArticleCategoryInfo>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * From distro_ArticleCategories WHERE DistributorUserId=@DistributorUserId ORDER BY [DisplaySequence]");
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
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
		public override bool UpdateMyArticRelease(int articlId, bool isrelease)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update distro_Articles set IsRelease=@IsRelease WHERE ArticleId = @ArticleId AND DistributorUserId = @DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "IsRelease", System.Data.DbType.Boolean, isrelease);
			this.database.AddInParameter(sqlStringCommand, "ArticleId", System.Data.DbType.Int32, articlId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
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
				System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("sub_HelpCategory_CreateUpdateDelete");
				this.database.AddInParameter(storedProcCommand, "Action", System.Data.DbType.Int32, (int)action);
				this.database.AddOutParameter(storedProcCommand, "Status", System.Data.DbType.Int32, 4);
				if (action != DataProviderAction.Create)
				{
					this.database.AddInParameter(storedProcCommand, "CategoryId", System.Data.DbType.Int32, helpCategory.CategoryId);
				}
				this.database.AddInParameter(storedProcCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
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
		public override bool AddHelp(HelpInfo help)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO distro_Helps(CategoryId, DistributorUserId, Title, Meta_Description, Meta_Keywords, Description, Content, AddedDate, IsShowFooter) VALUES (@CategoryId, @DistributorUserId, @Title, @Meta_Description, @Meta_Keywords, @Description, @Content, @AddedDate, @IsShowFooter)");
			this.database.AddInParameter(sqlStringCommand, "CategoryId", System.Data.DbType.Int32, help.CategoryId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Helps SET CategoryId = @CategoryId, AddedDate = @AddedDate, Title = @Title, Meta_Description = @Meta_Description, Meta_Keywords = @Meta_Keywords,  Description = @Description, Content = @Content, IsShowFooter = @IsShowFooter WHERE HelpId = @HelpId AND DistributorUserId = @DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "CategoryId", System.Data.DbType.Int32, help.CategoryId);
			this.database.AddInParameter(sqlStringCommand, "Title", System.Data.DbType.String, help.Title);
			this.database.AddInParameter(sqlStringCommand, "Meta_Description", System.Data.DbType.String, help.MetaDescription);
			this.database.AddInParameter(sqlStringCommand, "Meta_Keywords", System.Data.DbType.String, help.MetaKeywords);
			this.database.AddInParameter(sqlStringCommand, "Description", System.Data.DbType.String, help.Description);
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, help.Content);
			this.database.AddInParameter(sqlStringCommand, "AddedDate", System.Data.DbType.DateTime, help.AddedDate);
			this.database.AddInParameter(sqlStringCommand, "IsShowFooter", System.Data.DbType.Boolean, help.IsShowFooter);
			this.database.AddInParameter(sqlStringCommand, "HelpId", System.Data.DbType.Int32, help.HelpId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override bool DeleteHelp(int helpId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_Helps WHERE HelpId = @HelpId AND DistributorUserId = @DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "HelpId", System.Data.DbType.Int32, helpId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override int DeleteHelps(IList<int> helps)
		{
			int num = 0;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_Helps WHERE HelpId=@HelpId AND DistributorUserId = @DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_Helps WHERE HelpId=@HelpId AND DistributorUserId = @DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
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
			DataHelper.SwapSequence("distro_HelpCategories", "CategoryId", "DisplaySequence", categoryId, replaceCategoryId, displaySequence, replaceDisplaySequence);
		}
		public override HelpCategoryInfo GetHelpCategory(int categoryId)
		{
			HelpCategoryInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_HelpCategories WHERE CategoryId=@CategoryId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "CategoryId", System.Data.DbType.Int32, categoryId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_HelpCategories WHERE DistributorUserId=@DistributorUserId ORDER BY DisplaySequence");
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
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
			stringBuilder.AppendFormat("AND DistributorUserId = {0}", HiContext.Current.User.UserId);
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
			return DataHelper.PagingByTopnotin(helpQuery.PageIndex, helpQuery.PageSize, helpQuery.SortBy, helpQuery.SortOrder, helpQuery.IsCount, "vw_distro_Helps", "HelpId", stringBuilder.ToString(), "*");
		}
		public override DbQueryResult GetReceivedMessages(MessageBoxQuery query, UserRole role)
		{
			string text = string.Format("Accepter='{0}' AND Sernder", query.Accepter);
			text += ((role == UserRole.SiteManager) ? " = 'admin'" : "<> 'admin'");
			if (query.MessageStatus == MessageStatus.Replied)
			{
				text += " AND IsRead = 1";
			}
			if (query.MessageStatus == MessageStatus.NoReply)
			{
				text += " AND IsRead = 0";
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, "MessageId", SortAction.Desc, query.IsCount, "vw_Hishop_DistributorMessageBox", "MessageId", text, "*");
		}
		public override DbQueryResult GetSendedMessages(MessageBoxQuery query, UserRole role)
		{
			string text = string.Format("Sernder='{0}' AND Accepter", query.Sernder);
			text += ((role == UserRole.SiteManager) ? " = 'admin'" : "<> 'admin'");
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, "MessageId", SortAction.Desc, query.IsCount, "vw_Hishop_DistributorMessageBox", "MessageId", text, "*");
		}
		public override MessageBoxInfo GetMessage(long messageId)
		{
			MessageBoxInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vw_Hishop_DistributorMessageBox WHERE MessageId=@MessageId;");
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
			stringBuilder.Append("INSERT INTO [Hishop_DistributorMessageBox]([ContentId],[Sernder],[Accepter],[IsRead]) ");
			stringBuilder.Append("VALUES(@ContentId,@Sernder ,@Accepter,@IsRead) ");
			stringBuilder.Append("SET @errorSun=@errorSun+@@ERROR  ");
			stringBuilder.AppendFormat("INSERT INTO [{0}]([ContentId],[Sernder],[Accepter],[IsRead]) ", (toRole == UserRole.SiteManager) ? "Hishop_ManagerMessageBox" : "Hishop_MemberMessageBox");
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
		public override bool PostMessageIsRead(long messageId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_DistributorMessageBox set IsRead=1 where MessageId=@MessageId");
			this.database.AddInParameter(sqlStringCommand, "MessageId", System.Data.DbType.Int64, messageId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override int DeleteMessages(IList<long> messageList)
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("delete from Hishop_DistributorMessageBox where MessageId in ({0}) ", text));
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override int GetIsReadMessageToAdmin()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ISNULL(COUNT(*),0) FROM Hishop_DistributorMessageBox WHERE IsRead=0 AND Sernder = 'admin' AND Accepter=@Accepter");
			this.database.AddInParameter(sqlStringCommand, "Accepter", System.Data.DbType.String, HiContext.Current.User.Username);
			return (int)this.database.ExecuteScalar(sqlStringCommand);
		}
		public override DbQueryResult GetConsultationProducts(ProductConsultationAndReplyQuery consultationQuery)
		{
			DbQueryResult dbQueryResult = new DbQueryResult();
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("sub_ProductConsultation_Get");
			this.database.AddInParameter(storedProcCommand, "PageIndex", System.Data.DbType.Int32, consultationQuery.PageIndex);
			this.database.AddInParameter(storedProcCommand, "PageSize", System.Data.DbType.Int32, consultationQuery.PageSize);
			this.database.AddInParameter(storedProcCommand, "IsCount", System.Data.DbType.Boolean, consultationQuery.IsCount);
			if (consultationQuery.CategoryId.HasValue)
			{
				this.database.AddInParameter(storedProcCommand, "CategoryId", System.Data.DbType.Int32, consultationQuery.CategoryId.Value);
			}
			string text = CommentData.BuildProductConsultationQuery(consultationQuery);
			this.database.AddInParameter(storedProcCommand, "SqlPopulate", System.Data.DbType.String, text);
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_ProductConsultations SET ReplyText = @ReplyText, ReplyDate = @ReplyDate, ReplyUserId = @ReplyUserId WHERE ConsultationId = @ConsultationId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "ReplyText", System.Data.DbType.String, productConsultation.ReplyText);
			this.database.AddInParameter(sqlStringCommand, "ReplyDate", System.Data.DbType.DateTime, productConsultation.ReplyDate);
			this.database.AddInParameter(sqlStringCommand, "ReplyUserId", System.Data.DbType.Int32, productConsultation.ReplyUserId);
			this.database.AddInParameter(sqlStringCommand, "ConsultationId", System.Data.DbType.Int32, productConsultation.ConsultationId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override int DeleteProductConsultation(int consultationId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_ProductConsultations WHERE consultationId = @consultationId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "ConsultationId", System.Data.DbType.Int64, consultationId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override ProductConsultationInfo GetProductConsultation(int consultationId)
		{
			ProductConsultationInfo result = null;
			string text = "SELECT * FROM distro_ProductConsultations WHERE ConsultationId=@ConsultationId AND DistributorUserId=@DistributorUserId";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			this.database.AddInParameter(sqlStringCommand, "ConsultationId", System.Data.DbType.Int32, consultationId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
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
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("sub_ProductReviews_Get");
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_ProductReviews WHERE ReviewId=@ReviewId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "ReviewId", System.Data.DbType.Int32, reviewId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_ProductReviews WHERE ReviewId = @ReviewId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "ReviewId", System.Data.DbType.Int64, reviewId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("DELETE FROM distro_ProductReviews WHERE ReviewId in ({0})", text));
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override LeaveCommentInfo GetLeaveComment(long leaveId)
		{
			LeaveCommentInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_LeaveComments WHERE LeaveId=@LeaveId AND DistributorUserId=@DistributorUserId;");
			this.database.AddInParameter(sqlStringCommand, "LeaveId", System.Data.DbType.Int64, leaveId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
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
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("sub_LeaveComments_Get");
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_LeaveCommentReplys WHERE LeaveId=@LeaveId;DELETE FROM distro_LeaveComments WHERE LeaveId=@LeaveId");
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("DELETE FROM distro_LeaveCommentReplys WHERE LeaveId in ({0}) ;DELETE FROM distro_LeaveComments WHERE LeaveId in ({1}) AND DistributorUserId={2} ", text, text, HiContext.Current.User.UserId));
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override int ReplyLeaveComment(LeaveCommentReplyInfo leaveReply)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO distro_LeaveCommentReplys(LeaveId,UserId,ReplyContent,ReplyDate) VALUES(@LeaveId,@UserId,@ReplyContent,@ReplyDate); SELECT @@IDENTITY ");
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_LeaveCommentReplys WHERE replyId=@replyId;");
			this.database.AddInParameter(sqlStringCommand, "replyId", System.Data.DbType.Int64, leaveReplyId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override System.Data.DataTable GetReplyLeaveComments(long leaveId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_LeaveCommentReplys WHERE LeaveId=@LeaveId ");
			this.database.AddInParameter(sqlStringCommand, "LeaveId", System.Data.DbType.Int64, leaveId);
			System.Data.DataTable result = new System.Data.DataTable();
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		private static string BuildProductConsultationQuery(ProductConsultationAndReplyQuery consultationQuery)
		{
			HiContext arg_05_0 = HiContext.Current;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT c.ConsultationId FROM distro_Products p inner join distro_ProductConsultations c on p.productId=c.ProductId AND p.DistributorUserId=c.DistributorUserId WHERE c.DistributorUserId ={0}", HiContext.Current.User.UserId);
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
					stringBuilder.AppendFormat(" AND LOWER(p.ProductCode) LIKE '%{0}%'", DataHelper.CleanSearchString(consultationQuery.ProductCode));
				}
				if (!string.IsNullOrEmpty(consultationQuery.Keywords))
				{
					stringBuilder.AppendFormat(" AND p.ProductName LIKE '%{0}%'", DataHelper.CleanSearchString(consultationQuery.Keywords));
				}
				if (consultationQuery.CategoryId.HasValue)
				{
					stringBuilder.AppendFormat(" AND (p.CategoryId = {0}", consultationQuery.CategoryId.Value);
					stringBuilder.AppendFormat(" OR p.CategoryId IN (SELECT CategoryId FROM distro_Categories WHERE Path LIKE (SELECT Path FROM distro_Categories WHERE CategoryId = {0} AND distro_Categories.DistributorUserId={1}) + '%' AND  distro_Categories.DistributorUserId={1}))", consultationQuery.CategoryId.Value, HiContext.Current.User.UserId);
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
			stringBuilder.Append("SELECT ReviewId FROM distro_Products p inner join distro_ProductReviews r on (r.productId=p.ProductId  AND r.DistributorUserId=p.DistributorUserId)");
			stringBuilder.AppendFormat(" WHERE r.DistributorUserId ={0}", HiContext.Current.User.UserId);
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
				stringBuilder.AppendFormat(" OR  p.CategoryId IN (SELECT CategoryId FROM distro_Categories WHERE Path LIKE (SELECT Path FROM distro_Categories WHERE CategoryId = {0} AND DistributorUserId={1}) + '%'  AND DistributorUserId={1}))", reviewQuery.CategoryId.Value, HiContext.Current.User.UserId);
			}
			if (!string.IsNullOrEmpty(reviewQuery.SortBy))
			{
				stringBuilder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(reviewQuery.SortBy), reviewQuery.SortOrder.ToString());
			}
			return stringBuilder.ToString();
		}
		private static string BuildLeaveCommentQuery(LeaveCommentQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" SELECT l.LeaveId FROM distro_LeaveComments l where l.DistributorUserId={0}", HiContext.Current.User.UserId);
			if (query.MessageStatus == MessageStatus.Replied)
			{
				stringBuilder.Append(" and (select Count(ReplyId) from distro_LeaveCommentReplys where LeaveId=l.LeaveId) >0 ");
			}
			if (query.MessageStatus == MessageStatus.NoReply)
			{
				stringBuilder.Append(" and (select Count(ReplyId) from distro_LeaveCommentReplys where LeaveId=l.LeaveId) <=0 ");
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
