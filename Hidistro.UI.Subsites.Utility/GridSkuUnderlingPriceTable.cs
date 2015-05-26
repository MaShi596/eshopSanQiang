using Hidistro.Subsites.Commodities;
using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Subsites.Utility
{
	public class GridSkuUnderlingPriceTable : System.Web.UI.WebControls.WebControl
	{
		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			string text = this.Page.Request.QueryString["productIds"];
			if (!string.IsNullOrEmpty(text))
			{
				DataTable skuUnderlingPrices = SubSiteProducthelper.GetSkuUnderlingPrices(text);
				if (skuUnderlingPrices != null && skuUnderlingPrices.Rows.Count > 0)
				{
					System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
					stringBuilder.AppendLine("<table cellspacing=\"0\" border=\"0\" style=\"width:100%;border-collapse:collapse;\">");
					stringBuilder.AppendLine("<tr class=\"table_title\">");
					stringBuilder.AppendLine("<th class=\"td_right td_left\" scope=\"col\">货号</th>");
					stringBuilder.AppendLine("<th class=\"td_right td_left\" scope=\"col\" style=\"width:300px;\">商品</th>");
					stringBuilder.AppendLine("<th class=\"td_right td_left\" scope=\"col\">市场价</th>");
					stringBuilder.AppendLine("<th class=\"td_right td_left\" scope=\"col\">我的采购价</th>");
					stringBuilder.AppendLine("<th class=\"td_right td_left\" scope=\"col\">一口价</th>");
					for (int i = 7; i < skuUnderlingPrices.Columns.Count; i++)
					{
						string text2 = skuUnderlingPrices.Columns[i].ColumnName;
						text2 = text2.Substring(text2.IndexOf("_") + 1) + "价";
						stringBuilder.AppendFormat("<th class=\"td_right td_left\" scope=\"col\">{0}</th>", text2).AppendLine();
					}
					stringBuilder.AppendLine("</tr>");
					foreach (DataRow dataRow_ in skuUnderlingPrices.Rows)
					{
						this.CreateRow(dataRow_, skuUnderlingPrices, stringBuilder);
					}
					stringBuilder.AppendLine("</table>");
					writer.Write(stringBuilder.ToString());
				}
			}
		}
		private void CreateRow(DataRow dataRow_0, DataTable dtSkus, System.Text.StringBuilder stringBuilder_0)
		{
			string text = dataRow_0["SkuId"].ToString();
			stringBuilder_0.AppendFormat("<tr class=\"SkuPriceRow\" skuId=\"{0}\" >", text).AppendLine();
			stringBuilder_0.AppendFormat("<td>&nbsp;{0}</td>", dataRow_0["SKU"] + "&nbsp;").AppendLine();
			stringBuilder_0.AppendFormat("<td style=\"width:300px;\">{0} {1}</td>", dataRow_0["ProductName"], dataRow_0["SKUContent"]).AppendLine();
			stringBuilder_0.AppendFormat("<td>&nbsp;{0}</td>", (dataRow_0["MarketPrice"] != System.DBNull.Value) ? decimal.Parse(dataRow_0["MarketPrice"].ToString()).ToString("F2") : "").AppendLine();
			stringBuilder_0.AppendFormat("<td>&nbsp;{0}</td>", (dataRow_0["PurchasePrice"] != System.DBNull.Value) ? decimal.Parse(dataRow_0["PurchasePrice"].ToString()).ToString("F2") : "").AppendLine();
			stringBuilder_0.AppendFormat("<td><input type=\"text\" id=\"tdSalePrice_{0}\" style=\"width:60px\" class=\"forminput\" value=\"{1}\" />", text, decimal.Parse(dataRow_0["SalePrice"].ToString()).ToString("F2")).AppendLine();
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
				stringBuilder_0.AppendFormat("<td><input type=\"text\" id=\"{0}_tdMemberPrice_{1}\" name=\"tdMemberPrice_{1}\" style=\"width:50px\" class=\"forminput\" value=\"{2}\" />{3}", new object[]
				{
					columnName.Substring(0, columnName.IndexOf("_")),
					text,
					text2,
					text3
				}).AppendLine();
			}
			stringBuilder_0.AppendLine("</tr>");
		}
	}
}
