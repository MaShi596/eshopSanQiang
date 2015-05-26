using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
namespace Hidistro.Entities.Comments
{
	public class LeaveCommentReplyInfo
	{
		public long LeaveId
		{
			get;
			set;
		}
		public long ReplyId
		{
			get;
			set;
		}
		public int UserId
		{
			get;
			set;
		}
		[NotNullValidator(Ruleset = "ValLeaveCommentReply", MessageTemplate = "回复内容不能为空")]
		public string ReplyContent
		{
			get;
			set;
		}
		public System.DateTime ReplyDate
		{
			get;
			set;
		}
	}
}
