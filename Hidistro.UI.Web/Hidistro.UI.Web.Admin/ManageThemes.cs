using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Configuration;
using Hidistro.Entities;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.ControlPanel.Utility;
using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using Ionic.Zlib;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Themes)]
	public class ManageThemes : AdminPage
	{
		protected System.Web.UI.WebControls.Literal litThemeName;
		protected System.Web.UI.WebControls.Image imgThemeImgUrl;
		protected System.Web.UI.WebControls.Image Image1;
		protected System.Web.UI.WebControls.Literal lblThemeCount;
		protected System.Web.UI.WebControls.DataList dtManageThemes;
		protected System.Web.UI.WebControls.FileUpload fileTemplate;
		protected System.Web.UI.WebControls.Button btnUpload2;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hdtempname;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.litThemeName.Text = Hidistro.Membership.Context.HiContext.Current.SiteSettings.Theme;
			this.dtManageThemes.ItemCommand += new System.Web.UI.WebControls.DataListCommandEventHandler(this.dtManageThemes_ItemCommand);
			this.btnUpload2.Click += new System.EventHandler(this.btnUpload2_Click);
			if (!this.Page.IsPostBack)
			{
				Hidistro.Membership.Context.SiteSettings siteSettings = Hidistro.Membership.Context.HiContext.Current.SiteSettings;
				this.BindData(siteSettings);
			}
		}
		private void BindData(Hidistro.Membership.Context.SiteSettings siteSettings)
		{
			System.Collections.Generic.IList<ManageThemeInfo> list = this.LoadThemes(siteSettings.Theme);
			this.dtManageThemes.DataSource = list;
			this.dtManageThemes.DataBind();
			this.lblThemeCount.Text = list.Count.ToString();
		}
		private void dtManageThemes_ItemCommand(object sender, System.Web.UI.WebControls.DataListCommandEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				string text = this.dtManageThemes.DataKeys[e.Item.ItemIndex].ToString();
				string text2 = this.Page.Request.MapPath(Globals.ApplicationPath + "/Templates/library/") + text;
				string text3 = this.Page.Request.MapPath(Globals.ApplicationPath + "/Templates/master/") + text;
				if (e.CommandName == "btnUse")
				{
					if (!System.IO.Directory.Exists(text3))
					{
						try
						{
							this.CopyDir(text2, text3);
						}
						catch
						{
							this.ShowMsg("修改店铺模板失败", false);
						}
					}
					this.UserThems(text);
					this.ShowMsg("成功修改了店铺模板", true);
				}
				if (e.CommandName == "download")
				{
					try
					{
						new System.IO.DirectoryInfo(text3);
						System.Text.Encoding uTF = System.Text.Encoding.UTF8;
						using (ZipFile zipFile = new ZipFile())
						{
                            zipFile.CompressionLevel = CompressionLevel.Default;
							if (System.IO.Directory.Exists(text3))
							{
								zipFile.AddDirectory(text3);
							}
							else
							{
								zipFile.AddDirectory(text2);
							}
							System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
							response.ContentType = "application/zip";
							response.ContentEncoding = uTF;
							response.AddHeader("Content-Disposition", "attachment;filename=" + text + ".zip");
							response.Clear();
							zipFile.Save(response.OutputStream);
							response.Flush();
							response.Close();
						}
					}
					catch (System.Exception ex)
					{
						throw ex;
					}
				}
				if (e.CommandName == "back")
				{
					try
					{
						this.CopyDir(text2, text3);
						this.ShowMsg("恢复店铺模板成功", true);
					}
					catch
					{
						this.ShowMsg("恢复店铺模板失败", false);
					}
				}
			}
		}
		protected void UserThems(string name)
		{
			Hidistro.Membership.Context.SiteSettings siteSettings = Hidistro.Membership.Context.HiContext.Current.SiteSettings;
			siteSettings.Theme = name;
			Hidistro.Membership.Context.SettingsManager.Save(siteSettings);
			HiCache.Remove("AdsFileCache-Admin");
			HiCache.Remove("ProductSubjectFileCache-Admin");
			HiCache.Remove("ArticleSubjectFileCache-Admin");
			HiCache.Remove("ProductFileCache-Admin");
			HiCache.Remove("AdFileCache-Admin");
			HiCache.Remove(" ProductFileCache-Admin");
			HiCache.Remove("CommentFileCache-Admin");
			this.BindData(siteSettings);
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
		protected void btnUpload2_Click(object sender, System.EventArgs e)
		{
			string text = this.hdtempname.Value.Trim();
			if (string.IsNullOrEmpty(text))
			{
				this.ShowMsg("无法获取对应模板名称,请重新上传", false);
				return;
			}
			if (this.fileTemplate.PostedFile.ContentLength == 0 || (this.fileTemplate.PostedFile.ContentType != "application/x-zip-compressed" && this.fileTemplate.PostedFile.ContentType != "application/zip" && this.fileTemplate.PostedFile.ContentType != "application/octet-stream"))
			{
				this.ShowMsg("请上传正确的数据包文件", false);
				return;
			}
			string fileName = System.IO.Path.GetFileName(this.fileTemplate.PostedFile.FileName);
			if (!fileName.Equals(text + ".zip"))
			{
				this.ShowMsg("上传的模板压缩名与原模板名不一致", false);
				return;
			}
			string text2 = this.Page.Request.MapPath(Globals.ApplicationPath + "/Templates/master/");
			string text3 = System.IO.Path.Combine(text2, fileName);
			this.fileTemplate.PostedFile.SaveAs(text3);
			this.PrepareDataFiles(text2, new object[]
			{
				text3
			});
			System.IO.File.Delete(text3);
			this.ShowMsg("上传成功！", true);
			this.UserThems(text);
			this.hdtempname.Value = "";
		}
		public string PrepareDataFiles(string _datapath, params object[] initParams)
		{
            string path = (string)initParams[0];
            DirectoryInfo info = new DirectoryInfo(_datapath);
            DirectoryInfo info2 = info.CreateSubdirectory(Path.GetFileNameWithoutExtension(path));
            using (ZipFile file = ZipFile.Read(Path.Combine(info.FullName, path)))
            {
                foreach (ZipEntry entry in file)
                {
                    entry.Extract(info2.FullName, ExtractExistingFileAction.OverwriteSilently);
                }
            }
            return info2.FullName;
		}
		protected System.Collections.Generic.IList<ManageThemeInfo> LoadThemes(string currentThemeName)
		{
			System.Web.HttpContext context = Hidistro.Membership.Context.HiContext.Current.Context;
			System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
			System.Collections.Generic.IList<ManageThemeInfo> list = new System.Collections.Generic.List<ManageThemeInfo>();
			string path = context.Request.PhysicalApplicationPath + HiConfiguration.GetConfig().FilesPath + "\\Templates\\library";
			string[] array = System.IO.Directory.Exists(path) ? System.IO.Directory.GetDirectories(path) : null;
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string path2 = array2[i];
				System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(path2);
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
						if (string.Compare(manageThemeInfo.ThemeName, currentThemeName) == 0)
						{
							this.litThemeName.Text = manageThemeInfo.ThemeName;
							this.imgThemeImgUrl.ImageUrl = string.Concat(new string[]
							{
								Globals.ApplicationPath,
								"/Templates/library/",
								text,
								"/",
								xmlDocument.SelectSingleNode("ManageTheme/ImageUrl").InnerText
							});
							this.Image1.ImageUrl = string.Concat(new string[]
							{
								Globals.ApplicationPath,
								"/Templates/library/",
								text,
								"/",
								xmlDocument.SelectSingleNode("ManageTheme/BigImageUrl").InnerText
							});
						}
						list.Add(manageThemeInfo);
					}
				}
			}
			return list;
		}
	}
}
