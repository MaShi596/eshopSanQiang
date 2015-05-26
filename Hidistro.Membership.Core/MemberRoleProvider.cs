using Hidistro.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
namespace Hidistro.Membership.Core
{
	public abstract class MemberRoleProvider
	{
		private static readonly MemberRoleProvider _defaultInstance;
		static MemberRoleProvider()
		{
			MemberRoleProvider._defaultInstance = (DataProviders.CreateInstance("Hidistro.Membership.Data.RoleData,Hidistro.Membership.Data") as MemberRoleProvider);
		}
		public static MemberRoleProvider Instance()
		{
			return MemberRoleProvider._defaultInstance;
		}
		public abstract void UpdateRole(RoleInfo role);
		public abstract RoleInfo GetRole(Guid roleId, string roleName);
		public abstract ArrayList GetRoles();
		public abstract ArrayList GetRoles(int userId);
		public abstract IList<int> GetPrivilegesForUser(string userName);
		public abstract void AddDeletePrivilegeInRoles(Guid roleId, string privilege);
		public abstract void DeletePrivilegeInRoles(Guid roleId);
		public abstract IList<int> GetPrivilegeByRoles(Guid roleId);
		public abstract bool PrivilegeInRoles(Guid roleId, int privilege);
		public static RoleInfo PopulateRoleFromIDataReader(IDataReader reader)
		{
			return new RoleInfo
			{
				RoleID = (Guid)reader["RoleID"],
				Name = (string)reader["Name"],
				Description = reader["Description"] as string
			};
		}
	}
}
