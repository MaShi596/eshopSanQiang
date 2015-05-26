using Hidistro.Entities.Commodities;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Shopping;
using System;
using System.Data;
using System.Globalization;
using System.Text;
using System.Web;
namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class ShoppingHandler : System.Web.IHttpHandler
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
				string text = context.Request["action"];
				string text2 = text;
				if (text2 != null)
				{
					if (!(text2 == "AddToCartBySkus"))
					{
						if (!(text2 == "GetSkuByOptions"))
						{
							if (!(text2 == "UnUpsellingSku"))
							{
								if (text2 == "ClearBrowsed")
								{
									this.ClearBrowsedProduct(context);
								}
							}
							else
							{
								this.ProcessUnUpsellingSku(context);
							}
						}
						else
						{
							this.ProcessGetSkuByOptions(context);
						}
					}
					else
					{
						this.ProcessAddToCartBySkus(context);
					}
				}
			}
			catch (System.Exception ex)
			{
				context.Response.ContentType = "application/json";
				context.Response.Write("{\"Status\":\"" + ex.Message.Replace("\"", "'") + "\"}");
			}
		}
		private void ClearBrowsedProduct(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			BrowsedProductQueue.ClearQueue();
			context.Response.Write("{\"Status\":\"Succes\"}");
		}
		private void ProcessAddToCartBySkus(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int quantity = int.Parse(context.Request["quantity"], System.Globalization.NumberStyles.None);
			string skuId = context.Request["productSkuId"];
			ShoppingCartProcessor.AddLineItem(skuId, quantity);
			ShoppingCartInfo shoppingCart = ShoppingCartProcessor.GetShoppingCart();
			context.Response.Write(string.Concat(new string[]
			{
				"{\"Status\":\"OK\",\"TotalMoney\":\"",
				shoppingCart.GetTotal().ToString(".00"),
				"\",\"Quantity\":\"",
				shoppingCart.GetQuantity().ToString(),
				"\"}"
			}));
		}
		private void ProcessGetSkuByOptions(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int productId = int.Parse(context.Request["productId"], System.Globalization.NumberStyles.None);
			string text = context.Request["options"];
			if (string.IsNullOrEmpty(text))
			{
				context.Response.Write("{\"Status\":\"0\"}");
			}
			else
			{
				if (text.EndsWith(","))
				{
					text = text.Substring(0, text.Length - 1);
				}
				SKUItem productAndSku = ShoppingProcessor.GetProductAndSku(productId, text);
				if (productAndSku == null)
				{
					context.Response.Write("{\"Status\":\"1\"}");
				}
				else
				{
					System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
					stringBuilder.Append("{");
					stringBuilder.Append("\"Status\":\"OK\",");
					stringBuilder.AppendFormat("\"SkuId\":\"{0}\",", productAndSku.SkuId);
					stringBuilder.AppendFormat("\"SKU\":\"{0}\",", productAndSku.SKU);
					stringBuilder.AppendFormat("\"Weight\":\"{0}\",", productAndSku.Weight.ToString("F2"));
					stringBuilder.AppendFormat("\"Stock\":\"{0}\",", productAndSku.Stock);
					stringBuilder.AppendFormat("\"AlertStock\":\"{0}\",", productAndSku.AlertStock);
					stringBuilder.AppendFormat("\"SalePrice\":\"{0}\"", productAndSku.SalePrice.ToString("F2"));
					stringBuilder.Append("}");
					context.Response.ContentType = "application/json";
					context.Response.Write(stringBuilder.ToString());
				}
			}
		}
		private void ProcessUnUpsellingSku(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			int productId = int.Parse(context.Request["productId"], System.Globalization.NumberStyles.None);
			int attributeId = int.Parse(context.Request["AttributeId"], System.Globalization.NumberStyles.None);
			int valueId = int.Parse(context.Request["ValueId"], System.Globalization.NumberStyles.None);
			DataTable unUpUnUpsellingSkus = ShoppingProcessor.GetUnUpUnUpsellingSkus(productId, attributeId, valueId);
			if (unUpUnUpsellingSkus == null || unUpUnUpsellingSkus.Rows.Count == 0)
			{
				context.Response.Write("{\"Status\":\"1\"}");
			}
			else
			{
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
				stringBuilder.Append("{");
				stringBuilder.Append("\"Status\":\"OK\",");
				stringBuilder.Append("\"SkuItems\":[");
				foreach (DataRow dataRow in unUpUnUpsellingSkus.Rows)
				{
					stringBuilder.Append("{");
					stringBuilder.AppendFormat("\"AttributeId\":\"{0}\",", dataRow["AttributeId"].ToString());
					stringBuilder.AppendFormat("\"ValueId\":\"{0}\"", dataRow["ValueId"].ToString());
					stringBuilder.Append("},");
				}
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
				stringBuilder.Append("]");
				stringBuilder.Append("}");
				context.Response.Write(stringBuilder.ToString());
			}
		}
	}
}
