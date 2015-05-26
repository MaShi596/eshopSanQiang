using Hidistro.Core.Entities;
using System;
namespace Hidistro.Entities.Comments
{
	public class LeaveCommentQuery : Pagination
	{
		public int? AgentId
		{
			get;
			set;
		}
		public MessageStatus MessageStatus
		{
			get;
			set;
		}
	}
}
