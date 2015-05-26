using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Hishop.Web.CustomMade;
using System;
using System.Data;
using System.Net;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.OrderSendGoods)]
	public class Supplier_Admin_ShipOrderSendGoods : AdminPage
	{
		protected System.Web.UI.WebControls.Label lblOrderId;
		protected FormatedTimeLabel lblOrderTime;
		protected ShippingModeRadioButtonList radioShippingMode;
		protected ExpressRadioButtonList expressRadioButtonList;
		protected System.Web.UI.WebControls.TextBox txtShipOrderNumber;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtShipOrderNumberTip;
		protected System.Web.UI.WebControls.Button btnSendGoods;
		protected Supplier_Order_ItemsList itemsList;
		protected System.Web.UI.WebControls.Literal litShippingModeName;
		protected System.Web.UI.WebControls.Literal litReceivingInfo;
		protected System.Web.UI.WebControls.Label litShipToDate;
		protected System.Web.UI.WebControls.Label litRemark;
		private string orderId;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.orderId = this.Page.Request.QueryString["OrderId"];
			OrderInfo orderInfo = OrderHelper.GetOrderInfo(this.orderId);
			this.BindOrderItems(orderInfo);
			this.btnSendGoods.Click += new System.EventHandler(this.btnSendGoods_Click);
			this.radioShippingMode.SelectedIndexChanged += new System.EventHandler(this.radioShippingMode_SelectedIndexChanged);
			if (!this.Page.IsPostBack)
			{
				if (orderInfo == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				this.radioShippingMode.DataBind();
				this.radioShippingMode.SelectedValue = new int?(orderInfo.ShippingModeId);
				this.BindExpressCompany(orderInfo.ShippingModeId);
				this.expressRadioButtonList.SelectedValue = orderInfo.ExpressCompanyAbb;
				this.BindShippingAddress(orderInfo);
				this.litShippingModeName.Text = orderInfo.ModeName;
				this.litShipToDate.Text = orderInfo.ShipToDate;
				this.litRemark.Text = orderInfo.Remark;
				this.txtShipOrderNumber.Text = orderInfo.ShipOrderNumber;
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
				text += order.Address;
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
		private void BindExpressCompany(int modeId)
		{
			this.expressRadioButtonList.ExpressCompanies = SalesHelper.GetExpressCompanysByMode(modeId);
			this.expressRadioButtonList.DataBind();
		}
		private void radioShippingMode_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (this.radioShippingMode.SelectedValue.HasValue)
			{
				this.BindExpressCompany(this.radioShippingMode.SelectedValue.Value);
			}
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
			if (!this.radioShippingMode.SelectedValue.HasValue)
			{
				this.ShowMsg("请选择配送方式", false);
				return;
			}
			if (string.IsNullOrEmpty(this.txtShipOrderNumber.Text.Trim()) || this.txtShipOrderNumber.Text.Trim().Length > 20)
			{
				this.ShowMsg("运单号码不能为空，在1至20个字符之间", false);
				return;
			}
			if (string.IsNullOrEmpty(this.expressRadioButtonList.SelectedValue))
			{
				this.ShowMsg("请选择物流公司", false);
				return;
			}
			ShippingModeInfo shippingMode = SalesHelper.GetShippingMode(this.radioShippingMode.SelectedValue.Value, true);
			orderInfo.RealShippingModeId = this.radioShippingMode.SelectedValue.Value;
			orderInfo.RealModeName = shippingMode.Name;
			ExpressCompanyInfo expressCompanyInfo = ExpressHelper.FindNode(this.expressRadioButtonList.SelectedValue);
			if (expressCompanyInfo != null)
			{
				orderInfo.ExpressCompanyAbb = expressCompanyInfo.Kuaidi100Code;
				orderInfo.ExpressCompanyName = expressCompanyInfo.Name;
			}
			orderInfo.ShipOrderNumber = this.txtShipOrderNumber.Text;
			if (OrderHelper.SendGoods(orderInfo))
			{
				SendNote sendNote = new SendNote();
				sendNote.NoteId = Globals.GetGenerateId();
				sendNote.OrderId = this.orderId;
				sendNote.Operator = Hidistro.Membership.Context.HiContext.Current.User.Username;
				sendNote.Remark = "后台" + sendNote.Operator + "发货成功";
				OrderHelper.SaveSendNote(sendNote);
				if (this.orderId.IndexOf("OP") != -1)
				{
					string purchaseOrderId = this.orderId.Substring(this.orderId.IndexOf("_") + 1);
					PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(purchaseOrderId);
					if (purchaseOrder == null && !string.IsNullOrEmpty(purchaseOrder.TaobaoOrderId))
					{
						try
						{
							ExpressCompanyInfo expressCompanyInfo2 = ExpressHelper.FindNode(orderInfo.ExpressCompanyName);
							string requestUriString = string.Format("http://order1.kuaidiangtong.com/UpdateShipping.ashx?tid={0}&companycode={1}&outsid={2}", purchaseOrder.TaobaoOrderId, expressCompanyInfo2.TaobaoCode, orderInfo.ShipOrderNumber);
							System.Net.WebRequest webRequest = System.Net.WebRequest.Create(requestUriString);
							webRequest.GetResponse();
						}
						catch
						{
						}
					}
				}
				orderInfo.OnDeliver();
				this.CloseWindow();
				return;
			}
			this.ShowMsg("发货失败", false);
		}
		private void InsertStock(string orderId)
		{
			System.Data.DataTable dataTable = Methods.Supplier_Hishop_OrderItems(orderId);
			System.DateTime now = System.DateTime.Now;
			string stock_Code = System.DateTime.Now.ToString("yyyyMMdd") + Hidistro.Membership.Context.HiContext.Current.User.UserId + Supplier_Admin_ShipOrderSendGoods.GetNumPwd(6);
			string options = "发货出库单";
			int allCount = int.Parse(dataTable.Rows[0]["nums"].ToString());
			int userId = Hidistro.Membership.Context.HiContext.Current.User.UserId;
			string s = Methods.Supplier_StockInfoInsert(now, stock_Code, 2, allCount, options, userId);
			for (int i = 0; i < dataTable.Rows.Count; i++)
			{
				Methods.Supplier_StockItemInsert(int.Parse(s), dataTable.Rows[i]["SkuId"].ToString(), int.Parse(dataTable.Rows[i]["ShipmentQuantity"].ToString()), decimal.Parse(dataTable.Rows[i]["CostPrice"].ToString()));
			}
		}
		public static string GetNumPwd(int numCount)
		{
			string text = "0123456789";
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			for (int i = 0; i < numCount; i++)
			{
				stringBuilder.Append(text[new System.Random(System.Guid.NewGuid().GetHashCode()).Next(0, text.Length - 1)]);
			}
			return stringBuilder.ToString();
		}
	}
}
