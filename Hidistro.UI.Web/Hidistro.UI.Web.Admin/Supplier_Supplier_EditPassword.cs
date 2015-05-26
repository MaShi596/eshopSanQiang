using Hidistro.ControlPanel.Store;
using Hidistro.Core.Configuration;
using Hidistro.Membership.Context;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	public class Supplier_Supplier_EditPassword : AdminPage
	{
		private int userId;
		protected System.Web.UI.WebControls.Literal lblLoginNameValue;
		protected System.Web.UI.HtmlControls.HtmlGenericControl panelOld;
		protected System.Web.UI.WebControls.TextBox txtOldPassWord;
		protected System.Web.UI.WebControls.TextBox txtNewPassWord;
		protected System.Web.UI.WebControls.TextBox txtPassWordCompare;
		protected System.Web.UI.WebControls.Button btnEditPassWordOK;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.userId = Hidistro.Membership.Context.HiContext.Current.User.UserId;
			this.btnEditPassWordOK.Click += new System.EventHandler(this.btnEditPassWordOK_Click);
			if (!this.Page.IsPostBack)
			{
				Hidistro.Membership.Context.SiteManager manager = ManagerHelper.GetManager(this.userId);
				if (manager == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				this.lblLoginNameValue.Text = manager.Username;
				this.GetSecurity();
			}
		}
		private void btnEditPassWordOK_Click(object sender, System.EventArgs e)
		{
			Hidistro.Membership.Context.SiteManager manager = ManagerHelper.GetManager(this.userId);
			if (string.IsNullOrEmpty(this.txtNewPassWord.Text) || this.txtNewPassWord.Text.Length > 20 || this.txtNewPassWord.Text.Length < 6)
			{
				this.ShowMsg("密码不能为空，长度限制在6-20个字符之间", false);
				return;
			}
			if (string.Compare(this.txtNewPassWord.Text, this.txtPassWordCompare.Text) != 0)
			{
				this.ShowMsg("两次输入的密码不一样", false);
				return;
			}
			HiConfiguration config = HiConfiguration.GetConfig();
			if (string.IsNullOrEmpty(this.txtNewPassWord.Text) || this.txtNewPassWord.Text.Length < System.Web.Security.Membership.Provider.MinRequiredPasswordLength || this.txtNewPassWord.Text.Length > config.PasswordMaxLength)
			{
				this.ShowMsg(string.Format("管理员登录密码的长度只能在{0}和{1}个字符之间", System.Web.Security.Membership.Provider.MinRequiredPasswordLength, config.PasswordMaxLength), false);
				return;
			}
			if (this.userId == Hidistro.Membership.Context.HiContext.Current.User.UserId)
			{
				if (manager.ChangePassword(this.txtOldPassWord.Text, this.txtNewPassWord.Text))
				{
					this.ShowMsg("密码修改成功", true);
					return;
				}
				this.ShowMsg("修改密码失败，您输入的旧密码有误", false);
				return;
			}
			else
			{
				System.Web.HttpContext arg_126_0 = Hidistro.Membership.Context.HiContext.Current.Context;
				if (manager.ChangePassword(this.txtNewPassWord.Text))
				{
					this.ShowMsg("密码修改成功", true);
					return;
				}
				this.ShowMsg("修改密码失败，您输入的旧密码有误", false);
				return;
			}
		}
		private void GetSecurity()
		{
			if (Hidistro.Membership.Context.HiContext.Current.User.UserId != this.userId)
			{
				this.panelOld.Visible = false;
				return;
			}
			this.panelOld.Visible = true;
		}
	}
}
