using ASPNET.WebControls;
using Hidistro.ControlPanel.Sales;
using Hidistro.ControlPanel.Store;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.Entities.Store;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.ControlPanel.Utility;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.EditOrders)]
	public class EditOrder : AdminPage
	{
		protected Grid grdProducts;
		protected FormatedMoneyLabel lblAllPrice;
		protected System.Web.UI.WebControls.Label lblWeight;
		protected System.Web.UI.WebControls.GridView grdOrderGift;
		protected FormatedMoneyLabel fullDiscountAmount;
		protected System.Web.UI.WebControls.HyperLink lkbtnFullDiscount;
		protected System.Web.UI.WebControls.HyperLink lkbtnFullFree;
		protected System.Web.UI.WebControls.TextBox txtAdjustedFreight;
		protected System.Web.UI.WebControls.Literal litShipModeName;
		protected System.Web.UI.WebControls.TextBox txtAdjustedPayCharge;
		protected System.Web.UI.WebControls.Literal litPayName;
		protected FormatedMoneyLabel couponAmount;
		protected System.Web.UI.WebControls.TextBox txtAdjustedDiscount;
		protected System.Web.UI.WebControls.Literal litTax;
		protected System.Web.UI.WebControls.Literal litInvoiceTitle;
		protected System.Web.UI.WebControls.Literal litIntegral;
		protected System.Web.UI.WebControls.HyperLink hlkSentTimesPointPromotion;
		protected System.Web.UI.WebControls.Literal litTotal;
		protected System.Web.UI.WebControls.Button btnUpdateOrderAmount;
		private string orderId;
		private OrderInfo order;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.Page.Request.QueryString["OrderId"]))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.orderId = this.Page.Request.QueryString["OrderId"];
			this.btnUpdateOrderAmount.Click += new System.EventHandler(this.btnUpdateOrderAmount_Click);
			this.grdOrderGift.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdOrderGift_RowDeleting);
			this.grdProducts.RowDeleting += new System.Web.UI.WebControls.GridViewDeleteEventHandler(this.grdProducts_RowDeleting);
			this.grdProducts.RowCommand += new System.Web.UI.WebControls.GridViewCommandEventHandler(this.grdProducts_RowCommand);
			this.order = OrderHelper.GetOrderInfo(this.orderId);
			if (!this.Page.IsPostBack)
			{
				if (this.order == null)
				{
					base.GotoResourceNotFound();
					return;
				}
				this.BindProductList(this.order);
				this.BindOtherAmount(this.order);
				this.BindTatolAmount(this.order);
			}
		}
		private void btnUpdateOrderAmount_Click(object sender, System.EventArgs e)
		{
			if (!this.order.CheckAction(OrderActions.SELLER_MODIFY_TRADE))
			{
				this.ShowMsg("你当前订单的状态不能进行修改订单费用操作", false);
				return;
			}
			decimal adjustedFreight;
			decimal payCharge;
			decimal adjustedDiscount;
			if (!this.ValidateValues(out adjustedFreight, out payCharge, out adjustedDiscount))
			{
				return;
			}
			string text = string.Empty;
			this.order.AdjustedFreight = adjustedFreight;
			this.order.PayCharge = payCharge;
			this.order.AdjustedDiscount = adjustedDiscount;
			decimal total = this.order.GetTotal();
			ValidationResults validationResults = Validation.Validate<OrderInfo>(this.order, new string[]
			{
				"ValOrder"
			});
			if (!validationResults.IsValid)
			{
				using (System.Collections.Generic.IEnumerator<ValidationResult> enumerator = ((System.Collections.Generic.IEnumerable<ValidationResult>)validationResults).GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						ValidationResult current = enumerator.Current;
						text += Formatter.FormatErrorMessage(current.Message);
						this.ShowMsg(text, false);
						return;
					}
				}
			}
			if (total > 0m)
			{
				if (OrderHelper.UpdateOrderAmount(this.order))
				{
					this.BindTatolAmount(this.order);
					this.ShowMsg("成功的修改了订单金额", true);
					return;
				}
				this.ShowMsg("修改订单金额失败", false);
				return;
			}
			else
			{
				this.ShowMsg("订单的应付总金额不应该是负数,请重新输入订单折扣", false);
			}
		}
		private void BindProductList(OrderInfo order)
		{
			this.grdProducts.DataSource = order.LineItems.Values;
			this.grdProducts.DataBind();
			this.grdOrderGift.DataSource = order.Gifts;
			this.grdOrderGift.DataBind();
		}
		private void grdOrderGift_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			if (!this.order.CheckAction(OrderActions.SELLER_MODIFY_TRADE))
			{
				this.ShowMsg("你当前订单的状态不能进行修改订单费用操作", false);
				return;
			}
			int giftId = (int)this.grdOrderGift.DataKeys[e.RowIndex].Value;
			if (OrderHelper.DeleteOrderGift(this.order, giftId))
			{
				this.order = OrderHelper.GetOrderInfo(this.orderId);
				this.BindProductList(this.order);
				this.BindTatolAmount(this.order);
				this.ShowMsg("成功删除了一件礼品", true);
				return;
			}
			this.ShowMsg("删除礼品失败", false);
		}
		private void grdProducts_RowDeleting(object sender, System.Web.UI.WebControls.GridViewDeleteEventArgs e)
		{
			if (!this.order.CheckAction(OrderActions.SELLER_MODIFY_TRADE))
			{
				this.ShowMsg("你当前订单的状态不能进行修改订单费用操作", false);
				return;
			}
			if (this.order.LineItems.Values.Count <= 1)
			{
				this.ShowMsg("订单的最后一件商品不允许删除", false);
				return;
			}
			string string_ = this.grdProducts.DataKeys[e.RowIndex].Value.ToString();
			if (OrderHelper.DeleteLineItem(string_, this.order))
			{
				this.order = OrderHelper.GetOrderInfo(this.orderId);
				this.BindProductList(this.order);
				this.BindTatolAmount(this.order);
				this.ShowMsg("成功删除了一件商品", true);
				return;
			}
			this.ShowMsg("删除商品失败", false);
		}
		private void grdProducts_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
		{
			OrderInfo orderInfo = this.order;
			if (!this.order.CheckAction(OrderActions.SELLER_MODIFY_TRADE))
			{
				this.ShowMsg("你当前订单的状态不能进行修改订单费用操作", false);
				return;
			}
			if (e.CommandName == "setQuantity")
			{
				int rowIndex = ((System.Web.UI.WebControls.GridViewRow)((System.Web.UI.Control)e.CommandSource).NamingContainer).RowIndex;
				string text = this.grdProducts.DataKeys[rowIndex].Value.ToString();
				System.Web.UI.WebControls.TextBox textBox = this.grdProducts.Rows[rowIndex].FindControl("txtQuantity") as System.Web.UI.WebControls.TextBox;
				int quantity = orderInfo.LineItems[text].Quantity;
				int num;
				if (!int.TryParse(textBox.Text.Trim(), out num))
				{
					this.ShowMsg("商品数量填写错误", false);
					return;
				}
				if (num > OrderHelper.GetSkuStock(text))
				{
					this.ShowMsg("此商品库存不够", false);
					return;
				}
				if (num <= 0)
				{
					this.ShowMsg("商品购买数量不能为0", false);
					return;
				}
				orderInfo.LineItems[text].Quantity = num;
				orderInfo.LineItems[text].ShipmentQuantity = num;
				orderInfo.LineItems[text].ItemAdjustedPrice = orderInfo.LineItems[text].ItemListPrice;
				if (orderInfo.GetTotal() < 0m)
				{
					this.ShowMsg("订单总金额不允许小于0", false);
					return;
				}
				if (quantity != num)
				{
					if (OrderHelper.UpdateLineItem(text, this.order, num))
					{
						this.BindProductList(this.order);
						this.BindTatolAmount(this.order);
						this.ShowMsg("修改商品购买数量成功", true);
						return;
					}
					this.ShowMsg("修改商品购买数量失败", false);
				}
			}
		}
		private void BindTatolAmount(OrderInfo order)
		{
			decimal amount = order.GetAmount();
			this.lblAllPrice.Money = amount;
			this.lblWeight.Text = order.Weight.ToString(System.Globalization.CultureInfo.InvariantCulture);
			this.litIntegral.Text = order.Points.ToString(System.Globalization.CultureInfo.InvariantCulture);
			this.litTotal.Text = Globals.FormatMoney(order.GetTotal());
		}
		private void BindOtherAmount(OrderInfo order)
		{
			if (order.IsReduced)
			{
				this.fullDiscountAmount.Money = order.ReducedPromotionAmount;
				this.lkbtnFullDiscount.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails", new object[]
				{
					order.ReducedPromotionId
				});
				this.lkbtnFullDiscount.Text = order.ReducedPromotionName;
			}
			if (order.IsFreightFree)
			{
				this.lkbtnFullFree.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails", new object[]
				{
					order.FreightFreePromotionId
				});
				this.lkbtnFullFree.Text = order.FreightFreePromotionName;
			}
			if (order.IsSendTimesPoint)
			{
				this.hlkSentTimesPointPromotion.Text = order.SentTimesPointPromotionName;
				this.hlkSentTimesPointPromotion.NavigateUrl = Globals.GetSiteUrls().UrlData.FormatUrl("FavourableDetails", new object[]
				{
					order.SentTimesPointPromotionId
				});
			}
			this.txtAdjustedFreight.Text = order.AdjustedFreight.ToString("F", System.Globalization.CultureInfo.InvariantCulture);
			this.txtAdjustedPayCharge.Text = order.PayCharge.ToString("F", System.Globalization.CultureInfo.InvariantCulture);
			this.txtAdjustedDiscount.Text = order.AdjustedDiscount.ToString("F", System.Globalization.CultureInfo.InvariantCulture);
			if (!string.IsNullOrEmpty(order.PaymentType))
			{
				this.litPayName.Text = "(" + order.PaymentType + ")";
			}
			if (!string.IsNullOrEmpty(order.CouponName))
			{
				this.couponAmount.Text = "[" + order.CouponName + "]-" + Globals.FormatMoney(order.CouponValue);
			}
			else
			{
				this.couponAmount.Text = "-" + Globals.FormatMoney(order.CouponValue);
			}
			if (order.Tax > 0m)
			{
				this.litTax.Text = "<tr class=\"bg\"><td align=\"right\">税金(元)：</td><td colspan=\"2\"><span class='Name'>" + Globals.FormatMoney(order.Tax);
				System.Web.UI.WebControls.Literal expr_22B = this.litTax;
				expr_22B.Text += "</span></td></tr>";
			}
			if (order.InvoiceTitle.Length > 0)
			{
				this.litInvoiceTitle.Text = "<tr class=\"bg\"><td align=\"right\">发票抬头：</td><td colspan=\"2\"><span class='Name'>" + order.InvoiceTitle;
				System.Web.UI.WebControls.Literal expr_26F = this.litInvoiceTitle;
				expr_26F.Text += "</span></td></tr>";
			}
		}
		private bool ValidateValues(out decimal adjustedFreight, out decimal adjustedPayCharge, out decimal discountAmout)
		{
			string text = string.Empty;
			if (!decimal.TryParse(this.txtAdjustedFreight.Text.Trim(), out adjustedFreight))
			{
				text += Formatter.FormatErrorMessage("运费必须在0-1000万之间");
			}
			if (!decimal.TryParse(this.txtAdjustedPayCharge.Text.Trim(), out adjustedPayCharge))
			{
				text += Formatter.FormatErrorMessage("支付费用必须在0-1000万之间");
			}
			int num = 0;
			if (this.txtAdjustedDiscount.Text.Trim().IndexOf(".") > 0)
			{
				num = this.txtAdjustedDiscount.Text.Trim().Substring(this.txtAdjustedDiscount.Text.Trim().IndexOf(".") + 1).Length;
			}
			if (!decimal.TryParse(this.txtAdjustedDiscount.Text.Trim(), out discountAmout) || num > 2)
			{
				text += Formatter.FormatErrorMessage("订单折扣填写错误,订单折扣只能是数值，且不能超过2位小数");
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
