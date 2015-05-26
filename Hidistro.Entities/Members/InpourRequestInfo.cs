using System;
namespace Hidistro.Entities.Members
{
	public class InpourRequestInfo
	{
		public string InpourId
		{
			get;
			set;
		}
		public System.DateTime TradeDate
		{
			get;
			set;
		}
		public decimal InpourBlance
		{
			get;
			set;
		}
		public int UserId
		{
			get;
			set;
		}
		public int PaymentId
		{
			get;
			set;
		}
	}
}
