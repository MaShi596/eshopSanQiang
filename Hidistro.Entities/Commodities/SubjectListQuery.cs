using Hidistro.Core.Entities;
using System;
using System.Collections.Generic;
namespace Hidistro.Entities.Commodities
{
	public class SubjectListQuery : Pagination
	{
		private System.Collections.Generic.IList<AttributeValueInfo> attributeValues;
		public int TagId
		{
			get;
			set;
		}
		public string CategoryIds
		{
			get;
			set;
		}
		public int MaxNum
		{
			get;
			set;
		}
		public decimal? MinPrice
		{
			get;
			set;
		}
		public decimal? MaxPrice
		{
			get;
			set;
		}
		public string Keywords
		{
			get;
			set;
		}
		public int? BrandCategoryId
		{
			get;
			set;
		}
		public int? ProductTypeId
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
