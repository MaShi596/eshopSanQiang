using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Core.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Messages;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Plugins;
using Hishop.Web.CustomMade;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.OrderSendGoods)]
	public class Supplier_Admin_OrderOption : AdminPage
	{
		protected System.Web.UI.WebControls.Label lblOrderId;
		protected FormatedTimeLabel lblOrderTime;
		protected System.Web.UI.WebControls.Literal litShippingModeName;
		protected System.Web.UI.WebControls.Literal litReceivingInfo;
		protected System.Web.UI.WebControls.Label litShipToDate;
		protected System.Web.UI.WebControls.Label litRemark;
		protected System.Web.UI.WebControls.Literal txtShipPointNameAuto;
		protected System.Web.UI.WebControls.Literal txtShipPointNameAuto2;
		protected System.Web.UI.WebControls.CheckBox btnShipPointSelf;
		protected System.Web.UI.HtmlControls.HtmlGenericControl htmlDivShipPoint;
		protected System.Web.UI.WebControls.Literal txtShipPointName;
		protected System.Web.UI.WebControls.Literal txtShipPointName2;
		protected RegionSelector rsddlRegion;
		protected System.Web.UI.WebControls.Button btnSearchShipPoint;
		protected Grid grdShipPoints;
		protected Supplier_Admin_OrderItemsList itemsList;
		protected System.Web.UI.WebControls.Button btnSendGoods;
		private string orderId;
		private OrderInfo order;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.orderId = this.Page.Request.QueryString["OrderId"];
			this.order = OrderHelper.GetOrderInfo(this.orderId);
			this.BindOrderItems(this.order);
			this.btnSendGoods.Click += new System.EventHandler(this.btnSendGoods_Click);
			this.btnShipPointSelf.CheckedChanged += new System.EventHandler(this.btnShipPointSelf_CheckedChanged);
			this.btnSearchShipPoint.Click += new System.EventHandler(this.btnSearchShipPoint_Click);
			this.grdShipPoints.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdShipPoints_RowCommand);
			if (!this.Page.IsPostBack)
			{
				if (this.order == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				this.BindShippingAddress(this.order);
				this.litShippingModeName.Text = this.order.ModeName;
				this.litShipToDate.Text = this.order.ShipToDate;
				this.litRemark.Text = this.order.Remark;
				this.btnShipPointSelf.AutoPostBack = true;
				string[] array = this.order.ShippingRegion.Split("，".ToCharArray());
				System.Data.DataTable dataTable = null;
				if (array.Length == 3)
				{
					dataTable = Methods.Supplier_ShipPointGetByRegionName(array[0], array[1], array[2]);
					if (dataTable == null || dataTable.Rows.Count == 0)
					{
						dataTable = Methods.Supplier_ShipPointGetByRegionName(array[0], array[1], "");
					}
					if (dataTable == null || dataTable.Rows.Count == 0)
					{
						dataTable = Methods.Supplier_ShipPointGetByRegionName(array[0], "", "");
					}
				}
				if (array.Length == 2)
				{
					dataTable = Methods.Supplier_ShipPointGetByRegionName(array[0], array[1], "");
					if (dataTable == null || dataTable.Rows.Count == 0)
					{
						dataTable = Methods.Supplier_ShipPointGetByRegionName(array[0]);
					}
				}
				if (array.Length == 1)
				{
					dataTable = Methods.Supplier_ShipPointGetByRegionName(array[0], "", "");
				}
				if (dataTable != null && dataTable.Rows.Count > 0)
				{
					dataTable = Methods.Supplier_ShipPointGetByUserId(dataTable.Rows[0]["UserId"].ToString());
					if (dataTable != null && dataTable.Rows.Count > 0)
					{
						string text = "<b>" + (string)dataTable.Rows[0]["username"] + "</b> " + (string)dataTable.Rows[0]["Supplier_RegionName"];
						this.txtShipPointNameAuto.Text = text;
						if (dataTable.Rows[0]["comment"] != System.DBNull.Value)
						{
							this.txtShipPointNameAuto2.Text = (string)dataTable.Rows[0]["comment"];
						}
					}
					else
					{
						this.txtShipPointNameAuto.Text = "未匹配到";
					}
				}
				else
				{
					this.txtShipPointNameAuto.Text = "未匹配到";
				}
				this.rsddlRegion.SetSelectedRegionId(new int?(this.order.RegionId));
				System.Data.DataTable dataTable2 = Methods.Supplier_OrderSupGet(this.orderId);
				if (dataTable2 != null && dataTable2.Rows.Count > 0)
				{
					this.btnShipPointSelf.Checked = true;
					this.htmlDivShipPoint.Attributes.Add("style", "");
					this.BindShipPoints();
				}
			}
		}
		private void grdShipPoints_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			if (e.CommandName == "Select")
			{
				int rowIndex = ((System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer).RowIndex;
				int value = (int)this.grdShipPoints.DataKeys[rowIndex].Value;
				Methods.Supplier_OrderShipPointIdUpdate(this.orderId, new int?(value));
				this.BindShipPoints();
				this.BindShippingAddress(this.order);
			}
		}
		private void btnSearchShipPoint_Click(object sender, System.EventArgs e)
		{
			this.BindShipPoints();
		}
		private void btnShipPointSelf_CheckedChanged(object sender, System.EventArgs e)
		{
			if (this.btnShipPointSelf.Checked)
			{
				this.htmlDivShipPoint.Attributes.Add("style", "");
				Methods.Supplier_OrderShipPointIdUpdate(this.orderId, new int?(0));
				this.BindShipPoints();
				return;
			}
			this.htmlDivShipPoint.Attributes.Add("style", "display:none");
			Methods.Supplier_OrderShipPointIdUpdate(this.orderId, null);
		}
		private void BindShipPoints()
		{
			DbQueryResult dbQueryResult = Methods.Supplier_SGet(new ManagerQuery
			{
				RoleId = new System.Guid("5a26c830-b998-4569-bffc-c5ceae774a7a"),
				PageSize = 100000,
				PageIndex = 1
			}, this.rsddlRegion.SelectedRegions, null);
			this.grdShipPoints.DataSource = dbQueryResult.Data;
			this.grdShipPoints.DataBind();
			System.Data.DataTable dataTable = Methods.Supplier_OrderSupGet(this.orderId);
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				string text = "<b>" + (string)dataTable.Rows[0]["username"] + "</b> " + (string)dataTable.Rows[0]["Supplier_RegionName"];
				this.txtShipPointName.Text = text;
				if (dataTable.Rows[0]["comment"] != System.DBNull.Value)
				{
					this.txtShipPointName2.Text = (string)dataTable.Rows[0]["comment"];
					return;
				}
			}
			else
			{
				this.txtShipPointName.Text = "无";
				this.txtShipPointName2.Text = string.Empty;
			}
		}
		private void BindOrderItems(OrderInfo order)
		{
			this.lblOrderId.Text = order.OrderId;
			this.lblOrderTime.Time = order.OrderDate;
			this.itemsList.Order = order;
		}
		private void BindShippingAddress(OrderInfo order)
		{
			string text = string.Empty;
			if (!string.IsNullOrEmpty(order.ShippingRegion))
			{
				text = order.ShippingRegion;
			}
			if (!string.IsNullOrEmpty(order.Address))
			{
				text = text + " " + order.Address;
			}
			if (!string.IsNullOrEmpty(order.ShipTo))
			{
				text = text + "  " + order.ShipTo;
			}
			if (!string.IsNullOrEmpty(order.ZipCode))
			{
				text = text + "  " + order.ZipCode;
			}
			if (!string.IsNullOrEmpty(order.TelPhone))
			{
				text = text + "  " + order.TelPhone;
			}
			if (!string.IsNullOrEmpty(order.CellPhone))
			{
				text = text + "  " + order.CellPhone;
			}
			this.litReceivingInfo.Text = text;
		}
		private void btnSendGoods_Click(object sender, System.EventArgs e)
		{
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.orderId);
			if (orderInfo == null)
			{
				return;
			}
			if (orderInfo.GroupBuyId > 0 && orderInfo.GroupBuyStatus != GroupBuyStatus.Success)
			{
				this.ShowMsg("当前订单为团购订单，团购活动还未成功结束，所以不能发货", false);
				return;
			}
			if (!orderInfo.CheckAction(OrderActions.SELLER_SEND_GOODS))
			{
				this.ShowMsg("当前订单状态没有付款或不是等待发货的订单，所以不能发货", false);
				return;
			}
			if (Methods.Supplier_OrderIsFenPei(this.orderId))
			{
				this.ShowMsg("生成成功", true);
				return;
			}
			string text = Methods.Supplier_OrderItemSupplierUpdate(orderInfo);
			if (text != "true")
			{
				this.ShowMsg(text, false);
				return;
			}
			orderInfo.RealShippingModeId = 0;
			orderInfo.RealModeName = "配送方式(已实际发货单为准)";
			orderInfo.ShipOrderNumber = string.Format("{0}", string.Format(" <a style=\"color:red;cursor:pointer;\" target=\"_blank\" onclick=\"{0}\">物流详细</a>", "showWindow_ShipInfoPage('" + orderInfo.OrderId + "')"));
			if (OrderHelper.SendGoods(orderInfo))
			{
				Methods.Supplier_OrderItemsSupplierFenPeiOverUpdate(orderInfo.OrderId);
				if (!string.IsNullOrEmpty(orderInfo.GatewayOrderId) && orderInfo.GatewayOrderId.Trim().Length > 0)
				{
					PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode(orderInfo.PaymentTypeId);
					if (paymentMode != null)
					{
						PaymentRequest paymentRequest = PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), orderInfo.OrderId, orderInfo.GetTotal(), "订单发货", "订单号-" + orderInfo.OrderId, orderInfo.EmailAddress, orderInfo.OrderDate, Globals.FullPath(Globals.GetSiteUrls().Home), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentReturn_url", new object[]
						{
							paymentMode.Gateway
						})), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentNotify_url", new object[]
						{
							paymentMode.Gateway
						})), "");
						paymentRequest.SendGoods(orderInfo.GatewayOrderId, orderInfo.RealModeName, "物流单(已实际发货单为准,可在下单网站查看)", "EXPRESS");
					}
				}
				int num = orderInfo.UserId;
				if (num == 1100)
				{
					num = 0;
				}
				Hidistro.Membership.Core.IUser user = Hidistro.Membership.Context.Users.GetUser(num);
				Messenger.OrderShipping(orderInfo, user);
				orderInfo.OnDeliver();
				this.CloseWindow();
				return;
			}
			this.ShowMsg("发货失败", false);
		}
	}
}
