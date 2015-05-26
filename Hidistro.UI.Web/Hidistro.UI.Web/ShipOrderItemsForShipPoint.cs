using Hishop.Web.CustomMade;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web
{
	public class ShipOrderItemsForShipPoint : System.Web.UI.Page
	{
		protected System.Web.UI.HtmlControls.HtmlForm form1;
		protected System.Web.UI.WebControls.Repeater dlstOrderItems;
		private string orderId = string.Empty;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["OrderId"]))
			{
				this.orderId = System.Web.HttpContext.Current.Request.QueryString["OrderId"];
			}
			if (this.orderId == string.Empty)
			{
				return;
			}
			this.dlstOrderItems.DataSource = Methods.Supplier_OrderItemsGet(this.orderId);
			this.dlstOrderItems.DataBind();
		}
	}
}
