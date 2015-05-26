using Hidistro.Entities.Sales;
using Hishop.Web.CustomMade;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Common.Controls
{
	public class OrderStatusLabel : Label
	{
		private string orderId = string.Empty;
		private bool isfenpei;
		public object OrderStatusCode
		{
			get
			{
				return this.ViewState["OrderStatusCode"];
			}
			set
			{
				this.ViewState["OrderStatusCode"] = value;
			}
		}
		public override void DataBind()
		{
			object obj = DataBinder.Eval(this.Page.GetDataItem(), "OrderId");
			if (obj != null && obj != DBNull.Value)
			{
				this.orderId = (string)obj;
			}
			try
			{
				obj = DataBinder.Eval(this.Page.GetDataItem(), "IsFenPei");
				if (obj != null && obj != DBNull.Value)
				{
					this.isfenpei = (bool)obj;
				}
			}
			catch
			{
			}
			base.DataBind();
		}
		protected override void Render(HtmlTextWriter writer)
		{
			base.Text = OrderInfo.GetOrderStatusName((OrderStatus)this.OrderStatusCode);
			if ((OrderStatus)this.OrderStatusCode == OrderStatus.SellerAlreadySent)
			{
				base.Text = "<abbr style=\"color:green\">已发货</abbr>";
				if (!Methods.Supplier_ShipOrderHasAllSendGood(this.orderId))
				{
					base.Text = "配货发货中";
				}
				if (string.IsNullOrEmpty(this.orderId))
				{
					this.orderId = HttpContext.Current.Request.QueryString["orderid"];
				}
				base.Text += string.Format(" <a style=\"color:red;cursor:pointer;\" target=\"_blank\" onclick=\"{0}\">查看详细</a>", "showWindow_ShipInfoPage('" + this.orderId + "')");
			}
			base.Render(writer);
		}
	}
}
