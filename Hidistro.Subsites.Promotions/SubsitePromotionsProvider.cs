using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.Membership.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace Hidistro.Subsites.Promotions
{
	public abstract class SubsitePromotionsProvider
	{
		private static readonly SubsitePromotionsProvider _defaultInstance;
		static SubsitePromotionsProvider()
		{
			SubsitePromotionsProvider._defaultInstance = (DataProviders.CreateInstance("Hidistro.Subsites.Data.PromotionData,Hidistro.Subsites.Data") as SubsitePromotionsProvider);
		}
		public static SubsitePromotionsProvider Instance()
		{
			return SubsitePromotionsProvider._defaultInstance;
		}
		public abstract CouponActionStatus CreateCoupon(CouponInfo coupon, int count, out string lotNumber);
		public abstract IList<Member> GetMembersByRank(int? gradeId);
		public abstract IList<CouponItemInfo> GetCouponItemInfos(string lotNumber);
		public abstract DbQueryResult GetNewCoupons(Pagination page);
		public abstract DbQueryResult GetCouponsList(CouponItemInfoQuery componquery);
		public abstract bool DeleteCoupon(int couponId);
		public abstract CouponActionStatus UpdateCoupon(CouponInfo coupon);
		public abstract CouponInfo GetCouponDetails(int couponId);
		public abstract bool SendClaimCodes(int couponId, CouponItemInfo couponItem);
		public abstract List<int> GetUderlingIds(int? gradeId, string userName);
		public abstract DbQueryResult GetGifts(GiftQuery query);
		public abstract DbQueryResult GetAbstroGiftsById(GiftQuery query);
		public abstract GiftInfo GetMyGiftsDetails(int giftId);
		public abstract bool DownLoadGift(GiftInfo giftinfo);
		public abstract bool UpdateMyGifts(GiftInfo giftInfo);
		public abstract bool DeleteGiftById(int giftId);
		public abstract string GetPriceByProductId(int productId);
		public abstract int AddGroupBuy(GroupBuyInfo groupBuy, System.Data.Common.DbTransaction dbTran);
		public abstract bool ProductGroupBuyExist(int productId);
		public abstract bool AddGroupBuyCondition(int groupBuyId, IList<GropBuyConditionInfo> gropBuyConditions, System.Data.Common.DbTransaction dbTran);
		public abstract bool DeleteGroupBuy(int groupBuyId);
		public abstract bool DeleteGroupBuyCondition(int groupBuyId, System.Data.Common.DbTransaction dbTran);
		public abstract bool UpdateGroupBuy(GroupBuyInfo groupBuy, System.Data.Common.DbTransaction dbTran);
		public abstract void SwapGroupBuySequence(int groupBuyId, int displaySequence);
		public abstract GroupBuyInfo GetGroupBuy(int groupBuyId);
		public abstract DbQueryResult GetGroupBuyList(GroupBuyQuery query);
		public abstract decimal GetCurrentPrice(int groupBuyId, int prodcutQuantity);
		public abstract int GetOrderCount(int groupBuyId);
		public abstract bool SetGroupBuyStatus(int groupBuyId, GroupBuyStatus status);
		public abstract bool SetGroupBuyEndUntreated(int groupBuyId);
		public abstract DbQueryResult GetCountDownList(GroupBuyQuery query);
		public abstract void SwapCountDownSequence(int countDownId, int displaySequence);
		public abstract bool DeleteCountDown(int countDownId);
		public abstract bool AddCountDown(CountDownInfo countDownInfo);
		public abstract bool UpdateCountDown(CountDownInfo countDownInfo);
		public abstract bool ProductCountDownExist(int productId);
		public abstract CountDownInfo GetCountDownInfo(int countDownId);
		public abstract DbQueryResult GetBundlingProducts(BundlingInfoQuery query);
		public abstract BundlingInfo GetBundlingInfo(int bindID);
		public abstract int AddBundlingProduct(BundlingInfo bind, System.Data.Common.DbTransaction dbTran);
		public abstract bool AddBundlingProductItems(int BundlingID, List<BundlingItemInfo> bundlingiteminfo, System.Data.Common.DbTransaction dbTran);
		public abstract bool DeleteBundlingProduct(int bundlingID);
		public abstract bool UpdateBundlingProduct(BundlingInfo bind, System.Data.Common.DbTransaction dbTran);
		public abstract bool DeleteBundlingByID(int bundlingID, System.Data.Common.DbTransaction dbTran);
		public abstract System.Data.DataTable GetPromotions(bool isProductPromote);
		public abstract PromotionInfo GetPromotion(int activityId);
		public abstract IList<MemberGradeInfo> GetPromoteMemberGrades(int activityId);
		public abstract System.Data.DataTable GetPromotionProducts(int activityId);
		public abstract bool AddPromotionProducts(int activityId, string productIds);
		public abstract bool DeletePromotionProducts(int activityId, int? productId);
		public abstract int AddPromotion(PromotionInfo promotion, System.Data.Common.DbTransaction dbTran);
		public abstract bool AddPromotionMemberGrades(int activityId, IList<int> memberGrades, System.Data.Common.DbTransaction dbTran);
		public abstract bool EditPromotion(PromotionInfo promotion, System.Data.Common.DbTransaction dbTran);
		public abstract bool DeletePromotion(int activityId);
	}
}
