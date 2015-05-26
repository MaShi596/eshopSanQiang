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
	public class SendMyMessageToAdmin : DistributorPage
	{
		protected System.Web.UI.WebControls.TextBox txtTitle;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtTitleTip;
		protected System.Web.UI.WebControls.TextBox txtContent;
		protected System.Web.UI.WebControls.Button btnRefer;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnRefer.Click += new System.EventHandler(this.btnRefer_Click);
		}
		private void btnRefer_Click(object sender, System.EventArgs e)
		{
			if (!this.ValidateValues())
			{
				return;
			}
			if (!SubsiteCommentsHelper.SendMessageToManager(new MessageBoxInfo
			{
				Accepter = "admin",
				Sernder = Hidistro.Membership.Context.HiContext.Current.User.Username,
				Title = this.txtTitle.Text.Trim(),
				Content = this.txtContent.Text.Trim()
			}))
			{
				this.ShowMsg("发送失败", false);
				return;
			}
			this.ShowMsg("发送成功", true);
		}
		private bool ValidateValues()
		{
			string text = string.Empty;
			if (string.IsNullOrEmpty(this.txtTitle.Text.Trim()) || this.txtTitle.Text.Trim().Length > 60)
			{
				text += Formatter.FormatErrorMessage("标题不能为空，长度限制在1-60个字符内");
			}
			if (string.IsNullOrEmpty(this.txtContent.Text.Trim()) || this.txtContent.Text.Trim().Length > 300)
			{
				text += Formatter.FormatErrorMessage("内容不能为空，长度限制在1-300个字符内");
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowMsg(text, false);
				return false;
			}
			return true;
		}
	}
}
