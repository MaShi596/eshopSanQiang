using Hidistro.Membership.Context;
using Hidistro.Subsites.Store;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class EditTradePassword : DistributorPage
	{
		protected System.Web.UI.WebControls.TextBox txtOldTradePassword;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtOldTradePasswordTip;
		protected System.Web.UI.WebControls.TextBox txtNewTradePassword;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtNewTradePasswordTip;
		protected System.Web.UI.WebControls.TextBox txtTradePasswordCompare;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtTradePasswordCompareTip;
		protected System.Web.UI.WebControls.Button btnEditTradePassword;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnEditTradePassword.Click += new System.EventHandler(this.btnEditTradePassword_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
		}
		private void btnEditTradePassword_Click(object sender, System.EventArgs e)
		{
			Hidistro.Membership.Context.Distributor distributor = SubsiteStoreHelper.GetDistributor();
			if (string.IsNullOrEmpty(this.txtOldTradePassword.Text))
			{
				this.ShowMsg("请输入旧交易密码", false);
				return;
			}
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
			if (distributor.ChangeTradePassword(this.txtOldTradePassword.Text, this.txtNewTradePassword.Text))
			{
				distributor.OnDealPasswordChanged(new Hidistro.Membership.Context.UserEventArgs(distributor.Username, null, this.txtNewTradePassword.Text));
				this.ShowMsg("交易密码修改成功", true);
				return;
			}
			this.ShowMsg("交易密码修改失败", false);
		}
	}
}
