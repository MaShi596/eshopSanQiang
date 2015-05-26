using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Membership.Context;
using Hidistro.Subsites.Sales;
using Hidistro.Subsites.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.Subsites.Utility;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Shopadmin
{
	public class SubmitPurchaseOrder : DistributorPage
	{
		protected System.Web.UI.WebControls.TextBox txtShipTo;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtShipToTip;
		protected System.Web.UI.UserControl Skin1;
		protected RegionSelector rsddlRegion;
		protected System.Web.UI.WebControls.TextBox txtAddress;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtAddressTip;
		protected System.Web.UI.WebControls.TextBox txtZipcode;
		protected System.Web.UI.WebControls.TextBox txtTel;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtTelTip;
		protected System.Web.UI.WebControls.TextBox txtMobile;
		protected System.Web.UI.HtmlControls.HtmlGenericControl txtMobileTip;
		protected ShippingModeRadioButtonList radioShippingMode;
		protected DistributorPaymentRadioButtonList radioPaymentMode;
		protected System.Web.UI.WebControls.TextBox txtRemark;
		protected System.Web.UI.WebControls.Button btnSubmit;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
			if (!base.IsPostBack)
			{
				this.radioPaymentMode.DataBind();
				this.radioShippingMode.DataBind();
			}
		}
		private void btnSubmit_Click(object sender, System.EventArgs e)
		{
			if (!this.ValidateCreateOrder())
			{
				return;
			}
			PurchaseOrderInfo purchaseOrderInfo = this.GetPurchaseOrderInfo();
			if (purchaseOrderInfo.PurchaseOrderItems.Count == 0)
			{
				this.ShowMsg("您暂时未选择您要添加的商品", false);
				return;
			}
			if (!SubsiteSalesHelper.CreatePurchaseOrder(purchaseOrderInfo))
			{
				this.ShowMsg("提交采购单失败", false);
				return;
			}
			SubsiteSalesHelper.ClearPurchaseShoppingCart();
			int.Parse(this.radioPaymentMode.SelectedValue);
			PaymentModeInfo paymentMode = SubsiteStoreHelper.GetPaymentMode(int.Parse(this.radioPaymentMode.SelectedValue));
			if (paymentMode != null && paymentMode.Gateway.ToLower().Equals("hishop.plugins.payment.podrequest"))
			{
				this.ShowMsg("您选择的是货到付款方式，请等待主站发货", true);
				return;
			}
			if (paymentMode != null && paymentMode.Gateway.ToLower().Equals("hishop.plugins.payment.bankrequest"))
			{
				this.ShowMsg("您选择的是线下付款方式，请与主站管理员联系", true);
				return;
			}
			base.Response.Redirect(string.Concat(new string[]
			{
				Globals.ApplicationPath,
				"/Shopadmin/purchaseOrder/Pay.aspx?PurchaseOrderId=",
				purchaseOrderInfo.PurchaseOrderId,
				"&PayMode=",
				this.radioPaymentMode.SelectedValue
			}));
		}
		private PurchaseOrderInfo GetPurchaseOrderInfo()
		{
			PurchaseOrderInfo purchaseOrderInfo = new PurchaseOrderInfo();
			Hidistro.Membership.Context.Distributor distributor = Hidistro.Membership.Context.Users.GetUser(Hidistro.Membership.Context.HiContext.Current.User.UserId) as Hidistro.Membership.Context.Distributor;
			int num = int.Parse(this.radioPaymentMode.SelectedValue);
			PaymentModeInfo paymentMode = SubsiteStoreHelper.GetPaymentMode(num);
			if (paymentMode != null)
			{
				purchaseOrderInfo.PaymentTypeId = num;
				purchaseOrderInfo.PaymentType = paymentMode.Name;
				purchaseOrderInfo.Gateway = paymentMode.Gateway;
			}
			string purchaseOrderId = this.GeneratePurchaseOrderId();
			purchaseOrderInfo.PurchaseOrderId = purchaseOrderId;
			System.Collections.Generic.IList<PurchaseShoppingCartItemInfo> purchaseShoppingCartItemInfos = SubsiteSalesHelper.GetPurchaseShoppingCartItemInfos();
			decimal num2 = 0m;
			if (purchaseShoppingCartItemInfos.Count >= 1)
			{
				foreach (PurchaseShoppingCartItemInfo current in purchaseShoppingCartItemInfos)
				{
					PurchaseOrderItemInfo purchaseOrderItemInfo = new PurchaseOrderItemInfo();
					purchaseOrderItemInfo.PurchaseOrderId = purchaseOrderId;
					purchaseOrderItemInfo.SkuId = current.SkuId;
					purchaseOrderItemInfo.ThumbnailsUrl = current.ThumbnailsUrl;
					purchaseOrderItemInfo.SKUContent = current.SKUContent;
					purchaseOrderItemInfo.SKU = current.SKU;
					purchaseOrderItemInfo.Quantity = current.Quantity;
					purchaseOrderItemInfo.ProductId = current.ProductId;
					purchaseOrderItemInfo.ItemWeight = current.ItemWeight;
					purchaseOrderItemInfo.ItemCostPrice = current.CostPrice;
					purchaseOrderItemInfo.ItemPurchasePrice = current.ItemPurchasePrice;
					purchaseOrderItemInfo.ItemListPrice = current.ItemListPrice;
					purchaseOrderItemInfo.ItemDescription = current.ItemDescription;
					purchaseOrderItemInfo.ItemHomeSiteDescription = current.ItemDescription;
					num2 += current.ItemWeight * current.Quantity;
					purchaseOrderInfo.PurchaseOrderItems.Add(purchaseOrderItemInfo);
				}
				ShippingModeInfo shippingMode = SubsiteSalesHelper.GetShippingMode(this.radioShippingMode.SelectedValue.Value, true);
				purchaseOrderInfo.ShipTo = this.txtShipTo.Text.Trim();
				if (this.rsddlRegion.GetSelectedRegionId().HasValue)
				{
					purchaseOrderInfo.RegionId = this.rsddlRegion.GetSelectedRegionId().Value;
				}
				purchaseOrderInfo.Address = Globals.HtmlEncode(this.txtAddress.Text.Trim());
				purchaseOrderInfo.TelPhone = this.txtTel.Text.Trim();
				purchaseOrderInfo.ZipCode = this.txtZipcode.Text.Trim();
				purchaseOrderInfo.CellPhone = this.txtMobile.Text.Trim();
				purchaseOrderInfo.OrderId = null;
				purchaseOrderInfo.RealShippingModeId = this.radioShippingMode.SelectedValue.Value;
				purchaseOrderInfo.RealModeName = shippingMode.Name;
				purchaseOrderInfo.ShippingModeId = this.radioShippingMode.SelectedValue.Value;
				purchaseOrderInfo.ModeName = shippingMode.Name;
				purchaseOrderInfo.AdjustedFreight = SubsiteSalesHelper.CalcFreight(purchaseOrderInfo.RegionId, num2, shippingMode);
				purchaseOrderInfo.Freight = purchaseOrderInfo.AdjustedFreight;
				purchaseOrderInfo.ShippingRegion = this.rsddlRegion.SelectedRegions;
				purchaseOrderInfo.Remark = Globals.HtmlEncode(this.txtRemark.Text.Trim());
				purchaseOrderInfo.PurchaseStatus = OrderStatus.WaitBuyerPay;
				purchaseOrderInfo.DistributorId = distributor.UserId;
				purchaseOrderInfo.Distributorname = distributor.Username;
				purchaseOrderInfo.DistributorEmail = distributor.Email;
				purchaseOrderInfo.DistributorRealName = distributor.RealName;
				purchaseOrderInfo.DistributorQQ = distributor.QQ;
				purchaseOrderInfo.DistributorWangwang = distributor.Wangwang;
				purchaseOrderInfo.DistributorMSN = distributor.MSN;
				purchaseOrderInfo.RefundStatus = RefundStatus.None;
				purchaseOrderInfo.Weight = num2;
			}
			return purchaseOrderInfo;
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
	}
}
