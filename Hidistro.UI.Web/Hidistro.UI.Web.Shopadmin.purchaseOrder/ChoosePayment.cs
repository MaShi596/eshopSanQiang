using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Subsites.Sales;
using Hidistro.Subsites.Store;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin.purchaseOrder
{
	public class ChoosePayment : DistributorPage
	{
		protected DistributorPaymentRadioButtonList radioPaymentMode;
		protected System.Web.UI.WebControls.Button btnSubmit;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
			int num;
			int.TryParse(base.Request["PayMode"], out num);
			if (!this.Page.IsPostBack)
			{
				this.radioPaymentMode.DataBind();
				this.radioPaymentMode.SelectedValue = num.ToString();
			}
		}
		private void btnSubmit_Click(object sender, System.EventArgs e)
		{
			string text = base.Request["PurchaseOrderId"];
			PaymentModeInfo paymentMode = SubsiteStoreHelper.GetPaymentMode(int.Parse(this.radioPaymentMode.SelectedValue));
			if (paymentMode != null)
			{
				SubsiteSalesHelper.SetPayment(text, paymentMode.ModeId, paymentMode.Name, paymentMode.Gateway);
			}
			if (paymentMode != null && paymentMode.Gateway.ToLower().Equals("hishop.plugins.payment.podrequest"))
			{
				this.ShowMsg("您选择的是货到付款方式，请等待主站发货", true);
				return;
			}
			if (paymentMode != null && paymentMode.Gateway.ToLower().Equals("hishop.plugins.payment.bankrequest"))
			{
				this.ShowMsg("您选择的是线下付款方式，请与主站管理员联系", true);
				return;
			}
			base.Response.Redirect(string.Concat(new string[]
			{
				Globals.ApplicationPath,
				"/Shopadmin/purchaseOrder/Pay.aspx?PurchaseOrderId=",
				text,
				"&PayMode=",
				this.radioPaymentMode.SelectedValue
			}));
		}
	}
}
