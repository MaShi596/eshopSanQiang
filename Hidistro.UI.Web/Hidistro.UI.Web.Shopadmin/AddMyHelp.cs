using Hidistro.Entities.Comments;
using Hidistro.Subsites.Comments;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using kindeditor.Net;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class AddMyHelp : DistributorPage
	{
		protected DistributorHelpCategoryDropDownList dropHelpCategory;
		protected System.Web.UI.WebControls.TextBox txtHelpTitle;
		protected TrimTextBox txtMetaDescription;
		protected TrimTextBox txtMetaKeywords;
		protected System.Web.UI.WebControls.TextBox txtShortDesc;
		protected YesNoRadioButtonList radioShowFooter;
		protected KindeditorControl fcContent;
		protected System.Web.UI.WebControls.Button btnAddHelp;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnAddHelp.Click += new System.EventHandler(this.btnAddHelp_Click);
			if (!this.Page.IsPostBack)
			{
				this.dropHelpCategory.DataBind();
				if (this.Page.Request.QueryString["categoryId"] != null)
				{
					int value = 0;
					if (int.TryParse(this.Page.Request.QueryString["categoryId"], out value))
					{
						this.dropHelpCategory.SelectedValue = new int?(value);
					}
				}
			}
		}
		private void btnAddHelp_Click(object sender, System.EventArgs e)
		{
			HelpInfo helpInfo = new HelpInfo();
			if (!this.dropHelpCategory.SelectedValue.HasValue)
			{
				this.ShowMsg("请选择帮助分类", false);
				return;
			}
			helpInfo.AddedDate = System.DateTime.Now;
			helpInfo.CategoryId = this.dropHelpCategory.SelectedValue.Value;
			helpInfo.Title = this.txtHelpTitle.Text;
			helpInfo.MetaDescription = this.txtMetaDescription.Text;
			helpInfo.MetaKeywords = this.txtMetaKeywords.Text;
			helpInfo.Description = this.txtShortDesc.Text;
			helpInfo.Content = this.fcContent.Text;
			helpInfo.IsShowFooter = this.radioShowFooter.SelectedValue;
			ValidationResults validationResults = Validation.Validate<HelpInfo>(helpInfo, new string[]
			{
				"ValHelpInfo"
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
			if (this.radioShowFooter.SelectedValue)
			{
				HelpCategoryInfo helpCategory = SubsiteCommentsHelper.GetHelpCategory(helpInfo.CategoryId);
				if (!helpCategory.IsShowFooter)
				{
					this.ShowMsg("当选中的帮助分类设置不在底部帮助显示时，此分类下的帮助主题就不能设置在底部帮助显示", false);
					return;
				}
			}
			if (SubsiteCommentsHelper.CreateHelp(helpInfo))
			{
				this.txtHelpTitle.Text = string.Empty;
				this.txtShortDesc.Text = string.Empty;
                this.fcContent.Text = string.Empty;
				this.ShowMsg("成功添加了一个帮助主题", true);
				return;
			}
			this.ShowMsg("添加帮助主题错误", false);
		}
	}
}
