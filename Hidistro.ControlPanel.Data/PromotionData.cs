using Hidistro.ControlPanel.Promotions;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core.Enums;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
namespace Hidistro.ControlPanel.Data
{
	public class PromotionData : PromotionsProvider
	{
		private Database database;
		public PromotionData()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public override GiftActionStatus CreateUpdateDeleteGift(GiftInfo gift, DataProviderAction action)
		{
			GiftActionStatus result;
			if (null == gift)
			{
				result = GiftActionStatus.UnknowError;
			}
			else
			{
				System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Gift_CreateUpdateDelete");
				this.database.AddInParameter(storedProcCommand, "Action", System.Data.DbType.Int32, (int)action);
				this.database.AddOutParameter(storedProcCommand, "Status", System.Data.DbType.Int32, 4);
				if (DataProviderAction.Create != action)
				{
					this.database.AddInParameter(storedProcCommand, "GiftId", System.Data.DbType.Int32, gift.GiftId);
				}
				else
				{
					this.database.AddOutParameter(storedProcCommand, "GiftId", System.Data.DbType.Int32, 4);
				}
				if (DataProviderAction.Delete != action)
				{
					this.database.AddInParameter(storedProcCommand, "Name", System.Data.DbType.String, gift.Name);
					this.database.AddInParameter(storedProcCommand, "ShortDescription", System.Data.DbType.String, gift.ShortDescription);
					this.database.AddInParameter(storedProcCommand, "Unit", System.Data.DbType.String, gift.Unit);
					this.database.AddInParameter(storedProcCommand, "LongDescription", System.Data.DbType.String, gift.LongDescription);
					this.database.AddInParameter(storedProcCommand, "Title", System.Data.DbType.String, gift.Title);
					this.database.AddInParameter(storedProcCommand, "Meta_Description", System.Data.DbType.String, gift.Meta_Description);
					this.database.AddInParameter(storedProcCommand, "Meta_Keywords", System.Data.DbType.String, gift.Meta_Keywords);
					this.database.AddInParameter(storedProcCommand, "CostPrice", System.Data.DbType.Currency, gift.CostPrice);
					this.database.AddInParameter(storedProcCommand, "ImageUrl", System.Data.DbType.String, gift.ImageUrl);
					this.database.AddInParameter(storedProcCommand, "ThumbnailUrl40", System.Data.DbType.String, gift.ThumbnailUrl40);
					this.database.AddInParameter(storedProcCommand, "ThumbnailUrl60", System.Data.DbType.String, gift.ThumbnailUrl60);
					this.database.AddInParameter(storedProcCommand, "ThumbnailUrl100", System.Data.DbType.String, gift.ThumbnailUrl100);
					this.database.AddInParameter(storedProcCommand, "ThumbnailUrl160", System.Data.DbType.String, gift.ThumbnailUrl160);
					this.database.AddInParameter(storedProcCommand, "ThumbnailUrl180", System.Data.DbType.String, gift.ThumbnailUrl180);
					this.database.AddInParameter(storedProcCommand, "ThumbnailUrl220", System.Data.DbType.String, gift.ThumbnailUrl220);
					this.database.AddInParameter(storedProcCommand, "ThumbnailUrl310", System.Data.DbType.String, gift.ThumbnailUrl310);
					this.database.AddInParameter(storedProcCommand, "ThumbnailUrl410", System.Data.DbType.String, gift.ThumbnailUrl410);
					this.database.AddInParameter(storedProcCommand, "PurchasePrice", System.Data.DbType.Currency, gift.PurchasePrice);
					this.database.AddInParameter(storedProcCommand, "MarketPrice", System.Data.DbType.Currency, gift.MarketPrice);
					this.database.AddInParameter(storedProcCommand, "NeedPoint", System.Data.DbType.Int32, gift.NeedPoint);
					this.database.AddInParameter(storedProcCommand, "IsDownLoad", System.Data.DbType.Boolean, gift.IsDownLoad);
					this.database.AddInParameter(storedProcCommand, "IsPromotion", System.Data.DbType.Boolean, gift.IsPromotion);
				}
				else
				{
					this.database.AddInParameter(storedProcCommand, "IsPromotion", System.Data.DbType.Boolean, false);
				}
				this.database.ExecuteNonQuery(storedProcCommand);
				GiftActionStatus giftActionStatus = (GiftActionStatus)((int)this.database.GetParameterValue(storedProcCommand, "Status"));
				result = giftActionStatus;
			}
			return result;
		}
		public override DbQueryResult GetGifts(GiftQuery query)
		{
			string text = string.Format("[Name] LIKE '%{0}%'", DataHelper.CleanSearchString(query.Name));
			if (query.IsPromotion)
			{
				text += " AND IsPromotion = 1";
			}
			Pagination page = query.Page;
			return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Hishop_Gifts", "GiftId", text, "*");
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
		public override bool UpdateIsDownLoad(int giftId, bool isdownload)
		{
			string text = "update Hishop_Gifts set IsDownLoad=@IsDownLoad  where GiftId = @GiftId;";
			if (!isdownload)
			{
				text += "delete from distro_Gifts where GiftId=@GiftId;";
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			this.database.AddInParameter(sqlStringCommand, "GiftId", System.Data.DbType.Int32, giftId);
			this.database.AddInParameter(sqlStringCommand, "IsDownLoad", System.Data.DbType.Boolean, isdownload);
			bool result;
			try
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
			}
			catch
			{
				result = false;
			}
			return result;
		}
		public override CouponActionStatus CreateCoupon(CouponInfo coupon, int count, out string lotNumber)
		{
			CouponActionStatus couponActionStatus = CouponActionStatus.UnknowError;
			lotNumber = string.Empty;
			CouponActionStatus result;
			if (count <= 0)
			{
				lotNumber = string.Empty;
				if (null == coupon)
				{
					couponActionStatus = CouponActionStatus.UnknowError;
					result = CouponActionStatus.UnknowError;
					return result;
				}
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT CouponId  FROM Hishop_Coupons WHERE Name=@Name");
				this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, coupon.Name);
				if (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) >= 1)
				{
					couponActionStatus = CouponActionStatus.DuplicateName;
					result = CouponActionStatus.DuplicateName;
					return result;
				}
				sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_Coupons ([Name],  ClosingTime,StartTime, Description, Amount, DiscountValue,SentCount,UsedCount,NeedPoint) VALUES(@Name, @ClosingTime,@StartTime, @Description, @Amount, @DiscountValue,0,0,@NeedPoint); SELECT @@IDENTITY");
				this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, coupon.Name);
				this.database.AddInParameter(sqlStringCommand, "ClosingTime", System.Data.DbType.DateTime, coupon.ClosingTime);
				this.database.AddInParameter(sqlStringCommand, "StartTime", System.Data.DbType.DateTime, coupon.StartTime);
				this.database.AddInParameter(sqlStringCommand, "Description", System.Data.DbType.String, coupon.Description);
				this.database.AddInParameter(sqlStringCommand, "DiscountValue", System.Data.DbType.Currency, coupon.DiscountValue);
				this.database.AddInParameter(sqlStringCommand, "NeedPoint", System.Data.DbType.Int32, coupon.NeedPoint);
				if (coupon.Amount.HasValue)
				{
					this.database.AddInParameter(sqlStringCommand, "Amount", System.Data.DbType.Currency, coupon.Amount.Value);
				}
				else
				{
					this.database.AddInParameter(sqlStringCommand, "Amount", System.Data.DbType.Currency, DBNull.Value);
				}
				object obj = this.database.ExecuteScalar(sqlStringCommand);
				if (obj != null && obj != DBNull.Value)
				{
					couponActionStatus = CouponActionStatus.CreateClaimCodeSuccess;
				}
			}
			else
			{
				couponActionStatus = CouponActionStatus.CreateClaimCodeSuccess;
				System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_ClaimCode_Create");
				this.database.AddInParameter(storedProcCommand, "CouponId", System.Data.DbType.Int32, coupon.CouponId);
				this.database.AddInParameter(storedProcCommand, "row", System.Data.DbType.Int32, count);
				this.database.AddInParameter(storedProcCommand, "UserId", System.Data.DbType.Int32, null);
				this.database.AddInParameter(storedProcCommand, "UserName", System.Data.DbType.String, null);
				this.database.AddInParameter(storedProcCommand, "EmailAddress", System.Data.DbType.String, null);
				this.database.AddOutParameter(storedProcCommand, "ReturnLotNumber", System.Data.DbType.String, 300);
				try
				{
					this.database.ExecuteNonQuery(storedProcCommand);
					lotNumber = (string)this.database.GetParameterValue(storedProcCommand, "ReturnLotNumber");
				}
				catch
				{
					couponActionStatus = CouponActionStatus.CreateClaimCodeError;
				}
			}
			result = couponActionStatus;
			return result;
		}
		public override IList<CouponItemInfo> GetCouponItemInfos(string lotNumber)
		{
			IList<CouponItemInfo> list = new List<CouponItemInfo>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_CouponItems WHERE convert(nvarchar(300),LotNumber)=@LotNumber");
			this.database.AddInParameter(sqlStringCommand, "LotNumber", System.Data.DbType.String, lotNumber);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					CouponItemInfo item = DataMapper.PopulateCouponItem(dataReader);
					list.Add(item);
				}
			}
			return list;
		}
		public override CouponActionStatus UpdateCoupon(CouponInfo coupon)
		{
			CouponActionStatus result;
			if (null == coupon)
			{
				result = CouponActionStatus.UnknowError;
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT CouponId  FROM Hishop_Coupons WHERE Name=@Name AND CouponId<>@CouponId ");
				this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, coupon.Name);
				this.database.AddInParameter(sqlStringCommand, "CouponId", System.Data.DbType.Int32, coupon.CouponId);
				if (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) >= 1)
				{
					result = CouponActionStatus.DuplicateName;
				}
				else
				{
					sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Coupons SET [Name]=@Name, ClosingTime=@ClosingTime,StartTime=@StartTime, Description=@Description, Amount=@Amount, DiscountValue=@DiscountValue, NeedPoint = @NeedPoint WHERE CouponId=@CouponId");
					this.database.AddInParameter(sqlStringCommand, "CouponId", System.Data.DbType.String, coupon.CouponId);
					this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, coupon.Name);
					this.database.AddInParameter(sqlStringCommand, "ClosingTime", System.Data.DbType.DateTime, coupon.ClosingTime);
					this.database.AddInParameter(sqlStringCommand, "StartTime", System.Data.DbType.DateTime, coupon.StartTime);
					this.database.AddInParameter(sqlStringCommand, "Description", System.Data.DbType.String, coupon.Description);
					this.database.AddInParameter(sqlStringCommand, "DiscountValue", System.Data.DbType.Currency, coupon.DiscountValue);
					this.database.AddInParameter(sqlStringCommand, "NeedPoint", System.Data.DbType.Int32, coupon.NeedPoint);
					if (coupon.Amount.HasValue)
					{
						this.database.AddInParameter(sqlStringCommand, "Amount", System.Data.DbType.Currency, coupon.Amount.Value);
					}
					else
					{
						this.database.AddInParameter(sqlStringCommand, "Amount", System.Data.DbType.Currency, DBNull.Value);
					}
					if (this.database.ExecuteNonQuery(sqlStringCommand) == 1)
					{
						result = CouponActionStatus.Success;
					}
					else
					{
						result = CouponActionStatus.UnknowError;
					}
				}
			}
			return result;
		}
		public override bool DeleteCoupon(int couponId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Coupons WHERE CouponId = @CouponId");
			this.database.AddInParameter(sqlStringCommand, "CouponId", System.Data.DbType.Int32, couponId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override CouponInfo GetCouponDetails(int couponId)
		{
			CouponInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Coupons WHERE CouponId = @CouponId");
			this.database.AddInParameter(sqlStringCommand, "CouponId", System.Data.DbType.Int32, couponId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateCoupon(dataReader);
				}
			}
			return result;
		}
		public override DbQueryResult GetNewCoupons(Pagination page)
		{
			return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Hishop_Coupons", "CouponId", string.Empty, "*");
		}
		public override IList<Member> GetMembersByRank(int? gradeId)
		{
			IList<Member> list = new List<Member>();
			System.Data.Common.DbCommand sqlStringCommand;
			if (gradeId > 0)
			{
				sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vw_aspnet_Members WHERE GradeId=@GradeId");
				this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, gradeId);
			}
			else
			{
				sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vw_aspnet_Members");
			}
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(new Member(UserRole.Member)
					{
						UserId = (int)dataReader["UserId"],
						Email = dataReader["Email"].ToString(),
						Username = dataReader["UserName"].ToString()
					});
				}
			}
			return list;
		}
		public override DbQueryResult GetCouponsList(CouponItemInfoQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (query.CouponId.HasValue)
			{
				stringBuilder.AppendFormat("CouponId = {0}", query.CouponId.Value);
			}
			if (!string.IsNullOrEmpty(query.CounponName))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("Name = '{0}'", query.CounponName);
			}
			if (!string.IsNullOrEmpty(query.UserName))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("UserName='{0}'", DataHelper.CleanSearchString(query.UserName));
			}
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("Orderid='{0}'", DataHelper.CleanSearchString(query.OrderId));
			}
			if (query.CouponStatus.HasValue)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat(" CouponStatus={0} ", query.CouponStatus);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_CouponInfo", "ClaimCode", stringBuilder.ToString(), "*");
		}
		public override bool SendClaimCodes(int couponId, CouponItemInfo couponItem)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_CouponItems(CouponId, ClaimCode,LotNumber, GenerateTime, UserId,UserName,EmailAddress,CouponStatus) VALUES(@CouponId, @ClaimCode,@LotNumber, @GenerateTime, @UserId, @UserName,@EmailAddress,@CouponStatus)");
			this.database.AddInParameter(sqlStringCommand, "CouponId", System.Data.DbType.Int32, couponId);
			this.database.AddInParameter(sqlStringCommand, "ClaimCode", System.Data.DbType.String, couponItem.ClaimCode);
			this.database.AddInParameter(sqlStringCommand, "GenerateTime", System.Data.DbType.DateTime, couponItem.GenerateTime);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32);
			this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String);
			this.database.AddInParameter(sqlStringCommand, "LotNumber", System.Data.DbType.Guid, Guid.NewGuid());
			if (couponItem.UserId.HasValue)
			{
				this.database.SetParameterValue(sqlStringCommand, "UserId", couponItem.UserId.Value);
			}
			else
			{
				this.database.SetParameterValue(sqlStringCommand, "UserId", DBNull.Value);
			}
			if (!string.IsNullOrEmpty(couponItem.UserName))
			{
				this.database.SetParameterValue(sqlStringCommand, "UserName", couponItem.UserName);
			}
			else
			{
				this.database.SetParameterValue(sqlStringCommand, "UserName", DBNull.Value);
			}
			this.database.AddInParameter(sqlStringCommand, "EmailAddress", System.Data.DbType.String, couponItem.EmailAddress);
			this.database.AddInParameter(sqlStringCommand, "CouponStatus", System.Data.DbType.String, 0);
			return this.database.ExecuteNonQuery(sqlStringCommand) >= 1;
		}
		public override List<int> GetSendIds(int? gradeId, string userName)
		{
			List<int> list = new List<int>();
			string text = string.Format("SELECT UserId FROM vw_aspnet_Members WHERE UserName Like '%{0}%' ", DataHelper.CleanSearchString(userName));
			if (gradeId.HasValue)
			{
				string str = string.Format(" AND GradeId={0}", gradeId);
				text += str;
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					int item = Convert.ToInt32(dataReader[0]);
					list.Add(item);
				}
			}
			return list;
		}
		public override System.Data.DataTable GetPromotions(bool isProductPromote, bool isWholesale)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Promotions");
			if (isProductPromote)
			{
				if (isWholesale)
				{
					System.Data.Common.DbCommand expr_1E = sqlStringCommand;
					expr_1E.CommandText += string.Format(" WHERE PromoteType = {0}", 4);
				}
				else
				{
					System.Data.Common.DbCommand expr_41 = sqlStringCommand;
					expr_41.CommandText += string.Format(" WHERE PromoteType <> {0} AND PromoteType < 10", 4);
				}
			}
			else
			{
				if (isWholesale)
				{
					System.Data.Common.DbCommand expr_6A = sqlStringCommand;
					expr_6A.CommandText += string.Format(" WHERE PromoteType = {0} OR PromoteType = {1}", 13, 14);
				}
				else
				{
					System.Data.Common.DbCommand expr_95 = sqlStringCommand;
					expr_95.CommandText += string.Format(" WHERE PromoteType <> {0} AND PromoteType <> {1} AND PromoteType > 10", 13, 14);
				}
			}
			System.Data.Common.DbCommand expr_BE = sqlStringCommand;
			expr_BE.CommandText += " ORDER BY ActivityId DESC";
			System.Data.DataTable result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override PromotionInfo GetPromotion(int activityId)
		{
			PromotionInfo promotionInfo = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Promotions WHERE ActivityId = @ActivityId; SELECT GradeId FROM Hishop_PromotionMemberGrades WHERE ActivityId = @ActivityId");
			this.database.AddInParameter(sqlStringCommand, "ActivityId", System.Data.DbType.Int32, activityId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					promotionInfo = DataMapper.PopulatePromote(dataReader);
				}
				dataReader.NextResult();
				while (dataReader.Read())
				{
					promotionInfo.MemberGradeIds.Add((int)dataReader["GradeId"]);
				}
			}
			return promotionInfo;
		}
		public override IList<MemberGradeInfo> GetPromoteMemberGrades(int activityId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_MemberGrades WHERE GradeId IN (SELECT GradeId FROM Hishop_PromotionMemberGrades WHERE ActivityId = @ActivityId)");
			this.database.AddInParameter(sqlStringCommand, "ActivityId", System.Data.DbType.Int32, activityId);
			IList<MemberGradeInfo> list = new List<MemberGradeInfo>();
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulateMemberGrade(dataReader));
				}
			}
			return list;
		}
		public override System.Data.DataTable GetPromotionProducts(int activityId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vw_Hishop_BrowseProductList WHERE ProductId IN (SELECT ProductId FROM Hishop_PromotionProducts WHERE ActivityId = @ActivityId) ORDER BY DisplaySequence");
			this.database.AddInParameter(sqlStringCommand, "ActivityId", System.Data.DbType.Int32, activityId);
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override bool AddPromotionProducts(int activityId, string productIds)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("INSERT INTO Hishop_PromotionProducts SELECT @ActivityId, ProductId FROM Hishop_Products WHERE ProductId IN ({0})", productIds) + " AND ProductId NOT IN (SELECT ProductId FROM Hishop_PromotionProducts)");
			this.database.AddInParameter(sqlStringCommand, "ActivityId", System.Data.DbType.Int32, activityId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool DeletePromotionProducts(int activityId, int? productId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_PromotionProducts WHERE ActivityId = @ActivityId");
			if (productId.HasValue)
			{
				System.Data.Common.DbCommand expr_1E = sqlStringCommand;
				expr_1E.CommandText += string.Format(" AND ProductId = {0}", productId.Value);
			}
			this.database.AddInParameter(sqlStringCommand, "ActivityId", System.Data.DbType.Int32, activityId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override int AddPromotion(PromotionInfo promotion, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_Promotions(Name, PromoteType, Condition, DiscountValue, StartDate, EndDate, Description) VALUES(@Name, @PromoteType, @Condition, @DiscountValue, @StartDate, @EndDate, @Description); SELECT @@IDENTITY");
			this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, promotion.Name);
			this.database.AddInParameter(sqlStringCommand, "PromoteType", System.Data.DbType.Int32, (int)promotion.PromoteType);
			this.database.AddInParameter(sqlStringCommand, "Condition", System.Data.DbType.Currency, promotion.Condition);
			this.database.AddInParameter(sqlStringCommand, "DiscountValue", System.Data.DbType.Currency, promotion.DiscountValue);
			this.database.AddInParameter(sqlStringCommand, "StartDate", System.Data.DbType.DateTime, promotion.StartDate);
			this.database.AddInParameter(sqlStringCommand, "EndDate", System.Data.DbType.DateTime, promotion.EndDate);
			this.database.AddInParameter(sqlStringCommand, "Description", System.Data.DbType.String, promotion.Description);
			object obj;
			if (dbTran != null)
			{
				obj = this.database.ExecuteScalar(sqlStringCommand, dbTran);
			}
			else
			{
				obj = this.database.ExecuteScalar(sqlStringCommand);
			}
			int result;
			if (obj != null)
			{
				result = Convert.ToInt32(obj);
			}
			else
			{
				result = 0;
			}
			return result;
		}
		public override bool AddPromotionMemberGrades(int activityId, IList<int> memberGrades, System.Data.Common.DbTransaction dbTran)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("DELETE FROM Hishop_PromotionMemberGrades WHERE ActivityId = {0}", activityId);
			foreach (int current in memberGrades)
			{
				stringBuilder.AppendFormat(" INSERT INTO Hishop_PromotionMemberGrades (ActivityId, GradeId) VALUES ({0}, {1})", activityId, current);
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			bool result;
			if (dbTran != null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
			}
			return result;
		}
		public override bool EditPromotion(PromotionInfo promotion, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Promotions SET Name = @Name, PromoteType = @PromoteType, Condition = @Condition, DiscountValue = @DiscountValue, StartDate = @StartDate, EndDate = @EndDate, Description = @Description WHERE ActivityId = @ActivityId");
			this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, promotion.Name);
			this.database.AddInParameter(sqlStringCommand, "PromoteType", System.Data.DbType.Int32, (int)promotion.PromoteType);
			this.database.AddInParameter(sqlStringCommand, "Condition", System.Data.DbType.Currency, promotion.Condition);
			this.database.AddInParameter(sqlStringCommand, "DiscountValue", System.Data.DbType.Currency, promotion.DiscountValue);
			this.database.AddInParameter(sqlStringCommand, "StartDate", System.Data.DbType.DateTime, promotion.StartDate);
			this.database.AddInParameter(sqlStringCommand, "EndDate", System.Data.DbType.DateTime, promotion.EndDate);
			this.database.AddInParameter(sqlStringCommand, "Description", System.Data.DbType.String, promotion.Description);
			this.database.AddInParameter(sqlStringCommand, "ActivityId", System.Data.DbType.Int32, promotion.ActivityId);
			bool result;
			if (dbTran != null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
			}
			return result;
		}
		public override bool DeletePromotion(int activityId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Promotions WHERE ActivityId = @ActivityId");
			this.database.AddInParameter(sqlStringCommand, "ActivityId", System.Data.DbType.Int32, activityId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override string GetPriceByProductId(int productId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SalePrice FROM vw_Hishop_BrowseProductList WHERE ProductId=@ProductId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			return this.database.ExecuteScalar(sqlStringCommand).ToString();
		}
		public override int AddGroupBuy(GroupBuyInfo groupBuy, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_GroupBuy;INSERT INTO Hishop_GroupBuy(ProductId,NeedPrice,StartDate,EndDate,MaxCount,Content,Status,DisplaySequence) VALUES(@ProductId,@NeedPrice,@StartDate,@EndDate,@MaxCount,@Content,@Status,@DisplaySequence); SELECT @@IDENTITY");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, groupBuy.ProductId);
			this.database.AddInParameter(sqlStringCommand, "NeedPrice", System.Data.DbType.Currency, groupBuy.NeedPrice);
			this.database.AddInParameter(sqlStringCommand, "StartDate", System.Data.DbType.DateTime, groupBuy.StartDate);
			this.database.AddInParameter(sqlStringCommand, "EndDate", System.Data.DbType.DateTime, groupBuy.EndDate);
			this.database.AddInParameter(sqlStringCommand, "MaxCount", System.Data.DbType.Int32, groupBuy.MaxCount);
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, groupBuy.Content);
			this.database.AddInParameter(sqlStringCommand, "Status", System.Data.DbType.Int32, 1);
			object obj;
			if (dbTran != null)
			{
				obj = this.database.ExecuteScalar(sqlStringCommand, dbTran);
			}
			else
			{
				obj = this.database.ExecuteScalar(sqlStringCommand);
			}
			int result;
			if (obj != null)
			{
				result = Convert.ToInt32(obj);
			}
			else
			{
				result = 0;
			}
			return result;
		}
		public override bool ProductGroupBuyExist(int productId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_GroupBuy WHERE ProductId=@ProductId AND Status=@Status");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			this.database.AddInParameter(sqlStringCommand, "Status", System.Data.DbType.Int32, 1);
			return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
		}
		public override bool AddGroupBuyCondition(int groupBuyId, IList<GropBuyConditionInfo> gropBuyConditions, System.Data.Common.DbTransaction dbTran)
		{
			bool result;
			if (gropBuyConditions.Count <= 0)
			{
				result = false;
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_GroupBuyCondition(GroupBuyId,Count,Price) VALUES(@GroupBuyId,@Count,@Price)");
				this.database.AddInParameter(sqlStringCommand, "GroupBuyId", System.Data.DbType.Int32, groupBuyId);
				this.database.AddInParameter(sqlStringCommand, "Count", System.Data.DbType.Int32);
				this.database.AddInParameter(sqlStringCommand, "Price", System.Data.DbType.Currency);
				try
				{
					foreach (GropBuyConditionInfo current in gropBuyConditions)
					{
						this.database.SetParameterValue(sqlStringCommand, "Count", current.Count);
						this.database.SetParameterValue(sqlStringCommand, "Price", current.Price);
						if (dbTran != null)
						{
							this.database.ExecuteNonQuery(sqlStringCommand, dbTran);
						}
						else
						{
							this.database.ExecuteNonQuery(sqlStringCommand);
						}
					}
					result = true;
				}
				catch
				{
					result = false;
				}
			}
			return result;
		}
		public override bool DeleteGroupBuy(int groupBuyId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_GroupBuy WHERE GroupBuyId=@GroupBuyId");
			this.database.AddInParameter(sqlStringCommand, "GroupBuyId", System.Data.DbType.Int32, groupBuyId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool DeleteGroupBuyCondition(int groupBuyId, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_GroupBuyCondition WHERE GroupBuyId=@GroupBuyId");
			this.database.AddInParameter(sqlStringCommand, "GroupBuyId", System.Data.DbType.Int32, groupBuyId);
			bool result;
			if (dbTran != null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
			}
			return result;
		}
		public override bool UpdateGroupBuy(GroupBuyInfo groupBuy, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_GroupBuy SET ProductId=@ProductId,NeedPrice=@NeedPrice,StartDate=@StartDate,EndDate=@EndDate,MaxCount=@MaxCount,Content=@Content WHERE GroupBuyId=@GroupBuyId");
			this.database.AddInParameter(sqlStringCommand, "GroupBuyId", System.Data.DbType.Int32, groupBuy.GroupBuyId);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, groupBuy.ProductId);
			this.database.AddInParameter(sqlStringCommand, "NeedPrice", System.Data.DbType.Currency, groupBuy.NeedPrice);
			this.database.AddInParameter(sqlStringCommand, "StartDate", System.Data.DbType.DateTime, groupBuy.StartDate);
			this.database.AddInParameter(sqlStringCommand, "EndDate", System.Data.DbType.DateTime, groupBuy.EndDate);
			this.database.AddInParameter(sqlStringCommand, "MaxCount", System.Data.DbType.Int32, groupBuy.MaxCount);
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, groupBuy.Content);
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
		public override GroupBuyInfo GetGroupBuy(int groupBuyId)
		{
			GroupBuyInfo groupBuyInfo = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_GroupBuy WHERE GroupBuyId=@GroupBuyId;SELECT * FROM Hishop_GroupBuyCondition WHERE GroupBuyId=@GroupBuyId");
			this.database.AddInParameter(sqlStringCommand, "GroupBuyId", System.Data.DbType.Int32, groupBuyId);
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
		public override DbQueryResult GetGroupBuyList(GroupBuyQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1");
			if (!string.IsNullOrEmpty(query.ProductName))
			{
				stringBuilder.AppendFormat(" AND ProductName Like '%{0}%'", DataHelper.CleanSearchString(query.ProductName));
			}
			string selectFields = "GroupBuyId,ProductId,ProductName,MaxCount,NeedPrice,Status,OrderCount,ISNULL(ProdcutQuantity,0) AS ProdcutQuantity,StartDate,EndDate,DisplaySequence";
			return DataHelper.PagingByTopnotin(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_GroupBuy", "GroupBuyId", stringBuilder.ToString(), selectFields);
		}
		public override void SwapGroupBuySequence(int groupBuyId, int displaySequence)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_GroupBuy SET DisplaySequence = @DisplaySequence WHERE GroupBuyId=@GroupBuyId");
			this.database.AddInParameter(sqlStringCommand, "DisplaySequence", System.Data.DbType.Int32, displaySequence);
			this.database.AddInParameter(sqlStringCommand, "GroupBuyId", System.Data.DbType.Int32, groupBuyId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override decimal GetCurrentPrice(int groupBuyId, int prodcutQuantity)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @price money;SELECT @price = MIN(price) FROM Hishop_GroupBuyCondition WHERE GroupBuyId=@GroupBuyId AND Count<=@prodcutQuantity;if @price IS NULL SELECT @price = max(price) FROM Hishop_GroupBuyCondition WHERE GroupBuyId=@GroupBuyId ;select @price");
			this.database.AddInParameter(sqlStringCommand, "GroupBuyId", System.Data.DbType.Int32, groupBuyId);
			this.database.AddInParameter(sqlStringCommand, "prodcutQuantity", System.Data.DbType.Int32, prodcutQuantity);
			return (decimal)this.database.ExecuteScalar(sqlStringCommand);
		}
		public override int GetOrderCount(int groupBuyId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SUM(Quantity) FROM Hishop_OrderItems WHERE OrderId IN (SELECT OrderId FROM Hishop_Orders WHERE GroupBuyId = @GroupBuyId AND OrderStatus <> 1 AND OrderStatus <> 4)");
			this.database.AddInParameter(sqlStringCommand, "GroupBuyId", System.Data.DbType.Int32, groupBuyId);
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
		public override bool SetGroupBuyStatus(int groupBuyId, GroupBuyStatus status)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_GroupBuy SET Status=@Status WHERE GroupBuyId=@GroupBuyId;UPDATE Hishop_Orders SET GroupBuyStatus=@Status WHERE GroupBuyId=@GroupBuyId");
			this.database.AddInParameter(sqlStringCommand, "GroupBuyId", System.Data.DbType.Int32, groupBuyId);
			this.database.AddInParameter(sqlStringCommand, "Status", System.Data.DbType.Int32, (int)status);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool SetGroupBuyEndUntreated(int groupBuyId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_GroupBuy SET Status=@Status,EndDate=@EndDate WHERE GroupBuyId=@GroupBuyId");
			this.database.AddInParameter(sqlStringCommand, "GroupBuyId", System.Data.DbType.Int32, groupBuyId);
			this.database.AddInParameter(sqlStringCommand, "EndDate", System.Data.DbType.DateTime, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "Status", System.Data.DbType.Int32, 2);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override DbQueryResult GetCountDownList(GroupBuyQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(query.ProductName))
			{
				stringBuilder.AppendFormat("ProductName Like '%{0}%'", DataHelper.CleanSearchString(query.ProductName));
			}
			string selectFields = "CountDownId,productId,ProductName,CountDownPrice,StartDate,EndDate,DisplaySequence,MaxCount ";
			return DataHelper.PagingByTopnotin(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_CountDown", "CountDownId", stringBuilder.ToString(), selectFields);
		}
		public override void SwapCountDownSequence(int countDownId, int displaySequence)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_CountDown SET DisplaySequence = @DisplaySequence WHERE CountDownId=@CountDownId");
			this.database.AddInParameter(sqlStringCommand, "DisplaySequence", System.Data.DbType.Int32, displaySequence);
			this.database.AddInParameter(sqlStringCommand, "CountDownId", System.Data.DbType.Int32, countDownId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override bool DeleteCountDown(int countDownId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_CountDown WHERE CountDownId=@CountDownId");
			this.database.AddInParameter(sqlStringCommand, "CountDownId", System.Data.DbType.Int32, countDownId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool AddCountDown(CountDownInfo countDownInfo)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_CountDown;INSERT INTO Hishop_CountDown(ProductId,CountDownPrice,StartDate,EndDate,Content,DisplaySequence,MaxCount ) VALUES(@ProductId,@CountDownPrice,@StartDate,@EndDate,@Content,@DisplaySequence,@MaxCount );");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, countDownInfo.ProductId);
			this.database.AddInParameter(sqlStringCommand, "CountDownPrice", System.Data.DbType.Currency, countDownInfo.CountDownPrice);
			this.database.AddInParameter(sqlStringCommand, "StartDate", System.Data.DbType.DateTime, countDownInfo.StartDate);
			this.database.AddInParameter(sqlStringCommand, "EndDate", System.Data.DbType.DateTime, countDownInfo.EndDate);
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, countDownInfo.Content);
			this.database.AddInParameter(sqlStringCommand, "MaxCount", System.Data.DbType.Int32, countDownInfo.MaxCount);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool UpdateCountDown(CountDownInfo countDownInfo)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_CountDown SET ProductId=@ProductId,CountDownPrice=@CountDownPrice,StartDate=@StartDate,EndDate=@EndDate,Content=@Content,MaxCount=@MaxCount  WHERE CountDownId=@CountDownId");
			this.database.AddInParameter(sqlStringCommand, "CountDownId", System.Data.DbType.Int32, countDownInfo.CountDownId);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, countDownInfo.ProductId);
			this.database.AddInParameter(sqlStringCommand, "CountDownPrice", System.Data.DbType.Currency, countDownInfo.CountDownPrice);
			this.database.AddInParameter(sqlStringCommand, "StartDate", System.Data.DbType.DateTime, countDownInfo.StartDate);
			this.database.AddInParameter(sqlStringCommand, "EndDate", System.Data.DbType.DateTime, countDownInfo.EndDate);
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, countDownInfo.Content);
			this.database.AddInParameter(sqlStringCommand, "MaxCount", System.Data.DbType.Int32, countDownInfo.MaxCount);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool ProductCountDownExist(int productId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_CountDown WHERE ProductId=@ProductId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
		}
		public override CountDownInfo GetCountDownInfo(int countDownId)
		{
			CountDownInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_CountDown WHERE CountDownId=@CountDownId");
			this.database.AddInParameter(sqlStringCommand, "CountDownId", System.Data.DbType.Int32, countDownId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateCountDown(dataReader);
				}
			}
			return result;
		}
		public override DbQueryResult GetBundlingProducts(BundlingInfoQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1");
			if (!string.IsNullOrEmpty(query.ProductName))
			{
				stringBuilder.AppendFormat(" AND Name Like '%{0}%' ", DataHelper.CleanSearchString(query.ProductName));
			}
			string selectFields = "Bundlingid,Name,Num,price,SaleStatus,OrderCount,AddTime,DisplaySequence";
			return DataHelper.PagingByTopnotin(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_Hishop_BundlingProducts", "Bundlingid", stringBuilder.ToString(), selectFields);
		}
		public override BundlingInfo GetBundlingInfo(int bundlingID)
		{
			BundlingInfo bundlingInfo = null;
			StringBuilder stringBuilder = new StringBuilder("SELECT * FROM Hishop_BundlingProducts WHERE BundlingID=@BundlingID;");
			stringBuilder.Append("SELECT [BundlingID] ,a.[ProductId] ,[SkuId] ,[ProductNum], productName,");
			stringBuilder.Append(" (select saleprice FROM  Hishop_SKUs c where c.SkuId= a.SkuId ) as ProductPrice");
			stringBuilder.Append(" FROM  Hishop_BundlingProductItems a JOIN Hishop_Products p ON a.ProductID = p.ProductId where BundlingID=@BundlingID AND p.SaleStatus = 1");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "BundlingID", System.Data.DbType.Int32, bundlingID);
			List<BundlingItemInfo> list = new List<BundlingItemInfo>();
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					bundlingInfo = DataMapper.PopulateBindInfo(dataReader);
				}
				dataReader.NextResult();
				while (dataReader.Read())
				{
					BundlingItemInfo bundlingItemInfo = new BundlingItemInfo();
					bundlingItemInfo.ProductID = (int)dataReader["ProductID"];
					bundlingItemInfo.ProductNum = (int)dataReader["ProductNum"];
					if (dataReader["SkuId"] != DBNull.Value)
					{
						bundlingItemInfo.SkuId = (string)dataReader["SkuId"];
					}
					if (dataReader["ProductName"] != DBNull.Value)
					{
						bundlingItemInfo.ProductName = (string)dataReader["ProductName"];
					}
					if (dataReader["ProductPrice"] != DBNull.Value)
					{
						bundlingItemInfo.ProductPrice = (decimal)dataReader["ProductPrice"];
					}
					bundlingItemInfo.BundlingID = bundlingID;
					list.Add(bundlingItemInfo);
				}
			}
			bundlingInfo.BundlingItemInfos = list;
			return bundlingInfo;
		}
		public override int AddBundlingProduct(BundlingInfo bind, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM Hishop_BundlingProducts;INSERT INTO Hishop_BundlingProducts(Name,ShortDescription,Num,Price,SaleStatus,AddTime,DisplaySequence) VALUES(@Name,@ShortDescription,@Num,@Price,@SaleStatus,@AddTime,@DisplaySequence); SELECT @@IDENTITY");
			this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, bind.Name);
			this.database.AddInParameter(sqlStringCommand, "ShortDescription", System.Data.DbType.String, bind.ShortDescription);
			this.database.AddInParameter(sqlStringCommand, "Num", System.Data.DbType.Int32, bind.Num);
			this.database.AddInParameter(sqlStringCommand, "Price", System.Data.DbType.Currency, bind.Price);
			this.database.AddInParameter(sqlStringCommand, "SaleStatus", System.Data.DbType.Int32, bind.SaleStatus);
			this.database.AddInParameter(sqlStringCommand, "AddTime", System.Data.DbType.DateTime, bind.AddTime);
			object obj;
			if (dbTran != null)
			{
				obj = this.database.ExecuteScalar(sqlStringCommand, dbTran);
			}
			else
			{
				obj = this.database.ExecuteScalar(sqlStringCommand);
			}
			int result;
			if (obj != null)
			{
				result = Convert.ToInt32(obj);
			}
			else
			{
				result = 0;
			}
			return result;
		}
		public override bool AddBundlingProductItems(int bundlingID, List<BundlingItemInfo> BundlingItemInfos, System.Data.Common.DbTransaction dbTran)
		{
			bool result;
			if (BundlingItemInfos.Count <= 0)
			{
				result = false;
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_BundlingProductItems(BundlingID,ProductID,SkuId,ProductNum) VALUES(@BundlingID,@ProductID,@Skuid,@ProductNum)");
				this.database.AddInParameter(sqlStringCommand, "BundlingID", System.Data.DbType.Int32, bundlingID);
				this.database.AddInParameter(sqlStringCommand, "ProductID", System.Data.DbType.Int32);
				this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String);
				this.database.AddInParameter(sqlStringCommand, "ProductNum", System.Data.DbType.Int32);
				try
				{
					foreach (BundlingItemInfo current in BundlingItemInfos)
					{
						this.database.SetParameterValue(sqlStringCommand, "ProductID", current.ProductID);
						this.database.SetParameterValue(sqlStringCommand, "SkuId", current.SkuId);
						this.database.SetParameterValue(sqlStringCommand, "ProductNum", current.ProductNum);
						if (dbTran != null)
						{
							this.database.ExecuteNonQuery(sqlStringCommand, dbTran);
						}
						else
						{
							this.database.ExecuteNonQuery(sqlStringCommand);
						}
					}
					result = true;
				}
				catch
				{
					result = false;
				}
			}
			return result;
		}
		public override bool DeleteBundlingProduct(int BundlingID)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_BundlingProducts WHERE BundlingID=@BundlingID");
			this.database.AddInParameter(sqlStringCommand, "BundlingID", System.Data.DbType.Int32, BundlingID);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool UpdateBundlingProduct(BundlingInfo bind, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_BundlingProducts  SET Name=@Name,ShortDescription=@ShortDescription,Num=@Num,Price=@Price,SaleStatus=@SaleStatus,AddTime=@AddTime WHERE BundlingID=@BundlingID");
			this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, bind.Name);
			this.database.AddInParameter(sqlStringCommand, "BundlingID", System.Data.DbType.Int32, bind.BundlingID);
			this.database.AddInParameter(sqlStringCommand, "ShortDescription", System.Data.DbType.String, bind.ShortDescription);
			this.database.AddInParameter(sqlStringCommand, "Num", System.Data.DbType.Int32, bind.Num);
			this.database.AddInParameter(sqlStringCommand, "Price", System.Data.DbType.Currency, bind.Price);
			this.database.AddInParameter(sqlStringCommand, "SaleStatus", System.Data.DbType.Int32, bind.SaleStatus);
			this.database.AddInParameter(sqlStringCommand, "AddTime", System.Data.DbType.DateTime, bind.AddTime);
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
		public override bool DeleteBundlingByID(int BundlingID, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_BundlingProductItems WHERE  BundlingID=@BundlingID");
			this.database.AddInParameter(sqlStringCommand, "BundlingID", System.Data.DbType.Int32, BundlingID);
			bool result;
			if (dbTran != null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
			}
			return result;
		}
	}
}
