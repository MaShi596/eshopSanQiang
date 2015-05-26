using Hidistro.Core;
using System;
namespace Hidistro.SaleSystem.Member
{
	public abstract class MemberMasterProvider : MemberProvider
	{
		private static readonly MemberMasterProvider _defaultInstance;
		static MemberMasterProvider()
		{
			MemberMasterProvider._defaultInstance = (DataProviders.CreateInstance("Hidistro.SaleSystem.Data.MemberData,Hidistro.SaleSystem.Data") as MemberMasterProvider);
		}
		public static MemberMasterProvider CreateInstance()
		{
			return MemberMasterProvider._defaultInstance;
		}
	}
}
