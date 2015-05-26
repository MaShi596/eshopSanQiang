using Hidistro.Membership.Context;
using Hidistro.Subsites.Members;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class EditUnderlingLoginPassword : DistributorPage
	{
		protected System.Web.UI.WebControls.Literal litlUserName;
		protected System.Web.UI.WebControls.TextBox txtNewPassWord;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtNewPassWordTip;
		protected System.Web.UI.WebControls.TextBox txtPassWordCompare;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtPassWordCompareTip;
		protected System.Web.UI.WebControls.Button btnEditUser;
		private int currentUserId;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.currentUserId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.btnEditUser.Click += new System.EventHandler(this.btnEditUser_Click);
			if (!this.Page.IsPostBack)
			{
				Hidistro.Membership.Context.Member member = UnderlingHelper.GetMember(this.currentUserId);
				if (member == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				this.litlUserName.Text = member.Username;
			}
		}
		private void btnEditUser_Click(object sender, System.EventArgs e)
		{
			Hidistro.Membership.Context.Member member = UnderlingHelper.GetMember(this.currentUserId);
			if (string.IsNullOrEmpty(this.txtNewPassWord.Text) || this.txtNewPassWord.Text.Length > 20 || this.txtNewPassWord.Text.Length < 6)
			{
				this.ShowMsg("登录密码不能为空，长度限制在6-20个字符之间", false);
				return;
			}
			if (this.txtNewPassWord.Text != this.txtPassWordCompare.Text)
			{
				this.ShowMsg("输入的两次密码不一致", false);
				return;
			}
			if (member.ChangePassword(this.txtNewPassWord.Text))
			{
				this.ShowMsg("登录密码修改成功", true);
				return;
			}
			this.ShowMsg("登录密码修改失败", false);
		}
	}
}
