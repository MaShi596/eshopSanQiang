using Hidistro.Core;
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
	public class EditMyCategory : DistributorPage
	{
		protected System.Web.UI.WebControls.TextBox txtCategoryName;
		protected ProductTypeDownList dropProductTypes;
		protected System.Web.UI.WebControls.FileUpload fileUpload;
		protected HiImage imgPic;
		protected ImageLinkButton btnPicDelete;
		protected System.Web.UI.HtmlControls.HtmlGenericControl liRewriteName;
		protected System.Web.UI.WebControls.TextBox txtRewriteName;
		protected System.Web.UI.WebControls.TextBox txtPageKeyTitle;
		protected System.Web.UI.WebControls.TextBox txtPageKeyWords;
		protected System.Web.UI.WebControls.TextBox txtPageDesc;
		protected KindeditorControl fckNotes1;
		protected KindeditorControl fckNotes2;
		protected KindeditorControl fckNotes3;
		protected System.Web.UI.WebControls.Button btnSaveCategory;
		private int categoryId;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["categoryId"], out this.categoryId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.btnSaveCategory.Click += new System.EventHandler(this.btnSaveCategory_Click);
			
            //this.btnPicDelete.Click += new System.EventHandler(this.btnPicDelete_Click);
			



            if (!this.Page.IsPostBack)
			{
				CategoryInfo category = SubsiteCatalogHelper.GetCategory(this.categoryId);
				this.dropProductTypes.DataBind();
				this.dropProductTypes.SelectedValue = category.AssociatedProductType;
				if (category == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				Globals.EntityCoding(category, false);
				this.BindCategoryInfo(category);
				if (category.Depth > 1)
				{
					this.liRewriteName.Style.Add("display", "none");
				}
			}
		}
		private void btnPicDelete_Click(object sender, System.EventArgs e)
		{
			CategoryInfo category = SubsiteCatalogHelper.GetCategory(this.categoryId);
			try
			{
				ResourcesHelper.DeleteImage(category.Icon);
			}
			catch
			{
			}
			category.Icon = (this.imgPic.ImageUrl = null);
			if (SubsiteCatalogHelper.UpdateCategory(category) == CategoryActionStatus.Success)
			{
				this.btnPicDelete.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
				this.imgPic.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
			}
		}
		private void btnSaveCategory_Click(object sender, System.EventArgs e)
		{
			CategoryInfo category = SubsiteCatalogHelper.GetCategory(this.categoryId);
			if (category == null)
			{
				this.ShowMsg("编缉商品分类错误,未知", false);
				return;
			}
            //if (this.fileUpload.HasFile)
            //{
            //    try
            //    {
            //        ResourcesHelper.DeleteImage(category.Icon);
            //        category.Icon = SubsiteCatalogHelper.UploadCategoryIcon(this.fileUpload.PostedFile);
            //    }
            //    catch
            //    {
            //        this.ShowMsg("图片上传失败，您选择的不是图片类型的文件，或者网站的虚拟目录没有写入文件的权限", false);
            //        return;
            //    }
            //}
			category.AssociatedProductType = this.dropProductTypes.SelectedValue;
			category.Name = this.txtCategoryName.Text;
			category.RewriteName = this.txtRewriteName.Text;
			category.MetaTitle = this.txtPageKeyTitle.Text;
			category.MetaKeywords = this.txtPageKeyWords.Text;
			category.MetaDescription = this.txtPageDesc.Text;
			category.Notes1 = this.fckNotes1.Text;
			category.Notes2 = this.fckNotes2.Text;
			category.Notes3 = this.fckNotes3.Text;
			if (category.Depth > 1)
			{
				CategoryInfo category2 = SubsiteCatalogHelper.GetCategory(category.ParentCategoryId.Value);
				if (string.IsNullOrEmpty(category.Notes1))
				{
					category.Notes1 = category2.Notes1;
				}
				if (string.IsNullOrEmpty(category.Notes2))
				{
					category.Notes2 = category2.Notes2;
				}
				if (string.IsNullOrEmpty(category.Notes3))
				{
					category.Notes3 = category2.Notes3;
				}
				if (string.IsNullOrEmpty(category.Notes4))
				{
					category.Notes4 = category2.Notes4;
				}
				if (string.IsNullOrEmpty(category.Notes5))
				{
					category.Notes5 = category2.Notes5;
				}
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
			}
			else
			{
				CategoryActionStatus categoryActionStatus = SubsiteCatalogHelper.UpdateCategory(category);
				if (categoryActionStatus == CategoryActionStatus.Success)
				{
					this.ShowMsg("成功修改了当前商品分类", true);
					this.BindCategoryInfo(category);
					return;
				}
				if (categoryActionStatus == CategoryActionStatus.UpdateParentError)
				{
					this.ShowMsg("不能自己成为自己的上级分类", false);
					return;
				}
				this.ShowMsg("编缉商品分类错误,未知", false);
				return;
			}
		}
		private void BindCategoryInfo(CategoryInfo categoryInfo)
		{
			if (categoryInfo != null)
			{
				this.txtCategoryName.Text = categoryInfo.Name;
				this.dropProductTypes.SelectedValue = categoryInfo.AssociatedProductType;
				this.txtRewriteName.Text = categoryInfo.RewriteName;
				this.txtPageKeyTitle.Text = categoryInfo.MetaTitle;
				this.txtPageKeyWords.Text = categoryInfo.MetaKeywords;
				this.txtPageDesc.Text = categoryInfo.MetaDescription;
                this.fckNotes1.Text = categoryInfo.Notes1;
                this.fckNotes2.Text = categoryInfo.Notes2;
                this.fckNotes3.Text = categoryInfo.Notes3;
				//this.imgPic.ImageUrl = categoryInfo.Icon;
				//this.btnPicDelete.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
				//this.imgPic.Visible = !string.IsNullOrEmpty(this.imgPic.ImageUrl);
			}
		}
	}
}
