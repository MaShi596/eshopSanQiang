using Hidistro.Core.Entities;
using System;
namespace Hidistro.Entities.Sales
{
	public class PurchaseOrderQuery : Pagination
	{
		public OrderStatus PurchaseStatus
		{
			get;
			set;
		}
		public string DistributorName
		{
			get;
			set;
		}
		public string ProductName
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
		public string PurchaseOrderId
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
		public bool IsManualPurchaseOrder
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
	}
}
