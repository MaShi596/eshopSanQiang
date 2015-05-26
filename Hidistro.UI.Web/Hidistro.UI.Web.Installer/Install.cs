using Hidistro.Core;
using Hidistro.Core.Configuration;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
namespace Hidistro.UI.Web.Installer
{
	public class Install : System.Web.UI.Page
	{
		protected System.Web.UI.HtmlControls.HtmlForm form1;
		protected System.Web.UI.WebControls.Label lblErrMessage;
		protected System.Web.UI.WebControls.TextBox txtDbServer;
		protected System.Web.UI.WebControls.TextBox txtDbName;
		protected System.Web.UI.WebControls.TextBox txtDbUsername;
		protected System.Web.UI.WebControls.TextBox txtDbPassword;
		protected System.Web.UI.WebControls.TextBox txtUsername;
		protected System.Web.UI.WebControls.TextBox txtEmail;
		protected System.Web.UI.WebControls.TextBox txtPassword;
		protected System.Web.UI.WebControls.TextBox txtPassword2;
		protected System.Web.UI.WebControls.CheckBox chkIsAddDemo;
		protected System.Web.UI.WebControls.TextBox txtSiteName;
		protected System.Web.UI.WebControls.TextBox txtSiteDescription;
		protected System.Web.UI.WebControls.Button btnInstall;
		protected System.Web.UI.WebControls.Label litSetpErrorMessage;
		private string action;
		private string dbServer;
		private string dbName;
		private string dbUsername;
		private string dbPassword;
		private string username;
		private string email;
		private string password;
		private string password2;
		private bool isAddDemo;
		private string siteName;
		private string siteDescription;
		private bool testSuccessed;
		private System.Collections.Generic.IList<string> errorMsgs;
		private void LoadParameters()
		{
			if (!string.IsNullOrEmpty(base.Request["isCallback"]) && base.Request["isCallback"] == "true")
			{
				this.action = base.Request["action"];
				this.dbServer = base.Request["DBServer"];
				this.dbName = base.Request["DBName"];
				this.dbUsername = base.Request["DBUsername"];
				this.dbPassword = base.Request["DBPassword"];
				this.username = base.Request["Username"];
				this.email = base.Request["Email"];
				this.password = base.Request["Password"];
				this.password2 = base.Request["Password2"];
				this.isAddDemo = (!string.IsNullOrEmpty(base.Request["IsAddDemo"]) && base.Request["IsAddDemo"] == "true");
				this.testSuccessed = (!string.IsNullOrEmpty(base.Request["TestSuccessed"]) && base.Request["TestSuccessed"] == "true");
				this.siteName = ((string.IsNullOrEmpty(base.Request["SiteName"]) || base.Request["SiteName"].Trim().Length == 0) ? "Hishop" : base.Request["SiteName"]);
				this.siteDescription = ((string.IsNullOrEmpty(base.Request["SiteDescription"]) || base.Request["SiteDescription"].Trim().Length == 0) ? "最安全，最专业的网上商店系统" : base.Request["SiteDescription"]);
				return;
			}
			this.dbServer = this.txtDbServer.Text;
			this.dbName = this.txtDbName.Text;
			this.dbUsername = this.txtDbUsername.Text;
			this.dbPassword = this.txtDbPassword.Text;
			this.username = this.txtUsername.Text;
			this.email = this.txtEmail.Text;
			this.password = this.txtPassword.Text;
			this.password2 = this.txtPassword2.Text;
			this.isAddDemo = this.chkIsAddDemo.Checked;
			this.siteName = ((this.txtSiteName.Text.Trim().Length == 0) ? "Hishop" : this.txtSiteName.Text);
			this.siteDescription = ((this.txtSiteDescription.Text.Trim().Length == 0) ? "最安全，最专业的网上商店系统" : this.txtSiteDescription.Text);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.LoadParameters();
			this.btnInstall.Click += new System.EventHandler(this.btnInstall_Click);
			if (!string.IsNullOrEmpty(base.Request["isCallback"]) && base.Request["isCallback"] == "true")
			{
				string str = "无效的操作类型：" + this.action;
				bool flag = false;
				if (this.action == "Test")
				{
					flag = this.ExecuteTest();
				}
				base.Response.Clear();
				base.Response.ContentType = "application/json";
				if (flag)
				{
					base.Response.Write("{\"Status\":\"OK\"}");
				}
				else
				{
					string text = "";
					if (this.errorMsgs != null && this.errorMsgs.Count > 0)
					{
						foreach (string current in this.errorMsgs)
						{
							text = text + "{\"Text\":\"" + current + "\"},";
						}
						text = text.Substring(0, text.Length - 1);
						this.errorMsgs.Clear();
					}
					else
					{
						text = "{\"Text\":\"" + str + "\"}";
					}
					base.Response.Write("{\"Status\":\"Fail\",\"ErrorMsgs\":[" + text + "]}");
				}
				base.Response.End();
			}
		}
		private void btnInstall_Click(object sender, System.EventArgs e)
		{
			string empty = string.Empty;
			if (!this.ValidateUser(out empty))
			{
				this.ShowMsg(empty, false);
				return;
			}
			if (!this.testSuccessed && !this.ExecuteTest())
			{
				this.ShowMsg("数据库链接信息有误", false);
				return;
			}
			if (!this.CreateDataSchema(out empty))
			{
				this.ShowMsg(empty, false);
				return;
			}
			if (!this.AddBuiltInRoles(out empty))
			{
				this.ShowMsg(empty, false);
				return;
			}
			if (!this.CreateAnonymous(out empty))
			{
				this.ShowMsg(empty, false);
				return;
			}
			int num;
			if (!this.CreateAdministrator(out num, out empty))
			{
				this.ShowMsg(empty, false);
				return;
			}
			if (!this.AddInitData(out empty))
			{
				this.ShowMsg(empty, false);
				return;
			}
			if (this.isAddDemo && !this.AddDemoData(out empty))
			{
				return;
			}
			if (!this.SaveSiteSettings(out empty))
			{
				this.ShowMsg(empty, false);
				return;
			}
			if (!this.SaveConfig(out empty))
			{
				this.ShowMsg(empty, false);
				return;
			}
			this.Context.Response.Redirect(Globals.GetSiteUrls().Home, true);
		}
		private void ShowMsg(string errorMsg, bool seccess)
		{
			this.lblErrMessage.Text = errorMsg;
		}
		private bool CreateDataSchema(out string errorMsg)
		{
			string text = base.Request.MapPath("SqlScripts/Schema.sql");
			if (!System.IO.File.Exists(text))
			{
				errorMsg = "没有找到数据库架构文件-Schema.sql";
				return false;
			}
			return this.ExecuteScriptFile(text, out errorMsg);
		}
		private bool AddBuiltInRoles(out string errorMsg)
		{
			System.Data.Common.DbConnection dbConnection = null;
			System.Data.Common.DbTransaction dbTransaction = null;
			bool result;
			try
			{
				System.Data.Common.DbConnection dbConnection2;
				dbConnection = (dbConnection2 = new System.Data.SqlClient.SqlConnection(this.GetConnectionString()));
				try
				{
					dbConnection.Open();
					System.Data.Common.DbCommand dbCommand = dbConnection.CreateCommand();
					dbTransaction = dbConnection.BeginTransaction();
					dbCommand.Connection = dbConnection;
					dbCommand.Transaction = dbTransaction;
					dbCommand.CommandType = System.Data.CommandType.Text;
					dbCommand.CommandText = "INSERT INTO aspnet_Roles(RoleName, LoweredRoleName) VALUES(@RoleName, LOWER(@RoleName))";
					System.Data.Common.DbParameter value = new System.Data.SqlClient.SqlParameter("@RoleName", System.Data.SqlDbType.NVarChar, 256);
					dbCommand.Parameters.Add(value);
					RolesConfiguration rolesConfiguration = HiConfiguration.GetConfig().RolesConfiguration;
					dbCommand.Parameters["@RoleName"].Value = rolesConfiguration.Distributor;
					dbCommand.ExecuteNonQuery();
					dbCommand.Parameters["@RoleName"].Value = rolesConfiguration.Manager;
					dbCommand.ExecuteNonQuery();
					dbCommand.Parameters["@RoleName"].Value = rolesConfiguration.Member;
					dbCommand.ExecuteNonQuery();
					dbCommand.Parameters["@RoleName"].Value = rolesConfiguration.SystemAdministrator;
					dbCommand.ExecuteNonQuery();
					dbCommand.Parameters["@RoleName"].Value = rolesConfiguration.Underling;
					dbCommand.ExecuteNonQuery();
					dbTransaction.Commit();
					dbConnection.Close();
				}
				finally
				{
					if (dbConnection2 != null)
					{
						((System.IDisposable)dbConnection2).Dispose();
					}
				}
				errorMsg = null;
				result = true;
			}
			catch (System.Data.SqlClient.SqlException ex)
			{
				errorMsg = ex.Message;
				if (dbTransaction != null)
				{
					try
					{
						dbTransaction.Rollback();
					}
					catch (System.Exception ex2)
					{
						errorMsg = ex2.Message;
					}
				}
				if (dbConnection != null && dbConnection.State != System.Data.ConnectionState.Closed)
				{
					dbConnection.Close();
					dbConnection.Dispose();
				}
				result = false;
			}
			return result;
		}
		private bool CreateAnonymous(out string errorMsg)
		{
			System.Data.Common.DbConnection dbConnection = null;
			bool result;
			try
			{
				System.Data.Common.DbConnection dbConnection2;
				dbConnection = (dbConnection2 = new System.Data.SqlClient.SqlConnection(this.GetConnectionString()));
				try
				{
					System.Data.Common.DbCommand dbCommand = new System.Data.SqlClient.SqlCommand();
					dbCommand.Connection = dbConnection;
					dbCommand.CommandType = System.Data.CommandType.Text;
					dbCommand.CommandText = "INSERT INTO aspnet_Users  (UserName, LoweredUserName, IsAnonymous, UserRole, LastActivityDate, Password, PasswordFormat, PasswordSalt, IsApproved, IsLockedOut, CreateDate, LastLoginDate, LastPasswordChangedDate, LastLockoutDate, FailedPasswordAttemptCount, FailedPasswordAttemptWindowStart, FailedPasswordAnswerAttemptCount, FailedPasswordAnswerAttemptWindowStart) VALUES ('Anonymous', LOWER('Anonymous'), 1, @UserRole, @CreateDate, 'DVZTktxeMzDtXR7eik7Cdw==', 0, '', 1, 0, @CreateDate, @CreateDate, @CreateDate, CONVERT( datetime, '17540101', 112 ), 0, CONVERT( datetime, '17540101', 112 ), 0, CONVERT( datetime, '17540101', 112 ))";
					dbCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter("@UserRole", Hidistro.Membership.Core.Enums.UserRole.Anonymous));
					dbCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CreateDate", System.DateTime.Now));
					dbConnection.Open();
					dbCommand.ExecuteNonQuery();
					dbConnection.Close();
				}
				finally
				{
					if (dbConnection2 != null)
					{
						((System.IDisposable)dbConnection2).Dispose();
					}
				}
				errorMsg = null;
				result = true;
			}
			catch (System.Data.SqlClient.SqlException ex)
			{
				errorMsg = ex.Message;
				if (dbConnection != null && dbConnection.State != System.Data.ConnectionState.Closed)
				{
					dbConnection.Close();
					dbConnection.Dispose();
				}
				result = false;
			}
			return result;
		}
		private bool CreateAdministrator(out int newUserId, out string errorMsg)
		{
			System.Data.Common.DbConnection dbConnection = null;
			System.Data.Common.DbTransaction dbTransaction = null;
			bool result;
			try
			{
				System.Data.Common.DbConnection dbConnection2;
				dbConnection = (dbConnection2 = new System.Data.SqlClient.SqlConnection(this.GetConnectionString()));
				try
				{
					dbConnection.Open();
					RolesConfiguration rolesConfiguration = HiConfiguration.GetConfig().RolesConfiguration;
					System.Data.Common.DbCommand dbCommand = dbConnection.CreateCommand();
					dbTransaction = dbConnection.BeginTransaction();
					dbCommand.Connection = dbConnection;
					dbCommand.Transaction = dbTransaction;
					dbCommand.CommandType = System.Data.CommandType.Text;
					dbCommand.CommandText = "SELECT RoleId FROM aspnet_Roles WHERE [LoweredRoleName] = LOWER(@RoleName)";
					dbCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RoleName", rolesConfiguration.SystemAdministrator));
					System.Guid guid = (System.Guid)dbCommand.ExecuteScalar();
					dbCommand.Parameters["@RoleName"].Value = rolesConfiguration.Manager;
					System.Guid guid2 = (System.Guid)dbCommand.ExecuteScalar();
					dbCommand.Parameters.Clear();
					dbCommand.CommandText = "INSERT INTO aspnet_Users  (UserName, LoweredUserName, IsAnonymous, UserRole, LastActivityDate, Password, PasswordFormat, PasswordSalt, IsApproved, IsLockedOut, CreateDate, LastLoginDate, LastPasswordChangedDate, LastLockoutDate, FailedPasswordAttemptCount, FailedPasswordAttemptWindowStart, FailedPasswordAnswerAttemptCount, FailedPasswordAnswerAttemptWindowStart, Email, LoweredEmail) VALUES (@Username, LOWER(@Username), 0, @UserRole, @CreateDate, @Password, @PasswordFormat, @PasswordSalt, 1, 0, @CreateDate, @CreateDate, @CreateDate, CONVERT( datetime, '17540101', 112 ), 0, CONVERT( datetime, '17540101', 112 ), 0, CONVERT( datetime, '17540101', 112 ), @Email, LOWER(@Email));SELECT @@IDENTITY";
					dbCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Username", this.username));
					dbCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter("@UserRole", Hidistro.Membership.Core.Enums.UserRole.SiteManager));
					dbCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter("@CreateDate", System.DateTime.Now));
					dbCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Password", this.password));
					dbCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PasswordFormat", System.Web.Security.MembershipPasswordFormat.Clear));
					dbCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter("@PasswordSalt", ""));
					dbCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter("@Email", this.email));
					newUserId = System.Convert.ToInt32(dbCommand.ExecuteScalar());
					dbCommand.Parameters.Clear();
					dbCommand.CommandText = "INSERT INTO aspnet_Managers(UserId) VALUES(@UserId)";
					dbCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter("@UserId", newUserId));
					dbCommand.ExecuteNonQuery();
					dbCommand.CommandText = "INSERT INTO aspnet_UsersInRoles(UserId, RoleId) VALUES(@UserId, @RoleId)";
					dbCommand.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RoleId", guid2));
					dbCommand.ExecuteNonQuery();
					dbCommand.Parameters["@RoleId"].Value = guid;
					dbCommand.ExecuteNonQuery();
					dbTransaction.Commit();
					dbConnection.Close();
				}
				finally
				{
					if (dbConnection2 != null)
					{
						((System.IDisposable)dbConnection2).Dispose();
					}
				}
				errorMsg = null;
				result = true;
			}
			catch (System.Data.SqlClient.SqlException ex)
			{
				errorMsg = ex.Message;
				newUserId = 0;
				if (dbTransaction != null)
				{
					try
					{
						dbTransaction.Rollback();
					}
					catch (System.Exception ex2)
					{
						errorMsg = ex2.Message;
					}
				}
				if (dbConnection != null && dbConnection.State != System.Data.ConnectionState.Closed)
				{
					dbConnection.Close();
					dbConnection.Dispose();
				}
				result = false;
			}
			return result;
		}
		private bool AddInitData(out string errorMsg)
		{
			string text = base.Request.MapPath("SqlScripts/SiteInitData.zh-CN.Sql");
			if (!System.IO.File.Exists(text))
			{
				errorMsg = "没有找到初始化数据文件-SiteInitData.Sql";
				return false;
			}
			return this.ExecuteScriptFile(text, out errorMsg);
		}
		private bool AddDemoData(out string errorMsg)
		{
			string text = base.Request.MapPath("SqlScripts/SiteDemo.zh-CN.sql");
			if (!System.IO.File.Exists(text))
			{
				errorMsg = "没有找到演示数据文件-SiteDemo.Sql";
				return false;
			}
			return this.ExecuteScriptFile(text, out errorMsg);
		}
		private bool SaveSiteSettings(out string errorMsg)
		{
			errorMsg = null;
			if (this.siteName.Length <= 30 && this.siteDescription.Length <= 30)
			{
				bool result;
				try
				{
					string filename = base.Request.MapPath(Globals.ApplicationPath + "/config/SiteSettings.config");
					System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
					Hidistro.Membership.Context.SiteSettings siteSettings = new Hidistro.Membership.Context.SiteSettings(base.Request.Url.Host, null);
					xmlDocument.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\"?>" + System.Environment.NewLine + "<Settings></Settings>");
					siteSettings.SiteName = this.siteName;
					siteSettings.SiteDescription = this.siteDescription;
					siteSettings.WriteToXml(xmlDocument);
					xmlDocument.Save(filename);
					result = true;
				}
				catch (System.Exception ex)
				{
					errorMsg = ex.Message;
					result = false;
				}
				return result;
			}
			errorMsg = "网店名称和简单介绍的长度不能超过30个字符";
			return false;
		}
		private bool SaveConfig(out string errorMsg)
		{
			bool result;
			try
			{
				Configuration configuration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(base.Request.ApplicationPath);
				configuration.AppSettings.Settings.Remove("Installer");
				using (System.Security.Cryptography.RijndaelManaged cryptographer = this.GetCryptographer())
				{
					configuration.AppSettings.Settings["IV"].Value = System.Convert.ToBase64String(cryptographer.IV);
					configuration.AppSettings.Settings["Key"].Value = System.Convert.ToBase64String(cryptographer.Key);
				}
				System.Web.Configuration.MachineKeySection machineKeySection = (System.Web.Configuration.MachineKeySection)configuration.GetSection("system.web/machineKey");
				machineKeySection.ValidationKey = Install.CreateKey(20);
				machineKeySection.DecryptionKey = Install.CreateKey(24);
				machineKeySection.Validation = System.Web.Configuration.MachineKeyValidation.SHA1;
				machineKeySection.Decryption = "3DES";
				configuration.ConnectionStrings.ConnectionStrings["HidistroSqlServer"].ConnectionString = this.GetConnectionString();
				configuration.ConnectionStrings.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
				configuration.Save();
				errorMsg = null;
				result = true;
			}
			catch (System.Exception ex)
			{
				errorMsg = ex.Message;
				result = false;
			}
			return result;
		}
		private System.Security.Cryptography.RijndaelManaged GetCryptographer()
		{
			System.Security.Cryptography.RijndaelManaged rijndaelManaged = new System.Security.Cryptography.RijndaelManaged();
			rijndaelManaged.KeySize = 128;
			rijndaelManaged.GenerateIV();
			rijndaelManaged.GenerateKey();
			return rijndaelManaged;
		}
		private bool ExecuteTest()
		{
			this.errorMsgs = new System.Collections.Generic.List<string>();
			System.Data.Common.DbTransaction dbTransaction = null;
			System.Data.Common.DbConnection dbConnection = null;
			string item;
			try
			{
				if (this.ValidateConnectionStrings(out item))
				{
					System.Data.Common.DbConnection dbConnection2;
					dbConnection = (dbConnection2 = new System.Data.SqlClient.SqlConnection(this.GetConnectionString()));
					try
					{
						dbConnection.Open();
						System.Data.Common.DbCommand dbCommand = dbConnection.CreateCommand();
						dbTransaction = dbConnection.BeginTransaction();
						dbCommand.Connection = dbConnection;
						dbCommand.Transaction = dbTransaction;
						dbCommand.CommandText = "CREATE TABLE installTest(Test bit NULL)";
						dbCommand.ExecuteNonQuery();
						dbCommand.CommandText = "DROP TABLE installTest";
						dbCommand.ExecuteNonQuery();
						dbTransaction.Commit();
						dbConnection.Close();
						goto IL_9C;
					}
					finally
					{
						if (dbConnection2 != null)
						{
							((System.IDisposable)dbConnection2).Dispose();
						}
					}
				}
				this.errorMsgs.Add(item);
				IL_9C:;
			}
			catch (System.Exception ex)
			{
				this.errorMsgs.Add(ex.Message);
				if (dbTransaction != null)
				{
					try
					{
						dbTransaction.Rollback();
					}
					catch (System.Exception ex2)
					{
						this.errorMsgs.Add(ex2.Message);
					}
				}
				if (dbConnection != null && dbConnection.State != System.Data.ConnectionState.Closed)
				{
					dbConnection.Close();
					dbConnection.Dispose();
				}
			}
			string folderPath = base.Request.MapPath(Globals.ApplicationPath + "/config/test.txt");
			if (!Install.TestFolder(folderPath, out item))
			{
				this.errorMsgs.Add(item);
			}
			try
			{
				Configuration configuration = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(base.Request.ApplicationPath);
				if (configuration.ConnectionStrings.ConnectionStrings["HidistroSqlServer"].ConnectionString == "none")
				{
					configuration.ConnectionStrings.ConnectionStrings["HidistroSqlServer"].ConnectionString = "required";
				}
				else
				{
					configuration.ConnectionStrings.ConnectionStrings["HidistroSqlServer"].ConnectionString = "none";
				}
				configuration.Save();
			}
			catch (System.Exception ex3)
			{
				this.errorMsgs.Add(ex3.Message);
			}
			folderPath = base.Request.MapPath(Globals.ApplicationPath + "/storage/test.txt");
			if (!Install.TestFolder(folderPath, out item))
			{
				this.errorMsgs.Add(item);
			}
			return this.errorMsgs.Count == 0;
		}
		private bool ExecuteScriptFile(string pathToScriptFile, out string errorMsg)
		{
			System.IO.StreamReader streamReader = null;
			System.Data.SqlClient.SqlConnection sqlConnection = null;
			bool result;
			try
			{
				string applicationPath = Globals.ApplicationPath;
				System.IO.StreamReader streamReader2;
				streamReader = (streamReader2 = new System.IO.StreamReader(pathToScriptFile));
				try
				{
					System.Data.SqlClient.SqlConnection sqlConnection2;
					sqlConnection = (sqlConnection2 = new System.Data.SqlClient.SqlConnection(this.GetConnectionString()));
					try
					{
						System.Data.Common.DbCommand dbCommand = new System.Data.SqlClient.SqlCommand
						{
							Connection = sqlConnection,
							CommandType = System.Data.CommandType.Text,
							CommandTimeout = 60
						};
						sqlConnection.Open();
						while (!streamReader.EndOfStream)
						{
							string text = Install.NextSqlFromStream(streamReader);
							if (!string.IsNullOrEmpty(text))
							{
								dbCommand.CommandText = text.Replace("$VirsualPath$", applicationPath);
								dbCommand.ExecuteNonQuery();
							}
						}
						sqlConnection.Close();
					}
					finally
					{
						if (sqlConnection2 != null)
						{
							((System.IDisposable)sqlConnection2).Dispose();
						}
					}
					streamReader.Close();
				}
				finally
				{
					if (streamReader2 != null)
					{
						((System.IDisposable)streamReader2).Dispose();
					}
				}
				errorMsg = null;
				result = true;
			}
			catch (System.Data.SqlClient.SqlException ex)
			{
				errorMsg = ex.Message;
				if (sqlConnection != null && sqlConnection.State != System.Data.ConnectionState.Closed)
				{
					sqlConnection.Close();
					sqlConnection.Dispose();
				}
				if (streamReader != null)
				{
					streamReader.Close();
					streamReader.Dispose();
				}
				result = false;
			}
			return result;
		}
		private static string NextSqlFromStream(System.IO.StreamReader reader)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			string text = reader.ReadLine().Trim();
			while (!reader.EndOfStream && string.Compare(text, "GO", true, System.Globalization.CultureInfo.InvariantCulture) != 0)
			{
				stringBuilder.Append(text + System.Environment.NewLine);
				text = reader.ReadLine();
			}
			if (string.Compare(text, "GO", true, System.Globalization.CultureInfo.InvariantCulture) != 0)
			{
				stringBuilder.Append(text + System.Environment.NewLine);
			}
			return stringBuilder.ToString();
		}
		private bool ValidateConnectionStrings(out string msg)
		{
			msg = null;
			if (!string.IsNullOrEmpty(this.dbServer) && !string.IsNullOrEmpty(this.dbName) && !string.IsNullOrEmpty(this.dbUsername))
			{
				return true;
			}
			msg = "数据库连接信息不完整";
			return false;
		}
		private bool ValidateUser(out string msg)
		{
			msg = null;
			if (string.IsNullOrEmpty(this.username) || string.IsNullOrEmpty(this.email) || string.IsNullOrEmpty(this.password) || string.IsNullOrEmpty(this.password2))
			{
				msg = "管理员账号信息不完整";
				return false;
			}
			HiConfiguration config = HiConfiguration.GetConfig();
			if (this.username.Length > config.UsernameMaxLength || this.username.Length < config.UsernameMinLength)
			{
				msg = string.Format("管理员用户名的长度只能在{0}和{1}个字符之间", config.UsernameMinLength, config.UsernameMaxLength);
				return false;
			}
			if (string.Compare(this.username, "anonymous", true) == 0)
			{
				msg = "不能使用anonymous作为管理员用户名";
				return false;
			}
			if (!System.Text.RegularExpressions.Regex.IsMatch(this.username, config.UsernameRegex))
			{
				msg = "管理员用户名的格式不符合要求，用户名一般由字母、数字、下划线和汉字组成，且必须以汉字或字母开头";
				return false;
			}
			if (this.email.Length > 256)
			{
				msg = "电子邮件的长度必须小于256个字符";
				return false;
			}
			if (!System.Text.RegularExpressions.Regex.IsMatch(this.email, config.EmailRegex))
			{
				msg = "电子邮件的格式错误";
				return false;
			}
			if (this.password != this.password2)
			{
				msg = "管理员登录密码两次输入不一致";
				return false;
			}
			if (this.password.Length >= System.Web.Security.Membership.Provider.MinRequiredPasswordLength && this.password.Length <= config.PasswordMaxLength)
			{
				return true;
			}
			msg = string.Format("管理员登录密码的长度只能在{0}和{1}个字符之间", System.Web.Security.Membership.Provider.MinRequiredPasswordLength, config.PasswordMaxLength);
			return false;
		}
		private string GetConnectionString()
		{
			return string.Format("server={0};uid={1};pwd={2};Trusted_Connection=no;database={3}", new object[]
			{
				this.dbServer,
				this.dbUsername,
				this.dbPassword,
				this.dbName
			});
		}
		private static string CreateKey(int len)
		{
			byte[] array = new byte[len];
			new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(array);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(string.Format("{0:X2}", array[i]));
			}
			return stringBuilder.ToString();
		}
		private static bool TestFolder(string folderPath, out string errorMsg)
		{
			bool result;
			try
			{
				System.IO.File.WriteAllText(folderPath, "Hi");
				System.IO.File.AppendAllText(folderPath, ",This is a test file.");
				System.IO.File.Delete(folderPath);
				errorMsg = null;
				result = true;
			}
			catch (System.Exception ex)
			{
				errorMsg = ex.Message;
				result = false;
			}
			return result;
		}
	}
}
