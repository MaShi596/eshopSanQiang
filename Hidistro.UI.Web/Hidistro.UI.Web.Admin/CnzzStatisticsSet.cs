using Hidistro.ControlPanel.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.IO;
using System.Net;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[AdministerCheck(true)]
	public class CnzzStatisticsSet : AdminPage
	{
		protected System.Web.UI.HtmlControls.HtmlGenericControl div_pan1;
		protected System.Web.UI.WebControls.LinkButton hlinkCreate;
		protected System.Web.UI.WebControls.Literal litThemeName;
		protected System.Web.UI.HtmlControls.HtmlGenericControl div_pan2;
		protected System.Web.UI.WebControls.LinkButton hplinkSet;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.hlinkCreate.Click += new System.EventHandler(this.hlinkCreate_Click);
			this.hplinkSet.Click += new System.EventHandler(this.hplinkSet_Click);
			if (!base.IsPostBack)
			{
				Hidistro.Membership.Context.SiteSettings siteSettings = Hidistro.Membership.Context.HiContext.Current.SiteSettings;
				if (string.IsNullOrEmpty(siteSettings.CnzzPassword) || string.IsNullOrEmpty(siteSettings.CnzzUsername))
				{
					this.div_pan1.Visible = true;
					this.div_pan2.Visible = false;
					return;
				}
				this.div_pan1.Visible = false;
				this.div_pan2.Visible = true;
				if (siteSettings.EnabledCnzz)
				{
					this.hplinkSet.Text = "关闭统计功能";
					return;
				}
				this.hplinkSet.Text = "开启统计功能";
			}
		}
		protected void hlinkCreate_Click(object sender, System.EventArgs e)
		{
			string host = this.Page.Request.Url.Host;
			string str = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(host + "A9jkLUxm", "MD5").ToLower();
			string requestUriString = "http://wss.cnzz.com/user/companion/92hi.php?domain=" + host + "&key=" + str;
			System.Net.HttpWebRequest httpWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(requestUriString);
			System.Net.HttpWebResponse httpWebResponse = (System.Net.HttpWebResponse)httpWebRequest.GetResponse();
			System.IO.Stream responseStream = httpWebResponse.GetResponseStream();
			responseStream.ReadTimeout = 100;
			System.IO.StreamReader streamReader = new System.IO.StreamReader(responseStream);
			string text = streamReader.ReadToEnd().Trim();
			streamReader.Close();
			if (text.IndexOf("@") == -1)
			{
				this.ShowMsg("创建账号失败", false);
				return;
			}
			Hidistro.Membership.Context.SiteSettings siteSettings = Hidistro.Membership.Context.HiContext.Current.SiteSettings;
			string[] array = text.Split(new char[]
			{
				'@'
			});
			siteSettings.CnzzUsername = array[0];
			siteSettings.CnzzPassword = array[1];
			siteSettings.EnabledCnzz = false;
			this.div_pan1.Visible = false;
			this.div_pan2.Visible = true;
			this.hplinkSet.Text = "开启统计功能";
			Hidistro.Membership.Context.SettingsManager.Save(siteSettings);
			this.ShowMsg("创建账号成功", true);
		}
		protected void hplinkSet_Click(object sender, System.EventArgs e)
		{
			Hidistro.Membership.Context.SiteSettings siteSettings = Hidistro.Membership.Context.HiContext.Current.SiteSettings;
			this.div_pan1.Visible = false;
			this.div_pan2.Visible = true;
			if (siteSettings.EnabledCnzz)
			{
				siteSettings.EnabledCnzz = false;
				this.hplinkSet.Text = "开启统计功能";
				Hidistro.Membership.Context.SettingsManager.Save(siteSettings);
				this.ShowMsg("关闭统计功能成功", true);
				return;
			}
			siteSettings.EnabledCnzz = true;
			this.hplinkSet.Text = "关闭统计功能";
			Hidistro.Membership.Context.SettingsManager.Save(siteSettings);
			this.ShowMsg("开启统计功能成功", true);
		}
	}
}
