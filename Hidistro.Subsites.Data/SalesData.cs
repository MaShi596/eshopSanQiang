using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Sales;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
namespace Hidistro.Subsites.Data
{
	public class SalesData : SubsiteSalesProvider
	{
		private Database database;
		public SalesData()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public override bool AddShipper(ShippersInfo shipper)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_Shippers (DistributorUserId,IsDefault, ShipperTag, ShipperName, RegionId, Address, CellPhone, TelPhone, Zipcode, Remark) VALUES (@DistributorUserId, 0, @ShipperTag, @ShipperName, @RegionId, @Address, @CellPhone, @TelPhone, @Zipcode, @Remark)");
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "ShipperTag", System.Data.DbType.String, shipper.ShipperTag);
			this.database.AddInParameter(sqlStringCommand, "ShipperName", System.Data.DbType.String, shipper.ShipperName);
			this.database.AddInParameter(sqlStringCommand, "RegionId", System.Data.DbType.Int32, shipper.RegionId);
			this.database.AddInParameter(sqlStringCommand, "Address", System.Data.DbType.String, shipper.Address);
			this.database.AddInParameter(sqlStringCommand, "CellPhone", System.Data.DbType.String, shipper.CellPhone);
			this.database.AddInParameter(sqlStringCommand, "TelPhone", System.Data.DbType.String, shipper.TelPhone);
			this.database.AddInParameter(sqlStringCommand, "Zipcode", System.Data.DbType.String, shipper.Zipcode);
			this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, shipper.Remark);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool UpdateShipper(ShippersInfo shipper)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Shippers SET ShipperTag = @ShipperTag, ShipperName = @ShipperName, RegionId = @RegionId, Address = @Address, CellPhone = @CellPhone, TelPhone = @TelPhone, Zipcode = @Zipcode, Remark =@Remark WHERE DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "ShipperTag", System.Data.DbType.String, shipper.ShipperTag);
			this.database.AddInParameter(sqlStringCommand, "ShipperName", System.Data.DbType.String, shipper.ShipperName);
			this.database.AddInParameter(sqlStringCommand, "RegionId", System.Data.DbType.Int32, shipper.RegionId);
			this.database.AddInParameter(sqlStringCommand, "Address", System.Data.DbType.String, shipper.Address);
			this.database.AddInParameter(sqlStringCommand, "CellPhone", System.Data.DbType.String, shipper.CellPhone);
			this.database.AddInParameter(sqlStringCommand, "TelPhone", System.Data.DbType.String, shipper.TelPhone);
			this.database.AddInParameter(sqlStringCommand, "Zipcode", System.Data.DbType.String, shipper.Zipcode);
			this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, shipper.Remark);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override ShippersInfo GetMyShipper()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Shippers WHERE DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			ShippersInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateShipper(dataReader);
				}
			}
			return result;
		}
		public override PaymentModeActionStatus CreateUpdateDeletePaymentMode(PaymentModeInfo paymentMode, DataProviderAction action)
		{
			PaymentModeActionStatus result;
			if (null == paymentMode)
			{
				result = PaymentModeActionStatus.UnknowError;
			}
			else
			{
				System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("sub_PaymentType_CreateUpdateDelete");
				this.database.AddInParameter(storedProcCommand, "Action", System.Data.DbType.Int32, (int)action);
				this.database.AddOutParameter(storedProcCommand, "Status", System.Data.DbType.Int32, 4);
				this.database.AddInParameter(storedProcCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
				if (action == DataProviderAction.Create)
				{
					this.database.AddOutParameter(storedProcCommand, "ModeId", System.Data.DbType.Int32, 4);
				}
				else
				{
					this.database.AddInParameter(storedProcCommand, "ModeId", System.Data.DbType.Int32, paymentMode.ModeId);
				}
				if (action != DataProviderAction.Delete)
				{
					this.database.AddInParameter(storedProcCommand, "Name", System.Data.DbType.String, paymentMode.Name);
					this.database.AddInParameter(storedProcCommand, "Description", System.Data.DbType.String, paymentMode.Description);
					this.database.AddInParameter(storedProcCommand, "Gateway", System.Data.DbType.String, paymentMode.Gateway);
					this.database.AddInParameter(storedProcCommand, "IsUseInpour", System.Data.DbType.Boolean, paymentMode.IsUseInpour);
					this.database.AddInParameter(storedProcCommand, "Charge", System.Data.DbType.Currency, paymentMode.Charge);
					this.database.AddInParameter(storedProcCommand, "IsPercent", System.Data.DbType.Boolean, paymentMode.IsPercent);
					this.database.AddInParameter(storedProcCommand, "Settings", System.Data.DbType.String, paymentMode.Settings);
				}
				this.database.ExecuteNonQuery(storedProcCommand);
				PaymentModeActionStatus paymentModeActionStatus = (PaymentModeActionStatus)((int)this.database.GetParameterValue(storedProcCommand, "Status"));
				result = paymentModeActionStatus;
			}
			return result;
		}
		public override void SwapPaymentModeSequence(int modeId, int replaceModeId, int displaySequence, int replaceDisplaySequence)
		{
			DataHelper.SwapSequence("distro_PaymentTypes", "ModeId", "DisplaySequence", modeId, replaceModeId, displaySequence, replaceDisplaySequence);
		}
		public override PaymentModeInfo GetPaymentMode(int modeId)
		{
			PaymentModeInfo result = new PaymentModeInfo();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_PaymentTypes WHERE ModeId = @ModeId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "ModeId", System.Data.DbType.Int32, modeId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulatePayment(dataReader);
				}
			}
			return result;
		}
		public override PaymentModeInfo GetPaymentMode(string gateway)
		{
			PaymentModeInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT top 1 * FROM distro_PaymentTypes WHERE Gateway = @Gateway AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "Gateway", System.Data.DbType.String, gateway);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulatePayment(dataReader);
				}
			}
			return result;
		}
		public override IList<PaymentModeInfo> GetPaymentModes()
		{
			IList<PaymentModeInfo> list = new List<PaymentModeInfo>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_PaymentTypes WHERE DistributorUserId=@DistributorUserId Order by DisplaySequence desc");
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulatePayment(dataReader));
				}
			}
			return list;
		}
		public override DbQueryResult GetOrders(OrderQuery query)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("sub_Orders_Get");
			this.database.AddInParameter(storedProcCommand, "PageIndex", System.Data.DbType.Int32, query.PageIndex);
			this.database.AddInParameter(storedProcCommand, "PageSize", System.Data.DbType.Int32, query.PageSize);
			this.database.AddInParameter(storedProcCommand, "IsCount", System.Data.DbType.Boolean, query.IsCount);
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", System.Data.DbType.String, SalesData.BuildOrdersQuery(query));
			this.database.AddOutParameter(storedProcCommand, "TotalOrders", System.Data.DbType.Int32, 4);
			DbQueryResult dbQueryResult = new DbQueryResult();
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
			{
				dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			dbQueryResult.TotalRecords = (int)this.database.GetParameterValue(storedProcCommand, "TotalOrders");
			return dbQueryResult;
		}
		public override DbQueryResult GetSendGoodsOrders(OrderQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("DistributorUserId ='{0}' AND OrderStatus = 2", HiContext.Current.User.UserId);
			if (query.OrderId != string.Empty && query.OrderId != null)
			{
				stringBuilder.AppendFormat(" AND OrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId));
			}
			else
			{
				if (query.PaymentType.HasValue)
				{
					stringBuilder.AppendFormat(" AND PaymentTypeId = '{0}'", query.PaymentType.Value);
				}
				if (query.GroupBuyId.HasValue)
				{
					stringBuilder.AppendFormat(" AND GroupBuyId = {0}", query.GroupBuyId.Value);
				}
				if (!string.IsNullOrEmpty(query.ProductName))
				{
					stringBuilder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM distro_OrderItems WHERE DistributorUserId ={0} AND ItemDescription LIKE '%{1}%')", HiContext.Current.User.UserId, DataHelper.CleanSearchString(query.ProductName));
				}
				if (!string.IsNullOrEmpty(query.ShipTo))
				{
					stringBuilder.AppendFormat(" AND ShipTo LIKE '%{0}%'", DataHelper.CleanSearchString(query.ShipTo));
				}
				if (!string.IsNullOrEmpty(query.UserName))
				{
					stringBuilder.AppendFormat(" AND Username = '{0}' ", DataHelper.CleanSearchString(query.UserName));
				}
				if (query.StartDate.HasValue)
				{
					stringBuilder.AppendFormat(" AND OrderDate >= '{0}'", query.StartDate.Value);
				}
				if (query.EndDate.HasValue)
				{
					stringBuilder.AppendFormat(" AND OrderDate <= '{0}'", query.EndDate.Value);
				}
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "distro_Orders do", "OrderId", stringBuilder.ToString(), "OrderId, OrderDate,RefundStatus, ShipTo, OrderTotal, OrderStatus,ShippingRegion,Address,ISNULL(RealShippingModeId,ShippingModeId) ShippingModeId,(SELECT ShipOrderNumber FROM Hishop_PurchaseOrders WHERE OrderId=do.OrderId) ShipOrderNumber");
		}
		public override System.Data.DataTable GetRecentlyOrders(out int number)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TOP 12 OrderId, OrderDate, UserId, Username, Wangwang, RealName, ShipTo, OrderTotal, PaymentType,ISNULL(GroupBuyId,0) as GroupBuyId, ISNULL(GroupBuyStatus,0) as GroupBuyStatus,ManagerMark, OrderStatus, RefundStatus,ManagerRemark, (SELECT COUNT(*) FROM Hishop_PurchaseOrders WHERE OrderId=OrderId) AS PurchaseOrders FROM distro_Orders WHERE  DistributorUserId=@DistributorUserId ORDER BY OrderDate DESC");
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			System.Data.DataTable result = new System.Data.DataTable();
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			sqlStringCommand = this.database.GetSqlStringCommand("SELECT count(*) FROM distro_Orders WHERE  OrderStatus=2 AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			number = Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
			return result;
		}
		public override int DeleteOrders(string orderIds)
		{
			string text = string.Format("DELETE FROM distro_Orders WHERE OrderId IN({0}) AND DistributorUserId={1}", orderIds, HiContext.Current.User.UserId);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override OrderInfo GetOrderInfo(string orderId)
		{
			OrderInfo orderInfo = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_Orders Where OrderId = @OrderId AND DistributorUserId=@DistributorUserId; SELECT * FROM distro_OrderGifts Where OrderId = @OrderId; SELECT * FROM distro_OrderItems Where OrderId = @OrderId ");
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					orderInfo = DataMapper.PopulateOrder(dataReader);
				}
				dataReader.NextResult();
				while (dataReader.Read())
				{
					OrderGiftInfo item = DataMapper.PopulateOrderGift(dataReader);
					orderInfo.Gifts.Add(item);
				}
				dataReader.NextResult();
				while (dataReader.Read())
				{
					orderInfo.LineItems.Add((string)dataReader["SkuId"], DataMapper.PopulateLineItem(dataReader));
				}
			}
			return orderInfo;
		}
		public override int ConfirmPay(OrderInfo order, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Orders SET PayDate = @PayDate, OrderStatus = @OrderStatus,OrderPoint=@OrderPoint WHERE OrderId = @OrderId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "PayDate", System.Data.DbType.DateTime, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "OrderPoint", System.Data.DbType.Int32, order.Points);
			this.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, 2);
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, order.OrderId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			int result;
			if (dbTran != null)
			{
				result = this.database.ExecuteNonQuery(sqlStringCommand, dbTran);
			}
			else
			{
				result = this.database.ExecuteNonQuery(sqlStringCommand);
			}
			return result;
		}
		public override void UpdatePayOrderStock(string orderId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_SKUs Set Stock = CASE WHEN (Stock - (SELECT SUM(oi.ShipmentQuantity) FROM distro_OrderItems oi Where oi.SkuId =Hishop_SKUs.SkuId AND OrderId =@OrderId))<=0 Then 0 ELSE Stock - (SELECT SUM(oi.ShipmentQuantity) FROM distro_OrderItems oi  Where oi.SkuId =Hishop_SKUs.SkuId AND OrderId =@OrderId) END WHERE Hishop_SKUs.SkuId  IN (Select SkuId FROM distro_OrderItems Where OrderId =@OrderId)");
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override void UpdateRefundOrderStock(string orderId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_SKUs Set  Stock = Stock + (SELECT oi.ShipmentQuantity FROM distro_OrderItems oi  Where oi.SkuId =Hishop_SKUs.SkuId AND OrderId =@OrderId) WHERE Hishop_SKUs.SkuId  IN (Select SkuId FROM distro_OrderItems Where OrderId =@OrderId)");
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override bool CloseTransaction(OrderInfo order)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Orders SET OrderStatus=@OrderStatus,CloseReason=@CloseReason WHERE OrderId = @OrderId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, order.OrderId);
			this.database.AddInParameter(sqlStringCommand, "CloseReason", System.Data.DbType.String, order.CloseReason);
			this.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, 4);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override bool ConfirmOrderFinish(OrderInfo order)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Orders SET OrderStatus=@OrderStatus,FinishDate=@FinishDate WHERE OrderId = @OrderId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, order.OrderId);
			this.database.AddInParameter(sqlStringCommand, "FinishDate", System.Data.DbType.DateTime, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, 5);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool SaveRemark(OrderInfo order)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Orders SET ManagerMark=@ManagerMark,ManagerRemark=@ManagerRemark WHERE OrderId=@OrderId AND DistributorUserId=@DistributorUserId");
			if (order.ManagerMark.HasValue)
			{
				this.database.AddInParameter(sqlStringCommand, "ManagerMark", System.Data.DbType.Int32, (int)order.ManagerMark.Value);
			}
			else
			{
				this.database.AddInParameter(sqlStringCommand, "ManagerMark", System.Data.DbType.Int32, DBNull.Value);
			}
			this.database.AddInParameter(sqlStringCommand, "ManagerRemark", System.Data.DbType.String, order.ManagerRemark);
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, order.OrderId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override void UpdateUserStatistics(int userId, decimal refundAmount, bool isAllRefund)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Members SET Expenditure = ISNULL(Expenditure,0) - @refundAmount, OrderNumber = ISNULL(OrderNumber,0) - @refundNum WHERE UserId = @UserId AND ParentUserId=@ParentUserId");
			this.database.AddInParameter(sqlStringCommand, "refundAmount", System.Data.DbType.Decimal, refundAmount);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			this.database.AddInParameter(sqlStringCommand, "ParentUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			if (isAllRefund)
			{
				this.database.AddInParameter(sqlStringCommand, "refundNum", System.Data.DbType.Int32, 1);
			}
			else
			{
				this.database.AddInParameter(sqlStringCommand, "refundNum", System.Data.DbType.Int32, 0);
			}
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override int SendGoods(OrderInfo order)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Orders SET ShipOrderNumber = @ShipOrderNumber, RealShippingModeId = @RealShippingModeId, RealModeName = @RealModeName, OrderStatus = @OrderStatus,ShippingDate=@ShippingDate, ExpressCompanyName = @ExpressCompanyName, ExpressCompanyAbb = @ExpressCompanyAbb WHERE OrderId = @OrderId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", System.Data.DbType.String, order.ShipOrderNumber);
			this.database.AddInParameter(sqlStringCommand, "RealShippingModeId", System.Data.DbType.Int32, order.RealShippingModeId);
			this.database.AddInParameter(sqlStringCommand, "RealModeName", System.Data.DbType.String, order.RealModeName);
			this.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, 3);
			this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", System.Data.DbType.String, order.ExpressCompanyName);
			this.database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", System.Data.DbType.String, order.ExpressCompanyAbb);
			this.database.AddInParameter(sqlStringCommand, "ShippingDate", System.Data.DbType.DateTime, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, order.OrderId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override bool UpdateOrderAmount(OrderInfo order, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Orders SET OrderTotal = @OrderTotal, OrderProfit=@OrderProfit, AdjustedFreight = @AdjustedFreight, PayCharge = @PayCharge, AdjustedDiscount=@AdjustedDiscount, OrderPoint=@OrderPoint,OrderCostPrice=@OrderCostPrice, Amount=@Amount WHERE OrderId = @OrderId AND DistributorUserId=@DistributorUserId");
			SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
			this.database.AddInParameter(sqlStringCommand, "OrderTotal", System.Data.DbType.Currency, order.GetTotal());
			this.database.AddInParameter(sqlStringCommand, "AdjustedFreight", System.Data.DbType.Currency, order.AdjustedFreight);
			this.database.AddInParameter(sqlStringCommand, "PayCharge", System.Data.DbType.Currency, order.PayCharge);
			this.database.AddInParameter(sqlStringCommand, "AdjustedDiscount", System.Data.DbType.Currency, order.AdjustedDiscount);
			this.database.AddInParameter(sqlStringCommand, "OrderPoint", System.Data.DbType.Int32, Convert.ToInt32((order.GetTotal() - order.AdjustedFreight - order.PayCharge - order.Tax) / masterSettings.PointsRate));
			this.database.AddInParameter(sqlStringCommand, "OrderProfit", System.Data.DbType.Currency, order.GetProfit());
			this.database.AddInParameter(sqlStringCommand, "OrderCostPrice", System.Data.DbType.Currency, order.GetCostPrice());
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, order.OrderId);
			this.database.AddInParameter(sqlStringCommand, "Amount", System.Data.DbType.Currency, order.GetAmount());
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			bool result;
			if (dbTran != null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
			}
			return result;
		}
		public override bool DeleteOrderGift(string orderId, int giftId, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_OrderGifts WHERE OrderId=@OrderId AND GiftId=@GiftId ");
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			this.database.AddInParameter(sqlStringCommand, "GiftId", System.Data.DbType.Int32, giftId);
			bool result;
			if (dbTran != null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) >= 1);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
			}
			return result;
		}
		public override bool DeleteOrderProduct(string string_0, string orderId, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_OrderItems WHERE OrderId=@OrderId AND SkuId=@SkuId AND DistributorUserId=@DistributorUserId ");
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, string_0);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			bool result;
			if (dbTran != null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
			}
			return result;
		}
		public override bool SaveShippingAddress(OrderInfo order)
		{
			bool result;
			if (order == null)
			{
				result = false;
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Orders SET RegionId = @RegionId, ShippingRegion = @ShippingRegion, Address = @Address, ZipCode = @ZipCode,ShipTo = @ShipTo, TelPhone = @TelPhone, CellPhone = @CellPhone WHERE OrderId = @OrderId AND DistributorUserId=@DistributorUserId");
				this.database.AddInParameter(sqlStringCommand, "RegionId", System.Data.DbType.String, order.RegionId);
				this.database.AddInParameter(sqlStringCommand, "ShippingRegion", System.Data.DbType.String, order.ShippingRegion);
				this.database.AddInParameter(sqlStringCommand, "Address", System.Data.DbType.String, order.Address);
				this.database.AddInParameter(sqlStringCommand, "ZipCode", System.Data.DbType.String, order.ZipCode);
				this.database.AddInParameter(sqlStringCommand, "ShipTo", System.Data.DbType.String, order.ShipTo);
				this.database.AddInParameter(sqlStringCommand, "TelPhone", System.Data.DbType.String, order.TelPhone);
				this.database.AddInParameter(sqlStringCommand, "CellPhone", System.Data.DbType.String, order.CellPhone);
				this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
				this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, order.OrderId);
				result = (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
			}
			return result;
		}
		public override bool UpdateOrderShippingMode(OrderInfo order)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Orders SET ShippingModeId=@ShippingModeId ,ModeName=@ModeName,ExpressCompanyName=@ExpressCompanyName,ExpressCompanyAbb=@ExpressCompanyAbb WHERE OrderId = @OrderId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "ShippingModeId", System.Data.DbType.Int32, order.ShippingModeId);
			this.database.AddInParameter(sqlStringCommand, "ModeName", System.Data.DbType.String, order.ModeName);
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, order.OrderId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", System.Data.DbType.String, order.ExpressCompanyName);
			this.database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", System.Data.DbType.String, order.ExpressCompanyAbb);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override bool UpdateOrderPaymentType(OrderInfo order)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Orders SET PaymentTypeId=@PaymentTypeId ,PaymentType=@PaymentType WHERE OrderId = @OrderId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "PaymentTypeId", System.Data.DbType.Int32, order.PaymentTypeId);
			this.database.AddInParameter(sqlStringCommand, "PaymentType", System.Data.DbType.String, order.PaymentType);
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, order.OrderId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override int GetSkuStock(string skuId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Stock FROM Hishop_SKUs WHERE SkuId=@SkuId;");
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, skuId);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int result;
			if (obj != null && obj != DBNull.Value)
			{
				result = (int)obj;
			}
			else
			{
				result = 0;
			}
			return result;
		}
		public override bool GetAlertStock(string skuId)
		{
			bool result = false;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_SKUs WHERE SKuId=@SkuId AND Stock<=AlertStock");
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, skuId);
			if ((int)this.database.ExecuteScalar(sqlStringCommand) >= 1)
			{
				result = true;
			}
			return result;
		}
		public override bool UpdateLineItem(string orderId, LineItemInfo lineItem, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_OrderItems SET ShipmentQuantity=@ShipmentQuantity, ItemAdjustedPrice=@ItemAdjustedPrice,Weight=@Weight,Quantity=@Quantity, PromotionId = NULL, PromotionName = NULL WHERE OrderId=@OrderId AND SkuId=@SkuId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			this.database.AddInParameter(sqlStringCommand, "Quantity", System.Data.DbType.Int32, lineItem.Quantity);
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, lineItem.SkuId);
			this.database.AddInParameter(sqlStringCommand, "ShipmentQuantity", System.Data.DbType.Int32, lineItem.ShipmentQuantity);
			this.database.AddInParameter(sqlStringCommand, "ItemAdjustedPrice", System.Data.DbType.Currency, lineItem.ItemAdjustedPrice);
			this.database.AddInParameter(sqlStringCommand, "Weight", System.Data.DbType.Int32, lineItem.ItemWeight);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			bool result;
			if (dbTran != null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
			}
			return result;
		}
		public override void GetLineItemPromotions(int productId, int quantity, out int purchaseGiftId, out string purchaseGiftName, out int giveQuantity, out int wholesaleDiscountId, out string wholesaleDiscountName, out decimal? discountRate, int gradeId)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_LineItem_GetPromotionsInfo");
			this.database.AddInParameter(storedProcCommand, "Quantity", System.Data.DbType.Int32, quantity);
			this.database.AddInParameter(storedProcCommand, "ProductId", System.Data.DbType.Int32, productId);
			this.database.AddInParameter(storedProcCommand, "GradeId", System.Data.DbType.Int32, gradeId);
			this.database.AddInParameter(storedProcCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
			{
				purchaseGiftId = 0;
				giveQuantity = 0;
				wholesaleDiscountId = 0;
				purchaseGiftName = null;
				wholesaleDiscountName = null;
				discountRate = null;
				if (dataReader.Read())
				{
					if (DBNull.Value != dataReader["ActivityId"])
					{
						purchaseGiftId = (int)dataReader["ActivityId"];
					}
					if (DBNull.Value != dataReader["Name"])
					{
						purchaseGiftName = dataReader["Name"].ToString();
					}
					if (DBNull.Value != dataReader["BuyQuantity"] && DBNull.Value != dataReader["GiveQuantity"])
					{
						giveQuantity = quantity / (int)dataReader["BuyQuantity"] * (int)dataReader["GiveQuantity"];
					}
				}
				if (dataReader.NextResult() && dataReader.Read())
				{
					if (DBNull.Value != dataReader["ActivityId"])
					{
						wholesaleDiscountId = (int)dataReader["ActivityId"];
					}
					if (DBNull.Value != dataReader["Name"])
					{
						wholesaleDiscountName = dataReader["Name"].ToString();
					}
					if (DBNull.Value != dataReader["DiscountValue"])
					{
						discountRate = new decimal?(Convert.ToDecimal(dataReader["DiscountValue"]));
					}
				}
			}
		}
		public override LineItemInfo GetLineItemInfo(string string_0, string orderId)
		{
			LineItemInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_OrderItems WHERE SkuId=@SkuId AND OrderId=@OrderId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, string_0);
			this.database.AddInParameter(sqlStringCommand, "orderId", System.Data.DbType.String, orderId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateLineItem(dataReader);
				}
			}
			return result;
		}
		public override DbQueryResult GetOrderGifts(OrderGiftQuery query)
		{
			DbQueryResult dbQueryResult = new DbQueryResult();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT top {0} * from distro_OrderGifts where OrderId=@OrderId", query.PageSize);
			if (query.PageIndex == 1)
			{
				stringBuilder.Append(" ORDER BY GiftId ASC");
			}
			else
			{
				stringBuilder.AppendFormat(" and GiftId > (SELECT max(GiftId) from (SELECT top {0} GiftId from distro_OrderGifts where 0=0 and OrderId=@OrderId ORDER BY GiftId ASC ) as tbltemp) ORDER BY GiftId ASC", (query.PageIndex - 1) * query.PageSize);
			}
			if (query.IsCount)
			{
				stringBuilder.AppendFormat(";SELECT count(GiftId) as Total from distro_OrderGifts where OrderId=@OrderId", new object[0]);
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, query.OrderId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (query.IsCount && dataReader.NextResult())
				{
					dataReader.Read();
					dbQueryResult.TotalRecords = dataReader.GetInt32(0);
				}
			}
			return dbQueryResult;
		}
		public override bool ClearOrderGifts(string orderId, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_OrderGifts WHERE OrderId =@OrderId");
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			bool result;
			if (dbTran != null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) >= 1);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
			}
			return result;
		}
		public override bool AddOrderGift(string orderId, GiftInfo gift, int quantity, int promotype, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * from distro_OrderGifts where OrderId=@OrderId AND GiftId=@GiftId");
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			this.database.AddInParameter(sqlStringCommand, "GiftId", System.Data.DbType.Int32, gift.GiftId);
			bool result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (!dataReader.Read())
				{
					System.Data.Common.DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand("INSERT INTO distro_OrderGifts(OrderId,GiftId,DistributorUserId,GiftName,CostPrice,ThumbnailsUrl,Quantity,PromoType) VALUES(@OrderId,@GiftId,@DistributorUserId,@GiftName,@CostPrice,@ThumbnailsUrl,@Quantity,@PromoType)");
					this.database.AddInParameter(sqlStringCommand2, "OrderId", System.Data.DbType.String, orderId);
					this.database.AddInParameter(sqlStringCommand2, "GiftId", System.Data.DbType.Int32, gift.GiftId);
					this.database.AddInParameter(sqlStringCommand2, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
					this.database.AddInParameter(sqlStringCommand2, "GiftName", System.Data.DbType.String, gift.Name);
					this.database.AddInParameter(sqlStringCommand2, "CostPrice", System.Data.DbType.Currency, gift.PurchasePrice);
					this.database.AddInParameter(sqlStringCommand2, "ThumbnailsUrl", System.Data.DbType.String, gift.ThumbnailUrl40);
					this.database.AddInParameter(sqlStringCommand2, "Quantity", System.Data.DbType.Int32, quantity);
					this.database.AddInParameter(sqlStringCommand2, "PromoType", System.Data.DbType.Int32, promotype);
					if (dbTran != null)
					{
						result = (this.database.ExecuteNonQuery(sqlStringCommand2, dbTran) == 1);
					}
					else
					{
						result = (this.database.ExecuteNonQuery(sqlStringCommand2) == 1);
					}
				}
				else
				{
					System.Data.Common.DbCommand sqlStringCommand3 = this.database.GetSqlStringCommand("update distro_OrderGifts set Quantity=@Quantity where OrderId=@OrderId AND GiftId=@GiftId");
					this.database.AddInParameter(sqlStringCommand3, "OrderId", System.Data.DbType.String, orderId);
					this.database.AddInParameter(sqlStringCommand3, "GiftId", System.Data.DbType.Int32, gift.GiftId);
					this.database.AddInParameter(sqlStringCommand3, "Quantity", System.Data.DbType.Int32, (int)dataReader["Quantity"] + quantity);
					if (dbTran != null)
					{
						result = (this.database.ExecuteNonQuery(sqlStringCommand3, dbTran) == 1);
					}
					else
					{
						result = (this.database.ExecuteNonQuery(sqlStringCommand3) == 1);
					}
				}
			}
			return result;
		}
		public override PurchaseOrderInfo ConvertOrderToPurchaseOrder(OrderInfo order)
		{
			PurchaseOrderInfo result;
			if (order == null)
			{
				result = null;
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				string text = "";
				foreach (LineItemInfo current in order.LineItems.Values)
				{
					stringBuilder.AppendFormat("'" + current.SkuId + "',", new object[0]);
				}
				if (stringBuilder.Length > 0)
				{
					stringBuilder = stringBuilder.Remove(stringBuilder.Length - 1, 1);
					text = string.Format("SELECT S.SkuId, S.CostPrice, p.ProductName FROM Hishop_Products P LEFT OUTER JOIN Hishop_SKUs S ON P.ProductId = S.ProductId WHERE S.SkuId IN({0});", stringBuilder);
				}
				if (order.Gifts.Count > 0)
				{
					StringBuilder stringBuilder2 = new StringBuilder();
					foreach (OrderGiftInfo current2 in order.Gifts)
					{
						stringBuilder2.AppendFormat(current2.GiftId.ToString() + ",", new object[0]);
					}
					stringBuilder2.Remove(stringBuilder2.Length - 1, 1);
					text += string.Format(" SELECT GiftId, CostPrice FROM Hishop_Gifts WHERE GiftId IN({0});", stringBuilder2.ToString());
				}
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
				Dictionary<string, PurchaseOrderItemInfo> dictionary = new Dictionary<string, PurchaseOrderItemInfo>();
				Dictionary<int, decimal> dictionary2 = new Dictionary<int, decimal>();
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					if (order.LineItems.Values.Count > 0)
					{
						while (dataReader.Read())
						{
							PurchaseOrderItemInfo purchaseOrderItemInfo = new PurchaseOrderItemInfo();
							if (dataReader["CostPrice"] != DBNull.Value)
							{
								purchaseOrderItemInfo.ItemCostPrice = (decimal)dataReader["CostPrice"];
							}
							purchaseOrderItemInfo.ItemHomeSiteDescription = (string)dataReader["ProductName"];
							dictionary.Add((string)dataReader["SkuId"], purchaseOrderItemInfo);
						}
					}
					if (order.Gifts.Count > 0)
					{
						if (order.LineItems.Count > 0)
						{
							dataReader.NextResult();
						}
						while (dataReader.Read())
						{
							dictionary2.Add((int)dataReader["GiftId"], (DBNull.Value == dataReader["CostPrice"]) ? 0m : Convert.ToDecimal(dataReader["CostPrice"]));
						}
					}
				}
				Distributor distributor = HiContext.Current.User as Distributor;
				PurchaseOrderInfo purchaseOrderInfo = new PurchaseOrderInfo();
				purchaseOrderInfo.PurchaseOrderId = "PO" + order.OrderId;
				purchaseOrderInfo.OrderId = order.OrderId;
				purchaseOrderInfo.Remark = order.Remark;
				purchaseOrderInfo.PurchaseStatus = OrderStatus.WaitBuyerPay;
				purchaseOrderInfo.DistributorId = distributor.UserId;
				purchaseOrderInfo.Distributorname = distributor.Username;
				purchaseOrderInfo.DistributorEmail = distributor.Email;
				purchaseOrderInfo.DistributorRealName = distributor.RealName;
				purchaseOrderInfo.DistributorQQ = distributor.QQ;
				purchaseOrderInfo.DistributorWangwang = distributor.Wangwang;
				purchaseOrderInfo.DistributorMSN = distributor.MSN;
				purchaseOrderInfo.ShippingRegion = order.ShippingRegion;
				purchaseOrderInfo.Address = order.Address;
				purchaseOrderInfo.ZipCode = order.ZipCode;
				purchaseOrderInfo.ShipTo = order.ShipTo;
				purchaseOrderInfo.TelPhone = order.TelPhone;
				purchaseOrderInfo.CellPhone = order.CellPhone;
				purchaseOrderInfo.ShipToDate = order.ShipToDate;
				purchaseOrderInfo.ShippingModeId = order.ShippingModeId;
				purchaseOrderInfo.ModeName = order.ModeName;
				purchaseOrderInfo.RegionId = order.RegionId;
				purchaseOrderInfo.Freight = order.Freight;
				purchaseOrderInfo.AdjustedFreight = order.Freight;
				purchaseOrderInfo.ShipOrderNumber = order.ShipOrderNumber;
				purchaseOrderInfo.Weight = order.Weight;
				purchaseOrderInfo.RefundStatus = RefundStatus.None;
				purchaseOrderInfo.OrderTotal = order.GetTotal();
				purchaseOrderInfo.ExpressCompanyAbb = order.ExpressCompanyAbb;
				purchaseOrderInfo.ExpressCompanyName = order.ExpressCompanyName;
				purchaseOrderInfo.Tax = order.Tax;
				purchaseOrderInfo.InvoiceTitle = order.InvoiceTitle;
				foreach (LineItemInfo current3 in order.LineItems.Values)
				{
					PurchaseOrderItemInfo purchaseOrderItemInfo2 = new PurchaseOrderItemInfo();
					purchaseOrderItemInfo2.PurchaseOrderId = purchaseOrderInfo.PurchaseOrderId;
					purchaseOrderItemInfo2.SkuId = current3.SkuId;
					purchaseOrderItemInfo2.ProductId = current3.ProductId;
					purchaseOrderItemInfo2.SKU = current3.SKU;
					purchaseOrderItemInfo2.Quantity = current3.ShipmentQuantity;
					foreach (KeyValuePair<string, PurchaseOrderItemInfo> current4 in dictionary)
					{
						if (current4.Key == current3.SkuId)
						{
							purchaseOrderItemInfo2.ItemCostPrice = current4.Value.ItemCostPrice;
							purchaseOrderItemInfo2.ItemHomeSiteDescription = current4.Value.ItemHomeSiteDescription;
						}
					}
					purchaseOrderItemInfo2.ItemPurchasePrice = current3.ItemCostPrice;
					purchaseOrderItemInfo2.ItemListPrice = current3.ItemListPrice;
					purchaseOrderItemInfo2.ItemDescription = current3.ItemDescription;
					purchaseOrderItemInfo2.SKUContent = current3.SKUContent;
					purchaseOrderItemInfo2.ThumbnailsUrl = current3.ThumbnailsUrl;
					purchaseOrderItemInfo2.ItemWeight = current3.ItemWeight;
					if (string.IsNullOrEmpty(purchaseOrderItemInfo2.ItemHomeSiteDescription))
					{
						purchaseOrderItemInfo2.ItemHomeSiteDescription = purchaseOrderItemInfo2.ItemDescription;
					}
					purchaseOrderInfo.PurchaseOrderItems.Add(purchaseOrderItemInfo2);
				}
				foreach (OrderGiftInfo current5 in order.Gifts)
				{
					PurchaseOrderGiftInfo purchaseOrderGiftInfo = new PurchaseOrderGiftInfo();
					purchaseOrderGiftInfo.PurchaseOrderId = purchaseOrderInfo.PurchaseOrderId;
					foreach (KeyValuePair<int, decimal> current6 in dictionary2)
					{
						if (current6.Key == current5.GiftId)
						{
							purchaseOrderGiftInfo.CostPrice = current6.Value;
						}
					}
					purchaseOrderGiftInfo.PurchasePrice = current5.CostPrice;
					purchaseOrderGiftInfo.GiftId = current5.GiftId;
					purchaseOrderGiftInfo.GiftName = current5.GiftName;
					purchaseOrderGiftInfo.Quantity = current5.Quantity;
					purchaseOrderGiftInfo.ThumbnailsUrl = current5.ThumbnailsUrl;
					purchaseOrderInfo.PurchaseOrderGifts.Add(purchaseOrderGiftInfo);
				}
				result = purchaseOrderInfo;
			}
			return result;
		}
		public override PurchaseOrderTaobaoInfo GetPurchaseOrderTaobaoInfo(string tbOrderId)
		{
			PurchaseOrderTaobaoInfo purchaseOrderTaobaoInfo = new PurchaseOrderTaobaoInfo();
			if (tbOrderId.Trim() != "")
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PurchaseOrders Where TaobaoOrderId like @TaobaoOrderId");
				this.database.AddInParameter(sqlStringCommand, "TaobaoOrderId", System.Data.DbType.String, "%" + tbOrderId + "%");
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					if (dataReader.Read())
					{
						TimeSpan timeSpan = DateTime.Now - ((DateTime)dataReader["PurchaseDate"]).AddDays(7.0);
						if (timeSpan.Days > 0)
						{
							purchaseOrderTaobaoInfo.expire_time = timeSpan.Days.ToString();
						}
						purchaseOrderTaobaoInfo.order_id = tbOrderId;
						purchaseOrderTaobaoInfo.created = "true";
						purchaseOrderTaobaoInfo.status = "已下单";
						purchaseOrderTaobaoInfo.time = dataReader["PurchaseDate"].ToString();
					}
				}
			}
			return purchaseOrderTaobaoInfo;
		}
		public override bool CreatePurchaseOrder(PurchaseOrderInfo purchaseOrder, System.Data.Common.DbTransaction dbTran)
		{
			bool result;
			if (purchaseOrder == null)
			{
				result = false;
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("INSERT INTO Hishop_PurchaseOrders(PurchaseOrderId, OrderId, Remark, ManagerMark, ManagerRemark, AdjustedDiscount,PaymentTypeId,PaymentType,Gateway,PurchaseStatus, CloseReason, PurchaseDate, DistributorId, Distributorname,DistributorEmail, DistributorRealName, DistributorQQ, DistributorWangwang, DistributorMSN,ShippingRegion, Address, ZipCode, ShipTo, TelPhone, CellPhone, ShipToDate, ShippingModeId, ModeName,RealShippingModeId, RealModeName, RegionId, Freight, AdjustedFreight, ShipOrderNumber, Weight,ExpressCompanyName,ExpressCompanyAbb,RefundStatus, RefundAmount, RefundRemark, OrderTotal, PurchaseProfit, PurchaseTotal, TaobaoOrderId,Tax,InvoiceTitle)VALUES (@PurchaseOrderId, @OrderId, @Remark, @ManagerMark, @ManagerRemark, @AdjustedDiscount,@PaymentTypeId,@PaymentType,@Gateway,@PurchaseStatus, @CloseReason, @PurchaseDate, @DistributorId, @Distributorname,@DistributorEmail, @DistributorRealName, @DistributorQQ, @DistributorWangwang, @DistributorMSN,@ShippingRegion, @Address, @ZipCode, @ShipTo, @TelPhone, @CellPhone, @ShipToDate, @ShippingModeId, @ModeName,@RealShippingModeId, @RealModeName, @RegionId, @Freight, @AdjustedFreight, @ShipOrderNumber, @PurchaseWeight,@ExpressCompanyName,@ExpressCompanyAbb,@RefundStatus, @RefundAmount, @RefundRemark, @OrderTotal, @PurchaseProfit, @PurchaseTotal, @TaobaoOrderId,@Tax,@InvoiceTitle);");
				this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrder.PurchaseOrderId);
				if (purchaseOrder.OrderId == null)
				{
					this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, DBNull.Value);
				}
				else
				{
					this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, purchaseOrder.OrderId);
				}
				if (purchaseOrder.Remark == null)
				{
					this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, DBNull.Value);
				}
				else
				{
					this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, purchaseOrder.Remark);
				}
				if (purchaseOrder.ManagerMark.HasValue)
				{
					this.database.AddInParameter(sqlStringCommand, "ManagerMark", System.Data.DbType.Int32, (int)purchaseOrder.ManagerMark.Value);
				}
				else
				{
					this.database.AddInParameter(sqlStringCommand, "ManagerMark", System.Data.DbType.Int32, DBNull.Value);
				}
				this.database.AddInParameter(sqlStringCommand, "ManagerRemark", System.Data.DbType.String, purchaseOrder.ManagerRemark);
				this.database.AddInParameter(sqlStringCommand, "AdjustedDiscount", System.Data.DbType.Currency, purchaseOrder.AdjustedDiscount);
				this.database.AddInParameter(sqlStringCommand, "PurchaseStatus", System.Data.DbType.Int32, (int)purchaseOrder.PurchaseStatus);
				this.database.AddInParameter(sqlStringCommand, "CloseReason", System.Data.DbType.String, purchaseOrder.CloseReason);
				this.database.AddInParameter(sqlStringCommand, "PurchaseDate", System.Data.DbType.DateTime, DateTime.Now);
				this.database.AddInParameter(sqlStringCommand, "DistributorId", System.Data.DbType.Int32, purchaseOrder.DistributorId);
				this.database.AddInParameter(sqlStringCommand, "Distributorname", System.Data.DbType.String, purchaseOrder.Distributorname);
				this.database.AddInParameter(sqlStringCommand, "DistributorEmail", System.Data.DbType.String, purchaseOrder.DistributorEmail);
				this.database.AddInParameter(sqlStringCommand, "DistributorRealName", System.Data.DbType.String, purchaseOrder.DistributorRealName);
				this.database.AddInParameter(sqlStringCommand, "DistributorQQ", System.Data.DbType.String, purchaseOrder.DistributorQQ);
				this.database.AddInParameter(sqlStringCommand, "DistributorWangwang", System.Data.DbType.String, purchaseOrder.DistributorWangwang);
				this.database.AddInParameter(sqlStringCommand, "DistributorMSN", System.Data.DbType.String, purchaseOrder.DistributorMSN);
				this.database.AddInParameter(sqlStringCommand, "ShippingRegion", System.Data.DbType.String, purchaseOrder.ShippingRegion);
				this.database.AddInParameter(sqlStringCommand, "Address", System.Data.DbType.String, purchaseOrder.Address);
				this.database.AddInParameter(sqlStringCommand, "ZipCode", System.Data.DbType.String, purchaseOrder.ZipCode);
				this.database.AddInParameter(sqlStringCommand, "ShipTo", System.Data.DbType.String, purchaseOrder.ShipTo);
				this.database.AddInParameter(sqlStringCommand, "TelPhone", System.Data.DbType.String, purchaseOrder.TelPhone);
				this.database.AddInParameter(sqlStringCommand, "CellPhone", System.Data.DbType.String, purchaseOrder.CellPhone);
				this.database.AddInParameter(sqlStringCommand, "ShipToDate", System.Data.DbType.String, purchaseOrder.ShipToDate);
				this.database.AddInParameter(sqlStringCommand, "ShippingModeId", System.Data.DbType.Int32, purchaseOrder.ShippingModeId);
				this.database.AddInParameter(sqlStringCommand, "ModeName", System.Data.DbType.String, purchaseOrder.ModeName);
				this.database.AddInParameter(sqlStringCommand, "RealShippingModeId", System.Data.DbType.Int32, purchaseOrder.RealShippingModeId);
				this.database.AddInParameter(sqlStringCommand, "RealModeName", System.Data.DbType.String, purchaseOrder.RealModeName);
				this.database.AddInParameter(sqlStringCommand, "RegionId", System.Data.DbType.Int32, purchaseOrder.RegionId);
				this.database.AddInParameter(sqlStringCommand, "Freight", System.Data.DbType.Currency, purchaseOrder.Freight);
				this.database.AddInParameter(sqlStringCommand, "AdjustedFreight", System.Data.DbType.Currency, purchaseOrder.AdjustedFreight);
				this.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", System.Data.DbType.String, purchaseOrder.ShipOrderNumber);
				this.database.AddInParameter(sqlStringCommand, "PurchaseWeight", System.Data.DbType.Decimal, purchaseOrder.Weight);
				this.database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", System.Data.DbType.String, purchaseOrder.ExpressCompanyAbb);
				this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", System.Data.DbType.String, purchaseOrder.ExpressCompanyName);
				this.database.AddInParameter(sqlStringCommand, "RefundStatus", System.Data.DbType.Int32, (int)purchaseOrder.RefundStatus);
				this.database.AddInParameter(sqlStringCommand, "RefundAmount", System.Data.DbType.Currency, purchaseOrder.RefundAmount);
				this.database.AddInParameter(sqlStringCommand, "RefundRemark", System.Data.DbType.String, purchaseOrder.RefundRemark);
				this.database.AddInParameter(sqlStringCommand, "PaymentTypeId", System.Data.DbType.Int32, purchaseOrder.PaymentTypeId);
				this.database.AddInParameter(sqlStringCommand, "PaymentType", System.Data.DbType.String, purchaseOrder.PaymentType);
				this.database.AddInParameter(sqlStringCommand, "Gateway", System.Data.DbType.String, purchaseOrder.Gateway);
				this.database.AddInParameter(sqlStringCommand, "OrderTotal", System.Data.DbType.Currency, purchaseOrder.OrderTotal);
				this.database.AddInParameter(sqlStringCommand, "PurchaseProfit", System.Data.DbType.Currency, purchaseOrder.GetPurchaseProfit());
				this.database.AddInParameter(sqlStringCommand, "PurchaseTotal", System.Data.DbType.Currency, purchaseOrder.GetPurchaseTotal());
				this.database.AddInParameter(sqlStringCommand, "TaobaoOrderId", System.Data.DbType.String, purchaseOrder.TaobaoOrderId);
				this.database.AddInParameter(sqlStringCommand, "Tax", System.Data.DbType.Currency, purchaseOrder.Tax);
				this.database.AddInParameter(sqlStringCommand, "InvoiceTitle", System.Data.DbType.String, purchaseOrder.InvoiceTitle);
				int num = 0;
				foreach (PurchaseOrderItemInfo current in purchaseOrder.PurchaseOrderItems)
				{
					string text = num.ToString();
					stringBuilder.Append("INSERT INTO Hishop_PurchaseOrderItems(PurchaseOrderId, SkuId, ProductId, SKU, Quantity,  CostPrice, ").Append("ItemListPrice, ItemPurchasePrice, ItemDescription, ItemHomeSiteDescription, SKUContent, ThumbnailsUrl, Weight) VALUES( @PurchaseOrderId").Append(",@SkuId").Append(text).Append(",@ProductId").Append(text).Append(",@SKU").Append(text).Append(",@Quantity").Append(text).Append(",@CostPrice").Append(text).Append(",@ItemListPrice").Append(text).Append(",@ItemPurchasePrice").Append(text).Append(",@ItemDescription").Append(text).Append(",@ItemHomeSiteDescription").Append(text).Append(",@SKUContent").Append(text).Append(",@ThumbnailsUrl").Append(text).Append(",@Weight").Append(text).Append(");");
					this.database.AddInParameter(sqlStringCommand, "SkuId" + text, System.Data.DbType.String, current.SkuId);
					this.database.AddInParameter(sqlStringCommand, "ProductId" + text, System.Data.DbType.Int32, current.ProductId);
					this.database.AddInParameter(sqlStringCommand, "SKU" + text, System.Data.DbType.String, current.SKU);
					this.database.AddInParameter(sqlStringCommand, "Quantity" + text, System.Data.DbType.Int32, current.Quantity);
					this.database.AddInParameter(sqlStringCommand, "CostPrice" + text, System.Data.DbType.Currency, current.ItemCostPrice);
					this.database.AddInParameter(sqlStringCommand, "ItemListPrice" + text, System.Data.DbType.Currency, current.ItemListPrice);
					this.database.AddInParameter(sqlStringCommand, "ItemPurchasePrice" + text, System.Data.DbType.Currency, current.ItemPurchasePrice);
					this.database.AddInParameter(sqlStringCommand, "ItemDescription" + text, System.Data.DbType.String, current.ItemDescription);
					this.database.AddInParameter(sqlStringCommand, "ItemHomeSiteDescription" + text, System.Data.DbType.String, current.ItemHomeSiteDescription);
					this.database.AddInParameter(sqlStringCommand, "SKUContent" + text, System.Data.DbType.String, current.SKUContent);
					this.database.AddInParameter(sqlStringCommand, "ThumbnailsUrl" + text, System.Data.DbType.String, current.ThumbnailsUrl);
					this.database.AddInParameter(sqlStringCommand, "Weight" + text, System.Data.DbType.Int32, current.ItemWeight);
					num++;
				}
				foreach (PurchaseOrderGiftInfo current2 in purchaseOrder.PurchaseOrderGifts)
				{
					string text = num.ToString();
					stringBuilder.Append("INSERT INTO Hishop_PurchaseOrderGifts(PurchaseOrderId, GiftId, GiftName, CostPrice, PurchasePrice, ").Append("ThumbnailsUrl, Quantity) VALUES( @PurchaseOrderId,").Append("@GiftId").Append(text).Append(",@GiftName").Append(text).Append(",@CostPrice").Append(text).Append(",@PurchasePrice").Append(text).Append(",@ThumbnailsUrl").Append(text).Append(",@Quantity").Append(text).Append(");");
					this.database.AddInParameter(sqlStringCommand, "GiftId" + text, System.Data.DbType.Int32, current2.GiftId);
					this.database.AddInParameter(sqlStringCommand, "GiftName" + text, System.Data.DbType.String, current2.GiftName);
					this.database.AddInParameter(sqlStringCommand, "Quantity" + text, System.Data.DbType.Int32, current2.Quantity);
					this.database.AddInParameter(sqlStringCommand, "CostPrice" + text, System.Data.DbType.Currency, current2.CostPrice);
					this.database.AddInParameter(sqlStringCommand, "PurchasePrice" + text, System.Data.DbType.Currency, current2.PurchasePrice);
					this.database.AddInParameter(sqlStringCommand, "ThumbnailsUrl" + text, System.Data.DbType.String, current2.ThumbnailsUrl);
					num++;
				}
				sqlStringCommand.CommandText = stringBuilder.ToString().Remove(stringBuilder.Length - 1);
				if (dbTran != null)
				{
					result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
				}
				else
				{
					result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
				}
			}
			return result;
		}
		public override void UpdateProductStock(string purchaseOrderId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_SKUs Set Stock = CASE WHEN (Stock - (SELECT oi.Quantity FROM Hishop_PurchaseOrderItems oi Where oi.SkuId =Hishop_SKUs.SkuId AND oi.PurchaseOrderId =@PurchaseOrderId ))<=0 Then 0 ELSE Stock - (SELECT oi.Quantity FROM Hishop_PurchaseOrderItems oi  Where oi.SkuId =Hishop_SKUs.SkuId AND  oi.PurchaseOrderId =@PurchaseOrderId ) END WHERE Hishop_SKUs.SkuId  IN (Select SkuId FROM Hishop_PurchaseOrderItems Where PurchaseOrderId =@PurchaseOrderId )");
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrderId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override bool IsExitPurchaseOrder(long long_0)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_PurchaseOrders WHERE TaobaoOrderId = @TaobaoOrderId");
			this.database.AddInParameter(sqlStringCommand, "TaobaoOrderId", System.Data.DbType.String, long_0.ToString());
			return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
		}
		public override DbQueryResult GetPurchaseOrders(PurchaseOrderQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("DistributorId = {0}", HiContext.Current.User.UserId);
			if (query.IsManualPurchaseOrder)
			{
				stringBuilder.AppendFormat("AND OrderId IS NULL", new object[0]);
			}
			else
			{
				stringBuilder.AppendFormat("AND OrderId IS NOT NULL", new object[0]);
			}
			if (!string.IsNullOrEmpty(query.ShipTo))
			{
				stringBuilder.AppendFormat(" AND ShipTo LIKE '%{0}%'", DataHelper.CleanSearchString(query.ShipTo));
			}
			if (!string.IsNullOrEmpty(query.PurchaseOrderId))
			{
				stringBuilder.AppendFormat(" AND PurchaseOrderId = '{0}'", query.PurchaseOrderId);
			}
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				stringBuilder.AppendFormat(" AND OrderId = '{0}'", query.OrderId);
			}
			if (!string.IsNullOrEmpty(query.ProductName))
			{
				stringBuilder.AppendFormat(" AND PurchaseOrderId IN (SELECT PurchaseOrderId FROM Hishop_PurchaseOrderItems WHERE ItemDescription LIKE '%{0}%')", DataHelper.CleanSearchString(query.ProductName));
			}
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND PurchaseDate >= '{0}'", query.StartDate.Value);
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND PurchaseDate <= '{0}'", query.EndDate.Value);
			}
			if (query.PurchaseStatus != OrderStatus.All)
			{
				stringBuilder.AppendFormat(" AND PurchaseStatus ={0}", Convert.ToInt32(query.PurchaseStatus));
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "Hishop_PurchaseOrders", "PurchaseOrderId", stringBuilder.ToString(), "OrderId,ShipTo,RefundStatus, PurchaseOrderId, TaobaoOrderId, PurchaseDate, OrderTotal, PurchaseTotal, PurchaseStatus,PaymentTypeId,ShipOrderNumber,Gateway");
		}
		public override PurchaseOrderInfo GetPurchaseOrder(string purchaseOrderId)
		{
			PurchaseOrderInfo purchaseOrderInfo = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PurchaseOrders Where PurchaseOrderId = @PurchaseOrderId; SELECT  * FROM Hishop_PurchaseOrderGifts Where PurchaseOrderId = @PurchaseOrderId; SELECT  * FROM Hishop_PurchaseOrderItems Where PurchaseOrderId = @PurchaseOrderId ");
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrderId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					purchaseOrderInfo = DataMapper.PopulatePurchaseOrder(dataReader);
				}
				dataReader.NextResult();
				while (dataReader.Read())
				{
					purchaseOrderInfo.PurchaseOrderGifts.Add(DataMapper.PopulatePurchaseOrderGift(dataReader));
				}
				dataReader.NextResult();
				while (dataReader.Read())
				{
					purchaseOrderInfo.PurchaseOrderItems.Add(DataMapper.PopulatePurchaseOrderItem(dataReader));
				}
			}
			return purchaseOrderInfo;
		}
		public override bool DeletePurchaseShoppingCartItem(string string_0)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_PurchaseShoppingCarts WHERE SkuId=@SkuId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, string_0);
			return this.database.ExecuteNonQuery(sqlStringCommand) >= 1;
		}
		public override void ClearPurchaseShoppingCart()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_PurchaseShoppingCarts WHERE  DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override System.Data.DataTable GetRecentlyPurchaseOrders(out int number)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TOP 12 * FROM Hishop_PurchaseOrders WHERE DistributorId=@DistributorId ORDER BY PurchaseDate DESC");
			this.database.AddInParameter(sqlStringCommand, "DistributorId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			sqlStringCommand = this.database.GetSqlStringCommand("SELECT count(*) FROM Hishop_PurchaseOrders WHERE   PurchaseStatus=1 AND DistributorId=@DistributorId");
			this.database.AddInParameter(sqlStringCommand, "DistributorId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			number = Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
			return result;
		}
		public override System.Data.DataTable GetRecentlyManualPurchaseOrders(out int number)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT TOP 12 * FROM Hishop_PurchaseOrders WHERE   DistributorId=@DistributorId AND OrderId IS NULL ORDER BY PurchaseDate DESC");
			this.database.AddInParameter(sqlStringCommand, "DistributorId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			sqlStringCommand = this.database.GetSqlStringCommand("SELECT count(*) FROM Hishop_PurchaseOrders WHERE   PurchaseStatus=1 AND DistributorId=@DistributorId AND OrderId IS NULL");
			this.database.AddInParameter(sqlStringCommand, "DistributorId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			number = Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
			return result;
		}
		public override PurchaseOrderInfo GetPurchaseByOrderId(string orderId)
		{
			PurchaseOrderInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PurchaseOrders Where OrderId = @OrderId AND DistributorId=@DistributorId");
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			this.database.AddInParameter(sqlStringCommand, "DistributorId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulatePurchaseOrder(dataReader);
				}
			}
			return result;
		}
		public override int DeletePurchaseOrde(string purchaseOrderId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_PurchaseOrders WHERE PurchaseOrderId=@PurchaseOrderId AND DistributorId=@DistributorId");
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrderId);
			this.database.AddInParameter(sqlStringCommand, "DistributorId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override decimal GetRefundMoney(PurchaseOrderInfo purchaseOrder, out decimal refundMoney)
		{
			string text = string.Format("SELECT RefundMoney FROM Hishop_PurchaseOrderReturns WHERE HandleStatus=1 AND PurchaseOrderId='{0}'", purchaseOrder.PurchaseOrderId);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			decimal num = Convert.ToDecimal(this.database.ExecuteScalar(sqlStringCommand));
			return refundMoney = num;
		}
		public override bool SetPayment(string purchaseOrderId, int paymentTypeId, string paymentType, string gateway)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET PaymentTypeId=@PaymentTypeId,PaymentType=@PaymentType,Gateway=@Gateway WHERE PurchaseOrderId = @PurchaseOrderId");
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrderId);
			this.database.AddInParameter(sqlStringCommand, "PaymentTypeId", System.Data.DbType.Int32, paymentTypeId);
			this.database.AddInParameter(sqlStringCommand, "PaymentType", System.Data.DbType.String, paymentType);
			this.database.AddInParameter(sqlStringCommand, "Gateway", System.Data.DbType.String, gateway);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override bool ClosePurchaseOrder(PurchaseOrderInfo purchaseOrder)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET PurchaseStatus=@PurchaseStatus,CloseReason=@CloseReason WHERE PurchaseOrderId = @PurchaseOrderId AND DistributorId=@DistributorId");
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrder.PurchaseOrderId);
			this.database.AddInParameter(sqlStringCommand, "CloseReason", System.Data.DbType.String, purchaseOrder.CloseReason);
			this.database.AddInParameter(sqlStringCommand, "PurchaseStatus", System.Data.DbType.Int32, 4);
			this.database.AddInParameter(sqlStringCommand, "DistributorId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override bool ConfirmPurchaseOrderFinish(PurchaseOrderInfo purchaseOrder)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET PurchaseStatus=@PurchaseStatus,FinishDate=@FinishDate WHERE PurchaseOrderId = @PurchaseOrderId AND DistributorId=@DistributorId");
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrder.PurchaseOrderId);
			this.database.AddInParameter(sqlStringCommand, "FinishDate", System.Data.DbType.DateTime, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "PurchaseStatus", System.Data.DbType.Int32, 5);
			this.database.AddInParameter(sqlStringCommand, "DistributorId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override bool ConfirmPay(BalanceDetailInfo balance, string purchaseOrderId)
		{
			bool result;
			bool flag;
			using (System.Data.Common.DbConnection dbConnection = this.database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_DistributorBalanceDetails(UserId,UserName,TradeDate,TradeType,Expenses,Balance, Remark) VALUES(@UserId,@UserName,@TradeDate,@TradeType,@Expenses,@Balance, @Remark)");
					this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, balance.UserId);
					this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, balance.UserName);
					this.database.AddInParameter(sqlStringCommand, "TradeDate", System.Data.DbType.DateTime, balance.TradeDate);
					this.database.AddInParameter(sqlStringCommand, "TradeType", System.Data.DbType.Int32, (int)balance.TradeType);
					this.database.AddInParameter(sqlStringCommand, "Expenses", System.Data.DbType.Currency, balance.Expenses);
					this.database.AddInParameter(sqlStringCommand, "Balance", System.Data.DbType.Currency, balance.Balance);
					this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, balance.Remark);
					if (this.database.ExecuteNonQuery(sqlStringCommand, dbTransaction) <= 0)
					{
						dbTransaction.Rollback();
						result = false;
						return result;
					}
					System.Data.Common.DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET PurchaseStatus=@PurchaseStatus,PayDate=@PayDate WHERE PurchaseOrderId = @PurchaseOrderId AND DistributorId=@DistributorId");
					this.database.AddInParameter(sqlStringCommand2, "PurchaseOrderId", System.Data.DbType.String, purchaseOrderId);
					this.database.AddInParameter(sqlStringCommand2, "PayDate", System.Data.DbType.DateTime, DateTime.Now);
					this.database.AddInParameter(sqlStringCommand2, "DistributorId", System.Data.DbType.String, HiContext.Current.User.UserId);
					this.database.AddInParameter(sqlStringCommand2, "PurchaseStatus", System.Data.DbType.Int32, 2);
					if (this.database.ExecuteNonQuery(sqlStringCommand2, dbTransaction) <= 0)
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
		public override bool ConfirmPay(string purchaseOrderId)
		{
			bool result;
			bool flag;
			using (System.Data.Common.DbConnection dbConnection = this.database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET PurchaseStatus=@PurchaseStatus,PayDate=@PayDate WHERE PurchaseOrderId = @PurchaseOrderId");
					this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrderId);
					this.database.AddInParameter(sqlStringCommand, "PayDate", System.Data.DbType.DateTime, DateTime.Now);
					this.database.AddInParameter(sqlStringCommand, "PurchaseStatus", System.Data.DbType.Int32, 2);
					if (this.database.ExecuteNonQuery(sqlStringCommand, dbTransaction) <= 0)
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
		public override bool BatchConfirmPay(BalanceDetailInfo balance, string purchaseOrderIds)
		{
			bool flag = false;
			bool result;
			using (System.Data.Common.DbConnection dbConnection = this.database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					string str = "";
					string[] array = purchaseOrderIds.Trim().Split(new char[]
					{
						','
					});
					for (int i = 0; i < array.Length; i++)
					{
						string text = array[i];
						PurchaseOrderInfo purchaseOrder = this.GetPurchaseOrder(text);
						if (purchaseOrder.CheckAction(PurchaseOrderActions.DISTRIBUTOR_CONFIRM_PAY))
						{
							str = str + text + ",";
							OrderStatus purchaseStatus = purchaseOrder.PurchaseStatus;
							if (purchaseStatus == OrderStatus.WaitBuyerPay)
							{
								System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET Gateway = @Gateway, PaymentTypeId = (select ModeId from Hishop_PaymentTypes where Gateway = @Gateway),PaymentType = (select Name from Hishop_PaymentTypes where Gateway = @Gateway),PurchaseStatus=@PurchaseStatus,PayDate=@PayDate WHERE PurchaseOrderId = @PurchaseOrderId");
								this.database.AddInParameter(sqlStringCommand, "Gateway", System.Data.DbType.String, "hishop.plugins.payment.advancerequest");
								this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, text);
								this.database.AddInParameter(sqlStringCommand, "PayDate", System.Data.DbType.DateTime, DateTime.Now);
								this.database.AddInParameter(sqlStringCommand, "PurchaseStatus", System.Data.DbType.Int32, 2);
								if (this.database.ExecuteNonQuery(sqlStringCommand, dbTransaction) <= 0)
								{
									dbTransaction.Rollback();
									result = false;
									return result;
								}
								this.UpdateProductStock(purchaseOrder.PurchaseOrderId);
								this.UpdateDistributorAccount(purchaseOrder.GetPurchaseTotal());
								Users.ClearUserCache(Users.GetUser(purchaseOrder.DistributorId));
								flag = true;
							}
						}
					}
					System.Data.Common.DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand("INSERT INTO Hishop_DistributorBalanceDetails(UserId,UserName,TradeDate,TradeType,Expenses,Balance, Remark) VALUES(@UserId,@UserName,@TradeDate,@TradeType,@Expenses,@Balance, @Remark)");
					this.database.AddInParameter(sqlStringCommand2, "UserId", System.Data.DbType.Int32, balance.UserId);
					this.database.AddInParameter(sqlStringCommand2, "UserName", System.Data.DbType.String, balance.UserName);
					this.database.AddInParameter(sqlStringCommand2, "TradeDate", System.Data.DbType.DateTime, balance.TradeDate);
					this.database.AddInParameter(sqlStringCommand2, "TradeType", System.Data.DbType.Int32, (int)balance.TradeType);
					this.database.AddInParameter(sqlStringCommand2, "Expenses", System.Data.DbType.Currency, balance.Expenses);
					this.database.AddInParameter(sqlStringCommand2, "Balance", System.Data.DbType.Currency, balance.Balance);
					this.database.AddInParameter(sqlStringCommand2, "Remark", System.Data.DbType.String, balance.Remark);
					if (this.database.ExecuteNonQuery(sqlStringCommand2, dbTransaction) <= 0)
					{
						dbTransaction.Rollback();
						result = false;
						return result;
					}
					dbTransaction.Commit();
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
		public override bool GetNotPayment(string purchaseOrderId)
		{
			bool result = false;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select PurchaseStatus from dbo.Hishop_PurchaseOrders where PurchaseOrderId=@PurchaseOrderId AND (Gateway!='hishop.plugins.payment.podrequest' or Gateway is null)");
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrderId);
			if (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) == 1)
			{
				result = true;
			}
			return result;
		}
		public override bool BatchConfirmPay(string purchaseOrderIds)
		{
			bool flag = false;
			bool result;
			using (System.Data.Common.DbConnection dbConnection = this.database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					string str = "";
					string[] array = purchaseOrderIds.Split(new char[]
					{
						','
					});
					for (int i = 0; i < array.Length; i++)
					{
						string text = array[i];
						PurchaseOrderInfo purchaseOrder = this.GetPurchaseOrder(text);
						if (purchaseOrder.CheckAction(PurchaseOrderActions.DISTRIBUTOR_CONFIRM_PAY))
						{
							str = str + text + ",";
							OrderStatus purchaseStatus = purchaseOrder.PurchaseStatus;
							if (purchaseStatus == OrderStatus.WaitBuyerPay)
							{
								System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET PurchaseStatus=@PurchaseStatus,PayDate=@PayDate WHERE PurchaseOrderId = @PurchaseOrderId AND DistributorId=@DistributorId");
								this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, text);
								this.database.AddInParameter(sqlStringCommand, "PayDate", System.Data.DbType.DateTime, DateTime.Now);
								this.database.AddInParameter(sqlStringCommand, "DistributorId", System.Data.DbType.String, HiContext.Current.User.UserId);
								this.database.AddInParameter(sqlStringCommand, "PurchaseStatus", System.Data.DbType.Int32, 2);
								if (this.database.ExecuteNonQuery(sqlStringCommand, dbTransaction) <= 0)
								{
									dbTransaction.Rollback();
									result = false;
									return result;
								}
								this.UpdateProductStock(purchaseOrder.PurchaseOrderId);
								this.UpdateDistributorAccount(purchaseOrder.GetPurchaseTotal());
								Users.ClearUserCache(Users.GetUser(purchaseOrder.DistributorId));
								flag = true;
							}
						}
					}
					dbTransaction.Commit();
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
		public override void UpdateDistributorAccount(decimal expenditureAdd)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Distributors SET Expenditure=Expenditure+@expenditureAdd, PurchaseOrder = PurchaseOrder + 1 WHERE UserId=@UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "expenditureAdd", System.Data.DbType.Decimal, expenditureAdd);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override bool SavePurchaseOrderRemark(string purchaseOrderId, string remark)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET Remark=@Remark WHERE PurchaseOrderId=@PurchaseOrderId AND DistributorId=@DistributorId");
			this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, remark);
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrderId);
			this.database.AddInParameter(sqlStringCommand, "DistributorId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override DbQueryResult GetPurchaseOrderGifts(PurchaseOrderGiftQuery query)
		{
			DbQueryResult dbQueryResult = new DbQueryResult();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT top {0} * from Hishop_PurchaseOrderGifts where PurchaseOrderId=@PurchaseOrderId", query.PageSize);
			if (query.PageIndex == 1)
			{
				stringBuilder.Append(" ORDER BY GiftId ASC");
			}
			else
			{
				stringBuilder.AppendFormat(" and GiftId > (SELECT max(GiftId) from (SELECT top {0} GiftId from Hishop_PurchaseOrderGifts where PurchaseOrderId=@PurchaseOrderId ORDER BY GiftId ASC ) as tbltemp) ORDER BY GiftId ASC", (query.PageIndex - 1) * query.PageSize);
			}
			if (query.IsCount)
			{
				stringBuilder.AppendFormat(";SELECT count(GiftId) as Total from Hishop_PurchaseOrderGifts where PurchaseOrderId=@PurchaseOrderId", new object[0]);
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, query.PurchaseOrderId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (query.IsCount && dataReader.NextResult())
				{
					dataReader.Read();
					dbQueryResult.TotalRecords = dataReader.GetInt32(0);
				}
			}
			return dbQueryResult;
		}
		public override bool DeletePurchaseOrderGift(string purchaseOrderId, int giftId, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_PurchaseOrderGifts WHERE PurchaseOrderId =@PurchaseOrderId AND GiftId=@GiftId ");
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrderId);
			this.database.AddInParameter(sqlStringCommand, "GiftId", System.Data.DbType.Int32, giftId);
			bool result;
			if (dbTran != null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
			}
			return result;
		}
		public override bool ClearPurchaseOrderGifts(string purchaseOrderId, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_PurchaseOrderGifts WHERE PurchaseOrderId =@PurchaseOrderId");
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrderId);
			bool result;
			if (dbTran != null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) >= 1);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
			}
			return result;
		}
		public override bool ResetPurchaseTotal(PurchaseOrderInfo purchaseOrder, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_PurchaseOrders set PurchaseTotal=@PurchaseTotal,PurchaseProfit=@PurchaseProfit where PurchaseOrderId=@PurchaseOrderId");
			this.database.AddInParameter(sqlStringCommand, "PurchaseTotal", System.Data.DbType.Decimal, purchaseOrder.GetPurchaseTotal());
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrder.PurchaseOrderId);
			this.database.AddInParameter(sqlStringCommand, "PurchaseProfit", System.Data.DbType.Decimal, purchaseOrder.GetPurchaseProfit());
			bool result;
			if (dbTran != null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
			}
			return result;
		}
		public override bool AddPurchaseOrderGift(string purchaseOrderId, GiftInfo gift, int quantity, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * from Hishop_PurchaseOrderGifts where PurchaseOrderId=@PurchaseOrderId AND GiftId=@GiftId");
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrderId);
			this.database.AddInParameter(sqlStringCommand, "GiftId", System.Data.DbType.Int32, gift.GiftId);
			bool result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (!dataReader.Read())
				{
					System.Data.Common.DbCommand sqlStringCommand2 = this.database.GetSqlStringCommand("INSERT INTO Hishop_PurchaseOrderGifts(PurchaseOrderId,GiftId,GiftName,CostPrice,PurchasePrice,ThumbnailsUrl,Quantity) VALUES(@PurchaseOrderId,@GiftId,@GiftName,@CostPrice,@PurchasePrice,@ThumbnailsUrl,@Quantity)");
					this.database.AddInParameter(sqlStringCommand2, "PurchaseOrderId", System.Data.DbType.String, purchaseOrderId);
					this.database.AddInParameter(sqlStringCommand2, "GiftId", System.Data.DbType.Int32, gift.GiftId);
					this.database.AddInParameter(sqlStringCommand2, "GiftName", System.Data.DbType.String, gift.Name);
					this.database.AddInParameter(sqlStringCommand2, "CostPrice", System.Data.DbType.Currency, gift.CostPrice);
					this.database.AddInParameter(sqlStringCommand2, "PurchasePrice", System.Data.DbType.Currency, gift.PurchasePrice);
					this.database.AddInParameter(sqlStringCommand2, "ThumbnailsUrl", System.Data.DbType.String, gift.ThumbnailUrl40);
					this.database.AddInParameter(sqlStringCommand2, "Quantity", System.Data.DbType.Int32, quantity);
					if (dbTran != null)
					{
						result = (this.database.ExecuteNonQuery(sqlStringCommand2, dbTran) == 1);
					}
					else
					{
						result = (this.database.ExecuteNonQuery(sqlStringCommand2) == 1);
					}
				}
				else
				{
					System.Data.Common.DbCommand sqlStringCommand3 = this.database.GetSqlStringCommand("update Hishop_PurchaseOrderGifts set Quantity=@Quantity where PurchaseOrderId=@PurchaseOrderId AND GiftId=@GiftId");
					this.database.AddInParameter(sqlStringCommand3, "PurchaseOrderId", System.Data.DbType.String, purchaseOrderId);
					this.database.AddInParameter(sqlStringCommand3, "GiftId", System.Data.DbType.Int32, gift.GiftId);
					this.database.AddInParameter(sqlStringCommand3, "Quantity", System.Data.DbType.Int32, (int)dataReader["Quantity"] + quantity);
					if (dbTran != null)
					{
						result = (this.database.ExecuteNonQuery(sqlStringCommand3, dbTran) == 1);
					}
					else
					{
						result = (this.database.ExecuteNonQuery(sqlStringCommand3) == 1);
					}
				}
			}
			return result;
		}
		public override IList<PurchaseShoppingCartItemInfo> GetPurchaseShoppingCartItemInfos()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PurchaseShoppingCarts WHERE DistributorUserId=@DistributorUserId AND ProductId IN (SELECT ProductId FROM Hishop_Products WHERE PenetrationStatus=1 AND LineId IN (SELECT LineId FROM Hishop_DistributorProductLines WHERE UserId=@DistributorUserId))");
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			IList<PurchaseShoppingCartItemInfo> list = new List<PurchaseShoppingCartItemInfo>();
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulatePurchaseShoppingCartItemInfo(dataReader));
				}
			}
			return list;
		}
		public override bool AddPurchaseItem(PurchaseShoppingCartItemInfo item)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_PurchaseShoppingCarts (SkuId, ProductId,SKU,DistributorUserId,CostPrice,Quantity,ItemListPrice,ItemPurchasePrice,ItemDescription,ThumbnailsUrl,Weight,SKUContent)VALUES(@SkuId, @ProductId,@SKU,@DistributorUserId,@CostPrice,@Quantity,@ItemListPrice,@ItemPurchasePrice,@ItemDescription,@ThumbnailsUrl,@Weight,@SKUContent)");
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, item.SkuId);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, item.ProductId);
			this.database.AddInParameter(sqlStringCommand, "SKU", System.Data.DbType.String, item.SKU);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "Quantity", System.Data.DbType.Int32, item.Quantity);
			this.database.AddInParameter(sqlStringCommand, "ItemListPrice", System.Data.DbType.Currency, item.ItemListPrice);
			this.database.AddInParameter(sqlStringCommand, "ItemPurchasePrice", System.Data.DbType.Currency, item.ItemPurchasePrice);
			this.database.AddInParameter(sqlStringCommand, "CostPrice", System.Data.DbType.Currency, item.CostPrice);
			this.database.AddInParameter(sqlStringCommand, "ItemDescription", System.Data.DbType.String, item.ItemDescription);
			this.database.AddInParameter(sqlStringCommand, "ThumbnailsUrl", System.Data.DbType.String, item.ThumbnailsUrl);
			this.database.AddInParameter(sqlStringCommand, "Weight", System.Data.DbType.Decimal, item.ItemWeight);
			this.database.AddInParameter(sqlStringCommand, "SKUContent", System.Data.DbType.String, item.SKUContent);
			bool result;
			try
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
			}
			catch
			{
				result = false;
			}
			return result;
		}
		public override bool AddPurchaseOrderItem(PurchaseShoppingCartItemInfo item, string POrderId)
		{
			StringBuilder stringBuilder = new StringBuilder("INSERT INTO Hishop_PurchaseOrderItems (PurchaseOrderId,SkuId, ProductId,SKU,CostPrice,Quantity,ItemListPrice,ItemPurchasePrice,ItemDescription,ItemHomeSiteDescription,ThumbnailsUrl,Weight,SKUContent)");
			stringBuilder.Append("VALUES(@PurchaseOrderId,@SkuId, @ProductId,@SKU,@CostPrice,@Quantity,@ItemListPrice,@ItemPurchasePrice,@ItemDescription,@ItemHomeSiteDescription,@ThumbnailsUrl,@Weight,@SKUContent);");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, POrderId);
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, item.SkuId);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, item.ProductId);
			this.database.AddInParameter(sqlStringCommand, "SKU", System.Data.DbType.String, item.SKU);
			this.database.AddInParameter(sqlStringCommand, "Quantity", System.Data.DbType.Int32, item.Quantity);
			this.database.AddInParameter(sqlStringCommand, "ItemListPrice", System.Data.DbType.Currency, item.ItemListPrice);
			this.database.AddInParameter(sqlStringCommand, "ItemPurchasePrice", System.Data.DbType.Currency, item.ItemPurchasePrice);
			this.database.AddInParameter(sqlStringCommand, "CostPrice", System.Data.DbType.Currency, item.CostPrice);
			this.database.AddInParameter(sqlStringCommand, "ItemDescription", System.Data.DbType.String, item.ItemDescription);
			this.database.AddInParameter(sqlStringCommand, "ItemHomeSiteDescription", System.Data.DbType.String, item.ItemDescription);
			this.database.AddInParameter(sqlStringCommand, "ThumbnailsUrl", System.Data.DbType.String, item.ThumbnailsUrl);
			this.database.AddInParameter(sqlStringCommand, "Weight", System.Data.DbType.Int32, item.ItemWeight);
			this.database.AddInParameter(sqlStringCommand, "SKUContent", System.Data.DbType.String, item.SKUContent);
			bool result;
			try
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
			}
			catch
			{
				result = false;
			}
			return result;
		}
		public override bool UpdatePurchaseOrderQuantity(string POrderId, string SkuId, int Quantity)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrderItems SET Quantity=@Quantity WHERE PurchaseOrderId=@PurchaseOrderId AND SkuId=@SkuId;");
			this.database.AddInParameter(sqlStringCommand, "Quantity", System.Data.DbType.Int32, Quantity);
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, POrderId);
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, SkuId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override int GetCurrentPOrderItemQuantity(string POrderId, string skuId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Quantity FROM Hishop_PurchaseOrderItems WHERE PurchaseOrderId=@PurchaseOrderId AND SkuId=@SkuId");
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, POrderId);
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, skuId);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int result;
			if (obj != DBNull.Value && obj != null)
			{
				result = (int)obj;
			}
			else
			{
				result = 0;
			}
			return result;
		}
		public override bool DeletePurchaseOrderItem(string POrderId, string skuId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_PurchaseOrderItems WHERE PurchaseOrderId=@PurchaseOrderId AND SkuId=@SkuId");
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, POrderId);
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, skuId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool UpdatePurchaseOrder(PurchaseOrderInfo purchaseOrder)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_PurchaseOrders SET Weight=@Weight,PurchaseProfit=@PurchaseProfit,PurchaseTotal=@PurchaseTotal,AdjustedFreight=@AdjustedFreight WHERE PurchaseOrderId=@PurchaseOrderId");
			this.database.AddInParameter(sqlStringCommand, "Weight", System.Data.DbType.Decimal, purchaseOrder.Weight);
			this.database.AddInParameter(sqlStringCommand, "PurchaseProfit", System.Data.DbType.Decimal, purchaseOrder.GetPurchaseProfit());
			this.database.AddInParameter(sqlStringCommand, "PurchaseTotal", System.Data.DbType.Decimal, purchaseOrder.GetPurchaseTotal());
			this.database.AddInParameter(sqlStringCommand, "AdjustedFreight", System.Data.DbType.Decimal, purchaseOrder.AdjustedFreight);
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrder.PurchaseOrderId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override ShippingModeInfo GetShippingMode(int modeId, bool includeDetail)
		{
			ShippingModeInfo shippingModeInfo = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" SELECT * FROM Hishop_ShippingTypes st INNER JOIN Hishop_ShippingTemplates temp ON st.TemplateId=temp.TemplateId Where ModeId =@ModeId");
			if (includeDetail)
			{
				System.Data.Common.DbCommand expr_1A = sqlStringCommand;
				expr_1A.CommandText += " SELECT * FROM Hishop_ShippingTypeGroups Where TemplateId IN (SELECT TemplateId FROM Hishop_ShippingTypes Where ModeId =@ModeId)";
				System.Data.Common.DbCommand expr_30 = sqlStringCommand;
				expr_30.CommandText += " SELECT * FROM Hishop_ShippingRegions Where TemplateId IN (SELECT TemplateId FROM Hishop_ShippingTypes Where ModeId =@ModeId)";
			}
			this.database.AddInParameter(sqlStringCommand, "ModeId", System.Data.DbType.Int32, modeId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					shippingModeInfo = DataMapper.PopulateShippingMode(dataReader);
				}
				if (includeDetail)
				{
					dataReader.NextResult();
					while (dataReader.Read())
					{
						shippingModeInfo.ModeGroup.Add(DataMapper.PopulateShippingModeGroup(dataReader));
					}
					dataReader.NextResult();
					while (dataReader.Read())
					{
						foreach (ShippingModeGroupInfo current in shippingModeInfo.ModeGroup)
						{
							if (current.GroupId == (int)dataReader["GroupId"])
							{
								current.ModeRegions.Add(DataMapper.PopulateShippingRegion(dataReader));
							}
						}
					}
				}
			}
			return shippingModeInfo;
		}
		public override GiftInfo GetGiftDetails(int giftId)
		{
			GiftInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Gifts WHERE GiftId = @GiftId");
			this.database.AddInParameter(sqlStringCommand, "GiftId", System.Data.DbType.Int32, giftId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateGift(dataReader);
				}
			}
			return result;
		}
		public override DbQueryResult GetGifts(GiftQuery query)
		{
			string filter = null;
			if (!string.IsNullOrEmpty(query.Name))
			{
				filter = string.Format("[Name] LIKE '%{0}%'", DataHelper.CleanSearchString(query.Name));
			}
			Pagination page = query.Page;
			return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Hishop_Gifts", "GiftId", filter, "*");
		}
		public override IList<string> GetExpressCompanysByMode(int modeId)
		{
			IList<string> list = new List<string>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_TemplateRelatedShipping Where ModeId =@ModeId");
			this.database.AddInParameter(sqlStringCommand, "ModeId", System.Data.DbType.Int32, modeId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					if (dataReader["ExpressCompanyName"] != DBNull.Value)
					{
						list.Add((string)dataReader["ExpressCompanyName"]);
					}
				}
			}
			return list;
		}
		private static string BuildOrdersQuery(OrderQuery query)
		{
			if (null == query)
			{
				throw new ArgumentNullException("query");
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT OrderId FROM distro_Orders WHERE DistributorUserId ='{0}' ", HiContext.Current.User.UserId);
			if (query.OrderId != string.Empty && query.OrderId != null)
			{
				stringBuilder.AppendFormat(" AND OrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId));
			}
			else
			{
				if (query.PaymentType.HasValue)
				{
					stringBuilder.AppendFormat(" AND PaymentTypeId = '{0}'", query.PaymentType.Value);
				}
				if (query.GroupBuyId.HasValue)
				{
					stringBuilder.AppendFormat(" AND GroupBuyId = {0}", query.GroupBuyId.Value);
				}
				if (!string.IsNullOrEmpty(query.ProductName))
				{
					stringBuilder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM distro_OrderItems WHERE DistributorUserId ={0} AND ItemDescription LIKE '%{1}%')", HiContext.Current.User.UserId, DataHelper.CleanSearchString(query.ProductName));
				}
				if (!string.IsNullOrEmpty(query.ShipTo))
				{
					stringBuilder.AppendFormat(" AND ShipTo LIKE '%{0}%'", DataHelper.CleanSearchString(query.ShipTo));
				}
				if (!string.IsNullOrEmpty(query.UserName))
				{
					stringBuilder.AppendFormat(" AND Username = '{0}' ", DataHelper.CleanSearchString(query.UserName));
				}
				if (query.Status == OrderStatus.History)
				{
					stringBuilder.AppendFormat(" AND OrderStatus = {0} AND OrderDate < '{1}'", 5, DateTime.Now.AddMonths(-3));
				}
				else
				{
					if (query.Status != OrderStatus.All)
					{
						stringBuilder.AppendFormat(" AND OrderStatus = {0}", (int)query.Status);
					}
				}
				if (query.StartDate.HasValue)
				{
					stringBuilder.AppendFormat(" AND OrderDate >= '{0}'", query.StartDate.Value);
				}
				if (query.EndDate.HasValue)
				{
					stringBuilder.AppendFormat(" AND OrderDate <= '{0}'", query.EndDate.Value);
				}
			}
			if (!string.IsNullOrEmpty(query.SortBy))
			{
				stringBuilder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
			}
			return stringBuilder.ToString();
		}
		public override bool AddMemberPoint(UserPointInfo point)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO distro_PointDetails (OrderId,UserId, TradeDate, TradeType, Increased, Reduced, Points, Remark)VALUES(@OrderId,@UserId, @TradeDate, @TradeType, @Increased, @Reduced, @Points, @Remark)");
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, point.OrderId);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, point.UserId);
			this.database.AddInParameter(sqlStringCommand, "TradeDate", System.Data.DbType.DateTime, point.TradeDate);
			this.database.AddInParameter(sqlStringCommand, "TradeType", System.Data.DbType.Int32, (int)point.TradeType);
			this.database.AddInParameter(sqlStringCommand, "Increased", System.Data.DbType.Int32, point.Increased.HasValue ? point.Increased.Value : 0);
			this.database.AddInParameter(sqlStringCommand, "Reduced", System.Data.DbType.Int32, point.Reduced.HasValue ? point.Reduced.Value : 0);
			this.database.AddInParameter(sqlStringCommand, "Points", System.Data.DbType.Int32, point.Points);
			this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, point.Remark);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override int GetHistoryPoint(int userId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SUM(Increased) FROM distro_PointDetails WHERE UserId = @UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			return (int)this.database.ExecuteScalar(sqlStringCommand);
		}
		public override bool UpdateUserAccount(decimal orderTotal, int totalPoint, int userId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Members SET Expenditure = ISNULL(Expenditure,0) + @OrderPrice, OrderNumber = ISNULL(OrderNumber,0) + 1, Points = @Points WHERE UserId = @UserId AND ParentUserId=@ParentUserId");
			this.database.AddInParameter(sqlStringCommand, "ParentUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "OrderPrice", System.Data.DbType.Decimal, orderTotal);
			this.database.AddInParameter(sqlStringCommand, "Points", System.Data.DbType.Int32, totalPoint);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool ChangeMemberGrade(int userId, int gradId, int points)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ISNULL(Points, 0) AS Point, GradeId FROM distro_MemberGrades WHERE CreateUserId=@CreateUserId Order by Point Desc ");
			this.database.AddInParameter(sqlStringCommand, "CreateUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			bool result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read() && (int)dataReader["GradeId"] != gradId)
				{
					if ((int)dataReader["Point"] <= points)
					{
						result = this.UpdateUserRank(userId, (int)dataReader["GradeId"]);
						return result;
					}
				}
				result = true;
			}
			return result;
		}
		private bool UpdateUserRank(int userId, int gradeId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Members SET GradeId = @GradeId WHERE UserId = @UserId AND ParentUserId=@ParentUserId");
			this.database.AddInParameter(sqlStringCommand, "ParentUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, gradeId);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool UpdateProductSaleCounts(Dictionary<string, LineItemInfo> lineItems)
		{
			bool result;
			if (lineItems.Count <= 0)
			{
				result = false;
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
				StringBuilder stringBuilder = new StringBuilder();
				int num = 0;
				foreach (LineItemInfo current in lineItems.Values)
				{
					stringBuilder.Append("UPDATE distro_Products SET SaleCounts = SaleCounts + @SaleCounts").Append(num).Append(", ShowSaleCounts = ShowSaleCounts + @SaleCounts").Append(num).Append(" WHERE ProductId=@ProductId").Append(num).Append(" AND DistributorUserId=@DistributorUserId").Append(";");
					this.database.AddInParameter(sqlStringCommand, "SaleCounts" + num, System.Data.DbType.Int32, current.Quantity);
					this.database.AddInParameter(sqlStringCommand, "ProductId" + num, System.Data.DbType.Int32, current.ProductId);
					num++;
				}
				this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
				sqlStringCommand.CommandText = stringBuilder.ToString().Remove(stringBuilder.Length - 1);
				result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
			}
			return result;
		}
		public override StatisticsInfo GetStatistics()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Concat(new string[]
			{
				"SELECT ",
				string.Format(" (SELECT COUNT(OrderId) FROM distro_Orders WHERE OrderStatus={0} AND DistributorUserId={1}) AS orderNumbWaitConsignment,", 2, HiContext.Current.User.UserId),
				string.Format(" (SELECT COUNT(*) FROM distro_BalanceDrawRequest WHERE DistributorUserId={0} ) AS applyRequestWaitDispose,", HiContext.Current.User.UserId),
				string.Format(" (SELECT COUNT(PurchaseOrderId) FROM Hishop_PurchaseOrders WHERE PurchaseStatus={0} AND DistributorId={1}) AS purchaseOrderNumbWaitConsignment, ", 1, HiContext.Current.User.UserId),
				" 0 as productNumStokWarning,",
				string.Format("(SELECT Count(LeaveId) from distro_LeaveComments l where (SELECT count(replyId) from distro_LeaveCommentReplys where leaveId =l.leaveId)=0 and DistributorUserId={0} )  as leaveComments,", HiContext.Current.User.UserId),
				string.Format("(SELECT Count(ConsultationId) from distro_ProductConsultations where ReplyUserId is null AND DistributorUserId={0}) as productConsultations,", HiContext.Current.User.UserId),
				string.Format("(SELECT Count(*) from Hishop_DistributorMessageBox where IsRead=0 AND Accepter='{0}' AND Sernder <> 'admin') as messages,", HiContext.Current.User.Username),
				string.Format(" (SELECT count(orderId) from distro_orders where (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9) and  DistributorUserId={0} ", HiContext.Current.User.UserId),
				" and OrderDate>='",
				DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date),
				"') as orderNumbToday,",
				string.Format(" isnull((SELECT sum(OrderTotal)-isnull(sum(RefundAmount),0) from distro_orders where  (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9) AND DistributorUserId={0} ", HiContext.Current.User.UserId),
				" and OrderDate>='",
				DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date),
				"'),0) as orderPriceToday,",
				string.Format(" isnull((SELECT sum(orderProfit) from distro_orders where  (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9) AND DistributorUserId={0}  ", HiContext.Current.User.UserId),
				" and OrderDate>='",
				DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date),
				"'),0) as orderProfitToday,",
				string.Format(" (SELECT count(*) from vw_distro_Members where ParentUserId={0} and CreateDate>='" + DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date) + "' ) as userNewAddToday,", HiContext.Current.User.UserId),
				" 0 as agentNewAddToday, 0 as userNumbBirthdayToday ,",
				string.Format(" (SELECT count(orderId) from distro_orders where (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9) AND DistributorUserId={0} ", HiContext.Current.User.UserId),
				" and OrderDate>='",
				DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date.AddDays(-1.0)),
				"' and OrderDate<='",
				DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date),
				"') as orderNumbYesterday,",
				string.Format(" isnull((SELECT sum(orderTotal) from distro_orders where  (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9) AND DistributorUserId={0}  ", HiContext.Current.User.UserId),
				" and OrderDate>='",
				DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date.AddDays(-1.0)),
				"' and OrderDate<='",
				DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date),
				"'),0) as orderPriceYesterday,",
				string.Format(" isnull((SELECT sum(orderProfit) from distro_orders where  (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9) AND DistributorUserId={0}  ", HiContext.Current.User.UserId),
				" and OrderDate>='",
				DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date.AddDays(-1.0)),
				"' and OrderDate<='",
				DataHelper.GetSafeDateTimeFormat(DateTime.Now.Date),
				"'),0) as orderProfitYesterday,",
				string.Format(" (SELECT count(*) from vw_distro_Members where ParentUserId={0}) as userNumb,", HiContext.Current.User.UserId),
				" 0 as agentNumb,",
				string.Format(" isnull((SELECT sum(Balance) from vw_distro_Members WHERE ParentUserId={0}),0) as memberBalance,", HiContext.Current.User.UserId),
				" 0.00 as BalanceDrawRequested,",
				string.Format(" (SELECT COUNT(PurchaseOrderId) FROM Hishop_PurchaseOrders WHERE PurchaseStatus = 1 AND DistributorId={0}) AS purchaseOrderNumbWaitConsignment,", HiContext.Current.User.UserId),
				string.Format(" (SELECT count(productId) from distro_Products where SaleStatus={0} and DistributorUserId={1}) as productNumbOnSale,", 1, HiContext.Current.User.UserId),
				string.Format(" (SELECT count(productId) from distro_Products where SaleStatus={0} and DistributorUserId={1}) as productNumbInStock,", 1, HiContext.Current.User.UserId),
				string.Format(" (SELECT count(productId) from distro_Products where  DistributorUserId={0}) as authorizeProductCount,", HiContext.Current.User.UserId),
				string.Format(" (SELECT count(orderId) from distro_orders where (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9) AND DistributorUserId={0} ) as arealdyPaidNum,", HiContext.Current.User.UserId),
				string.Format(" (SELECT sum(OrderTotal) from distro_orders where (OrderStatus<>1 AND OrderStatus<>4 AND OrderStatus<>9) AND DistributorUserId={0} ) as arealdyPaidTotal,", HiContext.Current.User.UserId),
				string.Format(" (SELECT count(*) from distro_Products where DistributorUserId={0} and ProductId in (SELECT productId from Hishop_SKUs where Stock<=AlertStock group by productId)) as ProductAlert", HiContext.Current.User.UserId)
			}));
			StatisticsInfo result = new StatisticsInfo();
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateStatistics(dataReader);
				}
			}
			return result;
		}
		public override decimal GetDaySaleTotal(int year, int month, int int_0, SaleStatisticsType saleStatisticsType)
		{
			string text = this.BuiderSqlStringByType(saleStatisticsType);
			decimal result;
			if (text == null)
			{
				result = 0m;
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
				DateTime dateTime = new DateTime(year, month, int_0);
				DateTime dateTime2 = dateTime.AddDays(1.0);
				this.database.AddInParameter(sqlStringCommand, "@StartDate", System.Data.DbType.DateTime, dateTime);
				this.database.AddInParameter(sqlStringCommand, "@EndDate", System.Data.DbType.DateTime, dateTime2);
				object obj = this.database.ExecuteScalar(sqlStringCommand);
				decimal num = 0m;
				if (obj != null)
				{
					num = Convert.ToDecimal(obj);
				}
				result = num;
			}
			return result;
		}
		public override System.Data.DataTable GetDaySaleTotal(int year, int month, SaleStatisticsType saleStatisticsType)
		{
			string text = this.BuiderSqlStringByType(saleStatisticsType);
			System.Data.DataTable result;
			if (text == null)
			{
				result = null;
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
				this.database.AddInParameter(sqlStringCommand, "@StartDate", System.Data.DbType.DateTime);
				this.database.AddInParameter(sqlStringCommand, "@EndDate", System.Data.DbType.DateTime);
				System.Data.DataTable dataTable = this.CreateTable();
				decimal monthSaleTotal = this.GetMonthSaleTotal(year, month, saleStatisticsType);
				int dayCount = this.GetDayCount(year, month);
				int arg_9F_0;
				if (year == DateTime.Now.Year)
				{
					if (month == DateTime.Now.Month)
					{
						arg_9F_0 = DateTime.Now.Day;
						goto IL_9F;
					}
				}
				arg_9F_0 = dayCount;
				IL_9F:
				int num = arg_9F_0;
				for (int i = 1; i <= num; i++)
				{
					DateTime dateTime = new DateTime(year, month, i);
					DateTime dateTime2 = dateTime.AddDays(1.0);
					this.database.SetParameterValue(sqlStringCommand, "@StartDate", dateTime);
					this.database.SetParameterValue(sqlStringCommand, "@EndDate", dateTime2);
					object obj = this.database.ExecuteScalar(sqlStringCommand);
					decimal salesTotal = (obj == null) ? 0m : Convert.ToDecimal(obj);
					this.InsertToTable(dataTable, i, salesTotal, monthSaleTotal);
				}
				result = dataTable;
			}
			return result;
		}
		public override decimal GetMonthSaleTotal(int year, int month, SaleStatisticsType saleStatisticsType)
		{
			string text = this.BuiderSqlStringByType(saleStatisticsType);
			decimal result;
			if (text == null)
			{
				result = 0m;
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
				DateTime dateTime = new DateTime(year, month, 1);
				DateTime dateTime2 = dateTime.AddMonths(1);
				this.database.AddInParameter(sqlStringCommand, "@StartDate", System.Data.DbType.DateTime, dateTime);
				this.database.AddInParameter(sqlStringCommand, "@EndDate", System.Data.DbType.DateTime, dateTime2);
				object obj = this.database.ExecuteScalar(sqlStringCommand);
				decimal num = 0m;
				if (obj != null)
				{
					num = Convert.ToDecimal(obj);
				}
				result = num;
			}
			return result;
		}
		public override System.Data.DataTable GetMonthSaleTotal(int year, SaleStatisticsType saleStatisticsType)
		{
			string text = this.BuiderSqlStringByType(saleStatisticsType);
			System.Data.DataTable result;
			if (text == null)
			{
				result = null;
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
				this.database.AddInParameter(sqlStringCommand, "@StartDate", System.Data.DbType.DateTime);
				this.database.AddInParameter(sqlStringCommand, "@EndDate", System.Data.DbType.DateTime);
				System.Data.DataTable dataTable = this.CreateTable();
				int num = (year == DateTime.Now.Year) ? DateTime.Now.Month : 12;
				for (int i = 1; i <= num; i++)
				{
					DateTime dateTime = new DateTime(year, i, 1);
					DateTime dateTime2 = dateTime.AddMonths(1);
					this.database.SetParameterValue(sqlStringCommand, "@StartDate", dateTime);
					this.database.SetParameterValue(sqlStringCommand, "@EndDate", dateTime2);
					object obj = this.database.ExecuteScalar(sqlStringCommand);
					decimal salesTotal = (obj == null) ? 0m : Convert.ToDecimal(obj);
					decimal yearSaleTotal = this.GetYearSaleTotal(year, saleStatisticsType);
					this.InsertToTable(dataTable, i, salesTotal, yearSaleTotal);
				}
				result = dataTable;
			}
			return result;
		}
		public override decimal GetYearSaleTotal(int year, SaleStatisticsType saleStatisticsType)
		{
			string text = this.BuiderSqlStringByType(saleStatisticsType);
			decimal result;
			if (text == null)
			{
				result = 0m;
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
				DateTime dateTime = new DateTime(year, 1, 1);
				DateTime dateTime2 = dateTime.AddYears(1);
				this.database.AddInParameter(sqlStringCommand, "@StartDate", System.Data.DbType.DateTime, dateTime);
				this.database.AddInParameter(sqlStringCommand, "@EndDate", System.Data.DbType.DateTime, dateTime2);
				object obj = this.database.ExecuteScalar(sqlStringCommand);
				decimal num = 0m;
				if (obj != null)
				{
					num = Convert.ToDecimal(obj);
				}
				result = num;
			}
			return result;
		}
		public override OrderStatisticsInfo GetUserOrders(UserOrderQuery userOrder)
		{
			OrderStatisticsInfo orderStatisticsInfo = new OrderStatisticsInfo();
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("sub_OrderStatistics_Get");
			this.database.AddInParameter(storedProcCommand, "PageIndex", System.Data.DbType.Int32, userOrder.PageIndex);
			this.database.AddInParameter(storedProcCommand, "PageSize", System.Data.DbType.Int32, userOrder.PageSize);
			this.database.AddInParameter(storedProcCommand, "IsCount", System.Data.DbType.Boolean, userOrder.IsCount);
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", System.Data.DbType.String, SalesData.BuildUserOrderQuery(userOrder));
			this.database.AddOutParameter(storedProcCommand, "TotalUserOrders", System.Data.DbType.Int32, 4);
			this.database.AddInParameter(storedProcCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
			{
				orderStatisticsInfo.OrderTbl = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (dataReader.NextResult())
				{
					dataReader.Read();
					if (dataReader["OrderTotal"] != DBNull.Value)
					{
						orderStatisticsInfo.TotalOfPage += (decimal)dataReader["OrderTotal"];
					}
					if (dataReader["Profits"] != DBNull.Value)
					{
						orderStatisticsInfo.ProfitsOfPage += (decimal)dataReader["Profits"];
					}
				}
				if (dataReader.NextResult())
				{
					dataReader.Read();
					if (dataReader["OrderTotal"] != DBNull.Value)
					{
						orderStatisticsInfo.TotalOfSearch += (decimal)dataReader["OrderTotal"];
					}
					if (dataReader["Profits"] != DBNull.Value)
					{
						orderStatisticsInfo.ProfitsOfSearch += (decimal)dataReader["Profits"];
					}
				}
			}
			orderStatisticsInfo.TotalCount = (int)this.database.GetParameterValue(storedProcCommand, "TotaluserOrders");
			return orderStatisticsInfo;
		}
		public override OrderStatisticsInfo GetUserOrdersNoPage(UserOrderQuery userOrder)
		{
			OrderStatisticsInfo orderStatisticsInfo = new OrderStatisticsInfo();
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("sub_OrderStatisticsNoPage_Get");
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", System.Data.DbType.String, SalesData.BuildUserOrderQuery(userOrder));
			this.database.AddOutParameter(storedProcCommand, "TotalUserOrders", System.Data.DbType.Int32, 4);
			this.database.AddInParameter(storedProcCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
			{
				orderStatisticsInfo.OrderTbl = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (dataReader.NextResult())
				{
					dataReader.Read();
					if (dataReader["OrderTotal"] != DBNull.Value)
					{
						orderStatisticsInfo.TotalOfSearch += (decimal)dataReader["OrderTotal"];
					}
					if (dataReader["Profits"] != DBNull.Value)
					{
						orderStatisticsInfo.ProfitsOfSearch += (decimal)dataReader["Profits"];
					}
				}
			}
			orderStatisticsInfo.TotalCount = (int)this.database.GetParameterValue(storedProcCommand, "TotaluserOrders");
			return orderStatisticsInfo;
		}
		public override DbQueryResult GetSaleOrderLineItemsStatistics(SaleStatisticsQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("DistributorUserId = {0}", HiContext.Current.User.UserId);
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND orderDate >= '{0}'", query.StartDate.Value);
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND orderDate <= '{0}'", query.EndDate.Value);
			}
			if (stringBuilder.Length > 0)
			{
				stringBuilder.Append(" AND ");
			}
			stringBuilder.AppendFormat("OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2}", 1, 4, 9);
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_distro_SaleDetails", "OrderId", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*");
		}
		public override DbQueryResult GetSaleTargets()
		{
			DbQueryResult dbQueryResult = new DbQueryResult();
			string text = string.Empty;
			text = string.Format(string.Concat(new string[]
			{
				"SELECT ",
				string.Format(" (SELECT Count(OrderId) from distro_orders where DistributorUserId={0} AND OrderStatus != {1} AND OrderStatus != {2} AND OrderStatus != {3}) as OrderNumb ,", new object[]
				{
					HiContext.Current.User.UserId,
					1,
					4,
					9
				}),
				string.Format("ISNULL((SELECT sum(OrderTotal) from distro_orders where DistributorUserId={0} AND OrderStatus != {1} AND OrderStatus != {2} AND OrderStatus != {3}),0) as OrderPrice, ", new object[]
				{
					HiContext.Current.User.UserId,
					1,
					4,
					9
				}),
				string.Format(" (SELECT COUNT(*) from vw_distro_Members where ParentUserId={0}) as UserNumb, ", HiContext.Current.User.UserId),
				string.Format(" (SELECT count(*) from vw_distro_Members where UserID in (SELECT userid from distro_orders where DistributorUserId={0})) as UserOrderedNumb, ", HiContext.Current.User.UserId),
				string.Format(" ISNULL((SELECT sum(VistiCounts) from distro_products where DistributorUserId={0}),0) as ProductVisitNumb ", HiContext.Current.User.UserId)
			}), new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return dbQueryResult;
		}
		public override System.Data.DataTable GetProductSales(SaleStatisticsQuery productSale, out int totalProductSales)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("sub_ProductSales_Get");
			this.database.AddInParameter(storedProcCommand, "PageIndex", System.Data.DbType.Int32, productSale.PageIndex);
			this.database.AddInParameter(storedProcCommand, "PageSize", System.Data.DbType.Int32, productSale.PageSize);
			this.database.AddInParameter(storedProcCommand, "IsCount", System.Data.DbType.Boolean, productSale.IsCount);
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", System.Data.DbType.String, SalesData.BuildProductSaleQuery(productSale));
			this.database.AddInParameter(storedProcCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddOutParameter(storedProcCommand, "TotalProductSales", System.Data.DbType.Int32, 4);
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			totalProductSales = (int)this.database.GetParameterValue(storedProcCommand, "TotalProductSales");
			return result;
		}
		public override System.Data.DataTable GetProductSalesNoPage(SaleStatisticsQuery productSale, out int totalProductSales)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("sub_ProductSalesNoPage_Get");
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", System.Data.DbType.String, SalesData.BuildProductSaleQuery(productSale));
			this.database.AddInParameter(storedProcCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddOutParameter(storedProcCommand, "TotalProductSales", System.Data.DbType.Int32, 4);
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			totalProductSales = (int)this.database.GetParameterValue(storedProcCommand, "TotalProductSales");
			return result;
		}
		public override System.Data.DataTable GetProductVisitAndBuyStatistics(SaleStatisticsQuery query, out int totalProductSales)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("sub_ProductVisitAndBuyStatistics_Get");
			this.database.AddInParameter(storedProcCommand, "PageIndex", System.Data.DbType.Int32, query.PageIndex);
			this.database.AddInParameter(storedProcCommand, "PageSize", System.Data.DbType.Int32, query.PageSize);
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", System.Data.DbType.String, SalesData.BuildProductVisitAndBuyStatisticsQuery(query));
			this.database.AddInParameter(storedProcCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddOutParameter(storedProcCommand, "TotalProductSales", System.Data.DbType.Int32, 4);
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			totalProductSales = (int)this.database.GetParameterValue(storedProcCommand, "TotalProductSales");
			return result;
		}
		public override System.Data.DataTable GetProductVisitAndBuyStatisticsNoPage(SaleStatisticsQuery query, out int totalProductSales)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT ProductName,VistiCounts,SaleCounts as BuyCount ,(SaleCounts/(case when VistiCounts=0 then 1 else VistiCounts end))*100 as BuyPercentage ");
			stringBuilder.AppendFormat("FROM distro_Products WHERE SaleCounts>0 and DistributorUserId={0} ORDER BY BuyPercentage DESC;", HiContext.Current.User.UserId);
			stringBuilder.AppendFormat("SELECT COUNT(*) as TotalProductSales FROM distro_Products WHERE SaleCounts>0 and DistributorUserId={0};", HiContext.Current.User.UserId);
			sqlStringCommand.CommandText = stringBuilder.ToString();
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					result = DataHelper.ConverDataReaderToDataTable(dataReader);
				}
				if (dataReader.NextResult() && dataReader.Read())
				{
					totalProductSales = (int)dataReader["TotalProductSales"];
				}
				else
				{
					totalProductSales = 0;
				}
			}
			return result;
		}
		public override IList<UserStatisticsInfo> GetUserStatistics(Pagination page, out int totalRegionsUsers)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT TopRegionId as RegionId,COUNT(UserId) as UserCounts,(SELECT count(*) from distro_Members where ParentUserId={0}) as AllUserCounts FROM distro_Members where ParentUserId={1}  GROUP BY TopRegionId ", HiContext.Current.User.UserId, HiContext.Current.User.UserId));
			IList<UserStatisticsInfo> list = new List<UserStatisticsInfo>();
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				UserStatisticsInfo userStatisticsInfo = null;
				while (dataReader.Read())
				{
					userStatisticsInfo = DataMapper.PopulateUserStatistics(dataReader);
					list.Add(userStatisticsInfo);
				}
				if (userStatisticsInfo != null)
				{
					totalRegionsUsers = int.Parse(userStatisticsInfo.AllUserCounts.ToString());
				}
				else
				{
					totalRegionsUsers = 0;
				}
			}
			return list;
		}
		private static string BuildProductVisitAndBuyStatisticsQuery(SaleStatisticsQuery query)
		{
			if (null == query)
			{
				throw new ArgumentNullException("query");
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT ProductId,(SaleCounts*100/(case when VistiCounts=0 then 1 else VistiCounts end)) as BuyPercentage");
			stringBuilder.AppendFormat(" FROM distro_products where SaleCounts>0 and DistributorUserId={0}", HiContext.Current.User.UserId);
			if (!string.IsNullOrEmpty(query.SortBy))
			{
				stringBuilder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
			}
			return stringBuilder.ToString();
		}
		private static string BuildProductSaleQuery(SaleStatisticsQuery query)
		{
			if (null == query)
			{
				throw new ArgumentNullException("query");
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT ProductId, SUM(o.Quantity) AS ProductSaleCounts, SUM(o.ItemAdjustedPrice * o.Quantity) AS ProductSaleTotals,");
			stringBuilder.Append("  (SUM(o.ItemAdjustedPrice * o.Quantity) - SUM(o.CostPrice * o.ShipmentQuantity) )AS ProductProfitsTotals ");
			stringBuilder.AppendFormat(" FROM distro_OrderItems o  WHERE 0=0 and DistributorUserId={0}", HiContext.Current.User.UserId);
			stringBuilder.AppendFormat(" AND OrderId IN (SELECT  OrderId FROM distro_Orders WHERE OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2} and  DistributorUserId={3})", new object[]
			{
				4,
				1,
				9,
				HiContext.Current.User.UserId
			});
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM distro_Orders WHERE OrderDate >= '{0}' and  DistributorUserId={1})", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value), HiContext.Current.User.UserId);
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND OrderId IN (SELECT OrderId FROM distro_Orders WHERE OrderDate <= '{0}' and  DistributorUserId={1})", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value), HiContext.Current.User.UserId);
			}
			stringBuilder.Append(" GROUP BY ProductId HAVING ProductId IN");
			stringBuilder.AppendFormat(" (SELECT ProductId FROM distro_Products where DistributorUserId={0})", HiContext.Current.User.UserId);
			if (!string.IsNullOrEmpty(query.SortBy))
			{
				stringBuilder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
			}
			return stringBuilder.ToString();
		}
		private string BuiderSqlStringByType(SaleStatisticsType saleStatisticsType)
		{
			string str = string.Empty;
			switch (saleStatisticsType)
			{
			case SaleStatisticsType.SaleCounts:
				str = "SELECT COUNT(OrderId)";
				break;
			case SaleStatisticsType.SaleTotal:
				str = "SELECT Isnull(SUM(OrderTotal),0)";
				break;
			case SaleStatisticsType.Profits:
				str = "SELECT IsNull(SUM(OrderProfit),0)";
				break;
			}
			return str + string.Format(" FROM distro_Orders WHERE (OrderDate BETWEEN @StartDate AND @EndDate) AND OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2} AND DistributorUserId={3}", new object[]
			{
				1,
				4,
				9,
				HiContext.Current.User.UserId
			});
		}
		private static string BuildUserOrderQuery(UserOrderQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT OrderId FROM distro_Orders WHERE OrderStatus != {0} AND OrderStatus != {1} AND OrderStatus != {2} AND DistributorUserId = {3}", new object[]
			{
				1,
				4,
				9,
				HiContext.Current.User.UserId
			});
			string result;
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				stringBuilder.AppendFormat(" AND OrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId));
				result = stringBuilder.ToString();
			}
			else
			{
				if (!string.IsNullOrEmpty(query.UserName))
				{
					stringBuilder.AppendFormat(" AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName));
				}
				if (!string.IsNullOrEmpty(query.ShipTo))
				{
					stringBuilder.AppendFormat(" AND ShipTo LIKE '%{0}%'", DataHelper.CleanSearchString(query.ShipTo));
				}
				if (query.StartDate.HasValue)
				{
					stringBuilder.AppendFormat(" AND  OrderDate >= '{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
				}
				if (query.EndDate.HasValue)
				{
					stringBuilder.AppendFormat(" AND  OrderDate <= '{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
				}
				if (!string.IsNullOrEmpty(query.SortBy))
				{
					stringBuilder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
				}
				result = stringBuilder.ToString();
			}
			return result;
		}
		private System.Data.DataTable CreateTable()
		{
			System.Data.DataTable dataTable = new System.Data.DataTable();
			System.Data.DataColumn column = new System.Data.DataColumn("Date", typeof(int));
			System.Data.DataColumn column2 = new System.Data.DataColumn("SaleTotal", typeof(decimal));
			System.Data.DataColumn column3 = new System.Data.DataColumn("Percentage", typeof(decimal));
			System.Data.DataColumn column4 = new System.Data.DataColumn("Lenth", typeof(decimal));
			dataTable.Columns.Add(column);
			dataTable.Columns.Add(column2);
			dataTable.Columns.Add(column3);
			dataTable.Columns.Add(column4);
			return dataTable;
		}
		private int GetDayCount(int year, int month)
		{
			int result;
			if (month == 2)
			{
				if ((year % 4 == 0 && year % 100 != 0) || year % 400 == 0)
				{
					result = 29;
				}
				else
				{
					result = 28;
				}
			}
			else
			{
				if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12)
				{
					result = 31;
				}
				else
				{
					result = 30;
				}
			}
			return result;
		}
		private void InsertToTable(System.Data.DataTable table, int date, decimal salesTotal, decimal allSalesTotal)
		{
			System.Data.DataRow dataRow = table.NewRow();
			dataRow["Date"] = date;
			dataRow["SaleTotal"] = salesTotal;
			if (allSalesTotal != 0m)
			{
				dataRow["Percentage"] = salesTotal / allSalesTotal * 100m;
			}
			else
			{
				dataRow["Percentage"] = 0;
			}
			dataRow["Lenth"] = (decimal)dataRow["Percentage"] * 4m;
			table.Rows.Add(dataRow);
		}
		public override bool CheckRefund(string orderId, string adminRemark, int refundType, bool accept)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE distro_Orders SET OrderStatus = @OrderStatus WHERE OrderId = @OrderId and DistributorUserId=@DistributorUserId;");
			stringBuilder.Append(" update distro_OrderRefund set AdminRemark=@AdminRemark,HandleStatus=@HandleStatus,HandleTime=@HandleTime where HandleStatus=0 and OrderId = @OrderId and DistributorUserId=@DistributorUserId;");
			if (refundType == 1 && accept)
			{
				OrderInfo orderInfo = SubsiteSalesHelper.GetOrderInfo(orderId);
				if (orderInfo != null)
				{
					Member member = Users.GetUser(orderInfo.UserId, false) as Member;
					decimal num = orderInfo.GetTotal();
					decimal num2 = member.Balance + num;
					if (orderInfo.GroupBuyStatus != GroupBuyStatus.Failed)
					{
						num -= orderInfo.NeedPrice;
						num2 -= orderInfo.NeedPrice;
					}
					stringBuilder.Append("insert into distro_BalanceDetails(UserId,UserName,TradeDate,TradeType,Income,Balance,Remark,DistributorUserId)");
					stringBuilder.AppendFormat("values({0},'{1}',{2},{3},{4},{5},'{6}',{7})", new object[]
					{
						member.UserId,
						member.Username,
						"getdate()",
						5,
						num,
						num2,
						"订单" + orderId + "退款",
						HiContext.Current.User.UserId
					});
				}
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			if (accept)
			{
				this.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, 9);
				this.database.AddInParameter(sqlStringCommand, "HandleStatus", System.Data.DbType.Int32, 1);
			}
			else
			{
				this.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, 2);
				this.database.AddInParameter(sqlStringCommand, "HandleStatus", System.Data.DbType.Int32, 2);
			}
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.String, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "AdminRemark", System.Data.DbType.String, adminRemark);
			this.database.AddInParameter(sqlStringCommand, "HandleTime", System.Data.DbType.DateTime, DateTime.Now);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override void GetRefundType(string orderId, out int refundType, out string refundRemark)
		{
			refundType = 0;
			refundRemark = "";
			string text = string.Concat(new object[]
			{
				"select RefundType,RefundRemark from distro_OrderRefund where HandleStatus=0 and OrderId='",
				orderId,
				"' and DistributorUserId='",
				HiContext.Current.User.UserId,
				"'"
			});
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand);
			if (dataReader.Read())
			{
				refundType = (int)dataReader["RefundType"];
				refundRemark = (string)dataReader["RefundRemark"];
			}
		}
		public override bool CheckReturn(string orderId, decimal refundMoney, string adminRemark, int refundType, bool accept)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE distro_Orders SET OrderStatus = @OrderStatus WHERE OrderId = @OrderId and DistributorUserId=@DistributorUserId;");
			stringBuilder.Append(" update distro_OrderReturns set RefundMoney=@RefundMoney,AdminRemark=@AdminRemark,HandleStatus=@HandleStatus,HandleTime=@HandleTime where HandleStatus=0 and OrderId = @OrderId and DistributorUserId=@DistributorUserId;");
			if (refundType == 1 && accept)
			{
				stringBuilder.Append(" insert into distro_BalanceDetails(UserId,UserName,TradeDate,TradeType,Income");
				stringBuilder.AppendFormat(",Balance,Remark,DistributorUserId) select UserId,Username,getdate() as TradeDate,{0} as TradeType,", 7);
				stringBuilder.Append("@RefundMoney as Income,isnull((select top 1 Balance from distro_BalanceDetails b");
				stringBuilder.Append(" where b.UserId=a.UserId and b.DistributorUserId=a.DistributorUserId ");
				stringBuilder.AppendFormat("order by JournalNumber desc),0)+@RefundMoney as Balance,'订单{0}退货' as Remark,DistributorUserId", orderId);
				stringBuilder.Append(" from distro_Orders a where OrderId = @OrderId and DistributorUserId=@DistributorUserId;");
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			if (accept)
			{
				this.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, 10);
				this.database.AddInParameter(sqlStringCommand, "HandleStatus", System.Data.DbType.Int32, 1);
			}
			else
			{
				this.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, 3);
				this.database.AddInParameter(sqlStringCommand, "HandleStatus", System.Data.DbType.Int32, 2);
			}
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.String, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "RefundMoney", System.Data.DbType.Decimal, refundMoney);
			this.database.AddInParameter(sqlStringCommand, "AdminRemark", System.Data.DbType.String, adminRemark);
			this.database.AddInParameter(sqlStringCommand, "HandleTime", System.Data.DbType.DateTime, DateTime.Now);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override void GetRefundTypeFromReturn(string orderId, out int refundType, out string refundRemark)
		{
			refundType = 0;
			refundRemark = "";
			string text = string.Concat(new object[]
			{
				"select RefundType,Comments from distro_OrderReturns where HandleStatus=0 and OrderId='",
				orderId,
				"' and DistributorUserId='",
				HiContext.Current.User.UserId,
				"'"
			});
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand);
			if (dataReader.Read())
			{
				refundType = (int)dataReader["RefundType"];
				refundRemark = (string)dataReader["Comments"];
			}
		}
		public override DbQueryResult GetRefundApplys(RefundApplyQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" DistributorUserId={0}", HiContext.Current.User.UserId);
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				stringBuilder.AppendFormat(" AND OrderId = '{0}'", query.OrderId);
			}
			if (query.HandleStatus.HasValue)
			{
				stringBuilder.AppendFormat(" AND HandleStatus = {0}", query.HandleStatus);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_distro_OrderRefund", "RefundId", stringBuilder.ToString(), "*");
		}
		public override bool DelRefundApply(int refundId)
		{
			string text = string.Format("DELETE FROM distro_OrderRefund WHERE RefundId={0} and HandleStatus >0 ", refundId);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override DbQueryResult GetReturnsApplys(ReturnsApplyQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" DistributorUserId={0}", HiContext.Current.User.UserId);
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				stringBuilder.AppendFormat(" and OrderId = '{0}'", query.OrderId);
			}
			if (query.HandleStatus.HasValue)
			{
				stringBuilder.AppendFormat(" AND HandleStatus = {0}", query.HandleStatus);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_distro_OrderReturns", "ReturnsId", stringBuilder.ToString(), "*");
		}
		public override bool DelReturnsApply(int returnsId)
		{
			string text = string.Format("DELETE FROM distro_OrderReturns WHERE ReturnsId={0} and HandleStatus >0 ", returnsId);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool CheckReplace(string orderId, string adminRemark, bool accept)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE distro_Orders SET OrderStatus = @OrderStatus WHERE OrderId = @OrderId and DistributorUserId=@DistributorUserId;");
			stringBuilder.Append(" update distro_OrderReplace set AdminRemark=@AdminRemark,HandleStatus=@HandleStatus,HandleTime=@HandleTime where HandleStatus=0 and OrderId = @OrderId and DistributorUserId=@DistributorUserId;");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			if (accept)
			{
				this.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, 3);
				this.database.AddInParameter(sqlStringCommand, "HandleStatus", System.Data.DbType.Int32, 1);
			}
			else
			{
				this.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, 3);
				this.database.AddInParameter(sqlStringCommand, "HandleStatus", System.Data.DbType.Int32, 2);
			}
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.String, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "AdminRemark", System.Data.DbType.String, adminRemark);
			this.database.AddInParameter(sqlStringCommand, "HandleTime", System.Data.DbType.DateTime, DateTime.Now);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override string GetReplaceComments(string orderId)
		{
			string text = string.Concat(new object[]
			{
				"select Comments from distro_OrderReplace where HandleStatus=0 and OrderId='",
				orderId,
				"' and DistributorUserId=",
				HiContext.Current.User.UserId
			});
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			string result;
			if (obj == null || obj is DBNull)
			{
				result = "";
			}
			else
			{
				result = obj.ToString();
			}
			return result;
		}
		public override DbQueryResult GetReplaceApplys(ReplaceApplyQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" DistributorUserId={0}", HiContext.Current.User.UserId);
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				stringBuilder.AppendFormat(" and OrderId = '{0}'", query.OrderId);
			}
			if (query.HandleStatus.HasValue)
			{
				stringBuilder.AppendFormat(" AND HandleStatus = {0}", query.HandleStatus);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_distro_OrderReplace", "ReplaceId", stringBuilder.ToString(), "*");
		}
		public override bool DelReplaceApply(int replaceId)
		{
			string text = string.Format("DELETE FROM distro_OrderReplace WHERE ReplaceId={0} and HandleStatus >0 ", replaceId);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool SaveDebitNote(DebitNote note)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" insert into distro_OrderDebitNote(DistributorUserId,NoteId,OrderId,Operator,Remark) values(@DistributorUserId,@NoteId,@OrderId,@Operator,@Remark)");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "NoteId", System.Data.DbType.String, note.NoteId);
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, note.OrderId);
			this.database.AddInParameter(sqlStringCommand, "Operator", System.Data.DbType.String, note.Operator);
			this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, note.Remark);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool ApplyForPurchaseRefund(string purchaseOrderId, string remark, int refundType)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE Hishop_PurchaseOrders SET PurchaseStatus = @OrderStatus WHERE PurchaseOrderId = @PurchaseOrderId;");
			stringBuilder.Append(" insert into Hishop_PurchaseOrderRefund(PurchaseOrderId,ApplyForTime,RefundRemark,HandleStatus,RefundType) values(@PurchaseOrderId,@ApplyForTime,@RefundRemark,0,@RefundType);");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, 6);
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrderId);
			this.database.AddInParameter(sqlStringCommand, "ApplyForTime", System.Data.DbType.DateTime, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "RefundRemark", System.Data.DbType.String, remark);
			this.database.AddInParameter(sqlStringCommand, "RefundType", System.Data.DbType.Int32, refundType);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool CanPurchaseRefund(string purchaseOrderId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select PurchaseOrderId,HandleStatus from Hishop_PurchaseOrderRefund where PurchaseOrderId = @PurchaseOrderId");
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrderId);
			bool result = false;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					int num = Convert.ToInt32(dataReader["HandleStatus"]);
					if (num == 2)
					{
						result = true;
					}
				}
				else
				{
					result = true;
				}
			}
			return result;
		}
		public override bool ApplyForPurchaseReturn(string purchaseOrderId, string remark, int refundType)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE Hishop_PurchaseOrders SET PurchaseStatus = @OrderStatus WHERE PurchaseOrderId = @PurchaseOrderId;");
			stringBuilder.Append(" insert into Hishop_PurchaseOrderReturns(PurchaseOrderId,ApplyForTime,Comments,HandleStatus,RefundType,RefundMoney) values(@PurchaseOrderId,@ApplyForTime,@Comments,0,@RefundType,0);");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, 7);
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrderId);
			this.database.AddInParameter(sqlStringCommand, "ApplyForTime", System.Data.DbType.DateTime, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "Comments", System.Data.DbType.String, remark);
			this.database.AddInParameter(sqlStringCommand, "RefundType", System.Data.DbType.Int32, refundType);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool CanPurchaseReturn(string purchaseOrderId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select PurchaseOrderId,HandleStatus from Hishop_PurchaseOrderReturns where PurchaseOrderId = @PurchaseOrderId");
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrderId);
			bool result = false;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					int num = Convert.ToInt32(dataReader["HandleStatus"]);
					if (num == 2)
					{
						result = true;
					}
				}
				else
				{
					result = true;
				}
			}
			return result;
		}
		public override bool ApplyForPurchaseReplace(string purchaseOrderId, string remark)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE Hishop_PurchaseOrders SET PurchaseStatus = @OrderStatus WHERE PurchaseOrderId = @PurchaseOrderId;");
			stringBuilder.Append(" insert into Hishop_PurchaseOrderReplace(PurchaseOrderId,ApplyForTime,Comments,HandleStatus) values(@PurchaseOrderId,@ApplyForTime,@Comments,0);");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, 8);
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrderId);
			this.database.AddInParameter(sqlStringCommand, "ApplyForTime", System.Data.DbType.DateTime, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "Comments", System.Data.DbType.String, remark);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool CanPurchaseReplace(string purchaseOrderId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select PurchaseOrderId,HandleStatus from Hishop_PurchaseOrderReplace where PurchaseOrderId = @PurchaseOrderId AND HandleStatus = 0");
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrderId);
			return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) < 1;
		}
		public override bool SavePurchaseDebitNote(PurchaseDebitNote note)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" insert into Hishop_PurchaseDebitNote(NoteId,PurchaseOrderId,Operator,Remark) values(@NoteId,@PurchaseOrderId,@Operator,@Remark)");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "NoteId", System.Data.DbType.String, note.NoteId);
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, note.PurchaseOrderId);
			this.database.AddInParameter(sqlStringCommand, "Operator", System.Data.DbType.String, note.Operator);
			this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, note.Remark);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override DbQueryResult GetAllDebitNote(DebitNoteQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("DistributorUserId='{0}'", HiContext.Current.User.UserId);
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				stringBuilder.AppendFormat(" and OrderId='{0}'", query.OrderId);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_distro_OrderDebitNote", "NoteId", stringBuilder.ToString(), "*");
		}
		public override bool DelDebitNote(string noteId)
		{
			string text = string.Format("DELETE FROM distro_OrderDebitNote WHERE NoteId='{0}'", noteId);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
	}
}
