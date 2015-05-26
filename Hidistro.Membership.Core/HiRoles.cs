using System;
using System.Security.Principal;
using System.Web.Security;
namespace Hidistro.Membership.Core
{
	public static class HiRoles
	{
		public static string[] GetRolesFromPrinciple(IPrincipal user)
		{
			RolePrincipal rolePrincipal = user as RolePrincipal;
			string[] result;
			if (rolePrincipal != null)
			{
				result = rolePrincipal.GetRoles();
			}
			else
			{
				result = null;
			}
			return result;
		}
	}
}
