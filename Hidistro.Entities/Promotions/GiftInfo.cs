using Hidistro.Core;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
namespace Hidistro.Entities.Promotions
{
	public class GiftInfo
	{
		public int GiftId
		{
			get;
			set;
		}
		[HtmlCoding, StringLengthValidator(1, 60, Ruleset = "ValGift", MessageTemplate = "礼品名称不能为空，长度限制在1-60个字符之间")]
		public string Name
		{
			get;
			set;
		}
		[HtmlCoding, StringLengthValidator(0, 300, Ruleset = "ValGift", MessageTemplate = "礼品简单介绍长度限制在0-300个字符之间")]
		public string ShortDescription
		{
			get;
			set;
		}
		[StringLengthValidator(0, 10, Ruleset = "ValGift", MessageTemplate = "计量单位长度限制在0-10个字符之间")]
		public string Unit
		{
			get;
			set;
		}
		public string LongDescription
		{
			get;
			set;
		}
		[HtmlCoding, StringLengthValidator(0, 100, Ruleset = "ValGift", MessageTemplate = "详细页标题长度限制在0-100个字符之间")]
		public string Title
		{
			get;
			set;
		}
		[HtmlCoding, StringLengthValidator(0, 260, Ruleset = "ValGift", MessageTemplate = "详细页描述长度限制在0-260个字符之间")]
		public string Meta_Description
		{
			get;
			set;
		}
		[HtmlCoding, StringLengthValidator(0, 160, Ruleset = "ValGift", MessageTemplate = "详细页关键字长度限制在0-160个字符之间")]
		public string Meta_Keywords
		{
			get;
			set;
		}
		public string ImageUrl
		{
			get;
			set;
		}
		public string ThumbnailUrl40
		{
			get;
			set;
		}
		public string ThumbnailUrl60
		{
			get;
			set;
		}
		public string ThumbnailUrl100
		{
			get;
			set;
		}
		public string ThumbnailUrl160
		{
			get;
			set;
		}
		public string ThumbnailUrl180
		{
			get;
			set;
		}
		public string ThumbnailUrl220
		{
			get;
			set;
		}
		public string ThumbnailUrl310
		{
			get;
			set;
		}
		public string ThumbnailUrl410
		{
			get;
			set;
		}
		[NotNullValidator(Negated = true, Ruleset = "ValGift"), RangeValidator(typeof(decimal), "0.01", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset = "ValGift"), ValidatorComposition(CompositionType.Or, Ruleset = "ValGift", MessageTemplate = "成本价格，金额大小0.01-1000万之间")]
		public decimal? CostPrice
		{
			get;
			set;
		}
		[RangeValidator(typeof(decimal), "0.01", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset = "ValGift", MessageTemplate = "采购价格，金额大小0.01-1000万之间")]
		public decimal PurchasePrice
		{
			get;
			set;
		}
		[NotNullValidator(Negated = true, Ruleset = "ValGift"), RangeValidator(typeof(decimal), "0.01", RangeBoundaryType.Inclusive, "10000000", RangeBoundaryType.Inclusive, Ruleset = "ValGift"), ValidatorComposition(CompositionType.Or, Ruleset = "ValGift", MessageTemplate = "市场参考价格，金额大小0.01-1000万之间")]
		public decimal? MarketPrice
		{
			get;
			set;
		}
		[RangeValidator(0, RangeBoundaryType.Inclusive, 10000, RangeBoundaryType.Inclusive, Ruleset = "ValGift", MessageTemplate = "兑换所需积分不能为空，大小0-10000之间")]
		public int NeedPoint
		{
			get;
			set;
		}
		public bool IsDownLoad
		{
			get;
			set;
		}
		public bool IsPromotion
		{
			get;
			set;
		}
	}
}
