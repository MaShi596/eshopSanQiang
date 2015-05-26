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
	public class AddMyArticleCategory : DistributorPage
	{
		protected System.Web.UI.WebControls.TextBox txtArticleCategoryiesName;
		protected System.Web.UI.WebControls.FileUpload fileUpload;
		protected System.Web.UI.WebControls.TextBox txtArticleCategoryiesDesc;
		protected System.Web.UI.WebControls.Button btnSubmitArticleCategory;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSubmitArticleCategory.Click += new System.EventHandler(this.btnSubmitArticleCategory_Click);
		}
		private void btnSubmitArticleCategory_Click(object sender, System.EventArgs e)
		{
			string iconUrl = string.Empty;
			if (this.fileUpload.HasFile)
			{
				try
				{
					iconUrl = SubsiteCommentsHelper.UploadArticleImage(this.fileUpload.PostedFile);
				}
				catch
				{
					this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
					return;
				}
			}
			ArticleCategoryInfo articleCategoryInfo = new ArticleCategoryInfo();
			articleCategoryInfo.Name = this.txtArticleCategoryiesName.Text.Trim();
			articleCategoryInfo.IconUrl = iconUrl;
			articleCategoryInfo.Description = this.txtArticleCategoryiesDesc.Text.Trim();
			ValidationResults validationResults = Validation.Validate<ArticleCategoryInfo>(articleCategoryInfo, new string[]
			{
				"ValArticleCategoryInfo"
			});
			string text = string.Empty;
			if (validationResults.IsValid)
			{
				this.AddNewCategory(articleCategoryInfo);
				return;
			}
			foreach (ValidationResult current in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
			{
				text += Formatter.FormatErrorMessage(current.Message);
			}
			this.ShowMsg(text, false);
		}
		private void AddNewCategory(ArticleCategoryInfo category)
		{
			if (SubsiteCommentsHelper.CreateArticleCategory(category))
			{
				this.Reset();
				this.ShowMsg("成功添加了一个文章分类", true);
				return;
			}
			this.ShowMsg("未知错误", false);
		}
		private void Reset()
		{
			this.txtArticleCategoryiesName.Text = string.Empty;
			this.txtArticleCategoryiesDesc.Text = string.Empty;
		}
	}
}
