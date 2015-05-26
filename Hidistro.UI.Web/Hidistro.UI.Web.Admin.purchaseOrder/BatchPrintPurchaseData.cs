using Hidistro.ControlPanel.Sales;
using Hidistro.Core;
using Hidistro.Entities;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin.purchaseOrder
{
	public class BatchPrintPurchaseData : AdminPage
	{
		private const string OrdersName = "orders.xml";
		private const string ExpressName = "post.xml";
		private static int taskId = 0;
		private string _flag;
		private readonly System.Text.Encoding _encoding = System.Text.Encoding.Unicode;
		private string _zipFilename;
		private System.IO.DirectoryInfo _workDir;
		private System.IO.DirectoryInfo _baseDir;
		private System.IO.DirectoryInfo _flexDir;
		private static string picPath = string.Empty;
		private static bool isPO = true;
		protected static string orderIds = string.Empty;
		protected System.Web.UI.WebControls.Panel pnlTask;
		protected System.Web.UI.WebControls.Literal litNumber;
		protected System.Web.UI.WebControls.Panel pnlTaskEmpty;
		protected System.Web.UI.WebControls.Panel pnlPOEmpty;
		protected System.Web.UI.WebControls.Panel pnlShipper;
		protected ShippersDropDownList ddlShoperTag;
		protected System.Web.UI.WebControls.TextBox txtShipTo;
		protected RegionSelector dropRegions;
		protected System.Web.UI.WebControls.TextBox txtAddress;
		protected System.Web.UI.WebControls.TextBox txtZipcode;
		protected System.Web.UI.WebControls.TextBox txtTelphone;
		protected System.Web.UI.WebControls.TextBox txtCellphone;
		protected System.Web.UI.WebControls.Button btnUpdateAddrdss;
		protected System.Web.UI.WebControls.Panel pnlEmptySender;
		protected System.Web.UI.WebControls.Panel pnlTemplates;
		protected System.Web.UI.WebControls.DropDownList ddlTemplates;
		protected System.Web.UI.WebControls.TextBox txtStartCode;
		protected System.Web.UI.WebControls.Button btnPrint;
		protected System.Web.UI.WebControls.Panel pnlEmptyTemplates;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			int.TryParse(this.Page.Request.QueryString["taskId"], out BatchPrintPurchaseData.taskId);
			if (!string.IsNullOrEmpty(base.Request["PurchaseOrderIds"]))
			{
				BatchPrintPurchaseData.orderIds = base.Request["PurchaseOrderIds"];
				this.litNumber.Text = BatchPrintPurchaseData.orderIds.Trim(new char[]
				{
					','
				}).Split(new char[]
				{
					','
				}).Length.ToString();
			}
			this._flexDir = new System.IO.DirectoryInfo(System.Web.HttpContext.Current.Request.MapPath(Globals.ApplicationPath + "/Storage/master/flex/"));
			this.ddlShoperTag.SelectedIndexChanged += new System.EventHandler(this.ddlShoperTag_SelectedIndexChanged);
			this.btnUpdateAddrdss.Click += new System.EventHandler(this.btnUpdateAddrdss_Click);
			if (!this.Page.IsPostBack)
			{
				this.ddlShoperTag.IncludeDistributor = true;
				this.ddlShoperTag.DataBind();
				System.Collections.Generic.IList<ShippersInfo> shippers = SalesHelper.GetShippers(true);
				foreach (ShippersInfo current in shippers)
				{
					if (current.IsDefault)
					{
						this.ddlShoperTag.SelectedValue = current.ShipperId;
					}
				}
				this.LoadShipper();
				this.LoadTemplates();
			}
		}
		private void btnUpdateAddrdss_Click(object sender, System.EventArgs e)
		{
			if (!this.dropRegions.GetSelectedRegionId().HasValue)
			{
				this.ShowMsg("请选择发货地区！", false);
				return;
			}
			if (this.UpdateAddress())
			{
				this.ShowMsg("修改成功", true);
				return;
			}
			this.ShowMsg("修改失败，请确认信息填写正确或订单还未发货", false);
		}
		private bool UpdateAddress()
		{
			ShippersInfo shipper = SalesHelper.GetShipper(this.ddlShoperTag.SelectedValue);
			if (shipper != null)
			{
				shipper.Address = this.txtAddress.Text;
				shipper.CellPhone = this.txtCellphone.Text;
				shipper.RegionId = this.dropRegions.GetSelectedRegionId().Value;
				shipper.ShipperName = this.txtShipTo.Text;
				shipper.TelPhone = this.txtTelphone.Text;
				shipper.Zipcode = this.txtZipcode.Text;
				return SalesHelper.UpdateShipper(shipper);
			}
			return false;
		}
		private void ddlShoperTag_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.LoadShipper();
		}
		private void LoadShipper()
		{
			ShippersInfo shipper = SalesHelper.GetShipper(this.ddlShoperTag.SelectedValue);
			if (shipper != null)
			{
				this.txtAddress.Text = shipper.Address;
				this.txtCellphone.Text = shipper.CellPhone;
				this.txtShipTo.Text = shipper.ShipperName;
				this.txtTelphone.Text = shipper.TelPhone;
				this.txtZipcode.Text = shipper.Zipcode;
				this.dropRegions.SetSelectedRegionId(new int?(shipper.RegionId));
				this.pnlEmptySender.Visible = false;
				this.pnlShipper.Visible = true;
				return;
			}
			this.pnlShipper.Visible = false;
			this.pnlEmptySender.Visible = true;
		}
		private void LoadTemplates()
		{
			System.Data.DataTable isUserExpressTemplates = SalesHelper.GetIsUserExpressTemplates();
			if (isUserExpressTemplates != null && isUserExpressTemplates.Rows.Count > 0)
			{
				this.ddlTemplates.Items.Add(new System.Web.UI.WebControls.ListItem("-请选择-", ""));
				foreach (System.Data.DataRow dataRow in isUserExpressTemplates.Rows)
				{
					this.ddlTemplates.Items.Add(new System.Web.UI.WebControls.ListItem(dataRow["ExpressName"].ToString(), dataRow["XmlFile"].ToString()));
				}
				this.pnlEmptyTemplates.Visible = false;
				this.pnlTemplates.Visible = true;
				return;
			}
			this.pnlEmptyTemplates.Visible = true;
			this.pnlTemplates.Visible = false;
		}
		private string WritePurchaseOrderInfo(System.Data.DataRow order, ShippersInfo shipper, System.Data.DataTable dtLine, System.Data.DataSet ds)
		{
			string[] array = order["shippingRegion"].ToString().Split(new char[]
			{
				'，'
			});
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.AppendLine("<order>");
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>收货人-姓名</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", order["ShipTo"]);
			stringBuilder.AppendLine("</item>");
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>收货人-电话</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", order["TelPhone"]);
			stringBuilder.AppendLine("</item>");
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>收货人-手机</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", order["CellPhone"]);
			stringBuilder.AppendLine("</item>");
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>收货人-邮编</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", order["ZipCode"]);
			stringBuilder.AppendLine("</item>");
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>收货人-地址</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", order["Address"]);
			stringBuilder.AppendLine("</item>");
			string arg = string.Empty;
			if (array.Length > 0)
			{
				arg = array[0];
			}
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>收货人-地区1级</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", arg);
			stringBuilder.AppendLine("</item>");
			arg = string.Empty;
			if (array.Length > 1)
			{
				arg = array[1];
			}
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>收货人-地区2级</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", arg);
			stringBuilder.AppendLine("</item>");
			arg = string.Empty;
			if (array.Length > 2)
			{
				arg = array[2];
			}
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>收货人-地区3级</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", arg);
			stringBuilder.AppendLine("</item>");
			int num = 0;
			string arg2 = string.Empty;
			string arg3 = string.Empty;
			string arg4 = string.Empty;
			string arg5 = string.Empty;
			string arg6 = string.Empty;
			ShippersInfo shippersInfo = this.ForDistorShipper(ds, order);
			if (shippersInfo != null)
			{
				arg2 = shippersInfo.ShipperName;
				arg3 = shippersInfo.CellPhone;
				arg4 = shippersInfo.TelPhone;
				arg5 = shippersInfo.Address;
				arg6 = shippersInfo.Zipcode;
				num = shippersInfo.RegionId;
			}
			else
			{
				if (shipper != null)
				{
					arg2 = shipper.ShipperName;
					arg3 = shipper.CellPhone;
					arg4 = shipper.TelPhone;
					arg5 = shipper.Address;
					arg6 = shipper.Zipcode;
					num = shipper.RegionId;
				}
			}
			string[] array2 = new string[]
			{
				"",
				"",
				""
			};
			if (num > 0)
			{
				array2 = RegionHelper.GetFullRegion(num, "-").Split(new char[]
				{
					'-'
				});
			}
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>发货人-姓名</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", arg2);
			stringBuilder.AppendLine("</item>");
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>发货人-手机</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", arg3);
			stringBuilder.AppendLine("</item>");
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>发货人-电话</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", arg4);
			stringBuilder.AppendLine("</item>");
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>发货人-地址</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", arg5);
			stringBuilder.AppendLine("</item>");
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>发货人-邮编</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", arg6);
			stringBuilder.AppendLine("</item>");
			string arg7 = string.Empty;
			if (array2.Length > 0)
			{
				arg7 = array2[0];
			}
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>发货人-地区1级</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", arg7);
			stringBuilder.AppendLine("</item>");
			arg7 = string.Empty;
			if (array2.Length > 1)
			{
				arg7 = array2[1];
			}
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>发货人-地区2级</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", arg7);
			stringBuilder.AppendLine("</item>");
			arg7 = string.Empty;
			if (array2.Length > 2)
			{
				arg7 = array2[2];
			}
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>发货人-地区3级</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", arg7);
			stringBuilder.AppendLine("</item>");
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>订单-订单号</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", order["OrderId"]);
			stringBuilder.AppendLine("</item>");
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>订单-总金额</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", order["OrderTotal"]);
			stringBuilder.AppendLine("</item>");
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>订单-物品总重量</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", order["Weight"]);
			stringBuilder.AppendLine("</item>");
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>订单-备注</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", order["Remark"]);
			stringBuilder.AppendLine("</item>");
			System.Data.DataRow[] array3 = dtLine.Select(" PurchaseOrderId='" + order["PurchaseOrderId"] + "'");
			string text = string.Empty;
			if (array3.Length > 0)
			{
				System.Data.DataRow[] array4 = array3;
				for (int i = 0; i < array4.Length; i++)
				{
					System.Data.DataRow dataRow = array4[i];
					text = string.Concat(new object[]
					{
						text,
						"货号 ",
						dataRow["SKU"],
						" ×",
						dataRow["Quantity"],
						"\n"
					});
				}
				text = text.Replace("；", "");
			}
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>订单-详情</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", text);
			stringBuilder.AppendLine("</item>");
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>订单-送货时间</name>");
			stringBuilder.AppendFormat("<rename></rename>", new object[0]);
			stringBuilder.AppendLine("</item>");
			Hidistro.Membership.Context.SiteSettings siteSettings = Hidistro.Membership.Context.SettingsManager.GetSiteSettings((int)order["DistributorId"]);
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>网店名称</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", (siteSettings != null) ? siteSettings.SiteName : Hidistro.Membership.Context.HiContext.Current.SiteSettings.SiteName);
			stringBuilder.AppendLine("</item>");
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>自定义内容</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", "null");
			stringBuilder.AppendLine("</item>");
			stringBuilder.AppendLine("</order>");
			return stringBuilder.ToString();
		}
		private string WriteOrderInfo(System.Data.DataRow order, ShippersInfo shipper, System.Data.DataTable dtLine, System.Data.DataSet ds)
		{
			string[] array = order["shippingRegion"].ToString().Split(new char[]
			{
				'，'
			});
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			stringBuilder.AppendLine("<order>");
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>收货人-姓名</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", order["ShipTo"]);
			stringBuilder.AppendLine("</item>");
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>收货人-电话</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", order["TelPhone"]);
			stringBuilder.AppendLine("</item>");
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>收货人-手机</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", order["CellPhone"]);
			stringBuilder.AppendLine("</item>");
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>收货人-邮编</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", order["ZipCode"]);
			stringBuilder.AppendLine("</item>");
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>收货人-地址</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", order["Address"]);
			stringBuilder.AppendLine("</item>");
			string arg = string.Empty;
			if (array.Length > 0)
			{
				arg = array[0];
			}
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>收货人-地区1级</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", arg);
			stringBuilder.AppendLine("</item>");
			arg = string.Empty;
			if (array.Length > 1)
			{
				arg = array[1];
			}
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>收货人-地区2级</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", arg);
			stringBuilder.AppendLine("</item>");
			arg = string.Empty;
			if (array.Length > 2)
			{
				arg = array[2];
			}
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>收货人-地区3级</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", arg);
			stringBuilder.AppendLine("</item>");
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
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>发货人-姓名</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", (shipper != null) ? shipper.ShipperName : "");
			stringBuilder.AppendLine("</item>");
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>发货人-手机</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", (shipper != null) ? shipper.CellPhone : "");
			stringBuilder.AppendLine("</item>");
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>发货人-电话</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", (shipper != null) ? shipper.TelPhone : "");
			stringBuilder.AppendLine("</item>");
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>发货人-地址</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", (shipper != null) ? shipper.Address : "");
			stringBuilder.AppendLine("</item>");
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>发货人-邮编</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", (shipper != null) ? shipper.Zipcode : "");
			stringBuilder.AppendLine("</item>");
			string arg2 = string.Empty;
			if (array2.Length > 0)
			{
				arg2 = array2[0];
			}
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>发货人-地区1级</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", arg2);
			stringBuilder.AppendLine("</item>");
			arg2 = string.Empty;
			if (array2.Length > 1)
			{
				arg2 = array2[1];
			}
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>发货人-地区2级</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", arg2);
			stringBuilder.AppendLine("</item>");
			arg2 = string.Empty;
			if (array2.Length > 2)
			{
				arg2 = array2[2];
			}
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>发货人-地区3级</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", arg2);
			stringBuilder.AppendLine("</item>");
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>订单-订单号</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", order["OrderId"]);
			stringBuilder.AppendLine("</item>");
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>订单-总金额</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", this.CalculateOrderTotal(order, ds));
			stringBuilder.AppendLine("</item>");
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>订单-物品总重量</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", order["Weight"]);
			stringBuilder.AppendLine("</item>");
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>订单-备注</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", order["Remark"]);
			stringBuilder.AppendLine("</item>");
			System.Data.DataRow[] array3 = dtLine.Select(" OrderId='" + order["OrderId"] + "'");
			string text = string.Empty;
			if (array3.Length > 0)
			{
				System.Data.DataRow[] array4 = array3;
				for (int i = 0; i < array4.Length; i++)
				{
					System.Data.DataRow dataRow = array4[i];
					text = string.Concat(new object[]
					{
						text,
						"货号 ",
						dataRow["SKU"],
						" ×",
						dataRow["ShipmentQuantity"],
						"\n"
					});
				}
				text = text.Replace("；", "");
			}
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>订单-详情</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", text);
			stringBuilder.AppendLine("</item>");
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>订单-送货时间</name>");
			stringBuilder.AppendFormat("<rename></rename>", new object[0]);
			stringBuilder.AppendLine("</item>");
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>网店名称</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", Hidistro.Membership.Context.HiContext.Current.SiteSettings.SiteName);
			stringBuilder.AppendLine("</item>");
			stringBuilder.AppendLine("<item>");
			stringBuilder.AppendLine("<name>自定义内容</name>");
			stringBuilder.AppendFormat("<rename>{0}</rename>", "null");
			stringBuilder.AppendLine("</item>");
			stringBuilder.AppendLine("</order>");
			return stringBuilder.ToString();
		}
		private ShippersInfo ForDistorShipper(System.Data.DataSet ds, System.Data.DataRow order)
		{
			int num = 0;
			int.TryParse(order["DistributorId"].ToString(), out num);
			if (num <= 0 && ds.Tables.Count > 4)
			{
				return null;
			}
			System.Data.DataRow[] array = ds.Tables[4].Select("DistributorUserId=" + num);
			if (array.Length <= 0)
			{
				return null;
			}
			ShippersInfo shippersInfo = new ShippersInfo();
			System.Data.DataRow dataRow = array[0];
			if (dataRow["Address"] != System.DBNull.Value)
			{
				shippersInfo.Address = (string)dataRow["Address"];
			}
			if (dataRow["CellPhone"] != System.DBNull.Value)
			{
				shippersInfo.CellPhone = (string)dataRow["CellPhone"];
			}
			if (dataRow["RegionId"] != System.DBNull.Value)
			{
				shippersInfo.RegionId = (int)dataRow["RegionId"];
			}
			if (dataRow["Remark"] != System.DBNull.Value)
			{
				shippersInfo.Remark = (string)dataRow["Remark"];
			}
			if (dataRow["ShipperName"] != System.DBNull.Value)
			{
				shippersInfo.ShipperName = (string)dataRow["ShipperName"];
			}
			if (dataRow["ShipperTag"] != System.DBNull.Value)
			{
				shippersInfo.ShipperTag = (string)dataRow["ShipperTag"];
			}
			if (dataRow["TelPhone"] != System.DBNull.Value)
			{
				shippersInfo.TelPhone = (string)dataRow["TelPhone"];
			}
			if (dataRow["Zipcode"] != System.DBNull.Value)
			{
				shippersInfo.Zipcode = (string)dataRow["Zipcode"];
			}
			return shippersInfo;
		}
		private decimal CalculateOrderTotal(System.Data.DataRow order, System.Data.DataSet ds)
		{
			decimal d = 0m;
			decimal d2 = 0m;
			decimal d3 = 0m;
			decimal d4 = 0m;
			bool orderOptonFree = false;
			decimal.TryParse(order["AdjustedFreight"].ToString(), out d);
			decimal.TryParse(order["AdjustedPayCharge"].ToString(), out d2);
			string value = order["CouponCode"].ToString();
			decimal.TryParse(order["CouponValue"].ToString(), out d3);
			decimal.TryParse(order["AdjustedDiscount"].ToString(), out d4);
			bool.TryParse(order["OrderOptionFree"].ToString(), out orderOptonFree);
			System.Data.DataRow[] orderGift = ds.Tables[3].Select("OrderId='" + order["orderId"] + "'");
			System.Data.DataRow[] orderLine = ds.Tables[1].Select("OrderId='" + order["orderId"] + "'");
			System.Data.DataRow[] orderOption = ds.Tables[2].Select("OrderId='" + order["orderId"] + "'");
			decimal d5 = this.GetAmount(orderGift, orderLine, order);
			d5 += d;
			d5 += d2;
			d5 += this.GetOptionPrice(orderOption, orderOptonFree);
			if (!string.IsNullOrEmpty(value))
			{
				d5 -= d3;
			}
			return d5 + d4;
		}
		public decimal GetAmount(System.Data.DataRow[] orderGift, System.Data.DataRow[] orderLine, System.Data.DataRow order)
		{
			return this.GetGoodDiscountAmount(order, orderLine) + this.GetGiftAmount(orderGift);
		}
		public decimal GetGoodDiscountAmount(System.Data.DataRow order, System.Data.DataRow[] orderLine)
		{
			decimal num = 0m;
			decimal.TryParse(order["DiscountAmount"].ToString(), out num);
			return this.GetGoodsAmount(orderLine);
		}
		public decimal GetGoodsAmount(System.Data.DataRow[] rows)
		{
			decimal num = 0m;
			for (int i = 0; i < rows.Length; i++)
			{
				System.Data.DataRow dataRow = rows[i];
				num += decimal.Parse(dataRow["ItemAdjustedPrice"].ToString()) * int.Parse(dataRow["Quantity"].ToString());
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
		public decimal GetOptionAmout(System.Data.DataRow[] orderOption)
		{
			decimal num = 0m;
			for (int i = 0; i < orderOption.Length; i++)
			{
				System.Data.DataRow dataRow = orderOption[i];
				num += decimal.Parse(dataRow["AdjustedPrice"].ToString());
			}
			return num;
		}
		public decimal GetOptionPrice(System.Data.DataRow[] orderOption, bool OrderOptonFree)
		{
			if (!OrderOptonFree)
			{
				return this.GetOptionAmout(orderOption);
			}
			return 0m;
		}
	}
}
