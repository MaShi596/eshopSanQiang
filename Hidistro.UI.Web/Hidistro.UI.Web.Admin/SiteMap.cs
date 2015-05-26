using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Membership.Context;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[AdministerCheck(true)]
	public class SiteMap : AdminPage
	{
		protected System.Web.UI.WebControls.HyperLink Hysitemap;
		protected System.Web.UI.WebControls.TextBox tbsitemaptime;
		protected System.Web.UI.WebControls.TextBox tbsitemapnum;
		protected System.Web.UI.WebControls.Button btnSaveMapSettings;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.BindSiteMap();
			}
			string text = Hidistro.Membership.Context.HiContext.Current.HostPath + Globals.ApplicationPath + "/sitemapindex.xml";
			this.Hysitemap.Text = text;
			this.Hysitemap.NavigateUrl = text;
			this.Hysitemap.Target = "_blank";
			System.IO.StreamReader streamReader = new System.IO.StreamReader(base.Server.MapPath(Globals.ApplicationPath + "/robots.txt"), System.Text.Encoding.Default);
			string text2 = streamReader.ReadToEnd();
			streamReader.Close();
			if (text2.Contains("Sitemap"))
			{
				text2 = text2.Substring(0, text2.IndexOf("Sitemap"));
			}
			System.IO.FileStream fileStream = new System.IO.FileStream(base.Server.MapPath(Globals.ApplicationPath + "/robots.txt"), System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
			try
			{
				using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(fileStream, System.Text.Encoding.Default))
				{
					streamWriter.Flush();
					streamWriter.Write(text2);
					streamWriter.WriteLine("Sitemap: " + text);
					streamWriter.Flush();
					streamWriter.Dispose();
					streamWriter.Close();
				}
			}
			catch (System.Exception)
			{
			}
			finally
			{
				fileStream.Dispose();
				fileStream.Close();
			}
		}
		protected void BindSiteMap()
		{
			Hidistro.Membership.Context.SiteSettings masterSettings = Hidistro.Membership.Context.SettingsManager.GetMasterSettings(false);
			if (!string.IsNullOrEmpty(masterSettings.SiteMapTime))
			{
				this.tbsitemaptime.Text = masterSettings.SiteMapTime;
			}
			if (!string.IsNullOrEmpty(masterSettings.SiteMapNum))
			{
				this.tbsitemapnum.Text = masterSettings.SiteMapNum;
			}
		}
		protected void btnSaveMapSettings_Click(object sender, System.EventArgs e)
		{
			Hidistro.Membership.Context.SiteSettings masterSettings = Hidistro.Membership.Context.SettingsManager.GetMasterSettings(false);
			if (!string.IsNullOrEmpty(this.tbsitemaptime.Text) && !string.IsNullOrEmpty(this.tbsitemapnum.Text))
			{
				masterSettings.SiteMapNum = this.tbsitemapnum.Text;
				masterSettings.SiteMapTime = this.tbsitemaptime.Text;
				Hidistro.Membership.Context.SettingsManager.Save(masterSettings);
				this.BindSiteMap();
				this.ShowMsg("保存成功。", true);
				return;
			}
			this.ShowMsg("参数错误。", false);
		}
	}
}
