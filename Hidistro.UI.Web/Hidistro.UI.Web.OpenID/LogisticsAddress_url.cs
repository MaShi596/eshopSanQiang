using Hidistro.AccountCenter.Profile;
using Hidistro.ControlPanel.Members;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.UI.SaleSystem.CodeBehind;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Xml;
namespace Hidistro.UI.Web.OpenID
{
	public class LogisticsAddress_url : System.Web.UI.Page
	{
		protected System.Web.UI.HtmlControls.HtmlForm form1;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			int num = 0;
			System.Collections.Generic.SortedDictionary<string, string> requestPost = this.GetRequestPost();
			if (requestPost.Count > 0)
			{
				string openIdType = "hishop.plugins.openid.alipay.alipayservice";
				OpenIdSettingsInfo openIdSettings = OpenIdHelper.GetOpenIdSettings(openIdType);
				if (openIdSettings == null)
				{
					base.Response.Write("登录失败，没有找到对应的插件配置信息。");
					return;
				}
				System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
				xmlDocument.LoadXml(HiCryptographer.Decrypt(openIdSettings.Settings));
				AliPayNotify aliPayNotify = new AliPayNotify(requestPost, base.Request.Form["notify_id"], xmlDocument.FirstChild.SelectSingleNode("Partner").InnerText, xmlDocument.FirstChild.SelectSingleNode("Key").InnerText);
				string responseTxt = aliPayNotify.ResponseTxt;
				string a = base.Request.Form["sign"];
				string mysign = aliPayNotify.Mysign;
				if (responseTxt == "true" && a == mysign)
				{
					string text = base.Request.Form["receive_address"];
					if (!string.IsNullOrEmpty(text))
					{
						System.Xml.XmlDocument xmlDocument2 = new System.Xml.XmlDocument();
						xmlDocument2.LoadXml(text);
						ShippingAddressInfo shippingAddressInfo = new ShippingAddressInfo();
						shippingAddressInfo.UserId = Hidistro.Membership.Context.HiContext.Current.User.UserId;
						if (xmlDocument2.SelectSingleNode("/receiveAddress/address") != null && !string.IsNullOrEmpty(xmlDocument2.SelectSingleNode("/receiveAddress/address").InnerText))
						{
							shippingAddressInfo.Address = Globals.HtmlEncode(xmlDocument2.SelectSingleNode("/receiveAddress/address").InnerText);
						}
						if (xmlDocument2.SelectSingleNode("/receiveAddress/fullname") != null && !string.IsNullOrEmpty(xmlDocument2.SelectSingleNode("/receiveAddress/fullname").InnerText))
						{
							shippingAddressInfo.ShipTo = Globals.HtmlEncode(xmlDocument2.SelectSingleNode("/receiveAddress/fullname").InnerText);
						}
						if (xmlDocument2.SelectSingleNode("/receiveAddress/post") != null && !string.IsNullOrEmpty(xmlDocument2.SelectSingleNode("/receiveAddress/post").InnerText))
						{
							shippingAddressInfo.Zipcode = xmlDocument2.SelectSingleNode("/receiveAddress/post").InnerText;
						}
						if (xmlDocument2.SelectSingleNode("/receiveAddress/mobile_phone") != null && !string.IsNullOrEmpty(xmlDocument2.SelectSingleNode("/receiveAddress/mobile_phone").InnerText))
						{
							shippingAddressInfo.CellPhone = xmlDocument2.SelectSingleNode("/receiveAddress/mobile_phone").InnerText;
						}
						if (xmlDocument2.SelectSingleNode("/receiveAddress/phone") != null && !string.IsNullOrEmpty(xmlDocument2.SelectSingleNode("/receiveAddress/phone").InnerText))
						{
							shippingAddressInfo.TelPhone = xmlDocument2.SelectSingleNode("/receiveAddress/phone").InnerText;
						}
						string text2 = string.Empty;
						string text3 = string.Empty;
						string text4 = string.Empty;
						if (xmlDocument2.SelectSingleNode("/receiveAddress/area") != null && !string.IsNullOrEmpty(xmlDocument2.SelectSingleNode("/receiveAddress/area").InnerText))
						{
							text2 = xmlDocument2.SelectSingleNode("/receiveAddress/area").InnerText;
						}
						if (xmlDocument2.SelectSingleNode("/receiveAddress/city") != null && !string.IsNullOrEmpty(xmlDocument2.SelectSingleNode("/receiveAddress/city").InnerText))
						{
							text3 = xmlDocument2.SelectSingleNode("/receiveAddress/city").InnerText;
						}
						if (xmlDocument2.SelectSingleNode("/receiveAddress/prov") != null && !string.IsNullOrEmpty(xmlDocument2.SelectSingleNode("/receiveAddress/prov").InnerText))
						{
							text4 = xmlDocument2.SelectSingleNode("/receiveAddress/prov").InnerText;
						}
						if (string.IsNullOrEmpty(text2) && string.IsNullOrEmpty(text3) && string.IsNullOrEmpty(text4))
						{
							shippingAddressInfo.RegionId = 0;
						}
						else
						{
							shippingAddressInfo.RegionId = RegionHelper.GetRegionId(text2, text3, text4);
						}
						Hidistro.Membership.Context.SiteSettings arg_371_0 = Hidistro.Membership.Context.HiContext.Current.SiteSettings;
						if (PersonalHelper.GetShippingAddressCount(Hidistro.Membership.Context.HiContext.Current.User.UserId) < Hidistro.Membership.Context.HiContext.Current.Config.ShippingAddressQuantity)
						{
							num = PersonalHelper.AddShippingAddress(shippingAddressInfo);
						}
					}
				}
			}
			this.Page.Response.Redirect(Globals.ApplicationPath + "/SubmmitOrder.aspx?shippingId=" + num);
		}
		private System.Collections.Generic.SortedDictionary<string, string> GetRequestPost()
		{
			System.Collections.Generic.SortedDictionary<string, string> sortedDictionary = new System.Collections.Generic.SortedDictionary<string, string>();
			System.Collections.Specialized.NameValueCollection form = base.Request.Form;
			string[] allKeys = form.AllKeys;
			for (int i = 0; i < allKeys.Length; i++)
			{
				sortedDictionary.Add(allKeys[i], base.Request.Form[allKeys[i]]);
			}
			return sortedDictionary;
		}
	}
}
