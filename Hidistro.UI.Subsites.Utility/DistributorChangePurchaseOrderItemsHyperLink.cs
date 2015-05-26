using Hidistro.Core;
using Hidistro.Entities.Sales;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Subsites.Utility
{
	public class DistributorChangePurchaseOrderItemsHyperLink : System.Web.UI.WebControls.HyperLink
	{
		public object PurchaseStatusCode
		{
			get
			{
				object result;
				if (this.ViewState["purchaseStatusCode"] == null)
				{
					result = null;
				}
				else
				{
					result = this.ViewState["purchaseStatusCode"];
				}
				return result;
			}
			set
			{
				if (value != null)
				{
					this.ViewState["purchaseStatusCode"] = value;
				}
			}
		}
		public object PurchaseOrderId
		{
			get
			{
				object result;
				if (this.ViewState["PurchaseOrderId"] == null)
				{
					result = null;
				}
				else
				{
					result = this.ViewState["PurchaseOrderId"];
				}
				return result;
			}
			set
			{
				if (value != null)
				{
					this.ViewState["PurchaseOrderId"] = value;
				}
			}
		}
		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			OrderStatus orderStatus = (OrderStatus)this.PurchaseStatusCode;
			if (orderStatus != OrderStatus.WaitBuyerPay)
			{
				base.Visible = false;
				base.Text = string.Empty;
			}
			else
			{
				base.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/purchaseOrder/ChangePurchaseOrderItems.aspx?PurchaseOrderId=" + this.PurchaseOrderId;
			}
			base.Render(writer);
		}
	}
}
