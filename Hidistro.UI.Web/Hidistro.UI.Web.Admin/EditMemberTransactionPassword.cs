using Hidistro.ControlPanel.Members;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.Messages;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.EditMember)]
	public class EditMemberTransactionPassword : AdminPage
	{
		protected System.Web.UI.WebControls.Literal litlUserName;
		protected System.Web.UI.WebControls.TextBox txtTransactionPassWord;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtTransactionPassWordTip;
		protected System.Web.UI.WebControls.TextBox txtTransactionPassWordCompare;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtTransactionPassWordCompareTip;
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
				Hidistro.Membership.Context.Member member = MemberHelper.GetMember(this.currentUserId);
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
			Hidistro.Membership.Context.Member member = MemberHelper.GetMember(this.currentUserId);
			if (!member.IsOpenBalance)
			{
				this.ShowMsg("该会员没有开启预付款账户，无法修改交易密码", false);
				return;
			}
			if (string.IsNullOrEmpty(this.txtTransactionPassWord.Text) || this.txtTransactionPassWord.Text.Length > 20 || this.txtTransactionPassWord.Text.Length < 6)
			{
				this.ShowMsg("交易密码不能为空，长度限制在6-20个字符之间", false);
				return;
			}
			if (this.txtTransactionPassWord.Text != this.txtTransactionPassWordCompare.Text)
			{
				this.ShowMsg("输入的两次密码不一致", false);
				return;
			}
			if (member.ChangeTradePassword(this.txtTransactionPassWord.Text))
			{
				Messenger.UserDealPasswordChanged(member, this.txtTransactionPassWord.Text);
				member.OnDealPasswordChanged(new Hidistro.Membership.Context.UserEventArgs(member.Username, null, this.txtTransactionPassWord.Text));
				this.ShowMsg("交易密码修改成功", true);
				return;
			}
			this.ShowMsg("交易密码修改失败", false);
		}
	}
}
