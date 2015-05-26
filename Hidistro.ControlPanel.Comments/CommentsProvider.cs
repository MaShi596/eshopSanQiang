using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Comments;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core.Enums;
using System;
using System.Collections.Generic;
using System.Data;
namespace Hidistro.ControlPanel.Comments
{
	public abstract class CommentsProvider
	{
		private static readonly CommentsProvider _defaultInstance;
		static CommentsProvider()
		{
			CommentsProvider._defaultInstance = (DataProviders.CreateInstance("Hidistro.ControlPanel.Data.CommentData,Hidistro.ControlPanel.Data") as CommentsProvider);
		}
		public static CommentsProvider Instance()
		{
			return CommentsProvider._defaultInstance;
		}
		public abstract bool CreateUpdateDeleteArticleCategory(ArticleCategoryInfo articleCategory, DataProviderAction action);
		public abstract bool AddArticle(ArticleInfo article);
		public abstract bool UpdateArticle(ArticleInfo article);
		public abstract bool DeleteArticle(int articleId);
		public abstract void SwapArticleCategorySequence(int categoryId, int replaceCategoryId, int displaySequence, int replaceDisplaySequence);
		public abstract int DeleteArticles(IList<int> articles);
		public abstract ArticleCategoryInfo GetArticleCategory(int categoryId);
		public abstract IList<ArticleCategoryInfo> GetMainArticleCategories();
		public abstract ArticleInfo GetArticle(int articleId);
		public abstract DbQueryResult GetArticleList(ArticleQuery articleQuery);
		public abstract DbQueryResult GetRelatedArticsProducts(Pagination page, int articId);
		public abstract bool AddReleatesProdcutByArticId(int articId, int prodcutId);
		public abstract bool RemoveReleatesProductByArticId(int articId, int productId);
		public abstract bool RemoveReleatesProductByArticId(int articId);
		public abstract bool UpdateRelease(int articId, bool release);
		public abstract bool CreateUpdateDeleteHelpCategory(HelpCategoryInfo helpCategory, DataProviderAction action);
		public abstract int DeleteHelpCategorys(List<int> categoryIds);
		public abstract int DeleteHelps(IList<int> helps);
		public abstract bool AddHelp(HelpInfo help);
		public abstract bool UpdateHelp(HelpInfo help);
		public abstract bool DeleteHelp(int helpId);
		public abstract void SwapHelpCategorySequence(int categoryId, int replaceCategoryId, int displaySequence, int replaceDisplaySequence);
		public abstract HelpCategoryInfo GetHelpCategory(int categoryId);
		public abstract IList<HelpCategoryInfo> GetHelpCategorys();
		public abstract DbQueryResult GetHelpList(HelpQuery helpQuery);
		public abstract HelpInfo GetHelp(int helpId);
		public abstract bool AddAffiche(AfficheInfo affiche);
		public abstract bool UpdateAffiche(AfficheInfo affiche);
		public abstract bool DeleteAffiche(int afficheId);
		public abstract int DeleteAffiches(List<int> afficheIds);
		public abstract List<AfficheInfo> GetAfficheList();
		public abstract AfficheInfo GetAffiche(int afficheId);
		public abstract LeaveCommentInfo GetLeaveComment(long leaveId);
		public abstract DbQueryResult GetLeaveComments(LeaveCommentQuery query);
		public abstract bool DeleteLeaveComment(long leaveId);
		public abstract int DeleteLeaveComments(IList<long> leaveIds);
		public abstract int ReplyLeaveComment(LeaveCommentReplyInfo leaveReply);
		public abstract bool DeleteLeaveCommentReply(long leaveReplyId);
		public abstract DataTable GetReplyLeaveComments(long leaveId);
		public abstract DbQueryResult GetManagerReceivedMessages(MessageBoxQuery query, UserRole role);
		public abstract DbQueryResult GetManagerSendedMessages(MessageBoxQuery query, UserRole role);
		public abstract MessageBoxInfo GetManagerMessage(long messageId);
		public abstract bool PostManagerMessageIsRead(long messageId);
		public abstract int DeleteManagerMessages(IList<long> messageList);
		public abstract int GetMemberUnReadMessageNum();
		public abstract bool InsertMessage(MessageBoxInfo messageBoxInfo, UserRole toRole);
		public abstract bool AddMessage(MessageBoxInfo messageBoxInfo, UserRole toRole);
		public abstract IList<Distributor> GetDistributorsByRank(int? gradeId);
		public abstract IList<Member> GetMembersByRank(int? gradeId);
		public abstract DbQueryResult GetConsultationProducts(ProductConsultationAndReplyQuery consultationQuery);
		public abstract bool ReplyProductConsultation(ProductConsultationInfo productConsultation);
		public abstract int DeleteProductConsultation(int consultationId);
		public abstract ProductConsultationInfo GetProductConsultation(int consultationId);
		public abstract DataSet GetProductReviews(out int total, ProductReviewQuery reviewQuery);
		public abstract ProductReviewInfo GetProductReview(int reviewId);
		public abstract int DeleteProductReview(long reviewId);
		public abstract int DeleteReview(IList<int> reviews);
	}
}
