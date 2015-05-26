using Hidistro.Core.Configuration;
using Hidistro.Membership.Context;
using Hishop.Plugins;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading;
namespace Hidistro.Messages
{
	public static class Emails
	{
		internal static void EnqueuEmail(System.Net.Mail.MailMessage email, SiteSettings settings)
		{
			if (email != null && email.To != null && email.To.Count > 0)
			{
				if (settings.IsDistributorSettings)
				{
					EmailQueueProvider.Instance().QueueDistributorEmail(email, settings.UserId.Value);
				}
				else
				{
					EmailQueueProvider.Instance().QueueEmail(email);
				}
			}
		}
		public static void SendQueuedEmails(int failureInterval, int maxNumberOfTries, SiteSettings settings)
		{
			if (settings != null)
			{
				HiConfiguration config = HiConfiguration.GetConfig();
				System.Collections.Generic.Dictionary<System.Guid, System.Net.Mail.MailMessage> dictionary = EmailQueueProvider.Instance().DequeueEmail();
				System.Collections.Generic.IList<System.Guid> list = new System.Collections.Generic.List<System.Guid>();
				EmailSender emailSender = Messenger.CreateEmailSender(settings);
				if (emailSender != null)
				{
					int num = 0;
					short smtpServerConnectionLimit = config.SmtpServerConnectionLimit;
					foreach (System.Guid current in dictionary.Keys)
					{
						if (Messenger.SendMail(dictionary[current], emailSender))
						{
							EmailQueueProvider.Instance().DeleteQueuedEmail(current);
							if (smtpServerConnectionLimit != -1 && ++num >= (int)smtpServerConnectionLimit)
							{
								System.Threading.Thread.Sleep(new System.TimeSpan(0, 0, 0, 15, 0));
								num = 0;
							}
						}
						else
						{
							list.Add(current);
						}
					}
					if (list.Count > 0)
					{
						EmailQueueProvider.Instance().QueueSendingFailure(list, failureInterval, maxNumberOfTries);
					}
				}
			}
		}
		public static void SendSubsiteEmails(int failureInterval, int maxNumberOfTries)
		{
			HiConfiguration config = HiConfiguration.GetConfig();
			System.Collections.Generic.Dictionary<System.Guid, SubsiteMailMessage> dictionary = EmailQueueProvider.Instance().DequeueDistributorEmail();
			System.Collections.Generic.Dictionary<int, EmailSender> dictionary2 = new System.Collections.Generic.Dictionary<int, EmailSender>();
			System.Collections.Generic.IList<System.Guid> list = new System.Collections.Generic.List<System.Guid>();
			System.Collections.Generic.IList<int> list2 = new System.Collections.Generic.List<int>();
			int num = 0;
			short smtpServerConnectionLimit = config.SmtpServerConnectionLimit;
			foreach (System.Guid current in dictionary.Keys)
			{
				int distributorUserId = dictionary[current].DistributorUserId;
				if (!list2.Contains(distributorUserId))
				{
					EmailSender emailSender;
					if (!dictionary2.ContainsKey(distributorUserId))
					{
						SiteSettings siteSettings = SettingsManager.GetSiteSettings(distributorUserId);
						if (siteSettings == null)
						{
							list2.Add(distributorUserId);
							continue;
						}
						emailSender = Messenger.CreateEmailSender(siteSettings);
						if (emailSender == null)
						{
							list2.Add(distributorUserId);
							continue;
						}
						dictionary2.Add(distributorUserId, emailSender);
					}
					else
					{
						emailSender = dictionary2[distributorUserId];
					}
					if (Messenger.SendMail(dictionary[current].Mail, emailSender))
					{
						EmailQueueProvider.Instance().DeleteDistributorQueuedEmail(current);
						if (smtpServerConnectionLimit != -1 && ++num >= (int)smtpServerConnectionLimit)
						{
							System.Threading.Thread.Sleep(new System.TimeSpan(0, 0, 0, 15, 0));
							num = 0;
						}
					}
					else
					{
						list.Add(current);
					}
				}
			}
			if (list.Count > 0)
			{
				EmailQueueProvider.Instance().QueueDistributorSendingFailure(list, failureInterval, maxNumberOfTries);
			}
		}
	}
}
