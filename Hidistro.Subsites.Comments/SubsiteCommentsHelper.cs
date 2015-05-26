using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Comments;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
namespace Hidistro.Subsites.Comments
{
	public static class SubsiteCommentsHelper
	{
		public static List<AfficheInfo> GetAfficheList()
		{
			return SubsiteCommentsProvider.Instance().GetAfficheList();
		}
		public static AfficheInfo GetAffiche(int afficheId)
		{
			return SubsiteCommentsProvider.Instance().GetAffiche(afficheId);
		}
		public static int DeleteAffiches(List<int> affiches)
		{
			int result;
			if (affiches == null || affiches.Count == 0)
			{
				result = 0;
			}
			else
			{
				result = SubsiteCommentsProvider.Instance().DeleteAffiches(affiches);
			}
			return result;
		}
		public static bool CreateAffiche(AfficheInfo affiche)
		{
			bool result;
			if (null == affiche)
			{
				result = false;
			}
			else
			{
				Globals.EntityCoding(affiche, true);
				result = SubsiteCommentsProvider.Instance().AddAffiche(affiche);
			}
			return result;
		}
		public static bool UpdateAffiche(AfficheInfo affiche)
		{
			bool result;
			if (null == affiche)
			{
				result = false;
			}
			else
			{
				Globals.EntityCoding(affiche, true);
				result = SubsiteCommentsProvider.Instance().UpdateAffiche(affiche);
			}
			return result;
		}
		public static bool DeleteAffiche(int afficheId)
		{
			return SubsiteCommentsProvider.Instance().DeleteAffiche(afficheId);
		}
		public static ArticleCategoryInfo GetArticleCategory(int categoryId)
		{
			return SubsiteCommentsProvider.Instance().GetArticleCategory(categoryId);
		}
		public static IList<ArticleCategoryInfo> GetMainArticleCategories()
		{
			return SubsiteCommentsProvider.Instance().GetMainArticleCategories();
		}
		public static ArticleInfo GetArticle(int articleId)
		{
			return SubsiteCommentsProvider.Instance().GetArticle(articleId);
		}
		public static DbQueryResult GetArticleList(ArticleQuery articleQuery)
		{
			return SubsiteCommentsProvider.Instance().GetArticleList(articleQuery);
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
				result = SubsiteCommentsProvider.Instance().CreateUpdateDeleteArticleCategory(articleCategory, DataProviderAction.Create);
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
				result = SubsiteCommentsProvider.Instance().CreateUpdateDeleteArticleCategory(articleCategory, DataProviderAction.Update);
			}
			return result;
		}
		public static bool DeleteArticleCategory(int categoryId)
		{
			ArticleCategoryInfo articleCategoryInfo = new ArticleCategoryInfo();
			articleCategoryInfo.CategoryId = categoryId;
			return SubsiteCommentsProvider.Instance().CreateUpdateDeleteArticleCategory(articleCategoryInfo, DataProviderAction.Delete);
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
				result = SubsiteCommentsProvider.Instance().DeleteArticles(articles);
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
				result = SubsiteCommentsProvider.Instance().AddArticle(article);
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
				result = SubsiteCommentsProvider.Instance().UpdateArticle(article);
			}
			return result;
		}
		public static bool DeleteArticle(int articleId)
		{
			return SubsiteCommentsProvider.Instance().DeleteArticle(articleId);
		}
		public static void SwapArticleCategorySequence(int categoryId, int replaceCategoryId, int displaySequence, int replaceDisplaySequence)
		{
			SubsiteCommentsProvider.Instance().SwapArticleCategorySequence(categoryId, replaceCategoryId, displaySequence, replaceDisplaySequence);
		}
		public static bool UpdateMyArticRelease(int articlId, bool isrealse)
		{
			return SubsiteCommentsProvider.Instance().UpdateMyArticRelease(articlId, isrealse);
		}
		public static HelpCategoryInfo GetHelpCategory(int categoryId)
		{
			return SubsiteCommentsProvider.Instance().GetHelpCategory(categoryId);
		}
		public static IList<HelpCategoryInfo> GetHelpCategorys()
		{
			return SubsiteCommentsProvider.Instance().GetHelpCategorys();
		}
		public static DbQueryResult GetHelpList(HelpQuery helpQuery)
		{
			return SubsiteCommentsProvider.Instance().GetHelpList(helpQuery);
		}
		public static HelpInfo GetHelp(int helpId)
		{
			return SubsiteCommentsProvider.Instance().GetHelp(helpId);
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
				result = SubsiteCommentsProvider.Instance().CreateUpdateDeleteHelpCategory(helpCategory, DataProviderAction.Create);
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
				result = SubsiteCommentsProvider.Instance().CreateUpdateDeleteHelpCategory(helpCategory, DataProviderAction.Update);
			}
			return result;
		}
		public static bool DeleteHelpCategory(int categoryId)
		{
			HelpCategoryInfo helpCategoryInfo = new HelpCategoryInfo();
			helpCategoryInfo.CategoryId = new int?(categoryId);
			return SubsiteCommentsProvider.Instance().CreateUpdateDeleteHelpCategory(helpCategoryInfo, DataProviderAction.Delete);
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
				result = SubsiteCommentsProvider.Instance().DeleteHelps(helps);
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
				result = SubsiteCommentsProvider.Instance().AddHelp(help);
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
				result = SubsiteCommentsProvider.Instance().UpdateHelp(help);
			}
			return result;
		}
		public static bool DeleteHelp(int helpId)
		{
			return SubsiteCommentsProvider.Instance().DeleteHelp(helpId);
		}
		public static void SwapHelpCategorySequence(int categoryId, int replaceCategoryId, int displaySequence, int replaceDisplaySequence)
		{
			SubsiteCommentsProvider.Instance().SwapHelpCategorySequence(categoryId, replaceCategoryId, displaySequence, replaceDisplaySequence);
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
		public static DbQueryResult GetReceivedMessages(MessageBoxQuery query, UserRole role)
		{
			return SubsiteCommentsProvider.Instance().GetReceivedMessages(query, role);
		}
		public static DbQueryResult GetSendedMessages(MessageBoxQuery query, UserRole role)
		{
			return SubsiteCommentsProvider.Instance().GetSendedMessages(query, role);
		}
		public static MessageBoxInfo GetMessage(long messageId)
		{
			return SubsiteCommentsProvider.Instance().GetMessage(messageId);
		}
		public static int SendMessageToMember(IList<MessageBoxInfo> messageBoxInfos)
		{
			int num = 0;
			foreach (MessageBoxInfo current in messageBoxInfos)
			{
				if (SubsiteCommentsProvider.Instance().InsertMessage(current, UserRole.Member))
				{
					num++;
				}
			}
			return num;
		}
		public static bool SendMessageToManager(MessageBoxInfo messageBoxInfo)
		{
			return SubsiteCommentsProvider.Instance().InsertMessage(messageBoxInfo, UserRole.SiteManager);
		}
		public static bool PostMessageIsRead(long messageId)
		{
			return SubsiteCommentsProvider.Instance().PostMessageIsRead(messageId);
		}
		public static int DeleteMessages(IList<long> messageList)
		{
			return SubsiteCommentsProvider.Instance().DeleteMessages(messageList);
		}
		public static int GetIsReadMessageToAdmin()
		{
			return SubsiteCommentsProvider.Instance().GetIsReadMessageToAdmin();
		}
		public static DbQueryResult GetConsultationProducts(ProductConsultationAndReplyQuery consultationQuery)
		{
			return SubsiteCommentsProvider.Instance().GetConsultationProducts(consultationQuery);
		}
		public static ProductConsultationInfo GetProductConsultation(int consultationId)
		{
			return SubsiteCommentsProvider.Instance().GetProductConsultation(consultationId);
		}
		public static bool ReplyProductConsultation(ProductConsultationInfo productConsultation)
		{
			return SubsiteCommentsProvider.Instance().ReplyProductConsultation(productConsultation);
		}
		public static int DeleteProductConsultation(int consultationId)
		{
			return SubsiteCommentsProvider.Instance().DeleteProductConsultation(consultationId);
		}
		public static int DeleteReview(IList<int> reviews)
		{
			int result;
			if (reviews == null || reviews.Count == 0)
			{
				result = 0;
			}
			else
			{
				result = SubsiteCommentsProvider.Instance().DeleteReview(reviews);
			}
			return result;
		}
		public static DataSet GetProductReviews(out int total, ProductReviewQuery reviewQuery)
		{
			return SubsiteCommentsProvider.Instance().GetProductReviews(out total, reviewQuery);
		}
		public static ProductReviewInfo GetProductReview(int reviewId)
		{
			return SubsiteCommentsProvider.Instance().GetProductReview(reviewId);
		}
		public static int DeleteProductReview(long reviewId)
		{
			return SubsiteCommentsProvider.Instance().DeleteProductReview(reviewId);
		}
		public static LeaveCommentInfo GetLeaveComment(long leaveId)
		{
			return SubsiteCommentsProvider.Instance().GetLeaveComment(leaveId);
		}
		public static DbQueryResult GetLeaveComments(LeaveCommentQuery query)
		{
			return SubsiteCommentsProvider.Instance().GetLeaveComments(query);
		}
		public static bool DeleteLeaveComment(long leaveId)
		{
			return SubsiteCommentsProvider.Instance().DeleteLeaveComment(leaveId);
		}
		public static int ReplyLeaveComment(LeaveCommentReplyInfo leaveReply)
		{
			leaveReply.ReplyDate = DateTime.Now;
			return SubsiteCommentsProvider.Instance().ReplyLeaveComment(leaveReply);
		}
		public static bool DeleteLeaveCommentReply(long leaveReplyId)
		{
			return SubsiteCommentsProvider.Instance().DeleteLeaveCommentReply(leaveReplyId);
		}
		public static int DeleteLeaveComments(IList<long> leaveIds)
		{
			return SubsiteCommentsProvider.Instance().DeleteLeaveComments(leaveIds);
		}
		public static DataTable GetReplyLeaveComments(long leaveId)
		{
			return SubsiteCommentsProvider.Instance().GetReplyLeaveComments(leaveId);
		}
	}
}
