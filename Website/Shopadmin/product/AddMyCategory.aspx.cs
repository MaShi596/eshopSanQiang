using System;
using System.Collections.Generic;
using Hidistro.Entities.Commodities;
using Hidistro.Subsites.Commodities;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace Hidistro.UI.Web.Shopadmin
{
    /// <summary>
    /// 添加分类
    /// </summary>
    public partial class AddMyCategory : DistributorPage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                dropCategories.DataBind();
                dropProductTypes.DataBind();
            }
        }

        protected void btnSaveAddCategory_Click(object sender, EventArgs e)
        {
            CategoryInfo category = GetCategory();
            if (category != null)
            {
                if (SubsiteCatalogHelper.AddCategory(category) == CategoryActionStatus.Success)
                {
                    dropCategories.DataBind();
                    dropProductTypes.DataBind();
                    txtCategoryName.Text = string.Empty;
                    txtRewriteName.Text = string.Empty;
                    txtPageKeyTitle.Text = string.Empty;
                    txtPageKeyWords.Text = string.Empty;
                    txtPageDesc.Text = string.Empty;
                    fckNotes1.Text = string.Empty;
                    fckNotes2.Text = string.Empty;
                    fckNotes3.Text = string.Empty;
                    ShowMsg("成功添加了店铺分类", true);
                }
                else
                {
                    ShowMsg("添加店铺分类失败,未知错误", false);
                }
            }
        }

        protected void btnSaveCategory_Click(object sender, EventArgs e)
        {
            CategoryInfo category = GetCategory();
            if (category != null)
            {
                ValidationResults results = Validation.Validate<CategoryInfo>(category, new string[] { "ValCategory" });
                string msg = string.Empty;
                if (!results.IsValid)
                {
                    foreach (ValidationResult result in (IEnumerable<ValidationResult>)results)
                    {
                        msg = msg + Formatter.FormatErrorMessage(result.Message);
                    }
                    ShowMsg(msg, false);
                }
                else if (SubsiteCatalogHelper.AddCategory(category) == CategoryActionStatus.Success)
                {
                    base.Response.Redirect("ManageMyCategories.aspx", true);
                }
                else
                {
                    ShowMsg("添加店铺分类失败,未知错误", false);
                }
            }
        }

        CategoryInfo GetCategory()
        {
            CategoryInfo target = new CategoryInfo();
            target.Name = txtCategoryName.Text.Trim();
            target.ParentCategoryId = dropCategories.SelectedValue;
            target.AssociatedProductType = dropProductTypes.SelectedValue;

            if (!string.IsNullOrEmpty(txtRewriteName.Text.Trim()))
            {
                target.RewriteName = txtRewriteName.Text.Trim();
            }
            else
            {
                target.RewriteName = null;
            }

            target.MetaTitle = txtPageKeyTitle.Text.Trim();
            target.MetaKeywords = txtPageKeyWords.Text.Trim();
            target.MetaDescription = txtPageDesc.Text.Trim();
            target.Notes1 = fckNotes1.Text;
            target.Notes2 = fckNotes2.Text;
            target.Notes3 = fckNotes3.Text;
            target.DisplaySequence = 1;

            if (target.ParentCategoryId.HasValue)
            {
                CategoryInfo category = SubsiteCatalogHelper.GetCategory(target.ParentCategoryId.Value);
                if ((category == null) || (category.Depth >= 5))
                {
                    ShowMsg(string.Format("您选择的上级分类有误，店铺分类最多只支持{0}级分类", 5), false);
                    return null;
                }
                if (!string.IsNullOrEmpty(target.Notes1))
                {
                    target.Notes1 = category.Notes1;
                }
                if (!string.IsNullOrEmpty(target.Notes2))
                {
                    target.Notes2 = category.Notes2;
                }
                if (!string.IsNullOrEmpty(target.Notes3))
                {
                    target.Notes3 = category.Notes3;
                }
                if (!string.IsNullOrEmpty(target.RewriteName))
                {
                    target.RewriteName = category.RewriteName;
                }
            }
            ValidationResults results = Validation.Validate<CategoryInfo>(target, new string[] { "ValCategory" });
            string msg = string.Empty;
            if (results.IsValid)
            {
                return target;
            }
            foreach (ValidationResult result in (IEnumerable<ValidationResult>)results)
            {
                msg = msg + Formatter.FormatErrorMessage(result.Message);
            }
            ShowMsg(msg, false);
            return null;
        }


    }
}

