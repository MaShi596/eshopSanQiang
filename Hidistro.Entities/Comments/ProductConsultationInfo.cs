using Hidistro.Core;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using System;
namespace Hidistro.Entities.Comments
{
	public class ProductConsultationInfo
	{
		public int ConsultationId
		{
			get;
			set;
		}
		public int ProductId
		{
			get;
			set;
		}
		public int UserId
		{
			get;
			set;
		}
		[HtmlCoding, StringLengthValidator(1, 30, Ruleset = "Refer", MessageTemplate = "用户昵称为必填项，长度限制在30字符以内")]
		public string UserName
		{
			get;
			set;
		}
		[RegexValidator("^[a-zA-Z\\.0-9_-]+@[a-zA-Z0-9_-]+(\\.[a-zA-Z0-9_-]+)+$", Ruleset = "Refer", MessageTemplate = "邮箱地址必须为有效格式"), StringLengthValidator(1, 256, Ruleset = "Refer", MessageTemplate = "邮箱不能为空，长度限制在256字符以内")]
		public string UserEmail
		{
			get;
			set;
		}
		[HtmlCoding, StringLengthValidator(1, 300, Ruleset = "Refer", MessageTemplate = "咨询内容为必填项，长度限制在300字符以内")]
		public string ConsultationText
		{
			get;
			set;
		}
		public System.DateTime ConsultationDate
		{
			get;
			set;
		}
		[NotNullValidator(Ruleset = "Reply", MessageTemplate = "回内容为必填项")]
		public string ReplyText
		{
			get;
			set;
		}
		public System.DateTime? ReplyDate
		{
			get;
			set;
		}
		public int? ReplyUserId
		{
			get;
			set;
		}
	}
}
