using Hidistro.Core;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using System;
namespace Hidistro.Entities.Comments
{
	public class ArticleCategoryInfo
	{
		public int CategoryId
		{
			get;
			set;
		}
		[HtmlCoding, StringLengthValidator(1, 60, Ruleset = "ValArticleCategoryInfo", MessageTemplate = "分类名称不能为空，长度限制在60个字符以内")]
		public string Name
		{
			get;
			set;
		}
		public int DisplaySequence
		{
			get;
			set;
		}
		public string IconUrl
		{
			get;
			set;
		}
		[HtmlCoding, StringLengthValidator(0, 300, Ruleset = "ValArticleCategoryInfo", MessageTemplate = "分类介绍最多只能输入300个字符")]
		public string Description
		{
			get;
			set;
		}
	}
}
