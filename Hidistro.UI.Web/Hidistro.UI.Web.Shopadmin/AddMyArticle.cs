using Hidistro.Entities.Comments;
using Hidistro.Subsites.Comments;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class AddMyArticle : DistributorPage
	{
		protected DistributorArticleCategoryDropDownList dropArticleCategory;
		protected System.Web.UI.WebControls.TextBox txtArticleTitle;
		protected System.Web.UI.HtmlControls.HtmlInputCheckBox ckrrelease;
		protected TrimTextBox txtMetaDescription;
		protected TrimTextBox txtMetaKeywords;
		protected System.Web.UI.WebControls.FileUpload fileUpload;
		protected System.Web.UI.WebControls.TextBox txtShortDesc;
		protected KindeditorControl fcContent;
		protected System.Web.UI.WebControls.Button btnAddArticle;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnAddArticle.Click += new System.EventHandler(this.btnAddArticle_Click);
			if (!this.Page.IsPostBack)
			{
				this.dropArticleCategory.DataBind();
				if (!string.IsNullOrEmpty(this.Page.Request.QueryString["categoryId"]))
				{
					int value = 0;
					if (int.TryParse(this.Page.Request.QueryString["categoryId"], out value))
					{
						this.dropArticleCategory.SelectedValue = new int?(value);
					}
				}
			}
		}
		private void btnAddArticle_Click(object sender, System.EventArgs e)
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
			ArticleInfo articleInfo = new ArticleInfo();
			if (!this.dropArticleCategory.SelectedValue.HasValue)
			{
				this.ShowMsg("请选择文章分类", false);
			}
			else
			{
				articleInfo.CategoryId = this.dropArticleCategory.SelectedValue.Value;
				articleInfo.Title = this.txtArticleTitle.Text.Trim();
				articleInfo.MetaDescription = this.txtMetaDescription.Text.Trim();
				articleInfo.MetaKeywords = this.txtMetaKeywords.Text.Trim();
				articleInfo.IconUrl = iconUrl;
				articleInfo.Description = this.txtShortDesc.Text.Trim();
				articleInfo.Content = this.fcContent.Text;
				articleInfo.AddedDate = System.DateTime.Now;
				articleInfo.IsRelease = this.ckrrelease.Checked;
				ValidationResults validationResults = Validation.Validate<ArticleInfo>(articleInfo, new string[]
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
					return;
				}
				if (SubsiteCommentsHelper.CreateArticle(articleInfo))
				{
					this.txtArticleTitle.Text = string.Empty;
					this.txtShortDesc.Text = string.Empty;
                    this.fcContent.Text = string.Empty;
					this.ShowMsg("成功添加了一篇文章", true);
					return;
				}
				this.ShowMsg("添加文章错误", false);
				return;
			}
		}
	}
}
