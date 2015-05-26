using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Core.Enums;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml;
namespace Hidistro.UI.Web.API
{
	[WebService(Namespace = "http://tempuri.org/"), WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class PurchaseOrderAPIHandler : System.Web.IHttpHandler
	{
		private string message = "";
		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
		public void ProcessRequest(System.Web.HttpContext context)
		{
			string text = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
			string str = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
			string text2 = "";
			Hidistro.Membership.Context.SiteSettings masterSettings = Hidistro.Membership.Context.SettingsManager.GetMasterSettings(false);
			string format = "<trade><Oid>{0}</Oid><SellerUid>{1}</SellerUid><BuyerNick>{2}</BuyerNick><BuyerEmail>{3}</BuyerEmail><ReceiverName>{4}</ReceiverName><ReceiverState>{5}</ReceiverState><ReceiverCity>{6}</ReceiverCity><ReceiverDistrict>{7}</ReceiverDistrict><ReceiverAddress>{8}</ReceiverAddress><ReceiverZip>{9}</ReceiverZip><ReceiverMobile>{10}</ReceiverMobile><ReceiverPhone>{11}</ReceiverPhone><BuyerMemo>{12}</BuyerMemo><OrderMark>{13}</OrderMark><SellerMemo>{14}</SellerMemo><Nums>{15}</Nums><Price>{16}</Price><Payment>{17}</Payment><PostFee>{18}</PostFee><Profit>{19}</Profit><PurchaseTotal>{20}</PurchaseTotal><PaymentTs>{21}</PaymentTs><SentTs>{22}</SentTs><RefundStatus>{23}</RefundStatus><RefundAmount>{24}</RefundAmount><RefundRemark>{25}</RefundRemark><Status>{26}</Status><orders list=\"{27}\">{28}</orders></trade>";
			string orderitemfomat = "<order><Tid>{0}</Tid><Oid>{1}</Oid><GoodsIid>{2}</GoodsIid><Title>{3}</Title><OuterId>{4}</OuterId><SKUContent>{5}</SKUContent><Nums>{6}</Nums><Price>{7}</Price><Payment>{8}</Payment><CostPrice>{9}</CostPrice></order>";
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			string text3 = context.Request.QueryString["action"].ToString();
			string sign = context.Request.Form["sign"];
			string checkCode = masterSettings.CheckCode;
			string text4 = context.Request.Form["format"];
			System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
			System.Collections.Generic.SortedDictionary<string, string> sortedDictionary = new System.Collections.Generic.SortedDictionary<string, string>();
			string a;
			if ((a = text3) != null)
			{
				if (!(a == "tradelist"))
				{
					if (!(a == "tradedetails"))
					{
						if (!(a == "send"))
						{
							if (a == "mark")
							{
								string value = context.Request.Form["order_mark"].Trim();
								string text5 = context.Request.Form["seller_memo"].Trim();
								if (!string.IsNullOrEmpty(context.Request.Form["tid"].Trim()) && !string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(text5))
								{
									if (System.Convert.ToInt32(value) > 0 && System.Convert.ToInt32(value) < 7)
									{
										string text6 = context.Request.Form["tid"].Trim();
										sortedDictionary.Add("tid", text6);
										sortedDictionary.Add("order_mark", value);
										sortedDictionary.Add("seller_memo", text5);
										sortedDictionary.Add("format", text4);
										if (APIHelper.CheckSign(sortedDictionary, checkCode, sign))
										{
											PurchaseOrderInfo purchaseOrder = SalesHelper.GetPurchaseOrder(text6);
											purchaseOrder.ManagerMark = new OrderMark?((OrderMark)System.Enum.Parse(typeof(OrderMark), value, true));
											purchaseOrder.ManagerRemark = Globals.HtmlEncode(text5);
											if (SalesHelper.SaveAPIPurchaseOrderRemark(purchaseOrder))
											{
												stringBuilder.Append("<trade_get_response>");
												stringBuilder.Append(this.GetOrderDetails(format, orderitemfomat, purchaseOrder).ToString());
												stringBuilder.Append("</trade_get_response>");
												this.message = text + stringBuilder.ToString();
											}
											else
											{
												text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Paramter_Error, "save is failure ");
											}
										}
										else
										{
											text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Signature_Error, "sign");
										}
									}
									else
									{
										text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Format_Eroor, "order_mark");
									}
								}
								else
								{
									text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Empty_Error, "tid or order_mark or seller_memo");
								}
							}
						}
						else
						{
							string text7 = context.Request.Form["tid"].Trim();
							string text8 = context.Request.Form["out_sid"].Trim();
							string text9 = context.Request.Form["company_code"].Trim();
							if (!string.IsNullOrEmpty(text7) && !string.IsNullOrEmpty(text9) && !string.IsNullOrEmpty(text8))
							{
								sortedDictionary.Add("tid", text7);
								sortedDictionary.Add("out_sid", text8);
								sortedDictionary.Add("company_code", text9);
								sortedDictionary.Add("format", text4);
								if (APIHelper.CheckSign(sortedDictionary, checkCode, sign))
								{
									ExpressCompanyInfo expressCompanyInfo = ExpressHelper.FindNodeByCode(text9);
									if (!string.IsNullOrEmpty(expressCompanyInfo.Name))
									{
										ShippingModeInfo shippingModeByCompany = SalesHelper.GetShippingModeByCompany(expressCompanyInfo.Name);
										PurchaseOrderInfo purchaseOrder2 = SalesHelper.GetPurchaseOrder(text7);
										if (purchaseOrder2 != null)
										{
											ApiErrorCode apiErrorCode = this.SendOrders(purchaseOrder2, shippingModeByCompany, text8, expressCompanyInfo);
											if (apiErrorCode == ApiErrorCode.Success)
											{
												stringBuilder.Append("<trade_get_response>");
												purchaseOrder2 = SalesHelper.GetPurchaseOrder(text7);
												stringBuilder.Append(this.GetOrderDetails(format, orderitemfomat, purchaseOrder2).ToString());
												stringBuilder.Append("</trade_get_response>");
												this.message = text + stringBuilder.ToString();
											}
											else
											{
												text2 = MessageInfo.ShowMessageInfo(apiErrorCode, "It");
											}
										}
										else
										{
											text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.NoExists_Error, "tid");
										}
									}
									else
									{
										text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.NoExists_Error, "company_code");
									}
								}
								else
								{
									text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Signature_Error, "sign");
								}
							}
							else
							{
								text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Empty_Error, "paramters");
							}
						}
					}
					else
					{
						if (!string.IsNullOrEmpty(context.Request.Form["tid"].Trim()))
						{
							string text6 = context.Request.Form["tid"].Trim();
							if (APIHelper.CheckSign(new System.Collections.Generic.SortedDictionary<string, string>
							{

								{
									"tid",
									context.Request.Form["tid"]
								},

								{
									"format",
									text4
								}
							}, checkCode, sign))
							{
								string text10 = context.Request.Form["tid"].Replace("\r\n", "\n");
								if (!string.IsNullOrEmpty(text10))
								{
									text6 = text10;
									PurchaseOrderInfo purchaseOrder3 = SalesHelper.GetPurchaseOrder(text6);
									stringBuilder.Append("<trade_get_response>");
									stringBuilder.Append(this.GetOrderDetails(format, orderitemfomat, purchaseOrder3).ToString());
									stringBuilder.Append("</trade_get_response>");
									this.message = text + stringBuilder.ToString();
								}
								else
								{
									text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Format_Eroor, "tid");
								}
							}
							else
							{
								text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Signature_Error, "signature");
							}
						}
						else
						{
							text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Empty_Error, "tid");
						}
					}
				}
				else
				{
					PurchaseOrderQuery purchaseOrderQuery = new PurchaseOrderQuery
					{
						PageSize = 100
					};
					int num = 0;
					string value2 = context.Request.Form["status"].Trim();
					string text11 = context.Request.Form["buynick"].Trim();
					string value3 = context.Request.Form["pageindex"].Trim();
					string value4 = context.Request.Form["starttime"].Trim();
					string value5 = context.Request.Form["endtime"].Trim();
					if (!string.IsNullOrEmpty(value2) && System.Convert.ToInt32(value2) >= 0)
					{
						purchaseOrderQuery.PurchaseStatus = (OrderStatus)System.Enum.Parse(typeof(OrderStatus), value2, true);
					}
					else
					{
						text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Empty_Error, "status");
					}
					if (!string.IsNullOrEmpty(value3) && System.Convert.ToInt32(value3) > 0)
					{
						purchaseOrderQuery.PageIndex = System.Convert.ToInt32(value3);
					}
					else
					{
						text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Empty_Error, "pageindex");
					}
					if (string.IsNullOrEmpty(text2))
					{
						sortedDictionary.Add("status", value2);
						sortedDictionary.Add("buynick", text11);
						sortedDictionary.Add("pageindex", value3);
						sortedDictionary.Add("starttime", value4);
						sortedDictionary.Add("endtime", value5);
						sortedDictionary.Add("format", text4);
						if (APIHelper.CheckSign(sortedDictionary, checkCode, sign))
						{
							if (!string.IsNullOrEmpty(text11))
							{
								purchaseOrderQuery.DistributorName = text11;
							}
							if (!string.IsNullOrEmpty(value4))
							{
								purchaseOrderQuery.StartDate = new System.DateTime?(System.Convert.ToDateTime(value4));
							}
							if (!string.IsNullOrEmpty(value5))
							{
								purchaseOrderQuery.EndDate = new System.DateTime?(System.Convert.ToDateTime(value5));
							}
							purchaseOrderQuery.SortOrder = SortAction.Desc;
							purchaseOrderQuery.SortBy = "PurchaseDate";
							stringBuilder.Append("<trade_get_response>");
							stringBuilder.Append(this.GetOrderList(purchaseOrderQuery, format, orderitemfomat, out num));
							stringBuilder.Append("<totalrecord>" + num + "</totalrecord>");
							stringBuilder.Append("</trade_get_response>");
							this.message = text + stringBuilder.ToString();
						}
						else
						{
							text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Signature_Error, "sign");
						}
					}
					else
					{
						text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Empty_Error, "paramter");
					}
				}
			}
			if (this.message == "")
			{
				this.message = this.message + str + text2;
			}
			context.Response.ContentType = "text/xml";
			if (text4 == "json")
			{
				this.message = this.message.Replace(text, "");
				xmlDocument.Load(new System.IO.MemoryStream(System.Text.Encoding.GetEncoding("UTF-8").GetBytes(this.message)));
				this.message = JavaScriptConvert.SerializeXmlNode(xmlDocument);
				context.Response.ContentType = "text/json";
			}
			context.Response.Write(this.message);
		}
		public System.Text.StringBuilder GetOrderList(PurchaseOrderQuery query, string format, string orderitemfomat, out int totalrecords)
		{
			int num = 0;
			Globals.EntityCoding(query, true);
			System.Data.DataSet aPIPurchaseOrders = SalesHelper.GetAPIPurchaseOrders(query, out num);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			foreach (System.Data.DataRow dataRow in aPIPurchaseOrders.Tables[0].Rows)
			{
				string text = "false";
				System.Text.StringBuilder stringBuilder2 = new System.Text.StringBuilder();
				System.Data.DataRow[] childRows = dataRow.GetChildRows("PurchaseOrderRelation");
				for (int i = 0; i < childRows.Length; i++)
				{
					System.Data.DataRow dataRow2 = childRows[i];
					string text2 = dataRow2["SKUContent"].ToString();
					text = "true";
					stringBuilder2.AppendFormat(orderitemfomat, new object[]
					{
						dataRow2["Tid"].ToString(),
						dataRow2["PurchaseOrderId"].ToString(),
						dataRow2["ProductId"].ToString(),
						dataRow2["ItemDescription"].ToString(),
						dataRow2["SKU"].ToString(),
						text2,
						dataRow2["Quantity"].ToString(),
						decimal.Parse(dataRow2["ItemListPrice"].ToString()).ToString("F2"),
						decimal.Parse(dataRow2["ItemPurchasePrice"].ToString()).ToString("F2"),
						decimal.Parse(dataRow2["CostPrice"].ToString()).ToString("F2")
					});
				}
				System.Collections.Generic.Dictionary<string, string> shippingRegion = MessageInfo.GetShippingRegion(dataRow["ShippingRegion"].ToString());
				stringBuilder.AppendFormat(format, new object[]
				{
					dataRow["PurchaseOrderId"].ToString(),
					dataRow["SellerUid"].ToString(),
					dataRow["Username"].ToString(),
					dataRow["EmailAddress"].ToString(),
					dataRow["ShipTo"].ToString(),
					shippingRegion["Province"],
					shippingRegion["City"].ToString(),
					shippingRegion["District"],
					dataRow["Address"].ToString(),
					dataRow["ZipCode"].ToString(),
					dataRow["CellPhone"].ToString(),
					dataRow["TelPhone"].ToString(),
					dataRow["Remark"].ToString(),
					dataRow["ManagerMark"].ToString(),
					dataRow["ManagerRemark"].ToString(),
					dataRow["Nums"].ToString(),
					decimal.Parse(dataRow["OrderTotal"].ToString()).ToString("F2"),
					decimal.Parse(dataRow["OrderTotal"].ToString()).ToString("F2"),
					decimal.Parse(dataRow["AdjustedFreight"].ToString()).ToString("F2"),
					decimal.Parse(dataRow["Profit"].ToString()).ToString("F2"),
					decimal.Parse(dataRow["PurchaseTotal"].ToString()).ToString("F2"),
					dataRow["PayDate"].ToString(),
					dataRow["ShippingDate"].ToString(),
					dataRow["ReFundStatus"].ToString(),
					decimal.Parse(dataRow["RefundAmount"].ToString()).ToString("F2"),
					dataRow["RefundRemark"].ToString(),
					dataRow["OrderStatus"].ToString(),
					text,
					stringBuilder2
				});
			}
			totalrecords = num;
			return stringBuilder;
		}
		public System.Text.StringBuilder GetOrderDetails(string format, string orderitemfomat, PurchaseOrderInfo order)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			string text = string.Empty;
			if (order != null)
			{
				string text2 = "false";
				System.Text.StringBuilder stringBuilder2 = new System.Text.StringBuilder();
				long num = 0L;
				System.Collections.Generic.IList<PurchaseOrderItemInfo> purchaseOrderItems = order.PurchaseOrderItems;
				foreach (PurchaseOrderItemInfo current in purchaseOrderItems)
				{
					text2 = "true";
					stringBuilder2.AppendFormat(orderitemfomat, new object[]
					{
						"0",
						order.OrderId,
						current.ProductId.ToString(),
						current.ItemDescription,
						current.SKU,
						current.SKUContent,
						current.Quantity.ToString(),
						current.ItemListPrice.ToString("F2"),
						current.ItemPurchasePrice.ToString("F2"),
						current.ItemCostPrice.ToString("F2")
					});
					num += (long)current.Quantity;
				}
				System.Collections.Generic.Dictionary<string, string> shippingRegion = MessageInfo.GetShippingRegion(order.ShippingRegion);
				stringBuilder.AppendFormat(format, new object[]
				{
					order.OrderId,
					"0",
					order.Distributorname,
					order.DistributorEmail,
					order.ShipTo,
					shippingRegion["Province"],
					shippingRegion["City"].ToString(),
					shippingRegion["District"],
					order.Address,
					order.ZipCode,
					order.CellPhone,
					order.TelPhone,
					order.Remark,
					order.ManagerMark,
					order.ManagerRemark,
					num.ToString(),
					order.OrderTotal.ToString("F2"),
					order.OrderTotal.ToString("F2"),
					order.AdjustedFreight.ToString("F2"),
					order.GetPurchaseProfit().ToString("F2"),
					order.GetPurchaseTotal().ToString("F2"),
					order.PayDate.ToString(),
					order.ShippingDate.ToString(),
					((int)order.RefundStatus).ToString(),
					order.RefundAmount.ToString("F2"),
					order.RefundRemark,
					((int)order.PurchaseStatus).ToString(),
					text2,
					stringBuilder2
				});
				if (!string.IsNullOrEmpty(order.ShippingRegion))
				{
					text = order.ShippingRegion;
				}
				if (!string.IsNullOrEmpty(order.Address))
				{
					text += order.Address;
				}
				if (!string.IsNullOrEmpty(order.ShipTo))
				{
					text = text + "   " + order.ShipTo;
				}
				if (!string.IsNullOrEmpty(order.ZipCode))
				{
					text = text + "   " + order.ZipCode;
				}
				if (!string.IsNullOrEmpty(order.TelPhone))
				{
					text = text + "   " + order.TelPhone;
				}
				if (!string.IsNullOrEmpty(order.CellPhone))
				{
					text = text + "   " + order.CellPhone;
				}
				string text3 = "<ShipAddress>{0}</ShipAddress><ModeName>{1}</ModeName><ShipOrderNumber>{2}</ShipOrderNumber><ExpressCompanyName>{3}</ExpressCompanyName>";
				text3 = string.Format(text3, new object[]
				{
					text,
					order.RealModeName,
					order.ShipOrderNumber,
					order.ExpressCompanyName
				});
				stringBuilder = stringBuilder.Replace("</Status>", "</Status>" + text3);
			}
			return stringBuilder;
		}
		public ApiErrorCode SendOrders(PurchaseOrderInfo purchaseorder, ShippingModeInfo shippingmode, string out_id, ExpressCompanyInfo express)
		{
			if (string.IsNullOrEmpty(out_id))
			{
				return ApiErrorCode.ShipingOrderNumber_Error;
			}
			if (purchaseorder == null)
			{
				return ApiErrorCode.NoExists_Error;
			}
			if (purchaseorder.PurchaseStatus != OrderStatus.BuyerAlreadyPaid)
			{
				return ApiErrorCode.NoPay_Error;
			}
			if (shippingmode.ModeId <= 0)
			{
				return ApiErrorCode.NoShippingMode;
			}
			if (string.IsNullOrEmpty(express.Name))
			{
				return ApiErrorCode.Empty_Error;
			}
			purchaseorder.RealShippingModeId = shippingmode.ModeId;
			purchaseorder.RealModeName = shippingmode.Name;
			purchaseorder.ExpressCompanyName = express.Name;
			purchaseorder.ExpressCompanyAbb = express.Kuaidi100Code;
			purchaseorder.ShipOrderNumber = out_id;
			if (SalesHelper.SendAPIPurchaseOrderGoods(purchaseorder))
			{
				return ApiErrorCode.Success;
			}
			return ApiErrorCode.Unknown_Error;
		}
	}
}
