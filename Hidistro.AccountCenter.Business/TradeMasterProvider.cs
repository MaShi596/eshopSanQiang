using Hidistro.Core;
using System;
namespace Hidistro.AccountCenter.Business
{
	public abstract class TradeMasterProvider : TradeProvider
	{
		private static readonly TradeMasterProvider _defaultInstance;
		static TradeMasterProvider()
		{
			TradeMasterProvider._defaultInstance = (DataProviders.CreateInstance("Hidistro.AccountCenter.Data.BusinessData,Hidistro.AccountCenter.Data") as TradeMasterProvider);
		}
		public static TradeMasterProvider CreateInstance()
		{
			return TradeMasterProvider._defaultInstance;
		}
	}
}
