using Hidistro.ControlPanel.Comments;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.HelpCategories)]
	public class EditHelpCategory : AdminPage
	{
		private int categoryId;
		protected System.Web.UI.WebControls.TextBox txtHelpCategoryName;
		protected System.Web.UI.WebControls.FileUpload fileUpload;
		protected HiImage imgPic;
		protected ImageLinkButton btnPicDelete;
		protected YesNoRadioButtonList radioShowFooter;
		protected System.Web.UI.WebControls.TextBox txtHelpCategoryDesc;
		protected System.Web.UI.WebControls.Button btnSubmitHelpCategory;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSubmitHelpCategory.Click += new System.EventHandler(this.btnSubmitHelpCategory_Click);
			this.btnPicDelete.Click += new System.EventHandler(this.btnPicDelete_Click);
			if (!int.TryParse(base.Request.QueryString["CategoryId"], out this.categoryId))
			{
				base.GotoResourceNotFound();
				return;
			}
			if (!base.IsPostBack)
			{
				HelpCategoryInfo helpCategory = ArticleHelper.GetHelpCategory(this.categoryId);
				if (helpCategory == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				Globals.EntityCoding(helpCategory, false);
				this.txtHelpCategoryName.Text = helpCategory.Name;
				this.txtHelpCategoryDesc.Text = helpCategory.Description;
				this.radioShowFooter.SelectedValue = helpCategory.IsShowFooter;
				this.imgPic.ImageUrl = helpCategory.IconUrl;
				this.imgPic.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
				this.btnPicDelete.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
			}
		}
		private void btnSubmitHelpCategory_Click(object sender, System.EventArgs e)
		{
			string text = string.Empty;
			text = ArticleHelper.GetHelpCategory(this.categoryId).IconUrl;
			if (this.fileUpload.HasFile)
			{
				try
				{
					if (!string.IsNullOrEmpty(text))
					{
						ResourcesHelper.DeleteImage(text);
					}
					text = ArticleHelper.UploadHelpImage(this.fileUpload.PostedFile);
				}
				catch
				{
					this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
					return;
				}
			}
			HelpCategoryInfo helpCategoryInfo = new HelpCategoryInfo();
			helpCategoryInfo.CategoryId = new int?(this.categoryId);
			helpCategoryInfo.Name = this.txtHelpCategoryName.Text.Trim();
			helpCategoryInfo.IconUrl = text;
			helpCategoryInfo.Description = this.txtHelpCategoryDesc.Text.Trim();
			helpCategoryInfo.IsShowFooter = this.radioShowFooter.SelectedValue;
			ValidationResults validationResults = Validation.Validate<HelpCategoryInfo>(helpCategoryInfo, new string[]
			{
				"ValHelpCategoryInfo"
			});
			string text2 = string.Empty;
			if (validationResults.IsValid)
			{
				this.UpdateCategory(helpCategoryInfo);
				return;
			}
			foreach (ValidationResult current in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
			{
				text2 += Formatter.FormatErrorMessage(current.Message);
			}
			this.ShowMsg(text2, false);
		}
		private void UpdateCategory(HelpCategoryInfo category)
		{
			if (ArticleHelper.UpdateHelpCategory(category))
			{
				this.imgPic.ImageUrl = null;
				this.imgPic.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
				this.btnPicDelete.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
				this.CloseWindow();
				return;
			}
			this.ShowMsg("操作失败,未知错误", false);
		}
		private void btnPicDelete_Click(object sender, System.EventArgs e)
		{
			HelpCategoryInfo helpCategory = ArticleHelper.GetHelpCategory(this.categoryId);
			try
			{
				ResourcesHelper.DeleteImage(helpCategory.IconUrl);
			}
			catch
			{
			}
			helpCategory.IconUrl = (this.imgPic.ImageUrl = string.Empty);
			if (ArticleHelper.UpdateHelpCategory(helpCategory))
			{
				this.btnPicDelete.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
				this.imgPic.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
			}
		}
	}
}
