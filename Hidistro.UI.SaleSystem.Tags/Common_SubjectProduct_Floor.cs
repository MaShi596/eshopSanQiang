using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Membership.Context;
using Hidistro.SaleSystem.Catalog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_SubjectProduct_Floor : WebControl
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
			StringBuilder stringBuilder = new StringBuilder();
			XmlNode xmlNode = TagsHelper.FindProductNode(this.SubjectId, "floor");
			if (xmlNode != null)
			{
				stringBuilder.AppendFormat("<div class=\"floor{0} cssEdite\" type=\"floor\" id=\"products_{1}\" >", xmlNode.Attributes["ImageSize"].Value, this.SubjectId).AppendLine();
				this.RenderHeader(xmlNode, stringBuilder);
				stringBuilder.AppendLine("<div class=\"floor_bd\">");
				if (!string.IsNullOrEmpty(xmlNode.Attributes["AdImage"].Value))
				{
					stringBuilder.AppendFormat("<div class=\"floor_ad\"><img src=\"{0}\"  /></div>", xmlNode.Attributes["AdImage"].Value).AppendLine();
				}
				else
				{
					stringBuilder.AppendFormat("<div class=\"floor_ad\"><img src=\"{0}\"  /></div>", SettingsManager.GetMasterSettings(true).DefaultProductImage).AppendLine();
				}
				stringBuilder.AppendLine("<div class=\"floor_pro\">");
				DataTable productList = this.GetProductList(xmlNode);
				if (productList != null && productList.Rows.Count > 0)
				{
					stringBuilder.AppendLine("<ul>");
					foreach (DataRow dataRow in productList.Rows)
					{
						SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
						string str = masterSettings.DefaultProductImage;
						if (dataRow["ThumbnailUrl" + xmlNode.Attributes["ImageSize"].Value] != DBNull.Value)
						{
							str = dataRow["ThumbnailUrl" + xmlNode.Attributes["ImageSize"].Value].ToString();
						}
						stringBuilder.AppendLine("<li>");
						stringBuilder.AppendFormat("<div class=\"pic\"><a target=\"_blank\" href=\"{0}\"><img src=\"{1}\" alt=\"{2}\" /></a></div>", Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[]
						{
							dataRow["ProductId"]
						}), Globals.ApplicationPath + str, dataRow["ProductName"]).AppendLine();
						stringBuilder.AppendFormat("<div class=\"name\"><a target=\"_blank\" href=\"{0}\">{1}</a></div>", Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[]
						{
							dataRow["ProductId"]
						}), dataRow["ProductName"]).AppendLine();
						string arg = string.Empty;
						if (dataRow["MarketPrice"] != DBNull.Value)
						{
							arg = Globals.FormatMoney((decimal)dataRow["MarketPrice"]);
						}
						stringBuilder.AppendFormat("<div class=\"price\"><b><em>￥</em>{0}</b><span><em>￥</em>{1}</span></div>", Globals.FormatMoney((decimal)dataRow["RankPrice"]), arg).AppendLine();
						stringBuilder.AppendFormat("<a class=\"productview\" target=\"_blank\" href=\"{0}\">查看详细</a>", Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[]
						{
							dataRow["ProductId"]
						})).AppendLine();
						stringBuilder.AppendLine("</li>");
					}
					stringBuilder.AppendLine("</ul>");
				}
				stringBuilder.AppendLine("</div>");
				stringBuilder.AppendLine("</div>");
				stringBuilder.AppendLine("</div>");
			}
			return stringBuilder.ToString();
		}
		private void RenderHeader(XmlNode node, StringBuilder stringBuilder_0)
		{
			stringBuilder_0.AppendLine("<div class=\"floor_hd\">");
			stringBuilder_0.AppendLine("<div>");
			if (!string.IsNullOrEmpty(node.Attributes["ImageTitle"].Value))
			{
				stringBuilder_0.AppendFormat("<span class=\"icon\"><img src=\"{0}\" /></span>", Globals.ApplicationPath + node.Attributes["ImageTitle"].Value);
			}
			if (!string.IsNullOrEmpty(node.Attributes["Title"].Value))
			{
				stringBuilder_0.AppendFormat("<span class=\"title\">{0}</span>", node.Attributes["Title"].Value);
			}
			stringBuilder_0.AppendLine("</div>");
			int num = 0;
			if (int.TryParse(node.Attributes["CategoryId"].Value, out num))
			{
				IList<CategoryInfo> maxSubCategories = CategoryBrowser.GetMaxSubCategories(num, int.Parse(node.Attributes["SubCategoryNum"].Value));
				if (maxSubCategories != null && maxSubCategories.Count > 0)
				{
					stringBuilder_0.AppendLine("<ul>");
					foreach (CategoryInfo current in maxSubCategories)
					{
						stringBuilder_0.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", Globals.GetSiteUrls().SubCategory(current.CategoryId, current.RewriteName), current.Name).AppendLine("");
					}
					stringBuilder_0.AppendLine("</ul>");
				}
				if (node.Attributes["IsShowMoreLink"].Value == "true")
				{
					stringBuilder_0.AppendFormat("<em><a href=\"{0}\">更多>></a></em>", Globals.GetSiteUrls().SubCategory(num, null)).AppendLine();
				}
			}
			stringBuilder_0.AppendLine("</div>");
		}
		private DataTable GetProductList(XmlNode node)
		{
			SubjectListQuery subjectListQuery = new SubjectListQuery();
			subjectListQuery.SortBy = "DisplaySequence";
			subjectListQuery.SortOrder = SortAction.Desc;
			subjectListQuery.CategoryIds = node.Attributes["CategoryId"].Value;
			if (!string.IsNullOrEmpty(node.Attributes["TagId"].Value))
			{
				subjectListQuery.TagId = int.Parse(node.Attributes["TagId"].Value);
			}
			if (!string.IsNullOrEmpty(node.Attributes["BrandId"].Value))
			{
				subjectListQuery.BrandCategoryId = new int?(int.Parse(node.Attributes["BrandId"].Value));
			}
			subjectListQuery.MaxNum = int.Parse(node.Attributes["MaxNum"].Value);
			return ProductBrowser.GetSubjectList(subjectListQuery);
		}
	}
}
