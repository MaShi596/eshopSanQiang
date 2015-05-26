using Hidistro.Core;
using Hidistro.Membership.Context;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Mail;
namespace Hidistro.Messages
{
	public static class MessageTemplateHelper
	{
		private const string CacheKey = "Message-{0}";
		private const string DistributorCacheKey = "Message-{0}-{1}";
		internal static System.Net.Mail.MailMessage GetEmailTemplate(MessageTemplate template, string emailTo)
		{
			System.Net.Mail.MailMessage result;
			if (template == null || !template.SendEmail || string.IsNullOrEmpty(emailTo))
			{
				result = null;
			}
			else
			{
				System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage
				{
					IsBodyHtml = true,
					Priority = System.Net.Mail.MailPriority.High,
					Body = template.EmailBody.Trim(),
					Subject = template.EmailSubject.Trim()
				};
				mailMessage.To.Add(emailTo);
				result = mailMessage;
			}
			return result;
		}
		internal static MessageTemplate GetTemplate(string messageType)
		{
			messageType = messageType.ToLower();
			SiteSettings siteSettings = HiContext.Current.SiteSettings;
			string string_;
			if (siteSettings.IsDistributorSettings)
			{
				string_ = string.Format("Message-{0}-{1}", siteSettings.UserId.Value.ToString(System.Globalization.CultureInfo.InvariantCulture), messageType);
			}
			else
			{
				string_ = string.Format("Message-{0}", messageType);
			}
			MessageTemplate messageTemplate = HiCache.Get(string_) as MessageTemplate;
			if (messageTemplate == null)
			{
				messageTemplate = (siteSettings.IsDistributorSettings ? MessageTemplateHelper.GetDistributorMessageTemplate(messageType, siteSettings.UserId.Value) : MessageTemplateHelper.GetMessageTemplate(messageType));
				if (messageTemplate != null)
				{
					HiCache.Max(string_, messageTemplate);
				}
			}
			return messageTemplate;
		}
		public static void UpdateSettings(System.Collections.Generic.IList<MessageTemplate> templates)
		{
			if (templates != null && templates.Count != 0)
			{
				MessageTemplateProvider.Instance().UpdateSettings(templates);
				foreach (MessageTemplate current in templates)
				{
					HiCache.Remove(string.Format("Message-{0}", current.MessageType.ToLower()));
				}
			}
		}
		public static void UpdateTemplate(MessageTemplate template)
		{
			if (template != null)
			{
				MessageTemplateProvider.Instance().UpdateTemplate(template);
				HiCache.Remove(string.Format("Message-{0}", template.MessageType.ToLower()));
			}
		}
		public static MessageTemplate GetMessageTemplate(string messageType)
		{
			MessageTemplate result;
			if (string.IsNullOrEmpty(messageType))
			{
				result = null;
			}
			else
			{
				result = MessageTemplateProvider.Instance().GetMessageTemplate(messageType);
			}
			return result;
		}
		public static System.Collections.Generic.IList<MessageTemplate> GetMessageTemplates()
		{
			return MessageTemplateProvider.Instance().GetMessageTemplates();
		}
		public static void UpdateDistributorSettings(System.Collections.Generic.IList<MessageTemplate> templates)
		{
			if (templates != null && templates.Count != 0)
			{
				MessageTemplateProvider.Instance().UpdateDistributorSettings(templates);
				string arg = HiContext.Current.User.UserId.ToString(System.Globalization.CultureInfo.InvariantCulture);
				foreach (MessageTemplate current in templates)
				{
					HiCache.Remove(string.Format("Message-{0}-{1}", arg, current.MessageType.ToLower()));
				}
			}
		}
		public static void UpdateDistributorTemplate(MessageTemplate template)
		{
			if (template != null)
			{
				MessageTemplateProvider.Instance().UpdateDistributorTemplate(template);
				HiCache.Remove(string.Format("Message-{0}-{1}", HiContext.Current.User.UserId.ToString(System.Globalization.CultureInfo.InvariantCulture), template.MessageType.ToLower()));
			}
		}
		public static MessageTemplate GetDistributorMessageTemplate(string messageType, int distributorUserId)
		{
			MessageTemplate result;
			if (string.IsNullOrEmpty(messageType))
			{
				result = null;
			}
			else
			{
				result = MessageTemplateProvider.Instance().GetDistributorMessageTemplate(messageType, distributorUserId);
			}
			return result;
		}
		public static System.Collections.Generic.IList<MessageTemplate> GetDistributorMessageTemplates()
		{
			return MessageTemplateProvider.Instance().GetDistributorMessageTemplates();
		}
	}
}
