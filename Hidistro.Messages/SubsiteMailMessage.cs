using System;
using System.Net.Mail;
namespace Hidistro.Messages
{
	public class SubsiteMailMessage
	{
		public System.Net.Mail.MailMessage Mail
		{
			get;
			private set;
		}
		public int DistributorUserId
		{
			get;
			private set;
		}
		public SubsiteMailMessage(int distributorUserId, System.Net.Mail.MailMessage mail)
		{
			this.Mail = mail;
			this.DistributorUserId = distributorUserId;
		}
	}
}
