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
using System.Globalization;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Helps)]
	public class EditHelp : AdminPage
	{
		private int helpId;
		protected System.Web.UI.WebControls.Label lblEditHelp;
		protected HelpCategoryDropDownList dropHelpCategory;
		protected System.Web.UI.WebControls.TextBox txtHelpTitle;
		protected TrimTextBox txtMetaDescription;
		protected TrimTextBox txtMetaKeywords;
		protected System.Web.UI.WebControls.TextBox txtShortDesc;
		protected YesNoRadioButtonList radioShowFooter;
		protected KindeditorControl fcContent;
		protected System.Web.UI.WebControls.Button btnEditHelp;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["helpId"], out this.helpId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.btnEditHelp.Click += new System.EventHandler(this.btnEditHelp_Click);
			if (!this.Page.IsPostBack)
			{
				this.dropHelpCategory.DataBind();
				HelpInfo help = ArticleHelper.GetHelp(this.helpId);
				if (help == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				Globals.EntityCoding(help, false);
				this.txtHelpTitle.Text = help.Title;
				this.txtMetaDescription.Text = help.MetaDescription;
				this.txtMetaKeywords.Text = help.MetaKeywords;
				this.txtShortDesc.Text = help.Description;
                this.fcContent.Text = help.Content;
				this.lblEditHelp.Text = help.HelpId.ToString(System.Globalization.CultureInfo.InvariantCulture);
				this.dropHelpCategory.SelectedValue = new int?(help.CategoryId);
				this.radioShowFooter.SelectedValue = help.IsShowFooter;
			}
		}
		private void btnEditHelp_Click(object sender, System.EventArgs e)
		{
			HelpInfo helpInfo = new HelpInfo();
			if (!this.dropHelpCategory.SelectedValue.HasValue)
			{
				this.ShowMsg("请选择帮助分类", false);
				return;
			}
			helpInfo.HelpId = this.helpId;
			helpInfo.AddedDate = System.DateTime.Now;
			helpInfo.CategoryId = this.dropHelpCategory.SelectedValue.Value;
			helpInfo.Title = this.txtHelpTitle.Text.Trim();
			helpInfo.MetaDescription = this.txtMetaDescription.Text.Trim();
			helpInfo.MetaKeywords = this.txtMetaKeywords.Text.Trim();
			helpInfo.Description = this.txtShortDesc.Text.Trim();
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
				HelpCategoryInfo helpCategory = ArticleHelper.GetHelpCategory(helpInfo.CategoryId);
				if (!helpCategory.IsShowFooter)
				{
					this.ShowMsg("当选中的帮助分类设置不在底部帮助显示时，此分类下的帮助主题就不能设置在底部帮助显示", false);
					return;
				}
			}
			if (ArticleHelper.UpdateHelp(helpInfo))
			{
				this.ShowMsg("已经成功修改当前帮助", true);
				return;
			}
			this.ShowMsg("编辑底部帮助错误", false);
		}
	}
}
