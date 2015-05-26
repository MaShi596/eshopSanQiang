using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
namespace Hidistro.ControlPanel.Sales
{
	public sealed class SalesHelper
	{
		private SalesHelper()
		{
		}
		public static bool AddShipper(ShippersInfo shipper)
		{
			Globals.EntityCoding(shipper, true);
			return SalesProvider.Instance().AddShipper(shipper);
		}
		public static bool UpdateShipper(ShippersInfo shipper)
		{
			Globals.EntityCoding(shipper, true);
			return SalesProvider.Instance().UpdateShipper(shipper);
		}
		public static bool DeleteShipper(int shipperId)
		{
			return SalesProvider.Instance().DeleteShipper(shipperId);
		}
		public static ShippersInfo GetShipper(int shipperId)
		{
			return SalesProvider.Instance().GetShipper(shipperId);
		}
		public static IList<ShippersInfo> GetShippers(bool includeDistributor)
		{
			return SalesProvider.Instance().GetShippers(includeDistributor);
		}
		public static void SetDefalutShipper(int shipperId)
		{
			SalesProvider.Instance().SetDefalutShipper(shipperId);
		}
		public static bool AddExpressTemplate(string expressName, string xmlFile)
		{
			return SalesProvider.Instance().AddExpressTemplate(expressName, xmlFile);
		}
		public static bool UpdateExpressTemplate(int expressId, string expressName)
		{
			return SalesProvider.Instance().UpdateExpressTemplate(expressId, expressName);
		}
		public static bool SetExpressIsUse(int expressId)
		{
			return SalesProvider.Instance().SetExpressIsUse(expressId);
		}
		public static bool DeleteExpressTemplate(int expressId)
		{
			return SalesProvider.Instance().DeleteExpressTemplate(expressId);
		}
		public static System.Data.DataTable GetExpressTemplates()
		{
			return SalesProvider.Instance().GetExpressTemplates();
		}
		public static System.Data.DataTable GetIsUserExpressTemplates()
		{
			return SalesProvider.Instance().GetIsUserExpressTemplates();
		}
		public static void SwapPaymentModeSequence(int modeId, int replaceModeId, int displaySequence, int replaceDisplaySequence)
		{
			SalesProvider.Instance().SwapPaymentModeSequence(modeId, replaceModeId, displaySequence, replaceDisplaySequence);
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
				result = SalesProvider.Instance().CreateUpdateDeletePaymentMode(paymentMode, DataProviderAction.Create);
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
				result = SalesProvider.Instance().CreateUpdateDeletePaymentMode(paymentMode, DataProviderAction.Update);
			}
			return result;
		}
		public static bool DeletePaymentMode(int modeId)
		{
			PaymentModeInfo paymentModeInfo = new PaymentModeInfo();
			paymentModeInfo.ModeId = modeId;
			return SalesProvider.Instance().CreateUpdateDeletePaymentMode(paymentModeInfo, DataProviderAction.Delete) == PaymentModeActionStatus.Success;
		}
		public static IList<PaymentModeInfo> GetPaymentModes()
		{
			return SalesProvider.Instance().GetPaymentModes();
		}
		public static PaymentModeInfo GetPaymentMode(int modeId)
		{
			return SalesProvider.Instance().GetPaymentMode(modeId);
		}
		public static PaymentModeInfo GetPaymentMode(string gateway)
		{
			return SalesProvider.Instance().GetPaymentMode(gateway);
		}
		public static IList<ShippingModeInfo> GetShippingModes()
		{
			return SalesProvider.Instance().GetShippingModes(string.Empty);
		}
		public static IList<ShippingModeInfo> GetShippingModes(string paymentGateway)
		{
			return SalesProvider.Instance().GetShippingModes(paymentGateway);
		}
		public static void SwapShippingModeSequence(int modeId, int replaceModeId, int displaySequence, int replaceDisplaySequence)
		{
			SalesProvider.Instance().SwapShippingModeSequence(modeId, replaceModeId, displaySequence, replaceDisplaySequence);
		}
		public static ShippingModeInfo GetShippingMode(int modeId, bool includeDetail)
		{
			return SalesProvider.Instance().GetShippingMode(modeId, includeDetail);
		}
		public static bool CreateShippingMode(ShippingModeInfo shippingMode)
		{
			return null != shippingMode && SalesProvider.Instance().CreateShippingMode(shippingMode);
		}
		public static bool DeleteShippingMode(int modeId)
		{
			return SalesProvider.Instance().DeleteShippingMode(modeId);
		}
		public static bool UpdateShippMode(ShippingModeInfo shippingMode)
		{
			bool result;
			if (shippingMode == null)
			{
				result = false;
			}
			else
			{
				Globals.EntityCoding(shippingMode, true);
				result = SalesProvider.Instance().UpdateShippingMode(shippingMode);
			}
			return result;
		}
		public static bool CreateShippingTemplate(ShippingModeInfo shippingMode)
		{
			return SalesProvider.Instance().CreateShippingTemplate(shippingMode);
		}
		public static bool UpdateShippingTemplate(ShippingModeInfo shippingMode)
		{
			return SalesProvider.Instance().UpdateShippingTemplate(shippingMode);
		}
		public static bool DeleteShippingTemplate(int templateId)
		{
			return SalesProvider.Instance().DeleteShippingTemplate(templateId);
		}
		public static DbQueryResult GetShippingTemplates(Pagination pagin)
		{
			return SalesProvider.Instance().GetShippingTemplates(pagin);
		}
		public static ShippingModeInfo GetShippingTemplate(int templateId, bool includeDetail)
		{
			return SalesProvider.Instance().GetShippingTemplate(templateId, includeDetail);
		}
		public static System.Data.DataTable GetShippingAllTemplates()
		{
			return SalesProvider.Instance().GetShippingAllTemplates();
		}
		public static IList<string> GetExpressCompanysByMode(int modeId)
		{
			return SalesProvider.Instance().GetExpressCompanysByMode(modeId);
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
				result = SalesProvider.Instance().GetProductSales(productSale, out totalProductSales);
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
				result = SalesProvider.Instance().GetProductSalesNoPage(productSale, out totalProductSales);
			}
			return result;
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
				result = SalesProvider.Instance().GetUserStatistics(page, out totalProductSaleVisits);
			}
			return result;
		}
		public static OrderStatisticsInfo GetUserOrders(UserOrderQuery userOrder)
		{
			return SalesProvider.Instance().GetUserOrders(userOrder);
		}
		public static OrderStatisticsInfo GetUserOrdersNoPage(UserOrderQuery userOrder)
		{
			return SalesProvider.Instance().GetUserOrdersNoPage(userOrder);
		}
		public static AdminStatisticsInfo GetStatistics()
		{
			return SalesProvider.Instance().GetStatistics();
		}
		public static IList<OrderPriceStatisticsForChartInfo> GetOrderPriceStatisticsOfSevenDays(int days)
		{
			return SalesProvider.Instance().GetOrderPriceStatisticsOfSevenDays(days);
		}
		public static System.Data.DataTable GetMemberStatistics(SaleStatisticsQuery query, out int totalProductSales)
		{
			return SalesProvider.Instance().GetMemberStatistics(query, out totalProductSales);
		}
		public static System.Data.DataTable GetMemberStatisticsNoPage(SaleStatisticsQuery query)
		{
			return SalesProvider.Instance().GetMemberStatisticsNoPage(query);
		}
		public static System.Data.DataTable GetProductVisitAndBuyStatistics(SaleStatisticsQuery query, out int totalProductSales)
		{
			return SalesProvider.Instance().GetProductVisitAndBuyStatistics(query, out totalProductSales);
		}
		public static System.Data.DataTable GetProductVisitAndBuyStatisticsNoPage(SaleStatisticsQuery query, out int totalProductSales)
		{
			return SalesProvider.Instance().GetProductVisitAndBuyStatisticsNoPage(query, out totalProductSales);
		}
		public static DbQueryResult GetSaleOrderLineItemsStatistics(SaleStatisticsQuery query)
		{
			return SalesProvider.Instance().GetSaleOrderLineItemsStatistics(query);
		}
		public static DbQueryResult GetSaleOrderLineItemsStatisticsNoPage(SaleStatisticsQuery query)
		{
			return SalesProvider.Instance().GetSaleOrderLineItemsStatisticsNoPage(query);
		}
		public static DbQueryResult GetSaleTargets()
		{
			return SalesProvider.Instance().GetSaleTargets();
		}
		public static System.Data.DataTable GetWeekSaleTota(SaleStatisticsType saleStatisticsType)
		{
			return SalesProvider.Instance().GetWeekSaleTota(saleStatisticsType);
		}
		public static System.Data.DataTable GetDaySaleTotal(int year, int month, SaleStatisticsType saleStatisticsType)
		{
			return SalesProvider.Instance().GetDaySaleTotal(year, month, saleStatisticsType);
		}
		public static decimal GetDaySaleTotal(int year, int month, int int_0, SaleStatisticsType saleStatisticsType)
		{
			return SalesProvider.Instance().GetDaySaleTotal(year, month, int_0, saleStatisticsType);
		}
		public static decimal GetMonthSaleTotal(int year, int month, SaleStatisticsType saleStatisticsType)
		{
			return SalesProvider.Instance().GetMonthSaleTotal(year, month, saleStatisticsType);
		}
		public static System.Data.DataTable GetMonthSaleTotal(int year, SaleStatisticsType saleStatisticsType)
		{
			return SalesProvider.Instance().GetMonthSaleTotal(year, saleStatisticsType);
		}
		public static decimal GetYearSaleTotal(int year, SaleStatisticsType saleStatisticsType)
		{
			return SalesProvider.Instance().GetYearSaleTotal(year, saleStatisticsType);
		}
		public static IList<UserStatisticsForDate> GetUserAdd(int? year, int? month, int? days)
		{
			return SalesProvider.Instance().GetUserAdd(year, month, days);
		}
		public static DbQueryResult GetPurchaseOrders(PurchaseOrderQuery query)
		{
			return SalesProvider.Instance().GetPurchaseOrders(query);
		}
		public static System.Data.DataTable GetSendGoodsPurchaseOrders(string purchaseOrderIds)
		{
			return SalesProvider.Instance().GetSendGoodsPurchaseOrders(purchaseOrderIds);
		}
		public static System.Data.DataTable GetRecentlyPurchaseOrders(out int number)
		{
			return SalesProvider.Instance().GetRecentlyPurchaseOrders(out number);
		}
		public static PurchaseOrderInfo GetPurchaseOrder(string purchaseOrderId)
		{
			return SalesProvider.Instance().GetPurchaseOrder(purchaseOrderId);
		}
		public static int DeletePurchaseOrders(string purchaseOrderIds)
		{
			ManagerHelper.CheckPrivilege(Privilege.DeletePurchaseorder);
			int num = SalesProvider.Instance().DeletePurchaseOrders(purchaseOrderIds);
			if (num > 0)
			{
				EventLogs.WriteOperationLog(Privilege.DeletePurchaseorder, string.Format(CultureInfo.InvariantCulture, "删除了编号为\"{0}\"的采购单", new object[]
				{
					purchaseOrderIds
				}));
			}
			return num;
		}
		public static bool ClosePurchaseOrder(PurchaseOrderInfo purchaseOrder)
		{
			ManagerHelper.CheckPrivilege(Privilege.EditPurchaseorder);
			bool result;
			if (result = SalesProvider.Instance().ClosePurchaseOrder(purchaseOrder))
			{
				EventLogs.WriteOperationLog(Privilege.EditPurchaseorder, string.Format(CultureInfo.InvariantCulture, "关闭了编号为\"{0}\"的采购单", new object[]
				{
					purchaseOrder.PurchaseOrderId
				}));
			}
			return result;
		}
		public static bool UpdatePurchaseOrderShippingMode(PurchaseOrderInfo purchaseOrder)
		{
			ManagerHelper.CheckPrivilege(Privilege.EditPurchaseorder);
			bool result;
			if (purchaseOrder.CheckAction(PurchaseOrderActions.MASTER__MODIFY_SHIPPING_MODE))
			{
				bool flag;
				if (flag = SalesProvider.Instance().UpdatePurchaseOrderShippingMode(purchaseOrder))
				{
					EventLogs.WriteOperationLog(Privilege.EditPurchaseorder, string.Format(CultureInfo.InvariantCulture, "修改了编号为\"{0}\"的采购单的配送方式", new object[]
					{
						purchaseOrder.PurchaseOrderId
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
		public static bool SavePurchaseOrderShippingAddress(PurchaseOrderInfo purchaseOrder)
		{
			ManagerHelper.CheckPrivilege(Privilege.EditPurchaseorder);
			bool result;
			if (purchaseOrder.CheckAction(PurchaseOrderActions.MASTER_MODIFY_DELIVER_ADDRESS))
			{
				bool flag;
				if (flag = SalesProvider.Instance().SavePurchaseOrderShippingAddress(purchaseOrder))
				{
					EventLogs.WriteOperationLog(Privilege.EditPurchaseorder, string.Format(CultureInfo.InvariantCulture, "修改了编号为\"{0}\"的采购单的收货地址", new object[]
					{
						purchaseOrder.PurchaseOrderId
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
		public static bool SavePurchaseOrderRemark(PurchaseOrderInfo purchaseOrder)
		{
			ManagerHelper.CheckPrivilege(Privilege.PurchaseorderMark);
			bool result;
			if (result = SalesProvider.Instance().SavePurchaseOrderRemark(purchaseOrder))
			{
				EventLogs.WriteOperationLog(Privilege.PurchaseorderMark, string.Format(CultureInfo.InvariantCulture, "对编号为\"{0}\"的采购单备注", new object[]
				{
					purchaseOrder.PurchaseOrderId
				}));
			}
			return result;
		}
		public static bool SendPurchaseOrderGoods(PurchaseOrderInfo purchaseOrder)
		{
			Globals.EntityCoding(purchaseOrder, true);
			ManagerHelper.CheckPrivilege(Privilege.PurchaseorderSendGood);
			bool result;
			if (purchaseOrder.CheckAction(PurchaseOrderActions.MASTER_SEND_GOODS))
			{
				bool flag;
				if (flag = SalesProvider.Instance().SendPurchaseOrderGoods(purchaseOrder))
				{
					if (purchaseOrder.Gateway == "hishop.plugins.payment.podrequest")
					{
						SalesProvider.Instance().UpdateProductStock(purchaseOrder.PurchaseOrderId);
						SalesProvider.Instance().UpdateDistributorAccount(purchaseOrder.GetPurchaseTotal(), purchaseOrder.DistributorId);
						Users.ClearUserCache(Users.GetUser(purchaseOrder.DistributorId));
					}
					EventLogs.WriteOperationLog(Privilege.PurchaseorderSendGood, string.Format(CultureInfo.InvariantCulture, "对编号为\"{0}\"的采购单发货", new object[]
					{
						purchaseOrder.PurchaseOrderId
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
		public static bool ConfirmPurchaseOrderFinish(PurchaseOrderInfo purchaseOrder)
		{
			ManagerHelper.CheckPrivilege(Privilege.EditPurchaseorder);
			bool result;
			if (purchaseOrder.CheckAction(PurchaseOrderActions.MASTER_FINISH_TRADE))
			{
				bool flag;
				if (flag = SalesProvider.Instance().ConfirmPurchaseOrderFinish(purchaseOrder))
				{
					EventLogs.WriteOperationLog(Privilege.EditPurchaseorder, string.Format(CultureInfo.InvariantCulture, "完成编号为\"{0}\"的采购单", new object[]
					{
						purchaseOrder.PurchaseOrderId
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
		public static bool UpdatePurchaseOrderAmount(PurchaseOrderInfo purchaseOrder)
		{
			ManagerHelper.CheckPrivilege(Privilege.EditPurchaseorder);
			bool result;
			if (purchaseOrder.CheckAction(PurchaseOrderActions.MASTER__MODIFY_AMOUNT))
			{
				bool flag;
				if (flag = SalesProvider.Instance().UpdatePurchaseOrderAmount(purchaseOrder))
				{
					EventLogs.WriteOperationLog(Privilege.EditPurchaseorder, string.Format(CultureInfo.InvariantCulture, "修改编号为\"{0}\"的采购单的金额", new object[]
					{
						purchaseOrder.PurchaseOrderId
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
		public static bool ConfirmPayPurchaseOrder(PurchaseOrderInfo purchaseOrder)
		{
			ManagerHelper.CheckPrivilege(Privilege.EditPurchaseorder);
			bool result;
			if (result = SalesProvider.Instance().ConfirmPayPurchaseOrder(purchaseOrder))
			{
				SalesProvider.Instance().UpdateProductStock(purchaseOrder.PurchaseOrderId);
				SalesProvider.Instance().UpdateDistributorAccount(purchaseOrder.GetPurchaseTotal(), purchaseOrder.DistributorId);
				Users.ClearUserCache(Users.GetUser(purchaseOrder.DistributorId));
				EventLogs.WriteOperationLog(Privilege.EditPurchaseorder, string.Format(CultureInfo.InvariantCulture, "对编号为\"{0}\"的采购单线下收款", new object[]
				{
					purchaseOrder.PurchaseOrderId
				}));
			}
			return result;
		}
		public static bool AddPurchaseOrderItem(PurchaseShoppingCartItemInfo item, string POrderId)
		{
			SalesProvider salesProvider = SalesProvider.Instance();
			int currentPOrderItemQuantity = salesProvider.GetCurrentPOrderItemQuantity(POrderId, item.SkuId);
			bool result;
			if (currentPOrderItemQuantity == 0)
			{
				result = salesProvider.AddPurchaseOrderItem(item, POrderId);
			}
			else
			{
				result = salesProvider.UpdatePurchaseOrderQuantity(POrderId, item.SkuId, item.Quantity + currentPOrderItemQuantity);
			}
			return result;
		}
		public static bool UpdatePurchaseOrderItemQuantity(string POrderId, string SkuId, int Quantity)
		{
			return SalesProvider.Instance().UpdatePurchaseOrderQuantity(POrderId, SkuId, Quantity);
		}
		public static bool UpdatePurchaseOrder(PurchaseOrderInfo purchaseOrder)
		{
			return SalesProvider.Instance().UpdatePurchaseOrder(purchaseOrder);
		}
		public static bool DeletePurchaseOrderItem(string POrderId, string SkuId)
		{
			return SalesProvider.Instance().DeletePurchaseOrderItem(POrderId, SkuId);
		}
		public static bool SetPurchaseOrderShippingMode(string orderIds, int realShippingModeId, string realModeName)
		{
			return SalesProvider.Instance().SetPurchaseOrderShippingMode(orderIds, realShippingModeId, realModeName);
		}
		public static bool SetPurchaseOrderExpressComputerpe(string orderIds, string expressCompanyName, string expressCompanyAbb)
		{
			return SalesProvider.Instance().SetPurchaseOrderExpressComputerpe(orderIds, expressCompanyName, expressCompanyAbb);
		}
		public static bool SetPurchaseOrderShipNumber(string purchaseOrderId, string startNumber)
		{
			return SalesProvider.Instance().SetPurchaseOrderShipNumber(purchaseOrderId, startNumber);
		}
		public static void SetPurchaseOrderShipNumber(string[] orderIds, string startNumber)
		{
			int num = 0;
			for (int i = 0; i < orderIds.Length; i++)
			{
				long num2 = long.Parse(startNumber) + (long)num;
				if (SalesProvider.Instance().SetPurchaseOrderShipNumber(orderIds[i], num2.ToString()))
				{
					num++;
				}
			}
		}
		public static void SetPurchaseOrderPrinted(string[] purchaseOrderIds, bool isPrinted)
		{
			int num = 0;
			for (int i = purchaseOrderIds.Length - 1; i >= 0; i--)
			{
				if (SalesProvider.Instance().SetPurchaseOrderPrinted(purchaseOrderIds[i], isPrinted))
				{
					num++;
				}
			}
		}
		public static System.Data.DataSet GetPurchaseOrdersAndLines(string purchaseOrderIds)
		{
			return SalesProvider.Instance().GetPurchaseOrdersAndLines(purchaseOrderIds);
		}
		public static decimal CalcFreight(int regionId, int totalWeight, ShippingModeInfo shippingModeInfo)
		{
			decimal result = 0m;
			int topRegionId = RegionHelper.GetTopRegionId(regionId);
			decimal d = totalWeight;
			decimal d2 = 1m;
			if (d > shippingModeInfo.Weight && shippingModeInfo.AddWeight.HasValue && shippingModeInfo.AddWeight.Value > 0m)
			{
				if ((d - shippingModeInfo.Weight) % shippingModeInfo.AddWeight == 0m)
				{
					d2 = (d - shippingModeInfo.Weight) / shippingModeInfo.AddWeight.Value;
				}
				else
				{
                    d2 = 1 + ((d - shippingModeInfo.Weight) / shippingModeInfo.AddWeight.Value);
				}
			}
			if (shippingModeInfo.ModeGroup == null || shippingModeInfo.ModeGroup.Count == 0)
			{
				if (d > shippingModeInfo.Weight && shippingModeInfo.AddPrice.HasValue)
				{
					result = d2 * shippingModeInfo.AddPrice.Value + shippingModeInfo.Price;
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
						if (d > shippingModeInfo.Weight)
						{
							result = d2 * current.AddPrice + current.Price;
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
					if (d > shippingModeInfo.Weight && shippingModeInfo.AddPrice.HasValue)
					{
						result = d2 * shippingModeInfo.AddPrice.Value + shippingModeInfo.Price;
					}
					else
					{
						result = shippingModeInfo.Price;
					}
				}
			}
			return result;
		}
		public static System.Data.DataSet GetAPIPurchaseOrders(PurchaseOrderQuery query, out int totalrecord)
		{
			return SalesProvider.Instance().GetAPIPurchaseOrders(query, out totalrecord);
		}
		public static bool SendAPIPurchaseOrderGoods(PurchaseOrderInfo purchaseOrder)
		{
			Globals.EntityCoding(purchaseOrder, true);
			bool result;
			if (purchaseOrder.CheckAction(PurchaseOrderActions.MASTER_SEND_GOODS))
			{
				bool flag;
				if (flag = SalesProvider.Instance().SendPurchaseOrderGoods(purchaseOrder))
				{
					EventLogs.WriteOperationLog(Privilege.PurchaseorderSendGood, string.Format(CultureInfo.InvariantCulture, "对编号为\"{0}\"的采购单发货", new object[]
					{
						purchaseOrder.PurchaseOrderId
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
		public static bool SaveAPIPurchaseOrderRemark(PurchaseOrderInfo purchaseOrder)
		{
			bool result;
			if (result = SalesProvider.Instance().SavePurchaseOrderRemark(purchaseOrder))
			{
				EventLogs.WriteOperationLog(Privilege.PurchaseorderMark, string.Format(CultureInfo.InvariantCulture, "对编号为\"{0}\"的采购单备注", new object[]
				{
					purchaseOrder.PurchaseOrderId
				}));
			}
			return result;
		}
		public static ShippingModeInfo GetShippingModeByCompany(string companyname)
		{
			return SalesProvider.Instance().GetShippingModeByCompany(companyname);
		}
		public static bool CheckPurchaseRefund(PurchaseOrderInfo purchaseOrder, string Operator, string adminRemark, int refundType, bool accept)
		{
			ManagerHelper.CheckPrivilege(Privilege.PurchaseOrderRefundApply);
			bool result;
			if (purchaseOrder.PurchaseStatus != OrderStatus.ApplyForRefund)
			{
				result = false;
			}
			else
			{
				bool flag;
				if (flag = SalesProvider.Instance().CheckPurchaseRefund(purchaseOrder.PurchaseOrderId, Operator, adminRemark, refundType, accept))
				{
					if (accept)
					{
						SalesProvider.Instance().UpdateRefundSubmitPurchaseOrderStock(purchaseOrder);
					}
					EventLogs.WriteOperationLog(Privilege.PurchaseorderRefund, string.Format(CultureInfo.InvariantCulture, "对编号为\"{0}\"的采购单退款", new object[]
					{
						purchaseOrder.PurchaseOrderId
					}));
				}
				result = flag;
			}
			return result;
		}
		public static void GetPurchaseRefundType(string purchaseOrderId, out int refundType, out string remark)
		{
			SalesProvider.Instance().GetPurchaseRefundType(purchaseOrderId, out refundType, out remark);
		}
		public static bool CheckPurchaseReturn(string purchaseOrderId, string Operator, decimal refundMoney, string adminRemark, int refundType, bool accept)
		{
			ManagerHelper.CheckPrivilege(Privilege.PurchaseOrderReturnsApply);
			PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(purchaseOrderId);
			bool result;
			if (purchaseOrder.PurchaseStatus != OrderStatus.ApplyForReturns)
			{
				result = false;
			}
			else
			{
				bool flag;
				if (flag = SalesProvider.Instance().CheckPurchaseReturn(purchaseOrderId, Operator, refundMoney, adminRemark, refundType, accept))
				{
					EventLogs.WriteOperationLog(Privilege.PurchaseorderRefund, string.Format(CultureInfo.InvariantCulture, "对编号为\"{0}\"的采购单退货", new object[]
					{
						purchaseOrderId
					}));
				}
				result = flag;
			}
			return result;
		}
		public static void GetPurchaseRefundTypeFromReturn(string purchaseOrderId, out int refundType, out string remark)
		{
			SalesProvider.Instance().GetPurchaseRefundTypeFromReturn(purchaseOrderId, out refundType, out remark);
		}
		public static bool CheckPurchaseReplace(string purchaseOrderId, string adminRemark, bool accept)
		{
			ManagerHelper.CheckPrivilege(Privilege.PurchaseOrderReplaceApply);
			PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(purchaseOrderId);
			return purchaseOrder.PurchaseStatus == OrderStatus.ApplyForReplacement && SalesProvider.Instance().CheckPurchaseReplace(purchaseOrderId, adminRemark, accept);
		}
		public static string GetPurchaseReplaceComments(string purchaseOrderId)
		{
			return SalesProvider.Instance().GetPurchaseReplaceComments(purchaseOrderId);
		}
		public static DbQueryResult GetPurchaseRefundApplys(RefundApplyQuery query)
		{
			return SalesProvider.Instance().GetPurchaseRefundApplys(query);
		}
		public static bool DelPurchaseRefundApply(string[] refundIds, out int count)
		{
			ManagerHelper.CheckPrivilege(Privilege.PurchaseOrderRefundApply);
			bool result = true;
			count = 0;
			for (int i = 0; i < refundIds.Length; i++)
			{
				string text = refundIds[i];
				if (!string.IsNullOrEmpty(text))
				{
					if (SalesProvider.Instance().DelPurchaseRefundApply(int.Parse(text)))
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
		public static DbQueryResult GetPurchaseReturnsApplys(ReturnsApplyQuery query)
		{
			return SalesProvider.Instance().GetPurchaseReturnsApplys(query);
		}
		public static bool DelPurchaseReturnsApply(string[] returnsIds, out int count)
		{
			ManagerHelper.CheckPrivilege(Privilege.PurchaseOrderReturnsApply);
			bool result = true;
			count = 0;
			for (int i = 0; i < returnsIds.Length; i++)
			{
				string text = returnsIds[i];
				if (!string.IsNullOrEmpty(text))
				{
					if (SalesProvider.Instance().DelPurchaseReturnsApply(int.Parse(text)))
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
		public static DbQueryResult GetPurchaseReplaceApplys(ReplaceApplyQuery query)
		{
			return SalesProvider.Instance().GetPurchaseReplaceApplys(query);
		}
		public static bool DelPurchaseReplaceApply(string[] replaceIds, out int count)
		{
			bool result = true;
			count = 0;
			for (int i = 0; i < replaceIds.Length; i++)
			{
				string text = replaceIds[i];
				if (!string.IsNullOrEmpty(text))
				{
					if (SalesProvider.Instance().DelPurchaseReplaceApply(int.Parse(text)))
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
		public static bool SavePurchaseDebitNote(PurchaseDebitNote note)
		{
			return SalesProvider.Instance().SavePurchaseDebitNote(note);
		}
		public static DbQueryResult GetAllPurchaseDebitNote(DebitNoteQuery query)
		{
			return SalesProvider.Instance().GetAllPurchaseDebitNote(query);
		}
		public static bool DelPurchaseDebitNote(string[] noteIds, out int count)
		{
			ManagerHelper.CheckPrivilege(Privilege.DeletePurchaseorder);
			bool flag = true;
			count = 0;
			for (int i = 0; i < noteIds.Length; i++)
			{
				string text = noteIds[i];
				if (!string.IsNullOrEmpty(text) && (flag &= SalesProvider.Instance().DelPurchaseDebitNote(text)))
				{
					count++;
				}
			}
			return flag;
		}
		public static bool SavePurchaseSendNote(SendNote note)
		{
			return SalesProvider.Instance().SavePurchaseSendNote(note);
		}
		public static DbQueryResult GetAllPurchaseSendNote(RefundApplyQuery query)
		{
			return SalesProvider.Instance().GetAllPurchaseSendNote(query);
		}
		public static bool DelPurchaseSendNote(string[] noteIds, out int count)
		{
			ManagerHelper.CheckPrivilege(Privilege.DeletePurchaseorder);
			bool flag = true;
			count = 0;
			for (int i = 0; i < noteIds.Length; i++)
			{
				string text = noteIds[i];
				if (!string.IsNullOrEmpty(text) && (flag &= SalesProvider.Instance().DelPurchaseSendNote(text)))
				{
					count++;
				}
			}
			return flag;
		}
	}
}
