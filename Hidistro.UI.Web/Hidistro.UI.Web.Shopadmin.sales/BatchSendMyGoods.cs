using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Core.Enums;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Messages;
using Hidistro.Subsites.Sales;
using Hidistro.UI.Subsites.Utility;
using Hishop.Plugins;
using System;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin.sales
{
	public class BatchSendMyGoods : DistributorPage
	{
		protected Pager pager1;
		protected Grid grdOrderGoods;
		protected DropdownColumn dropShippId;
		protected Pager pager2;
		protected System.Web.UI.WebControls.Button btnBatchSendGoods;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnBatchSendGoods.Click += new System.EventHandler(this.btnSendGoods_Click);
			if (!this.Page.IsPostBack)
			{
				this.BindData();
			}
		}
		private void btnSendGoods_Click(object sender, System.EventArgs e)
		{
			if (this.grdOrderGoods.Rows.Count <= 0)
			{
				this.ShowMsg("没有要进行发货的订单。", false);
				return;
			}
			DropdownColumn dropdownColumn = (DropdownColumn)this.grdOrderGoods.Columns[4];
			System.Web.UI.WebControls.ListItemCollection selectedItems = dropdownColumn.SelectedItems;
			int num = 0;
			for (int i = 0; i < selectedItems.Count; i++)
			{
				string orderId = (string)this.grdOrderGoods.DataKeys[this.grdOrderGoods.Rows[i].RowIndex].Value;
				System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)this.grdOrderGoods.Rows[i].FindControl("txtShippOrderNumber");
				System.Web.UI.WebControls.ListItem listItem = selectedItems[i];
				int num2 = 0;
				int.TryParse(listItem.Value, out num2);
				OrderInfo orderInfo = SubsiteSalesHelper.GetOrderInfo(orderId);
				if (orderInfo != null && (orderInfo.GroupBuyId <= 0 || orderInfo.GroupBuyStatus == GroupBuyStatus.Success) && orderInfo.OrderStatus == OrderStatus.BuyerAlreadyPaid && num2 > 0 && !string.IsNullOrEmpty(textBox.Text) && textBox.Text.Length <= 20)
				{
					ShippingModeInfo shippingMode = SubsiteSalesHelper.GetShippingMode(num2, true);
					orderInfo.RealShippingModeId = shippingMode.ModeId;
					orderInfo.RealModeName = shippingMode.Name;
					orderInfo.ShipOrderNumber = textBox.Text;
					if (SubsiteSalesHelper.SendGoods(orderInfo))
					{
						if (!string.IsNullOrEmpty(orderInfo.GatewayOrderId) && orderInfo.GatewayOrderId.Trim().Length > 0)
						{
							PaymentModeInfo paymentMode = SubsiteSalesHelper.GetPaymentMode(orderInfo.PaymentTypeId);
							if (paymentMode != null)
							{
								PaymentRequest paymentRequest = PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), orderInfo.OrderId, orderInfo.GetTotal(), "订单发货", "订单号-" + orderInfo.OrderId, orderInfo.EmailAddress, orderInfo.OrderDate, Globals.FullPath(Globals.GetSiteUrls().Home), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentReturn_url", new object[]
								{
									paymentMode.Gateway
								})), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentNotify_url", new object[]
								{
									paymentMode.Gateway
								})), "");
								paymentRequest.SendGoods(orderInfo.GatewayOrderId, orderInfo.RealModeName, orderInfo.ShipOrderNumber, "EXPRESS");
							}
						}
						int num3 = orderInfo.UserId;
						if (num3 == 1100)
						{
							num3 = 0;
						}
						Hidistro.Membership.Core.IUser user = Hidistro.Membership.Context.Users.GetUser(num3);
						Messenger.OrderShipping(orderInfo, user);
						orderInfo.OnDeliver();
					}
					num++;
				}
			}
			if (num == 0)
			{
				this.ShowMsg("批量发货失败！，发货数量0个", false);
				return;
			}
			if (num > 0)
			{
				this.BindData();
				this.ShowMsg(string.Format("批量发货成功！，发货数量{0}个", num), true);
			}
		}
		private void BindData()
		{
            DropdownColumn column = (DropdownColumn)this.grdOrderGoods.Columns[4];
            column.DataSource = SalesHelper.GetShippingModes();
            DbQueryResult sendGoodsOrders = SubsiteSalesHelper.GetSendGoodsOrders(this.GetOrderQuery());
            this.grdOrderGoods.DataSource = sendGoodsOrders.Data;
            this.grdOrderGoods.DataBind();
            this.pager2.TotalRecords = this.pager1.TotalRecords = sendGoodsOrders.TotalRecords;
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
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["GroupBuyId"]))
			{
				orderQuery.GroupBuyId = new int?(int.Parse(this.Page.Request.QueryString["GroupBuyId"]));
			}
			orderQuery.PageIndex = this.pager1.PageIndex;
			orderQuery.PageSize = this.pager1.PageSize;
			orderQuery.SortBy = "OrderDate";
			orderQuery.SortOrder = SortAction.Desc;
			return orderQuery;
		}
	}
}
