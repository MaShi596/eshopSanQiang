using Hidistro.Core;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using System;
namespace Hidistro.Entities.Comments
{
	public class AfficheInfo
	{
		public int AfficheId
		{
			get;
			set;
		}
		[HtmlCoding, StringLengthValidator(1, 60, Ruleset = "ValAfficheInfo", MessageTemplate = "公告标题不能为空，长度限制在60个字符以内")]
		public string Title
		{
			get;
			set;
		}
		[StringLengthValidator(1, 999999999, Ruleset = "ValAfficheInfo", MessageTemplate = "公告内容不能为空")]
		public string Content
		{
			get;
			set;
		}
		public System.DateTime AddedDate
		{
			get;
			set;
		}
	}
}
