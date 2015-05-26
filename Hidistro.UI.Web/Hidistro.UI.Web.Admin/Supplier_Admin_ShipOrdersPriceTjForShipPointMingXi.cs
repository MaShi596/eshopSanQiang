using ASPNET.WebControls;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Web.CustomMade;
using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.Orders)]
	public class Supplier_Admin_ShipOrdersPriceTjForShipPointMingXi : AdminPage
	{
		private int userid;
		protected System.Web.UI.WebControls.HyperLink hlinkAllOrder;
		protected System.Web.UI.WebControls.HyperLink hlinkNotPay;
		protected System.Web.UI.WebControls.HyperLink hlinkYetPay;
		protected System.Web.UI.WebControls.HyperLink hlinkSendGoods;
		protected System.Web.UI.WebControls.HyperLink hlinkTradeFinished;
		protected System.Web.UI.WebControls.HyperLink hlinkClose;
		protected System.Web.UI.WebControls.HyperLink hlinkHistory;
		protected WebCalendar calendarStartDate;
		protected WebCalendar calendarEndDate;
		protected System.Web.UI.WebControls.TextBox txtUserName;
		protected System.Web.UI.WebControls.TextBox txtOrderId;
		protected System.Web.UI.WebControls.Label lblStatus;
		protected System.Web.UI.WebControls.TextBox txtProductName;
		protected System.Web.UI.WebControls.TextBox txtShopTo;
		protected System.Web.UI.WebControls.DropDownList ddlIsPrinted;
		protected ShippingModeDropDownList shippingModeDropDownList;
		protected RegionSelector dropRegion;
		protected System.Web.UI.WebControls.Button btnSearchButton;
		protected PageSize hrefPageSize;
		protected System.Web.UI.HtmlControls.HtmlGenericControl htmlLiShipOrderPriceAll;
		protected System.Web.UI.WebControls.Literal litlShipOrderPriceAll;
		protected Pager pager1;
		protected ImageLinkButton lkbtnDeleteCheck;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hidOrderId;
		protected System.Web.UI.WebControls.DataList dlstOrders;
		protected Pager pager;
		protected CloseTranReasonDropDownList ddlCloseReason;
		protected FormatedMoneyLabel lblOrderTotalForRemark;
		protected OrderRemarkImageRadioButtonList orderRemarkImageForRemark;
		protected System.Web.UI.WebControls.TextBox txtRemark;
		protected System.Web.UI.WebControls.Button btnCloseOrder;
		protected System.Web.UI.WebControls.Button btnRemark;
		protected System.Web.UI.WebControls.Button btnOrderGoods;
		protected System.Web.UI.WebControls.Button btnProductGoods;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["userid"]))
			{
				int.TryParse(System.Web.HttpContext.Current.Request.QueryString["userid"], out this.userid);
			}
			this.btnSearchButton.Click += new System.EventHandler(this.btnSearchButton_Click);
			if (!this.Page.IsPostBack)
			{
				this.BindOrders();
			}
		}
		protected void btnSearchButton_Click(object sender, System.EventArgs e)
		{
			this.ReloadOrders(true);
		}
		private void BindOrders()
		{
			OrderQuery orderQuery = this.GetOrderQuery();
			orderQuery.Status = OrderStatus.SellerAlreadySent;
			DbQueryResult dbQueryResult = Methods.Supplier_ShipOrderSGet(orderQuery, this.userid);
			this.dlstOrders.DataSource = dbQueryResult.Data;
			this.dlstOrders.DataBind();
			this.pager.TotalRecords=dbQueryResult.TotalRecords;
			this.pager1.TotalRecords=dbQueryResult.TotalRecords;
			this.txtUserName.Text = orderQuery.UserName;
			this.txtOrderId.Text = orderQuery.OrderId;
			this.txtProductName.Text = orderQuery.ProductName;
			this.txtShopTo.Text = orderQuery.ShipTo;
            this.calendarStartDate.SelectedDate = orderQuery.StartDate;
            this.calendarEndDate.SelectedDate = orderQuery.EndDate;
			this.lblStatus.Text = ((int)orderQuery.Status).ToString();
			this.shippingModeDropDownList.SelectedValue = orderQuery.ShippingModeId;
			if (orderQuery.IsPrinted.HasValue)
			{
				this.ddlIsPrinted.SelectedValue = orderQuery.IsPrinted.Value.ToString();
			}
			if (orderQuery.RegionId.HasValue)
			{
				this.dropRegion.SetSelectedRegionId(orderQuery.RegionId);
			}
			if (orderQuery.Status == OrderStatus.BuyerAlreadyPaid)
			{
				this.htmlLiShipOrderPriceAll.Visible = false;
				return;
			}
			this.htmlLiShipOrderPriceAll.Visible = true;
			this.litlShipOrderPriceAll.Text = Methods.Supplier_ShipOrderAmountPriceAllGet(orderQuery, this.userid).ToString("f2");
		}
		private OrderQuery GetOrderQuery()
		{
			OrderQuery orderQuery = new OrderQuery();
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
			{
				orderQuery.OrderId = Globals.UrlDecode(this.Page.Request.QueryString["OrderId"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ProductName"]))
			{
				orderQuery.ProductName = Globals.UrlDecode(this.Page.Request.QueryString["ProductName"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ShipTo"]))
			{
				orderQuery.ShipTo = Globals.UrlDecode(this.Page.Request.QueryString["ShipTo"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["UserName"]))
			{
				orderQuery.UserName = Globals.UrlDecode(this.Page.Request.QueryString["UserName"]);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["StartDate"]))
			{
				orderQuery.StartDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["StartDate"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["GroupBuyId"]))
			{
				orderQuery.GroupBuyId = new int?(int.Parse(this.Page.Request.QueryString["GroupBuyId"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["EndDate"]))
			{
				orderQuery.EndDate = new System.DateTime?(System.DateTime.Parse(this.Page.Request.QueryString["EndDate"]));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["OrderStatus"]))
			{
				int status = 0;
				if (int.TryParse(this.Page.Request.QueryString["OrderStatus"], out status))
				{
					orderQuery.Status = (OrderStatus)status;
				}
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["IsPrinted"]))
			{
				int value = 0;
				if (int.TryParse(this.Page.Request.QueryString["IsPrinted"], out value))
				{
					orderQuery.IsPrinted = new int?(value);
				}
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["ModeId"]))
			{
				int value2 = 0;
				if (int.TryParse(this.Page.Request.QueryString["ModeId"], out value2))
				{
					orderQuery.ShippingModeId = new int?(value2);
				}
			}
			int value3;
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["region"]) && int.TryParse(this.Page.Request.QueryString["region"], out value3))
			{
				orderQuery.RegionId = new int?(value3);
			}
			orderQuery.PageIndex = this.pager.PageIndex;
			orderQuery.PageSize = this.pager.PageSize;
			orderQuery.SortBy = "OrderDate";
			orderQuery.SortOrder = SortAction.Desc;
			return orderQuery;
		}
		private void ReloadOrders(bool isSearch)
		{
			System.Collections.Specialized.NameValueCollection nameValueCollection = new System.Collections.Specialized.NameValueCollection();
			nameValueCollection.Add("UserName", this.txtUserName.Text);
			nameValueCollection.Add("OrderId", this.txtOrderId.Text);
			nameValueCollection.Add("ProductName", this.txtProductName.Text);
			nameValueCollection.Add("ShipTo", this.txtShopTo.Text);
			nameValueCollection.Add("PageSize", this.pager.PageSize.ToString());
			nameValueCollection.Add("userid", this.userid.ToString());
			nameValueCollection.Add("OrderStatus", this.lblStatus.Text);
			if (this.calendarStartDate.SelectedDate.HasValue)
			{
				nameValueCollection.Add("StartDate", this.calendarStartDate.SelectedDate.Value.ToString());
			}
			if (this.calendarEndDate.SelectedDate.HasValue)
			{
				nameValueCollection.Add("EndDate", this.calendarEndDate.SelectedDate.Value.ToString());
			}
			if (!isSearch)
			{
				nameValueCollection.Add("pageIndex", this.pager.PageIndex.ToString());
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["GroupBuyId"]))
			{
				nameValueCollection.Add("GroupBuyId", this.Page.Request.QueryString["GroupBuyId"]);
			}
			if (this.shippingModeDropDownList.SelectedValue.HasValue)
			{
				nameValueCollection.Add("ModeId", this.shippingModeDropDownList.SelectedValue.Value.ToString());
			}
			if (!string.IsNullOrEmpty(this.ddlIsPrinted.SelectedValue))
			{
				nameValueCollection.Add("IsPrinted", this.ddlIsPrinted.SelectedValue);
			}
			if (this.dropRegion.GetSelectedRegionId().HasValue)
			{
				nameValueCollection.Add("region", this.dropRegion.GetSelectedRegionId().Value.ToString());
			}
			base.ReloadPage(nameValueCollection);
		}
	}
}
