using Hidistro.Subsites.Commodities;
using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Subsites.Utility
{
	public class ProductAttributeDisplay : System.Web.UI.WebControls.WebControl
	{
		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			int productId = 0;
			if (int.TryParse(this.Page.Request.QueryString["ProductId"], out productId))
			{
				DataTable productAttribute = SubSiteProducthelper.GetProductAttribute(productId);
				if (productAttribute != null && productAttribute.Rows.Count > 0)
				{
					System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
					stringBuilder.Append("<input type=\"hidden\" id=\"attributeContent\" value=\"1\" />");
					stringBuilder.Append("<span  class=\"Property\">");
					stringBuilder.Append("<table width=\"0\" border=\"0\" cellspacing=\"0\">");
					foreach (DataRow dataRow in productAttribute.Rows)
					{
						stringBuilder.AppendFormat("<tr><td width=\"20%\" align=\"right\">{0}：</td><td>{1}</td>", dataRow["AttributeName"], dataRow["ValueStr"]);
					}
					stringBuilder.Append("</table>");
					stringBuilder.Append("</span>");
					writer.Write(stringBuilder.ToString());
					return;
				}
			}
			writer.Write("<input type=\"hidden\" id=\"attributeContent\" value=\"0\" />");
		}
	}
}
