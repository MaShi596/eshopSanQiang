using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Store;
using Hidistro.Messages;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using kindeditor.Net;
using System;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.MessageTemplets)]
	public class EditEmailTemplet : AdminPage
	{
		private string emailType;
		protected System.Web.UI.WebControls.Label litEmailType;
		protected System.Web.UI.WebControls.Literal litEmailDescription;
		protected System.Web.UI.WebControls.TextBox txtEmailSubject;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtEmailSubjectTip;
		protected KindeditorControl fcContent;
		protected System.Web.UI.WebControls.Button btnSaveEmailTemplet;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSaveEmailTemplet.Click += new System.EventHandler(this.btnSaveEmailTemplet_Click);
			this.emailType = this.Page.Request.QueryString["MessageType"];
			if (!this.Page.IsPostBack)
			{
				if (string.IsNullOrEmpty(this.emailType))
				{
					base.GotoResourceNotFound();
					return;
				}
				MessageTemplate messageTemplate = MessageTemplateHelper.GetMessageTemplate(this.emailType);
				if (messageTemplate == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				this.litEmailType.Text = messageTemplate.Name;
				this.litEmailDescription.Text = messageTemplate.TagDescription;
				this.txtEmailSubject.Text = messageTemplate.EmailSubject;
                this.fcContent.Text = messageTemplate.EmailBody;
			}
		}
		private void btnSaveEmailTemplet_Click(object sender, System.EventArgs e)
		{
			MessageTemplate messageTemplate = MessageTemplateHelper.GetMessageTemplate(this.emailType);
			if (messageTemplate == null)
			{
				return;
			}
			string text = string.Empty;
			bool flag = true;
			if (string.IsNullOrEmpty(this.txtEmailSubject.Text))
			{
				text += Formatter.FormatErrorMessage("邮件标题不能为空");
				flag = false;
			}
			else
			{
				if (this.txtEmailSubject.Text.Trim().Length < 1 || this.txtEmailSubject.Text.Trim().Length > 60)
				{
					text += Formatter.FormatErrorMessage("邮件标题长度限制在1-60个字符之间");
					flag = false;
				}
			}
			if (string.IsNullOrEmpty(this.fcContent.Text) || this.fcContent.Text.Trim().Length == 0)
			{
				text += Formatter.FormatErrorMessage("邮件内容不能为空");
				flag = false;
			}
			if (!flag)
			{
				this.ShowMsg(text, false);
				return;
			}
			string text2 = this.fcContent.Text;
			System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("<img\\b[^>]*?\\bsrc[\\s]*=[\\s]*[\"']?[\\s]*(?<imgUrl>[^\"'>]*)[^>]*?/?[\\s]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
			foreach (System.Text.RegularExpressions.Match match in regex.Matches(text2))
			{
				string value = match.Groups["imgUrl"].Value;
				if (value.StartsWith("/"))
				{
					text2 = text2.Replace(value, string.Format("http://{0}{1}", base.Request.Url.Host, value));
				}
			}
			messageTemplate.EmailBody = text2;
			messageTemplate.EmailSubject = this.txtEmailSubject.Text.Trim();
			MessageTemplateHelper.UpdateTemplate(messageTemplate);
			this.Page.Response.Redirect(Globals.GetAdminAbsolutePath("tools/SendMessageTemplets.aspx"));
		}
	}
}
