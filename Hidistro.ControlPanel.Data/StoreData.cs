using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Store;
using Hidistro.Membership.Core;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
namespace Hidistro.ControlPanel.Data
{
	public class StoreData : StoreProvider
	{
		private Database database;
		public StoreData()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public override DbQueryResult GetManagers(ManagerQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" 0=0 ", DataHelper.CleanSearchString(query.Username));
			if (!string.IsNullOrEmpty(query.Username))
			{
				stringBuilder.AppendFormat(" and UserName LIKE '%{0}%' ", DataHelper.CleanSearchString(query.Username));
			}
			if (query.RoleId != Guid.Empty)
			{
				stringBuilder.AppendFormat(" AND UserId IN (SELECT UserId FROM aspnet_UsersInRoles WHERE RoleId = '{0}')", query.RoleId);
			}
			else
			{
				stringBuilder.AppendFormat(" AND ( supplier_regionid is null and supplier_gradeid is null ) ", new object[0]);
			}
			return DataHelper.PagingByRownumber(query.PageIndex, query.PageSize, query.SortBy, query.SortOrder, query.IsCount, "vw_aspnet_Managers", "UserId", stringBuilder.ToString(), "*");
		}
		public override void ClearRolePrivilege(Guid roleId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT UserName FROM vw_aspnet_Managers WHERE UserId IN (SELECT UserId FROM aspnet_UsersInRoles WHERE RoleId = '{0}')", roleId));
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					RoleHelper.SignOut((string)dataReader["UserName"]);
				}
			}
		}
		public override bool DeleteManager(int userId)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Manager_Delete");
			this.database.AddInParameter(storedProcCommand, "UserId", System.Data.DbType.Int32, userId);
			this.database.AddParameter(storedProcCommand, "ReturnValue", System.Data.DbType.Int32, System.Data.ParameterDirection.ReturnValue, string.Empty, System.Data.DataRowVersion.Default, null);
			this.database.ExecuteNonQuery(storedProcCommand);
			object parameterValue = this.database.GetParameterValue(storedProcCommand, "ReturnValue");
			return parameterValue != null && parameterValue != DBNull.Value && Convert.ToInt32(parameterValue) == 0;
		}
		public override IList<FriendlyLinksInfo> GetFriendlyLinks()
		{
			IList<FriendlyLinksInfo> list = new List<FriendlyLinksInfo>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_FriendlyLinks ORDER BY DisplaySequence DESC");
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulateFriendlyLink(dataReader));
				}
			}
			return list;
		}
		public override FriendlyLinksInfo GetFriendlyLink(int linkId)
		{
			FriendlyLinksInfo result = new FriendlyLinksInfo();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_FriendlyLinks WHERE LinkId=@LinkId");
			this.database.AddInParameter(sqlStringCommand, "LinkId", System.Data.DbType.Int32, linkId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateFriendlyLink(dataReader);
				}
			}
			return result;
		}
		public override bool CreateUpdateDeleteFriendlyLink(FriendlyLinksInfo friendlyLink, DataProviderAction action)
		{
			bool result;
			if (null == friendlyLink)
			{
				result = false;
			}
			else
			{
				System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_FriendlyLink_CreateUpdateDelete");
				this.database.AddInParameter(storedProcCommand, "Action", System.Data.DbType.Int32, (int)action);
				this.database.AddOutParameter(storedProcCommand, "Status", System.Data.DbType.Int32, 4);
				if (action != DataProviderAction.Create)
				{
					this.database.AddInParameter(storedProcCommand, "LinkId", System.Data.DbType.Int32, friendlyLink.LinkId);
				}
				if (action != DataProviderAction.Delete)
				{
					this.database.AddInParameter(storedProcCommand, "ImageUrl", System.Data.DbType.String, friendlyLink.ImageUrl);
					this.database.AddInParameter(storedProcCommand, "LinkUrl", System.Data.DbType.String, friendlyLink.LinkUrl);
					this.database.AddInParameter(storedProcCommand, "Title", System.Data.DbType.String, friendlyLink.Title);
					this.database.AddInParameter(storedProcCommand, "Visible", System.Data.DbType.Boolean, friendlyLink.Visible);
				}
				this.database.ExecuteNonQuery(storedProcCommand);
				result = ((int)this.database.GetParameterValue(storedProcCommand, "Status") == 0);
			}
			return result;
		}
		public override void SwapFriendlyLinkSequence(int linkId, int replaceLinkId, int displaySequence, int replaceDisplaySequence)
		{
			DataHelper.SwapSequence("Hishop_FriendlyLinks", "LinkId", "DisplaySequence", linkId, replaceLinkId, displaySequence, replaceDisplaySequence);
		}
		public override int FriendlyLinkDelete(int linkId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_FriendlyLinks WHERE linkid = @linkid");
			this.database.AddInParameter(sqlStringCommand, "Linkid", System.Data.DbType.Int32, linkId);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override void AddHotkeywords(int categoryId, string Keywords)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Hotkeywords_Log");
			this.database.AddInParameter(storedProcCommand, "Keywords", System.Data.DbType.String, Keywords);
			this.database.AddInParameter(storedProcCommand, "CategoryId", System.Data.DbType.Int32, categoryId);
			this.database.AddInParameter(storedProcCommand, "SearchTime", System.Data.DbType.DateTime, DateTime.Now);
			this.database.ExecuteNonQuery(storedProcCommand);
		}
		public override string GetHotkeyword(int int_0)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Keywords FROM Hishop_Hotkeywords WHERE Hid=@Hid");
			this.database.AddInParameter(sqlStringCommand, "Hid", System.Data.DbType.Int32, int_0);
			return this.database.ExecuteScalar(sqlStringCommand).ToString();
		}
		public override System.Data.DataTable GetHotKeywords()
		{
			System.Data.DataTable result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *,(SELECT Name FROM Hishop_Categories WHERE CategoryId = h.CategoryId) AS CategoryName FROM Hishop_Hotkeywords h ORDER BY Frequency DESC");
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override void DeleteHotKeywords(int hId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" Delete FROM Hishop_Hotkeywords Where Hid =@Hid");
			this.database.AddInParameter(sqlStringCommand, "Hid", System.Data.DbType.Int32, hId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override void SwapHotWordsSequence(int int_0, int replaceHid, int displaySequence, int replaceDisplaySequence)
		{
			DataHelper.SwapSequence("Hishop_Hotkeywords", "Hid", "Frequency", int_0, replaceHid, displaySequence, replaceDisplaySequence);
		}
		public override void UpdateHotWords(int int_0, int categoryId, string hotKeyWords)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update Hishop_Hotkeywords Set CategoryId = @CategoryId, Keywords =@Keywords Where Hid =@Hid");
			this.database.AddInParameter(sqlStringCommand, "Hid", System.Data.DbType.Int32, int_0);
			this.database.AddInParameter(sqlStringCommand, "CategoryId", System.Data.DbType.Int32, categoryId);
			this.database.AddInParameter(sqlStringCommand, "Keywords", System.Data.DbType.String, hotKeyWords);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override System.Data.DataSet GetVotes()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *, (SELECT ISNULL(SUM(ItemCount),0) FROM Hishop_VoteItems WHERE VoteId = Hishop_Votes.VoteId) AS VoteCounts FROM Hishop_Votes");
			return this.database.ExecuteDataSet(sqlStringCommand);
		}
		public override int SetVoteIsBackup(long voteId)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Votes_IsBackup");
			this.database.AddInParameter(storedProcCommand, "VoteId", System.Data.DbType.Int64, voteId);
			return this.database.ExecuteNonQuery(storedProcCommand);
		}
		public override long CreateVote(VoteInfo vote)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_Votes_Create");
			this.database.AddInParameter(storedProcCommand, "VoteName", System.Data.DbType.String, vote.VoteName);
			this.database.AddInParameter(storedProcCommand, "IsBackup", System.Data.DbType.Boolean, vote.IsBackup);
			this.database.AddInParameter(storedProcCommand, "MaxCheck", System.Data.DbType.Int32, vote.MaxCheck);
			this.database.AddOutParameter(storedProcCommand, "VoteId", System.Data.DbType.Int64, 8);
			long result = 0L;
			if (this.database.ExecuteNonQuery(storedProcCommand) > 0)
			{
				result = (long)this.database.GetParameterValue(storedProcCommand, "VoteId");
			}
			return result;
		}
		public override bool UpdateVote(VoteInfo vote, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_Votes SET VoteName = @VoteName, MaxCheck = @MaxCheck WHERE VoteId = @VoteId");
			this.database.AddInParameter(sqlStringCommand, "VoteName", System.Data.DbType.String, vote.VoteName);
			this.database.AddInParameter(sqlStringCommand, "MaxCheck", System.Data.DbType.Int32, vote.MaxCheck);
			this.database.AddInParameter(sqlStringCommand, "VoteId", System.Data.DbType.Int64, vote.VoteId);
			return this.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1;
		}
		public override int DeleteVote(long voteId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Votes WHERE VoteId = @VoteId");
			this.database.AddInParameter(sqlStringCommand, "VoteId", System.Data.DbType.Int64, voteId);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override int CreateVoteItem(VoteItemInfo voteItem, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_VoteItems(VoteId, VoteItemName, ItemCount) Values(@VoteId, @VoteItemName, @ItemCount)");
			this.database.AddInParameter(sqlStringCommand, "VoteId", System.Data.DbType.Int64, voteItem.VoteId);
			this.database.AddInParameter(sqlStringCommand, "VoteItemName", System.Data.DbType.String, voteItem.VoteItemName);
			this.database.AddInParameter(sqlStringCommand, "ItemCount", System.Data.DbType.Int32, voteItem.ItemCount);
			int result;
			if (dbTran == null)
			{
				result = this.database.ExecuteNonQuery(sqlStringCommand);
			}
			else
			{
				result = this.database.ExecuteNonQuery(sqlStringCommand, dbTran);
			}
			return result;
		}
		public override bool DeleteVoteItem(long voteId, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_VoteItems WHERE VoteId = @VoteId");
			this.database.AddInParameter(sqlStringCommand, "VoteId", System.Data.DbType.Int64, voteId);
			return this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0;
		}
		public override VoteInfo GetVoteById(long voteId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *, (SELECT ISNULL(SUM(ItemCount),0) FROM Hishop_VoteItems WHERE VoteId = @VoteId) AS VoteCounts FROM Hishop_Votes WHERE VoteId = @VoteId");
			this.database.AddInParameter(sqlStringCommand, "VoteId", System.Data.DbType.Int64, voteId);
			VoteInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateVote(dataReader);
				}
			}
			return result;
		}
		public override IList<VoteItemInfo> GetVoteItems(long voteId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_VoteItems WHERE VoteId = @VoteId");
			this.database.AddInParameter(sqlStringCommand, "VoteId", System.Data.DbType.Int64, voteId);
			IList<VoteItemInfo> list = new List<VoteItemInfo>();
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					VoteItemInfo item = DataMapper.PopulateVoteItem(dataReader);
					list.Add(item);
				}
			}
			return list;
		}
		public override int GetVoteCounts(long voteId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ISNULL(SUM(ItemCount),0) FROM Hishop_VoteItems WHERE VoteId = @VoteId");
			this.database.AddInParameter(sqlStringCommand, "VoteId", System.Data.DbType.Int64, voteId);
			return (int)this.database.ExecuteScalar(sqlStringCommand);
		}
		private string StringCut(string string_0, string string_1, string string_2)
		{
			string text = string_0.Substring(string_0.IndexOf(string_1) + string_1.Length);
			return text.Substring(0, text.IndexOf(string_2));
		}
		public override string BackupData(string path)
		{
			string text;
			using (System.Data.Common.DbConnection dbConnection = this.database.CreateConnection())
			{
				text = dbConnection.Database;
			}
			string text2 = text + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".bak";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("backup database [{0}] to disk='{1}'", text, path + text2));
			string result;
			try
			{
				this.database.ExecuteNonQuery(sqlStringCommand);
				result = text2;
			}
			catch
			{
				result = string.Empty;
			}
			return result;
		}
		public override bool RestoreData(string bakFullName)
		{
			string arg;
			string dataSource;
			using (System.Data.Common.DbConnection dbConnection = this.database.CreateConnection())
			{
				arg = dbConnection.Database;
				dataSource = dbConnection.DataSource;
			}
			System.Data.SqlClient.SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection(string.Format("Data Source={0};Initial Catalog=master;Integrated Security=SSPI", dataSource));
			bool result;
			try
			{
				sqlConnection.Open();
				System.Data.SqlClient.SqlCommand sqlCommand = new System.Data.SqlClient.SqlCommand(string.Format("SELECT spid FROM sysprocesses ,sysdatabases WHERE sysprocesses.dbid=sysdatabases.dbid AND sysdatabases.Name='{0}'", arg), sqlConnection);
				ArrayList arrayList = new ArrayList();
				using (System.Data.IDataReader dataReader = sqlCommand.ExecuteReader())
				{
					while (dataReader.Read())
					{
						arrayList.Add(dataReader.GetInt16(0));
					}
				}
				for (int i = 0; i < arrayList.Count; i++)
				{
					sqlCommand = new System.Data.SqlClient.SqlCommand(string.Format("KILL {0}", arrayList[i].ToString()), sqlConnection);
					sqlCommand.ExecuteNonQuery();
				}
				sqlCommand = new System.Data.SqlClient.SqlCommand(string.Format("RESTORE DATABASE [{0}]  FROM DISK = '{1}' WITH REPLACE", arg, bakFullName), sqlConnection);
				sqlCommand.ExecuteNonQuery();
				result = true;
			}
			catch
			{
				result = false;
			}
			finally
			{
				sqlConnection.Close();
			}
			return result;
		}
		public override void Restor()
		{
			try
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
				this.database.ExecuteNonQuery(sqlStringCommand);
			}
			catch
			{
			}
		}
		public override bool DeleteLog(long logId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_Logs WHERE LogId = @LogId");
			this.database.AddInParameter(sqlStringCommand, "LogId", System.Data.DbType.Int64, logId);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override bool DeleteAllLogs()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("TRUNCATE TABLE Hishop_Logs");
			bool result;
			try
			{
				this.database.ExecuteNonQuery(sqlStringCommand);
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}
		public override int DeleteLogs(string strIds)
		{
			int result;
			if (strIds.Length <= 0)
			{
				result = 0;
			}
			else
			{
				string text = string.Format("DELETE FROM Hishop_Logs WHERE LogId IN ({0})", strIds);
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
				result = this.database.ExecuteNonQuery(sqlStringCommand);
			}
			return result;
		}
		public override DbQueryResult GetLogs(OperationLogQuery query)
		{
			StringBuilder stringBuilder = new StringBuilder();
			Pagination page = query.Page;
			if (query.FromDate.HasValue)
			{
				stringBuilder.AppendFormat("AddedTime >= '{0}'", DataHelper.GetSafeDateTimeFormat(query.FromDate.Value));
			}
			if (query.ToDate.HasValue)
			{
				if (!string.IsNullOrEmpty(stringBuilder.ToString()))
				{
					stringBuilder.Append(" AND");
				}
				stringBuilder.AppendFormat(" AddedTime <= '{0}'", DataHelper.GetSafeDateTimeFormat(query.ToDate.Value));
			}
			if (!string.IsNullOrEmpty(query.OperationUserName))
			{
				if (!string.IsNullOrEmpty(stringBuilder.ToString()))
				{
					stringBuilder.Append(" AND");
				}
				stringBuilder.AppendFormat(" UserName = '{0}'", DataHelper.CleanSearchString(query.OperationUserName));
			}
			return DataHelper.PagingByTopsort(page.PageIndex, page.PageSize, page.SortBy, page.SortOrder, page.IsCount, "Hishop_Logs", "LogId", stringBuilder.ToString(), "*");
		}
		public override IList<string> GetOperationUserNames()
		{
			IList<string> list = new List<string>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT DISTINCT UserName FROM Hishop_Logs");
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(dataReader["UserName"].ToString());
				}
			}
			return list;
		}
		public override void WriteOperationLogEntry(OperationLogEntry entry)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO [Hishop_Logs]([PageUrl],[AddedTime],[UserName],[IPAddress],[Privilege],[Description]) VALUES(@PageUrl,@AddedTime,@UserName,@IPAddress,@Privilege,@Description)");
			this.database.AddInParameter(sqlStringCommand, "PageUrl", System.Data.DbType.String, entry.PageUrl);
			this.database.AddInParameter(sqlStringCommand, "AddedTime", System.Data.DbType.DateTime, entry.AddedTime);
			this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, entry.UserName);
			this.database.AddInParameter(sqlStringCommand, "IPAddress", System.Data.DbType.String, entry.IpAddress);
			this.database.AddInParameter(sqlStringCommand, "Privilege", System.Data.DbType.Int32, (int)entry.Privilege);
			this.database.AddInParameter(sqlStringCommand, "Description", System.Data.DbType.String, entry.Description);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override void EnqueuCellphone(string cellphoneNumber, string subject)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_CellphoneQueue (CellphoneId, CellphoneNumber, Subject, NextTryTime, NumberOfTries)VALUES(newid(), @CellphoneNumber,@Subject, getdate(), 0)");
			this.database.AddInParameter(sqlStringCommand, "CellphoneNumber", System.Data.DbType.String, cellphoneNumber);
			this.database.AddInParameter(sqlStringCommand, "Subject", System.Data.DbType.String, subject);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override System.Data.DataTable DequeueCellphone()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_CellphoneQueue WHERE NextTryTime < getdate()");
			System.Data.DataTable result;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override void DeleteQueuedCellphone(Guid cellphoneId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_CellphoneQueue WHERE CellphoneId = @CellphoneId");
			this.database.AddInParameter(sqlStringCommand, "CellphoneId", System.Data.DbType.Guid, cellphoneId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override void QueueSendingFailure(IList<Guid> cellphoneIds)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_CellphoneQueue SET NextTryTime = getdate(), NumberOfTries = NumberOfTries +1 WHERE CellphoneId = @CellphoneId DELETE FROM Hishop_CellphoneQueue WHERE NumberOfTries > 10");
			this.database.AddInParameter(sqlStringCommand, "CellphoneId", System.Data.DbType.Guid);
			foreach (Guid current in cellphoneIds)
			{
				this.database.SetParameterValue(sqlStringCommand, "CellphoneId", current);
				this.database.ExecuteNonQuery(sqlStringCommand);
			}
		}
	}
}
