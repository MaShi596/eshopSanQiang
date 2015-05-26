using ASPNET.WebControls;
using Hidistro.ControlPanel.Distribution;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Configuration;
using Hidistro.Core.Entities;
using Hidistro.Entities;
using Hidistro.Entities.Distribution;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.DistributorSiteRequests)]
	public class SiteRequests : AdminPage
	{
		private string userName;
		protected System.Web.UI.WebControls.TextBox txtDistributorName;
		protected System.Web.UI.WebControls.Button btnSearch;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidRequestId;
		protected PageSize hrefPageSize;
		protected Pager pager;
		protected Grid grdDistributorDomainRequests;
		protected Pager pager1;
		protected System.Web.UI.HtmlControls.HtmlGenericControl spanUserName;
		protected System.Web.UI.HtmlControls.HtmlGenericControl domainName1;
		protected System.Web.UI.HtmlControls.HtmlGenericControl spanDistributorName;
		protected System.Web.UI.HtmlControls.HtmlGenericControl spanUserNameForRefuse;
		protected System.Web.UI.WebControls.TextBox txtReason;
		protected System.Web.UI.WebControls.Button btnSave;
		protected System.Web.UI.WebControls.Button btnRefuse;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!string.IsNullOrEmpty(base.Request["showMessage"]) && base.Request["showMessage"] == "true")
			{
				int requestId = 0;
				if (string.IsNullOrEmpty(base.Request["requestId"]) || !int.TryParse(base.Request["requestId"], out requestId))
				{
					base.Response.Write("{\"Status\":\"0\"}");
					base.Response.End();
					return;
				}
				SiteRequestInfo siteRequestInfo = DistributorHelper.GetSiteRequestInfo(requestId);
				if (siteRequestInfo == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				Hidistro.Membership.Context.Distributor distributor = DistributorHelper.GetDistributor(siteRequestInfo.UserId);
				if (distributor == null)
				{
					base.Response.Write("{\"Status\":\"0\"}");
					base.Response.End();
					return;
				}
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
				stringBuilder.AppendFormat(",\"UserName\":\"{0}\"", distributor.Username);
				stringBuilder.AppendFormat(",\"RealName\":\"{0}\"", distributor.RealName);
				stringBuilder.AppendFormat(",\"CompanyName\":\"{0}\"", distributor.CompanyName);
				stringBuilder.AppendFormat(",\"Email\":\"{0}\"", distributor.Email);
				stringBuilder.AppendFormat(",\"Area\":\"{0}\"", RegionHelper.GetFullRegion(distributor.RegionId, string.Empty));
				stringBuilder.AppendFormat(",\"Address\":\"{0}\"", distributor.Address);
				stringBuilder.AppendFormat(",\"QQ\":\"{0}\"", distributor.QQ);
				stringBuilder.AppendFormat(",\"MSN\":\"{0}\"", distributor.MSN);
				stringBuilder.AppendFormat(",\"PostCode\":\"{0}\"", distributor.Zipcode);
				stringBuilder.AppendFormat(",\"Wangwang\":\"{0}\"", distributor.Wangwang);
				stringBuilder.AppendFormat(",\"CellPhone\":\"{0}\"", distributor.CellPhone);
				stringBuilder.AppendFormat(",\"Telephone\":\"{0}\"", distributor.TelPhone);
				stringBuilder.AppendFormat(",\"RegisterDate\":\"{0}\"", distributor.CreateDate);
				stringBuilder.AppendFormat(",\"LastLoginDate\":\"{0}\"", distributor.LastLoginDate);
				stringBuilder.AppendFormat(",\"Domain1\":\"{0}\"", siteRequestInfo.FirstSiteUrl);
				base.Response.Clear();
				base.Response.ContentType = "application/json";
				base.Response.Write("{\"Status\":\"1\"" + stringBuilder.ToString() + "}");
				base.Response.End();
			}
			this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			this.btnRefuse.Click += new System.EventHandler(this.btnRefuse_Click);
			this.LoadParameters();
			if (!this.Page.IsPostBack)
			{
				this.BindRequests();
			}
		}
		private void LoadParameters()
		{
			if (!this.Page.IsPostBack)
			{
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["userName"]))
				{
					this.userName = this.Page.Request.QueryString["userName"];
				}
				this.txtDistributorName.Text = this.userName;
				return;
			}
			this.userName = this.txtDistributorName.Text.Trim();
		}
		private void btnRefuse_Click(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.txtReason.Text.Trim()))
			{
				this.ShowMsg("拒绝原因不能为空", false);
				return;
			}
			if (DistributorHelper.RefuseSiteRequest(int.Parse(this.hidRequestId.Value), this.txtReason.Text.Trim()))
			{
				this.BindRequests();
				this.ShowMsg("您拒绝了该分销商的站点开通申请", true);
				return;
			}
			this.ShowMsg("拒绝该该分销商的站点开通申请失败", false);
		}
		private void ReBind(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("userName", this.txtDistributorName.Text.Trim());
			nameValueCollection.Add("pageSize", this.pager.PageSize.ToString());
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
			}
			base.ReloadPage(nameValueCollection);
		}
		private void BindRequests()
		{
			Pagination pagination = new Pagination();
			pagination.PageIndex = this.pager.PageIndex;
			pagination.PageSize = this.pager.PageSize;
			int totalRecords = 0;
			System.Data.DataTable domainRequests = DistributorHelper.GetDomainRequests(pagination, this.userName, out totalRecords);
			this.grdDistributorDomainRequests.DataSource = domainRequests;
			this.grdDistributorDomainRequests.DataBind();
			this.pager.TotalRecords=totalRecords;
			this.pager1.TotalRecords=totalRecords;
		}
		private void btnSearch_Click(object sender, System.EventArgs e)
		{
			this.ReBind(true);
		}
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			SiteRequestInfo siteRequestInfo = DistributorHelper.GetSiteRequestInfo(int.Parse(this.hidRequestId.Value));
			if (siteRequestInfo == null)
			{
				base.GotoResourceNotFound();
				return;
			}
			Hidistro.Membership.Context.SiteSettings siteSettings = new Hidistro.Membership.Context.SiteSettings(siteRequestInfo.FirstSiteUrl, new int?(siteRequestInfo.UserId));
			siteSettings.Disabled = false;
			siteSettings.CreateDate = new System.DateTime?(System.DateTime.Now);
			siteSettings.RequestDate = new System.DateTime?(siteRequestInfo.RequestTime);
			siteSettings.LogoUrl = "/utility/pics/agentlogo.jpg";
			bool flag;
			bool flag2;
			int siteQty;
			Hidistro.Membership.Context.LicenseChecker.Check(out flag, out flag2, out siteQty);
			if (!DistributorHelper.AddSiteSettings(siteSettings, siteRequestInfo.RequestId, siteQty))
			{
				this.ShowMsg("开通分销商站点失败，可能是您能够开启的数量已经达到了授权的上限或是授权已过有效期！", false);
				return;
			}
			System.Collections.Generic.IList<ManageThemeInfo> list = this.LoadThemes();
			string text = this.Page.Request.MapPath(Globals.ApplicationPath + "/Storage/sites/") + siteSettings.UserId.ToString();
			string text2 = this.Page.Request.MapPath(Globals.ApplicationPath + "/Templates/sites/") + siteSettings.UserId.ToString() + "\\" + list[0].ThemeName;
			string srcPath = this.Page.Request.MapPath(Globals.ApplicationPath + "/Templates/library/") + list[0].ThemeName;
			if (!System.IO.Directory.Exists(text))
			{
				try
				{
					System.IO.Directory.CreateDirectory(text);
					System.IO.Directory.CreateDirectory(text + "/article");
					System.IO.Directory.CreateDirectory(text + "/brand");
					System.IO.Directory.CreateDirectory(text + "/fckfiles");
					System.IO.Directory.CreateDirectory(text + "/help");
					System.IO.Directory.CreateDirectory(text + "/link");
					System.IO.Directory.CreateDirectory(text + "/category");
				}
				catch
				{
					this.ShowMsg("开通分销商站点失败", false);
					return;
				}
			}
			if (!System.IO.Directory.Exists(text2))
			{
				try
				{
					this.CopyDir(srcPath, text2);
					siteSettings.Theme = list[0].ThemeName;
					Hidistro.Membership.Context.SettingsManager.Save(siteSettings);
				}
				catch
				{
					this.ShowMsg("开通分销商站点失败", false);
					return;
				}
			}
			this.BindRequests();
			this.ShowMsg("成功开通了分销商的站点", true);
		}
		protected System.Collections.Generic.IList<ManageThemeInfo> LoadThemes()
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
						list.Add(manageThemeInfo);
					}
				}
			}
			return list;
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
	}
}
