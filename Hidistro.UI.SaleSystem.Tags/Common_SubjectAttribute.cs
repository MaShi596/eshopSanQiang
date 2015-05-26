using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Catalog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_SubjectAttribute : WebControl
	{
		public int CommentId
		{
			get;
			set;
		}
		protected override void Render(HtmlTextWriter writer)
		{
			writer.Write(this.RendHtml());
		}
		public string RendHtml()
		{
			StringBuilder stringBuilder = new StringBuilder();
			XmlNode xmlNode = TagsHelper.FindCommentNode(this.CommentId, "attribute");
			if (xmlNode != null)
			{
				stringBuilder.AppendFormat("<div class=\"attribute_bd cssEdite\" type=\"attribute\" id=\"comments_{0}\" >", this.CommentId).AppendLine();
				int categoryId = 0;
				int num = 0;
				int.TryParse(xmlNode.Attributes["CategoryId"].Value, out categoryId);
				int.TryParse(xmlNode.Attributes["MaxNum"].Value, out num);
				string rewriteName = null;
				CategoryInfo category = CategoryBrowser.GetCategory(categoryId);
				if (category != null)
				{
					rewriteName = category.RewriteName;
				}
				IList<AttributeInfo> attributeInfoByCategoryId = CategoryBrowser.GetAttributeInfoByCategoryId(categoryId, 1000);
				if (attributeInfoByCategoryId != null && attributeInfoByCategoryId.Count > 0)
				{
					foreach (AttributeInfo current in attributeInfoByCategoryId)
					{
						stringBuilder.AppendLine("<dl class=\"attribute_dl\">");
						stringBuilder.AppendFormat("<dt class=\"attribute_name\">{0}：</dt>", current.AttributeName).AppendLine();
						stringBuilder.AppendLine("<dd class=\"attribute_val\">");
						stringBuilder.AppendLine("<div class=\"h_chooselist\">");
						foreach (AttributeValueInfo current2 in current.AttributeValues)
						{
							stringBuilder.AppendFormat("<a href=\"{0}\" >{1}</a>", string.Concat(new object[]
							{
								Globals.GetSiteUrls().SubCategory(categoryId, rewriteName),
								"?valueStr=",
								current2.AttributeId,
								"_",
								current2.ValueId
							}), current2.ValueStr).AppendLine();
						}
						stringBuilder.AppendLine("</div>");
						stringBuilder.AppendLine("</dd>");
						stringBuilder.AppendLine("</dl>");
					}
				}
				stringBuilder.AppendLine("</div>");
			}
			return stringBuilder.ToString();
		}
	}
}
