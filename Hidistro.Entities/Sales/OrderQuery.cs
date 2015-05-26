using Hidistro.Core.Entities;
using System;
namespace Hidistro.Entities.Sales
{
	public class OrderQuery : Pagination
	{
		public OrderStatus Status
		{
			get;
			set;
		}
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
		public string ProductName
		{
			get;
			set;
		}
		public string OrderId
		{
			get;
			set;
		}
		public string ShipId
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
		public int? PaymentType
		{
			get;
			set;
		}
		public int? GroupBuyId
		{
			get;
			set;
		}
		public int? ShippingModeId
		{
			get;
			set;
		}
		public int? IsPrinted
		{
			get;
			set;
		}
		public int? RegionId
		{
			get;
			set;
		}
	}
}
