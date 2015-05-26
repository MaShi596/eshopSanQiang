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
	public class Common_SubjectProduct_Top : WebControl
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
			XmlNode xmlNode = TagsHelper.FindProductNode(this.SubjectId, "top");
			StringBuilder stringBuilder = new StringBuilder();
			if (xmlNode != null)
			{
				int num = 0;
				int.TryParse(xmlNode.Attributes["ImageNum"].Value, out num);
				bool flag = false;
				bool.TryParse(xmlNode.Attributes["IsShowPrice"].Value, out flag);
				bool flag2 = false;
				bool.TryParse(xmlNode.Attributes["IsShowSaleCounts"].Value, out flag2);
				bool flag3 = false;
				bool.TryParse(xmlNode.Attributes["IsImgShowPrice"].Value, out flag3);
				bool flag4 = false;
				bool.TryParse(xmlNode.Attributes["IsImgShowSaleCounts"].Value, out flag4);
				stringBuilder.AppendFormat("<div class=\"sale_top{0} cssEdite\" type=\"top\" id=\"products_{1}\" >", xmlNode.Attributes["ImageSize"].Value, this.SubjectId);
				DataTable productList = this.GetProductList(xmlNode);
				if (productList != null && productList.Rows.Count > 0)
				{
					int num2 = 0;
					stringBuilder.AppendLine("<ul>");
					foreach (DataRow dataRow in productList.Rows)
					{
						SiteSettings masterSettings = SettingsManager.GetMasterSettings(false);
						string str = masterSettings.DefaultProductImage;
						if (dataRow["ThumbnailUrl" + xmlNode.Attributes["ImageSize"].Value] != DBNull.Value)
						{
							str = dataRow["ThumbnailUrl" + xmlNode.Attributes["ImageSize"].Value].ToString();
						}
						num2++;
						stringBuilder.AppendFormat("<li class=\"sale_top{0}\">", num2).AppendLine();
						stringBuilder.AppendFormat("<em>{0}</em>", num2).AppendLine();
						if (num2 <= num)
						{
							stringBuilder.AppendFormat("<div class=\"pic\"><a target=\"_blank\" href=\"{0}\"><img src=\"{1}\" alt=\"{2}\" /></a></div>", Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[]
							{
								dataRow["ProductId"]
							}), Globals.ApplicationPath + str, dataRow["ProductName"]).AppendLine();
						}
						stringBuilder.AppendLine("<div class=\"info\">");
						stringBuilder.AppendFormat("<div class=\"name\"><a target=\"_blank\" href=\"{0}\">{1}</a></div>", Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[]
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
							stringBuilder.AppendFormat("<div class=\"price\"><b><em>￥</em>{0}</b><span><em>￥</em>{1}</span></div>", Globals.FormatMoney((decimal)dataRow["RankPrice"]), arg).AppendLine();
							stringBuilder.AppendFormat("<a class=\"productview\" target=\"_blank\" href=\"{0}\">查看详细</a>", Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[]
							{
								dataRow["ProductId"]
							})).AppendLine();
						}
						if ((flag2 && num2 > num) || (flag4 && num2 <= num))
						{
							stringBuilder.AppendFormat("<div class=\"sale\">已售出<b>{0}</b>件 </div>", dataRow["SaleCounts"]).AppendLine();
						}
						stringBuilder.Append("</div>");
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
			return ProductBrowser.GetSubjectList(new SubjectListQuery
			{
				SortBy = "ShowSaleCounts",
				SortOrder = SortAction.Desc,
				CategoryIds = node.Attributes["CategoryId"].Value,
				MaxNum = int.Parse(node.Attributes["MaxNum"].Value)
			});
		}
	}
}
