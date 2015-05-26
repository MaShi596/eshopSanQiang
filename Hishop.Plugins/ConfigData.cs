using System;
using System.Collections.Generic;
using System.Xml;
namespace Hishop.Plugins
{
	public class ConfigData
	{
		private readonly XmlDocument xmlDocument_0;
		private readonly XmlNode root;
		public bool IsValid
		{
			get;
			internal set;
		}
		public IList<string> ErrorMsgs
		{
			get;
			private set;
		}
		public bool NeedProtect
		{
			get;
			internal set;
		}
		internal XmlNodeList AttributeNodes
		{
			get
			{
				return this.root.ChildNodes;
			}
		}
		public string SettingsXml
		{
			get
			{
				return this.root.OuterXml;
			}
		}
		public ConfigData()
		{
			this.IsValid = true;
			this.ErrorMsgs = new List<string>();
			this.xmlDocument_0 = new XmlDocument();
			this.root = this.xmlDocument_0.CreateElement("xml");
			this.xmlDocument_0.AppendChild(this.root);
		}
		public ConfigData(string string_0)
		{
			this.IsValid = true;
			this.ErrorMsgs = new List<string>();
			this.xmlDocument_0 = new XmlDocument();
			this.xmlDocument_0.LoadXml(string_0);
			this.root = this.xmlDocument_0.FirstChild;
		}
		internal void Add(string attributeName, string string_0)
		{
			if (!string.IsNullOrEmpty(attributeName) && !string.IsNullOrEmpty(string_0) && attributeName.Trim().Length > 0 && string_0.Length > 0)
			{
				XmlNode xmlNode = this.xmlDocument_0.CreateElement(attributeName);
				xmlNode.InnerText = string_0;
				this.root.AppendChild(xmlNode);
			}
		}
	}
}
