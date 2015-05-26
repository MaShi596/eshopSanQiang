using ASPNET.WebControls;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Sales;
using Hidistro.Subsites.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class NoSiteDefault : DistributorPage
	{
		protected System.Web.UI.WebControls.Literal ltrAdminName;
		protected FormatedTimeLabel lblTime;
		protected Grid grdPurchaseOrders;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidPurchaseOrderId;
		protected DistributorClosePurchaseOrderReasonDropDownList ddlCloseReason;
		protected System.Web.UI.WebControls.Button btnClosePurchaseOrder;
		protected System.Web.UI.WebControls.Label lblPurchaseOrderNumbers;
		protected System.Web.UI.WebControls.HyperLink hpkWaitPayPurchaseOrder;
		protected System.Web.UI.WebControls.HyperLink allPurchaseOrder;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.grdPurchaseOrders.RowDataBound += new System.Web.UI.WebControls.GridViewRowEventHandler(this.grdPurchaseOrders_RowDataBound);
			this.btnClosePurchaseOrder.Click += new System.EventHandler(this.btnClosePurchaseOrder_Click);
			if (!base.IsPostBack)
			{
				Hidistro.Membership.Context.Distributor distributor = SubsiteStoreHelper.GetDistributor();
				this.ltrAdminName.Text = distributor.Username;
				this.lblTime.Time = distributor.LastLoginDate;
				this.BindPurchaseOrders();
			}
		}
		private void BindPurchaseOrders()
		{
			int num;
			System.Data.DataTable recentlyManualPurchaseOrders = SubsiteSalesHelper.GetRecentlyManualPurchaseOrders(out num);
			this.lblPurchaseOrderNumbers.Text = recentlyManualPurchaseOrders.Rows.Count.ToString();
			this.hpkWaitPayPurchaseOrder.Text = num.ToString();
			this.allPurchaseOrder.NavigateUrl = Globals.ApplicationPath + "/Shopadmin/purchaseOrder/ManageMyManualPurchaseOrder.aspx";
			this.hpkWaitPayPurchaseOrder.NavigateUrl = Globals.ApplicationPath + string.Format("/Shopadmin/purchaseOrder/ManageMyManualPurchaseOrder.aspx?PurchaseStatus={0}", 1);
			this.grdPurchaseOrders.DataSource = recentlyManualPurchaseOrders;
			this.grdPurchaseOrders.DataBind();
		}
		private void grdPurchaseOrders_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
		{
			if (e.Row.RowType == System.Web.UI.WebControls.DataControlRowType.DataRow)
			{
				System.Web.UI.HtmlControls.HtmlGenericControl htmlGenericControl = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Row.FindControl("lkBtnCancelPurchaseOrder");
				System.Web.UI.WebControls.HyperLink hyperLink = (System.Web.UI.WebControls.HyperLink)e.Row.FindControl("lkbtnPay");
				OrderStatus orderStatus = (OrderStatus)System.Web.UI.DataBinder.Eval(e.Row.DataItem, "PurchaseStatus");
				if (orderStatus == OrderStatus.WaitBuyerPay)
				{
					htmlGenericControl.Visible = true;
					htmlGenericControl.InnerHtml += "<br />";
					hyperLink.Visible = true;
				}
			}
		}
		private void btnClosePurchaseOrder_Click(object sender, System.EventArgs e)
		{
			if (!string.IsNullOrEmpty(this.hidPurchaseOrderId.Value))
			{
				string value = this.hidPurchaseOrderId.Value;
				PurchaseOrderInfo purchaseOrder = SubsiteSalesHelper.GetPurchaseOrder(value);
				purchaseOrder.CloseReason = this.ddlCloseReason.SelectedValue;
				if (SubsiteSalesHelper.ClosePurchaseOrder(purchaseOrder))
				{
					this.BindPurchaseOrders();
					this.ShowMsg("取消采购成功", true);
					return;
				}
				this.ShowMsg("取消采购失败", false);
			}
		}
	}
}
