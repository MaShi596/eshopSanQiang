using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Distribution;
using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Subsites.Store;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
namespace Hidistro.Subsites.Data
{
	public class StoreData : SubsiteStoreProvider
	{
		private Database database;
		public StoreData()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public override IList<FriendlyLinksInfo> GetFriendlyLinks()
		{
			IList<FriendlyLinksInfo> list = new List<FriendlyLinksInfo>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_FriendlyLinks WHERE DistributorUserId=@DistributorUserId ORDER BY DisplaySequence DESC");
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_FriendlyLinks WHERE LinkId=@LinkId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "LinkId", System.Data.DbType.Int32, linkId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
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
				System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("sub_FriendlyLink_CreateUpdateDelete");
				this.database.AddInParameter(storedProcCommand, "Action", System.Data.DbType.Int32, (int)action);
				this.database.AddOutParameter(storedProcCommand, "Status", System.Data.DbType.Int32, 4);
				if (action != DataProviderAction.Create)
				{
					this.database.AddInParameter(storedProcCommand, "LinkId", System.Data.DbType.Int32, friendlyLink.LinkId);
				}
				this.database.AddInParameter(storedProcCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
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
			DataHelper.SwapSequence("distro_FriendlyLinks", "LinkId", "DisplaySequence", linkId, replaceLinkId, displaySequence, replaceDisplaySequence);
		}
		public override int FriendlyLinkDelete(int linkId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_FriendlyLinks WHERE LinkId = @LinkId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "LinkId", System.Data.DbType.Int32, linkId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override System.Data.DataSet GetVotes()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *, (SELECT ISNULL(SUM(ItemCount),0) FROM distro_VoteItems WHERE VoteId = distro_Votes.VoteId) AS VoteCounts FROM distro_Votes WHERE DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return this.database.ExecuteDataSet(sqlStringCommand);
		}
		public override int SetVoteIsBackup(long voteId)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("sub_Votes_IsBackup");
			this.database.AddInParameter(storedProcCommand, "VoteId", System.Data.DbType.Int64, voteId);
			return this.database.ExecuteNonQuery(storedProcCommand);
		}
		public override long CreateVote(VoteInfo vote)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("sub_Votes_Create");
			this.database.AddInParameter(storedProcCommand, "VoteName", System.Data.DbType.String, vote.VoteName);
			this.database.AddInParameter(storedProcCommand, "IsBackup", System.Data.DbType.Boolean, vote.IsBackup);
			this.database.AddInParameter(storedProcCommand, "MaxCheck", System.Data.DbType.Int32, vote.MaxCheck);
			this.database.AddInParameter(storedProcCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_Votes SET VoteName = @VoteName, IsBackup = @IsBackup, MaxCheck = @MaxCheck WHERE VoteId = @VoteId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "VoteName", System.Data.DbType.String, vote.VoteName);
			this.database.AddInParameter(sqlStringCommand, "IsBackup", System.Data.DbType.Boolean, vote.IsBackup);
			this.database.AddInParameter(sqlStringCommand, "MaxCheck", System.Data.DbType.Int32, vote.MaxCheck);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "VoteId", System.Data.DbType.Int64, vote.VoteId);
			return this.database.ExecuteNonQuery(sqlStringCommand, dbTran) == 1;
		}
		public override int DeleteVote(long voteId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_Votes WHERE VoteId = @VoteId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "VoteId", System.Data.DbType.Int64, voteId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override int CreateVoteItem(VoteItemInfo voteItem, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO distro_VoteItems(VoteId, VoteItemName, ItemCount) Values(@VoteId, @VoteItemName, @ItemCount)");
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_VoteItems WHERE VoteId = @VoteId");
			this.database.AddInParameter(sqlStringCommand, "VoteId", System.Data.DbType.Int64, voteId);
			return this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0;
		}
		public override VoteInfo GetVoteById(long voteId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *, (SELECT ISNULL(SUM(ItemCount),0) FROM distro_VoteItems WHERE VoteId = @VoteId) AS VoteCounts FROM distro_Votes WHERE VoteId = @VoteId AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "VoteId", System.Data.DbType.Int64, voteId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_VoteItems WHERE VoteId = @VoteId");
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ISNULL(SUM(ItemCount),0) FROM distro_VoteItems WHERE VoteId = @VoteId");
			this.database.AddInParameter(sqlStringCommand, "VoteId", System.Data.DbType.Int64, voteId);
			return (int)this.database.ExecuteScalar(sqlStringCommand);
		}
		public override void AddHotkeywords(int categoryId, string Keywords)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("sub_Hotkeywords_Log");
			this.database.AddInParameter(storedProcCommand, "CategoryId", System.Data.DbType.Int32, categoryId);
			this.database.AddInParameter(storedProcCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(storedProcCommand, "Keywords", System.Data.DbType.String, Keywords);
			this.database.AddInParameter(storedProcCommand, "SearchTime", System.Data.DbType.DateTime, DateTime.Now);
			this.database.ExecuteNonQuery(storedProcCommand);
		}
		public override System.Data.DataTable GetHotKeywords()
		{
			System.Data.DataTable result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT *, (SELECT Name FROM distro_Categories WHERE CategoryId = h.CategoryId AND DistributorUserId=@DistributorUserId)  AS CategoryName FROM distro_Hotkeywords h WHERE DistributorUserId=@DistributorUserId ORDER BY Frequency DESC");
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override void DeleteHotKeywords(int hId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" Delete FROM distro_Hotkeywords Where Hid =@Hid AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "Hid", System.Data.DbType.Int32, hId);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override void SwapHotWordsSequence(int int_0, int replaceHid, int displaySequence, int replaceDisplaySequence)
		{
			DataHelper.SwapSequence("distro_Hotkeywords", "Hid", "Frequency", int_0, replaceHid, displaySequence, replaceDisplaySequence);
		}
		public override void UpdateHotWords(int int_0, int categoryId, string hotKeyWords)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("Update distro_Hotkeywords Set CategoryId = @CategoryId, Keywords =@Keywords Where Hid =@Hid AND DistributorUserId=@DistributorUserId");
			this.database.AddInParameter(sqlStringCommand, "Hid", System.Data.DbType.Int32, int_0);
			this.database.AddInParameter(sqlStringCommand, "CategoryId", System.Data.DbType.Int32, categoryId);
			this.database.AddInParameter(sqlStringCommand, "Keywords", System.Data.DbType.String, hotKeyWords);
			this.database.AddInParameter(sqlStringCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override AccountSummaryInfo GetMyAccountSummary()
		{
			AccountSummaryInfo accountSummaryInfo = new AccountSummaryInfo();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SUM(Amount) AS FreezeBalance FROM Hishop_DistributorBalanceDrawRequest WHERE UserId=@UserId; SELECT TOP 1 Balance AS AccountAmount FROM Hishop_DistributorBalanceDetails WHERE UserId= @UserId ORDER BY JournalNumber DESC;");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read() && DBNull.Value != dataReader["FreezeBalance"])
				{
					accountSummaryInfo.DrawRequestBalance = (accountSummaryInfo.FreezeBalance = (decimal)dataReader["FreezeBalance"]);
				}
				if (dataReader.NextResult() && dataReader.Read() && DBNull.Value != dataReader["AccountAmount"])
				{
					accountSummaryInfo.AccountAmount = (decimal)dataReader["AccountAmount"];
				}
			}
			accountSummaryInfo.UseableBalance = accountSummaryInfo.AccountAmount - accountSummaryInfo.FreezeBalance;
			return accountSummaryInfo;
		}
		public override DbQueryResult GetMyBalanceDetails(BalanceDetailQuery query)
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
				string text = StoreData.BuildBalanceDetailsQuery(query);
				stringBuilder.AppendFormat("select top {0} B.JournalNumber,B.UserId,B.UserName, B.TradeDate,B.TradeType,B.Income,B.Expenses,B.Balance,B.Remark", query.PageSize);
				stringBuilder.Append(" from Hishop_DistributorBalanceDetails B where 0=0 ");
				if (query.PageIndex == 1)
				{
					stringBuilder.AppendFormat("{0} ORDER BY JournalNumber DESC", text);
				}
				else
				{
					stringBuilder.AppendFormat(" and JournalNumber < (select min(JournalNumber) from (select top {0} JournalNumber from Hishop_DistributorBalanceDetails where 0=0 {1} ORDER BY JournalNumber DESC ) as tbltemp) {1} ORDER BY JournalNumber DESC", (query.PageIndex - 1) * query.PageSize, text);
				}
				if (query.IsCount)
				{
					stringBuilder.AppendFormat(";select count(JournalNumber) as Total from Hishop_DistributorBalanceDetails where 0=0 {0}", text);
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
		public override bool BalanceDrawRequest(BalanceDrawRequestInfo balanceDrawRequest)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_DistributorBalanceDrawRequest VALUES(@UserId,@UserName,@RequestTime,@Amount,@AccountName,@BankName,@MerchantCode)");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, balanceDrawRequest.UserId);
			this.database.AddInParameter(sqlStringCommand, "UserName", System.Data.DbType.String, balanceDrawRequest.UserName);
			this.database.AddInParameter(sqlStringCommand, "RequestTime", System.Data.DbType.DateTime, balanceDrawRequest.RequestTime);
			this.database.AddInParameter(sqlStringCommand, "Amount", System.Data.DbType.Currency, balanceDrawRequest.Amount);
			this.database.AddInParameter(sqlStringCommand, "MerchantCode", System.Data.DbType.String, balanceDrawRequest.MerchantCode);
			this.database.AddInParameter(sqlStringCommand, "BankName", System.Data.DbType.String, balanceDrawRequest.BankName);
			this.database.AddInParameter(sqlStringCommand, "AccountName", System.Data.DbType.String, balanceDrawRequest.AccountName);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override bool DistroHasDrawRequest()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_DistributorBalanceDrawRequest WHERE UserId=@UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return Convert.ToInt32(this.database.ExecuteScalar(sqlStringCommand)) >= 1;
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
				System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("sub_DistributorInpourRequest_Create");
				this.database.AddOutParameter(storedProcCommand, "Status", System.Data.DbType.Int32, 4);
				this.database.AddInParameter(storedProcCommand, "InpourId", System.Data.DbType.String, inpourRequest.InpourId);
				this.database.AddInParameter(storedProcCommand, "TradeDate", System.Data.DbType.DateTime, inpourRequest.TradeDate);
				this.database.AddInParameter(storedProcCommand, "InpourBlance", System.Data.DbType.Currency, inpourRequest.InpourBlance);
				this.database.AddInParameter(storedProcCommand, "UserId", System.Data.DbType.Int32, inpourRequest.UserId);
				this.database.AddInParameter(storedProcCommand, "PaymentId", System.Data.DbType.String, inpourRequest.PaymentId);
				this.database.ExecuteNonQuery(storedProcCommand);
				result = ((int)this.database.GetParameterValue(storedProcCommand, "Status") == 0);
			}
			return result;
		}
		public override InpourRequestInfo GetInpouRequest(string inpourId)
		{
			InpourRequestInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_DistributorInpourRequest WHERE InpourId = @InpourId");
			this.database.AddInParameter(sqlStringCommand, "InpourId", System.Data.DbType.String, inpourId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					result = DataMapper.PopulateInpourRequest(dataReader);
				}
			}
			return result;
		}
		public override void RemoveInpourRequest(string inpourId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_DistributorInpourRequest WHERE InpourId = @InpourId");
			this.database.AddInParameter(sqlStringCommand, "InpourId", System.Data.DbType.String, inpourId);
			this.database.ExecuteNonQuery(sqlStringCommand);
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
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_DistributorBalanceDetails(UserId,UserName, TradeDate, TradeType, Income, Balance, Remark, InpourId) VALUES (@UserId, @UserName, @TradeDate, @TradeType, @Income, @Balance, @Remark, @InpourId)");
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM Hishop_DistributorBalanceDetails WHERE InpourId = @InpourId");
			this.database.AddInParameter(sqlStringCommand, "InpourId", System.Data.DbType.String, inpourId);
			return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
		}
		public override PaymentModeInfo GetPaymentMode(string gateway)
		{
			PaymentModeInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT top 1 * FROM Hishop_PaymentTypes WHERE Gateway = @Gateway");
			this.database.AddInParameter(sqlStringCommand, "Gateway", System.Data.DbType.String, gateway);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulatePayment(dataReader);
				}
			}
			return result;
		}
		public override PaymentModeInfo GetPaymentMode(int modeId)
		{
			PaymentModeInfo result = new PaymentModeInfo();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PaymentTypes WHERE ModeId = @ModeId");
			this.database.AddInParameter(sqlStringCommand, "ModeId", System.Data.DbType.Int32, modeId);
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PaymentTypes Order by DisplaySequence desc");
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulatePayment(dataReader));
				}
			}
			return list;
		}
		public override IList<PaymentModeInfo> GetDistributorPaymentModes()
		{
			IList<PaymentModeInfo> list = new List<PaymentModeInfo>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PaymentTypes where IsUseInDistributor=1 Order by DisplaySequence desc");
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulatePayment(dataReader));
				}
			}
			return list;
		}
		public override SiteRequestInfo GetMySiteRequest()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_SiteRequest WHERE UserId=@UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			SiteRequestInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulSiteRequest(dataReader);
				}
			}
			return result;
		}
		public override bool AddSiteRequest(SiteRequestInfo siteRequest)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_SiteRequest(UserId,FirstSiteUrl,RequestTime,RequestStatus,RefuseReason)VALUES(@UserId,@FirstSiteUrl,@RequestTime,@RequestStatus,@RefuseReason)");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "FirstSiteUrl", System.Data.DbType.String, siteRequest.FirstSiteUrl);
			this.database.AddInParameter(sqlStringCommand, "RequestTime", System.Data.DbType.DateTime, siteRequest.RequestTime);
			this.database.AddInParameter(sqlStringCommand, "RequestStatus", System.Data.DbType.Int32, (int)siteRequest.RequestStatus);
			this.database.AddInParameter(sqlStringCommand, "RefuseReason", System.Data.DbType.String, siteRequest.RefuseReason);
			return this.database.ExecuteNonQuery(sqlStringCommand) == 1;
		}
		public override void DeleteSiteRequest()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_SiteRequest WHERE UserId=@UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override bool IsExitSiteUrl(string siteUrl)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT COUNT(*) FROM distro_Settings WHERE SiteUrl = @SiteUrl");
			this.database.AddInParameter(sqlStringCommand, "SiteUrl", System.Data.DbType.String, siteUrl);
			return (int)this.database.ExecuteScalar(sqlStringCommand) > 0;
		}
		private static string BuildBalanceDetailsQuery(BalanceDetailQuery query)
		{
			IUser user = HiContext.Current.User;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(" AND UserId = {0}", user.UserId);
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
		public override DistributorGradeInfo GetDistributorGradeInfo(int gradeId)
		{
			DistributorGradeInfo result = new DistributorGradeInfo();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_DistributorGrades WHERE GradeId=@GradeId");
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, gradeId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulDistributorGrade(dataReader);
				}
			}
			return result;
		}
		public override bool BindTaobaoSetting(bool isUserHomeSiteApp, string topkey, string topSecret)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Distributors SET IsUserHomeSiteApp = @IsUserHomeSiteApp, Topkey = @Topkey, TopSecret = @TopSecret WHERE UserId = @UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "IsUserHomeSiteApp", System.Data.DbType.Boolean, isUserHomeSiteApp);
			this.database.AddInParameter(sqlStringCommand, "Topkey", System.Data.DbType.String, topkey);
			this.database.AddInParameter(sqlStringCommand, "TopSecret", System.Data.DbType.String, topSecret);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
	}
}
