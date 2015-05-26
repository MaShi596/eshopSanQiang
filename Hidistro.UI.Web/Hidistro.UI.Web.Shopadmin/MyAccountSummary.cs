using Hidistro.Entities.Members;
using Hidistro.Subsites.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
namespace Hidistro.UI.Web.Shopadmin
{
	public class MyAccountSummary : DistributorPage
	{
		protected FormatedMoneyLabel lblAccountAmount;
		protected FormatedMoneyLabel lblUseableBalance;
		protected FormatedMoneyLabel lblFreezeBalance;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!base.IsPostBack)
			{
				AccountSummaryInfo myAccountSummary = SubsiteStoreHelper.GetMyAccountSummary();
				this.lblAccountAmount.Money = myAccountSummary.AccountAmount;
				this.lblFreezeBalance.Money = myAccountSummary.FreezeBalance;
				this.lblUseableBalance.Money = myAccountSummary.UseableBalance;
			}
		}
	}
}
