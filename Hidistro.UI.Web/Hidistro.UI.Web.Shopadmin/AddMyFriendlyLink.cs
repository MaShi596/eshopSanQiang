using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.Subsites.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class AddMyFriendlyLink : DistributorPage
	{
		protected System.Web.UI.WebControls.TextBox txtaddTitle;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtaddTitleTip;
		protected System.Web.UI.WebControls.FileUpload uploadImageUrl;
		protected System.Web.UI.WebControls.TextBox txtaddLinkUrl;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtaddLinkUrlTip;
		protected YesNoRadioButtonList radioShowLinks;
		protected System.Web.UI.WebControls.Button btnSubmitLinks;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSubmitLinks.Click += new System.EventHandler(this.btnSubmitLinks_Click);
		}
		private void btnSubmitLinks_Click(object sender, System.EventArgs e)
		{
			string imageUrl = string.Empty;
			if (!this.uploadImageUrl.HasFile && string.IsNullOrEmpty(this.txtaddTitle.Text.Trim()))
			{
				this.ShowMsg("友情链接Logo和网站名称不能同时为空", false);
				return;
			}
			FriendlyLinksInfo friendlyLinksInfo = new FriendlyLinksInfo();
			if (this.uploadImageUrl.HasFile)
			{
				try
				{
					imageUrl = SubsiteStoreHelper.UploadLinkImage(this.uploadImageUrl.PostedFile);
				}
				catch
				{
					this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
					return;
				}
			}
			friendlyLinksInfo.ImageUrl = imageUrl;
			friendlyLinksInfo.LinkUrl = this.txtaddLinkUrl.Text;
			friendlyLinksInfo.Title = Globals.HtmlEncode(this.txtaddTitle.Text.Trim());
			friendlyLinksInfo.Visible = this.radioShowLinks.SelectedValue;
			ValidationResults validationResults = Validation.Validate<FriendlyLinksInfo>(friendlyLinksInfo, new string[]
			{
				"ValFriendlyLinksInfo"
			});
			string text = string.Empty;
			if (validationResults.IsValid)
			{
				this.AddNewFriendlyLink(friendlyLinksInfo);
				this.Reset();
				return;
			}
			foreach (ValidationResult current in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
			{
				text += Formatter.FormatErrorMessage(current.Message);
			}
			this.ShowMsg(text, false);
		}
		private void AddNewFriendlyLink(FriendlyLinksInfo friendlyLink)
		{
			if (SubsiteStoreHelper.CreateFriendlyLink(friendlyLink))
			{
				this.ShowMsg("成功添加了一个友情链接", true);
				return;
			}
			this.ShowMsg("未知错误", false);
		}
		private void Reset()
		{
			this.txtaddTitle.Text = string.Empty;
			this.radioShowLinks.SelectedValue = true;
			this.txtaddLinkUrl.Text = string.Empty;
		}
	}
}
