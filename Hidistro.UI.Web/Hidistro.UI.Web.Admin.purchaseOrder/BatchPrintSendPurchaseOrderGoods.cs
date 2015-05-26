using Hidistro.ControlPanel.Sales;
using Hidistro.Entities.Sales;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
namespace Hidistro.UI.Web.Admin.purchaseOrder
{
	public class BatchPrintSendPurchaseOrderGoods : System.Web.UI.Page
	{
		protected System.Web.UI.HtmlControls.HtmlHead Head1;
		protected System.Web.UI.HtmlControls.HtmlGenericControl divContent;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(base.Request["purchaseorderIds"]))
			{
				return;
			}
			string orderIds = base.Request["purchaseorderIds"].Trim(new char[]
			{
				','
			});
			foreach (PurchaseOrderInfo current in this.GetPrintData(orderIds))
			{
				System.Web.UI.HtmlControls.HtmlGenericControl htmlGenericControl = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
				htmlGenericControl.Attributes["class"] = "order print";
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder("");
				stringBuilder.AppendFormat("<div class=\"info\"><div class=\"prime-info\" style=\"margin-right: 20px;\"><p><span><h3 style=\"font-weight: normal\">{0}</h3></span></p></div><ul class=\"sub-info\"><li><span>生成时间： </span>{1}</li><li><span>采购单编号： </span>{2}</li></ul><br class=\"clear\" /></div>", current.ShipTo, current.PurchaseDate.ToString("yyyy-MM-dd HH:mm"), current.PurchaseOrderId);
				stringBuilder.Append("<table><col class=\"col-0\" /><col class=\"col-1\" /><col class=\"col-2\" /><col class=\"col-3\" /><col class=\"col-4\" /><col class=\"col-5\" /><thead><tr><th>货号</th><th>商品名称</th><th>规格</th><th>数量</th><th>单价</th><th>总价</th></tr></thead><tbody>");
				System.Collections.Generic.IList<PurchaseOrderItemInfo> purchaseOrderItems = current.PurchaseOrderItems;
				if (purchaseOrderItems != null)
				{
					foreach (PurchaseOrderItemInfo current2 in purchaseOrderItems)
					{
						stringBuilder.AppendFormat("<tr><td>{0}</td>", current2.SKU);
						stringBuilder.AppendFormat("<td>{0}</td>", current2.ItemDescription);
						stringBuilder.AppendFormat("<td>{0}</td>", current2.SKUContent);
						stringBuilder.AppendFormat("<td>{0}</td>", current2.Quantity);
						stringBuilder.AppendFormat("<td>{0}</td>", System.Math.Round(current2.ItemListPrice, 2));
						stringBuilder.AppendFormat("<td>{0}</td></tr>", System.Math.Round(current2.GetSubTotal(), 2));
					}
				}
				string value = "";
				System.Collections.Generic.IList<PurchaseOrderGiftInfo> purchaseOrderGifts = current.PurchaseOrderGifts;
				if (purchaseOrderGifts != null && purchaseOrderGifts.Count > 0)
				{
					PurchaseOrderGiftInfo purchaseOrderGiftInfo = purchaseOrderGifts[0];
					value = string.Format("<li><span>赠送礼品：</span>{0},数量：{1}</li>", purchaseOrderGiftInfo.GiftName, purchaseOrderGiftInfo.Quantity);
				}
				stringBuilder.AppendFormat("</tbody></table><ul class=\"price\"><li><span>商品总价： </span>{0}</li><li><span>运费： </span>{1}</li>", System.Math.Round(current.GetProductAmount(), 2), System.Math.Round(current.AdjustedFreight, 2));
				decimal adjustedDiscount = current.AdjustedDiscount;
				if (adjustedDiscount > 0m)
				{
					stringBuilder.AppendFormat("<li><span>管理员手工打折：</span>{0}</li>", System.Math.Round(adjustedDiscount, 2));
				}
				stringBuilder.Append(value);
				stringBuilder.AppendFormat("<li><span>实付金额：</span>{0}</li></ul><br class=\"clear\" /><br><br>", System.Math.Round(current.GetPurchaseTotal(), 2));
				htmlGenericControl.InnerHtml = stringBuilder.ToString();
				this.divContent.Controls.AddAt(0, htmlGenericControl);
			}
		}
		private System.Collections.Generic.List<PurchaseOrderInfo> GetPrintData(string orderIds)
		{
			System.Collections.Generic.List<PurchaseOrderInfo> list = new System.Collections.Generic.List<PurchaseOrderInfo>();
			string[] array = orderIds.Split(new char[]
			{
				','
			});
			for (int i = 0; i < array.Length; i++)
			{
				string purchaseOrderId = array[i];
				PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(purchaseOrderId);
				list.Add(purchaseOrder);
			}
			return list;
		}
	}
}
