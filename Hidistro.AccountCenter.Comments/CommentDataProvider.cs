using Hidistro.Core.Entities;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Commodities;
using Hidistro.Membership.Context;
using System;
using System.Collections.Generic;
using System.Data;
namespace Hidistro.AccountCenter.Comments
{
	public abstract class CommentDataProvider
	{
		public static CommentDataProvider Instance()
		{
			CommentDataProvider result;
			if (HiContext.Current.SiteSettings.IsDistributorSettings)
			{
				result = CommentSubsiteDataProvider.CreateInstance();
			}
			else
			{
				result = CommentMasterDataProvider.CreateInstance();
			}
			return result;
		}
		public abstract bool InsertMessage(MessageBoxInfo messageBoxInfos);
		public abstract int DeleteMemberMessages(IList<long> messageList);
		public abstract DbQueryResult GetMemberSendedMessages(MessageBoxQuery query);
		public abstract DbQueryResult GetMemberReceivedMessages(MessageBoxQuery query);
		public abstract MessageBoxInfo GetMemberMessage(long messageId);
		public abstract bool PostMemberMessageIsRead(long messageId);
		public abstract bool AddProductToFavorite(int productId);
		public abstract bool ExistsProduct(int productId);
		public abstract int UpdateFavorite(int favoriteId, string tags, string remark);
		public abstract int DeleteFavorite(int favoriteId);
		public abstract DbQueryResult GetFavorites(int userId, string tags, Pagination page);
		public abstract ProductInfo GetProductDetails(int productId);
		public abstract bool DeleteFavorites(string string_0);
		public abstract int GetUserProductReviewsCount();
		public abstract DataSet GetUserProductReviewsAndReplys(UserProductReviewAndReplyQuery query, out int total);
		public abstract DataSet GetProductConsultationsAndReplys(ProductConsultationAndReplyQuery query, out int total);
		public abstract DbQueryResult GetBatchBuyProducts(ProductQuery query);
	}
}
