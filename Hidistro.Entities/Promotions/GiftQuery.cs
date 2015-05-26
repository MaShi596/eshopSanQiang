using Hidistro.Core.Entities;
using System;
namespace Hidistro.Entities.Promotions
{
	public class GiftQuery
	{
		public Pagination Page
		{
			get;
			set;
		}
		public string Name
		{
			get;
			set;
		}
		public bool IsPromotion
		{
			get;
			set;
		}
		public GiftQuery()
		{
			this.Page = new Pagination();
		}
	}
}
