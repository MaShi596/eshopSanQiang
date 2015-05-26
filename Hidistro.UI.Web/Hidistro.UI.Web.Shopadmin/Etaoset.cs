using Hidistro.Core;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class Etaoset : DistributorPage
	{
		private Hidistro.Membership.Context.SiteSettings siteSettings;
		protected System.Web.UI.WebControls.Button BtnCreateEtao;
		protected System.Web.UI.WebControls.Label lbEtaoCreate;
		protected System.Web.UI.HtmlControls.HtmlGenericControl etaoset;
		protected System.Web.UI.WebControls.FileUpload fudVerifyFile;
		protected System.Web.UI.WebControls.Button btnUpoad;
		protected System.Web.UI.WebControls.TextBox txtEtaoID;
		protected YesNoRadioButtonList rdobltIsCreateFeed;
		protected System.Web.UI.HtmlControls.HtmlGenericControl incDir;
		protected System.Web.UI.WebControls.Label lblEtaoFeedInc;
		protected System.Web.UI.HtmlControls.HtmlGenericControl fulDir;
		protected System.Web.UI.WebControls.Label lblEtaoFeedFull;
		protected System.Web.UI.WebControls.Button btnChangeEmailSettings;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.siteSettings = Hidistro.Membership.Context.SettingsManager.GetSiteSettings(Hidistro.Membership.Context.HiContext.Current.User.UserId);
			if (this.siteSettings.IsOpenEtao)
			{
				this.etaoset.Visible = true;
				this.lbEtaoCreate.Visible = false;
				this.BtnCreateEtao.Visible = false;
				if (!this.Page.IsPostBack)
				{
					this.LoadEtaoOpen();
					return;
				}
			}
			else
			{
				if (this.siteSettings.EtaoStatus == -1)
				{
					this.BtnCreateEtao.Visible = false;
					this.lbEtaoCreate.Visible = true;
					this.lbEtaoCreate.Text = "您已经于" + this.siteSettings.EtaoApplyTime.ToString() + " 申请开通一淘，请等待管理员审核。";
					this.etaoset.Visible = false;
					return;
				}
				if (this.siteSettings.EtaoStatus == 0)
				{
					if (!this.siteSettings.IsOpenEtao)
					{
						this.BtnCreateEtao.Visible = true;
						this.lbEtaoCreate.Visible = false;
						this.etaoset.Visible = false;
						return;
					}
				}
				else
				{
					if (this.siteSettings.EtaoStatus == 1 && !this.siteSettings.IsOpenEtao)
					{
						this.BtnCreateEtao.Visible = false;
						this.lbEtaoCreate.Visible = true;
						this.lbEtaoCreate.Text = "您的一淘已被管理员暂停。";
						this.etaoset.Visible = false;
					}
				}
			}
		}
		protected void LoadEtaoOpen()
		{
			if (!string.IsNullOrEmpty(this.siteSettings.EtaoID))
			{
				this.txtEtaoID.Text = this.siteSettings.EtaoID;
			}
			this.rdobltIsCreateFeed.SelectedValue = (this.siteSettings.EtaoStatus == 1);
			this.lblEtaoFeedInc.Text = string.Concat(new object[]
			{
				"http://",
				this.siteSettings.SiteUrl,
				Globals.ApplicationPath,
				"/Storage/Root/",
				this.siteSettings.UserId,
				"/IncrementIndex.xml"
			});
			this.lblEtaoFeedFull.Text = string.Concat(new object[]
			{
				"http://",
				this.siteSettings.SiteUrl,
				Globals.ApplicationPath,
				"/Storage/Root/",
				this.siteSettings.UserId,
				"/FullIndex.xml"
			});
		}
		protected void btnUpoad_Click(object sender, System.EventArgs e)
		{
			if (!this.fudVerifyFile.HasFile)
			{
				this.ShowMsg("需要选择验证文件再点击上传。", false);
				return;
			}
			if (this.fudVerifyFile.PostedFile.ContentType.ToLower(System.Globalization.CultureInfo.InvariantCulture) != "text/plain")
			{
				this.ShowMsg("只能上传TXT文本文件", false);
				return;
			}
			if (this.fudVerifyFile.FileName.ToLower(System.Globalization.CultureInfo.InvariantCulture).EndsWith(".txt"))
			{
				if (this.fudVerifyFile.FileName.IndexOf('.') == this.fudVerifyFile.FileName.Length - 4)
				{
					if (this.fudVerifyFile.FileName.ToLower() != "etao_domain_verify.txt")
					{
						this.ShowMsg("你上传的不是一淘的验证文件!", false);
						return;
					}
					string text = "etao_domain_verify.txt";
					string str = string.Empty;
					string text2 = string.Empty;
					if (!string.IsNullOrEmpty(Globals.ApplicationPath))
					{
						if (Globals.ApplicationPath.EndsWith("\\"))
						{
							str = Globals.ApplicationPath;
						}
						else
						{
							str = Globals.ApplicationPath + "\\";
						}
						text2 = Hidistro.Membership.Context.HiContext.Current.Context.Request.MapPath(str + text);
					}
					else
					{
						text2 = Hidistro.Membership.Context.HiContext.Current.Context.Request.MapPath("/");
						if (text2.EndsWith("\\"))
						{
							text2 += text;
						}
						else
						{
							text2 = text2 + "\\" + text;
						}
					}
					this.fudVerifyFile.SaveAs(text2);
					this.ShowMsg("上传成功。", true);
					return;
				}
			}
			this.ShowMsg("文件名只能有一个.号", false);
		}
		protected void btnChangeEmailSettings_Click(object sender, System.EventArgs e)
		{
			this.siteSettings.EtaoID = this.txtEtaoID.Text;
			this.siteSettings.EtaoStatus = (this.rdobltIsCreateFeed.SelectedValue ? 1 : 0);
			Hidistro.Membership.Context.SettingsManager.Save(this.siteSettings);
			this.ShowMsg("保存成功。", true);
		}
		protected void BtnCreateEtao_Click(object sender, System.EventArgs e)
		{
			this.siteSettings.EtaoStatus = -1;
			this.siteSettings.EtaoApplyTime = new System.DateTime?(System.DateTime.Now);
			Hidistro.Membership.Context.SettingsManager.Save(this.siteSettings);
			this.ShowMsg("申请成功。", true);
			this.BtnCreateEtao.Visible = false;
			this.lbEtaoCreate.Visible = true;
			this.lbEtaoCreate.Text = "您已经于" + this.siteSettings.EtaoApplyTime.ToString() + " 申请开通一淘，请等待管理员审核。";
		}
	}
}
