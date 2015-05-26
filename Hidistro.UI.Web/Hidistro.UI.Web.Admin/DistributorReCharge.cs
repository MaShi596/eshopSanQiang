using Hidistro.ControlPanel.Distribution;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.DistributorReCharge)]
	public class DistributorReCharge : AdminPage
	{
		private int userId;
		protected System.Web.UI.WebControls.Literal litUserNames;
		protected FormatedMoneyLabel lblUseableBalance;
		protected System.Web.UI.WebControls.TextBox txtReCharge;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtReChargeTip;
		protected System.Web.UI.WebControls.TextBox txtRemarks;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtRemarksTip;
		protected System.Web.UI.WebControls.Button btnReChargeOK;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!int.TryParse(this.Page.Request.QueryString["userId"], out this.userId))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.btnReChargeOK.Click += new System.EventHandler(this.btnReChargeOK_Click);
			if (!this.Page.IsPostBack)
			{
				Hidistro.Membership.Context.Distributor distributor = DistributorHelper.GetDistributor(this.userId);
				if (distributor == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				this.litUserNames.Text = distributor.Username;
				this.lblUseableBalance.Money = distributor.Balance - distributor.RequestBalance;
			}
		}
		private void btnReChargeOK_Click(object sender, System.EventArgs e)
		{
			int num = 0;
			if (this.txtReCharge.Text.Trim().IndexOf(".") > 0)
			{
				num = this.txtReCharge.Text.Trim().Substring(this.txtReCharge.Text.Trim().IndexOf(".") + 1).Length;
			}
			decimal num2;
			if (!decimal.TryParse(this.txtReCharge.Text.Trim(), out num2) || num > 2)
			{
				this.ShowMsg("本次充值要给当前客户加款的金额只能是数值，且不能超过2位小数", false);
				return;
			}
			if (num2 < -10000000m || num2 > 10000000m)
			{
				this.ShowMsg("金额大小必须在正负1000万之间", false);
				return;
			}
			Hidistro.Membership.Context.Distributor distributor = Hidistro.Membership.Context.Users.GetUser(this.userId, false) as Hidistro.Membership.Context.Distributor;
			if (distributor == null)
			{
				this.ShowMsg("此分销商已经不存在", false);
				return;
			}
			decimal num3 = num2 + distributor.Balance;
			BalanceDetailInfo balanceDetailInfo = new BalanceDetailInfo();
			balanceDetailInfo.UserId = this.userId;
			balanceDetailInfo.UserName = distributor.Username;
			balanceDetailInfo.TradeDate = System.DateTime.Now;
			balanceDetailInfo.TradeType = TradeTypes.BackgroundAddmoney;
			balanceDetailInfo.Income = new decimal?(num2);
			balanceDetailInfo.Balance = num3;
			balanceDetailInfo.Remark = Globals.HtmlEncode(this.txtRemarks.Text.Trim());
			ValidationResults validationResults = Validation.Validate<BalanceDetailInfo>(balanceDetailInfo, new string[]
			{
				"ValBalanceDetail"
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
			if (DistributorHelper.AddBalance(balanceDetailInfo, num2))
			{
				this.Page.ClientScript.RegisterClientScriptBlock(base.GetType(), "success", string.Format("<script>alert(\"本次充值已成功，充值金额：{0}\");window.location.href=\"DistributorReCharge.aspx?userId={1}\"</script>", num2, this.userId));
			}
			this.txtReCharge.Text = string.Empty;
			this.txtRemarks.Text = string.Empty;
			this.lblUseableBalance.Money = num3;
		}
	}
}
