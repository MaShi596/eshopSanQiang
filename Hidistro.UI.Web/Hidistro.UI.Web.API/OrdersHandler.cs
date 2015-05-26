using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Membership.Core;
using Hidistro.Messages;
using Hishop.Plugins;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Services;
namespace Hidistro.UI.Web.API
{
	[WebService(Namespace = "http://tempuri.org/"), WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class OrdersHandler : System.Web.IHttpHandler
	{
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
			string format = "<trade><Oid>{0}</Oid><SellerUid>{1}</SellerUid><BuyerNick>{2}</BuyerNick><BuyerEmail>{3}</BuyerEmail><ReceiverName>{4}</ReceiverName><ReceiverState>{5}</ReceiverState><ReceiverCity>{6}</ReceiverCity><ReceiverDistrict>{7}</ReceiverDistrict><ReceiverAddress>{8}</ReceiverAddress><ReceiverZip>{9}</ReceiverZip><ReceiverMobile>{10}</ReceiverMobile><ReceiverPhone>{11}</ReceiverPhone><BuyerMemo>{12}</BuyerMemo><OrderMark>{13}</OrderMark><SellerMemo>{14}</SellerMemo><Nums>{15}</Nums><Price>{16}</Price><Payment>{17}</Payment><PostFee>{18}</PostFee><DiscountFee>{19}</DiscountFee><AdjustFee>{20}</AdjustFee><PaymentTs>{21}</PaymentTs><SentTs>{22}</SentTs><RefundStatus>{23}</RefundStatus><RefundAmount>{24}</RefundAmount><RefundRemark>{25}</RefundRemark><Status>{26}</Status><orders list=\"{27}\">{28}</orders></trade>";
			string orderitemfomat = "<order><Tid>{0}</Tid><Oid>{1}</Oid><GoodsIid>{2}</GoodsIid><Title>{3}</Title><OuterId>{4}</OuterId><SKUContent>{5}</SKUContent><Nums>{6}</Nums><Price>{7}</Price><Payment>{8}</Payment></order>";
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			string text3 = context.Request.QueryString["action"].ToString();
			string sign = context.Request.Form["sign"];
			string checkCode = masterSettings.CheckCode;
			string value = context.Request.Form["format"];
			new System.Collections.Generic.Dictionary<string, string>();
			System.Collections.Generic.SortedDictionary<string, string> sortedDictionary = new System.Collections.Generic.SortedDictionary<string, string>();
			try
			{
				if (!string.IsNullOrEmpty(text3))
				{
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
										string value2 = context.Request.Form["order_mark"].Trim();
										string text4 = context.Request.Form["seller_memo"].Trim();
										if (string.IsNullOrEmpty(context.Request.Form["tid"].Trim()) || string.IsNullOrEmpty(value2) || string.IsNullOrEmpty(text4))
										{
											text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Empty_Error, "tid or order_mark or seller_memo");
											goto IL_7CA;
										}
										if (System.Convert.ToInt32(value2) <= 0 || System.Convert.ToInt32(value2) >= 7)
										{
											text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Format_Eroor, "order_mark");
											goto IL_7CA;
										}
										string text5 = context.Request.Form["tid"].Trim();
										sortedDictionary.Add("tid", text5);
										sortedDictionary.Add("order_mark", value2);
										sortedDictionary.Add("seller_memo", text4);
										sortedDictionary.Add("format", value);
										if (!APIHelper.CheckSign(sortedDictionary, checkCode, sign))
										{
											text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Signature_Error, "sign");
											goto IL_7CA;
										}
										OrderInfo orderInfo = OrderHelper.GetOrderInfo(text5);
										orderInfo.ManagerMark = new OrderMark?((OrderMark)System.Enum.Parse(typeof(OrderMark), value2, true));
										orderInfo.ManagerRemark = Globals.HtmlEncode(text4);
										if (OrderHelper.SaveRemarkAPI(orderInfo))
										{
											stringBuilder.Append("<trade_get_response>");
											stringBuilder.Append(this.GetOrderDetails(format, orderitemfomat, orderInfo).ToString());
											stringBuilder.Append("</trade_get_response>");
											goto IL_7CA;
										}
										text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Paramter_Error, "save is failure ");
										goto IL_7CA;
									}
								}
								else
								{
									string text6 = context.Request.Form["tid"].Trim();
									string text7 = context.Request.Form["out_sid"].Trim();
									string text8 = context.Request.Form["company_code"].Trim();
									if (string.IsNullOrEmpty(text6) || string.IsNullOrEmpty(text8) || string.IsNullOrEmpty(text7))
									{
										text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Empty_Error, "paramters");
										goto IL_7CA;
									}
									sortedDictionary.Add("tid", text6);
									sortedDictionary.Add("out_sid", text7);
									sortedDictionary.Add("company_code", text8);
									sortedDictionary.Add("format", value);
									if (!APIHelper.CheckSign(sortedDictionary, checkCode, sign))
									{
										text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Signature_Error, "sign");
										goto IL_7CA;
									}
									ExpressCompanyInfo expressCompanyInfo = ExpressHelper.FindNodeByCode(text8);
									if (string.IsNullOrEmpty(expressCompanyInfo.Name))
									{
										text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.NoExists_Error, "company_code");
										goto IL_7CA;
									}
									ShippingModeInfo shippingModeByCompany = SalesHelper.GetShippingModeByCompany(expressCompanyInfo.Name);
									OrderInfo orderInfo2 = OrderHelper.GetOrderInfo(text6);
									if (orderInfo2 == null)
									{
										text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.NoExists_Error, "tid");
										goto IL_7CA;
									}
									ApiErrorCode apiErrorCode = this.SendOrders(orderInfo2, shippingModeByCompany, text7, expressCompanyInfo);
									if (apiErrorCode == ApiErrorCode.Success)
									{
										stringBuilder.Append("<trade_get_response>");
										orderInfo2 = OrderHelper.GetOrderInfo(text6);
										stringBuilder.Append(this.GetOrderDetails(format, orderitemfomat, orderInfo2).ToString());
										stringBuilder.Append("</trade_get_response>");
										goto IL_7CA;
									}
									text2 = MessageInfo.ShowMessageInfo(apiErrorCode, "It");
									goto IL_7CA;
								}
							}
							else
							{
								if (string.IsNullOrEmpty(context.Request.Form["tid"].Trim()))
								{
									text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Empty_Error, "tid");
									goto IL_7CA;
								}
								string text5 = context.Request.Form["tid"].Trim();
								if (!APIHelper.CheckSign(new System.Collections.Generic.SortedDictionary<string, string>
								{

									{
										"tid",
										context.Request.Form["tid"]
									},

									{
										"format",
										value
									}
								}, checkCode, sign))
								{
									text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Signature_Error, "signature");
									goto IL_7CA;
								}
								string text9 = context.Request.Form["tid"].Replace("\r\n", "\n");
								if (!string.IsNullOrEmpty(text9))
								{
									text5 = text9;
									OrderInfo orderInfo3 = OrderHelper.GetOrderInfo(text5);
									stringBuilder.Append("<trade_get_response>");
									stringBuilder.Append(this.GetOrderDetails(format, orderitemfomat, orderInfo3).ToString());
									stringBuilder.Append("</trade_get_response>");
									goto IL_7CA;
								}
								text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Format_Eroor, "tid");
								goto IL_7CA;
							}
						}
						else
						{
							OrderQuery orderQuery = new OrderQuery
							{
								PageSize = 100
							};
							int num = 0;
							string value3 = context.Request.Form["status"].Trim();
							string text10 = context.Request.Form["buyernick"].Trim();
							string value4 = context.Request.Form["pageindex"].Trim();
							string value5 = context.Request.Form["starttime"].Trim();
							string value6 = context.Request.Form["endtime"].Trim();
							if (!string.IsNullOrEmpty(value3) && System.Convert.ToInt32(value3) >= 0)
							{
								orderQuery.Status = (OrderStatus)System.Enum.Parse(typeof(OrderStatus), value3, true);
							}
							else
							{
								text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Empty_Error, "status");
							}
							if (!string.IsNullOrEmpty(value4) && System.Convert.ToInt32(value4) > 0)
							{
								orderQuery.PageIndex = System.Convert.ToInt32(value4);
							}
							else
							{
								text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Empty_Error, "pageindex");
							}
							if (!string.IsNullOrEmpty(text2))
							{
								text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Empty_Error, "paramter");
								goto IL_7CA;
							}
							sortedDictionary.Add("status", value3);
							sortedDictionary.Add("buyernick", text10);
							sortedDictionary.Add("pageindex", value4);
							sortedDictionary.Add("starttime", value5);
							sortedDictionary.Add("endtime", value6);
							sortedDictionary.Add("format", value);
							if (APIHelper.CheckSign(sortedDictionary, checkCode, sign))
							{
								if (!string.IsNullOrEmpty(text10))
								{
									orderQuery.UserName = text10;
								}
								if (!string.IsNullOrEmpty(value5))
								{
									orderQuery.StartDate = new System.DateTime?(System.Convert.ToDateTime(value5));
								}
								if (!string.IsNullOrEmpty(value6))
								{
									orderQuery.EndDate = new System.DateTime?(System.Convert.ToDateTime(value6));
								}
								stringBuilder.Append("<trade_get_response>");
								stringBuilder.Append(this.GetOrderList(orderQuery, format, orderitemfomat, out num).ToString());
								stringBuilder.Append("<totalrecord>" + num + "</totalrecord>");
								stringBuilder.Append("</trade_get_response>");
								goto IL_7CA;
							}
							text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Signature_Error, "sign");
							goto IL_7CA;
						}
					}
					text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Paramter_Error, "paramters");
					IL_7CA:
					text += stringBuilder.ToString();
				}
				else
				{
					text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Paramter_Error, "sign");
				}
			}
			catch (System.Exception ex)
			{
				text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Unknown_Error, ex.Message);
			}
			if (!string.IsNullOrEmpty(text2))
			{
				text = str + text2;
			}
			context.Response.ContentType = "text/xml";
			context.Response.Write(text);
		}
		public System.Text.StringBuilder GetOrderList(OrderQuery query, string format, string orderitemfomat, out int totalrecords)
		{
			int num = 0;
			Globals.EntityCoding(query, true);
			System.Data.DataSet tradeOrders = OrderHelper.GetTradeOrders(query, out num);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			foreach (System.Data.DataRow dataRow in tradeOrders.Tables[0].Rows)
			{
				string text = "false";
				System.Text.StringBuilder stringBuilder2 = new System.Text.StringBuilder();
				System.Data.DataRow[] childRows = dataRow.GetChildRows("OrderRelation");
				for (int i = 0; i < childRows.Length; i++)
				{
					System.Data.DataRow dataRow2 = childRows[i];
					string text2 = dataRow2["SKUContent"].ToString();
					text = "true";
					stringBuilder2.AppendFormat(orderitemfomat, new object[]
					{
						dataRow2["Tid"].ToString(),
						dataRow2["OrderId"].ToString(),
						dataRow2["ProductId"].ToString(),
						dataRow2["ItemDescription"].ToString(),
						dataRow2["SKU"].ToString(),
						text2,
						dataRow2["Quantity"].ToString(),
						decimal.Parse(dataRow2["ItemListPrice"].ToString()).ToString("F2"),
						decimal.Parse(dataRow2["ItemAdjustedPrice"].ToString()).ToString("F2")
					});
				}
				System.Collections.Generic.Dictionary<string, string> shippingRegion = MessageInfo.GetShippingRegion(dataRow["ShippingRegion"].ToString());
				stringBuilder.AppendFormat(format, new object[]
				{
					dataRow["OrderId"].ToString(),
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
					decimal.Parse(dataRow["DiscountValue"].ToString()).ToString("F2"),
					decimal.Parse(dataRow["AdjustedDiscount"].ToString()).ToString("F2"),
					dataRow["PayDate"].ToString(),
					dataRow["ShippingDate"].ToString(),
					dataRow["ReFundStatus"].ToString(),
					dataRow["RefundAmount"].ToString(),
					dataRow["RefundRemark"].ToString(),
					dataRow["OrderStatus"].ToString(),
					text,
					stringBuilder2
				});
			}
			totalrecords = num;
			return stringBuilder;
		}
		public System.Text.StringBuilder GetOrderDetails(string format, string orderitemfomat, OrderInfo order)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			string text = string.Empty;
			if (order != null)
			{
				string text2 = "false";
				System.Text.StringBuilder stringBuilder2 = new System.Text.StringBuilder();
				long num = 0L;
				System.Collections.Generic.Dictionary<string, LineItemInfo> lineItems = order.LineItems;
				foreach (LineItemInfo current in lineItems.Values)
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
						current.ItemListPrice.ToString(),
						current.ItemAdjustedPrice.ToString()
					});
					num += (long)current.Quantity;
				}
				System.Collections.Generic.Dictionary<string, string> shippingRegion = MessageInfo.GetShippingRegion(order.ShippingRegion);
				stringBuilder.AppendFormat(format, new object[]
				{
					order.OrderId,
					"0",
					order.Username,
					order.EmailAddress,
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
					order.GetTotal().ToString(),
					order.GetTotal().ToString(),
					order.AdjustedFreight.ToString(),
					order.ReducedPromotionAmount.ToString(),
					order.AdjustedDiscount.ToString(),
					order.PayDate.ToString(),
					order.ShippingDate.ToString(),
					((int)order.RefundStatus).ToString(),
					order.RefundAmount.ToString(),
					order.RefundRemark,
					((int)order.OrderStatus).ToString(),
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
		public ApiErrorCode SendOrders(OrderInfo order, ShippingModeInfo shippingmode, string out_id, ExpressCompanyInfo express)
		{
			if (order.GroupBuyId > 0 && order.GroupBuyStatus != GroupBuyStatus.Success)
			{
				return ApiErrorCode.Group_Error;
			}
			if (!order.CheckAction(OrderActions.SELLER_SEND_GOODS))
			{
				return ApiErrorCode.NoPay_Error;
			}
			if (shippingmode.ModeId <= 0)
			{
				return ApiErrorCode.NoShippingMode;
			}
			if (string.IsNullOrEmpty(out_id) || out_id.Length > 20)
			{
				return ApiErrorCode.ShipingOrderNumber_Error;
			}
			order.RealShippingModeId = shippingmode.ModeId;
			order.RealModeName = shippingmode.Name;
			order.ExpressCompanyName = express.Name;
			order.ExpressCompanyAbb = express.Kuaidi100Code;
			order.ShipOrderNumber = out_id;
			if (OrderHelper.SendAPIGoods(order))
			{
				if (!string.IsNullOrEmpty(order.GatewayOrderId) && order.GatewayOrderId.Trim().Length > 0)
				{
					PaymentModeInfo paymentMode = SalesHelper.GetPaymentMode(order.PaymentTypeId);
					if (paymentMode != null)
					{
						PaymentRequest paymentRequest = PaymentRequest.CreateInstance(paymentMode.Gateway, HiCryptographer.Decrypt(paymentMode.Settings), order.OrderId, order.GetTotal(), "订单发货", "订单号-" + order.OrderId, order.EmailAddress, order.OrderDate, Globals.FullPath(Globals.GetSiteUrls().Home), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentReturn_url", new object[]
						{
							paymentMode.Gateway
						})), Globals.FullPath(Globals.GetSiteUrls().UrlData.FormatUrl("PaymentNotify_url", new object[]
						{
							paymentMode.Gateway
						})), "");
						paymentRequest.SendGoods(order.GatewayOrderId, order.RealModeName, order.ShipOrderNumber, "EXPRESS");
					}
				}
				int num = order.UserId;
				if (num == 1100)
				{
					num = 0;
				}
				Hidistro.Membership.Core.IUser user = Hidistro.Membership.Context.Users.GetUser(num);
				Messenger.OrderShipping(order, user);
				order.OnDeliver();
				return ApiErrorCode.Success;
			}
			return ApiErrorCode.Unknown_Error;
		}
	}
}
