using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Subsites.Sales;
using Hidistro.UI.Common.Controls;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class PurchaseOrder_ShippingAddress : System.Web.UI.UserControl
	{
		protected System.Web.UI.HtmlControls.HtmlTableRow tr_company;
		protected System.Web.UI.WebControls.Literal litCompanyName;
		protected System.Web.UI.WebControls.Literal lblShipAddress;
		protected System.Web.UI.WebControls.Literal litShipToDate;
		protected System.Web.UI.WebControls.Literal litModeName;
		protected System.Web.UI.WebControls.Literal ltrShipNum;
		protected FormatedTimeLabel lblPurchaseDate;
		protected System.Web.UI.WebControls.TextBox txtRemark;
		protected System.Web.UI.WebControls.Button btnSaveRemark;
		protected System.Web.UI.WebControls.Panel plExpress;
		protected System.Web.UI.HtmlControls.HtmlAnchor power;
		private PurchaseOrderInfo purchaseOrder;
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
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSaveRemark.Click += new System.EventHandler(this.btnSaveRemark_Click);
			if (!this.Page.IsPostBack)
			{
				this.LoadControl();
			}
		}
		private void btnSaveRemark_Click(object sender, System.EventArgs e)
		{
			if (this.purchaseOrder.PurchaseStatus == OrderStatus.WaitBuyerPay || this.purchaseOrder.PurchaseStatus == OrderStatus.BuyerAlreadyPaid)
			{
				SubsiteSalesHelper.SavePurchaseOrderRemark(this.purchaseOrder.PurchaseOrderId, Globals.HtmlEncode(this.txtRemark.Text));
			}
			base.Response.Redirect(System.Web.HttpContext.Current.Request.Url.ToString(), true);
		}
		public void LoadControl()
		{
			string text = string.Empty;
			if (!string.IsNullOrEmpty(this.PurchaseOrder.ShippingRegion))
			{
				text = this.PurchaseOrder.ShippingRegion;
			}
			if (!string.IsNullOrEmpty(this.PurchaseOrder.Address))
			{
				text += this.PurchaseOrder.Address;
			}
			if (!string.IsNullOrEmpty(this.PurchaseOrder.ZipCode))
			{
				text = text + "," + this.PurchaseOrder.ZipCode;
			}
			if (!string.IsNullOrEmpty(this.PurchaseOrder.ShipTo))
			{
				text = text + "," + this.PurchaseOrder.ShipTo;
			}
			if (!string.IsNullOrEmpty(this.PurchaseOrder.TelPhone))
			{
				text = text + "," + this.PurchaseOrder.TelPhone;
			}
			if (!string.IsNullOrEmpty(this.PurchaseOrder.CellPhone))
			{
				text = text + "," + this.PurchaseOrder.CellPhone;
			}
			this.lblShipAddress.Text = text;
			this.litShipToDate.Text = this.PurchaseOrder.ShipToDate;
			if (this.PurchaseOrder.PurchaseStatus != OrderStatus.Finished)
			{
				if (this.PurchaseOrder.PurchaseStatus != OrderStatus.SellerAlreadySent)
				{
					this.litModeName.Text = this.PurchaseOrder.ModeName;
					goto IL_17A;
				}
			}
			this.litModeName.Text = this.PurchaseOrder.RealModeName;
			this.ltrShipNum.Text = "  物流单号：" + this.PurchaseOrder.ShipOrderNumber;
			IL_17A:
			if (!string.IsNullOrEmpty(this.purchaseOrder.ExpressCompanyName))
			{
				this.litCompanyName.Text = this.purchaseOrder.ExpressCompanyName;
				this.tr_company.Visible = true;
			}
			this.txtRemark.Text = Globals.HtmlDecode(this.PurchaseOrder.Remark);
			this.lblPurchaseDate.Time = this.PurchaseOrder.PurchaseDate;
			if (this.purchaseOrder.PurchaseStatus != OrderStatus.WaitBuyerPay)
			{
				if (this.purchaseOrder.PurchaseStatus != OrderStatus.BuyerAlreadyPaid)
				{
					this.btnSaveRemark.Enabled = false;
					goto IL_21C;
				}
			}
			this.btnSaveRemark.Enabled = true;
			IL_21C:
			if ((this.PurchaseOrder.PurchaseStatus == OrderStatus.SellerAlreadySent || this.PurchaseOrder.PurchaseStatus == OrderStatus.Finished) && !string.IsNullOrEmpty(this.PurchaseOrder.ExpressCompanyAbb))
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
		}
	}
}
