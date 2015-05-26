using Hidistro.Core;
using Hidistro.Entities.Comments;
using System;
namespace Hidistro.SaleSystem.Comments
{
	public static class CommentProcessor
	{
		public static bool InsertLeaveComment(LeaveCommentInfo leave)
		{
			Globals.EntityCoding(leave, true);
			return CommentProvider.Instance().InsertLeaveComment(leave);
		}
	}
}
