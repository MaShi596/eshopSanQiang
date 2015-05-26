using Hidistro.Core.Entities;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using System;
using System.Collections.Generic;
using System.Data;
namespace Hidistro.SaleSystem.Comments
{
	public abstract class CommentProvider
	{
		public static CommentProvider Instance()
		{
			CommentProvider result;
			if (HiContext.Current.SiteSettings.IsDistributorSettings)
			{
				result = CommentSubsiteProvider.CreateInstance();
			}
			else
			{
				result = CommentMasterProvider.CreateInstance();
			}
			return result;
		}
		public abstract IList<FriendlyLinksInfo> GetFriendlyLinksIsVisible(int? number);
		public abstract DataSet GetHelps();
		public abstract List<AfficheInfo> GetAfficheList();
		public abstract AfficheInfo GetAffiche(int afficheId);
		public abstract AfficheInfo GetFrontOrNextAffiche(int afficheId, string type);
		public abstract DataSet GetVoteByIsShow();
		public abstract VoteInfo GetVoteById(long voteId);
		public abstract IList<VoteItemInfo> GetVoteItems(long voteId);
		public abstract VoteItemInfo GetVoteItemById(long voteItemId);
		public abstract int Vote(long voteItemId);
		public abstract DataTable GetHotKeywords(int categoryId, int hotKeywordsNum);
		public abstract DataSet GetAllHotKeywords();
		public abstract ArticleCategoryInfo GetArticleCategory(int categoryId);
		public abstract ArticleInfo GetArticle(int articleId);
		public abstract ArticleInfo GetFrontOrNextArticle(int articleId, string type, int categoryId);
		public abstract IList<ArticleInfo> GetArticleList(int categoryId, int maxNum);
		public abstract DbQueryResult GetArticleList(ArticleQuery articleQuery);
		public abstract IList<ArticleCategoryInfo> GetArticleMainCategories();
		public abstract DataTable GetArticlProductList(int articlId);
		public abstract HelpCategoryInfo GetHelpCategory(int categoryId);
		public abstract DbQueryResult GetHelpList(HelpQuery helpQuery);
		public abstract DataSet GetHelpTitleList();
		public abstract IList<HelpCategoryInfo> GetHelpCategorys();
		public abstract HelpInfo GetHelp(int helpId);
		public abstract HelpInfo GetFrontOrNextHelp(int helpId, int categoryId, string type);
		public abstract DataTable GetPromotes(Pagination pagination, int promotiontype, out int totalPromotes);
		public abstract PromotionInfo GetPromote(int activityId);
		public abstract PromotionInfo GetFrontOrNextPromoteInfo(PromotionInfo promote, string type);
		public abstract DbQueryResult GetLeaveComments(LeaveCommentQuery query);
		public abstract bool InsertLeaveComment(LeaveCommentInfo leave);
	}
}
