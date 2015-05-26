using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Catalog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_GoodsList_CurrentTop : WebControl
	{
		private int imageSize = 60;
		public int MaxNum
		{
			get;
			set;
		}
		public int ImageNum
		{
			get;
			set;
		}
		public bool IsShowPrice
		{
			get;
			set;
		}
		public bool IsShowSaleCounts
		{
			get;
			set;
		}
		public int ImageSize
		{
			get
			{
				return this.imageSize;
			}
			set
			{
				this.imageSize = value;
			}
		}
		protected override void Render(HtmlTextWriter writer)
		{
			writer.Write(this.RendHtml());
		}
		public string RendHtml()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("<div class=\"sale_top\" >", new object[0]);
			DataTable productList = this.GetProductList();
			if (productList != null && productList.Rows.Count > 0)
			{
				int num = 0;
				stringBuilder.AppendLine("<ul>");
				foreach (DataRow dataRow in productList.Rows)
				{
					num++;
					stringBuilder.AppendFormat("<li class=\"saleitem{0}\">", num).AppendLine();
					stringBuilder.AppendFormat("<em>{0}</em>", num).AppendLine();
					if (num <= this.ImageNum)
					{
						stringBuilder.AppendFormat("<div class=\"img\"><a target=\"_blank\" href=\"{0}\"><img src=\"{1}\" /></a></div>", Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[]
						{
							dataRow["ProductId"]
						}), Globals.ApplicationPath + dataRow["ThumbnailUrl" + this.ImageSize]).AppendLine();
					}
					stringBuilder.AppendLine("<div class=\"info\">");
					stringBuilder.AppendFormat("<div class=\"name\"><a target=\"_blank\" href=\"{0}\">{1}</a></div>", Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[]
					{
						dataRow["ProductId"]
					}), dataRow["ProductName"]).AppendLine();
					if (this.IsShowPrice)
					{
						string arg = string.Empty;
						if (dataRow["MarketPrice"] != DBNull.Value)
						{
							arg = Globals.FormatMoney((decimal)dataRow["MarketPrice"]);
						}
						stringBuilder.AppendFormat("<div class=\"price\"><b>{0}</b><span>{1}</span></div>", Globals.FormatMoney((decimal)dataRow["RankPrice"]), arg).AppendLine();
					}
					if (this.IsShowSaleCounts)
					{
						stringBuilder.AppendFormat("<div class=\"sale\">已售出<b>{0}</b>件 </div>", dataRow["SaleCounts"]).AppendLine();
					}
					stringBuilder.Append("</div>");
					stringBuilder.AppendLine("</li>");
				}
				stringBuilder.AppendLine("</ul>");
			}
			stringBuilder.Append("</div>");
			return stringBuilder.ToString();
		}
		private DataTable GetProductList()
		{
			SubjectListQuery subjectListQuery = new SubjectListQuery();
			subjectListQuery.CategoryIds = this.Page.Request.QueryString["categoryId"];
			int value;
			if (int.TryParse(this.Page.Request.QueryString["brand"], out value))
			{
				subjectListQuery.BrandCategoryId = new int?(value);
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["valueStr"]))
			{
				IList<AttributeValueInfo> list = new List<AttributeValueInfo>();
				string text = Globals.UrlDecode(this.Page.Request.QueryString["valueStr"]);
				text = Globals.HtmlEncode(text);
				string[] array = text.Split(new char[]
				{
					'-'
				});
				if (!string.IsNullOrEmpty(text))
				{
					for (int i = 0; i < array.Length; i++)
					{
						string[] array2 = array[i].Split(new char[]
						{
							'_'
						});
						if (array2.Length > 0 && !string.IsNullOrEmpty(array2[1]) && array2[1] != "0")
						{
							list.Add(new AttributeValueInfo
							{
								AttributeId = Convert.ToInt32(array2[0]),
								ValueId = Convert.ToInt32(array2[1])
							});
						}
					}
				}
				subjectListQuery.AttributeValues = list;
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["keywords"]))
			{
				subjectListQuery.Keywords = DataHelper.CleanSearchString(Globals.HtmlEncode(Globals.UrlDecode(this.Page.Request.QueryString["keywords"])));
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["minSalePrice"]))
			{
				decimal value2 = 0m;
				if (decimal.TryParse(Globals.UrlDecode(this.Page.Request.QueryString["minSalePrice"]), out value2))
				{
					subjectListQuery.MinPrice = new decimal?(value2);
				}
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["maxSalePrice"]))
			{
				decimal value3 = 0m;
				if (decimal.TryParse(Globals.UrlDecode(this.Page.Request.QueryString["maxSalePrice"]), out value3))
				{
					subjectListQuery.MaxPrice = new decimal?(value3);
				}
			}
			if (!string.IsNullOrEmpty(this.Page.Request.QueryString["TagIds"]))
			{
				int tagId = 0;
				if (int.TryParse(this.Page.Request.QueryString["TagIds"], out tagId))
				{
					subjectListQuery.TagId = tagId;
				}
			}
			subjectListQuery.MaxNum = this.MaxNum;
			subjectListQuery.SortBy = "ShowSaleCounts";
			subjectListQuery.SortOrder = SortAction.Desc;
			Globals.EntityCoding(subjectListQuery, true);
			return ProductBrowser.GetSubjectList(subjectListQuery);
		}
	}
}
