using Hidistro.ControlPanel.Sales;
using Hidistro.Entities.Sales;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.HtmlControls;
namespace Hidistro.UI.Web.Admin.sales
{
	public class BatchPrintSendOrderGoods : AdminPage
	{
		protected System.Web.UI.HtmlControls.HtmlHead Head1;
		protected System.Web.UI.HtmlControls.HtmlGenericControl divContent;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			string orderIds = base.Request["orderIds"].Trim(new char[]
			{
				','
			});
			if (string.IsNullOrEmpty(base.Request["orderIds"]))
			{
				return;
			}
			foreach (OrderInfo current in this.GetPrintData(orderIds))
			{
				System.Web.UI.HtmlControls.HtmlGenericControl htmlGenericControl = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
				htmlGenericControl.Attributes["class"] = "order print";
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder("");
				stringBuilder.AppendFormat("<div class=\"info\"><div class=\"prime-info\" style=\"margin-right: 20px;\"><p><span><h3 style=\"font-weight: normal\">{0}</h3></span></p></div><ul class=\"sub-info\"><li><span>生成时间： </span>{1}</li><li><span>订单编号： </span>{2}</li></ul><br class=\"clear\" /></div>", current.ShipTo, current.OrderDate.ToString("yyyy-MM-dd HH:mm"), current.OrderId);
				stringBuilder.Append("<table><col class=\"col-0\" /><col class=\"col-1\" /><col class=\"col-2\" /><col class=\"col-3\" /><col class=\"col-4\" /><col class=\"col-5\" /><thead><tr><th>货号</th><th>商品名称</th><th>规格</th><th>数量</th><th>单价</th><th>总价</th></tr></thead><tbody>");
				System.Collections.Generic.Dictionary<string, LineItemInfo> lineItems = current.LineItems;
				if (lineItems != null)
				{
					foreach (string current2 in lineItems.Keys)
					{
						LineItemInfo lineItemInfo = lineItems[current2];
						stringBuilder.AppendFormat("<tr><td>{0}</td>", lineItemInfo.SKU);
						stringBuilder.AppendFormat("<td>{0}</td>", lineItemInfo.ItemDescription);
						stringBuilder.AppendFormat("<td>{0}</td>", lineItemInfo.SKUContent);
						stringBuilder.AppendFormat("<td>{0}</td>", lineItemInfo.ShipmentQuantity);
						stringBuilder.AppendFormat("<td>{0}</td>", System.Math.Round(lineItemInfo.ItemListPrice, 2));
						stringBuilder.AppendFormat("<td>{0}</td></tr>", System.Math.Round(lineItemInfo.GetSubTotal(), 2));
					}
				}
				string value = "";
				System.Collections.Generic.IList<OrderGiftInfo> gifts = current.Gifts;
				if (gifts != null && gifts.Count > 0)
				{
					OrderGiftInfo orderGiftInfo = gifts[0];
					value = string.Format("<li><span>赠送礼品：</span>{0},数量：{1}</li>", orderGiftInfo.GiftName, orderGiftInfo.Quantity);
				}
				stringBuilder.AppendFormat("</tbody></table><ul class=\"price\"><li><span>商品总价： </span>{0}</li><li><span>运费： </span>{1}</li>", System.Math.Round(current.GetAmount(), 2), System.Math.Round(current.AdjustedFreight, 2));
				decimal reducedPromotionAmount = current.ReducedPromotionAmount;
				if (reducedPromotionAmount > 0m)
				{
					stringBuilder.AppendFormat("<li><span>优惠金额：</span>{0}</li>", System.Math.Round(reducedPromotionAmount, 2));
				}
				decimal payCharge = current.PayCharge;
				if (payCharge > 0m)
				{
					stringBuilder.AppendFormat("<li><span>支付手续费：</span>{0}</li>", System.Math.Round(payCharge, 2));
				}
				if (!string.IsNullOrEmpty(current.CouponCode))
				{
					decimal couponValue = current.CouponValue;
					if (couponValue > 0m)
					{
						stringBuilder.AppendFormat("<li><span>优惠券：</span>{0}</li>", System.Math.Round(couponValue, 2));
					}
				}
				decimal adjustedDiscount = current.AdjustedDiscount;
				if (adjustedDiscount > 0m)
				{
					stringBuilder.AppendFormat("<li><span>管理员手工打折：</span>{0}</li>", System.Math.Round(adjustedDiscount, 2));
				}
				stringBuilder.Append(value);
				stringBuilder.AppendFormat("<li><span>实付金额：</span>{0}</li></ul><br class=\"clear\" /><br><br>", System.Math.Round(current.GetTotal(), 2));
				htmlGenericControl.InnerHtml = stringBuilder.ToString();
				this.divContent.Controls.AddAt(0, htmlGenericControl);
			}
		}
		private System.Collections.Generic.List<OrderInfo> GetPrintData(string orderIds)
		{
			System.Collections.Generic.List<OrderInfo> list = new System.Collections.Generic.List<OrderInfo>();
			string[] array = orderIds.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				string orderId = array[i];
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderId);
				list.Add(orderInfo);
			}
			return list;
		}
	}
}
