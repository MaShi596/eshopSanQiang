using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
namespace Hidistro.Entities.Distribution
{
	[HasSelfValidation]
	public class SiteRequestInfo
	{
		public int RequestId
		{
			get;
			set;
		}
		public int UserId
		{
			get;
			set;
		}
		[StringLengthValidator(1, 30, Ruleset = "ValSiteRequest", MessageTemplate = "域名不能为空,长度限制在30个字符以内,必须为有效格式")]
		public string FirstSiteUrl
		{
			get;
			set;
		}
		public System.DateTime RequestTime
		{
			get;
			set;
		}
		public SiteRequestStatus RequestStatus
		{
			get;
			set;
		}
		public string RefuseReason
		{
			get;
			set;
		}
	}
}
