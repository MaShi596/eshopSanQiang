using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Sales;
using Hidistro.Subsites.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin.purchaseOrder
{
	public class BatchPay : DistributorPage
	{
		protected FormatedMoneyLabel lblUseableBalance;
		protected FormatedMoneyLabel lblTotalPrice;
		protected System.Web.UI.WebControls.TextBox txtTradePassword;
		protected System.Web.UI.WebControls.Button btnConfirmPay;
		protected System.Web.UI.HtmlControls.HtmlGenericControl PaySucceess;
		protected System.Web.UI.WebControls.ImageButton imgBtnBack;
		private string purchaseorderIds;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(base.Request["purchaseorderIds"]))
			{
				return;
			}
			this.purchaseorderIds = base.Request["purchaseorderIds"].Trim(new char[]
			{
				','
			});
			this.lblTotalPrice.Money = this.GetPayTotal();
			Hidistro.Membership.Context.Distributor distributor = SubsiteStoreHelper.GetDistributor();
			this.lblUseableBalance.Money = distributor.Balance - distributor.RequestBalance;
			this.btnConfirmPay.Click += new System.EventHandler(this.btnConfirmPay_Click);
			this.imgBtnBack.Click += new System.Web.UI.ImageClickEventHandler(this.btnBack_Click);
		}
		private decimal GetPayTotal()
		{
			decimal num = 0m;
			new System.Collections.Generic.List<PurchaseOrderInfo>();
			string[] array = this.purchaseorderIds.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				string purchaseOrderId = array[i];
				PurchaseOrderInfo purchaseOrder = SubsiteSalesHelper.GetPurchaseOrder(purchaseOrderId);
				if (purchaseOrder.PurchaseStatus == OrderStatus.WaitBuyerPay && purchaseOrder.Gateway != "hishop.plugins.payment.podrequest")
				{
					num += purchaseOrder.GetPurchaseTotal();
				}
			}
			return num;
		}
		private void btnConfirmPay_Click(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.txtTradePassword.Text))
			{
				this.ShowMsg("请输入交易密码", false);
				return;
			}
			if ((decimal)this.lblUseableBalance.Money < (decimal)this.lblTotalPrice.Money)
			{
				this.ShowMsg("您的预付款金额不足", false);
				return;
			}
			Hidistro.Membership.Context.Distributor distributor = SubsiteStoreHelper.GetDistributor();
			if (distributor.Balance - distributor.RequestBalance < (decimal)this.lblTotalPrice.Money)
			{
				this.ShowMsg("您的预付款金额不足", false);
				return;
			}
			if ((decimal)this.lblTotalPrice.Money == 0m)
			{
				this.ShowMsg("付款失败，采购单实付款共计不能为0", false);
				return;
			}
			string text = string.Empty;
			int num = 0;
			string[] array = this.purchaseorderIds.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				string text2 = array[i];
				if (SubsiteSalesHelper.GetNotPayment(text2))
				{
					text = text + text2 + ",";
					num++;
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				text = text.Substring(0, text.Length - 1);
			}
			if (num == 0)
			{
				this.ShowMsg("您所选的采购单没有待付款的采购单", false);
				return;
			}
			BalanceDetailInfo balanceDetailInfo = new BalanceDetailInfo();
			balanceDetailInfo.UserId = distributor.UserId;
			balanceDetailInfo.UserName = distributor.Username;
			balanceDetailInfo.TradeType = TradeTypes.Consume;
			balanceDetailInfo.TradeDate = System.DateTime.Now;
			balanceDetailInfo.Expenses = new decimal?((decimal)this.lblTotalPrice.Money);
			balanceDetailInfo.Balance = distributor.Balance - (decimal)this.lblTotalPrice.Money;
			balanceDetailInfo.Remark = " 批量付款的采购单编号：" + text;
			distributor.TradePassword = this.txtTradePassword.Text;
			if (!Hidistro.Membership.Context.Users.ValidTradePassword(distributor))
			{
				this.ShowMsg("交易密码错误", false);
				return;
			}
			if (!SubsiteSalesHelper.BatchConfirmPay(balanceDetailInfo, text))
			{
				this.ShowMsg("付款失败", false);
				return;
			}
			int num2 = 0;
			string[] array2 = this.purchaseorderIds.Split(new char[]
			{
				','
			});
			for (int j = 0; j < array2.Length; j++)
			{
				string purchaseOrderId = array2[j];
				SubsiteSalesHelper.SavePurchaseDebitNote(new PurchaseDebitNote
				{
					NoteId = Globals.GetGenerateId() + num2,
					PurchaseOrderId = purchaseOrderId,
					Operator = Hidistro.Membership.Context.HiContext.Current.User.Username,
					Remark = "分销商采购单预付款支付成功"
				});
				num2++;
			}
			this.PaySucceess.Visible = true;
		}
		private void btnBack_Click(object sender, System.EventArgs e)
		{
			base.Response.Redirect("ManageMyManualPurchaseOrder.aspx");
		}
	}
}
