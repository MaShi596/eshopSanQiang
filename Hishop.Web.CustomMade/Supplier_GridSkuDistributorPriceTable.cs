using Hidistro.ControlPanel.Commodities;
using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hishop.Web.CustomMade
{
	public class Supplier_GridSkuDistributorPriceTable : WebControl
	{
		protected override void Render(HtmlTextWriter writer)
		{
			string text = this.Page.Request.QueryString["productIds"];
			if (!string.IsNullOrEmpty(text))
			{
				System.Data.DataTable skuDistributorPrices = ProductHelper.GetSkuDistributorPrices(text);
				if (skuDistributorPrices != null && skuDistributorPrices.Rows.Count > 0)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine("<table cellspacing=\"0\" border=\"0\" style=\"width:100%;border-collapse:collapse;\">");
					stringBuilder.AppendLine("<tr class=\"table_title\">");
					stringBuilder.AppendLine("<th class=\"td_right td_left\" scope=\"col\">货号</th>");
					stringBuilder.AppendLine("<th class=\"td_right td_left\" scope=\"col\">商品</th>");
					stringBuilder.AppendLine("<th class=\"td_right td_left\" scope=\"col\">市场价</th>");
					stringBuilder.AppendLine("<th class=\"td_right td_left\" scope=\"col\" style=\"display:none;\">成本价</th>");
					stringBuilder.AppendLine("<th class=\"td_right td_left\" scope=\"col\">采购价</th>");
					for (int i = 7; i < skuDistributorPrices.Columns.Count; i++)
					{
						string text2 = skuDistributorPrices.Columns[i].ColumnName;
						text2 = text2.Substring(text2.IndexOf("_") + 1) + "采购价";
						stringBuilder.AppendFormat("<th class=\"td_right td_left\" scope=\"col\" style=\"width:100px;display:none;\">{0}</th>", text2);
					}
					stringBuilder.AppendLine("</tr>");
					foreach (System.Data.DataRow dataRow_ in skuDistributorPrices.Rows)
					{
						this.CreateRow(dataRow_, skuDistributorPrices, stringBuilder);
					}
					stringBuilder.Append("</table>");
					writer.Write(stringBuilder.ToString());
				}
			}
		}
		private void CreateRow(System.Data.DataRow dataRow_0, System.Data.DataTable dtSkus, StringBuilder stringBuilder_0)
		{
			string text = dataRow_0["SkuId"].ToString();
			stringBuilder_0.AppendFormat("<tr class=\"SkuPriceRow\" skuId=\"{0}\" >", text).AppendLine();
			stringBuilder_0.AppendFormat("<td>&nbsp;{0}</td>", dataRow_0["SKU"]);
			stringBuilder_0.AppendFormat("<td>{0} {1}</td>", dataRow_0["ProductName"], dataRow_0["SKUContent"]);
			stringBuilder_0.AppendFormat("<td>&nbsp;{0}</td>", (dataRow_0["MarketPrice"] != DBNull.Value) ? decimal.Parse(dataRow_0["MarketPrice"].ToString()).ToString("F2") : "").AppendLine();
			stringBuilder_0.AppendFormat("<td style=\"display:none;\"><input type=\"text\" id=\"tdCostPrice_{0}\" style=\"width:50px\" value=\"{1}\" />", text, (dataRow_0["CostPrice"] != DBNull.Value) ? decimal.Parse(dataRow_0["CostPrice"].ToString()).ToString("F2") : "").AppendLine();
			stringBuilder_0.AppendFormat("<td><input type=\"text\" id=\"tdPurchasePrice_{0}\" style=\"width:50px\" value=\"{1}\" />", text, decimal.Parse(dataRow_0["PurchasePrice"].ToString()).ToString("F2")).AppendLine();
			for (int i = 7; i < dtSkus.Columns.Count; i++)
			{
				string columnName = dtSkus.Columns[i].ColumnName;
				string[] array = dataRow_0[columnName].ToString().Split(new char[]
				{
					'|'
				});
				string text2 = "";
				if (array[0].ToString() != "")
				{
					text2 = decimal.Parse(array[0].ToString()).ToString("F2");
				}
				string text3 = array[1].ToString();
				stringBuilder_0.AppendFormat("<td style=\"display:none;\"><input type=\"text\" id=\"{0}_tdDistributorPrice_{1}\" name=\"tdDistributorPrice_{1}\" style=\"width:50px\" value=\"{2}\" />{3}</td>", new object[]
				{
					columnName.Substring(0, columnName.IndexOf("_")),
					text,
					text2,
					text3
				}).AppendLine();
			}
			stringBuilder_0.Append("</tr>");
		}
	}
}
