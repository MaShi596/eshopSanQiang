using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text;
using System.Xml;
namespace Hidistro.Jobs
{
	public class FeedGlobals
	{
		public static void MakeSellerCats(System.Data.DataSet dataSet_0, string prefixRootPath, string seller_ID, string sellerCatsVersion)
		{
			System.Data.DataTable dataTable = dataSet_0.Tables[1];
			if (dataTable != null && dataTable.Rows.Count > 0 && !(prefixRootPath.Trim() == ""))
			{
				XmlDocument xmlDocument = new XmlDocument();
				XmlDeclaration newChild = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);
				xmlDocument.AppendChild(newChild);
				XmlElement xmlElement = xmlDocument.CreateElement("", "root", "");
				xmlDocument.AppendChild(xmlElement);
				FeedGlobals.CreateXMlNodeValue(xmlDocument, xmlElement, "version", sellerCatsVersion);
				FeedGlobals.CreateXMlNodeValue(xmlDocument, xmlElement, "modified", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
				FeedGlobals.CreateXMlNodeValue(xmlDocument, xmlElement, "seller_id", seller_ID);
				XmlElement xmlElement2 = FeedGlobals.CreateXMlNode(xmlDocument, xmlElement, "seller_cats");
				System.Data.DataRow[] array = dataTable.Select("depth=1");
				System.Data.DataRow[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					System.Data.DataRow dataRow = array2[i];
					XmlElement xmlElement3 = xmlDocument.CreateElement("cat");
					xmlElement2.AppendChild(xmlElement3);
					FeedGlobals.CreateXMlNodeValue(xmlDocument, xmlElement3, "scid", dataRow["CategoryId"].ToString());
					FeedGlobals.CreateXMlNodeValue(xmlDocument, xmlElement3, "name", dataRow["Name"].ToString());
					System.Data.DataRow[] array3 = dataTable.Select("ParentCategoryId=" + dataRow["categoryId"]);
					if (array3 != null && array3.Length > 0)
					{
						XmlElement xmlElement4 = FeedGlobals.CreateXMlNode(xmlDocument, xmlElement3, "cats");
						System.Data.DataRow[] array4 = array3;
						for (int j = 0; j < array4.Length; j++)
						{
							System.Data.DataRow dataRow2 = array4[j];
							XmlElement xmlElement5 = xmlDocument.CreateElement("cat");
							xmlElement4.AppendChild(xmlElement5);
							FeedGlobals.CreateXMlNodeValue(xmlDocument, xmlElement5, "scid", dataRow2["CategoryId"].ToString());
							FeedGlobals.CreateXMlNodeValue(xmlDocument, xmlElement5, "name", dataRow2["Name"].ToString());
						}
					}
				}
				if (File.Exists(prefixRootPath + "SellerCats.xml"))
				{
					File.Delete(prefixRootPath + "SellerCats.xml");
				}
				xmlDocument.Save(prefixRootPath + "SellerCats.xml");
			}
		}
		internal static string GetCategoryIds(string MainCategoryPath)
		{
			string[] array = MainCategoryPath.Split(new char[]
			{
				'|'
			});
			string result;
			if (array.Length == 1)
			{
				result = array[0];
			}
			else
			{
				if (array[1] != "")
				{
					result = array[0] + "," + array[1];
				}
				else
				{
					result = array[0];
				}
			}
			return result;
		}
		internal static System.Data.DataSet GetETaoFeedProducts(int DistributorUserId)
		{
			Database database = DatabaseFactory.CreateDatabase();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("select top 500 p.DistributorUserId,p2.*,brandname,  (SELECT     MIN(SalePrice) AS Expr1 FROM    dbo.Hishop_SKUs WHERE    (ProductId = p.ProductId)) AS SalePrice from distro_Products p inner join Hishop_Products p2 on p.ProductId=p2.ProductId   left join Hishop_BrandCategories  a on  p.brandId=a.brandId where p.DistributorUserId=" + DistributorUserId + " and p.salestatus=1 and p.categoryId!=0; ");
			stringBuilder.Append("select * from Hishop_Categories;");
			System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand(stringBuilder.ToString());
			return database.ExecuteDataSet(sqlStringCommand);
		}
		internal static System.Data.DataTable GetDistributorFeed()
		{
			Database database = DatabaseFactory.CreateDatabase();
			string text = "SELECT [UserId],[SiteUrl] ,[IsOpenEtao],[EtaoID] ,[EtaoStatus] FROM [distro_Settings]";
			System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand(text);
			return database.ExecuteDataSet(sqlStringCommand).Tables[0];
		}
		internal static string GetEtaoSku(int productid)
		{
			Database database = DatabaseFactory.CreateDatabase();
			StringBuilder stringBuilder = new StringBuilder();
			List<Attribute> list = new List<Attribute>();
			List<string> list2 = new List<string>();
			stringBuilder.AppendFormat("select AttributeName,valuestr from (SELECT   AttributeName, ValueStr FROM Hishop_SKUItems s join Hishop_Attributes a on s.AttributeId = a.AttributeId join Hishop_AttributeValues av on s.ValueId = av.ValueId WHERE SkuId IN (SELECT SkuId FROM Hishop_SKUs WHERE ProductId = {0})) s   group by  AttributeName,valuestr", productid);
			System.Data.Common.DbCommand sqlStringCommand = database.GetSqlStringCommand(stringBuilder.ToString());
			using (System.Data.IDataReader dataReader = database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(new Attribute
					{
						AttrName = dataReader["AttributeName"].ToString(),
						AttrValue = dataReader["valuestr"].ToString()
					});
					if (!list2.Contains(dataReader["AttributeName"].ToString()))
					{
						list2.Add(dataReader["AttributeName"].ToString());
					}
				}
			}
			string text = "";
			foreach (string current in list2)
			{
				string text2 = "";
				foreach (Attribute current2 in list)
				{
					if (current == current2.AttrName)
					{
						text2 = text2 + current2.AttrValue + ",";
					}
				}
				string text3 = text;
				text = string.Concat(new string[]
				{
					text3,
					current,
					":",
					text2.Substring(0, text2.Length - 1),
					";"
				});
			}
			return text;
		}
		internal static XmlElement CreateXMlNode(XmlDocument xmlDocument_0, XmlElement rootContent, string nodeName)
		{
			XmlElement xmlElement = xmlDocument_0.CreateElement(nodeName);
			rootContent.AppendChild(xmlElement);
			return xmlElement;
		}
		internal static void CreateXMlNodeValue(XmlDocument xmlDocument_0, XmlElement rootContent, string nodeName, string nodeValue)
		{
			XmlElement xmlElement = xmlDocument_0.CreateElement(nodeName);
			XmlText newChild = xmlDocument_0.CreateTextNode(nodeValue);
			xmlElement.AppendChild(newChild);
			rootContent.AppendChild(xmlElement);
		}
		internal static void CreateXMlNodeAttr(XmlDocument xmlDocument_0, XmlElement rootContent, string nodeName, string attrName, string attrValue, string nodeValue)
		{
			XmlElement xmlElement = xmlDocument_0.CreateElement(nodeName);
			XmlAttribute xmlAttribute = xmlDocument_0.CreateAttribute(attrName);
			xmlAttribute.Value = attrValue;
			xmlElement.Attributes.Append(xmlAttribute);
			XmlText newChild = xmlDocument_0.CreateTextNode(nodeValue);
			xmlElement.AppendChild(newChild);
			rootContent.AppendChild(xmlElement);
		}
	}
}
