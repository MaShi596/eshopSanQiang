using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Promotions;
using System;
using System.Collections.Generic;
namespace Hidistro.Subsites.Promotions
{
	public static class SubsiteCouponHelper
	{
		public static CouponActionStatus CreateCoupon(CouponInfo coupon, int count, out string lotNumber)
		{
			Globals.EntityCoding(coupon, true);
			return SubsitePromotionsProvider.Instance().CreateCoupon(coupon, count, out lotNumber);
		}
		public static IList<CouponItemInfo> GetCouponItemInfos(string lotNumber)
		{
			return SubsitePromotionsProvider.Instance().GetCouponItemInfos(lotNumber);
		}
		public static DbQueryResult GetNewCoupons(Pagination page)
		{
			return SubsitePromotionsProvider.Instance().GetNewCoupons(page);
		}
		public static bool DeleteCoupon(int couponId)
		{
			return SubsitePromotionsProvider.Instance().DeleteCoupon(couponId);
		}
		public static CouponActionStatus UpdateCoupon(CouponInfo coupon)
		{
			Globals.EntityCoding(coupon, true);
			return SubsitePromotionsProvider.Instance().UpdateCoupon(coupon);
		}
		public static CouponInfo GetCoupon(int couponId)
		{
			return SubsitePromotionsProvider.Instance().GetCouponDetails(couponId);
		}
		public static List<int> GetUderlingIds(int? gradeId, string userName)
		{
			return SubsitePromotionsProvider.Instance().GetUderlingIds(gradeId, userName);
		}
		public static void SendClaimCodes(int couponId, IList<CouponItemInfo> listCouponItem)
		{
			foreach (CouponItemInfo current in listCouponItem)
			{
				SubsitePromotionsProvider.Instance().SendClaimCodes(couponId, current);
			}
		}
	}
}
