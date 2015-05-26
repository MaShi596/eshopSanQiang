using Hidistro.AccountCenter.Profile;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Promotions;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.SaleSystem.Member;
using Hidistro.SaleSystem.Shopping;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class SubmmitOrderHandler : System.Web.IHttpHandler
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
			try
			{
				string text = context.Request["Action"];
				string text2 = text;
				switch (text2)
				{
				case "GetUserShippingAddress":
					this.GetUserShippingAddress(context);
					break;
				case "CalculateFreight":
					this.CalculateFreight(context);
					break;
				case "ProcessorPaymentMode":
					this.ProcessorPaymentMode(context);
					break;
				case "ProcessorUseCoupon":
					this.ProcessorUseCoupon(context);
					break;
				case "GetRegionId":
					this.GetUserRegionId(context);
					break;
				case "AddShippingAddress":
					this.AddUserShippingAddress(context);
					break;
				case "UpdateShippingAddress":
					this.UpdateShippingAddress(context);
					break;
				}
			}
			catch
			{
			}
		}
		private void GetUserRegionId(System.Web.HttpContext context)
		{
			string text = context.Request["Prov"];
			string text2 = context.Request["City"];
			string text3 = context.Request["Areas"];
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("{");
			if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2) && !string.IsNullOrEmpty(text3))
			{
				stringBuilder.Append("\"Status\":\"OK\",\"RegionId\":\"" + RegionHelper.GetRegionId(text3, text2, text) + "\"}");
			}
			else
			{
				stringBuilder.Append("\"Status\":\"NOK\"}");
			}
			context.Response.ContentType = "text/plain";
			context.Response.Write(stringBuilder);
		}
		private void GetUserShippingAddress(System.Web.HttpContext context)
		{
			ShippingAddressInfo shippingAddress = MemberProcessor.GetShippingAddress(int.Parse(context.Request["ShippingId"], System.Globalization.NumberStyles.None));
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("{");
			if (shippingAddress != null)
			{
				stringBuilder.Append("\"Status\":\"OK\",");
				stringBuilder.AppendFormat("\"ShipTo\":\"{0}\",", Globals.HtmlDecode(shippingAddress.ShipTo));
				stringBuilder.AppendFormat("\"Address\":\"{0}\",", Globals.HtmlDecode(shippingAddress.Address));
				stringBuilder.AppendFormat("\"Zipcode\":\"{0}\",", Globals.HtmlDecode(shippingAddress.Zipcode));
				stringBuilder.AppendFormat("\"CellPhone\":\"{0}\",", Globals.HtmlDecode(shippingAddress.CellPhone));
				stringBuilder.AppendFormat("\"TelPhone\":\"{0}\",", Globals.HtmlDecode(shippingAddress.TelPhone));
				stringBuilder.AppendFormat("\"RegionId\":\"{0}\"", shippingAddress.RegionId);
			}
			else
			{
				stringBuilder.Append("\"Status\":\"0\"");
			}
			stringBuilder.Append("}");
			context.Response.ContentType = "text/plain";
			context.Response.Write(stringBuilder);
		}
		private void CalculateFreight(System.Web.HttpContext context)
		{
			decimal money = 0m;
			if (!string.IsNullOrEmpty(context.Request.Params["ModeId"]) && !string.IsNullOrEmpty(context.Request["RegionId"]))
			{
				int modeId = int.Parse(context.Request["ModeId"], System.Globalization.NumberStyles.None);
				int value = int.Parse(context.Request["Weight"], System.Globalization.NumberStyles.None);
				int regionId = int.Parse(context.Request["RegionId"], System.Globalization.NumberStyles.None);
				ShippingModeInfo shippingMode = ShoppingProcessor.GetShippingMode(modeId, true);
				money = ShoppingProcessor.CalcFreight(regionId, value, shippingMode);
			}
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("{");
			stringBuilder.Append("\"Status\":\"OK\",");
			stringBuilder.AppendFormat("\"Price\":\"{0}\"", Globals.FormatMoney(money));
			stringBuilder.Append("}");
			context.Response.ContentType = "text/plain";
			context.Response.Write(stringBuilder.ToString());
		}
		private void ProcessorPaymentMode(System.Web.HttpContext context)
		{
			decimal money = 0m;
			if (!string.IsNullOrEmpty(context.Request.Params["ModeId"]))
			{
				int modeId = int.Parse(context.Request["ModeId"], System.Globalization.NumberStyles.None);
				decimal cartMoney = decimal.Parse(context.Request["TotalPrice"]);
				PaymentModeInfo paymentMode = ShoppingProcessor.GetPaymentMode(modeId);
				money = paymentMode.CalcPayCharge(cartMoney);
			}
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.Append("{");
			stringBuilder.Append("\"Status\":\"OK\",");
			stringBuilder.AppendFormat("\"Charge\":\"{0}\"", Globals.FormatMoney(money));
			stringBuilder.Append("}");
			context.Response.ContentType = "text/plain";
			context.Response.Write(stringBuilder.ToString());
		}
		private void ProcessorUseCoupon(System.Web.HttpContext context)
		{
			decimal orderAmount = decimal.Parse(context.Request["CartTotal"]);
			string claimCode = context.Request["CouponCode"];
			CouponInfo couponInfo = ShoppingProcessor.UseCoupon(orderAmount, claimCode);
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			if (couponInfo != null)
			{
				stringBuilder.Append("{");
				stringBuilder.Append("\"Status\":\"OK\",");
				stringBuilder.AppendFormat("\"CouponName\":\"{0}\",", couponInfo.Name);
				stringBuilder.AppendFormat("\"DiscountValue\":\"{0}\"", Globals.FormatMoney(couponInfo.DiscountValue));
				stringBuilder.Append("}");
			}
			else
			{
				stringBuilder.Append("{");
				stringBuilder.Append("\"Status\":\"ERROR\"");
				stringBuilder.Append("}");
			}
			context.Response.ContentType = "application/json";
			context.Response.Write(stringBuilder);
		}
		private void AddUserShippingAddress(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string str = "";
			if (this.ValShippingAddress(context, ref str))
			{
				ShippingAddressInfo shippingAddressInfo = this.GetShippingAddressInfo(context);
				if (PersonalHelper.CreateShippingAddress(shippingAddressInfo))
				{
					System.Collections.Generic.IList<ShippingAddressInfo> shippingAddress = PersonalHelper.GetShippingAddress(Hidistro.Membership.Context.HiContext.Current.User.UserId);
					shippingAddressInfo = shippingAddress[shippingAddress.Count - 1];
					context.Response.Write(string.Concat(new object[]
					{
						"{\"Status\":\"OK\",\"Result\":{\"ShipTo\":\"",
						shippingAddressInfo.ShipTo,
						"\",\"RegionId\":\"",
						RegionHelper.GetFullRegion(shippingAddressInfo.RegionId, " "),
						"\",\"ShippingAddress\":\"",
						shippingAddressInfo.Address,
						"\",\"ShippingId\":\"",
						shippingAddressInfo.ShippingId,
						"\",\"CellPhone\":\"",
						shippingAddressInfo.CellPhone,
						"\"}}"
					}));
				}
				else
				{
					context.Response.Write("{\"Status\":\"Error\",\"Result\":\"地址已经在，请重新输入一次再试\"}");
				}
			}
			else
			{
				context.Response.Write("{\"Status\":\"Error\",\"Result\":\"" + str + "\"}");
			}
		}
		private void UpdateShippingAddress(System.Web.HttpContext context)
		{
			context.Response.ContentType = "application/json";
			string str = "";
			str = "请选择要修改的收货地址";
			if (this.ValShippingAddress(context, ref str) || string.IsNullOrEmpty(context.Request.Params["ShippingId"]) || System.Convert.ToInt32(context.Request.Params["ShippingId"]) > 0)
			{
				ShippingAddressInfo shippingAddressInfo = this.GetShippingAddressInfo(context);
				shippingAddressInfo.ShippingId = System.Convert.ToInt32(context.Request.Params["ShippingId"]);
				if (PersonalHelper.UpdateShippingAddress(shippingAddressInfo))
				{
					context.Response.Write(string.Concat(new object[]
					{
						"{\"Status\":\"OK\",\"Result\":{\"ShipTo\":\"",
						shippingAddressInfo.ShipTo,
						"\",\"RegionId\":\"",
						RegionHelper.GetFullRegion(shippingAddressInfo.RegionId, " "),
						"\",\"ShippingAddress\":\"",
						shippingAddressInfo.Address,
						"\",\"ShippingId\":\"",
						shippingAddressInfo.ShippingId,
						"\",\"CellPhone\":\"",
						shippingAddressInfo.CellPhone,
						"\"}}"
					}));
				}
				else
				{
					context.Response.Write("{\"Status\":\"Error\",\"Result\":\"地址已经在，请重新输入一次再试\"}");
				}
			}
			else
			{
				context.Response.Write("{\"Status\":\"Error\",\"Result\":\"" + str + "\"}");
			}
		}
		private bool ValShippingAddress(System.Web.HttpContext context, ref string erromsg)
		{
			System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("[\\u4e00-\\u9fa5a-zA-Z]+[\\u4e00-\\u9fa5_a-zA-Z0-9]*");
			bool result;
			if (string.IsNullOrEmpty(context.Request.Params["ShippingTo"].Trim()) || !regex.IsMatch(context.Request.Params["ShippingTo"].Trim()))
			{
				erromsg = "收货人名字不能为空，只能是汉字或字母开头，长度在2-20个字符之间";
				result = false;
			}
			else
			{
				if (string.IsNullOrEmpty(context.Request.Params["AddressDetails"].Trim()))
				{
					erromsg = "详细地址不能为空";
					result = false;
				}
				else
				{
					if (context.Request.Params["AddressDetails"].Trim().Length < 3 || context.Request.Params["AddressDetails"].Trim().Length > 60)
					{
						erromsg = "详细地址长度在3-60个字符之间";
						result = false;
					}
					else
					{
						if (string.IsNullOrEmpty(context.Request.Params["RegionId"].Trim()) || System.Convert.ToInt32(context.Request.Params["RegionId"].Trim()) <= 0)
						{
							erromsg = "请选择收货地址";
							result = false;
						}
						else
						{
							if (string.IsNullOrEmpty(context.Request.Params["TelPhone"].Trim()) && string.IsNullOrEmpty(context.Request.Params["CellHphone"].Trim().Trim()))
							{
								erromsg = "电话号码和手机二者必填其一";
								result = false;
							}
							else
							{
								if (!string.IsNullOrEmpty(context.Request.Params["TelPhone"].Trim()) && (context.Request.Params["TelPhone"].Trim().Length < 3 || context.Request.Params["TelPhone"].Trim().Length > 20))
								{
									erromsg = "电话号码长度限制在3-20个字符之间";
									result = false;
								}
								else
								{
									if (!string.IsNullOrEmpty(context.Request.Params["CellHphone"].Trim()) && (context.Request.Params["CellHphone"].Trim().Length < 3 || context.Request.Params["CellHphone"].Trim().Length > 20))
									{
										erromsg = "手机号码长度限制在3-20个字符之间";
										result = false;
									}
									else
									{
										if (PersonalHelper.GetShippingAddressCount(Hidistro.Membership.Context.HiContext.Current.User.UserId) >= Hidistro.Membership.Context.HiContext.Current.Config.ShippingAddressQuantity)
										{
											erromsg = string.Format("最多只能添加{0}个收货地址", Hidistro.Membership.Context.HiContext.Current.Config.ShippingAddressQuantity);
											result = false;
										}
										else
										{
											result = true;
										}
									}
								}
							}
						}
					}
				}
			}
			return result;
		}
		private ShippingAddressInfo GetShippingAddressInfo(System.Web.HttpContext context)
		{
			return new ShippingAddressInfo
			{
				UserId = Hidistro.Membership.Context.HiContext.Current.User.UserId,
				ShipTo = context.Request.Params["ShippingTo"].Trim(),
				RegionId = System.Convert.ToInt32(context.Request.Params["RegionId"].Trim()),
				Address = context.Request.Params["AddressDetails"].Trim(),
				Zipcode = context.Request.Params["ZipCode"].Trim(),
				CellPhone = context.Request.Params["CellHphone"].Trim(),
				TelPhone = context.Request.Params["TelPhone"].Trim()
			};
		}
	}
}
