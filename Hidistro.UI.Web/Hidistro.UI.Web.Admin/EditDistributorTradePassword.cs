using Hidistro.ControlPanel.Distribution;
using Hidistro.ControlPanel.Store;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.Messages;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.EditDistributor)]
	public class EditDistributorTradePassword : AdminPage
	{
		private int userId;
		protected System.Web.UI.WebControls.Literal litUserName;
		protected WangWangConversations WangWangConversations;
		protected System.Web.UI.WebControls.TextBox txtNewTradePassword;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtNewTradePasswordTip;
		protected System.Web.UI.WebControls.TextBox txtTradePasswordCompare;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtTradePasswordCompareTip;
		protected System.Web.UI.WebControls.Button btnEditDistributorTradePassword;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnEditDistributorTradePassword.Click += new System.EventHandler(this.btnEditDistributorTradePassword_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.userId))
			{
				base.GotoResourceNotFound();
				return;
			}
			if (!base.IsPostBack)
			{
				this.LoadControl();
			}
		}
		private void LoadControl()
		{
			Hidistro.Membership.Context.Distributor distributor = DistributorHelper.GetDistributor(this.userId);
			if (distributor == null)
			{
				base.GotoResourceNotFound();
				return;
			}
			this.litUserName.Text = distributor.Username;
			this.WangWangConversations.WangWangAccounts = distributor.Wangwang;
		}
		private void btnEditDistributorTradePassword_Click(object sender, System.EventArgs e)
		{
			Hidistro.Membership.Context.Distributor distributor = DistributorHelper.GetDistributor(this.userId);
			if (string.IsNullOrEmpty(this.txtNewTradePassword.Text) || this.txtNewTradePassword.Text.Length > 20 || this.txtNewTradePassword.Text.Length < 6)
			{
				this.ShowMsg("交易密码不能为空，长度限制在6-20个字符之间", false);
				return;
			}
			if (this.txtNewTradePassword.Text != this.txtTradePasswordCompare.Text)
			{
				this.ShowMsg("输入的两次密码不一致", false);
				return;
			}
			if (distributor.ChangeTradePassword(this.txtNewTradePassword.Text))
			{
				Messenger.UserDealPasswordChanged(distributor, this.txtNewTradePassword.Text);
				distributor.OnDealPasswordChanged(new Hidistro.Membership.Context.UserEventArgs(distributor.Username, null, this.txtNewTradePassword.Text));
				this.ShowMsg("交易密码修改成功", true);
				return;
			}
			this.ShowMsg("交易密码修改失败", false);
		}
	}
}
