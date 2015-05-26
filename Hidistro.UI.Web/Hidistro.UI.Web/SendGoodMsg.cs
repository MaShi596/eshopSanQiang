using Hishop.Web.CustomMade;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web
{
	public class SendGoodMsg : System.Web.UI.Page
	{
		protected System.Web.UI.HtmlControls.HtmlForm form1;
		protected System.Web.UI.WebControls.DataList dlstOrderItems;
		private string orderId = string.Empty;
		private int userid;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["OrderId"]))
			{
				this.orderId = System.Web.HttpContext.Current.Request.QueryString["OrderId"];
			}
			if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["userid"]))
			{
				int.TryParse(System.Web.HttpContext.Current.Request.QueryString["userid"], out this.userid);
			}
			if (this.orderId == string.Empty)
			{
				return;
			}
			System.Data.DataSet dataSource = Methods.Supplier_ShipOrderShipInfoGet(this.orderId, this.userid);
			this.dlstOrderItems.DataSource = dataSource;
			this.dlstOrderItems.DataBind();
		}
	}
}
