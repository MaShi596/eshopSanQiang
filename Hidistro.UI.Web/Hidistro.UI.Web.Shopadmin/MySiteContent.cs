using Hidistro.Core;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class MySiteContent : DistributorPage
	{
		protected System.Web.UI.WebControls.TextBox txtSiteName;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtSiteNameTip;
		protected System.Web.UI.WebControls.FileUpload fileUpload;
		protected HiImage imgLogo;
		protected ImageLinkButton btnDeleteLogo;
		protected KindeditorControl fkFooter;
		protected KindeditorControl fckRegisterAgreement;
		protected DecimalLengthDropDownList dropBitNumber;
		protected System.Web.UI.WebControls.TextBox txtNamePrice;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtNamePriceTip;
		protected System.Web.UI.WebControls.TextBox txtShowDays;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtShowDaysTip;
		protected System.Web.UI.WebControls.TextBox txtSiteDescription;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtSiteDescriptionTip;
		protected System.Web.UI.WebControls.TextBox txtSearchMetaDescription;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtSearchMetaDescriptionTip;
		protected System.Web.UI.WebControls.TextBox txtSearchMetaKeywords;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtSearchMetaKeywordsTip;
		protected KindeditorControl fcOnLineServer;
		protected System.Web.UI.WebControls.Button btnOK;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnDeleteLogo.Click += new System.EventHandler(this.btnDeleteLogo_Click);
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			if (!this.Page.IsPostBack)
			{
				Hidistro.Membership.Context.SiteSettings siteSettings = Hidistro.Membership.Context.SettingsManager.GetSiteSettings(Hidistro.Membership.Context.HiContext.Current.User.UserId);
				if (siteSettings == null)
				{
					this.Page.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/store/SiteRequest.aspx");
				}
				this.LoadSiteContent(siteSettings);
			}
		}
		private void LoadSiteContent(Hidistro.Membership.Context.SiteSettings siteSettings)
		{
			this.txtSiteName.Text = siteSettings.SiteName;
			this.imgLogo.ImageUrl = siteSettings.LogoUrl;
			if (!string.IsNullOrEmpty(siteSettings.LogoUrl))
			{
				this.btnDeleteLogo.Visible = true;
			}
			else
			{
				this.btnDeleteLogo.Visible = false;
			}
			this.txtSiteDescription.Text = siteSettings.SiteDescription;
			this.txtSearchMetaDescription.Text = siteSettings.SearchMetaDescription;
			this.txtSearchMetaKeywords.Text = siteSettings.SearchMetaKeywords;
			if (!string.IsNullOrEmpty(siteSettings.HtmlOnlineServiceCode))
			{
                this.fcOnLineServer.Text = siteSettings.HtmlOnlineServiceCode.Replace("\\", "'");
			}
			this.fkFooter.Text=siteSettings.Footer;
			this.fckRegisterAgreement.Text=siteSettings.RegisterAgreement;
			this.dropBitNumber.SelectedValue = siteSettings.DecimalLength;
			this.txtNamePrice.Text = siteSettings.YourPriceName;
			this.txtShowDays.Text = siteSettings.OrderShowDays.ToString(System.Globalization.CultureInfo.InvariantCulture);
		}
		private void btnDeleteLogo_Click(object sender, System.EventArgs e)
		{
			Hidistro.Membership.Context.SiteSettings siteSettings = Hidistro.Membership.Context.SettingsManager.GetSiteSettings(Hidistro.Membership.Context.HiContext.Current.User.UserId);
			try
			{
				SubsiteStoreHelper.DeleteImage(siteSettings.LogoUrl);
			}
			catch
			{
			}
			siteSettings.LogoUrl = string.Empty;
			Hidistro.Membership.Context.SettingsManager.Save(siteSettings);
			this.LoadSiteContent(siteSettings);
		}
		private bool ValidateValues(out int showDays)
		{
			string text = string.Empty;
			if (!int.TryParse(this.txtShowDays.Text, out showDays))
			{
				text += Formatter.FormatErrorMessage("订单显示设置不能为空,必须为正整数,范围在1-90之间");
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowMsg(text, false);
				return false;
			}
			return true;
		}
		private void btnOK_Click(object sender, System.EventArgs e)
		{
			int orderShowDays;
			if (!this.ValidateValues(out orderShowDays))
			{
				return;
			}
			Hidistro.Membership.Context.SiteSettings siteSettings = Hidistro.Membership.Context.SettingsManager.GetSiteSettings(Hidistro.Membership.Context.HiContext.Current.User.UserId);
			if (this.fileUpload.HasFile)
			{
				try
				{
					siteSettings.LogoUrl = SubsiteStoreHelper.UploadLogo(this.fileUpload.PostedFile);
				}
				catch
				{
					this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
					return;
				}
			}
			siteSettings.SiteName = this.txtSiteName.Text.Trim();
			siteSettings.SiteDescription = this.txtSiteDescription.Text.Trim();
			siteSettings.RegisterAgreement = this.fckRegisterAgreement.Text.Trim();
			siteSettings.SearchMetaDescription = this.txtSearchMetaDescription.Text.Trim();
			siteSettings.SearchMetaKeywords = this.txtSearchMetaKeywords.Text.Trim();
			if (!string.IsNullOrEmpty(this.fcOnLineServer.Text))
			{
				siteSettings.HtmlOnlineServiceCode = this.fcOnLineServer.Text.Trim().Replace("'", "\\");
			}
			else
			{
				siteSettings.HtmlOnlineServiceCode = string.Empty;
			}
			siteSettings.Footer = this.fkFooter.Text;
			siteSettings.DecimalLength = this.dropBitNumber.SelectedValue;
			if (this.txtNamePrice.Text.Length <= 20)
			{
				siteSettings.YourPriceName = this.txtNamePrice.Text;
			}
			siteSettings.OrderShowDays = orderShowDays;
			Globals.EntityCoding(siteSettings, true);
			if (this.ValidationSettings(siteSettings))
			{
				Hidistro.Membership.Context.SettingsManager.Save(siteSettings);
				this.ShowMsg("成功修改了店铺基本信息", true);
				this.LoadSiteContent(siteSettings);
				return;
			}
		}
		private bool ValidationSettings(Hidistro.Membership.Context.SiteSettings setting)
		{
			ValidationResults validationResults = Validation.Validate<Hidistro.Membership.Context.SiteSettings>(setting, new string[]
			{
				"ValMasterSettings"
			});
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(current.Message);
				}
				this.ShowMsg(text, false);
			}
			return validationResults.IsValid;
		}
	}
}
