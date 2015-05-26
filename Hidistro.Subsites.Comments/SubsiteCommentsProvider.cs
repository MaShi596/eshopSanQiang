using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Comments;
using Hidistro.Membership.Core.Enums;
using System;
using System.Collections.Generic;
using System.Data;
namespace Hidistro.Subsites.Comments
{
	public abstract class SubsiteCommentsProvider
	{
		private static readonly SubsiteCommentsProvider _defaultInstance;
		static SubsiteCommentsProvider()
		{
			SubsiteCommentsProvider._defaultInstance = (DataProviders.CreateInstance("Hidistro.Subsites.Data.CommentData,Hidistro.Subsites.Data") as SubsiteCommentsProvider);
		}
		public static SubsiteCommentsProvider Instance()
		{
			return SubsiteCommentsProvider._defaultInstance;
		}
		public abstract bool AddAffiche(AfficheInfo affiche);
		public abstract bool UpdateAffiche(AfficheInfo affiche);
		public abstract bool DeleteAffiche(int afficheId);
		public abstract int DeleteAffiches(List<int> afficheIds);
		public abstract List<AfficheInfo> GetAfficheList();
		public abstract AfficheInfo GetAffiche(int afficheId);
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
		public abstract bool UpdateMyArticRelease(int articlId, bool isrelease);
		public abstract bool CreateUpdateDeleteHelpCategory(HelpCategoryInfo helpCategory, DataProviderAction action);
		public abstract int DeleteHelps(IList<int> helps);
		public abstract bool AddHelp(HelpInfo help);
		public abstract bool UpdateHelp(HelpInfo help);
		public abstract bool DeleteHelp(int helpId);
		public abstract void SwapHelpCategorySequence(int categoryId, int replaceCategoryId, int displaySequence, int replaceDisplaySequence);
		public abstract HelpCategoryInfo GetHelpCategory(int categoryId);
		public abstract IList<HelpCategoryInfo> GetHelpCategorys();
		public abstract DbQueryResult GetHelpList(HelpQuery helpQuery);
		public abstract HelpInfo GetHelp(int helpId);
		public abstract DbQueryResult GetReceivedMessages(MessageBoxQuery query, UserRole role);
		public abstract DbQueryResult GetSendedMessages(MessageBoxQuery query, UserRole role);
		public abstract MessageBoxInfo GetMessage(long messageId);
		public abstract bool InsertMessage(MessageBoxInfo messageBoxInfo, UserRole toRole);
		public abstract bool PostMessageIsRead(long messageId);
		public abstract int DeleteMessages(IList<long> messageList);
		public abstract int GetIsReadMessageToAdmin();
		public abstract DbQueryResult GetConsultationProducts(ProductConsultationAndReplyQuery consultationQuery);
		public abstract bool ReplyProductConsultation(ProductConsultationInfo productConsultation);
		public abstract int DeleteProductConsultation(int consultationId);
		public abstract ProductConsultationInfo GetProductConsultation(int consultationId);
		public abstract DataSet GetProductReviews(out int total, ProductReviewQuery reviewQuery);
		public abstract ProductReviewInfo GetProductReview(int reviewId);
		public abstract int DeleteProductReview(long reviewId);
		public abstract int DeleteReview(IList<int> reviews);
		public abstract LeaveCommentInfo GetLeaveComment(long leaveId);
		public abstract DbQueryResult GetLeaveComments(LeaveCommentQuery query);
		public abstract bool DeleteLeaveComment(long leaveId);
		public abstract int DeleteLeaveComments(IList<long> leaveIds);
		public abstract int ReplyLeaveComment(LeaveCommentReplyInfo leaveReply);
		public abstract bool DeleteLeaveCommentReply(long leaveReplyId);
		public abstract DataTable GetReplyLeaveComments(long leaveId);
	}
}
