using Hidistro.Entities.Sales;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml;
namespace Hidistro.Entities
{
	public static class ExpressHelper
	{
		private static string path = HttpContext.Current.Request.MapPath("~/Express.xml");
		public static ExpressCompanyInfo FindNode(string company)
		{
			ExpressCompanyInfo expressCompanyInfo = null;
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			string xpath = string.Format("//company[@name='{0}']", company);
			XmlNode xmlNode2 = xmlNode.SelectSingleNode(xpath);
			if (xmlNode2 != null)
			{
				expressCompanyInfo = new ExpressCompanyInfo();
				expressCompanyInfo.Name = company;
				expressCompanyInfo.Kuaidi100Code = xmlNode2.Attributes["Kuaidi100Code"].Value;
				expressCompanyInfo.TaobaoCode = xmlNode2.Attributes["TaobaoCode"].Value;
			}
			return expressCompanyInfo;
		}
		public static ExpressCompanyInfo FindNodeByCode(string code)
		{
			ExpressCompanyInfo expressCompanyInfo = null;
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			string xpath = string.Format("//company[@TaobaoCode='{0}']", code);
			XmlNode xmlNode2 = xmlNode.SelectSingleNode(xpath);
			if (xmlNode2 != null)
			{
				expressCompanyInfo = new ExpressCompanyInfo();
				expressCompanyInfo.Name = xmlNode2.Attributes["name"].Value;
				expressCompanyInfo.Kuaidi100Code = xmlNode2.Attributes["Kuaidi100Code"].Value;
				expressCompanyInfo.TaobaoCode = code;
			}
			return expressCompanyInfo;
		}
		public static System.Collections.Generic.IList<ExpressCompanyInfo> GetAllExpress()
		{
			System.Collections.Generic.IList<ExpressCompanyInfo> list = new System.Collections.Generic.List<ExpressCompanyInfo>();
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("companys");
			foreach (XmlNode xmlNode3 in xmlNode2.ChildNodes)
			{
				list.Add(new ExpressCompanyInfo
				{
					Name = xmlNode3.Attributes["name"].Value,
					Kuaidi100Code = xmlNode3.Attributes["Kuaidi100Code"].Value,
					TaobaoCode = xmlNode3.Attributes["TaobaoCode"].Value
				});
			}
			return list;
		}
		public static System.Collections.Generic.IList<string> GetAllExpressName()
		{
			System.Collections.Generic.IList<string> list = new System.Collections.Generic.List<string>();
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("companys");
			foreach (XmlNode xmlNode3 in xmlNode2.ChildNodes)
			{
				list.Add(xmlNode3.Attributes["name"].Value);
			}
			return list;
		}
		public static DataTable GetExpressTable()
		{
			DataTable dataTable = new DataTable();
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("companys");
			dataTable.Columns.Add("Name");
			dataTable.Columns.Add("Kuaidi100Code");
			dataTable.Columns.Add("TaobaoCode");
			foreach (XmlNode xmlNode3 in xmlNode2.ChildNodes)
			{
				DataRow dataRow = dataTable.NewRow();
				dataRow["Name"] = xmlNode3.Attributes["name"].Value;
				dataRow["Kuaidi100Code"] = xmlNode3.Attributes["Kuaidi100Code"].Value;
				dataRow["TaobaoCode"] = xmlNode3.Attributes["TaobaoCode"].Value;
				dataTable.Rows.Add(dataRow);
			}
			return dataTable;
		}
		public static void DeleteExpress(string name)
		{
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("companys");
			foreach (XmlNode xmlNode3 in xmlNode2.ChildNodes)
			{
				if (xmlNode3.Attributes["name"].Value == name)
				{
					xmlNode2.RemoveChild(xmlNode3);
					break;
				}
			}
			xmlNode.Save(ExpressHelper.path);
		}
		public static void AddExpress(string name, string kuaidi100Code, string taobaoCode)
		{
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("companys");
			XmlElement xmlElement = xmlNode.CreateElement("company");
			xmlElement.SetAttribute("name", name);
			xmlElement.SetAttribute("Kuaidi100Code", kuaidi100Code);
			xmlElement.SetAttribute("TaobaoCode", taobaoCode);
			xmlNode2.AppendChild(xmlElement);
			xmlNode.Save(ExpressHelper.path);
		}
		public static bool IsExitExpress(string name)
		{
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("companys");
			bool result;
			foreach (XmlNode xmlNode3 in xmlNode2.ChildNodes)
			{
				if (xmlNode3.Attributes["name"].Value == name)
				{
					result = true;
					return result;
				}
			}
			result = false;
			return result;
		}
		public static void UpdateExpress(string oldcompanyname, string name, string kuaidi100Code, string taobaoCode)
		{
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("companys");
			foreach (XmlNode xmlNode3 in xmlNode2.ChildNodes)
			{
				if (xmlNode3.Attributes["name"].Value == oldcompanyname)
				{
					xmlNode3.Attributes["name"].Value = name;
					xmlNode3.Attributes["Kuaidi100Code"].Value = kuaidi100Code;
					xmlNode3.Attributes["TaobaoCode"].Value = taobaoCode;
					break;
				}
			}
			xmlNode.Save(ExpressHelper.path);
		}
		public static string GetDataByKuaidi100(string computer, string expressNo)
		{
			string arg = "29833628d495d7a5";
			XmlDocument xmlNode = ExpressHelper.GetXmlNode();
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("companys");
			if (xmlNode2 != null)
			{
				arg = xmlNode2.Attributes["Kuaidi100NewKey"].Value;
			}
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(string.Format("http://kuaidi100.com/api?com={0}&nu={1}&show=2&id={2}", computer, expressNo, arg));
			httpWebRequest.Timeout = 8000;
			string text = "暂时没有此快递单号的信息";
			HttpWebResponse httpWebResponse;
			string result;
			try
			{
				httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			}
			catch
			{
				result = text;
				return result;
			}
			if (httpWebResponse.StatusCode == HttpStatusCode.OK)
			{
				System.IO.Stream responseStream = httpWebResponse.GetResponseStream();
				System.IO.StreamReader streamReader = new System.IO.StreamReader(responseStream, System.Text.Encoding.GetEncoding("UTF-8"));
				text = streamReader.ReadToEnd();
				text = text.Replace("&amp;", "");
				text = text.Replace("&nbsp;", "");
				text = text.Replace("&", "");
			}
			result = text;
			return result;
		}
		public static string GetExpressData(string computer, string expressNo)
		{
			return ExpressHelper.GetDataByKuaidi100(computer, expressNo);
		}
		private static XmlDocument GetXmlNode()
		{
			XmlDocument xmlDocument = new XmlDocument();
			if (!string.IsNullOrEmpty(ExpressHelper.path))
			{
				xmlDocument.Load(ExpressHelper.path);
			}
			return xmlDocument;
		}
	}
}
