using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;
namespace Hidistro.Membership.ASPNETProvider
{
	public class SqlRoleProvider : RoleProvider
	{
		private string _AppName;
		private string _sqlConnectionString;
		private int _CommandTimeout;
		private int CommandTimeout
		{
			get
			{
				return this._CommandTimeout;
			}
		}
		public override string ApplicationName
		{
			get
			{
				return this._AppName;
			}
			set
			{
				this._AppName = value;
				if (this._AppName.Length > 256)
				{
					throw new ProviderException(SR.GetString("The application name is too long."));
				}
			}
		}
		public override void Initialize(string name, NameValueCollection config)
		{
			if (config == null)
			{
				throw new ArgumentNullException("config");
			}
			if (string.IsNullOrEmpty(name))
			{
				name = "SqlRoleProvider";
			}
			if (string.IsNullOrEmpty(config["description"]))
			{
				config.Remove("description");
				config.Add("description", SR.GetString("SQL role provider."));
			}
			base.Initialize(name, config);
			this._CommandTimeout = SecUtility.GetIntValue(config, "commandTimeout", 30, true, 0);
			string text = config["connectionStringName"];
			if (text == null || text.Length < 1)
			{
				throw new ProviderException(SR.GetString("The attribute 'connectionStringName' is missing or empty."));
			}
			this._sqlConnectionString = SqlConnectionHelper.GetConnectionString(text, true, true);
			if (this._sqlConnectionString == null || this._sqlConnectionString.Length < 1)
			{
				throw new ProviderException(SR.GetString("The connection name '{0}' was not found in the applications configuration or the connection string is empty.", text));
			}
			this._AppName = config["applicationName"];
			if (string.IsNullOrEmpty(this._AppName))
			{
				this._AppName = SecUtility.GetDefaultAppName();
			}
			if (this._AppName.Length > 256)
			{
				throw new ProviderException(SR.GetString("The application name is too long."));
			}
			config.Remove("connectionStringName");
			config.Remove("applicationName");
			config.Remove("commandTimeout");
			if (config.Count > 0)
			{
				string key = config.GetKey(0);
				if (!string.IsNullOrEmpty(key))
				{
					throw new ProviderException(SR.GetString("Attribute not recognized '{0}'", key));
				}
			}
		}
		public override bool IsUserInRole(string username, string roleName)
		{
			SecUtility.CheckParameter(ref roleName, true, true, true, 256, "roleName");
			SecUtility.CheckParameter(ref username, true, false, true, 256, "username");
			if (username.Length < 1)
			{
				return false;
			}
			bool result;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_UsersInRoles_IsUserInRole", sqlConnectionHolder.Connection);
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.CommandTimeout = this.CommandTimeout;
					SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlCommand.Parameters.Add(this.CreateInputParam("@UserName", SqlDbType.NVarChar, username));
					sqlCommand.Parameters.Add(this.CreateInputParam("@RoleName", SqlDbType.NVarChar, roleName));
					sqlCommand.ExecuteNonQuery();
					switch (this.GetReturnValue(sqlCommand))
					{
					case 0:
						result = false;
						break;
					case 1:
						result = true;
						break;
					case 2:
						result = false;
						break;
					case 3:
						result = false;
						break;
					default:
						throw new ProviderException(SR.GetString("Stored procedure call failed."));
					}
				}
				finally
				{
					if (sqlConnectionHolder != null)
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
			return result;
		}
		public override string[] GetRolesForUser(string username)
		{
			SecUtility.CheckParameter(ref username, true, false, true, 256, "username");
			if (username.Length < 1)
			{
				return new string[0];
			}
			string[] result;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_UsersInRoles_GetRolesForUser", sqlConnectionHolder.Connection);
					SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
					SqlDataReader sqlDataReader = null;
					StringCollection stringCollection = new StringCollection();
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.CommandTimeout = this.CommandTimeout;
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlCommand.Parameters.Add(this.CreateInputParam("@UserName", SqlDbType.NVarChar, username));
					try
					{
						sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.SequentialAccess);
						while (sqlDataReader.Read())
						{
							stringCollection.Add(sqlDataReader.GetString(0));
						}
					}
					catch
					{
						throw;
					}
					finally
					{
						if (sqlDataReader != null)
						{
							sqlDataReader.Close();
						}
					}
					if (stringCollection.Count > 0)
					{
						string[] array = new string[stringCollection.Count];
						stringCollection.CopyTo(array, 0);
						result = array;
					}
					else
					{
						switch (this.GetReturnValue(sqlCommand))
						{
						case 0:
							result = new string[0];
							break;
						case 1:
							result = new string[0];
							break;
						default:
							throw new ProviderException(SR.GetString("Stored procedure call failed."));
						}
					}
				}
				finally
				{
					if (sqlConnectionHolder != null)
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
			return result;
		}
		public override void CreateRole(string roleName)
		{
			SecUtility.CheckParameter(ref roleName, true, true, true, 256, "roleName");
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Roles_CreateRole", sqlConnectionHolder.Connection);
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.CommandTimeout = this.CommandTimeout;
					SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlCommand.Parameters.Add(this.CreateInputParam("@RoleName", SqlDbType.NVarChar, roleName));
					sqlCommand.ExecuteNonQuery();
					switch (this.GetReturnValue(sqlCommand))
					{
					case 0:
						break;
					case 1:
						throw new ProviderException(SR.GetString("The role '{0}' already exists.", roleName));
					default:
						throw new ProviderException(SR.GetString("Stored procedure call failed."));
					}
				}
				finally
				{
					if (sqlConnectionHolder != null)
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
		}
		public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
		{
			SecUtility.CheckParameter(ref roleName, true, true, true, 256, "roleName");
			bool result;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Roles_DeleteRole", sqlConnectionHolder.Connection);
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.CommandTimeout = this.CommandTimeout;
					SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlCommand.Parameters.Add(this.CreateInputParam("@RoleName", SqlDbType.NVarChar, roleName));
					sqlCommand.Parameters.Add(this.CreateInputParam("@DeleteOnlyIfRoleIsEmpty", SqlDbType.Bit, throwOnPopulatedRole ? 1 : 0));
					sqlCommand.ExecuteNonQuery();
					int returnValue = this.GetReturnValue(sqlCommand);
					if (returnValue == 2)
					{
						throw new ProviderException(SR.GetString("This role cannot be deleted because there are users present in it."));
					}
					result = (returnValue == 0);
				}
				finally
				{
					if (sqlConnectionHolder != null)
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
			return result;
		}
		public override bool RoleExists(string roleName)
		{
			SecUtility.CheckParameter(ref roleName, true, true, true, 256, "roleName");
			bool result;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Roles_RoleExists", sqlConnectionHolder.Connection);
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.CommandTimeout = this.CommandTimeout;
					SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlCommand.Parameters.Add(this.CreateInputParam("@RoleName", SqlDbType.NVarChar, roleName));
					sqlCommand.ExecuteNonQuery();
					switch (this.GetReturnValue(sqlCommand))
					{
					case 0:
						result = false;
						break;
					case 1:
						result = true;
						break;
					default:
						throw new ProviderException(SR.GetString("Stored procedure call failed."));
					}
				}
				finally
				{
					if (sqlConnectionHolder != null)
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
			return result;
		}
		public override void AddUsersToRoles(string[] usernames, string[] roleNames)
		{
			SecUtility.CheckArrayParameter(ref roleNames, true, true, true, 256, "roleNames");
			SecUtility.CheckArrayParameter(ref usernames, true, true, true, 256, "usernames");
			bool flag = false;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					int i = usernames.Length;
					while (i > 0)
					{
						string text = usernames[usernames.Length - i];
						i--;
						int num = usernames.Length - i;
						while (num < usernames.Length && text.Length + usernames[num].Length + 1 < 4000)
						{
							text = text + "," + usernames[num];
							i--;
							num++;
						}
						int j = roleNames.Length;
						while (j > 0)
						{
							string text2 = roleNames[roleNames.Length - j];
							j--;
							num = roleNames.Length - j;
							while (num < roleNames.Length && text2.Length + roleNames[num].Length + 1 < 4000)
							{
								text2 = text2 + "," + roleNames[num];
								j--;
								num++;
							}
							if (!flag && (i > 0 || j > 0))
							{
								new SqlCommand("BEGIN TRANSACTION", sqlConnectionHolder.Connection).ExecuteNonQuery();
								flag = true;
							}
							this.AddUsersToRolesCore(sqlConnectionHolder.Connection, text, text2);
						}
					}
					if (flag)
					{
						new SqlCommand("COMMIT TRANSACTION", sqlConnectionHolder.Connection).ExecuteNonQuery();
						flag = false;
					}
				}
				catch
				{
					if (flag)
					{
						try
						{
							new SqlCommand("ROLLBACK TRANSACTION", sqlConnectionHolder.Connection).ExecuteNonQuery();
						}
						catch
						{
						}
						flag = false;
					}
					throw;
				}
				finally
				{
					if (sqlConnectionHolder != null)
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
		}
		private void AddUsersToRolesCore(SqlConnection conn, string usernames, string roleNames)
		{
			SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_UsersInRoles_AddUsersToRoles", conn);
			SqlDataReader sqlDataReader = null;
			SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
			string param = string.Empty;
			string param2 = string.Empty;
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandTimeout = this.CommandTimeout;
			sqlParameter.Direction = ParameterDirection.ReturnValue;
			sqlCommand.Parameters.Add(sqlParameter);
			sqlCommand.Parameters.Add(this.CreateInputParam("@RoleNames", SqlDbType.NVarChar, roleNames));
			sqlCommand.Parameters.Add(this.CreateInputParam("@UserNames", SqlDbType.NVarChar, usernames));
			sqlCommand.Parameters.Add(this.CreateInputParam("@CurrentTime", SqlDbType.DateTime, DateTime.Now));
			try
			{
				sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.SingleRow);
				if (sqlDataReader.Read())
				{
					if (sqlDataReader.FieldCount > 0)
					{
						param = sqlDataReader.GetString(0);
					}
					if (sqlDataReader.FieldCount > 1)
					{
						param2 = sqlDataReader.GetString(1);
					}
				}
			}
			finally
			{
				if (sqlDataReader != null)
				{
					sqlDataReader.Close();
				}
			}
			switch (this.GetReturnValue(sqlCommand))
			{
			case 0:
				return;
			case 1:
				throw new ProviderException(SR.GetString("The user '{0}' was not found.", param));
			case 2:
				throw new ProviderException(SR.GetString("The role '{0}' was not found.", param));
			case 3:
				throw new ProviderException(SR.GetString("The user '{0}' is already in role '{1}'.", param, param2));
			default:
				throw new ProviderException(SR.GetString("Stored procedure call failed."));
			}
		}
		public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
		{
			SecUtility.CheckArrayParameter(ref roleNames, true, true, true, 256, "roleNames");
			SecUtility.CheckArrayParameter(ref usernames, true, true, true, 256, "usernames");
			bool flag = false;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					int i = usernames.Length;
					while (i > 0)
					{
						string text = usernames[usernames.Length - i];
						i--;
						int num = usernames.Length - i;
						while (num < usernames.Length && text.Length + usernames[num].Length + 1 < 4000)
						{
							text = text + "," + usernames[num];
							i--;
							num++;
						}
						int j = roleNames.Length;
						while (j > 0)
						{
							string text2 = roleNames[roleNames.Length - j];
							j--;
							num = roleNames.Length - j;
							while (num < roleNames.Length && text2.Length + roleNames[num].Length + 1 < 4000)
							{
								text2 = text2 + "," + roleNames[num];
								j--;
								num++;
							}
							if (!flag && (i > 0 || j > 0))
							{
								new SqlCommand("BEGIN TRANSACTION", sqlConnectionHolder.Connection).ExecuteNonQuery();
								flag = true;
							}
							this.RemoveUsersFromRolesCore(sqlConnectionHolder.Connection, text, text2);
						}
					}
					if (flag)
					{
						new SqlCommand("COMMIT TRANSACTION", sqlConnectionHolder.Connection).ExecuteNonQuery();
						flag = false;
					}
				}
				catch
				{
					if (flag)
					{
						new SqlCommand("ROLLBACK TRANSACTION", sqlConnectionHolder.Connection).ExecuteNonQuery();
						flag = false;
					}
					throw;
				}
				finally
				{
					if (sqlConnectionHolder != null)
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
		}
		private void RemoveUsersFromRolesCore(SqlConnection conn, string usernames, string roleNames)
		{
			SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_UsersInRoles_RemoveUsersFromRoles", conn);
			SqlDataReader sqlDataReader = null;
			SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
			string param = string.Empty;
			string text = string.Empty;
			sqlCommand.CommandType = CommandType.StoredProcedure;
			sqlCommand.CommandTimeout = this.CommandTimeout;
			sqlParameter.Direction = ParameterDirection.ReturnValue;
			sqlCommand.Parameters.Add(sqlParameter);
			sqlCommand.Parameters.Add(this.CreateInputParam("@UserNames", SqlDbType.NVarChar, usernames));
			sqlCommand.Parameters.Add(this.CreateInputParam("@RoleNames", SqlDbType.NVarChar, roleNames));
			try
			{
				sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.SingleRow);
				if (sqlDataReader.Read())
				{
					if (sqlDataReader.FieldCount > 0)
					{
						param = sqlDataReader.GetString(0);
					}
					if (sqlDataReader.FieldCount > 1)
					{
						text = sqlDataReader.GetString(1);
					}
				}
			}
			finally
			{
				if (sqlDataReader != null)
				{
					sqlDataReader.Close();
				}
			}
			switch (this.GetReturnValue(sqlCommand))
			{
			case 0:
				return;
			case 1:
				throw new ProviderException(SR.GetString("The user '{0}' was not found.", param));
			case 2:
				throw new ProviderException(SR.GetString("The role '{0}' was not found.", text));
			case 3:
				throw new ProviderException(SR.GetString("The user '{0}' is already not in role '{1}'.", param, text));
			default:
				throw new ProviderException(SR.GetString("Stored procedure call failed."));
			}
		}
		public override string[] GetUsersInRole(string roleName)
		{
			SecUtility.CheckParameter(ref roleName, true, true, true, 256, "roleName");
			string[] result;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_UsersInRoles_GetUsersInRoles", sqlConnectionHolder.Connection);
					SqlDataReader sqlDataReader = null;
					SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
					StringCollection stringCollection = new StringCollection();
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.CommandTimeout = this.CommandTimeout;
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlCommand.Parameters.Add(this.CreateInputParam("@RoleName", SqlDbType.NVarChar, roleName));
					try
					{
						sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.SequentialAccess);
						while (sqlDataReader.Read())
						{
							stringCollection.Add(sqlDataReader.GetString(0));
						}
					}
					catch
					{
						throw;
					}
					finally
					{
						if (sqlDataReader != null)
						{
							sqlDataReader.Close();
						}
					}
					if (stringCollection.Count < 1)
					{
						switch (this.GetReturnValue(sqlCommand))
						{
						case 0:
							result = new string[0];
							break;
						case 1:
							throw new ProviderException(SR.GetString("The role '{0}' was not found.", roleName));
						default:
							throw new ProviderException(SR.GetString("Stored procedure call failed."));
						}
					}
					else
					{
						string[] array = new string[stringCollection.Count];
						stringCollection.CopyTo(array, 0);
						result = array;
					}
				}
				finally
				{
					if (sqlConnectionHolder != null)
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
			return result;
		}
		public override string[] GetAllRoles()
		{
			string[] result;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Roles_GetAllRoles", sqlConnectionHolder.Connection);
					StringCollection stringCollection = new StringCollection();
					SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
					SqlDataReader sqlDataReader = null;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.CommandTimeout = this.CommandTimeout;
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(sqlParameter);
					try
					{
						sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.SequentialAccess);
						while (sqlDataReader.Read())
						{
							stringCollection.Add(sqlDataReader.GetString(0));
						}
					}
					catch
					{
						throw;
					}
					finally
					{
						if (sqlDataReader != null)
						{
							sqlDataReader.Close();
						}
					}
					string[] array = new string[stringCollection.Count];
					stringCollection.CopyTo(array, 0);
					result = array;
				}
				finally
				{
					if (sqlConnectionHolder != null)
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
			return result;
		}
		public override string[] FindUsersInRole(string roleName, string usernameToMatch)
		{
			SecUtility.CheckParameter(ref roleName, true, true, true, 256, "roleName");
			SecUtility.CheckParameter(ref usernameToMatch, true, true, false, 256, "usernameToMatch");
			string[] result;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_UsersInRoles_FindUsersInRole", sqlConnectionHolder.Connection);
					SqlDataReader sqlDataReader = null;
					SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
					StringCollection stringCollection = new StringCollection();
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.CommandTimeout = this.CommandTimeout;
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlCommand.Parameters.Add(this.CreateInputParam("@RoleName", SqlDbType.NVarChar, roleName));
					sqlCommand.Parameters.Add(this.CreateInputParam("@UserNameToMatch", SqlDbType.NVarChar, usernameToMatch));
					try
					{
						sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.SequentialAccess);
						while (sqlDataReader.Read())
						{
							stringCollection.Add(sqlDataReader.GetString(0));
						}
					}
					catch
					{
						throw;
					}
					finally
					{
						if (sqlDataReader != null)
						{
							sqlDataReader.Close();
						}
					}
					if (stringCollection.Count < 1)
					{
						switch (this.GetReturnValue(sqlCommand))
						{
						case 0:
							result = new string[0];
							break;
						case 1:
							throw new ProviderException(SR.GetString("The role '{0}' was not found.", roleName));
						default:
							throw new ProviderException(SR.GetString("Stored procedure call failed."));
						}
					}
					else
					{
						string[] array = new string[stringCollection.Count];
						stringCollection.CopyTo(array, 0);
						result = array;
					}
				}
				finally
				{
					if (sqlConnectionHolder != null)
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
			return result;
		}
		private SqlParameter CreateInputParam(string paramName, SqlDbType dbType, object objValue)
		{
			SqlParameter sqlParameter = new SqlParameter(paramName, dbType);
			if (objValue == null)
			{
				objValue = string.Empty;
			}
			sqlParameter.Value = objValue;
			return sqlParameter;
		}
		private int GetReturnValue(SqlCommand sqlCommand_0)
		{
			IEnumerator enumerator = sqlCommand_0.Parameters.GetEnumerator();
			int result;
			try
			{
				while (enumerator.MoveNext())
				{
					SqlParameter sqlParameter = (SqlParameter)enumerator.Current;
					if (sqlParameter.Direction == ParameterDirection.ReturnValue && sqlParameter.Value != null && sqlParameter.Value is int)
					{
						result = (int)sqlParameter.Value;
						return result;
					}
				}
				return -1;
			}
			finally
			{
				IDisposable disposable = enumerator as IDisposable;
				if (disposable != null)
				{
					disposable.Dispose();
				}
			}
			return result;
		}
	}
}
