using System;
namespace Hidistro.Messages
{
	internal static class MessageType
	{
		internal const string ChangedDealPassword = "ChangedDealPassword";
		internal const string ChangedPassword = "ChangedPassword";
		internal const string ForgottenPassword = "ForgottenPassword";
		internal const string NewUserAccountCreated = "NewUserAccountCreated";
		internal const string OrderCreated = "OrderCreated";
		internal const string OrderPayment = "OrderPayment";
		internal const string OrderShipping = "OrderShipping";
		internal const string OrderRefund = "OrderRefund";
		internal const string OrderClosed = "OrderClosed";
		internal const string AcceptDistributorRequest = "AcceptDistributorRequest";
	}
}
