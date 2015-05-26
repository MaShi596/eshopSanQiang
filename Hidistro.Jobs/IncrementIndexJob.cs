using Hidistro.Core;
using Hidistro.Core.Jobs;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Xml;
namespace Hidistro.Jobs
{
	public class IncrementIndexJob : IJob
	{
		private string prefixRootPath;
		private string prefixFullPath;
		private string prefixIncPath;
		private string fullVersion = "1.0";
		private string incVersion = "1.0";
		private string sellerCatsVersion = "1.0";
		private string seller_ID = "";
		private string webSite = "";
		private string storgePath = "";
		private IList<string> productsList = new List<string>();
		private void InitData()
		{
			this.fullVersion = this.fullVersion;
			this.incVersion = this.incVersion;
			this.sellerCatsVersion = this.sellerCatsVersion;
		}
		public void Execute(XmlNode node)
		{
			this.InitData();
			System.Data.DataTable distributorFeed = FeedGlobals.GetDistributorFeed();
			int count = distributorFeed.Rows.Count;
			for (int i = 0; i < count; i++)
			{
				if (Convert.ToBoolean(distributorFeed.Rows[i]["IsOpenEtao"]) && Convert.ToInt32(distributorFeed.Rows[i]["EtaoStatus"]) == 1)
				{
					this.seller_ID = Convert.ToString(distributorFeed.Rows[i]["EtaoID"]);
					this.webSite = "http://" + Convert.ToString(distributorFeed.Rows[i]["SiteUrl"]);
					this.prefixRootPath = Globals.MapPath("/Storage/Root/" + distributorFeed.Rows[i]["UserId"].ToString() + "/");
					this.storgePath = "/Storage/Root/" + distributorFeed.Rows[i]["UserId"].ToString() + "/";
					this.prefixFullPath = "Item_Full";
					this.prefixIncPath = "Item_Inc";
					System.Data.DataSet eTaoFeedProducts = FeedGlobals.GetETaoFeedProducts(Convert.ToInt32(distributorFeed.Rows[i]["UserId"]));
					this.MakeProductDetail(eTaoFeedProducts);
					this.MakeIncrementIndex(eTaoFeedProducts, this.fullVersion, this.prefixFullPath);
					FeedGlobals.MakeSellerCats(eTaoFeedProducts, this.prefixRootPath, this.seller_ID, this.sellerCatsVersion);
				}
			}
		}
		public void MakeIncrementIndex(System.Data.DataSet dataSet_0, string StrVersion, string StrFileName)
		{
			System.Data.DataTable dataTable = dataSet_0.Tables[0];
			if (dataTable != null && dataTable.Rows.Count > 0 && !(StrFileName.Trim() == ""))
			{
				string str = this.webSite + this.storgePath;
				XmlDocument xmlDocument = new XmlDocument();
				XmlDeclaration newChild = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);
				xmlDocument.AppendChild(newChild);
				XmlElement xmlElement = xmlDocument.CreateElement("", "root", "");
				xmlDocument.AppendChild(xmlElement);
				FeedGlobals.CreateXMlNodeValue(xmlDocument, xmlElement, "version", StrVersion);
				FeedGlobals.CreateXMlNodeValue(xmlDocument, xmlElement, "modified", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
				FeedGlobals.CreateXMlNodeValue(xmlDocument, xmlElement, "seller_id", this.seller_ID);
				FeedGlobals.CreateXMlNodeValue(xmlDocument, xmlElement, "cat_url", str + "SellerCats.xml");
				FeedGlobals.CreateXMlNodeValue(xmlDocument, xmlElement, "dir", str + this.prefixIncPath + "/");
				XmlElement xmlElement2 = xmlDocument.CreateElement("item_ids");
				xmlElement.AppendChild(xmlElement2);
				foreach (System.Data.DataRow dataRow in dataTable.Rows)
				{
					if (dataRow != null && dataRow["productId"].ToString().Trim() != "" && this.productsList.Contains(dataRow["productId"].ToString().Trim()))
					{
						XmlElement xmlElement3 = xmlDocument.CreateElement("outer_id");
						XmlAttribute xmlAttribute = xmlDocument.CreateAttribute("action");
						xmlAttribute.Value = "upload";
						xmlElement3.Attributes.Append(xmlAttribute);
						XmlText newChild2 = xmlDocument.CreateTextNode(dataRow["productId"].ToString().Trim());
						xmlElement3.AppendChild(newChild2);
						xmlElement2.AppendChild(xmlElement3);
					}
				}
				if (File.Exists(this.prefixRootPath + "IncrementIndex.xml"))
				{
					File.Delete(this.prefixRootPath + "IncrementIndex.xml");
				}
				xmlDocument.Save(this.prefixRootPath + "IncrementIndex.xml");
			}
		}
		public void MakeProductDetail(System.Data.DataSet dataSet_0)
		{
			if (dataSet_0 != null && dataSet_0.Tables.Count > 0)
			{
				string text = this.prefixRootPath + this.prefixIncPath + "\\";
				string text2 = this.prefixRootPath + this.prefixIncPath + "\\";
				if (!Directory.Exists(text2))
				{
					Directory.CreateDirectory(text2);
				}
				System.Data.DataTable dataTable = dataSet_0.Tables[0];
				foreach (System.Data.DataRow dataRow in dataTable.Rows)
				{
					try
					{
						XmlDocument xmlDocument = new XmlDocument();
						XmlDeclaration newChild = xmlDocument.CreateXmlDeclaration(this.fullVersion, "utf-8", null);
						xmlDocument.AppendChild(newChild);
						XmlElement xmlElement = xmlDocument.CreateElement("", "item", "");
						xmlDocument.AppendChild(xmlElement);
						FeedGlobals.CreateXMlNodeValue(xmlDocument, xmlElement, "seller_id", this.seller_ID);
						FeedGlobals.CreateXMlNodeValue(xmlDocument, xmlElement, "outer_id", dataRow["productId"].ToString());
						FeedGlobals.CreateXMlNodeValue(xmlDocument, xmlElement, "title", dataRow["ProductName"].ToString());
						FeedGlobals.CreateXMlNodeValue(xmlDocument, xmlElement, "type", "fixed");
						FeedGlobals.CreateXMlNodeValue(xmlDocument, xmlElement, "price", Math.Round(Convert.ToDecimal(dataRow["SalePrice"]), 2).ToString());
						FeedGlobals.CreateXMlNodeValue(xmlDocument, xmlElement, "discount", "");
						FeedGlobals.CreateXMlNodeValue(xmlDocument, xmlElement, "desc", (dataRow["ShortDescription"] == null || dataRow["ShortDescription"] == DBNull.Value) ? "" : dataRow["ShortDescription"].ToString());
						FeedGlobals.CreateXMlNodeValue(xmlDocument, xmlElement, "brand", (dataRow["brandName"] == null || dataRow["brandName"] == DBNull.Value) ? "" : dataRow["brandName"].ToString());
						FeedGlobals.CreateXMlNodeValue(xmlDocument, xmlElement, "tags", (dataRow["Meta_Keywords"] == DBNull.Value || dataRow["Meta_Keywords"] == null) ? "" : dataRow["Meta_Keywords"].ToString());
						FeedGlobals.CreateXMlNodeValue(xmlDocument, xmlElement, "image", (dataRow["ImageUrl1"] == null || dataRow["ImageUrl1"] == DBNull.Value) ? "" : (this.webSite + dataRow["ImageUrl1"].ToString()));
						XmlElement xmlElement2 = xmlDocument.CreateElement("more_images");
						xmlElement.AppendChild(xmlElement2);
						if (!string.IsNullOrEmpty(Convert.ToString(dataRow["ImageUrl2"])))
						{
							XmlElement xmlElement3 = xmlDocument.CreateElement("img");
							XmlText newChild2 = xmlDocument.CreateTextNode(this.webSite + dataRow["ImageUrl2"].ToString());
							xmlElement3.AppendChild(newChild2);
							xmlElement2.AppendChild(xmlElement3);
						}
						if (!string.IsNullOrEmpty(Convert.ToString(dataRow["ImageUrl3"])))
						{
							XmlElement xmlElement3 = xmlDocument.CreateElement("img");
							XmlText newChild2 = xmlDocument.CreateTextNode(this.webSite + dataRow["ImageUrl3"].ToString());
							xmlElement3.AppendChild(newChild2);
							xmlElement2.AppendChild(xmlElement3);
						}
						if (!string.IsNullOrEmpty(Convert.ToString(dataRow["ImageUrl4"])))
						{
							XmlElement xmlElement3 = xmlDocument.CreateElement("img");
							XmlText newChild2 = xmlDocument.CreateTextNode(this.webSite + dataRow["ImageUrl4"].ToString());
							xmlElement3.AppendChild(newChild2);
							xmlElement2.AppendChild(xmlElement3);
						}
						if (!string.IsNullOrEmpty(Convert.ToString(dataRow["ImageUrl5"])))
						{
							XmlElement xmlElement3 = xmlDocument.CreateElement("img");
							XmlText newChild2 = xmlDocument.CreateTextNode(this.webSite + dataRow["ImageUrl5"].ToString());
							xmlElement3.AppendChild(newChild2);
							xmlElement2.AppendChild(xmlElement3);
						}
						FeedGlobals.CreateXMlNodeValue(xmlDocument, xmlElement, "scids", FeedGlobals.GetCategoryIds((string)dataRow["MainCategoryPath"]));
						FeedGlobals.CreateXMlNodeValue(xmlDocument, xmlElement, "post_fee", "0");
						FeedGlobals.CreateXMlNodeValue(xmlDocument, xmlElement, "props", FeedGlobals.GetEtaoSku((int)dataRow["productId"]));
						FeedGlobals.CreateXMlNodeValue(xmlDocument, xmlElement, "showcase", "0");
						FeedGlobals.CreateXMlNodeValue(xmlDocument, xmlElement, "href", this.webSite + Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[]
						{
							dataRow["productId"]
						}));
						text = text2 + dataRow["productId"].ToString().Trim() + ".xml";
						if (File.Exists(text))
						{
							File.Delete(text);
						}
						xmlDocument.Save(text);
						this.productsList.Add(dataRow["productId"].ToString().Trim());
					}
					catch
					{
					}
				}
			}
		}
	}
}
