using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	public class Order_ShippingAddress : System.Web.UI.UserControl
	{
		protected System.Web.UI.HtmlControls.HtmlTableRow tr_company;
		protected System.Web.UI.WebControls.Literal litCompanyName;
		protected System.Web.UI.WebControls.Literal lblShipAddress;
		protected System.Web.UI.WebControls.Label lkBtnEditShippingAddress;
		protected System.Web.UI.WebControls.Literal litShipToDate;
		protected System.Web.UI.WebControls.Literal litModeName;
		protected System.Web.UI.WebControls.Label litRemark;
		protected System.Web.UI.WebControls.TextBox txtpost;
		protected System.Web.UI.WebControls.Button btnupdatepost;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hdtagId;
		protected System.Web.UI.WebControls.Panel plExpress;
		protected System.Web.UI.HtmlControls.HtmlAnchor power;
		protected string edit = "";
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
			if (!this.Page.IsPostBack)
			{
				this.LoadControl();
			}
			OrderStatus arg_1E_0 = this.order.OrderStatus;
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
			this.btnupdatepost.Click += new System.EventHandler(this.btnupdatepost_Click);
		}
		private void btnupdatepost_Click(object sender, System.EventArgs e)
		{
			this.order.ShipOrderNumber = this.txtpost.Text;
			OrderHelper.SetOrderShipNumber(this.order.OrderId, this.order.ShipOrderNumber);
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
			if (this.order.OrderStatus != OrderStatus.WaitBuyerPay)
			{
				if (this.order.OrderStatus != OrderStatus.BuyerAlreadyPaid)
				{
					this.lkBtnEditShippingAddress.Visible = false;
					goto IL_130;
				}
			}
			this.lkBtnEditShippingAddress.Visible = true;
			IL_130:
			this.litShipToDate.Text = this.order.ShipToDate;
			if (this.order.OrderStatus != OrderStatus.Finished)
			{
				if (this.order.OrderStatus != OrderStatus.SellerAlreadySent)
				{
					this.litModeName.Text = this.order.ModeName;
					goto IL_1BD;
				}
			}
			this.litModeName.Text = this.order.RealModeName + " 发货单号：" + this.order.ShipOrderNumber;
			this.txtpost.Text = this.order.ShipOrderNumber;
			IL_1BD:
			if (!string.IsNullOrEmpty(this.order.ExpressCompanyName))
			{
				this.litCompanyName.Text = this.order.ExpressCompanyName;
				this.tr_company.Visible = true;
			}
			this.litRemark.Text = this.order.Remark;
		}
	}
}
