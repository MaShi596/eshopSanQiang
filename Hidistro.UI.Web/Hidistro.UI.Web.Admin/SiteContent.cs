using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Urls;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.SiteSettings)]
	public class SiteContent : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtSiteName;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtSiteNameTip;
		protected System.Web.UI.WebControls.FileUpload fileUpload;
		protected System.Web.UI.WebControls.Button btnUpoad;
		protected HiImage imgLogo;
		protected ImageLinkButton btnDeleteLogo;
		protected System.Web.UI.WebControls.TextBox txtDomainName;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtDomainNameTip;
		protected YesNoRadioButtonList radEnableHtmRewrite;
		protected KindeditorControl fkFooter;
		protected KindeditorControl fckRegisterAgreement;
		protected System.Web.UI.WebControls.Literal txtkeycode;
		protected System.Web.UI.WebControls.Button btnkey;
		protected System.Web.UI.HtmlControls.HtmlGenericControl P1;
		protected DecimalLengthDropDownList dropBitNumber;
		protected ImageUploader uploader1;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtProductPointSetTip;
		protected System.Web.UI.WebControls.TextBox txtNamePrice;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtNamePriceTip;
		protected YesNoRadioButtonList radiIsOpenSiteSale;
		protected YesNoRadioButtonList radiIsShowGroupBuy;
		protected YesNoRadioButtonList radiIsShowCountDown;
		protected YesNoRadioButtonList radiIsShowOnlineGift;
		protected KindeditorControl fcOnLineServer;
		protected System.Web.UI.WebControls.Button btnOK;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnUpoad.Click += new System.EventHandler(this.btnUpoad_Click);
			this.btnDeleteLogo.Click += new System.EventHandler(this.btnDeleteLogo_Click);
			this.btnkey.Click += new System.EventHandler(this.btnkey_Click);
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			if (!this.Page.IsPostBack)
			{
				Hidistro.Membership.Context.SiteSettings masterSettings = Hidistro.Membership.Context.SettingsManager.GetMasterSettings(false);
				this.LoadSiteContent(masterSettings);
				this.radEnableHtmRewrite.SelectedValue = SiteUrls.GetEnableHtmRewrite();
			}
		}
		private void LoadSiteContent(Hidistro.Membership.Context.SiteSettings siteSettings)
		{
			this.txtSiteName.Text = siteSettings.SiteName;
			this.txtDomainName.Text = siteSettings.SiteUrl;
			this.imgLogo.ImageUrl = siteSettings.LogoUrl;
			if (!string.IsNullOrEmpty(siteSettings.LogoUrl))
			{
				this.btnDeleteLogo.Visible = true;
			}
			else
			{
				this.btnDeleteLogo.Visible = false;
			}
            this.fcOnLineServer.Text = siteSettings.HtmlOnlineServiceCode;
			this.fkFooter.Text=siteSettings.Footer;
			this.fckRegisterAgreement.Text=siteSettings.RegisterAgreement;
			this.txtkeycode.Text = siteSettings.CheckCode;
			this.dropBitNumber.SelectedValue = siteSettings.DecimalLength;
			this.txtNamePrice.Text = siteSettings.YourPriceName;
			this.uploader1.UploadedImageUrl = siteSettings.DefaultProductImage;
			this.radiIsOpenSiteSale.SelectedValue = siteSettings.IsOpenSiteSale;
		}
		private void btnUpoad_Click(object sender, System.EventArgs e)
		{
			Hidistro.Membership.Context.SiteSettings masterSettings = Hidistro.Membership.Context.SettingsManager.GetMasterSettings(false);
			if (this.fileUpload.HasFile)
			{
				try
				{
					masterSettings.LogoUrl = StoreHelper.UploadLogo(this.fileUpload.PostedFile);
				}
				catch
				{
					this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
					return;
				}
			}
			Hidistro.Membership.Context.SettingsManager.Save(masterSettings);
			this.LoadSiteContent(masterSettings);
		}
		private void btnDeleteLogo_Click(object sender, System.EventArgs e)
		{
			Hidistro.Membership.Context.SiteSettings masterSettings = Hidistro.Membership.Context.SettingsManager.GetMasterSettings(false);
			try
			{
				StoreHelper.DeleteImage(masterSettings.LogoUrl);
			}
			catch
			{
			}
			masterSettings.LogoUrl = string.Empty;
			if (!this.ValidationSettings(masterSettings))
			{
				return;
			}
			Hidistro.Membership.Context.SettingsManager.Save(masterSettings);
			this.LoadSiteContent(masterSettings);
		}
		protected void btnkey_Click(object sender, System.EventArgs e)
		{
			this.txtkeycode.Text = this.CreateKey(20);
			this.btnOK_Click(sender, e);
		}
		private void btnOK_Click(object sender, System.EventArgs e)
		{
			Hidistro.Membership.Context.SiteSettings masterSettings = Hidistro.Membership.Context.SettingsManager.GetMasterSettings(false);
			masterSettings.SiteName = this.txtSiteName.Text.Trim();
			masterSettings.SiteUrl = this.txtDomainName.Text.Trim();
			masterSettings.HtmlOnlineServiceCode = this.fcOnLineServer.Text;
			masterSettings.Footer = this.fkFooter.Text;
			masterSettings.RegisterAgreement = this.fckRegisterAgreement.Text;
			masterSettings.DefaultProductImage = this.uploader1.UploadedImageUrl;
			masterSettings.DecimalLength = this.dropBitNumber.SelectedValue;
			if (this.txtNamePrice.Text.Length <= 20)
			{
				masterSettings.YourPriceName = this.txtNamePrice.Text;
			}
			masterSettings.DefaultProductImage = this.uploader1.UploadedImageUrl;
			masterSettings.DefaultProductThumbnail1 = this.uploader1.ThumbnailUrl40;
			masterSettings.DefaultProductThumbnail2 = this.uploader1.ThumbnailUrl60;
			masterSettings.DefaultProductThumbnail3 = this.uploader1.ThumbnailUrl100;
			masterSettings.DefaultProductThumbnail4 = this.uploader1.ThumbnailUrl160;
			masterSettings.DefaultProductThumbnail5 = this.uploader1.ThumbnailUrl180;
			masterSettings.DefaultProductThumbnail6 = this.uploader1.ThumbnailUrl220;
			masterSettings.DefaultProductThumbnail7 = this.uploader1.ThumbnailUrl310;
			masterSettings.DefaultProductThumbnail8 = this.uploader1.ThumbnailUrl410;
			masterSettings.CheckCode = this.txtkeycode.Text;
			masterSettings.IsOpenSiteSale = this.radiIsOpenSiteSale.SelectedValue;
			masterSettings.CheckCode = this.txtkeycode.Text.Trim();
			if (!this.ValidationSettings(masterSettings))
			{
				return;
			}
			Globals.EntityCoding(masterSettings, true);
			Hidistro.Membership.Context.SettingsManager.Save(masterSettings);
			if (this.radEnableHtmRewrite.SelectedValue != SiteUrls.GetEnableHtmRewrite())
			{
				if (this.radEnableHtmRewrite.SelectedValue)
				{
					SiteUrls.EnableHtmRewrite();
				}
				else
				{
					SiteUrls.CloseHtmRewrite();
				}
			}
			this.ShowMsg("成功修改了店铺基本信息", true);
			this.LoadSiteContent(masterSettings);
		}
		private string CreateKey(int len)
		{
			byte[] array = new byte[len];
			new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(array);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(string.Format("{0:X2}", array[i]));
			}
			return stringBuilder.ToString();
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
