using Hidistro.Core.Entities;
using System;
namespace Hidistro.Entities.Members
{
	public class MemberQuery : Pagination
	{
		public string Username
		{
			get;
			set;
		}
		public string Realname
		{
			get;
			set;
		}
		public int? GradeId
		{
			get;
			set;
		}
		public bool? IsApproved
		{
			get;
			set;
		}
		public System.DateTime? StartTime
		{
			get;
			set;
		}
		public System.DateTime? EndTime
		{
			get;
			set;
		}
		public int? OrderNumber
		{
			get;
			set;
		}
		public decimal? OrderMoney
		{
			get;
			set;
		}
		public string CharSymbol
		{
			get;
			set;
		}
		public string ClientType
		{
			get;
			set;
		}
	}
}
