using Hidistro.Membership.Context;
using Hidistro.Messages;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	public class UpdatePassword : MemberTemplatedWebControl
	{
		private SmallStatusMessage status;
		private System.Web.UI.WebControls.TextBox txtOdlPassword;
		private System.Web.UI.WebControls.TextBox txtNewPassword;
		private System.Web.UI.WebControls.TextBox txtNewPassword2;
		private IButton btnChangePassword;
		private System.Web.UI.HtmlControls.HtmlGenericControl LkUpdateTradePassword;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-UpdatePassword.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.status = (SmallStatusMessage)this.FindControl("status");
			this.txtOdlPassword = (System.Web.UI.WebControls.TextBox)this.FindControl("txtOdlPassword");
			this.txtNewPassword = (System.Web.UI.WebControls.TextBox)this.FindControl("txtNewPassword");
			this.txtNewPassword2 = (System.Web.UI.WebControls.TextBox)this.FindControl("txtNewPassword2");
			this.btnChangePassword = ButtonManager.Create(this.FindControl("btnChangePassword"));
			this.LkUpdateTradePassword = (System.Web.UI.HtmlControls.HtmlGenericControl)this.FindControl("one2");
			PageTitle.AddSiteNameTitle("修改登录密码", HiContext.Current.Context);
			this.btnChangePassword.Click += new System.EventHandler(this.btnChangePassword_Click);
			if (!this.Page.IsPostBack)
			{
				Member member = Users.GetUser(HiContext.Current.User.UserId, false) as Member;
				if (!member.IsOpenBalance)
				{
					this.LkUpdateTradePassword.Visible = false;
				}
			}
		}
		protected void btnChangePassword_Click(object sender, System.EventArgs e)
		{
			Member member = HiContext.Current.User as Member;
			if (member.MembershipUser == null || !member.MembershipUser.IsLockedOut)
			{
				member.Password = this.txtOdlPassword.Text;
				if (member.ChangePassword(this.txtOdlPassword.Text, this.txtNewPassword2.Text))
				{
					Messenger.UserPasswordChanged(member, this.txtNewPassword2.Text);
					member.OnPasswordChanged(new UserEventArgs(member.Username, this.txtNewPassword2.Text, null));
					this.ShowMessage("你已经成功的修改了登录密码", true);
				}
				else
				{
					this.ShowMessage("当前登录密码输入错误", false);
				}
			}
		}
	}
}
