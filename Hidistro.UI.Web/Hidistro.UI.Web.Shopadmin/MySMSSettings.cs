using Hidistro.Core;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using Hishop.Plugins;
using Ionic.Zlib;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Xml;
namespace Hidistro.UI.Web.Shopadmin
{
	public class MySMSSettings : DistributorPage
	{
		protected System.Web.UI.WebControls.Label lbNum;
		protected System.Web.UI.WebControls.Button btnSaveSMSSettings;
		protected System.Web.UI.WebControls.TextBox txtTestCellPhone;
		protected System.Web.UI.WebControls.TextBox txtTestSubject;
		protected System.Web.UI.WebControls.Button btnTestSend;
		protected System.Web.UI.WebControls.HiddenField txtSelectedName;
		protected System.Web.UI.WebControls.HiddenField txtConfigData;
		protected Script Script1;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnSaveSMSSettings.Click += new System.EventHandler(this.btnSaveSMSSettings_Click);
			this.btnTestSend.Click += new System.EventHandler(this.btnTestSend_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				Hidistro.Membership.Context.SiteSettings siteSettings = Hidistro.Membership.Context.SettingsManager.GetSiteSettings(Hidistro.Membership.Context.HiContext.Current.User.UserId);
				if (siteSettings.SMSEnabled)
				{
					ConfigData configData = new ConfigData(HiCryptographer.Decrypt(siteSettings.SMSSettings));
					this.txtConfigData.Value = configData.SettingsXml;
				}
				this.lbNum.Text = this.GetAmount(siteSettings);
				this.txtSelectedName.Value = "hishop.plugins.sms.ymsms";
			}
		}
		private ConfigData LoadConfig(out string selectedName)
		{
			selectedName = base.Request.Form["ddlSms"];
			this.txtSelectedName.Value = selectedName;
			this.txtConfigData.Value = "";
			if (string.IsNullOrEmpty(selectedName) || selectedName.Length == 0)
			{
				return null;
			}
			ConfigablePlugin configablePlugin = SMSSender.CreateInstance(selectedName);
			if (configablePlugin == null)
			{
				return null;
			}
			ConfigData configData = configablePlugin.GetConfigData(base.Request.Form);
			if (configData != null)
			{
				this.txtConfigData.Value = configData.SettingsXml;
			}
			return configData;
		}
		private void btnSaveSMSSettings_Click(object sender, System.EventArgs e)
		{
			string text;
			ConfigData configData = this.LoadConfig(out text);
			Hidistro.Membership.Context.SiteSettings siteSettings = Hidistro.Membership.Context.SettingsManager.GetSiteSettings(Hidistro.Membership.Context.HiContext.Current.User.UserId);
			if (!string.IsNullOrEmpty(text) && configData != null)
			{
				if (!configData.IsValid)
				{
					string text2 = "";
					foreach (string current in configData.ErrorMsgs)
					{
						text2 += Formatter.FormatErrorMessage(current);
					}
					this.ShowMsg(text2, false);
					return;
				}
				siteSettings.SMSSender = text;
				siteSettings.SMSSettings = HiCryptographer.Encrypt(configData.SettingsXml);
			}
			else
			{
				siteSettings.SMSSender = string.Empty;
				siteSettings.SMSSettings = string.Empty;
			}
			Hidistro.Membership.Context.SettingsManager.Save(siteSettings);
			this.Page.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/tools/MySendMessageTemplets.aspx");
		}
		private void btnTestSend_Click(object sender, System.EventArgs e)
		{
			string text;
			ConfigData configData = this.LoadConfig(out text);
			if (string.IsNullOrEmpty(text) || configData == null)
			{
				this.ShowMsg("请先选择发送方式并填写配置信息", false);
				return;
			}
			if (!configData.IsValid)
			{
				string text2 = "";
				foreach (string current in configData.ErrorMsgs)
				{
					text2 += Formatter.FormatErrorMessage(current);
				}
				this.ShowMsg(text2, false);
				return;
			}
			if (string.IsNullOrEmpty(this.txtTestCellPhone.Text) || string.IsNullOrEmpty(this.txtTestSubject.Text) || this.txtTestCellPhone.Text.Trim().Length == 0 || this.txtTestSubject.Text.Trim().Length == 0)
			{
				this.ShowMsg("接收手机号和发送内容不能为空", false);
				return;
			}
			if (!System.Text.RegularExpressions.Regex.IsMatch(this.txtTestCellPhone.Text.Trim(), "^(13|14|15|18)\\d{9}$"))
			{
				this.ShowMsg("请填写正确的手机号码", false);
				return;
			}
			SMSSender sMSSender = SMSSender.CreateInstance(text, configData.SettingsXml);
			string string_;
			bool success = sMSSender.Send(this.txtTestCellPhone.Text.Trim(), this.txtTestSubject.Text.Trim(), out string_);
			this.ShowMsg(string_, success);
		}
		protected string GetAmount(Hidistro.Membership.Context.SiteSettings settings)
		{
			if (string.IsNullOrEmpty(settings.SMSSettings))
			{
				return "";
			}
			string xml = HiCryptographer.Decrypt(settings.SMSSettings);
			System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
			xmlDocument.LoadXml(xml);
			string innerText = xmlDocument.SelectSingleNode("xml/Appkey").InnerText;
			string postData = "method=getAmount&Appkey=" + innerText;
			string text = this.PostData("http://sms.kuaidiantong.cn/getAmount.aspx", postData);
			int num;
			if (int.TryParse(text, out num))
			{
				return "您的短信剩余条数为：" + text.ToString();
			}
			return "获取短信条数发生错误，请检查Appkey是否输入正确!";
		}
		public string PostData(string url, string postData)
		{
            string str = string.Empty;
            try
            {
                Uri requestUri = new Uri(url);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUri);
                byte[] bytes = Encoding.UTF8.GetBytes(postData);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = bytes.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream2 = response.GetResponseStream())
                    {
                        Encoding encoding = Encoding.UTF8;
                        Stream stream3 = stream2;
                        if (response.ContentEncoding.ToLower() == "gzip")
                        {
                            stream3 = new GZipStream(stream2, CompressionMode.Decompress);
                        }
                        else if (response.ContentEncoding.ToLower() == "deflate")
                        {
                            stream3 = new DeflateStream(stream2, CompressionMode.Decompress);
                        }
                        using (StreamReader reader = new StreamReader(stream3, encoding))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                str = string.Format("获取信息错误：{0}", exception.Message);
            }
            return str;
		}
	}
}
