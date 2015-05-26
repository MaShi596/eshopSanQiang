using Hidistro.Core;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
namespace Hidistro.Entities.Members
{
	public class DistributorGradeInfo
	{
		public int GradeId
		{
			get;
			set;
		}
		[HtmlCoding, StringLengthValidator(1, 60, Ruleset = "ValDistributorGrade", MessageTemplate = "分销商等级名称不能为空，长度限制在60个字符以内")]
		public string Name
		{
			get;
			set;
		}
		[HtmlCoding, StringLengthValidator(0, 300, Ruleset = "ValDistributorGrade", MessageTemplate = "备注的长度限制在300个字符以内")]
		public string Description
		{
			get;
			set;
		}
		[RangeValidator(typeof(int), "1", RangeBoundaryType.Inclusive, "100", RangeBoundaryType.Inclusive, Ruleset = "ValDistributorGrade", MessageTemplate = "等级折扣必须在1-100之间")]
		public int Discount
		{
			get;
			set;
		}
	}
}
