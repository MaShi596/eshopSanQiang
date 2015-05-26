using Hidistro.Entities.Members;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class ConfirmBalanceDrawRequest : DistributorPage
	{
		protected System.Web.UI.WebControls.Literal litRealName;
		protected FormatedMoneyLabel lblAmount;
		protected System.Web.UI.WebControls.Literal litBankName;
		protected System.Web.UI.WebControls.Literal litName;
		protected System.Web.UI.WebControls.Literal litBankCode;
		protected System.Web.UI.WebControls.Button btnOK;
		protected System.Web.UI.HtmlControls.HtmlGenericControl message;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			if (!base.IsPostBack)
			{
				BalanceDrawRequestInfo balanceDrawRequestInfo = new BalanceDrawRequestInfo();
				if (this.Session["BalanceDrawRequest"] != null)
				{
					balanceDrawRequestInfo = (BalanceDrawRequestInfo)this.Session["BalanceDrawRequest"];
					Hidistro.Membership.Context.Distributor distributor = Hidistro.Membership.Context.Users.GetUser(balanceDrawRequestInfo.UserId) as Hidistro.Membership.Context.Distributor;
					this.litRealName.Text = distributor.RealName;
					this.litName.Text = balanceDrawRequestInfo.AccountName;
					this.litBankName.Text = balanceDrawRequestInfo.BankName;
					this.litBankCode.Text = balanceDrawRequestInfo.MerchantCode;
					this.lblAmount.Money = balanceDrawRequestInfo.Amount;
					return;
				}
				base.GotoResourceNotFound();
			}
		}
		private void btnOK_Click(object sender, System.EventArgs e)
		{
			if (SubsiteStoreHelper.DistroHasDrawRequest())
			{
				this.ShowMsg("对不起，您的上一笔提现申请尚未进行处理", false);
				return;
			}
			if (this.Session["BalanceDrawRequest"] != null)
			{
				BalanceDrawRequestInfo balanceDrawRequest = (BalanceDrawRequestInfo)this.Session["BalanceDrawRequest"];
				if (SubsiteStoreHelper.BalanceDrawRequest(balanceDrawRequest))
				{
					this.message.Visible = true;
					return;
				}
				this.ShowMsg("写入提现信息失败", false);
			}
		}
	}
}
