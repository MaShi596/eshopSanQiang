using Hidistro.Core;
using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using Hishop.Plugins;
using System;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class ReChargeConfirm : DistributorPage
	{
		protected System.Web.UI.WebControls.Literal litRealName;
		protected System.Web.UI.WebControls.Literal lblPaymentName;
		protected FormatedMoneyLabel lblBlance;
		protected System.Web.UI.WebControls.Literal litPayCharge;
		protected System.Web.UI.WebControls.Button btnConfirm;
		private int paymentModeId;
		private decimal balance;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			int.TryParse(this.Page.Request.QueryString["modeId"], out this.paymentModeId);
			decimal.TryParse(this.Page.Request.QueryString["blance"], out this.balance);
			this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
			if (!this.Page.IsPostBack && this.paymentModeId > 0 && this.balance > 0m)
			{
				PaymentModeInfo paymentMode = SubsiteStoreHelper.GetPaymentMode(this.paymentModeId);
				this.litRealName.Text = Hidistro.Membership.Context.HiContext.Current.User.Username;
				if (paymentMode != null)
				{
					this.lblPaymentName.Text = paymentMode.Name;
					this.lblBlance.Money = this.balance;
					this.ViewState["PayCharge"] = paymentMode.CalcPayCharge(this.balance);
					this.litPayCharge.Text = Globals.FormatMoney(paymentMode.CalcPayCharge(this.balance));
				}
			}
		}
		private void btnConfirm_Click(object sender, System.EventArgs e)
		{
			PaymentModeInfo paymentMode = SubsiteStoreHelper.GetPaymentMode(this.paymentModeId);
			InpourRequestInfo inpourRequestInfo = new InpourRequestInfo
			{
				InpourId = this.GenerateInpourId(),
				TradeDate = System.DateTime.Now,
				InpourBlance = this.balance,
				UserId = Hidistro.Membership.Context.HiContext.Current.User.UserId,
				PaymentId = paymentMode.ModeId
			};
			if (SubsiteStoreHelper.AddInpourBalance(inpourRequestInfo))
			{
				string attach = "";
				System.Web.HttpCookie httpCookie = Hidistro.Membership.Context.HiContext.Current.Context.Request.Cookies["Token_" + Hidistro.Membership.Context.HiContext.Current.User.UserId.ToString()];
				if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
				{
					attach = httpCookie.Value;
				}
				string text = inpourRequestInfo.InpourId.ToString(System.Globalization.CultureInfo.InvariantCulture);
				PaymentRequest paymentRequest = PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), text, inpourRequestInfo.InpourBlance + paymentMode.CalcPayCharge(inpourRequestInfo.InpourBlance), "预付款充值", "操作流水号-" + text, Hidistro.Membership.Context.HiContext.Current.User.Email, inpourRequestInfo.TradeDate, Globals.FullPath(Globals.GetSiteUrls().Home), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("DistributorInpourReturn_url", new object[]
				{
					paymentMode.Gateway
				})), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("DistributorInpourNotify_url", new object[]
				{
					paymentMode.Gateway
				})), attach);
				paymentRequest.SendRequest();
			}
		}
		private string GenerateInpourId()
		{
			string text = string.Empty;
			System.Random random = new System.Random();
			for (int i = 0; i < 7; i++)
			{
				int num = random.Next();
				text += ((char)(48 + (ushort)(num % 10))).ToString();
			}
			return System.DateTime.Now.ToString("yyyyMMdd") + text;
		}
	}
}
