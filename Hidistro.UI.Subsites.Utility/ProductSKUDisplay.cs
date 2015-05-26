using Hidistro.Core;
using Hidistro.Subsites.Commodities;
using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Subsites.Utility
{
	public class ProductSKUDisplay : System.Web.UI.WebControls.WebControl
	{
		public new string CssClass
		{
			get;
			set;
		}
		public string HeadRowClass
		{
			get;
			set;
		}
		public string HeadColumnClass
		{
			get;
			set;
		}
		public string RowClass
		{
			get;
			set;
		}
		public string ColumnClass
		{
			get;
			set;
		}
		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			int productId = 0;
			if (int.TryParse(this.Page.Request.QueryString["ProductId"], out productId))
			{
				DataTable productSKU = SubSiteProducthelper.GetProductSKU(productId);
				if (productSKU != null && productSKU.Rows.Count > 0)
				{
					System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
					stringBuilder.Append("<input type=\"hidden\" id=\"skuContent\" value=\"1\" />");
					stringBuilder.AppendFormat("<table id=\"tbSkuContent\" width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"0\" class=\"{0}\" style=\"display:inline;\">", this.CssClass);
					stringBuilder.AppendFormat("<tr class=\"{0}\">", this.HeadRowClass);
					for (int i = productSKU.Columns.Count - 1; i > 0; i--)
					{
						stringBuilder.AppendFormat("<td class=\"{0}\">{1}</td>", this.HeadColumnClass, productSKU.Columns[i].ColumnName);
					}
					stringBuilder.Append("</tr>");
					foreach (DataRow dataRow in productSKU.Rows)
					{
						stringBuilder.AppendFormat("<tr class=\"{0}\">", this.RowClass);
						for (int j = productSKU.Columns.Count - 1; j > 0; j--)
						{
							string columnName = productSKU.Columns[j].ColumnName;
							string text = dataRow[columnName].ToString();
							if (columnName.Equals("一口价"))
							{
								stringBuilder.AppendFormat("<td class=\"{0}\"><input type=\"text\" style=\"width:80px\" class=\"skuPriceItem\" id=\"{1}\" value=\"{2}\" /></td>", this.RowClass, dataRow["SkuId"], decimal.Parse(text).ToString("F2"));
							}
							else
							{
								if (text.StartsWith("/Storage/master/sku/") && (text.ToLower().EndsWith(".jpg") || text.ToLower().EndsWith(".gif") || text.ToLower().EndsWith(".png") || text.ToLower().EndsWith(".ico") || text.ToLower().EndsWith(".bmp")))
								{
									stringBuilder.AppendFormat("<td class=\"{0}\"><img src=\"{1}\" /></td>", this.RowClass, Globals.ApplicationPath + text);
								}
								else
								{
									decimal num = 0m;
									int num2 = 0;
									if (decimal.TryParse(text, out num) && !int.TryParse(text, out num2))
									{
										text = num.ToString("F2");
									}
									stringBuilder.AppendFormat("<td class=\"{0}\">{1}</td>", this.RowClass, text);
								}
							}
						}
						stringBuilder.Append("</tr>");
					}
					stringBuilder.Append("</table>");
					writer.Write(stringBuilder.ToString());
					return;
				}
			}
			writer.Write("<input type=\"hidden\" id=\"skuContent\" value=\"0\" />");
		}
	}
}
