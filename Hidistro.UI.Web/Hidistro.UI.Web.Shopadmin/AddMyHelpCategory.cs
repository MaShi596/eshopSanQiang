using Hidistro.Entities.Comments;
using Hidistro.Subsites.Comments;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class AddMyHelpCategory : DistributorPage
	{
		protected System.Web.UI.WebControls.TextBox txtHelpCategoryName;
		protected System.Web.UI.WebControls.FileUpload fileUpload;
		protected YesNoRadioButtonList radioShowFooter;
		protected System.Web.UI.WebControls.TextBox txtHelpCategoryDesc;
		protected System.Web.UI.WebControls.Button btnSubmitHelpCategory;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSubmitHelpCategory.Click += new System.EventHandler(this.btnSubmitHelpCategory_Click);
		}
		private void btnSubmitHelpCategory_Click(object sender, System.EventArgs e)
		{
			string iconUrl = string.Empty;
			if (this.fileUpload.HasFile)
			{
				try
				{
					iconUrl = SubsiteCommentsHelper.UploadHelpImage(this.fileUpload.PostedFile);
				}
				catch
				{
					this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
					return;
				}
			}
			HelpCategoryInfo helpCategoryInfo = new HelpCategoryInfo();
			helpCategoryInfo.Name = this.txtHelpCategoryName.Text.Trim();
			helpCategoryInfo.IconUrl = iconUrl;
			helpCategoryInfo.Description = this.txtHelpCategoryDesc.Text.Trim();
			helpCategoryInfo.IsShowFooter = this.radioShowFooter.SelectedValue;
			ValidationResults validationResults = Validation.Validate<HelpCategoryInfo>(helpCategoryInfo, new string[]
			{
				"ValHelpCategoryInfo"
			});
			string text = string.Empty;
			if (validationResults.IsValid)
			{
				this.AddNewCategory(helpCategoryInfo);
				this.Reset();
				return;
			}
			foreach (ValidationResult current in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
			{
				text += Formatter.FormatErrorMessage(current.Message);
			}
			this.ShowMsg(text, false);
		}
		private void AddNewCategory(HelpCategoryInfo category)
		{
			if (SubsiteCommentsHelper.CreateHelpCategory(category))
			{
				this.ShowMsg("成功添加了一个帮助分类", true);
				return;
			}
			this.ShowMsg("操作失败,未知错误", false);
		}
		private void Reset()
		{
			this.txtHelpCategoryName.Text = string.Empty;
			this.txtHelpCategoryDesc.Text = string.Empty;
		}
	}
}
