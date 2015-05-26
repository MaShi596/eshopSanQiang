using Hidistro.Core;
using Hidistro.Membership.Context;
using Hidistro.Messages;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using kindeditor.Net;
using System;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class EditMyEmailTemplet : DistributorPage
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
				MessageTemplate distributorMessageTemplate = MessageTemplateHelper.GetDistributorMessageTemplate(this.emailType, Hidistro.Membership.Context.HiContext.Current.User.UserId);
				if (distributorMessageTemplate == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				this.litEmailType.Text = distributorMessageTemplate.Name;
				this.litEmailDescription.Text = distributorMessageTemplate.TagDescription;
				this.txtEmailSubject.Text = distributorMessageTemplate.EmailSubject;
                this.fcContent.Text = distributorMessageTemplate.EmailBody;
			}
		}
		private void btnSaveEmailTemplet_Click(object sender, System.EventArgs e)
		{
			MessageTemplate distributorMessageTemplate = MessageTemplateHelper.GetDistributorMessageTemplate(this.emailType, Hidistro.Membership.Context.HiContext.Current.User.UserId);
			if (distributorMessageTemplate == null)
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
			distributorMessageTemplate.EmailBody = text2;
			distributorMessageTemplate.EmailSubject = this.txtEmailSubject.Text.Trim();
			MessageTemplateHelper.UpdateDistributorTemplate(distributorMessageTemplate);
			this.Page.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/tools/MySendMessageTemplets.aspx");
		}
	}
}
