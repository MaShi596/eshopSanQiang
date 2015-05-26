using Hidistro.ControlPanel.Commodities;
using Hidistro.ControlPanel.Distribution;
using Hidistro.ControlPanel.Members;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Members;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
namespace Hidistro.UI.Web.Admin
{
	public class ProductBasePage : AdminPage
	{
		protected void DoCallback()
		{
			base.Response.Clear();
			base.Response.ContentType = "application/json";
			string text = base.Request.QueryString["action"];
			if (text.Equals("getPrepareData"))
			{
				int typeId = int.Parse(base.Request.QueryString["typeId"]);
				System.Collections.Generic.IList<AttributeInfo> attributes = ProductTypeHelper.GetAttributes(typeId);
				System.Data.DataTable dataTable = ProductTypeHelper.GetBrandCategoriesByTypeId(typeId);
				if (dataTable.Rows.Count == 0)
				{
					dataTable = ControlProvider.Instance().GetBrandCategories();
				}
				base.Response.Write(this.GenerateJsonString(attributes, dataTable));
				attributes.Clear();
			}
			else
			{
				if (text.Equals("getMemberGradeList"))
				{
					System.Collections.Generic.IList<MemberGradeInfo> memberGrades = MemberHelper.GetMemberGrades();
					if (memberGrades != null && memberGrades.Count != 0)
					{
						System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
						stringBuilder.Append("{\"Status\":\"OK\",\"MemberGrades\":[");
						foreach (MemberGradeInfo current in memberGrades)
						{
							stringBuilder.Append("{");
							stringBuilder.AppendFormat("\"GradeId\":\"{0}\",", current.GradeId);
							stringBuilder.AppendFormat("\"Name\":\"{0}\",", current.Name);
							stringBuilder.AppendFormat("\"Discount\":\"{0}\"", current.Discount);
							stringBuilder.Append("},");
						}
						stringBuilder.Remove(stringBuilder.Length - 1, 1);
						stringBuilder.Append("]}");
						base.Response.Write(stringBuilder.ToString());
					}
					else
					{
						base.Response.Write("{\"Status\":\"0\"}");
					}
				}
				else
				{
					if (text.Equals("getDistributorGradeList"))
					{
						System.Data.DataTable distributorGrades = DistributorHelper.GetDistributorGrades();
						if (distributorGrades != null && distributorGrades.Rows.Count != 0)
						{
							System.Text.StringBuilder stringBuilder2 = new System.Text.StringBuilder();
							stringBuilder2.Append("{\"Status\":\"OK\",\"DistributorGrades\":[");
							foreach (System.Data.DataRow dataRow in distributorGrades.Rows)
							{
								stringBuilder2.Append("{");
								stringBuilder2.AppendFormat("\"GradeId\":\"{0}\",", (int)dataRow["GradeId"]);
								stringBuilder2.AppendFormat("\"Name\":\"{0}\",", (string)dataRow["Name"]);
								stringBuilder2.AppendFormat("\"Discount\":\"{0}\"", (int)dataRow["Discount"]);
								stringBuilder2.Append("},");
							}
							stringBuilder2.Remove(stringBuilder2.Length - 1, 1);
							stringBuilder2.Append("]}");
							base.Response.Write(stringBuilder2.ToString());
						}
						else
						{
							base.Response.Write("{\"Status\":\"0\"}");
						}
					}
				}
			}
			base.Response.End();
		}
		protected void GetMemberPrices(SKUItem sku, string xml)
		{
			if (string.IsNullOrEmpty(xml))
			{
				return;
			}
			System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
			xmlDocument.LoadXml(xml);
			foreach (System.Xml.XmlNode xmlNode in xmlDocument.DocumentElement.SelectNodes("//grande"))
			{
				if (!string.IsNullOrEmpty(xmlNode.Attributes["price"].Value) && xmlNode.Attributes["price"].Value.Trim().Length != 0)
				{
					sku.MemberPrices.Add(int.Parse(xmlNode.Attributes["id"].Value), decimal.Parse(xmlNode.Attributes["price"].Value.Trim()));
				}
			}
		}
		protected void GetDistributorPrices(SKUItem sku, string xml)
		{
			if (string.IsNullOrEmpty(xml))
			{
				return;
			}
			System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
			xmlDocument.LoadXml(xml);
			foreach (System.Xml.XmlNode xmlNode in xmlDocument.DocumentElement.SelectNodes("//grande"))
			{
				if (!string.IsNullOrEmpty(xmlNode.Attributes["price"].Value) && xmlNode.Attributes["price"].Value.Trim().Length != 0)
				{
					sku.DistributorPrices.Add(int.Parse(xmlNode.Attributes["id"].Value), decimal.Parse(xmlNode.Attributes["price"].Value.Trim()));
				}
			}
		}
		protected System.Collections.Generic.Dictionary<int, System.Collections.Generic.IList<int>> GetAttributes(string attributesXml)
		{
			System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
			System.Collections.Generic.Dictionary<int, System.Collections.Generic.IList<int>> dictionary = null;
			System.Collections.Generic.Dictionary<int, System.Collections.Generic.IList<int>> result;
			try
			{
				xmlDocument.LoadXml(attributesXml);
				System.Xml.XmlNodeList xmlNodeList = xmlDocument.SelectNodes("//item");
				if (xmlNodeList != null && xmlNodeList.Count != 0)
				{
					dictionary = new System.Collections.Generic.Dictionary<int, System.Collections.Generic.IList<int>>();
					foreach (System.Xml.XmlNode xmlNode in xmlNodeList)
					{
						int key = int.Parse(xmlNode.Attributes["attributeId"].Value);
						System.Collections.Generic.IList<int> list = new System.Collections.Generic.List<int>();
						foreach (System.Xml.XmlNode xmlNode2 in xmlNode.ChildNodes)
						{
							if (xmlNode2.Attributes["valueId"].Value != "")
							{
								list.Add(int.Parse(xmlNode2.Attributes["valueId"].Value));
							}
						}
						if (list.Count > 0)
						{
							dictionary.Add(key, list);
						}
					}
					return dictionary;
				}
				result = null;
			}
			catch
			{
				return dictionary;
			}
			return result;
		}
		protected System.Collections.Generic.Dictionary<string, SKUItem> GetSkus(string skusXml)
		{
			System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
			System.Collections.Generic.Dictionary<string, SKUItem> dictionary = null;
			System.Collections.Generic.Dictionary<string, SKUItem> result;
			try
			{
				xmlDocument.LoadXml(skusXml);
				System.Xml.XmlNodeList xmlNodeList = xmlDocument.SelectNodes("//item");
				if (xmlNodeList != null && xmlNodeList.Count != 0)
				{
					dictionary = new System.Collections.Generic.Dictionary<string, SKUItem>();
					foreach (System.Xml.XmlNode xmlNode in xmlNodeList)
					{
						SKUItem sKUItem = new SKUItem
						{
							SKU = xmlNode.Attributes["skuCode"].Value,
							SalePrice = decimal.Parse(xmlNode.Attributes["salePrice"].Value),
							CostPrice = (xmlNode.Attributes["costPrice"].Value.Length > 0) ? decimal.Parse(xmlNode.Attributes["costPrice"].Value) : 0m,
							PurchasePrice = decimal.Parse(xmlNode.Attributes["purchasePrice"].Value),
							Stock = int.Parse(xmlNode.Attributes["qty"].Value),
							AlertStock = int.Parse(xmlNode.Attributes["alertQty"].Value),
							Weight = (xmlNode.Attributes["weight"].Value.Length > 0) ? decimal.Parse(xmlNode.Attributes["weight"].Value) : 0m
						};
						string text = "";
						foreach (System.Xml.XmlNode xmlNode2 in xmlNode.SelectSingleNode("skuFields").ChildNodes)
						{
							text = text + xmlNode2.Attributes["valueId"].Value + "_";
							sKUItem.SkuItems.Add(int.Parse(xmlNode2.Attributes["attributeId"].Value), int.Parse(xmlNode2.Attributes["valueId"].Value));
						}
						System.Xml.XmlNode xmlNode3 = xmlNode.SelectSingleNode("memberPrices");
						if (xmlNode3 != null && xmlNode3.ChildNodes.Count > 0)
						{
							foreach (System.Xml.XmlNode xmlNode4 in xmlNode3.ChildNodes)
							{
								if (!string.IsNullOrEmpty(xmlNode4.Attributes["price"].Value) && xmlNode4.Attributes["price"].Value.Trim().Length != 0)
								{
									sKUItem.MemberPrices.Add(int.Parse(xmlNode4.Attributes["id"].Value), decimal.Parse(xmlNode4.Attributes["price"].Value.Trim()));
								}
							}
						}
						System.Xml.XmlNode xmlNode5 = xmlNode.SelectSingleNode("distributorPrices");
						if (xmlNode5 != null && xmlNode5.ChildNodes.Count > 0)
						{
							foreach (System.Xml.XmlNode xmlNode6 in xmlNode5.ChildNodes)
							{
								if (!string.IsNullOrEmpty(xmlNode6.Attributes["price"].Value) && xmlNode6.Attributes["price"].Value.Trim().Length != 0)
								{
									sKUItem.DistributorPrices.Add(int.Parse(xmlNode6.Attributes["id"].Value), decimal.Parse(xmlNode6.Attributes["price"].Value.Trim()));
								}
							}
						}
						sKUItem.SkuId = text.Substring(0, text.Length - 1);
						dictionary.Add(sKUItem.SkuId, sKUItem);
					}
					result = dictionary;
				}
				else
				{
					result = null;
				}
			}
			catch
			{
				result = null;
			}
			return result;
		}
		private string GenerateJsonString(System.Collections.Generic.IList<AttributeInfo> attributes, System.Data.DataTable tbBrandCategories)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			System.Text.StringBuilder stringBuilder2 = new System.Text.StringBuilder();
			System.Text.StringBuilder stringBuilder3 = new System.Text.StringBuilder();
			System.Text.StringBuilder stringBuilder4 = new System.Text.StringBuilder();
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			if (attributes != null && attributes.Count > 0)
			{
				stringBuilder2.Append("\"Attributes\":[");
				stringBuilder3.Append("\"SKUs\":[");
				foreach (AttributeInfo current in attributes)
				{
					if (current.UsageMode == AttributeUseageMode.Choose)
					{
						flag2 = true;
						stringBuilder3.Append("{");
						stringBuilder3.AppendFormat("\"Name\":\"{0}\",", current.AttributeName);
						stringBuilder3.AppendFormat("\"AttributeId\":\"{0}\",", current.AttributeId.ToString(System.Globalization.CultureInfo.InvariantCulture));
						stringBuilder3.AppendFormat("\"UseAttributeImage\":\"{0}\",", current.UseAttributeImage ? 1 : 0);
						stringBuilder3.AppendFormat("\"SKUValues\":[{0}]", this.GenerateValueItems(current.AttributeValues));
						stringBuilder3.Append("},");
					}
					else
					{
						if (current.UsageMode == AttributeUseageMode.View || current.UsageMode == AttributeUseageMode.MultiView)
						{
							flag = true;
							stringBuilder2.Append("{");
							stringBuilder2.AppendFormat("\"Name\":\"{0}\",", current.AttributeName);
							stringBuilder2.AppendFormat("\"AttributeId\":\"{0}\",", current.AttributeId.ToString(System.Globalization.CultureInfo.InvariantCulture));
							stringBuilder2.AppendFormat("\"UsageMode\":\"{0}\",", ((int)current.UsageMode).ToString());
							stringBuilder2.AppendFormat("\"AttributeValues\":[{0}]", this.GenerateValueItems(current.AttributeValues));
							stringBuilder2.Append("},");
						}
					}
				}
				if (stringBuilder2.Length > 14)
				{
					stringBuilder2.Remove(stringBuilder2.Length - 1, 1);
				}
				if (stringBuilder3.Length > 8)
				{
					stringBuilder3.Remove(stringBuilder3.Length - 1, 1);
				}
				stringBuilder2.Append("]");
				stringBuilder3.Append("]");
			}
			if (tbBrandCategories != null && tbBrandCategories.Rows.Count > 0)
			{
				flag3 = true;
				stringBuilder4.AppendFormat("\"BrandCategories\":[{0}]", this.GenerateBrandString(tbBrandCategories));
			}
			stringBuilder.Append("{\"HasAttribute\":\"" + flag.ToString() + "\",");
			stringBuilder.Append("\"HasSKU\":\"" + flag2.ToString() + "\",");
			stringBuilder.Append("\"HasBrandCategory\":\"" + flag3.ToString() + "\",");
			if (flag)
			{
				stringBuilder.Append(stringBuilder2.ToString()).Append(",");
			}
			if (flag2)
			{
				stringBuilder.Append(stringBuilder3.ToString()).Append(",");
			}
			if (flag3)
			{
				stringBuilder.Append(stringBuilder4.ToString()).Append(",");
			}
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
			stringBuilder.Append("}");
			return stringBuilder.ToString();
		}
		private string GenerateValueItems(System.Collections.Generic.IList<AttributeValueInfo> values)
		{
			if (values != null && values.Count != 0)
			{
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
				foreach (AttributeValueInfo current in values)
				{
					stringBuilder.Append("{");
					stringBuilder.AppendFormat("\"ValueId\":\"{0}\",\"ValueStr\":\"{1}\"", current.ValueId.ToString(System.Globalization.CultureInfo.InvariantCulture), current.ValueStr);
					stringBuilder.Append("},");
				}
				stringBuilder.Remove(stringBuilder.Length - 1, 1);
				return stringBuilder.ToString();
			}
			return string.Empty;
		}
		private string GenerateBrandString(System.Data.DataTable tb)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			foreach (System.Data.DataRow dataRow in tb.Rows)
			{
				stringBuilder.Append("{");
				stringBuilder.AppendFormat("\"BrandId\":\"{0}\",\"BrandName\":\"{1}\"", dataRow["BrandID"], dataRow["BrandName"]);
				stringBuilder.Append("},");
			}
			stringBuilder.Remove(stringBuilder.Length - 1, 1);
			return stringBuilder.ToString();
		}
		protected string DownRemotePic(string productDescrip)
		{
			Hidistro.Membership.Context.SiteSettings arg_0A_0 = Hidistro.Membership.Context.HiContext.Current.SiteSettings;
			string str = string.Format("/Storage/master/gallery/{0}/", System.DateTime.Now.Year.ToString() + System.DateTime.Now.Month.ToString());
			string text = System.Web.HttpContext.Current.Request.MapPath(Globals.ApplicationPath + str);
			if (!System.IO.Directory.Exists(text))
			{
				System.IO.Directory.CreateDirectory(text);
			}
			System.Collections.Generic.IList<string> outsiteLinkImgs = this.GetOutsiteLinkImgs(productDescrip);
			if (outsiteLinkImgs.Count > 0)
			{
				foreach (string current in outsiteLinkImgs)
				{
					System.Net.WebClient webClient = new System.Net.WebClient();
					string text2 = System.Guid.NewGuid().ToString("N", System.Globalization.CultureInfo.InvariantCulture);
					string text3 = current.Substring(current.LastIndexOf('.'));
					try
					{
						webClient.DownloadFile(current, text + text2 + text3);
						productDescrip = productDescrip.Replace(current, Globals.ApplicationPath + str + text2 + text3);
					}
					catch
					{
					}
				}
			}
			return productDescrip;
		}
		private System.Collections.Generic.IList<string> GetOutsiteLinkImgs(string html)
		{
			Hidistro.Membership.Context.SiteSettings siteSettings = Hidistro.Membership.Context.HiContext.Current.SiteSettings;
			Hidistro.Membership.Context.SiteSettings masterSettings = Hidistro.Membership.Context.SettingsManager.GetMasterSettings(false);
			System.Collections.Generic.IList<string> list = new System.Collections.Generic.List<string>();
			System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("(src)[^>]*[^/].(?:jpg|bmp|gif|png)(?:\"|') ", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
			System.Text.RegularExpressions.MatchCollection matchCollection = regex.Matches(html);
			for (int i = 0; i < matchCollection.Count; i++)
			{
				string text = matchCollection[i].Value.Replace("\\", "").Replace("\"", "").Replace("'", "").Trim();
				text = text.Substring(4);
				if (text.ToLower(System.Globalization.CultureInfo.InvariantCulture).IndexOf(siteSettings.SiteUrl.ToLower(System.Globalization.CultureInfo.InvariantCulture)) == -1 && text.ToLower(System.Globalization.CultureInfo.InvariantCulture).IndexOf(masterSettings.SiteUrl.ToLower(System.Globalization.CultureInfo.InvariantCulture)) == -1)
				{
					list.Add(text);
				}
			}
			return list;
		}
	}
}
