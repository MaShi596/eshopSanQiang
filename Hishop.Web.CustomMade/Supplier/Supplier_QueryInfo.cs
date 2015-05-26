using Hidistro.Core.Entities;
using System;
namespace Hishop.Web.CustomMade.Supplier
{
	public class Supplier_QueryInfo : Pagination
	{
		public DateTime? StartDate
		{
			get;
			set;
		}
		public DateTime? EndDate
		{
			get;
			set;
		}
		public string Code
		{
			get;
			set;
		}
		public string DateDay
		{
			get;
			set;
		}
		public string Status
		{
			get;
			set;
		}
	}
}
