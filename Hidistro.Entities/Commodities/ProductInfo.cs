using Hidistro.Core;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Hidistro.Entities.Commodities
{
	public class ProductInfo
	{
		private System.Collections.Generic.Dictionary<string, SKUItem> skus;
		private SKUItem defaultSku;
		public SKUItem DefaultSku
		{
			get
			{
				SKUItem arg_23_0;
				if ((arg_23_0 = this.defaultSku) == null)
				{
					arg_23_0 = (this.defaultSku = this.Skus.Values.First<SKUItem>());
				}
				return arg_23_0;
			}
		}
		public System.Collections.Generic.Dictionary<string, SKUItem> Skus
		{
			get
			{
				System.Collections.Generic.Dictionary<string, SKUItem> arg_18_0;
				if ((arg_18_0 = this.skus) == null)
				{
					arg_18_0 = (this.skus = new System.Collections.Generic.Dictionary<string, SKUItem>());
				}
				return arg_18_0;
			}
		}
		public string SkuId
		{
			get
			{
				return this.DefaultSku.SkuId;
			}
		}
		public string SKU
		{
			get
			{
				return this.DefaultSku.SKU;
			}
		}
		public decimal Weight
		{
			get
			{
				return this.DefaultSku.Weight;
			}
		}
		public int Stock
		{
			get
			{
				return this.Skus.Values.Sum((SKUItem skuitem_0) => skuitem_0.Stock);
			}
		}
		public int AlertStock
		{
			get
			{
				return this.DefaultSku.AlertStock;
			}
		}
		public decimal CostPrice
		{
			get
			{
				return this.DefaultSku.CostPrice;
			}
		}
		public decimal MinSalePrice
		{
			get
			{
				decimal[] minSalePrice = new decimal[]
				{
					79228162514264337593543950335m
				};
				foreach (SKUItem current in 
					from skuitem_0 in this.Skus.Values
					where skuitem_0.SalePrice < minSalePrice[0]
					select skuitem_0)
				{
					minSalePrice[0] = current.SalePrice;
				}
				return minSalePrice[0];
			}
		}
		public decimal MaxSalePrice
		{
			get
			{
                decimal[] maxSalePrice = new decimal[1];
                foreach (SKUItem item in from skuitem_0 in this.Skus.Values
                                         where skuitem_0.SalePrice > maxSalePrice[0]
                                         select skuitem_0)
                {
                    maxSalePrice[0] = item.SalePrice;
                }
                return maxSalePrice[0];
			}
		}
		public decimal PurchasePrice
		{
			get
			{
				return this.DefaultSku.PurchasePrice;
			}
		}
		public int? TypeId
		{
			get;
			set;
		}
		public int CategoryId
		{
			get;
			set;
		}
		public int ProductId
		{
			get;
			set;
		}
		[HtmlCoding]
		public string ProductName
		{
			get;
			set;
		}
		public string ProductCode
		{
			get;
			set;
		}
		[HtmlCoding]
		public string ShortDescription
		{
			get;
			set;
		}
		public string Unit
		{
			get;
			set;
		}
		public string Description
		{
			get;
			set;
		}
		[HtmlCoding]
		public string Title
		{
			get;
			set;
		}
		[HtmlCoding]
		public string MetaDescription
		{
			get;
			set;
		}
		[HtmlCoding]
		public string MetaKeywords
		{
			get;
			set;
		}
		public ProductSaleStatus SaleStatus
		{
			get;
			set;
		}
		public System.DateTime AddedDate
		{
			get;
			set;
		}
		public int VistiCounts
		{
			get;
			set;
		}
		public int SaleCounts
		{
			get;
			set;
		}
		public int ShowSaleCounts
		{
			get;
			set;
		}
		public int DisplaySequence
		{
			get;
			set;
		}
		public string ImageUrl1
		{
			get;
			set;
		}
		public string ImageUrl2
		{
			get;
			set;
		}
		public string ImageUrl3
		{
			get;
			set;
		}
		public string ImageUrl4
		{
			get;
			set;
		}
		public string ImageUrl5
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
		public string ThumbnailUrl160
		{
			get;
			set;
		}
		public string ThumbnailUrl180
		{
			get;
			set;
		}
		public string ThumbnailUrl220
		{
			get;
			set;
		}
		public string ThumbnailUrl310
		{
			get;
			set;
		}
		public string ThumbnailUrl410
		{
			get;
			set;
		}
		public int LineId
		{
			get;
			set;
		}
		public decimal? MarketPrice
		{
			get;
			set;
		}
		public decimal LowestSalePrice
		{
			get;
			set;
		}
		public PenetrationStatus PenetrationStatus
		{
			get;
			set;
		}
		public int? BrandId
		{
			get;
			set;
		}
		public string MainCategoryPath
		{
			get;
			set;
		}
		public string ExtendCategoryPath
		{
			get;
			set;
		}
		public bool HasSKU
		{
			get;
			set;
		}
		public long TaobaoProductId
		{
			get;
			set;
		}
	}
}
