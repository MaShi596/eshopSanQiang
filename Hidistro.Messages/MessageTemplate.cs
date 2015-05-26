using System;
namespace Hidistro.Messages
{
	public class MessageTemplate
	{
		public string Name
		{
			get;
			private set;
		}
		public string MessageType
		{
			get;
			set;
		}
		public bool SendEmail
		{
			get;
			set;
		}
		public bool SendSMS
		{
			get;
			set;
		}
		public bool SendInnerMessage
		{
			get;
			set;
		}
		public string TagDescription
		{
			get;
			private set;
		}
		public string EmailSubject
		{
			get;
			set;
		}
		public string EmailBody
		{
			get;
			set;
		}
		public string InnerMessageSubject
		{
			get;
			set;
		}
		public string InnerMessageBody
		{
			get;
			set;
		}
		public string SMSBody
		{
			get;
			set;
		}
		public MessageTemplate()
		{
		}
		public MessageTemplate(string tagDescription, string name)
		{
			this.TagDescription = tagDescription;
			this.Name = name;
		}
	}
}
