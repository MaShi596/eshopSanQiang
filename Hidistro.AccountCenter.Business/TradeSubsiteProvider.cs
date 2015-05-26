using Hidistro.Core;
using System;
namespace Hidistro.AccountCenter.Business
{
	public abstract class TradeSubsiteProvider : TradeProvider
	{
		private static readonly TradeSubsiteProvider _defaultInstance;
		static TradeSubsiteProvider()
		{
			TradeSubsiteProvider._defaultInstance = (DataProviders.CreateInstance("Hidistro.AccountCenter.DistributionData.BusinessData,Hidistro.AccountCenter.DistributionData") as TradeSubsiteProvider);
		}
		public static TradeSubsiteProvider CreateInstance()
		{
			return TradeSubsiteProvider._defaultInstance;
		}
	}
}
