using Hidistro.AccountCenter.Profile;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
namespace Hidistro.AccountCenter.Data
{
	public class ProfileData : PersonalMasterProvider
	{
		private Database database;
		public ProfileData()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public override DbQueryResult GetBalanceDetails(BalanceDetailQuery query)
		{
			DbQueryResult result;
			if (null == query)
			{
				result = null;
			}
			else
			{
				DbQueryResult dbQueryResult = new DbQueryResult();
				StringBuilder stringBuilder = new StringBuilder();
				string text = ProfileData.BuildBalanceDetailsQuery(query);
				stringBuilder.AppendFormat("SELECT TOP {0} * FROM Hishop_BalanceDetails B WHERE 0=0", query.PageSize);
				if (query.PageIndex == 1)
				{
					stringBuilder.AppendFormat(" {0} ORDER BY JournalNumber DESC;", text);
				}
				else
				{
					stringBuilder.AppendFormat(" AND JournalNumber < (SELECT MIN(JournalNumber) FROM (SELECT TOP {0} JournalNumber FROM Hishop_BalanceDetails WHERE 0=0 {1} ORDER BY JournalNumber DESC ) AS T) {1} ORDER BY JournalNumber DESC;", (query.PageIndex - 1) * query.PageSize, text);
				}
				if (query.IsCount)
				{
					stringBuilder.AppendFormat(" SELECT COUNT(JournalNumber) AS Total from Hishop_BalanceDetails WHERE 0=0 {0}", text);
				}
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					dbQueryResult.Data = DataHelper.ConverDataReaderToDataTable(dataReader);
					if (query.IsCount && dataReader.NextResult())
					{
						dataReader.Read();
						dbQueryResult.TotalRecords = dataReader.GetInt32(0);
					}
				}
				result = dbQueryResult;
			}
			return result;
		}
		public override bool AddBalanceDetail(BalanceDetailInfo balanceDetails)
		{
			bool result;
			if (null == balanceDetails)
			{
				result = false;
			}
			else
			{
				string text = "INSERT INTO Hishop_BalanceDetails(UserId, UserName, TradeDate, TradeType, Income, Balance,Remark, InpourId) VALUES (@UserId, @UserName,@TradeDate, @TradeType, @Income, @Balance, @Remark, @InpourId)";
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
				this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, balanceDetails.UserId);
				this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, balanceDetails.UserName);
				this.database.AddInParameter(sqlStringCommand, "TradeDate", System.Data.DbType.DateTime, balanceDetails.TradeDate);
				this.database.AddInParameter(sqlStringCommand, "TradeType", System.Data.DbType.Int32, (int)balanceDetails.TradeType);
				this.database.AddInParameter(sqlStringCommand, "Income", System.Data.DbType.Currency, balanceDetails.Income);
				this.database.AddInParameter(sqlStringCommand, "Balance", System.Data.DbType.Currency, balanceDetails.Balance);
				this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, balanceDetails.Remark);
				this.database.AddInParameter(sqlStringCommand, "InpourId", System.Data.DbType.String, balanceDetails.InpourId);
				result = (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
			}
			return result;
		}
		public override bool IsRecharge(string inpourId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_BalanceDetails WHERE InpourId = @InpourId");
			this.database.AddInParameter(sqlStringCommand, "InpourId", System.Data.DbType.String, inpourId);
			return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
		}
		public override bool AddInpourBlance(InpourRequestInfo inpourRequest)
		{
			bool result;
			if (null == inpourRequest)
			{
				result = false;
			}
			else
			{
				System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("ac_Member_InpourRequest_Create");
				this.database.AddInParameter(storedProcCommand, "InpourId", System.Data.DbType.String, inpourRequest.InpourId);
				this.database.AddInParameter(storedProcCommand, "TradeDate", System.Data.DbType.DateTime, inpourRequest.TradeDate);
				this.database.AddInParameter(storedProcCommand, "InpourBlance", System.Data.DbType.Currency, inpourRequest.InpourBlance);
				this.database.AddInParameter(storedProcCommand, "UserId", System.Data.DbType.Int32, inpourRequest.UserId);
				this.database.AddInParameter(storedProcCommand, "PaymentId", System.Data.DbType.String, inpourRequest.PaymentId);
				this.database.AddOutParameter(storedProcCommand, "Status", System.Data.DbType.Int32, 4);
				this.database.ExecuteNonQuery(storedProcCommand);
				result = ((int)this.database.GetParameterValue(storedProcCommand, "Status") == 0);
			}
			return result;
		}
		public override InpourRequestInfo GetInpourBlance(string inpourId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_InpourRequest WHERE InpourId = @InpourId;");
			this.database.AddInParameter(sqlStringCommand, "InpourId", System.Data.DbType.String, inpourId);
			InpourRequestInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateInpourRequest(dataReader);
				}
			}
			return result;
		}
		public override void RemoveInpourRequest(string inpourId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_InpourRequest WHERE InpourId = @InpourId");
			this.database.AddInParameter(sqlStringCommand, "InpourId", System.Data.DbType.String, inpourId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override void GetStatisticsNum(out int noPayOrderNum, out int noReadMessageNum, out int noReplyLeaveCommentNum)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT COUNT(*) AS NoPayOrderNum FROM Hishop_Orders WHERE UserId = {0} AND OrderStatus = {1};", HiContext.Current.User.UserId, 1);
			stringBuilder.AppendFormat(" SELECT COUNT(*) AS NoReadMessageNum FROM Hishop_MemberMessageBox WHERE Accepter = '{0}' AND IsRead=0 ;", HiContext.Current.User.Username);
			stringBuilder.AppendFormat(" SELECT COUNT(*) AS NoReplyLeaveCommentNum FROM Hishop_ProductConsultations WHERE UserId = {0} AND ViewDate is null AND ReplyUserId is not null;", HiContext.Current.User.UserId);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read() && DBNull.Value != dataReader["NoPayOrderNum"])
				{
					num = (int)dataReader["NoPayOrderNum"];
				}
				if (dataReader.NextResult() && dataReader.Read() && DBNull.Value != dataReader["NoReadMessageNum"])
				{
					num2 = (int)dataReader["NoReadMessageNum"];
				}
				if (dataReader.NextResult() && dataReader.Read() && DBNull.Value != dataReader["NoReplyLeaveCommentNum"])
				{
					num3 = (int)dataReader["NoReplyLeaveCommentNum"];
				}
			}
			noPayOrderNum = num;
			noReadMessageNum = num2;
			noReplyLeaveCommentNum = num3;
		}
		public override bool ViewProductConsultations()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" update Hishop_ProductConsultations set ViewDate=getdate() WHERE UserId = {0} AND ViewDate is null AND ReplyUserId is not null;", HiContext.Current.User.UserId);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool BalanceDrawRequest(BalanceDrawRequestInfo balanceDrawRequest)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_BalanceDrawRequest (UserId,UserName,RequestTime, Amount, AccountName, BankName, MerchantCode, Remark) VALUES (@UserId,@UserName,@RequestTime, @Amount, @AccountName, @BankName, @MerchantCode, @Remark)");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, balanceDrawRequest.UserId);
			this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, balanceDrawRequest.UserName);
			this.database.AddInParameter(sqlStringCommand, "RequestTime", System.Data.DbType.DateTime, balanceDrawRequest.RequestTime);
			this.database.AddInParameter(sqlStringCommand, "Amount", System.Data.DbType.Currency, balanceDrawRequest.Amount);
			this.database.AddInParameter(sqlStringCommand, "AccountName", System.Data.DbType.String, balanceDrawRequest.AccountName);
			this.database.AddInParameter(sqlStringCommand, "BankName", System.Data.DbType.String, balanceDrawRequest.BankName);
			this.database.AddInParameter(sqlStringCommand, "MerchantCode", System.Data.DbType.String, balanceDrawRequest.MerchantCode);
			this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, balanceDrawRequest.Remark);
			return this.database.ExecuteNonQuery(sqlStringCommand) >= 1;
		}
		public override int GetShippingAddressCount(int userId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT count(ShippingId) as Count FROM Hishop_UserShippingAddresses WHERE  UserID = @UserID");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			int result = 0;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = (int)dataReader["Count"];
				}
			}
			return result;
		}
		public override bool CreateUpdateDeleteShippingAddress(ShippingAddressInfo shippingAddress, DataProviderAction action)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("ac_Member_ShippingAddress_CreateUpdateDelete");
			this.database.AddInParameter(storedProcCommand, "Action", System.Data.DbType.Int32, (int)action);
			this.database.AddOutParameter(storedProcCommand, "Status", System.Data.DbType.Int32, 4);
			if (action != DataProviderAction.Create)
			{
				this.database.AddInParameter(storedProcCommand, "ShippingId", System.Data.DbType.Int32, shippingAddress.ShippingId);
			}
			if (action != DataProviderAction.Delete)
			{
				this.database.AddInParameter(storedProcCommand, "RegionId", System.Data.DbType.Int32, shippingAddress.RegionId);
				this.database.AddInParameter(storedProcCommand, "UserId", System.Data.DbType.Int32, shippingAddress.UserId);
				this.database.AddInParameter(storedProcCommand, "ShipTo", System.Data.DbType.String, shippingAddress.ShipTo);
				this.database.AddInParameter(storedProcCommand, "Address", System.Data.DbType.String, shippingAddress.Address);
				this.database.AddInParameter(storedProcCommand, "Zipcode", System.Data.DbType.String, shippingAddress.Zipcode);
				this.database.AddInParameter(storedProcCommand, "TelPhone", System.Data.DbType.String, shippingAddress.TelPhone);
				this.database.AddInParameter(storedProcCommand, "CellPhone", System.Data.DbType.String, shippingAddress.CellPhone);
			}
			this.database.ExecuteNonQuery(storedProcCommand);
			return (int)this.database.GetParameterValue(storedProcCommand, "Status") == 0;
		}
		public override int AddShippingAddress(ShippingAddressInfo shippingAddress)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_UserShippingAddresses(RegionId,UserId,ShipTo,Address,Zipcode,EmailAddress,TelPhone,CellPhone) VALUES(@RegionId,@UserId,@ShipTo,@Address,@Zipcode,@EmailAddress,@TelPhone,@CellPhone); SELECT @@IDENTITY");
			this.database.AddInParameter(sqlStringCommand, "RegionId", System.Data.DbType.Int32, shippingAddress.RegionId);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, shippingAddress.UserId);
			this.database.AddInParameter(sqlStringCommand, "ShipTo", System.Data.DbType.String, shippingAddress.ShipTo);
			this.database.AddInParameter(sqlStringCommand, "Address", System.Data.DbType.String, shippingAddress.Address);
			this.database.AddInParameter(sqlStringCommand, "Zipcode", System.Data.DbType.String, shippingAddress.Zipcode);
			this.database.AddInParameter(sqlStringCommand, "TelPhone", System.Data.DbType.String, shippingAddress.TelPhone);
			this.database.AddInParameter(sqlStringCommand, "CellPhone", System.Data.DbType.String, shippingAddress.CellPhone);
			return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
		}
		public override IList<ShippingAddressInfo> GetShippingAddress(int userId)
		{
			IList<ShippingAddressInfo> list = new List<ShippingAddressInfo>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_UserShippingAddresses WHERE  UserID = @UserID");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulateShippingAddress(dataReader));
				}
			}
			return list;
		}
		public override ShippingAddressInfo GetUserShippingAddress(int shippingId)
		{
			ShippingAddressInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_UserShippingAddresses WHERE ShippingId = @ShippingId");
			this.database.AddInParameter(sqlStringCommand, "ShippingId", System.Data.DbType.Int32, shippingId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateShippingAddress(dataReader);
				}
			}
			return result;
		}
		public override MemberGradeInfo GetMemberGrade(int gradeId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_MemberGrades WHERE GradeId = @GradeId");
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, gradeId);
			MemberGradeInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateMemberGrade(dataReader);
				}
			}
			return result;
		}
		public override IList<MemberGradeInfo> GetMemberGrades()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_MemberGrades");
			IList<MemberGradeInfo> list = new List<MemberGradeInfo>();
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					list.Add(DataMapper.PopulateMemberGrade(dataReader));
				}
			}
			return list;
		}
		public override DbQueryResult GetMyReferralMembers(MemberQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("ReferralUserId = {0}", HiContext.Current.User.UserId);
			if (query.GradeId.HasValue)
			{
				stringBuilder.AppendFormat(" AND GradeId = {0}", query.GradeId.Value);
			}
			if (!string.IsNullOrEmpty(query.Username))
			{
				stringBuilder.AppendFormat(" AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Username));
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_aspnet_Members", "UserId", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*");
		}
		private static string BuildBalanceDetailsQuery(BalanceDetailQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (query.UserId.HasValue)
			{
				stringBuilder.AppendFormat(" AND UserId = {0}", query.UserId.Value);
			}
			if (!string.IsNullOrEmpty(query.UserName))
			{
				stringBuilder.AppendFormat(" AND UserId IN (SELECT UserId FROM vw_aspnet_Members WHERE UserName='{0}')", DataHelper.CleanSearchString(query.UserName));
			}
			if (query.FromDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND TradeDate >= '{0}'", DataHelper.GetSafeDateTimeFormat(query.FromDate.Value));
			}
			if (query.ToDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND TradeDate <= '{0}'", DataHelper.GetSafeDateTimeFormat(query.ToDate.Value));
			}
			if (query.TradeType != TradeTypes.NotSet)
			{
				stringBuilder.AppendFormat(" AND TradeType = {0}", (int)query.TradeType);
			}
			return stringBuilder.ToString();
		}
	}
}
