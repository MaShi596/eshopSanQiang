using Hidistro.ControlPanel.Members;
using Hidistro.Entities.Members;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
namespace Hidistro.ControlPanel.Data
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
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("aspnet_OpenIdSettings_Save");
			this.database.AddInParameter(storedProcCommand, "OpenIdType", System.Data.DbType.String, settings.OpenIdType.ToLower());
			this.database.AddInParameter(storedProcCommand, "Name", System.Data.DbType.String, settings.Name);
			this.database.AddInParameter(storedProcCommand, "Description", System.Data.DbType.String, settings.Description);
			this.database.AddInParameter(storedProcCommand, "Settings", System.Data.DbType.String, settings.Settings);
			this.database.ExecuteNonQuery(storedProcCommand);
		}
		public override void DeleteSettings(string openIdType)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM aspnet_OpenIdSettings WHERE LOWER(OpenIdType)=LOWER(@OpenIdType)");
			this.database.AddInParameter(sqlStringCommand, "OpenIdType", System.Data.DbType.String, openIdType.ToLower());
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override OpenIdSettingsInfo GetOpenIdSettings(string openIdType)
		{
			OpenIdSettingsInfo openIdSettingsInfo = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM aspnet_OpenIdSettings WHERE LOWER(OpenIdType)=LOWER(@OpenIdType)");
			this.database.AddInParameter(sqlStringCommand, "OpenIdType", System.Data.DbType.String, openIdType.ToLower());
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
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT OpenIdType FROM aspnet_OpenIdSettings");
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(dataReader.GetString(0));
				}
			}
			return list;
		}
	}
}
