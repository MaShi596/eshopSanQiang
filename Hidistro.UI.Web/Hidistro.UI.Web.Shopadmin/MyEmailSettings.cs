using Hidistro.Core;
using Hidistro.Core.Configuration;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using Hishop.Plugins;
using System;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class MyEmailSettings : DistributorPage
	{
		protected System.Web.UI.WebControls.Button btnChangeEmailSettings;
		protected System.Web.UI.WebControls.TextBox txtTestEmail;
		protected System.Web.UI.WebControls.Button btnTestEmailSettings;
		protected System.Web.UI.WebControls.HiddenField txtSelectedName;
		protected System.Web.UI.WebControls.HiddenField txtConfigData;
		protected Script Script1;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnChangeEmailSettings.Click += new System.EventHandler(this.btnChangeEmailSettings_Click);
			this.btnTestEmailSettings.Click += new System.EventHandler(this.btnTestEmailSettings_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				Hidistro.Membership.Context.SiteSettings siteSettings = Hidistro.Membership.Context.SettingsManager.GetSiteSettings(Hidistro.Membership.Context.HiContext.Current.User.UserId);
				if (siteSettings.EmailEnabled)
				{
					this.txtSelectedName.Value = siteSettings.EmailSender.ToLower();
					ConfigData configData = new ConfigData(HiCryptographer.Decrypt(siteSettings.EmailSettings));
					this.txtConfigData.Value = configData.SettingsXml;
				}
			}
		}
		private ConfigData LoadConfig(out string selectedName)
		{
			selectedName = base.Request.Form["ddlEmails"];
			this.txtSelectedName.Value = selectedName;
			this.txtConfigData.Value = "";
			if (string.IsNullOrEmpty(selectedName) || selectedName.Length == 0)
			{
				return null;
			}
			ConfigablePlugin configablePlugin = EmailSender.CreateInstance(selectedName);
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
		private void btnChangeEmailSettings_Click(object sender, System.EventArgs e)
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
				siteSettings.EmailSender = text;
				siteSettings.EmailSettings = HiCryptographer.Encrypt(configData.SettingsXml);
			}
			else
			{
				siteSettings.EmailSender = string.Empty;
				siteSettings.EmailSettings = string.Empty;
			}
			Hidistro.Membership.Context.SettingsManager.Save(siteSettings);
			this.Page.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/tools/MySendMessageTemplets.aspx");
		}
		private void btnTestEmailSettings_Click(object sender, System.EventArgs e)
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
			if (string.IsNullOrEmpty(this.txtTestEmail.Text) || this.txtTestEmail.Text.Trim().Length == 0)
			{
				this.ShowMsg("请填写接收测试邮件的邮箱地址", false);
				return;
			}
			if (!System.Text.RegularExpressions.Regex.IsMatch(this.txtTestEmail.Text.Trim(), "([a-zA-Z\\.0-9_-])+@([a-zA-Z0-9_-])+((\\.[a-zA-Z0-9_-]{2,4}){1,2})"))
			{
				this.ShowMsg("请填写正确的邮箱地址", false);
				return;
			}
			System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage
			{
				IsBodyHtml = true,
				Priority = System.Net.Mail.MailPriority.High,
				Body = "Success",
				Subject = "This is a test mail"
			};
			mailMessage.To.Add(this.txtTestEmail.Text.Trim());
			EmailSender emailSender = EmailSender.CreateInstance(text, configData.SettingsXml);
			try
			{
				if (emailSender.Send(mailMessage, System.Text.Encoding.GetEncoding(HiConfiguration.GetConfig().EmailEncoding)))
				{
					this.ShowMsg("发送测试邮件成功", true);
				}
				else
				{
					this.ShowMsg("发送测试邮件失败", false);
				}
			}
			catch
			{
				this.ShowMsg("邮件配置错误", false);
			}
		}
	}
}
