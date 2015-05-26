using Hidistro.Core.Jobs;
using Hidistro.Messages;
using System;
using System.Globalization;
using System.Xml;
namespace Hidistro.Jobs
{
	public class SubsiteEmailJob : IJob
	{
		private int failureInterval = 15;
		private int numberOfTries = 3;
		public void Execute(XmlNode node)
		{
			if (null != node)
			{
				XmlAttribute xmlAttribute = node.Attributes["failureInterval"];
				XmlAttribute xmlAttribute2 = node.Attributes["numberOfTries"];
				if (xmlAttribute != null)
				{
					try
					{
						this.failureInterval = int.Parse(xmlAttribute.Value, CultureInfo.InvariantCulture);
					}
					catch
					{
						this.failureInterval = 15;
					}
				}
				if (xmlAttribute2 != null)
				{
					try
					{
						this.numberOfTries = int.Parse(xmlAttribute2.Value, CultureInfo.InvariantCulture);
					}
					catch
					{
						this.numberOfTries = 3;
					}
				}
				Emails.SendSubsiteEmails(this.failureInterval, this.numberOfTries);
			}
		}
	}
}
