using System;
using System.Globalization;
namespace Hidistro.Core.Configuration
{
	public class RolesConfiguration
	{
		private string distributor = "Distributor";
		private string member = "Member";
		private string underling = "Underling";
		private string systemAdmin = "SystemAdministrator";
		private string manager = "Manager";
		public string Distributor
		{
			get
			{
				return this.distributor;
			}
		}
		public string Member
		{
			get
			{
				return this.member;
			}
		}
		public string Underling
		{
			get
			{
				return this.underling;
			}
		}
		public string Manager
		{
			get
			{
				return this.manager;
			}
		}
		public string SystemAdministrator
		{
			get
			{
				return this.systemAdmin;
			}
		}
		public string RoleList()
		{
			return string.Format(CultureInfo.InvariantCulture, "^({0}|{1}|{2}|{3}|{4})$", new object[]
			{
				this.Distributor,
				this.Member,
				this.Underling,
				this.SystemAdministrator,
				this.Manager
			});
		}
	}
}
