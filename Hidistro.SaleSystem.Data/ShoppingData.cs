using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core.Enums;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Shopping;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
namespace Hidistro.SaleSystem.Data
{
	public class ShoppingData : ShoppingMasterProvider
	{
		private Database database;
		public ShoppingData()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		public override IList<string> GetSkuIdsBysku(string string_0)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT SkuId FROM Hishop_SKUs WHERE SKU = @SKU");
			this.database.AddInParameter(sqlStringCommand, "SKU", System.Data.DbType.String, string_0);
			IList<string> list = new List<string>();
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add((string)dataReader["SkuId"]);
				}
			}
			return list;
		}
		public override System.Data.DataTable GetProductInfoBySku(string skuId)
		{
			System.Data.DataTable result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" SELECT s.SkuId, s.SKU, s.ProductId, s.Stock, AttributeName, ValueStr FROM Hishop_SKUs s left join Hishop_SKUItems si on s.SkuId = si.SkuId left join Hishop_Attributes a on si.AttributeId = a.AttributeId left join Hishop_AttributeValues av on si.ValueId = av.ValueId WHERE s.SkuId = @SkuId AND s.ProductId IN (SELECT ProductId FROM Hishop_Products WHERE SaleStatus=1)");
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, skuId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override SKUItem GetProductAndSku(int productId, string options)
		{
			SKUItem result;
			if (string.IsNullOrEmpty(options))
			{
				result = null;
			}
			else
			{
				string[] array = options.Split(new char[]
				{
					','
				});
				if (array == null || array.Length <= 0)
				{
					result = null;
				}
				else
				{
					StringBuilder stringBuilder = new StringBuilder();
					if (HiContext.Current.User.UserRole == UserRole.Member)
					{
                        Hidistro.Membership.Context.Member member = HiContext.Current.User as Hidistro.Membership.Context.Member;
						int memberDiscount = MemberProvider.Instance().GetMemberDiscount(member.GradeId);
						stringBuilder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock, AlertStock, CostPrice, PurchasePrice,");
						stringBuilder.AppendFormat(" (CASE WHEN (SELECT COUNT(*) FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) = 1", member.GradeId);
						stringBuilder.AppendFormat(" THEN (SELECT MemberSalePrice FROM Hishop_SKUMemberPrice WHERE SkuId = s.SkuId AND GradeId = {0}) ELSE SalePrice*{1}/100 END) AS SalePrice", member.GradeId, memberDiscount);
						stringBuilder.Append(" FROM Hishop_SKUs s WHERE ProductId = @ProductId");
					}
					else
					{
						stringBuilder.Append("SELECT SkuId, ProductId, SKU,Weight, Stock, AlertStock, CostPrice, PurchasePrice, SalePrice FROM Hishop_SKUs WHERE ProductId = @ProductId");
					}
					string[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						string text = array2[i];
						string[] array3 = text.Split(new char[]
						{
							':'
						});
						stringBuilder.AppendFormat(" AND SkuId IN (SELECT SkuId FROM Hishop_SKUItems WHERE AttributeId = {0} AND ValueId = {1}) ", array3[0], array3[1]);
					}
					SKUItem sKUItem = null;
					System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
					this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
					using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
					{
						if (dataReader.Read())
						{
							sKUItem = DataMapper.PopulateSKU(dataReader);
						}
					}
					result = sKUItem;
				}
			}
			return result;
		}
		public override int GetStock(int productId, string skuId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT Stock FROM Hishop_SKUs WHERE ProductId = @ProductId AND SkuId= @SkuId");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, skuId);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			int result;
			if (obj == null || obj == DBNull.Value)
			{
				result = 0;
			}
			else
			{
				result = (int)obj;
			}
			return result;
		}
		public override System.Data.DataTable GetUnUpUnUpsellingSkus(int productId, int attributeId, int valueId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_SKUItems WHERE SKUId IN (SELECT SKUId FROM Hishop_SKUs WHERE ProductId = @ProductId) AND (SKUId in (SELECT SKUId FROM Hishop_SKUItems WHERE AttributeId = @AttributeId AND ValueId=@ValueId) OR AttributeId = @AttributeId)");
			this.database.AddInParameter(sqlStringCommand, "ProductId", System.Data.DbType.Int32, productId);
			this.database.AddInParameter(sqlStringCommand, "AttributeId", System.Data.DbType.Int32, attributeId);
			this.database.AddInParameter(sqlStringCommand, "ValueId", System.Data.DbType.Int32, valueId);
			System.Data.DataTable result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override decimal GetCostPrice(string skuId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT CostPrice FROM Hishop_SKUs WHERE SkuId=@SkuId");
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, skuId);
			object obj = this.database.ExecuteScalar(sqlStringCommand);
			decimal result;
			if (obj != null && obj != DBNull.Value)
			{
				result = (decimal)obj;
			}
			else
			{
				result = 0m;
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
        public override void AddLineItem(Hidistro.Membership.Context.Member member, string skuId, int quantity)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_ShoppingCart_AddLineItem");
			this.database.AddInParameter(storedProcCommand, "UserId", System.Data.DbType.Int32, member.UserId);
			this.database.AddInParameter(storedProcCommand, "SkuId", System.Data.DbType.String, skuId);
			this.database.AddInParameter(storedProcCommand, "Quantity", System.Data.DbType.Int32, quantity);
			this.database.ExecuteNonQuery(storedProcCommand);
		}
		public override void ClearShoppingCart(int userId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ShoppingCarts WHERE UserId = @UserId; DELETE FROM Hishop_GiftShoppingCarts WHERE UserId = @UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override ShoppingCartInfo GetShoppingCart(int userId)
		{
			ShoppingCartInfo shoppingCartInfo = new ShoppingCartInfo();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_ShoppingCarts WHERE UserId = @UserId;SELECT * FROM Hishop_GiftShoppingCarts gc JOIN Hishop_Gifts g ON gc.GiftId = g.GiftId WHERE gc.UserId = @UserId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
                Hidistro.Membership.Context.Member member = HiContext.Current.User as Hidistro.Membership.Context.Member;
				while (dataReader.Read())
				{
					ShoppingCartItemInfo cartItemInfo = this.GetCartItemInfo(member, (string)dataReader["SkuId"], (int)dataReader["Quantity"]);
					if (cartItemInfo != null)
					{
						shoppingCartInfo.LineItems.Add((string)dataReader["SkuId"], cartItemInfo);
					}
				}
				dataReader.NextResult();
				while (dataReader.Read())
				{
					ShoppingCartGiftInfo shoppingCartGiftInfo = DataMapper.PopulateGiftCartItem(dataReader);
					shoppingCartGiftInfo.Quantity = (int)dataReader["Quantity"];
					shoppingCartInfo.LineGifts.Add(shoppingCartGiftInfo);
				}
			}
			return shoppingCartInfo;
		}
        public override PromotionInfo GetReducedPromotion(Hidistro.Membership.Context.Member member, decimal amount, int quantity, out decimal reducedAmount)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Promotions WHERE DateDiff(DD, StartDate, getdate()) >= 0 AND DateDiff(DD, EndDate, getdate()) <= 0 AND PromoteType BETWEEN 11 AND 14 AND ActivityId IN (SELECT ActivityId FROM Hishop_PromotionMemberGrades WHERE GradeId = @GradeId)");
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, member.GradeId);
			IList<PromotionInfo> list = new List<PromotionInfo>();
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulatePromote(dataReader));
				}
			}
			PromotionInfo result = null;
			reducedAmount = 0m;
			foreach (PromotionInfo current in list)
			{
				switch (current.PromoteType)
				{
				case PromoteType.FullAmountDiscount:
					if (amount >= current.Condition && amount - amount * current.DiscountValue > reducedAmount)
					{
						reducedAmount = amount - amount * current.DiscountValue;
						result = current;
					}
					break;
				case PromoteType.FullAmountReduced:
					if (amount >= current.Condition && current.DiscountValue > reducedAmount)
					{
						reducedAmount = current.DiscountValue;
						result = current;
					}
					break;
				case PromoteType.FullQuantityDiscount:
					if (quantity >= (int)current.Condition && amount - amount * current.DiscountValue > reducedAmount)
					{
						reducedAmount = amount - amount * current.DiscountValue;
						result = current;
					}
					break;
				case PromoteType.FullQuantityReduced:
					if (quantity >= (int)current.Condition && current.DiscountValue > reducedAmount)
					{
						reducedAmount = current.DiscountValue;
						result = current;
					}
					break;
				}
			}
			return result;
		}
        public override PromotionInfo GetSendPromotion(Hidistro.Membership.Context.Member member, decimal amount, PromoteType promoteType)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_Promotions WHERE DateDiff(DD, StartDate, getdate()) >= 0 AND DateDiff(DD, EndDate, getdate()) <= 0 AND PromoteType = @PromoteType AND Condition <= @Condition AND ActivityId IN (SELECT ActivityId FROM Hishop_PromotionMemberGrades WHERE GradeId = @GradeId) ORDER BY DiscountValue DESC");
			this.database.AddInParameter(sqlStringCommand, "PromoteType", System.Data.DbType.Int32, (int)promoteType);
			this.database.AddInParameter(sqlStringCommand, "Condition", System.Data.DbType.Currency, amount);
			this.database.AddInParameter(sqlStringCommand, "GradeId", System.Data.DbType.Int32, member.GradeId);
			PromotionInfo result = null;
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulatePromote(dataReader);
				}
			}
			return result;
		}
		public override void RemoveLineItem(int userId, string skuId)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_ShoppingCarts WHERE UserId = @UserId AND SkuId = @SkuId");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, userId);
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, skuId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
        public override void UpdateLineItemQuantity(Hidistro.Membership.Context.Member member, string skuId, int quantity)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_ShoppingCarts SET Quantity = @Quantity WHERE UserId = @UserId AND SkuId = @SkuId");
			this.database.AddInParameter(sqlStringCommand, "Quantity", System.Data.DbType.Int32, quantity);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, member.UserId);
			this.database.AddInParameter(sqlStringCommand, "SkuId", System.Data.DbType.String, skuId);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override bool AddGiftItem(int giftId, int quantity, PromoteType promotype)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("IF  EXISTS(SELECT GiftId FROM Hishop_GiftShoppingCarts WHERE UserId = @UserId AND GiftId = @GiftId AND PromoType=@PromoType) UPDATE Hishop_GiftShoppingCarts SET Quantity = Quantity + @Quantity WHERE UserId = @UserId AND GiftId = @GiftId AND PromoType=@PromoType; ELSE INSERT INTO Hishop_GiftShoppingCarts(UserId, GiftId, Quantity, AddTime,PromoType) VALUES (@UserId, @GiftId, @Quantity, @AddTime,@PromoType)");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "GiftId", System.Data.DbType.Int32, giftId);
			this.database.AddInParameter(sqlStringCommand, "Quantity", System.Data.DbType.Int32, quantity);
			this.database.AddInParameter(sqlStringCommand, "AddTime", System.Data.DbType.DateTime, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "PromoType", System.Data.DbType.Int32, (int)promotype);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override void UpdateGiftItemQuantity(int giftId, int quantity, PromoteType promotype)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("UPDATE Hishop_GiftShoppingCarts SET Quantity = @Quantity WHERE UserId = @UserId AND GiftId = @GiftId AND PromoType=@PromoType");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "GiftId", System.Data.DbType.Int32, giftId);
			this.database.AddInParameter(sqlStringCommand, "Quantity", System.Data.DbType.Int32, quantity);
			this.database.AddInParameter(sqlStringCommand, "PromoType", System.Data.DbType.Int32, (int)promotype);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override int GetGiftItemQuantity(PromoteType promotype)
		{
			int result = 0;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ISNULL(SUM(Quantity),0) as Quantity FROM Hishop_GiftShoppingCarts WHERE UserId = @UserId AND PromoType=@PromoType");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "PromoType", System.Data.DbType.Int32, (int)promotype);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = int.Parse(dataReader["Quantity"].ToString());
				}
			}
			return result;
		}
		public override void RemoveGiftItem(int giftId, PromoteType promotype)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("DELETE FROM Hishop_GiftShoppingCarts WHERE UserId = @UserId AND GiftId = @GiftId AND PromoType=@PromoType");
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			this.database.AddInParameter(sqlStringCommand, "GiftId", System.Data.DbType.Int32, giftId);
			this.database.AddInParameter(sqlStringCommand, "PromoType", System.Data.DbType.Int32, (int)promotype);
			this.database.ExecuteNonQuery(sqlStringCommand);
		}
		public override int CountDownOrderCount(int productid)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("select   isnull(sum(quantity),0) as Quanity from dbo.Hishop_OrderItems where productid=@productid and  orderid in(select orderid from dbo.Hishop_Orders where userid=@userid and orderstatus!=4 AND ISNULL(CountDownBuyId, 0) > 0)");
			this.database.AddInParameter(sqlStringCommand, "productid", System.Data.DbType.Int32, productid);
			this.database.AddInParameter(sqlStringCommand, "userid", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			return (int)this.database.ExecuteScalar(sqlStringCommand);
		}
        public override ShoppingCartItemInfo GetCartItemInfo(Hidistro.Membership.Context.Member member, string skuId, int quantity)
		{
			ShoppingCartItemInfo shoppingCartItemInfo = null;
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_ShoppingCart_GetItemInfo");
			this.database.AddInParameter(storedProcCommand, "Quantity", System.Data.DbType.Int32, quantity);
			this.database.AddInParameter(storedProcCommand, "UserId", System.Data.DbType.Int32, (member != null) ? member.UserId : 0);
			this.database.AddInParameter(storedProcCommand, "SkuId", System.Data.DbType.String, skuId);
			this.database.AddInParameter(storedProcCommand, "GradeId", System.Data.DbType.Int32, (member != null) ? member.GradeId : 0);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(storedProcCommand))
			{
				if (dataReader.Read())
				{
					shoppingCartItemInfo = new ShoppingCartItemInfo();
					shoppingCartItemInfo.SkuId = skuId;
					ShoppingCartItemInfo arg_BD_0 = shoppingCartItemInfo;
					shoppingCartItemInfo.ShippQuantity = quantity;
					arg_BD_0.Quantity = quantity;
					shoppingCartItemInfo.ProductId = (int)dataReader["ProductId"];
					if (dataReader["SKU"] != DBNull.Value)
					{
						shoppingCartItemInfo.SKU = (string)dataReader["SKU"];
					}
					shoppingCartItemInfo.Name = (string)dataReader["ProductName"];
					if (DBNull.Value != dataReader["Weight"])
					{
						shoppingCartItemInfo.Weight = (int)dataReader["Weight"];
					}
					shoppingCartItemInfo.MemberPrice = (shoppingCartItemInfo.AdjustedPrice = (decimal)dataReader["SalePrice"]);
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
					PromotionInfo promotionInfo = null;
					if (dataReader.NextResult() && dataReader.Read())
					{
						promotionInfo = DataMapper.PopulatePromote(dataReader);
					}
					if (promotionInfo != null)
					{
						switch (promotionInfo.PromoteType)
						{
						case PromoteType.Discount:
							shoppingCartItemInfo.PromotionId = promotionInfo.ActivityId;
							shoppingCartItemInfo.PromotionName = promotionInfo.Name;
							shoppingCartItemInfo.AdjustedPrice = shoppingCartItemInfo.MemberPrice * promotionInfo.DiscountValue;
							break;
						case PromoteType.Amount:
							shoppingCartItemInfo.PromotionId = promotionInfo.ActivityId;
							shoppingCartItemInfo.PromotionName = promotionInfo.Name;
							shoppingCartItemInfo.AdjustedPrice = promotionInfo.DiscountValue;
							break;
						case PromoteType.Reduced:
							shoppingCartItemInfo.PromotionId = promotionInfo.ActivityId;
							shoppingCartItemInfo.PromotionName = promotionInfo.Name;
							shoppingCartItemInfo.AdjustedPrice = shoppingCartItemInfo.MemberPrice - promotionInfo.DiscountValue;
							break;
						case PromoteType.QuantityDiscount:
							if (shoppingCartItemInfo.Quantity >= (int)promotionInfo.Condition)
							{
								shoppingCartItemInfo.PromotionId = promotionInfo.ActivityId;
								shoppingCartItemInfo.PromotionName = promotionInfo.Name;
								shoppingCartItemInfo.AdjustedPrice = shoppingCartItemInfo.MemberPrice * promotionInfo.DiscountValue;
							}
							break;
						case PromoteType.SentGift:
							shoppingCartItemInfo.PromotionId = promotionInfo.ActivityId;
							shoppingCartItemInfo.PromotionName = promotionInfo.Name;
							shoppingCartItemInfo.IsSendGift = true;
							break;
						case PromoteType.SentProduct:
							if (shoppingCartItemInfo.Quantity / (int)promotionInfo.Condition >= 1)
							{
								shoppingCartItemInfo.PromotionId = promotionInfo.ActivityId;
								shoppingCartItemInfo.PromotionName = promotionInfo.Name;
								shoppingCartItemInfo.ShippQuantity = shoppingCartItemInfo.Quantity + shoppingCartItemInfo.Quantity / (int)promotionInfo.Condition * (int)promotionInfo.DiscountValue;
							}
							break;
						}
					}
				}
			}
			return shoppingCartItemInfo;
		}
		public override Dictionary<string, decimal> GetCostPriceForItems(string skuIds)
		{
			Dictionary<string, decimal> dictionary = new Dictionary<string, decimal>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(string.Format("SELECT s.SkuId, s.CostPrice FROM Hishop_SKUs s WHERE SkuId IN ({0})", skuIds));
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					decimal value = (dataReader["CostPrice"] == DBNull.Value) ? 0m : ((decimal)dataReader["CostPrice"]);
					dictionary.Add((string)dataReader["SkuId"], value);
				}
			}
			return dictionary;
		}
		public override IList<ShippingModeInfo> GetShippingModes()
		{
			IList<ShippingModeInfo> list = new List<ShippingModeInfo>();
			string text = "SELECT * FROM Hishop_ShippingTypes st INNER JOIN Hishop_ShippingTemplates temp ON st.TemplateId=temp.TemplateId Order By DisplaySequence";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulateShippingMode(dataReader));
				}
			}
			return list;
		}
		public override ShippingModeInfo GetShippingMode(int modeId, bool includeDetail)
		{
			ShippingModeInfo shippingModeInfo = null;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT * FROM Hishop_ShippingTypes st INNER JOIN Hishop_ShippingTemplates temp ON st.TemplateId=temp.TemplateId Where ModeId =@ModeId;");
			if (includeDetail)
			{
				stringBuilder.Append("SELECT GroupId,TemplateId,Price,AddPrice FROM Hishop_ShippingTypeGroups Where TemplateId IN (SELECT TemplateId FROM Hishop_ShippingTypes WHERE ModeId =@ModeId);");
				stringBuilder.Append("SELECT TemplateId,GroupId,RegionId FROM Hishop_ShippingRegions Where TemplateId IN (SELECT TemplateId FROM Hishop_ShippingTypes Where ModeId =@ModeId);");
				stringBuilder.Append(" SELECT * FROM Hishop_TemplateRelatedShipping Where ModeId =@ModeId");
			}
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "ModeId", System.Data.DbType.Int32, modeId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					shippingModeInfo = DataMapper.PopulateShippingMode(dataReader);
				}
				if (includeDetail)
				{
					dataReader.NextResult();
					while (dataReader.Read())
					{
						shippingModeInfo.ModeGroup.Add(DataMapper.PopulateShippingModeGroup(dataReader));
					}
					dataReader.NextResult();
					while (dataReader.Read())
					{
						foreach (ShippingModeGroupInfo current in shippingModeInfo.ModeGroup)
						{
							if (current.GroupId == (int)dataReader["GroupId"])
							{
								current.ModeRegions.Add(DataMapper.PopulateShippingRegion(dataReader));
							}
						}
					}
					dataReader.NextResult();
					while (dataReader.Read())
					{
						if (dataReader["ExpressCompanyName"] != DBNull.Value)
						{
							shippingModeInfo.ExpressCompany.Add((string)dataReader["ExpressCompanyName"]);
						}
					}
				}
			}
			return shippingModeInfo;
		}
		public override IList<string> GetExpressCompanysByMode(int modeId)
		{
			IList<string> list = new List<string>();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_TemplateRelatedShipping Where ModeId =@ModeId");
			this.database.AddInParameter(sqlStringCommand, "ModeId", System.Data.DbType.Int32, modeId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					if (dataReader["ExpressCompanyName"] != DBNull.Value)
					{
						list.Add((string)dataReader["ExpressCompanyName"]);
					}
				}
			}
			return list;
		}
		public override IList<PaymentModeInfo> GetPaymentModes()
		{
			IList<PaymentModeInfo> list = new List<PaymentModeInfo>();
			string text = "SELECT * FROM Hishop_PaymentTypes Order by DisplaySequence desc";
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				while (dataReader.Read())
				{
					list.Add(DataMapper.PopulatePayment(dataReader));
				}
			}
			return list;
		}
		public override PaymentModeInfo GetPaymentMode(int modeId)
		{
			PaymentModeInfo result = new PaymentModeInfo();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PaymentTypes WHERE ModeId = @ModeId;");
			this.database.AddInParameter(sqlStringCommand, "ModeId", System.Data.DbType.Int32, modeId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulatePayment(dataReader);
				}
			}
			return result;
		}
		public override CouponInfo GetCoupon(string couponCode)
		{
			CouponInfo result = null;
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT c.* FROM Hishop_Coupons c INNER  JOIN Hishop_CouponItems ci ON ci.CouponId = c.CouponId Where ci.ClaimCode =@ClaimCode   and  CouponStatus=0 AND  @DateTime>c.StartTime and  @DateTime <c.ClosingTime");
			this.database.AddInParameter(sqlStringCommand, "ClaimCode", System.Data.DbType.String, couponCode);
			this.database.AddInParameter(sqlStringCommand, "DateTime", System.Data.DbType.DateTime, DateTime.UtcNow);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					result = DataMapper.PopulateCoupon(dataReader);
				}
			}
			return result;
		}
		public override System.Data.DataTable GetCoupon(decimal orderAmount)
		{
			System.Data.DataTable result = new System.Data.DataTable();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT ci.ClaimCode,c.DiscountValue,(ClaimCode+'\u3000\u3000\u3000\u3000\u3000面值'+cast(DiscountValue as varchar(10))) as DisplayText FROM Hishop_Coupons c INNER  JOIN Hishop_CouponItems ci ON ci.CouponId = c.CouponId Where  @DateTime>c.StartTime and @DateTime <c.ClosingTime AND ((Amount>0 and @orderAmount>=Amount) or (Amount=0 and @orderAmount>=DiscountValue))    and  CouponStatus=0  AND UserId=@UserId");
			this.database.AddInParameter(sqlStringCommand, "DateTime", System.Data.DbType.DateTime, DateTime.UtcNow);
			this.database.AddInParameter(sqlStringCommand, "orderAmount", System.Data.DbType.Decimal, orderAmount);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, HiContext.Current.User.UserId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				result = DataHelper.ConverDataReaderToDataTable(dataReader);
			}
			return result;
		}
		public override OrderInfo GetOrderInfo(string orderId)
		{
			OrderInfo orderInfo = null;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(" SELECT * FROM Hishop_Orders Where OrderId = @OrderId ");
			stringBuilder.Append(" SELECT * FROM Hishop_OrderGifts Where OrderId = @OrderId ");
			stringBuilder.Append(" SELECT o.*,(SELECT Stock FROM Hishop_SKUs WHERE SkuId=o.SkuId)  as Stock FROM Hishop_OrderItems o Where o.OrderId = @OrderId  ");
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(stringBuilder.ToString());
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					orderInfo = DataMapper.PopulateOrder(dataReader);
				}
				dataReader.NextResult();
				while (dataReader.Read())
				{
					orderInfo.Gifts.Add(DataMapper.PopulateOrderGift(dataReader));
				}
				dataReader.NextResult();
				while (dataReader.Read())
				{
					orderInfo.LineItems.Add((string)dataReader["SkuId"], DataMapper.PopulateLineItem(dataReader));
				}
			}
			return orderInfo;
		}
		public override bool CreatOrder(OrderInfo orderInfo)
		{
			bool flag = false;
			bool result;
			using (System.Data.Common.DbConnection dbConnection = this.database.CreateConnection())
			{
				dbConnection.Open();
				System.Data.Common.DbTransaction dbTransaction = dbConnection.BeginTransaction();
				try
				{
					if (!this.CreatOrder(orderInfo, dbTransaction))
					{
						dbTransaction.Rollback();
						result = false;
						return result;
					}
					if (orderInfo.LineItems.Count > 0 && !this.AddOrderLineItems(orderInfo.OrderId, orderInfo.LineItems.Values, dbTransaction))
					{
						dbTransaction.Rollback();
						result = false;
						return result;
					}
					if (orderInfo.Gifts.Count > 0 && !this.AddOrderGiftItems(orderInfo.OrderId, orderInfo.Gifts, dbTransaction))
					{
						dbTransaction.Rollback();
						result = false;
						return result;
					}
					if (!string.IsNullOrEmpty(orderInfo.CouponCode) && !this.AddCouponUseRecord(orderInfo, dbTransaction))
					{
						dbTransaction.Rollback();
						result = false;
						return result;
					}
					dbTransaction.Commit();
					flag = true;
				}
				catch
				{
					dbTransaction.Rollback();
					throw;
				}
				finally
				{
					dbConnection.Close();
				}
			}
			result = flag;
			return result;
		}
		private bool CreatOrder(OrderInfo orderInfo, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand storedProcCommand = this.database.GetStoredProcCommand("ss_CreateOrder");
			this.database.AddInParameter(storedProcCommand, "OrderId", System.Data.DbType.String, orderInfo.OrderId);
			this.database.AddInParameter(storedProcCommand, "OrderDate", System.Data.DbType.DateTime, orderInfo.OrderDate);
			this.database.AddInParameter(storedProcCommand, "UserId", System.Data.DbType.Int32, orderInfo.UserId);
			this.database.AddInParameter(storedProcCommand, "UserName", System.Data.DbType.String, orderInfo.Username);
			this.database.AddInParameter(storedProcCommand, "Wangwang", System.Data.DbType.String, orderInfo.Wangwang);
			this.database.AddInParameter(storedProcCommand, "RealName", System.Data.DbType.String, orderInfo.RealName);
			this.database.AddInParameter(storedProcCommand, "EmailAddress", System.Data.DbType.String, orderInfo.EmailAddress);
			this.database.AddInParameter(storedProcCommand, "Remark", System.Data.DbType.String, orderInfo.Remark);
			this.database.AddInParameter(storedProcCommand, "AdjustedDiscount", System.Data.DbType.Currency, orderInfo.AdjustedDiscount);
			this.database.AddInParameter(storedProcCommand, "OrderStatus", System.Data.DbType.Int32, (int)orderInfo.OrderStatus);
			this.database.AddInParameter(storedProcCommand, "ShippingRegion", System.Data.DbType.String, orderInfo.ShippingRegion);
			this.database.AddInParameter(storedProcCommand, "Address", System.Data.DbType.String, orderInfo.Address);
			this.database.AddInParameter(storedProcCommand, "ZipCode", System.Data.DbType.String, orderInfo.ZipCode);
			this.database.AddInParameter(storedProcCommand, "ShipTo", System.Data.DbType.String, orderInfo.ShipTo);
			this.database.AddInParameter(storedProcCommand, "TelPhone", System.Data.DbType.String, orderInfo.TelPhone);
			this.database.AddInParameter(storedProcCommand, "CellPhone", System.Data.DbType.String, orderInfo.CellPhone);
			this.database.AddInParameter(storedProcCommand, "ShipToDate", System.Data.DbType.String, orderInfo.ShipToDate);
			this.database.AddInParameter(storedProcCommand, "ShippingModeId", System.Data.DbType.Int32, orderInfo.ShippingModeId);
			this.database.AddInParameter(storedProcCommand, "ModeName", System.Data.DbType.String, orderInfo.ModeName);
			this.database.AddInParameter(storedProcCommand, "RegionId", System.Data.DbType.Int32, orderInfo.RegionId);
			this.database.AddInParameter(storedProcCommand, "Freight", System.Data.DbType.Currency, orderInfo.Freight);
			this.database.AddInParameter(storedProcCommand, "AdjustedFreight", System.Data.DbType.Currency, orderInfo.AdjustedFreight);
			this.database.AddInParameter(storedProcCommand, "ShipOrderNumber", System.Data.DbType.String, orderInfo.ShipOrderNumber);
			this.database.AddInParameter(storedProcCommand, "Weight", System.Data.DbType.Int32, orderInfo.Weight);
			this.database.AddInParameter(storedProcCommand, "ExpressCompanyName", System.Data.DbType.String, orderInfo.ExpressCompanyName);
			this.database.AddInParameter(storedProcCommand, "ExpressCompanyAbb", System.Data.DbType.String, orderInfo.ExpressCompanyAbb);
			this.database.AddInParameter(storedProcCommand, "PaymentTypeId", System.Data.DbType.Int32, orderInfo.PaymentTypeId);
			this.database.AddInParameter(storedProcCommand, "PaymentType", System.Data.DbType.String, orderInfo.PaymentType);
			this.database.AddInParameter(storedProcCommand, "PayCharge", System.Data.DbType.Currency, orderInfo.PayCharge);
			this.database.AddInParameter(storedProcCommand, "RefundStatus", System.Data.DbType.Int32, (int)orderInfo.RefundStatus);
			this.database.AddInParameter(storedProcCommand, "Gateway", System.Data.DbType.String, orderInfo.Gateway);
			this.database.AddInParameter(storedProcCommand, "OrderTotal", System.Data.DbType.Currency, orderInfo.GetTotal());
			this.database.AddInParameter(storedProcCommand, "OrderPoint", System.Data.DbType.Int32, orderInfo.Points);
			this.database.AddInParameter(storedProcCommand, "OrderCostPrice", System.Data.DbType.Currency, orderInfo.GetCostPrice());
			this.database.AddInParameter(storedProcCommand, "OrderProfit", System.Data.DbType.Currency, orderInfo.GetProfit());
			this.database.AddInParameter(storedProcCommand, "Amount", System.Data.DbType.Currency, orderInfo.GetAmount());
			this.database.AddInParameter(storedProcCommand, "ReducedPromotionId", System.Data.DbType.Int32, orderInfo.ReducedPromotionId);
			this.database.AddInParameter(storedProcCommand, "ReducedPromotionName", System.Data.DbType.String, orderInfo.ReducedPromotionName);
			this.database.AddInParameter(storedProcCommand, "ReducedPromotionAmount", System.Data.DbType.Currency, orderInfo.ReducedPromotionAmount);
			this.database.AddInParameter(storedProcCommand, "IsReduced", System.Data.DbType.Boolean, orderInfo.IsReduced);
			this.database.AddInParameter(storedProcCommand, "SentTimesPointPromotionId", System.Data.DbType.Int32, orderInfo.SentTimesPointPromotionId);
			this.database.AddInParameter(storedProcCommand, "SentTimesPointPromotionName", System.Data.DbType.String, orderInfo.SentTimesPointPromotionName);
			this.database.AddInParameter(storedProcCommand, "TimesPoint", System.Data.DbType.Currency, orderInfo.TimesPoint);
			this.database.AddInParameter(storedProcCommand, "IsSendTimesPoint", System.Data.DbType.Boolean, orderInfo.IsSendTimesPoint);
			this.database.AddInParameter(storedProcCommand, "FreightFreePromotionId", System.Data.DbType.Int32, orderInfo.FreightFreePromotionId);
			this.database.AddInParameter(storedProcCommand, "FreightFreePromotionName", System.Data.DbType.String, orderInfo.FreightFreePromotionName);
			this.database.AddInParameter(storedProcCommand, "IsFreightFree", System.Data.DbType.Boolean, orderInfo.IsFreightFree);
			this.database.AddInParameter(storedProcCommand, "CouponName", System.Data.DbType.String, orderInfo.CouponName);
			this.database.AddInParameter(storedProcCommand, "CouponCode", System.Data.DbType.String, orderInfo.CouponCode);
			this.database.AddInParameter(storedProcCommand, "CouponAmount", System.Data.DbType.Currency, orderInfo.CouponAmount);
			this.database.AddInParameter(storedProcCommand, "CouponValue", System.Data.DbType.Currency, orderInfo.CouponValue);
			if (orderInfo.GroupBuyId > 0)
			{
				this.database.AddInParameter(storedProcCommand, "GroupBuyId", System.Data.DbType.Int32, orderInfo.GroupBuyId);
				this.database.AddInParameter(storedProcCommand, "NeedPrice", System.Data.DbType.Currency, orderInfo.NeedPrice);
				this.database.AddInParameter(storedProcCommand, "GroupBuyStatus", System.Data.DbType.Int32, 1);
			}
			else
			{
				this.database.AddInParameter(storedProcCommand, "GroupBuyId", System.Data.DbType.Int32, DBNull.Value);
				this.database.AddInParameter(storedProcCommand, "NeedPrice", System.Data.DbType.Currency, DBNull.Value);
				this.database.AddInParameter(storedProcCommand, "GroupBuyStatus", System.Data.DbType.Int32, DBNull.Value);
			}
			if (orderInfo.CountDownBuyId > 0)
			{
				this.database.AddInParameter(storedProcCommand, "CountDownBuyId ", System.Data.DbType.Int32, orderInfo.CountDownBuyId);
			}
			else
			{
				this.database.AddInParameter(storedProcCommand, "CountDownBuyId ", System.Data.DbType.Int32, DBNull.Value);
			}
			if (orderInfo.BundlingID > 0)
			{
				this.database.AddInParameter(storedProcCommand, "BundlingID ", System.Data.DbType.Int32, orderInfo.BundlingID);
				this.database.AddInParameter(storedProcCommand, "BundlingPrice", System.Data.DbType.Currency, orderInfo.BundlingPrice);
			}
			else
			{
				this.database.AddInParameter(storedProcCommand, "BundlingID ", System.Data.DbType.Int32, DBNull.Value);
				this.database.AddInParameter(storedProcCommand, "BundlingPrice", System.Data.DbType.Currency, DBNull.Value);
			}
			this.database.AddInParameter(storedProcCommand, "Tax", System.Data.DbType.Currency, orderInfo.Tax);
			this.database.AddInParameter(storedProcCommand, "InvoiceTitle", System.Data.DbType.String, orderInfo.InvoiceTitle);
			return this.database.ExecuteNonQuery(storedProcCommand, dbTran) == 1;
		}
		private bool AddOrderLineItems(string orderId, ICollection lineItems, System.Data.Common.DbTransaction dbTran)
		{
			bool result;
			if (lineItems == null || lineItems.Count == 0)
			{
				result = false;
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
				this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
				int num = 0;
				StringBuilder stringBuilder = new StringBuilder();
				foreach (LineItemInfo lineItemInfo in lineItems)
				{
					string text = num.ToString(CultureInfo.InvariantCulture);
					stringBuilder.Append("INSERT INTO Hishop_OrderItems(OrderId, SkuId, ProductId, SKU, Quantity, ShipmentQuantity, CostPrice").Append(",ItemListPrice, ItemAdjustedPrice, ItemDescription, ThumbnailsUrl, Weight, SKUContent, PromotionId, PromotionName) VALUES( @OrderId").Append(",@SkuId").Append(text).Append(",@ProductId").Append(text).Append(",@SKU").Append(text).Append(",@Quantity").Append(text).Append(",@ShipmentQuantity").Append(text).Append(",@CostPrice").Append(text).Append(",@ItemListPrice").Append(text).Append(",@ItemAdjustedPrice").Append(text).Append(",@ItemDescription").Append(text).Append(",@ThumbnailsUrl").Append(text).Append(",@Weight").Append(text).Append(",@SKUContent").Append(text).Append(",@PromotionId").Append(text).Append(",@PromotionName").Append(text).Append(");");
					this.database.AddInParameter(sqlStringCommand, "SkuId" + text, System.Data.DbType.String, lineItemInfo.SkuId);
					this.database.AddInParameter(sqlStringCommand, "ProductId" + text, System.Data.DbType.Int32, lineItemInfo.ProductId);
					this.database.AddInParameter(sqlStringCommand, "SKU" + text, System.Data.DbType.String, lineItemInfo.SKU);
					this.database.AddInParameter(sqlStringCommand, "Quantity" + text, System.Data.DbType.Int32, lineItemInfo.Quantity);
					this.database.AddInParameter(sqlStringCommand, "ShipmentQuantity" + text, System.Data.DbType.Int32, lineItemInfo.ShipmentQuantity);
					this.database.AddInParameter(sqlStringCommand, "CostPrice" + text, System.Data.DbType.Currency, lineItemInfo.ItemCostPrice);
					this.database.AddInParameter(sqlStringCommand, "ItemListPrice" + text, System.Data.DbType.Currency, lineItemInfo.ItemListPrice);
					this.database.AddInParameter(sqlStringCommand, "ItemAdjustedPrice" + text, System.Data.DbType.Currency, lineItemInfo.ItemAdjustedPrice);
					this.database.AddInParameter(sqlStringCommand, "ItemDescription" + text, System.Data.DbType.String, lineItemInfo.ItemDescription);
					this.database.AddInParameter(sqlStringCommand, "ThumbnailsUrl" + text, System.Data.DbType.String, lineItemInfo.ThumbnailsUrl);
					this.database.AddInParameter(sqlStringCommand, "Weight" + text, System.Data.DbType.Int32, lineItemInfo.ItemWeight);
					this.database.AddInParameter(sqlStringCommand, "SKUContent" + text, System.Data.DbType.String, lineItemInfo.SKUContent);
					this.database.AddInParameter(sqlStringCommand, "PromotionId" + text, System.Data.DbType.Int32, lineItemInfo.PromotionId);
					this.database.AddInParameter(sqlStringCommand, "PromotionName" + text, System.Data.DbType.String, lineItemInfo.PromotionName);
					num++;
					if (num == 50)
					{
						sqlStringCommand.CommandText = stringBuilder.ToString();
						int num2;
						if (dbTran != null)
						{
							num2 = this.database.ExecuteNonQuery(sqlStringCommand, dbTran);
						}
						else
						{
							num2 = this.database.ExecuteNonQuery(sqlStringCommand);
						}
						if (num2 <= 0)
						{
							result = false;
							return result;
						}
						stringBuilder.Remove(0, stringBuilder.Length);
						sqlStringCommand.Parameters.Clear();
						this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
						num = 0;
					}
				}
				if (stringBuilder.ToString().Length > 0)
				{
					sqlStringCommand.CommandText = stringBuilder.ToString();
					if (dbTran != null)
					{
						result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
					}
					else
					{
						result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
					}
				}
				else
				{
					result = true;
				}
			}
			return result;
		}
		private bool AddOrderGiftItems(string orderId, IList<OrderGiftInfo> orderGifts, System.Data.Common.DbTransaction dbTran)
		{
			bool result;
			if (orderGifts == null || orderGifts.Count == 0)
			{
				result = false;
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
				this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
				int num = 0;
				StringBuilder stringBuilder = new StringBuilder();
				foreach (OrderGiftInfo current in orderGifts)
				{
					string text = num.ToString(CultureInfo.InvariantCulture);
					stringBuilder.Append("INSERT INTO Hishop_OrderGifts(OrderId, GiftId, GiftName, CostPrice, ThumbnailsUrl, Quantity,PromoType) VALUES( @OrderId,").Append("@GiftId").Append(text).Append(",@GiftName").Append(text).Append(",@CostPrice").Append(text).Append(",@ThumbnailsUrl").Append(text).Append(",@Quantity").Append(text).Append(",@PromoType").Append(text).Append(");");
					this.database.AddInParameter(sqlStringCommand, "GiftId" + text, System.Data.DbType.Int32, current.GiftId);
					this.database.AddInParameter(sqlStringCommand, "GiftName" + text, System.Data.DbType.String, current.GiftName);
					this.database.AddInParameter(sqlStringCommand, "CostPrice" + text, System.Data.DbType.Currency, current.CostPrice);
					this.database.AddInParameter(sqlStringCommand, "ThumbnailsUrl" + text, System.Data.DbType.String, current.ThumbnailsUrl);
					this.database.AddInParameter(sqlStringCommand, "Quantity" + text, System.Data.DbType.Int32, current.Quantity);
					this.database.AddInParameter(sqlStringCommand, "PromoType" + text, System.Data.DbType.Int32, current.PromoteType);
					num++;
					if (num == 50)
					{
						sqlStringCommand.CommandText = stringBuilder.ToString();
						int num2;
						if (dbTran != null)
						{
							num2 = this.database.ExecuteNonQuery(sqlStringCommand, dbTran);
						}
						else
						{
							num2 = this.database.ExecuteNonQuery(sqlStringCommand);
						}
						if (num2 <= 0)
						{
							result = false;
							return result;
						}
						stringBuilder.Remove(0, stringBuilder.Length);
						sqlStringCommand.Parameters.Clear();
						this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, orderId);
						num = 0;
					}
				}
				if (stringBuilder.ToString().Length > 0)
				{
					sqlStringCommand.CommandText = stringBuilder.ToString();
					if (dbTran != null)
					{
						result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
					}
					else
					{
						result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
					}
				}
				else
				{
					result = true;
				}
			}
			return result;
		}
		private bool AddCouponUseRecord(OrderInfo orderinfo, System.Data.Common.DbTransaction dbTran)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("update  Hishop_CouponItems  set userName=@UserName,Userid=@Userid,Orderid=@Orderid,CouponStatus=@CouponStatus,EmailAddress=@EmailAddress,UsedTime=@UsedTime WHERE ClaimCode=@ClaimCode and CouponStatus!=1");
			this.database.AddInParameter(sqlStringCommand, "ClaimCode", System.Data.DbType.String, orderinfo.CouponCode);
			this.database.AddInParameter(sqlStringCommand, "userName", System.Data.DbType.String, orderinfo.Username);
			this.database.AddInParameter(sqlStringCommand, "userid", System.Data.DbType.Int32, orderinfo.UserId);
			this.database.AddInParameter(sqlStringCommand, "CouponStatus", System.Data.DbType.Int32, 1);
			this.database.AddInParameter(sqlStringCommand, "UsedTime", System.Data.DbType.DateTime, DateTime.Now);
			this.database.AddInParameter(sqlStringCommand, "EmailAddress", System.Data.DbType.String, orderinfo.EmailAddress);
			this.database.AddInParameter(sqlStringCommand, "Orderid", System.Data.DbType.String, orderinfo.OrderId);
			bool result;
			if (dbTran != null)
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
			}
			else
			{
				result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
			}
			return result;
		}
		public override bool AddMemberPoint(UserPointInfo point)
		{
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("INSERT INTO Hishop_PointDetails (OrderId,UserId, TradeDate, TradeType, Increased, Reduced, Points, Remark)VALUES(@OrderId,@UserId, @TradeDate, @TradeType, @Increased, @Reduced, @Points, @Remark)");
			this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, point.OrderId);
			this.database.AddInParameter(sqlStringCommand, "UserId", System.Data.DbType.Int32, point.UserId);
			this.database.AddInParameter(sqlStringCommand, "TradeDate", System.Data.DbType.DateTime, point.TradeDate);
			this.database.AddInParameter(sqlStringCommand, "TradeType", System.Data.DbType.Int32, (int)point.TradeType);
			this.database.AddInParameter(sqlStringCommand, "Increased", System.Data.DbType.Int32, point.Increased.HasValue ? point.Increased.Value : 0);
			this.database.AddInParameter(sqlStringCommand, "Reduced", System.Data.DbType.Int32, point.Reduced.HasValue ? point.Reduced.Value : 0);
			this.database.AddInParameter(sqlStringCommand, "Points", System.Data.DbType.Int32, point.Points);
			this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, point.Remark);
			return this.database.ExecuteNonQuery(sqlStringCommand) > 0;
		}
		public override System.Data.DataTable GetYetShipOrders(int days)
		{
			System.Data.DataTable result;
			if (days <= 0)
			{
				result = null;
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * from Hishop_Orders where OrderStatus=@OrderStatus and ShippingDate>=@FromDate and ShippingDate<=@ToDate;");
				this.database.AddInParameter(sqlStringCommand, "OrderStatus", System.Data.DbType.Int32, 3);
				this.database.AddInParameter(sqlStringCommand, "FromDate", System.Data.DbType.DateTime, DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")).AddDays((double)(-(double)days)));
				this.database.AddInParameter(sqlStringCommand, "ToDate", System.Data.DbType.DateTime, DateTime.Now);
				System.Data.DataTable dataTable = null;
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					dataTable = DataHelper.ConverDataReaderToDataTable(dataReader);
				}
				result = dataTable;
			}
			return result;
		}
		public override PurchaseOrderInfo GetPurchaseOrder(string purchaseOrderId)
		{
			PurchaseOrderInfo purchaseOrderInfo = new PurchaseOrderInfo();
			System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand("SELECT * FROM Hishop_PurchaseOrders Where PurchaseOrderId = @PurchaseOrderId");
			this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrderId);
			using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
			{
				if (dataReader.Read())
				{
					purchaseOrderInfo.PurchaseOrderId = (string)dataReader["PurchaseOrderId"];
					if (DBNull.Value != dataReader["ExpressCompanyAbb"])
					{
						purchaseOrderInfo.ExpressCompanyAbb = (string)dataReader["ExpressCompanyAbb"];
					}
					if (DBNull.Value != dataReader["ExpressCompanyName"])
					{
						purchaseOrderInfo.ExpressCompanyName = (string)dataReader["ExpressCompanyName"];
					}
					if (DBNull.Value != dataReader["ShipOrderNumber"])
					{
						purchaseOrderInfo.ShipOrderNumber = (string)dataReader["ShipOrderNumber"];
					}
					if (DBNull.Value != dataReader["PurchaseStatus"])
					{
						purchaseOrderInfo.PurchaseStatus = (OrderStatus)dataReader["PurchaseStatus"];
					}
				}
			}
			return purchaseOrderInfo;
		}
	}
}
