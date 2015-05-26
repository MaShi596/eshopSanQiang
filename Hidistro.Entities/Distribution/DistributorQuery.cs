using Hidistro.Core.Entities;
using System;
namespace Hidistro.Entities.Distribution
{
	public class DistributorQuery : Pagination
	{
		public bool IsApproved
		{
			get;
			set;
		}
		public string Username
		{
			get;
			set;
		}
		public string RealName
		{
			get;
			set;
		}
		public int? GradeId
		{
			get;
			set;
		}
		public int? LineId
		{
			get;
			set;
		}
	}
}
