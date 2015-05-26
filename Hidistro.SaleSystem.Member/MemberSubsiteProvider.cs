using Hidistro.Core;
using System;
namespace Hidistro.SaleSystem.Member
{
	public abstract class MemberSubsiteProvider : MemberProvider
	{
		private static readonly MemberSubsiteProvider _defaultInstance;
		static MemberSubsiteProvider()
		{
			MemberSubsiteProvider._defaultInstance = (DataProviders.CreateInstance("Hidistro.SaleSystem.DistributionData.MemberData,Hidistro.SaleSystem.DistributionData") as MemberSubsiteProvider);
		}
		public static MemberSubsiteProvider CreateInstance()
		{
			return MemberSubsiteProvider._defaultInstance;
		}
	}
}
