using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Comments;
using Hidistro.SaleSystem.Comments;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_SubjectArticle : WebControl
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
			XmlNode xmlNode = TagsHelper.FindCommentNode(this.CommentId, "article");
			StringBuilder stringBuilder = new StringBuilder();
			if (xmlNode != null)
			{
				stringBuilder.AppendFormat("<div class=\"article cssEdite\" type=\"article\" id=\"comments_{0}\" >", this.CommentId).AppendLine();
				this.RenderHeader(xmlNode, stringBuilder);
				stringBuilder.AppendLine("<div class=\"article_bd\">");
				if (!string.IsNullOrEmpty(xmlNode.Attributes["AdImage"].Value))
				{
					stringBuilder.AppendFormat("<div class=\"article_ad\"><img src=\"{0}\" /></div>", xmlNode.Attributes["AdImage"].Value).AppendLine();
				}
				int categoryId = 0;
				int maxNum = 0;
				int.TryParse(xmlNode.Attributes["CategoryId"].Value, out categoryId);
				int.TryParse(xmlNode.Attributes["MaxNum"].Value, out maxNum);
				IList<ArticleInfo> articleList = CommentBrowser.GetArticleList(categoryId, maxNum);
				if (articleList != null && articleList.Count > 0)
				{
					stringBuilder.AppendLine("<div class=\"article_list\">");
					stringBuilder.AppendLine("<ul>");
					foreach (ArticleInfo current in articleList)
					{
						stringBuilder.AppendFormat("<li><a target=\"_blank\" href=\"{0}\">{1}</a></li>", Globals.GetSiteUrls().UrlData.FormatUrl("ArticleDetails", new object[]
						{
							current.ArticleId
						}), current.Title).AppendLine();
					}
					stringBuilder.AppendLine("</ul>");
					stringBuilder.AppendLine("</div>");
				}
				stringBuilder.AppendLine("</div>");
				stringBuilder.AppendLine("</div>");
			}
			return stringBuilder.ToString();
		}
		private void RenderHeader(XmlNode node, StringBuilder stringBuilder_0)
		{
			stringBuilder_0.AppendLine("<div class=\"article_hd\">");
			stringBuilder_0.AppendLine("<h2>");
			if (!string.IsNullOrEmpty(node.Attributes["ImageTitle"].Value))
			{
				stringBuilder_0.AppendFormat("<img src=\"{0}\" />", Globals.ApplicationPath + node.Attributes["ImageTitle"].Value);
			}
			if (!string.IsNullOrEmpty(node.Attributes["Title"].Value))
			{
				stringBuilder_0.Append(node.Attributes["Title"].Value);
			}
			stringBuilder_0.AppendLine("</h2>");
			stringBuilder_0.AppendLine("</div>");
		}
	}
}
