using Hidistro.Entities;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.SaleSystem.Shopping;
using Hidistro.Subsites.Commodities;
using Hidistro.Subsites.Sales;
using System;
using System.Data;
using System.Web;
namespace Hidistro.UI.Web.API
{
	public class PurchaseOrderHandler : System.Web.IHttpHandler
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
			context.Response.ContentType = "application/json";
			GzipExtention.Gzip(context);
			string text = context.Request["action"];
			string a;
			if ((a = text) != null)
			{
				if (!(a == "PurchaseOrderAdd"))
				{
					return;
				}
				this.ProcessPurchaseOrderAdd(context);
			}
		}
		private void ProcessPurchaseOrderAdd(System.Web.HttpContext context)
		{
			PurchaseOrderInfo purchaseOrderInfo = new PurchaseOrderInfo();
			decimal num = 0m;
			if (string.IsNullOrEmpty(context.Request["Products"]))
			{
				context.Response.Write("{\"PurchaseOrderAddResponse\":\"-1\"}");
				return;
			}
			int num2 = int.Parse(context.Request["distributorUserId"]);
			string[] array = context.Request["Products"].Split(new char[]
			{
				';'
			});
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i];
				string[] array3 = text.Split(new char[]
				{
					','
				});
				System.Data.DataTable skuContent = SubSiteProducthelper.GetSkuContent(long.Parse(array3[0]), array3[1], num2);
				if (skuContent != null && skuContent.Rows.Count > 0)
				{
					PurchaseOrderItemInfo purchaseOrderItemInfo = new PurchaseOrderItemInfo();
					foreach (System.Data.DataRow dataRow in skuContent.Rows)
					{
						if (!string.IsNullOrEmpty(dataRow["AttributeName"].ToString()) && !string.IsNullOrEmpty(dataRow["ValueStr"].ToString()))
						{
							PurchaseOrderItemInfo expr_124 = purchaseOrderItemInfo;
							object sKUContent = expr_124.SKUContent;
							expr_124.SKUContent = string.Concat(new object[]
							{
								sKUContent,
								dataRow["AttributeName"],
								":",
								dataRow["ValueStr"],
								"; "
							});
						}
					}
					purchaseOrderItemInfo.PurchaseOrderId = purchaseOrderInfo.PurchaseOrderId;
					purchaseOrderItemInfo.SkuId = (string)skuContent.Rows[0]["SkuId"];
					purchaseOrderItemInfo.ProductId = (int)skuContent.Rows[0]["ProductId"];
					if (skuContent.Rows[0]["SKU"] != System.DBNull.Value)
					{
						purchaseOrderItemInfo.SKU = (string)skuContent.Rows[0]["SKU"];
					}
					if (skuContent.Rows[0]["Weight"] != System.DBNull.Value)
					{
						purchaseOrderItemInfo.ItemWeight = (decimal)skuContent.Rows[0]["Weight"];
					}
					purchaseOrderItemInfo.ItemPurchasePrice = (decimal)skuContent.Rows[0]["PurchasePrice"];
					purchaseOrderItemInfo.Quantity = int.Parse(array3[2]);
					purchaseOrderItemInfo.ItemListPrice = (decimal)skuContent.Rows[0]["SalePrice"];
					if (skuContent.Rows[0]["CostPrice"] != System.DBNull.Value)
					{
						purchaseOrderItemInfo.ItemCostPrice = (decimal)skuContent.Rows[0]["CostPrice"];
					}
					purchaseOrderItemInfo.ItemDescription = (string)skuContent.Rows[0]["ProductName"];
					purchaseOrderItemInfo.ItemHomeSiteDescription = (string)skuContent.Rows[0]["ProductName"];
					if (skuContent.Rows[0]["ThumbnailUrl40"] != System.DBNull.Value)
					{
						purchaseOrderItemInfo.ThumbnailsUrl = (string)skuContent.Rows[0]["ThumbnailUrl40"];
					}
					num += purchaseOrderItemInfo.ItemWeight * purchaseOrderItemInfo.Quantity;
					purchaseOrderInfo.PurchaseOrderItems.Add(purchaseOrderItemInfo);
				}
			}
			if (purchaseOrderInfo.PurchaseOrderItems.Count <= 0)
			{
				context.Response.Write("{\"PurchaseOrderAddResponse\":\"-3\"}");
				return;
			}
			purchaseOrderInfo.Weight = num;
			purchaseOrderInfo.TaobaoOrderId = context.Request["TaobaoOrderId"];
			purchaseOrderInfo.PurchaseOrderId = "MPO" + purchaseOrderInfo.TaobaoOrderId;
			purchaseOrderInfo.ShipTo = context.Request["ShipTo"];
			string text2 = context.Request["ReceiverState"];
			string text3 = context.Request["ReceiverCity"];
			string text4 = context.Request["ReceiverDistrict"];
			purchaseOrderInfo.ShippingRegion = text2 + text3 + text4;
			purchaseOrderInfo.RegionId = RegionHelper.GetRegionId(text4, text3, text2);
			purchaseOrderInfo.Address = context.Request["Address"];
			purchaseOrderInfo.TelPhone = context.Request["TelPhone"];
			purchaseOrderInfo.CellPhone = context.Request["CellPhone"];
			purchaseOrderInfo.ZipCode = context.Request["ZipCode"];
			purchaseOrderInfo.OrderTotal = decimal.Parse(context.Request["OrderTotal"]);
			ShippingModeInfo shippingMode = ShoppingProcessor.GetShippingMode(Hidistro.Membership.Context.HiContext.Current.SiteSettings.TaobaoShippingType, true);
			if (shippingMode != null)
			{
				purchaseOrderInfo.ModeName = shippingMode.Name;
				purchaseOrderInfo.AdjustedFreight = (purchaseOrderInfo.Freight = ShoppingProcessor.CalcFreight(purchaseOrderInfo.RegionId, num, shippingMode));
			}
			purchaseOrderInfo.PurchaseStatus = OrderStatus.WaitBuyerPay;
			purchaseOrderInfo.RefundStatus = RefundStatus.None;
			Hidistro.Membership.Context.Distributor distributor = Hidistro.Membership.Context.Users.GetUser(num2) as Hidistro.Membership.Context.Distributor;
			if (distributor != null)
			{
				purchaseOrderInfo.DistributorId = distributor.UserId;
				purchaseOrderInfo.Distributorname = distributor.Username;
				purchaseOrderInfo.DistributorEmail = distributor.Email;
				purchaseOrderInfo.DistributorRealName = distributor.RealName;
				purchaseOrderInfo.DistributorQQ = distributor.QQ;
				purchaseOrderInfo.DistributorWangwang = distributor.Wangwang;
				purchaseOrderInfo.DistributorMSN = distributor.MSN;
			}
			if (!SubsiteSalesHelper.CreatePurchaseOrder(purchaseOrderInfo))
			{
				context.Response.Write("{\"PurchaseOrderAddResponse\":\"0\"}");
				return;
			}
			context.Response.Write("{\"PurchaseOrderAddResponse\":\"1\"}");
		}
	}
}
