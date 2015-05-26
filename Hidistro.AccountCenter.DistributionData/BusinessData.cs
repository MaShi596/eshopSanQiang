using Hidistro.AccountCenter.Business;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
namespace Hidistro.AccountCenter.DistributionData
{
	public class BusinessData : TradeSubsiteProvider
	{
		private Database database;
		public BusinessData()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public override DbQueryResult GetUserPoints(int pageIndex)
		{
			return DataHelper.PagingByRownumber(pageIndex, 10, "JournalNumber", SortAction.Desc, true, "distro_PointDetails", "JournalNumber", string.Format("UserId={0}", HiContext.Current.User.UserId), "*");
		}
		public override System.Data.DataTable GetUserCoupons(int userId)
		{
			System.Data.DataTable result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT c.*, ci.ClaimCode,ci.CouponStatus FROM distro_CouponItems ci INNER JOIN distro_Coupons c ON c.CouponId = ci.CouponId WHERE ci.UserId = @UserId AND c.ClosingTime > @ClosingTime");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			this.database.AddInParameter(sqlStringCommand, "ClosingTime", System.Data.DbType.DateTime, DateTime.Now);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override System.Data.DataTable GetChangeCoupons()
		{
			System.Data.DataTable result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_Coupons WHERE NeedPoint > 0 AND ClosingTime > @ClosingTime AND DistributorUserId = @DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "ClosingTime", System.Data.DbType.DateTime, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override bool SendClaimCodes(CouponItemInfo couponItem)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO distro_CouponItems(CouponId, ClaimCode, GenerateTime, UserId, UserName, EmailAddress,LotNumber) VALUES(@CouponId, @ClaimCode, @GenerateTime, @UserId, @UserName, @EmailAddress,@LotNumber)");
			this.database.AddInParameter(sqlStringCommand, "CouponId", System.Data.DbType.Int32, couponItem.CouponId);
			this.database.AddInParameter(sqlStringCommand, "ClaimCode", System.Data.DbType.String, couponItem.ClaimCode);
			this.database.AddInParameter(sqlStringCommand, "GenerateTime", System.Data.DbType.DateTime, couponItem.GenerateTime);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32);
			this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, couponItem.UserName);
			this.database.AddInParameter(sqlStringCommand, "LotNumber", System.Data.DbType.Guid, Guid.NewGuid());
			if (couponItem.UserId.HasValue)
			{
				this.database.SetParameterValue(sqlStringCommand, "UserId", couponItem.UserId.Value);
			}
			else
			{
				this.database.SetParameterValue(sqlStringCommand, "UserId", DBNull.Value);
			}
			this.database.AddInParameter(sqlStringCommand, "EmailAddress", System.Data.DbType.String, couponItem.EmailAddress);
			return this.database.ExecuteNonQuery(sqlStringCommand) >= 1;
		}
		public override bool ExitCouponClaimCode(string claimCode)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(ClaimCode) FROM distro_CouponItems WHERE ClaimCode = @ClaimCode AND ISNULL(UserId, 0) = 0");
			this.database.AddInParameter(sqlStringCommand, "ClaimCode", System.Data.DbType.String, claimCode);
			return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
		}
		public override int AddClaimCodeToUser(string claimCode, int userId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_CouponItems SET UserId = @UserId, UserName=@UserName WHERE ClaimCode = @ClaimCode");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, HiContext.Current.User.Username);
			this.database.AddInParameter(sqlStringCommand, "ClaimCode", System.Data.DbType.String, claimCode);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override OrderInfo GetOrderInfo(string orderId)
		{
			OrderInfo orderInfo = null;
			string text = "SELECT *,null as Tax,null as InvoiceTitle FROM distro_Orders WHERE OrderId = @OrderId;";
			text += "SELECT * FROM distro_OrderGifts WHERE OrderId = @OrderId;";
			text += "SELECT * FROM distro_OrderItems WHERE OrderId = @OrderId;";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					orderInfo = DataMapper.PopulateOrder(dataReader);
				}
				dataReader.NextResult();
				while (dataReader.Read())
				{
					orderInfo.Gifts.Add(DataMapper.PopulateOrderGift(dataReader));
				}
				dataReader.NextResult();
				while (dataReader.Read())
				{
					orderInfo.LineItems.Add((string)dataReader["SkuId"], DataMapper.PopulateLineItem(dataReader));
				}
			}
			return orderInfo;
		}
		public override DbQueryResult GetUserOrder(int userId, OrderQuery query)
		{
			if (string.IsNullOrEmpty(query.SortBy))
			{
				query.SortBy = "OrderDate";
			}
			return DataHelper.PagingByTopsort(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, true, "distro_Orders", null, BusinessData.BuildMemberOrdersQuery(userId, query), "*");
		}
		public override bool CloseOrder(string orderId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Orders SET OrderStatus = @OrderStatus WHERE OrderId = @OrderId");
			this.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, 4);
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool UpdateOrderPaymentType(OrderInfo order)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Orders SET PaymentTypeId=@PaymentTypeId,PaymentType=@PaymentType,Gateway=@Gateway WHERE OrderId = @OrderId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "PaymentTypeId", System.Data.DbType.Int32, order.PaymentTypeId);
			this.database.AddInParameter(sqlStringCommand, "PaymentType", System.Data.DbType.String, order.PaymentType);
			this.database.AddInParameter(sqlStringCommand, "Gateway", System.Data.DbType.String, order.Gateway);
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, order.OrderId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override void UpdateStockPayOrder(string orderId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_SKUs Set Stock = CASE WHEN (Stock - (SELECT SUM(oi.ShipmentQuantity) FROM distro_OrderItems oi Where oi.SkuId =Hishop_SKUs.SkuId AND OrderId =@OrderId))<=0 Then 0 ELSE Stock - (SELECT SUM(oi.ShipmentQuantity) FROM distro_OrderItems oi  Where oi.SkuId =Hishop_SKUs.SkuId AND OrderId =@OrderId) END WHERE Hishop_SKUs.SkuId  IN (Select SkuId FROM distro_OrderItems Where OrderId =@OrderId)");
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override IList<PaymentModeInfo> GetPaymentModes()
		{
			IList<PaymentModeInfo> list = new List<PaymentModeInfo>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_PaymentTypes WHERE DistributorUserId=@DistributorUserId Order by DisplaySequence desc");
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulatePayment(dataReader));
				}
			}
			return list;
		}
		public override PaymentModeInfo GetPaymentMode(int modeId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_PaymentTypes WHERE ModeId = @ModeId;");
			this.database.AddInParameter(sqlStringCommand, "ModeId", System.Data.DbType.Int32, modeId);
			PaymentModeInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulatePayment(dataReader);
				}
			}
			return result;
		}
		public override bool UserPayOrder(OrderInfo order, bool isBalancePayOrder, System.Data.Common.DbTransaction dbTran)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("UPDATE distro_Orders SET OrderStatus = {0}, PayDate = '{1}', GatewayOrderId = @GatewayOrderId WHERE OrderId = '{2}';", 2, DateTime.Now, order.OrderId);
			bool result;
			if (isBalancePayOrder)
			{
				Member member = Users.GetUser(order.UserId, false) as Member;
				decimal num = member.Balance - order.GetTotal();
				if (member.Balance - member.RequestBalance < order.GetTotal())
				{
					result = false;
					return result;
				}
				stringBuilder.AppendFormat("INSERT INTO distro_BalanceDetails(UserId, UserName,DistributorUserId, TradeDate, TradeType, Expenses, Balance, Remark) VALUES({0},'{1}', {2}, '{3}', {4}, {5}, {6}, '{7}');", new object[]
				{
					order.UserId,
					HiContext.Current.User.Username,
					HiContext.Current.SiteSettings.UserId.Value,
					DateTime.Now,
					3,
					order.GetTotal(),
					num,
					string.Format("对订单{0}付款", order.OrderId)
				});
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "GatewayOrderId", System.Data.DbType.String, order.GatewayOrderId);
			result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) >= 1);
			return result;
		}
		public override bool ConfirmOrderFinish(OrderInfo order)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Orders SET OrderStatus=@OrderStatus,FinishDate=@FinishDate WHERE OrderId = @OrderId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, order.OrderId);
			this.database.AddInParameter(sqlStringCommand, "FinishDate", System.Data.DbType.DateTime, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, 5);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool ApplyForRefund(string orderId, string remark, int refundType)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE distro_Orders SET OrderStatus = @OrderStatus WHERE OrderId = @OrderId");
			stringBuilder.Append(" AND DistributorUserId=@DistributorUserId;");
			stringBuilder.Append(" insert into distro_OrderRefund(OrderId,DistributorUserId,ApplyForTime,RefundRemark,HandleStatus,RefundType) values(@OrderId,@DistributorUserId,@ApplyForTime,@RefundRemark,0,@RefundType);");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, 6);
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
			this.database.AddInParameter(sqlStringCommand, "ApplyForTime", System.Data.DbType.DateTime, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "RefundRemark", System.Data.DbType.String, remark);
			this.database.AddInParameter(sqlStringCommand, "RefundType", System.Data.DbType.Int32, refundType);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool CanRefund(string orderId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select OrderId,HandleStatus from distro_OrderRefund where OrderId = @OrderId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
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
		public override bool ApplyForReturn(string orderId, string remark, int refundType)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE distro_Orders SET OrderStatus = @OrderStatus WHERE OrderId = @OrderId");
			stringBuilder.Append(" AND DistributorUserId=@DistributorUserId;");
			stringBuilder.Append(" insert into distro_OrderReturns(OrderId,DistributorUserId,ApplyForTime,Comments,HandleStatus,RefundType,RefundMoney) values(@OrderId,@DistributorUserId,@ApplyForTime,@Comments,0,@RefundType,0);");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, 7);
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
			this.database.AddInParameter(sqlStringCommand, "ApplyForTime", System.Data.DbType.DateTime, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "Comments", System.Data.DbType.String, remark);
			this.database.AddInParameter(sqlStringCommand, "RefundType", System.Data.DbType.Int32, refundType);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool CanReturn(string orderId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select OrderId,HandleStatus from distro_OrderReturns where OrderId = @OrderId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
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
		public override bool ApplyForReplace(string orderId, string remark)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("UPDATE distro_Orders SET OrderStatus = @OrderStatus WHERE OrderId = @OrderId");
			stringBuilder.Append(" AND DistributorUserId=@DistributorUserId;");
			stringBuilder.Append(" insert into distro_OrderReplace(OrderId,DistributorUserId,ApplyForTime,Comments,HandleStatus) values(@OrderId,@DistributorUserId,@ApplyForTime,@Comments,0);");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, 8);
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
			this.database.AddInParameter(sqlStringCommand, "ApplyForTime", System.Data.DbType.DateTime, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "Comments", System.Data.DbType.String, remark);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool CanReplace(string orderId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select OrderId,HandleStatus from distro_OrderReplace where OrderId = @OrderId AND HandleStatus = 0 AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
			return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) < 1;
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
			this.database.AddInParameter(sqlStringCommand, "ParentUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
			this.database.AddInParameter(sqlStringCommand, "OrderPrice", System.Data.DbType.Decimal, orderTotal);
			this.database.AddInParameter(sqlStringCommand, "Points", System.Data.DbType.Int32, totalPoint);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool ChangeMemberGrade(int userId, int gradId, int points)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ISNULL(Points, 0) AS Point, GradeId FROM distro_MemberGrades WHERE CreateUserId=@CreateUserId Order by Point Desc ");
			this.database.AddInParameter(sqlStringCommand, "CreateUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
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
			this.database.AddInParameter(sqlStringCommand, "ParentUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
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
					stringBuilder.Append("UPDATE distro_Products SET SaleCounts = SaleCounts + @SaleCounts").Append(num).Append(", ShowSaleCounts = ShowSaleCounts +  @SaleCounts").Append(num).Append(" WHERE ProductId=@ProductId").Append(num).Append(" AND DistributorUserId=@DistributorUserId").Append(";");
					this.database.AddInParameter(sqlStringCommand, "SaleCounts" + num, System.Data.DbType.Int32, current.Quantity);
					this.database.AddInParameter(sqlStringCommand, "ProductId" + num, System.Data.DbType.Int32, current.ProductId);
					num++;
				}
				this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
				sqlStringCommand.CommandText = stringBuilder.ToString().Remove(stringBuilder.Length - 1);
				result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
			}
			return result;
		}
		public override GroupBuyInfo GetGroupBuy(int groupBuyId)
		{
			GroupBuyInfo groupBuyInfo = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_GroupBuy WHERE GroupBuyId=@GroupBuyId AND DistributorUserId=@DistributorUserId;SELECT * FROM distro_GroupBuyCondition WHERE GroupBuyId=@GroupBuyId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "GroupBuyId", System.Data.DbType.Int32, groupBuyId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					groupBuyInfo = DataMapper.PopulateGroupBuy(dataReader);
				}
				dataReader.NextResult();
				while (dataReader.Read())
				{
					GropBuyConditionInfo gropBuyConditionInfo = new GropBuyConditionInfo();
					gropBuyConditionInfo.Count = (int)dataReader["Count"];
					gropBuyConditionInfo.Price = (decimal)dataReader["Price"];
					groupBuyInfo.GroupBuyConditions.Add(gropBuyConditionInfo);
				}
			}
			return groupBuyInfo;
		}
		public override CountDownInfo CountDownBuy(int CountDownId)
		{
			CountDownInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_CountDown WHERE CountDownId=@CountDownId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "CountDownId", System.Data.DbType.Int32, CountDownId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateCountDown(dataReader);
				}
			}
			return result;
		}
		public override int GetOrderCount(int groupBuyId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SUM(Quantity) FROM distro_OrderItems WHERE OrderId IN (SELECT OrderId FROM distro_Orders WHERE GroupBuyId = @GroupBuyId AND OrderStatus <> 1 AND OrderStatus <> 4 AND DistributorUserId=@DistributorUserId)");
			this.database.AddInParameter(sqlStringCommand, "GroupBuyId", System.Data.DbType.Int32, groupBuyId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
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
		public override bool SetGroupBuyEndUntreated(int groupBuyId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_GroupBuy SET Status=@Status,EndDate=@EndDate WHERE GroupBuyId=@GroupBuyId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "GroupBuyId", System.Data.DbType.Int32, groupBuyId);
			this.database.AddInParameter(sqlStringCommand, "Status", System.Data.DbType.Int32, 2);
			this.database.AddInParameter(sqlStringCommand, "EndDate", System.Data.DbType.DateTime, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		private static string BuildMemberOrdersQuery(int userId, OrderQuery query)
		{
			if (null == query)
			{
				throw new ArgumentNullException("query");
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" UserId = {0}", userId);
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				stringBuilder.AppendFormat(" AND OrderId = '{0}'", DataHelper.CleanSearchString(query.OrderId).ToLower());
			}
			if (!string.IsNullOrEmpty(query.ShipId))
			{
				stringBuilder.AppendFormat(" AND ShipOrderNumber = '{0}'", DataHelper.CleanSearchString(query.ShipId));
			}
			if (!string.IsNullOrEmpty(query.ShipTo))
			{
				stringBuilder.AppendFormat(" AND ShipTo LIKE '%{0}%'", DataHelper.CleanSearchString(query.ShipTo));
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
				stringBuilder.AppendFormat(" AND OrderDate > '{0}'", query.StartDate);
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND OrderDate < '{0}'", query.EndDate);
			}
			return stringBuilder.ToString();
		}
		public override DbQueryResult GetRefundApplys(RefundApplyQuery query, int userId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" UserId={0}", userId);
			stringBuilder.AppendFormat(" and DistributorUserId={0}", HiContext.Current.SiteSettings.UserId.Value);
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				stringBuilder.AppendFormat(" and OrderId = '{0}'", query.OrderId);
			}
			if (query.HandleStatus.HasValue)
			{
				stringBuilder.AppendFormat(" AND HandleStatus = {0}", query.HandleStatus);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_distro_OrderRefund", "RefundId", stringBuilder.ToString(), "*");
		}
		public override DbQueryResult GetReturnsApplys(ReturnsApplyQuery query, int userId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" UserId={0}", userId);
			stringBuilder.AppendFormat(" and DistributorUserId={0}", HiContext.Current.SiteSettings.UserId.Value);
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
		public override decimal GetRefundMoney(OrderInfo order, out decimal refundMoney)
		{
			string text = string.Format("SELECT RefundMoney FROM dbo.distro_OrderReturns WHERE HandleStatus=1 AND OrderId={0}", order.OrderId);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			decimal num = Convert.ToDecimal(this.database.ExecuteScalar(sqlStringCommand));
			return refundMoney = num;
		}
		public override DbQueryResult GetReplaceApplys(ReplaceApplyQuery query, int userId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" UserId={0}", userId);
			stringBuilder.AppendFormat(" and DistributorUserId={0}", HiContext.Current.SiteSettings.UserId.Value);
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
		public override bool SaveDebitNote(DebitNote note)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("insert into distro_OrderDebitNote(DistributorUserId,NoteId,OrderId,Operator,Remark) values(@DistributorUserId,@NoteId,@OrderId,@Operator,@Remark)");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId);
			this.database.AddInParameter(sqlStringCommand, "NoteId", System.Data.DbType.String, note.NoteId);
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, note.OrderId);
			this.database.AddInParameter(sqlStringCommand, "Operator", System.Data.DbType.String, note.Operator);
			this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, note.Remark);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
	}
}
