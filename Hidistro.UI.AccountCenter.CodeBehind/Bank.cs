using Hidistro.AccountCenter.Business;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.AccountCenter.CodeBehind
{
	public class Bank : MemberTemplatedWebControl
	{
		private System.Web.UI.WebControls.Label lblPaymentName;
		private System.Web.UI.WebControls.Label lblDescription;
		private string orderId;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "User/Skin-Bank.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			this.lblPaymentName = (System.Web.UI.WebControls.Label)this.FindControl("lblPaymentName");
			this.lblDescription = (System.Web.UI.WebControls.Label)this.FindControl("lblDescription");
			this.orderId = this.Page.Request.QueryString["orderId"];
			PageTitle.AddSiteNameTitle("订单线下支付", HiContext.Current.Context);
			if (string.IsNullOrEmpty(this.orderId))
			{
				base.GotoResourceNotFound();
			}
			if (!this.Page.IsPostBack)
			{
				OrderInfo orderInfo = TradeHelper.GetOrderInfo(this.orderId);
				PaymentModeInfo paymentMode = TradeHelper.GetPaymentMode(orderInfo.PaymentTypeId);
				if (paymentMode != null)
				{
					this.lblPaymentName.Text = paymentMode.Name;
					this.lblDescription.Text = paymentMode.Description;
				}
			}
		}
	}
}
