using Hidistro.Core;
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
	public class ReCharge : DistributorPage
	{
		protected System.Web.UI.WebControls.Literal litRealName;
		protected FormatedMoneyLabel lblUseableBalance;
		protected PaymentRadioButtonList radioBtnPayment;
		protected System.Web.UI.WebControls.TextBox txtReChargeBalance;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtReChargeBalanceTip;
		protected System.Web.UI.WebControls.Button btnReChargeNext;
		protected override void OnInitComplete(System.EventArgs e)
		{
			base.OnInitComplete(e);
			this.btnReChargeNext.Click += new System.EventHandler(this.btnReChargeNext_Click);
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
		private void btnReChargeNext_Click(object sender, System.EventArgs e)
		{
			if (this.radioBtnPayment.Items.Count == 0)
			{
				this.ShowMsg("主站没有添加支付方式", false);
				return;
			}
			if (this.radioBtnPayment.SelectedValue == null)
			{
				this.ShowMsg("请选择支付方式", false);
				return;
			}
			int num = 0;
			if (this.txtReChargeBalance.Text.Trim().IndexOf(".") > 0)
			{
				num = this.txtReChargeBalance.Text.Trim().Substring(this.txtReChargeBalance.Text.Trim().IndexOf(".") + 1).Length;
			}
			decimal num2;
			if (!decimal.TryParse(this.txtReChargeBalance.Text.Trim(), out num2) || num > 2)
			{
				this.ShowMsg("充值金额只能是数值，且不能超过2位小数", false);
				return;
			}
			if (!(num2 <= 0m) && !(num2 > 10000000m))
			{
				base.Response.Redirect(Globals.ApplicationPath + string.Format("/Shopadmin/store/ReChargeConfirm.aspx?modeId={0}&blance={1}", this.radioBtnPayment.SelectedValue, num2), true);
				return;
			}
			this.ShowMsg("充值金额只能是非负数值，限制在1000万以内", false);
		}
	}
}
