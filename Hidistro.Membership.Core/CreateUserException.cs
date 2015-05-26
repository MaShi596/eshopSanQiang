using Hidistro.Membership.Core.Enums;
using System;
using System.Runtime.Serialization;
namespace Hidistro.Membership.Core
{
	public class CreateUserException : Exception
	{
		private CreateUserStatus status;
		public CreateUserStatus CreateUserStatus
		{
			get
			{
				return this.status;
			}
		}
		public CreateUserException()
		{
		}
		public CreateUserException(CreateUserStatus status)
		{
			this.status = status;
		}
		public CreateUserException(string message) : base(message)
		{
		}
		public CreateUserException(CreateUserStatus status, string message) : base(message)
		{
			this.status = status;
		}
		public CreateUserException(string message, Exception inner) : base(message, inner)
		{
		}
		public CreateUserException(CreateUserStatus status, string message, Exception inner) : base(message, inner)
		{
			this.status = status;
		}
		protected CreateUserException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
