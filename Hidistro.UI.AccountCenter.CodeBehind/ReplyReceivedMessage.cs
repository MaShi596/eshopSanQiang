using Hidistro.AccountCenter.Comments;
using Hidistro.Entities.Comments;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	public class ReplyReceivedMessage : MemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.Literal litAddresser;
		private System.Web.UI.WebControls.Literal litTitle;
		private FormatedTimeLabel litDate;
		private System.Web.UI.WebControls.Literal litContent;
		private System.Web.UI.WebControls.TextBox txtReplyTitle;
		private System.Web.UI.WebControls.TextBox txtReplyContent;
		private System.Web.UI.WebControls.Button btnReplyReceivedMessage;
		private long messageId = 0L;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-ReplyReceivedMessage.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.litAddresser = (System.Web.UI.WebControls.Literal)this.FindControl("litAddresser");
			this.litTitle = (System.Web.UI.WebControls.Literal)this.FindControl("litTitle");
			this.litDate = (FormatedTimeLabel)this.FindControl("litDate");
			this.litContent = (System.Web.UI.WebControls.Literal)this.FindControl("litContent");
			this.txtReplyTitle = (System.Web.UI.WebControls.TextBox)this.FindControl("txtReplyTitle");
			this.txtReplyContent = (System.Web.UI.WebControls.TextBox)this.FindControl("txtReplyContent");
			this.btnReplyReceivedMessage = (System.Web.UI.WebControls.Button)this.FindControl("btnReplyReceivedMessage");
			this.btnReplyReceivedMessage.Click += new System.EventHandler(this.btnReplyReceivedMessage_Click);
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["MessageId"]))
			{
				this.messageId = long.Parse(this.Page.Request.QueryString["MessageId"]);
			}
			if (!this.Page.IsPostBack)
			{
				CommentsHelper.PostMemberMessageIsRead(this.messageId);
				MessageBoxInfo memberMessage = CommentsHelper.GetMemberMessage(this.messageId);
				this.litAddresser.Text = "管理员";
				this.litTitle.Text = memberMessage.Title;
				this.litContent.Text = memberMessage.Content;
				this.litDate.Time = memberMessage.Date;
			}
		}
		private void btnReplyReceivedMessage_Click(object sender, System.EventArgs e)
		{
			string text = "";
			if (string.IsNullOrEmpty(this.txtReplyTitle.Text) || this.txtReplyTitle.Text.Length > 60)
			{
				text += Formatter.FormatErrorMessage("标题不能为空，长度限制在1-60个字符内");
			}
			if (string.IsNullOrEmpty(this.txtReplyContent.Text) || this.txtReplyContent.Text.Length > 300)
			{
				text += Formatter.FormatErrorMessage("内容不能为空，长度限制在1-300个字符内");
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowMessage(text, false);
			}
			else
			{
				if (CommentsHelper.SendMessage(new MessageBoxInfo
				{
					Sernder = HiContext.Current.User.Username,
					Accepter = HiContext.Current.SiteSettings.IsDistributorSettings ? Users.GetUser(HiContext.Current.SiteSettings.UserId.Value).Username : "admin",
					Title = this.txtReplyTitle.Text.Trim(),
					Content = this.txtReplyContent.Text.Trim()
				}))
				{
					this.ShowMessage("回复成功", true);
				}
				else
				{
					this.ShowMessage("回复失败", false);
				}
			}
		}
	}
}
