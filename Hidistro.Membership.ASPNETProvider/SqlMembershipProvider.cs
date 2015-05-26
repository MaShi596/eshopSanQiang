using System;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
namespace Hidistro.Membership.ASPNETProvider
{
	public class SqlMembershipProvider : MembershipProvider
	{
		private const int PASSWORD_SIZE = 14;
		private string _sqlConnectionString;
		private bool _EnablePasswordRetrieval;
		private bool _EnablePasswordReset;
		private bool _RequiresQuestionAndAnswer;
		private string _AppName;
		private bool _RequiresUniqueEmail;
		private int _MaxInvalidPasswordAttempts;
		private int _CommandTimeout;
		private int _PasswordAttemptWindow;
		private int _MinRequiredPasswordLength;
		private int _MinRequiredNonalphanumericCharacters;
		private string _PasswordStrengthRegularExpression;
		private MembershipPasswordFormat _PasswordFormat;
		public override bool EnablePasswordRetrieval
		{
			get
			{
				return this._EnablePasswordRetrieval;
			}
		}
		public override bool EnablePasswordReset
		{
			get
			{
				return this._EnablePasswordReset;
			}
		}
		public override bool RequiresQuestionAndAnswer
		{
			get
			{
				return this._RequiresQuestionAndAnswer;
			}
		}
		public override bool RequiresUniqueEmail
		{
			get
			{
				return this._RequiresUniqueEmail;
			}
		}
		public override MembershipPasswordFormat PasswordFormat
		{
			get
			{
				return this._PasswordFormat;
			}
		}
		public override int MaxInvalidPasswordAttempts
		{
			get
			{
				return this._MaxInvalidPasswordAttempts;
			}
		}
		public override int PasswordAttemptWindow
		{
			get
			{
				return this._PasswordAttemptWindow;
			}
		}
		public override int MinRequiredPasswordLength
		{
			get
			{
				return this._MinRequiredPasswordLength;
			}
		}
		public override int MinRequiredNonAlphanumericCharacters
		{
			get
			{
				return this._MinRequiredNonalphanumericCharacters;
			}
		}
		public override string PasswordStrengthRegularExpression
		{
			get
			{
				return this._PasswordStrengthRegularExpression;
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
				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentNullException("value");
				}
				if (value.Length > 256)
				{
					throw new ProviderException(SR.GetString("The application name is too long."));
				}
				this._AppName = value;
			}
		}
		private int CommandTimeout
		{
			get
			{
				return this._CommandTimeout;
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
				name = "SqlMembershipProvider";
			}
			if (string.IsNullOrEmpty(config["description"]))
			{
				config.Remove("description");
				config.Add("description", SR.GetString("SQL membership provider."));
			}
			base.Initialize(name, config);
			this._EnablePasswordRetrieval = SecUtility.GetBooleanValue(config, "enablePasswordRetrieval", false);
			this._EnablePasswordReset = SecUtility.GetBooleanValue(config, "enablePasswordReset", true);
			this._RequiresQuestionAndAnswer = SecUtility.GetBooleanValue(config, "requiresQuestionAndAnswer", true);
			this._RequiresUniqueEmail = SecUtility.GetBooleanValue(config, "requiresUniqueEmail", true);
			this._MaxInvalidPasswordAttempts = SecUtility.GetIntValue(config, "maxInvalidPasswordAttempts", 5, false, 0);
			this._PasswordAttemptWindow = SecUtility.GetIntValue(config, "passwordAttemptWindow", 10, false, 0);
			this._MinRequiredPasswordLength = SecUtility.GetIntValue(config, "minRequiredPasswordLength", 7, false, 128);
			this._MinRequiredNonalphanumericCharacters = SecUtility.GetIntValue(config, "minRequiredNonalphanumericCharacters", 1, true, 128);
			this._PasswordStrengthRegularExpression = config["passwordStrengthRegularExpression"];
			if (this._PasswordStrengthRegularExpression != null)
			{
				this._PasswordStrengthRegularExpression = this._PasswordStrengthRegularExpression.Trim();
				if (this._PasswordStrengthRegularExpression.Length == 0)
				{
					goto IL_157;
				}
				try
				{
					new Regex(this._PasswordStrengthRegularExpression);
					goto IL_157;
				}
				catch (ArgumentException ex)
				{
					throw new ProviderException(ex.Message, ex);
				}
			}
			this._PasswordStrengthRegularExpression = string.Empty;
			IL_157:
			if (this._MinRequiredNonalphanumericCharacters > this._MinRequiredPasswordLength)
			{
				throw new HttpException(SR.GetString("The minRequiredNonalphanumericCharacters can not be greater than minRequiredPasswordLength."));
			}
			this._CommandTimeout = SecUtility.GetIntValue(config, "commandTimeout", 30, true, 0);
			this._AppName = config["applicationName"];
			if (string.IsNullOrEmpty(this._AppName))
			{
				this._AppName = SecUtility.GetDefaultAppName();
			}
			if (this._AppName.Length > 256)
			{
				throw new ProviderException(SR.GetString("The application name is too long."));
			}
			string text = config["passwordFormat"];
			if (text == null)
			{
				text = "Hashed";
			}
			string a;
			if ((a = text) != null)
			{
				if (!(a == "Clear"))
				{
					if (!(a == "Encrypted"))
					{
						if (!(a == "Hashed"))
						{
							goto IL_386;
						}
						this._PasswordFormat = MembershipPasswordFormat.Hashed;
					}
					else
					{
						this._PasswordFormat = MembershipPasswordFormat.Encrypted;
					}
				}
				else
				{
					this._PasswordFormat = MembershipPasswordFormat.Clear;
				}
				if (this.PasswordFormat == MembershipPasswordFormat.Hashed && this.EnablePasswordRetrieval)
				{
					throw new ProviderException(SR.GetString("Configured settings are invalid: Hashed passwords cannot be retrieved. Either set the password format to different type, or set supportsPasswordRetrieval to false."));
				}
				string text2 = config["connectionStringName"];
				if (text2 == null || text2.Length < 1)
				{
					throw new ProviderException(SR.GetString("The attribute 'connectionStringName' is missing or empty."));
				}
				this._sqlConnectionString = SqlConnectionHelper.GetConnectionString(text2, true, true);
				if (this._sqlConnectionString != null && this._sqlConnectionString.Length >= 1)
				{
					config.Remove("connectionStringName");
					config.Remove("enablePasswordRetrieval");
					config.Remove("enablePasswordReset");
					config.Remove("requiresQuestionAndAnswer");
					config.Remove("applicationName");
					config.Remove("requiresUniqueEmail");
					config.Remove("maxInvalidPasswordAttempts");
					config.Remove("passwordAttemptWindow");
					config.Remove("commandTimeout");
					config.Remove("passwordFormat");
					config.Remove("name");
					config.Remove("minRequiredPasswordLength");
					config.Remove("minRequiredNonalphanumericCharacters");
					config.Remove("passwordStrengthRegularExpression");
					if (config.Count > 0)
					{
						string key = config.GetKey(0);
						if (!string.IsNullOrEmpty(key))
						{
							throw new ProviderException(SR.GetString("Attribute not recognized '{0}'", key));
						}
					}
					return;
				}
				throw new ProviderException(SR.GetString("The connection name '{0}' was not found in the applications configuration or the connection string is empty.", text2));
			}
			IL_386:
			throw new ProviderException(SR.GetString("Password format specified is invalid."));
		}
		public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
		{
			if (!SecUtility.ValidateParameter(ref password, true, true, false, 128))
			{
				status = MembershipCreateStatus.InvalidPassword;
				return null;
			}
			string text = this.GenerateSalt();
			string text2 = this.EncodePassword(password, (int)this._PasswordFormat, text);
			if (text2.Length > 128)
			{
				status = MembershipCreateStatus.InvalidPassword;
				return null;
			}
			if (passwordAnswer != null)
			{
				passwordAnswer = passwordAnswer.Trim();
			}
			string objValue;
			if (!string.IsNullOrEmpty(passwordAnswer))
			{
				if (passwordAnswer.Length > 128)
				{
					status = MembershipCreateStatus.InvalidAnswer;
					return null;
				}
				objValue = this.EncodePassword(passwordAnswer.ToLower(CultureInfo.InvariantCulture), (int)this._PasswordFormat, text);
			}
			else
			{
				objValue = passwordAnswer;
			}
			if (!SecUtility.ValidateParameter(ref objValue, this.RequiresQuestionAndAnswer, true, false, 128))
			{
				status = MembershipCreateStatus.InvalidAnswer;
				return null;
			}
			if (!SecUtility.ValidateParameter(ref username, true, true, true, 256))
			{
				status = MembershipCreateStatus.InvalidUserName;
				return null;
			}
			if (!SecUtility.ValidateParameter(ref email, this.RequiresUniqueEmail, this.RequiresUniqueEmail, false, 256))
			{
				status = MembershipCreateStatus.InvalidEmail;
				return null;
			}
			if (!SecUtility.ValidateParameter(ref passwordQuestion, this.RequiresQuestionAndAnswer, true, false, 256))
			{
				status = MembershipCreateStatus.InvalidQuestion;
				return null;
			}
			if (password.Length < this.MinRequiredPasswordLength)
			{
				status = MembershipCreateStatus.InvalidPassword;
				return null;
			}
			int num = 0;
			for (int i = 0; i < password.Length; i++)
			{
				if (!char.IsLetterOrDigit(password, i))
				{
					num++;
				}
			}
			if (num < this.MinRequiredNonAlphanumericCharacters)
			{
				status = MembershipCreateStatus.InvalidPassword;
				return null;
			}
			if (this.PasswordStrengthRegularExpression.Length > 0 && !Regex.IsMatch(password, this.PasswordStrengthRegularExpression))
			{
				status = MembershipCreateStatus.InvalidPassword;
				return null;
			}
			ValidatePasswordEventArgs validatePasswordEventArgs = new ValidatePasswordEventArgs(username, password, true);
			this.OnValidatingPassword(validatePasswordEventArgs);
			if (validatePasswordEventArgs.Cancel)
			{
				status = MembershipCreateStatus.InvalidPassword;
				return null;
			}
			MembershipUser result;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					DateTime dateTime = this.RoundToSeconds(DateTime.Now);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Membership_CreateUser", sqlConnectionHolder.Connection);
					sqlCommand.CommandTimeout = this.CommandTimeout;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.Parameters.Add(this.CreateInputParam("@UserName", SqlDbType.NVarChar, username));
					sqlCommand.Parameters.Add(this.CreateInputParam("@Password", SqlDbType.NVarChar, text2));
					sqlCommand.Parameters.Add(this.CreateInputParam("@PasswordSalt", SqlDbType.NVarChar, text));
					sqlCommand.Parameters.Add(this.CreateInputParam("@Email", SqlDbType.NVarChar, email));
					sqlCommand.Parameters.Add(this.CreateInputParam("@PasswordQuestion", SqlDbType.NVarChar, passwordQuestion));
					sqlCommand.Parameters.Add(this.CreateInputParam("@PasswordAnswer", SqlDbType.NVarChar, objValue));
					sqlCommand.Parameters.Add(this.CreateInputParam("@IsApproved", SqlDbType.Bit, isApproved));
					sqlCommand.Parameters.Add(this.CreateInputParam("@UniqueEmail", SqlDbType.Int, this.RequiresUniqueEmail ? 1 : 0));
					sqlCommand.Parameters.Add(this.CreateInputParam("@PasswordFormat", SqlDbType.Int, (int)this.PasswordFormat));
					sqlCommand.Parameters.Add(this.CreateInputParam("@CurrentTime", SqlDbType.DateTime, dateTime));
					SqlParameter sqlParameter = this.CreateInputParam("@UserId", SqlDbType.Int, providerUserKey);
					sqlParameter.Direction = ParameterDirection.InputOutput;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlCommand.ExecuteNonQuery();
					int num2 = (sqlParameter.Value != null) ? ((int)sqlParameter.Value) : -1;
					if (num2 < 0 || num2 > 11)
					{
						num2 = 11;
					}
					status = (MembershipCreateStatus)num2;
					if (num2 != 0)
					{
						result = null;
					}
					else
					{
						providerUserKey = (int)sqlCommand.Parameters["@UserId"].Value;
						result = new MembershipUser(this.Name, username, providerUserKey, email, passwordQuestion, null, isApproved, false, dateTime, dateTime, dateTime, dateTime, new DateTime(1754, 1, 1));
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
		public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
		{
			SecUtility.CheckParameter(ref username, true, true, true, 256, "username");
			SecUtility.CheckParameter(ref password, true, true, false, 128, "password");
			string salt;
			int passwordFormat;
			if (!this.CheckPassword(username, password, false, false, out salt, out passwordFormat))
			{
				return false;
			}
			SecUtility.CheckParameter(ref newPasswordQuestion, this.RequiresQuestionAndAnswer, this.RequiresQuestionAndAnswer, false, 256, "newPasswordQuestion");
			if (newPasswordAnswer != null)
			{
				newPasswordAnswer = newPasswordAnswer.Trim();
			}
			SecUtility.CheckParameter(ref newPasswordAnswer, this.RequiresQuestionAndAnswer, this.RequiresQuestionAndAnswer, false, 128, "newPasswordAnswer");
			string objValue;
			if (!string.IsNullOrEmpty(newPasswordAnswer))
			{
				objValue = this.EncodePassword(newPasswordAnswer.ToLower(CultureInfo.InvariantCulture), passwordFormat, salt);
			}
			else
			{
				objValue = newPasswordAnswer;
			}
			SecUtility.CheckParameter(ref objValue, this.RequiresQuestionAndAnswer, this.RequiresQuestionAndAnswer, false, 128, "newPasswordAnswer");
			bool result;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Membership_ChangePasswordQuestionAndAnswer", sqlConnectionHolder.Connection);
					sqlCommand.CommandTimeout = this.CommandTimeout;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.Parameters.Add(this.CreateInputParam("@UserName", SqlDbType.NVarChar, username));
					sqlCommand.Parameters.Add(this.CreateInputParam("@NewPasswordQuestion", SqlDbType.NVarChar, newPasswordQuestion));
					sqlCommand.Parameters.Add(this.CreateInputParam("@NewPasswordAnswer", SqlDbType.NVarChar, objValue));
					SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlCommand.ExecuteNonQuery();
					int num = (sqlParameter.Value != null) ? ((int)sqlParameter.Value) : -1;
					if (num != 0)
					{
						throw new ProviderException(this.GetExceptionText(num));
					}
					result = (num == 0);
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
		public override string GetPassword(string username, string passwordAnswer)
		{
			if (!this.EnablePasswordRetrieval)
			{
				throw new NotSupportedException(SR.GetString("This Membership Provider has not been configured to support password retrieval."));
			}
			SecUtility.CheckParameter(ref username, true, true, true, 256, "username");
			string encodedPasswordAnswer = this.GetEncodedPasswordAnswer(username, passwordAnswer);
			SecUtility.CheckParameter(ref encodedPasswordAnswer, this.RequiresQuestionAndAnswer, this.RequiresQuestionAndAnswer, false, 128, "passwordAnswer");
			int passwordFormat = 0;
			int status = 0;
			string passwordFromDB = this.GetPasswordFromDB(username, encodedPasswordAnswer, this.RequiresQuestionAndAnswer, out passwordFormat, out status);
			if (passwordFromDB != null)
			{
				return this.UnEncodePassword(passwordFromDB, passwordFormat);
			}
			string exceptionText = this.GetExceptionText(status);
			if (this.IsStatusDueToBadPassword(status))
			{
				throw new MembershipPasswordException(exceptionText);
			}
			throw new ProviderException(exceptionText);
		}
		public override bool ChangePassword(string username, string oldPassword, string newPassword)
		{
			SecUtility.CheckParameter(ref username, true, true, true, 256, "username");
			SecUtility.CheckParameter(ref oldPassword, true, true, false, 128, "oldPassword");
			SecUtility.CheckParameter(ref newPassword, true, true, false, 128, "newPassword");
			string text = null;
			int num;
			if (!this.CheckPassword(username, oldPassword, false, false, out text, out num))
			{
				return false;
			}
			if (newPassword.Length < this.MinRequiredPasswordLength)
			{
				throw new ArgumentException(SR.GetString("The length of parameter '{0}' needs to be greater or equal to '{1}'.", "newPassword", this.MinRequiredPasswordLength.ToString(CultureInfo.InvariantCulture)));
			}
			int num2 = 0;
			for (int i = 0; i < newPassword.Length; i++)
			{
				if (!char.IsLetterOrDigit(newPassword, i))
				{
					num2++;
				}
			}
			if (num2 < this.MinRequiredNonAlphanumericCharacters)
			{
				throw new ArgumentException(SR.GetString("Non alpha numeric characters in '{0}' needs to be greater than or equal to '{1}'.", "newPassword", this.MinRequiredNonAlphanumericCharacters.ToString(CultureInfo.InvariantCulture)));
			}
			if (this.PasswordStrengthRegularExpression.Length > 0 && !Regex.IsMatch(newPassword, this.PasswordStrengthRegularExpression))
			{
				throw new ArgumentException(SR.GetString("The parameter '{0}' does not match the regular expression specified in config file.", "newPassword"));
			}
			string text2 = this.EncodePassword(newPassword, num, text);
			if (text2.Length > 128)
			{
				throw new ArgumentException(SR.GetString("The password is too long: it must not exceed 128 chars after encrypting."), "newPassword");
			}
			ValidatePasswordEventArgs validatePasswordEventArgs = new ValidatePasswordEventArgs(username, newPassword, false);
			this.OnValidatingPassword(validatePasswordEventArgs);
			if (!validatePasswordEventArgs.Cancel)
			{
				bool result;
				try
				{
					SqlConnectionHolder sqlConnectionHolder = null;
					try
					{
						sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
						SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Membership_SetPassword", sqlConnectionHolder.Connection);
						sqlCommand.CommandTimeout = this.CommandTimeout;
						sqlCommand.CommandType = CommandType.StoredProcedure;
						sqlCommand.Parameters.Add(this.CreateInputParam("@UserName", SqlDbType.NVarChar, username));
						sqlCommand.Parameters.Add(this.CreateInputParam("@NewPassword", SqlDbType.NVarChar, text2));
						sqlCommand.Parameters.Add(this.CreateInputParam("@PasswordSalt", SqlDbType.NVarChar, text));
						sqlCommand.Parameters.Add(this.CreateInputParam("@PasswordFormat", SqlDbType.Int, num));
						sqlCommand.Parameters.Add(this.CreateInputParam("@CurrentTime", SqlDbType.DateTime, DateTime.Now));
						SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
						sqlParameter.Direction = ParameterDirection.ReturnValue;
						sqlCommand.Parameters.Add(sqlParameter);
						sqlCommand.ExecuteNonQuery();
						int num3 = (sqlParameter.Value != null) ? ((int)sqlParameter.Value) : -1;
						if (num3 != 0)
						{
							string exceptionText = this.GetExceptionText(num3);
							if (this.IsStatusDueToBadPassword(num3))
							{
								throw new MembershipPasswordException(exceptionText);
							}
							throw new ProviderException(exceptionText);
						}
						else
						{
							result = true;
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
			if (validatePasswordEventArgs.FailureInformation != null)
			{
				throw validatePasswordEventArgs.FailureInformation;
			}
			throw new ArgumentException(SR.GetString("The custom password validation failed."), "newPassword");
		}
		public override string ResetPassword(string username, string passwordAnswer)
		{
			if (!this.EnablePasswordReset)
			{
				throw new NotSupportedException(SR.GetString("This provider is not configured to allow password resets. To enable password reset, set enablePasswordReset to \"true\" in the configuration file."));
			}
			SecUtility.CheckParameter(ref username, true, true, true, 256, "username");
			int num;
			string text;
			int num2;
			string text2;
			int num3;
			int num4;
			bool flag;
			DateTime dateTime;
			DateTime dateTime2;
			this.GetPasswordWithFormat(username, false, out num, out text, out num2, out text2, out num3, out num4, out flag, out dateTime, out dateTime2);
			if (num != 0)
			{
				if (this.IsStatusDueToBadPassword(num))
				{
					throw new MembershipPasswordException(this.GetExceptionText(num));
				}
				throw new ProviderException(this.GetExceptionText(num));
			}
			else
			{
				if (passwordAnswer != null)
				{
					passwordAnswer = passwordAnswer.Trim();
				}
				string objValue;
				if (!string.IsNullOrEmpty(passwordAnswer))
				{
					objValue = this.EncodePassword(passwordAnswer.ToLower(CultureInfo.InvariantCulture), num2, text2);
				}
				else
				{
					objValue = passwordAnswer;
				}
				SecUtility.CheckParameter(ref objValue, this.RequiresQuestionAndAnswer, this.RequiresQuestionAndAnswer, false, 128, "passwordAnswer");
				string text3 = this.GeneratePassword();
				ValidatePasswordEventArgs validatePasswordEventArgs = new ValidatePasswordEventArgs(username, text3, false);
				this.OnValidatingPassword(validatePasswordEventArgs);
				if (!validatePasswordEventArgs.Cancel)
				{
					string result;
					try
					{
						SqlConnectionHolder sqlConnectionHolder = null;
						try
						{
							sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
							SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Membership_ResetPassword", sqlConnectionHolder.Connection);
							sqlCommand.CommandTimeout = this.CommandTimeout;
							sqlCommand.CommandType = CommandType.StoredProcedure;
							sqlCommand.Parameters.Add(this.CreateInputParam("@UserName", SqlDbType.NVarChar, username));
							sqlCommand.Parameters.Add(this.CreateInputParam("@NewPassword", SqlDbType.NVarChar, this.EncodePassword(text3, num2, text2)));
							sqlCommand.Parameters.Add(this.CreateInputParam("@MaxInvalidPasswordAttempts", SqlDbType.Int, this.MaxInvalidPasswordAttempts));
							sqlCommand.Parameters.Add(this.CreateInputParam("@PasswordAttemptWindow", SqlDbType.Int, this.PasswordAttemptWindow));
							sqlCommand.Parameters.Add(this.CreateInputParam("@PasswordSalt", SqlDbType.NVarChar, text2));
							sqlCommand.Parameters.Add(this.CreateInputParam("@PasswordFormat", SqlDbType.Int, num2));
							sqlCommand.Parameters.Add(this.CreateInputParam("@CurrentTime", SqlDbType.DateTime, DateTime.Now));
							if (this.RequiresQuestionAndAnswer)
							{
								sqlCommand.Parameters.Add(this.CreateInputParam("@PasswordAnswer", SqlDbType.NVarChar, objValue));
							}
							SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
							sqlParameter.Direction = ParameterDirection.ReturnValue;
							sqlCommand.Parameters.Add(sqlParameter);
							sqlCommand.ExecuteNonQuery();
							num = ((sqlParameter.Value != null) ? ((int)sqlParameter.Value) : -1);
							if (num != 0)
							{
								string exceptionText = this.GetExceptionText(num);
								if (this.IsStatusDueToBadPassword(num))
								{
									throw new MembershipPasswordException(exceptionText);
								}
								throw new ProviderException(exceptionText);
							}
							else
							{
								result = text3;
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
				if (validatePasswordEventArgs.FailureInformation != null)
				{
					throw validatePasswordEventArgs.FailureInformation;
				}
				throw new ProviderException(SR.GetString("The custom password validation failed."));
			}
		}
		public override void UpdateUser(MembershipUser user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			string email = user.UserName;
			SecUtility.CheckParameter(ref email, true, true, true, 256, "UserName");
			email = user.Email;
			SecUtility.CheckParameter(ref email, this.RequiresUniqueEmail, this.RequiresUniqueEmail, false, 256, "Email");
			user.Email = email;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Membership_UpdateUser", sqlConnectionHolder.Connection);
					sqlCommand.CommandTimeout = this.CommandTimeout;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.Parameters.Add(this.CreateInputParam("@UserName", SqlDbType.NVarChar, user.UserName));
					sqlCommand.Parameters.Add(this.CreateInputParam("@Email", SqlDbType.NVarChar, user.Email));
					sqlCommand.Parameters.Add(this.CreateInputParam("@Comment", SqlDbType.NText, user.Comment));
					sqlCommand.Parameters.Add(this.CreateInputParam("@IsApproved", SqlDbType.Bit, user.IsApproved ? 1 : 0));
					sqlCommand.Parameters.Add(this.CreateInputParam("@LastLoginDate", SqlDbType.DateTime, user.LastLoginDate));
					sqlCommand.Parameters.Add(this.CreateInputParam("@LastActivityDate", SqlDbType.DateTime, user.LastActivityDate));
					sqlCommand.Parameters.Add(this.CreateInputParam("@UniqueEmail", SqlDbType.Int, this.RequiresUniqueEmail ? 1 : 0));
					sqlCommand.Parameters.Add(this.CreateInputParam("@CurrentTime", SqlDbType.DateTime, DateTime.Now));
					SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlCommand.ExecuteNonQuery();
					int num = (sqlParameter.Value != null) ? ((int)sqlParameter.Value) : -1;
					if (num != 0)
					{
						throw new ProviderException(this.GetExceptionText(num));
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
		public override bool ValidateUser(string username, string password)
		{
			return SecUtility.ValidateParameter(ref username, true, true, true, 256) && SecUtility.ValidateParameter(ref password, true, true, false, 128) && this.CheckPassword(username, password, true, true);
		}
		public override bool UnlockUser(string username)
		{
			SecUtility.CheckParameter(ref username, true, true, true, 256, "username");
			bool result;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Membership_UnlockUser", sqlConnectionHolder.Connection);
					sqlCommand.CommandTimeout = this.CommandTimeout;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.Parameters.Add(this.CreateInputParam("@UserName", SqlDbType.NVarChar, username));
					SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlCommand.ExecuteNonQuery();
					if (((sqlParameter.Value != null) ? ((int)sqlParameter.Value) : -1) == 0)
					{
						result = true;
					}
					else
					{
						result = false;
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
		public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
		{
			if (providerUserKey == null)
			{
				throw new ArgumentNullException("providerUserKey");
			}
			SqlDataReader sqlDataReader = null;
			MembershipUser result;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Membership_GetUserByUserId", sqlConnectionHolder.Connection);
					sqlCommand.CommandTimeout = this.CommandTimeout;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.Parameters.Add(this.CreateInputParam("@UserId", SqlDbType.Int, providerUserKey));
					sqlCommand.Parameters.Add(this.CreateInputParam("@UpdateLastActivity", SqlDbType.Bit, userIsOnline));
					sqlCommand.Parameters.Add(this.CreateInputParam("@CurrentTime", SqlDbType.DateTime, DateTime.Now));
					SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlDataReader = sqlCommand.ExecuteReader();
					if (sqlDataReader.Read())
					{
						string nullableString = this.GetNullableString(sqlDataReader, 0);
						string nullableString2 = this.GetNullableString(sqlDataReader, 1);
						string nullableString3 = this.GetNullableString(sqlDataReader, 2);
						bool boolean = sqlDataReader.GetBoolean(3);
						DateTime dateTime = sqlDataReader.GetDateTime(4);
						DateTime dateTime2 = sqlDataReader.GetDateTime(5);
						DateTime dateTime3 = sqlDataReader.GetDateTime(6);
						DateTime dateTime4 = sqlDataReader.GetDateTime(7);
						string nullableString4 = this.GetNullableString(sqlDataReader, 8);
						bool boolean2 = sqlDataReader.GetBoolean(9);
						DateTime dateTime5 = sqlDataReader.GetDateTime(10);
						result = new MembershipUser(this.Name, nullableString4, providerUserKey, nullableString, nullableString2, nullableString3, boolean, boolean2, dateTime, dateTime2, dateTime3, dateTime4, dateTime5);
					}
					else
					{
						result = null;
					}
				}
				finally
				{
					if (sqlDataReader != null)
					{
						sqlDataReader.Close();
						sqlDataReader = null;
					}
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
		public override MembershipUser GetUser(string username, bool userIsOnline)
		{
			SecUtility.CheckParameter(ref username, true, false, true, 256, "username");
			SqlDataReader sqlDataReader = null;
			MembershipUser result;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Membership_GetUserByName", sqlConnectionHolder.Connection);
					sqlCommand.CommandTimeout = this.CommandTimeout;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.Parameters.Add(this.CreateInputParam("@UserName", SqlDbType.NVarChar, username));
					sqlCommand.Parameters.Add(this.CreateInputParam("@UpdateLastActivity", SqlDbType.Bit, userIsOnline));
					sqlCommand.Parameters.Add(this.CreateInputParam("@CurrentTime", SqlDbType.DateTime, DateTime.Now));
					SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlDataReader = sqlCommand.ExecuteReader();
					if (sqlDataReader.Read())
					{
						string nullableString = this.GetNullableString(sqlDataReader, 0);
						string nullableString2 = this.GetNullableString(sqlDataReader, 1);
						string nullableString3 = this.GetNullableString(sqlDataReader, 2);
						bool boolean = sqlDataReader.GetBoolean(3);
						DateTime dateTime = sqlDataReader.GetDateTime(4);
						DateTime dateTime2 = sqlDataReader.GetDateTime(5);
						DateTime dateTime3 = sqlDataReader.GetDateTime(6);
						DateTime dateTime4 = sqlDataReader.GetDateTime(7);
						int @int = sqlDataReader.GetInt32(8);
						bool boolean2 = sqlDataReader.GetBoolean(9);
						DateTime dateTime5 = sqlDataReader.GetDateTime(10);
						result = new MembershipUser(this.Name, username, @int, nullableString, nullableString2, nullableString3, boolean, boolean2, dateTime, dateTime2, dateTime3, dateTime4, dateTime5);
					}
					else
					{
						result = null;
					}
				}
				finally
				{
					if (sqlDataReader != null)
					{
						sqlDataReader.Close();
						sqlDataReader = null;
					}
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
		public override string GetUserNameByEmail(string email)
		{
			SecUtility.CheckParameter(ref email, false, false, false, 256, "email");
			string result;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Membership_GetUserByEmail", sqlConnectionHolder.Connection);
					string text = null;
					SqlDataReader sqlDataReader = null;
					sqlCommand.CommandTimeout = this.CommandTimeout;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.Parameters.Add(this.CreateInputParam("@Email", SqlDbType.NVarChar, email));
					SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(sqlParameter);
					try
					{
						sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.SequentialAccess);
						if (sqlDataReader.Read())
						{
							text = this.GetNullableString(sqlDataReader, 0);
							if (this.RequiresUniqueEmail && sqlDataReader.Read())
							{
								throw new ProviderException(SR.GetString("More than one user has the specified e-mail address."));
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
					result = text;
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
		public override bool DeleteUser(string username, bool deleteAllRelatedData)
		{
			SecUtility.CheckParameter(ref username, true, true, true, 256, "username");
			bool result;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					SqlCommand sqlCommand = new SqlCommand("aspnet_Membership_DeleteUser", sqlConnectionHolder.Connection);
					sqlCommand.CommandTimeout = this.CommandTimeout;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.Parameters.Add(this.CreateInputParam("@UserName", SqlDbType.NVarChar, username));
					SqlParameter sqlParameter = new SqlParameter("@NumTablesDeletedFrom", SqlDbType.Int);
					sqlParameter.Direction = ParameterDirection.Output;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlCommand.ExecuteNonQuery();
					int num = (sqlParameter.Value != null) ? ((int)sqlParameter.Value) : -1;
					result = (num > 0);
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
		public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
		{
			if (pageIndex < 0)
			{
				throw new ArgumentException(SR.GetString("The pageIndex must be greater than or equal to zero."), "pageIndex");
			}
			if (pageSize < 1)
			{
				throw new ArgumentException(SR.GetString("The pageSize must be greater than zero."), "pageSize");
			}
			long num = (long)pageIndex * (long)pageSize + (long)pageSize - 1L;
			if (num > 2147483647L)
			{
				throw new ArgumentException(SR.GetString("The combination of pageIndex and pageSize cannot exceed the maximum value of System.Int32."), "pageIndex and pageSize");
			}
			MembershipUserCollection membershipUserCollection = new MembershipUserCollection();
			totalRecords = 0;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Membership_GetAllUsers", sqlConnectionHolder.Connection);
					SqlDataReader sqlDataReader = null;
					SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
					sqlCommand.CommandTimeout = this.CommandTimeout;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.Parameters.Add(this.CreateInputParam("@PageIndex", SqlDbType.Int, pageIndex));
					sqlCommand.Parameters.Add(this.CreateInputParam("@PageSize", SqlDbType.Int, pageSize));
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(sqlParameter);
					try
					{
						sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.SequentialAccess);
						while (sqlDataReader.Read())
						{
							string nullableString = this.GetNullableString(sqlDataReader, 0);
							string nullableString2 = this.GetNullableString(sqlDataReader, 1);
							string nullableString3 = this.GetNullableString(sqlDataReader, 2);
							string nullableString4 = this.GetNullableString(sqlDataReader, 3);
							bool boolean = sqlDataReader.GetBoolean(4);
							DateTime dateTime = sqlDataReader.GetDateTime(5);
							DateTime dateTime2 = sqlDataReader.GetDateTime(6);
							DateTime dateTime3 = sqlDataReader.GetDateTime(7);
							DateTime dateTime4 = sqlDataReader.GetDateTime(8);
							int @int = sqlDataReader.GetInt32(9);
							bool boolean2 = sqlDataReader.GetBoolean(10);
							DateTime dateTime5 = sqlDataReader.GetDateTime(11);
							membershipUserCollection.Add(new MembershipUser(this.Name, nullableString, @int, nullableString2, nullableString3, nullableString4, boolean, boolean2, dateTime, dateTime2, dateTime3, dateTime4, dateTime5));
						}
					}
					finally
					{
						if (sqlDataReader != null)
						{
							sqlDataReader.Close();
						}
						if (sqlParameter.Value != null && sqlParameter.Value is int)
						{
							totalRecords = (int)sqlParameter.Value;
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
			return membershipUserCollection;
		}
		public override int GetNumberOfUsersOnline()
		{
			int result;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Membership_GetNumberOfUsersOnline", sqlConnectionHolder.Connection);
					SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
					sqlCommand.CommandTimeout = this.CommandTimeout;
					sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.Add(this.CreateInputParam("@MinutesSinceLastInActive", SqlDbType.Int, System.Web.Security.Membership.UserIsOnlineTimeWindow));
					sqlCommand.Parameters.Add(this.CreateInputParam("@CurrentTime", SqlDbType.DateTime, DateTime.Now));
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlCommand.ExecuteNonQuery();
					int num = (sqlParameter.Value != null) ? ((int)sqlParameter.Value) : -1;
					result = num;
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
		public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			SecUtility.CheckParameter(ref usernameToMatch, true, true, false, 256, "usernameToMatch");
			if (pageIndex < 0)
			{
				throw new ArgumentException(SR.GetString("The pageIndex must be greater than or equal to zero."), "pageIndex");
			}
			if (pageSize < 1)
			{
				throw new ArgumentException(SR.GetString("The pageSize must be greater than zero."), "pageSize");
			}
			long num = (long)pageIndex * (long)pageSize + (long)pageSize - 1L;
			if (num > 2147483647L)
			{
				throw new ArgumentException(SR.GetString("The combination of pageIndex and pageSize cannot exceed the maximum value of System.Int32."), "pageIndex and pageSize");
			}
			MembershipUserCollection result;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				totalRecords = 0;
				SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
				sqlParameter.Direction = ParameterDirection.ReturnValue;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Membership_FindUsersByName", sqlConnectionHolder.Connection);
					MembershipUserCollection membershipUserCollection = new MembershipUserCollection();
					SqlDataReader sqlDataReader = null;
					sqlCommand.CommandTimeout = this.CommandTimeout;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.Parameters.Add(this.CreateInputParam("@UserNameToMatch", SqlDbType.NVarChar, usernameToMatch));
					sqlCommand.Parameters.Add(this.CreateInputParam("@PageIndex", SqlDbType.Int, pageIndex));
					sqlCommand.Parameters.Add(this.CreateInputParam("@PageSize", SqlDbType.Int, pageSize));
					sqlCommand.Parameters.Add(sqlParameter);
					try
					{
						sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.SequentialAccess);
						while (sqlDataReader.Read())
						{
							string nullableString = this.GetNullableString(sqlDataReader, 0);
							string nullableString2 = this.GetNullableString(sqlDataReader, 1);
							string nullableString3 = this.GetNullableString(sqlDataReader, 2);
							string nullableString4 = this.GetNullableString(sqlDataReader, 3);
							bool boolean = sqlDataReader.GetBoolean(4);
							DateTime dateTime = sqlDataReader.GetDateTime(5);
							DateTime dateTime2 = sqlDataReader.GetDateTime(6);
							DateTime dateTime3 = sqlDataReader.GetDateTime(7);
							DateTime dateTime4 = sqlDataReader.GetDateTime(8);
							int @int = sqlDataReader.GetInt32(9);
							bool boolean2 = sqlDataReader.GetBoolean(10);
							DateTime dateTime5 = sqlDataReader.GetDateTime(11);
							membershipUserCollection.Add(new MembershipUser(this.Name, nullableString, @int, nullableString2, nullableString3, nullableString4, boolean, boolean2, dateTime, dateTime2, dateTime3, dateTime4, dateTime5));
						}
						result = membershipUserCollection;
					}
					finally
					{
						if (sqlDataReader != null)
						{
							sqlDataReader.Close();
						}
						if (sqlParameter.Value != null && sqlParameter.Value is int)
						{
							totalRecords = (int)sqlParameter.Value;
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
		public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			SecUtility.CheckParameter(ref emailToMatch, false, false, false, 256, "emailToMatch");
			if (pageIndex < 0)
			{
				throw new ArgumentException(SR.GetString("The pageIndex must be greater than or equal to zero."), "pageIndex");
			}
			if (pageSize < 1)
			{
				throw new ArgumentException(SR.GetString("The pageSize must be greater than zero."), "pageSize");
			}
			long num = (long)pageIndex * (long)pageSize + (long)pageSize - 1L;
			if (num > 2147483647L)
			{
				throw new ArgumentException(SR.GetString("The combination of pageIndex and pageSize cannot exceed the maximum value of System.Int32."), "pageIndex and pageSize");
			}
			MembershipUserCollection result;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				totalRecords = 0;
				SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
				sqlParameter.Direction = ParameterDirection.ReturnValue;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Membership_FindUsersByEmail", sqlConnectionHolder.Connection);
					MembershipUserCollection membershipUserCollection = new MembershipUserCollection();
					SqlDataReader sqlDataReader = null;
					sqlCommand.CommandTimeout = this.CommandTimeout;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.Parameters.Add(this.CreateInputParam("@EmailToMatch", SqlDbType.NVarChar, emailToMatch));
					sqlCommand.Parameters.Add(this.CreateInputParam("@PageIndex", SqlDbType.Int, pageIndex));
					sqlCommand.Parameters.Add(this.CreateInputParam("@PageSize", SqlDbType.Int, pageSize));
					sqlCommand.Parameters.Add(sqlParameter);
					try
					{
						sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.SequentialAccess);
						while (sqlDataReader.Read())
						{
							string nullableString = this.GetNullableString(sqlDataReader, 0);
							string nullableString2 = this.GetNullableString(sqlDataReader, 1);
							string nullableString3 = this.GetNullableString(sqlDataReader, 2);
							string nullableString4 = this.GetNullableString(sqlDataReader, 3);
							bool boolean = sqlDataReader.GetBoolean(4);
							DateTime dateTime = sqlDataReader.GetDateTime(5);
							DateTime dateTime2 = sqlDataReader.GetDateTime(6);
							DateTime dateTime3 = sqlDataReader.GetDateTime(7);
							DateTime dateTime4 = sqlDataReader.GetDateTime(8);
							int @int = sqlDataReader.GetInt32(9);
							bool boolean2 = sqlDataReader.GetBoolean(10);
							DateTime dateTime5 = sqlDataReader.GetDateTime(11);
							membershipUserCollection.Add(new MembershipUser(this.Name, nullableString, @int, nullableString2, nullableString3, nullableString4, boolean, boolean2, dateTime, dateTime2, dateTime3, dateTime4, dateTime5));
						}
						result = membershipUserCollection;
					}
					finally
					{
						if (sqlDataReader != null)
						{
							sqlDataReader.Close();
						}
						if (sqlParameter.Value != null && sqlParameter.Value is int)
						{
							totalRecords = (int)sqlParameter.Value;
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
		private bool CheckPassword(string username, string password, bool updateLastLoginActivityDate, bool failIfNotApproved)
		{
			string text;
			int num;
			return this.CheckPassword(username, password, updateLastLoginActivityDate, failIfNotApproved, out text, out num);
		}
		private bool CheckPassword(string username, string password, bool updateLastLoginActivityDate, bool failIfNotApproved, out string salt, out int passwordFormat)
		{
			SqlConnectionHolder sqlConnectionHolder = null;
			int num;
			string text;
			int num2;
			int num3;
			bool flag;
			DateTime dateTime;
			DateTime dateTime2;
			this.GetPasswordWithFormat(username, updateLastLoginActivityDate, out num, out text, out passwordFormat, out salt, out num2, out num3, out flag, out dateTime, out dateTime2);
			if (num != 0)
			{
				return false;
			}
			if (!flag && failIfNotApproved)
			{
				return false;
			}
			string value = this.EncodePassword(password, passwordFormat, salt);
			bool flag2;
			if ((flag2 = text.Equals(value)) && num2 == 0 && num3 == 0)
			{
				return true;
			}
			try
			{
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Membership_UpdateUserInfo", sqlConnectionHolder.Connection);
					DateTime now = DateTime.Now;
					sqlCommand.CommandTimeout = this.CommandTimeout;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.Parameters.Add(this.CreateInputParam("@UserName", SqlDbType.NVarChar, username));
					sqlCommand.Parameters.Add(this.CreateInputParam("@IsPasswordCorrect", SqlDbType.Bit, flag2));
					sqlCommand.Parameters.Add(this.CreateInputParam("@UpdateLastLoginActivityDate", SqlDbType.Bit, updateLastLoginActivityDate));
					sqlCommand.Parameters.Add(this.CreateInputParam("@MaxInvalidPasswordAttempts", SqlDbType.Int, this.MaxInvalidPasswordAttempts));
					sqlCommand.Parameters.Add(this.CreateInputParam("@PasswordAttemptWindow", SqlDbType.Int, this.PasswordAttemptWindow));
					sqlCommand.Parameters.Add(this.CreateInputParam("@CurrentTime", SqlDbType.DateTime, now));
					sqlCommand.Parameters.Add(this.CreateInputParam("@LastLoginDate", SqlDbType.DateTime, flag2 ? now : dateTime));
					sqlCommand.Parameters.Add(this.CreateInputParam("@LastActivityDate", SqlDbType.DateTime, flag2 ? now : dateTime2));
					SqlParameter sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlCommand.ExecuteNonQuery();
					num = ((sqlParameter.Value != null) ? ((int)sqlParameter.Value) : -1);
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
			return flag2;
		}
		private void GetPasswordWithFormat(string username, bool updateLastLoginActivityDate, out int status, out string password, out int passwordFormat, out string passwordSalt, out int failedPasswordAttemptCount, out int failedPasswordAnswerAttemptCount, out bool isApproved, out DateTime lastLoginDate, out DateTime lastActivityDate)
		{
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				SqlDataReader sqlDataReader = null;
				SqlParameter sqlParameter = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Membership_GetPasswordWithFormat", sqlConnectionHolder.Connection);
					sqlCommand.CommandTimeout = this.CommandTimeout;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.Parameters.Add(this.CreateInputParam("@UserName", SqlDbType.NVarChar, username));
					sqlCommand.Parameters.Add(this.CreateInputParam("@UpdateLastLoginActivityDate", SqlDbType.Bit, updateLastLoginActivityDate));
					sqlCommand.Parameters.Add(this.CreateInputParam("@CurrentTime", SqlDbType.DateTime, DateTime.Now));
					sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.SingleRow);
					status = -1;
					if (sqlDataReader.Read())
					{
						password = sqlDataReader.GetString(0);
						passwordFormat = sqlDataReader.GetInt32(1);
						passwordSalt = sqlDataReader.GetString(2);
						failedPasswordAttemptCount = sqlDataReader.GetInt32(3);
						failedPasswordAnswerAttemptCount = sqlDataReader.GetInt32(4);
						isApproved = sqlDataReader.GetBoolean(5);
						lastLoginDate = sqlDataReader.GetDateTime(6);
						lastActivityDate = sqlDataReader.GetDateTime(7);
					}
					else
					{
						password = null;
						passwordFormat = 0;
						passwordSalt = null;
						failedPasswordAttemptCount = 0;
						failedPasswordAnswerAttemptCount = 0;
						isApproved = false;
						lastLoginDate = DateTime.Now;
						lastActivityDate = DateTime.Now;
					}
				}
				finally
				{
					if (sqlDataReader != null)
					{
						sqlDataReader.Close();
						sqlDataReader = null;
						status = ((sqlParameter.Value != null) ? ((int)sqlParameter.Value) : -1);
					}
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
		private string GetPasswordFromDB(string username, string passwordAnswer, bool requiresQuestionAndAnswer, out int passwordFormat, out int status)
		{
			string result;
			try
			{
				SqlConnectionHolder sqlConnectionHolder = null;
				SqlDataReader sqlDataReader = null;
				SqlParameter sqlParameter = null;
				try
				{
					sqlConnectionHolder = SqlConnectionHelper.GetConnection(this._sqlConnectionString, true);
					SqlCommand sqlCommand = new SqlCommand("dbo.aspnet_Membership_GetPassword", sqlConnectionHolder.Connection);
					sqlCommand.CommandTimeout = this.CommandTimeout;
					sqlCommand.CommandType = CommandType.StoredProcedure;
					sqlCommand.Parameters.Add(this.CreateInputParam("@UserName", SqlDbType.NVarChar, username));
					sqlCommand.Parameters.Add(this.CreateInputParam("@MaxInvalidPasswordAttempts", SqlDbType.Int, this.MaxInvalidPasswordAttempts));
					sqlCommand.Parameters.Add(this.CreateInputParam("@PasswordAttemptWindow", SqlDbType.Int, this.PasswordAttemptWindow));
					sqlCommand.Parameters.Add(this.CreateInputParam("@CurrentTime", SqlDbType.DateTime, DateTime.Now));
					if (requiresQuestionAndAnswer)
					{
						sqlCommand.Parameters.Add(this.CreateInputParam("@PasswordAnswer", SqlDbType.NVarChar, passwordAnswer));
					}
					sqlParameter = new SqlParameter("@ReturnValue", SqlDbType.Int);
					sqlParameter.Direction = ParameterDirection.ReturnValue;
					sqlCommand.Parameters.Add(sqlParameter);
					sqlDataReader = sqlCommand.ExecuteReader(CommandBehavior.SingleRow);
					status = -1;
					string text;
					if (sqlDataReader.Read())
					{
						text = sqlDataReader.GetString(0);
						passwordFormat = sqlDataReader.GetInt32(1);
					}
					else
					{
						text = null;
						passwordFormat = 0;
					}
					result = text;
				}
				finally
				{
					if (sqlDataReader != null)
					{
						sqlDataReader.Close();
						sqlDataReader = null;
						status = ((sqlParameter.Value != null) ? ((int)sqlParameter.Value) : -1);
					}
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
		private string GetEncodedPasswordAnswer(string username, string passwordAnswer)
		{
			if (passwordAnswer != null)
			{
				passwordAnswer = passwordAnswer.Trim();
			}
			if (string.IsNullOrEmpty(passwordAnswer))
			{
				return passwordAnswer;
			}
			int num;
			string text;
			int passwordFormat;
			string salt;
			int num2;
			int num3;
			bool flag;
			DateTime dateTime;
			DateTime dateTime2;
			this.GetPasswordWithFormat(username, false, out num, out text, out passwordFormat, out salt, out num2, out num3, out flag, out dateTime, out dateTime2);
			if (num != 0)
			{
				throw new ProviderException(this.GetExceptionText(num));
			}
			return this.EncodePassword(passwordAnswer.ToLower(CultureInfo.InvariantCulture), passwordFormat, salt);
		}
		public virtual string GeneratePassword()
		{
			return System.Web.Security.Membership.GeneratePassword((this.MinRequiredPasswordLength < 14) ? 14 : this.MinRequiredPasswordLength, this.MinRequiredNonAlphanumericCharacters);
		}
		private SqlParameter CreateInputParam(string paramName, SqlDbType dbType, object objValue)
		{
			SqlParameter sqlParameter = new SqlParameter(paramName, dbType);
			if (objValue == null)
			{
				sqlParameter.IsNullable = true;
				sqlParameter.Value = DBNull.Value;
			}
			else
			{
				sqlParameter.Value = objValue;
			}
			return sqlParameter;
		}
		private string GetNullableString(SqlDataReader reader, int int_0)
		{
			if (!reader.IsDBNull(int_0))
			{
				return reader.GetString(int_0);
			}
			return null;
		}
		private string GetExceptionText(int status)
		{
			string strString;
			switch (status)
			{
			case 0:
				return string.Empty;
			case 1:
				strString = "The user was not found.";
				break;
			case 2:
				strString = "The password supplied is wrong.";
				break;
			case 3:
				strString = "The password-answer supplied is wrong.";
				break;
			case 4:
				strString = "The password supplied is invalid.  Passwords must conform to the password strength requirements configured for the default provider.";
				break;
			case 5:
				strString = "The password-question supplied is invalid.  Note that the current provider configuration requires a valid password question and answer.  As a result, a CreateUser overload that accepts question and answer parameters must also be used.";
				break;
			case 6:
				strString = "The password-answer supplied is invalid.";
				break;
			case 7:
				strString = "The E-mail supplied is invalid.";
				break;
			default:
				if (status != 99)
				{
					strString = "The Provider encountered an unknown error.";
				}
				else
				{
					strString = "The user account has been locked out.";
				}
				break;
			}
			return SR.GetString(strString);
		}
		private bool IsStatusDueToBadPassword(int status)
		{
			return (status >= 2 && status <= 6) || status == 99;
		}
		private DateTime RoundToSeconds(DateTime dateTime_0)
		{
			return new DateTime(dateTime_0.Year, dateTime_0.Month, dateTime_0.Day, dateTime_0.Hour, dateTime_0.Minute, dateTime_0.Second);
		}
		internal string GenerateSalt()
		{
			byte[] array = new byte[16];
			new RNGCryptoServiceProvider().GetBytes(array);
			return Convert.ToBase64String(array);
		}
		internal string EncodePassword(string pass, int passwordFormat, string salt)
		{
			if (passwordFormat == 0)
			{
				return pass;
			}
			byte[] bytes = Encoding.Unicode.GetBytes(pass);
			byte[] array = Convert.FromBase64String(salt);
			byte[] array2 = new byte[array.Length + bytes.Length];
			Buffer.BlockCopy(array, 0, array2, 0, array.Length);
			Buffer.BlockCopy(bytes, 0, array2, array.Length, bytes.Length);
			byte[] inArray;
			if (passwordFormat == 1)
			{
                HashAlgorithm hashAlgorithm = HashAlgorithm.Create(System.Web.Security.Membership.HashAlgorithmType);
				inArray = hashAlgorithm.ComputeHash(array2);
			}
			else
			{
				inArray = this.EncryptPassword(array2);
			}
			return Convert.ToBase64String(inArray);
		}
		internal string UnEncodePassword(string pass, int passwordFormat)
		{
			switch (passwordFormat)
			{
			case 0:
				return pass;
			case 1:
				throw new ProviderException(SR.GetString("Hashed passwords cannot be decoded."));
			default:
			{
				byte[] encodedPassword = Convert.FromBase64String(pass);
				byte[] array = this.DecryptPassword(encodedPassword);
				if (array == null)
				{
					return null;
				}
				return Encoding.Unicode.GetString(array, 16, array.Length - 16);
			}
			}
		}
	}
}
