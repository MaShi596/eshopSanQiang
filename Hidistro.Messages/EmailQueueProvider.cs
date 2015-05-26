using Hidistro.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Mail;
namespace Hidistro.Messages
{
	public abstract class EmailQueueProvider
	{
		private static readonly EmailQueueProvider DefaultInstance;
		static EmailQueueProvider()
		{
			EmailQueueProvider.DefaultInstance = (DataProviders.CreateInstance("Hidistro.Messages.Data.EmailQueueData,Hidistro.Messages.Data") as EmailQueueProvider);
		}
		public static EmailQueueProvider Instance()
		{
			return EmailQueueProvider.DefaultInstance;
		}
		public abstract void QueueEmail(System.Net.Mail.MailMessage message);
		public abstract void QueueDistributorEmail(System.Net.Mail.MailMessage message, int userId);
		public abstract System.Collections.Generic.Dictionary<System.Guid, System.Net.Mail.MailMessage> DequeueEmail();
		public abstract System.Collections.Generic.Dictionary<System.Guid, SubsiteMailMessage> DequeueDistributorEmail();
		public abstract void DeleteQueuedEmail(System.Guid emailId);
		public abstract void DeleteDistributorQueuedEmail(System.Guid emailId);
		public abstract void QueueSendingFailure(System.Collections.Generic.IList<System.Guid> list, int failureInterval, int maxNumberOfTries);
		public abstract void QueueDistributorSendingFailure(System.Collections.Generic.IList<System.Guid> list, int failureInterval, int maxNumberOfTries);
		public static System.Net.Mail.MailMessage PopulateEmailFromIDataReader(IDataReader reader)
		{
			System.Net.Mail.MailMessage result;
			if (null == reader)
			{
				result = null;
			}
			else
			{
				try
				{
					System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage
					{
						Priority = (System.Net.Mail.MailPriority)((int)reader["EmailPriority"]),
						IsBodyHtml = (bool)reader["IsBodyHtml"]
					};
					if (reader["EmailSubject"] != System.DBNull.Value)
					{
						mailMessage.Subject = (string)reader["EmailSubject"];
					}
					if (reader["EmailTo"] != System.DBNull.Value)
					{
						mailMessage.To.Add((string)reader["EmailTo"]);
					}
					if (reader["EmailBody"] != System.DBNull.Value)
					{
						mailMessage.Body = (string)reader["EmailBody"];
					}
					if (reader["EmailCc"] != System.DBNull.Value)
					{
						string[] array = ((string)reader["EmailCc"]).Split(new char[]
						{
							','
						});
						string[] array2 = array;
						for (int i = 0; i < array2.Length; i++)
						{
							string text = array2[i];
							if (!string.IsNullOrEmpty(text))
							{
								mailMessage.CC.Add(new System.Net.Mail.MailAddress(text));
							}
						}
					}
					if (reader["EmailBcc"] != System.DBNull.Value)
					{
						string[] array3 = ((string)reader["EmailBcc"]).Split(new char[]
						{
							','
						});
						string[] array2 = array3;
						for (int i = 0; i < array2.Length; i++)
						{
							string text2 = array2[i];
							if (!string.IsNullOrEmpty(text2))
							{
								mailMessage.Bcc.Add(new System.Net.Mail.MailAddress(text2));
							}
						}
					}
					result = mailMessage;
				}
				catch
				{
					result = null;
				}
			}
			return result;
		}
	}
}
