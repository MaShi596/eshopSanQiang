using Hidistro.ControlPanel.Members;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.UI.SaleSystem.CodeBehind;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Xml;
namespace Hidistro.UI.Web.OpenID
{
	public class LogisticsAddress : System.Web.UI.Page
	{
		protected System.Web.UI.HtmlControls.HtmlForm form1;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			string openIdType = "hishop.plugins.openid.alipay.alipayservice";
			OpenIdSettingsInfo openIdSettings = OpenIdHelper.GetOpenIdSettings(openIdType);
			if (openIdSettings == null)
			{
				return;
			}
			string value = base.Request.QueryString["alipaytoken"];
			System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
			xmlDocument.LoadXml(HiCryptographer.Decrypt(openIdSettings.Settings));
			System.Collections.Generic.SortedDictionary<string, string> sortedDictionary = new System.Collections.Generic.SortedDictionary<string, string>();
			sortedDictionary.Add("service", "user.logistics.address.query");
			sortedDictionary.Add("partner", xmlDocument.FirstChild.SelectSingleNode("Partner").InnerText);
			sortedDictionary.Add("_input_charset", "utf-8");
			sortedDictionary.Add("return_url", Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("LogisticsAddress_url")));
			sortedDictionary.Add("token", value);
			System.Collections.Generic.Dictionary<string, string> dictionary = OpenIdFunction.FilterPara(sortedDictionary);
			string value2 = OpenIdFunction.BuildMysign(dictionary, xmlDocument.FirstChild.SelectSingleNode("Key").InnerText, "MD5", "utf-8");
			dictionary.Add("sign", value2);
			dictionary.Add("sign_type", "MD5");
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			foreach (System.Collections.Generic.KeyValuePair<string, string> current in dictionary)
			{
				stringBuilder.Append(OpenIdFunction.CreateField(current.Key, current.Value));
			}
			sortedDictionary.Clear();
			dictionary.Clear();
			OpenIdFunction.Submit(OpenIdFunction.CreateForm(stringBuilder.ToString(), "https://mapi.alipay.com/gateway.do?_input_charset=utf-8"));
		}
	}
}
