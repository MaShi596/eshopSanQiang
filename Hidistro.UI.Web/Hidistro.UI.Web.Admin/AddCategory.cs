using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
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
	[PrivilegeCheck(Privilege.AddProductCategory)]
	public class AddCategory : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtCategoryName;
		protected ProductCategoriesDropDownList dropCategories;
		protected ProductTypeDownList dropProductTypes;
		protected System.Web.UI.WebControls.FileUpload fileUpload;
		protected System.Web.UI.WebControls.TextBox txtSKUPrefix;
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
			if (!string.IsNullOrEmpty(base.Request["isCallback"]) && base.Request["isCallback"] == "true")
			{
				int categoryId = 0;
				int.TryParse(base.Request["parentCategoryId"], out categoryId);
				CategoryInfo category = CatalogHelper.GetCategory(categoryId);
				if (category != null)
				{
					base.Response.Clear();
					base.Response.ContentType = "application/json";
					base.Response.Write("{ ");
					base.Response.Write(string.Format("\"SKUPrefix\":\"{0}\"", category.SKUPrefix));
					base.Response.Write("}");
					base.Response.End();
				}
			}
			if (!this.Page.IsPostBack)
			{
				this.dropCategories.DataBind();
				this.dropProductTypes.DataBind();
			}
		}
		private void btnSaveCategory_Click(object sender, System.EventArgs e)
		{
			CategoryInfo category = this.GetCategory();
			if (category == null)
			{
				return;
			}
			if (CatalogHelper.AddCategory(category) == CategoryActionStatus.Success)
			{
				base.Response.Redirect(Globals.GetAdminAbsolutePath("/product/ManageCategories.aspx"), true);
				return;
			}
			this.ShowMsg("添加商品分类失败,未知错误", false);
		}
		private void btnSaveAddCategory_Click(object sender, System.EventArgs e)
		{
			CategoryInfo category = this.GetCategory();
			if (category == null)
			{
				return;
			}
			if (CatalogHelper.AddCategory(category) == CategoryActionStatus.Success)
			{
				this.ShowMsg("成功添加了商品分类", true);
				this.dropCategories.DataBind();
				this.dropProductTypes.DataBind();
				this.txtCategoryName.Text = string.Empty;
				this.txtSKUPrefix.Text = string.Empty;
				this.txtRewriteName.Text = string.Empty;
				this.txtPageKeyTitle.Text = string.Empty;
				this.txtPageKeyWords.Text = string.Empty;
				this.txtPageDesc.Text = string.Empty;
				this.fckNotes1.Text=string.Empty;
				this.fckNotes2.Text=string.Empty;
                this.fckNotes3.Text = string.Empty;
				return;
			}
			this.ShowMsg("添加商品分类失败,未知错误", false);
		}
		private CategoryInfo GetCategory()
		{
			CategoryInfo categoryInfo = new CategoryInfo();
			categoryInfo.Name = this.txtCategoryName.Text.Trim();
			categoryInfo.ParentCategoryId = this.dropCategories.SelectedValue;
			categoryInfo.SKUPrefix = this.txtSKUPrefix.Text.Trim();
			categoryInfo.AssociatedProductType = this.dropProductTypes.SelectedValue;
			if (!string.IsNullOrEmpty(this.txtRewriteName.Text.Trim()))
			{
				categoryInfo.RewriteName = this.txtRewriteName.Text.Trim();
			}
			else
			{
				categoryInfo.RewriteName = null;
			}
			string icon = string.Empty;
			if (this.fileUpload.HasFile)
			{
				CategoryInfo result;
				try
				{
					icon = CatalogHelper.UploadCategoryIcon(this.fileUpload.PostedFile);
					goto IL_C8;
				}
				catch
				{
					this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
					result = null;
				}
				return result;
				IL_C8:
				categoryInfo.Icon = icon;
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
				CategoryInfo category = CatalogHelper.GetCategory(categoryInfo.ParentCategoryId.Value);
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
	}
}
