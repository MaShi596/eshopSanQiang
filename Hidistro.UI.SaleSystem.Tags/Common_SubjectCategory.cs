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
	public class Common_SubjectCategory : WebControl
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
			XmlNode xmlNode = TagsHelper.FindCommentNode(this.CommentId, "category");
			StringBuilder stringBuilder = new StringBuilder();
			if (xmlNode != null)
			{
				stringBuilder.AppendFormat("<div class=\"category cssEdite\" type=\"category\" id=\"comments_{0}\" >", this.CommentId).AppendLine();
				int parentCategoryId = 0;
				int maxNum = 0;
				int.TryParse(xmlNode.Attributes["CategoryId"].Value, out parentCategoryId);
				int.TryParse(xmlNode.Attributes["MaxNum"].Value, out maxNum);
				IList<CategoryInfo> maxSubCategories = CategoryBrowser.GetMaxSubCategories(parentCategoryId, maxNum);
				if (maxSubCategories != null && maxSubCategories.Count > 0)
				{
					stringBuilder.AppendLine("<ul>");
					foreach (CategoryInfo current in maxSubCategories)
					{
						stringBuilder.AppendFormat("<li><a target=\"_blank\" href=\"{0}\">{1}</a></li>", Globals.GetSiteUrls().SubCategory(current.CategoryId, current.RewriteName), current.Name).AppendLine();
					}
					stringBuilder.AppendLine("</ul>");
				}
				stringBuilder.AppendLine("</div>");
			}
			return stringBuilder.ToString();
		}
	}
}
