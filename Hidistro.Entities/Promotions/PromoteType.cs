using System;
namespace Hidistro.Entities.Promotions
{
	public enum PromoteType
	{
		NotSet,
		Discount,
		Amount,
		Reduced,
		QuantityDiscount,
		SentGift,
		SentProduct,
		FullAmountDiscount = 11,
		FullAmountReduced,
		FullQuantityDiscount,
		FullQuantityReduced,
		FullAmountSentGift,
		FullAmountSentTimesPoint,
		FullAmountSentFreight
	}
}
