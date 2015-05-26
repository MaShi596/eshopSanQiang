using Hidistro.Entities.Commodities;
using Hidistro.Subsites.Commodities;
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
	public class AddMyCategory : DistributorPage
	{
		protected System.Web.UI.WebControls.TextBox txtCategoryName;
		protected DistributorProductCategoriesDropDownList dropCategories;
		protected ProductTypeDownList dropProductTypes;
		protected System.Web.UI.WebControls.FileUpload fileUpload;
		protected System.Web.UI.HtmlControls.HtmlGenericControl liURL;
		protected System.Web.UI.WebControls.TextBox txtRewriteName;
		protected System.Web.UI.WebControls.TextBox txtPageKeyTitle;
		protected System.Web.UI.WebControls.TextBox txtPageKeyWords;
		protected System.Web.UI.WebControls.TextBox txtPageDesc;
		protected KindeditorControl fckNotes1;
		protected KindeditorControl fckNotes2;
		protected KindeditorControl fckNotes3;
		protected System.Web.UI.WebControls.Button btnSaveCategory;
		protected System.Web.UI.WebControls.Button btnSaveAddCategory;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSaveCategory.Click += new System.EventHandler(this.btnSaveCategory_Click);
			this.btnSaveAddCategory.Click += new System.EventHandler(this.btnSaveAddCategory_Click);
			if (!this.Page.IsPostBack)
			{
				this.dropCategories.DataBind();
				this.dropProductTypes.DataBind();
			}
		}
		private CategoryInfo GetCategory()
		{
			CategoryInfo categoryInfo = new CategoryInfo();
			string icon = string.Empty;
			if (this.fileUpload.HasFile)
			{
				CategoryInfo result;
				try
				{
					icon = SubsiteCatalogHelper.UploadCategoryIcon(this.fileUpload.PostedFile);
					goto IL_42;
				}
				catch
				{
					this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
					result = null;
				}
				return result;
			}
			IL_42:
			categoryInfo.Icon = icon;
			categoryInfo.Name = this.txtCategoryName.Text.Trim();
			categoryInfo.ParentCategoryId = this.dropCategories.SelectedValue;
			categoryInfo.AssociatedProductType = this.dropProductTypes.SelectedValue;
			if (!string.IsNullOrEmpty(this.txtRewriteName.Text.Trim()))
			{
				categoryInfo.RewriteName = this.txtRewriteName.Text.Trim();
			}
			else
			{
				categoryInfo.RewriteName = null;
			}
			categoryInfo.MetaTitle = this.txtPageKeyTitle.Text.Trim();
			categoryInfo.MetaKeywords = this.txtPageKeyWords.Text.Trim();
			categoryInfo.MetaDescription = this.txtPageDesc.Text.Trim();
			categoryInfo.Notes1 = this.fckNotes1.Text;
			categoryInfo.Notes2 = this.fckNotes2.Text;
			categoryInfo.Notes3 = this.fckNotes3.Text;
			categoryInfo.DisplaySequence = 0;
			if (categoryInfo.ParentCategoryId.HasValue)
			{
				CategoryInfo category = SubsiteCatalogHelper.GetCategory(categoryInfo.ParentCategoryId.Value);
				if (category == null || category.Depth >= 5)
				{
					this.ShowMsg(string.Format("您选择的上级分类有误，商品分类最多只支持{0}级分类", 5), false);
					return null;
				}
				if (string.IsNullOrEmpty(categoryInfo.Notes1))
				{
					categoryInfo.Notes1 = category.Notes1;
				}
				if (string.IsNullOrEmpty(categoryInfo.Notes2))
				{
					categoryInfo.Notes2 = category.Notes2;
				}
				if (string.IsNullOrEmpty(categoryInfo.Notes3))
				{
					categoryInfo.Notes3 = category.Notes3;
				}
				if (string.IsNullOrEmpty(categoryInfo.RewriteName))
				{
					categoryInfo.RewriteName = category.RewriteName;
				}
			}
			ValidationResults validationResults = Validation.Validate<CategoryInfo>(categoryInfo, new string[]
			{
				"ValCategory"
			});
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(current.Message);
				}
				this.ShowMsg(text, false);
				return null;
			}
			return categoryInfo;
		}
		private void btnSaveAddCategory_Click(object sender, System.EventArgs e)
		{
			CategoryInfo category = this.GetCategory();
			if (category == null)
			{
				return;
			}
			if (SubsiteCatalogHelper.AddCategory(category) == CategoryActionStatus.Success)
			{
				this.ShowMsg("成功添加了商品分类", true);
				this.dropCategories.DataBind();
				this.dropProductTypes.DataBind();
				this.txtCategoryName.Text = string.Empty;
				this.txtRewriteName.Text = string.Empty;
				this.txtPageKeyTitle.Text = string.Empty;
				this.txtPageKeyWords.Text = string.Empty;
				this.txtPageDesc.Text = string.Empty;
                this.fckNotes1.Text = string.Empty;
                this.fckNotes2.Text = string.Empty;
                this.fckNotes3.Text = string.Empty;
				return;
			}
			this.ShowMsg("添加商品分类失败,未知错误", false);
		}
		private void btnSaveCategory_Click(object sender, System.EventArgs e)
		{
			CategoryInfo category = this.GetCategory();
			if (category == null)
			{
				return;
			}
			ValidationResults validationResults = Validation.Validate<CategoryInfo>(category, new string[]
			{
				"ValCategory"
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
			if (SubsiteCatalogHelper.AddCategory(category) == CategoryActionStatus.Success)
			{
				base.Response.Redirect("ManageMyCategories.aspx", true);
				return;
			}
			this.ShowMsg("添加商品分类失败,未知错误", false);
		}
	}
}
