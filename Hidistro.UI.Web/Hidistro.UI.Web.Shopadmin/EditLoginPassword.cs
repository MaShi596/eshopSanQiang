using Hidistro.Membership.Context;
using Hidistro.Subsites.Store;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class EditLoginPassword : DistributorPage
	{
		protected System.Web.UI.WebControls.TextBox txtOldPassword;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtOldPasswordTip;
		protected System.Web.UI.WebControls.TextBox txtNewPassword;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtNewPasswordTip;
		protected System.Web.UI.WebControls.TextBox txtPasswordCompare;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtPasswordCompareTip;
		protected System.Web.UI.WebControls.Button btnEditLoginPassword;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnEditLoginPassword.Click += new System.EventHandler(this.btnEditLoginPassword_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
		}
		private void btnEditLoginPassword_Click(object sender, System.EventArgs e)
		{
			Hidistro.Membership.Context.Distributor distributor = SubsiteStoreHelper.GetDistributor();
			if (string.IsNullOrEmpty(this.txtOldPassword.Text))
			{
				this.ShowMsg("旧登录密码不能为空", false);
				return;
			}
			if (string.IsNullOrEmpty(this.txtNewPassword.Text) || this.txtNewPassword.Text.Length > 20 || this.txtNewPassword.Text.Length < 6)
			{
				this.ShowMsg("新登录密码不能为空，长度限制在6-20个字符之间", false);
				return;
			}
			if (this.txtNewPassword.Text != this.txtPasswordCompare.Text)
			{
				this.ShowMsg("两次输入的密码不一致", false);
				return;
			}
			if (distributor.ChangePassword(this.txtOldPassword.Text, this.txtNewPassword.Text))
			{
				distributor.OnPasswordChanged(new Hidistro.Membership.Context.UserEventArgs(distributor.Username, this.txtNewPassword.Text, null));
				this.ShowMsg("登录密码修改成功", true);
				return;
			}
			this.ShowMsg("登录密码修改失败", false);
		}
	}
}
