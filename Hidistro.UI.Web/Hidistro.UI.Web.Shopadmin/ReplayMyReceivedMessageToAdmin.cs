using Hidistro.Entities.Comments;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Comments;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class ReplayMyReceivedMessageToAdmin : DistributorPage
	{
		private long messageId;
		protected System.Web.UI.WebControls.Label litAddresser;
		protected FormatedTimeLabel litDate;
		protected System.Web.UI.WebControls.Literal litTitle;
		protected System.Web.UI.WebControls.Literal litContent;
		protected System.Web.UI.WebControls.DataList dtlistReceivedMessagesReplyed;
		protected System.Web.UI.WebControls.TextBox txtReplyTitle;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtReplyTitleTip;
		protected System.Web.UI.WebControls.TextBox txtContes;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtContesTip;
		protected System.Web.UI.WebControls.Button btnReplyReplyReceivedMessages;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!long.TryParse(base.Request.QueryString["MessageId"], out this.messageId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.btnReplyReplyReceivedMessages.Click += new System.EventHandler(this.btnReplyReplyReceivedMessages_Click);
			if (!this.Page.IsPostBack)
			{
				SubsiteCommentsHelper.PostMessageIsRead(this.messageId);
				MessageBoxInfo message = SubsiteCommentsHelper.GetMessage(this.messageId);
				this.litTitle.Text = message.Title;
				this.litContent.Text = message.Content;
			}
		}
		protected void btnReplyReplyReceivedMessages_Click(object sender, System.EventArgs e)
		{
			if (SubsiteCommentsHelper.SendMessageToManager(new MessageBoxInfo
			{
				Title = this.txtReplyTitle.Text,
				Content = this.txtContes.Text.Trim(),
				Sernder = Hidistro.Membership.Context.HiContext.Current.User.Username,
				Accepter = "admin"
			}))
			{
				this.ShowMsg("成功的回复了管理员的消息", true);
				this.txtReplyTitle.Text = string.Empty;
				this.txtContes.Text = string.Empty;
				return;
			}
			this.ShowMsg("回复管理员的消息失败", false);
		}
	}
}
