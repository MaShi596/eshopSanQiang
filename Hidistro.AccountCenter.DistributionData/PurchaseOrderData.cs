using Hidistro.AccountCenter.Business;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Membership.Core.Enums;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
namespace Hidistro.AccountCenter.DistributionData
{
	public class PurchaseOrderData : PurchaseOrderProvider
	{
		private Database database;
		public PurchaseOrderData()
		{
			this.database = DatabaseFactory.CreateDatabase();
		}
		private PurchaseOrderInfo ConvertOrderToPurchaseOrder(OrderInfo order)
		{
			PurchaseOrderInfo result;
			if (order == null)
			{
				result = null;
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				string text = "";
				foreach (LineItemInfo current in order.LineItems.Values)
				{
					stringBuilder.AppendFormat("'" + current.SkuId + "',", new object[0]);
				}
				if (stringBuilder.Length > 0)
				{
					stringBuilder = stringBuilder.Remove(stringBuilder.Length - 1, 1);
					text = string.Format("SELECT S.SkuId, S.CostPrice, p.ProductName FROM Hishop_Products P JOIN Hishop_SKUs S ON P.ProductId = S.ProductId WHERE S.SkuId IN({0});", stringBuilder);
				}
				if (order.Gifts.Count > 0)
				{
					StringBuilder stringBuilder2 = new StringBuilder();
					foreach (OrderGiftInfo current2 in order.Gifts)
					{
						stringBuilder2.AppendFormat(current2.GiftId.ToString() + ",", new object[0]);
					}
					stringBuilder2.Remove(stringBuilder2.Length - 1, 1);
					text += string.Format(" SELECT GiftId, CostPrice FROM Hishop_Gifts WHERE GiftId IN({0});", stringBuilder2.ToString());
				}
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(text);
				Dictionary<string, PurchaseOrderItemInfo> dictionary = new Dictionary<string, PurchaseOrderItemInfo>();
				Dictionary<int, decimal> dictionary2 = new Dictionary<int, decimal>();
				using (System.Data.IDataReader dataReader = this.database.ExecuteReader(sqlStringCommand))
				{
					if (order.LineItems.Values.Count > 0)
					{
						while (dataReader.Read())
						{
							PurchaseOrderItemInfo purchaseOrderItemInfo = new PurchaseOrderItemInfo();
							if (dataReader["CostPrice"] != DBNull.Value)
							{
								purchaseOrderItemInfo.ItemCostPrice = (decimal)dataReader["CostPrice"];
							}
							purchaseOrderItemInfo.ItemHomeSiteDescription = (string)dataReader["ProductName"];
							dictionary.Add((string)dataReader["SkuId"], purchaseOrderItemInfo);
						}
					}
					if (order.Gifts.Count > 0)
					{
						if (order.LineItems.Count > 0)
						{
							dataReader.NextResult();
						}
						while (dataReader.Read())
						{
							dictionary2.Add((int)dataReader["GiftId"], (DBNull.Value == dataReader["CostPrice"]) ? 0m : Convert.ToDecimal(dataReader["CostPrice"]));
						}
					}
				}
				IUser user = Users.GetUser(HiContext.Current.SiteSettings.UserId.Value, false);
				if (user == null || user.UserRole != UserRole.Distributor)
				{
					result = null;
				}
				else
				{
					Distributor distributor = user as Distributor;
					PurchaseOrderInfo purchaseOrderInfo = new PurchaseOrderInfo();
					purchaseOrderInfo.PurchaseOrderId = "PO" + order.OrderId;
					purchaseOrderInfo.OrderId = order.OrderId;
					purchaseOrderInfo.Remark = order.Remark;
					purchaseOrderInfo.PurchaseStatus = OrderStatus.WaitBuyerPay;
					purchaseOrderInfo.DistributorId = distributor.UserId;
					purchaseOrderInfo.Distributorname = distributor.Username;
					purchaseOrderInfo.DistributorEmail = distributor.Email;
					purchaseOrderInfo.DistributorRealName = distributor.RealName;
					purchaseOrderInfo.DistributorQQ = distributor.QQ;
					purchaseOrderInfo.DistributorWangwang = distributor.Wangwang;
					purchaseOrderInfo.DistributorMSN = distributor.MSN;
					purchaseOrderInfo.ShippingRegion = order.ShippingRegion;
					purchaseOrderInfo.Address = order.Address;
					purchaseOrderInfo.ZipCode = order.ZipCode;
					purchaseOrderInfo.ShipTo = order.ShipTo;
					purchaseOrderInfo.TelPhone = order.TelPhone;
					purchaseOrderInfo.CellPhone = order.CellPhone;
					purchaseOrderInfo.ShipToDate = order.ShipToDate;
					purchaseOrderInfo.ShippingModeId = order.ShippingModeId;
					purchaseOrderInfo.ModeName = order.ModeName;
					purchaseOrderInfo.RegionId = order.RegionId;
					purchaseOrderInfo.Freight = order.Freight;
					purchaseOrderInfo.AdjustedFreight = order.Freight;
					purchaseOrderInfo.ShipOrderNumber = order.ShipOrderNumber;
					purchaseOrderInfo.Weight = order.Weight;
					purchaseOrderInfo.RefundStatus = RefundStatus.None;
					purchaseOrderInfo.OrderTotal = order.GetTotal();
					purchaseOrderInfo.ExpressCompanyName = order.ExpressCompanyName;
					purchaseOrderInfo.ExpressCompanyAbb = order.ExpressCompanyAbb;
					purchaseOrderInfo.Tax = order.Tax;
					purchaseOrderInfo.InvoiceTitle = order.InvoiceTitle;
					foreach (LineItemInfo current3 in order.LineItems.Values)
					{
						PurchaseOrderItemInfo purchaseOrderItemInfo2 = new PurchaseOrderItemInfo();
						purchaseOrderItemInfo2.PurchaseOrderId = purchaseOrderInfo.PurchaseOrderId;
						purchaseOrderItemInfo2.SkuId = current3.SkuId;
						purchaseOrderItemInfo2.ProductId = current3.ProductId;
						purchaseOrderItemInfo2.SKU = current3.SKU;
						purchaseOrderItemInfo2.Quantity = current3.ShipmentQuantity;
						foreach (KeyValuePair<string, PurchaseOrderItemInfo> current4 in dictionary)
						{
							if (current4.Key == current3.SkuId)
							{
								purchaseOrderItemInfo2.ItemCostPrice = current4.Value.ItemCostPrice;
								purchaseOrderItemInfo2.ItemHomeSiteDescription = current4.Value.ItemHomeSiteDescription;
							}
						}
						purchaseOrderItemInfo2.ItemPurchasePrice = current3.ItemCostPrice;
						purchaseOrderItemInfo2.ItemListPrice = current3.ItemListPrice;
						purchaseOrderItemInfo2.ItemDescription = current3.ItemDescription;
						purchaseOrderItemInfo2.SKUContent = current3.SKUContent;
						purchaseOrderItemInfo2.ThumbnailsUrl = current3.ThumbnailsUrl;
						purchaseOrderItemInfo2.ItemWeight = current3.ItemWeight;
						if (string.IsNullOrEmpty(purchaseOrderItemInfo2.ItemHomeSiteDescription))
						{
							purchaseOrderItemInfo2.ItemHomeSiteDescription = purchaseOrderItemInfo2.ItemDescription;
						}
						purchaseOrderInfo.PurchaseOrderItems.Add(purchaseOrderItemInfo2);
					}
					foreach (OrderGiftInfo current5 in order.Gifts)
					{
						PurchaseOrderGiftInfo purchaseOrderGiftInfo = new PurchaseOrderGiftInfo();
						purchaseOrderGiftInfo.PurchaseOrderId = purchaseOrderInfo.PurchaseOrderId;
						foreach (KeyValuePair<int, decimal> current6 in dictionary2)
						{
							if (current6.Key == current5.GiftId)
							{
								purchaseOrderGiftInfo.CostPrice = current6.Value;
							}
						}
						purchaseOrderGiftInfo.PurchasePrice = current5.CostPrice;
						purchaseOrderGiftInfo.GiftId = current5.GiftId;
						purchaseOrderGiftInfo.GiftName = current5.GiftName;
						purchaseOrderGiftInfo.Quantity = current5.Quantity;
						purchaseOrderGiftInfo.ThumbnailsUrl = current5.ThumbnailsUrl;
						purchaseOrderInfo.PurchaseOrderGifts.Add(purchaseOrderGiftInfo);
					}
					result = purchaseOrderInfo;
				}
			}
			return result;
		}
		public override bool CreatePurchaseOrder(OrderInfo order, System.Data.Common.DbTransaction dbTran)
		{
			PurchaseOrderInfo purchaseOrderInfo = this.ConvertOrderToPurchaseOrder(order);
			bool result;
			if (purchaseOrderInfo == null)
			{
				result = false;
			}
			else
			{
				System.Data.Common.DbCommand sqlStringCommand = this.database.GetSqlStringCommand(" ");
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append("INSERT INTO Hishop_PurchaseOrders(PurchaseOrderId, OrderId, Remark, ManagerMark, ManagerRemark, AdjustedDiscount,PurchaseStatus, CloseReason, PurchaseDate, DistributorId, Distributorname,DistributorEmail, DistributorRealName, DistributorQQ, DistributorWangwang, DistributorMSN,ShippingRegion, Address, ZipCode, ShipTo, TelPhone, CellPhone, ShipToDate, ShippingModeId, ModeName,RealShippingModeId, RealModeName, RegionId, Freight, AdjustedFreight, ShipOrderNumber, Weight,ExpressCompanyName,ExpressCompanyAbb,RefundStatus, RefundAmount, RefundRemark, OrderTotal, PurchaseProfit, PurchaseTotal,Tax,InvoiceTitle)VALUES (@PurchaseOrderId, @OrderId, @Remark, @ManagerMark, @ManagerRemark, @AdjustedDiscount,@PurchaseStatus, @CloseReason, @PurchaseDate, @DistributorId, @Distributorname,@DistributorEmail, @DistributorRealName, @DistributorQQ, @DistributorWangwang, @DistributorMSN,@ShippingRegion, @Address, @ZipCode, @ShipTo, @TelPhone, @CellPhone, @ShipToDate, @ShippingModeId, @ModeName,@RealShippingModeId, @RealModeName, @RegionId, @Freight, @AdjustedFreight, @ShipOrderNumber, @PurchaseWeight,@ExpressCompanyName,@ExpressCompanyAbb,@RefundStatus, @RefundAmount, @RefundRemark, @OrderTotal, @PurchaseProfit, @PurchaseTotal,@Tax,@InvoiceTitle);");
				this.database.AddInParameter(sqlStringCommand, "PurchaseOrderId", System.Data.DbType.String, purchaseOrderInfo.PurchaseOrderId);
				this.database.AddInParameter(sqlStringCommand, "OrderId", System.Data.DbType.String, purchaseOrderInfo.OrderId);
				this.database.AddInParameter(sqlStringCommand, "Remark", System.Data.DbType.String, purchaseOrderInfo.Remark);
				if (purchaseOrderInfo.ManagerMark.HasValue)
				{
					this.database.AddInParameter(sqlStringCommand, "ManagerMark", System.Data.DbType.Int32, (int)purchaseOrderInfo.ManagerMark.Value);
				}
				else
				{
					this.database.AddInParameter(sqlStringCommand, "ManagerMark", System.Data.DbType.Int32, DBNull.Value);
				}
				this.database.AddInParameter(sqlStringCommand, "ManagerRemark", System.Data.DbType.String, purchaseOrderInfo.ManagerRemark);
				this.database.AddInParameter(sqlStringCommand, "AdjustedDiscount", System.Data.DbType.Currency, purchaseOrderInfo.AdjustedDiscount);
				this.database.AddInParameter(sqlStringCommand, "PurchaseStatus", System.Data.DbType.Int32, (int)purchaseOrderInfo.PurchaseStatus);
				this.database.AddInParameter(sqlStringCommand, "CloseReason", System.Data.DbType.String, purchaseOrderInfo.CloseReason);
				this.database.AddInParameter(sqlStringCommand, "PurchaseDate", System.Data.DbType.DateTime, DateTime.Now);
				this.database.AddInParameter(sqlStringCommand, "DistributorId", System.Data.DbType.Int32, purchaseOrderInfo.DistributorId);
				this.database.AddInParameter(sqlStringCommand, "Distributorname", System.Data.DbType.String, purchaseOrderInfo.Distributorname);
				this.database.AddInParameter(sqlStringCommand, "DistributorEmail", System.Data.DbType.String, purchaseOrderInfo.DistributorEmail);
				this.database.AddInParameter(sqlStringCommand, "DistributorRealName", System.Data.DbType.String, purchaseOrderInfo.DistributorRealName);
				this.database.AddInParameter(sqlStringCommand, "DistributorQQ", System.Data.DbType.String, purchaseOrderInfo.DistributorQQ);
				this.database.AddInParameter(sqlStringCommand, "DistributorWangwang", System.Data.DbType.String, purchaseOrderInfo.DistributorWangwang);
				this.database.AddInParameter(sqlStringCommand, "DistributorMSN", System.Data.DbType.String, purchaseOrderInfo.DistributorMSN);
				this.database.AddInParameter(sqlStringCommand, "ShippingRegion", System.Data.DbType.String, purchaseOrderInfo.ShippingRegion);
				this.database.AddInParameter(sqlStringCommand, "Address", System.Data.DbType.String, purchaseOrderInfo.Address);
				this.database.AddInParameter(sqlStringCommand, "ZipCode", System.Data.DbType.String, purchaseOrderInfo.ZipCode);
				this.database.AddInParameter(sqlStringCommand, "ShipTo", System.Data.DbType.String, purchaseOrderInfo.ShipTo);
				this.database.AddInParameter(sqlStringCommand, "TelPhone", System.Data.DbType.String, purchaseOrderInfo.TelPhone);
				this.database.AddInParameter(sqlStringCommand, "CellPhone", System.Data.DbType.String, purchaseOrderInfo.CellPhone);
				this.database.AddInParameter(sqlStringCommand, "ShipToDate", System.Data.DbType.String, purchaseOrderInfo.ShipToDate);
				this.database.AddInParameter(sqlStringCommand, "ShippingModeId", System.Data.DbType.Int32, purchaseOrderInfo.ShippingModeId);
				this.database.AddInParameter(sqlStringCommand, "ModeName", System.Data.DbType.String, purchaseOrderInfo.ModeName);
				this.database.AddInParameter(sqlStringCommand, "RealShippingModeId", System.Data.DbType.Int32, purchaseOrderInfo.RealShippingModeId);
				this.database.AddInParameter(sqlStringCommand, "RealModeName", System.Data.DbType.String, purchaseOrderInfo.RealModeName);
				this.database.AddInParameter(sqlStringCommand, "RegionId", System.Data.DbType.Int32, purchaseOrderInfo.RegionId);
				this.database.AddInParameter(sqlStringCommand, "Freight", System.Data.DbType.Currency, purchaseOrderInfo.Freight);
				this.database.AddInParameter(sqlStringCommand, "AdjustedFreight", System.Data.DbType.Currency, purchaseOrderInfo.AdjustedFreight);
				this.database.AddInParameter(sqlStringCommand, "ShipOrderNumber", System.Data.DbType.String, purchaseOrderInfo.ShipOrderNumber);
				this.database.AddInParameter(sqlStringCommand, "PurchaseWeight", System.Data.DbType.Int32, (int)purchaseOrderInfo.Weight);
				this.database.AddInParameter(sqlStringCommand, "ExpressCompanyAbb", System.Data.DbType.String, purchaseOrderInfo.ExpressCompanyAbb);
				this.database.AddInParameter(sqlStringCommand, "ExpressCompanyName", System.Data.DbType.String, purchaseOrderInfo.ExpressCompanyName);
				this.database.AddInParameter(sqlStringCommand, "RefundStatus", System.Data.DbType.Int32, (int)purchaseOrderInfo.RefundStatus);
				this.database.AddInParameter(sqlStringCommand, "RefundAmount", System.Data.DbType.Currency, purchaseOrderInfo.RefundAmount);
				this.database.AddInParameter(sqlStringCommand, "RefundRemark", System.Data.DbType.String, purchaseOrderInfo.RefundRemark);
				this.database.AddInParameter(sqlStringCommand, "OrderTotal", System.Data.DbType.Currency, purchaseOrderInfo.OrderTotal);
				this.database.AddInParameter(sqlStringCommand, "PurchaseProfit", System.Data.DbType.Currency, purchaseOrderInfo.GetPurchaseProfit());
				this.database.AddInParameter(sqlStringCommand, "PurchaseTotal", System.Data.DbType.Currency, purchaseOrderInfo.GetPurchaseTotal());
				this.database.AddInParameter(sqlStringCommand, "Tax", System.Data.DbType.Currency, purchaseOrderInfo.Tax);
				this.database.AddInParameter(sqlStringCommand, "InvoiceTitle", System.Data.DbType.String, purchaseOrderInfo.InvoiceTitle);
				int num = 0;
				foreach (PurchaseOrderItemInfo current in purchaseOrderInfo.PurchaseOrderItems)
				{
					string text = num.ToString();
					stringBuilder.Append("INSERT INTO Hishop_PurchaseOrderItems(PurchaseOrderId, SkuId, ProductId, SKU, Quantity,  CostPrice, ").Append("ItemListPrice, ItemPurchasePrice, ItemDescription, ItemHomeSiteDescription, SKUContent, ThumbnailsUrl, Weight) VALUES( @PurchaseOrderId").Append(",@SkuId").Append(text).Append(",@ProductId").Append(text).Append(",@SKU").Append(text).Append(",@Quantity").Append(text).Append(",@CostPrice").Append(text).Append(",@ItemListPrice").Append(text).Append(",@ItemPurchasePrice").Append(text).Append(",@ItemDescription").Append(text).Append(",@ItemHomeSiteDescription").Append(text).Append(",@SKUContent").Append(text).Append(",@ThumbnailsUrl").Append(text).Append(",@Weight").Append(text).Append(");");
					this.database.AddInParameter(sqlStringCommand, "SkuId" + text, System.Data.DbType.String, current.SkuId);
					this.database.AddInParameter(sqlStringCommand, "ProductId" + text, System.Data.DbType.Int32, current.ProductId);
					this.database.AddInParameter(sqlStringCommand, "SKU" + text, System.Data.DbType.String, current.SKU);
					this.database.AddInParameter(sqlStringCommand, "Quantity" + text, System.Data.DbType.Int32, current.Quantity);
					this.database.AddInParameter(sqlStringCommand, "CostPrice" + text, System.Data.DbType.Currency, current.ItemCostPrice);
					this.database.AddInParameter(sqlStringCommand, "ItemListPrice" + text, System.Data.DbType.Currency, current.ItemListPrice);
					this.database.AddInParameter(sqlStringCommand, "ItemPurchasePrice" + text, System.Data.DbType.Currency, current.ItemPurchasePrice);
					this.database.AddInParameter(sqlStringCommand, "ItemDescription" + text, System.Data.DbType.String, current.ItemDescription);
					this.database.AddInParameter(sqlStringCommand, "ItemHomeSiteDescription" + text, System.Data.DbType.String, current.ItemHomeSiteDescription);
					this.database.AddInParameter(sqlStringCommand, "SKUContent" + text, System.Data.DbType.String, current.SKUContent);
					this.database.AddInParameter(sqlStringCommand, "ThumbnailsUrl" + text, System.Data.DbType.String, current.ThumbnailsUrl);
					this.database.AddInParameter(sqlStringCommand, "Weight" + text, System.Data.DbType.Int32, current.ItemWeight);
					num++;
				}
				foreach (PurchaseOrderGiftInfo current2 in purchaseOrderInfo.PurchaseOrderGifts)
				{
					string text = num.ToString();
					stringBuilder.Append("INSERT INTO Hishop_PurchaseOrderGifts(PurchaseOrderId, GiftId, GiftName, CostPrice, PurchasePrice, ").Append("ThumbnailsUrl, Quantity) VALUES( @PurchaseOrderId,").Append("@GiftId").Append(text).Append(",@GiftName").Append(text).Append(",@CostPrice").Append(text).Append(",@PurchasePrice").Append(text).Append(",@ThumbnailsUrl").Append(text).Append(",@Quantity").Append(text).Append(");");
					this.database.AddInParameter(sqlStringCommand, "GiftId" + text, System.Data.DbType.Int32, current2.GiftId);
					this.database.AddInParameter(sqlStringCommand, "GiftName" + text, System.Data.DbType.String, current2.GiftName);
					this.database.AddInParameter(sqlStringCommand, "Quantity" + text, System.Data.DbType.Int32, current2.Quantity);
					this.database.AddInParameter(sqlStringCommand, "CostPrice" + text, System.Data.DbType.Currency, current2.CostPrice);
					this.database.AddInParameter(sqlStringCommand, "PurchasePrice" + text, System.Data.DbType.Currency, current2.PurchasePrice);
					this.database.AddInParameter(sqlStringCommand, "ThumbnailsUrl" + text, System.Data.DbType.String, current2.ThumbnailsUrl);
					num++;
				}
				sqlStringCommand.CommandText = stringBuilder.ToString().Remove(stringBuilder.Length - 1);
				if (dbTran != null)
				{
					result = (this.database.ExecuteNonQuery(sqlStringCommand, dbTran) > 0);
				}
				else
				{
					result = (this.database.ExecuteNonQuery(sqlStringCommand) > 0);
				}
			}
			return result;
		}
	}
}
