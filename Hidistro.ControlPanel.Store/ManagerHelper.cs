using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using System;
using System.Web;
namespace Hidistro.ControlPanel.Store
{
	public static class ManagerHelper
	{
		public static CreateUserStatus CreateAdministrator(SiteManager administrator)
		{
			return ManagerHelper.Create(administrator, HiContext.Current.Config.RolesConfiguration.SystemAdministrator);
		}
		public static CreateUserStatus Create(SiteManager managerToCreate, string department)
		{
			CreateUserStatus result;
			if (managerToCreate == null || managerToCreate.UserRole != UserRole.SiteManager)
			{
				result = CreateUserStatus.UnknownFailure;
			}
			else
			{
				string[] roles = new string[]
				{
					HiContext.Current.Config.RolesConfiguration.Manager,
					department
				};
				CreateUserStatus createUserStatus = Users.CreateUser(managerToCreate, roles);
				result = createUserStatus;
			}
			return result;
		}
		public static SiteManager GetManager(int userId)
		{
			IUser user = Users.GetUser(userId, false);
			SiteManager result;
			if (user != null && !user.IsAnonymous && user.UserRole == UserRole.SiteManager)
			{
				result = (user as SiteManager);
			}
			else
			{
				result = null;
			}
			return result;
		}
		public static DbQueryResult GetManagers(ManagerQuery query)
		{
			return StoreProvider.Instance().GetManagers(query);
		}
		public static void ClearRolePrivilege(Guid roleId)
		{
			StoreProvider.Instance().ClearRolePrivilege(roleId);
		}
		public static bool Delete(int userId)
		{
			SiteManager siteManager = HiContext.Current.User as SiteManager;
			return siteManager.UserId != userId && StoreProvider.Instance().DeleteManager(userId);
		}
		public static bool Update(SiteManager manager)
		{
			return Users.UpdateUser(manager);
		}
		public static LoginUserStatus ValidLogin(SiteManager manager)
		{
			LoginUserStatus result;
			if (manager == null)
			{
				result = LoginUserStatus.InvalidCredentials;
			}
			else
			{
				result = Users.ValidateUser(manager);
			}
			return result;
		}
		public static void CheckPrivilege(Privilege privilege)
		{
			IUser user = HiContext.Current.User;
			if (user.IsAnonymous || user.UserRole != UserRole.SiteManager)
			{
				HttpContext.Current.Response.Redirect(Globals.GetAdminAbsolutePath("/accessDenied.aspx?privilege=" + privilege.ToString()));
			}
			else
			{
				SiteManager siteManager = user as SiteManager;
				if (!siteManager.IsAdministrator && !siteManager.HasPrivilege((int)privilege))
				{
					HttpContext.Current.Response.Redirect(Globals.GetAdminAbsolutePath("/accessDenied.aspx?privilege=" + privilege.ToString()));
				}
			}
		}
	}
}
