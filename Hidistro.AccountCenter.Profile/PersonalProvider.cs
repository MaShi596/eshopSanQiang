using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using System;
using System.Collections.Generic;
namespace Hidistro.AccountCenter.Profile
{
	public abstract class PersonalProvider
	{
		public static PersonalProvider Instance()
		{
			PersonalProvider result;
			if (HiContext.Current.SiteSettings.IsDistributorSettings)
			{
				result = PersonalSubsiteProvider.CreateInstance();
			}
			else
			{
				result = PersonalMasterProvider.CreateInstance();
			}
			return result;
		}
		public abstract DbQueryResult GetBalanceDetails(BalanceDetailQuery query);
		public abstract bool AddBalanceDetail(BalanceDetailInfo balanceDetails);
		public abstract bool IsRecharge(string inpourId);
		public abstract bool AddInpourBlance(InpourRequestInfo inpourRequest);
		public abstract InpourRequestInfo GetInpourBlance(string inpourId);
		public abstract void RemoveInpourRequest(string inpourId);
		public abstract void GetStatisticsNum(out int noPayOrderNum, out int noReadMessageNum, out int noReplyLeaveCommentNum);
		public abstract bool ViewProductConsultations();
		public abstract bool BalanceDrawRequest(BalanceDrawRequestInfo balanceDrawRequest);
		public abstract int GetShippingAddressCount(int userId);
		public abstract bool CreateUpdateDeleteShippingAddress(ShippingAddressInfo shippingAddress, DataProviderAction action);
		public abstract int AddShippingAddress(ShippingAddressInfo shippingAddress);
		public abstract IList<ShippingAddressInfo> GetShippingAddress(int userId);
		public abstract ShippingAddressInfo GetUserShippingAddress(int shippingId);
		public abstract MemberGradeInfo GetMemberGrade(int gradeId);
		public abstract IList<MemberGradeInfo> GetMemberGrades();
		public abstract DbQueryResult GetMyReferralMembers(MemberQuery query);
	}
}
