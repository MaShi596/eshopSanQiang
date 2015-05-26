using Hidistro.Core.Entities;
using System;
namespace Hidistro.Entities.Sales
{
	[System.Serializable]
	public class UserOrderQuery : Pagination
	{
		public string UserName
		{
			get;
			set;
		}
		public string ShipTo
		{
			get;
			set;
		}
		public string OrderId
		{
			get;
			set;
		}
		public System.DateTime? StartDate
		{
			get;
			set;
		}
		public System.DateTime? EndDate
		{
			get;
			set;
		}
	}
}
