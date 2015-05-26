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
	public class Common_SubjectProduct_Simple : WebControl
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
			XmlNode xmlNode = TagsHelper.FindProductNode(this.SubjectId, "simple");
			StringBuilder stringBuilder = new StringBuilder();
			if (xmlNode != null)
			{
				stringBuilder.AppendFormat("<div class=\"pro_simple{0} cssEdite\" type=\"simple\" id=\"products_{1}\" >", xmlNode.Attributes["ImageSize"].Value, this.SubjectId);
				DataTable productList = this.GetProductList(xmlNode);
				if (productList != null && productList.Rows.Count > 0)
				{
					stringBuilder.AppendLine("<ul>");
					foreach (DataRow dataRow in productList.Rows)
					{
						SiteSettings masterSettings = SettingsManager.GetMasterSettings(true);
						string str = masterSettings.DefaultProductImage;
						if (dataRow["ThumbnailUrl" + xmlNode.Attributes["ImageSize"].Value] != DBNull.Value)
						{
							str = dataRow["ThumbnailUrl" + xmlNode.Attributes["ImageSize"].Value].ToString();
						}
						stringBuilder.AppendLine("<li>");
						stringBuilder.AppendFormat("<div class=\"pic\"><a target=\"_blank\" href=\"{0}\"><img src=\"{1}\"  alt=\"{2}\" /></a></div>", Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[]
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
				stringBuilder.Append("</div>");
			}
			return stringBuilder.ToString();
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
			if (!string.IsNullOrEmpty(node.Attributes["TypeId"].Value))
			{
				subjectListQuery.ProductTypeId = new int?(int.Parse(node.Attributes["TypeId"].Value));
			}
			string value = node.Attributes["AttributeString"].Value;
			if (!string.IsNullOrEmpty(value))
			{
				IList<AttributeValueInfo> list = new List<AttributeValueInfo>();
				string[] array = value.Split(new char[]
				{
					','
				});
				for (int i = 0; i < array.Length; i++)
				{
					string[] array2 = array[i].Split(new char[]
					{
						'_'
					});
					list.Add(new AttributeValueInfo
					{
						AttributeId = Convert.ToInt32(array2[0]),
						ValueId = Convert.ToInt32(array2[1])
					});
				}
				subjectListQuery.AttributeValues = list;
			}
			subjectListQuery.MaxNum = int.Parse(node.Attributes["MaxNum"].Value);
			return ProductBrowser.GetSubjectList(subjectListQuery);
		}
	}
}
