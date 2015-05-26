using Hidistro.Membership.Context;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace Hidistro.Messages.Data
{
	public class MessageTemplateData : MessageTemplateProvider
	{
		private readonly Database database;
		public MessageTemplateData()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public override void UpdateSettings(IList<MessageTemplate> templates)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_MessageTemplates SET SendEmail = @SendEmail, SendSMS = @SendSMS, SendInnerMessage = @SendInnerMessage WHERE LOWER(MessageType) = LOWER(@MessageType)");
			this.database.AddInParameter(sqlStringCommand, "SendEmail", System.Data.DbType.Boolean);
			this.database.AddInParameter(sqlStringCommand, "SendSMS", System.Data.DbType.Boolean);
			this.database.AddInParameter(sqlStringCommand, "SendInnerMessage", System.Data.DbType.Boolean);
			this.database.AddInParameter(sqlStringCommand, "MessageType", System.Data.DbType.String);
			foreach (MessageTemplate current in templates)
			{
				this.database.SetParameterValue(sqlStringCommand, "SendEmail", current.SendEmail);
				this.database.SetParameterValue(sqlStringCommand, "SendSMS", current.SendSMS);
				this.database.SetParameterValue(sqlStringCommand, "SendInnerMessage", current.SendInnerMessage);
				this.database.SetParameterValue(sqlStringCommand, "MessageType", current.MessageType);
				this.database.ExecuteNonQuery(sqlStringCommand);
			}
		}
		public override void UpdateTemplate(MessageTemplate template)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_MessageTemplates SET EmailSubject = @EmailSubject, EmailBody = @EmailBody, InnerMessageSubject = @InnerMessageSubject, InnerMessageBody = @InnerMessageBody, SMSBody = @SMSBody WHERE LOWER(MessageType) = LOWER(@MessageType)");
			this.database.AddInParameter(sqlStringCommand, "EmailSubject", System.Data.DbType.String, template.EmailSubject);
			this.database.AddInParameter(sqlStringCommand, "EmailBody", System.Data.DbType.String, template.EmailBody);
			this.database.AddInParameter(sqlStringCommand, "InnerMessageSubject", System.Data.DbType.String, template.InnerMessageSubject);
			this.database.AddInParameter(sqlStringCommand, "InnerMessageBody", System.Data.DbType.String, template.InnerMessageBody);
			this.database.AddInParameter(sqlStringCommand, "SMSBody", System.Data.DbType.String, template.SMSBody);
			this.database.AddInParameter(sqlStringCommand, "MessageType", System.Data.DbType.String, template.MessageType);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override MessageTemplate GetMessageTemplate(string messageType)
		{
			MessageTemplate result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_MessageTemplates WHERE LOWER(MessageType) = LOWER(@MessageType)");
			this.database.AddInParameter(sqlStringCommand, "MessageType", System.Data.DbType.String, messageType);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					result = MessageTemplateProvider.PopulateEmailTempletFromIDataReader(dataReader);
				}
				dataReader.Close();
			}
			return result;
		}
		public override IList<MessageTemplate> GetMessageTemplates()
		{
			IList<MessageTemplate> list = new List<MessageTemplate>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_MessageTemplates");
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(MessageTemplateProvider.PopulateEmailTempletFromIDataReader(dataReader));
				}
				dataReader.Close();
			}
			return list;
		}
		public override void UpdateDistributorSettings(IList<MessageTemplate> templates)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_MessageTemplates SET SendEmail = @SendEmail, SendSMS = @SendSMS, SendInnerMessage = @SendInnerMessage WHERE LOWER(MessageType) = LOWER(@MessageType) AND UserId=@UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "SendEmail", System.Data.DbType.Boolean);
			this.database.AddInParameter(sqlStringCommand, "SendSMS", System.Data.DbType.Boolean);
			this.database.AddInParameter(sqlStringCommand, "SendInnerMessage", System.Data.DbType.Boolean);
			this.database.AddInParameter(sqlStringCommand, "MessageType", System.Data.DbType.String);
			foreach (MessageTemplate current in templates)
			{
				this.database.SetParameterValue(sqlStringCommand, "SendEmail", current.SendEmail);
				this.database.SetParameterValue(sqlStringCommand, "SendSMS", current.SendSMS);
				this.database.SetParameterValue(sqlStringCommand, "SendInnerMessage", current.SendInnerMessage);
				this.database.SetParameterValue(sqlStringCommand, "MessageType", current.MessageType);
				this.database.ExecuteNonQuery(sqlStringCommand);
			}
		}
		public override void UpdateDistributorTemplate(MessageTemplate template)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE distro_MessageTemplates SET EmailSubject = @EmailSubject, EmailBody = @EmailBody, InnerMessageSubject = @InnerMessageSubject, InnerMessageBody = @InnerMessageBody, SMSBody = @SMSBody WHERE LOWER(MessageType) = LOWER(@MessageType) AND UserId=@UserId");
			this.database.AddInParameter(sqlStringCommand, "EmailSubject", System.Data.DbType.String, template.EmailSubject);
			this.database.AddInParameter(sqlStringCommand, "EmailBody", System.Data.DbType.String, template.EmailBody);
			this.database.AddInParameter(sqlStringCommand, "InnerMessageSubject", System.Data.DbType.String, template.InnerMessageSubject);
			this.database.AddInParameter(sqlStringCommand, "InnerMessageBody", System.Data.DbType.String, template.InnerMessageBody);
			this.database.AddInParameter(sqlStringCommand, "SMSBody", System.Data.DbType.String, template.SMSBody);
			this.database.AddInParameter(sqlStringCommand, "MessageType", System.Data.DbType.String, template.MessageType);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override MessageTemplate GetDistributorMessageTemplate(string messageType, int distributorUserId)
		{
			MessageTemplate result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_MessageTemplates WHERE LOWER(MessageType) = LOWER(@MessageType) AND UserId=@UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, distributorUserId);
			this.database.AddInParameter(sqlStringCommand, "MessageType", System.Data.DbType.String, messageType);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					result = MessageTemplateProvider.PopulateEmailTempletFromIDataReader(dataReader);
				}
				dataReader.Close();
			}
			return result;
		}
		public override IList<MessageTemplate> GetDistributorMessageTemplates()
		{
			IList<MessageTemplate> list = new List<MessageTemplate>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_MessageTemplates WHERE UserId=@UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(MessageTemplateProvider.PopulateEmailTempletFromIDataReader(dataReader));
				}
				dataReader.Close();
			}
			return list;
		}
	}
}
