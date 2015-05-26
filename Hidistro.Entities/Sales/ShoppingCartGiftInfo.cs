using System;
namespace Hidistro.Entities.Sales
{
	public class ShoppingCartGiftInfo
	{
		public int UserId
		{
			get;
			set;
		}
		public int GiftId
		{
			get;
			set;
		}
		public string Name
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
		public int NeedPoint
		{
			get;
			set;
		}
		public int Quantity
		{
			get;
			set;
		}
		public string ThumbnailUrl40
		{
			get;
			set;
		}
		public string ThumbnailUrl60
		{
			get;
			set;
		}
		public string ThumbnailUrl100
		{
			get;
			set;
		}
		public int PromoType
		{
			get;
			set;
		}
		public int SubPointTotal
		{
			get
			{
				int result;
				if (this.PromoType <= 0)
				{
					result = this.NeedPoint * this.Quantity;
				}
				else
				{
					result = 0;
				}
				return result;
			}
		}
	}
}
