using Hidistro.Entities;
using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_SlideAd : WebControl
	{
		public int AdId
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
			XmlNode xmlNode = TagsHelper.FindAdNode(this.AdId, "slide");
			StringBuilder stringBuilder = new StringBuilder();
			if (xmlNode != null)
			{
				string text = "600";
				string text2 = "300";
				if (!string.IsNullOrEmpty(xmlNode.Attributes["AdImageSize"].Value) && xmlNode.Attributes["AdImageSize"].Value.Contains("*"))
				{
					text = xmlNode.Attributes["AdImageSize"].Value.Split(new char[]
					{
						'*'
					})[0];
					text2 = xmlNode.Attributes["AdImageSize"].Value.Split(new char[]
					{
						'*'
					})[1];
				}
				stringBuilder.AppendFormat("<div class=\"ad_slide cssEdite\" type=\"slide\" id=\"ads_{0}\" >", this.AdId).AppendLine();
				stringBuilder.AppendLine("<div class=\"focusWarp\">");
				stringBuilder.AppendLine("<ul class=\"imgList\">");
				if (!string.IsNullOrEmpty(xmlNode.Attributes["Image1"].Value))
				{
					stringBuilder.AppendFormat("<li><a {0} target=\"_blank\"><img src=\"{1}\" width=\"{2}\" height=\"{3}\" /></a></li>", new object[]
					{
						(xmlNode.Attributes["Url1"].Value.Length == 0) ? "" : ("href=\"" + xmlNode.Attributes["Url1"].Value + "\""),
						xmlNode.Attributes["Image1"].Value,
						text,
						text2
					}).AppendLine();
				}
				if (!string.IsNullOrEmpty(xmlNode.Attributes["Image2"].Value))
				{
					stringBuilder.AppendFormat("<li><a {0} target=\"_blank\"><img src=\"{1}\" width=\"{2}\" height=\"{3}\" /></a></li>", new object[]
					{
						(xmlNode.Attributes["Url2"].Value.Length == 0) ? "" : ("href=\"" + xmlNode.Attributes["Url2"].Value + "\""),
						xmlNode.Attributes["Image2"].Value,
						text,
						text2
					}).AppendLine();
				}
				if (!string.IsNullOrEmpty(xmlNode.Attributes["Image3"].Value))
				{
					stringBuilder.AppendFormat("<li><a {0} target=\"_blank\"><img src=\"{1}\" width=\"{2}\" height=\"{3}\" /></a></li>", new object[]
					{
						(xmlNode.Attributes["Url3"].Value.Length == 0) ? "" : ("href=\"" + xmlNode.Attributes["Url3"].Value + "\""),
						xmlNode.Attributes["Image3"].Value,
						text,
						text2
					}).AppendLine();
				}
				if (!string.IsNullOrEmpty(xmlNode.Attributes["Image4"].Value))
				{
					stringBuilder.AppendFormat("<li><a {0} target=\"_blank\"><img src=\"{1}\" width=\"{2}\" height=\"{3}\" /></a></li>", new object[]
					{
						(xmlNode.Attributes["Url4"].Value.Length == 0) ? "" : ("href=\"" + xmlNode.Attributes["Url4"].Value + "\""),
						xmlNode.Attributes["Image4"].Value,
						text,
						text2
					}).AppendLine();
				}
				if (!string.IsNullOrEmpty(xmlNode.Attributes["Image5"].Value))
				{
					stringBuilder.AppendFormat("<li><a {0} target=\"_blank\"><img src=\"{1}\" width=\"{2}\" height=\"{3}\" /></a></li>", new object[]
					{
						(xmlNode.Attributes["Url5"].Value.Length == 0) ? "" : ("href=\"" + xmlNode.Attributes["Url5"].Value + "\""),
						xmlNode.Attributes["Image5"].Value,
						text,
						text2
					}).AppendLine();
				}
				if (!string.IsNullOrEmpty(xmlNode.Attributes["Image6"].Value))
				{
					stringBuilder.AppendFormat("<li><a {0} target=\"_blank\"><img src=\"{1}\" width=\"{2}\" height=\"{3}\" /></a></li>", new object[]
					{
						(xmlNode.Attributes["Url6"].Value.Length == 0) ? "" : ("href=\"" + xmlNode.Attributes["Url6"].Value + "\""),
						xmlNode.Attributes["Image6"].Value,
						text,
						text2
					}).AppendLine();
				}
				if (!string.IsNullOrEmpty(xmlNode.Attributes["Image7"].Value))
				{
					stringBuilder.AppendFormat("<li><a {0} target=\"_blank\"><img src=\"{1}\" width=\"{2}\" height=\"{3}\" /></a></li>", new object[]
					{
						(xmlNode.Attributes["Url7"].Value.Length == 0) ? "" : ("href=\"" + xmlNode.Attributes["Url7"].Value + "\""),
						xmlNode.Attributes["Image7"].Value,
						text,
						text2
					}).AppendLine();
				}
				if (!string.IsNullOrEmpty(xmlNode.Attributes["Image8"].Value))
				{
					stringBuilder.AppendFormat("<li><a {0} target=\"_blank\"><img src=\"{1}\" width=\"{2}\" height=\"{3}\" /></a></li>", new object[]
					{
						(xmlNode.Attributes["Url8"].Value.Length == 0) ? "" : ("href=\"" + xmlNode.Attributes["Url8"].Value + "\""),
						xmlNode.Attributes["Image8"].Value,
						text,
						text2
					}).AppendLine();
				}
				if (!string.IsNullOrEmpty(xmlNode.Attributes["Image9"].Value))
				{
					stringBuilder.AppendFormat("<li><a {0} target=\"_blank\"><img src=\"{1}\" width=\"{2}\" height=\"{3}\" /></a></li>", new object[]
					{
						(xmlNode.Attributes["Url9"].Value.Length == 0) ? "" : ("href=\"" + xmlNode.Attributes["Url9"].Value + "\""),
						xmlNode.Attributes["Image9"].Value,
						text,
						text2
					}).AppendLine();
				}
				if (!string.IsNullOrEmpty(xmlNode.Attributes["Image10"].Value))
				{
					stringBuilder.AppendFormat("<li><a {0} target=\"_blank\"><img src=\"{1}\" width=\"{2}\" height=\"{3}\" /></a></li>", new object[]
					{
						(xmlNode.Attributes["Url10"].Value.Length == 0) ? "" : ("href=\"" + xmlNode.Attributes["Url10"].Value + "\""),
						xmlNode.Attributes["Image10"].Value,
						text,
						text2
					}).AppendLine();
				}
				stringBuilder.AppendLine("</ul>");
				stringBuilder.AppendLine("</div>");
				stringBuilder.AppendLine("</div>");
				stringBuilder.AppendLine("<script type=\"text/javascript\">");
				stringBuilder.Append("$(function(){");
				stringBuilder.AppendFormat("$(\"#ads_{0}\").mogFocus({{scrollWidth:" + text + "}}); ", this.AdId).AppendLine();
				stringBuilder.Append("});");
				stringBuilder.AppendLine("</script>");
			}
			return stringBuilder.ToString();
		}
	}
}
