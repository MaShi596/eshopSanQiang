using System;
namespace Hidistro.Entities.Sales
{
	public class OrderGiftInfo
	{
		public string OrderId
		{
			get;
			set;
		}
		public int GiftId
		{
			get;
			set;
		}
		public string GiftName
		{
			get;
			set;
		}
		public decimal CostPrice
		{
			get;
			set;
		}
		public int Quantity
		{
			get;
			set;
		}
		public string ThumbnailsUrl
		{
			get;
			set;
		}
		public int PromoteType
		{
			get;
			set;
		}
	}
}
