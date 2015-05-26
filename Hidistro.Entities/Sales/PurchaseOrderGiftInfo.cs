using System;
namespace Hidistro.Entities.Sales
{
	public class PurchaseOrderGiftInfo
	{
		public string PurchaseOrderId
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
		public decimal PurchasePrice
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
		public decimal GetSubTotal()
		{
			return this.PurchasePrice * this.Quantity;
		}
	}
}
