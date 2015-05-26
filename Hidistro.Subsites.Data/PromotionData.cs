using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Promotions;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core.Enums;
using Hidistro.Subsites.Promotions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
namespace Hidistro.Subsites.Data
{
	public class PromotionData : SubsitePromotionsProvider
	{
		private Database database;
		public PromotionData()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public override CouponActionStatus CreateCoupon(CouponInfo coupon, int count, out string lotNumber)
		{
			CouponActionStatus couponActionStatus = CouponActionStatus.UnknowError;
			lotNumber = string.Empty;
			CouponActionStatus result;
			if (count <= 0)
			{
				if (null == coupon)
				{
					couponActionStatus = CouponActionStatus.UnknowError;
					result = CouponActionStatus.UnknowError;
					return result;
				}
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT CouponId  FROM distro_Coupons WHERE Name=@Name AND DistributorUserId=@DistributorUserId");
				this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, coupon.Name);
				this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
				if (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) >= 1)
				{
					couponActionStatus = CouponActionStatus.DuplicateName;
					result = CouponActionStatus.DuplicateName;
					return result;
				}
				sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO distro_Coupons ([Name],  ClosingTime,StartTime, Description, Amount, DiscountValue,DistributorUserId,SentCount,UsedCount,NeedPoint) VALUES(@Name, @ClosingTime,@StartTime, @Description, @Amount, @DiscountValue,@DistributorUserId,0,0,@NeedPoint); SELECT @@IDENTITY");
				this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, coupon.Name);
				this.database.AddInParameter(sqlStringCommand, "ClosingTime", System.Data.DbType.DateTime, coupon.ClosingTime);
				this.database.AddInParameter(sqlStringCommand, "StartTime", System.Data.DbType.DateTime, coupon.StartTime);
				this.database.AddInParameter(sqlStringCommand, "Description", System.Data.DbType.String, coupon.Description);
				this.database.AddInParameter(sqlStringCommand, "DiscountValue", System.Data.DbType.Currency, coupon.DiscountValue);
				this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
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
				System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_ClaimCode_Create");
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_CouponItems WHERE convert(nvarchar(300),LotNumber)=@LotNumber");
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
		public override DbQueryResult GetNewCoupons(Pagination page)
		{
			string filter = string.Format("DistributorUserId={0}", HiContext.Current.User.UserId);
			return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "distro_Coupons", "CouponId", filter, "*");
		}
		public override IList<Member> GetMembersByRank(int? gradeId)
		{
			IList<Member> list = new List<Member>();
			System.Data.Common.DbCommand sqlStringCommand;
			if (gradeId > 0)
			{
				sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vw_distro_Members WHERE GradeId=@GradeId AND ParentUserId=@ParentUserId");
				this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, gradeId);
				this.database.AddInParameter(sqlStringCommand, "ParentUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			}
			else
			{
				sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vw_distro_Members WHERE ParentUserId=@ParentUserId");
				this.database.AddInParameter(sqlStringCommand, "ParentUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
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
		public override bool DeleteCoupon(int couponId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_Coupons WHERE CouponId = @CouponId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "CouponId", System.Data.DbType.Int32, couponId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
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
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT CouponId  FROM distro_Coupons WHERE Name=@Name AND CouponId<>@CouponId AND DistributorUserId=@DistributorUserId");
				this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, coupon.Name);
				this.database.AddInParameter(sqlStringCommand, "CouponId", System.Data.DbType.Int32, coupon.CouponId);
				this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
				if (Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) >= 1)
				{
					result = CouponActionStatus.DuplicateName;
				}
				else
				{
					sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Coupons SET [Name]=@Name, ClosingTime=@ClosingTime,StartTime=@StartTime, Description=@Description, Amount=@Amount, DiscountValue=@DiscountValue, NeedPoint = @NeedPoint WHERE CouponId=@CouponId");
					this.database.AddInParameter(sqlStringCommand, "CouponId", System.Data.DbType.String, coupon.CouponId);
					this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, coupon.Name);
					this.database.AddInParameter(sqlStringCommand, "StartTime", System.Data.DbType.DateTime, coupon.StartTime);
					this.database.AddInParameter(sqlStringCommand, "ClosingTime", System.Data.DbType.DateTime, coupon.ClosingTime);
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
		public override CouponInfo GetCouponDetails(int couponId)
		{
			CouponInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_Coupons WHERE CouponId = @CouponId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "CouponId", System.Data.DbType.Int32, couponId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateCoupon(dataReader);
				}
			}
			return result;
		}
		public override bool SendClaimCodes(int couponId, CouponItemInfo couponItem)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO distro_CouponItems(CouponId, ClaimCode, GenerateTime, UserId,UserName, EmailAddress,LotNumber) VALUES(@CouponId, @ClaimCode, @GenerateTime, @UserId,@UserName, @EmailAddress,@LotNumber)");
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
			return this.database.ExecuteNonQuery(sqlStringCommand) >= 1;
		}
		public override List<int> GetUderlingIds(int? gradeId, string userName)
		{
			List<int> list = new List<int>();
			string text = string.Format("SELECT UserId FROM vw_distro_Members WHERE UserName Like '%{0}%' AND ParentUserId={1}", DataHelper.CleanSearchString(userName), HiContext.Current.User.UserId);
			if (gradeId.HasValue)
			{
				string str = string.Format(" AND GradeId={0} ", gradeId);
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
		public override DbQueryResult GetGifts(GiftQuery query)
		{
			string text = "IsDownLoad=1 and GiftId not in (select GiftId from distro_Gifts where DistributorUserId=" + HiContext.Current.User.UserId + ")";
			if (!string.IsNullOrEmpty(query.Name))
			{
				text += string.Format(" and [Name] LIKE '%{0}%'", DataHelper.CleanSearchString(query.Name));
			}
			Pagination page = query.Page;
			return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Hishop_Gifts", "GiftId", text, "*");
		}
		public override DbQueryResult GetAbstroGiftsById(GiftQuery query)
		{
			string text = "d_DistributorUserId=" + HiContext.Current.User.UserId;
			if (query.IsPromotion)
			{
				text += " AND d_IsPromotion = 1";
			}
			if (!string.IsNullOrEmpty(query.Name))
			{
				text += string.Format(" AND ([Name] LIKE '%{0}%' or d_Name like '%{1}%')", DataHelper.CleanSearchString(query.Name), DataHelper.CleanSearchString(query.Name));
			}
			Pagination page = query.Page;
			return DataHelper.PagingByRownumber(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "vw_distro_Gifts", "d_GiftId", text, "d_GiftId,d_DistributorUserId,d_Name,d_NeedPoint,Name,GiftId,PurchasePrice,ThumbnailUrl40,d_IsPromotion");
		}
		public override bool DownLoadGift(GiftInfo gift)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("insert into distro_Gifts VALUES (@GiftId,@DistributorUserId,@Name,@ShortDescription,@Title,@Meta_Description,@Meta_Keywords,@NeedPoint,@IsPromotion)");
			this.database.AddInParameter(sqlStringCommand, "GiftId", System.Data.DbType.Int32, gift.GiftId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, gift.Name);
			this.database.AddInParameter(sqlStringCommand, "ShortDescription", System.Data.DbType.String, gift.ShortDescription);
			this.database.AddInParameter(sqlStringCommand, "Title", System.Data.DbType.String, gift.Title);
			this.database.AddInParameter(sqlStringCommand, "Meta_Description", System.Data.DbType.String, gift.Meta_Description);
			this.database.AddInParameter(sqlStringCommand, "Meta_Keywords", System.Data.DbType.String, gift.Meta_Keywords);
			this.database.AddInParameter(sqlStringCommand, "NeedPoint", System.Data.DbType.Int32, gift.NeedPoint);
			this.database.AddInParameter(sqlStringCommand, "IsPromotion", System.Data.DbType.Boolean, gift.IsPromotion);
			return this.database.ExecuteNonQuery(sqlStringCommand) >= 1;
		}
		public override bool DeleteGiftById(int giftId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("delete from distro_Gifts where GiftId=@Id and DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "Id", System.Data.DbType.Int32, giftId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand) >= 1;
		}
		public override GiftInfo GetMyGiftsDetails(int Id)
		{
			GiftInfo giftInfo = new GiftInfo();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select d_Name as [Name],d_Title as Title,d_Meta_Description as Meta_Description,d_Meta_Keywords as Meta_Keywords,d_NeedPoint as NeedPoint,GiftId,ShortDescription,Unit, LongDescription,CostPrice,ImageUrl, ThumbnailUrl40,ThumbnailUrl60,ThumbnailUrl100,ThumbnailUrl160, ThumbnailUrl180, ThumbnailUrl220, ThumbnailUrl310, ThumbnailUrl410, PurchasePrice,MarketPrice,IsDownLoad,d_IsPromotion as IsPromotion from vw_distro_Gifts where d_DistributorUserId=@DistributorUserId and GiftId=@Id");
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "Id", System.Data.DbType.Int32, Id);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					giftInfo = DataMapper.PopulateGift(dataReader);
				}
			}
			Globals.EntityCoding(giftInfo, false);
			return giftInfo;
		}
		public override bool UpdateMyGifts(GiftInfo giftInfo)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update distro_Gifts set [Name]=@Name,ShortDescription=@ShortDescription,Title=@Title,Meta_Description=@Meta_Description,Meta_Keywords=@Meta_Keywords,NeedPoint=@NeedPoint,IsPromotion=@IsPromotion where GiftId=@Id and DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, giftInfo.Name);
			this.database.AddInParameter(sqlStringCommand, "ShortDescription", System.Data.DbType.String, giftInfo.ShortDescription);
			this.database.AddInParameter(sqlStringCommand, "Title", System.Data.DbType.String, giftInfo.Title);
			this.database.AddInParameter(sqlStringCommand, "Meta_Description", System.Data.DbType.String, giftInfo.Meta_Description);
			this.database.AddInParameter(sqlStringCommand, "Meta_Keywords", System.Data.DbType.String, giftInfo.Meta_Keywords);
			this.database.AddInParameter(sqlStringCommand, "NeedPoint", System.Data.DbType.Int32, giftInfo.NeedPoint);
			this.database.AddInParameter(sqlStringCommand, "Id", System.Data.DbType.Int32, giftInfo.GiftId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "IsPromotion", System.Data.DbType.Boolean, giftInfo.IsPromotion);
			return this.database.ExecuteNonQuery(sqlStringCommand) >= 1;
		}
		public override string GetPriceByProductId(int productId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT (SELECT SalePrice FROM vw_distro_SkuPrices WHERE SkuId = p.SkuId AND DistributoruserId = @DistributorUserId) AS SalePrice FROM vw_distro_BrowseProductList p where p.ProductId=@ProductId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return this.database.ExecuteScalar(sqlStringCommand).ToString();
		}
		public override int AddGroupBuy(GroupBuyInfo groupBuy, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM distro_GroupBuy;INSERT INTO distro_GroupBuy(ProductId,DistributorUserId,NeedPrice,StartDate,EndDate,MaxCount,Content,Status,DisplaySequence) VALUES(@ProductId,@DistributorUserId,@NeedPrice,@StartDate,@EndDate,@MaxCount,@Content,@Status,@DisplaySequence); SELECT @@IDENTITY");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, groupBuy.ProductId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM distro_GroupBuy WHERE ProductId=@ProductId AND Status=@Status AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
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
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO distro_GroupBuyCondition(GroupBuyId,Count,Price,DistributorUserId) VALUES(@GroupBuyId,@Count,@Price,@DistributorUserId)");
				this.database.AddInParameter(sqlStringCommand, "GroupBuyId", System.Data.DbType.Int32, groupBuyId);
				this.database.AddInParameter(sqlStringCommand, "Count", System.Data.DbType.Int32);
				this.database.AddInParameter(sqlStringCommand, "Price", System.Data.DbType.Currency);
				this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_GroupBuy WHERE GroupBuyId=@GroupBuyId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "GroupBuyId", System.Data.DbType.Int32, groupBuyId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool DeleteGroupBuyCondition(int groupBuyId, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_GroupBuyCondition WHERE GroupBuyId=@GroupBuyId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "GroupBuyId", System.Data.DbType.Int32, groupBuyId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_GroupBuy SET ProductId=@ProductId,NeedPrice=@NeedPrice,StartDate=@StartDate,EndDate=@EndDate,MaxCount=@MaxCount,Content=@Content WHERE GroupBuyId=@GroupBuyId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "GroupBuyId", System.Data.DbType.Int32, groupBuy.GroupBuyId);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, groupBuy.ProductId);
			this.database.AddInParameter(sqlStringCommand, "NeedPrice", System.Data.DbType.Currency, groupBuy.NeedPrice);
			this.database.AddInParameter(sqlStringCommand, "StartDate", System.Data.DbType.DateTime, groupBuy.StartDate);
			this.database.AddInParameter(sqlStringCommand, "EndDate", System.Data.DbType.DateTime, groupBuy.EndDate);
			this.database.AddInParameter(sqlStringCommand, "MaxCount", System.Data.DbType.Int32, groupBuy.MaxCount);
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, groupBuy.Content);
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
		public override GroupBuyInfo GetGroupBuy(int groupBuyId)
		{
			GroupBuyInfo groupBuyInfo = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_GroupBuy WHERE GroupBuyId=@GroupBuyId AND DistributorUserId=@DistributorUserId;SELECT * FROM distro_GroupBuyCondition WHERE GroupBuyId=@GroupBuyId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "GroupBuyId", System.Data.DbType.Int32, groupBuyId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
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
			stringBuilder.AppendFormat(" DistributorUserId={0}", HiContext.Current.User.UserId);
			if (!string.IsNullOrEmpty(query.ProductName))
			{
				stringBuilder.AppendFormat(" AND ProductName Like '%{0}%'", DataHelper.CleanSearchString(query.ProductName));
			}
			string selectFields = "GroupBuyId,ProductId, ProductName,MaxCount,NeedPrice,Status,OrderCount,ISNULL(ProdcutQuantity,0) AS ProdcutQuantity,StartDate,EndDate,DisplaySequence";
			return DataHelper.PagingByTopnotin(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_distro_GroupBuy", "GroupBuyId", stringBuilder.ToString(), selectFields);
		}
		public override void SwapGroupBuySequence(int groupBuyId, int displaySequence)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_GroupBuy SET DisplaySequence = @DisplaySequence WHERE GroupBuyId=@GroupBuyId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "DisplaySequence", System.Data.DbType.Int32, displaySequence);
			this.database.AddInParameter(sqlStringCommand, "GroupBuyId", System.Data.DbType.Int32, groupBuyId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override decimal GetCurrentPrice(int groupBuyId, int prodcutQuantity)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @price money;SELECT @price = MIN(price) FROM distro_GroupBuyCondition WHERE GroupBuyId=@GroupBuyId AND Count<=@prodcutQuantity AND DistributorUserId=@DistributorUserId;if @price IS NULL SELECT @price = max(price) FROM distro_GroupBuyCondition WHERE GroupBuyId=@GroupBuyId AND DistributorUserId=@DistributorUserId;select @price");
			this.database.AddInParameter(sqlStringCommand, "GroupBuyId", System.Data.DbType.Int32, groupBuyId);
			this.database.AddInParameter(sqlStringCommand, "prodcutQuantity", System.Data.DbType.Int32, prodcutQuantity);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return (decimal)this.database.ExecuteScalar(sqlStringCommand);
		}
		public override int GetOrderCount(int groupBuyId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SUM(Quantity) FROM distro_OrderItems WHERE OrderId IN (SELECT OrderId FROM distro_Orders WHERE GroupBuyId = @GroupBuyId AND DistributorUserId=@DistributorUserId AND OrderStatus <> 1 AND OrderStatus <> 4)");
			this.database.AddInParameter(sqlStringCommand, "GroupBuyId", System.Data.DbType.Int32, groupBuyId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_GroupBuy SET Status=@Status WHERE GroupBuyId=@GroupBuyId AND DistributorUserId=@DistributorUserId;UPDATE distro_Orders SET GroupBuyStatus=@Status WHERE GroupBuyId=@GroupBuyId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "GroupBuyId", System.Data.DbType.Int32, groupBuyId);
			this.database.AddInParameter(sqlStringCommand, "Status", System.Data.DbType.Int32, (int)status);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool SetGroupBuyEndUntreated(int groupBuyId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_GroupBuy SET Status=@Status,EndDate=@EndDate WHERE GroupBuyId=@GroupBuyId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "GroupBuyId", System.Data.DbType.Int32, groupBuyId);
			this.database.AddInParameter(sqlStringCommand, "EndDate", System.Data.DbType.DateTime, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "Status", System.Data.DbType.Int32, 2);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override DbQueryResult GetCountDownList(GroupBuyQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" DistributorUserId={0}", HiContext.Current.User.UserId);
			if (!string.IsNullOrEmpty(query.ProductName))
			{
				stringBuilder.AppendFormat(" AND ProductName Like '%{0}%'", DataHelper.CleanSearchString(query.ProductName));
			}
			string selectFields = "CountDownId,ProductId, ProductName,CountDownPrice,StartDate,EndDate,DisplaySequence,MaxCount";
			return DataHelper.PagingByTopnotin(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_distro_CountDown", "CountDownId", stringBuilder.ToString(), selectFields);
		}
		public override void SwapCountDownSequence(int countDownId, int displaySequence)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_CountDown SET DisplaySequence = @DisplaySequence WHERE CountDownId=@CountDownId");
			this.database.AddInParameter(sqlStringCommand, "DisplaySequence", System.Data.DbType.Int32, displaySequence);
			this.database.AddInParameter(sqlStringCommand, "CountDownId", System.Data.DbType.Int32, countDownId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override bool DeleteCountDown(int countDownId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_CountDown WHERE CountDownId=@CountDownId AND DistributorUserId = @DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "CountDownId", System.Data.DbType.Int32, countDownId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool AddCountDown(CountDownInfo countDownInfo)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM distro_CountDown; INSERT INTO distro_CountDown(ProductId,DistributorUserId,CountDownPrice,StartDate,EndDate,Content,DisplaySequence,MaxCount) VALUES(@ProductId,@DistributorUserId,@CountDownPrice,@StartDate,@EndDate,@Content,@DisplaySequence,@MaxCount);");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, countDownInfo.ProductId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "CountDownPrice", System.Data.DbType.Currency, countDownInfo.CountDownPrice);
			this.database.AddInParameter(sqlStringCommand, "StartDate", System.Data.DbType.DateTime, countDownInfo.StartDate);
			this.database.AddInParameter(sqlStringCommand, "EndDate", System.Data.DbType.DateTime, countDownInfo.EndDate);
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, countDownInfo.Content);
			this.database.AddInParameter(sqlStringCommand, "MaxCount", System.Data.DbType.Int32, countDownInfo.MaxCount);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool UpdateCountDown(CountDownInfo countDownInfo)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_CountDown SET ProductId=@ProductId,CountDownPrice=@CountDownPrice,StartDate=@StartDate,EndDate=@EndDate,Content=@Content,MaxCount=@MaxCount WHERE CountDownId=@CountDownId AND DistributorUserId = @DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "CountDownId", System.Data.DbType.Int32, countDownInfo.CountDownId);
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, countDownInfo.ProductId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "CountDownPrice", System.Data.DbType.Currency, countDownInfo.CountDownPrice);
			this.database.AddInParameter(sqlStringCommand, "StartDate", System.Data.DbType.DateTime, countDownInfo.StartDate);
			this.database.AddInParameter(sqlStringCommand, "EndDate", System.Data.DbType.DateTime, countDownInfo.EndDate);
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, countDownInfo.Content);
			this.database.AddInParameter(sqlStringCommand, "MaxCount", System.Data.DbType.Int32, countDownInfo.MaxCount);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool ProductCountDownExist(int productId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM distro_CountDown WHERE ProductId=@ProductId AND DistributorUserId = @DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
		}
		public override DbQueryResult GetCouponsList(CouponItemInfoQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("DistributorUserId = {0}", HiContext.Current.User.UserId);
			if (query.CouponId.HasValue)
			{
				stringBuilder.AppendFormat(" AND CouponId = {0}", query.CouponId.Value);
			}
			if (!string.IsNullOrEmpty(query.CounponName))
			{
				stringBuilder.AppendFormat(" AND Name = '{0}'", query.CounponName);
			}
			if (!string.IsNullOrEmpty(query.UserName))
			{
				stringBuilder.AppendFormat(" AND UserName='{0}'", DataHelper.CleanSearchString(query.UserName));
			}
			if (!string.IsNullOrEmpty(query.OrderId))
			{
				stringBuilder.AppendFormat(" AND Orderid='{0}'", DataHelper.CleanSearchString(query.OrderId));
			}
			if (query.CouponStatus.HasValue)
			{
				stringBuilder.AppendFormat(" AND CouponStatus={0} ", query.CouponStatus);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_distro_CouponInfo", "ClaimCode", stringBuilder.ToString(), "*");
		}
		public override CountDownInfo GetCountDownInfo(int countDownId)
		{
			CountDownInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_CountDown WHERE CountDownId=@CountDownId AND DistributorUserId = @DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "CountDownId", System.Data.DbType.Int32, countDownId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateCountDown(dataReader);
				}
			}
			return result;
		}
		public override System.Data.DataTable GetPromotions(bool isProductPromote)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_Promotions WHERE DistributorUserId=" + HiContext.Current.User.UserId);
			if (isProductPromote)
			{
				System.Data.Common.DbCommand expr_31 = sqlStringCommand;
				expr_31.CommandText += " AND PromoteType < 10";
			}
			else
			{
				System.Data.Common.DbCommand expr_49 = sqlStringCommand;
				expr_49.CommandText += " AND PromoteType > 10";
			}
			System.Data.Common.DbCommand expr_5F = sqlStringCommand;
			expr_5F.CommandText += " ORDER BY ActivityId DESC";
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_Promotions WHERE ActivityId = @ActivityId AND DistributorUserId=@DistributorUserId; SELECT GradeId FROM distro_PromotionMemberGrades WHERE ActivityId = @ActivityId");
			this.database.AddInParameter(sqlStringCommand, "ActivityId", System.Data.DbType.Int32, activityId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_MemberGrades WHERE CreateUserId=@CreateUserId AND GradeId IN (SELECT GradeId FROM distro_PromotionMemberGrades WHERE ActivityId = @ActivityId)");
			this.database.AddInParameter(sqlStringCommand, "ActivityId", System.Data.DbType.Int32, activityId);
			this.database.AddInParameter(sqlStringCommand, "CreateUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vw_distro_BrowseProductList WHERE DistributorUserId=@DistributorUserId AND ProductId IN (SELECT ProductId FROM distro_PromotionProducts WHERE ActivityId = @ActivityId AND DistributorUserId=@DistributorUserId) ORDER BY DisplaySequence");
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("INSERT INTO distro_PromotionProducts SELECT @ActivityId, ProductId,DistributorUserId FROM distro_Products WHERE ProductId IN ({0})", productIds) + " AND ProductId NOT IN (SELECT ProductId FROM distro_PromotionProducts WHERE DistributorUserId=@DistributorUserId) AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "ActivityId", System.Data.DbType.Int32, activityId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool DeletePromotionProducts(int activityId, int? productId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_PromotionProducts WHERE ActivityId = @ActivityId AND DistributorUserId=@DistributorUserId");
			if (productId.HasValue)
			{
				System.Data.Common.DbCommand expr_1E = sqlStringCommand;
				expr_1E.CommandText += string.Format(" AND ProductId = {0}", productId.Value);
			}
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "ActivityId", System.Data.DbType.Int32, activityId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override int AddPromotion(PromotionInfo promotion, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO distro_Promotions(DistributorUserId,Name, PromoteType, Condition, DiscountValue, StartDate, EndDate, Description) VALUES(@DistributorUserId,@Name, @PromoteType, @Condition, @DiscountValue, @StartDate, @EndDate, @Description); SELECT @@IDENTITY");
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
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
			stringBuilder.AppendFormat("DELETE FROM distro_PromotionMemberGrades WHERE ActivityId = {0}", activityId);
			foreach (int current in memberGrades)
			{
				stringBuilder.AppendFormat(" INSERT INTO distro_PromotionMemberGrades (ActivityId, GradeId) VALUES ({0}, {1})", activityId, current);
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Promotions SET Name = @Name, PromoteType = @PromoteType, Condition = @Condition, DiscountValue = @DiscountValue, StartDate = @StartDate, EndDate = @EndDate, Description = @Description WHERE ActivityId = @ActivityId");
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_Promotions WHERE ActivityId = @ActivityId");
			this.database.AddInParameter(sqlStringCommand, "ActivityId", System.Data.DbType.Int32, activityId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override DbQueryResult GetBundlingProducts(BundlingInfoQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" 1=1");
			stringBuilder.AppendFormat(" AND  DistributorUserId={0} ", HiContext.Current.User.UserId);
			if (!string.IsNullOrEmpty(query.ProductName))
			{
				stringBuilder.AppendFormat(" AND Name Like '%{0}%'", DataHelper.CleanSearchString(query.ProductName));
			}
			string selectFields = "Bundlingid,DistributorUserId,Name,Num,price,SaleStatus,OrderCount,AddTime,DisplaySequence";
			return DataHelper.PagingByTopnotin(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_distro_BundlingProducts", "Bundlingid", stringBuilder.ToString(), selectFields);
		}
		public override BundlingInfo GetBundlingInfo(int bundlingID)
		{
			BundlingInfo bundlingInfo = null;
			StringBuilder stringBuilder = new StringBuilder("SELECT * FROM distro_BundlingProducts WHERE BundlingID=@BundlingID and DistributorUserId=@DistributorUserId;");
			stringBuilder.Append("SELECT [BundlingID] ,a.[ProductId] ,[SkuId] ,[ProductNum], productName, ");
			stringBuilder.Append(" (select saleprice FROM  Hishop_SKUs c where c.SkuId= a.SkuId ) as ProductPrice");
			stringBuilder.Append(" FROM  distro_BundlingProductItems a JOIN distro_Products p ON a.ProductID = p.ProductId AND p.DistributorUserId = @DistributorUserId");
			stringBuilder.Append(" where BundlingID=@BundlingID AND p.SaleStatus = 1");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @DisplaySequence AS INT SELECT @DisplaySequence = (CASE WHEN MAX(DisplaySequence) IS NULL THEN 1 ELSE MAX(DisplaySequence) + 1 END) FROM distro_BundlingProducts;INSERT INTO distro_BundlingProducts(DistributorUserId,Name,ShortDescription,Num,Price,SaleStatus,AddTime,DisplaySequence) VALUES(@DistributorUserId,@Name,@ShortDescription,@Num,@Price,@SaleStatus,@AddTime,@DisplaySequence); SELECT @@IDENTITY");
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
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
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO distro_BundlingProductItems(BundlingID,ProductID,SkuId,ProductNum) VALUES(@BundlingID,@ProductID,@Skuid,@ProductNum)");
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_BundlingProducts WHERE BundlingID=@BundlingID");
			this.database.AddInParameter(sqlStringCommand, "BundlingID", System.Data.DbType.Int32, BundlingID);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool UpdateBundlingProduct(BundlingInfo bind, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_BundlingProducts  SET Name=@Name,ShortDescription=@ShortDescription,Num=@Num,Price=@Price,SaleStatus=@SaleStatus,AddTime=@AddTime WHERE BundlingID=@BundlingID");
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_BundlingProductItems WHERE  BundlingID=@BundlingID");
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
