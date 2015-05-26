using System;
namespace Hidistro.Entities.Sales
{
	public class UserPointInfo
	{
		public long JournalNumber
		{
			get;
			set;
		}
		public string OrderId
		{
			get;
			set;
		}
		public int UserId
		{
			get;
			set;
		}
		public System.DateTime TradeDate
		{
			get;
			set;
		}
		public UserPointTradeType TradeType
		{
			get;
			set;
		}
		public int? Increased
		{
			get;
			set;
		}
		public int? Reduced
		{
			get;
			set;
		}
		public int Points
		{
			get;
			set;
		}
		public string Remark
		{
			get;
			set;
		}
	}
}
