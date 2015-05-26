using System;
namespace Hidistro.Entities.Sales
{
	public class PurchaseDebitNote
	{
		public string NoteId
		{
			get;
			set;
		}
		public string PurchaseOrderId
		{
			get;
			set;
		}
		public string Username
		{
			get;
			set;
		}
		public decimal PayMoney
		{
			get;
			set;
		}
		public decimal? PayGateMoney
		{
			get;
			set;
		}
		public string PayMethod
		{
			get;
			set;
		}
		public string Operator
		{
			get;
			set;
		}
		public System.DateTime PayTime
		{
			get;
			set;
		}
		public string Remark
		{
			get;
			set;
		}
	}
}
