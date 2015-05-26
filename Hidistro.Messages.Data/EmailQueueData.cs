using Hidistro.Core;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Net.Mail;
namespace Hidistro.Messages.Data
{
	public class EmailQueueData : EmailQueueProvider
	{
		private Database database;
		public EmailQueueData()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public override void QueueEmail(MailMessage message)
		{
			if (message != null)
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_EmailQueue(EmailId, EmailTo, EmailCc, EmailBcc, EmailSubject, EmailBody, EmailPriority, IsBodyHtml, NextTryTime, NumberOfTries) VALUES(@EmailId, @EmailTo, @EmailCc, @EmailBcc, @EmailSubject, @EmailBody,@EmailPriority, @IsBodyHtml, @NextTryTime, @NumberOfTries)");
				this.database.AddInParameter(sqlStringCommand, "EmailId", System.Data.DbType.Guid, Guid.NewGuid());
				this.database.AddInParameter(sqlStringCommand, "EmailTo", System.Data.DbType.String, Globals.ToDelimitedString(message.To, ","));
				if (message.CC != null)
				{
					this.database.AddInParameter(sqlStringCommand, "EmailCc", System.Data.DbType.String, Globals.ToDelimitedString(message.CC, ","));
				}
				else
				{
					this.database.AddInParameter(sqlStringCommand, "EmailCc", System.Data.DbType.String, DBNull.Value);
				}
				if (message.Bcc != null)
				{
					this.database.AddInParameter(sqlStringCommand, "EmailBcc", System.Data.DbType.String, Globals.ToDelimitedString(message.Bcc, ","));
				}
				else
				{
					this.database.AddInParameter(sqlStringCommand, "EmailBcc", System.Data.DbType.String, DBNull.Value);
				}
				this.database.AddInParameter(sqlStringCommand, "EmailSubject", System.Data.DbType.String, message.Subject);
				this.database.AddInParameter(sqlStringCommand, "EmailBody", System.Data.DbType.String, message.Body);
				this.database.AddInParameter(sqlStringCommand, "EmailPriority", System.Data.DbType.Int32, (int)message.Priority);
				this.database.AddInParameter(sqlStringCommand, "IsBodyHtml", System.Data.DbType.Boolean, message.IsBodyHtml);
				this.database.AddInParameter(sqlStringCommand, "NextTryTime", System.Data.DbType.DateTime, DateTime.Parse("1900-1-1 12:00:00"));
				this.database.AddInParameter(sqlStringCommand, "NumberOfTries", System.Data.DbType.Int32, 0);
				this.database.ExecuteNonQuery(sqlStringCommand);
			}
		}
		public override void QueueDistributorEmail(MailMessage message, int userId)
		{
			if (message != null)
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO distro_EmailQueue(UserId, EmailId, EmailTo, EmailCc, EmailBcc, EmailSubject, EmailBody, EmailPriority, IsBodyHtml, NextTryTime, NumberOfTries) VALUES(@UserId, @EmailId, @EmailTo, @EmailCc, @EmailBcc, @EmailSubject, @EmailBody,@EmailPriority, @IsBodyHtml, @NextTryTime, @NumberOfTries)");
				this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
				this.database.AddInParameter(sqlStringCommand, "EmailId", System.Data.DbType.Guid, Guid.NewGuid());
				this.database.AddInParameter(sqlStringCommand, "EmailTo", System.Data.DbType.String, Globals.ToDelimitedString(message.To, ","));
				if (message.CC != null)
				{
					this.database.AddInParameter(sqlStringCommand, "EmailCc", System.Data.DbType.String, Globals.ToDelimitedString(message.CC, ","));
				}
				else
				{
					this.database.AddInParameter(sqlStringCommand, "EmailCc", System.Data.DbType.String, DBNull.Value);
				}
				if (message.Bcc != null)
				{
					this.database.AddInParameter(sqlStringCommand, "EmailBcc", System.Data.DbType.String, Globals.ToDelimitedString(message.Bcc, ","));
				}
				else
				{
					this.database.AddInParameter(sqlStringCommand, "EmailBcc", System.Data.DbType.String, DBNull.Value);
				}
				this.database.AddInParameter(sqlStringCommand, "EmailSubject", System.Data.DbType.String, message.Subject);
				this.database.AddInParameter(sqlStringCommand, "EmailBody", System.Data.DbType.String, message.Body);
				this.database.AddInParameter(sqlStringCommand, "EmailPriority", System.Data.DbType.Int32, (int)message.Priority);
				this.database.AddInParameter(sqlStringCommand, "IsBodyHtml", System.Data.DbType.Boolean, message.IsBodyHtml);
				this.database.AddInParameter(sqlStringCommand, "NextTryTime", System.Data.DbType.DateTime, DateTime.Parse("1900-1-1 12:00:00"));
				this.database.AddInParameter(sqlStringCommand, "NumberOfTries", System.Data.DbType.Int32, 0);
				this.database.ExecuteNonQuery(sqlStringCommand);
			}
		}
		public override Dictionary<Guid, MailMessage> DequeueEmail()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_EmailQueue WHERE NextTryTime < getdate()");
			Dictionary<Guid, MailMessage> dictionary = new Dictionary<Guid, MailMessage>();
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					MailMessage mailMessage = EmailQueueProvider.PopulateEmailFromIDataReader(dataReader);
					if (mailMessage != null)
					{
						dictionary.Add((Guid)dataReader["EmailId"], mailMessage);
					}
					else
					{
						this.DeleteQueuedEmail((Guid)dataReader["EmailId"]);
					}
				}
				dataReader.Close();
			}
			return dictionary;
		}
		public override Dictionary<Guid, SubsiteMailMessage> DequeueDistributorEmail()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_EmailQueue WHERE NextTryTime < getdate()");
			Dictionary<Guid, SubsiteMailMessage> dictionary = new Dictionary<Guid, SubsiteMailMessage>();
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					MailMessage mailMessage = EmailQueueProvider.PopulateEmailFromIDataReader(dataReader);
					if (mailMessage != null)
					{
						dictionary.Add((Guid)dataReader["EmailId"], new SubsiteMailMessage((int)dataReader["UserId"], mailMessage));
					}
					else
					{
						this.DeleteDistributorQueuedEmail((Guid)dataReader["EmailId"]);
					}
				}
				dataReader.Close();
			}
			return dictionary;
		}
		public override void DeleteQueuedEmail(Guid emailId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_EmailQueue WHERE EmailId = @EmailId");
			this.database.AddInParameter(sqlStringCommand, "EmailId", System.Data.DbType.Guid, emailId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override void DeleteDistributorQueuedEmail(Guid emailId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_EmailQueue WHERE EmailId = @EmailId");
			this.database.AddInParameter(sqlStringCommand, "EmailId", System.Data.DbType.Guid, emailId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override void QueueSendingFailure(IList<Guid> list, int failureInterval, int maxNumberOfTries)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("cp_EmailQueue_Failure");
			this.database.AddInParameter(storedProcCommand, "EmailId", System.Data.DbType.Guid);
			this.database.AddInParameter(storedProcCommand, "FailureInterval", System.Data.DbType.Int32, failureInterval);
			this.database.AddInParameter(storedProcCommand, "MaxNumberOfTries", System.Data.DbType.Int32, maxNumberOfTries);
			foreach (Guid current in list)
			{
				storedProcCommand.Parameters[0].Value = current;
				this.database.ExecuteNonQuery(storedProcCommand);
			}
		}
		public override void QueueDistributorSendingFailure(IList<Guid> list, int failureInterval, int maxNumberOfTries)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("sub_EmailQueue_Failure");
			this.database.AddInParameter(storedProcCommand, "EmailId", System.Data.DbType.Guid);
			this.database.AddInParameter(storedProcCommand, "FailureInterval", System.Data.DbType.Int32, failureInterval);
			this.database.AddInParameter(storedProcCommand, "MaxNumberOfTries", System.Data.DbType.Int32, maxNumberOfTries);
			foreach (Guid current in list)
			{
				storedProcCommand.Parameters[0].Value = current;
				this.database.ExecuteNonQuery(storedProcCommand);
			}
		}
	}
}
