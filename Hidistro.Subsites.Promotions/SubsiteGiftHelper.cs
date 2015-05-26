using Hidistro.Core.Entities;
using Hidistro.Entities.Promotions;
using System;
namespace Hidistro.Subsites.Promotions
{
	public static class SubsiteGiftHelper
	{
		public static DbQueryResult GetGifts(GiftQuery query)
		{
			return SubsitePromotionsProvider.Instance().GetGifts(query);
		}
		public static DbQueryResult GetAbstroGiftsById(GiftQuery query)
		{
			return SubsitePromotionsProvider.Instance().GetAbstroGiftsById(query);
		}
		public static bool DownLoadGift(GiftInfo giftInfo)
		{
			return SubsitePromotionsProvider.Instance().DownLoadGift(giftInfo);
		}
		public static bool DeleteGiftById(int giftId)
		{
			return SubsitePromotionsProvider.Instance().DeleteGiftById(giftId);
		}
		public static GiftInfo GetMyGiftsDetails(int Id)
		{
			return SubsitePromotionsProvider.Instance().GetMyGiftsDetails(Id);
		}
		public static bool UpdateMyGifts(GiftInfo giftInfo)
		{
			return SubsitePromotionsProvider.Instance().UpdateMyGifts(giftInfo);
		}
	}
}
