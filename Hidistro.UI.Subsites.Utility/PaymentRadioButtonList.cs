using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Store;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Subsites.Utility
{
	public class PaymentRadioButtonList : System.Web.UI.WebControls.RadioButtonList
	{
		public PaymentRadioButtonList()
		{
			base.Items.Clear();
			System.Collections.Generic.IList<PaymentModeInfo> paymentModes = SubsiteStoreHelper.GetPaymentModes();
			foreach (PaymentModeInfo current in paymentModes)
			{
				string text = current.Gateway.ToLower();
				if (current.IsUseInpour && !text.Equals("hishop.plugins.payment.advancerequest") && !text.Equals("hishop.plugins.payment.bankrequest") && !text.Equals("hishop.plugins.payment.codrequest"))
				{
					if (text.Equals("hishop.plugins.payment.alipay_shortcut.shortcutrequest"))
					{
						System.Web.HttpCookie httpCookie = Hidistro.Membership.Context.HiContext.Current.Context.Request.Cookies["Token_" + Hidistro.Membership.Context.HiContext.Current.User.UserId.ToString()];
						if (httpCookie != null && !string.IsNullOrEmpty(httpCookie.Value))
						{
							this.Items.Add(new System.Web.UI.WebControls.ListItem(Globals.HtmlDecode(current.Name), current.ModeId.ToString(System.Globalization.CultureInfo.InvariantCulture)));
						}
					}
					else
					{
						this.Items.Add(new System.Web.UI.WebControls.ListItem(Globals.HtmlDecode(current.Name), current.ModeId.ToString(System.Globalization.CultureInfo.InvariantCulture)));
					}
				}
			}
			this.SelectedIndex = 0;
			this.RepeatDirection = System.Web.UI.WebControls.RepeatDirection.Horizontal;
		}
	}
}
