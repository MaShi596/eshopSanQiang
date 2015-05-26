using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class BalanceDraw : DistributorPage
	{
		protected System.Web.UI.WebControls.Literal litRealName;
		protected FormatedMoneyLabel lblUseableBalance;
		protected System.Web.UI.WebControls.TextBox txtDrawBalance;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtDrawBalanceTip;
		protected System.Web.UI.WebControls.TextBox txtTradePassword;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtTradePasswordTip;
		protected System.Web.UI.WebControls.TextBox txtBankName;
		protected System.Web.UI.WebControls.TextBox txtAccountName;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtAccountNameTip;
		protected System.Web.UI.WebControls.TextBox txtMerchantCode;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtMerchantCodeTip;
		protected System.Web.UI.WebControls.TextBox txtMerchantCodeCompare;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtMerchantCodeCompareTip;
		protected System.Web.UI.WebControls.Button btnDrawNext;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnDrawNext.Click += new System.EventHandler(this.btnDrawNext_Click);
		}
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!base.IsPostBack)
			{
				AccountSummaryInfo myAccountSummary = SubsiteStoreHelper.GetMyAccountSummary();
				this.lblUseableBalance.Money = myAccountSummary.UseableBalance;
				Hidistro.Membership.Context.Distributor distributor = SubsiteStoreHelper.GetDistributor();
				this.litRealName.Text = distributor.RealName;
			}
		}
		private void btnDrawNext_Click(object sender, System.EventArgs e)
		{
			if (SubsiteStoreHelper.DistroHasDrawRequest())
			{
				this.ShowMsg("对不起，您的上一笔提现申请尚未进行处理", false);
				return;
			}
			decimal num;
			if (!decimal.TryParse(this.txtDrawBalance.Text.Trim(), out num))
			{
				this.ShowMsg(" 提现金额只能是数值，限制在1000万以内", false);
				return;
			}
			if (string.Compare(this.txtMerchantCodeCompare.Text.Trim(), this.txtMerchantCode.Text.Trim()) != 0)
			{
				this.ShowMsg(" 两次输入的帐号不一致,请重新输入", false);
				return;
			}
			if (num > (decimal)this.lblUseableBalance.Money)
			{
				this.ShowMsg(" 您的可用金额不足", false);
				return;
			}
			if (string.IsNullOrEmpty(this.txtTradePassword.Text))
			{
				this.ShowMsg("请输入交易密码", false);
				return;
			}
			Hidistro.Membership.Context.Distributor distributor = SubsiteStoreHelper.GetDistributor();
			BalanceDrawRequestInfo balanceDrawRequestInfo = new BalanceDrawRequestInfo();
			balanceDrawRequestInfo.UserId = distributor.UserId;
			balanceDrawRequestInfo.UserName = distributor.Username;
			balanceDrawRequestInfo.RequestTime = System.DateTime.Now;
			balanceDrawRequestInfo.MerchantCode = this.txtMerchantCode.Text.Trim();
			balanceDrawRequestInfo.BankName = this.txtBankName.Text.Trim();
			balanceDrawRequestInfo.Amount = num;
			balanceDrawRequestInfo.AccountName = this.txtAccountName.Text.Trim();
			balanceDrawRequestInfo.Remark = string.Empty;
			ValidationResults validationResults = Validation.Validate<BalanceDrawRequestInfo>(balanceDrawRequestInfo, new string[]
			{
				"ValBalanceDrawRequestInfo"
			});
			string text = string.Empty;
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult current in (System.Collections.Generic.IEnumerable<ValidationResult>)validationResults)
				{
					text += Formatter.FormatErrorMessage(current.Message);
				}
				this.ShowMsg(text, false);
				return;
			}
			distributor.TradePassword = this.txtTradePassword.Text;
			if (Hidistro.Membership.Context.Users.ValidTradePassword(distributor))
			{
				this.Session["BalanceDrawRequest"] = balanceDrawRequestInfo;
				base.Response.Redirect(Globals.ApplicationPath + "/ShopAdmin/store/ConfirmBalanceDrawRequest.aspx", true);
				return;
			}
			this.ShowMsg("交易密码不正确,请重新输入", false);
		}
	}
}
