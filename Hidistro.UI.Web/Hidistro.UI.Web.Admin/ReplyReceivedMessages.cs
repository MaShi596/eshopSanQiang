using Hidistro.ControlPanel.Comments;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Comments;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ProductReviewsManage)]
	public class ReplyReceivedMessages : AdminPage
	{
		private long messageId;
		protected System.Web.UI.WebControls.Label litAddresser;
		protected FormatedTimeLabel litDate;
		protected System.Web.UI.WebControls.Literal litTitle;
		protected System.Web.UI.WebControls.Literal litContent;
		protected System.Web.UI.WebControls.DataList dtlistMessageReply;
		protected System.Web.UI.WebControls.TextBox txtTitle;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtTitleTip;
		protected System.Web.UI.WebControls.TextBox txtContes;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtContesTip;
		protected System.Web.UI.WebControls.Button btnReplyReplyReceivedMessages;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!long.TryParse(this.Page.Request.QueryString["MessageId"], out this.messageId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.btnReplyReplyReceivedMessages.Click += new System.EventHandler(this.btnReplyReplyReceivedMessages_Click);
			if (!this.Page.IsPostBack)
			{
				NoticeHelper.PostManagerMessageIsRead(this.messageId);
				MessageBoxInfo managerMessage = NoticeHelper.GetManagerMessage(this.messageId);
				this.litTitle.Text = managerMessage.Title;
				this.litContent.Text = managerMessage.Content;
				this.ViewState["Sernder"] = managerMessage.Sernder;
			}
		}
		protected void btnReplyReplyReceivedMessages_Click(object sender, System.EventArgs e)
		{
			if (NoticeHelper.SendMessageToMember(new System.Collections.Generic.List<MessageBoxInfo>
			{
				new MessageBoxInfo
				{
					Accepter = (string)this.ViewState["Sernder"],
					Sernder = "admin",
					Title = this.txtTitle.Text.Trim(),
					Content = this.txtContes.Text.Trim()
				}
			}) > 0)
			{
				this.ShowMsg("成功回复了会员的站内信.", true);
				return;
			}
			this.ShowMsg("回复会员的站内信失败.", false);
		}
	}
}
