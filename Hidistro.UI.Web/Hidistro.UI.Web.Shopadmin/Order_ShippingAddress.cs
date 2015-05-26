using Hidistro.Core;
using Hidistro.Entities.Sales;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class Order_ShippingAddress : System.Web.UI.UserControl
	{
		protected System.Web.UI.HtmlControls.HtmlTableRow tr_company;
		protected System.Web.UI.WebControls.Literal litCompanyName;
		protected System.Web.UI.WebControls.Literal lblShipAddress;
		protected System.Web.UI.WebControls.LinkButton lkBtnEditShippingAddress;
		protected System.Web.UI.WebControls.Literal litShipToDate;
		protected System.Web.UI.WebControls.Literal litModeName;
		protected System.Web.UI.WebControls.Literal ltrShipNum;
		protected System.Web.UI.WebControls.Label litRemark;
		protected System.Web.UI.WebControls.Panel plExpress;
		protected System.Web.UI.HtmlControls.HtmlAnchor power;
		private OrderInfo order;
		public OrderInfo Order
		{
			get
			{
				return this.order;
			}
			set
			{
				this.order = value;
			}
		}
		protected override void OnLoad(System.EventArgs e)
		{
			this.LoadControl();
		}
		public void LoadControl()
		{
			string text = string.Empty;
			if (!string.IsNullOrEmpty(this.order.ShippingRegion))
			{
				text = this.order.ShippingRegion;
			}
			if (!string.IsNullOrEmpty(this.order.Address))
			{
				text += this.order.Address;
			}
			if (!string.IsNullOrEmpty(this.order.ShipTo))
			{
				text = text + "   " + this.order.ShipTo;
			}
			if (!string.IsNullOrEmpty(this.order.ZipCode))
			{
				text = text + "   " + this.order.ZipCode;
			}
			if (!string.IsNullOrEmpty(this.order.TelPhone))
			{
				text = text + "   " + this.order.TelPhone;
			}
			if (!string.IsNullOrEmpty(this.order.CellPhone))
			{
				text = text + "   " + this.order.CellPhone;
			}
			this.lblShipAddress.Text = text;
			if (this.order.OrderStatus == OrderStatus.WaitBuyerPay)
			{
				this.lkBtnEditShippingAddress.Visible = true;
			}
			this.litShipToDate.Text = this.order.ShipToDate;
			if (this.order.OrderStatus != OrderStatus.Finished)
			{
				if (this.order.OrderStatus != OrderStatus.SellerAlreadySent)
				{
					this.litModeName.Text = this.order.ModeName;
					goto IL_194;
				}
			}
			this.litModeName.Text = this.order.RealModeName;
			this.ltrShipNum.Text = "  物流单号：" + this.order.ShipOrderNumber;
			IL_194:
			if (!string.IsNullOrEmpty(this.order.ExpressCompanyName))
			{
				this.litCompanyName.Text = this.order.ExpressCompanyName;
				this.tr_company.Visible = true;
			}
			this.litRemark.Text = this.order.Remark;
			if ((this.order.OrderStatus == OrderStatus.SellerAlreadySent || this.order.OrderStatus == OrderStatus.Finished) && !string.IsNullOrEmpty(this.order.ExpressCompanyAbb))
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
