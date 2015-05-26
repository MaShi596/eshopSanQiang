using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Distribution;
using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace Hidistro.Subsites.Store
{
	public abstract class SubsiteStoreProvider
	{
		private static readonly SubsiteStoreProvider _defaultInstance;
		static SubsiteStoreProvider()
		{
			SubsiteStoreProvider._defaultInstance = (DataProviders.CreateInstance("Hidistro.Subsites.Data.StoreData,Hidistro.Subsites.Data") as SubsiteStoreProvider);
		}
		public static SubsiteStoreProvider Instance()
		{
			return SubsiteStoreProvider._defaultInstance;
		}
		public abstract IList<FriendlyLinksInfo> GetFriendlyLinks();
		public abstract FriendlyLinksInfo GetFriendlyLink(int linkId);
		public abstract bool CreateUpdateDeleteFriendlyLink(FriendlyLinksInfo friendlyLink, DataProviderAction action);
		public abstract void SwapFriendlyLinkSequence(int linkId, int replaceLinkId, int displaySequence, int replaceDisplaySequence);
		public abstract int FriendlyLinkDelete(int linkId);
		public abstract System.Data.DataSet GetVotes();
		public abstract int SetVoteIsBackup(long voteId);
		public abstract long CreateVote(VoteInfo vote);
		public abstract bool UpdateVote(VoteInfo vote, System.Data.Common.DbTransaction dbTran);
		public abstract int DeleteVote(long voteId);
		public abstract int CreateVoteItem(VoteItemInfo voteItem, System.Data.Common.DbTransaction dbTran);
		public abstract bool DeleteVoteItem(long voteItemId, System.Data.Common.DbTransaction dbTran);
		public abstract VoteInfo GetVoteById(long voteId);
		public abstract IList<VoteItemInfo> GetVoteItems(long voteId);
		public abstract int GetVoteCounts(long voteId);
		public abstract void DeleteHotKeywords(int hId);
		public abstract void SwapHotWordsSequence(int int_0, int replaceHid, int displaySequence, int replaceDisplaySequence);
		public abstract void UpdateHotWords(int int_0, int categoryId, string hotKeyWords);
		public abstract void AddHotkeywords(int categoryId, string keywords);
		public abstract System.Data.DataTable GetHotKeywords();
		public abstract AccountSummaryInfo GetMyAccountSummary();
		public abstract DbQueryResult GetMyBalanceDetails(BalanceDetailQuery query);
		public abstract bool BalanceDrawRequest(BalanceDrawRequestInfo balanceDrawRequest);
		public abstract bool AddInpourBlance(InpourRequestInfo inpourRequest);
		public abstract InpourRequestInfo GetInpouRequest(string inpourId);
		public abstract void RemoveInpourRequest(string inpourId);
		public abstract bool AddBalanceDetail(BalanceDetailInfo balanceDetails);
		public abstract bool IsRecharge(string inpourId);
		public abstract bool DistroHasDrawRequest();
		public abstract PaymentModeInfo GetPaymentMode(string gateway);
		public abstract PaymentModeInfo GetPaymentMode(int modeId);
		public abstract IList<PaymentModeInfo> GetPaymentModes();
		public abstract IList<PaymentModeInfo> GetDistributorPaymentModes();
		public abstract SiteRequestInfo GetMySiteRequest();
		public abstract bool AddSiteRequest(SiteRequestInfo siteRequest);
		public abstract void DeleteSiteRequest();
		public abstract bool IsExitSiteUrl(string siteUrl);
		public abstract DistributorGradeInfo GetDistributorGradeInfo(int gradeId);
		public abstract bool BindTaobaoSetting(bool isUserHomeSiteApp, string topkey, string topSecret);
	}
}
