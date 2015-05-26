using Hidistro.Core;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using System;
namespace Hidistro.Entities.Comments
{
	public class HelpInfo
	{
		public int CategoryId
		{
			get;
			set;
		}
		public int HelpId
		{
			get;
			set;
		}
		[HtmlCoding, StringLengthValidator(1, 60, Ruleset = "ValHelpInfo", MessageTemplate = "帮助主题不能为空，长度限制在60个字符以内")]
		public string Title
		{
			get;
			set;
		}
		[HtmlCoding, StringLengthValidator(0, 260, Ruleset = "ValHelpInfo", MessageTemplate = "告诉搜索引擎此帮助页面的主要内容，长度限制在260个字符以内")]
		public string MetaDescription
		{
			get;
			set;
		}
		[HtmlCoding, StringLengthValidator(0, 160, Ruleset = "ValHelpInfo", MessageTemplate = "让用户可以通过搜索引擎搜索到此帮助的浏览页面，长度限制在160个字符以内")]
		public string MetaKeywords
		{
			get;
			set;
		}
		[HtmlCoding, StringLengthValidator(0, 300, Ruleset = "ValHelpInfo", MessageTemplate = "摘要的长度限制在300个字符以内")]
		public string Description
		{
			get;
			set;
		}
		[StringLengthValidator(1, 999999999, Ruleset = "ValHelpInfo", MessageTemplate = "帮助内容不能为空")]
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
		public bool IsShowFooter
		{
			get;
			set;
		}
	}
}
