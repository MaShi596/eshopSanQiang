using Hidistro.Core;
using System;
namespace Hidistro.Messages
{
	public abstract class InnerMessageProvider
	{
		private static readonly InnerMessageProvider DefaultInstance;
		static InnerMessageProvider()
		{
			InnerMessageProvider.DefaultInstance = (DataProviders.CreateInstance("Hidistro.Messages.Data.InnerMessageData,Hidistro.Messages.Data") as InnerMessageProvider);
		}
		public static InnerMessageProvider Instance()
		{
			return InnerMessageProvider.DefaultInstance;
		}
		public abstract bool SendMessage(string subject, string message, string sendto);
		public abstract bool SendDistributorMessage(string subject, string message, string distributor, string sendto);
	}
}
