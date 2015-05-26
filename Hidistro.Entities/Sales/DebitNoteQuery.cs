using Hidistro.Core.Entities;
using System;
namespace Hidistro.Entities.Sales
{
	public class DebitNoteQuery : Pagination
	{
		public string OrderId
		{
			get;
			set;
		}
	}
}
