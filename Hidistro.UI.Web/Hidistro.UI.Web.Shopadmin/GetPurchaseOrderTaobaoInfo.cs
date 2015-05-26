using Hidistro.Entities.Sales;
using Hidistro.Subsites.Sales;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Text;
namespace Hidistro.UI.Web.Shopadmin
{
	public class GetPurchaseOrderTaobaoInfo : DistributorPage
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			string text = base.Request.Form["order_id"];
			if (text == null)
			{
				text = "";
			}
			PurchaseOrderTaobaoInfo purchaseOrderTaobaoInfo = SubsiteSalesHelper.GetPurchaseOrderTaobaoInfo(text);
			base.Response.ContentType = "text/plain";
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("{");
			stringBuilder.Append("\"info\":");
			stringBuilder.Append(purchaseOrderTaobaoInfo.ToJson());
			stringBuilder.Append(",\"msg\":\"\",\"result\":\"success\"");
			stringBuilder.Append("}");
			base.Response.Write(stringBuilder.ToString());
		}
	}
}
