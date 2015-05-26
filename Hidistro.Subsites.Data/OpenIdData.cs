using Hidistro.Entities.Members;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Members;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace Hidistro.Subsites.Data
{
	public class OpenIdData : OpenIdProvider
	{
		private Database database;
		public OpenIdData()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public override void SaveSettings(OpenIdSettingsInfo settings)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("distro_OpenIdSettings_Save");
			this.database.AddInParameter(storedProcCommand, "OpenIdType", System.Data.DbType.String, settings.OpenIdType.ToLower());
			this.database.AddInParameter(storedProcCommand, "Name", System.Data.DbType.String, settings.Name);
			this.database.AddInParameter(storedProcCommand, "Description", System.Data.DbType.String, settings.Description);
			this.database.AddInParameter(storedProcCommand, "Settings", System.Data.DbType.String, settings.Settings);
			this.database.AddInParameter(storedProcCommand, "UserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.ExecuteNonQuery(storedProcCommand);
		}
		public override void DeleteSettings(string openIdType)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM distro_OpenIdSettings WHERE LOWER(OpenIdType)=LOWER(@OpenIdType) AND UserId=@UserId");
			this.database.AddInParameter(sqlStringCommand, "OpenIdType", System.Data.DbType.String, openIdType.ToLower());
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override OpenIdSettingsInfo GetOpenIdSettings(string openIdType)
		{
			OpenIdSettingsInfo openIdSettingsInfo = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM distro_OpenIdSettings WHERE LOWER(OpenIdType)=LOWER(@OpenIdType) AND UserId=@UserId");
			this.database.AddInParameter(sqlStringCommand, "OpenIdType", System.Data.DbType.String, openIdType.ToLower());
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					openIdSettingsInfo = new OpenIdSettingsInfo
					{
						OpenIdType = openIdType,
						Name = (string)dataReader["Name"],
						Settings = (string)dataReader["Settings"]
					};
					if (dataReader["Description"] != DBNull.Value)
					{
						openIdSettingsInfo.Description = (string)dataReader["Description"];
					}
				}
			}
			return openIdSettingsInfo;
		}
		public override IList<string> GetConfigedTypes()
		{
			IList<string> list = new List<string>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT OpenIdType FROM distro_OpenIdSettings WHERE UserId=@UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(dataReader.GetString(0).ToLower());
				}
			}
			return list;
		}
	}
}
