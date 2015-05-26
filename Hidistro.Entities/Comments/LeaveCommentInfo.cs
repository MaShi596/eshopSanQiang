using Hidistro.Core;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using System;
namespace Hidistro.Entities.Comments
{
	public class LeaveCommentInfo
	{
		public long LeaveId
		{
			get;
			set;
		}
		public int? UserId
		{
			get;
			set;
		}
		[HtmlCoding, StringLengthValidator(1, 60, Ruleset = "Refer", MessageTemplate = "用户名为必填项，长度限制在60字符以内")]
		public string UserName
		{
			get;
			set;
		}
		[HtmlCoding, StringLengthValidator(1, 60, Ruleset = "Refer", MessageTemplate = "标题为必填项，长度限制在60字符以内")]
		public string Title
		{
			get;
			set;
		}
		[HtmlCoding, StringLengthValidator(1, 300, Ruleset = "Refer", MessageTemplate = "留言内容为必填项，长度限制在300字符以内")]
		public string PublishContent
		{
			get;
			set;
		}
		public System.DateTime PublishDate
		{
			get;
			set;
		}
		public System.DateTime LastDate
		{
			get;
			set;
		}
	}
}
