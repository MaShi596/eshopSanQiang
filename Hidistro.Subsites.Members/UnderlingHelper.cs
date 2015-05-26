using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using System;
using System.Collections.Generic;
using System.Data;
namespace Hidistro.Subsites.Members
{
	public static class UnderlingHelper
	{
		public static DbQueryResult GetMembers(MemberQuery query)
		{
			return UnderlingProvider.Instance().GetMembers(query);
		}
		public static DataTable GetMembersNopage(MemberQuery query, IList<string> fields)
		{
			return UnderlingProvider.Instance().GetMembersNopage(query, fields);
		}
		public static Member GetMember(int userId)
		{
			IUser user = Users.GetUser(userId, false);
			Member result;
			if (user != null && user.UserRole == UserRole.Underling)
			{
				result = (user as Member);
			}
			else
			{
				result = null;
			}
			return result;
		}
		public static bool DeleteMember(int userId)
		{
			IUser user = Users.GetUser(userId);
			bool result;
			if (result = UnderlingProvider.Instance().DeleteMember(userId))
			{
				Users.ClearUserCache(user);
			}
			return result;
		}
		public static bool Update(Member underling)
		{
			return Users.UpdateUser(underling);
		}
		public static IList<UserStatisticsForDate> GetUserIncrease(int? year, int? month, int? days)
		{
			return UnderlingProvider.Instance().GetUserIncrease(year, month, days);
		}
		public static DataTable GetUnderlingStatistics(SaleStatisticsQuery query, out int total)
		{
			return UnderlingProvider.Instance().GetUnderlingStatistics(query, out total);
		}
		public static DataTable GetUnderlingStatisticsNoPage(SaleStatisticsQuery query)
		{
			return UnderlingProvider.Instance().GetUnderlingStatisticsNoPage(query);
		}
		public static IList<MemberGradeInfo> GetUnderlingGrades()
		{
			return UnderlingProvider.Instance().GetUnderlingGrades();
		}
		public static bool CreateUnderlingGrade(MemberGradeInfo underlingGrade)
		{
			Globals.EntityCoding(underlingGrade, true);
			return UnderlingProvider.Instance().CreateUnderlingGrade(underlingGrade);
		}
		public static bool UpdateUnderlingGrade(MemberGradeInfo underlingGrade)
		{
			Globals.EntityCoding(underlingGrade, true);
			return UnderlingProvider.Instance().UpdateUnderlingGrade(underlingGrade);
		}
		public static bool HasSamePointMemberGrade(MemberGradeInfo memberGrade)
		{
			return UnderlingProvider.Instance().HasSamePointMemberGrade(memberGrade);
		}
		public static bool DeleteUnderlingGrade(int gradeId)
		{
			bool result;
			if (result = UnderlingProvider.Instance().DeleteUnderlingGrade(gradeId))
			{
				UnderlingProvider.Instance().DeleteSKUMemberPrice(gradeId);
			}
			return result;
		}
		public static void SetDefalutUnderlingGrade(int gradeId)
		{
			UnderlingProvider.Instance().SetDefalutUnderlingGrade(gradeId);
		}
		public static MemberGradeInfo GetMemberGrade(int gradeId)
		{
			return UnderlingProvider.Instance().GetMemberGrade(gradeId);
		}
		public static bool AddUnderlingBalanceDetail(BalanceDetailInfo balanceDetails)
		{
			bool result;
			if (result = UnderlingProvider.Instance().AddUnderlingBalanceDetail(balanceDetails))
			{
				Users.ClearUserCache(Users.GetUser(balanceDetails.UserId));
			}
			return result;
		}
		public static DbQueryResult GetBalanceDetails(BalanceDetailQuery query)
		{
			return UnderlingProvider.Instance().GetBalanceDetails(query);
		}
		public static DbQueryResult GetUnderlingBlanceList(MemberQuery query)
		{
			return UnderlingProvider.Instance().GetUnderlingBlanceList(query);
		}
		public static bool DealBalanceDrawRequest(int userId, bool agree)
		{
			bool result;
			if (result = UnderlingProvider.Instance().DealBalanceDrawRequest(userId, agree))
			{
				Users.ClearUserCache(Users.GetUser(userId));
			}
			return result;
		}
		public static DbQueryResult GetBalanceDrawRequests(BalanceDrawRequestQuery query)
		{
			return UnderlingProvider.Instance().GetBalanceDrawRequests(query);
		}
	}
}
