using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Subsites.Members;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using Hishop.Plugins;
using kindeditor.Net;
using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class OpenIdSettings : DistributorPage
	{
		private string openIdType;
		protected System.Web.UI.WebControls.Literal lblDisplayName;
		protected System.Web.UI.WebControls.TextBox txtName;
		protected KindeditorControl fcContent;
		protected System.Web.UI.WebControls.Button btnSave;
		protected System.Web.UI.WebControls.HiddenField txtSelectedName;
		protected System.Web.UI.WebControls.HiddenField txtConfigData;
		protected Script Script1;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.openIdType = base.Request.QueryString["t"];
			if (string.IsNullOrEmpty(this.openIdType) || this.openIdType.Trim().Length == 0)
			{
				base.GotoResourceNotFound();
			}
			PluginItem pluginItem = OpenIdPlugins.Instance().GetPluginItem(this.openIdType);
			if (pluginItem == null)
			{
				base.GotoResourceNotFound();
			}
			if (!this.Page.IsPostBack)
			{
				this.lblDisplayName.Text = pluginItem.DisplayName;
				this.txtSelectedName.Value = this.openIdType;
				OpenIdSettingsInfo openIdSettings = SubSiteOpenIdHelper.GetOpenIdSettings(this.openIdType);
				if (openIdSettings != null)
				{
					ConfigData configData = new ConfigData(HiCryptographer.Decrypt(openIdSettings.Settings));
					this.txtConfigData.Value = configData.SettingsXml;
					this.txtName.Text = openIdSettings.Name;
                    this.fcContent.Text = openIdSettings.Description;
				}
			}
		}
		private ConfigData LoadConfig()
		{
			this.txtSelectedName.Value = this.openIdType;
			this.txtConfigData.Value = "";
			ConfigablePlugin configablePlugin = OpenIdService.CreateInstance(this.openIdType);
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
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			ConfigData configData;
			if (!this.ValidateValues(out configData))
			{
				return;
			}
			OpenIdSettingsInfo settings = new OpenIdSettingsInfo
			{
				Name = this.txtName.Text.Trim(),
				Description = this.fcContent.Text,
				OpenIdType = this.openIdType,
				Settings = HiCryptographer.Encrypt(configData.SettingsXml)
			};
			SubSiteOpenIdHelper.SaveSettings(settings);
			base.Response.Redirect("openidservices.aspx");
		}
		private bool ValidateValues(out ConfigData data)
		{
			string text = string.Empty;
			data = this.LoadConfig();
			if (!data.IsValid)
			{
				foreach (string current in data.ErrorMsgs)
				{
					text += Formatter.FormatErrorMessage(current);
				}
			}
			if (string.IsNullOrEmpty(this.txtName.Text) || this.txtName.Text.Trim().Length == 0 || this.txtName.Text.Length > 50)
			{
				text += Formatter.FormatErrorMessage("显示名称不能为空，长度限制在50个字符以内");
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowMsg(text, false);
				return false;
			}
			return true;
		}
	}
}
