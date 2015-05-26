using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using System;
using System.Collections;
using System.Globalization;
using System.Web;
namespace Hidistro.Membership.Context
{
	public static class Users
	{
		public static void ClearUserCache(IUser user)
		{
			System.Collections.Hashtable hashtable = Users.smethod_0();
			hashtable[Users.smethod_1(user.UserId.ToString(System.Globalization.CultureInfo.InvariantCulture))] = null;
			hashtable[Users.smethod_1(user.Username)] = null;
		}
		public static CreateUserStatus CreateUser(IUser user, string[] roles)
		{
			CreateUserStatus createUserStatus = UserHelper.Create(user.MembershipUser, roles);
			if (createUserStatus == CreateUserStatus.Created && !Class0.smethod_0(user.UserRole).vmethod_2(user))
			{
				HiMembership.Delete(user.Username);
				createUserStatus = CreateUserStatus.UnknownFailure;
			}
			return createUserStatus;
		}
		public static CreateUserStatus CreateUser(IUser user, string role)
		{
			return Users.CreateUser(user, new string[]
			{
				role
			});
		}
		public static IUser FindUserByUsername(string username)
		{
			return Users.GetUser(0, username, true, false);
		}
		public static AnonymousUser GetAnonymousUser()
		{
			AnonymousUser anonymousUser = HiCache.Get("DataCache-AnonymousUser") as AnonymousUser;
			if (anonymousUser == null)
			{
				anonymousUser = MemberUserProvider.Instance().GetAnonymousUser();
				if (anonymousUser != null && anonymousUser.Username != null && anonymousUser.UserId > 0)
				{
					HiCache.Insert("DataCache-AnonymousUser", anonymousUser, 120);
				}
			}
			return anonymousUser;
		}
		public static string GetLoggedOnUsername()
		{
			HttpContext current = HttpContext.Current;
			if (current.User.Identity.IsAuthenticated && !string.IsNullOrEmpty(current.User.Identity.Name))
			{
				return current.User.Identity.Name;
			}
			return "Anonymous";
		}
		public static IUser GetUser()
		{
			IUser user = Users.GetUser(0, Users.GetLoggedOnUsername(), true, true);
			if (!user.IsAnonymous)
			{
				ApplicationType applicationType = HiContext.Current.ApplicationType;
				if (applicationType == ApplicationType.Unknown)
				{
					return Users.GetAnonymousUser();
				}
				if (applicationType == ApplicationType.Admin && user.UserRole != UserRole.SiteManager)
				{
					return Users.GetAnonymousUser();
				}
				if (applicationType == ApplicationType.Distributor && user.UserRole != UserRole.Distributor)
				{
					return Users.GetAnonymousUser();
				}
				if ((applicationType == ApplicationType.Member || applicationType == ApplicationType.Common) && user.UserRole != UserRole.Member && user.UserRole != UserRole.Underling)
				{
					return Users.GetAnonymousUser();
				}
			}
			return user;
		}
		public static IUser GetUser(int userId)
		{
			return Users.GetUser(userId, null, true, false);
		}
		public static IUser GetUser(int userId, bool isCacheable)
		{
			return Users.GetUser(userId, null, isCacheable, false);
		}
		public static IUser GetUser(int userId, string username, bool isCacheable, bool userIsOnline)
		{
			if (userId == 0 && username == "Anonymous")
			{
				return Users.GetAnonymousUser();
			}
			System.Collections.Hashtable hashtable = Users.smethod_0();
			string key = (userId > 0) ? Users.smethod_1(userId.ToString()) : Users.smethod_1(username);
			IUser user;
			if (isCacheable)
			{
				user = (hashtable[key] as IUser);
				if (user != null)
				{
					return user;
				}
			}
			HiMembershipUser membershipUser = UserHelper.GetMembershipUser(userId, username, userIsOnline);
			if (membershipUser == null)
			{
				return Users.GetAnonymousUser();
			}
			user = Class0.smethod_0(membershipUser.UserRole).vmethod_3(membershipUser);
			if (isCacheable)
			{
				hashtable[Users.smethod_1(user.Username)] = user;
				hashtable[Users.smethod_1(user.UserId.ToString())] = user;
			}
			return user;
		}
		public static bool UpdateUser(IUser user)
		{
			if (user == null)
			{
				return false;
			}
			bool result;
			if (result = UserHelper.UpdateUser(user.MembershipUser))
			{
				result = Class0.smethod_0(user.UserRole).vmethod_6(user);
				HiContext current = HiContext.Current;
				if (current.User.UserId == user.UserId)
				{
					current.User = user;
				}
			}
			Users.ClearUserCache(user);
			return result;
		}
		private static System.Collections.Hashtable smethod_0()
		{
			System.Collections.Hashtable hashtable = HiCache.Get("DataCache-UserLookuptable") as System.Collections.Hashtable;
			if (hashtable == null)
			{
				hashtable = new System.Collections.Hashtable();
				HiCache.Insert("DataCache-UserLookuptable", hashtable, 120);
			}
			return hashtable;
		}
		private static string smethod_1(object object_0)
		{
			return string.Format("User-{0}", new object[]
			{
				object_0
			}).ToLower();
		}
		public static LoginUserStatus ValidateUser(IUser user)
		{
			return UserHelper.ValidateUser(user.MembershipUser);
		}
		public static bool ValidTradePassword(IUser user)
		{
			return Class0.smethod_0(user.UserRole).vmethod_7(user.Username, user.TradePassword);
		}
		public static IUser GetContexUser()
		{
			return Users.GetUser(0, Users.GetLoggedOnUsername(), true, true);
		}
	}
}
