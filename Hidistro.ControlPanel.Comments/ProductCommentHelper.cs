using Hidistro.Core.Entities;
using Hidistro.Entities.Comments;
using System;
using System.Collections.Generic;
using System.Data;
namespace Hidistro.ControlPanel.Comments
{
	public sealed class ProductCommentHelper
	{
		private ProductCommentHelper()
		{
		}
		public static DbQueryResult GetConsultationProducts(ProductConsultationAndReplyQuery consultationQuery)
		{
			return CommentsProvider.Instance().GetConsultationProducts(consultationQuery);
		}
		public static ProductConsultationInfo GetProductConsultation(int consultationId)
		{
			return CommentsProvider.Instance().GetProductConsultation(consultationId);
		}
		public static bool ReplyProductConsultation(ProductConsultationInfo productConsultation)
		{
			return CommentsProvider.Instance().ReplyProductConsultation(productConsultation);
		}
		public static int DeleteProductConsultation(int consultationId)
		{
			return CommentsProvider.Instance().DeleteProductConsultation(consultationId);
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
				result = CommentsProvider.Instance().DeleteReview(reviews);
			}
			return result;
		}
		public static DataSet GetProductReviews(out int total, ProductReviewQuery reviewQuery)
		{
			return CommentsProvider.Instance().GetProductReviews(out total, reviewQuery);
		}
		public static ProductReviewInfo GetProductReview(int reviewId)
		{
			return CommentsProvider.Instance().GetProductReview(reviewId);
		}
		public static int DeleteProductReview(long reviewId)
		{
			return CommentsProvider.Instance().DeleteProductReview(reviewId);
		}
	}
}
