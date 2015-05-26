using Hidistro.Core;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class SendMyMessage : DistributorPage
	{
		private int userId;
		protected System.Web.UI.WebControls.TextBox txtTitle;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtTitleTip;
		protected System.Web.UI.WebControls.TextBox txtContent;
		protected System.Web.UI.WebControls.Button btnRefer;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnRefer.Click += new System.EventHandler(this.btnRefer_Click);
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["UserId"]) && !int.TryParse(this.Page.Request.QueryString["UserId"], out this.userId))
			{
				base.GotoResourceNotFound();
			}
		}
		private void btnRefer_Click(object sender, System.EventArgs e)
		{
			if (!this.ValidateValues())
			{
				return;
			}
			this.Session["Title"] = Globals.UrlEncode(this.txtTitle.Text.Replace("\r\n", ""));
			this.Session["Content"] = Globals.UrlEncode(this.txtContent.Text.Replace("\r\n", ""));
			if (this.userId == 0)
			{
				base.Response.Redirect(Globals.ApplicationPath + "/shopadmin/comment/SendMyMessageSelectUser.aspx", true);
				return;
			}
			if (this.userId > 0)
			{
				base.Response.Redirect(Globals.ApplicationPath + string.Format("/shopadmin/comment/SendMyMessageSelectUser.aspx?UserId={0}", this.userId), true);
			}
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
