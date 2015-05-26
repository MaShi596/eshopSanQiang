using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using System;
using System.Collections.Generic;
namespace Hidistro.AccountCenter.Profile
{
	public static class PersonalHelper
	{
		public static DbQueryResult GetBalanceDetails(BalanceDetailQuery query)
		{
			return PersonalProvider.Instance().GetBalanceDetails(query);
		}
		public static bool Recharge(BalanceDetailInfo balanceDetails)
		{
			bool result;
			if (!(result = PersonalProvider.Instance().IsRecharge(balanceDetails.InpourId)))
			{
				result = PersonalProvider.Instance().AddBalanceDetail(balanceDetails);
				PersonalProvider.Instance().RemoveInpourRequest(balanceDetails.InpourId);
			}
			return result;
		}
		public static bool AddInpourBlance(InpourRequestInfo inpourRequest)
		{
			return PersonalProvider.Instance().AddInpourBlance(inpourRequest);
		}
		public static InpourRequestInfo GetInpourBlance(string inpourId)
		{
			return PersonalProvider.Instance().GetInpourBlance(inpourId);
		}
		public static void RemoveInpourRequest(string inpourId)
		{
			PersonalProvider.Instance().RemoveInpourRequest(inpourId);
		}
		public static void GetStatisticsNum(out int noPayOrderNum, out int noReadMessageNum, out int noReplyLeaveCommentNum)
		{
			PersonalProvider.Instance().GetStatisticsNum(out noPayOrderNum, out noReadMessageNum, out noReplyLeaveCommentNum);
		}
		public static bool ViewProductConsultations()
		{
			return PersonalProvider.Instance().ViewProductConsultations();
		}
		public static bool BalanceDrawRequest(BalanceDrawRequestInfo balanceDrawRequest)
		{
			Globals.EntityCoding(balanceDrawRequest, true);
			bool result;
			if (result = PersonalProvider.Instance().BalanceDrawRequest(balanceDrawRequest))
			{
				Users.ClearUserCache(HiContext.Current.User);
			}
			return result;
		}
		public static int GetShippingAddressCount(int userId)
		{
			return PersonalProvider.Instance().GetShippingAddressCount(userId);
		}
		public static bool CreateShippingAddress(ShippingAddressInfo shippingAddress)
		{
			bool result;
			if (null == shippingAddress)
			{
				result = false;
			}
			else
			{
				Globals.EntityCoding(shippingAddress, true);
				result = PersonalProvider.Instance().CreateUpdateDeleteShippingAddress(shippingAddress, DataProviderAction.Create);
			}
			return result;
		}
		public static int AddShippingAddress(ShippingAddressInfo shippingAddress)
		{
			int result;
			if (null == shippingAddress)
			{
				result = 0;
			}
			else
			{
				Globals.EntityCoding(shippingAddress, true);
				result = PersonalProvider.Instance().AddShippingAddress(shippingAddress);
			}
			return result;
		}
		public static bool UpdateShippingAddress(ShippingAddressInfo shippingAddress)
		{
			bool result;
			if (null == shippingAddress)
			{
				result = false;
			}
			else
			{
				Globals.EntityCoding(shippingAddress, true);
				result = PersonalProvider.Instance().CreateUpdateDeleteShippingAddress(shippingAddress, DataProviderAction.Update);
			}
			return result;
		}
		public static bool DeleteShippingAddress(int shippingId)
		{
			ShippingAddressInfo shippingAddressInfo = new ShippingAddressInfo();
			shippingAddressInfo.ShippingId = shippingId;
			return PersonalProvider.Instance().CreateUpdateDeleteShippingAddress(shippingAddressInfo, DataProviderAction.Delete);
		}
		public static IList<ShippingAddressInfo> GetShippingAddress(int userId)
		{
			return PersonalProvider.Instance().GetShippingAddress(userId);
		}
		public static ShippingAddressInfo GetUserShippingAddress(int shippingId)
		{
			return PersonalProvider.Instance().GetUserShippingAddress(shippingId);
		}
		public static MemberGradeInfo GetMemberGrade(int gradeId)
		{
			return PersonalProvider.Instance().GetMemberGrade(gradeId);
		}
		public static IList<MemberGradeInfo> GetMemberGrades()
		{
			return PersonalProvider.Instance().GetMemberGrades();
		}
		public static DbQueryResult GetMyReferralMembers(MemberQuery query)
		{
			return PersonalProvider.Instance().GetMyReferralMembers(query);
		}
	}
}
