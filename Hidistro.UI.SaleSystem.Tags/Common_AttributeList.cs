using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.SaleSystem.Catalog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.SaleSystem.Tags
{
	public class Common_AttributeList : WebControl
	{
		private int categoryId;
		private int brandId;
		private string valueStr = string.Empty;
		protected override void OnInit(EventArgs eventArgs_0)
		{
			int.TryParse(this.Context.Request.QueryString["categoryId"], out this.categoryId);
			int.TryParse(this.Context.Request.QueryString["brand"], out this.brandId);
			this.valueStr = Globals.UrlDecode(this.Page.Request.QueryString["valueStr"]);
			base.OnInit(eventArgs_0);
		}
		protected override void Render(HtmlTextWriter writer)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("<div class=\"attribute_bd\">");
			this.RendeBrand(stringBuilder);
			this.RendeAttribute(stringBuilder);
			stringBuilder.AppendLine("</div>");
			writer.Write(stringBuilder.ToString());
		}
		private void RendeBrand(StringBuilder stringBuilder_0)
		{
			DataTable brandCategories = CategoryBrowser.GetBrandCategories(this.categoryId, 1000);
			if (brandCategories != null && brandCategories.Rows.Count > 0)
			{
				stringBuilder_0.AppendLine("<dl class=\"attribute_dl\">");
				stringBuilder_0.AppendLine("<dt class=\"attribute_name\">品牌：</dt>");
				stringBuilder_0.AppendLine("<dd class=\"attribute_val\">");
				stringBuilder_0.AppendLine("<div class=\"h_chooselist\">");
				string text = "all";
				if (this.brandId == 0)
				{
					text += " select";
				}
				stringBuilder_0.AppendFormat("<a class=\"{0}\" href=\"{1}\" >全部</a>", text, this.CreateUrl("brand", "")).AppendLine();
				foreach (DataRow dataRow in brandCategories.Rows)
				{
					text = string.Empty;
					if (this.brandId == (int)dataRow["BrandId"])
					{
						text += " select";
					}
					stringBuilder_0.AppendFormat("<a class=\"{0}\" href=\"{1}\" >{2}</a>", text, this.CreateUrl("brand", dataRow["BrandId"].ToString()), dataRow["BrandName"]).AppendLine();
				}
				stringBuilder_0.AppendLine("</div>");
				stringBuilder_0.AppendLine("</dd>");
				stringBuilder_0.AppendLine("</dl>");
			}
		}
		private void RendeAttribute(StringBuilder stringBuilder_0)
		{
			IList<AttributeInfo> attributeInfoByCategoryId = CategoryBrowser.GetAttributeInfoByCategoryId(this.categoryId, 1000);
			if (attributeInfoByCategoryId != null && attributeInfoByCategoryId.Count > 0)
			{
				foreach (AttributeInfo current in attributeInfoByCategoryId)
				{
					stringBuilder_0.AppendLine("<dl class=\"attribute_dl\">");
					if (current.AttributeValues.Count > 0)
					{
						stringBuilder_0.AppendFormat("<dt class=\"attribute_name\">{0}：</dt>", current.AttributeName).AppendLine();
						stringBuilder_0.AppendLine("<dd class=\"attribute_val\">");
						stringBuilder_0.AppendLine("<div class=\"h_chooselist\">");
						string paraValue = this.RemoveAttribute(this.valueStr, current.AttributeId, 0);
						string arg = "all select";
						if (!string.IsNullOrEmpty(this.valueStr) && new Regex(string.Format("{0}_[1-9]+", current.AttributeId)).IsMatch(this.valueStr))
						{
							arg = "all";
						}
						stringBuilder_0.AppendFormat("<a class=\"{0}\" href=\"{1}\" >全部</a>", arg, this.CreateUrl("valuestr", paraValue)).AppendLine();
						foreach (AttributeValueInfo current2 in current.AttributeValues)
						{
							string arg2 = string.Empty;
							paraValue = this.RemoveAttribute(this.valueStr, current.AttributeId, current2.ValueId);
							if (!string.IsNullOrEmpty(this.valueStr))
							{
								string[] source = this.valueStr.Split(new char[]
								{
									'-'
								});
								if (source.Contains(current.AttributeId + "_" + current2.ValueId))
								{
									arg2 = "select";
								}
							}
							stringBuilder_0.AppendFormat("<a class=\"{0}\" href=\"{1}\" >{2}</a>", arg2, this.CreateUrl("valuestr", paraValue), current2.ValueStr).AppendLine();
						}
						stringBuilder_0.AppendLine("</div>");
						stringBuilder_0.AppendLine("</dd>");
					}
					stringBuilder_0.AppendLine("</dl>");
				}
			}
		}
		private string RemoveAttribute(string paraValue, int attributeId, int valueId)
		{
			string text = string.Empty;
			if (!string.IsNullOrEmpty(paraValue))
			{
				string[] array = paraValue.Split(new char[]
				{
					'-'
				});
				if (array != null && array.Length > 0)
				{
					string[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						string text2 = array2[i];
						if (!string.IsNullOrEmpty(text2))
						{
							string[] array3 = text2.Split(new char[]
							{
								'_'
							});
							if (array3 != null && array3.Length > 0 && array3[0] != attributeId.ToString())
							{
								text = text + text2 + "-";
							}
						}
					}
				}
			}
			return string.Concat(new object[]
			{
				text,
				attributeId,
				"_",
				valueId
			});
		}
		private string CreateUrl(string paraName, string paraValue)
		{
			string text = this.Context.Request.RawUrl;
			if (text.IndexOf("?") >= 0)
			{
				string text2 = text.Substring(text.IndexOf("?") + 1);
				string[] array = text2.Split(new char[]
				{
					Convert.ToChar("&")
				});
				text = text.Replace(text2, "");
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string text3 = array2[i];
					if (!text3.ToLower().StartsWith(paraName + "=") && !text3.ToLower().StartsWith("pageindex="))
					{
						text = text + text3 + "&";
					}
				}
				text = text + paraName + "=" + Globals.UrlEncode(paraValue);
			}
			else
			{
				string text4 = text;
				text = string.Concat(new string[]
				{
					text4,
					"?",
					paraName,
					"=",
					Globals.UrlEncode(paraValue)
				});
			}
			return text;
		}
	}
}
