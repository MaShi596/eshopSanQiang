using Hidistro.ControlPanel.Distribution;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Distribution;
using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
namespace Hidistro.ControlPanel.Data
{
	public class DistributionData : DistributorProvider
	{
		private Database database;
		public DistributionData()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public override bool AddDistributorGrade(DistributorGradeInfo distributorGrade)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO aspnet_DistributorGrades(Name, Description, Discount) VALUES(@Name,@Description,@Discount)");
			this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, distributorGrade.Name);
			this.database.AddInParameter(sqlStringCommand, "Description", System.Data.DbType.String, distributorGrade.Description);
			this.database.AddInParameter(sqlStringCommand, "Discount", System.Data.DbType.Int32, distributorGrade.Discount);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override bool ExistGradeName(string gradeName)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM aspnet_DistributorGrades WHERE Name=@Name");
			this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, gradeName);
			return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
		}
		public override bool UpdateDistributorGrade(DistributorGradeInfo distributorGrade)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_DistributorGrades SET Name =@Name,Description = @Description,Discount = @Discount WHERE GradeId = @GradeId");
			this.database.AddInParameter(sqlStringCommand, "Name", System.Data.DbType.String, distributorGrade.Name);
			this.database.AddInParameter(sqlStringCommand, "Description", System.Data.DbType.String, distributorGrade.Description);
			this.database.AddInParameter(sqlStringCommand, "Discount", System.Data.DbType.Int32, distributorGrade.Discount);
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Currency, distributorGrade.GradeId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override bool DeleteDistributorGrade(int gradeId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM aspnet_DistributorGrades  WHERE GradeId = @GradeId AND not exists (select GradeId from dbo.aspnet_Distributors where GradeId=@GradeId)");
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Currency, gradeId);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool ExistDistributor(int gradeId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM aspnet_Distributors  WHERE GradeId = @GradeId");
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Currency, gradeId);
			return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
		}
		public override System.Data.DataTable GetDistributorGrades()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_DistributorGrades");
			System.Data.DataTable result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
				dataReader.Close();
			}
			return result;
		}
		public override DistributorGradeInfo GetDistributorGradeInfo(int gradeId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_DistributorGrades WHERE GradeId = @GradeId");
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Currency, gradeId);
			DistributorGradeInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulDistributorGrade(dataReader);
				}
			}
			return result;
		}
		public override DbQueryResult GetDistributors(DistributorQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("IsApproved = {0}", query.IsApproved ? 1 : 0);
			if (!string.IsNullOrEmpty(query.Username))
			{
				stringBuilder.AppendFormat(" AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Username));
			}
			if (!string.IsNullOrEmpty(query.RealName))
			{
				stringBuilder.AppendFormat(" AND RealName LIKE '%{0}%'", DataHelper.CleanSearchString(query.RealName));
			}
			if (query.GradeId.HasValue)
			{
				stringBuilder.AppendFormat(" AND GradeId = {0}", query.GradeId);
			}
			if (query.LineId.HasValue)
			{
				stringBuilder.AppendFormat(" AND UserId IN (SELECT UserId FROM Hishop_DistributorProductLines WHERE LineId={0})", query.LineId.Value);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_aspnet_Distributors", "UserId", stringBuilder.ToString(), "*");
		}
		public override IList<Distributor> GetDistributors()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM vw_aspnet_Distributors");
			IList<Distributor> list = new List<Distributor>();
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(new Distributor
					{
						UserId = (int)dataReader["UserId"],
						Username = (string)dataReader["UserName"]
					});
				}
			}
			return list;
		}
		public override System.Data.DataTable GetDistributorsNopage(DistributorQuery query, IList<string> fields)
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
				stringBuilder.AppendFormat("SELECT {0} FROM vw_aspnet_Distributors WHERE IsApproved=1 ", text);
				if (!string.IsNullOrEmpty(query.Username))
				{
					stringBuilder.AppendFormat(" AND UserName LIKE '%{0}%'", query.Username);
				}
				if (query.GradeId.HasValue)
				{
					stringBuilder.AppendFormat(" AND GradeId={0}", query.GradeId);
				}
				if (!string.IsNullOrEmpty(query.RealName))
				{
					stringBuilder.AppendFormat(" AND RealName LIKE '%{0}%'", query.RealName);
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
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Distribution_Delete");
			this.database.AddInParameter(storedProcCommand, "UserId", System.Data.DbType.Int32, userId);
			this.database.AddParameter(storedProcCommand, "ReturnValue", System.Data.DbType.Int32, System.Data.ParameterDirection.ReturnValue, string.Empty, System.Data.DataRowVersion.Default, null);
			this.database.ExecuteNonQuery(storedProcCommand);
			object parameterValue = this.database.GetParameterValue(storedProcCommand, "ReturnValue");
			return parameterValue != null && parameterValue != DBNull.Value && Convert.ToInt32(parameterValue) == 0;
		}
		public override IList<int> GetDistributorProductLines(int userId)
		{
			IList<int> list = new List<int>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT LineId FROM Hishop_DistributorProductLines WHERE UserId = @UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add((int)dataReader["LineId"]);
				}
			}
			return list;
		}
		public override bool AddDistributorProductLines(int userId, IList<int> lineIds)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("DELETE FROM Hishop_DistributorProductLines WHERE UserId = {0}", userId);
			foreach (int current in lineIds)
			{
				stringBuilder.AppendFormat(" INSERT INTO Hishop_DistributorProductLines(LineId, UserId) VALUES ({0}, {1})", current, userId);
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override DbQueryResult GetDistributorBalanceDetails(BalanceDetailQuery query)
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
				string text = DistributionData.BuildBalanceDetailsQuery(query);
				stringBuilder.AppendFormat("SELECT TOP {0} * FROM Hishop_DistributorBalanceDetails B WHERE 0=0", query.PageSize);
				if (query.PageIndex == 1)
				{
					stringBuilder.AppendFormat(" {0} ORDER BY JournalNumber DESC", text);
				}
				else
				{
					stringBuilder.AppendFormat(" and JournalNumber < (select min(JournalNumber) from (select top {0} JournalNumber from Hishop_DistributorBalanceDetails where 0=0 {1} ORDER BY JournalNumber DESC ) as tbltemp) {1} ORDER BY JournalNumber DESC", (query.PageIndex - 1) * query.PageSize, text);
				}
				if (query.IsCount)
				{
					stringBuilder.AppendFormat(" ;select count(JournalNumber) as Total from Hishop_DistributorBalanceDetails where 0=0 {0}", text);
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
		public override DbQueryResult GetDistributorBalanceDetailsNoPage(BalanceDetailQuery query)
		{
			DbQueryResult dbQueryResult = new DbQueryResult();
			StringBuilder stringBuilder = new StringBuilder();
			string arg = DistributionData.BuildBalanceDetailsQuery(query);
			stringBuilder.Append("SELECT * FROM Hishop_DistributorBalanceDetails WHERE 0=0 ");
			stringBuilder.AppendFormat("{0} ORDER BY JournalNumber DESC", arg);
			if (query.IsCount)
			{
				stringBuilder.AppendFormat(";select count(JournalNumber) as Total from Hishop_DistributorBalanceDetails where 0=0 {0}", arg);
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
		public override DbQueryResult GetDistributorBalanceDrawRequests(BalanceDrawRequestQuery query)
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
				string text = DistributionData.BuildBalanceDrawRequestQuery(query);
				stringBuilder.AppendFormat("select top {0} *", query.PageSize);
				stringBuilder.Append(" from Hishop_DistributorBalanceDrawRequest B where 0=0 ");
				if (query.PageIndex == 1)
				{
					stringBuilder.AppendFormat("{0} ORDER BY RequestTime DESC", text);
				}
				else
				{
					stringBuilder.AppendFormat(" and RequestTime < (select min(RequestTime) from (select top {0} RequestTime from Hishop_DistributorBalanceDrawRequest where 0=0 {1} ORDER BY RequestTime DESC ) as tbltemp) {1} ORDER BY RequestTime DESC", (query.PageIndex - 1) * query.PageSize, text);
				}
				if (query.IsCount)
				{
					stringBuilder.AppendFormat(";select count(*) as Total from Hishop_DistributorBalanceDrawRequest where 0=0 {0}", text);
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
		public override DbQueryResult GetDistributorBalance(DistributorQuery query)
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
					text = string.Format("AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.Username));
				}
				if (!string.IsNullOrEmpty(query.RealName))
				{
					text += string.Format(" AND RealName LIKE '%{0}%'", DataHelper.CleanSearchString(query.RealName));
				}
				stringBuilder.AppendFormat("SELECT TOP {0} *", query.PageSize);
				stringBuilder.Append(" FROM vw_aspnet_Distributors WHERE IsApproved = 1");
				if (query.PageIndex == 1)
				{
					stringBuilder.AppendFormat(" {0} ORDER BY CreateDate DESC", text);
				}
				else
				{
					stringBuilder.AppendFormat(" AND CreateDate < (select min(CreateDate) FROM (SELECT TOP {0} CreateDate FROM vw_aspnet_Distributors WHERE IsApproved = 1 {1} ORDER BY CreateDate DESC ) AS tbltemp) {1} ORDER BY CreateDate DESC", (query.PageIndex - 1) * query.PageSize, text);
				}
				if (query.IsCount)
				{
					stringBuilder.AppendFormat(" ;SELECT COUNT(CreateDate) AS Total FROM vw_aspnet_Distributors WHERE  IsApproved = 1 {0}", text);
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
		public override bool DealDistributorBalanceDrawRequest(int userId, bool agree)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_DistributorBalanceDrawRequest_Update");
			this.database.AddOutParameter(storedProcCommand, "Status", System.Data.DbType.Int32, 4);
			this.database.AddInParameter(storedProcCommand, "UserId", System.Data.DbType.Int32, userId);
			this.database.AddInParameter(storedProcCommand, "Agree", System.Data.DbType.Boolean, agree);
			this.database.ExecuteNonQuery(storedProcCommand);
			object parameterValue = this.database.GetParameterValue(storedProcCommand, "Status");
			return parameterValue != DBNull.Value && parameterValue != null && (int)this.database.GetParameterValue(storedProcCommand, "Status") == 0;
		}
		public override bool InsertBalanceDetail(BalanceDetailInfo balanceDetails, System.Data.Common.DbTransaction dbTran)
		{
			bool result;
			if (null == balanceDetails)
			{
				result = false;
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_DistributorBalanceDetails (UserId,UserName, TradeDate, TradeType, Income, Expenses, Balance, Remark) VALUES(@UserId,@UserName, @TradeDate, @TradeType, @Income, @Expenses, @Balance, @Remark);UPDATE aspnet_Distributors SET Balance = @Balance WHERE UserId = @UserId;");
				this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, balanceDetails.UserId);
				this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, balanceDetails.UserName);
				this.database.AddInParameter(sqlStringCommand, "TradeDate", System.Data.DbType.DateTime, balanceDetails.TradeDate);
				this.database.AddInParameter(sqlStringCommand, "TradeType", System.Data.DbType.Int32, (int)balanceDetails.TradeType);
				this.database.AddInParameter(sqlStringCommand, "Income", System.Data.DbType.Currency, balanceDetails.Income);
				this.database.AddInParameter(sqlStringCommand, "Expenses", System.Data.DbType.Currency, balanceDetails.Expenses);
				this.database.AddInParameter(sqlStringCommand, "Balance", System.Data.DbType.Currency, balanceDetails.Balance);
				this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, balanceDetails.Remark);
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
		private static string BuildBalanceDetailsQuery(BalanceDetailQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (query.UserId.HasValue)
			{
				stringBuilder.AppendFormat(" AND UserId = {0}", query.UserId.Value);
			}
			if (!string.IsNullOrEmpty(query.UserName))
			{
				stringBuilder.AppendFormat(" AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName));
			}
			if (query.TradeType != TradeTypes.NotSet)
			{
				stringBuilder.AppendFormat(" AND TradeType = {0}", (int)query.TradeType);
			}
			if (query.FromDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND TradeDate >= '{0}'", DataHelper.GetSafeDateTimeFormat(query.FromDate.Value));
			}
			if (query.ToDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND TradeDate <= '{0}'", DataHelper.GetSafeDateTimeFormat(query.ToDate.Value));
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
				stringBuilder.AppendFormat(" AND UserId IN (SELECT UserId FROM vw_aspnet_Distributors WHERE UserName='{0}')", DataHelper.CleanSearchString(query.UserName));
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
		public override System.Data.DataTable GetDomainRequests(Pagination pagination, string name, out int total)
		{
			System.Data.DataTable result = null;
			string text = "SELECT COUNT(*) FROM Hishop_SiteRequest LEFT JOIN vw_aspnet_Distributors ON Hishop_SiteRequest.UserId=vw_aspnet_Distributors.UserId WHERE  Hishop_SiteRequest.RequestStatus=@RequestStatus";
			string text2 = string.Empty;
			if (!string.IsNullOrEmpty(name))
			{
				text2 += string.Format(" AND UserName LIKE '%{0}%'", DataHelper.CleanSearchString(name));
			}
			text += text2;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			this.database.AddInParameter(sqlStringCommand, "RequestStatus", System.Data.DbType.Int32, 1);
			total = Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
			string text3 = string.Empty;
			if (pagination.PageIndex == 1)
			{
				text3 = string.Format("SELECT TOP {0} UserName,Wangwang,FirstSiteUrl,RequestTime,Email,RequestStatus,RequestId,Hishop_SiteRequest.UserId FROM Hishop_SiteRequest LEFT JOIN vw_aspnet_Distributors ON Hishop_SiteRequest.UserId=vw_aspnet_Distributors.UserId WHERE  Hishop_SiteRequest.RequestStatus={1} ", pagination.PageSize, 1);
			}
			else
			{
				text3 = string.Format("SELECT TOP {0} UserName,Wangwang,FirstSiteUrl,RequestTime,Email,RequestStatus,RequestId,Hishop_SiteRequest.UserId FROM Hishop_SiteRequest LEFT JOIN vw_aspnet_Distributors ON Hishop_SiteRequest.UserId=vw_aspnet_Distributors.UserId WHERE Hishop_SiteRequest.RequestStatus={2} AND RequestId NOT IN (SELECT TOP {1} RequestId FROM Hishop_SiteRequest ORDER BY RequestId DESC) ", pagination.PageSize, pagination.PageSize * (pagination.PageIndex - 1), 1);
			}
			text3 = text3 + text2 + " ORDER BY RequestId DESC";
			sqlStringCommand = this.database.GetSqlStringCommand(text3);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override bool AddInitData(int distributorId, System.Data.Common.DbTransaction dbtran)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_DistributionInitData_Create");
			this.database.AddInParameter(storedProcCommand, "UserId", System.Data.DbType.Int32, distributorId);
			bool result;
			if (dbtran != null)
			{
				result = (this.database.ExecuteNonQuery(storedProcCommand, dbtran) >= 1);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(storedProcCommand) >= 1);
			}
			return result;
		}
		public override SiteRequestInfo GetSiteRequestInfo(int requestId)
		{
			SiteRequestInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_SiteRequest WHERE RequestId=@RequestId");
			this.database.AddInParameter(sqlStringCommand, "RequestId", System.Data.DbType.Int32, requestId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulSiteRequest(dataReader);
				}
			}
			return result;
		}
		public override bool RefuseSiteRequest(int requestId, string reason)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_SiteRequest SET RequestStatus=@RequestStatus,RefuseReason=@RefuseReason WHERE RequestId=@RequestId");
			this.database.AddInParameter(sqlStringCommand, "RequestId", System.Data.DbType.Int32, requestId);
			this.database.AddInParameter(sqlStringCommand, "RequestStatus", System.Data.DbType.Int32, 3);
			this.database.AddInParameter(sqlStringCommand, "RefuseReason", System.Data.DbType.String, reason);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override bool AddSiteSettings(SiteSettings settings, int requestId, System.Data.Common.DbTransaction dbtran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO distro_Settings(UserId,SiteUrl,Disabled, RequestDate,CreateDate,LogoUrl,SiteDescription,SiteName,Theme,Footer,SearchMetaKeywords,SearchMetaDescription,DecimalLength,YourPriceName,DefaultProductImage,PointsRate,OrderShowDays,HtmlOnlineServiceCode) VALUES(@UserId,@SiteUrl,@Disabled,@RequestDate,@CreateDate,@LogoUrl,@SiteDescription,@SiteName,@Theme,@Footer,@SearchMetaKeywords,@SearchMetaDescription,@DecimalLength,@YourPriceName,@DefaultProductImage,@PointsRate,@OrderShowDays,@HtmlOnlineServiceCode)");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, settings.UserId);
			this.database.AddInParameter(sqlStringCommand, "SiteUrl", System.Data.DbType.String, settings.SiteUrl);
			this.database.AddInParameter(sqlStringCommand, "LogoUrl", System.Data.DbType.String, settings.LogoUrl);
			this.database.AddInParameter(sqlStringCommand, "SiteDescription", System.Data.DbType.String, settings.SiteDescription);
			this.database.AddInParameter(sqlStringCommand, "SiteName", System.Data.DbType.String, settings.SiteName);
			this.database.AddInParameter(sqlStringCommand, "Theme", System.Data.DbType.String, settings.Theme);
			this.database.AddInParameter(sqlStringCommand, "Footer", System.Data.DbType.String, settings.Footer);
			this.database.AddInParameter(sqlStringCommand, "SearchMetaKeywords", System.Data.DbType.String, settings.SearchMetaKeywords);
			this.database.AddInParameter(sqlStringCommand, "SearchMetaDescription", System.Data.DbType.String, settings.SearchMetaDescription);
			this.database.AddInParameter(sqlStringCommand, "DecimalLength", System.Data.DbType.Int32, settings.DecimalLength);
			this.database.AddInParameter(sqlStringCommand, "YourPriceName", System.Data.DbType.String, settings.YourPriceName);
			this.database.AddInParameter(sqlStringCommand, "Disabled", System.Data.DbType.Boolean, settings.Disabled);
			this.database.AddInParameter(sqlStringCommand, "DefaultProductImage", System.Data.DbType.String, settings.DefaultProductImage);
			this.database.AddInParameter(sqlStringCommand, "PointsRate", System.Data.DbType.Decimal, settings.PointsRate);
			this.database.AddInParameter(sqlStringCommand, "OrderShowDays", System.Data.DbType.Int32, settings.OrderShowDays);
			this.database.AddInParameter(sqlStringCommand, "HtmlOnlineServiceCode", System.Data.DbType.String, settings.HtmlOnlineServiceCode);
			this.database.AddInParameter(sqlStringCommand, "RequestDate", System.Data.DbType.DateTime, settings.RequestDate);
			this.database.AddInParameter(sqlStringCommand, "CreateDate", System.Data.DbType.DateTime, settings.CreateDate);
			bool result;
			if (dbtran != null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, dbtran) >= 1);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) >= 1);
			}
			return result;
		}
		public override bool AcceptSiteRequest(int siteQty, int requestId, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_SiteRequest SET RequestStatus=@RequestStatus WHERE RequestId=@RequestId AND (SELECT COUNT(UserId) FROM distro_Settings)<@SiteQty");
			this.database.AddInParameter(sqlStringCommand, "RequestStatus", System.Data.DbType.Int32, 2);
			this.database.AddInParameter(sqlStringCommand, "RequestId", System.Data.DbType.Int32, requestId);
			this.database.AddInParameter(sqlStringCommand, "SiteQty", System.Data.DbType.Int32, siteQty);
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
		public override System.Data.DataTable GetDistributorSites(Pagination pagination, string name, string trueName, out int total)
		{
			string str = string.Empty;
			System.Data.DataTable result = null;
			if (!string.IsNullOrEmpty(name))
			{
				str = string.Format(" AND UserName Like '%{0}%'", DataHelper.CleanSearchString(name));
			}
			if (!string.IsNullOrEmpty(trueName))
			{
				str = string.Format(" AND RealName Like '%{0}%'", DataHelper.CleanSearchString(trueName));
			}
			string text = "SELECT COUNT(*) FROM distro_Settings LEFT JOIN vw_aspnet_Distributors ON distro_Settings.UserId=vw_aspnet_Distributors.UserId WHERE 1=1 ";
			text += str;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			total = Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
			string text2 = string.Empty;
			if (pagination.PageIndex == 1)
			{
				text2 = string.Format("SELECT TOP {0} UserName,RealName,Wangwang,SiteUrl,RequestDate,Disabled ,[IsOpenEtao] ,[EtaoID] ,[EtaoApplyTime] ,[EtaoStatus],distro_Settings.UserId FROM distro_Settings LEFT JOIN vw_aspnet_Distributors ON distro_Settings.UserId=vw_aspnet_Distributors.UserId WHERE  1=1 ", pagination.PageSize);
			}
			else
			{
				text2 = string.Format("SELECT TOP {0} UserName,RealName,Wangwang,SiteUrl,RequestDate,Disabled,[IsOpenEtao] ,[EtaoID] ,[EtaoApplyTime] ,[EtaoStatus],distro_Settings.UserId       FROM distro_Settings LEFT JOIN vw_aspnet_Distributors ON distro_Settings.UserId=vw_aspnet_Distributors.UserId WHERE  distro_Settings.UserId NOT IN (SELECT TOP {1} UserId       FROM distro_Settings ORDER BY RequestDate DESC) ", pagination.PageSize, pagination.PageSize * (pagination.PageIndex - 1));
			}
			text2 = text2 + str + " ORDER BY RequestDate DESC";
			sqlStringCommand = this.database.GetSqlStringCommand(text2);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override System.Data.DataTable GetEtaoRequests(Pagination pagination, string name, string trueName, out int total)
		{
			string str = string.Empty;
			System.Data.DataTable result = null;
			if (!string.IsNullOrEmpty(name))
			{
				str = string.Format(" AND UserName Like '%{0}%'", DataHelper.CleanSearchString(name));
			}
			if (!string.IsNullOrEmpty(trueName))
			{
				str = string.Format(" AND RealName Like '%{0}%'", DataHelper.CleanSearchString(trueName));
			}
			string text = "SELECT COUNT(*) FROM distro_Settings LEFT JOIN vw_aspnet_Distributors ON distro_Settings.UserId=vw_aspnet_Distributors.UserId WHERE EtaoStatus=-1 ";
			text += str;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			total = Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
			string text2 = string.Empty;
			if (pagination.PageIndex == 1)
			{
				text2 = string.Format("SELECT TOP {0} UserName,RealName,Wangwang,SiteUrl,RequestDate,[Disabled] ,[IsOpenEtao] ,[EtaoID] ,[EtaoApplyTime] ,[EtaoStatus],distro_Settings.UserId FROM distro_Settings LEFT JOIN vw_aspnet_Distributors ON distro_Settings.UserId=vw_aspnet_Distributors.UserId WHERE  EtaoStatus=-1   ", pagination.PageSize);
			}
			else
			{
				text2 = string.Format("SELECT TOP {0} UserName,RealName,Wangwang,SiteUrl,[Disabled],[IsOpenEtao] ,[EtaoID] ,[EtaoApplyTime] ,[EtaoStatus],distro_Settings.UserId       FROM distro_Settings LEFT JOIN vw_aspnet_Distributors ON distro_Settings.UserId=vw_aspnet_Distributors.UserId WHERE  EtaoStatus=-1 and  distro_Settings.UserId NOT IN (SELECT TOP {1} UserId       FROM distro_Settings  where EtaoStatus=-1  ORDER BY EtaoApplyTime DESC) ", pagination.PageSize, pagination.PageSize * (pagination.PageIndex - 1));
			}
			text2 = text2 + str + " ORDER BY EtaoApplyTime DESC";
			sqlStringCommand = this.database.GetSqlStringCommand(text2);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override System.Data.DataTable GetEtaoSites(Pagination pagination, string name, string trueName, out int total)
		{
			string str = string.Empty;
			System.Data.DataTable result = null;
			if (!string.IsNullOrEmpty(name))
			{
				str = string.Format(" AND UserName Like '%{0}%'", DataHelper.CleanSearchString(name));
			}
			if (!string.IsNullOrEmpty(trueName))
			{
				str = string.Format(" AND RealName Like '%{0}%'", DataHelper.CleanSearchString(trueName));
			}
			string text = "SELECT COUNT(*) FROM distro_Settings LEFT JOIN vw_aspnet_Distributors ON distro_Settings.UserId=vw_aspnet_Distributors.UserId WHERE  EtaoStatus=1 ";
			text += str;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			total = Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand));
			string text2 = string.Empty;
			if (pagination.PageIndex == 1)
			{
				text2 = string.Format("SELECT TOP {0} UserName,RealName,Wangwang,SiteUrl,RequestDate,[Disabled] ,[IsOpenEtao] ,[EtaoID] ,[EtaoApplyTime] ,[EtaoStatus],distro_Settings.UserId FROM distro_Settings LEFT JOIN vw_aspnet_Distributors ON distro_Settings.UserId=vw_aspnet_Distributors.UserId WHERE  EtaoStatus=1  ", pagination.PageSize);
			}
			else
			{
				text2 = string.Format("SELECT TOP {0} UserName,RealName,Wangwang,SiteUrl,RequestDate,[Disabled],[IsOpenEtao] ,[EtaoID] ,[EtaoApplyTime] ,[EtaoStatus],distro_Settings.UserId       FROM distro_Settings LEFT JOIN vw_aspnet_Distributors ON distro_Settings.UserId=vw_aspnet_Distributors.UserId WHERE  EtaoStatus=1 and  distro_Settings.UserId NOT IN (SELECT TOP {1} UserId       FROM distro_Settings  where  EtaoStatus=1 ORDER BY EtaoApplyTime DESC) ", pagination.PageSize, pagination.PageSize * (pagination.PageIndex - 1));
			}
			text2 = text2 + str + " ORDER BY EtaoApplyTime DESC";
			sqlStringCommand = this.database.GetSqlStringCommand(text2);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override System.Data.DataTable GetDayDistributionTotal(int year, int month, SaleStatisticsType saleStatisticsType)
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
				decimal monthDistributionTotal = this.GetMonthDistributionTotal(year, month, saleStatisticsType);
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
					this.InsertToTable(dataTable, i, salesTotal, monthDistributionTotal);
				}
				result = dataTable;
			}
			return result;
		}
		public override System.Data.DataTable GetMonthDistributionTotal(int year, SaleStatisticsType saleStatisticsType)
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
					decimal yearDistributionTotal = this.GetYearDistributionTotal(year, saleStatisticsType);
					this.InsertToTable(dataTable, i, salesTotal, yearDistributionTotal);
				}
				result = dataTable;
			}
			return result;
		}
		public override decimal GetYearDistributionTotal(int year, SaleStatisticsType saleStatisticsType)
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
		public override OrderStatisticsInfo GetPurchaseOrders(UserOrderQuery order)
		{
			OrderStatisticsInfo orderStatisticsInfo = new OrderStatisticsInfo();
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_PurchaseOrderStatistics_Get");
			this.database.AddInParameter(storedProcCommand, "PageIndex", System.Data.DbType.Int32, order.PageIndex);
			this.database.AddInParameter(storedProcCommand, "PageSize", System.Data.DbType.Int32, order.PageSize);
			this.database.AddInParameter(storedProcCommand, "IsCount", System.Data.DbType.Boolean, order.IsCount);
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", System.Data.DbType.String, DistributionData.BuildPurchaseOrderQuery(order));
			this.database.AddOutParameter(storedProcCommand, "TotalPurchaseOrders", System.Data.DbType.Int32, 4);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
			{
				orderStatisticsInfo.OrderTbl = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (dataReader.NextResult())
				{
					dataReader.Read();
					if (dataReader["PurchaseTotal"] != DBNull.Value)
					{
						orderStatisticsInfo.TotalOfPage += (decimal)dataReader["PurchaseTotal"];
					}
					if (dataReader["PurchaseProfits"] != DBNull.Value)
					{
						orderStatisticsInfo.ProfitsOfPage += (decimal)dataReader["PurchaseProfits"];
					}
				}
				if (dataReader.NextResult())
				{
					dataReader.Read();
					if (dataReader["PurchaseTotal"] != DBNull.Value)
					{
						orderStatisticsInfo.TotalOfSearch += (decimal)dataReader["PurchaseTotal"];
					}
					if (dataReader["PurchaseProfits"] != DBNull.Value)
					{
						orderStatisticsInfo.ProfitsOfSearch += (decimal)dataReader["PurchaseProfits"];
					}
				}
			}
			orderStatisticsInfo.TotalCount = (int)this.database.GetParameterValue(storedProcCommand, "TotalPurchaseOrders");
			return orderStatisticsInfo;
		}
		public override OrderStatisticsInfo GetPurchaseOrdersNoPage(UserOrderQuery order)
		{
			OrderStatisticsInfo orderStatisticsInfo = new OrderStatisticsInfo();
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_PurchaseOrderStatisticsNoPage_Get");
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", System.Data.DbType.String, DistributionData.BuildPurchaseOrderQuery(order));
			this.database.AddOutParameter(storedProcCommand, "TotalPurchaseOrders", System.Data.DbType.Int32, 4);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
			{
				orderStatisticsInfo.OrderTbl = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (dataReader.NextResult())
				{
					dataReader.Read();
					if (dataReader["PurchaseTotal"] != DBNull.Value)
					{
						orderStatisticsInfo.TotalOfSearch += (decimal)dataReader["PurchaseTotal"];
					}
					if (dataReader["PurchaseProfits"] != DBNull.Value)
					{
						orderStatisticsInfo.ProfitsOfSearch += (decimal)dataReader["PurchaseProfits"];
					}
				}
			}
			orderStatisticsInfo.TotalCount = (int)this.database.GetParameterValue(storedProcCommand, "TotalPurchaseOrders");
			return orderStatisticsInfo;
		}
		public override OrderStatisticsInfo GetDistributorStatistics(SaleStatisticsQuery query, out int totalDistributors)
		{
			OrderStatisticsInfo orderStatisticsInfo = new OrderStatisticsInfo();
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_DistributorStatistics_Get");
			this.database.AddInParameter(storedProcCommand, "PageIndex", System.Data.DbType.Int32, query.PageIndex);
			this.database.AddInParameter(storedProcCommand, "PageSize", System.Data.DbType.Int32, query.PageSize);
			this.database.AddInParameter(storedProcCommand, "IsCount", System.Data.DbType.Boolean, query.IsCount);
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", System.Data.DbType.String, DistributionData.BuildDistributorStatisticsQuery(query));
			this.database.AddOutParameter(storedProcCommand, "TotalDistributors", System.Data.DbType.Int32, 4);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
			{
				orderStatisticsInfo.OrderTbl = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (dataReader.NextResult())
				{
					dataReader.Read();
					if (dataReader["SaleTotals"] != DBNull.Value)
					{
						orderStatisticsInfo.TotalOfSearch += (decimal)dataReader["SaleTotals"];
					}
					if (dataReader["Profits"] != DBNull.Value)
					{
						orderStatisticsInfo.ProfitsOfSearch += (decimal)dataReader["Profits"];
					}
				}
			}
			totalDistributors = (int)this.database.GetParameterValue(storedProcCommand, "TotalDistributors");
			return orderStatisticsInfo;
		}
		public override OrderStatisticsInfo GetDistributorStatisticsNoPage(SaleStatisticsQuery query)
		{
			OrderStatisticsInfo orderStatisticsInfo = new OrderStatisticsInfo();
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_DistributorStatisticsNoPage_Get");
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", System.Data.DbType.String, DistributionData.BuildDistributorStatisticsQuery(query));
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
			{
				orderStatisticsInfo.OrderTbl = DataHelper.ConverDataReaderToDataTable(dataReader);
				if (dataReader.NextResult())
				{
					dataReader.Read();
					if (dataReader["SaleTotals"] != DBNull.Value)
					{
						orderStatisticsInfo.TotalOfSearch += (decimal)dataReader["SaleTotals"];
					}
					if (dataReader["Profits"] != DBNull.Value)
					{
						orderStatisticsInfo.ProfitsOfSearch += (decimal)dataReader["Profits"];
					}
				}
			}
			return orderStatisticsInfo;
		}
		public override System.Data.DataTable GetDistributionProductSales(SaleStatisticsQuery productSale, out int totalProductSales)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_DistributionProductSales_Get");
			this.database.AddInParameter(storedProcCommand, "PageIndex", System.Data.DbType.Int32, productSale.PageIndex);
			this.database.AddInParameter(storedProcCommand, "PageSize", System.Data.DbType.Int32, productSale.PageSize);
			this.database.AddInParameter(storedProcCommand, "IsCount", System.Data.DbType.Boolean, productSale.IsCount);
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", System.Data.DbType.String, DistributionData.BuildProductSaleQuery(productSale));
			this.database.AddOutParameter(storedProcCommand, "TotalProductSales", System.Data.DbType.Int32, 4);
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			totalProductSales = (int)this.database.GetParameterValue(storedProcCommand, "TotalProductSales");
			return result;
		}
		public override System.Data.DataTable GetDistributionProductSalesNoPage(SaleStatisticsQuery productSale, out int totalProductSales)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_DistributionProductSalesNoPage_Get");
			this.database.AddInParameter(storedProcCommand, "sqlPopulate", System.Data.DbType.String, DistributionData.BuildProductSaleQuery(productSale));
			this.database.AddOutParameter(storedProcCommand, "TotalProductSales", System.Data.DbType.Int32, 4);
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			totalProductSales = (int)this.database.GetParameterValue(storedProcCommand, "TotalProductSales");
			return result;
		}
		public override decimal GetMonthDistributionTotal(int year, int month, SaleStatisticsType saleStatisticsType)
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
		private static string BuildProductSaleQuery(SaleStatisticsQuery query)
		{
			if (null == query)
			{
				throw new ArgumentNullException("query");
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT ProductId, SUM(o.Quantity) AS ProductSaleCounts, SUM(o.ItemPurchasePrice * o.Quantity) AS ProductSaleTotals,");
			stringBuilder.Append("  (SUM(o.ItemPurchasePrice * o.Quantity) - SUM(o.CostPrice * o.Quantity) )AS ProductProfitsTotals ");
			stringBuilder.AppendFormat(" FROM Hishop_PurchaseOrderItems o  WHERE 0=0 ", new object[0]);
			stringBuilder.AppendFormat(" AND PurchaseOrderId IN (SELECT  PurchaseOrderId FROM Hishop_PurchaseOrders WHERE PurchaseStatus != {0} AND PurchaseStatus != {1} AND PurchaseStatus != {2} )", 1, 4, 9);
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND PurchaseOrderId IN (SELECT PurchaseOrderId FROM Hishop_PurchaseOrders WHERE PurchaseDate >= '{0}')", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND PurchaseOrderId IN (SELECT PurchaseOrderId FROM Hishop_PurchaseOrders WHERE PurchaseDate <= '{0}')", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
			}
			stringBuilder.Append(" GROUP BY ProductId HAVING ProductId IN");
			stringBuilder.Append(" (SELECT ProductId FROM Hishop_Products)");
			if (!string.IsNullOrEmpty(query.SortBy))
			{
				stringBuilder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
			}
			return stringBuilder.ToString();
		}
		private static string BuildDistributorStatisticsQuery(SaleStatisticsQuery query)
		{
			if (null == query)
			{
				throw new ArgumentNullException("query");
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT UserId, UserName ");
			if (query.StartDate.HasValue || query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(",  ( select isnull(SUM(PurchaseTotal),0) from Hishop_PurchaseOrders where PurchaseStatus != {0} AND PurchaseStatus != {1} AND PurchaseStatus != {2}", 1, 4, 9);
				if (query.StartDate.HasValue)
				{
					stringBuilder.AppendFormat(" and PurchaseDate>='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
				}
				if (query.EndDate.HasValue)
				{
					stringBuilder.AppendFormat(" and PurchaseDate<='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
				}
				stringBuilder.Append(" and DistributorId = vw_aspnet_Distributors.UserId) as SaleTotals");
				stringBuilder.AppendFormat(",(select Count(PurchaseOrderId) from Hishop_PurchaseOrders where PurchaseStatus != {0} AND PurchaseStatus != {1} AND PurchaseStatus != {2}", 1, 4, 9);
				if (query.StartDate.HasValue)
				{
					stringBuilder.AppendFormat(" and PurchaseDate>='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
				}
				if (query.EndDate.HasValue)
				{
					stringBuilder.AppendFormat(" and PurchaseDate<='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
				}
				stringBuilder.Append(" and DistributorId = vw_aspnet_Distributors.UserId) as PurchaseOrderCount ");
				stringBuilder.AppendFormat(",(select isnull(SUM(PurchaseProfit),0) from Hishop_PurchaseOrders where PurchaseStatus != {0} AND PurchaseStatus != {1} AND PurchaseStatus != {2}", 1, 4, 9);
				if (query.StartDate.HasValue)
				{
					stringBuilder.AppendFormat(" and PurchaseDate>='{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
				}
				if (query.EndDate.HasValue)
				{
					stringBuilder.AppendFormat(" and PurchaseDate<='{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
				}
				stringBuilder.Append(" and DistributorId = vw_aspnet_Distributors.UserId) as Profits ");
			}
			else
			{
				stringBuilder.Append(",ISNULL(Expenditure,0) as SaleTotals,ISNULL(PurchaseOrder,0) as PurchaseOrderCount");
				stringBuilder.AppendFormat(",(select isnull(SUM(PurchaseProfit),0) from Hishop_PurchaseOrders where PurchaseStatus != {0} AND PurchaseStatus != {1} AND PurchaseStatus != {2}", 1, 4, 9);
				stringBuilder.Append(" and DistributorId = vw_aspnet_Distributors.UserId) as Profits ");
			}
			stringBuilder.Append(" from vw_aspnet_Distributors WHERE IsApproved = 1");
			if (!string.IsNullOrEmpty(query.SortBy))
			{
				stringBuilder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
			}
			return stringBuilder.ToString();
		}
		private static string BuildPurchaseOrderQuery(UserOrderQuery query)
		{
			if (null == query)
			{
				throw new ArgumentNullException("query");
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("SELECT PurchaseOrderId FROM Hishop_PurchaseOrders WHERE PurchaseStatus != {0} AND PurchaseStatus != {1} AND PurchaseStatus != {2}", 1, 4, 9);
			if (!string.IsNullOrEmpty(query.UserName))
			{
				stringBuilder.AppendFormat(" AND Distributorname LIKE '%{0}%'", DataHelper.CleanSearchString(query.UserName));
			}
			if (query.StartDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND  PurchaseDate >= '{0}'", DataHelper.GetSafeDateTimeFormat(query.StartDate.Value));
			}
			if (query.EndDate.HasValue)
			{
				stringBuilder.AppendFormat(" AND  PurchaseDate <= '{0}'", DataHelper.GetSafeDateTimeFormat(query.EndDate.Value));
			}
			if (!string.IsNullOrEmpty(query.SortBy))
			{
				stringBuilder.AppendFormat(" ORDER BY {0} {1}", DataHelper.CleanSearchString(query.SortBy), query.SortOrder.ToString());
			}
			return stringBuilder.ToString();
		}
		private string BuiderSqlStringByType(SaleStatisticsType saleStatisticsType)
		{
			StringBuilder stringBuilder = new StringBuilder();
			switch (saleStatisticsType)
			{
			case SaleStatisticsType.SaleCounts:
				stringBuilder.Append("SELECT COUNT(PurchaseOrderId) FROM Hishop_PurchaseOrders WHERE (PurchaseDate BETWEEN @StartDate AND @EndDate)");
				stringBuilder.AppendFormat(" AND PurchaseStatus != {0} AND PurchaseStatus != {1} AND PurchaseStatus != {2}", 1, 4, 9);
				break;
			case SaleStatisticsType.SaleTotal:
				stringBuilder.Append("SELECT Isnull(SUM(PurchaseTotal),0)");
				stringBuilder.Append(" FROM Hishop_PurchaseOrders WHERE  (PurchaseDate BETWEEN @StartDate AND @EndDate)");
				stringBuilder.AppendFormat(" AND PurchaseStatus != {0} AND PurchaseStatus != {1} AND PurchaseStatus != {2}", 1, 4, 9);
				break;
			case SaleStatisticsType.Profits:
				stringBuilder.Append("SELECT IsNull(SUM(PurchaseProfit),0) FROM Hishop_PurchaseOrders WHERE (PurchaseDate BETWEEN @StartDate AND @EndDate)");
				stringBuilder.AppendFormat(" AND PurchaseStatus != {0} AND PurchaseStatus != {1} AND PurchaseStatus != {2}", 1, 4, 9);
				break;
			}
			return stringBuilder.ToString();
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
	}
}
