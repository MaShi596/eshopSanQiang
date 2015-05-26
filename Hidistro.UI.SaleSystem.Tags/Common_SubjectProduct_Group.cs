using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Membership.Context;
using Hidistro.SaleSystem.Catalog;
using Hidistro.SaleSystem.Comments;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_SubjectProduct_Group : WebControl
	{
		private int categoryId;
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
			XmlNode xmlNode = TagsHelper.FindProductNode(this.SubjectId, "group");
			if (xmlNode != null)
			{
				int.TryParse(xmlNode.Attributes["CategoryId"].Value, out this.categoryId);
				stringBuilder.AppendFormat("<div class=\"group{0} cssEdite\" type=\"group\" id=\"products_{1}\" >", xmlNode.Attributes["ImageSize"].Value, this.SubjectId).AppendLine();
				this.RenderHeader(xmlNode, stringBuilder);
				stringBuilder.AppendLine("<div class=\"group_bd\">");
				this.RenderLift(xmlNode, stringBuilder);
				this.RenderMiddle(xmlNode, stringBuilder);
				this.RenderRight(xmlNode, stringBuilder);
				stringBuilder.AppendLine("</div>");
				stringBuilder.AppendLine("</div>");
			}
			return stringBuilder.ToString();
		}
		private void RenderHeader(XmlNode node, StringBuilder stringBuilder_0)
		{
			stringBuilder_0.AppendLine("<div class=\"group_hd\">");
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
			CategoryInfo category;
			for (category = CategoryBrowser.GetCategory(this.categoryId); category != null; category = CategoryBrowser.GetCategory(this.categoryId))
			{
				if (category.Depth == 1)
				{
					break;
				}
				this.categoryId = category.ParentCategoryId.Value;
			}
			if (category != null)
			{
				this.categoryId = category.CategoryId;
			}
			DataTable hotKeywords = CommentBrowser.GetHotKeywords(this.categoryId, int.Parse(node.Attributes["HotKeywordNum"].Value));
			if (hotKeywords != null && hotKeywords.Rows.Count > 0)
			{
				stringBuilder_0.AppendLine("<ul>");
				foreach (DataRow dataRow in hotKeywords.Rows)
				{
					stringBuilder_0.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", Globals.GetSiteUrls().SubCategory((int)dataRow["CategoryId"], null) + "?keywords=" + Globals.UrlEncode((string)dataRow["Keywords"]), dataRow["Keywords"]).AppendLine("");
				}
				stringBuilder_0.AppendLine("</ul>");
			}
			if (node.Attributes["IsShowMoreLink"].Value == "true")
			{
				stringBuilder_0.AppendFormat("<em><a href=\"{0}\">更多>></a></em>", Globals.GetSiteUrls().SubCategory(this.categoryId, null)).AppendLine();
			}
			stringBuilder_0.AppendLine("</div>");
		}
		private void RenderLift(XmlNode node, StringBuilder stringBuilder_0)
		{
			stringBuilder_0.AppendLine("<div class=\"bd_left\">");
			IList<CategoryInfo> maxSubCategories = CategoryBrowser.GetMaxSubCategories(this.categoryId, int.Parse(node.Attributes["SubCategoryNum"].Value));
			if (maxSubCategories != null && maxSubCategories.Count > 0)
			{
				stringBuilder_0.AppendLine("<ul>");
				foreach (CategoryInfo current in maxSubCategories)
				{
					stringBuilder_0.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", Globals.GetSiteUrls().SubCategory(current.CategoryId, current.RewriteName), current.Name).AppendLine("");
				}
				stringBuilder_0.AppendLine("</ul>");
			}
			if (!string.IsNullOrEmpty(node.Attributes["AdImageLeft"].Value))
			{
				stringBuilder_0.AppendFormat("<div class=\"bd_left_ad\"><img src=\"{0}\"  /></div>", node.Attributes["AdImageLeft"].Value);
			}
			stringBuilder_0.AppendLine("</div>");
		}
		private void RenderMiddle(XmlNode node, StringBuilder stringBuilder_0)
		{
			stringBuilder_0.AppendLine("<div class=\"bd_middle\">");
			if (!string.IsNullOrEmpty(node.Attributes["AdImageRight"].Value))
			{
				stringBuilder_0.AppendFormat("<div class=\"bd_right_ad\"><img src=\"{0}\"  /></div>", node.Attributes["AdImageRight"].Value);
			}
			DataTable productList = this.GetProductList(node);
			if (productList != null && productList.Rows.Count > 0)
			{
				stringBuilder_0.AppendLine("<ul>");
				foreach (DataRow dataRow in productList.Rows)
				{
					SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
					string str;
					if (dataRow["ThumbnailUrl" + node.Attributes["ImageSize"].Value] != DBNull.Value)
					{
						str = dataRow["ThumbnailUrl" + node.Attributes["ImageSize"].Value].ToString();
					}
					else
					{
						str = masterSettings.DefaultProductImage;
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
			stringBuilder_0.AppendLine("</div>");
		}
		private void RenderRight(XmlNode node, StringBuilder stringBuilder_0)
		{
			stringBuilder_0.AppendLine("<div class=\"bd_right\">");
			this.RenderBrand(node, stringBuilder_0);
			this.RenderSaleTop(node, stringBuilder_0);
			stringBuilder_0.AppendLine("</div>");
		}
		private void RenderBrand(XmlNode node, StringBuilder stringBuilder_0)
		{
			DataTable brandCategories = CategoryBrowser.GetBrandCategories(this.categoryId, int.Parse(node.Attributes["BrandNum"].Value));
			if (brandCategories != null && brandCategories.Rows.Count > 0)
			{
				stringBuilder_0.AppendLine("<div class=\"bd_brand\">");
				stringBuilder_0.AppendLine("<ul>");
				foreach (DataRow dataRow in brandCategories.Rows)
				{
					stringBuilder_0.AppendFormat("<li><a href=\"{0}\"><img src=\"{1}\" /></a></li>", Globals.GetSiteUrls().SubBrandDetails((int)dataRow["BrandId"], dataRow["RewriteName"]), dataRow["Logo"]).AppendLine();
				}
				stringBuilder_0.AppendLine("</ul>");
				stringBuilder_0.AppendLine("</div>");
			}
		}
		private void RenderSaleTop(XmlNode node, StringBuilder stringBuilder_0)
		{
			DataTable saleProductRanking = ProductBrowser.GetSaleProductRanking(new int?(this.categoryId), int.Parse(node.Attributes["SaleTopNum"].Value));
			if (saleProductRanking != null && saleProductRanking.Rows.Count > 0)
			{
				int num = 0;
				int.TryParse(node.Attributes["ImageNum"].Value, out num);
				bool flag = false;
				bool.TryParse(node.Attributes["IsShowPrice"].Value, out flag);
				bool flag2 = false;
				bool.TryParse(node.Attributes["IsShowSaleCounts"].Value, out flag2);
				bool flag3 = false;
				bool.TryParse(node.Attributes["IsImgShowPrice"].Value, out flag3);
				bool flag4 = false;
				bool.TryParse(node.Attributes["IsImgShowSaleCounts"].Value, out flag4);
				stringBuilder_0.AppendLine("<div class=\"bd_saletop\">");
				stringBuilder_0.AppendLine("<ul>");
				int num2 = 0;
				SiteSettings siteSettings = HiContext.Current.SiteSettings;
				foreach (DataRow dataRow in saleProductRanking.Rows)
				{
					string str = siteSettings.DefaultProductImage;
					if (dataRow["ThumbnailUrl" + node.Attributes["TopImageSize"].Value] != DBNull.Value)
					{
						str = dataRow["ThumbnailUrl" + node.Attributes["TopImageSize"].Value].ToString();
					}
					num2++;
					stringBuilder_0.AppendFormat("<li class=\"sale_top{0}\">", num2).AppendLine();
					if (num2 <= num)
					{
						stringBuilder_0.AppendFormat("<div class=\"pic\"><a target=\"_blank\" href=\"{0}\"><img src=\"{1}\" /></a></div>", Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[]
						{
							dataRow["ProductId"]
						}), Globals.ApplicationPath + str).AppendLine();
					}
					stringBuilder_0.AppendLine("<div class=\"info\">");
					stringBuilder_0.AppendFormat("<div class=\"name\"><a target=\"_blank\" href=\"{0}\">{1}</a></div>", Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[]
					{
						dataRow["ProductId"]
					}), dataRow["ProductName"]).AppendLine();
					if ((flag && num2 > num) || (flag3 && num2 <= num))
					{
						string arg = string.Empty;
						if (dataRow["MarketPrice"] != DBNull.Value)
						{
							arg = Globals.FormatMoney((decimal)dataRow["MarketPrice"]);
						}
						stringBuilder_0.AppendFormat("<div class=\"price\"><b>{0}</b><span>{1}</span></div>", Globals.FormatMoney((decimal)dataRow["SalePrice"]), arg).AppendLine();
					}
					if ((flag2 && num2 > num) || (flag4 && num2 <= num))
					{
						stringBuilder_0.AppendFormat("<div class=\"sale\">已售出<b>{0}</b>件 </div>", dataRow["SaleCounts"]).AppendLine();
					}
					stringBuilder_0.Append("</div>");
					stringBuilder_0.AppendLine("</li>");
				}
				stringBuilder_0.AppendLine("</ul>");
				stringBuilder_0.AppendLine("</div>");
			}
		}
		private DataTable GetProductList(XmlNode node)
		{
			return ProductBrowser.GetSubjectList(new SubjectListQuery
			{
				SortBy = "DisplaySequence",
				SortOrder = SortAction.Desc,
				CategoryIds = node.Attributes["CategoryId"].Value,
				MaxNum = int.Parse(node.Attributes["MaxNum"].Value)
			});
		}
	}
}
