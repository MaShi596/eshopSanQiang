using Hidistro.Core.Entities;
using System;
namespace Hidistro.Entities.Members
{
	public class BalanceDrawRequestQuery : Pagination
	{
		public System.DateTime? FromDate
		{
			get;
			set;
		}
		public System.DateTime? ToDate
		{
			get;
			set;
		}
		public int? UserId
		{
			get;
			set;
		}
		public string UserName
		{
			get;
			set;
		}
	}
}
