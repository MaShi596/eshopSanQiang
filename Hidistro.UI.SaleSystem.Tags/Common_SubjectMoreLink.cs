using Hidistro.Core;
using Hidistro.Entities;
using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_SubjectMoreLink : WebControl
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
			XmlNode xmlNode = TagsHelper.FindCommentNode(this.CommentId, "morelink");
			StringBuilder stringBuilder = new StringBuilder();
			if (xmlNode != null)
			{
				stringBuilder.AppendFormat("<div class=\"morelink cssEdite\" type=\"morelink\" id=\"comments_{0}\" >", this.CommentId).AppendLine();
				int categoryId = 0;
				if (int.TryParse(xmlNode.Attributes["CategoryId"].Value, out categoryId))
				{
					stringBuilder.AppendFormat("<em><a href=\"{0}\">{1}</a></em>", Globals.GetSiteUrls().SubCategory(categoryId, null), xmlNode.Attributes["Title"].Value).AppendLine();
				}
				else
				{
					stringBuilder.AppendFormat("<em><a href=\"{0}/SubCategory.aspx\">{1}</a></em>", Globals.ApplicationPath, xmlNode.Attributes["Title"].Value).AppendLine();
				}
				stringBuilder.AppendLine("</div>");
			}
			return stringBuilder.ToString();
		}
	}
}
