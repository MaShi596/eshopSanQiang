using Hidistro.Entities.Commodities;
using Hidistro.Subsites.Commodities;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Data;
using System.Text;
namespace Hidistro.UI.Web.Shopadmin.purchaseOrder
{
	public class ReturnSearchPurchaseProduct : DistributorPage
	{
		protected void Page_Load(object sender, System.EventArgs e)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			string keywords = "";
			string productCode = "";
			string value = "1";
			if (base.Request.Params["serachName"] != null)
			{
				keywords = base.Request.Params["serachName"].Trim();
			}
			if (base.Request.Params["serachSKU"] != null)
			{
				productCode = base.Request.Params["serachSKU"].Trim();
			}
			if (base.Request.Params["page"] != null)
			{
				value = base.Request.Params["page"].Trim();
			}
			ProductQuery productQuery = new ProductQuery();
			productQuery.PageSize = 8;
			productQuery.PageIndex = System.Convert.ToInt32(value);
			productQuery.Keywords = keywords;
			productQuery.ProductCode = productCode;
			int num = 0;
			stringBuilder.Append("{");
			stringBuilder.Append("\"data\":[");
			System.Data.DataTable puchaseProducts = SubSiteProducthelper.GetPuchaseProducts(productQuery, out num);
			int count = puchaseProducts.Rows.Count;
			for (int i = 0; i < count; i++)
			{
				System.Data.DataRow dataRow = puchaseProducts.Rows[i];
				stringBuilder.Append("{");
				stringBuilder.AppendFormat("\"skuId\":\"{0}\"", dataRow["SkuId"]);
				stringBuilder.AppendFormat(",\"sku\":\"{0}\"", dataRow["SKU"]);
				stringBuilder.AppendFormat(",\"productId\":\"{0}\"", dataRow["ProductId"].ToString().Trim());
				string arg = dataRow["ProductName"].ToString().Trim();
				stringBuilder.AppendFormat(",\"Name\":\"{0}\"", arg);
				stringBuilder.AppendFormat(",\"Price\":\"{0}\"", dataRow["PurchasePrice"]);
				stringBuilder.AppendFormat(",\"Stock\":\"{0}\"", dataRow["Stock"]);
				if (i == count - 1)
				{
					stringBuilder.Append("}");
				}
				else
				{
					stringBuilder.Append("},");
				}
			}
			stringBuilder.AppendFormat("],\"recCount\":\"{0}\"", num);
			stringBuilder.Append("}");
			base.Response.ContentType = "application/json";
			string s = stringBuilder.ToString();
			base.Response.Write(s);
			base.Response.End();
		}
	}
}
