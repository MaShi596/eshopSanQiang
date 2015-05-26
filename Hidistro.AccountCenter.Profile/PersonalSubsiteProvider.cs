using Hidistro.Core;
using System;
namespace Hidistro.AccountCenter.Profile
{
	public abstract class PersonalSubsiteProvider : PersonalProvider
	{
		private static readonly PersonalSubsiteProvider _defaultInstance;
		static PersonalSubsiteProvider()
		{
			PersonalSubsiteProvider._defaultInstance = (DataProviders.CreateInstance("Hidistro.AccountCenter.DistributionData.ProfileData,Hidistro.AccountCenter.DistributionData") as PersonalSubsiteProvider);
		}
		public static PersonalSubsiteProvider CreateInstance()
		{
			return PersonalSubsiteProvider._defaultInstance;
		}
	}
}
