using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Distribution;
using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Web;
namespace Hidistro.Subsites.Store
{
	public static class SubsiteStoreHelper
	{
		public static AccountSummaryInfo GetMyAccountSummary()
		{
			return SubsiteStoreProvider.Instance().GetMyAccountSummary();
		}
		public static DbQueryResult GetMyBalanceDetails(BalanceDetailQuery query)
		{
			return SubsiteStoreProvider.Instance().GetMyBalanceDetails(query);
		}
		public static bool BalanceDrawRequest(BalanceDrawRequestInfo balanceDrawRequest)
		{
			return SubsiteStoreProvider.Instance().BalanceDrawRequest(balanceDrawRequest);
		}
		public static bool DistroHasDrawRequest()
		{
			return SubsiteStoreProvider.Instance().DistroHasDrawRequest();
		}
		public static bool AddInpourBalance(InpourRequestInfo inpourRequest)
		{
			return SubsiteStoreProvider.Instance().AddInpourBlance(inpourRequest);
		}
		public static InpourRequestInfo GetInpouRequest(string inpourId)
		{
			return SubsiteStoreProvider.Instance().GetInpouRequest(inpourId);
		}
		public static void RemoveInpourRequest(string inpourId)
		{
			SubsiteStoreProvider.Instance().RemoveInpourRequest(inpourId);
		}
		public static bool Recharge(BalanceDetailInfo balanceDetails)
		{
			bool result;
			if (!(result = SubsiteStoreProvider.Instance().IsRecharge(balanceDetails.InpourId)))
			{
				result = SubsiteStoreProvider.Instance().AddBalanceDetail(balanceDetails);
				SubsiteStoreProvider.Instance().RemoveInpourRequest(balanceDetails.InpourId);
			}
			return result;
		}
		public static PaymentModeInfo GetPaymentMode(string gateway)
		{
			return SubsiteStoreProvider.Instance().GetPaymentMode(gateway);
		}
		public static PaymentModeInfo GetPaymentMode(int modeId)
		{
			return SubsiteStoreProvider.Instance().GetPaymentMode(modeId);
		}
		public static IList<PaymentModeInfo> GetPaymentModes()
		{
			return SubsiteStoreProvider.Instance().GetPaymentModes();
		}
		public static IList<PaymentModeInfo> GetDistributorPaymentModes()
		{
			return SubsiteStoreProvider.Instance().GetDistributorPaymentModes();
		}
		public static CreateUserStatus CreateDistributor(Distributor distributor)
		{
			return Users.CreateUser(distributor, HiContext.Current.Config.RolesConfiguration.Distributor);
		}
		public static bool UpdateDistributor(Distributor distributor)
		{
			Globals.EntityCoding(distributor, true);
			return Users.UpdateUser(distributor);
		}
		public static Distributor GetDistributor()
		{
			IUser user = Users.GetUser(HiContext.Current.User.UserId, false);
			Distributor result;
			if (user != null && user.UserRole == UserRole.Distributor)
			{
				result = (user as Distributor);
			}
			else
			{
				result = null;
			}
			return result;
		}
		public static IList<FriendlyLinksInfo> GetFriendlyLinks()
		{
			return SubsiteStoreProvider.Instance().GetFriendlyLinks();
		}
		public static FriendlyLinksInfo GetFriendlyLink(int linkId)
		{
			return SubsiteStoreProvider.Instance().GetFriendlyLink(linkId);
		}
		public static bool UpdateFriendlyLink(FriendlyLinksInfo friendlyLink)
		{
			return null != friendlyLink && SubsiteStoreProvider.Instance().CreateUpdateDeleteFriendlyLink(friendlyLink, DataProviderAction.Update);
		}
		public static int FriendlyLinkDelete(int linkId)
		{
			return SubsiteStoreProvider.Instance().FriendlyLinkDelete(linkId);
		}
		public static bool CreateFriendlyLink(FriendlyLinksInfo friendlyLink)
		{
			return null != friendlyLink && SubsiteStoreProvider.Instance().CreateUpdateDeleteFriendlyLink(friendlyLink, DataProviderAction.Create);
		}
		public static void SwapFriendlyLinkSequence(int linkId, int replaceLinkId, int displaySequence, int replaceDisplaySequence)
		{
			SubsiteStoreProvider.Instance().SwapFriendlyLinkSequence(linkId, replaceLinkId, displaySequence, replaceDisplaySequence);
		}
		public static System.Data.DataSet GetVotes()
		{
			return SubsiteStoreProvider.Instance().GetVotes();
		}
		public static int SetVoteIsBackup(long voteId)
		{
			return SubsiteStoreProvider.Instance().SetVoteIsBackup(voteId);
		}
		public static int CreateVote(VoteInfo vote)
		{
			int num = 0;
			long num2 = SubsiteStoreProvider.Instance().CreateVote(vote);
			if (num2 > 0L)
			{
				num = 1;
				if (vote.VoteItems != null)
				{
					foreach (VoteItemInfo current in vote.VoteItems)
					{
						current.VoteId = num2;
						current.ItemCount = 0;
						num += SubsiteStoreProvider.Instance().CreateVoteItem(current, null);
					}
				}
			}
			return num;
		}
		public static bool UpdateVote(VoteInfo vote)
		{
			Database database = DatabaseFactory.CreateDatabase();
			bool result;
			using (System.Data.Common.DbConnection dbConnection = database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					if (!SubsiteStoreProvider.Instance().UpdateVote(vote, dbTransaction))
					{
						dbTransaction.Rollback();
						result = false;
					}
					else
					{
						if (!SubsiteStoreProvider.Instance().DeleteVoteItem(vote.VoteId, dbTransaction))
						{
							dbTransaction.Rollback();
							result = false;
						}
						else
						{
							int num = 0;
							if (vote.VoteItems != null)
							{
								foreach (VoteItemInfo current in vote.VoteItems)
								{
									current.VoteId = vote.VoteId;
									current.ItemCount = 0;
									num += SubsiteStoreProvider.Instance().CreateVoteItem(current, dbTransaction);
								}
								if (num < vote.VoteItems.Count)
								{
									dbTransaction.Rollback();
									result = false;
									return result;
								}
							}
							dbTransaction.Commit();
							result = true;
						}
					}
				}
				catch
				{
					dbTransaction.Rollback();
					result = false;
				}
				finally
				{
					dbConnection.Close();
				}
			}
			return result;
		}
		public static int DeleteVote(long voteId)
		{
			return SubsiteStoreProvider.Instance().DeleteVote(voteId);
		}
		public static VoteInfo GetVoteById(long voteId)
		{
			return SubsiteStoreProvider.Instance().GetVoteById(voteId);
		}
		public static IList<VoteItemInfo> GetVoteItems(long voteId)
		{
			return SubsiteStoreProvider.Instance().GetVoteItems(voteId);
		}
		public static int GetVoteCounts(long voteId)
		{
			return SubsiteStoreProvider.Instance().GetVoteCounts(voteId);
		}
		public static void DeleteHotKeywords(int int_0)
		{
			SubsiteStoreProvider.Instance().DeleteHotKeywords(int_0);
		}
		public static void SwapHotWordsSequence(int int_0, int replaceHid, int displaySequence, int replaceDisplaySequence)
		{
			SubsiteStoreProvider.Instance().SwapHotWordsSequence(int_0, replaceHid, displaySequence, replaceDisplaySequence);
		}
		public static void UpdateHotWords(int int_0, int categoryId, string hotKeyWords)
		{
			SubsiteStoreProvider.Instance().UpdateHotWords(int_0, categoryId, hotKeyWords);
		}
		public static void AddHotkeywords(int categoryId, string keywords)
		{
			SubsiteStoreProvider.Instance().AddHotkeywords(categoryId, keywords);
		}
		public static System.Data.DataTable GetHotKeywords()
		{
			return SubsiteStoreProvider.Instance().GetHotKeywords();
		}
		public static SiteRequestInfo GetMySiteRequest()
		{
			return SubsiteStoreProvider.Instance().GetMySiteRequest();
		}
		public static bool AddSiteRequest(SiteRequestInfo siteRequest)
		{
			return SubsiteStoreProvider.Instance().AddSiteRequest(siteRequest);
		}
		public static void DeleteSiteRequest()
		{
			SubsiteStoreProvider.Instance().DeleteSiteRequest();
		}
		public static bool IsExitSiteUrl(string siteUrl)
		{
			return SubsiteStoreProvider.Instance().IsExitSiteUrl(siteUrl);
		}
		public static LoginUserStatus ValidLogin(Distributor distributor)
		{
			LoginUserStatus result;
			if (distributor == null)
			{
				result = LoginUserStatus.InvalidCredentials;
			}
			else
			{
				result = Users.ValidateUser(distributor);
			}
			return result;
		}
		public static string UploadLinkImage(HttpPostedFile postedFile)
		{
			string result;
			if (!ResourcesHelper.CheckPostedFile(postedFile))
			{
				result = string.Empty;
			}
			else
			{
				string text = HiContext.Current.GetStoragePath() + "/link/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
				postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + text));
				result = text;
			}
			return result;
		}
		public static string UploadLogo(HttpPostedFile postedFile)
		{
			string result;
			if (!ResourcesHelper.CheckPostedFile(postedFile))
			{
				result = string.Empty;
			}
			else
			{
				string text = HiContext.Current.GetStoragePath() + "/" + ResourcesHelper.GenerateFilename(Path.GetExtension(postedFile.FileName));
				postedFile.SaveAs(HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + text));
				result = text;
			}
			return result;
		}
		public static void DeleteImage(string imageUrl)
		{
			if (!string.IsNullOrEmpty(imageUrl))
			{
				try
				{
					string path = HiContext.Current.Context.Request.MapPath(Globals.ApplicationPath + imageUrl);
					if (File.Exists(path))
					{
						File.Delete(path);
					}
				}
				catch
				{
				}
			}
		}
		public static DistributorGradeInfo GetDistributorGradeInfo(int gradeId)
		{
			return SubsiteStoreProvider.Instance().GetDistributorGradeInfo(gradeId);
		}
		public static bool BindTaobaoSetting(bool isUserHomeSiteApp, string topkey, string topSecret)
		{
			return HiContext.Current.User.UserRole == UserRole.Distributor && SubsiteStoreProvider.Instance().BindTaobaoSetting(isUserHomeSiteApp, topkey, topSecret);
		}
	}
}
