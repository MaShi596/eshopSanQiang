using Hidistro.Membership.Context;
using System;
using System.Collections;
using System.Xml;
internal class Class5
{
	private static readonly System.Collections.Hashtable hashtable_0 = new System.Collections.Hashtable();
	private static volatile Class5 class5_0 = null;
	private static readonly object object_0 = new object();
	private Class5()
	{
		Class5.hashtable_0.Clear();
		XmlNode configSection = HiContext.Current.Config.GetConfigSection("Hishop/Extensions");
		if (configSection != null)
		{
			foreach (XmlNode xmlNode in configSection.ChildNodes)
			{
				if (xmlNode.NodeType != XmlNodeType.Comment && xmlNode.Name.Equals("add"))
				{
					string value = xmlNode.Attributes["name"].Value;
					string value2 = xmlNode.Attributes["type"].Value;
					XmlAttribute xmlAttribute = xmlNode.Attributes["enabled"];
					if (xmlAttribute == null || xmlAttribute.Value != "false")
					{
						System.Type type = System.Type.GetType(value2);
						if (type == null)
						{
							throw new System.Exception(value2 + " does not exist");
						}
						IExtension extension = System.Activator.CreateInstance(type) as IExtension;
						if (extension == null)
						{
							throw new System.Exception(value2 + " does not implement IExtension or is not configured correctly");
						}
						extension.Init();
						Class5.hashtable_0.Add(value, extension);
					}
				}
			}
		}
	}
	internal static void smethod_0()
	{
		if (Class5.class5_0 == null)
		{
			lock (Class5.object_0)
			{
				if (Class5.class5_0 == null)
				{
					Class5.class5_0 = new Class5();
				}
			}
		}
	}
}
