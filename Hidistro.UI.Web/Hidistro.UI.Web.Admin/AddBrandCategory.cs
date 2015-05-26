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
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.BrandCategories)]
	public class AddBrandCategory : AdminPage
	{
		protected System.Web.UI.WebControls.TextBox txtBrandName;
		protected System.Web.UI.WebControls.FileUpload fileUpload;
		protected System.Web.UI.WebControls.TextBox txtCompanyUrl;
		protected System.Web.UI.WebControls.TextBox txtReUrl;
		protected System.Web.UI.WebControls.TextBox txtkeyword;
		protected System.Web.UI.WebControls.TextBox txtMetaDescription;
		protected KindeditorControl fckDescription;
		protected ProductTypesCheckBoxList chlistProductTypes;
		protected System.Web.UI.WebControls.Button btnSave;
		protected System.Web.UI.WebControls.Button btnAddBrandCategory;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			this.btnAddBrandCategory.Click += new System.EventHandler(this.btnAddBrandCategory_Click);
			if (!base.IsPostBack)
			{
				this.chlistProductTypes.DataBind();
			}
		}
		private void btnSave_Click(object sender, System.EventArgs e)
		{
			BrandCategoryInfo brandCategoryInfo = this.GetBrandCategoryInfo();
			if (this.fileUpload.HasFile)
			{
				try
				{
					brandCategoryInfo.Logo = CatalogHelper.UploadBrandCategorieImage(this.fileUpload.PostedFile);
				}
				catch
				{
					this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
					return;
				}
				if (!this.ValidationBrandCategory(brandCategoryInfo))
				{
					return;
				}
				if (CatalogHelper.AddBrandCategory(brandCategoryInfo))
				{
					base.Response.Redirect(Globals.GetAdminAbsolutePath("/product/BrandCategories.aspx"), true);
					return;
				}
				this.ShowMsg("添加品牌分类失败", true);
				return;
			}
			else
			{
				this.ShowMsg("请上传一张品牌logo图片", false);
			}
		}
		protected void btnAddBrandCategory_Click(object sender, System.EventArgs e)
		{
			BrandCategoryInfo brandCategoryInfo = this.GetBrandCategoryInfo();
			if (this.fileUpload.HasFile)
			{
				try
				{
					brandCategoryInfo.Logo = CatalogHelper.UploadBrandCategorieImage(this.fileUpload.PostedFile);
				}
				catch
				{
					this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
					return;
				}
				if (!this.ValidationBrandCategory(brandCategoryInfo))
				{
					return;
				}
				if (CatalogHelper.AddBrandCategory(brandCategoryInfo))
				{
					this.ShowMsg("成功添加品牌分类", true);
					return;
				}
				this.ShowMsg("添加品牌分类失败", true);
				return;
			}
			else
			{
				this.ShowMsg("请上传一张品牌logo图片", false);
			}
		}
		private BrandCategoryInfo GetBrandCategoryInfo()
		{
			BrandCategoryInfo brandCategoryInfo = new BrandCategoryInfo();
			brandCategoryInfo.BrandName = Globals.HtmlEncode(this.txtBrandName.Text.Trim());
			if (!string.IsNullOrEmpty(this.txtCompanyUrl.Text))
			{
				brandCategoryInfo.CompanyUrl = this.txtCompanyUrl.Text.Trim();
			}
			else
			{
				brandCategoryInfo.CompanyUrl = null;
			}
			brandCategoryInfo.RewriteName = Globals.HtmlEncode(this.txtReUrl.Text.Trim());
			brandCategoryInfo.MetaKeywords = Globals.HtmlEncode(this.txtkeyword.Text.Trim());
			brandCategoryInfo.MetaDescription = Globals.HtmlEncode(this.txtMetaDescription.Text.Trim());
			System.Collections.Generic.IList<int> list = new System.Collections.Generic.List<int>();
			foreach (System.Web.UI.WebControls.ListItem listItem in this.chlistProductTypes.Items)
			{
				if (listItem.Selected)
				{
					list.Add(int.Parse(listItem.Value));
				}
			}
			brandCategoryInfo.ProductTypes = list;
			brandCategoryInfo.Description = ((string.IsNullOrEmpty(this.fckDescription.Text) || this.fckDescription.Text.Length <= 0) ? null : this.fckDescription.Text);
			return brandCategoryInfo;
		}
		private bool ValidationBrandCategory(BrandCategoryInfo brandCategory)
		{
			ValidationResults validationResults = Validation.Validate<BrandCategoryInfo>(brandCategory, new string[]
			{
				"ValBrandCategory"
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
