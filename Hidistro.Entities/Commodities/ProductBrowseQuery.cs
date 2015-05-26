using Hidistro.Core.Entities;
using System;
using System.Collections.Generic;
namespace Hidistro.Entities.Commodities
{
	public class ProductBrowseQuery : Pagination
	{
		private System.Collections.Generic.IList<AttributeValueInfo> attributeValues;
		public bool IsPrecise
		{
			get;
			set;
		}
		public string TagIds
		{
			get;
			set;
		}
		public string Keywords
		{
			get;
			set;
		}
		public string ProductCode
		{
			get;
			set;
		}
		public int? CategoryId
		{
			get;
			set;
		}
		public int? BrandId
		{
			get;
			set;
		}
		public decimal? MinSalePrice
		{
			get;
			set;
		}
		public decimal? MaxSalePrice
		{
			get;
			set;
		}
		public System.Collections.Generic.IList<AttributeValueInfo> AttributeValues
		{
			get
			{
				if (this.attributeValues == null)
				{
					this.attributeValues = new System.Collections.Generic.List<AttributeValueInfo>();
				}
				return this.attributeValues;
			}
			set
			{
				this.attributeValues = value;
			}
		}
	}
}
