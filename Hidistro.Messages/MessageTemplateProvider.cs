using Hidistro.Core;
using System;
using System.Collections.Generic;
using System.Data;
namespace Hidistro.Messages
{
	public abstract class MessageTemplateProvider
	{
		private static readonly MessageTemplateProvider DefaultInstance;
		static MessageTemplateProvider()
		{
			MessageTemplateProvider.DefaultInstance = (DataProviders.CreateInstance("Hidistro.Messages.Data.MessageTemplateData,Hidistro.Messages.Data") as MessageTemplateProvider);
		}
		public static MessageTemplateProvider Instance()
		{
			return MessageTemplateProvider.DefaultInstance;
		}
		public abstract void UpdateSettings(System.Collections.Generic.IList<MessageTemplate> templates);
		public abstract void UpdateTemplate(MessageTemplate template);
		public abstract MessageTemplate GetMessageTemplate(string messageType);
		public abstract System.Collections.Generic.IList<MessageTemplate> GetMessageTemplates();
		public abstract void UpdateDistributorSettings(System.Collections.Generic.IList<MessageTemplate> templates);
		public abstract void UpdateDistributorTemplate(MessageTemplate template);
		public abstract MessageTemplate GetDistributorMessageTemplate(string messageType, int distributorUserId);
		public abstract System.Collections.Generic.IList<MessageTemplate> GetDistributorMessageTemplates();
		public static MessageTemplate PopulateEmailTempletFromIDataReader(IDataReader reader)
		{
			MessageTemplate result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				MessageTemplate messageTemplate = new MessageTemplate((string)reader["TagDescription"], (string)reader["Name"])
				{
					MessageType = (string)reader["MessageType"],
					SendInnerMessage = (bool)reader["SendInnerMessage"],
					SendSMS = (bool)reader["SendSMS"],
					SendEmail = (bool)reader["SendEmail"],
					EmailSubject = (string)reader["EmailSubject"],
					EmailBody = (string)reader["EmailBody"],
					InnerMessageSubject = (string)reader["InnerMessageSubject"],
					InnerMessageBody = (string)reader["InnerMessageBody"],
					SMSBody = (string)reader["SMSBody"]
				};
				result = messageTemplate;
			}
			return result;
		}
	}
}
