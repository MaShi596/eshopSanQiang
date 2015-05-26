using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Members;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
namespace Hidistro.Subsites.Data
{
	public class UnderlingData : UnderlingProvider
	{
		private Database database;
		public UnderlingData()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public override DbQueryResult GetMembers(MemberQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("ParentUserId={0}", HiContext.Current.User.UserId);
			if (query.GradeId.HasValue)
			{
				stringBuilder.AppendFormat("AND GradeId = {0}", query.GradeId.Value);
			}
			if (query.IsApproved.HasValue)
			{
				stringBuilder.AppendFormat("AND IsApproved = '{0}'", query.IsApproved.Value);
			}
			if (!string.IsNullOrEmpty(query.Username))
			{
				stringBuilder.AppendFormat("AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Username));
			}
			if (!string.IsNullOrEmpty(query.Realname))
			{
				stringBuilder.AppendFormat(" AND RealName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Realname));
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_distro_Members", "UserId", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*");
		}
		public override System.Data.DataTable GetMembersNopage(MemberQuery query, IList<string> fields)
		{
			System.Data.DataTable result;
			if (fields.Count == 0)
			{
				result = null;
			}
			else
			{
				System.Data.DataTable dataTable = null;
				string text = string.Empty;
				foreach (string current in fields)
				{
					text = text + current + ",";
				}
				text = text.Substring(0, text.Length - 1);
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("SELECT {0} FROM vw_distro_Members WHERE ParentUserId={1} ", text, HiContext.Current.User.UserId);
				if (!string.IsNullOrEmpty(query.Username))
				{
					stringBuilder.AppendFormat(" AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Username));
				}
				if (!string.IsNullOrEmpty(query.Realname))
				{
					stringBuilder.AppendFormat(" AND RealName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Realname));
				}
				if (query.GradeId.HasValue)
				{
					stringBuilder.AppendFormat(" AND GradeId={0}", query.GradeId);
				}
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
					dataReader.Close();
				}
				result = dataTable;
			}
			return result;
		}
		public override bool DeleteMember(int userId)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("sub_Member_Delete");
			this.database.AddInParameter(storedProcCommand, "UserId", System.Data.DbType.Int32, userId);
			Member member = Users.GetUser(userId) as Member;
			this.database.AddInParameter(storedProcCommand, "UserName", System.Data.DbType.String, member.Username);
			this.database.AddParameter(storedProcCommand, "ReturnValue", System.Data.DbType.Int32, System.Data.ParameterDirection.ReturnValue, string.Empty, System.Data.DataRowVersion.Default, null);
			this.database.ExecuteNonQuery(storedProcCommand);
			object parameterValue = this.database.GetParameterValue(storedProcCommand, "ReturnValue");
			return parameterValue != null && parameterValue != DBNull.Value && Convert.ToInt32(parameterValue) == 0;
		}
		public override IList<UserStatisticsForDate> GetUserIncrease(int? year, int? month, int? days)
		{
			IList<UserStatisticsForDate> list = new List<UserStatisticsForDate>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT (SELECT COUNT(*) FROM vw_distro_Members WHERE ParentUserId = @ParentUserId AND CreateDate >=@StartDate AND  CreateDate<= @EndDate) AS UserIncrease;");
			this.database.AddInParameter(sqlStringCommand, "ParentUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "StartDate", System.Data.DbType.DateTime);
			this.database.AddInParameter(sqlStringCommand, "EndDate", System.Data.DbType.DateTime);
			DateTime date = default(DateTime);
			DateTime dateTime = default(DateTime);
			if (days.HasValue)
			{
				date = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")).AddDays(1.0).AddDays((double)(-(double)days.Value));
			}
			else
			{
				if (year.HasValue && month.HasValue)
				{
					date = new DateTime(year.Value, month.Value, 1);
				}
				else
				{
					if (year.HasValue && !month.HasValue)
					{
						date = new DateTime(year.Value, 1, 1);
					}
				}
			}
			if (days.HasValue)
			{
				for (int i = 1; i <= days; i++)
				{
					UserStatisticsForDate userStatisticsForDate = new UserStatisticsForDate();
					if (i > 1)
					{
						date = dateTime;
					}
					dateTime = date.AddDays(1.0);
					this.database.SetParameterValue(sqlStringCommand, "@StartDate", DataHelper.GetSafeDateTimeFormat(date));
					this.database.SetParameterValue(sqlStringCommand, "@EndDate", DataHelper.GetSafeDateTimeFormat(dateTime));
					userStatisticsForDate.UserCounts = (int)this.database.ExecuteScalar(sqlStringCommand);
					userStatisticsForDate.TimePoint = date.Day;
					list.Add(userStatisticsForDate);
				}
			}
			else
			{
				if (year.HasValue && month.HasValue)
				{
					int num = DateTime.DaysInMonth(year.Value, month.Value);
					for (int i = 1; i <= num; i++)
					{
						UserStatisticsForDate userStatisticsForDate = new UserStatisticsForDate();
						if (i > 1)
						{
							date = dateTime;
						}
						dateTime = date.AddDays(1.0);
						this.database.SetParameterValue(sqlStringCommand, "@StartDate", DataHelper.GetSafeDateTimeFormat(date));
						this.database.SetParameterValue(sqlStringCommand, "@EndDate", DataHelper.GetSafeDateTimeFormat(dateTime));
						userStatisticsForDate.UserCounts = (int)this.database.ExecuteScalar(sqlStringCommand);
						userStatisticsForDate.TimePoint = i;
						list.Add(userStatisticsForDate);
					}
				}
				else
				{
					if (year.HasValue && !month.HasValue)
					{
						int num2 = 12;
						for (int i = 1; i <= num2; i++)
						{
							UserStatisticsForDate userStatisticsForDate = new UserStatisticsForDate();
							if (i > 1)
							{
								date = dateTime;
							}
							dateTime = date.AddMonths(1);
							this.database.SetParameterValue(sqlStringCommand, "@StartDate", DataHelper.GetSafeDateTimeFormat(date));
							this.database.SetParameterValue(sqlStringCommand, "@EndDate", DataHelper.GetSafeDateTimeFormat(dateTime));
							userStatisticsForDate.UserCounts = (int)this.database.ExecuteScalar(sqlStringCommand);
							userStatisticsForDate.TimePoint = i;
							list.Add(userStatisticsForDate);
						}
					}
				}
			}
			return list;
		}
		public override System.Data.DataTable GetUnderlingStatistics(SaleStatisticsQuery query, out int total)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_MemberStatistics_Get");
			this.database.AddInParameter(storedProcCommand, "PageIndex", System.Data.DbType.Int32, query.PageIndex);
			this.database.AddInParameter(storedProcCommand, "PageSize", System.Data.DbType.Int32, query.PageSize);
			this.database.AddInParameter(storedProcCommand, "IsCount", System.Data.DbType.Boolean, query.IsCount);
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", System.Data.DbType.String, UnderlingData.BuildUnderlingStatisticsQuery(query));
			this.database.AddOutParameter(storedProcCommand, "TotalProductSales", System.Data.DbType.Int32, 4);
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			total = (int)this.database.GetParameterValue(storedProcCommand, "TotalProductSales");
			return result;
		}
		public override System.Data.DataTable GetUnderlingStatisticsNoPage(SaleStatisticsQuery query)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(UnderlingData.BuildUnderlingStatisticsQuery(query));
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override IList<MemberGradeInfo> GetUnderlingGrades()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT * FROM distro_MemberGrades WHERE CreateUserId = {0} ORDER BY GradeId DESC;", HiContext.Current.User.UserId));
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
		public override MemberGradeInfo GetMemberGrade(int gradeId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_MemberGrades WHERE GradeId = @GradeId");
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
		public override bool CreateUnderlingGrade(MemberGradeInfo underlingGrade)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (underlingGrade.IsDefault)
			{
				stringBuilder.AppendFormat("UPDATE distro_MemberGrades SET IsDefault = 0 WHERE CreateUserId = {0};", HiContext.Current.User.UserId);
			}
			stringBuilder.AppendFormat("INSERT INTO distro_MemberGrades(CreateUserId, [Name], Description, Points, IsDefault, Discount) VALUES (@CreateUserId, @Name, @Description, @Points, @IsDefault, @Discount);", new object[0]);
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, underlingGrade.Name);
			this.database.AddInParameter(sqlStringCommand, "Description", System.Data.DbType.String, underlingGrade.Description);
			this.database.AddInParameter(sqlStringCommand, "Points", System.Data.DbType.Int32, underlingGrade.Points);
			this.database.AddInParameter(sqlStringCommand, "IsDefault", System.Data.DbType.Boolean, underlingGrade.IsDefault);
			this.database.AddInParameter(sqlStringCommand, "Discount", System.Data.DbType.Currency, underlingGrade.Discount);
			this.database.AddInParameter(sqlStringCommand, "CreateUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool UpdateUnderlingGrade(MemberGradeInfo underlingGrade)
		{
			string text = "UPDATE distro_MemberGrades SET [Name] = @Name, Description = @Description, Points = @Points, Discount = @Discount WHERE GradeId = @GradeId;";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, underlingGrade.Name);
			this.database.AddInParameter(sqlStringCommand, "Description", System.Data.DbType.String, underlingGrade.Description);
			this.database.AddInParameter(sqlStringCommand, "Points", System.Data.DbType.Int32, underlingGrade.Points);
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, underlingGrade.GradeId);
			this.database.AddInParameter(sqlStringCommand, "Discount", System.Data.DbType.Currency, underlingGrade.Discount);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool HasSamePointMemberGrade(MemberGradeInfo memberGrade)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT COUNT(GradeId) as Count FROM distro_MemberGrades WHERE Points=@Points AND CreateUserId = {0} AND GradeId<>@GradeId;", HiContext.Current.User.UserId));
			this.database.AddInParameter(sqlStringCommand, "Points", System.Data.DbType.Int32, memberGrade.Points);
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, memberGrade.GradeId);
			return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
		}
		public override bool DeleteUnderlingGrade(int gradeId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_MemberGrades WHERE GradeId = @GradeId AND IsDefault = 0 AND CreateUserId=@CreateUserId AND NOT EXISTS(SELECT * FROM distro_Members WHERE GradeId = @GradeId AND ParentUserId=@ParentUserId)");
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, gradeId);
			this.database.AddInParameter(sqlStringCommand, "DistributoruserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "CreateUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "ParentUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override void DeleteSKUMemberPrice(int gradeId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_SKUMemberPrice WHERE GradeId = @GradeId AND DistributoruserId=@DistributoruserId");
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, gradeId);
			this.database.AddInParameter(sqlStringCommand, "DistributoruserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override void SetDefalutUnderlingGrade(int gradeId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("IF EXISTS(SELECT * FROM distro_MemberGrades WHERE IsDefault=0 AND GradeId = {0}) ", gradeId);
			stringBuilder.Append("BEGIN ");
			stringBuilder.AppendFormat("UPDATE distro_MemberGrades SET IsDefault = 0 WHERE CreateUserId = {0};", HiContext.Current.User.UserId);
			stringBuilder.AppendFormat("UPDATE distro_MemberGrades SET IsDefault = 1 WHERE GradeId = {0} AND CreateUserId = {1}; ", gradeId, HiContext.Current.User.UserId);
			stringBuilder.Append("END");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override bool AddUnderlingBalanceDetail(BalanceDetailInfo balanceDetails)
		{
			bool result;
			if (null == balanceDetails)
			{
				result = false;
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO distro_BalanceDetails (UserId,UserName, DistributorUserId, TradeDate, TradeType, Income, Expenses, Balance, Remark) VALUES(@UserId,@UserName, @DistributorUserId, @TradeDate, @TradeType, @Income, @Expenses, @Balance, @Remark)");
				this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, balanceDetails.UserId);
				this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, balanceDetails.UserName);
				this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
				this.database.AddInParameter(sqlStringCommand, "TradeDate", System.Data.DbType.DateTime, balanceDetails.TradeDate);
				this.database.AddInParameter(sqlStringCommand, "TradeType", System.Data.DbType.Int32, (int)balanceDetails.TradeType);
				this.database.AddInParameter(sqlStringCommand, "Income", System.Data.DbType.Currency, balanceDetails.Income);
				this.database.AddInParameter(sqlStringCommand, "Expenses", System.Data.DbType.Currency, balanceDetails.Expenses);
				this.database.AddInParameter(sqlStringCommand, "Balance", System.Data.DbType.Currency, balanceDetails.Balance);
				this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, balanceDetails.Remark);
				result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
			}
			return result;
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
				string text = UnderlingData.BuildBalanceDetailsQuery(query);
				stringBuilder.AppendFormat("SELECT TOP {0} * FROM distro_BalanceDetails B WHERE 0=0", query.PageSize);
				if (query.PageIndex == 1)
				{
					stringBuilder.AppendFormat("{0} ORDER BY JournalNumber DESC;", text);
				}
				else
				{
					stringBuilder.AppendFormat("AND JournalNumber < (SELECT MIN(JournalNumber) FROM (SELECT TOP {0} JournalNumber FROM distro_BalanceDetails WHERE 0=0 {1} ORDER BY JournalNumber DESC ) AS T) {1} ORDER BY JournalNumber DESC;", (query.PageIndex - 1) * query.PageSize, text);
				}
				if (query.IsCount)
				{
					stringBuilder.AppendFormat("SELECT COUNT(JournalNumber) AS Total from distro_BalanceDetails WHERE 0=0 {0}", text);
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
		public override DbQueryResult GetUnderlingBlanceList(MemberQuery query)
		{
			DbQueryResult result;
			if (null == query)
			{
				result = new DbQueryResult();
			}
			else
			{
				DbQueryResult dbQueryResult = new DbQueryResult();
				StringBuilder stringBuilder = new StringBuilder();
				string text = string.Format(" AND ParentUserId={0}", HiContext.Current.User.UserId);
				if (!string.IsNullOrEmpty(query.Username))
				{
					text += string.Format(" AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Username));
				}
				if (!string.IsNullOrEmpty(query.Realname))
				{
					text += string.Format(" AND RealName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Realname));
				}
				stringBuilder.AppendFormat("SELECT TOP {0} * FROM vw_distro_Members WHERE 0=0", query.PageSize);
				if (query.PageIndex == 1)
				{
					stringBuilder.AppendFormat(" {0} ORDER BY CreateDate DESC", text);
				}
				else
				{
					stringBuilder.AppendFormat(" AND CreateDate < (select min(CreateDate) FROM (SELECT TOP {0} CreateDate FROM vw_distro_Members WHERE 0=0 {1} ORDER BY CreateDate DESC ) AS tbltemp) {1} ORDER BY CreateDate DESC", (query.PageIndex - 1) * query.PageSize, text);
				}
				if (query.IsCount)
				{
					stringBuilder.AppendFormat(";SELECT COUNT(CreateDate) AS Total FROM vw_distro_Members WHERE 0=0 {0}", text);
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
		public override DbQueryResult GetBalanceDrawRequests(BalanceDrawRequestQuery query)
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
				string text = UnderlingData.BuildBalanceDrawRequestQuery(query);
				stringBuilder.AppendFormat("SELECT Top {0} * FROM distro_BalanceDrawRequest B WHERE 0=0", query.PageSize);
				if (query.PageIndex == 1)
				{
					stringBuilder.AppendFormat("{0} ORDER BY RequestTime DESC;", text);
				}
				else
				{
					stringBuilder.AppendFormat(" AND RequestTime < (SELECT MIN(RequestTime) FROM (SELECT TOP {0} RequestTime FROM distro_BalanceDrawRequest WHERE 0=0 {1} ORDER BY RequestTime DESC ) as T) {1} ORDER BY RequestTime DESC;", (query.PageIndex - 1) * query.PageSize, text);
				}
				if (query.IsCount)
				{
					stringBuilder.AppendFormat("SELECT COUNT(*) AS Total from distro_BalanceDrawRequest WHERE 0=0 {0}", text);
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
		public override bool DealBalanceDrawRequest(int userId, bool agree)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("sub_UnderlingBalanceDrawRequest_Update");
			this.database.AddOutParameter(storedProcCommand, "Status", System.Data.DbType.Int32, 4);
			this.database.AddInParameter(storedProcCommand, "UserId", System.Data.DbType.Int32, userId);
			this.database.AddInParameter(storedProcCommand, "Agree", System.Data.DbType.Boolean, agree);
			this.database.ExecuteNonQuery(storedProcCommand);
			object parameterValue = this.database.GetParameterValue(storedProcCommand, "Status");
			return parameterValue != DBNull.Value && parameterValue != null && (int)this.database.GetParameterValue(storedProcCommand, "Status") == 0;
		}
		private static string BuildUnderlingStatisticsQuery(SaleStatisticsQuery query)
		{
			if (null == query)
			{
				throw new ArgumentNullException("query");
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT UserId, UserName ");
			if (query.StartDate.HasValue || query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(",  ( select isnull(SUM(OrderTotal),0) from distro_Orders where OrderStatus != {0} AND OrderStatus != {1} AND DistributorUserId={2}", 4, 1, HiContext.Current.User.UserId);
				if (query.StartDate.HasValue)
				{
					stringBuilder.AppendFormat(" and OrderDate>='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
				}
				if (query.EndDate.HasValue)
				{
					stringBuilder.AppendFormat(" and OrderDate<='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
				}
				stringBuilder.Append(" and userId = vw_distro_Members.UserId) as SaleTotals");
				stringBuilder.AppendFormat(",(select Count(OrderId) from distro_Orders where OrderStatus != {0} AND OrderStatus != {1} AND DistributorUserId={2}", 4, 1, HiContext.Current.User.UserId);
				if (query.StartDate.HasValue)
				{
					stringBuilder.AppendFormat(" and OrderDate>='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
				}
				if (query.EndDate.HasValue)
				{
					stringBuilder.AppendFormat(" and OrderDate<='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
				}
				stringBuilder.Append(" and userId = vw_distro_Members.UserId) as OrderCount ");
			}
			else
			{
				stringBuilder.Append(",ISNULL(Expenditure,0) as SaleTotals,ISNULL(OrderNumber,0) as OrderCount ");
			}
			stringBuilder.AppendFormat(" from vw_distro_Members WHERE ParentUserId={0} AND Expenditure > 0", HiContext.Current.User.UserId);
			if (!string.IsNullOrEmpty(query.SortBy))
			{
				stringBuilder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
			}
			return stringBuilder.ToString();
		}
		private static string BuildBalanceDetailsQuery(BalanceDetailQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" AND DistributorUserId = {0}", HiContext.Current.User.UserId);
			if (query.UserId.HasValue)
			{
				stringBuilder.AppendFormat(" AND UserId = {0}", query.UserId.Value);
			}
			if (!string.IsNullOrEmpty(query.UserName))
			{
				stringBuilder.AppendFormat(" AND UserName='{0}'", DataHelper.CleanSearchString(query.UserName));
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
		private static string BuildBalanceDrawRequestQuery(BalanceDrawRequestQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" AND DistributorUserId = {0}", HiContext.Current.User.UserId);
			if (query.UserId.HasValue)
			{
				stringBuilder.AppendFormat(" AND UserId = {0}", query.UserId.Value);
			}
			if (!string.IsNullOrEmpty(query.UserName))
			{
				stringBuilder.AppendFormat(" AND UserId IN (SELECT UserId FROM vw_distro_Members WHERE UserName='{0}')", DataHelper.CleanSearchString(query.UserName));
			}
			if (query.FromDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND RequestTime >= '{0}'", DataHelper.GetSafeDateTimeFormat(query.FromDate.Value));
			}
			if (query.ToDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND RequestTime <= '{0}'", DataHelper.GetSafeDateTimeFormat(query.ToDate.Value));
			}
			return stringBuilder.ToString();
		}
	}
}
