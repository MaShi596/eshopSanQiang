using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Comments;
using Hidistro.Membership.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
namespace Hidistro.ControlPanel.Comments
{
	public static class ArticleHelper
	{
		public static ArticleCategoryInfo GetArticleCategory(int categoryId)
		{
			return CommentsProvider.Instance().GetArticleCategory(categoryId);
		}
		public static IList<ArticleCategoryInfo> GetMainArticleCategories()
		{
			return CommentsProvider.Instance().GetMainArticleCategories();
		}
		public static ArticleInfo GetArticle(int articleId)
		{
			return CommentsProvider.Instance().GetArticle(articleId);
		}
		public static DbQueryResult GetArticleList(ArticleQuery articleQuery)
		{
			return CommentsProvider.Instance().GetArticleList(articleQuery);
		}
		public static bool CreateArticleCategory(ArticleCategoryInfo articleCategory)
		{
			bool result;
			if (null == articleCategory)
			{
				result = false;
			}
			else
			{
				Globals.EntityCoding(articleCategory, true);
				result = CommentsProvider.Instance().CreateUpdateDeleteArticleCategory(articleCategory, DataProviderAction.Create);
			}
			return result;
		}
		public static bool UpdateArticleCategory(ArticleCategoryInfo articleCategory)
		{
			bool result;
			if (null == articleCategory)
			{
				result = false;
			}
			else
			{
				Globals.EntityCoding(articleCategory, true);
				result = CommentsProvider.Instance().CreateUpdateDeleteArticleCategory(articleCategory, DataProviderAction.Update);
			}
			return result;
		}
		public static bool DeleteArticleCategory(int categoryId)
		{
			ArticleCategoryInfo articleCategoryInfo = new ArticleCategoryInfo();
			articleCategoryInfo.CategoryId = categoryId;
			return CommentsProvider.Instance().CreateUpdateDeleteArticleCategory(articleCategoryInfo, DataProviderAction.Delete);
		}
		public static int DeleteArticles(IList<int> articles)
		{
			int result;
			if (articles == null || articles.Count == 0)
			{
				result = 0;
			}
			else
			{
				result = CommentsProvider.Instance().DeleteArticles(articles);
			}
			return result;
		}
		public static bool CreateArticle(ArticleInfo article)
		{
			bool result;
			if (null == article)
			{
				result = false;
			}
			else
			{
				Globals.EntityCoding(article, true);
				result = CommentsProvider.Instance().AddArticle(article);
			}
			return result;
		}
		public static bool UpdateArticle(ArticleInfo article)
		{
			bool result;
			if (null == article)
			{
				result = false;
			}
			else
			{
				Globals.EntityCoding(article, true);
				result = CommentsProvider.Instance().UpdateArticle(article);
			}
			return result;
		}
		public static bool DeleteArticle(int articleId)
		{
			return CommentsProvider.Instance().DeleteArticle(articleId);
		}
		public static void SwapArticleCategorySequence(int categoryId, int replaceCategoryId, int displaySequence, int replaceDisplaySequence)
		{
			CommentsProvider.Instance().SwapArticleCategorySequence(categoryId, replaceCategoryId, displaySequence, replaceDisplaySequence);
		}
		public static DbQueryResult GetRelatedArticsProducts(Pagination page, int articId)
		{
			return CommentsProvider.Instance().GetRelatedArticsProducts(page, articId);
		}
		public static bool AddReleatesProdcutByArticId(int articId, int productId)
		{
			return CommentsProvider.Instance().AddReleatesProdcutByArticId(articId, productId);
		}
		public static bool RemoveReleatesProductByArticId(int articId, int productId)
		{
			return CommentsProvider.Instance().RemoveReleatesProductByArticId(articId, productId);
		}
		public static bool RemoveReleatesProductByArticId(int articId)
		{
			return CommentsProvider.Instance().RemoveReleatesProductByArticId(articId);
		}
		public static bool UpdateRelease(int articId, bool release)
		{
			return CommentsProvider.Instance().UpdateRelease(articId, release);
		}
		public static HelpCategoryInfo GetHelpCategory(int categoryId)
		{
			return CommentsProvider.Instance().GetHelpCategory(categoryId);
		}
		public static IList<HelpCategoryInfo> GetHelpCategorys()
		{
			return CommentsProvider.Instance().GetHelpCategorys();
		}
		public static DbQueryResult GetHelpList(HelpQuery helpQuery)
		{
			return CommentsProvider.Instance().GetHelpList(helpQuery);
		}
		public static HelpInfo GetHelp(int helpId)
		{
			return CommentsProvider.Instance().GetHelp(helpId);
		}
		public static bool CreateHelpCategory(HelpCategoryInfo helpCategory)
		{
			bool result;
			if (null == helpCategory)
			{
				result = false;
			}
			else
			{
				Globals.EntityCoding(helpCategory, true);
				result = CommentsProvider.Instance().CreateUpdateDeleteHelpCategory(helpCategory, DataProviderAction.Create);
			}
			return result;
		}
		public static bool UpdateHelpCategory(HelpCategoryInfo helpCategory)
		{
			bool result;
			if (null == helpCategory)
			{
				result = false;
			}
			else
			{
				Globals.EntityCoding(helpCategory, true);
				result = CommentsProvider.Instance().CreateUpdateDeleteHelpCategory(helpCategory, DataProviderAction.Update);
			}
			return result;
		}
		public static bool DeleteHelpCategory(int categoryId)
		{
			HelpCategoryInfo helpCategoryInfo = new HelpCategoryInfo();
			helpCategoryInfo.CategoryId = new int?(categoryId);
			return CommentsProvider.Instance().CreateUpdateDeleteHelpCategory(helpCategoryInfo, DataProviderAction.Delete);
		}
		public static int DeleteHelpCategorys(List<int> categoryIds)
		{
			int result;
			if (categoryIds == null || categoryIds.Count == 0)
			{
				result = 0;
			}
			else
			{
				result = CommentsProvider.Instance().DeleteHelpCategorys(categoryIds);
			}
			return result;
		}
		public static int DeleteHelps(IList<int> helps)
		{
			int result;
			if (helps == null || helps.Count == 0)
			{
				result = 0;
			}
			else
			{
				result = CommentsProvider.Instance().DeleteHelps(helps);
			}
			return result;
		}
		public static bool CreateHelp(HelpInfo help)
		{
			bool result;
			if (null == help)
			{
				result = false;
			}
			else
			{
				Globals.EntityCoding(help, true);
				result = CommentsProvider.Instance().AddHelp(help);
			}
			return result;
		}
		public static bool UpdateHelp(HelpInfo help)
		{
			bool result;
			if (null == help)
			{
				result = false;
			}
			else
			{
				Globals.EntityCoding(help, true);
				result = CommentsProvider.Instance().UpdateHelp(help);
			}
			return result;
		}
		public static bool DeleteHelp(int helpId)
		{
			return CommentsProvider.Instance().DeleteHelp(helpId);
		}
		public static void SwapHelpCategorySequence(int categoryId, int replaceCategoryId, int displaySequence, int replaceDisplaySequence)
		{
			CommentsProvider.Instance().SwapHelpCategorySequence(categoryId, replaceCategoryId, displaySequence, replaceDisplaySequence);
		}
		public static string UploadArticleImage(HttpPostedFile postedFile)
		{
			string result;
			if (!ResourcesHelper.CheckPostedFile(postedFile))
			{
				result = string.Empty;
			}
			else
			{
				string text = HiContext.Current.GetStoragePath() + "/article/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
				postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + text));
				result = text;
			}
			return result;
		}
		public static string UploadHelpImage(HttpPostedFile postedFile)
		{
			string result;
			if (!ResourcesHelper.CheckPostedFile(postedFile))
			{
				result = string.Empty;
			}
			else
			{
				string text = HiContext.Current.GetStoragePath() + "/help/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
				postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + text));
				result = text;
			}
			return result;
		}
	}
}
