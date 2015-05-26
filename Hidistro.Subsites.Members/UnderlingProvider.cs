using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using System;
using System.Collections.Generic;
using System.Data;
namespace Hidistro.Subsites.Members
{
	public abstract class UnderlingProvider
	{
		private static readonly UnderlingProvider _defaultInstance;
		static UnderlingProvider()
		{
			UnderlingProvider._defaultInstance = (DataProviders.CreateInstance("Hidistro.Subsites.Data.UnderlingData,Hidistro.Subsites.Data") as UnderlingProvider);
		}
		public static UnderlingProvider Instance()
		{
			return UnderlingProvider._defaultInstance;
		}
		public abstract DbQueryResult GetMembers(MemberQuery query);
		public abstract DataTable GetMembersNopage(MemberQuery query, IList<string> fields);
		public abstract bool DeleteMember(int userId);
		public abstract IList<UserStatisticsForDate> GetUserIncrease(int? year, int? month, int? days);
		public abstract DataTable GetUnderlingStatistics(SaleStatisticsQuery query, out int total);
		public abstract DataTable GetUnderlingStatisticsNoPage(SaleStatisticsQuery query);
		public abstract IList<MemberGradeInfo> GetUnderlingGrades();
		public abstract bool CreateUnderlingGrade(MemberGradeInfo underlingGrade);
		public abstract bool UpdateUnderlingGrade(MemberGradeInfo underlingGrade);
		public abstract bool HasSamePointMemberGrade(MemberGradeInfo memberGrade);
		public abstract bool DeleteUnderlingGrade(int gradeId);
		public abstract void DeleteSKUMemberPrice(int gradeId);
		public abstract void SetDefalutUnderlingGrade(int gradeId);
		public abstract MemberGradeInfo GetMemberGrade(int gradeId);
		public abstract bool AddUnderlingBalanceDetail(BalanceDetailInfo balanceDetails);
		public abstract DbQueryResult GetBalanceDetails(BalanceDetailQuery query);
		public abstract DbQueryResult GetUnderlingBlanceList(MemberQuery query);
		public abstract bool DealBalanceDrawRequest(int userId, bool agree);
		public abstract DbQueryResult GetBalanceDrawRequests(BalanceDrawRequestQuery query);
	}
}
