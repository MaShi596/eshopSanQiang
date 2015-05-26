using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.SaleSystem.Catalog;
using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_SubjectBrand : WebControl
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
			XmlNode xmlNode = TagsHelper.FindCommentNode(this.CommentId, "brand");
			StringBuilder stringBuilder = new StringBuilder();
			if (xmlNode != null)
			{
				stringBuilder.AppendFormat("<div class=\"brand cssEdite\" type=\"brand\" id=\"comments_{0}\" >", this.CommentId).AppendLine();
				int categoryId = 0;
				int maxNum = 0;
				bool flag = true;
				bool flag2 = true;
				int.TryParse(xmlNode.Attributes["CategoryId"].Value, out categoryId);
				int.TryParse(xmlNode.Attributes["MaxNum"].Value, out maxNum);
				bool.TryParse(xmlNode.Attributes["IsShowLogo"].Value, out flag);
				bool.TryParse(xmlNode.Attributes["IsShowTitle"].Value, out flag2);
				string value = xmlNode.Attributes["ImageSize"].Value;
				DataTable brandCategories = CategoryBrowser.GetBrandCategories(categoryId, maxNum);
				if (brandCategories != null && brandCategories.Rows.Count > 0)
				{
					stringBuilder.AppendLine("<ul>");
					foreach (DataRow dataRow in brandCategories.Rows)
					{
						stringBuilder.AppendLine("<li>");
						if (flag)
						{
							stringBuilder.AppendFormat("<div class=\"pic\"><a target=\"_blank\" href=\"{0}\"><img src=\"{1}\" width=\"{2}\"></a></div>", Globals.GetSiteUrls().SubBrandDetails((int)dataRow["BrandId"], dataRow["RewriteName"]), dataRow["Logo"], value.Split(new char[]
							{
								'*'
							})[0]).AppendLine();
						}
						if (flag2)
						{
							stringBuilder.AppendFormat("<div class=\"name\"><a target=\"_blank\" href=\"{0}\">{1}</a></div>", Globals.GetSiteUrls().SubBrandDetails((int)dataRow["BrandId"], dataRow["RewriteName"]), dataRow["BrandName"]).AppendLine();
						}
						stringBuilder.AppendLine("</li>");
					}
					stringBuilder.AppendLine("</ul>");
				}
				stringBuilder.AppendLine("</div>");
			}
			return stringBuilder.ToString();
		}
	}
}
