using System;
namespace Hidistro.Entities.Sales
{
	public class LineItemInfo
	{
		public string SkuId
		{
			get;
			set;
		}
		public int ProductId
		{
			get;
			set;
		}
		public string SKU
		{
			get;
			set;
		}
		public int Quantity
		{
			get;
			set;
		}
		public int ShipmentQuantity
		{
			get;
			set;
		}
		public decimal ItemCostPrice
		{
			get;
			set;
		}
		public decimal ItemListPrice
		{
			get;
			set;
		}
		public decimal ItemAdjustedPrice
		{
			get;
			set;
		}
		public string ItemDescription
		{
			get;
			set;
		}
		public string ThumbnailsUrl
		{
			get;
			set;
		}
		public decimal ItemWeight
		{
			get;
			set;
		}
		public string SKUContent
		{
			get;
			set;
		}
		public int PromotionId
		{
			get;
			set;
		}
		public string PromotionName
		{
			get;
			set;
		}
		public decimal GetSubTotal()
		{
			return this.ItemAdjustedPrice * this.Quantity;
		}
	}
}
