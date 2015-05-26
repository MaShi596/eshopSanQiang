using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Net;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.PurchaseorderSendGood)]
	public class SendPurchaseOrderGoods : AdminPage
	{
		private string purchaseorderId;
		protected ShippingModeRadioButtonList radioShippingMode;
		protected ExpressRadioButtonList expressRadioButtonList;
		protected System.Web.UI.WebControls.TextBox txtShipOrderNumber;
		protected System.Web.UI.WebControls.Button btnSendGoods;
		protected PurchaseOrder_Items itemsList;
		protected System.Web.UI.WebControls.Literal litFreight;
		protected System.Web.UI.WebControls.Literal lblModeName;
		protected System.Web.UI.WebControls.Literal litDiscount;
		protected System.Web.UI.WebControls.Literal litTotalPrice;
		protected System.Web.UI.WebControls.Literal litReceivingInfo;
		protected System.Web.UI.WebControls.Label litShipToDate;
		protected System.Web.UI.WebControls.Literal litShippingModeName;
		protected System.Web.UI.WebControls.Literal litRemark;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.Page.Request.QueryString["PurchaseOrderId"]))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.purchaseorderId = this.Page.Request.QueryString["PurchaseOrderId"];
			PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(this.purchaseorderId);
			if (purchaseOrder == null)
			{
				base.GotoResourceNotFound();
				return;
			}
			this.itemsList.PurchaseOrder = purchaseOrder;
			this.btnSendGoods.Click += new System.EventHandler(this.btnSendGoods_Click);
			this.radioShippingMode.SelectedIndexChanged += new System.EventHandler(this.radioShippingMode_SelectedIndexChanged);
			if (!this.Page.IsPostBack)
			{
				this.radioShippingMode.DataBind();
				this.radioShippingMode.SelectedValue = new int?(purchaseOrder.ShippingModeId);
				this.BindExpressCompany(purchaseOrder.ShippingModeId);
				this.expressRadioButtonList.SelectedValue = purchaseOrder.ExpressCompanyAbb;
				this.BindShippingAddress(purchaseOrder);
				this.BindCharges(purchaseOrder);
				this.txtShipOrderNumber.Text = purchaseOrder.ShipOrderNumber;
				this.litShippingModeName.Text = purchaseOrder.ModeName;
				this.litShipToDate.Text = purchaseOrder.ShipToDate;
				this.litRemark.Text = purchaseOrder.Remark;
			}
		}
		private void BindCharges(PurchaseOrderInfo purchaseOrder)
		{
			this.lblModeName.Text = purchaseOrder.ModeName;
			this.litFreight.Text = Globals.FormatMoney(purchaseOrder.AdjustedFreight);
			this.litDiscount.Text = Globals.FormatMoney(purchaseOrder.AdjustedDiscount);
			this.litTotalPrice.Text = Globals.FormatMoney(purchaseOrder.GetPurchaseTotal());
		}
		private void BindShippingAddress(PurchaseOrderInfo purchaseorder)
		{
			string text = string.Empty;
			if (!string.IsNullOrEmpty(purchaseorder.ShippingRegion))
			{
				text = purchaseorder.ShippingRegion;
			}
			if (!string.IsNullOrEmpty(purchaseorder.Address))
			{
				text += purchaseorder.Address;
			}
			if (!string.IsNullOrEmpty(purchaseorder.ZipCode))
			{
				text = text + "," + purchaseorder.ZipCode;
			}
			if (!string.IsNullOrEmpty(purchaseorder.ShipTo))
			{
				text = text + "," + purchaseorder.ShipTo;
			}
			if (!string.IsNullOrEmpty(purchaseorder.TelPhone))
			{
				text = text + "," + purchaseorder.TelPhone;
			}
			if (!string.IsNullOrEmpty(purchaseorder.CellPhone))
			{
				text = text + "," + purchaseorder.CellPhone;
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
			if (string.IsNullOrEmpty(this.txtShipOrderNumber.Text.Trim()))
			{
				this.ShowMsg("请填写运单号", false);
				return;
			}
			PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(this.purchaseorderId);
			if (purchaseOrder == null)
			{
				return;
			}
			if (!purchaseOrder.CheckAction(PurchaseOrderActions.MASTER_SEND_GOODS))
			{
				this.ShowMsg("当前订单状态没有付款或不是等待发货的订单，所以不能发货", false);
				return;
			}
			if (!this.radioShippingMode.SelectedValue.HasValue)
			{
				this.ShowMsg("请选择配送方式", false);
				return;
			}
			if (string.IsNullOrEmpty(this.expressRadioButtonList.SelectedValue))
			{
				this.ShowMsg("请选择物流配送公司", false);
				return;
			}
			ShippingModeInfo shippingMode = SalesHelper.GetShippingMode(this.radioShippingMode.SelectedValue.Value, true);
			purchaseOrder.RealShippingModeId = this.radioShippingMode.SelectedValue.Value;
			purchaseOrder.RealModeName = shippingMode.Name;
			ExpressCompanyInfo expressCompanyInfo = ExpressHelper.FindNode(this.expressRadioButtonList.SelectedValue);
			if (expressCompanyInfo != null)
			{
				purchaseOrder.ExpressCompanyAbb = expressCompanyInfo.Kuaidi100Code;
				purchaseOrder.ExpressCompanyName = expressCompanyInfo.Name;
			}
			purchaseOrder.ShipOrderNumber = this.txtShipOrderNumber.Text;
			if (SalesHelper.SendPurchaseOrderGoods(purchaseOrder))
			{
				SendNote sendNote = new SendNote();
				sendNote.NoteId = Globals.GetGenerateId();
				sendNote.OrderId = this.purchaseorderId;
				sendNote.Operator = Hidistro.Membership.Context.HiContext.Current.User.Username;
				sendNote.Remark = "后台" + sendNote.Operator + "发货成功";
				SalesHelper.SavePurchaseSendNote(sendNote);
				if (!string.IsNullOrEmpty(purchaseOrder.TaobaoOrderId))
				{
					try
					{
						ExpressCompanyInfo expressCompanyInfo2 = ExpressHelper.FindNode(purchaseOrder.ExpressCompanyName);
						string requestUriString = string.Format("http://order1.kuaidiangtong.com/UpdateShipping.ashx?tid={0}&companycode={1}&outsid={2}", purchaseOrder.TaobaoOrderId, expressCompanyInfo2.TaobaoCode, purchaseOrder.ShipOrderNumber);
						System.Net.WebRequest webRequest = System.Net.WebRequest.Create(requestUriString);
						webRequest.GetResponse();
					}
					catch
					{
					}
				}
				this.ShowMsg("发货成功", true);
				return;
			}
			this.ShowMsg("发货失败", false);
		}
	}
}
