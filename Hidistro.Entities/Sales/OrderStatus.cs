using System;
namespace Hidistro.Entities.Sales
{
	public enum OrderStatus
	{
		All,
		WaitBuyerPay,
		BuyerAlreadyPaid,
		SellerAlreadySent,
		Closed,
		Finished,
		ApplyForRefund,
		ApplyForReturns,
		ApplyForReplacement,
		Refunded,
		Returned,
		History = 99
	}
}
