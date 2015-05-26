using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.Messages;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.MessageTemplets)]
	public class SendMessageTemplets : AdminPage
	{
		protected Grid grdEmailTemplets;
		protected System.Web.UI.WebControls.Button btnSaveSendSetting;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSaveSendSetting.Click += new System.EventHandler(this.btnSaveSendSetting_Click);
			if (!this.Page.IsPostBack)
			{
				this.grdEmailTemplets.DataSource = MessageTemplateHelper.GetMessageTemplates();
				this.grdEmailTemplets.DataBind();
			}
		}
		private void btnSaveSendSetting_Click(object sender, System.EventArgs e)
		{
			System.Collections.Generic.List<MessageTemplate> list = new System.Collections.Generic.List<MessageTemplate>();
			foreach (System.Web.UI.WebControls.GridViewRow gridViewRow in this.grdEmailTemplets.Rows)
			{
				MessageTemplate messageTemplate = new MessageTemplate();
				System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)gridViewRow.FindControl("chkSendEmail");
				messageTemplate.SendEmail = checkBox.Checked;
				System.Web.UI.WebControls.CheckBox checkBox2 = (System.Web.UI.WebControls.CheckBox)gridViewRow.FindControl("chkInnerMessage");
				messageTemplate.SendInnerMessage = checkBox2.Checked;
				System.Web.UI.WebControls.CheckBox checkBox3 = (System.Web.UI.WebControls.CheckBox)gridViewRow.FindControl("chkCellPhoneMessage");
				messageTemplate.SendSMS = checkBox3.Checked;
				messageTemplate.MessageType = (string)this.grdEmailTemplets.DataKeys[gridViewRow.RowIndex].Value;
				list.Add(messageTemplate);
			}
			MessageTemplateHelper.UpdateSettings(list);
		}
	}
}
