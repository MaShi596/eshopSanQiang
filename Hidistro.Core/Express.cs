using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
namespace Hidistro.Core
{
	public static class Express
	{
		public static string GetExpressType()
		{
			string result = "kuaidi100";
			string text = null;
			HttpContext current = HttpContext.Current;
			if (current != null)
			{
				text = current.Request.MapPath("~/Express.xml");
			}
			XmlDocument xmlDocument = new XmlDocument();
			if (!string.IsNullOrEmpty(text))
			{
				xmlDocument.Load(text);
				XmlNode xmlNode = xmlDocument.SelectSingleNode("expressapi");
				if (xmlNode != null)
				{
					result = xmlNode.Attributes["usetype"].InnerText;
				}
			}
			return result;
		}
		public static string GetDataByKuaidi100(string computer, string expressNo)
		{
			string arg = "29833628d495d7a5";
			string text = null;
			HttpContext current = HttpContext.Current;
			if (current != null)
			{
				text = current.Request.MapPath("~/Express.xml");
			}
			XmlDocument xmlDocument = new XmlDocument();
			if (!string.IsNullOrEmpty(text))
			{
				xmlDocument.Load(text);
				XmlNode xmlNode = xmlDocument.SelectSingleNode("expressapi");
				if (xmlNode != null && xmlNode.ChildNodes.Count > 0)
				{
					foreach (XmlNode xmlNode2 in xmlNode)
					{
						if (xmlNode2.Name == "kuaidi100")
						{
							arg = xmlNode2.Attributes["key"].InnerText;
						}
					}
				}
			}
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Format("http://kuaidi100.com/api?com={0}&nu={1}&show=2&id={2}", computer, expressNo, arg));
			httpWebRequest.Timeout = 8000;
			string text2 = "暂时没有此快递单号的信息";
			HttpWebResponse httpWebResponse;
			string result;
			try
			{
				httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			}
			catch
			{
				result = text2;
				return result;
			}
			if (httpWebResponse.StatusCode == HttpStatusCode.OK)
			{
				Stream responseStream = httpWebResponse.GetResponseStream();
				StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8"));
				text2 = streamReader.ReadToEnd();
				text2 = text2.Replace("&amp;", "");
				text2 = text2.Replace("&nbsp;", "");
				text2 = text2.Replace("&", "");
			}
			result = text2;
			return result;
		}
		public static string GetDataByTaobaoTop(string computer, string expressNo)
		{
			return "暂时没有此快递单号的信息";
		}
		public static string GetExpressData(string computer, string expressNo)
		{
			string result;
			if (Express.GetExpressType() == "kuaidi100")
			{
				result = Express.GetDataByKuaidi100(computer, expressNo);
			}
			else
			{
				result = Express.GetDataByTaobaoTop(computer, expressNo);
			}
			return result;
		}
	}
}
