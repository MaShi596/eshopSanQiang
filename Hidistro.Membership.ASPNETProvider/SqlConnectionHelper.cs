using System;
using System.Configuration;
namespace Hidistro.Membership.ASPNETProvider
{
	internal static class SqlConnectionHelper
	{
		internal const string s_strUpperDataDirWithToken = "|DATADIRECTORY|";
		private static object s_lock = new object();
		internal static SqlConnectionHolder GetConnection(string connectionString, bool revertImpersonation)
		{
			connectionString.ToUpperInvariant();
			SqlConnectionHolder sqlConnectionHolder = new SqlConnectionHolder(connectionString);
			bool flag = true;
			try
			{
				try
				{
					sqlConnectionHolder.Open(null, revertImpersonation);
					flag = false;
				}
				finally
				{
					if (flag)
					{
						sqlConnectionHolder.Close();
						sqlConnectionHolder = null;
					}
				}
			}
			catch
			{
				throw;
			}
			return sqlConnectionHolder;
		}
		internal static string GetConnectionString(string specifiedConnectionString, bool lookupConnectionString, bool appLevel)
		{
			if (specifiedConnectionString != null && specifiedConnectionString.Length >= 1)
			{
				string text = null;
				if (lookupConnectionString)
				{
					ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings[specifiedConnectionString];
					if (connectionStringSettings != null)
					{
						text = connectionStringSettings.ConnectionString;
					}
					if (text == null)
					{
						return null;
					}
				}
				else
				{
					text = specifiedConnectionString;
				}
				return text;
			}
			return null;
		}
	}
}
