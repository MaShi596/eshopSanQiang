using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.UI.Common.Controls;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	public class PurchaseOrder_ShippingAddress : System.Web.UI.UserControl
	{
		private PurchaseOrderInfo purchaseOrder;
		protected string edit = "";
		protected System.Web.UI.HtmlControls.HtmlTableRow tr_company;
		protected System.Web.UI.WebControls.Literal litCompanyName;
		protected System.Web.UI.WebControls.Literal lblShipAddress;
		protected System.Web.UI.WebControls.Label lkBtnEditShippingAddress;
		protected System.Web.UI.WebControls.Literal litShipToDate;
		protected System.Web.UI.WebControls.Literal litModeName;
		protected FormatedTimeLabel lblPurchaseDate;
		protected System.Web.UI.WebControls.Label litRemark;
		protected System.Web.UI.WebControls.TextBox txtpost;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hdtagId;
		protected System.Web.UI.WebControls.Button btnupdatepost;
		protected System.Web.UI.WebControls.Panel plExpress;
		protected System.Web.UI.HtmlControls.HtmlAnchor power;
		public PurchaseOrderInfo PurchaseOrder
		{
			get
			{
				return this.purchaseOrder;
			}
			set
			{
				this.purchaseOrder = value;
			}
		}
		protected override void OnLoad(System.EventArgs e)
		{
			if (!this.Page.IsPostBack)
			{
				this.LoadControl();
			}
			OrderStatus arg_1E_0 = this.purchaseOrder.PurchaseStatus;
			if ((this.purchaseOrder.PurchaseStatus == OrderStatus.SellerAlreadySent || this.purchaseOrder.PurchaseStatus == OrderStatus.Finished) && !string.IsNullOrEmpty(this.purchaseOrder.ExpressCompanyAbb))
			{
				if (this.plExpress != null)
				{
					this.plExpress.Visible = true;
				}
				if (Express.GetExpressType() == "kuaidi100" && this.power != null)
				{
					this.power.Visible = true;
				}
			}
			this.btnupdatepost.Click += new System.EventHandler(this.btnupdatepost_Click);
		}
		public void LoadControl()
		{
			string text = string.Empty;
			if (!string.IsNullOrEmpty(this.purchaseOrder.ShippingRegion))
			{
				text = this.PurchaseOrder.ShippingRegion;
			}
			if (!string.IsNullOrEmpty(this.purchaseOrder.Address))
			{
				text += this.PurchaseOrder.Address;
			}
			if (!string.IsNullOrEmpty(this.purchaseOrder.ZipCode))
			{
				text = text + "，" + this.purchaseOrder.ZipCode;
			}
			if (!string.IsNullOrEmpty(this.PurchaseOrder.ShipTo))
			{
				text = text + "，" + this.purchaseOrder.ShipTo;
			}
			if (!string.IsNullOrEmpty(this.purchaseOrder.TelPhone))
			{
				text = text + "，" + this.purchaseOrder.TelPhone;
			}
			if (!string.IsNullOrEmpty(this.purchaseOrder.CellPhone))
			{
				text = text + "，" + this.purchaseOrder.CellPhone;
			}
			this.lblShipAddress.Text = text;
			if (this.purchaseOrder.PurchaseStatus == OrderStatus.WaitBuyerPay || this.purchaseOrder.PurchaseStatus == OrderStatus.BuyerAlreadyPaid)
			{
				this.lkBtnEditShippingAddress.Visible = true;
			}
			this.litShipToDate.Text = this.purchaseOrder.ShipToDate;
			if (this.purchaseOrder.PurchaseStatus != OrderStatus.Finished)
			{
				if (this.purchaseOrder.PurchaseStatus != OrderStatus.SellerAlreadySent)
				{
					this.litModeName.Text = this.purchaseOrder.ModeName;
					goto IL_1AD;
				}
			}
			this.txtpost.Text = this.purchaseOrder.ShipOrderNumber;
			this.litModeName.Text = this.purchaseOrder.RealModeName + " 发货单号：" + this.purchaseOrder.ShipOrderNumber;
			IL_1AD:
			if (!string.IsNullOrEmpty(this.purchaseOrder.ExpressCompanyName))
			{
				this.litCompanyName.Text = this.purchaseOrder.ExpressCompanyName;
				this.tr_company.Visible = true;
			}
			this.litRemark.Text = this.purchaseOrder.Remark;
			this.lblPurchaseDate.Time = this.purchaseOrder.PurchaseDate;
		}
		private void btnupdatepost_Click(object sender, System.EventArgs e)
		{
			this.purchaseOrder.ShipOrderNumber = this.txtpost.Text;
			OrderHelper.EditPurchaseOrderShipNumber(this.purchaseOrder.PurchaseOrderId, this.purchaseOrder.OrderId, this.purchaseOrder.ShipOrderNumber);
			this.ShowMsg("修改发货单号成功", true);
			this.LoadControl();
		}
		protected virtual void ShowMsg(string msg, bool success)
		{
			string str = string.Format("ShowMsg(\"{0}\", {1})", msg, success ? "true" : "false");
			if (!this.Page.ClientScript.IsClientScriptBlockRegistered("ServerMessageScript"))
			{
				this.Page.ClientScript.RegisterStartupScript(base.GetType(), "ServerMessageScript", "<script language='JavaScript' defer='defer'>setTimeout(function(){" + str + "},300);</script>");
			}
		}
	}
}
