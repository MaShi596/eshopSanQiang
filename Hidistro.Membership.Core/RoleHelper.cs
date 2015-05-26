using Hidistro.Core;
using Hidistro.Core.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
namespace Hidistro.Membership.Core
{
	public class RoleHelper
	{
		private const string PrivilegeCachekey = "DataCache-ManagerPrivileges:{0}";
		private static readonly string defaultRoles;
		private static readonly RolesConfiguration rolesConfig;
		public static string SystemAdministrator
		{
			get
			{
				return RoleHelper.rolesConfig.SystemAdministrator;
			}
		}
		public static string Manager
		{
			get
			{
				return RoleHelper.rolesConfig.Manager;
			}
		}
		static RoleHelper()
		{
			RoleHelper.defaultRoles = null;
			RoleHelper.rolesConfig = null;
			RoleHelper.rolesConfig = HiConfiguration.GetConfig().RolesConfiguration;
			RoleHelper.defaultRoles = RoleHelper.rolesConfig.RoleList();
		}
		public static bool IsBuiltInRole(string roleName)
		{
			return Regex.IsMatch(roleName, RoleHelper.defaultRoles, RegexOptions.IgnoreCase);
		}
		public static bool RoleExists(string roleName)
		{
			return Roles.RoleExists(roleName);
		}
		public static void AddRole(string roleName)
		{
			if (!Roles.RoleExists(roleName))
			{
				Roles.CreateRole(roleName);
			}
		}
		public static void DeleteRole(RoleInfo role)
		{
			if (role != null && !RoleHelper.IsBuiltInRole(role.Name))
			{
				Roles.DeleteRole(role.Name);
			}
		}
		public static void UpdateRole(RoleInfo role)
		{
			if (role != null && !RoleHelper.IsBuiltInRole(role.Name) && (role.Name != null && role.Name.Length != 0))
			{
				MemberRoleProvider.Instance().UpdateRole(role);
			}
		}
		public static RoleInfo GetRole(Guid roleID)
		{
			return MemberRoleProvider.Instance().GetRole(roleID, null);
		}
		public static RoleInfo GetRole(string roleName)
		{
			return MemberRoleProvider.Instance().GetRole(Guid.Empty, roleName);
		}
		public static ArrayList GetRoles()
		{
			return MemberRoleProvider.Instance().GetRoles();
		}
		public static ArrayList GetRoles(int userID)
		{
			return MemberRoleProvider.Instance().GetRoles(userID);
		}
		public static void AddUserToRole(string userName, string roleName)
		{
			Roles.AddUserToRole(userName, roleName);
		}
		public static void RemoveUserFromRole(string userName, string roleName)
		{
			Roles.RemoveUserFromRole(userName, roleName);
		}
		public static string[] GetUserRoleNames(string username)
		{
			string[] result = null;
			try
			{
				result = Roles.GetRolesForUser(username);
			}
			catch
			{
				FormsAuthentication.SignOut();
				HttpContext.Current.Response.Redirect(Globals.GetSiteUrls().Home);
			}
			return result;
		}
		public static IList<int> GetUserPrivileges(string username)
		{
			string string_ = string.Format("DataCache-ManagerPrivileges:{0}", username);
			IList<int> list = HiCache.Get(string_) as List<int>;
			if (list == null)
			{
				try
				{
					list = MemberRoleProvider.Instance().GetPrivilegesForUser(username);
					HiCache.Insert(string_, list, 360);
				}
				catch
				{
					FormsAuthentication.SignOut();
					HttpContext.Current.Response.Redirect(Globals.GetSiteUrls().Home);
				}
			}
			return list;
		}
		public static void SignOut(string username)
		{
			string string_ = string.Format("DataCache-ManagerPrivileges:{0}", username);
			HiCache.Remove(string_);
		}
		public static ArrayList GetUserRoles(string username)
		{
			ArrayList arrayList = new ArrayList();
			string[] userRoleNames = RoleHelper.GetUserRoleNames(username);
			string[] array = userRoleNames;
			for (int i = 0; i < array.Length; i++)
			{
				string roleName = array[i];
				try
				{
					arrayList.Add(RoleHelper.GetRole(roleName));
				}
				catch
				{
				}
			}
			return arrayList;
		}
		public static ArrayList GetRolesWithOutUser(string username)
		{
			ArrayList userRoles = RoleHelper.GetUserRoles(username);
			ArrayList roles = RoleHelper.GetRoles();
			ArrayList arrayList = new ArrayList();
			foreach (RoleInfo roleInfo in roles)
			{
				if (!userRoles.Contains(roleInfo))
				{
					arrayList.Add(roleInfo);
				}
			}
			return arrayList;
		}
		public static void AddPrivilegeInRoles(Guid roleId, string privilege)
		{
			if (string.IsNullOrEmpty(privilege))
			{
				MemberRoleProvider.Instance().DeletePrivilegeInRoles(roleId);
			}
			else
			{
				MemberRoleProvider.Instance().AddDeletePrivilegeInRoles(roleId, privilege);
			}
		}
		public static IList<int> GetPrivilegeByRoles(Guid roleId)
		{
			return MemberRoleProvider.Instance().GetPrivilegeByRoles(roleId);
		}
		public static bool PrivilegeInRoles(Guid roleId, int privilege)
		{
			return MemberRoleProvider.Instance().PrivilegeInRoles(roleId, privilege);
		}
	}
}
