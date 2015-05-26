using System;
using System.Net.Mail;
using System.Text;
using System.Xml;
namespace Hishop.Plugins
{
	public abstract class EmailSender : ConfigablePlugin, IPlugin
	{
		public static EmailSender CreateInstance(string name, string configXml)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			Type plugin = EmailPlugins.Instance().GetPlugin("EmailSender", name);
			if (plugin == null)
			{
				return null;
			}
			EmailSender emailSender = Activator.CreateInstance(plugin) as EmailSender;
			if (emailSender != null && !string.IsNullOrEmpty(configXml))
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(configXml);
				emailSender.InitConfig(xmlDocument.FirstChild);
			}
			return emailSender;
		}
		public static EmailSender CreateInstance(string name)
		{
			return EmailSender.CreateInstance(name, null);
		}
		public abstract bool Send(MailMessage mail, Encoding emailEncoding);
	}
}
