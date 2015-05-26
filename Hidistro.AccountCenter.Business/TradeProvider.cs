using Hidistro.Core.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace Hidistro.AccountCenter.Business
{
	public abstract class TradeProvider
	{
		public static TradeProvider Instance()
		{
			TradeProvider result;
			if (HiContext.Current.SiteSettings.IsDistributorSettings)
			{
				result = TradeSubsiteProvider.CreateInstance();
			}
			else
			{
				result = TradeMasterProvider.CreateInstance();
			}
			return result;
		}
		public abstract DbQueryResult GetUserPoints(int pageIndex);
		public abstract System.Data.DataTable GetUserCoupons(int userId);
		public abstract System.Data.DataTable GetChangeCoupons();
		public abstract bool SendClaimCodes(CouponItemInfo couponItem);
		public abstract bool ExitCouponClaimCode(string claimCode);
		public abstract int AddClaimCodeToUser(string claimCode, int userId);
		public abstract OrderInfo GetOrderInfo(string orderId);
		public abstract DbQueryResult GetUserOrder(int userId, OrderQuery query);
		public abstract bool UserPayOrder(OrderInfo order, bool isBalancePayOrder, System.Data.Common.DbTransaction dbTran);
		public abstract bool ConfirmOrderFinish(OrderInfo order);
		public abstract bool CloseOrder(string orderId);
		public abstract bool UpdateOrderPaymentType(OrderInfo order);
		public abstract bool CanRefund(string orderId);
		public abstract bool ApplyForRefund(string orderId, string remark, int refundType);
		public abstract bool CanReturn(string orderId);
		public abstract bool ApplyForReturn(string orderId, string remark, int refundType);
		public abstract bool ApplyForReplace(string orderId, string remark);
		public abstract bool CanReplace(string orderId);
		public abstract void UpdateStockPayOrder(string orderId);
		public abstract IList<PaymentModeInfo> GetPaymentModes();
		public abstract PaymentModeInfo GetPaymentMode(int modeId);
		public abstract bool AddMemberPoint(UserPointInfo point);
		public abstract int GetHistoryPoint(int userId);
		public abstract bool UpdateUserAccount(decimal orderTotal, int totalPoint, int userId);
		public abstract bool ChangeMemberGrade(int userId, int gradId, int points);
		public abstract bool UpdateProductSaleCounts(Dictionary<string, LineItemInfo> lineItems);
		public abstract GroupBuyInfo GetGroupBuy(int groupBuyId);
		public abstract CountDownInfo CountDownBuy(int CountDownId);
		public abstract int GetOrderCount(int groupBuyId);
		public abstract bool SetGroupBuyEndUntreated(int groupBuyId);
		public abstract DbQueryResult GetRefundApplys(RefundApplyQuery query, int userId);
		public abstract DbQueryResult GetReturnsApplys(ReturnsApplyQuery query, int userId);
		public abstract decimal GetRefundMoney(OrderInfo order, out decimal refundMoney);
		public abstract DbQueryResult GetReplaceApplys(ReplaceApplyQuery query, int userId);
		public abstract bool SaveDebitNote(DebitNote note);
	}
}
