using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Promotions;
using System;
namespace Hidistro.ControlPanel.Promotions
{
	public static class GiftHelper
	{
		public static GiftActionStatus AddGift(GiftInfo gift)
		{
			Globals.EntityCoding(gift, true);
			return PromotionsProvider.Instance().CreateUpdateDeleteGift(gift, DataProviderAction.Create);
		}
		public static GiftActionStatus UpdateGift(GiftInfo gift)
		{
			Globals.EntityCoding(gift, true);
			return PromotionsProvider.Instance().CreateUpdateDeleteGift(gift, DataProviderAction.Update);
		}
		public static bool DeleteGift(int giftId)
		{
			GiftInfo gift = new GiftInfo
			{
				GiftId = giftId
			};
			GiftActionStatus giftActionStatus = PromotionsProvider.Instance().CreateUpdateDeleteGift(gift, DataProviderAction.Delete);
			return giftActionStatus == GiftActionStatus.Success;
		}
		public static DbQueryResult GetGifts(GiftQuery query)
		{
			return PromotionsProvider.Instance().GetGifts(query);
		}
		public static GiftInfo GetGiftDetails(int giftId)
		{
			return PromotionsProvider.Instance().GetGiftDetails(giftId);
		}
		public static bool UpdateIsDownLoad(int giftId, bool isdownload)
		{
			return PromotionsProvider.Instance().UpdateIsDownLoad(giftId, isdownload);
		}
	}
}
