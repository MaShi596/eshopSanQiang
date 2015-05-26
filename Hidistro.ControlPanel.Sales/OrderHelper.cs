using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Store;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
namespace Hidistro.ControlPanel.Sales
{
	public static class OrderHelper
	{
		public static OrderInfo GetOrderInfo(string orderId)
		{
			return SalesProvider.Instance().GetOrderInfo(orderId);
		}
		public static IList<LineItemInfo> GetLineItemInfo(string orderId)
		{
			return SalesProvider.Instance().GetLineItemInfo(orderId);
		}
		public static DbQueryResult GetOrders(OrderQuery query)
		{
			return SalesProvider.Instance().GetOrders(query);
		}
		public static void SetOrderShipNumber(string[] orderIds, string startNumber)
		{
			int num = 0;
			for (int i = 0; i < orderIds.Length; i++)
			{
				long num2 = long.Parse(startNumber) + (long)num;
				if (SalesProvider.Instance().EditOrderShipNumber(orderIds[i], num2.ToString()))
				{
					num++;
				}
			}
		}
		public static bool SetOrderShipNumber(string orderId, string startNumber)
		{
			return SalesProvider.Instance().EditOrderShipNumber(orderId, startNumber);
		}
		public static bool EditPurchaseOrderShipNumber(string purchaseOrderId, string orderId, string startNumber)
		{
			return SalesProvider.Instance().EditPurchaseOrderShipNumber(purchaseOrderId, orderId, startNumber);
		}
		public static void SetOrderPrinted(string[] orderIds, bool isPrinted)
		{
			int num = 0;
			for (int i = orderIds.Length - 1; i >= 0; i--)
			{
				if (SalesProvider.Instance().SetOrderPrinted(orderIds[i], isPrinted))
				{
					num++;
				}
			}
		}
		public static System.Data.DataTable GetSendGoodsOrders(string orderIds)
		{
			return SalesProvider.Instance().GetSendGoodsOrders(orderIds);
		}
		public static System.Data.DataTable GetRecentlyOrders(out int number)
		{
			return SalesProvider.Instance().GetRecentlyOrders(out number);
		}
		public static int DeleteOrders(string orderIds)
		{
			ManagerHelper.CheckPrivilege(Privilege.DeleteOrder);
			int num = SalesProvider.Instance().DeleteOrders(orderIds);
			if (num > 0)
			{
				EventLogs.WriteOperationLog(Privilege.DeleteOrder, string.Format(CultureInfo.InvariantCulture, "删除了编号为\"{0}\"的订单", new object[]
				{
					orderIds
				}));
			}
			return num;
		}
		public static bool CloseTransaction(OrderInfo order)
		{
			ManagerHelper.CheckPrivilege(Privilege.EditOrders);
			bool result;
			if (order.CheckAction(OrderActions.SELLER_CLOSE))
			{
				bool flag;
				if (flag = SalesProvider.Instance().CloseTransaction(order))
				{
					EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "关闭了订单“{0}”", new object[]
					{
						order.OrderId
					}));
				}
				result = flag;
			}
			else
			{
				result = false;
			}
			return result;
		}
		private static void UpdateUserStatistics(int userId, decimal refundAmount, bool isAllRefund)
		{
			SalesProvider.Instance().UpdateUserStatistics(userId, refundAmount, isAllRefund);
		}
		private static void ReducedPoint(OrderInfo order, Member member)
		{
			UserPointInfo userPointInfo = new UserPointInfo();
			userPointInfo.OrderId = order.OrderId;
			userPointInfo.UserId = member.UserId;
			userPointInfo.TradeDate = DateTime.Now;
			userPointInfo.TradeType = UserPointTradeType.Refund;
			userPointInfo.Reduced = new int?(order.Points);
			userPointInfo.Points = member.Points - userPointInfo.Reduced.Value;
			SalesProvider.Instance().AddMemberPoint(userPointInfo);
		}
		private static void ReduceDeduct(string orderId, decimal refundAmount, Member member)
		{
			int referralDeduct = HiContext.Current.SiteSettings.ReferralDeduct;
			if (referralDeduct > 0 && member.ReferralUserId.HasValue && member.ReferralUserId.Value > 0)
			{
				IUser user = Users.GetUser(member.ReferralUserId.Value, false);
				if (user != null && user.UserRole == UserRole.Member)
				{
					Member member2 = user as Member;
					if (member.ParentUserId == member2.ParentUserId && member2.IsOpenBalance)
					{
						decimal balance = member2.Balance - refundAmount * referralDeduct / 100m;
						BalanceDetailInfo balanceDetailInfo = new BalanceDetailInfo();
						balanceDetailInfo.UserId = member2.UserId;
						balanceDetailInfo.UserName = member2.Username;
						balanceDetailInfo.TradeDate = DateTime.Now;
						balanceDetailInfo.TradeType = TradeTypes.ReferralDeduct;
						balanceDetailInfo.Expenses = new decimal?(refundAmount * referralDeduct / 100m);
						balanceDetailInfo.Balance = balance;
						balanceDetailInfo.Remark = string.Format("退回提成因为{0}的订单{1}已退款", member.Username, orderId);
						MemberProvider.Instance().InsertBalanceDetail(balanceDetailInfo);
					}
				}
			}
		}
		private static void UpdateUserAccount(OrderInfo order)
		{
			int num = order.UserId;
			if (num == 1100)
			{
				num = 0;
			}
			IUser user = Users.GetUser(num, false);
			if (user != null && user.UserRole == UserRole.Member)
			{
				Member member = user as Member;
				UserPointInfo userPointInfo = new UserPointInfo();
				userPointInfo.OrderId = order.OrderId;
				userPointInfo.UserId = member.UserId;
				userPointInfo.TradeDate = DateTime.Now;
				userPointInfo.TradeType = UserPointTradeType.Bounty;
				userPointInfo.Increased = new int?(order.Points);
				userPointInfo.Points = order.Points + member.Points;
				int arg_A2_0 = userPointInfo.Points;
				if (userPointInfo.Points < 0)
				{
					userPointInfo.Points = 2147483647;
				}
				SalesProvider.Instance().AddMemberPoint(userPointInfo);
				int referralDeduct = HiContext.Current.SiteSettings.ReferralDeduct;
				if (referralDeduct > 0 && member.ReferralUserId.HasValue && member.ReferralUserId.Value > 0)
				{
					IUser user2 = Users.GetUser(member.ReferralUserId.Value, false);
					if (user2 != null && user2.UserRole == UserRole.Member)
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
							balanceDetailInfo.Remark = string.Format("提成来自{0}的订单{1}", order.Username, order.OrderId);
							MemberProvider.Instance().InsertBalanceDetail(balanceDetailInfo);
						}
					}
				}
				SalesProvider.Instance().UpdateUserAccount(order.GetTotal(), order.UserId);
				int historyPoint = SalesProvider.Instance().GetHistoryPoint(member.UserId);
				SalesProvider.Instance().ChangeMemberGrade(member.UserId, member.GradeId, historyPoint);
				Users.ClearUserCache(user);
			}
		}
		public static bool UpdateOrderShippingMode(OrderInfo order)
		{
			ManagerHelper.CheckPrivilege(Privilege.EditOrders);
			bool result;
			if (order.CheckAction(OrderActions.MASTER_SELLER_MODIFY_SHIPPING_MODE))
			{
				bool flag;
				if (flag = SalesProvider.Instance().UpdateOrderShippingMode(order))
				{
					EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "修改了订单“{0}”的配送方式", new object[]
					{
						order.OrderId
					}));
				}
				result = flag;
			}
			else
			{
				result = false;
			}
			return result;
		}
		public static bool UpdateOrderPaymentType(OrderInfo order)
		{
			ManagerHelper.CheckPrivilege(Privilege.EditOrders);
			bool result;
			if (order.CheckAction(OrderActions.MASTER_SELLER_MODIFY_PAYMENT_MODE))
			{
				bool flag;
				if (flag = SalesProvider.Instance().UpdateOrderPaymentType(order))
				{
					EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "修改了订单“{0}”的支付方式", new object[]
					{
						order.OrderId
					}));
				}
				result = flag;
			}
			else
			{
				result = false;
			}
			return result;
		}
		public static bool MondifyAddress(OrderInfo order)
		{
			ManagerHelper.CheckPrivilege(Privilege.EditOrders);
			bool result;
			if (order.CheckAction(OrderActions.MASTER_SELLER_MODIFY_DELIVER_ADDRESS))
			{
				bool flag;
				if (flag = SalesProvider.Instance().SaveShippingAddress(order))
				{
					EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "修改了订单“{0}”的收货地址", new object[]
					{
						order.OrderId
					}));
				}
				result = flag;
			}
			else
			{
				result = false;
			}
			return result;
		}
		public static bool SaveRemark(OrderInfo order)
		{
			ManagerHelper.CheckPrivilege(Privilege.RemarkOrder);
			bool result;
			if (result = SalesProvider.Instance().SaveOrderRemark(order))
			{
				EventLogs.WriteOperationLog(Privilege.RemarkOrder, string.Format(CultureInfo.InvariantCulture, "对订单“{0}”进行了备注", new object[]
				{
					order.OrderId
				}));
			}
			return result;
		}
		public static bool SetOrderShippingMode(string purchaseOrderIds, int realShippingModeId, string realModeName)
		{
			return SalesProvider.Instance().SetOrderShippingMode(purchaseOrderIds, realShippingModeId, realModeName);
		}
		public static bool SetOrderExpressComputerpe(string purchaseOrderIds, string expressCompanyName, string expressCompanyAbb)
		{
			return SalesProvider.Instance().SetOrderExpressComputerpe(purchaseOrderIds, expressCompanyName, expressCompanyAbb);
		}
		public static bool ConfirmPay(OrderInfo order)
		{
			ManagerHelper.CheckPrivilege(Privilege.CofimOrderPay);
			bool result = false;
			if (order.CheckAction(OrderActions.SELLER_CONFIRM_PAY) && (result = (SalesProvider.Instance().ConfirmPay(order) > 0)))
			{
				SalesProvider.Instance().UpdatePayOrderStock(order.OrderId);
				SalesProvider.Instance().UpdateProductSaleCounts(order.LineItems);
				OrderHelper.UpdateUserAccount(order);
				EventLogs.WriteOperationLog(Privilege.CofimOrderPay, string.Format(CultureInfo.InvariantCulture, "确认收款编号为\"{0}\"的订单", new object[]
				{
					order.OrderId
				}));
			}
			return result;
		}
		public static bool ConfirmOrderFinish(OrderInfo order)
		{
			ManagerHelper.CheckPrivilege(Privilege.EditOrders);
			bool result = false;
			if (order.CheckAction(OrderActions.SELLER_FINISH_TRADE) && (result = SalesProvider.Instance().ConfirmOrderFinish(order)))
			{
				EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "完成编号为\"{0}\"的订单", new object[]
				{
					order.OrderId
				}));
			}
			return result;
		}
		public static bool SendGoods(OrderInfo order)
		{
			ManagerHelper.CheckPrivilege(Privilege.OrderSendGoods);
			bool result = false;
			if (order.CheckAction(OrderActions.SELLER_SEND_GOODS))
			{
				order.OrderStatus = OrderStatus.SellerAlreadySent;
				if (result = (SalesProvider.Instance().SendGoods(order) > 0))
				{
					if (order.Gateway.ToLower() == "hishop.plugins.payment.podrequest")
					{
						SalesProvider.Instance().UpdatePayOrderStock(order.OrderId);
						SalesProvider.Instance().UpdateProductSaleCounts(order.LineItems);
						OrderHelper.UpdateUserAccount(order);
					}
					EventLogs.WriteOperationLog(Privilege.OrderSendGoods, string.Format(CultureInfo.InvariantCulture, "发货编号为\"{0}\"的订单", new object[]
					{
						order.OrderId
					}));
				}
			}
			return result;
		}
		public static bool UpdateOrderAmount(OrderInfo order)
		{
			ManagerHelper.CheckPrivilege(Privilege.EditOrders);
			bool result = false;
			if (order.CheckAction(OrderActions.SELLER_MODIFY_TRADE) && (result = SalesProvider.Instance().UpdateOrderAmount(order, null)))
			{
				EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "修改了编号为\"{0}\"订单的金额", new object[]
				{
					order.OrderId
				}));
			}
			return result;
		}
		public static bool DeleteOrderGift(OrderInfo order, int giftId)
		{
			ManagerHelper.CheckPrivilege(Privilege.EditOrders);
			Database database = DatabaseFactory.CreateDatabase();
			bool result;
			using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					SalesProvider salesProvider = SalesProvider.Instance();
					OrderGiftInfo orderGift = salesProvider.GetOrderGift(giftId, order.OrderId);
					order.Gifts.Remove(orderGift);
					if (!salesProvider.DeleteOrderGift(order.OrderId, orderGift.GiftId, dbTransaction))
					{
						dbTransaction.Rollback();
						result = false;
					}
					else
					{
						if (!salesProvider.UpdateOrderAmount(order, dbTransaction))
						{
							dbTransaction.Rollback();
							result = false;
						}
						else
						{
							dbTransaction.Commit();
							EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "删除了订单号为\"{0}\"的订单礼品", new object[]
							{
								order.OrderId
							}));
							result = true;
						}
					}
				}
				catch
				{
					dbTransaction.Rollback();
					result = false;
				}
				finally
				{
					dbConnection.Close();
				}
			}
			return result;
		}
		public static bool DeleteLineItem(string string_0, OrderInfo order)
		{
			ManagerHelper.CheckPrivilege(Privilege.EditOrders);
			Database database = DatabaseFactory.CreateDatabase();
			bool result;
			bool flag;
			using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					SalesProvider salesProvider = SalesProvider.Instance();
					order.LineItems.Remove(string_0);
					if (!salesProvider.DeleteLineItem(string_0, order.OrderId, dbTransaction))
					{
						dbTransaction.Rollback();
						result = false;
						return result;
					}
					if (!salesProvider.UpdateOrderAmount(order, dbTransaction))
					{
						dbTransaction.Rollback();
						result = false;
						return result;
					}
					dbTransaction.Commit();
					flag = true;
				}
				catch
				{
					dbTransaction.Rollback();
					flag = false;
				}
				finally
				{
					dbConnection.Close();
				}
			}
			if (flag)
			{
				EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "删除了订单号为\"{0}\"的订单商品", new object[]
				{
					order.OrderId
				}));
			}
			result = flag;
			return result;
		}
		public static bool UpdateLineItem(string string_0, OrderInfo order, int quantity)
		{
			ManagerHelper.CheckPrivilege(Privilege.EditOrders);
			Database database = DatabaseFactory.CreateDatabase();
			bool result;
			bool flag;
			using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					SalesProvider salesProvider = SalesProvider.Instance();
					order.LineItems[string_0].Quantity = quantity;
					order.LineItems[string_0].ShipmentQuantity = quantity;
					order.LineItems[string_0].ItemAdjustedPrice = order.LineItems[string_0].ItemListPrice;
					if (!salesProvider.UpdateLineItem(order.OrderId, order.LineItems[string_0], dbTransaction))
					{
						dbTransaction.Rollback();
						result = false;
						return result;
					}
					if (!salesProvider.UpdateOrderAmount(order, dbTransaction))
					{
						dbTransaction.Rollback();
						result = false;
						return result;
					}
					dbTransaction.Commit();
					flag = true;
				}
				catch (Exception)
				{
					dbTransaction.Rollback();
					flag = false;
				}
				finally
				{
					dbConnection.Close();
				}
			}
			if (flag)
			{
				EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "修改了订单号为\"{0}\"的订单商品数量", new object[]
				{
					order.OrderId
				}));
			}
			result = flag;
			return result;
		}
		public static int GetSkuStock(string skuId)
		{
			return SalesProvider.Instance().GetSkuStock(skuId);
		}
		public static LineItemInfo GetLineItemInfo(string string_0, string orderId)
		{
			return SalesProvider.Instance().GetLineItemInfo(string_0, orderId);
		}
		public static DbQueryResult GetOrderGifts(OrderGiftQuery query)
		{
			return SalesProvider.Instance().GetOrderGifts(query);
		}
		public static DbQueryResult GetGifts(GiftQuery query)
		{
			return SalesProvider.Instance().GetGifts(query);
		}
		public static bool ClearOrderGifts(OrderInfo order)
		{
			ManagerHelper.CheckPrivilege(Privilege.EditOrders);
			Database database = DatabaseFactory.CreateDatabase();
			bool result;
			bool flag;
			using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					SalesProvider salesProvider = SalesProvider.Instance();
					order.Gifts.Clear();
					if (!salesProvider.ClearOrderGifts(order.OrderId, dbTransaction))
					{
						dbTransaction.Rollback();
						result = false;
						return result;
					}
					if (!salesProvider.UpdateOrderAmount(order, dbTransaction))
					{
						dbTransaction.Rollback();
						result = false;
						return result;
					}
					dbTransaction.Commit();
					flag = true;
				}
				catch
				{
					dbTransaction.Rollback();
					flag = false;
				}
				finally
				{
					dbConnection.Close();
				}
			}
			if (flag)
			{
				EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "清空了订单号为\"{0}\"的订单礼品", new object[]
				{
					order.OrderId
				}));
			}
			result = flag;
			return result;
		}
		public static bool AddOrderGift(OrderInfo order, GiftInfo giftinfo, int quantity, int promotype)
		{
			ManagerHelper.CheckPrivilege(Privilege.EditOrders);
			Database database = DatabaseFactory.CreateDatabase();
			bool result;
			bool flag2;
			using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					SalesProvider salesProvider = SalesProvider.Instance();
					OrderGiftInfo orderGiftInfo = new OrderGiftInfo();
					orderGiftInfo.OrderId = order.OrderId;
					orderGiftInfo.Quantity = quantity;
					orderGiftInfo.GiftName = giftinfo.Name;
					decimal arg_5C_0 = orderGiftInfo.CostPrice;
					orderGiftInfo.CostPrice = Convert.ToDecimal(giftinfo.CostPrice);
					orderGiftInfo.GiftId = giftinfo.GiftId;
					orderGiftInfo.ThumbnailsUrl = giftinfo.ThumbnailUrl40;
					orderGiftInfo.PromoteType = promotype;
					bool flag = false;
					foreach (OrderGiftInfo current in order.Gifts)
					{
						if (giftinfo.GiftId == current.GiftId)
						{
							flag = true;
							current.Quantity = quantity;
							current.PromoteType = promotype;
							break;
						}
					}
					if (!flag)
					{
						order.Gifts.Add(orderGiftInfo);
					}
					if (!salesProvider.AddOrderGift(order.OrderId, orderGiftInfo, quantity, dbTransaction))
					{
						dbTransaction.Rollback();
						result = false;
						return result;
					}
					if (!salesProvider.UpdateOrderAmount(order, dbTransaction))
					{
						dbTransaction.Rollback();
						result = false;
						return result;
					}
					dbTransaction.Commit();
					flag2 = true;
				}
				catch
				{
					dbTransaction.Rollback();
					flag2 = false;
				}
				finally
				{
					dbConnection.Close();
				}
			}
			if (flag2)
			{
				EventLogs.WriteOperationLog(Privilege.EditOrders, string.Format(CultureInfo.InvariantCulture, "成功的为订单号为\"{0}\"的订单添加了礼品", new object[]
				{
					order.OrderId
				}));
			}
			result = flag2;
			return result;
		}
		public static IList<GiftInfo> GetGiftList(GiftQuery query)
		{
			return SalesProvider.Instance().GetGiftList(query);
		}
		public static System.Data.DataSet GetTradeOrders(OrderQuery query, out int records)
		{
			return SalesProvider.Instance().GetTradeOrders(query, out records);
		}
		public static System.Data.DataSet GetTradeOrders(string orderId)
		{
			return SalesProvider.Instance().GetTradeOrders(orderId);
		}
		public static bool SendAPIGoods(OrderInfo order)
		{
			bool result = false;
			if (order.CheckAction(OrderActions.SELLER_SEND_GOODS))
			{
				order.OrderStatus = OrderStatus.SellerAlreadySent;
				if (result = (SalesProvider.Instance().SendGoods(order) > 0))
				{
					EventLogs.WriteOperationLog(Privilege.OrderSendGoods, string.Format(CultureInfo.InvariantCulture, "发货编号为\"{0}\"的订单", new object[]
					{
						order.OrderId
					}));
				}
			}
			return result;
		}
		public static bool SaveRemarkAPI(OrderInfo order)
		{
			bool result;
			if (result = SalesProvider.Instance().SaveOrderRemark(order))
			{
				EventLogs.WriteOperationLog(Privilege.RemarkOrder, string.Format(CultureInfo.InvariantCulture, "对订单“{0}”进行了备注", new object[]
				{
					order.OrderId
				}));
			}
			return result;
		}
		public static System.Data.DataSet GetOrdersAndLines(string orderIds)
		{
			return SalesProvider.Instance().GetOrdersAndLines(orderIds);
		}
		public static System.Data.DataSet GetOrderGoods(string orderIds)
		{
			return SalesProvider.Instance().GetOrderGoods(orderIds);
		}
		public static System.Data.DataSet GetProductGoods(string orderIds)
		{
			return SalesProvider.Instance().GetProductGoods(orderIds);
		}
		public static System.Data.DataSet GetPurchaseOrderGoods(string purchaseorderIds)
		{
			return SalesProvider.Instance().GetPurchaseOrderGoods(purchaseorderIds);
		}
		public static System.Data.DataSet GetPurchaseProductGoods(string purchaseorderIds)
		{
			return SalesProvider.Instance().GetPurchaseProductGoods(purchaseorderIds);
		}
		public static bool CheckRefund(OrderInfo order, string Operator, string adminRemark, int refundType, bool accept)
		{
			ManagerHelper.CheckPrivilege(Privilege.OrderRefundApply);
			bool result;
			if (order.OrderStatus != OrderStatus.ApplyForRefund)
			{
				result = false;
			}
			else
			{
				bool flag;
				if (flag = SalesProvider.Instance().CheckRefund(order.OrderId, Operator, adminRemark, refundType, accept))
				{
					if (accept)
					{
						IUser user = Users.GetUser(order.UserId, false);
						if (user != null && user.UserRole == UserRole.Member)
						{
							OrderHelper.ReducedPoint(order, user as Member);
							OrderHelper.ReduceDeduct(order.OrderId, order.GetTotal(), user as Member);
							Users.ClearUserCache(user);
						}
						OrderHelper.UpdateUserStatistics(order.UserId, order.RefundAmount, true);
						SalesProvider.Instance().UpdateRefundOrderStock(order.OrderId);
					}
					if (accept && order.GroupBuyId > 0)
					{
						EventLogs.WriteOperationLog(Privilege.RefundOrder, string.Format(CultureInfo.InvariantCulture, "对订单“{0}”成功的扣除违约金后退款", new object[]
						{
							order.OrderId
						}));
					}
					else
					{
						EventLogs.WriteOperationLog(Privilege.RefundOrder, string.Format(CultureInfo.InvariantCulture, "对订单“{0}”成功的进行了全额退款", new object[]
						{
							order.OrderId
						}));
					}
				}
				result = flag;
			}
			return result;
		}
		public static void GetRefundType(string orderId, out int refundType, out string remark)
		{
			SalesProvider.Instance().GetRefundType(orderId, out refundType, out remark);
		}
		public static bool CheckReturn(OrderInfo order, string Operator, decimal refundMoney, string adminRemark, int refundType, bool accept)
		{
			ManagerHelper.CheckPrivilege(Privilege.OrderReturnsApply);
			bool result;
			if (order.OrderStatus != OrderStatus.ApplyForReturns)
			{
				result = false;
			}
			else
			{
				bool flag;
				if (flag = SalesProvider.Instance().CheckReturn(order.OrderId, Operator, refundMoney, adminRemark, refundType, accept))
				{
					if (accept)
					{
						order.RefundAmount = refundMoney;
						IUser user = Users.GetUser(order.UserId, false);
						if (user != null && user.UserRole == UserRole.Member)
						{
							OrderHelper.ReducedPoint(order, user as Member);
							OrderHelper.ReduceDeduct(order.OrderId, order.RefundAmount, user as Member);
							Users.ClearUserCache(user);
						}
						OrderHelper.UpdateUserStatistics(order.UserId, order.RefundAmount, false);
					}
					EventLogs.WriteOperationLog(Privilege.RefundOrder, string.Format(CultureInfo.InvariantCulture, "对订单“{0}”成功的进行了退货", new object[]
					{
						order.OrderId
					}));
				}
				result = flag;
			}
			return result;
		}
		public static void GetRefundTypeFromReturn(string orderId, out int refundType, out string remark)
		{
			SalesProvider.Instance().GetRefundTypeFromReturn(orderId, out refundType, out remark);
		}
		public static bool CheckReplace(string orderId, string adminRemark, bool accept)
		{
			ManagerHelper.CheckPrivilege(Privilege.OrderReplaceApply);
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderId);
			return orderInfo.OrderStatus == OrderStatus.ApplyForReplacement && SalesProvider.Instance().CheckReplace(orderId, adminRemark, accept);
		}
		public static string GetReplaceComments(string orderId)
		{
			return SalesProvider.Instance().GetReplaceComments(orderId);
		}
		public static DbQueryResult GetRefundApplys(RefundApplyQuery query)
		{
			return SalesProvider.Instance().GetRefundApplys(query);
		}
		public static bool DelRefundApply(string[] refundIds, out int count)
		{
			ManagerHelper.CheckPrivilege(Privilege.OrderRefundApply);
			bool result = true;
			count = 0;
			for (int i = 0; i < refundIds.Length; i++)
			{
				string text = refundIds[i];
				if (!string.IsNullOrEmpty(text))
				{
					if (SalesProvider.Instance().DelRefundApply(int.Parse(text)))
					{
						count++;
					}
					else
					{
						result = false;
					}
				}
			}
			return result;
		}
		public static DbQueryResult GetReturnsApplys(ReturnsApplyQuery query)
		{
			return SalesProvider.Instance().GetReturnsApplys(query);
		}
		public static bool DelReturnsApply(string[] returnsIds, out int count)
		{
			ManagerHelper.CheckPrivilege(Privilege.OrderReturnsApply);
			bool result = true;
			count = 0;
			for (int i = 0; i < returnsIds.Length; i++)
			{
				string text = returnsIds[i];
				if (!string.IsNullOrEmpty(text))
				{
					if (SalesProvider.Instance().DelReturnsApply(int.Parse(text)))
					{
						count++;
					}
					else
					{
						result = false;
					}
				}
			}
			return result;
		}
		public static DbQueryResult GetReplaceApplys(ReplaceApplyQuery query)
		{
			return SalesProvider.Instance().GetReplaceApplys(query);
		}
		public static bool DelReplaceApply(string[] replaceIds, out int count)
		{
			bool result = true;
			count = 0;
			for (int i = 0; i < replaceIds.Length; i++)
			{
				string text = replaceIds[i];
				if (!string.IsNullOrEmpty(text))
				{
					if (SalesProvider.Instance().DelReplaceApply(int.Parse(text)))
					{
						count++;
					}
					else
					{
						result = false;
					}
				}
			}
			return result;
		}
		public static bool SaveDebitNote(DebitNote note)
		{
			return SalesProvider.Instance().SaveDebitNote(note);
		}
		public static DbQueryResult GetAllDebitNote(DebitNoteQuery query)
		{
			return SalesProvider.Instance().GetAllDebitNote(query);
		}
		public static bool DelDebitNote(string[] noteIds, out int count)
		{
			ManagerHelper.CheckPrivilege(Privilege.DeleteOrder);
			bool flag = true;
			count = 0;
			for (int i = 0; i < noteIds.Length; i++)
			{
				string text = noteIds[i];
				if (!string.IsNullOrEmpty(text) && (flag &= SalesProvider.Instance().DelDebitNote(text)))
				{
					count++;
				}
			}
			return flag;
		}
		public static bool SaveSendNote(SendNote note)
		{
			return SalesProvider.Instance().SaveSendNote(note);
		}
		public static DbQueryResult GetAllSendNote(RefundApplyQuery query)
		{
			return SalesProvider.Instance().GetAllSendNote(query);
		}
		public static bool DelSendNote(string[] noteIds, out int count)
		{
			ManagerHelper.CheckPrivilege(Privilege.DeleteOrder);
			bool flag = true;
			count = 0;
			for (int i = 0; i < noteIds.Length; i++)
			{
				string text = noteIds[i];
				if (!string.IsNullOrEmpty(text) && (flag &= SalesProvider.Instance().DelSendNote(text)))
				{
					count++;
				}
			}
			return flag;
		}
	}
}
