using System;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
namespace Hidistro.Membership.Core
{
	public class RoleInfo : IComparable
	{
		private Guid roleID = Guid.Empty;
		private string name;
		private string description;
		public Guid RoleID
		{
			get
			{
				return this.roleID;
			}
			set
			{
				this.roleID = value;
			}
		}
		[StringLengthValidator(1, 60, Ruleset = "ValRoleInfo", MessageTemplate = "部门名称不能为空，长度限制在60个字符以内")]
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}
		[StringLengthValidator(0, 100, Ruleset = "ValRoleInfo", MessageTemplate = "职能说明的长度限制在100个字符以内")]
		public string Description
		{
			get
			{
				return this.description;
			}
			set
			{
				this.description = value;
			}
		}
		public bool IsRoleIDAssigned
		{
			get
			{
				return this.RoleID != Guid.Empty;
			}
		}
		public RoleInfo()
		{
		}
		public RoleInfo(Guid roleID, string name)
		{
			this.roleID = roleID;
			this.name = name;
		}
		public override bool Equals(object object_0)
		{
			RoleInfo roleInfo = object_0 as RoleInfo;
			return roleInfo != null && roleInfo.RoleID == this.RoleID && roleInfo.Name == this.Name;
		}
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		public int CompareTo(object target)
		{
			RoleInfo roleInfo = target as RoleInfo;
			int result;
			if (roleInfo != null)
			{
				if (this.RoleID == roleInfo.RoleID)
				{
					result = 0;
				}
				else
				{
					result = string.Compare(this.Name, roleInfo.Name, true, CultureInfo.InvariantCulture);
				}
			}
			else
			{
				result = -1;
			}
			return result;
		}
	}
}
