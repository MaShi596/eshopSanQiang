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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
namespace Hidistro.UI.Web.Admin
{
	[PrivilegeCheck(Privilege.ManagePurchaseorder)]
	public class PurchaseOrderDetails : AdminPage
	{
		private string purchaseOrderId;
		private PurchaseOrderInfo purchaseOrder;
		protected System.Web.UI.WebControls.Literal litPurchaseOrderId;
		protected PuchaseStatusLabel lblPurchaseOrderStatus;
		protected System.Web.UI.WebControls.Label lbCloseReason;
		protected System.Web.UI.WebControls.Label lbReason;
		protected System.Web.UI.WebControls.Literal litUserName;
		protected System.Web.UI.WebControls.Literal litRealName;
		protected System.Web.UI.WebControls.Literal litUserTel;
		protected System.Web.UI.WebControls.Literal litUserEmail;
		protected System.Web.UI.WebControls.Literal litOrderId;
		protected System.Web.UI.WebControls.Literal litPayTime;
		protected System.Web.UI.WebControls.Literal litSendGoodTime;
		protected System.Web.UI.WebControls.Literal litFinishTime;
		protected System.Web.UI.HtmlControls.HtmlAnchor lkbtnEditPrice;
		protected System.Web.UI.HtmlControls.HtmlAnchor lbtnClocsOrder;
		protected System.Web.UI.WebControls.HyperLink lkbtnSendGoods;
		protected PurchaseOrder_Items itemsList;
		protected PurchaseOrder_Charges chargesList;
		protected PurchaseOrder_ShippingAddress shippingAddress;
		protected System.Web.UI.WebControls.Literal spanOrderId;
		protected System.Web.UI.WebControls.Literal spanpurcharseOrderId;
		protected FormatedTimeLabel lblpurchaseDateForRemark;
		protected FormatedMoneyLabel lblpurchaseTotalForRemark;
		protected OrderRemarkImageRadioButtonList orderRemarkImageForRemark;
		protected System.Web.UI.WebControls.TextBox txtRemark;
		protected ClosePurchaseOrderReasonDropDownList ddlCloseReason;
		protected ShippingModeDropDownList ddlshippingMode;
		protected System.Web.UI.WebControls.Label lblPurchaseOrderAmount;
		protected System.Web.UI.WebControls.TextBox txtPurchaseOrderDiscount;
		protected System.Web.UI.WebControls.Label lblPurchaseOrderAmount1;
		protected System.Web.UI.WebControls.Label lblPurchaseOrderAmount2;
		protected System.Web.UI.WebControls.Label lblPurchaseOrderAmount3;
		protected System.Web.UI.WebControls.Button btnClosePurchaseOrder;
		protected System.Web.UI.WebControls.Button btnEditOrder;
		protected System.Web.UI.WebControls.Button btnRemark;
		protected System.Web.UI.WebControls.Button btnMondifyShip;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hdpurchaseorder;
		protected void Page_Load(object sender, System.EventArgs e)
		{
			if (string.IsNullOrEmpty(this.Page.Request.QueryString["purchaseOrderId"]))
			{
				base.GotoResourceNotFound();
				return;
			}
			this.purchaseOrderId = this.Page.Request.QueryString["purchaseOrderId"];
			this.btnClosePurchaseOrder.Click += new System.EventHandler(this.btnClosePurchaseOrder_Click);
			this.btnMondifyShip.Click += new System.EventHandler(this.btnMondifyShip_Click);
			this.btnEditOrder.Click += new System.EventHandler(this.btnEditOrder_Click);
			this.btnRemark.Click += new System.EventHandler(this.btnRemark_Click);
			this.purchaseOrder = SalesHelper.GetPurchaseOrder(this.purchaseOrderId);
			this.LoadUserControl(this.purchaseOrder);
			if (!base.IsPostBack)
			{
				this.lblPurchaseOrderStatus.PuchaseStatusCode = this.purchaseOrder.PurchaseStatus;
				this.litPurchaseOrderId.Text = this.purchaseOrder.PurchaseOrderId;
				this.litUserName.Text = this.purchaseOrder.Distributorname;
				this.litRealName.Text = this.purchaseOrder.DistributorRealName;
				this.litUserTel.Text = this.purchaseOrder.TelPhone;
				this.litUserEmail.Text = this.purchaseOrder.DistributorEmail;
				if ((int)this.lblPurchaseOrderStatus.PuchaseStatusCode != 4)
				{
					this.lbCloseReason.Visible = false;
				}
				else
				{
					this.lbReason.Text = this.purchaseOrder.CloseReason;
				}
				if (!string.IsNullOrEmpty(this.purchaseOrder.OrderId))
				{
					this.litOrderId.Text = "对应的子站订单编号：" + this.purchaseOrder.OrderId;
				}
				if (this.purchaseOrder.PurchaseStatus != OrderStatus.WaitBuyerPay && this.purchaseOrder.PurchaseStatus != OrderStatus.Closed && this.purchaseOrder.Gateway != "hishop.plugins.payment.podrequest")
				{
					this.litPayTime.Text = "付款时间：" + this.purchaseOrder.PayDate.ToString("yyyy-MM-dd HH:mm:ss");
				}
				if (this.purchaseOrder.PurchaseStatus == OrderStatus.SellerAlreadySent || this.purchaseOrder.PurchaseStatus == OrderStatus.Finished || this.purchaseOrder.PurchaseStatus == OrderStatus.Returned || this.purchaseOrder.PurchaseStatus == OrderStatus.ApplyForReturns || this.purchaseOrder.PurchaseStatus == OrderStatus.ApplyForReplacement)
				{
					this.litSendGoodTime.Text = "发货时间：" + this.purchaseOrder.ShippingDate.ToString("yyyy-MM-dd HH:mm:ss");
				}
				if (this.purchaseOrder.PurchaseStatus == OrderStatus.Finished)
				{
					this.litFinishTime.Text = "完成时间：" + this.purchaseOrder.FinishDate.ToString("yyyy-MM-dd HH:mm:ss");
				}
				if (this.purchaseOrder.PurchaseStatus != OrderStatus.BuyerAlreadyPaid && (this.purchaseOrder.PurchaseStatus != OrderStatus.WaitBuyerPay || !(this.purchaseOrder.Gateway == "hishop.plugins.payment.podrequest")))
				{
					this.lkbtnSendGoods.Visible = false;
				}
				else
				{
					this.lkbtnSendGoods.Visible = true;
				}
				if (this.purchaseOrder.PurchaseStatus == OrderStatus.WaitBuyerPay)
				{
					this.lbtnClocsOrder.Visible = true;
					this.lkbtnEditPrice.Visible = true;
				}
				else
				{
					this.lbtnClocsOrder.Visible = false;
					this.lkbtnEditPrice.Visible = false;
				}
				this.hdpurchaseorder.Value = this.purchaseOrderId;
				this.BindEditOrderPrice(this.purchaseOrder);
				this.BindRemark(this.purchaseOrder);
				this.ddlshippingMode.DataBind();
				this.ddlshippingMode.SelectedValue = new int?(this.purchaseOrder.ShippingModeId);
			}
		}
		private void LoadUserControl(PurchaseOrderInfo purchaseOrder)
		{
			this.itemsList.PurchaseOrder = purchaseOrder;
			this.chargesList.PurchaseOrder = purchaseOrder;
			this.shippingAddress.PurchaseOrder = purchaseOrder;
		}
		private void BindEditOrderPrice(PurchaseOrderInfo purchaseOrder)
		{
			this.lblPurchaseOrderAmount.Text = (purchaseOrder.GetPurchaseTotal() - purchaseOrder.AdjustedDiscount).ToString("F", System.Globalization.CultureInfo.InvariantCulture);
			this.lblPurchaseOrderAmount1.Text = this.lblPurchaseOrderAmount.Text;
			this.lblPurchaseOrderAmount2.Text = purchaseOrder.AdjustedDiscount.ToString("F", System.Globalization.CultureInfo.InvariantCulture);
			this.lblPurchaseOrderAmount3.Text = purchaseOrder.GetPurchaseTotal().ToString("F", System.Globalization.CultureInfo.InvariantCulture);
		}
		private void BindRemark(PurchaseOrderInfo purchaseOrder)
		{
			this.spanOrderId.Text = purchaseOrder.OrderId;
			this.spanpurcharseOrderId.Text = purchaseOrder.PurchaseOrderId;
			this.lblpurchaseDateForRemark.Time = purchaseOrder.PurchaseDate;
			this.lblpurchaseTotalForRemark.Money = purchaseOrder.GetPurchaseTotal();
			this.txtRemark.Text = Globals.HtmlDecode(purchaseOrder.ManagerRemark);
			this.orderRemarkImageForRemark.SelectedValue = purchaseOrder.ManagerMark;
		}
		private void btnRemark_Click(object sender, System.EventArgs e)
		{
			if (this.txtRemark.Text.Length > 300)
			{
				this.ShowMsg("备忘录长度限制在300个字符以内", false);
				return;
			}
			this.purchaseOrder.PurchaseOrderId = this.spanpurcharseOrderId.Text;
			if (this.orderRemarkImageForRemark.SelectedItem != null)
			{
				this.purchaseOrder.ManagerMark = this.orderRemarkImageForRemark.SelectedValue;
			}
			this.purchaseOrder.ManagerRemark = Globals.HtmlEncode(this.txtRemark.Text);
			if (SalesHelper.SavePurchaseOrderRemark(this.purchaseOrder))
			{
				this.BindRemark(this.purchaseOrder);
				this.ShowMsg("保存备忘录成功", true);
				return;
			}
			this.ShowMsg("保存失败", false);
		}
		private void btnClosePurchaseOrder_Click(object sender, System.EventArgs e)
		{
			this.purchaseOrder.CloseReason = this.ddlCloseReason.SelectedValue;
			if (SalesHelper.ClosePurchaseOrder(this.purchaseOrder))
			{
				this.Page.Response.Redirect(Globals.ApplicationPath + string.Format("/Admin/purchaseOrder/ClosedPurchaseOrderDetails.aspx?PurchaseOrderId={0}", this.purchaseOrder.PurchaseOrderId));
				return;
			}
			this.ShowMsg("取消采购失败", false);
		}
		private void btnMondifyShip_Click(object sender, System.EventArgs e)
		{
			ShippingModeInfo shippingModeInfo = new ShippingModeInfo();
			shippingModeInfo = SalesHelper.GetShippingMode(this.ddlshippingMode.SelectedValue.Value, false);
			this.purchaseOrder.ShippingModeId = shippingModeInfo.ModeId;
			this.purchaseOrder.ModeName = shippingModeInfo.Name;
			if (SalesHelper.UpdatePurchaseOrderShippingMode(this.purchaseOrder))
			{
				this.chargesList.LoadControl();
				this.shippingAddress.LoadControl();
				this.ShowMsg("修改配送方式成功", true);
				return;
			}
			this.ShowMsg("修改配送方式失败", false);
		}
		private void btnEditOrder_Click(object sender, System.EventArgs e)
		{
			decimal adjustedDiscount;
			if (!decimal.TryParse(this.txtPurchaseOrderDiscount.Text.Trim(), out adjustedDiscount))
			{
				this.ShowMsg("请正确填写打折或者涨价金额", false);
				return;
			}
			this.purchaseOrder.AdjustedDiscount = adjustedDiscount;
			ValidationResults validationResults = Validation.Validate<PurchaseOrderInfo>(this.purchaseOrder, new string[]
			{
				"ValPurchaseOrder"
			});
			string text = string.Empty;
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
			if (this.purchaseOrder.GetPurchaseTotal() < 0m)
			{
				this.ShowMsg("折扣值不能使得采购单总金额为负", false);
			}
			else
			{
				if (SalesHelper.UpdatePurchaseOrderAmount(this.purchaseOrder))
				{
					this.chargesList.LoadControl();
					this.BindEditOrderPrice(this.purchaseOrder);
					this.ShowMsg("修改成功", true);
					return;
				}
				this.ShowMsg("修改失败", false);
				return;
			}
		}
	}
}
