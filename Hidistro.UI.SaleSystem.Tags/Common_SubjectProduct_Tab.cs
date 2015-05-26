using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Membership.Context;
using Hidistro.SaleSystem.Catalog;
using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_SubjectProduct_Tab : WebControl
	{
		public int SubjectId
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
			XmlNode xmlNode = TagsHelper.FindProductNode(this.SubjectId, "tab");
			StringBuilder stringBuilder = new StringBuilder();
			if (xmlNode != null)
			{
				stringBuilder.AppendFormat("<div class=\"pro_tab{0} cssEdite\" type=\"tab\" id=\"products_{1}\" >", xmlNode.Attributes["ImageSize"].Value, this.SubjectId).AppendLine();
				this.RenderHeader(xmlNode, stringBuilder);
				stringBuilder.AppendFormat("<div class=\"tab_item\" id=\"products_{0}_item_1\">", this.SubjectId);
				this.RenderProdcutItem(xmlNode, stringBuilder, "Where1");
				stringBuilder.AppendLine("</div>");
				if (!string.IsNullOrEmpty(xmlNode.Attributes["TabTitle2"].Value))
				{
					stringBuilder.AppendFormat("<div style=\"display:none;\" class=\"tab_item\" id=\"products_{0}_item_2\">", this.SubjectId);
					this.RenderProdcutItem(xmlNode, stringBuilder, "Where2");
					stringBuilder.AppendLine("</div>");
				}
				if (!string.IsNullOrEmpty(xmlNode.Attributes["TabTitle3"].Value))
				{
					stringBuilder.AppendFormat("<div style=\"display:none;\" class=\"tab_item\" id=\"products_{0}_item_3\">", this.SubjectId);
					this.RenderProdcutItem(xmlNode, stringBuilder, "Where3");
					stringBuilder.AppendLine("</div>");
				}
				if (!string.IsNullOrEmpty(xmlNode.Attributes["TabTitle4"].Value))
				{
					stringBuilder.AppendFormat("<div style=\"display:none;\" class=\"tab_item\" id=\"products_{0}_item_4\">", this.SubjectId);
					this.RenderProdcutItem(xmlNode, stringBuilder, "Where4");
					stringBuilder.AppendLine("</div>");
				}
				if (!string.IsNullOrEmpty(xmlNode.Attributes["TabTitle5"].Value))
				{
					stringBuilder.AppendFormat("<div style=\"display:none;\" class=\"tab_item\" id=\"products_{0}_item_5\">", this.SubjectId);
					this.RenderProdcutItem(xmlNode, stringBuilder, "Where5");
					stringBuilder.AppendLine("</div>");
				}
				if (!string.IsNullOrEmpty(xmlNode.Attributes["TabTitle6"].Value))
				{
					stringBuilder.AppendFormat("<div style=\"display:none;\" class=\"tab_item\" id=\"products_{0}_item_6\">", this.SubjectId);
					this.RenderProdcutItem(xmlNode, stringBuilder, "Where6");
					stringBuilder.AppendLine("</div>");
				}
				if (!string.IsNullOrEmpty(xmlNode.Attributes["TabTitle7"].Value))
				{
					stringBuilder.AppendFormat("<div style=\"display:none;\" class=\"tab_item\" id=\"products_{0}_item_7\">", this.SubjectId);
					this.RenderProdcutItem(xmlNode, stringBuilder, "Where7");
					stringBuilder.AppendLine("</div>");
				}
				if (!string.IsNullOrEmpty(xmlNode.Attributes["TabTitle8"].Value))
				{
					stringBuilder.AppendFormat("<div style=\"display:none;\" class=\"tab_item\" id=\"products_{0}_item_8\">", this.SubjectId);
					this.RenderProdcutItem(xmlNode, stringBuilder, "Where8");
					stringBuilder.AppendLine("</div>");
				}
				stringBuilder.AppendLine("</div>");
			}
			return stringBuilder.ToString();
		}
		private void RenderHeader(XmlNode node, StringBuilder stringBuilder_0)
		{
			stringBuilder_0.AppendLine("<div class=\"tab_hd\">");
			stringBuilder_0.AppendLine("<ul>");
			stringBuilder_0.AppendFormat("<li class=\"select\" onmouseover=\"changeTab(this, 'products_{0}', '_item_1')\">{1}</li>", this.SubjectId, node.Attributes["TabTitle1"].Value).AppendLine();
			if (!string.IsNullOrEmpty(node.Attributes["TabTitle2"].Value))
			{
				stringBuilder_0.AppendFormat("<li onmouseover=\"changeTab(this, 'products_{0}', '_item_2')\">{1}</li>", this.SubjectId, node.Attributes["TabTitle2"].Value).AppendLine();
			}
			if (!string.IsNullOrEmpty(node.Attributes["TabTitle3"].Value))
			{
				stringBuilder_0.AppendFormat("<li onmouseover=\"changeTab(this, 'products_{0}', '_item_3')\">{1}</li>", this.SubjectId, node.Attributes["TabTitle3"].Value).AppendLine();
			}
			if (!string.IsNullOrEmpty(node.Attributes["TabTitle4"].Value))
			{
				stringBuilder_0.AppendFormat("<li onmouseover=\"changeTab(this, 'products_{0}', '_item_4')\">{1}</li>", this.SubjectId, node.Attributes["TabTitle4"].Value).AppendLine();
			}
			if (!string.IsNullOrEmpty(node.Attributes["TabTitle5"].Value))
			{
				stringBuilder_0.AppendFormat("<li onmouseover=\"changeTab(this, 'products_{0}', '_item_5')\">{1}</li>", this.SubjectId, node.Attributes["TabTitle5"].Value).AppendLine();
			}
			if (!string.IsNullOrEmpty(node.Attributes["TabTitle6"].Value))
			{
				stringBuilder_0.AppendFormat("<li onmouseover=\"changeTab(this, 'products_{0}', '_item_6')\">{1}</li>", this.SubjectId, node.Attributes["TabTitle6"].Value).AppendLine();
			}
			if (!string.IsNullOrEmpty(node.Attributes["TabTitle7"].Value))
			{
				stringBuilder_0.AppendFormat("<li onmouseover=\"changeTab(this, 'products_{0}', '_item_7')\">{1}</li>", this.SubjectId, node.Attributes["TabTitle7"].Value).AppendLine();
			}
			if (!string.IsNullOrEmpty(node.Attributes["TabTitle8"].Value))
			{
				stringBuilder_0.AppendFormat("<li onmouseover=\"changeTab(this, 'products_{0}', '_item_8')\">{1}</li>", this.SubjectId, node.Attributes["TabTitle8"].Value).AppendLine();
			}
			stringBuilder_0.AppendLine("</ul>");
			stringBuilder_0.AppendLine("</div>");
		}
		private void RenderProdcutItem(XmlNode node, StringBuilder stringBuilder_0, string whereName)
		{
			DataTable tabProductList = this.GetTabProductList(node, whereName);
			if (tabProductList != null && tabProductList.Rows.Count > 0)
			{
				stringBuilder_0.AppendLine("<ul>");
				foreach (DataRow dataRow in tabProductList.Rows)
				{
					SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
					string str = masterSettings.DefaultProductImage;
					if (dataRow["ThumbnailUrl" + node.Attributes["ImageSize"].Value] != DBNull.Value)
					{
						str = dataRow["ThumbnailUrl" + node.Attributes["ImageSize"].Value].ToString();
					}
					stringBuilder_0.AppendLine("<li>");
					stringBuilder_0.AppendFormat("<div class=\"pic\"><a target=\"_blank\" href=\"{0}\"><img src=\"{1}\" alt=\"{2}\" /></a></div>", Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[]
					{
						dataRow["ProductId"]
					}), Globals.ApplicationPath + str, dataRow["ProductName"]).AppendLine();
					stringBuilder_0.AppendFormat("<div class=\"name\"><a target=\"_blank\" href=\"{0}\">{1}</a></div>", Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[]
					{
						dataRow["ProductId"]
					}), dataRow["ProductName"]).AppendLine();
					string arg = string.Empty;
					if (dataRow["MarketPrice"] != DBNull.Value)
					{
						arg = Globals.FormatMoney((decimal)dataRow["MarketPrice"]);
					}
					stringBuilder_0.AppendFormat("<div class=\"price\"><b><em>￥</em>{0}</b><span><em>￥</em>{1}</span></div>", Globals.FormatMoney((decimal)dataRow["RankPrice"]), arg).AppendLine();
					stringBuilder_0.AppendFormat("<a class=\"productview\" target=\"_blank\" href=\"{0}\">查看详细</a>", Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[]
					{
						dataRow["ProductId"]
					})).AppendLine();
					stringBuilder_0.AppendLine("</li>");
				}
				stringBuilder_0.AppendLine("</ul>");
			}
		}
		private DataTable GetTabProductList(XmlNode node, string whereName)
		{
			SubjectListQuery subjectListQuery = new SubjectListQuery();
			subjectListQuery.SortBy = "DisplaySequence";
			subjectListQuery.SortOrder = SortAction.Desc;
			string value = node.Attributes[whereName].Value;
			if (!string.IsNullOrEmpty(value))
			{
				string[] array = value.Split(new char[]
				{
					','
				});
				subjectListQuery.CategoryIds = array[0];
				if (!string.IsNullOrEmpty(array[1]))
				{
					subjectListQuery.TagId = int.Parse(array[1]);
				}
				if (!string.IsNullOrEmpty(array[2]))
				{
					subjectListQuery.BrandCategoryId = new int?(int.Parse(array[2]));
				}
			}
			subjectListQuery.MaxNum = int.Parse(node.Attributes["MaxNum"].Value);
			return ProductBrowser.GetSubjectList(subjectListQuery);
		}
	}
}
