using Hidistro.ControlPanel.Commodities;
using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Membership.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Services;
namespace Hidistro.UI.Web.API
{
	[WebService(Namespace = "http://tempuri.org/"), WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
	public class ProductsHandler : System.Web.IHttpHandler
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
			string text3 = context.Request.QueryString["action"].ToString();
			string sign = context.Request.Form["sign"];
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			string skucontentformat = "<Item><lid>{0}</lid><OuterId>{1}</OuterId><SKUContent>{2}</SKUContent><Nums>{3}</Nums><Price>{4}</Price></Item>";
			string strformat = "<product><Iid>{0}</Iid><OuterId>{1}</OuterId><Title>{2}</Title><PicUrl>{3}</PicUrl><Url>{4}</Url><MarketPrice>{5}</MarketPrice><Price>{6}</Price><Weight>{7}</Weight><Status>{8}</Status><SkuItems list=\"{9}\">{10}</SkuItems></product>";
			System.Collections.Generic.SortedDictionary<string, string> sortedDictionary = new System.Collections.Generic.SortedDictionary<string, string>();
			string checkCode = masterSettings.CheckCode;
			string value = context.Request.Form["format"];
			try
			{
				if (!string.IsNullOrEmpty(text3))
				{
					string hostPath = Hidistro.Membership.Context.HiContext.Current.HostPath;
					string a;
					if ((a = text3) != null)
					{
						if (!(a == "productview"))
						{
							if (a == "productsimpleview")
							{
								goto IL_7BC;
							}
							if (!(a == "stockview"))
							{
								if (!(a == "quantity"))
								{
									if (a == "statue")
									{
										string value2 = context.Request.Form["state"].Trim();
										string text4 = context.Request.Form["productId"].Trim();
										if (string.IsNullOrEmpty(value2) || string.IsNullOrEmpty(text4))
										{
											text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Empty_Error, "state or productId");
											goto IL_7BC;
										}
										ProductSaleStatus productSaleStatus = (ProductSaleStatus)System.Enum.Parse(typeof(ProductSaleStatus), value2, true);
										int num = System.Convert.ToInt32(text4);
										if (num <= 0)
										{
											text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Format_Eroor, "productId");
											goto IL_7BC;
										}
										sortedDictionary.Add("productid", text4);
										sortedDictionary.Add("state", value2);
										sortedDictionary.Add("format", value);
										if (APIHelper.CheckSign(sortedDictionary, checkCode, sign))
										{
											bool flag = false;
											stringBuilder.Append("<item_update_statue_response>");
											if (productSaleStatus == ProductSaleStatus.OnSale)
											{
												if (ProductHelper.UpShelf(num) > 0)
												{
													flag = true;
												}
												else
												{
													text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Format_Eroor, "productId");
												}
											}
											else
											{
												if (productSaleStatus == ProductSaleStatus.UnSale)
												{
													if (ProductHelper.OffShelf(num) > 0)
													{
														flag = true;
													}
													else
													{
														text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Format_Eroor, "productId");
													}
												}
											}
											if (flag)
											{
												stringBuilder.Append(string.Concat(new string[]
												{
													"<item><num_iid>",
													text4,
													"</num_iid><modified>",
													System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"),
													"</modified></item>"
												}));
											}
											else
											{
												text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Unknown_Error, "update");
											}
											stringBuilder.Append("</item_update_statue_response>");
											goto IL_7BC;
										}
										text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Signature_Error, "sign");
										goto IL_7BC;
									}
								}
								else
								{
									if (string.IsNullOrEmpty(context.Request.Form["productId"].Trim()) || string.IsNullOrEmpty(context.Request.Form["quantity"].Trim()))
									{
										text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Empty_Error, "paramters");
										goto IL_7BC;
									}
									string text5 = context.Request.Form["productId"];
									string text6 = "";
									string text7 = "";
									int type = 1;
									int stock = System.Convert.ToInt32(context.Request.Form["quantity"].Trim());
									if (!string.IsNullOrEmpty(context.Request.Form["sku_id"].Trim()))
									{
										text6 = context.Request.Form["sku_id"];
									}
									if (!string.IsNullOrEmpty(context.Request.Form["outer_id"].Trim()))
									{
										text7 = context.Request.Form["outer_id"];
									}
									if (!string.IsNullOrEmpty(context.Request.Form["type"]))
									{
										type = System.Convert.ToInt32(context.Request.Form["type"]);
									}
									sortedDictionary.Add("productId", text5.ToString());
									sortedDictionary.Add("quantity", stock.ToString());
									sortedDictionary.Add("sku_id", text6);
									sortedDictionary.Add("outer_id", text7);
									sortedDictionary.Add("type", type.ToString());
									sortedDictionary.Add("format", value);
									if (!APIHelper.CheckSign(sortedDictionary, checkCode, sign))
									{
										text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Signature_Error, "sign");
										goto IL_7BC;
									}
									ApiErrorCode apiErrorCode = ProductHelper.UpdateProductStock(System.Convert.ToInt32(text5), text6, text7, type, stock);
									if (ApiErrorCode.Success == apiErrorCode)
									{
										stringBuilder.Append("<trade_get_response>");
										stringBuilder.Append(this.GetProductDetailsView(System.Convert.ToInt32(text5), hostPath, strformat, skucontentformat).ToString());
										stringBuilder.Append("</trade_get_response>");
										goto IL_7BC;
									}
									text2 = MessageInfo.ShowMessageInfo(apiErrorCode, "paramters");
									goto IL_7BC;
								}
							}
							else
							{
								if (string.IsNullOrEmpty(context.Request.Form["productId"].Trim()) || System.Convert.ToInt32(context.Request.Form["ProductId"].Trim()) <= 0)
								{
									text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Empty_Error, "productId");
									goto IL_7BC;
								}
								int num = System.Convert.ToInt32(context.Request.Form["productId"].Trim());
								sortedDictionary.Add("productid", num.ToString());
								sortedDictionary.Add("format", value);
								if (APIHelper.CheckSign(sortedDictionary, checkCode, sign))
								{
									stringBuilder.Append("<trade_get_response>");
									stringBuilder.Append(this.GetProductDetailsView(num, hostPath, strformat, skucontentformat).ToString());
									stringBuilder.Append("</trade_get_response>");
									goto IL_7BC;
								}
								text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Signature_Error, "sign");
								goto IL_7BC;
							}
						}
						else
						{
							ProductQuery productQuery = new ProductQuery
							{
								SaleStatus = ProductSaleStatus.OnSale,
								PageSize = 10,
								PageIndex = 1
							};
							string value2 = context.Request.Form["state"].Trim();
							string value3 = context.Request.Form["pageindex"].Trim();
							string value4 = context.Request.Form["pagesize"].Trim();
							if (string.IsNullOrEmpty(value2) || string.IsNullOrEmpty(value3))
							{
								text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Empty_Error, "state or pageindex");
								goto IL_7BC;
							}
							sortedDictionary.Add("state", value2);
							sortedDictionary.Add("pageindex", value3);
							sortedDictionary.Add("pagesize", value4);
							sortedDictionary.Add("format", value);
							if (!APIHelper.CheckSign(sortedDictionary, checkCode, sign))
							{
								text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Signature_Error, "sign");
								goto IL_7BC;
							}
							if (!string.IsNullOrEmpty(value4) && System.Convert.ToInt32(value4) > 0)
							{
								productQuery.PageSize = System.Convert.ToInt32(value4);
							}
							if (System.Convert.ToInt32(value3) > 0)
							{
								productQuery.PageIndex = System.Convert.ToInt32(value3);
								productQuery.SaleStatus = (ProductSaleStatus)System.Enum.Parse(typeof(ProductSaleStatus), value2, true);
								Globals.EntityCoding(productQuery, true);
								int num2 = 0;
								stringBuilder.Append("<trade_get_response>");
								stringBuilder.Append(this.GetProductView(productQuery, hostPath, strformat, skucontentformat, out num2).ToString());
								stringBuilder.Append("<totalrecord>" + num2 + "</totalrecord>");
								stringBuilder.Append("</trade_get_response>");
								goto IL_7BC;
							}
							text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Format_Eroor, "pageindex");
							goto IL_7BC;
						}
					}
					text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Paramter_Error, "paramters");
					IL_7BC:
					text += stringBuilder.ToString();
				}
				else
				{
					text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Empty_Error, "modeId");
				}
			}
			catch (System.Exception ex)
			{
				text2 = MessageInfo.ShowMessageInfo(ApiErrorCode.Unknown_Error, ex.Message);
			}
			if (text2 != "")
			{
				text = str + text2;
			}
			context.Response.ContentType = "text/xml";
			context.Response.Write(text);
		}
		public System.Text.StringBuilder GetProductView(ProductQuery query, string localhost, string strformat, string skucontentformat, out int recordes)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			int num = 0;
			System.Data.DataSet productsByQuery = ProductHelper.GetProductsByQuery(query, out num);
			System.Text.StringBuilder stringBuilder2 = new System.Text.StringBuilder();
			foreach (System.Data.DataRow dataRow in productsByQuery.Tables[0].Rows)
			{
				string text = "false";
				System.Data.DataRow[] childRows = dataRow.GetChildRows("ProductRealation");
				System.Data.DataRow[] array = childRows;
				for (int i = 0; i < array.Length; i++)
				{
					System.Data.DataRow dataRow2 = array[i];
					text = "true";
					string skuContent = MessageInfo.GetSkuContent(dataRow2["SkuId"].ToString());
					stringBuilder2.AppendFormat(skucontentformat, new object[]
					{
						dataRow2["ProductId"].ToString(),
						dataRow2["SKuId"].ToString(),
						skuContent,
						dataRow2["Stock"].ToString(),
						dataRow2["SalePrice"].ToString()
					});
				}
				string text2 = localhost + Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[]
				{
					dataRow["ProductId"].ToString()
				});
				stringBuilder.AppendFormat(strformat, new object[]
				{
					dataRow["ProductId"].ToString(),
					dataRow["ProductCode"].ToString(),
					dataRow["ProductName"].ToString(),
					localhost + dataRow["ThumbnailUrl60"].ToString(),
					text2,
					dataRow["MarketPrice"].ToString(),
					dataRow["SalePrice"].ToString(),
					dataRow["Weight"].ToString(),
					dataRow["SaleStatus"].ToString(),
					text,
					stringBuilder2
				});
			}
			recordes = num;
			return stringBuilder;
		}
		public System.Text.StringBuilder GetProductDetailsView(int pid, string localhost, string strformat, string skucontentformat)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			string text = "false";
			string text2 = "";
			System.Data.DataSet productSkuDetials = ProductHelper.GetProductSkuDetials(pid);
			foreach (System.Data.DataRow dataRow in productSkuDetials.Tables[1].Rows)
			{
				text = "true";
				string skuContent = MessageInfo.GetSkuContent(dataRow["SkuId"].ToString());
				text2 += string.Format(skucontentformat, new object[]
				{
					dataRow["ProductId"].ToString(),
					dataRow["SKU"],
					skuContent,
					dataRow["Stock"],
					dataRow["SalePrice"]
				});
			}
			foreach (System.Data.DataRow dataRow2 in productSkuDetials.Tables[0].Rows)
			{
				string text3 = localhost + Globals.GetSiteUrls().UrlData.FormatUrl("productDetails", new object[]
				{
					dataRow2["ProductId"].ToString()
				});
				stringBuilder.AppendFormat(strformat, new object[]
				{
					dataRow2["ProductId"].ToString(),
					dataRow2["ProductCode"].ToString(),
					dataRow2["ProductName"].ToString(),
					localhost + dataRow2["ThumbnailUrl60"].ToString(),
					text3,
					dataRow2["MarketPrice"].ToString(),
					dataRow2["SalePrice"].ToString(),
					dataRow2["Weight"].ToString(),
					dataRow2["SaleStatus"].ToString(),
					text,
					text2
				});
			}
			return stringBuilder;
		}
	}
}
