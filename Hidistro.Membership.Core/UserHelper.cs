using Hidistro.Membership.Core.Enums;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
namespace Hidistro.Membership.Core
{
	public static class UserHelper
	{
		public static CreateUserStatus Create(HiMembershipUser userToCreate, string[] roles)
		{
			return UserHelper.Create(userToCreate, null, null, roles);
		}
		public static CreateUserStatus Create(HiMembershipUser userToCreate, string passwordQuestion, string passwordAnswer, string[] roles)
		{
			CreateUserStatus result;
			if (userToCreate == null)
			{
				result = CreateUserStatus.UnknownFailure;
			}
			else
			{
				MemberUserProvider memberUserProvider = MemberUserProvider.Instance();
				try
				{
					CreateUserStatus createUserStatus = memberUserProvider.CreateMembershipUser(userToCreate, passwordQuestion, passwordAnswer);
					if (createUserStatus == CreateUserStatus.Created)
					{
						Roles.AddUserToRoles(userToCreate.Username, roles);
					}
				}
				catch (CreateUserException ex)
				{
					result = ex.CreateUserStatus;
					return result;
				}
				result = CreateUserStatus.Created;
			}
			return result;
		}
		public static HiMembershipUser GetMembershipUser(int userId, string username, bool userIsOnline)
		{
			MemberUserProvider memberUserProvider = MemberUserProvider.Instance();
			return memberUserProvider.GetMembershipUser(userId, username, userIsOnline);
		}
		public static bool UpdateUser(HiMembershipUser user)
		{
			bool result;
			if (user == null)
			{
				result = false;
			}
			else
			{
				MemberUserProvider memberUserProvider = MemberUserProvider.Instance();
				result = memberUserProvider.UpdateMembershipUser(user);
			}
			return result;
		}
		public static LoginUserStatus ValidateUser(HiMembershipUser user)
		{
			LoginUserStatus result;
			if (user == null)
			{
				result = LoginUserStatus.UnknownError;
			}
			else
			{
				if (!user.IsApproved)
				{
					result = LoginUserStatus.AccountPending;
				}
				else
				{
					if (user.IsLockedOut)
					{
						result = LoginUserStatus.AccountLockedOut;
					}
					else
					{
						if (!HiMembership.ValidateUser(user.Username, user.Password))
						{
							result = LoginUserStatus.InvalidCredentials;
						}
						else
						{
							result = LoginUserStatus.Success;
						}
					}
				}
			}
			return result;
		}
		public static string CreateSalt()
		{
			byte[] array = new byte[16];
			new RNGCryptoServiceProvider().GetBytes(array);
			return Convert.ToBase64String(array);
		}
		public static string EncodePassword(MembershipPasswordFormat format, string cleanString, string salt)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(salt.ToLower() + cleanString);
			string result;
			switch (format)
			{
			case MembershipPasswordFormat.Clear:
				result = cleanString;
				break;
			case MembershipPasswordFormat.Hashed:
			{
				byte[] value = ((HashAlgorithm)CryptoConfig.CreateFromName("SHA1")).ComputeHash(bytes);
				result = BitConverter.ToString(value);
				break;
			}
			default:
			{
				byte[] value = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(bytes);
				result = BitConverter.ToString(value);
				break;
			}
			}
			return result;
		}
		public static string GetUsernameWithOpenId(string openId, string openIdType)
		{
			return MemberUserProvider.Instance().GetUsernameWithOpenId(openId, openIdType);
		}
		public static bool BindOpenId(string username, string openId, string openIdType)
		{
			return MemberUserProvider.Instance().BindOpenId(username, openId, openIdType);
		}
	}
}
