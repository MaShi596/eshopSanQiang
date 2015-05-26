using Hidistro.ControlPanel.Comments;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Articles)]
	public class EditArticle : AdminPage
	{
		private int articleId;
		protected ArticleCategoryDropDownList dropArticleCategory;
		protected System.Web.UI.WebControls.TextBox txtArticleTitle;
		protected System.Web.UI.HtmlControls.HtmlInputCheckBox ckrrelease;
		protected TrimTextBox txtMetaDescription;
		protected TrimTextBox txtMetaKeywords;
		protected System.Web.UI.WebControls.FileUpload fileUpload;
		protected HiImage imgPic;
		protected ImageLinkButton btnPicDelete;
		protected System.Web.UI.WebControls.TextBox txtShortDesc;
		protected KindeditorControl fcContent;
		protected System.Web.UI.WebControls.Button btnAddArticle;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["articleId"], out this.articleId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.btnAddArticle.Click += new System.EventHandler(this.btnAddArticle_Click);
			this.btnPicDelete.Click += new System.EventHandler(this.btnPicDelete_Click);
			if (!this.Page.IsPostBack)
			{
				this.dropArticleCategory.DataBind();
				ArticleInfo article = ArticleHelper.GetArticle(this.articleId);
				if (article == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				Globals.EntityCoding(article, false);
				this.txtArticleTitle.Text = article.Title;
				this.txtMetaDescription.Text = article.MetaDescription;
				this.txtMetaKeywords.Text = article.MetaKeywords;
				this.imgPic.ImageUrl = article.IconUrl;
				this.txtShortDesc.Text = article.Description;
                this.fcContent.Text = article.Content;
				this.dropArticleCategory.SelectedValue = new int?(article.CategoryId);
				this.btnPicDelete.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
				this.ckrrelease.Checked = article.IsRelease;
			}
		}
		private void btnAddArticle_Click(object sender, System.EventArgs e)
		{
			if (!this.dropArticleCategory.SelectedValue.HasValue)
			{
				this.ShowMsg("请选择文章分类", false);
				return;
			}
			ArticleInfo article = ArticleHelper.GetArticle(this.articleId);
			if (this.fileUpload.HasFile)
			{
				try
				{
					ResourcesHelper.DeleteImage(article.IconUrl);
					article.IconUrl = ArticleHelper.UploadArticleImage(this.fileUpload.PostedFile);
					this.imgPic.ImageUrl = article.IconUrl;
				}
				catch
				{
					this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
					return;
				}
			}
			article.ArticleId = this.articleId;
			article.CategoryId = this.dropArticleCategory.SelectedValue.Value;
			article.Title = this.txtArticleTitle.Text.Trim();
			article.MetaDescription = this.txtMetaDescription.Text.Trim();
			article.MetaKeywords = this.txtMetaKeywords.Text.Trim();
			article.Description = this.txtShortDesc.Text.Trim();
			article.Content = this.fcContent.Text;
			article.AddedDate = System.DateTime.Now;
			article.IsRelease = this.ckrrelease.Checked;
			ValidationResults validationResults = Validation.Validate<ArticleInfo>(article, new string[]
			{
				"ValArticleInfo"
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
			else
			{
				if (ArticleHelper.UpdateArticle(article))
				{
					this.ShowMsg("已经成功修改当前文章", true);
					return;
				}
				this.ShowMsg("修改文章失败", false);
				return;
			}
		}
		private void btnPicDelete_Click(object sender, System.EventArgs e)
		{
			ArticleInfo article = ArticleHelper.GetArticle(this.articleId);
			try
			{
				ResourcesHelper.DeleteImage(article.IconUrl);
			}
			catch
			{
			}
			article.IconUrl = (this.imgPic.ImageUrl = null);
			if (ArticleHelper.UpdateArticle(article))
			{
				this.btnPicDelete.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
				this.imgPic.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
			}
		}
	}
}
