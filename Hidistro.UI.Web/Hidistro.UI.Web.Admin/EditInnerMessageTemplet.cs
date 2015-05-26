using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.Messages;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.MessageTemplets)]
	public class EditInnerMessageTemplet : AdminPage
	{
		private string messageType;
		protected System.Web.UI.WebControls.Label litEmailType;
		protected System.Web.UI.WebControls.Literal litTagDescription;
		protected System.Web.UI.WebControls.TextBox txtMessageSubject;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtMessageSubjectTip;
		protected System.Web.UI.WebControls.TextBox txtContent;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtContentTip;
		protected System.Web.UI.WebControls.Button btnSaveMessageTemplet;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.messageType = this.Page.Request.QueryString["MessageType"];
			this.btnSaveMessageTemplet.Click += new System.EventHandler(this.btnSaveMessageTemplet_Click);
			if (!base.IsPostBack)
			{
				if (string.IsNullOrEmpty(this.messageType))
				{
					base.GotoResourceNotFound();
					return;
				}
				MessageTemplate messageTemplate = MessageTemplateHelper.GetMessageTemplate(this.messageType);
				if (messageTemplate == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				this.litEmailType.Text = messageTemplate.Name;
				this.litTagDescription.Text = messageTemplate.TagDescription;
				this.txtMessageSubject.Text = messageTemplate.InnerMessageSubject;
				this.txtContent.Text = messageTemplate.InnerMessageBody;
			}
		}
		private void btnSaveMessageTemplet_Click(object sender, System.EventArgs e)
		{
			MessageTemplate messageTemplate = MessageTemplateHelper.GetMessageTemplate(this.messageType);
			if (messageTemplate == null)
			{
				return;
			}
			string text = string.Empty;
			bool flag = true;
			if (string.IsNullOrEmpty(this.txtMessageSubject.Text))
			{
				text += Formatter.FormatErrorMessage("消息标题不能为空");
				flag = false;
			}
			if (this.txtMessageSubject.Text.Trim().Length < 1 || this.txtMessageSubject.Text.Trim().Length > 60)
			{
				text += Formatter.FormatErrorMessage("消息标题长度限制在1-60个字符之间");
				flag = false;
			}
			if (string.IsNullOrEmpty(this.txtContent.Text))
			{
				text += Formatter.FormatErrorMessage("消息内容不能为空");
				flag = false;
			}
			if (this.txtContent.Text.Trim().Length < 1 || this.txtContent.Text.Trim().Length > 300)
			{
				text += Formatter.FormatErrorMessage("消息长度限制在300个字符以内");
				flag = false;
			}
			if (!flag)
			{
				this.ShowMsg(text, false);
				return;
			}
			messageTemplate.InnerMessageSubject = this.txtMessageSubject.Text.Trim();
			messageTemplate.InnerMessageBody = this.txtContent.Text;
			MessageTemplateHelper.UpdateTemplate(messageTemplate);
			this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("tools/SendMessageTemplets.aspx"));
		}
	}
}
