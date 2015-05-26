using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Shopping;
using System;
using System.Web;
namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class ExpressData : System.Web.IHttpHandler
	{
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
		public void ProcessRequest(System.Web.HttpContext context)
		{
			try
			{
				this.SearchExpressData(context);
			}
			catch
			{
			}
		}
		private void SearchExpressData(System.Web.HttpContext context)
		{
			string text = "{";
			if (!string.IsNullOrEmpty(context.Request["OrderId"]))
			{
				string orderId = context.Request["OrderId"];
				OrderInfo orderInfo = ShoppingProcessor.GetOrderInfo(orderId);
				if (orderInfo != null)
				{
					bool arg_60_0;
					if (orderInfo.OrderStatus != OrderStatus.SellerAlreadySent)
					{
						if (orderInfo.OrderStatus != OrderStatus.Finished)
						{
							arg_60_0 = true;
							goto IL_60;
						}
					}
					arg_60_0 = string.IsNullOrEmpty(orderInfo.ExpressCompanyAbb);
					IL_60:
					if (!arg_60_0)
					{
						string expressData = Express.GetExpressData(orderInfo.ExpressCompanyAbb, orderInfo.ShipOrderNumber);
						text = text + "\"Express\":\"" + expressData + "\"";
					}
				}
			}
			else
			{
				if (!string.IsNullOrEmpty(context.Request["PurchaseOrderId"]))
				{
					string purchaseOrderId = context.Request["PurchaseOrderId"];
					PurchaseOrderInfo purchaseOrder = ShoppingProcessor.GetPurchaseOrder(purchaseOrderId);
					if (purchaseOrder != null)
					{
						bool arg_E9_0;
						if (purchaseOrder.PurchaseStatus != OrderStatus.SellerAlreadySent)
						{
							if (purchaseOrder.PurchaseStatus != OrderStatus.Finished)
							{
								arg_E9_0 = true;
								goto IL_E9;
							}
						}
						arg_E9_0 = string.IsNullOrEmpty(purchaseOrder.ExpressCompanyAbb);
						IL_E9:
						if (!arg_E9_0)
						{
							string expressData = Express.GetExpressData(purchaseOrder.ExpressCompanyAbb, purchaseOrder.ShipOrderNumber);
							text = text + "\"Express\":\"" + expressData + "\"";
						}
					}
				}
			}
			text += "}";
			context.Response.ContentType = "text/plain";
			context.Response.Write(text);
			context.Response.End();
		}
	}
}
