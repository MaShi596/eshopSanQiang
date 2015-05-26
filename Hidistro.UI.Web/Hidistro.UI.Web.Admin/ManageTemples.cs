using Hidistro.ControlPanel.Distribution;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Configuration;
using Hidistro.Entities;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ManageDistributorSites)]
	public class ManageTemples : AdminPage
	{
		protected System.Web.UI.WebControls.Literal litUserName;
		protected System.Web.UI.WebControls.Literal litDomain;
		protected System.Web.UI.WebControls.DataList dtManageThemes;
		protected System.Web.UI.WebControls.Button btnSave;
		private int userId;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSave.Click += new System.EventHandler(this.btnManageThemesOK_Click);
			this.dtManageThemes.ItemDataBound += new System.Web.UI.WebControls.DataListItemEventHandler(this.dtManageThemes_ItemDataBound);
			if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.userId))
			{
				base.GotoResourceNotFound();
				return;
			}
			if (!base.IsPostBack)
			{
				this.LoadInfo();
				this.GetThemes();
			}
		}
		private void dtManageThemes_ItemDataBound(object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
		{
			Hidistro.Membership.Context.SiteSettings siteSettings = Hidistro.Membership.Context.SettingsManager.GetSiteSettings(this.userId);
			string str = siteSettings.UserId.ToString();
			System.Collections.Generic.IList<ManageThemeInfo> list = this.LoadThemes("sites\\\\" + str);
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)e.Item.FindControl("rbCheckThemes");
				foreach (ManageThemeInfo current in list)
				{
					if (current.ThemeName == checkBox.Text)
					{
						checkBox.Checked = true;
					}
				}
			}
		}
		private void btnManageThemesOK_Click(object sender, System.EventArgs e)
		{
			Hidistro.Membership.Context.SiteSettings siteSettings = Hidistro.Membership.Context.SettingsManager.GetSiteSettings(this.userId);
			string text = this.Page.Request.MapPath(Globals.ApplicationPath + "/Templates/sites/") + siteSettings.UserId.ToString();
			if (!System.IO.Directory.Exists(text))
			{
				System.IO.Directory.CreateDirectory(text);
			}
			foreach (System.Web.UI.WebControls.DataListItem dataListItem in this.dtManageThemes.Items)
			{
				System.Web.UI.WebControls.CheckBox checkBox = dataListItem.FindControl("rbCheckThemes") as System.Web.UI.WebControls.CheckBox;
				if (checkBox.Checked)
				{
					DisplayThemesImages displayThemesImages = (DisplayThemesImages)dataListItem.FindControl("themeImg");
					string srcPath = this.Page.Request.MapPath(Globals.ApplicationPath + "/Templates/library/") + displayThemesImages.ThemeName;
					string text2 = text + "\\" + displayThemesImages.ThemeName;
					if (!System.IO.Directory.Exists(text2))
					{
						try
						{
							this.CopyDir(srcPath, text2);
						}
						catch
						{
							this.ShowMsg("修改模板失败", false);
						}
					}
				}
			}
			this.ShowMsg("成功修改了店铺模板", true);
			this.GetThemes();
		}
		private void CopyDir(string srcPath, string aimPath)
		{
			try
			{
				if (aimPath[aimPath.Length - 1] != System.IO.Path.DirectorySeparatorChar)
				{
					aimPath += System.IO.Path.DirectorySeparatorChar;
				}
				if (!System.IO.Directory.Exists(aimPath))
				{
					System.IO.Directory.CreateDirectory(aimPath);
				}
				string[] fileSystemEntries = System.IO.Directory.GetFileSystemEntries(srcPath);
				string[] array = fileSystemEntries;
				for (int i = 0; i < array.Length; i++)
				{
					string text = array[i];
					if (System.IO.Directory.Exists(text))
					{
						this.CopyDir(text, aimPath + System.IO.Path.GetFileName(text));
					}
					else
					{
						System.IO.File.Copy(text, aimPath + System.IO.Path.GetFileName(text), true);
					}
				}
			}
			catch
			{
				this.ShowMsg("无法复制!", false);
			}
		}
		private void LoadInfo()
		{
			Hidistro.Membership.Context.SiteSettings siteSettings = Hidistro.Membership.Context.SettingsManager.GetSiteSettings(this.userId);
			if (siteSettings == null)
			{
				base.GotoResourceNotFound();
				return;
			}
			this.litDomain.Text = siteSettings.SiteUrl;
			Hidistro.Membership.Context.Distributor distributor = DistributorHelper.GetDistributor(this.userId);
			if (distributor == null)
			{
				base.GotoResourceNotFound();
				return;
			}
			this.litUserName.Text = distributor.Username;
		}
		protected System.Collections.Generic.IList<ManageThemeInfo> LoadThemes(string path)
		{
			System.Web.HttpContext context = Hidistro.Membership.Context.HiContext.Current.Context;
			System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
			System.Collections.Generic.IList<ManageThemeInfo> list = new System.Collections.Generic.List<ManageThemeInfo>();
			string path2 = context.Request.PhysicalApplicationPath + HiConfiguration.GetConfig().FilesPath + "\\Templates\\" + path;
			string[] array = System.IO.Directory.Exists(path2) ? System.IO.Directory.GetDirectories(path2) : null;
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string path3 = array2[i];
				System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(path3);
				string text = directoryInfo.Name.ToLower(System.Globalization.CultureInfo.InvariantCulture);
				if (text.Length > 0 && !text.StartsWith("_"))
				{
					System.IO.FileInfo[] files = directoryInfo.GetFiles(text + ".xml");
					System.IO.FileInfo[] array3 = files;
					for (int j = 0; j < array3.Length; j++)
					{
						System.IO.FileInfo fileInfo = array3[j];
						ManageThemeInfo manageThemeInfo = new ManageThemeInfo();
						System.IO.FileStream fileStream = fileInfo.OpenRead();
						xmlDocument.Load(fileStream);
						fileStream.Close();
						manageThemeInfo.Name = xmlDocument.SelectSingleNode("ManageTheme/Name").InnerText;
						manageThemeInfo.ThemeImgUrl = xmlDocument.SelectSingleNode("ManageTheme/ImageUrl").InnerText;
						manageThemeInfo.ThemeName = text;
						list.Add(manageThemeInfo);
					}
				}
			}
			return list;
		}
		public void GetThemes()
		{
			this.dtManageThemes.DataSource = this.LoadThemes("library");
			this.dtManageThemes.DataBind();
		}
	}
}
