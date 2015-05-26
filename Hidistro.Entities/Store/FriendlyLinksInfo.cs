using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
namespace Hidistro.Entities.Store
{
	public class FriendlyLinksInfo
	{
		public int? LinkId
		{
			get;
			set;
		}
		public string ImageUrl
		{
			get;
			set;
		}
		[StringLengthValidator(0, 60, Ruleset = "ValFriendlyLinksInfo", MessageTemplate = "网站名称长度限制在60个字符以内")]
		public string Title
		{
			get;
			set;
		}
		[IgnoreNulls, RegexValidator("^(http://).*[\\.]+.*", Ruleset = "ValFriendlyLinksInfo"), StringLengthValidator(0, Ruleset = "ValFriendlyLinksInfo"), ValidatorComposition(CompositionType.Or, Ruleset = "ValFriendlyLinksInfo", MessageTemplate = "网站地址必须为有效格式")]
		public string LinkUrl
		{
			get;
			set;
		}
		public int DisplaySequence
		{
			get;
			set;
		}
		public bool Visible
		{
			get;
			set;
		}
	}
}
