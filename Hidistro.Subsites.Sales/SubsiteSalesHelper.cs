using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using Hidistro.Subsites.Members;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace Hidistro.Subsites.Sales
{
	public static class SubsiteSalesHelper
	{
		public static bool SetMyShipper(ShippersInfo shipper)
		{
			ShippersInfo myShipper = SubsiteSalesProvider.Instance().GetMyShipper();
			Globals.EntityCoding(shipper, true);
			bool result;
			if (myShipper == null)
			{
				result = SubsiteSalesProvider.Instance().AddShipper(shipper);
			}
			else
			{
				result = SubsiteSalesProvider.Instance().UpdateShipper(shipper);
			}
			return result;
		}
		public static ShippersInfo GetMyShipper()
		{
			return SubsiteSalesProvider.Instance().GetMyShipper();
		}
		public static void SwapPaymentModeSequence(int modeId, int replaceModeId, int displaySequence, int replaceDisplaySequence)
		{
			SubsiteSalesProvider.Instance().SwapPaymentModeSequence(modeId, replaceModeId, displaySequence, replaceDisplaySequence);
		}
		public static PaymentModeActionStatus CreatePaymentMode(PaymentModeInfo paymentMode)
		{
			PaymentModeActionStatus result;
			if (null == paymentMode)
			{
				result = PaymentModeActionStatus.UnknowError;
			}
			else
			{
				Globals.EntityCoding(paymentMode, true);
				result = SubsiteSalesProvider.Instance().CreateUpdateDeletePaymentMode(paymentMode, DataProviderAction.Create);
			}
			return result;
		}
		public static PaymentModeActionStatus UpdatePaymentMode(PaymentModeInfo paymentMode)
		{
			PaymentModeActionStatus result;
			if (null == paymentMode)
			{
				result = PaymentModeActionStatus.UnknowError;
			}
			else
			{
				Globals.EntityCoding(paymentMode, true);
				result = SubsiteSalesProvider.Instance().CreateUpdateDeletePaymentMode(paymentMode, DataProviderAction.Update);
			}
			return result;
		}
		public static bool DeletePaymentMode(int modeId)
		{
			PaymentModeInfo paymentModeInfo = new PaymentModeInfo();
			paymentModeInfo.ModeId = modeId;
			PaymentModeActionStatus paymentModeActionStatus = SubsiteSalesProvider.Instance().CreateUpdateDeletePaymentMode(paymentModeInfo, DataProviderAction.Delete);
			return paymentModeActionStatus == PaymentModeActionStatus.Success;
		}
		public static IList<PaymentModeInfo> GetPaymentModes()
		{
			return SubsiteSalesProvider.Instance().GetPaymentModes();
		}
		public static PaymentModeInfo GetPaymentMode(int modeId)
		{
			return SubsiteSalesProvider.Instance().GetPaymentMode(modeId);
		}
		public static PaymentModeInfo GetPaymentMode(string gateway)
		{
			return SubsiteSalesProvider.Instance().GetPaymentMode(gateway);
		}
		public static DbQueryResult GetOrders(OrderQuery query)
		{
			return SubsiteSalesProvider.Instance().GetOrders(query);
		}
		public static DbQueryResult GetSendGoodsOrders(OrderQuery query)
		{
			return SubsiteSalesProvider.Instance().GetSendGoodsOrders(query);
		}
		public static int DeleteOrders(string orderIds)
		{
			return SubsiteSalesProvider.Instance().DeleteOrders(orderIds);
		}
		public static System.Data.DataTable GetRecentlyOrders(out int number)
		{
			return SubsiteSalesProvider.Instance().GetRecentlyOrders(out number);
		}
		public static OrderInfo GetOrderInfo(string orderId)
		{
			return SubsiteSalesProvider.Instance().GetOrderInfo(orderId);
		}
		public static bool ConfirmPay(OrderInfo order)
		{
			bool result;
			bool flag;
			if (order.CheckAction(OrderActions.SELLER_CONFIRM_PAY))
			{
				Database database = DatabaseFactory.CreateDatabase();
				using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
				{
					dbConnection.Open();
					System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
					try
					{
						SubsiteSalesProvider subsiteSalesProvider = SubsiteSalesProvider.Instance();
						if (subsiteSalesProvider.ConfirmPay(order, dbTransaction) <= 0)
						{
							dbTransaction.Rollback();
							result = false;
							return result;
						}
						if (order.GroupBuyId <= 0)
						{
							PurchaseOrderInfo purchaseOrder = subsiteSalesProvider.ConvertOrderToPurchaseOrder(order);
							if (!subsiteSalesProvider.CreatePurchaseOrder(purchaseOrder, dbTransaction))
							{
								dbTransaction.Rollback();
								result = false;
								return result;
							}
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
					SubsiteSalesProvider.Instance().UpdateProductSaleCounts(order.LineItems);
					SubsiteSalesHelper.UpdateUserAccount(order);
				}
			}
			else
			{
				flag = false;
			}
			result = flag;
			return result;
		}
		public static bool CreatePurchaseOrder(OrderInfo order)
		{
			bool result;
			if (order.CheckAction(OrderActions.SUBSITE_CREATE_PURCHASEORDER))
			{
				Database database = DatabaseFactory.CreateDatabase();
				using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
				{
					dbConnection.Open();
					System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
					try
					{
						SubsiteSalesProvider subsiteSalesProvider = SubsiteSalesProvider.Instance();
						PurchaseOrderInfo purchaseOrder = subsiteSalesProvider.ConvertOrderToPurchaseOrder(order);
						if (!subsiteSalesProvider.CreatePurchaseOrder(purchaseOrder, dbTransaction))
						{
							dbTransaction.Rollback();
							result = false;
							return result;
						}
						dbTransaction.Commit();
						result = true;
						return result;
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
			}
			result = false;
			return result;
		}
		public static bool CloseTransaction(OrderInfo order)
		{
			return order.CheckAction(OrderActions.SELLER_CLOSE) && SubsiteSalesProvider.Instance().CloseTransaction(order);
		}
		public static bool ConfirmOrderFinish(OrderInfo order)
		{
			return order.CheckAction(OrderActions.SELLER_FINISH_TRADE) && SubsiteSalesProvider.Instance().ConfirmOrderFinish(order);
		}
		public static PurchaseOrderTaobaoInfo GetPurchaseOrderTaobaoInfo(string tbOrderId)
		{
			return SubsiteSalesProvider.Instance().GetPurchaseOrderTaobaoInfo(tbOrderId);
		}
		public static bool CreatePurchaseOrder(PurchaseOrderInfo purchaseOrderInfo)
		{
			return SubsiteSalesProvider.Instance().CreatePurchaseOrder(purchaseOrderInfo, null);
		}
		public static bool SaveRemark(OrderInfo order)
		{
			return SubsiteSalesProvider.Instance().SaveRemark(order);
		}
		private static void UpdateUserStatistics(int userId, decimal refundAmount, bool isAllRefund)
		{
			SubsiteSalesProvider.Instance().UpdateUserStatistics(userId, refundAmount, isAllRefund);
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
			SubsiteSalesProvider.Instance().AddMemberPoint(userPointInfo);
		}
		private static void ReduceDeduct(string orderId, decimal refundAmount, Member member)
		{
			SiteSettings siteSettings = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId);
			int referralDeduct = siteSettings.ReferralDeduct;
			if (referralDeduct > 0 && member.ReferralUserId.HasValue && member.ReferralUserId.Value > 0)
			{
				IUser user = Users.GetUser(member.ReferralUserId.Value, false);
				if (user != null && user.UserRole == UserRole.Underling)
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
						UnderlingProvider.Instance().AddUnderlingBalanceDetail(balanceDetailInfo);
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
			if (user != null && user.UserRole == UserRole.Underling)
			{
				Member member = user as Member;
				UserPointInfo userPointInfo = new UserPointInfo();
				userPointInfo.OrderId = order.OrderId;
				userPointInfo.UserId = member.UserId;
				userPointInfo.TradeDate = DateTime.Now;
				userPointInfo.TradeType = UserPointTradeType.Bounty;
				userPointInfo.Increased = new int?(order.Points);
				userPointInfo.Points = order.Points + member.Points;
				int arg_9A_0 = userPointInfo.Points;
				if (userPointInfo.Points < 0)
				{
					userPointInfo.Points = 2147483647;
				}
				SubsiteSalesProvider.Instance().AddMemberPoint(userPointInfo);
				SiteSettings siteSettings = SettingsManager.GetSiteSettings(HiContext.Current.User.UserId);
				int referralDeduct = siteSettings.ReferralDeduct;
				if (referralDeduct > 0 && member.ReferralUserId.HasValue && member.ReferralUserId.Value > 0)
				{
					IUser user2 = Users.GetUser(member.ReferralUserId.Value, false);
					if (user2 != null && user2.UserRole == UserRole.Underling)
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
							UnderlingProvider.Instance().AddUnderlingBalanceDetail(balanceDetailInfo);
						}
					}
				}
				SubsiteSalesProvider.Instance().UpdateUserAccount(order.GetTotal(), userPointInfo.Points, order.UserId);
				int historyPoint = SubsiteSalesProvider.Instance().GetHistoryPoint(member.UserId);
				SubsiteSalesProvider.Instance().ChangeMemberGrade(member.UserId, member.GradeId, historyPoint);
			}
			Users.ClearUserCache(user);
		}
		public static bool SendGoods(OrderInfo order)
		{
			bool result = false;
			if (order.CheckAction(OrderActions.SELLER_SEND_GOODS))
			{
				order.OrderStatus = OrderStatus.SellerAlreadySent;
				result = (SubsiteSalesProvider.Instance().SendGoods(order) > 0);
			}
			return result;
		}
		public static bool UpdateOrderAmount(OrderInfo order)
		{
			return order.CheckAction(OrderActions.SELLER_MODIFY_TRADE) && SubsiteSalesProvider.Instance().UpdateOrderAmount(order, null);
		}
		public static bool DeleteOrderGift(OrderInfo order, int giftId)
		{
			Database database = DatabaseFactory.CreateDatabase();
			bool result;
			bool flag;
			using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					if (!SubsiteSalesProvider.Instance().DeleteOrderGift(order.OrderId, giftId, dbTransaction))
					{
						dbTransaction.Rollback();
						result = false;
						return result;
					}
					foreach (OrderGiftInfo current in order.Gifts)
					{
						if (current.GiftId == giftId)
						{
							order.Gifts.Remove(current);
							break;
						}
					}
					if (!SubsiteSalesProvider.Instance().UpdateOrderAmount(order, dbTransaction))
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
			result = flag;
			return result;
		}
		public static bool DeleteOrderProduct(string string_0, OrderInfo order)
		{
			Database database = DatabaseFactory.CreateDatabase();
			bool result;
			bool flag;
			using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					if (!SubsiteSalesProvider.Instance().DeleteOrderProduct(string_0, order.OrderId, dbTransaction))
					{
						dbTransaction.Rollback();
						result = false;
						return result;
					}
					order.LineItems.Remove(string_0);
					if (!SubsiteSalesProvider.Instance().UpdateOrderAmount(order, dbTransaction))
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
			result = flag;
			return result;
		}
		public static bool UpdateLineItem(string string_0, OrderInfo order, int quantity)
		{
			Database database = DatabaseFactory.CreateDatabase();
			bool result;
			using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					SubsiteSalesProvider subsiteSalesProvider = SubsiteSalesProvider.Instance();
					order.LineItems[string_0].ShipmentQuantity = quantity;
					order.LineItems[string_0].Quantity = quantity;
					order.LineItems[string_0].ItemAdjustedPrice = order.LineItems[string_0].ItemListPrice;
					if (!subsiteSalesProvider.UpdateLineItem(order.OrderId, order.LineItems[string_0], dbTransaction))
					{
						dbTransaction.Rollback();
						result = false;
					}
					else
					{
						if (!subsiteSalesProvider.UpdateOrderAmount(order, dbTransaction))
						{
							dbTransaction.Rollback();
							result = false;
						}
						else
						{
							dbTransaction.Commit();
							result = true;
						}
					}
				}
				catch (Exception)
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
		public static bool MondifyAddress(OrderInfo order)
		{
			return order.CheckAction(OrderActions.SUBSITE_SELLER_MODIFY_DELIVER_ADDRESS) && SubsiteSalesProvider.Instance().SaveShippingAddress(order);
		}
		public static bool UpdateOrderShippingMode(OrderInfo order)
		{
			return order.CheckAction(OrderActions.SUBSITE_SELLER_MODIFY_SHIPPING_MODE) && SubsiteSalesProvider.Instance().UpdateOrderShippingMode(order);
		}
		public static bool UpdateOrderPaymentType(OrderInfo order)
		{
			return order.CheckAction(OrderActions.SUBSITE_SELLER_MODIFY_PAYMENT_MODE) && SubsiteSalesProvider.Instance().UpdateOrderPaymentType(order);
		}
		public static int GetSkuStock(string skuId)
		{
			return SubsiteSalesProvider.Instance().GetSkuStock(skuId);
		}
		public static bool GetAlertStock(string skuId)
		{
			return SubsiteSalesProvider.Instance().GetAlertStock(skuId);
		}
		public static LineItemInfo GetLineItemInfo(string string_0, string orderId)
		{
			return SubsiteSalesProvider.Instance().GetLineItemInfo(string_0, orderId);
		}
		public static bool UpdateOrderItem(string orderId, LineItemInfo lineItem)
		{
			return SubsiteSalesProvider.Instance().UpdateLineItem(orderId, lineItem, null);
		}
		public static DbQueryResult GetOrderGifts(OrderGiftQuery query)
		{
			return SubsiteSalesProvider.Instance().GetOrderGifts(query);
		}
		public static bool ClearOrderGifts(OrderInfo order)
		{
			Database database = DatabaseFactory.CreateDatabase();
			bool result;
			bool flag;
			using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					if (!SubsiteSalesProvider.Instance().ClearOrderGifts(order.OrderId, dbTransaction))
					{
						dbTransaction.Rollback();
						result = false;
						return result;
					}
					order.Gifts.Clear();
					if (!SubsiteSalesProvider.Instance().UpdateOrderAmount(order, dbTransaction))
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
			result = flag;
			return result;
		}
		public static bool AddOrderGift(OrderInfo order, GiftInfo gift, int quantity, int promotype)
		{
			Database database = DatabaseFactory.CreateDatabase();
			bool result;
			bool flag2;
			using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					if (!SubsiteSalesProvider.Instance().AddOrderGift(order.OrderId, gift, quantity, promotype, dbTransaction))
					{
						dbTransaction.Rollback();
						result = false;
						return result;
					}
					bool flag = false;
					foreach (OrderGiftInfo current in order.Gifts)
					{
						if (current.GiftId == gift.GiftId)
						{
							flag = true;
							current.Quantity += quantity;
						}
					}
					if (!flag)
					{
						OrderGiftInfo orderGiftInfo = new OrderGiftInfo();
						orderGiftInfo.GiftId = gift.GiftId;
						orderGiftInfo.OrderId = order.OrderId;
						orderGiftInfo.GiftName = gift.Name;
						orderGiftInfo.Quantity = quantity;
						orderGiftInfo.CostPrice = gift.PurchasePrice;
						orderGiftInfo.ThumbnailsUrl = gift.ThumbnailUrl40;
						orderGiftInfo.PromoteType = promotype;
						order.Gifts.Add(orderGiftInfo);
					}
					if (!SubsiteSalesProvider.Instance().UpdateOrderAmount(order, dbTransaction))
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
			result = flag2;
			return result;
		}
		public static DbQueryResult GetPurchaseOrders(PurchaseOrderQuery query)
		{
			return SubsiteSalesProvider.Instance().GetPurchaseOrders(query);
		}
		public static PurchaseOrderInfo GetPurchaseOrder(string purchaseOrderId)
		{
			return SubsiteSalesProvider.Instance().GetPurchaseOrder(purchaseOrderId);
		}
		public static System.Data.DataTable GetRecentlyPurchaseOrders(out int number)
		{
			return SubsiteSalesProvider.Instance().GetRecentlyPurchaseOrders(out number);
		}
		public static System.Data.DataTable GetRecentlyManualPurchaseOrders(out int number)
		{
			return SubsiteSalesProvider.Instance().GetRecentlyManualPurchaseOrders(out number);
		}
		public static PurchaseOrderInfo GetPurchaseByOrderId(string orderId)
		{
			return SubsiteSalesProvider.Instance().GetPurchaseByOrderId(orderId);
		}
		public static int DeletePurchaseOrde(string purchaseOrderId)
		{
			return SubsiteSalesProvider.Instance().DeletePurchaseOrde(purchaseOrderId);
		}
		public static IList<PurchaseShoppingCartItemInfo> GetPurchaseShoppingCartItemInfos()
		{
			return SubsiteSalesProvider.Instance().GetPurchaseShoppingCartItemInfos();
		}
		public static bool AddPurchaseItem(PurchaseShoppingCartItemInfo item)
		{
			return SubsiteSalesProvider.Instance().AddPurchaseItem(item);
		}
		public static bool AddPurchaseOrderItem(PurchaseShoppingCartItemInfo item, string POrderId)
		{
			SubsiteSalesProvider subsiteSalesProvider = SubsiteSalesProvider.Instance();
			int currentPOrderItemQuantity = subsiteSalesProvider.GetCurrentPOrderItemQuantity(POrderId, item.SkuId);
			bool result;
			if (currentPOrderItemQuantity == 0)
			{
				result = subsiteSalesProvider.AddPurchaseOrderItem(item, POrderId);
			}
			else
			{
				result = subsiteSalesProvider.UpdatePurchaseOrderQuantity(POrderId, item.SkuId, item.Quantity + currentPOrderItemQuantity);
			}
			return result;
		}
		public static bool UpdatePurchaseOrderItemQuantity(string POrderId, string SkuId, int Quantity)
		{
			return SubsiteSalesProvider.Instance().UpdatePurchaseOrderQuantity(POrderId, SkuId, Quantity);
		}
		public static bool UpdatePurchaseOrder(PurchaseOrderInfo purchaseOrder)
		{
			return SubsiteSalesProvider.Instance().UpdatePurchaseOrder(purchaseOrder);
		}
		public static bool DeletePurchaseOrderItem(string POrderId, string SkuId)
		{
			return SubsiteSalesProvider.Instance().DeletePurchaseOrderItem(POrderId, SkuId);
		}
		public static bool DeletePurchaseShoppingCartItem(string string_0)
		{
			return SubsiteSalesProvider.Instance().DeletePurchaseShoppingCartItem(string_0);
		}
		public static void ClearPurchaseShoppingCart()
		{
			SubsiteSalesProvider.Instance().ClearPurchaseShoppingCart();
		}
		public static bool IsExitPurchaseOrder(long long_0)
		{
			return SubsiteSalesProvider.Instance().IsExitPurchaseOrder(long_0);
		}
		public static decimal GetRefundMoney(PurchaseOrderInfo purchaseOrder, out decimal refundMoney)
		{
			return SubsiteSalesProvider.Instance().GetRefundMoney(purchaseOrder, out refundMoney);
		}
		public static bool SetPayment(string purchaseOrderId, int paymentTypeId, string paymentType, string gateway)
		{
			return SubsiteSalesProvider.Instance().SetPayment(purchaseOrderId, paymentTypeId, paymentType, gateway);
		}
		public static bool ClosePurchaseOrder(PurchaseOrderInfo purchaseOrder)
		{
			return purchaseOrder.CheckAction(PurchaseOrderActions.DISTRIBUTOR_CLOSE) && SubsiteSalesProvider.Instance().ClosePurchaseOrder(purchaseOrder);
		}
		public static bool ConfirmPurchaseOrderFinish(PurchaseOrderInfo purchaseOrder)
		{
			return purchaseOrder.CheckAction(PurchaseOrderActions.DISTRIBUTOR_CONFIRM_GOODS) && SubsiteSalesProvider.Instance().ConfirmPurchaseOrderFinish(purchaseOrder);
		}
		public static bool ConfirmPay(BalanceDetailInfo balance, PurchaseOrderInfo purchaseOrder)
		{
			bool result;
			if (!purchaseOrder.CheckAction(PurchaseOrderActions.DISTRIBUTOR_CONFIRM_PAY))
			{
				result = false;
			}
			else
			{
				bool flag;
				if (flag = SubsiteSalesProvider.Instance().ConfirmPay(balance, purchaseOrder.PurchaseOrderId))
				{
					SubsiteSalesProvider.Instance().UpdateProductStock(purchaseOrder.PurchaseOrderId);
					SubsiteSalesProvider.Instance().UpdateDistributorAccount(purchaseOrder.GetPurchaseTotal());
					Users.ClearUserCache(Users.GetUser(purchaseOrder.DistributorId));
				}
				result = flag;
			}
			return result;
		}
		public static bool BatchConfirmPay(BalanceDetailInfo balance, string purchaseOrderIds)
		{
			return SubsiteSalesProvider.Instance().BatchConfirmPay(balance, purchaseOrderIds);
		}
		public static bool GetNotPayment(string purchaseOrderId)
		{
			return SubsiteSalesProvider.Instance().GetNotPayment(purchaseOrderId);
		}
		public static bool ConfirmPay(PurchaseOrderInfo purchaseOrder)
		{
			bool result;
			if (!purchaseOrder.CheckAction(PurchaseOrderActions.DISTRIBUTOR_CONFIRM_PAY))
			{
				result = false;
			}
			else
			{
				bool flag;
				if (flag = SubsiteSalesProvider.Instance().ConfirmPay(purchaseOrder.PurchaseOrderId))
				{
					SubsiteSalesProvider.Instance().UpdateProductStock(purchaseOrder.PurchaseOrderId);
					SubsiteSalesProvider.Instance().UpdateDistributorAccount(purchaseOrder.GetPurchaseTotal());
					Users.ClearUserCache(Users.GetUser(purchaseOrder.DistributorId));
				}
				result = flag;
			}
			return result;
		}
		public static bool BatchConfirmPay(string purchaseOrderIds)
		{
			return SubsiteSalesProvider.Instance().BatchConfirmPay(purchaseOrderIds);
		}
		public static bool SavePurchaseOrderRemark(string purchaseOrderId, string remark)
		{
			return SubsiteSalesProvider.Instance().SavePurchaseOrderRemark(purchaseOrderId, remark);
		}
		public static DbQueryResult GetPurchaseOrderGifts(PurchaseOrderGiftQuery query)
		{
			return SubsiteSalesProvider.Instance().GetPurchaseOrderGifts(query);
		}
		public static bool DeletePurchaseOrderGift(PurchaseOrderInfo purchaseOrder, int giftId)
		{
			Database database = DatabaseFactory.CreateDatabase();
			bool result;
			bool flag;
			using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					if (!SubsiteSalesProvider.Instance().DeletePurchaseOrderGift(purchaseOrder.PurchaseOrderId, giftId, dbTransaction))
					{
						dbTransaction.Rollback();
						result = false;
						return result;
					}
					foreach (PurchaseOrderGiftInfo current in purchaseOrder.PurchaseOrderGifts)
					{
						if (current.GiftId == giftId)
						{
							purchaseOrder.PurchaseOrderGifts.Remove(current);
							break;
						}
					}
					if (!SubsiteSalesProvider.Instance().ResetPurchaseTotal(purchaseOrder, dbTransaction))
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
			result = flag;
			return result;
		}
		public static bool ClearPurchaseOrderGifts(PurchaseOrderInfo purchaseOrder)
		{
			Database database = DatabaseFactory.CreateDatabase();
			bool result;
			bool flag;
			using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					if (!SubsiteSalesProvider.Instance().ClearPurchaseOrderGifts(purchaseOrder.PurchaseOrderId, dbTransaction))
					{
						dbTransaction.Rollback();
						result = false;
						return result;
					}
					purchaseOrder.PurchaseOrderGifts.Clear();
					if (!SubsiteSalesProvider.Instance().ResetPurchaseTotal(purchaseOrder, dbTransaction))
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
			result = flag;
			return result;
		}
		public static bool ResetPurchaseTotal(PurchaseOrderInfo purchaseOrder)
		{
			return SubsiteSalesProvider.Instance().ResetPurchaseTotal(purchaseOrder, null);
		}
		public static bool AddPurchaseOrderGift(PurchaseOrderInfo purchaseOrder, GiftInfo gift, int quantity)
		{
			Database database = DatabaseFactory.CreateDatabase();
			bool result;
			bool flag2;
			using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					if (!SubsiteSalesProvider.Instance().AddPurchaseOrderGift(purchaseOrder.PurchaseOrderId, gift, quantity, dbTransaction))
					{
						dbTransaction.Rollback();
						result = false;
						return result;
					}
					bool flag = false;
					foreach (PurchaseOrderGiftInfo current in purchaseOrder.PurchaseOrderGifts)
					{
						if (current.GiftId == gift.GiftId)
						{
							flag = true;
							current.Quantity += quantity;
						}
					}
					if (!flag)
					{
						PurchaseOrderGiftInfo purchaseOrderGiftInfo = new PurchaseOrderGiftInfo();
						purchaseOrderGiftInfo.GiftId = gift.GiftId;
						purchaseOrderGiftInfo.PurchaseOrderId = purchaseOrder.PurchaseOrderId;
						purchaseOrderGiftInfo.GiftName = gift.Name;
						purchaseOrderGiftInfo.Quantity = quantity;
						purchaseOrderGiftInfo.PurchasePrice = gift.PurchasePrice;
						purchaseOrderGiftInfo.ThumbnailsUrl = gift.ThumbnailUrl40;
						purchaseOrder.PurchaseOrderGifts.Add(purchaseOrderGiftInfo);
					}
					if (!SubsiteSalesProvider.Instance().ResetPurchaseTotal(purchaseOrder, dbTransaction))
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
			result = flag2;
			return result;
		}
		public static ShippingModeInfo GetShippingMode(int modeId, bool includeDetail)
		{
			return SubsiteSalesProvider.Instance().GetShippingMode(modeId, includeDetail);
		}
		public static DbQueryResult GetGifts(GiftQuery query)
		{
			return SubsiteSalesProvider.Instance().GetGifts(query);
		}
		public static GiftInfo GetGiftDetails(int giftId)
		{
			return SubsiteSalesProvider.Instance().GetGiftDetails(giftId);
		}
		public static IList<string> GetExpressCompanysByMode(int modeId)
		{
			return SubsiteSalesProvider.Instance().GetExpressCompanysByMode(modeId);
		}
		public static StatisticsInfo GetStatistics()
		{
			return SubsiteSalesProvider.Instance().GetStatistics();
		}
		public static decimal GetDaySaleTotal(int year, int month, int int_0, SaleStatisticsType saleStatisticsType)
		{
			return SubsiteSalesProvider.Instance().GetDaySaleTotal(year, month, int_0, saleStatisticsType);
		}
		public static System.Data.DataTable GetDaySaleTotal(int year, int month, SaleStatisticsType saleStatisticsType)
		{
			return SubsiteSalesProvider.Instance().GetDaySaleTotal(year, month, saleStatisticsType);
		}
		public static decimal GetMonthSaleTotal(int year, int month, SaleStatisticsType saleStatisticsType)
		{
			return SubsiteSalesProvider.Instance().GetMonthSaleTotal(year, month, saleStatisticsType);
		}
		public static System.Data.DataTable GetMonthSaleTotal(int year, SaleStatisticsType saleStatisticsType)
		{
			return SubsiteSalesProvider.Instance().GetMonthSaleTotal(year, saleStatisticsType);
		}
		public static decimal GetYearSaleTotal(int year, SaleStatisticsType saleStatisticsType)
		{
			return SubsiteSalesProvider.Instance().GetYearSaleTotal(year, saleStatisticsType);
		}
		public static OrderStatisticsInfo GetUserOrders(UserOrderQuery userOrder)
		{
			return SubsiteSalesProvider.Instance().GetUserOrders(userOrder);
		}
		public static OrderStatisticsInfo GetUserOrdersNoPage(UserOrderQuery userOrder)
		{
			return SubsiteSalesProvider.Instance().GetUserOrdersNoPage(userOrder);
		}
		public static DbQueryResult GetSaleOrderLineItemsStatistics(SaleStatisticsQuery query)
		{
			return SubsiteSalesProvider.Instance().GetSaleOrderLineItemsStatistics(query);
		}
		public static DbQueryResult GetSaleTargets()
		{
			return SubsiteSalesProvider.Instance().GetSaleTargets();
		}
		public static System.Data.DataTable GetProductSales(SaleStatisticsQuery productSale, out int totalProductSales)
		{
			System.Data.DataTable result;
			if (productSale == null)
			{
				totalProductSales = 0;
				result = null;
			}
			else
			{
				result = SubsiteSalesProvider.Instance().GetProductSales(productSale, out totalProductSales);
			}
			return result;
		}
		public static System.Data.DataTable GetProductSalesNoPage(SaleStatisticsQuery productSale, out int totalProductSales)
		{
			System.Data.DataTable result;
			if (productSale == null)
			{
				totalProductSales = 0;
				result = null;
			}
			else
			{
				result = SubsiteSalesProvider.Instance().GetProductSalesNoPage(productSale, out totalProductSales);
			}
			return result;
		}
		public static System.Data.DataTable GetProductVisitAndBuyStatistics(SaleStatisticsQuery query, out int totalProductSales)
		{
			return SubsiteSalesProvider.Instance().GetProductVisitAndBuyStatistics(query, out totalProductSales);
		}
		public static System.Data.DataTable GetProductVisitAndBuyStatisticsNoPage(SaleStatisticsQuery query, out int totalProductSales)
		{
			return SubsiteSalesProvider.Instance().GetProductVisitAndBuyStatisticsNoPage(query, out totalProductSales);
		}
		public static IList<UserStatisticsInfo> GetUserStatistics(Pagination page, out int totalProductSaleVisits)
		{
			IList<UserStatisticsInfo> result;
			if (page == null)
			{
				totalProductSaleVisits = 0;
				result = null;
			}
			else
			{
				result = SubsiteSalesProvider.Instance().GetUserStatistics(page, out totalProductSaleVisits);
			}
			return result;
		}
		public static decimal CalcFreight(int regionId, decimal totalWeight, ShippingModeInfo shippingModeInfo)
		{
			decimal result = 0m;
			int topRegionId = RegionHelper.GetTopRegionId(regionId);
			int value = 1;
			if (totalWeight > shippingModeInfo.Weight && shippingModeInfo.AddWeight.HasValue && shippingModeInfo.AddWeight.Value > 0m)
			{
				if ((totalWeight - shippingModeInfo.Weight) % shippingModeInfo.AddWeight == 0m)
				{
					value = Convert.ToInt32(Math.Truncate((totalWeight - shippingModeInfo.Weight) / shippingModeInfo.AddWeight.Value));
				}
				else
				{
                    value = Convert.ToInt32(Math.Truncate(1 + ((totalWeight - shippingModeInfo.Weight) / shippingModeInfo.AddWeight.Value)));
				}
			}
			if (shippingModeInfo.ModeGroup == null || shippingModeInfo.ModeGroup.Count == 0)
			{
				if (totalWeight > shippingModeInfo.Weight && shippingModeInfo.AddPrice.HasValue)
				{
					result = value * shippingModeInfo.AddPrice.Value + shippingModeInfo.Price;
				}
				else
				{
					result = shippingModeInfo.Price;
				}
			}
			else
			{
				int? num = null;
				foreach (ShippingModeGroupInfo current in shippingModeInfo.ModeGroup)
				{
					foreach (ShippingRegionInfo current2 in current.ModeRegions)
					{
						if (topRegionId == current2.RegionId)
						{
							num = new int?(current2.GroupId);
							break;
						}
					}
					if (num.HasValue)
					{
						if (totalWeight > shippingModeInfo.Weight)
						{
							result = value * current.AddPrice + current.Price;
						}
						else
						{
							result = current.Price;
						}
						break;
					}
				}
				if (!num.HasValue)
				{
					if (totalWeight > shippingModeInfo.Weight && shippingModeInfo.AddPrice.HasValue)
					{
						result = value * shippingModeInfo.AddPrice.Value + shippingModeInfo.Price;
					}
					else
					{
						result = shippingModeInfo.Price;
					}
				}
			}
			return result;
		}
		public static bool CheckRefund(OrderInfo order, string adminRemark, int refundType, bool accept)
		{
			bool result;
			if (order.OrderStatus != OrderStatus.ApplyForRefund)
			{
				result = false;
			}
			else
			{
				bool flag;
				if ((flag = SubsiteSalesProvider.Instance().CheckRefund(order.OrderId, adminRemark, refundType, accept)) && accept)
				{
					IUser user = Users.GetUser(order.UserId, false);
					if (user != null && user.UserRole == UserRole.Underling)
					{
						SubsiteSalesHelper.ReducedPoint(order, user as Member);
						SubsiteSalesHelper.ReduceDeduct(order.OrderId, order.GetTotal(), user as Member);
						Users.ClearUserCache(user);
					}
					SubsiteSalesHelper.UpdateUserStatistics(order.UserId, order.RefundAmount, true);
				}
				result = flag;
			}
			return result;
		}
		public static void GetRefundType(string orderId, out int refundType, out string refundRemark)
		{
			SubsiteSalesProvider.Instance().GetRefundType(orderId, out refundType, out refundRemark);
		}
		public static bool CheckReturn(OrderInfo order, decimal refundMoney, string adminRemark, int refundType, bool accept)
		{
			bool result;
			if (order.OrderStatus != OrderStatus.ApplyForReturns)
			{
				result = false;
			}
			else
			{
				bool flag;
				if ((flag = SubsiteSalesProvider.Instance().CheckReturn(order.OrderId, refundMoney, adminRemark, refundType, accept)) && accept)
				{
					IUser user = Users.GetUser(order.UserId, false);
					if (user != null && user.UserRole == UserRole.Underling)
					{
						order.RefundAmount = refundMoney;
						SubsiteSalesHelper.ReducedPoint(order, user as Member);
						SubsiteSalesHelper.ReduceDeduct(order.OrderId, order.RefundAmount, user as Member);
						Users.ClearUserCache(user);
					}
					SubsiteSalesHelper.UpdateUserStatistics(order.UserId, order.RefundAmount, false);
				}
				result = flag;
			}
			return result;
		}
		public static void GetRefundTypeFromReturn(string orderId, out int refundType, out string refundRemark)
		{
			SubsiteSalesProvider.Instance().GetRefundTypeFromReturn(orderId, out refundType, out refundRemark);
		}
		public static DbQueryResult GetRefundApplys(RefundApplyQuery query)
		{
			return SubsiteSalesProvider.Instance().GetRefundApplys(query);
		}
		public static bool DelRefundApply(string[] refundIds, out int count)
		{
			bool result = true;
			count = 0;
			for (int i = 0; i < refundIds.Length; i++)
			{
				string text = refundIds[i];
				if (!string.IsNullOrEmpty(text))
				{
					if (SubsiteSalesProvider.Instance().DelRefundApply(int.Parse(text)))
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
			return SubsiteSalesProvider.Instance().GetReturnsApplys(query);
		}
		public static bool DelReturnsApply(string[] returnsIds, out int count)
		{
			bool result = true;
			count = 0;
			for (int i = 0; i < returnsIds.Length; i++)
			{
				string text = returnsIds[i];
				if (!string.IsNullOrEmpty(text))
				{
					if (SubsiteSalesProvider.Instance().DelReturnsApply(int.Parse(text)))
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
		public static bool CheckReplace(string orderId, string adminRemark, bool accept)
		{
			OrderInfo orderInfo = SubsiteSalesHelper.GetOrderInfo(orderId);
			return orderInfo.OrderStatus == OrderStatus.ApplyForReplacement && SubsiteSalesProvider.Instance().CheckReplace(orderId, adminRemark, accept);
		}
		public static string GetReplaceComments(string orderId)
		{
			return SubsiteSalesProvider.Instance().GetReplaceComments(orderId);
		}
		public static DbQueryResult GetReplaceApplys(ReplaceApplyQuery query)
		{
			return SubsiteSalesProvider.Instance().GetReplaceApplys(query);
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
					if (SubsiteSalesProvider.Instance().DelReplaceApply(int.Parse(text)))
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
			return SubsiteSalesProvider.Instance().SaveDebitNote(note);
		}
		public static bool ApplyForPurchaseRefund(string purchaseOrderId, string remark, int refundType)
		{
			return SubsiteSalesProvider.Instance().ApplyForPurchaseRefund(purchaseOrderId, remark, refundType);
		}
		public static bool CanPurchaseRefund(string purchaseOrderId)
		{
			return SubsiteSalesProvider.Instance().CanPurchaseRefund(purchaseOrderId);
		}
		public static bool ApplyForPurchaseReturn(string purchaseOrderId, string remark, int refundType)
		{
			return SubsiteSalesProvider.Instance().ApplyForPurchaseReturn(purchaseOrderId, remark, refundType);
		}
		public static bool CanPurchaseReturn(string purchaseOrderId)
		{
			return SubsiteSalesProvider.Instance().CanPurchaseReturn(purchaseOrderId);
		}
		public static bool ApplyForPurchaseReplace(string purchaseOrderId, string remark)
		{
			return SubsiteSalesProvider.Instance().ApplyForPurchaseReplace(purchaseOrderId, remark);
		}
		public static bool CanPurchaseReplace(string purchaseOrderId)
		{
			return SubsiteSalesProvider.Instance().CanPurchaseReplace(purchaseOrderId);
		}
		public static bool SavePurchaseDebitNote(PurchaseDebitNote note)
		{
			return SubsiteSalesProvider.Instance().SavePurchaseDebitNote(note);
		}
		public static DbQueryResult GetAllDebitNote(DebitNoteQuery query)
		{
			return SubsiteSalesProvider.Instance().GetAllDebitNote(query);
		}
		public static bool DelDebitNote(string[] noteIds, out int count)
		{
			bool flag = true;
			count = 0;
			for (int i = 0; i < noteIds.Length; i++)
			{
				string text = noteIds[i];
				if (!string.IsNullOrEmpty(text) && (flag &= SubsiteSalesProvider.Instance().DelDebitNote(text)))
				{
					count++;
				}
			}
			return flag;
		}
	}
}
