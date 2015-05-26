using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Store;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace Hidistro.ControlPanel.Store
{
	public abstract class StoreProvider
	{
		private static readonly StoreProvider _defaultInstance;
		static StoreProvider()
		{
			StoreProvider._defaultInstance = (DataProviders.CreateInstance("Hidistro.ControlPanel.Data.StoreData,Hidistro.ControlPanel.Data") as StoreProvider);
		}
		public static StoreProvider Instance()
		{
			return StoreProvider._defaultInstance;
		}
		public abstract DbQueryResult GetManagers(ManagerQuery query);
		public abstract void ClearRolePrivilege(Guid roleId);
		public abstract bool DeleteManager(int userId);
		public abstract IList<FriendlyLinksInfo> GetFriendlyLinks();
		public abstract FriendlyLinksInfo GetFriendlyLink(int linkId);
		public abstract bool CreateUpdateDeleteFriendlyLink(FriendlyLinksInfo friendlyLink, DataProviderAction action);
		public abstract void SwapFriendlyLinkSequence(int linkId, int replaceLinkId, int displaySequence, int replaceDisplaySequence);
		public abstract int FriendlyLinkDelete(int linkId);
		public abstract void DeleteHotKeywords(int hId);
		public abstract void SwapHotWordsSequence(int int_0, int replaceHid, int displaySequence, int replaceDisplaySequence);
		public abstract void UpdateHotWords(int int_0, int categoryId, string hotKeyWords);
		public abstract void AddHotkeywords(int categoryId, string keywords);
		public abstract string GetHotkeyword(int int_0);
		public abstract System.Data.DataTable GetHotKeywords();
		public abstract System.Data.DataSet GetVotes();
		public abstract int SetVoteIsBackup(long voteId);
		public abstract long CreateVote(VoteInfo vote);
		public abstract bool UpdateVote(VoteInfo vote, System.Data.Common.DbTransaction dbTran);
		public abstract int DeleteVote(long voteId);
		public abstract int CreateVoteItem(VoteItemInfo voteItem, System.Data.Common.DbTransaction dbTran);
		public abstract bool DeleteVoteItem(long voteId, System.Data.Common.DbTransaction dbTran);
		public abstract VoteInfo GetVoteById(long voteId);
		public abstract IList<VoteItemInfo> GetVoteItems(long voteId);
		public abstract int GetVoteCounts(long voteId);
		public abstract string BackupData(string path);
		public abstract bool RestoreData(string bakFullName);
		public abstract void Restor();
		public abstract void WriteOperationLogEntry(OperationLogEntry entry);
		public abstract int DeleteLogs(string strIds);
		public abstract bool DeleteLog(long logId);
		public abstract bool DeleteAllLogs();
		public abstract DbQueryResult GetLogs(OperationLogQuery query);
		public abstract IList<string> GetOperationUserNames();
		public abstract void EnqueuCellphone(string cellphoneNumber, string subject);
		public abstract System.Data.DataTable DequeueCellphone();
		public abstract void DeleteQueuedCellphone(Guid cellphoneId);
		public abstract void QueueSendingFailure(IList<Guid> cellphoneIds);
	}
}
