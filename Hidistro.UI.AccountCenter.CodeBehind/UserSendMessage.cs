using Hidistro.AccountCenter.Comments;
using Hidistro.Core;
using Hidistro.Entities.Comments;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	public class UserSendMessage : MemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.RadioButton radioAdminSelect;
		private System.Web.UI.WebControls.TextBox txtTitle;
		private System.Web.UI.WebControls.TextBox txtContent;
		private IButton btnRefer;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UserSendMessage.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.radioAdminSelect = (System.Web.UI.WebControls.RadioButton)this.FindControl("radioAdminSelect");
			this.txtTitle = (System.Web.UI.WebControls.TextBox)this.FindControl("txtTitle");
			this.txtContent = (System.Web.UI.WebControls.TextBox)this.FindControl("txtContent");
			this.btnRefer = ButtonManager.Create(this.FindControl("btnRefer"));
			this.btnRefer.Click += new System.EventHandler(this.btnRefer_Click);
			if (!this.Page.IsPostBack)
			{
				this.radioAdminSelect.Enabled = false;
				this.radioAdminSelect.Checked = true;
				this.txtTitle.Text = this.txtTitle.Text.Trim();
				this.txtContent.Text = this.txtContent.Text.Trim();
			}
		}
		private void btnRefer_Click(object sender, System.EventArgs e)
		{
			string text = "";
			if (string.IsNullOrEmpty(this.txtTitle.Text) || this.txtTitle.Text.Length > 60)
			{
				text += Formatter.FormatErrorMessage("标题不能为空，长度限制在1-60个字符内");
			}
			if (string.IsNullOrEmpty(this.txtContent.Text) || this.txtContent.Text.Length > 300)
			{
				text += Formatter.FormatErrorMessage("内容不能为空，长度限制在1-300个字符内");
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowMessage(text, false);
			}
			else
			{
				MessageBoxInfo messageBoxInfo = new MessageBoxInfo();
				messageBoxInfo.Sernder = HiContext.Current.User.Username;
				messageBoxInfo.Accepter = (HiContext.Current.SiteSettings.IsDistributorSettings ? Users.GetUser(HiContext.Current.SiteSettings.UserId.Value).Username : "admin");
				messageBoxInfo.Title = Globals.HtmlEncode(this.txtTitle.Text.Replace("~", ""));
				messageBoxInfo.Content = Globals.HtmlEncode(this.txtContent.Text.Replace("~", ""));
				this.txtTitle.Text = string.Empty;
				this.txtContent.Text = string.Empty;
				if (CommentsHelper.SendMessage(messageBoxInfo))
				{
					this.ShowMessage("发送信息成功", true);
				}
				else
				{
					this.ShowMessage("发送信息失败", true);
				}
			}
		}
	}
}
