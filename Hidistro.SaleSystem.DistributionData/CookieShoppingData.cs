using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.SaleSystem.Shopping;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Web;
using System.Xml;
namespace Hidistro.SaleSystem.DistributionData
{
	public class CookieShoppingData : CookieShoppingSubsiteProvider
	{
		private const string CartDataCookieName = "Hid_distro_ShoppingCart_Data_New";
		private Database database;
		public CookieShoppingData()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public override decimal GetCostPrice(string skuId)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_distro_GetGroupBuyProductCostPrices");
			this.database.AddInParameter(storedProcCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
			this.database.AddInParameter(storedProcCommand, "SkuId", System.Data.DbType.String, skuId);
			decimal result = 0m;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
			{
				int value = 100;
				if (dataReader.Read())
				{
					value = dataReader.GetInt32(0);
				}
				if (dataReader.NextResult() && dataReader.Read())
				{
					if (dataReader["DistributorPurchasePrice"] != DBNull.Value)
					{
						result = (decimal)dataReader["DistributorPurchasePrice"];
					}
					else
					{
						result = Math.Round((decimal)dataReader["PurchasePrice"] * (value / 100m), 2);
					}
				}
			}
			return result;
		}
		public override int GetSkuStock(string skuId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Stock FROM Hishop_SKUs WHERE SkuId=@SkuId;");
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, skuId);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int result;
			if (obj != null && obj != DBNull.Value)
			{
				result = (int)obj;
			}
			else
			{
				result = 0;
			}
			return result;
		}
		public override ShoppingCartInfo GetShoppingCart()
		{
			XmlDocument shoppingCartData = this.GetShoppingCartData();
			ShoppingCartInfo shoppingCartInfo = null;
			XmlNodeList xmlNodeList = shoppingCartData.SelectNodes("//sc/lis/l");
			XmlNodeList xmlNodeList2 = shoppingCartData.SelectNodes("//sc/gf/l");
			if ((xmlNodeList != null && xmlNodeList.Count > 0) || (xmlNodeList2 != null && xmlNodeList2.Count > 0))
			{
				shoppingCartInfo = new ShoppingCartInfo();
			}
			if (xmlNodeList != null && xmlNodeList.Count > 0)
			{
				IList<string> list = new List<string>();
				Dictionary<string, int> dictionary = new Dictionary<string, int>();
				foreach (XmlNode xmlNode in xmlNodeList)
				{
					list.Add(xmlNode.Attributes["s"].Value);
					dictionary.Add(xmlNode.Attributes["s"].Value, int.Parse(xmlNode.Attributes["q"].Value));
				}
				this.LoadCartProduct(shoppingCartInfo, dictionary, list);
			}
			if (xmlNodeList2 != null && xmlNodeList2.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
				Dictionary<int, int> dictionary3 = new Dictionary<int, int>();
				foreach (XmlNode xmlNode2 in xmlNodeList2)
				{
					stringBuilder.AppendFormat("{0},", int.Parse(xmlNode2.Attributes["g"].Value));
					dictionary2.Add(int.Parse(xmlNode2.Attributes["g"].Value), int.Parse(xmlNode2.Attributes["g"].Value));
					dictionary3.Add(int.Parse(xmlNode2.Attributes["g"].Value), int.Parse(xmlNode2.Attributes["q"].Value));
				}
				this.LoadCartGift(shoppingCartInfo, dictionary2, dictionary3, stringBuilder.ToString());
			}
			return shoppingCartInfo;
		}
		public override void ClearShoppingCart()
		{
			HiContext.Current.Context.Response.Cookies["Hid_distro_ShoppingCart_Data_New"].Value = null;
			HiContext.Current.Context.Response.Cookies["Hid_distro_ShoppingCart_Data_New"].Expires = new DateTime(1999, 10, 12);
			HiContext.Current.Context.Response.Cookies["Hid_distro_ShoppingCart_Data_New"].Path = "/";
		}
		public override void AddLineItem(string skuId, int quantity)
		{
			XmlDocument shoppingCartData = this.GetShoppingCartData();
			XmlNode xmlNode = shoppingCartData.SelectSingleNode("//sc/lis");
			XmlNode xmlNode2 = xmlNode.SelectSingleNode(string.Concat(new object[]
			{
				"l[@s='",
				skuId,
				"' and @d='",
				HiContext.Current.SiteSettings.UserId.Value,
				"']"
			}));
			if (xmlNode2 == null)
			{
				xmlNode2 = CookieShoppingData.CreateLineItemNode(shoppingCartData, skuId, quantity);
				xmlNode.InsertBefore(xmlNode2, xmlNode.FirstChild);
			}
			else
			{
				xmlNode2.Attributes["q"].Value = (int.Parse(xmlNode2.Attributes["q"].Value) + quantity).ToString(CultureInfo.InvariantCulture);
			}
			this.SaveShoppingCartData(shoppingCartData);
		}
		public override void RemoveLineItem(string skuId)
		{
			XmlDocument shoppingCartData = this.GetShoppingCartData();
			XmlNode xmlNode = shoppingCartData.SelectSingleNode("//sc/lis");
			XmlNode xmlNode2 = xmlNode.SelectSingleNode(string.Concat(new object[]
			{
				"l[@s='",
				skuId,
				"' and @d='",
				HiContext.Current.SiteSettings.UserId.Value,
				"']"
			}));
			if (xmlNode2 != null)
			{
				xmlNode.RemoveChild(xmlNode2);
				this.SaveShoppingCartData(shoppingCartData);
			}
		}
		public override void UpdateLineItemQuantity(string skuId, int quantity)
		{
			if (quantity <= 0)
			{
				this.RemoveLineItem(skuId);
			}
			else
			{
				XmlDocument shoppingCartData = this.GetShoppingCartData();
				XmlNode xmlNode = shoppingCartData.SelectSingleNode("//lis");
				XmlNode xmlNode2 = xmlNode.SelectSingleNode(string.Concat(new object[]
				{
					"l[@s='",
					skuId,
					"' and @d='",
					HiContext.Current.SiteSettings.UserId.Value,
					"']"
				}));
				if (xmlNode2 != null)
				{
					xmlNode2.Attributes["q"].Value = quantity.ToString(CultureInfo.InvariantCulture);
					this.SaveShoppingCartData(shoppingCartData);
				}
			}
		}
		public override bool AddGiftItem(int giftId, int quantity)
		{
			XmlDocument shoppingCartData = this.GetShoppingCartData();
			XmlNode xmlNode = shoppingCartData.SelectSingleNode("//sc/gf");
			XmlNode xmlNode2 = xmlNode.SelectSingleNode(string.Concat(new object[]
			{
				"l[@g=",
				giftId,
				" and @d='",
				HiContext.Current.SiteSettings.UserId.Value,
				"']"
			}));
			if (xmlNode2 == null)
			{
				xmlNode2 = CookieShoppingData.CreateGiftLineItemNode(shoppingCartData, giftId, quantity);
				xmlNode.InsertBefore(xmlNode2, xmlNode.FirstChild);
			}
			else
			{
				xmlNode2.Attributes["q"].Value = (int.Parse(xmlNode2.Attributes["q"].Value) + quantity).ToString(CultureInfo.InvariantCulture);
			}
			this.SaveShoppingCartData(shoppingCartData);
			return true;
		}
		public override void UpdateGiftItemQuantity(int giftId, int quantity)
		{
			if (quantity <= 0)
			{
				this.RemoveGiftItem(giftId);
			}
			else
			{
				XmlDocument shoppingCartData = this.GetShoppingCartData();
				XmlNode xmlNode = shoppingCartData.SelectSingleNode("//sc/gf");
				XmlNode xmlNode2 = xmlNode.SelectSingleNode(string.Concat(new object[]
				{
					"l[@g='",
					giftId,
					"' and @d='",
					HiContext.Current.SiteSettings.UserId.Value,
					"']"
				}));
				if (xmlNode2 != null)
				{
					xmlNode2.Attributes["q"].Value = quantity.ToString(CultureInfo.InvariantCulture);
					this.SaveShoppingCartData(shoppingCartData);
				}
			}
		}
		public override void RemoveGiftItem(int giftId)
		{
			XmlDocument shoppingCartData = this.GetShoppingCartData();
			XmlNode xmlNode = shoppingCartData.SelectSingleNode("//sc/gf");
			XmlNode xmlNode2 = xmlNode.SelectSingleNode(string.Concat(new object[]
			{
				"l[@g='",
				giftId,
				"' and @d='",
				HiContext.Current.SiteSettings.UserId.Value,
				"']"
			}));
			if (xmlNode2 != null)
			{
				xmlNode.RemoveChild(xmlNode2);
				this.SaveShoppingCartData(shoppingCartData);
			}
		}
		private ShoppingCartItemInfo GetCartItemInfo(string skuId, int quantity)
		{
			ShoppingCartItemInfo shoppingCartItemInfo = null;
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_distro_ShoppingCart_GetItemInfo");
			this.database.AddInParameter(storedProcCommand, "Quantity", System.Data.DbType.Int32, quantity);
			this.database.AddInParameter(storedProcCommand, "UserId", System.Data.DbType.Int32, 0);
			this.database.AddInParameter(storedProcCommand, "SkuId", System.Data.DbType.String, skuId);
			this.database.AddInParameter(storedProcCommand, "GradeId", System.Data.DbType.Int32, 0);
			this.database.AddInParameter(storedProcCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
			{
				if (dataReader.Read())
				{
					shoppingCartItemInfo = new ShoppingCartItemInfo();
					shoppingCartItemInfo.SkuId = skuId;
					shoppingCartItemInfo.ProductId = (int)dataReader["ProductId"];
					shoppingCartItemInfo.Name = dataReader["ProductName"].ToString();
					if (DBNull.Value != dataReader["Weight"])
					{
						shoppingCartItemInfo.Weight = (int)dataReader["Weight"];
					}
					if (DBNull.Value != dataReader["SalePrice"])
					{
						shoppingCartItemInfo.MemberPrice = (shoppingCartItemInfo.AdjustedPrice = (decimal)dataReader["SalePrice"]);
					}
					if (DBNull.Value != dataReader["ThumbnailUrl40"])
					{
						shoppingCartItemInfo.ThumbnailUrl40 = dataReader["ThumbnailUrl40"].ToString();
					}
					if (DBNull.Value != dataReader["ThumbnailUrl60"])
					{
						shoppingCartItemInfo.ThumbnailUrl60 = dataReader["ThumbnailUrl60"].ToString();
					}
					if (DBNull.Value != dataReader["ThumbnailUrl100"])
					{
						shoppingCartItemInfo.ThumbnailUrl100 = dataReader["ThumbnailUrl100"].ToString();
					}
					if (dataReader["SKU"] != DBNull.Value)
					{
						shoppingCartItemInfo.SKU = (string)dataReader["SKU"];
					}
					shoppingCartItemInfo.Quantity = (shoppingCartItemInfo.ShippQuantity = (int)dataReader["TotalQuantity"]);
					string text = string.Empty;
					if (dataReader.NextResult())
					{
						while (dataReader.Read())
						{
							if (dataReader["AttributeName"] != DBNull.Value && !string.IsNullOrEmpty((string)dataReader["AttributeName"]) && dataReader["ValueStr"] != DBNull.Value && !string.IsNullOrEmpty((string)dataReader["ValueStr"]))
							{
								object obj = text;
								text = string.Concat(new object[]
								{
									obj,
									dataReader["AttributeName"],
									"：",
									dataReader["ValueStr"],
									"; "
								});
							}
						}
					}
					shoppingCartItemInfo.SkuContent = text;
				}
			}
			return shoppingCartItemInfo;
		}
		public override bool GetShoppingProductInfo(int productId, string skuId, out ProductSaleStatus saleStatus, out int stock, out int totalQuantity)
		{
			saleStatus = ProductSaleStatus.Delete;
			stock = 0;
			totalQuantity = 0;
			bool result = false;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Stock，SaleStatus,AlertStock FROM Hishop_Skus s INNER JOIN Hishop_Products p ON s.ProductId=p.ProductId WHERE s.ProductId=@ProductId AND s.SkuId=@SkuId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, skuId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					saleStatus = (ProductSaleStatus)((int)dataReader["SaleStatus"]);
					stock = (int)dataReader["Stock"];
					int num = (int)dataReader["AlertStock"];
					if (stock <= num)
					{
						saleStatus = ProductSaleStatus.UnSale;
					}
					result = true;
				}
				totalQuantity = this.GetShoppingProductQuantity(skuId, productId);
			}
			return result;
		}
		public override Dictionary<string, decimal> GetCostPriceForItems(int userId)
		{
			Dictionary<string, decimal> dictionary = new Dictionary<string, decimal>();
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_distro_ShoppingCart_GetCostPrices");
			this.database.AddInParameter(storedProcCommand, "UserId", System.Data.DbType.Int32, userId);
			this.database.AddInParameter(storedProcCommand, "DistributorUserId", System.Data.DbType.Int32, HiContext.Current.SiteSettings.UserId.Value);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
			{
				while (dataReader.Read())
				{
					dictionary.Add((string)dataReader["SkuId"], (decimal)dataReader["PurchasePrice"]);
				}
			}
			return dictionary;
		}
		private void LoadCartGift(ShoppingCartInfo cartInfo, Dictionary<int, int> giftIdList, Dictionary<int, int> giftQuantityList, string giftIds)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT g.*,hg.Unit,hg.LongDescription,hg.CostPrice,hg.ImageUrl,hg.ThumbnailUrl40,hg.ThumbnailUrl60,hg.ThumbnailUrl100,hg.PurchasePrice,hg.MarketPrice,hg.IsDownLoad FROM distro_Gifts g ON gc.GiftId = g.GiftId join Hishop_Gifts hg on hg.GiftId=g.GiftId WHERE g.GiftId IN ({0}) g.DistributorUserId={1}", giftIds.TrimEnd(new char[]
			{
				','
			}), HiContext.Current.SiteSettings.UserId.Value));
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					ShoppingCartGiftInfo shoppingCartGiftInfo = DataMapper.PopulateGiftCartItem(dataReader);
					shoppingCartGiftInfo.Quantity = giftQuantityList[shoppingCartGiftInfo.GiftId];
					cartInfo.LineGifts.Add(shoppingCartGiftInfo);
				}
			}
		}
		private void LoadCartProduct(ShoppingCartInfo cartInfo, Dictionary<string, int> productQuantityList, IList<string> skuIds)
		{
			foreach (string current in skuIds)
			{
				ShoppingCartItemInfo cartItemInfo = this.GetCartItemInfo(current, productQuantityList[current]);
				if (cartItemInfo != null)
				{
					cartInfo.LineItems.Add(current, cartItemInfo);
				}
			}
		}
		private int GetShoppingProductQuantity(string skuId, int ProductId)
		{
			int result = 0;
			XmlDocument shoppingCartData = this.GetShoppingCartData();
			XmlNode xmlNode = shoppingCartData.SelectSingleNode(string.Concat(new object[]
			{
				"//sc/lis/l[SkuId='",
				skuId,
				"' AND p=",
				ProductId,
				" AND d=",
				HiContext.Current.SiteSettings.UserId.Value,
				"]"
			}));
			if (xmlNode != null)
			{
				int.TryParse(xmlNode.Attributes["q"].Value, out result);
			}
			return result;
		}
		private XmlDocument GetShoppingCartData()
		{
			XmlDocument xmlDocument = new XmlDocument();
			HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["Hid_distro_ShoppingCart_Data_New"];
			if (httpCookie == null || string.IsNullOrEmpty(httpCookie.Value))
			{
				xmlDocument = CookieShoppingData.CreateEmptySchema();
			}
			else
			{
				try
				{
					xmlDocument.LoadXml(Globals.UrlDecode(httpCookie.Value));
				}
				catch
				{
					this.ClearShoppingCart();
					xmlDocument = CookieShoppingData.CreateEmptySchema();
				}
			}
			return xmlDocument;
		}
		private void SaveShoppingCartData(XmlDocument xmlDocument_0)
		{
			if (xmlDocument_0 == null)
			{
				this.ClearShoppingCart();
			}
			else
			{
				HttpCookie httpCookie = HiContext.Current.Context.Request.Cookies["Hid_distro_ShoppingCart_Data_New"];
				if (httpCookie == null)
				{
					httpCookie = new HttpCookie("Hid_distro_ShoppingCart_Data_New");
				}
				httpCookie.Value = Globals.UrlEncode(xmlDocument_0.OuterXml);
				httpCookie.Expires = DateTime.Now.AddDays(3.0);
				HiContext.Current.Context.Response.Cookies.Add(httpCookie);
			}
		}
		private static XmlDocument CreateEmptySchema()
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml("<sc><lis></lis><gf></gf></sc>");
			return xmlDocument;
		}
		private static XmlNode CreateLineItemNode(XmlDocument xmlDocument_0, string skuId, int quantity)
		{
			XmlNode xmlNode = xmlDocument_0.CreateElement("l");
			xmlDocument_0.SelectSingleNode("//lis");
			XmlAttribute xmlAttribute = xmlDocument_0.CreateAttribute("d");
			xmlAttribute.Value = HiContext.Current.SiteSettings.UserId.Value.ToString(CultureInfo.InvariantCulture);
			xmlNode.Attributes.Append(xmlAttribute);
			XmlAttribute xmlAttribute2 = xmlDocument_0.CreateAttribute("s");
			xmlAttribute2.Value = skuId;
			xmlNode.Attributes.Append(xmlAttribute2);
			XmlAttribute xmlAttribute3 = xmlDocument_0.CreateAttribute("q");
			xmlAttribute3.Value = quantity.ToString(CultureInfo.InvariantCulture);
			xmlNode.Attributes.Append(xmlAttribute3);
			return xmlNode;
		}
		private static XmlNode CreateGiftLineItemNode(XmlDocument xmlDocument_0, int giftId, int quantity)
		{
			XmlNode xmlNode = xmlDocument_0.CreateElement("l");
			xmlDocument_0.SelectSingleNode("//gf");
			XmlAttribute xmlAttribute = xmlDocument_0.CreateAttribute("d");
			xmlAttribute.Value = HiContext.Current.SiteSettings.UserId.Value.ToString(CultureInfo.InvariantCulture);
			xmlNode.Attributes.Append(xmlAttribute);
			XmlAttribute xmlAttribute2 = xmlDocument_0.CreateAttribute("q");
			xmlAttribute2.Value = quantity.ToString(CultureInfo.InvariantCulture);
			xmlNode.Attributes.Append(xmlAttribute2);
			XmlAttribute xmlAttribute3 = xmlDocument_0.CreateAttribute("g");
			xmlAttribute3.Value = giftId.ToString();
			xmlNode.Attributes.Append(xmlAttribute3);
			return xmlNode;
		}
		private static int GenerateLastItemId(XmlDocument xmlDocument_0)
		{
			XmlNode xmlNode = xmlDocument_0.SelectSingleNode("/sc");
			XmlAttribute xmlAttribute = xmlNode.Attributes["lid"];
			int result;
			if (xmlAttribute == null)
			{
				xmlAttribute = xmlDocument_0.CreateAttribute("lid");
				xmlNode.Attributes.Append(xmlAttribute);
				result = 1;
			}
			else
			{
				result = int.Parse(xmlAttribute.Value) + 1;
			}
			xmlAttribute.Value = result.ToString(CultureInfo.InvariantCulture);
			return result;
		}
	}
}
