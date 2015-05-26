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
	[PrivilegeCheck(Privilege.ArticleCategories)]
	public class EditArticleCategory : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtArticleCategoryiesName;
		protected System.Web.UI.WebControls.FileUpload fileUpload;
		protected HiImage imgPic;
		protected ImageLinkButton btnPicDelete;
		protected System.Web.UI.WebControls.TextBox txtArticleCategoryiesDesc;
		protected System.Web.UI.WebControls.Button btnSubmitArticleCategory;
		private int categoryId;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSubmitArticleCategory.Click += new System.EventHandler(this.btnSubmitArticleCategory_Click);
			this.btnPicDelete.Click += new System.EventHandler(this.btnPicDelete_Click);
			if (!int.TryParse(base.Request.QueryString["CategoryId"], out this.categoryId))
			{
				base.GotoResourceNotFound();
				return;
			}
			if (!base.IsPostBack)
			{
				ArticleCategoryInfo articleCategory = ArticleHelper.GetArticleCategory(this.categoryId);
				if (articleCategory == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				Globals.EntityCoding(articleCategory, false);
				this.txtArticleCategoryiesName.Text = articleCategory.Name;
				this.txtArticleCategoryiesDesc.Text = articleCategory.Description;
				this.imgPic.ImageUrl = articleCategory.IconUrl;
				this.btnPicDelete.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
				this.imgPic.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
			}
		}
		private void btnPicDelete_Click(object sender, System.EventArgs e)
		{
			ArticleCategoryInfo articleCategory = ArticleHelper.GetArticleCategory(this.categoryId);
			try
			{
				ResourcesHelper.DeleteImage(articleCategory.IconUrl);
			}
			catch
			{
			}
			articleCategory.IconUrl = (this.imgPic.ImageUrl = null);
			if (ArticleHelper.UpdateArticleCategory(articleCategory))
			{
				this.btnPicDelete.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
				this.imgPic.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
			}
		}
		private void btnSubmitArticleCategory_Click(object sender, System.EventArgs e)
		{
			ArticleCategoryInfo articleCategory = ArticleHelper.GetArticleCategory(this.categoryId);
			if (articleCategory == null)
			{
				return;
			}
			if (this.fileUpload.HasFile)
			{
				try
				{
					ResourcesHelper.DeleteImage(articleCategory.IconUrl);
					articleCategory.IconUrl = ArticleHelper.UploadArticleImage(this.fileUpload.PostedFile);
				}
				catch
				{
					this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
					return;
				}
			}
			articleCategory.Name = this.txtArticleCategoryiesName.Text.Trim();
			articleCategory.Description = this.txtArticleCategoryiesDesc.Text.Trim();
			ValidationResults validationResults = Validation.Validate<ArticleCategoryInfo>(articleCategory, new string[]
			{
				"ValArticleCategoryInfo"
			});
			string text = string.Empty;
			if (validationResults.IsValid)
			{
				this.UpdateCategory(articleCategory);
				return;
			}
			foreach (ValidationResult current in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
			{
				text += Formatter.FormatErrorMessage(current.Message);
			}
			this.ShowMsg(text, false);
		}
		private void UpdateCategory(ArticleCategoryInfo category)
		{
			if (ArticleHelper.UpdateArticleCategory(category))
			{
				this.imgPic.ImageUrl = null;
				this.CloseWindow();
			}
			else
			{
				this.ShowMsg("未知错误", false);
			}
			this.btnPicDelete.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
			this.imgPic.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
		}
	}
}
