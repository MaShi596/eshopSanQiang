using Hidistro.Core;
using Hidistro.Entities.Commodities;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Commodities;
using Hidistro.Subsites.Sales;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
namespace Hidistro.UI.Web.Shopadmin.purchaseOrder
{
	public class SubmitTaobaoPurchaseorderCart : DistributorPage
	{
		private class tbOrderItem
		{
			public string id
			{
				get;
				set;
			}
			public string title
			{
				get;
				set;
			}
			public string spec
			{
				get;
				set;
			}
			public string price
			{
				get;
				set;
			}
			public string number
			{
				get;
				set;
			}
			public string bn
			{
				get;
				set;
			}
			public string memo
			{
				get;
				set;
			}
			public string url
			{
				get;
				set;
			}
			public string localSkuId
			{
				get;
				set;
			}
			public string localSKU
			{
				get;
				set;
			}
			public string localProductId
			{
				get;
				set;
			}
			public string localProductName
			{
				get;
				set;
			}
			public string thumbnailUrl100
			{
				get;
				set;
			}
			public string localPrice
			{
				get;
				set;
			}
			public string localStock
			{
				get;
				set;
			}
			public tbOrderItem()
			{
				this.id = "";
				this.title = "";
				this.spec = "";
				this.price = "";
				this.number = "0";
				this.bn = "";
				this.memo = "";
				this.url = "";
				this.localSkuId = "";
				this.localSKU = "";
				this.localStock = "0";
				this.localPrice = "0";
				this.localProductId = "";
				this.localProductName = "";
				this.thumbnailUrl100 = "";
			}
		}
		private class tbOrder
		{
			public System.Collections.Generic.IList<SubmitTaobaoPurchaseorderCart.tbOrderItem> items;
			public string orderId
			{
				get;
				set;
			}
			public string buyer
			{
				get;
				set;
			}
			public string createTime
			{
				get;
				set;
			}
			public string orderMemo
			{
				get;
				set;
			}
			public double orderCost
			{
				get;
				set;
			}
			public tbOrder()
			{
				this.orderId = "";
				this.buyer = "";
				this.createTime = "";
				this.orderMemo = "";
				this.orderCost = 0.0;
				this.items = new System.Collections.Generic.List<SubmitTaobaoPurchaseorderCart.tbOrderItem>();
			}
		}
		private System.Collections.Generic.IList<SubmitTaobaoPurchaseorderCart.tbOrder> tbOrders;
		protected System.Web.UI.HtmlControls.HtmlHead Head1;
		protected HeadContainer HeadContainer1;
		protected Hidistro.UI.Common.Controls.Style Style1;
		protected Hidistro.UI.Common.Controls.Style Style2;
		protected Hidistro.UI.Common.Controls.Style Style3;
		protected Hidistro.UI.Common.Controls.Style Style4;
		protected Script Script2;
		protected Script Script3;
		protected Script Script1;
		protected System.Web.UI.HtmlControls.HtmlForm aspnetForm;
		protected System.Web.UI.WebControls.Repeater rpTaobaoOrder;
		protected System.Web.UI.WebControls.TextBox txtShipTo;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtShipToTip;
		protected RegionSelector rsddlRegion;
		protected System.Web.UI.WebControls.TextBox txtAddress;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtAddressTip;
		protected System.Web.UI.WebControls.TextBox txtZipcode;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtZipcodeTip;
		protected System.Web.UI.WebControls.TextBox txtTel;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtTelTip;
		protected System.Web.UI.WebControls.TextBox txtMobile;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtMobileTip;
		protected ShippingModeRadioButtonList radioShippingMode;
		protected System.Web.UI.WebControls.Button btnSubmit;
		protected System.Web.UI.WebControls.Button btnMatch;
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (string.Compare(base.Request.RequestType, "post", true) != 0)
            {
                this.btnSubmit.Enabled = false;
            }
            else if (base.IsPostBack)
            {
                this.tbOrders = (IList<tbOrder>)this.Session["tbOrders"];
            }
            else
            {
                XmlDocument document = new XmlDocument();
                if (string.IsNullOrEmpty(base.Request.Form["data"]))
                {
                    this.ShowMsg("数据丢失，请关闭此页重新操作", false);
                }
                else
                {
                    document.LoadXml(base.Request.Form["data"]);
                    this.tbOrders = new List<tbOrder>();
                    XmlNodeList list = document.FirstChild.SelectNodes("order");
                    string innerText = null;
                    string str2 = null;
                    for (int i = 0; i < list.Count; i++)
                    {
                        string str4;
                        tbOrder order = new tbOrder();
                        XmlNode node = list.Item(i);
                        if (innerText == null)
                        {
                            innerText = node.SelectSingleNode("ship_addr").InnerText;
                            str2 = node.SelectSingleNode("ship_name").InnerText;
                        }
                        else
                        {
                            str4 = node.SelectSingleNode("ship_addr").InnerText;
                            string str3 = node.SelectSingleNode("ship_name").InnerText;
                            if ((innerText != str4) || (str2 != str3))
                            {
                                this.ShowMsg("收货人地址/姓名不一致不能合并下单！", false);
                                break;
                            }
                            str2 = str3;
                            innerText = str4;
                        }
                        string[] strArray = innerText.Split(new char[] { ' ' });
                        this.txtShipTo.Text = str2;
                        if (strArray.Length >= 4)
                        {
                            str4 = strArray[0] + "," + strArray[1] + "," + strArray[2];
                            this.rsddlRegion.SelectedRegions = str4;
                            this.txtAddress.Text = strArray[3];
                        }
                        this.txtZipcode.Text = node.SelectSingleNode("ship_zipcode").InnerText;
                        this.txtTel.Text = node.SelectSingleNode("ship_tel").InnerText;
                        this.txtMobile.Text = node.SelectSingleNode("ship_mobile").InnerText;
                        this.radioShippingMode.DataBind();
                        if (this.radioShippingMode.Items.Count > 0)
                        {
                            this.radioShippingMode.Items[0].Selected = true;
                        }
                        order.orderId = node.SelectSingleNode("order_id").InnerText;
                        order.buyer = node.SelectSingleNode("buyer").InnerText;
                        order.createTime = node.SelectSingleNode("createtime").InnerText;
                        order.orderMemo = node.SelectSingleNode("order_memo").InnerText;
                        XmlNode node2 = node.SelectSingleNode("items");
                        double num = 0.0;
                        for (int j = 0; j < node2.ChildNodes.Count; j++)
                        {
                            tbOrderItem item = new tbOrderItem
                            {
                                id = string.Format("{0}_{1}", order.orderId, j),
                                title = node2.ChildNodes[j].SelectSingleNode("title").InnerText,
                                spec = node2.ChildNodes[j].SelectSingleNode("spec").InnerText,
                                price = node2.ChildNodes[j].SelectSingleNode("price").InnerText,
                                number = node2.ChildNodes[j].SelectSingleNode("number").InnerText
                            };
                            if (string.IsNullOrEmpty(item.number))
                            {
                                item.number = "1";
                            }
                            item.url = node2.ChildNodes[j].SelectSingleNode("url").InnerText;
                            HttpRequest request = HttpContext.Current.Request;
                            if (request.Cookies[Globals.UrlEncode(item.title + item.spec)] != null)
                            {
                                ProductQuery query = new ProductQuery
                                {
                                    PageSize = 1,
                                    PageIndex = 1,
                                    ProductCode = Globals.UrlDecode(request.Cookies[Globals.UrlEncode(item.title + item.spec)].Value)
                                };
                                int count = 0;
                                DataTable puchaseProducts = SubSiteProducthelper.GetPuchaseProducts(query, out count);
                                if (puchaseProducts.Rows.Count > 0)
                                {
                                    item.localSkuId = puchaseProducts.Rows[0]["SkuId"].ToString();
                                    item.localSKU = puchaseProducts.Rows[0]["SKU"].ToString();
                                    item.localProductId = puchaseProducts.Rows[0]["ProductId"].ToString().Trim();
                                    item.localProductName = puchaseProducts.Rows[0]["ProductName"].ToString();
                                    item.thumbnailUrl100 = puchaseProducts.Rows[0]["ThumbnailUrl100"].ToString();
                                    item.localPrice = puchaseProducts.Rows[0]["PurchasePrice"].ToString();
                                    item.localStock = puchaseProducts.Rows[0]["Stock"].ToString();
                                    num += Convert.ToDouble(puchaseProducts.Rows[0]["PurchasePrice"]) * Convert.ToInt32(item.number);
                                }
                            }
                            else
                            {
                                ProductQuery query2 = new ProductQuery
                                {
                                    PageSize = 1,
                                    PageIndex = 1,
                                    Keywords = item.title
                                };
                                int num5 = 0;
                                DataTable table2 = SubSiteProducthelper.GetPuchaseProducts(query2, out num5);
                                if (num5 == 1)
                                {
                                    item.localSkuId = table2.Rows[0]["SkuId"].ToString();
                                    item.localSKU = table2.Rows[0]["SKU"].ToString();
                                    item.localProductId = table2.Rows[0]["ProductId"].ToString().Trim();
                                    item.localProductName = table2.Rows[0]["ProductName"].ToString();
                                    item.thumbnailUrl100 = table2.Rows[0]["ThumbnailUrl100"].ToString();
                                    item.localPrice = table2.Rows[0]["PurchasePrice"].ToString();
                                    item.localStock = table2.Rows[0]["Stock"].ToString();
                                    num += Convert.ToDouble(table2.Rows[0]["PurchasePrice"]) * Convert.ToInt32(item.number);
                                }
                            }
                            order.items.Add(item);
                            order.orderCost = num;
                        }
                        this.tbOrders.Add(order);
                    }
                    this.Session["tbOrders"] = this.tbOrders;
                    this.pageDataBind();
                }
            }
        }

		private void pageDataBind()
		{
			this.rpTaobaoOrder.DataSource = this.tbOrders;
			this.rpTaobaoOrder.DataBind();
		}
		protected string isFindProduct(string productId, int type)
		{
			if (productId.Trim() == "")
			{
				if (type != 0)
				{
					return "block";
				}
				return "none";
			}
			else
			{
				if (type != 1)
				{
					return "block";
				}
				return "none";
			}
		}
		protected void rpTaobaoOrder_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == System.Web.UI.WebControls.ListItemType.Item || e.Item.ItemType == System.Web.UI.WebControls.ListItemType.AlternatingItem)
			{
				System.Collections.Generic.IList<SubmitTaobaoPurchaseorderCart.tbOrder> list = (System.Collections.Generic.IList<SubmitTaobaoPurchaseorderCart.tbOrder>)this.rpTaobaoOrder.DataSource;
				System.Web.UI.WebControls.Repeater repeater = (System.Web.UI.WebControls.Repeater)e.Item.FindControl("reOrderItems");
				repeater.DataSource = list[e.Item.ItemIndex].items;
				repeater.DataBind();
			}
		}
		protected void ButtonDelete_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
		{
			string text = e.CommandArgument.ToString();
			string[] array = text.Split(new char[]
			{
				'_'
			});
			foreach (SubmitTaobaoPurchaseorderCart.tbOrder current in this.tbOrders)
			{
				if (current.orderId == array[0])
				{
					if (current.items.Count <= 1)
					{
						this.ShowMsg("每个淘宝订单至少保留一件商品！", false);
						return;
					}
					foreach (SubmitTaobaoPurchaseorderCart.tbOrderItem current2 in current.items)
					{
						if (current2.id == text)
						{
							current.items.Remove(current2);
							if (current2.localSkuId != "" && current2.localPrice != "")
							{
								current.orderCost -= System.Convert.ToDouble(current2.localPrice);
							}
							break;
						}
					}
					break;
				}
			}
			this.pageDataBind();
		}
		protected void ButtonAdd_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
		{
			string text = e.CommandArgument.ToString();
			string[] array = text.Split(new char[]
			{
				'_'
			});
			foreach (SubmitTaobaoPurchaseorderCart.tbOrder current in this.tbOrders)
			{
				if (current.orderId == array[0])
				{
					foreach (SubmitTaobaoPurchaseorderCart.tbOrderItem current2 in current.items)
					{
						if (current2.id == text)
						{
							if (current2.number != "")
							{
								if (System.Convert.ToInt32(current2.number) >= System.Convert.ToInt32(current2.localStock))
								{
									this.ShowMsg("库存不足，请检查后再下单", false);
								}
								else
								{
									current2.number = (System.Convert.ToInt32(current2.number) + 1).ToString();
								}
							}
							else
							{
								current2.number = "1";
							}
							if (current2.localSkuId != "" && current2.localPrice != "")
							{
								current.orderCost += System.Convert.ToDouble(current2.localPrice);
							}
							break;
						}
					}
					break;
				}
			}
			this.pageDataBind();
		}
		protected void ButtonDec_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
		{
			string text = e.CommandArgument.ToString();
			string[] array = text.Split(new char[]
			{
				'_'
			});
			foreach (SubmitTaobaoPurchaseorderCart.tbOrder current in this.tbOrders)
			{
				if (current.orderId == array[0])
				{
					foreach (SubmitTaobaoPurchaseorderCart.tbOrderItem current2 in current.items)
					{
						if (current2.id == text)
						{
							if (current2.number != "" && System.Convert.ToInt32(current2.number) > 1)
							{
								current2.number = (System.Convert.ToInt32(current2.number) - 1).ToString();
								if (current2.localSkuId != "" && current2.localPrice != "")
								{
									current.orderCost -= System.Convert.ToDouble(current2.localPrice);
								}
							}
							break;
						}
					}
					break;
				}
			}
			this.pageDataBind();
		}
		protected void btnMatch_Click(object sender, System.EventArgs e)
		{
			if (base.Request.Form["radioSerachResult"] == null)
			{
				return;
			}
			string skuId = base.Request.Form["radioSerachResult"].Trim();
			System.Data.DataTable puchaseProduct = SubSiteProducthelper.GetPuchaseProduct(skuId);
			if (puchaseProduct != null && puchaseProduct.Rows.Count > 0)
			{
				string text = base.Request.Form["serachProductId"].Trim();
				string b = text.Substring(0, text.IndexOf('_'));
				foreach (SubmitTaobaoPurchaseorderCart.tbOrder current in this.tbOrders)
				{
					if (current.orderId == b)
					{
						foreach (SubmitTaobaoPurchaseorderCart.tbOrderItem current2 in current.items)
						{
							if (current2.id == text)
							{
								double num;
								double.TryParse(current2.localPrice, out num);
								int num2;
								int.TryParse(current2.number, out num2);
								current.orderCost -= num * (double)num2;
								current2.localSkuId = puchaseProduct.Rows[0]["SkuId"].ToString();
								current2.localSKU = puchaseProduct.Rows[0]["SKU"].ToString();
								current2.localProductId = puchaseProduct.Rows[0]["ProductId"].ToString().Trim();
								current2.localProductName = puchaseProduct.Rows[0]["ProductName"].ToString();
								current2.thumbnailUrl100 = puchaseProduct.Rows[0]["ThumbnailUrl100"].ToString();
								current2.localPrice = puchaseProduct.Rows[0]["PurchasePrice"].ToString();
								current2.localStock = puchaseProduct.Rows[0]["Stock"].ToString();
								double.TryParse(current2.localPrice, out num);
								current.orderCost += num * (double)num2;
								break;
							}
						}
						break;
					}
				}
			}
			this.pageDataBind();
		}
		private bool ValidateCreateOrder()
		{
			string text = string.Empty;
			if (!this.rsddlRegion.GetSelectedRegionId().HasValue || this.rsddlRegion.GetSelectedRegionId().Value == 0)
			{
				text += Formatter.FormatErrorMessage("请选择收货地址");
			}
			string pattern = "[\\u4e00-\\u9fa5a-zA-Z]+[\\u4e00-\\u9fa5_a-zA-Z0-9]*";
			System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern);
			if (string.IsNullOrEmpty(this.txtShipTo.Text) || !regex.IsMatch(this.txtShipTo.Text.Trim()))
			{
				text += Formatter.FormatErrorMessage("请输入正确的收货人姓名");
			}
			if (!string.IsNullOrEmpty(this.txtShipTo.Text) && (this.txtShipTo.Text.Trim().Length < 2 || this.txtShipTo.Text.Trim().Length > 20))
			{
				text += Formatter.FormatErrorMessage("收货人姓名的长度限制在2-20个字符");
			}
			if (string.IsNullOrEmpty(this.txtAddress.Text.Trim()) || this.txtAddress.Text.Trim().Length > 100)
			{
				text += Formatter.FormatErrorMessage("请输入收货人详细地址,在100个字符以内");
			}
			regex = new System.Text.RegularExpressions.Regex("^[0-9]*$");
			if (string.IsNullOrEmpty(this.txtZipcode.Text.Trim()) || this.txtZipcode.Text.Trim().Length > 10 || this.txtZipcode.Text.Trim().Length < 6 || !regex.IsMatch(this.txtZipcode.Text.Trim()))
			{
				text += Formatter.FormatErrorMessage("请输入收货人邮政编码,在6-10个数字之间");
			}
			regex = new System.Text.RegularExpressions.Regex("^[0-9]*$");
			if (!string.IsNullOrEmpty(this.txtMobile.Text.Trim()) && (!regex.IsMatch(this.txtMobile.Text.Trim()) || this.txtMobile.Text.Trim().Length > 20 || this.txtMobile.Text.Trim().Length < 3))
			{
				text += Formatter.FormatErrorMessage("手机号码长度限制在3-20个字符之间,只能输入数字");
			}
			regex = new System.Text.RegularExpressions.Regex("^[0-9-]*$");
			if (!string.IsNullOrEmpty(this.txtTel.Text.Trim()) && (!regex.IsMatch(this.txtTel.Text.Trim()) || this.txtTel.Text.Trim().Length > 20 || this.txtTel.Text.Trim().Length < 3))
			{
				text += Formatter.FormatErrorMessage("电话号码长度限制在3-20个字符之间,只能输入数字和字符“-”");
			}
			if (!this.radioShippingMode.SelectedValue.HasValue)
			{
				text += Formatter.FormatErrorMessage("请选择配送方式");
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.ShowMsg(text, false);
				return false;
			}
			return true;
		}
		private string GeneratePurchaseOrderId()
		{
			string text = string.Empty;
			System.Random random = new System.Random();
			for (int i = 0; i < 7; i++)
			{
				int num = random.Next();
				text += ((char)(48 + (ushort)(num % 10))).ToString();
			}
			return "MPO" + System.DateTime.Now.ToString("yyyyMMdd") + text;
		}
		public static void RemoveCookie(string cookieName, string key)
		{
			System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
			if (response != null)
			{
				System.Web.HttpCookie httpCookie = response.Cookies[cookieName];
				if (httpCookie != null)
				{
					if (!string.IsNullOrEmpty(key) && httpCookie.HasKeys)
					{
						httpCookie.Values.Remove(key);
						return;
					}
					response.Cookies.Remove(cookieName);
				}
			}
		}
		private void ResponseCookies()
		{
			System.Web.HttpRequest request = System.Web.HttpContext.Current.Request;
			System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
			for (int i = 0; i < this.rpTaobaoOrder.Items.Count; i++)
			{
				System.Web.UI.WebControls.Repeater repeater = (System.Web.UI.WebControls.Repeater)this.rpTaobaoOrder.Items[i].FindControl("reOrderItems");
				for (int j = 0; j < repeater.Items.Count; j++)
				{
					System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)repeater.Items[j].FindControl("chkSave");
					if (checkBox.Checked)
					{
						System.Web.UI.WebControls.Label label = (System.Web.UI.WebControls.Label)repeater.Items[j].FindControl("lblSKU");
						System.Web.UI.WebControls.Label label2 = (System.Web.UI.WebControls.Label)repeater.Items[j].FindControl("lblTitle");
						System.Web.UI.WebControls.Label label3 = (System.Web.UI.WebControls.Label)repeater.Items[j].FindControl("lblSpec");
						if (request.Cookies[Globals.UrlEncode(label2.Text + label3.Text)] != null)
						{
							response.Cookies.Remove(Globals.UrlEncode(label2.Text + label3.Text));
						}
						System.Web.HttpCookie httpCookie = new System.Web.HttpCookie(Globals.UrlEncode(label2.Text + label3.Text));
						httpCookie.Value = Globals.UrlEncode(label.Text);
						httpCookie.Path = "/";
						httpCookie.Expires = System.DateTime.Now.AddDays(3650.0);
						response.Cookies.Add(httpCookie);
					}
				}
			}
		}
		protected void btnSubmit_Click(object sender, System.EventArgs e)
		{
			if (!this.ValidateCreateOrder())
			{
				return;
			}
			string text = "";
			PurchaseOrderInfo purchaseOrderInfo = new PurchaseOrderInfo();
			Hidistro.Membership.Context.Distributor distributor = Hidistro.Membership.Context.Users.GetUser(Hidistro.Membership.Context.HiContext.Current.User.UserId) as Hidistro.Membership.Context.Distributor;
			string purchaseOrderId = this.GeneratePurchaseOrderId();
			purchaseOrderInfo.PurchaseOrderId = purchaseOrderId;
			decimal num = 0m;
			for (int i = 0; i < this.rpTaobaoOrder.Items.Count; i++)
			{
				System.Web.UI.WebControls.CheckBox checkBox = (System.Web.UI.WebControls.CheckBox)this.rpTaobaoOrder.Items[i].FindControl("chkTbOrder");
				if (checkBox.Checked)
				{
					text += this.tbOrders[i].orderId;
					text += ",";
					System.Web.UI.WebControls.Repeater repeater = (System.Web.UI.WebControls.Repeater)this.rpTaobaoOrder.Items[i].FindControl("reOrderItems");
					System.Collections.Generic.IList<SubmitTaobaoPurchaseorderCart.tbOrderItem> items = this.tbOrders[i].items;
					for (int j = 0; j < repeater.Items.Count; j++)
					{
						if (items[j].localSkuId.Trim() == "")
						{
							string string_ = string.Format("在授权给分销商的商品中没有找到淘宝商品：{0}！请重新查找", items[j].title);
							this.ShowMsg(string_, false);
							return;
						}
						string localSkuId = items[j].localSkuId;
						System.Web.UI.WebControls.TextBox textBox = (System.Web.UI.WebControls.TextBox)repeater.Items[j].FindControl("productNumber");
						int num2 = System.Convert.ToInt32(textBox.Text);
						bool flag = false;
						foreach (PurchaseOrderItemInfo current in purchaseOrderInfo.PurchaseOrderItems)
						{
							if (current.SKU == localSkuId)
							{
								flag = true;
								current.Quantity += num2;
								num += current.ItemWeight * num2;
							}
						}
						if (!flag)
						{
							System.Data.DataTable skuContentBySku = SubSiteProducthelper.GetSkuContentBySku(localSkuId);
							PurchaseOrderItemInfo purchaseOrderItemInfo = new PurchaseOrderItemInfo();
							if (num2 > (int)skuContentBySku.Rows[0]["Stock"])
							{
								this.ShowMsg("商品库存不够", false);
								return;
							}
							foreach (System.Data.DataRow dataRow in skuContentBySku.Rows)
							{
								if (!string.IsNullOrEmpty(dataRow["AttributeName"].ToString()) && !string.IsNullOrEmpty(dataRow["ValueStr"].ToString()))
								{
									PurchaseOrderItemInfo expr_23B = purchaseOrderItemInfo;
									object sKUContent = expr_23B.SKUContent;
									expr_23B.SKUContent = string.Concat(new object[]
									{
										sKUContent,
										dataRow["AttributeName"],
										":",
										dataRow["ValueStr"],
										"; "
									});
								}
							}
							purchaseOrderItemInfo.PurchaseOrderId = purchaseOrderId;
							purchaseOrderItemInfo.SkuId = localSkuId;
							purchaseOrderItemInfo.ProductId = (int)skuContentBySku.Rows[0]["ProductId"];
							if (skuContentBySku.Rows[0]["SKU"] != System.DBNull.Value)
							{
								purchaseOrderItemInfo.SKU = (string)skuContentBySku.Rows[0]["SKU"];
							}
							if (skuContentBySku.Rows[0]["Weight"] != System.DBNull.Value)
							{
								purchaseOrderItemInfo.ItemWeight = (decimal)skuContentBySku.Rows[0]["Weight"];
							}
							purchaseOrderItemInfo.ItemPurchasePrice = (decimal)skuContentBySku.Rows[0]["PurchasePrice"];
							purchaseOrderItemInfo.Quantity = num2;
							purchaseOrderItemInfo.ItemListPrice = (decimal)skuContentBySku.Rows[0]["SalePrice"];
							if (skuContentBySku.Rows[0]["CostPrice"] != System.DBNull.Value)
							{
								purchaseOrderItemInfo.ItemCostPrice = (decimal)skuContentBySku.Rows[0]["CostPrice"];
							}
							purchaseOrderItemInfo.ItemDescription = (string)skuContentBySku.Rows[0]["ProductName"];
							purchaseOrderItemInfo.ItemHomeSiteDescription = (string)skuContentBySku.Rows[0]["ProductName"];
							if (skuContentBySku.Rows[0]["ThumbnailUrl40"] != System.DBNull.Value)
							{
								purchaseOrderItemInfo.ThumbnailsUrl = (string)skuContentBySku.Rows[0]["ThumbnailUrl40"];
							}
							num += purchaseOrderItemInfo.ItemWeight * num2;
							purchaseOrderInfo.PurchaseOrderItems.Add(purchaseOrderItemInfo);
						}
					}
				}
			}
			if (text == "")
			{
				this.ShowMsg("至少选择一个淘宝订单！！", false);
				return;
			}
			ShippingModeInfo shippingMode = SubsiteSalesHelper.GetShippingMode(this.radioShippingMode.SelectedValue.Value, true);
			purchaseOrderInfo.ShipTo = this.txtShipTo.Text.Trim();
			if (this.rsddlRegion.GetSelectedRegionId().HasValue)
			{
				purchaseOrderInfo.RegionId = this.rsddlRegion.GetSelectedRegionId().Value;
			}
			purchaseOrderInfo.Address = this.txtAddress.Text.Trim();
			purchaseOrderInfo.TelPhone = this.txtTel.Text.Trim();
			purchaseOrderInfo.ZipCode = this.txtZipcode.Text.Trim();
			purchaseOrderInfo.CellPhone = this.txtMobile.Text.Trim();
			purchaseOrderInfo.OrderId = null;
			purchaseOrderInfo.RealShippingModeId = this.radioShippingMode.SelectedValue.Value;
			purchaseOrderInfo.RealModeName = shippingMode.Name;
			purchaseOrderInfo.ShippingModeId = this.radioShippingMode.SelectedValue.Value;
			purchaseOrderInfo.ModeName = shippingMode.Name;
			purchaseOrderInfo.AdjustedFreight = SubsiteSalesHelper.CalcFreight(purchaseOrderInfo.RegionId, num, shippingMode);
			purchaseOrderInfo.Freight = purchaseOrderInfo.AdjustedFreight;
			purchaseOrderInfo.ShippingRegion = this.rsddlRegion.SelectedRegions;
			purchaseOrderInfo.PurchaseStatus = OrderStatus.WaitBuyerPay;
			purchaseOrderInfo.DistributorId = distributor.UserId;
			purchaseOrderInfo.Distributorname = distributor.Username;
			purchaseOrderInfo.DistributorEmail = distributor.Email;
			purchaseOrderInfo.DistributorRealName = distributor.RealName;
			purchaseOrderInfo.DistributorQQ = distributor.QQ;
			purchaseOrderInfo.DistributorWangwang = distributor.Wangwang;
			purchaseOrderInfo.DistributorMSN = distributor.MSN;
			purchaseOrderInfo.RefundStatus = RefundStatus.None;
			purchaseOrderInfo.Weight = num;
			purchaseOrderInfo.Remark = null;
			purchaseOrderInfo.TaobaoOrderId = text;
			if (purchaseOrderInfo.PurchaseOrderItems.Count == 0)
			{
				this.ShowMsg("您暂时未选择您要添加的商品", false);
				return;
			}
			if (SubsiteSalesHelper.CreatePurchaseOrder(purchaseOrderInfo))
			{
				SubsiteSalesHelper.ClearPurchaseShoppingCart();
				this.ResponseCookies();
				base.Response.Redirect(Globals.ApplicationPath + "/Shopadmin/purchaseOrder/ChoosePayment.aspx?PurchaseOrderId=" + purchaseOrderInfo.PurchaseOrderId);
				return;
			}
			this.ShowMsg("提交采购单失败", false);
		}
	}
}
