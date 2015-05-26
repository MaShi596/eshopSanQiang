using Hidistro.AccountCenter.Profile;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace Hidistro.AccountCenter.Business
{
	public static class TradeHelper
	{
		public static DbQueryResult GetUserPoints(int pageIndex)
		{
			return TradeProvider.Instance().GetUserPoints(pageIndex);
		}
		public static System.Data.DataTable GetUserCoupons(int userId)
		{
			return TradeProvider.Instance().GetUserCoupons(userId);
		}
		public static System.Data.DataTable GetChangeCoupons()
		{
			return TradeProvider.Instance().GetChangeCoupons();
		}
		public static bool PointChageCoupon(int couponId, int needPoint, int currentPoint)
		{
			Member member = HiContext.Current.User as Member;
			UserPointInfo userPointInfo = new UserPointInfo();
			userPointInfo.OrderId = string.Empty;
			userPointInfo.UserId = member.UserId;
			userPointInfo.TradeDate = DateTime.Now;
			userPointInfo.TradeType = UserPointTradeType.ChangeCoupon;
			userPointInfo.Increased = null;
			userPointInfo.Reduced = new int?(needPoint);
			userPointInfo.Points = currentPoint - needPoint;
			bool result;
			if (userPointInfo.Points < 0)
			{
				result = false;
			}
			else
			{
				if (TradeProvider.Instance().AddMemberPoint(userPointInfo))
				{
					CouponItemInfo couponItemInfo = new CouponItemInfo();
					couponItemInfo.CouponId = couponId;
					couponItemInfo.UserId = new int?(member.UserId);
					couponItemInfo.UserName = member.Username;
					couponItemInfo.EmailAddress = member.Email;
					couponItemInfo.ClaimCode = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 15);
					couponItemInfo.GenerateTime = DateTime.Now;
					Users.ClearUserCache(member);
					if (TradeProvider.Instance().SendClaimCodes(couponItemInfo))
					{
						result = true;
						return result;
					}
				}
				result = false;
			}
			return result;
		}
		public static bool ExitCouponClaimCode(string claimCode)
		{
			return TradeProvider.Instance().ExitCouponClaimCode(claimCode);
		}
		public static int AddClaimCodeToUser(string claimCode, int userId)
		{
			return TradeProvider.Instance().AddClaimCodeToUser(claimCode, userId);
		}
		public static OrderInfo GetOrderInfo(string orderId)
		{
			return TradeProvider.Instance().GetOrderInfo(orderId);
		}
		public static DbQueryResult GetUserOrder(int userId, OrderQuery query)
		{
			return TradeProvider.Instance().GetUserOrder(userId, query);
		}
		public static GroupBuyInfo GetGroupBuy(int groupBuyId)
		{
			return TradeProvider.Instance().GetGroupBuy(groupBuyId);
		}
		public static CountDownInfo GetCountDownBuy(int CountDownId)
		{
			return TradeProvider.Instance().CountDownBuy(CountDownId);
		}
		public static int GetOrderCount(int groupBuyId)
		{
			return TradeProvider.Instance().GetOrderCount(groupBuyId);
		}
		public static bool SetGroupBuyEndUntreated(int groupBuyId)
		{
			return TradeProvider.Instance().SetGroupBuyEndUntreated(groupBuyId);
		}
		public static bool UserPayOrder(OrderInfo order, bool isBalancePayOrder)
		{
			bool flag = false;
			bool result;
			if (order.CheckAction(OrderActions.BUYER_PAY))
			{
				Database database = DatabaseFactory.CreateDatabase();
				using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
				{
					dbConnection.Open();
					System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
					try
					{
						if (!TradeProvider.Instance().UserPayOrder(order, isBalancePayOrder, dbTransaction))
						{
							dbTransaction.Rollback();
							result = false;
							return result;
						}
						if (HiContext.Current.SiteSettings.IsDistributorSettings && order.GroupBuyId <= 0)
						{
							PurchaseOrderProvider purchaseOrderProvider = PurchaseOrderProvider.CreateInstance();
							if (!purchaseOrderProvider.CreatePurchaseOrder(order, dbTransaction))
							{
								dbTransaction.Rollback();
								result = false;
								return result;
							}
						}
						flag = true;
						dbTransaction.Commit();
					}
					catch
					{
						dbTransaction.Rollback();
						result = false;
						return result;
					}
					finally
					{
						dbConnection.Close();
					}
				}
				if (flag)
				{
					if (!HiContext.Current.SiteSettings.IsDistributorSettings)
					{
						TradeProvider.Instance().UpdateStockPayOrder(order.OrderId);
					}
					TradeProvider.Instance().UpdateProductSaleCounts(order.LineItems);
					if (order.UserId != 0 && order.UserId != 1100)
					{
						IUser user = Users.GetUser(order.UserId, false);
						bool arg_145_0;
						if (user != null)
						{
							if (user.UserRole == UserRole.Member)
							{
								arg_145_0 = false;
								goto IL_145;
							}
						}
						arg_145_0 = (user == null || user.UserRole != UserRole.Underling);
						IL_145:
						if (!arg_145_0)
						{
							Member member = user as Member;
							UserPointInfo userPointInfo = new UserPointInfo();
							userPointInfo.OrderId = order.OrderId;
							userPointInfo.UserId = member.UserId;
							userPointInfo.TradeDate = DateTime.Now;
							userPointInfo.TradeType = UserPointTradeType.Bounty;
							userPointInfo.Increased = new int?(order.Points);
							userPointInfo.Points = order.Points + member.Points;
							int arg_1B7_0 = userPointInfo.Points;
							if (userPointInfo.Points < 0)
							{
								userPointInfo.Points = 2147483647;
							}
							TradeProvider.Instance().AddMemberPoint(userPointInfo);
							int referralDeduct = HiContext.Current.SiteSettings.ReferralDeduct;
							if (referralDeduct > 0 && member.ReferralUserId.HasValue && member.ReferralUserId.Value > 0)
							{
								IUser user2 = Users.GetUser(member.ReferralUserId.Value, false);
								if (user2 != null && (user2.UserRole == UserRole.Member || user2.UserRole == UserRole.Underling))
								{
									Member member2 = user2 as Member;
									if (member.ParentUserId == member2.ParentUserId && member2.IsOpenBalance)
									{
										decimal balance = member2.Balance + order.GetTotal() * referralDeduct / 100m;
										BalanceDetailInfo balanceDetailInfo = new BalanceDetailInfo();
										balanceDetailInfo.UserId = member2.UserId;
										balanceDetailInfo.UserName = member2.Username;
										balanceDetailInfo.TradeDate = DateTime.Now;
										balanceDetailInfo.TradeType = TradeTypes.ReferralDeduct;
										balanceDetailInfo.Income = new decimal?(order.GetTotal() * referralDeduct / 100m);
										balanceDetailInfo.Balance = balance;
										balanceDetailInfo.Remark = string.Format("提成来自'{0}'的订单{1}", order.Username, order.OrderId);
										PersonalProvider.Instance().AddBalanceDetail(balanceDetailInfo);
									}
								}
							}
							TradeProvider.Instance().UpdateUserAccount(order.GetTotal(), userPointInfo.Points, order.UserId);
							int historyPoint = TradeProvider.Instance().GetHistoryPoint(member.UserId);
							TradeProvider.Instance().ChangeMemberGrade(member.UserId, member.GradeId, historyPoint);
							Users.ClearUserCache(user);
						}
					}
				}
			}
			result = flag;
			return result;
		}
		public static bool ConfirmOrderFinish(OrderInfo order)
		{
			bool result = false;
			if (order.CheckAction(OrderActions.BUYER_CONFIRM_GOODS))
			{
				result = TradeProvider.Instance().ConfirmOrderFinish(order);
			}
			return result;
		}
		public static bool CloseOrder(string orderId)
		{
			return TradeProvider.Instance().CloseOrder(orderId);
		}
		public static bool UpdateOrderPaymentType(OrderInfo order)
		{
			return TradeProvider.Instance().UpdateOrderPaymentType(order);
		}
		public static bool ApplyForRefund(string orderId, string remark, int refundType)
		{
			return TradeProvider.Instance().ApplyForRefund(orderId, remark, refundType);
		}
		public static bool CanRefund(string orderId)
		{
			return TradeProvider.Instance().CanRefund(orderId);
		}
		public static bool ApplyForReturn(string orderId, string remark, int refundType)
		{
			return TradeProvider.Instance().ApplyForReturn(orderId, remark, refundType);
		}
		public static bool CanReturn(string orderId)
		{
			return TradeProvider.Instance().CanReturn(orderId);
		}
		public static bool ApplyForReplace(string orderId, string remark)
		{
			return TradeProvider.Instance().ApplyForReplace(orderId, remark);
		}
		public static bool CanReplace(string orderId)
		{
			return TradeProvider.Instance().CanReplace(orderId);
		}
		public static IList<PaymentModeInfo> GetPaymentModes()
		{
			return TradeProvider.Instance().GetPaymentModes();
		}
		public static PaymentModeInfo GetPaymentMode(int modeId)
		{
			return TradeProvider.Instance().GetPaymentMode(modeId);
		}
		public static DbQueryResult GetRefundApplys(RefundApplyQuery query, int userId)
		{
			return TradeProvider.Instance().GetRefundApplys(query, userId);
		}
		public static DbQueryResult GetReturnsApplys(ReturnsApplyQuery query, int userId)
		{
			return TradeProvider.Instance().GetReturnsApplys(query, userId);
		}
		public static decimal GetRefundMoney(OrderInfo order, out decimal refundMoney)
		{
			return TradeProvider.Instance().GetRefundMoney(order, out refundMoney);
		}
		public static DbQueryResult GetReplaceApplys(ReplaceApplyQuery query, int userId)
		{
			return TradeProvider.Instance().GetReplaceApplys(query, userId);
		}
		public static bool SaveDebitNote(DebitNote note)
		{
			return TradeProvider.Instance().SaveDebitNote(note);
		}
	}
}
