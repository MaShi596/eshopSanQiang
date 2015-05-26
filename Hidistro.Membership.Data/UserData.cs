using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Web.Security;
namespace Hidistro.Membership.Data
{
	public class UserData : MemberUserProvider
	{
		private Database database;
		public UserData()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public override CreateUserStatus CreateMembershipUser(HiMembershipUser userToCreate, string passwordQuestion, string passwordAnswer)
		{
			CreateUserStatus createUserStatus = CreateUserStatus.UnknownFailure;
			CreateUserStatus result;
			if (userToCreate == null)
			{
				result = CreateUserStatus.UnknownFailure;
			}
			else
			{
				bool flag = false;
				if (!string.IsNullOrEmpty(passwordQuestion) && !string.IsNullOrEmpty(passwordAnswer))
				{
					flag = true;
					if (passwordAnswer.Length > 128 || passwordQuestion.Length > 256)
					{
						throw new CreateUserException(CreateUserStatus.InvalidQuestionAnswer);
					}
				}
				MembershipUser membershipUser = HiMembership.Create(userToCreate.Username, userToCreate.Password, userToCreate.Email);
				if (membershipUser != null)
				{
					userToCreate.UserId = (int)membershipUser.ProviderUserKey;
					System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Users SET IsAnonymous = @IsAnonymous, IsApproved = @IsApproved, PasswordQuestion = @PasswordQuestion, PasswordAnswer = @PasswordAnswer, Gender = @Gender, BirthDate = @BirthDate, UserRole = @UserRole WHERE UserId = @UserId");
					this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userToCreate.UserId);
					this.database.AddInParameter(sqlStringCommand, "IsAnonymous", System.Data.DbType.Boolean, userToCreate.IsAnonymous);
					this.database.AddInParameter(sqlStringCommand, "IsApproved", System.Data.DbType.Boolean, userToCreate.IsApproved);
					this.database.AddInParameter(sqlStringCommand, "Gender", System.Data.DbType.Int32, (int)userToCreate.Gender);
					this.database.AddInParameter(sqlStringCommand, "BirthDate", System.Data.DbType.DateTime, null);
					this.database.AddInParameter(sqlStringCommand, "UserRole", System.Data.DbType.Int32, (int)userToCreate.UserRole);
					this.database.AddInParameter(sqlStringCommand, "PasswordQuestion", System.Data.DbType.String, null);
					this.database.AddInParameter(sqlStringCommand, "PasswordAnswer", System.Data.DbType.String, null);
					if (userToCreate.BirthDate.HasValue)
					{
						this.database.SetParameterValue(sqlStringCommand, "BirthDate", userToCreate.BirthDate.Value);
					}
					if (flag)
					{
						string text = null;
						try
						{
							int num;
							int format;
							string salt;
							this.GetPasswordWithFormat(userToCreate.Username, false, out num, out format, out salt);
							if (num == 0)
							{
								text = UserHelper.EncodePassword((MembershipPasswordFormat)format, passwordAnswer, salt);
								this.database.SetParameterValue(sqlStringCommand, "PasswordQuestion", passwordQuestion);
								this.database.SetParameterValue(sqlStringCommand, "PasswordAnswer", text);
							}
							if (num != 0 || (!string.IsNullOrEmpty(text) && text.Length > 128))
							{
								HiMembership.Delete(userToCreate.Username);
								throw new CreateUserException(CreateUserStatus.InvalidQuestionAnswer);
							}
						}
						catch
						{
							HiMembership.Delete(userToCreate.Username);
							throw new CreateUserException(CreateUserStatus.UnknownFailure);
						}
					}
					if (this.database.ExecuteNonQuery(sqlStringCommand) != 1)
					{
						HiMembership.Delete(userToCreate.Username);
						throw new CreateUserException(createUserStatus);
					}
					createUserStatus = CreateUserStatus.Created;
				}
				result = createUserStatus;
			}
			return result;
		}
		public override bool UpdateMembershipUser(HiMembershipUser user)
		{
			bool result;
			if (user == null)
			{
				result = false;
			}
			else
			{
				try
				{
					HiMembership.Update(user.Membership);
				}
				catch
				{
					result = false;
					return result;
				}
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Users SET MobilePIN = @MobilePIN, Gender = @Gender, BirthDate = @BirthDate WHERE UserId = @UserId");
				this.database.AddInParameter(sqlStringCommand, "MobilePIN", System.Data.DbType.String, user.MobilePIN);
				this.database.AddInParameter(sqlStringCommand, "Gender", System.Data.DbType.Int32, (int)user.Gender);
				this.database.AddInParameter(sqlStringCommand, "BirthDate", System.Data.DbType.DateTime, user.BirthDate);
				this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, user.UserId);
				result = (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
			}
			return result;
		}
		public override HiMembershipUser GetMembershipUser(int userId, string username, bool isOnline)
		{
			MembershipUser membershipUser = string.IsNullOrEmpty(username) ? HiMembership.GetUser(userId, isOnline) : HiMembership.GetUser(username, isOnline);
			HiMembershipUser result;
			if (membershipUser == null)
			{
				result = null;
			}
			else
			{
				HiMembershipUser hiMembershipUser = null;
				System.Data.Common.DbCommand sqlStringCommand;
				if (!string.IsNullOrEmpty(username))
				{
					sqlStringCommand = this.database.GetSqlStringCommand("SELECT MobilePIN, IsAnonymous, Gender, BirthDate, UserRole FROM aspnet_Users WHERE LoweredUserName = LOWER(@Username)");
					this.database.AddInParameter(sqlStringCommand, "Username", System.Data.DbType.String, username);
				}
				else
				{
					sqlStringCommand = this.database.GetSqlStringCommand("SELECT MobilePIN, IsAnonymous, Gender, BirthDate, UserRole FROM aspnet_Users WHERE UserId = @UserId");
					this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
				}
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					if (dataReader.Read())
					{
						hiMembershipUser = new HiMembershipUser((bool)dataReader["IsAnonymous"], (UserRole)Convert.ToInt32(dataReader["UserRole"]), membershipUser);
						if (dataReader["MobilePIN"] != DBNull.Value)
						{
							hiMembershipUser.MobilePIN = (string)dataReader["MobilePIN"];
						}
						if (dataReader["Gender"] != DBNull.Value)
						{
							hiMembershipUser.Gender = (Gender)Convert.ToInt32(dataReader["Gender"]);
						}
						if (dataReader["BirthDate"] != DBNull.Value)
						{
							hiMembershipUser.BirthDate = new DateTime?((DateTime)dataReader["BirthDate"]);
						}
					}
					dataReader.Close();
				}
				result = hiMembershipUser;
			}
			return result;
		}
		public override AnonymousUser GetAnonymousUser()
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT @UserId = UserId FROM aspnet_Users WHERE IsAnonymous = 1");
			this.database.AddOutParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, 4);
			this.database.ExecuteNonQuery(sqlStringCommand);
			int userId = (int)this.database.GetParameterValue(sqlStringCommand, "UserId");
			HiMembershipUser membershipUser = this.GetMembershipUser(userId, "Anonymous", true);
			return new AnonymousUser(membershipUser);
		}
		public override bool ValidatePasswordAnswer(string username, string answer)
		{
			int num;
			int format;
			string salt;
			this.GetPasswordWithFormat(username, true, out num, out format, out salt);
			bool result;
			if (num != 0)
			{
				result = false;
			}
			else
			{
				string text = UserHelper.EncodePassword((MembershipPasswordFormat)format, answer, salt);
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT UserId FROM aspnet_Users WHERE LOWER(@Username) = LoweredUserName AND (PasswordAnswer = @PasswordAnswer OR (PasswordQuestion IS NULL AND PasswordAnswer IS NULL))");
				this.database.AddInParameter(sqlStringCommand, "Username", System.Data.DbType.String, username);
				this.database.AddInParameter(sqlStringCommand, "PasswordAnswer", System.Data.DbType.String, text);
				object obj = this.database.ExecuteScalar(sqlStringCommand);
				result = (obj != null && obj != DBNull.Value);
			}
			return result;
		}
		public override bool ChangePasswordQuestionAndAnswer(string username, string newQuestion, string newAnswer)
		{
			int num;
			int format;
			string salt;
			this.GetPasswordWithFormat(username, true, out num, out format, out salt);
			bool result;
			if (num != 0)
			{
				result = false;
			}
			else
			{
				string text = UserHelper.EncodePassword((MembershipPasswordFormat)format, newAnswer, salt);
				if (text.Length > 128)
				{
					result = false;
				}
				else
				{
					System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE aspnet_Users SET PasswordQuestion = @PasswordQuestion, PasswordAnswer = @PasswordAnswer WHERE LOWER(@Username) = LoweredUserName");
					this.database.AddInParameter(sqlStringCommand, "PasswordQuestion", System.Data.DbType.String, newQuestion);
					this.database.AddInParameter(sqlStringCommand, "PasswordAnswer", System.Data.DbType.String, text);
					this.database.AddInParameter(sqlStringCommand, "Username", System.Data.DbType.String, username);
					result = (this.database.ExecuteNonQuery(sqlStringCommand) == 1);
				}
			}
			return result;
		}
		private void GetPasswordWithFormat(string username, bool updateLastLoginActivityDate, out int status, out int passwordFormat, out string passwordSalt)
		{
			passwordFormat = 0;
			passwordSalt = null;
			status = -1;
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("aspnet_Membership_GetPasswordWithFormat");
			this.database.AddInParameter(storedProcCommand, "UserName", System.Data.DbType.String, username);
			this.database.AddInParameter(storedProcCommand, "UpdateLastLoginActivityDate", System.Data.DbType.Boolean, updateLastLoginActivityDate);
			this.database.AddInParameter(storedProcCommand, "CurrentTime", System.Data.DbType.DateTime, DateTime.Now);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
			{
				if (dataReader.Read())
				{
					passwordFormat = dataReader.GetInt32(1);
					passwordSalt = dataReader.GetString(2);
					status = 0;
				}
			}
		}
		private void GetPasswordWithFormat(string username, bool updateLastLoginActivityDate, out int status, out string password, out int passwordFormat, out string passwordSalt, out int failedPasswordAttemptCount, out int failedPasswordAnswerAttemptCount, out bool isApproved, out DateTime lastLoginDate, out DateTime lastActivityDate)
		{
			password = null;
			passwordFormat = 0;
			passwordSalt = null;
			failedPasswordAttemptCount = 0;
			failedPasswordAnswerAttemptCount = 0;
			isApproved = false;
			lastLoginDate = DateTime.Now;
			lastActivityDate = DateTime.Now;
			status = -1;
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("aspnet_Membership_GetPasswordWithFormat");
			this.database.AddInParameter(storedProcCommand, "UserName", System.Data.DbType.String, username);
			this.database.AddInParameter(storedProcCommand, "UpdateLastLoginActivityDate", System.Data.DbType.Boolean, updateLastLoginActivityDate);
			this.database.AddInParameter(storedProcCommand, "CurrentTime", System.Data.DbType.DateTime, DateTime.Now);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
			{
				if (dataReader.Read())
				{
					password = dataReader.GetString(0);
					passwordFormat = dataReader.GetInt32(1);
					passwordSalt = dataReader.GetString(2);
					failedPasswordAttemptCount = dataReader.GetInt32(3);
					failedPasswordAnswerAttemptCount = dataReader.GetInt32(4);
					isApproved = dataReader.GetBoolean(5);
					lastLoginDate = dataReader.GetDateTime(6);
					lastActivityDate = dataReader.GetDateTime(7);
					status = 0;
				}
			}
		}
		public override bool BindOpenId(string username, string openId, string openIdType)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("aspnet_OpenId_Bind");
			this.database.AddInParameter(storedProcCommand, "UserName", System.Data.DbType.String, username);
			this.database.AddInParameter(storedProcCommand, "OpenId", System.Data.DbType.String, openId);
			this.database.AddInParameter(storedProcCommand, "OpenIdType", System.Data.DbType.String, openIdType);
			return this.database.ExecuteNonQuery(storedProcCommand) == 1;
		}
		public override string GetUsernameWithOpenId(string openId, string openIdType)
		{
			string result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT UserName FROM aspnet_Users WHERE LOWER(OpenId)=LOWER(@OpenId) AND LOWER(OpenIdType)=LOWER(@OpenIdType)");
			this.database.AddInParameter(sqlStringCommand, "OpenId", System.Data.DbType.String, openId);
			this.database.AddInParameter(sqlStringCommand, "OpenIdType", System.Data.DbType.String, openIdType);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = dataReader.GetString(0);
				}
			}
			return result;
		}
	}
}
