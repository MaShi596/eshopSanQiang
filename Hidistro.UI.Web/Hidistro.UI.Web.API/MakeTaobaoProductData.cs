using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
using Hidistro.Membership.Context;
using Hidistro.UI.SaleSystem.CodeBehind;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
namespace Hidistro.UI.Web.API
{
	public class MakeTaobaoProductData : System.Web.UI.Page
	{
		protected System.Web.UI.HtmlControls.HtmlHead Head1;
		protected System.Web.UI.HtmlControls.HtmlForm form1;
		private int productId;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			int.TryParse(base.Request.QueryString["productId"], out this.productId);
			System.Data.DataSet taobaoProductDetails = ProductHelper.GetTaobaoProductDetails(this.productId);
			System.Data.DataTable dataTable = taobaoProductDetails.Tables[0];
			System.Collections.Generic.SortedDictionary<string, string> sortedDictionary = new System.Collections.Generic.SortedDictionary<string, string>();
			sortedDictionary.Add("SiteUrl", Hidistro.Membership.Context.HiContext.Current.SiteUrl);
			sortedDictionary.Add("_input_charset", "utf-8");
			sortedDictionary.Add("return_url", Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("MakeTaobaoProductData_url")));
			sortedDictionary.Add("ProductId", this.productId.ToString());
			sortedDictionary.Add("HasSKU", dataTable.Rows[0]["HasSKU"].ToString());
			sortedDictionary.Add("ProductName", (string)dataTable.Rows[0]["ProductName"]);
			sortedDictionary.Add("ProductCode", (string)dataTable.Rows[0]["ProductCode"]);
			sortedDictionary.Add("CategoryName", (dataTable.Rows[0]["CategoryName"] == System.DBNull.Value) ? "" : ((string)dataTable.Rows[0]["CategoryName"]));
			sortedDictionary.Add("ProductLineName", (dataTable.Rows[0]["ProductLineName"] == System.DBNull.Value) ? "" : ((string)dataTable.Rows[0]["ProductLineName"]));
			sortedDictionary.Add("BrandName", (dataTable.Rows[0]["BrandName"] == System.DBNull.Value) ? "" : ((string)dataTable.Rows[0]["BrandName"]));
			sortedDictionary.Add("SalePrice", System.Convert.ToDecimal(dataTable.Rows[0]["SalePrice"]).ToString("F2"));
			sortedDictionary.Add("MarketPrice", (dataTable.Rows[0]["MarketPrice"] == System.DBNull.Value) ? "0" : System.Convert.ToDecimal(dataTable.Rows[0]["MarketPrice"]).ToString("F2"));
			sortedDictionary.Add("CostPrice", System.Convert.ToDecimal(dataTable.Rows[0]["CostPrice"]).ToString("F2"));
			sortedDictionary.Add("PurchasePrice", System.Convert.ToDecimal(dataTable.Rows[0]["PurchasePrice"]).ToString("F2"));
			sortedDictionary.Add("Stock", dataTable.Rows[0]["Stock"].ToString());
			System.Data.DataTable dataTable2 = taobaoProductDetails.Tables[1];
			if (dataTable2.Rows.Count > 0)
			{
				string text = string.Empty;
				foreach (System.Data.DataRow dataRow in dataTable2.Rows)
				{
					object obj = text;
					text = string.Concat(new object[]
					{
						obj,
						dataRow["AttributeName"],
						":",
						dataRow["ValueStr"],
						";"
					});
				}
				text = text.Remove(text.Length - 1);
				sortedDictionary.Add("Attributes", text);
			}
			System.Data.DataTable dataTable3 = taobaoProductDetails.Tables[2];
			if (dataTable3.Rows.Count > 0)
			{
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
				System.Text.StringBuilder stringBuilder2 = new System.Text.StringBuilder();
				for (int i = dataTable3.Columns.Count - 1; i >= 0; i--)
				{
					stringBuilder2.Append(dataTable3.Columns[i].ColumnName).Append(";");
				}
				foreach (System.Data.DataRow dataRow2 in dataTable3.Rows)
				{
					for (int j = dataTable3.Columns.Count - 1; j >= 0; j--)
					{
						stringBuilder.Append(dataRow2[dataTable3.Columns[j].ColumnName]).Append(";");
					}
					stringBuilder.Remove(stringBuilder.Length - 1, 1).Append(",");
				}
				stringBuilder2.Remove(stringBuilder2.Length - 1, 1).Append(",").Append(stringBuilder.Remove(stringBuilder.Length - 1, 1));
				sortedDictionary.Add("skus", stringBuilder2.ToString());
			}
			System.Data.DataTable dataTable4 = taobaoProductDetails.Tables[3];
			if (dataTable4.Rows.Count > 0)
			{
				sortedDictionary.Add("Cid", dataTable4.Rows[0]["Cid"].ToString());
				if (dataTable4.Rows[0]["StuffStatus"] != System.DBNull.Value)
				{
					sortedDictionary.Add("StuffStatus", (string)dataTable4.Rows[0]["StuffStatus"]);
				}
				sortedDictionary.Add("ProTitle", (string)dataTable4.Rows[0]["ProTitle"]);
				sortedDictionary.Add("Num", dataTable4.Rows[0]["Num"].ToString());
				sortedDictionary.Add("LocationState", (string)dataTable4.Rows[0]["LocationState"]);
				sortedDictionary.Add("LocationCity", (string)dataTable4.Rows[0]["LocationCity"]);
				sortedDictionary.Add("FreightPayer", (string)dataTable4.Rows[0]["FreightPayer"]);
				if (dataTable4.Rows[0]["PostFee"] != System.DBNull.Value)
				{
					sortedDictionary.Add("PostFee", dataTable4.Rows[0]["PostFee"].ToString());
				}
				if (dataTable4.Rows[0]["ExpressFee"] != System.DBNull.Value)
				{
					sortedDictionary.Add("ExpressFee", dataTable4.Rows[0]["ExpressFee"].ToString());
				}
				if (dataTable4.Rows[0]["EMSFee"] != System.DBNull.Value)
				{
					sortedDictionary.Add("EMSFee", dataTable4.Rows[0]["EMSFee"].ToString());
				}
				sortedDictionary.Add("HasInvoice", dataTable4.Rows[0]["HasInvoice"].ToString());
				sortedDictionary.Add("HasWarranty", dataTable4.Rows[0]["HasWarranty"].ToString());
				sortedDictionary.Add("HasDiscount", dataTable4.Rows[0]["HasDiscount"].ToString());
				if (dataTable4.Rows[0]["PropertyAlias"] != System.DBNull.Value)
				{
					sortedDictionary.Add("PropertyAlias", (string)dataTable4.Rows[0]["PropertyAlias"]);
				}
				if (dataTable4.Rows[0]["SkuProperties"] != System.DBNull.Value)
				{
					sortedDictionary.Add("SkuProperties", (string)dataTable4.Rows[0]["SkuProperties"]);
				}
				if (dataTable4.Rows[0]["SkuQuantities"] != System.DBNull.Value)
				{
					sortedDictionary.Add("SkuQuantities", (string)dataTable4.Rows[0]["SkuQuantities"]);
				}
				if (dataTable4.Rows[0]["SkuPrices"] != System.DBNull.Value)
				{
					sortedDictionary.Add("SkuPrices", (string)dataTable4.Rows[0]["SkuPrices"]);
				}
				if (dataTable4.Rows[0]["SkuOuterIds"] != System.DBNull.Value)
				{
					sortedDictionary.Add("SkuOuterIds", (string)dataTable4.Rows[0]["SkuOuterIds"]);
				}
				if (dataTable4.Rows[0]["inputpids"] != System.DBNull.Value)
				{
					sortedDictionary.Add("inputpids", (string)dataTable4.Rows[0]["inputpids"]);
				}
				if (dataTable4.Rows[0]["inputstr"] != System.DBNull.Value)
				{
					sortedDictionary.Add("inputstr", (string)dataTable4.Rows[0]["inputstr"]);
				}
				if (dataTable4.Rows[0]["FoodAttributes"] != System.DBNull.Value)
				{
					sortedDictionary.Add("FoodAttributes", System.Web.HttpUtility.UrlEncode((string)dataTable4.Rows[0]["FoodAttributes"]));
				}
			}
			System.Collections.Generic.Dictionary<string, string> dictionary = OpenIdFunction.FilterPara(sortedDictionary);
			System.Text.StringBuilder stringBuilder3 = new System.Text.StringBuilder();
			foreach (System.Collections.Generic.KeyValuePair<string, string> current in dictionary)
			{
				stringBuilder3.Append(OpenIdFunction.CreateField(current.Key, current.Value));
			}
			sortedDictionary.Clear();
			dictionary.Clear();
			string action = "http://order1.kuaidiangtong.com/MakeTaoBaoData.aspx";
			if (dataTable4.Rows.Count > 0)
			{
				action = "http://order1.kuaidiangtong.com/UpdateTaoBaoData.aspx";
			}
			OpenIdFunction.Submit(OpenIdFunction.CreateForm(stringBuilder3.ToString(), action));
		}
	}
}
