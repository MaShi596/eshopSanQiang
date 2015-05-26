using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
namespace Hidistro.Entities.Store
{
	public class VoteInfo
	{
		public long VoteId
		{
			get;
			set;
		}
		[StringLengthValidator(1, 60, Ruleset = "ValVote", MessageTemplate = "投票调查的标题不能为空，长度限制在60个字符以内")]
		public string VoteName
		{
			get;
			set;
		}
		public bool IsBackup
		{
			get;
			set;
		}
		[RangeValidator(1, RangeBoundaryType.Inclusive, 100, RangeBoundaryType.Inclusive, Ruleset = "ValVote", MessageTemplate = "最多可选项数不允许为空，范围为1-100之间的整数")]
		public int MaxCheck
		{
			get;
			set;
		}
		public int VoteCounts
		{
			get;
			set;
		}
		public System.Collections.Generic.IList<VoteItemInfo> VoteItems
		{
			get;
			set;
		}
	}
}
