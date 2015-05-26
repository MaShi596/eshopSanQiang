using Hidistro.Core.Entities;
using System;
namespace Hidistro.Entities.Store
{
	public class ManagerQuery : Pagination
	{
		public string Username
		{
			get;
			set;
		}
		public System.Guid RoleId
		{
			get;
			set;
		}
	}
}
