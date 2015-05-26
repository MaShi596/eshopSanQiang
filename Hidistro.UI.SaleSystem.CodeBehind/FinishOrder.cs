using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Shopping;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI.WebControls;
namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class FinishOrder : HtmlTemplatedWebControl
	{
		private string orderId;
		private System.Web.UI.WebControls.Literal litOrderId;
		private FormatedMoneyLabel litOrderPrice;
		private System.Web.UI.WebControls.Literal litPaymentName;
		private System.Web.UI.WebControls.Button btnSubMitOrder;
		protected override void OnInit(System.EventArgs eventArgs_0)
		{
			if (this.SkinName == null)
			{
				this.SkinName = "Skin-FinishOrder.html";
			}
			base.OnInit(eventArgs_0);
		}
		protected override void AttachChildControls()
		{
			if (this.Page.Request.QueryString["orderId"] == null)
			{
				base.GotoResourceNotFound();
			}
			this.orderId = this.Page.Request.QueryString["orderId"];
			this.litOrderId = (System.Web.UI.WebControls.Literal)this.FindControl("litOrderId");
			this.litOrderPrice = (FormatedMoneyLabel)this.FindControl("litOrderPrice");
			this.litPaymentName = (System.Web.UI.WebControls.Literal)this.FindControl("litPaymentName");
			this.btnSubMitOrder = (System.Web.UI.WebControls.Button)this.FindControl("btnSubMitOrder");
			this.btnSubMitOrder.Click += new System.EventHandler(this.btnSubMitOrder_Click);
			if (!this.Page.IsPostBack)
			{
				this.LoadOrderInfo();
			}
		}
		private void btnSubMitOrder_Click(object sender, System.EventArgs e)
		{
			OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(this.orderId);
			if (orderInfo != null)
			{
				if (orderInfo.Gateway != "hishop.plugins.payment.advancerequest")
				{
					System.Web.HttpContext.Current.Response.Redirect(Globals.GetSiteUrls().UrlData.FormatUrl("sendPayment", new object[]
					{
						this.orderId
					}));
				}
				else
				{
					System.Web.HttpContext.Current.Response.Redirect(string.Format("/user/pay.aspx?OrderId={0}", this.orderId));
				}
			}
		}
		public void LoadOrderInfo()
		{
			OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(this.orderId);
			if (orderInfo != null)
			{
				this.litOrderId.Text = orderInfo.OrderId;
				this.litOrderPrice.Money = orderInfo.GetTotal();
				this.litPaymentName.Text = orderInfo.PaymentType;
				if (orderInfo.Gateway == "hishop.plugins.payment.podrequest")
				{
					this.btnSubMitOrder.Visible = false;
				}
			}
		}
	}
}
