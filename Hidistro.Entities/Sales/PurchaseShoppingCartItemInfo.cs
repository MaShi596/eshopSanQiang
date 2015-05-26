using System;
namespace Hidistro.Entities.Sales
{
	public class PurchaseShoppingCartItemInfo
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
		public decimal ItemListPrice
		{
			get;
			set;
		}
		public decimal ItemPurchasePrice
		{
			get;
			set;
		}
		public decimal CostPrice
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
		public decimal GetSubTotal()
		{
			return this.ItemPurchasePrice * this.Quantity;
		}
	}
}
