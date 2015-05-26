using Hidistro.ControlPanel.Sales;
using Hidistro.Entities;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Xml;
namespace Hidistro.UI.Web.Admin.purchaseOrder
{
	public class PrintPurchase : System.Web.UI.Page
	{
		protected string orderIds = "";
		protected string mailNo = "";
		protected string templateName = "";
		protected string width = "";
		protected string height = "";
		protected System.Web.UI.HtmlControls.HtmlHead Head1;
		protected System.Web.UI.HtmlControls.HtmlGenericControl divContent;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (!base.IsPostBack)
			{
				this.mailNo = base.Request["mailNo"];
				int shipperId = int.Parse(base.Request["shipperId"]);
				this.orderIds = base.Request["purchaseOrderIds"].Trim(new char[]
				{
					','
				});
				string text = System.Web.HttpContext.Current.Request.MapPath(string.Format("../../Storage/master/flex/{0}", base.Request["template"]));
				if (System.IO.File.Exists(text))
				{
					System.Xml.XmlDocument xmlDocument = new System.Xml.XmlDocument();
					xmlDocument.Load(text);
					System.Xml.XmlNode xmlNode = xmlDocument.DocumentElement.SelectSingleNode("//printer");
					this.templateName = xmlNode.SelectSingleNode("kind").InnerText;
					string innerText = xmlNode.SelectSingleNode("pic").InnerText;
					string innerText2 = xmlNode.SelectSingleNode("size").InnerText;
					this.width = innerText2.Split(new char[]
					{
						':'
					})[0];
					this.height = innerText2.Split(new char[]
					{
						':'
					})[1];
					System.Data.DataSet printData = this.GetPrintData(this.orderIds);
					int num = 0;
					foreach (System.Data.DataRow dataRow in printData.Tables[0].Rows)
					{
						System.Web.UI.HtmlControls.HtmlGenericControl htmlGenericControl = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
						if (!string.IsNullOrEmpty(innerText) && innerText != "noimage")
						{
							using (Image image = Image.FromFile(System.Web.HttpContext.Current.Request.MapPath(string.Format("../../Storage/master/flex/{0}", innerText))))
							{
								htmlGenericControl.Attributes["style"] = string.Format("background-image: url(../../Storage/master/flex/{0}); width: {1}px; height: {2}px;text-align: center; position: relative;", innerText, image.Width, image.Height);
							}
						}
						System.Data.DataTable dataTable = printData.Tables[1];
						ShippersInfo shipper = SalesHelper.GetShipper(shipperId);
						string[] array = dataRow["shippingRegion"].ToString().Split(new char[]
						{
							'，'
						});
						foreach (System.Xml.XmlNode xmlNode2 in xmlNode.SelectNodes("item"))
						{
							string text2 = xmlNode2.SelectSingleNode("name").InnerText;
							System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder(text2);
							stringBuilder.Replace("收货人-姓名", dataRow["ShipTo"].ToString());
							stringBuilder.Replace("收货人-电话", dataRow["TelPhone"].ToString());
							stringBuilder.Replace("收货人-手机", dataRow["CellPhone"].ToString());
							stringBuilder.Replace("收货人-邮编", dataRow["ZipCode"].ToString());
							stringBuilder.Replace("收货人-地址", dataRow["Address"].ToString());
							string newValue = string.Empty;
							if (array.Length > 0)
							{
								newValue = array[0];
							}
							stringBuilder.Replace("收货人-地区1级", newValue);
							newValue = string.Empty;
							if (array.Length > 1)
							{
								newValue = array[1];
							}
							stringBuilder.Replace("收货人-地区2级", newValue);
							newValue = string.Empty;
							if (array.Length > 2)
							{
								newValue = array[2];
							}
							stringBuilder.Replace("收货人-地区3级", newValue);
							string[] array2 = new string[]
							{
								"",
								"",
								""
							};
							if (shipper != null)
							{
								array2 = RegionHelper.GetFullRegion(shipper.RegionId, "-").Split(new char[]
								{
									'-'
								});
							}
							stringBuilder.Replace("发货人-姓名", (shipper != null) ? shipper.ShipperName : "");
							stringBuilder.Replace("发货人-手机", (shipper != null) ? shipper.CellPhone : "");
							stringBuilder.Replace("发货人-电话", (shipper != null) ? shipper.TelPhone : "");
							stringBuilder.Replace("发货人-地址", (shipper != null) ? shipper.Address : "");
							stringBuilder.Replace("发货人-邮编", (shipper != null) ? shipper.Zipcode : "");
							string newValue2 = string.Empty;
							if (array2.Length > 0)
							{
								newValue2 = array2[0];
							}
							stringBuilder.Replace("发货人-地区1级", newValue2);
							newValue2 = string.Empty;
							if (array2.Length > 1)
							{
								newValue2 = array2[1];
							}
							stringBuilder.Replace("发货人-地区2级", newValue2);
							newValue2 = string.Empty;
							if (array2.Length > 2)
							{
								newValue2 = array2[2];
							}
							stringBuilder.Replace("发货人-地区3级", newValue2);
							stringBuilder.Replace("订单-订单号", "订单号：" + dataRow["PurchaseOrderId"].ToString());
							stringBuilder.Replace("订单-总金额", this.CalculateOrderTotal(dataRow, printData).ToString());
							stringBuilder.Replace("订单-物品总重量", dataRow["Weight"].ToString());
							stringBuilder.Replace("订单-备注", dataRow["Remark"].ToString());
							System.Data.DataRow[] array3 = dataTable.Select(" PurchaseOrderId='" + dataRow["PurchaseOrderId"] + "'");
							string text3 = string.Empty;
							if (array3.Length > 0)
							{
								System.Data.DataRow[] array4 = array3;
								for (int i = 0; i < array4.Length; i++)
								{
									System.Data.DataRow dataRow2 = array4[i];
									text3 = string.Concat(new object[]
									{
										text3,
										"规格 ",
										dataRow2["SKUContent"],
										" ×",
										dataRow2["Quantity"],
										"\n"
									});
								}
								text3 = text3.Replace("；", "");
							}
							stringBuilder.Replace("订单-详情", text3);
							stringBuilder.Replace("订单-送货时间", "");
							stringBuilder.Replace("网店名称", Hidistro.Membership.Context.HiContext.Current.SiteSettings.SiteName);
							stringBuilder.Replace("自定义内容", "");
							text2 = stringBuilder.ToString();
							string innerText3 = xmlNode2.SelectSingleNode("font").InnerText;
							string arg_685_0 = xmlNode2.SelectSingleNode("fontsize").InnerText;
							string innerText4 = xmlNode2.SelectSingleNode("position").InnerText;
							string innerText5 = xmlNode2.SelectSingleNode("align").InnerText;
							string str = innerText4.Split(new char[]
							{
								':'
							})[0];
							string str2 = innerText4.Split(new char[]
							{
								':'
							})[1];
							string str3 = innerText4.Split(new char[]
							{
								':'
							})[2];
							string str4 = innerText4.Split(new char[]
							{
								':'
							})[3];
							System.Web.UI.HtmlControls.HtmlGenericControl htmlGenericControl2 = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
							htmlGenericControl2.Visible = true;
							htmlGenericControl2.InnerText = text2.Split(new char[]
							{
								'_'
							})[0];
							htmlGenericControl2.Style["font-family"] = innerText3;
							htmlGenericControl2.Style["font-size"] = "16px";
							htmlGenericControl2.Style["width"] = str + "px";
							htmlGenericControl2.Style["height"] = str2 + "px";
							htmlGenericControl2.Style["text-align"] = innerText5;
							htmlGenericControl2.Style["position"] = "absolute";
							htmlGenericControl2.Style["left"] = str3 + "px";
							htmlGenericControl2.Style["top"] = str4 + "px";
							htmlGenericControl2.Style["padding"] = "0";
							htmlGenericControl2.Style["margin-left"] = "0px";
							htmlGenericControl2.Style["margin-top"] = "0px";
							htmlGenericControl.Controls.Add(htmlGenericControl2);
						}
						this.divContent.Controls.Add(htmlGenericControl);
						num++;
						if (num < printData.Tables[0].Rows.Count)
						{
							System.Web.UI.HtmlControls.HtmlGenericControl htmlGenericControl3 = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
							htmlGenericControl3.Attributes["class"] = "PageNext";
							this.divContent.Controls.Add(htmlGenericControl3);
						}
					}
				}
			}
		}
		private System.Data.DataSet GetPrintData(string orderIds)
		{
			orderIds = "'" + orderIds.Replace(",", "','") + "'";
			return SalesHelper.GetPurchaseOrdersAndLines(orderIds);
		}
		private decimal CalculateOrderTotal(System.Data.DataRow order, System.Data.DataSet ds)
		{
			decimal d = 0m;
			decimal d2 = 0m;
			decimal d3 = 0m;
			decimal.TryParse(order["AdjustedFreight"].ToString(), out d);
			decimal.TryParse(order["AdjustedDiscount"].ToString(), out d3);
			System.Data.DataRow[] orderGift = ds.Tables[2].Select("PurchaseOrderId='" + order["PurchaseOrderId"] + "'");
			System.Data.DataRow[] orderLine = ds.Tables[1].Select("PurchaseOrderId='" + order["PurchaseOrderId"] + "'");
			decimal d4 = this.GetAmount(orderGift, orderLine, order);
			d4 += d;
			d4 += d2;
			return d4 + d3;
		}
		public decimal GetAmount(System.Data.DataRow[] orderGift, System.Data.DataRow[] orderLine, System.Data.DataRow order)
		{
			return this.GetGoodDiscountAmount(order, orderLine) + this.GetGiftAmount(orderGift);
		}
		public decimal GetGoodDiscountAmount(System.Data.DataRow order, System.Data.DataRow[] orderLine)
		{
			return this.GetGoodsAmount(orderLine);
		}
		public decimal GetGoodsAmount(System.Data.DataRow[] rows)
		{
			decimal num = 0m;
			for (int i = 0; i < rows.Length; i++)
			{
				System.Data.DataRow dataRow = rows[i];
				num += decimal.Parse(dataRow["ItemPurchasePrice"].ToString()) * int.Parse(dataRow["Quantity"].ToString());
			}
			return num;
		}
		public decimal GetGiftAmount(System.Data.DataRow[] rows)
		{
			decimal num = 0m;
			for (int i = 0; i < rows.Length; i++)
			{
				System.Data.DataRow dataRow = rows[i];
				num += decimal.Parse(dataRow["CostPrice"].ToString());
			}
			return num;
		}
	}
}
