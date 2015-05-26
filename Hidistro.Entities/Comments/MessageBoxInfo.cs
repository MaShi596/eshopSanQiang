using System;
namespace Hidistro.Entities.Comments
{
	public class MessageBoxInfo
	{
		public long ContentId
		{
			get;
			set;
		}
		public string Title
		{
			get;
			set;
		}
		public string Content
		{
			get;
			set;
		}
		public System.DateTime Date
		{
			get;
			set;
		}
		public long MessageId
		{
			get;
			set;
		}
		public string Sernder
		{
			get;
			set;
		}
		public string Accepter
		{
			get;
			set;
		}
		public bool IsRead
		{
			get;
			set;
		}
	}
}
