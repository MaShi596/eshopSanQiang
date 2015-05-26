using System;
using System.Xml;
namespace Hishop.Plugins
{
	public abstract class SMSSender : ConfigablePlugin, IPlugin
	{
		public static SMSSender CreateInstance(string name, string configXml)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}
			Type plugin = SMSPlugins.Instance().GetPlugin("SMSSender", name);
			if (plugin == null)
			{
				return null;
			}
			SMSSender sMSSender = Activator.CreateInstance(plugin) as SMSSender;
			if (sMSSender != null && !string.IsNullOrEmpty(configXml))
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(configXml);
				sMSSender.InitConfig(xmlDocument.FirstChild);
			}
			return sMSSender;
		}
		public static SMSSender CreateInstance(string name)
		{
			return SMSSender.CreateInstance(name, null);
		}
		public abstract bool Send(string cellPhone, string message, out string returnMsg);
		public abstract bool Send(string[] phoneNumbers, string message, out string returnMsg);
	}
}
