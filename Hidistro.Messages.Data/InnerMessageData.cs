using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
namespace Hidistro.Messages.Data
{
	public class InnerMessageData : InnerMessageProvider
	{
		private readonly Database database;
		public InnerMessageData()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public override bool SendMessage(string subject, string message, string sendto)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @ContentId int; INSERT INTO [Hishop_MessageContent]([Title],[Content],[Date]) VALUES (@Title,@Content,@Date) SET @ContentId = @@IDENTITY INSERT INTO [Hishop_MemberMessageBox]([ContentId],[Sernder],[Accepter],[IsRead]) VALUES (@ContentId,'admin' ,@Accepter,0)");
			this.database.AddInParameter(sqlStringCommand, "Title", System.Data.DbType.String, subject);
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, message);
			this.database.AddInParameter(sqlStringCommand, "Date", System.Data.DbType.DateTime, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "Accepter", System.Data.DbType.String, sendto);
			return this.database.ExecuteNonQuery(sqlStringCommand) >= 1;
		}
		public override bool SendDistributorMessage(string subject, string message, string distributor, string sendto)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DECLARE @ContentId int; INSERT INTO [Hishop_MessageContent]([Title],[Content],[Date]) VALUES (@Title,@Content,@Date) SET @ContentId = @@IDENTITY INSERT INTO [Hishop_MemberMessageBox]([ContentId],[Sernder],[Accepter],[IsRead]) VALUES (@ContentId,@Sernder ,@Accepter,0)");
			this.database.AddInParameter(sqlStringCommand, "Title", System.Data.DbType.String, subject);
			this.database.AddInParameter(sqlStringCommand, "Content", System.Data.DbType.String, message);
			this.database.AddInParameter(sqlStringCommand, "Date", System.Data.DbType.DateTime, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "Sernder", System.Data.DbType.String, distributor);
			this.database.AddInParameter(sqlStringCommand, "Accepter", System.Data.DbType.String, sendto);
			return this.database.ExecuteNonQuery(sqlStringCommand) >= 1;
		}
	}
}
