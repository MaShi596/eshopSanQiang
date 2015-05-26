using Hidistro.ControlPanel.Members;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Membership.Context;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
namespace Hidistro.ControlPanel.Data
{
	public class MemberData : MemberProvider
	{
		private Database database;
		public MemberData()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public override DbQueryResult GetMembers(MemberQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (query.GradeId.HasValue)
			{
				stringBuilder.AppendFormat("GradeId = {0}", query.GradeId.Value);
			}
			if (query.IsApproved.HasValue)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("IsApproved = '{0}'", query.IsApproved.Value);
			}
			if (!string.IsNullOrEmpty(query.Username))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Username));
			}
			if (!string.IsNullOrEmpty(query.Realname))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendFormat(" AND ", new object[0]);
				}
				stringBuilder.AppendFormat("RealName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Realname));
			}
			if (!string.IsNullOrEmpty(query.ClientType))
			{
				string clientType = query.ClientType;
				string text;
				object obj;
				if (clientType != null)
				{
					if (clientType == "new")
					{
						text = "SElECT UserId FROM aspnet_Users WHERE 1=1";
						if (query.StartTime.HasValue)
						{
							obj = text;
							text = string.Concat(new object[]
							{
								obj,
								" AND datediff(dd,CreateDate,'",
								query.StartTime.Value.Date,
								"')<=0"
							});
						}
						if (query.EndTime.HasValue)
						{
							obj = text;
							text = string.Concat(new object[]
							{
								obj,
								" AND datediff(dd,CreateDate,'",
								query.EndTime.Value.Date,
								"')>=0"
							});
						}
						if (stringBuilder.Length > 0)
						{
							stringBuilder.AppendFormat(" AND ", new object[0]);
						}
						stringBuilder.Append("UserId IN (" + text + ")");
						goto IL_560;
					}
					if (clientType == "activy")
					{
						text = "SELECT UserId FROM Hishop_Orders WHERE 1=1";
						if (query.OrderNumber.HasValue)
						{
							obj = text;
							text = string.Concat(new object[]
							{
								obj,
								" AND datediff(dd,OrderDate,'",
								query.StartTime.Value.Date,
								"')<=0"
							});
							obj = text;
							text = string.Concat(new object[]
							{
								obj,
								" AND datediff(dd,OrderDate,'",
								query.EndTime.Value.Date,
								"')>=0"
							});
							obj = text;
							text = string.Concat(new object[]
							{
								obj,
								" GROUP BY UserId HAVING COUNT(*)",
								query.CharSymbol,
								query.OrderNumber.Value
							});
						}
						if (query.OrderMoney.HasValue)
						{
							obj = text;
							text = string.Concat(new object[]
							{
								obj,
								" AND datediff(dd,OrderDate,'",
								query.StartTime.Value.Date,
								"')<=0"
							});
							obj = text;
							text = string.Concat(new object[]
							{
								obj,
								" AND datediff(dd,OrderDate,'",
								query.EndTime.Value.Date,
								"')>=0"
							});
							obj = text;
							text = string.Concat(new object[]
							{
								obj,
								" GROUP BY UserId HAVING SUM(OrderTotal)",
								query.CharSymbol,
								query.OrderMoney.Value
							});
						}
						if (stringBuilder.Length > 0)
						{
							stringBuilder.AppendFormat(" AND ", new object[0]);
						}
						stringBuilder.AppendFormat("UserId IN (" + text + ")", new object[0]);
						goto IL_560;
					}
				}
				text = "SELECT UserId FROM Hishop_Orders WHERE 1=1";
				obj = text;
				text = string.Concat(new object[]
				{
					obj,
					" AND datediff(dd,OrderDate,'",
					query.StartTime.Value.Date,
					"')<=0"
				});
				obj = text;
				text = string.Concat(new object[]
				{
					obj,
					" AND datediff(dd,OrderDate,'",
					query.EndTime.Value.Date,
					"')>=0"
				});
				text += " GROUP BY UserId";
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendFormat(" AND ", new object[0]);
				}
				stringBuilder.AppendFormat("UserId NOT IN (" + text + ")", new object[0]);
			}
			IL_560:
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_aspnet_Members", "UserId", (stringBuilder.Length > 0) ? stringBuilder.ToString() : null, "*");
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
				stringBuilder.AppendFormat("SELECT {0} FROM vw_aspnet_Members WHERE 1=1 ", text);
				if (!string.IsNullOrEmpty(query.Username))
				{
					stringBuilder.AppendFormat(" AND UserName LIKE '%{0}%'", query.Username);
				}
				if (query.GradeId.HasValue)
				{
					stringBuilder.AppendFormat(" AND GradeId={0}", query.GradeId);
				}
				if (!string.IsNullOrEmpty(query.Realname))
				{
					stringBuilder.AppendFormat(" AND Realname LIKE '%{0}%'", query.Realname);
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
		public override bool Delete(int userId)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Member_Delete");
			Member member = Users.GetUser(userId) as Member;
			this.database.AddInParameter(storedProcCommand, "UserId", System.Data.DbType.Int32, userId);
			this.database.AddInParameter(storedProcCommand, "UserName", System.Data.DbType.String, member.Username);
			this.database.AddParameter(storedProcCommand, "ReturnValue", System.Data.DbType.Int32, System.Data.ParameterDirection.ReturnValue, string.Empty, System.Data.DataRowVersion.Default, null);
			this.database.ExecuteNonQuery(storedProcCommand);
			object parameterValue = this.database.GetParameterValue(storedProcCommand, "ReturnValue");
			return parameterValue != null && parameterValue != DBNull.Value && Convert.ToInt32(parameterValue) == 0;
		}
		public override IList<MemberGradeInfo> GetMemberGrades()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_MemberGrades");
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
		public override bool HasSamePointMemberGrade(MemberGradeInfo memberGrade)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(GradeId) as Count FROM aspnet_MemberGrades WHERE Points=@Points AND GradeId<>@GradeId;");
			this.database.AddInParameter(sqlStringCommand, "Points", System.Data.DbType.Int32, memberGrade.Points);
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, memberGrade.GradeId);
			return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
		}
		public override bool CreateMemberGrade(MemberGradeInfo memberGrade)
		{
			string text = string.Empty;
			if (memberGrade.IsDefault)
			{
				text += "UPDATE aspnet_MemberGrades SET IsDefault = 0";
			}
			text += " INSERT INTO aspnet_MemberGrades ([Name], Description, Points, IsDefault, Discount) VALUES (@Name, @Description, @Points, @IsDefault, @Discount)";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, memberGrade.Name);
			this.database.AddInParameter(sqlStringCommand, "Description", System.Data.DbType.String, memberGrade.Description);
			this.database.AddInParameter(sqlStringCommand, "Points", System.Data.DbType.Int32, memberGrade.Points);
			this.database.AddInParameter(sqlStringCommand, "IsDefault", System.Data.DbType.Boolean, memberGrade.IsDefault);
			this.database.AddInParameter(sqlStringCommand, "Discount", System.Data.DbType.Int32, memberGrade.Discount);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool DeleteMemberGrade(int gradeId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM aspnet_MemberGrades WHERE GradeId = @GradeId AND IsDefault = 0 AND NOT EXISTS(SELECT * FROM aspnet_Members WHERE GradeId = @GradeId)");
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, gradeId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool UpdateMemberGrade(MemberGradeInfo memberGrade)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_MemberGrades SET [Name] = @Name, Description = @Description, Points = @Points, Discount = @Discount WHERE GradeId = @GradeId");
			this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, memberGrade.Name);
			this.database.AddInParameter(sqlStringCommand, "Description", System.Data.DbType.String, memberGrade.Description);
			this.database.AddInParameter(sqlStringCommand, "Points", System.Data.DbType.Int32, memberGrade.Points);
			this.database.AddInParameter(sqlStringCommand, "Discount", System.Data.DbType.Int32, memberGrade.Discount);
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, memberGrade.GradeId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override void SetDefalutMemberGrade(int gradeId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_MemberGrades SET IsDefault = 0;UPDATE aspnet_MemberGrades SET IsDefault = 1 WHERE GradeId = @GradeId");
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, gradeId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override bool InsertBalanceDetail(BalanceDetailInfo balanceDetails)
		{
			bool result;
			if (null == balanceDetails)
			{
				result = false;
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_BalanceDetails (UserId, UserName, TradeDate, TradeType, Income, Expenses, Balance, Remark) VALUES(@UserId, @UserName, @TradeDate, @TradeType, @Income, @Expenses, @Balance, @Remark);");
				this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, balanceDetails.UserId);
				this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, balanceDetails.UserName);
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
				result = new DbQueryResult();
			}
			else
			{
				DbQueryResult dbQueryResult = new DbQueryResult();
				StringBuilder stringBuilder = new StringBuilder();
				string text = MemberData.BuildBalanceDetailsQuery(query);
				stringBuilder.AppendFormat("SELECT TOP {0} *", query.PageSize);
				stringBuilder.Append(" FROM Hishop_BalanceDetails B where 0=0 ");
				if (query.PageIndex == 1)
				{
					stringBuilder.AppendFormat("{0} ORDER BY JournalNumber DESC", text);
				}
				else
				{
					stringBuilder.AppendFormat(" and JournalNumber < (select min(JournalNumber) from (select top {0} JournalNumber from Hishop_BalanceDetails where 0=0 {1} ORDER BY JournalNumber DESC ) as tbltemp) {1} ORDER BY JournalNumber DESC", (query.PageIndex - 1) * query.PageSize, text);
				}
				if (query.IsCount)
				{
					stringBuilder.AppendFormat(";select count(JournalNumber) as Total from Hishop_BalanceDetails where 0=0 {0}", text);
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
		public override DbQueryResult GetMemberBlanceList(MemberQuery query)
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
				string text = string.Empty;
				if (!string.IsNullOrEmpty(query.Username))
				{
					text = string.Format("AND UserId IN (SELECT UserId FROM vw_aspnet_Members WHERE UserName LIKE '%{0}%')", DataHelper.CleanSearchString(query.Username));
				}
				if (!string.IsNullOrEmpty(query.Realname))
				{
					text += string.Format(" AND RealName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Realname));
				}
				stringBuilder.AppendFormat("SELECT TOP {0} *", query.PageSize);
				stringBuilder.Append(" FROM vw_aspnet_Members WHERE 0=0");
				if (query.PageIndex == 1)
				{
					stringBuilder.AppendFormat("{0} ORDER BY CreateDate DESC", text);
				}
				else
				{
					stringBuilder.AppendFormat("AND CreateDate < (select min(CreateDate) FROM (SELECT TOP {0} CreateDate FROM vw_aspnet_Members WHERE 0=0 {1} ORDER BY CreateDate DESC ) AS tbltemp) {1} ORDER BY CreateDate DESC", (query.PageIndex - 1) * query.PageSize, text);
				}
				if (query.IsCount)
				{
					stringBuilder.AppendFormat(";SELECT COUNT(CreateDate) AS Total FROM vw_aspnet_Members WHERE 0=0 {0}", text);
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
		public override DbQueryResult GetBalanceDetailsNoPage(BalanceDetailQuery query)
		{
			DbQueryResult dbQueryResult = new DbQueryResult();
			StringBuilder stringBuilder = new StringBuilder();
			string arg = MemberData.BuildBalanceDetailsQuery(query);
			stringBuilder.Append("SELECT * FROM Hishop_BalanceDetails WHERE 0=0 ");
			stringBuilder.AppendFormat("{0} ORDER BY JournalNumber DESC", arg);
			if (query.IsCount)
			{
				stringBuilder.AppendFormat(";select count(JournalNumber) as Total from Hishop_BalanceDetails where 0=0 {0}", arg);
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
			return dbQueryResult;
		}
		public override DbQueryResult GetBalanceDrawRequests(BalanceDrawRequestQuery query)
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
				string text = MemberData.BuildBalanceDrawRequestQuery(query);
				stringBuilder.AppendFormat("select top {0} *", query.PageSize);
				stringBuilder.Append(" from Hishop_BalanceDrawRequest B where 0=0 ");
				if (query.PageIndex == 1)
				{
					stringBuilder.AppendFormat("{0} ORDER BY RequestTime DESC", text);
				}
				else
				{
					stringBuilder.AppendFormat(" and RequestTime < (select min(RequestTime) from (select top {0} RequestTime from Hishop_BalanceDrawRequest where 0=0 {1} ORDER BY RequestTime DESC ) as tbltemp) {1} ORDER BY RequestTime DESC", (query.PageIndex - 1) * query.PageSize, text);
				}
				if (query.IsCount)
				{
					stringBuilder.AppendFormat(";select count(*) as Total from Hishop_BalanceDrawRequest where 0=0 {0}", text);
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
		public override DbQueryResult GetBalanceDrawRequestsNoPage(BalanceDrawRequestQuery query)
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
				string arg = MemberData.BuildBalanceDrawRequestQuery(query);
				stringBuilder.Append("select *");
				stringBuilder.Append(" from Hishop_BalanceDrawRequest B where 0=0 ");
				stringBuilder.AppendFormat("{0} ORDER BY RequestTime DESC", arg);
				if (query.IsCount)
				{
					stringBuilder.AppendFormat(";select count(*) as Total from Hishop_BalanceDrawRequest where 0=0 {0}", arg);
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
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_BalanceDrawRequest_Update");
			this.database.AddOutParameter(storedProcCommand, "Status", System.Data.DbType.Int32, 4);
			this.database.AddInParameter(storedProcCommand, "UserId", System.Data.DbType.Int32, userId);
			this.database.AddInParameter(storedProcCommand, "Agree", System.Data.DbType.Boolean, agree);
			this.database.ExecuteNonQuery(storedProcCommand);
			object parameterValue = this.database.GetParameterValue(storedProcCommand, "Status");
			return parameterValue != DBNull.Value && parameterValue != null && (int)this.database.GetParameterValue(storedProcCommand, "Status") == 0;
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
				stringBuilder.AppendFormat(" AND RequestTime >= '{0}'", DataHelper.GetSafeDateTimeFormat(query.FromDate.Value));
			}
			if (query.ToDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND RequestTime <= '{0}'", DataHelper.GetSafeDateTimeFormat(query.ToDate.Value));
			}
			return stringBuilder.ToString();
		}
		public override bool InsertClientSet(Dictionary<int, MemberClientSet> clientsets)
		{
			StringBuilder stringBuilder = new StringBuilder("DELETE FROM  [Hishop_MemberClientSet];");
			foreach (KeyValuePair<int, MemberClientSet> current in clientsets)
			{
				string text = "";
				string text2 = "";
				if (current.Value.StartTime.HasValue)
				{
					text = current.Value.StartTime.Value.ToString("yyyy-MM-dd");
				}
				if (current.Value.EndTime.HasValue)
				{
					text2 = current.Value.EndTime.Value.ToString("yyyy-MM-dd");
				}
				stringBuilder.AppendFormat(string.Concat(new object[]
				{
					"INSERT INTO Hishop_MemberClientSet(ClientTypeId,StartTime,EndTime,LastDay,ClientChar,ClientValue) VALUES (",
					current.Key,
					",'",
					text,
					"','",
					text2,
					"',",
					current.Value.LastDay,
					",'",
					current.Value.ClientChar,
					"',",
					current.Value.ClientValue,
					");"
				}), new object[0]);
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override Dictionary<int, MemberClientSet> GetMemberClientSet()
		{
			Dictionary<int, MemberClientSet> dictionary = new Dictionary<int, MemberClientSet>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_MemberClientSet");
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					MemberClientSet memberClientSet = DataMapper.PopulateMemberClientSet(dataReader);
					dictionary.Add(memberClientSet.ClientTypeId, memberClientSet);
				}
			}
			return dictionary;
		}
	}
}
