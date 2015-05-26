using Hidistro.Core;
using System;
namespace Hidistro.AccountCenter.Profile
{
	public abstract class PersonalMasterProvider : PersonalProvider
	{
		private static readonly PersonalMasterProvider _defaultInstance;
		static PersonalMasterProvider()
		{
			PersonalMasterProvider._defaultInstance = (DataProviders.CreateInstance("Hidistro.AccountCenter.Data.ProfileData,Hidistro.AccountCenter.Data") as PersonalMasterProvider);
		}
		public static PersonalMasterProvider CreateInstance()
		{
			return PersonalMasterProvider._defaultInstance;
		}
	}
}
