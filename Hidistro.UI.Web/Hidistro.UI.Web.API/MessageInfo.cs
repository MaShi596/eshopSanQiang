using Hidistro.Entities.Commodities;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
namespace Hidistro.UI.Web.API
{
	public class MessageInfo
	{
		public static string ShowMessageInfo(ApiErrorCode messageenum, string field)
		{
			string text = "<error_response><code>{0}</code><msg>" + field + " {1}</msg></error_response>";
			switch (messageenum)
			{
			case ApiErrorCode.Paramter_Error:
				text = string.Format(text, 101, "is error");
				break;
			case ApiErrorCode.Format_Eroor:
				text = string.Format(text, 102, "format is error");
				break;
			case ApiErrorCode.Signature_Error:
				text = string.Format(text, 103, "signature is error");
				break;
			case ApiErrorCode.Empty_Error:
				text = string.Format(text, 104, "is empty");
				break;
			case ApiErrorCode.NoExists_Error:
				text = string.Format(text, 105, "is not exists");
				break;
			case ApiErrorCode.Exists_Error:
				text = string.Format(text, 105, "is exists");
				break;
			case ApiErrorCode.Paramter_Diffrent:
				text = string.Format(text, 107, "is diffrent");
				break;
			case ApiErrorCode.Group_Error:
				text = string.Format(text, 108, "is not the end grouporder");
				break;
			case ApiErrorCode.NoPay_Error:
				text = string.Format(text, 109, "is not pay money");
				break;
			case ApiErrorCode.NoShippingMode:
				text = string.Format(text, 110, "is not shippingmodel");
				break;
			case ApiErrorCode.ShipingOrderNumber_Error:
				text = string.Format(text, 111, "is shippingordernumer error");
				break;
			default:
				switch (messageenum)
				{
				case ApiErrorCode.Session_Empty:
					text = string.Format(text, 200, "sessionId is no exist");
					break;
				case ApiErrorCode.Session_Error:
					text = string.Format(text, 201, "sessionId is no Invalid");
					break;
				case ApiErrorCode.Session_TimeOut:
					text = string.Format(text, 202, "is timeout");
					break;
				case ApiErrorCode.Username_Exist:
					text = string.Format(text, 203, "account is Exist");
					break;
				case ApiErrorCode.Ban_Register:
					text = string.Format(text, 204, "Prohibition on registration");
					break;
				default:
					switch (messageenum)
					{
					case ApiErrorCode.SaleState_Error:
						text = string.Format(text, 300, "cant not buy product");
						break;
					case ApiErrorCode.Stock_Error:
						text = string.Format(text, 301, "stock is lack");
						break;
					default:
						text = string.Format(text, 999, "unknown_Error");
						break;
					}
					break;
				}
				break;
			}
			return text;
		}
		public static string GetSkuContent(string skuId)
		{
			string text = "";
			if (!string.IsNullOrEmpty(skuId.Trim()))
			{
				System.Data.DataTable skuContentBySku = ControlProvider.Instance().GetSkuContentBySku(skuId);
				foreach (System.Data.DataRow dataRow in skuContentBySku.Rows)
				{
					if (!string.IsNullOrEmpty(dataRow["AttributeName"].ToString()) && !string.IsNullOrEmpty(dataRow["ValueStr"].ToString()))
					{
						object obj = text;
						text = string.Concat(new object[]
						{
							obj,
							dataRow["AttributeName"],
							":",
							dataRow["ValueStr"],
							"; "
						});
					}
				}
			}
			if (!(text == ""))
			{
				return text;
			}
			return "不存在";
		}
		public static string GetOrderSkuContent(string skucontent)
		{
			string text = "";
			skucontent = skucontent.Replace("；", "");
			if (!string.IsNullOrEmpty(skucontent) && skucontent.IndexOf("：") >= 0)
			{
				text = skucontent.Substring(0, skucontent.IndexOf("："));
			}
			if (!(text == ""))
			{
				return text;
			}
			return "不存在";
		}
		public static System.Collections.Generic.Dictionary<string, string> GetShippingRegion(string regionstr)
		{
			System.Collections.Generic.Dictionary<string, string> dictionary = new System.Collections.Generic.Dictionary<string, string>
			{

				{
					"Province",
					""
				},

				{
					"City",
					""
				},

				{
					"District",
					""
				}
			};
			string[] array = regionstr.Split(new char[]
			{
				'，'
			});
			if (array.Length >= 1)
			{
				dictionary["Province"] = array[0].ToString();
			}
			if (array.Length >= 2)
			{
				dictionary["City"] = array[1].ToString();
			}
			if (array.Length >= 3)
			{
				dictionary["District"] = array[2].ToString();
			}
			return dictionary;
		}
	}
}
