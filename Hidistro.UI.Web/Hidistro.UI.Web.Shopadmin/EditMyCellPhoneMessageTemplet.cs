using Hidistro.Core;
using Hidistro.Membership.Context;
using Hidistro.Messages;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class EditMyCellPhoneMessageTemplet : DistributorPage
	{
		protected System.Web.UI.WebControls.Label litEmailType;
		protected System.Web.UI.WebControls.Literal litTagDescription;
		protected System.Web.UI.WebControls.TextBox txtContent;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtContentTip;
		protected System.Web.UI.WebControls.Button btnSaveCellPhoneMessageTemplet;
		private string messageType;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.messageType = this.Page.Request.QueryString["MessageType"];
			this.btnSaveCellPhoneMessageTemplet.Click += new System.EventHandler(this.btnSaveCellPhoneMessageTemplet_Click);
			if (!base.IsPostBack)
			{
				if (string.IsNullOrEmpty(this.messageType))
				{
					base.GotoResourceNotFound();
					return;
				}
				MessageTemplate distributorMessageTemplate = MessageTemplateHelper.GetDistributorMessageTemplate(this.messageType, Hidistro.Membership.Context.HiContext.Current.User.UserId);
				if (distributorMessageTemplate == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				this.litEmailType.Text = distributorMessageTemplate.Name;
				this.litTagDescription.Text = distributorMessageTemplate.TagDescription;
				this.txtContent.Text = distributorMessageTemplate.SMSBody;
			}
		}
		private void btnSaveCellPhoneMessageTemplet_Click(object sender, System.EventArgs e)
		{
			MessageTemplate distributorMessageTemplate = MessageTemplateHelper.GetDistributorMessageTemplate(this.messageType, Hidistro.Membership.Context.HiContext.Current.User.UserId);
			if (distributorMessageTemplate == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(this.txtContent.Text))
			{
				this.ShowMsg("短信内容不能为空", false);
				return;
			}
			if (this.txtContent.Text.Trim().Length >= 1 && this.txtContent.Text.Trim().Length <= 300)
			{
				distributorMessageTemplate.SMSBody = this.txtContent.Text.Trim();
				MessageTemplateHelper.UpdateDistributorTemplate(distributorMessageTemplate);
				this.Page.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/tools/MySendMessageTemplets.aspx");
				return;
			}
			this.ShowMsg("长度限制在1-300个字符之间", false);
		}
	}
}
